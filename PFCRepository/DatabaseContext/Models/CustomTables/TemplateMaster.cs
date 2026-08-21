using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PFCRepository.DatabaseContext.Models.CustomTables
{
    [Table("MST_TemplateMaster")]
    public class TemplateMaster
    {
        [Key]
        public int TemplateMasterID { get; set; }
        public int? TemplateCategoryID { get; set; }

        [Column(TypeName = "varchar(50)")]
        [Required]
        public string TemplateName { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string AliasName { get; set; }

        [Column(TypeName = "varchar(5)")]
        public string TemplateDataModel { get; set; }

        [Column(TypeName = "varchar(2)")]
        public string CountryCode { get; set; }
        public bool CanDuplicate { get; set; }
        public bool CanUpload { get; set; }
        public bool CanEdit { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; } = false;
        //public byte[] RowVersion { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string CreatedBy { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string ModifiedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime ModifiedDate { get; set; }
        public virtual TemplateCategory TemplateCategory { get; set; }
        public virtual ICollection<TemplateData> TemplateData { get; set; }
        public virtual ICollection<TemplateStructure> TemplateStructure { get; set; }
    }
}
