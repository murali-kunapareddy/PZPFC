using MathNet.Numerics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Tls;
using PFCWebAPP.DatabaseContext.Models.CustomTables;
using PFCWebAPP.Filters;
using PFCWebAPP.Repositories.BackOps.Interfaces;
using PFCWebAPP.Repositories.BackOps.Models;
using PFCWebAPP.Repositories.Common.Enums;
using PFCWebAPP.Repositories.Common.Interfaces;

namespace PFCWebAPP.Controllers
{
    [PFCExceptionFilter]
    public class BackOpsController : BaseController
    {
        private readonly ILoggingProvider _objLoggingProvider;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IBackOpsProvider _objBackOpsProvider;


        public BackOpsController(ILoggingProvider objLoggingProvider, IHttpContextAccessor contextAccessor, IBackOpsProvider objBackOpsProvider)
        {
            httpContextAccessor = contextAccessor;
            _objLoggingProvider = objLoggingProvider;
            _objBackOpsProvider = objBackOpsProvider;
            _objLoggingProvider.LogMessage(LogType.Info, "BackOps Page");
        }
        public IActionResult Index()
        {
            return View();
        }


        #region ------ Roles --------
        /// <summary>
        /// Retrive Roles
        /// </summary>
        /// <returns></returns> 
        [PFCRoleBasedAuthorizeFilter]
        [PFCSessionExpireFilter]
        public ActionResult Roles()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Roles :" , ex);
                throw;
            }
            
        }

        /// <summary>
        /// Retrive Roles Info with Json format
        /// </summary>
        /// <returns>List of Roles</returns>
        [HttpPost]
        public ActionResult GetRoles()
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: GetRoles using Ajax Call");
                var lstRoles = _objBackOpsProvider.GetRoles();
                _objLoggingProvider.LogMessage(LogType.Info, "End: GetRoles using Ajax Call");
                return Json(lstRoles, new Newtonsoft.Json.JsonSerializerSettings());
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("GetRoles :" , ex);
                throw;
            }
        }

        #endregion


        #region -- User Role Mapping --

        [PFCRoleBasedAuthorizeFilter]
        [PFCSessionExpireFilter]
        public ActionResult UserRoleMapping() {
            return View();
        }


        /// <summary>
        /// GetUserRoleMapping
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUserRoleMapping()
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: GetUserRoleMapping using Ajax Call");
                var lstUserRoleMappingDetails = _objBackOpsProvider.GetUserRoleMappingDetails();
                _objLoggingProvider.LogMessage(LogType.Info, "End: GetUserRoleMapping using Ajax Call");
                return Json(lstUserRoleMappingDetails, new Newtonsoft.Json.JsonSerializerSettings());
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("GetUserRoleMapping :" , ex);
                throw;
            }
        }

        /// <summary>
        /// ModifyUserRoleMapping
        /// </summary>
        /// <param name="UserSESA"></param>
        /// <returns></returns>
        public ActionResult ModifyUserRoleMapping(string UserSESA)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: GetUserRoleMappingDetailsBySESA");
                if (string.IsNullOrEmpty(UserSESA))
                {
                    _objLoggingProvider.LogMessage(LogType.Info, "End: GetUserRoleMappingDetailsBySESA-NotAvailable/UnAuthorised");
                    return RedirectToAction("NotAvailable", "UnAuthorised", new { ErrorMessage = "Invalid User SESA" });
                }
                var lstUserRoleMappingDetails = _objBackOpsProvider.GetUserRoleMappingDetailsBySESA(UserSESA);
                if (lstUserRoleMappingDetails != null && lstUserRoleMappingDetails.lstUserRoleMappingModel.Where(x => x.UserSESA.ToUpper().Trim() == UserSESA.ToUpper().Trim()).Count() > 0)
                {
                    _objLoggingProvider.LogMessage(LogType.Info, "End: GetUserRoleMappingDetailsBySESA");
                    return Json(lstUserRoleMappingDetails);
                }
                else
                {
                    _objLoggingProvider.LogMessage(LogType.Info, "End: GetUserRoleMappingDetailsBySESA-NotAvailable/UnAuthorised");
                    return RedirectToAction("NotAvailable", "UnAuthorised", new { ErrorMessage = "Invalid User SESA" });
                }
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("ModifyUserRoleMapping :" , ex);
                throw;
            }
        }



        /// <summary>
        /// ModifyUserRoleMapping
        /// </summary>
        /// <param name="PortalUserRoleMappingDetails"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ModifyUserRoleMapping(UserRoleMappingViewModel PortalUserRoleMappingDetails)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: SaveUserRoleMappingDetails");
                var lstUserRoleMappingDetails = _objBackOpsProvider.SaveUserRoleMappingDetails(PortalUserRoleMappingDetails);
                _objLoggingProvider.LogMessage(LogType.Info, "End: SaveUserRoleMappingDetails");
                // return  RedirectToAction("ModifyUserRoleMapping", new { UserSESA = PortalUserRoleMappingDetails.UserSESA });
                return Json("Success");
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("ModifyUserRoleMapping :" , ex);
                throw;
            }
        }


        #endregion


        #region --- Users Management ---

        /// <summary>
        /// Users Mater Data
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [PFCRoleBasedAuthorizeFilter]
        [PFCSessionExpireFilter]
        [HttpGet]
        public IActionResult Users()
        {
            try
            {
                return View();
            }
            catch(Exception ex)
            {
                _objLoggingProvider.LogException("Users :" , ex);
                throw;
            }
            
        }

        /// <summary>
        /// Retrive Portal Users with Json format
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetUsers()
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: GetUsers using Ajax Call");
                var lstPortalusers = _objBackOpsProvider.GetUsers();
                _objLoggingProvider.LogMessage(LogType.Info, "End: GetUsers using Ajax Call");
                return Json(lstPortalusers, new Newtonsoft.Json.JsonSerializerSettings());
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("GetUsers :" , ex);
                throw;
            }
        }

        /// <summary>
        /// Add / Update User
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [PFCRoleBasedAuthorizeFilter]
        [PFCSessionExpireFilter]
        [HttpGet]
        public ActionResult AddUser(string UserSESA = "")
        {
            try
            {
                UserMaster UM = new();
                if (UserSESA != null && UserSESA.Trim() != "")
                {
                    _objLoggingProvider.LogMessage(LogType.Info, "Start: Fetch Portal UserInfo for Update");
                    var objUserMaster = _objBackOpsProvider.GetUserBySESA(UserSESA).FirstOrDefault();
                    _objLoggingProvider.LogMessage(LogType.Info, "End: Fetch Portal UserInfo for Update");
                    if (objUserMaster != null)
                        return View(objUserMaster);
                    else
                        _objLoggingProvider.LogMessage(LogType.Info, "Start: Fetch Portal UserInfo for Update");
                    return RedirectToAction("NotAvailable", "UnAuthorised", new { ErrorMessage = "Invalid User SESA" });
                }
                else
                    return View(UM);
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("AddUser :" , ex);
                throw;
            }
        }

        public ActionResult ModifyUser(string UserSESA = "")
        {
            try
            {
                UserMaster UM = new();
                if (UserSESA != null && UserSESA.Trim() != "")
                {
                    _objLoggingProvider.LogMessage(LogType.Info, "Start: Fetch Portal UserInfo for Update");
                    var objUserMaster = _objBackOpsProvider.GetUserBySESA(UserSESA).FirstOrDefault();
                    _objLoggingProvider.LogMessage(LogType.Info, "End: Fetch Portal UserInfo for Update");
                    if (objUserMaster != null)
                        return Json(objUserMaster);
                    else
                        _objLoggingProvider.LogMessage(LogType.Info, "Start: Fetch Portal UserInfo for Update");
                    return RedirectToAction("NotAvailable", "UnAuthorised", new { ErrorMessage = "Invalid User SESA" });
                }
                else
                    return View(UM);
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("ModifyUser :" , ex);
                throw;
            }
        }

        /// <summary>
        /// Add / Update User
        /// </summary>
        /// <param name="um"></param>
        /// <returns></returns>
        [HttpPost]
        [PFCRoleBasedAuthorizeFilter]
        [PFCSessionExpireFilter]
        public IActionResult AddUser(UserMaster um)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: AddPortal Users");
                if (ModelState.IsValid)
                {
                    _objBackOpsProvider.SaveUser(um);
                    _objLoggingProvider.LogMessage(LogType.Info, "End: AddPortal Users");
                    return RedirectToAction("Users");
                }
                else
                {
                    _objLoggingProvider.LogMessage(LogType.Warn, "Start: Invaid User Input");
                    return View(um);
                }

            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("AddUser :" , ex);
                throw;
            }
        }

        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="um"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateUser(UserMaster um)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: UpdatePortal Users");
                if (ModelState.IsValid)
                {
                    _objBackOpsProvider.SaveUser(um);
                    _objLoggingProvider.LogMessage(LogType.Info, "End: UpdatePortal Users");
                    return Json("success");
                }
                else
                {
                    _objLoggingProvider.LogMessage(LogType.Warn, "End: Invaid User Input");
                    return Json(um);
                }

            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("UpdateUser :" , ex);
                throw;
            }
        }


        /// <summary>
        /// DeleteorReActivateUser
        /// </summary>
        /// <param name="UserSESA"></param>
        /// <param name="CanReActivateUser"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteorReActivateUser(string UserSESA, bool CanReActivateUser)
        {
            try
            {
                if (UserSESA != null && UserSESA.Trim() != "")
                {
                    _objLoggingProvider.LogMessage(LogType.Info, "Start: Delete Portal User");
                    var objUserMaster = _objBackOpsProvider.DeleteorReActivateUser(UserSESA, CanReActivateUser);
                    _objLoggingProvider.LogMessage(LogType.Info, "End: Delete Portal Users");
                }
                return Json("success");
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("DeleteorReActivateUser", ex);
                ViewBag.DeleteErrorMessage = "Some thing went wrong, please contact support team";
                //throw;
                return Json("Some thing went wrong, please contact support team");
            }
        }


        /// <summary>
        /// To Check If User is Deleted
        /// </summary>
        /// <param name="UserSESA"></param>
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult IsDeletedPortalUser(string UserSESA)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: Is Deleted Portal User");
                var objUserMaster = _objBackOpsProvider.IsExistigRecord(UserSESA);
                _objLoggingProvider.LogMessage(LogType.Info, "End:Is Deleted Portal Users");
                return Json(objUserMaster, new Newtonsoft.Json.JsonSerializerSettings());
                
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("IsDeletedPortalUser :" , ex);
                throw;
            }

        }


        #endregion


        #region --- BackOps Validations ---
        /// <summary>
        /// Validate USER SESA
        /// </summary>
        /// <param name="UserSESA"></param>
        /// <returns>json</returns>
        public JsonResult IsUserNameAvailable(string UserSESA)
        {
            if (UserSESA != null)
            {
                var objUserMaster = _objBackOpsProvider.GetUserBySESA(UserSESA).FirstOrDefault();
                if (objUserMaster != null)
                    return Json(false, new Newtonsoft.Json.JsonSerializerSettings());
                return Json(true, new Newtonsoft.Json.JsonSerializerSettings());
            }
            else
                return Json(false, new Newtonsoft.Json.JsonSerializerSettings());
        }

        /// <summary>
        /// Validate Country Code
        /// </summary>
        /// <param name="Country"></param>
        /// <returns>json</returns>
        public JsonResult IsCountyCodeAvailable(string Country)
        {
            if (Country != null)
            {
                var CCode = from a in Utilities.Common.MasterCountryList()
                            where a.Key.ToUpper().Trim() == Country.ToString().ToUpper().Trim()
                            select a.Key;
                if (CCode == null || !CCode.Any())
                    return Json(false, new Newtonsoft.Json.JsonSerializerSettings());
                else
                    return Json(true, new Newtonsoft.Json.JsonSerializerSettings());
            }
            else
                return Json(false, new Newtonsoft.Json.JsonSerializerSettings());
        }
        #endregion

        #region ETLStatusTracking
        /// <summary>
        /// CheckIfETLJobIsInProgress
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public ETLJobProcessHistory CheckIfETLJobIsInProgress()
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: CheckIfETLJobIsInProgress Details using Ajax Call");
                return _objBackOpsProvider.GetETLJobStatus();
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("CheckIfETLJobIsInProgress :", ex);
                throw;
            }
        }
        #endregion
    }
}
