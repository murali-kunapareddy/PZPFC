using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFCWebAPP.DatabaseContext.Models.CustomTables
{
    [Table("MST_NotificationTemplate")]
    public class NotificationTemplates
    {
        [Key]
        public int NotificationTemplateID { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string SalesOrganization { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string TemplateName { get; set; }

        [Column(TypeName = "varchar(1024)")]
        public string TemplateSubject { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string TemplateBody { get; set; }

        [Column(TypeName = "varchar(1000)")]
        public string TemplateVars { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string DefaultSentTo { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string DefaultCcTo { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string DefaultBccTo { get; set; }

        public int Priority { get; set; }

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
    }
}
