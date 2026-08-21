using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PFCRepository.DatabaseContext.Models.CustomTables
{
    [Table("MST_TemplateStructure")]
    public class TemplateStructure
    {
        [Key]
        public int TemplateStructureID { get; set; }
        public int? TemplateMasterID { get; set; }

        [Column(TypeName = "varchar(50)")]
        [Required]
        public string PropertyName { get; set; }

        [Column(TypeName = "varchar(250)")]
        public string PropertyDescription { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string PropertyDataType { get; set; }
        public int SequenceNo { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; } = false;
        //public byte[] RowVersion { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        public virtual TemplateMaster TemplateMaster { get; set; }
    }
}
