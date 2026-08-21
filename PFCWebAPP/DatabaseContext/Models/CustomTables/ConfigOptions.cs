using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFCWebAPP.DatabaseContext.Models.CustomTables
{
    [Table("MST_ConfigOptions")]
    public class ConfigOptions
    {
        [Key]
        public long ConfigOptionID { get; set; }

        [Column(TypeName = "varchar(50)")]
        [Required]
        public string ConfigType { get; set; }

        [Column(TypeName = "varchar(100)")]
        [Required]
        public string ConfigValue { get; set; }

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
    }
}
