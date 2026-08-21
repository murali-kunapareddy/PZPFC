using PFCRepository.DatabaseContext.Models.CustomTables;
using PFCRepository.Repositories.BackOps.Models;

namespace PFCRepository.Repositories.BackOps.Interfaces
{
    public interface IBackOpsProvider : IDisposable
    {
        

        IQueryable<Role> GetRoles();

        List<UserRolesModel> GetUserRoleMappingDetails();

        UserRoleMappingViewModel GetUserRoleMappingDetailsBySESA(string UserSESA);

        IQueryable<UserMaster> GetUsers();

        IQueryable<UserMaster> GetUserBySESA(string UserSESA);

        UserMaster SaveUser(UserMaster user);


        bool IsExistigRecord(string UserSESA);


    }
}
