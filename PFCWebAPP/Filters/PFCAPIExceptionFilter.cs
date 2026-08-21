using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Security;
using System.Dynamic;
using System.Security.Claims;
using ICSharpCode.SharpZipLib.Zip;

namespace PFCWebAPP.Filters
{
    public class PFCAPIExceptionFilter : ExceptionFilterAttribute
    {
        private static NLog.ILogger _objLoggingProvider = LogManager.GetCurrentClassLogger();
       
        public override void OnException(ExceptionContext context)
        {
            var apiResponse = CreateApiResponse(context.Exception);

            context.Result = new ObjectResult(apiResponse)
            {
                StatusCode = apiResponse.StatusCode
            };
            context.ExceptionHandled = true;
        }
        private ApiResponse CreateApiResponse(Exception exception)
        {
            // Determine the type of exception and set the response accordingly
            if (exception is ApiException apiException)
            {
                return new ApiResponse
                {
                    StatusCode = apiException.StatusCode,
                    Message = apiException.ErrorMessage,
                    // Include more details if necessary
                };
            }
            else
            {
                // Handle unknown exceptions
                return new ApiResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An unexpected error occurred.",
                    // Include more details if necessary
                };
            }
        }
        public class ApiResponse
        {
            public int StatusCode { get; set; }
            public string Message { get; set; }
            // You can include additional properties as needed
        }
        public class ApiException : Exception
        {
            public int StatusCode { get; }
            public string ErrorMessage { get; }

            public ApiException(int statusCode, string errorMessage)
            {
                StatusCode = statusCode;
                ErrorMessage = errorMessage;
            }
        }
    }
}
