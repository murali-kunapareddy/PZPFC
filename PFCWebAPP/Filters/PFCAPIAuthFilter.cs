using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SE.CA.PingComponent.Entities;
using System.Security.Principal;
using PFCWebAPP.DatabaseContext.Models.CustomTables;
using NLog;
using PFCWebAPP.Repositories.BackOps;
using System.Text;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Text.Json;
using PFCWebAPP.Repositories.Common.Interfaces;
using System.Globalization;
using System;
using NuGet.Protocol;
using NPOI.XWPF.UserModel;
using PFCWebAPP.Repositories.Common;
using PFCWebAPP.Repositories.PriceList;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using PFCWebAPP.Repositories.Configure;
using PFCWebAPP.Utilities;

namespace PFCWebAPP.Filters
{
    public class PFCAPIAuthFilter : Attribute, IAuthorizationFilter
    {
        private static IBackOpsRepository objBackOpsRepository;
        private static IPriceListRepository objPriceListRepository;
        private static IConfigureRepository objConfigureRepository;
        private static INotificationProvider objNotificationProvider;
        private static NLog.ILogger _objLoggingProvider = LogManager.GetCurrentClassLogger();
        //public IEnumerable<string> AllowedHosts { get; }

        //public PFCAPIAuthFilter(params string[] allowedHosts)
        //{
        //    AllowedHosts = allowedHosts;
        //}
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Get host from the request and check if it's in the enumeration of allowed hosts
            //string host = context.HttpContext.Request.Host.Host;
            //if (!AllowedHosts.Contains(host, StringComparer.OrdinalIgnoreCase))
            //{
            //    // Request came from an unauthorized host, return bad request
            //    context.Result = new BadRequestObjectResult("Host is not allowed");
            //}
            _objLoggingProvider.Info("***************API*************");
            _objLoggingProvider.Info("API Request Log: Started");
            _objLoggingProvider.Info("API Request Log - OnAuthorization: START");
            context.HttpContext.Session.SetString("isFromApi", "true");
            string ApiSession = "API"
                + DateTime.UtcNow.ToString("yy")
                + DateTime.UtcNow.ToString("MM")
                + DateTime.UtcNow.ToString("dd")
                + DateTime.UtcNow.ToString("HH")
                + DateTime.UtcNow.ToString("mm")
                + DateTime.UtcNow.ToString("ss")
                + DateTime.UtcNow.ToString("fff");
            context.HttpContext.Session.SetString("ApiSession", ApiSession);

            objBackOpsRepository = (IBackOpsRepository)context.HttpContext.RequestServices.GetService(typeof(IBackOpsRepository));
            objPriceListRepository = (IPriceListRepository)context.HttpContext.RequestServices.GetService(typeof(IPriceListRepository));
            objConfigureRepository = (IConfigureRepository)context.HttpContext.RequestServices.GetService(typeof(IConfigureRepository));
            objNotificationProvider = (INotificationProvider)context.HttpContext.RequestServices.GetService(typeof(INotificationProvider));
            string UserSESA = "";
            string Jsonbody = "";
            var request = context.HttpContext.Request;

            request.EnableBuffering();
            request.Body.Position = 0;
            using (var reader = new StreamReader(request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true))
            {
                
                var body = reader.ReadToEnd();
                
                request.Body.Position = 0;

                if (body.StartsWith("{"))
                {
                    // JSON request body
                    var jsonDoc = JsonDocument.Parse(body);
                    Jsonbody = body;
                    if (jsonDoc.RootElement.TryGetProperty("CustomerId", out JsonElement customerIdElement))
                    {
                        UserSESA = customerIdElement.GetString();
                        _objLoggingProvider.Info($"OnAuthorization JSON - CustomerId: {UserSESA}");
                    }
                    else
                    {
                        UserSESA = "PAYLOAD_INVALID";                        
                        _objLoggingProvider.Error("OnAuthorization JSON - INVALID PAYLOAD");
                    }
                    
                }
                else if (body.StartsWith("<"))
                {
                    // XML request body
                    XDocument doc = XDocument.Parse(body);
                    XNamespace ns = doc.Root.GetDefaultNamespace();
                    XElement customerIDElement = doc.Root.Element(ns + "CustomerId");
                    Jsonbody = JsonConvert.SerializeXNode(doc);
                    if (customerIDElement != null)
                    {
                        UserSESA = customerIDElement.Value;
                        _objLoggingProvider.Info($"OnAuthorization xml - CustomerId: {customerIDElement.Value}");
                    }
                    else
                    {
                        UserSESA = "PAYLOAD_INVALID";                        
                        _objLoggingProvider.Error("OnAuthorization xml - INVALID PAYLOAD");                        
                    }
                }
                else
                {
                    UserSESA = "PAYLOAD_INVALID";
                    Jsonbody = body;
                    _objLoggingProvider.Error("OnAuthorization - INVALID PAYLOAD");
                }
            }

