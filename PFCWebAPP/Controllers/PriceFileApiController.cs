using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PFCWebAPP.Repositories.Common.Interfaces;
using PFCWebAPP.Repositories.Configure;
using PFCWebAPP.Repositories.Configure.Interfaces;
using PFCWebAPP.Repositories.PriceList;
using PFCWebAPP.Repositories.PriceList.Interfaces;
using PFCWebAPP.Repositories.PriceList.Models.API;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using PFCWebAPP.Filters;
using System.Xml.Serialization;
using System.Xml;
using Microsoft.IdentityModel.Tokens;
using PFCWebAPP.Repositories.PriceList.Models;
using NPOI.SS.Formula.Functions;
using NuGet.Protocol;
using NPOI.XSSF.Streaming.Values;
using PFCWebAPP.Filters;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Dynamic;
using PFCWebAPP.Utilities;
using PFCWebAPP.Repositories.Common.Enums;
using PFCWebAPP.DatabaseContext.Models.CustomTables;

namespace PFCWebAPP.Controllers
{
    [Route("api/GetCustomerPriceFile")]
    [ApiController]
    [PFCAPIAuthFilter]
    [PFCAPIExceptionFilter]
    public class PriceFileApiController : ControllerBase
    {
        private readonly ILoggingProvider _objLoggingProvider;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IPriceListProvider _priceListpProvider;
        public readonly ICommonProvider _objCommonProvider;
        public readonly INotificationProvider _notificationProvider;

        public PriceFileApiController(ILoggingProvider objLoggingProvider, IHttpContextAccessor contextAccessor, IPriceListProvider priceListpProvider,ICommonProvider objCommonProvider, INotificationProvider notificationProvider)
        {
            httpContextAccessor = contextAccessor;
            _objLoggingProvider = objLoggingProvider;
            _priceListpProvider = priceListpProvider;
            _objCommonProvider = objCommonProvider;
            _notificationProvider = notificationProvider;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PFCReport()
        {
            _objLoggingProvider.LogMessage(LogType.Info, "API Controller Action Started");
            MySEPriceList mySEPriceList;
            var TempOutPut = new Dictionary<string, string>();
            // Read the request body asynchronously
            using var reader = new StreamReader(Request.Body);
            var requestBody = await reader.ReadToEndAsync();

            // Check the Content-Type header
            var contentType = Request.ContentType;

            if (contentType == null || contentType == string.Empty)
            {
                ModelState.AddModelError("Custom Error", "Please Enter a Valid Input");
                _objLoggingProvider.LogMessage(LogType.Info, "Content Type is Empty");
                return BadRequest(ModelState);
            }
            if (contentType.Contains("application/xml"))
            {
                // Handle XML input

                // Create an XmlReaderSettings object
                var settings = new XmlReaderSettings
                {
                    ConformanceLevel = ConformanceLevel.Fragment,
                    IgnoreWhitespace = true,
                    IgnoreComments = true
                };

                // Create an XmlReader object
                using var xmlReader = XmlReader.Create(new StringReader(requestBody), settings);

                // Move to the root element
                xmlReader.MoveToContent();

                // Get the name and namespace of the root element
                var rootName = xmlReader.LocalName;
                var rootNamespace = xmlReader.NamespaceURI;

                // Create an XmlRootAttribute with the root element's name and namespace
                var rootAttribute = new XmlRootAttribute(rootName)
                {
                    Namespace = rootNamespace
                };

                // Create an XmlSerializer with the XmlRootAttribute and the type of your model
                var xmlSerializer = new XmlSerializer(typeof(MySEPriceList), rootAttribute);

                // Deserialize the XML data into an instance of your model
                mySEPriceList = (MySEPriceList)xmlSerializer.Deserialize(new StringReader(requestBody));
            }
            else if (contentType.Contains("application/json"))
            {
                // Handle JSON input

                mySEPriceList = System.Text.Json.JsonSerializer.Deserialize<MySEPriceList>(requestBody);
            }
            else
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Invalid Content-Type. Only application/xml and application/json are supported.");
                return BadRequest("Invalid Content-Type. Only application/xml and application/json are supported.");
            }

            // Validate MySEPriceList object
            var context = new ValidationContext(mySEPriceList, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(mySEPriceList, context, results, true);

            if (!isValid)
            {
                foreach (var validationResult in results)
                {
                    ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
                }
                _objLoggingProvider.LogMessage(LogType.Error, "ValidationResult : Error");
                return BadRequest(ModelState);
            }

            QueueModel queueModel = new()
            {
                CustomerId = mySEPriceList.CustomerId.Trim().ToString(),
                CustomerEmail = mySEPriceList.CustomerEmail.Trim().ToString(),
                Distributionchannel = mySEPriceList.DistributionChannel.Trim().ToString(),
                SalesOrganization = mySEPriceList.SalesOrganization.Trim().ToString().ToUpper(),
                PricingFiletype = mySEPriceList.PricingFileType.Trim().ToString(),
                PriceStatus = mySEPriceList.PriceStatus.Trim().ToString(),
                PricingDate = Convert.ToDateTime(mySEPriceList.PricingDate),
                CreatedBy = httpContextAccessor.HttpContext.Session.GetString("ApiSession").ToString()
            };

            var ApiRequestResponse = _priceListpProvider.SaveApiRequests(queueModel);

            if (ApiRequestResponse == null) 
            {
                _objLoggingProvider.LogMessage(LogType.Error, "ApiRequestResponse : Unknown Error");
                return BadRequest("Unknown Error! Please Contact Admin");
            }
            _objLoggingProvider.LogMessage(LogType.Info, "API Controller Action Completed");            
            _objLoggingProvider.LogMessage(LogType.Info, "API Request Log: Completed");
            _objLoggingProvider.LogMessage(LogType.Info, "***************API*************");
            return Ok("We have received the request");

        }
    }
}