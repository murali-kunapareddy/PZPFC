using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using NLog;
using PFCWebAPP.Repositories.BackOps;
using SE.CA.PingComponent.Entities;
using SE.CA.PingComponent;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using PFCWebAPP.DatabaseContext.Models.CustomTables;

namespace PFCWebAPP.Filters
{
    public class PFCAuthFilter : PingAuth, IAuthorizationFilter
    {
        private static NLog.ILogger _objLoggingProvider = LogManager.GetCurrentClassLogger();
        private static IBackOpsRepository objBackOpsRepository;

        /// <summary>
        /// PFCAuthFilter
        /// </summary>
        public PFCAuthFilter()
        {
            //_objLoggingProvider.Info("PING PFCAuth Filter : ========Constructor Initiated==========");
        }



        /// </summary>
        /// <param name="filterContext"></param>
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            objBackOpsRepository = (IBackOpsRepository)filterContext.HttpContext.RequestServices.GetService(typeof(IBackOpsRepository));
            try
            {
                PingAuthResponse PingAuthResponseEnum = base.IsAuthorizeUser(filterContext);
                switch (PingAuthResponseEnum)
                {
                    case PingAuthResponse.CodeRedirectURL: //CodeRedirectURL will come when Code not exist & Get Home Page Redirection
                        _objLoggingProvider.Info("PING Auth : CodeRedirectURL");
                        break;
                    case PingAuthResponse.Success:
                        #region Session Value Check : Extra Validation Check

                        if (filterContext.HttpContext.Session != null && filterContext.HttpContext.Session.GetString("JWTToken") != null && filterContext.HttpContext.Session.GetString("JWTTokenUser") != null)
                        {
                            string JWTTokenUser = filterContext.HttpContext.Session.GetString("JWTTokenUser").ToString();
                            string tkn = filterContext.HttpContext.Session.GetString("JWTToken").ToString();

                            if (!String.IsNullOrEmpty(tkn) && !String.IsNullOrEmpty(JWTTokenUser))
                            {
                                _objLoggingProvider.Info("PING Auth : Success");

                                 
                                if (new UserInfo().GetUserSesa(tkn).Trim().ToUpper() != JWTTokenUser.Trim().ToUpper())
                                {
                                    _objLoggingProvider.Error("Token Mismatch: ", "JWTTokenUser: " + JWTTokenUser + " JWTToken:" + tkn);
                                    filterContext.HttpContext.Session.Clear();
                                    base.IsAuthorizeUser(filterContext);
                                }
                                else if (filterContext.HttpContext.Session.GetString("PingTokenValidatedFrom").ToString().Trim().ToUpper() == PingTokenValidationType.TokenValidatedFromPingServer.ToString().Trim().ToUpper())
                                {
                                    _objLoggingProvider.Info("TokenValidatedFromPingServer: "+ tkn);

                                    // add user logs on first time user login
                                    var UserDetails = new UserInfo().GetUserDetails(tkn);

                                    //User found
                                    if (filterContext.HttpContext.Session.GetString("UserSESA") == null)
                                    {
                                        filterContext.HttpContext.Session.SetString("UserSESA", UserDetails.EmployeeSESA.ToString());

                                    }

                                    SaveUserLog(UserDetails, filterContext);
                                }

                            }
                            else
                            {
                                _objLoggingProvider.Error("PING Auth : JWTToken or JWTTokenUser Empty");
                                filterContext.HttpContext.Session.Clear();
                                base.IsAuthorizeUser(filterContext);


                            }
                        }
                        else
                        {
                            _objLoggingProvider.Error("PING Auth : Failed Session Null or Empty");
                            filterContext.HttpContext.Session.Clear();
                            base.IsAuthorizeUser(filterContext);

                        }
                        #endregion
                        break;
                    case PingAuthResponse.Failed:
                    case PingAuthResponse.InvalidJWTToken:
                    case PingAuthResponse.TokenExpired:
                        _objLoggingProvider.Info("PING Auth : Failed");
                        filterContext.Result = new RedirectToRouteResult(
                                      new RouteValueDictionary(new
                                      {
                                          action = "Index",
                                          controller = "UnAuthorised"
                                      }
                                      ));
                        break;


                }
            }
            catch (Exception ex)
            {
                _objLoggingProvider.Error(ex);
                throw;
            }


        }


       
        public static string Truncate(string input, int truncLength)
        {
            return (!String.IsNullOrEmpty(input) && input.Length >= truncLength)
                       ? input.Substring(0, truncLength)
                       : input;
        }


        private static void SaveUserLog(UserInfo UserDetails, AuthorizationFilterContext filterContext)
        {
            try
            {

                UserLog objUserLog = new UserLog()
                {
                    IPAddress = Truncate(filterContext.HttpContext.Connection.LocalIpAddress.ToString(), 50),
                    MachineName = Truncate(WindowsIdentity.GetCurrent().Name, 50),
                    OperatingSystem = Truncate(Environment.OSVersion.VersionString, 50),
                    UserAgent = Truncate(filterContext.HttpContext.Request?.Headers["user-agent"], 100),
                    UserHostAddress = Truncate(filterContext.HttpContext.Connection.RemoteIpAddress.ToString(), 100),
                    UserSESA = UserDetails.EmployeeSESA
                };
                objBackOpsRepository.UserLogRepository.InsertEntity(objUserLog);
                _objLoggingProvider.Info("PING Auth : Userlog");


            }
            catch (Exception ex)
            {
                _objLoggingProvider.Error("Error while Save Userlog Details", ex);
            }
        }
    }
}
