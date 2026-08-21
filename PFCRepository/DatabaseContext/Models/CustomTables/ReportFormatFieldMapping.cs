using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFCRepository.DatabaseContext.Models.CustomTables
{
    [Table("MST_ReportFormatFieldMapping")]
    public class ReportFormatFieldMapping
    {
        [Key]
        public int ReportFormatFieldMappingID { get; set; }
        public int? ReportFormatMasterID { get; set; }
        public int? ReportFormatFieldMasterID { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string AliasName { get; set; }
        public int SequenceNo { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; } = false;

        [Column(TypeName = "varchar(100)")]
        public string CreatedBy { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string ModifiedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime ModifiedDate { get; set; }
        public virtual ReportFormatFieldMaster ReportFormatFieldMaster { get; set; }

        public virtual ReportFormatMaster ReportFormatMaster { get; set; }
    }
}
