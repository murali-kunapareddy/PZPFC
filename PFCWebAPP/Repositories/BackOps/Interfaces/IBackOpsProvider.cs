using PFCWebAPP.DatabaseContext.Models.CustomTables;
using PFCWebAPP.Repositories.BackOps.Models;

namespace PFCWebAPP.Repositories.BackOps.Interfaces
{
    public interface IBackOpsProvider : IDisposable
    {
        UserRoleMenuModel GetUserRoleMenuDetailsByRoleID(int RoleID);

        IQueryable<Role> GetRoles();

        List<UserRolesModel> GetUserRoleMappingDetails();

        UserRoleMappingViewModel GetUserRoleMappingDetailsBySESA(string UserSESA);

        bool SaveUserRoleMappingDetails(UserRoleMappingViewModel UserRoleMappingViewModel);

        IQueryable<UserMaster> GetUsers();

        IQueryable<UserMaster> GetUserBySESA(string UserSESA);

        UserMaster SaveUser(UserMaster user);

        bool DeleteorReActivateUser(string UserSESA, bool CanReActivateUser);

        bool IsExistigRecord(string UserSESA);

        ETLJobProcessHistory GetETLJobStatus();


    }
}
