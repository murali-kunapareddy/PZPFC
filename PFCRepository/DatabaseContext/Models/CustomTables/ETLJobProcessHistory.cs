using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFCRepository.DatabaseContext.Models.CustomTables
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
