using PFCRepository.Repositories.BackOps.Models;
using PFCRepository.Repositories.Common.Interfaces;
using PFCRepository.Utilities;
using Microsoft.Data.SqlClient;
using System.Data;
using PFCRepository.Repositories.Common.ServiceProviders;
using PFCRepository.Repositories.BackOps.Interfaces;
using PFCRepository.DatabaseContext.Models.CustomTables;
using System.Transactions;
using PFCRepository.DatabaseContext;
using NPOI.SS.Formula.Functions;
using NPOI.SS.Formula.Eval;
using static NPOI.HSSF.Util.HSSFColor;
using PFCRepository.Repositories.Common.Enums;

namespace PFCRepository.Repositories.BackOps.ServiceProviders
{
    public class BackOpsProvider : IBackOpsProvider
    {
        private readonly ILoggingProvider _objLoggingProvider;
        private readonly IBackOpsRepository _objBackOpsRepository;
        public readonly ICommonProvider _objCommonProvider;
        public BackOpsProvider(ILoggingProvider objLoggingProvider, IBackOpsRepository objBackOpsRepository, ICommonProvider objCommonProvider)
        {
            _objLoggingProvider = objLoggingProvider;
            _objBackOpsRepository = objBackOpsRepository;
            _objCommonProvider = objCommonProvider;

        }


        #region MenuBar
        /// <summary>
        /// GetMenuItemsByRoleID
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        

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
