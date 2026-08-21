using System.ComponentModel.DataAnnotations.Schema;

namespace PFCWebAPP.DatabaseContext.Models.CustomTables
{
    [Table("ETL_JobProcessHistory")]
    public class ETLJobProcessHistory
    {
        public int ETLJobProcessHistoryId { get; set; }
        public DateTime ETLDate { get; set; }
        public DateTime ETLStartDateTime { get; set; }
        public DateTime ETLEndDateTime { get; set; }
        public string ETLStatus { get; set; }
        public string ETLMessage { get; set; }
        public string TimeZone { get; set; }
    }
}
