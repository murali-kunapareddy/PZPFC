using PFCRepository.DatabaseContext;
using PFCRepository.DatabaseContext.Models.CustomTables;

namespace PFCRepository.Repositories.BackOps
{
    public class BackOpsRepository : IBackOpsRepository
    {
        #region Private member variables...

        private readonly PFCDBContext _dbContext;

        private GenericRepository<Menu> _MenuRepository;
        private GenericRepository<Role> _RoleRepository;
        private GenericRepository<RoleMenu> _RoleMenuRepository;
        private GenericRepository<UserLog> _UserLogRepository;
        private GenericRepository<UserMaster> _UserMasterRepository;
        private GenericRepository<UserRoleMapping> _UserRoleMappingRepository;
        private GenericRepository<UserConfigSetting> _UserConfigSettingRepository;
        private GenericRepository<TemplateMaster> _TemplateMasterRepository;
        private GenericRepository<TemplateData> _TemplateDataRepository;

        #endregion

        /// <summary>
        /// BackOpsRepository
        /// </summary>
        /// <param name="dbContext"></param>
        public BackOpsRepository(PFCDBContext dbContext)
        {
            _dbContext =  dbContext;
        }

        #region Public Repository Creation properties...
        /// <summary>
        /// 
        /// </summary>
        public GenericRepository<Menu> MenuRepository => _MenuRepository ?? (_MenuRepository = new GenericRepository<Menu>(_dbContext));
        public GenericRepository<Role> RoleRepository => _RoleRepository ?? (_RoleRepository = new GenericRepository<Role>(_dbContext));
        public GenericRepository<RoleMenu> RoleMenuRepository => _RoleMenuRepository ?? (_RoleMenuRepository = new GenericRepository<RoleMenu>(_dbContext));
        public GenericRepository<UserLog> UserLogRepository => _UserLogRepository ?? (_UserLogRepository = new GenericRepository<UserLog>(_dbContext));
        public GenericRepository<UserMaster> UserMasterRepository => _UserMasterRepository ?? (_UserMasterRepository = new GenericRepository<UserMaster>(_dbContext));
        public GenericRepository<UserRoleMapping> UserRoleMappingRepository => _UserRoleMappingRepository ?? (_UserRoleMappingRepository = new GenericRepository<UserRoleMapping>(_dbContext));
        public GenericRepository<UserConfigSetting> UserConfigSettingRepository => _UserConfigSettingRepository ?? (_UserConfigSettingRepository = new GenericRepository<UserConfigSetting>(_dbContext));
        public GenericRepository<TemplateMaster> TemplateMasterRepository => _TemplateMasterRepository ?? (_TemplateMasterRepository = new GenericRepository<TemplateMaster>(_dbContext));
        public GenericRepository<TemplateData> TemplateDataRepository => _TemplateDataRepository ?? (_TemplateDataRepository = new GenericRepository<TemplateData>(_dbContext));



        #endregion






        #region Implementing IDiosposable...

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
        #endregion



    }
}
