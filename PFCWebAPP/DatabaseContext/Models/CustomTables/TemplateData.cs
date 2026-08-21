using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PFCWebAPP.DatabaseContext.Models.CustomTables
{
    [Table("MST_TemplateData")]
    public class TemplateData
    {
        [Key]
        public int TemplateDataID { get; set; }
        public int? TemplateMasterID { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        [Required]
        public string Data { get; set; }
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
        //public byte[] RowVersion { get; set; }
        public virtual TemplateMaster TemplateMaster { get; set; }
    }
}
