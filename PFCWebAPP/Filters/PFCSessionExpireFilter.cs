using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using NLog;
using PFCWebAPP.Utilities;

namespace PFCWebAPP.Filters
{
    public class PFCSessionExpireFilter : Attribute, IAuthorizationFilter
    {
        private static NLog.ILogger _objLoggingProvider = LogManager.GetCurrentClassLogger();

        public PFCSessionExpireFilter()
        {
            //_objLoggingProvider.Info("SessionExpire Filter Filter : ========Constructor Initiated==========");
        }
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            try
            {
                if (filterContext.HttpContext.Session.GetString("SelectedRoleID") == null || filterContext.HttpContext.Session.GetString("JWTToken") == null || filterContext.HttpContext.Session.GetString("JWTTokenUser") == null)
                {
                    _objLoggingProvider.Trace("SessionExpired : Redirecting to " + AppConfig.SessionExpireUrl);

                    var redirectionUrl = AppConfig.SessionExpireUrl;
                    filterContext.Result = new RedirectResult(redirectionUrl);
                    return;
                }

            }
            catch (Exception ex)
            {
                _objLoggingProvider.Error(ex);
                throw;
            }
        }
    }
}
