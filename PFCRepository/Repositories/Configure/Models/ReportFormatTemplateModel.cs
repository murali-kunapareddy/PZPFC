using PFCRepository.DatabaseContext.Models.CustomTables;
using System.Data;

namespace PFCRepository.Repositories.Configure.Models
{
    public class ReportFormatTemplateModel
    {
        public int SelectedReportFormatTempMasterID { get; set; }
        
        public ReportFormatMaster SelectedTemplateDetails { get; set; }

        public List<ReportFormatDataTableViewModel> ReportFormatsDataTable { get; set; } = new List<ReportFormatDataTableViewModel>();

    }
}
