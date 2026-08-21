using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PFCRepository.DatabaseContext.Models.CustomTables
{
    [Table("MST_TemplateCategory")]
    public class TemplateCategory
    {
        [Key]
        public int TemplateCategoryID { get; set; }

        [Column(TypeName = "varchar(50)")]
        [Required]
        public string CategoryName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; } = false;

        [Column(TypeName = "varchar(100)")]
        public string CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }

        //public byte[] RowVersion { get; set; }
        public virtual ICollection<TemplateMaster> TemplateMaster { get; set; }
    }
}
