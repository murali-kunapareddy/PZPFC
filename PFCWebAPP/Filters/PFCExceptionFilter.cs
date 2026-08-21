using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Security;

namespace PFCWebAPP.Filters
{
    public class PFCExceptionFilter : Attribute, IExceptionFilter
    {

        private static NLog.ILogger _objLoggingProvider = LogManager.GetCurrentClassLogger();

        public void OnException(ExceptionContext context)
        {
            try
            {
                switch (context.Exception)
                {
                    case SecurityException _:
                        SetCodeAndMsg(context, 403, context.Exception.Message);
                        break;
                    case AggregateException _:
                        var aggrException = (AggregateException)context.Exception;
                        SetCodeAndMsg(context, 400, string.Join(". ", aggrException.InnerExceptions.Select(x => x.Message)));
                        break;
                    case ArgumentException _:
                        SetCodeAndMsg(context, 400, context.Exception.Message);
                        break;
                    //case NullReferenceException _:
                    //    SetCodeAndMsg(context, 500, context.Exception.Message);
                    //    break;
                    default:
                        SetCodeAndMsg(context, 500, context.Exception.Message);
                        break;
                }
            }
            catch (Exception ex)
            {
                _objLoggingProvider.Error(ex);
            }
        }


        private static void SetCodeAndMsg(ExceptionContext context, int code, string message)
        {
            try
            {
                _objLoggingProvider.Error("Exception", context.Exception);

                context.Result = new RedirectToRouteResult(
                                         new RouteValueDictionary(new
                                         {
                                             action = "Info",
                                             controller = "Error",
                                             StatusCode = code
                                         }
                                         ));
            }
            catch (Exception ex)
            {
                _objLoggingProvider.Error(ex);
            }
        }
    }
}
