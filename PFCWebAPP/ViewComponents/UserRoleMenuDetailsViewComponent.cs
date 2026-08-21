using Microsoft.AspNetCore.Mvc;
using PFCWebAPP.Filters;
using PFCWebAPP.Repositories.BackOps.Interfaces;
using PFCWebAPP.Repositories.Common.Enums;
using PFCWebAPP.Repositories.Common.Interfaces;

namespace PFCWebAPP.ViewComponents
{
    [PFCAuthFilter]
    [PFCRoleBasedAuthorizeFilter]
    [ViewComponent(Name = "UserRoleMenuDetails")]
    public class UserRoleMenuDetailsViewComponent : ViewComponent
    {
        private readonly ILoggingProvider _objLoggingProvider;
        private readonly IBackOpsProvider _objBackOpsProvider;
        private readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// UserRoleMenuDetailsViewComponent
        /// </summary>
        /// <param name="objLoggingProvider"></param>
        /// <param name="objBackOpsProvider"></param>
        /// <param name="contextAccessor"></param>
        public UserRoleMenuDetailsViewComponent(ILoggingProvider objLoggingProvider, IBackOpsProvider objBackOpsProvider, IHttpContextAccessor contextAccessor)
        {
            _objLoggingProvider = objLoggingProvider;
            _objBackOpsProvider = objBackOpsProvider;
            httpContextAccessor = contextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync(int RoleID = 0)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: GetUserRoleMenuDetailsByRoleID-_RenderMenuBarByUserSESA");
                if (httpContextAccessor.HttpContext.Session.GetString("SelectedRoleID") != null)
                {
                    RoleID = Convert.ToInt32(httpContextAccessor.HttpContext.Session.GetString("SelectedRoleID").ToString());
                }
                var UserRoleMenuDetails = _objBackOpsProvider.GetUserRoleMenuDetailsByRoleID(RoleID);
                _objLoggingProvider.LogMessage(LogType.Info, "End: GetUserRoleMenuDetailsByRoleID-_RenderMenuBarByUserSESA");
                return await Task.FromResult((IViewComponentResult)View("RenderMenuBarByUserSESA", UserRoleMenuDetails));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
