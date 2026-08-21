namespace PFCWebAPP.Repositories.PriceList.Models
{
    public class ReportFormatCustom
    {
        public int ReportFormatMasterId { get; set; }
        public string ReportFormatName { get; set; }
        public string ReportFormatAliasName { get; set; }
        public int? FormatFieldMasterID { get; set; }
        public string ReportFieldAliasName { get; set; }
        public int ReportFieldSequence { get; set; }
        public string ReportFieldMasterFieldName { get; set; }
        public string ReportFieldMasterDescription { get; set; }
        public string ReportFormatFieldMasterDataType { get; set; }
    }
}
