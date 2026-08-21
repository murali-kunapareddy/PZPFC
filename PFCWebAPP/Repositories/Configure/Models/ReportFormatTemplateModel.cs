using Microsoft.AspNetCore.Mvc.Rendering;
using PFCWebAPP.DatabaseContext.Models.CustomTables;
using System.Data;

namespace PFCWebAPP.Repositories.Configure.Models
{
    public class ReportFormatTemplateModel
    {
        public int SelectedReportFormatTempMasterID { get; set; }
        public IEnumerable<SelectListItem> ReportFormats {get; set; }
        public ReportFormatMaster SelectedTemplateDetails { get; set; }

        public List<ReportFormatDataTableViewModel> ReportFormatsDataTable { get; set; } = new List<ReportFormatDataTableViewModel>();

    }
}
