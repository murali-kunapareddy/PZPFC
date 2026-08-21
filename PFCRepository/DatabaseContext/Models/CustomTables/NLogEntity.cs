using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFCRepository.DatabaseContext.Models.CustomTables
{
    [Table("TRN_NLog")]
    public class NLogEntity
    {
        [Key]
        public long NLogID { get; set; }

        [Column(TypeName = "varchar(50)")]
        [StringLength(50)]
        public string MachineName { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime Logged { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "varchar(5)")]
        [StringLength(5)]
        public string Level { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string Message { get; set; }

        [Column(TypeName = "varchar(300)")]
        [StringLength(300)]
        public string Logger { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string Properties { get; set; }

        [Column(TypeName = "nvarchar(300)")]
        public string Callsite { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string Exception { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string StackTrace { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string ThreadID { get; set; }
    }
}
