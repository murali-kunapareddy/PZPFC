using PFCWebAPP.DatabaseContext.Models.CustomTables;
using PFCWebAPP.DatabaseContext;

namespace PFCWebAPP.Repositories.Configure
{
    public class ConfigureRepository : IConfigureRepository
    {
        #region Private member variables...

        private readonly PFCDBContext _dbContext;
        private GenericRepository<AppConfigSetting> _AppConfigSettingRepository;
        private GenericRepository<UserConfigSetting> _UserConfigSettingRepository;
        private GenericRepository<TemplateCategory> _TemplateCategoryRepository;
        private GenericRepository<TemplateMaster> _TemplateMasterRepository;
        private GenericRepository<TemplateStructure> _TemplateStructureRepository;
        private GenericRepository<TemplateData> _TemplateDataRepository;
        private GenericRepository<MaterialMaster> _MaterialMasterRepository;
        private GenericRepository<CustomerContacts> _CustomerContactDetailsRepository;
        private GenericRepository<ReportFormatMaster> _ReportFormatMasterRepository;
        private GenericRepository<ReportFormatFieldMapping> _ReportFormatFieldMappingRepository;
        private GenericRepository<ReportFormatFieldMaster> _ReportFormatFieldMasterRepository;
        private GenericRepository<ConfigOptions> _ConfigOptionsRepository;
        private GenericRepository<UserLog> _UserLogRepository;

        #endregion


        /// <summary>
        /// BackOpsRepository
        /// </summary>
        /// <param name="dbContext"></param>
        public ConfigureRepository(PFCDBContext dbContext)
        {
            _dbContext = dbContext;
        }


        public GenericRepository<AppConfigSetting> AppConfigSettingRepository => _AppConfigSettingRepository ?? (_AppConfigSettingRepository = new GenericRepository<AppConfigSetting>(_dbContext));
        public GenericRepository<UserConfigSetting> UserConfigSettingRepository => _UserConfigSettingRepository ?? (_UserConfigSettingRepository = new GenericRepository<UserConfigSetting>(_dbContext));
        public GenericRepository<TemplateCategory> TemplateCategoryRepository => _TemplateCategoryRepository ?? (_TemplateCategoryRepository = new GenericRepository<TemplateCategory>(_dbContext));
        public GenericRepository<TemplateMaster> TemplateMasterRepository => _TemplateMasterRepository ?? (_TemplateMasterRepository = new GenericRepository<TemplateMaster>(_dbContext));
        public GenericRepository<TemplateStructure> TemplateStructureRepository => _TemplateStructureRepository ?? (_TemplateStructureRepository = new GenericRepository<TemplateStructure>(_dbContext));
        public GenericRepository<TemplateData> TemplateDataRepository => _TemplateDataRepository ?? (_TemplateDataRepository = new GenericRepository<TemplateData>(_dbContext));
        public GenericRepository<MaterialMaster> MaterialMasterRepository => _MaterialMasterRepository ?? (_MaterialMasterRepository = new GenericRepository<MaterialMaster>(_dbContext));
        public GenericRepository<CustomerContacts> CustomerContactRepository => _CustomerContactDetailsRepository ?? (_CustomerContactDetailsRepository = new GenericRepository<CustomerContacts>(_dbContext));
        public GenericRepository<ReportFormatMaster> ReportFormatMasterRepository => _ReportFormatMasterRepository ?? (_ReportFormatMasterRepository = new GenericRepository<ReportFormatMaster>(_dbContext));
        public GenericRepository<ReportFormatFieldMapping> ReportFormatFieldMappingRepository => _ReportFormatFieldMappingRepository ?? (_ReportFormatFieldMappingRepository = new GenericRepository<ReportFormatFieldMapping>(_dbContext));
        public GenericRepository<ReportFormatFieldMaster> ReportFormatFieldMasterRepository => _ReportFormatFieldMasterRepository ?? (_ReportFormatFieldMasterRepository = new GenericRepository < ReportFormatFieldMaster >(_dbContext));
        public GenericRepository<ConfigOptions> ConfigOptionsRepository => _ConfigOptionsRepository ?? (_ConfigOptionsRepository = new GenericRepository<ConfigOptions>(_dbContext));

        public GenericRepository<UserLog> UserLogRepository => _UserLogRepository ?? (_UserLogRepository = new GenericRepository<UserLog> (_dbContext));

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