            UserLog userLog = SaveUserLog(UserSESA, context);
            if (userLog != null && AppConfig.WhitelistingEnvironment == "PROD") {
                var AuthorizedConfigResult = objConfigureRepository.ConfigOptionsRepository.GetManyQueryable().FirstOrDefault(o => o.ConfigValue == userLog.UserHostAddress && o.ConfigType == AppConfig.WhitelistIPAddresses);
                if (AuthorizedConfigResult == null)
                {                  
                    UserSESA = "UNAUTHORIZED_USER";
                    _objLoggingProvider.Error("AuthorizedConfigResult - UNAUTHORIZED_USER");
                }                
            }

            if (UserSESA == "PAYLOAD_INVALID" || UserSESA == "UNAUTHORIZED_USER")
            {
                var createdBy = $"UserLogID-{userLog?.UserLogID.ToString() ?? "0"}";
                SaveQueueHistoryInvalidPayload(UserSESA, Jsonbody, createdBy);                
                int statusCode = UserSESA == "PAYLOAD_INVALID" ? StatusCodes.Status400BadRequest : StatusCodes.Status401Unauthorized;
                context.Result = new ObjectResult($"{UserSESA} : Unsuccessful Request")
                {
                    StatusCode = statusCode
                };
                objNotificationProvider.SendNotificationForUnAuthorized("ADMIN", createdBy);
            }
            _objLoggingProvider.Info($"API Request Log - OnAuthorization: END");
        }

        public static string Truncate(string input, int truncLength)
        {
            return (!String.IsNullOrEmpty(input) && input.Length >= truncLength)
                       ? input.Substring(0, truncLength)
                       : input;
        }

        private static void SaveQueueHistoryInvalidPayload(string QueueStatus, string QueueMessage, string UserSession)
        {
            try
            {
                QueueHistory queueHistory = new QueueHistory()
                {
                    SalesOrganization = "NA",
                    UserConfigId = 0,
                    Distributionchannel = "NA",
                    CustomerId = "NA",
                    PricingFiletype = "NA",
                    PriceStatus = "NA",
                    PricingDate = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    CustomerEmail = "NA",
                    QueueStatus = QueueStatus,
                    QueueMessage = QueueMessage.ToJson(),
                    IsActive = false,
                    IsDeleted = true,
                    CreatedBy = UserSession,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedBy = "",
                    ModifiedDate = DateTime.UtcNow,
                };
                objPriceListRepository.QueueHistoryRepository.InsertEntity(queueHistory);
            }
            catch (Exception ex)
            {
                _objLoggingProvider.Error(ex, "Error while Save Userlog Details");
            }
        }
        private static UserLog SaveUserLog(string UserSESA, AuthorizationFilterContext filterContext)
        {
            UserLog objUserLog = new();
            if (UserSESA == "PAYLOAD_INVALID" || UserSESA == "UNAUTHORIZED_USER")
            {
                UserSESA = "API_INVALID";
            }

            try
            {
                objUserLog = new UserLog()
                {
                    IPAddress = Truncate(filterContext.HttpContext.Connection.LocalIpAddress.ToString(), 50),
                    MachineName = Truncate(WindowsIdentity.GetCurrent().Name, 50),
                    OperatingSystem = Truncate(Environment.OSVersion.VersionString, 50),
                    UserAgent = Truncate(filterContext.HttpContext.Request?.Headers["user-agent"], 100),
                    UserHostAddress = Truncate(filterContext.HttpContext.Connection.RemoteIpAddress.ToString(), 100),
                    UserSESA = Truncate(UserSESA, 15)
                };
                var UserLog = objBackOpsRepository.UserLogRepository.InsertEntity(objUserLog);
                _objLoggingProvider.Info("API User Auth : Userlog Entry Succeeded");                
            }
            catch (Exception ex)
            {                
                _objLoggingProvider.Error(ex,"Error while Save Userlog Details");                
            }
            return objUserLog;
        }
    }
}