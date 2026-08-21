using PFCWebAPP.DatabaseContext.Models.CustomTables;

namespace PFCWebAPP.Repositories.BackOps
{
    public interface IBackOpsRepository : IDisposable
    {
        /// <summary>
        /// Get/Set Property for  Menu repository.
        /// </summary>
        GenericRepository<Menu> MenuRepository { get; }

        /// <summary>
        /// Get/Set Property for  Role repository.
        /// </summary>
        GenericRepository<Role> RoleRepository { get; }

        /// <summary>
        /// Get/Set Property for  RoleMenu repository.
        /// </summary>
        GenericRepository<RoleMenu> RoleMenuRepository { get; }    

        /// <summary>
        /// Get/Set Property for  UserLog repository.
        /// </summary>
        GenericRepository<UserLog> UserLogRepository { get; }

        /// <summary>
        /// Get/Set Property for  UserMaster repository.
        /// </summary>
        GenericRepository<UserMaster> UserMasterRepository { get; }

        /// <summary>
        /// Get/Set Property for  UserRoleMapping  repository.
        /// </summary>
        GenericRepository<UserRoleMapping> UserRoleMappingRepository { get; }

        /// <summary>
        /// Get/Set Property for  UserConfigSettings  repository.
        /// </summary>
        GenericRepository<UserConfigSetting> UserConfigSettingRepository { get; }

        /// <summary>
        /// Get/Set Property for  TemplateMaster  repository.
        /// </summary>
        GenericRepository<TemplateMaster> TemplateMasterRepository { get; }

        /// <summary>
        /// Get/Set Property for  TemplateData  repository.
        /// </summary>
        GenericRepository<TemplateData> TemplateDataRepository { get; }

        /// <summary>
        /// Get/Set Property for  ETLJobProcessHistory  repository.
        /// </summary>
        GenericRepository<ETLJobProcessHistory> ETLJobProcessHistoryRepository { get; }

        /// <summary>
        /// Get/Set Property for  MvkeOverride  repository.
        /// </summary>
        GenericRepository<MvkeOverride> MvkeOverrideRepository { get; }


    }
}
