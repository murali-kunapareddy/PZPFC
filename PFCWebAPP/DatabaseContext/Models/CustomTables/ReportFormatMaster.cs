using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFCWebAPP.DatabaseContext.Models.CustomTables
{
    [Table("MST_ReportFormatMaster")]
    public class ReportFormatMaster
    {
        [Key]
        public int ReportFormatMasterID { get; set; }

        [Column(TypeName = "varchar(50)")]
        [Required]
        public string FormatName { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string AliasName { get; set; }

        [Column(TypeName = "varchar(2)")]
        public string CountryCode { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; } = false;

        [Column(TypeName = "varchar(100)")]
        public string CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        public virtual ICollection<ReportFormatFieldMapping> ReportFormatFieldMapping { get; set; }
    }
}
