using PFCWebAPP.Repositories.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PFCWebAPP.Repositories.Common.Enums;
using PFCWebAPP.Filters;

namespace PFCWebAPP.Controllers
{
    [PFCExceptionFilter]
    public class AccountController : BaseController
    {

        private readonly ILoggingProvider _objLoggingProvider;
        private readonly IHttpContextAccessor _objHttpContextAccessor;

        public AccountController(ILoggingProvider objLoggingProvider, IHttpContextAccessor objHttpContextAccessor)
        {
            _objLoggingProvider = objLoggingProvider;
            _objHttpContextAccessor = objHttpContextAccessor;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult Index(string code = "")
        {
            try
            {
                if (_objHttpContextAccessor.HttpContext.Session.GetString("JWTToken") == null)
                    return RedirectToAction("AccessDenied", "UnAuthorised");
                else
                    return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Account ", ex);
                throw;
            }
        }

        /// <summary>
        /// SwitchRole
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public ActionResult SwitchRole(int RoleID)
        {
            try
            {
                _objHttpContextAccessor.HttpContext.Session.SetString("SelectedRoleID", RoleID.ToString());
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("SwitchRole :" , ex);
                throw;
            }
        }
    }
}
