using PFCWebAPP.Repositories.BackOps.Models;
using PFCWebAPP.Repositories.Common.Interfaces;
using PFCWebAPP.Utilities;
using Microsoft.Data.SqlClient;
using SE.CA.PingComponent.Entities;
using System.Data;
using PFCWebAPP.Repositories.Common.ServiceProviders;
using PFCWebAPP.Repositories.BackOps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PFCWebAPP.DatabaseContext.Models.CustomTables;
using PFCWebAPP.Filters;
using System.Transactions;
using PFCWebAPP.DatabaseContext;
using NuGet.Protocol;
using NPOI.SS.Formula.Functions;
using NPOI.SS.Formula.Eval;
using static NPOI.HSSF.Util.HSSFColor;
using PFCWebAPP.Repositories.Common.Enums;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Newtonsoft.Json;
using Org.BouncyCastle.Tls;

namespace PFCWebAPP.Repositories.BackOps.ServiceProviders
{
    public class BackOpsProvider : IBackOpsProvider
    {
        private readonly ILoggingProvider _objLoggingProvider;
        private readonly IBackOpsRepository _objBackOpsRepository;
        private readonly IHttpContextAccessor _objHttpContextAccessor;
        public readonly ICommonProvider _objCommonProvider;
        public BackOpsProvider(ILoggingProvider objLoggingProvider, IBackOpsRepository objBackOpsRepository, IHttpContextAccessor objHttpContextAccessor, ICommonProvider objCommonProvider)
        {
            _objLoggingProvider = objLoggingProvider;
            _objBackOpsRepository = objBackOpsRepository;
            _objHttpContextAccessor = objHttpContextAccessor;
            _objCommonProvider = objCommonProvider;

        }


