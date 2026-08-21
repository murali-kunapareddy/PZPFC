using PFCWebAPP.DatabaseContext.Models.CustomTables;
using PFCWebAPP.Repositories.Configure.Models;
using System.Data;

namespace PFCWebAPP.Repositories.Configure.Interfaces
{
    public interface IConfigureProvider : IDisposable
    {
        /// <summary>
        /// GetReportFormats
        /// </summary>
        /// <param name=""></param>
        /// <returns>List of Report Formats</returns>
        IQueryable<ReportFormatMaster> GetReportFormats();


        /// <summary>
        /// GetTemplatesByCategory
        /// </summary>
        /// <param name="CategoryName"></param>
        /// <returns></returns>
        List<TemplateMasterDetails> GetTemplatesByCategory(string CategoryName);

        /// <summary>
        /// GetTemplateMasterDetailsByTemplateID
        /// </summary>
        /// <param name="TemplateMasterID"></param>
        /// <returns></returns>
        TemplateMasterDetails GetTemplateMasterDetailsByTemplateID(int TemplateMasterID);

        /// <summary>
        /// GetTemplateStructureIntoByTemplateID
        /// </summary>
        /// <param name="TemplateMasterID"></param>
        /// <returns></returns>
        List<TemplateStructure> GetTemplateStructureIntoByTemplateID(int TemplateMasterID);

        /// <summary>
        /// GetTemplateDataByTemplateID
        /// </summary>
        /// <param name="TemplateMasterID"></param>
        /// <returns></returns>
        DataTable GetTemplateDataByTemplateID(int TemplateMasterID, int DisplayMaxRecords = 0);

        /// <summary>
        /// GetTemplateDataByTemplateIDV2
        /// </summary>
        /// <param name="TemplateMasterID"></param>
        /// <param name="DisplayMaxRecords"></param>
        /// <returns></returns>
        DataSet GetTemplateDataByTemplateIDV2(int TemplateMasterID, int DisplayMaxRecords = 0);

        /// <summary>
        /// UpdateExcelDataIntoTemplateTables
        /// </summary>
        /// <param name="FileNameWithPath"></param>
        /// <param name="TemplateMasterID"></param>
        /// <returns></returns>
        bool UpdateExcelDataIntoTemplateTables(string FileNameWithPath, int TemplateMasterID);

        /// <summary>
        /// GetReportFormatDetailsByID
        /// </summary>
        /// <param name="TemplateMasterID"></param>
        /// <returns>List of Report Format Details by ID</returns>
        List<ReportFormatDataTableViewModel> GetReportFormatDetailsByID(int TemplateMasterID);


        /// <summary>
        /// GetTemplateInfoByTemplateName
        /// </summary>
        /// <param name="TemplateName"></param>
        /// <returns>To Validate if Template Exixts or not</returns>
        List<TemplateMaster> GetTemplateInfoByTemplateName(string TemplateName);

        /// <summary>
        /// GetReportContents
        /// </summary>
        /// <returns>List of TemplateMasters</returns>
        //IQueryable<TemplateMaster> GetReportContents();

        /// <summary>
        /// SaveReportContent
        /// </summary>
        /// <param name="rcM"></param>
        /// <returns>Primary key after Insertion of Report Content</returns>
        int SaveReportContent(ReportContentModel rcM);

        /// <summary>
        /// DeleteTemplateMaster
        /// </summary>
        /// <param name="TemplateMasterID"></param>
        /// <returns>true/false after Updating the IsActive and IsDeleted status to false and true respectively</returns>
        bool DeleteTemplateMaster(int TemplateMasterID);

        /// <summary>
        /// IsTemplateDeleted
        /// </summary>
        /// <param name="TemplateName"></param>
        /// <param name="CountryCode"></param>
        /// <returns>integer based on condition</returns>
        int IsTemplateDeleted(string TemplateName, string CountryCode);

        /// <summary>
        /// ReActivateTemplate
        /// </summary>
        /// <param name="TemplateName"></param>
        /// <param name="CountryCode"></param>
        /// <returns>true/false after Updating the IsActive and IsDeleted status to true and false respectively</returns>
        int ReActivateTemplate(string TemplateName, string CountryCode);

        List<AppConfigSetting> GetAppConfigSettings();

        List<ConfigOptions> GetAppConfigOptions();

        bool SaveAppConfigSettingDetails(List<AppConfigSetting> objAppConfigSettingModel);

        IQueryable<UserLog> GetUsersLogInfo();


        /// <summary>
        /// SaveCustomerTemplate
        /// </summary>
        /// <param name="rcM"></param>
        /// <returns>Primary key after Insertion of Report Content</returns>
        int SaveCustomerTemplate(ReportContentModel rcM);



    }
}
