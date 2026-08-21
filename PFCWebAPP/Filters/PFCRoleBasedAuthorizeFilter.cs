using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NLog;
using PFCWebAPP.Repositories.Common.Interfaces;
using PFCWebAPP.Repositories.Common.ServiceProviders;
using System.Data;
using PFCWebAPP.DatabaseContext.Models;
using PFCWebAPP.Repositories.BackOps;

namespace PFCWebAPP.Filters
{
    public class PFCRoleBasedAuthorizeFilter : Attribute, IAuthorizationFilter
    {

        private static NLog.ILogger _objLoggingProvider = LogManager.GetCurrentClassLogger();
        //private static IBackOpsRepository _objBackOpsRepository;

        public PFCRoleBasedAuthorizeFilter()
        {
           
        }




        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            try
            {
                //_objBackOpsRepository = (IBackOpsRepository)filterContext.HttpContext.RequestServices.GetService(typeof(IBackOpsRepository));

                int SelectedRoleID = -1;
                if (filterContext.HttpContext.Session.GetString("SelectedRoleID") != null)
                {
                    SelectedRoleID = Convert.ToInt32(filterContext.HttpContext.Session.GetString("SelectedRoleID").ToString());
                }
                var descriptor = filterContext.ActionDescriptor as ControllerActionDescriptor;
                var actionName = descriptor.ActionName;
                var controllerName = descriptor.ControllerName;

                using (ISqlHelper objSqlHelper = new SqlHelper())
                {
                    string strQuery = "select Top 1 1 from [dbo].[MST_RoleMenus] RM " +
                   "inner join dbo.MST_Roles R on R.RoleID = RM.RoleID " +
                   "inner join dbo.MST_Menus M on M.MenuID = RM.MenuID " +
                   "inner join dbo.MST_UserRoleMapping UR on UR.RoleID = R.RoleID " +
                   "where M.IsActive = 1 and R.IsActive = 1 " +
                   "and M.IsActive = 1 and UR.IsActive =1 and RM.IsActive =1 " +
                   "and R.RoleID = @RoleID and M.ControllerName =@ControllerName " +
                   "and M.ActionName =@ActionName";

                    List<SqlParameter> lstSqlParameters = new List<SqlParameter>();

                    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@RoleID", Value = SelectedRoleID });
                    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@ControllerName", Value = controllerName });
                    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@ActionName", Value = actionName });

                    var dtRoleMenus = objSqlHelper.ExecuteTable(CommandType.Text, strQuery, lstSqlParameters.ToArray());


                    //bool isExits = (from RM in _objBackOpsRepository.RoleMenuRepository.GetManyQueryable()
                    //         join R in _objBackOpsRepository.RoleRepository.GetManyQueryable() on RM.RoleID equals R.RoleID
                    //         join M in _objBackOpsRepository.MenuRepository.GetManyQueryable() on RM.MenuID equals M.MenuID
                    //         join UR in _objBackOpsRepository.UserRoleMappingRepository.GetManyQueryable() on R.RoleID equals UR.RoleID
                    //         where M.IsActive == true && R.IsActive == true && RM.IsActive == true && UR.IsActive == true
                    //         && M.IsActive == true && R.RoleID == SelectedRoleID && M.ControllerName == controllerName && M.ActionName == actionName
                    //         select M).Any();

                   
                    if (dtRoleMenus != null && dtRoleMenus.Rows.Count > 0) //isExits
                    {
                        //return true;
                    }
                    else
                    {
                        //return false;
                        filterContext.Result = new RedirectToRouteResult(
                                          new RouteValueDictionary(new
                                          {
                                              action = "UnAuthorisedUserRole",
                                              controller = "UnAuthorised"
                                          }
                                          ));
                    }
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