        #region MenuBar
        /// <summary>
        /// GetMenuItemsByRoleID
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public UserRoleMenuModel GetUserRoleMenuDetailsByRoleID(int RoleID)
        {
            List<Menu> lsMenu = new List<Menu>();
            UserRoleMenuModel URM = new UserRoleMenuModel();
            try
            {
                if (_objHttpContextAccessor.HttpContext.Session.GetString("JWTToken") != null)
                {
                    string tkn = _objHttpContextAccessor.HttpContext.Session.GetString("JWTToken").ToString();
                    if (!string.IsNullOrEmpty(tkn))
                    {
                        var UserDetails = new UserInfo().GetUserDetails(tkn);
                        URM.UserName = UserDetails.FirstName + " " + UserDetails.LastName;
                        URM.UserSESA = UserDetails.EmployeeSESA;
                        URM.ETLStartDate = DateTime.UtcNow;
                        URM.ETLEndDate = DateTime.UtcNow.AddHours(1);
                        if (_objCommonProvider.IsValidUser(UserDetails.EmployeeSESA))
                        {

                            URM.lstRoles = GetRolesByUserSESA(URM.UserSESA);
                            if (RoleID <= 0 && URM.lstRoles != null && URM.lstRoles.Count > 0)
                            {
                                URM.SelectedRoleID = URM.lstRoles.OrderBy(x => x.SortOrder).FirstOrDefault().RoleID;
                                URM.SelectedRoleName = URM.lstRoles.OrderBy(x => x.SortOrder).FirstOrDefault().RoleName;

                                _objHttpContextAccessor.HttpContext.Session.SetString("SelectedRoleID", URM.SelectedRoleID.ToString());
                            }
                            else if (RoleID > 0 && URM.lstRoles != null && URM.lstRoles.Count > 0 && URM.lstRoles.Where(x => x.RoleID == RoleID).Count() == 0)
                            {
                                URM.SelectedRoleID = URM.lstRoles.OrderBy(x => x.SortOrder).FirstOrDefault().RoleID;
                                URM.SelectedRoleName = URM.lstRoles.OrderBy(x => x.SortOrder).FirstOrDefault().RoleName;

                                _objHttpContextAccessor.HttpContext.Session.SetString("SelectedRoleID", URM.SelectedRoleID.ToString());
                            }
                            else if (URM.lstRoles == null || URM.lstRoles.Count == 0)
                            {
                                URM.SelectedRoleID = 0;

                                _objHttpContextAccessor.HttpContext.Session.SetString("SelectedRoleID", URM.SelectedRoleID.ToString());
                            }
                            else
                            {
                                URM.SelectedRoleID = RoleID;
                            }
                            if (URM.SelectedRoleID > 0)
                            {



                                lsMenu = (from RM in _objBackOpsRepository.RoleMenuRepository.GetManyQueryable()
                                          join R in _objBackOpsRepository.RoleRepository.GetManyQueryable() on RM.RoleID equals R.RoleID
                                          join M in _objBackOpsRepository.MenuRepository.GetManyQueryable() on RM.MenuID equals M.MenuID
                                          join UR in _objBackOpsRepository.UserRoleMappingRepository.GetManyQueryable() on R.RoleID equals UR.RoleID
                                          where M.IsActive == true && R.IsActive == true && RM.IsActive == true && UR.IsActive == true
                                          && M.IsActive == true && M.CanShowMenu== true && R.RoleID == URM.SelectedRoleID && UR.UserSESA == URM.UserSESA
                                          select new Menu
                                          {
                                              MenuID = M.MenuID,
                                              MenuName = M.MenuName,
                                              ParentID = M.ParentID,
                                              ControllerName = M.ControllerName,
                                              ActionName = M.ActionName,
                                              HrefVal = M.HrefVal,
                                              SortOrder= M.SortOrder,
                                              AliasName= M.AliasName,
                                              IsActive = true
                                          }).ToList();
                                if (lsMenu != null && lsMenu.Count > 0)
                                {
                                    URM.lstMenus = lsMenu;
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("GetUserRoleMenuDetailsByRoleID :" , ex);
                throw;
            }

            return URM;
        }

        /// <summary>
        /// GetRolesByUserSESA
        /// </summary>
        /// <param name="UserSESA"></param>
        /// <returns></returns>
        public List<Role> GetRolesByUserSESA(string UserSESA)
        {
            List<Role> lsRoles = new List<Role>();
            try
            {

                _objLoggingProvider.LogMessage(LogType.Info, "Start: Get User Roles by SESA");
                lsRoles = (from R in _objBackOpsRepository.RoleRepository.GetManyQueryable()
                           join UR in _objBackOpsRepository.UserRoleMappingRepository.GetManyQueryable() on R.RoleID equals UR.RoleID
                           where UR.UserSESA == UserSESA && UR.IsActive == true && R.IsActive == true
                           select new Role
                           {
                               RoleID = R.RoleID,
                               RoleName = R.RoleName,
                               SortOrder = R.SortOrder,
                               IsActive = true
                           }).ToList();
                _objLoggingProvider.LogMessage(LogType.Info, "End: Get User Roles by SESA");

            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("GetRolesByUserSESA :" ,ex);
                throw;
            }

            return lsRoles;

        }

        #endregion


        #region --Roles --

        /// <summary>
        /// Roles
        /// </summary>
        /// <returns>Returns List of Role which are Active state</returns>
        /// 
        
        public IQueryable<Role> GetRoles()
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: Get Predefined Roles");
                IQueryable<Role> lst_Role = _objBackOpsRepository.RoleRepository.GetManyQueryable().Where(x => x.IsActive == true);
                _objLoggingProvider.LogMessage(LogType.Info, "end: Get Predefined Roles");
                return lst_Role;

            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("GetRoles :" , ex);
                throw;
            }

        }

        #endregion


        #region -- Roles Mapping --

        /// <summary>
        /// GetUserRoleMappingDetails
        /// </summary>
        /// <param name=></param>
        /// <returns>Returns List of Users with their Roles</returns>
        public List<UserRolesModel> GetUserRoleMappingDetails()
        {
            List<UserRolesModel> lstUserRolesModel = new List<UserRolesModel>();
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: Get Roles Mapped to User");
                var UserTableData = _objBackOpsRepository.UserMasterRepository.GetManyQueryable(p => p.UserMasterID > 0 && p.IsActive == true);
                var UserRoles = _objBackOpsRepository.RoleRepository.GetManyQueryable(r =>r.IsActive == true);
                var UserRoleMapp = _objBackOpsRepository.UserRoleMappingRepository.GetManyQueryable(m =>m.IsActive == true);
                var lstUserRoles = (
                    from a in UserTableData
                    join mr in UserRoleMapp on a.UserSESA equals mr.UserSESA
                    join r in UserRoles on mr.RoleID equals r.RoleID
                    group r.RoleName by a into User
                    select new UserRolesModel
                    {
                        UserSESA = User.Key.UserSESA,
                        UserName = User.Key.FirstName +" "+ User.Key.LastName,
                        Email = User.Key.Email,
                        Roles = string.Join(", ", User) 
                    }
                    ).ToList();
                _objLoggingProvider.LogMessage(LogType.Info, "End: Get Roles Mapped to User");
                return lstUserRoles;
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("GetUserRoleMappingDetails :" , ex);
                throw;
            }

        }


        /// <summary>
        /// GetUserRoleMappingDetails
        /// </summary>
        /// <param name="UserSESA"></param>
        /// <returns>Scope for Updating User Roles</returns>
        public UserRoleMappingViewModel GetUserRoleMappingDetailsBySESA(string UserSESA)
        {
            UserRoleMappingViewModel UserRoleMappingViewModel = new UserRoleMappingViewModel();
            UserRoleMappingViewModel.UserSESA = UserSESA;
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: Get Roles Mapped to UserSESA");
                var UserTableData = _objBackOpsRepository.UserMasterRepository.GetQueryable(u =>u.UserSESA == UserSESA).FirstOrDefault();
                var UserRoles = _objBackOpsRepository.RoleRepository.GetManyQueryable(r => r.IsActive == true);
                var UserRoleMapp = _objBackOpsRepository.UserRoleMappingRepository.GetManyQueryable(m => m.UserSESA == UserSESA);
                var lstResult = (
                                 from r in UserRoles
                                 join mr in UserRoleMapp on r.RoleID equals mr.RoleID into res
                                 from re in res.DefaultIfEmpty()
                                 select new UserRoleMappingModel
                                 {
                                     RoleID = r.RoleID,
                                     RoleName = r.RoleName,
                                     UserRoleMappingID = re != null ? re.UserRoleMappingID : 0,
                                     UserSESA = UserTableData.UserSESA,
                                     UserRoleMappingStatus = re != null? re.IsActive:false,
                                 }
                                 ).ToList();    

                 UserRoleMappingViewModel.lstUserRoleMappingModel = lstResult;
                _objLoggingProvider.LogMessage(LogType.Info, "End: Get Roles Mapped to UserSESA");
                return UserRoleMappingViewModel;
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("GetUserRoleMappingDetailsBySESA :" , ex);
                throw;
            }

        }


        public bool SaveUserRoleMappingDetails(UserRoleMappingViewModel UserRoleMappingViewModel)
        {
            try
            {
                string UserSESA = _objCommonProvider.GetLoginUserSESA();

                foreach (var UserRoleMappingModel in UserRoleMappingViewModel.lstUserRoleMappingModel)
                {

                    using (var scope = new TransactionScope())
                    {
                        try
                        {
                            _objLoggingProvider.LogMessage(LogType.Info, "Start: Save User Roles");
                            UserRoleMapping URM = new UserRoleMapping();
                            URM.UserSESA = UserRoleMappingViewModel.UserSESA;
                            URM.RoleID = UserRoleMappingModel.RoleID;
                            URM.UserRoleMappingID = UserRoleMappingModel.UserRoleMappingID;
                            URM.IsActive = UserRoleMappingModel.UserRoleMappingStatus;

                            if (URM.UserRoleMappingID == 0 && UserRoleMappingModel.UserRoleMappingStatus == true)
                            {
                                URM.CreatedBy = UserSESA;
                                URM.CreatedDate = DateTime.UtcNow;
                                URM.ModifiedBy = UserSESA;
                                URM.ModifiedDate = DateTime.UtcNow;

                                _objBackOpsRepository.UserRoleMappingRepository.InsertEntity(URM);
                            }
                            else if(URM.UserRoleMappingID != 0)
                            {

                                URM.ModifiedBy = UserSESA;
                                URM.ModifiedDate = DateTime.UtcNow;
                                _objBackOpsRepository.UserRoleMappingRepository.UpdateEntity(URM);
                            }
                            scope.Complete();
                            _objLoggingProvider.LogMessage(LogType.Info, "End: Save User Roles");
                        }
                        catch (Exception ex)
                        {
                            scope.Dispose();
                            _objLoggingProvider.LogException("SaveUserRoleMappingDetails :" , ex);
                            throw;
                        }

                    }

                }

                int SelectedRoleID = -1;
                if (_objHttpContextAccessor.HttpContext.Session.GetString("SelectedRoleID") != null)
                {
                    SelectedRoleID = Convert.ToInt32(_objHttpContextAccessor.HttpContext.Session.GetString("SelectedRoleID").ToString());
                }
                if (_objHttpContextAccessor.HttpContext.Session.GetString("JWTToken") != null)
                {
                    string tkn = _objHttpContextAccessor.HttpContext.Session.GetString("JWTToken").ToString();
                    if (!string.IsNullOrEmpty(tkn))
                    {
                        var UserDetails = new UserInfo().GetUserDetails(tkn);
                        if (UserRoleMappingViewModel.UserSESA == UserDetails.EmployeeSESA)
                        {
                            var res = _objBackOpsRepository.UserRoleMappingRepository.GetFirst(x => x.IsActive == true && x.RoleID == SelectedRoleID && x.UserSESA == UserDetails.EmployeeSESA);
                            //string strQuery = "select top 1 1 from MST_UserRoleMapping M where M.IsActive = 1  and  M.RoleID = " + SelectedRoleID + " and M.UserSESA ='" + UserDetails.EmployeeSESA + "'";
                            //DataTable dtIsValidUserRoleMapping = _objBackOpsRepository.RoleMenuRepository.GetDataWithDataTable(strQuery);
                            if (res != null)
                            {
                                var lstRoles = GetRolesByUserSESA(UserDetails.EmployeeSESA);
                                if (lstRoles != null && lstRoles.Count > 0)
                                {
                                    SelectedRoleID = lstRoles.OrderBy(x => x.SortOrder).FirstOrDefault().RoleID;
                                    _objHttpContextAccessor.HttpContext.Session.SetString("SelectedRoleID", SelectedRoleID.ToString());
                                }
                                else
                                {
                                    _objHttpContextAccessor.HttpContext.Session.SetString("SelectedRoleID", "-1");
                                }

                            }
                        }
                    }
                }


            }
            catch (Exception ex) {
                _objLoggingProvider.LogException("SaveUserRoleMappingDetails :" , ex);
                throw;
            }
            return true;

        }

        #endregion


        #region -- User Management --

        /// <summary>
        /// GetUsers
        /// </summary>
        /// <returns>lst_UserMaster</returns>
        /// param : 
        public IQueryable<UserMaster> GetUsers()
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: Get Users Master List");
                IQueryable<UserMaster>? lst_UserMaster = null;
                lst_UserMaster = _objBackOpsRepository.UserMasterRepository.GetManyQueryable().Where(x => x.IsDeleted == false && x.IsActive == true);
                _objLoggingProvider.LogMessage(LogType.Info, "End: Get Users Master List");
                return lst_UserMaster;

            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("GetUsers :" , ex);
                throw;
            }

        }

