using PFCWebAPP.DatabaseContext.Models.CustomTables;

namespace PFCWebAPP.Repositories.Configure
{
    public interface IConfigureRepository : IDisposable
    {


        /// <summary>
        /// Get/Set Property for  AppConfigSetting  repository.
        /// </summary>
        GenericRepository<AppConfigSetting> AppConfigSettingRepository { get; }

        /// <summary>
        /// Get/Set Property for  UserConfigSetting  repository.
        /// </summary>
        GenericRepository<UserConfigSetting> UserConfigSettingRepository { get; }


        /// <summary>
        /// Get/Set Property for  TemplateCategory  repository.
        /// </summary>
        GenericRepository<TemplateCategory> TemplateCategoryRepository { get; }

        /// <summary>
        /// Get/Set Property for  TemplateMaster  repository.
        /// </summary>
        GenericRepository<TemplateMaster> TemplateMasterRepository { get; }

        /// <summary>
        /// Get/Set Property for  TemplateStructure  repository.
        /// </summary>
        GenericRepository<TemplateStructure> TemplateStructureRepository { get; }

        /// <summary>
        /// Get/Set Property for  TemplateDataStructure  repository.
        /// </summary>
        GenericRepository<TemplateData> TemplateDataRepository { get; }

        /// <summary>
        /// Get/Set Property for  MaterialMasterRepository  repository.
        /// </summary>
        GenericRepository<MaterialMaster> MaterialMasterRepository { get; }

        /// <summary>
        /// Get/Set Property for  CustomerContactRepository  repository.
        /// </summary>
        GenericRepository<CustomerContacts> CustomerContactRepository { get; }

        /// <summary>
        /// Get/Set Property for  ReportFormatMasterRepository  repository.
        /// </summary>
        GenericRepository<ReportFormatMaster> ReportFormatMasterRepository { get; }

        /// <summary>
        /// Get/Set Property for  ReportFormatMasterRepository  repository.
        /// </summary>
        GenericRepository<ReportFormatFieldMapping> ReportFormatFieldMappingRepository { get; }

        /// <summary>
        /// Get/Set Property for  ReportFormatMasterRepository  repository.
        /// </summary>
        GenericRepository<ReportFormatFieldMaster> ReportFormatFieldMasterRepository { get; }


        /// <summary>
        /// Get/Set Property for  ReportFormatMasterRepository  repository.
        /// </summary>
        GenericRepository<ConfigOptions> ConfigOptionsRepository { get; }

        /// <summary>
        /// Get/Set Property for  UserLogRepository  repository.
        /// </summary>
        GenericRepository<UserLog> UserLogRepository { get; }


    }
}
