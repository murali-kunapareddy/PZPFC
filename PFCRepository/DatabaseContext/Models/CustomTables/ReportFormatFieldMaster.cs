using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PFCRepository.DatabaseContext.Models.CustomTables
{
    [Table("MST_ReportFormatFieldMaster")]
    public class ReportFormatFieldMaster
    {
        [Key]
        public int ReportFormatFieldMasterID { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Required]
        public string FieldName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string FieldDescription { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string DataType { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string AlignmentType { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string ColorCode { get; set; }

        public int SequenceNo { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; } = false;

        [Column(TypeName = "varchar(100)")]
        public string CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        public virtual ICollection<ReportFormatFieldMapping> ReportFormatFieldMapping { get; set; }
    }
}
