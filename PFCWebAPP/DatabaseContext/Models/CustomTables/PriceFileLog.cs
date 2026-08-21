using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFCWebAPP.DatabaseContext.Models.CustomTables
{
    [Table("TRN_PriceFileLog")]
    public class PriceFileLog
    {
        [Key]
        public long PriceFileLogID { get; set; }
        public int PriceFileHeaderID { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string LogType { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string FunctionName { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string LogInformation { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string LogReference1 { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string LogReference2 { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; } = false;

        [Column(TypeName = "varchar(100)")]
        public string CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
    }
}
