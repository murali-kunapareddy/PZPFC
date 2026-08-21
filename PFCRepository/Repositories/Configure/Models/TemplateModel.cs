using System.ComponentModel.DataAnnotations;
using System.Data;

namespace PFCRepository.Repositories.Configure.Models
{
    /// <summary>
    /// TemplateModel
    /// </summary>
    public class TemplateModel
    {
        public List<DDLTemplateMasterDetails> ddlTemplateMasterDetails { get; set; }
        public DataTable SelectedTemplateDataInDataTable { get; set; }
        public int SelectedTemplateMasterID { get; set; }
        public TemplateMasterDetails SelectedTemplateDetails { get; set; }
        public TemplateSourceFileDetails objTemplateSourceFileDetails { get; set; }
        public string ColumnMisMatchDataInfo { get; set; }
        public string InvalidInfo { get; set; }
        public int DisplayMaxRecords { get; set; }
        public int TotalRecordsCount { get; set; }
    }

    /// <summary>
    /// TemplateMasterDetails
    /// </summary>
    public class TemplateMasterDetails
    {
        public int TemplateMasterID { get; set; }
        public int? TemplateCategoryID { get; set; }
        public string TemplateCategoryName { get; set; }
        public string TemplateName { get; set; }
        public string AliasName { get; set; }
        public string TemplateDataModel { get; set; }
        public string CountryCode { get; set; }
        public bool CanDuplicate { get; set; }
        public bool CanUpload { get; set; }
        public bool CanEdit { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// DDLTemplateMasterDetails
    /// </summary>
    public class DDLTemplateMasterDetails
    {
        public int TemplateMasterID { get; set; }
        public string TemplateName { get; set; }
    }


    /// <summary>
    /// TemplateSourceFileDetails
    /// </summary>
    public class TemplateSourceFileDetails

    {

        [Display(Name = "Selected Template Source Excel")]
        public int TemplateMasterID { get; set; }
        public string TemplateName { get; set; }
        public string SourceFile { get; set; }
        public DataTable TemplateTable { get; set; }
    }
}