        /// <summary>
        /// GetUserBySESA
        /// </summary>
        /// <returns>lst_UserMaster</returns>
        /// param : UserSESA

        public IQueryable<UserMaster> GetUserBySESA(string UserSESA)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: Get User Details by UserSESA");
                IQueryable<UserMaster>? lst_UserMaster = null;
                lst_UserMaster = _objBackOpsRepository.UserMasterRepository.GetManyQueryable().Where(x => x.UserSESA.ToUpper().Trim() == UserSESA.ToUpper().Trim() && x.IsDeleted == false && x.IsActive == true);
                _objLoggingProvider.LogMessage(LogType.Info, "End: Get User Details by UserSESA");
                return lst_UserMaster;
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("GetUserBySESA :" , ex);
                throw;
            }
        }

        /// <summary>
        /// SaveUser
        /// </summary>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        public UserMaster SaveUser(UserMaster UserInfo)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: SavePortal User");
                string UserSESA = _objCommonProvider.GetLoginUserSESA();
                var userRoles =  _objBackOpsRepository.RoleRepository.GetManyQueryable().Where(x => x.IsActive == true && x.RoleName.ToLower() == "User".ToLower()).ToList();

                using var scope = new TransactionScope();
                try
                {
                    if (UserInfo.UserMasterID == 0)
                    {
                        #region -- UserManagemnet --

                        UserInfo.CreatedBy = UserSESA;
                        UserInfo.CreatedDate = DateTime.UtcNow;
                        UserInfo.ModifiedBy = UserSESA;
                        UserInfo.ModifiedDate = DateTime.UtcNow;
                        UserInfo.Country = UserInfo.Country.ToUpper();

                        UserInfo = _objBackOpsRepository.UserMasterRepository.InsertEntity(UserInfo);

                        #endregion

                        #region -- UserRoleMapping --

                        UserRoleMapping URM = new();
                        URM.UserSESA = UserInfo.UserSESA;
                        URM.RoleID = userRoles[0].RoleID;
                        URM.IsActive = true;
                        URM.CreatedBy = UserSESA;
                        URM.CreatedDate = DateTime.UtcNow;
                        _objBackOpsRepository.UserRoleMappingRepository.InsertEntity(URM);

                        #endregion
                    }
                    else
                    {
                        UserInfo.ModifiedBy = UserSESA;
                        UserInfo.ModifiedDate = DateTime.UtcNow;
                        UserInfo.Country = UserInfo.Country.ToUpper();
                        _objBackOpsRepository.UserMasterRepository.UpdateEntity(UserInfo);
                    }
                    scope.Complete();
                    _objLoggingProvider.LogMessage(LogType.Info, "End: Save Portal User");
                    return UserInfo;
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    _objLoggingProvider.LogException("SaveUser :" , ex);
                    throw;
                }

            }
            catch (Exception)
            {
                throw;
            }

        }


        /// <summary>
        /// SaveUser
        /// </summary>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        public bool DeleteorReActivateUser(string UserSESA, bool CanReActivateUser)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: Delete or Reactivate User");
                string UpdatedBy = _objCommonProvider.GetLoginUserSESA();

                bool updateIsActiveStatus = false;
                bool updateIsDeletedStatus = false;

                UserMaster userMaster = _objBackOpsRepository.UserMasterRepository.GetFirst(u => u.UserSESA.ToUpper().Trim() == UserSESA.ToUpper().Trim());
                List<UserRoleMapping> userRoleMapp = _objBackOpsRepository.UserRoleMappingRepository.GetAllEntities(m => m.UserSESA.ToUpper().Trim() == UserSESA.ToUpper().Trim()).ToList();
                //List<UserConfigSetting> userConfigSettings = _objBackOpsRepository.UserConfigSettingRepository.GetAllEntities(m => m.UserSESA.ToUpper().Trim() == UserSESA.ToUpper().Trim()).ToList();


                if (CanReActivateUser == true)
                {
                    updateIsActiveStatus = true;
                    updateIsDeletedStatus = false;
                }
                else
                {
                    updateIsActiveStatus = false;
                    updateIsDeletedStatus = true;
                }

                using var scope = new TransactionScope();
                try
                {
                    #region -- Update UsersMaster --
                    if(userMaster != null)
                    {
                        userMaster.UserSESA = UserSESA;
                        userMaster.IsDeleted = updateIsDeletedStatus;
                        userMaster.IsActive = updateIsActiveStatus;
                        userMaster.ModifiedBy = UpdatedBy;
                        userMaster.ModifiedDate = DateTime.UtcNow;

                        _objBackOpsRepository.UserMasterRepository.UpdateEntity(userMaster);
                    }
                    

                    #endregion

                    #region -- Update UserRoleMapping --
                    if(userRoleMapp != null)
                    {
                        foreach (UserRoleMapping rM in userRoleMapp)
                        {

                            if(updateIsActiveStatus == true)
                            {
                                if(rM.RoleID == 3)
                                {
                                    rM.UserSESA = UserSESA;
                                    rM.IsActive = updateIsActiveStatus;
                                    rM.ModifiedBy = UpdatedBy;
                                    rM.ModifiedDate = DateTime.UtcNow;
                                    rM.IsDeleted = updateIsDeletedStatus;
                                }
                                else
                                {
                                    rM.UserSESA = UserSESA;
                                    rM.IsActive = false;
                                    rM.ModifiedBy = UpdatedBy;
                                    rM.ModifiedDate = DateTime.UtcNow;
                                    rM.IsDeleted = true;
                                }
                            }
                            else
                            {
                                rM.UserSESA = UserSESA;
                                rM.IsActive = updateIsActiveStatus;
                                rM.ModifiedBy = UpdatedBy;
                                rM.ModifiedDate = DateTime.UtcNow;
                                rM.IsDeleted = updateIsDeletedStatus;
                            }
                            

                            _objBackOpsRepository.UserRoleMappingRepository.UpdateEntity(rM);
                        }
                    }

                    #endregion

                    #region -- Update UserConfigSettings --

                    //if (userConfigSettings != null)
                    //{
                    //    foreach (UserConfigSetting rCS in userConfigSettings)
                    //    {
                    //        rCS.UserSESA = UserSESA;
                    //        rCS.IsActive = updateIsActiveStatus;
                    //        rCS.ModifiedBy = UpdatedBy;
                    //        rCS.ModifiedDate = DateTime.UtcNow;
                    //        rCS.IsDeleted = updateIsDeletedStatus;

                    //        _objBackOpsRepository.UserConfigSettingRepository.UpdateEntity(rCS);
                    //    }
                    //}
                    #endregion

                    #region -- Update UserSession


                    //updating the session if login userSESA and userSESA being Deleted both are same
                    if (UserSESA == UpdatedBy && CanReActivateUser == false)
                    {
                        _objHttpContextAccessor.HttpContext?.Session.SetString("SelectedRoleID", "0");
                    }
                    #endregion

                    scope.Complete();
                    _objLoggingProvider.LogMessage(LogType.Info, "End: Delete or Reactivate Portal User");
                    return true;
                }
                catch (Exception ex)
                {
                    _objLoggingProvider.LogMessage(LogType.Error, "DeleteorReActivateUser :" + ex.ToString());
                    scope.Dispose();
                    throw;
                   
                }

            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("DeleteorReActivateUser :" , ex);
                throw;
            }

        }

        /// <summary>
        /// SaveUser
        /// </summary>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        public bool IsExistigRecord(string UserSESA)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: Check If Existig Portal User");
                UserMaster userMaster = _objBackOpsRepository.UserMasterRepository.GetFirst(u => u.UserSESA.ToUpper().Trim() == UserSESA.ToUpper().Trim() && u.IsDeleted == true && u.IsActive == false);
                _objLoggingProvider.LogMessage(LogType.Info, "End: Check If Existig Portal User");
                if (userMaster != null) {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            catch(Exception ex)
            {
                _objLoggingProvider.LogException("IsExistigRecord :" , ex);
                throw;
            }
        }


        #endregion

        #region ETLJobStatusTracking
        public ETLJobProcessHistory GetETLJobStatus()
        {
            try
            {
                ETLJobProcessHistory objEtlProcessHistory = new ETLJobProcessHistory();
                DateTime todayDate = DateTime.Now.Date;
                DateTime yesterdayDate = todayDate.AddDays(-1);
                _objLoggingProvider.LogMessage(LogType.Info, "Start:GetETLJobStatus");

                objEtlProcessHistory = _objBackOpsRepository.ETLJobProcessHistoryRepository.GetManyQueryable()
                   .Where(j => j.ETLStartDateTime.Date <= todayDate
                   && j.ETLEndDateTime.Date >= todayDate)
                   .OrderBy(j => j.ETLJobProcessHistoryId)
                   .FirstOrDefault();

                if (objEtlProcessHistory == null)
                {
                    objEtlProcessHistory = _objBackOpsRepository.ETLJobProcessHistoryRepository.GetManyQueryable()
                       .Where(j => j.ETLStartDateTime.Date <= yesterdayDate
                       && j.ETLEndDateTime.Date >= yesterdayDate)
                       .OrderByDescending(j => j.ETLJobProcessHistoryId)
                       .FirstOrDefault();
                }

                _objLoggingProvider.LogMessage(LogType.Info, "End:GetETLJobStatus");
                return objEtlProcessHistory;

            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("GetETLJobStatus :", ex);
                throw;
            }
        }
        #endregion




        #region Dispose

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="isDispose"></param>
        private void Dispose(bool isDispose)
        {
            if (_objBackOpsRepository != null && isDispose)
            {
                _objBackOpsRepository.Dispose();
            }
            if (_objLoggingProvider != null && isDispose)
            {
                _objLoggingProvider.Dispose();
            }
            if (_objCommonProvider != null && isDispose)
            {
                _objCommonProvider.Dispose();
            }
            
        }


        #endregion
    }
}
