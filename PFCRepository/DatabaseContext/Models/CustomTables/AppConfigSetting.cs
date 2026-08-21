using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace PFCRepository.DatabaseContext.Models.CustomTables
{
    [Table("MST_AppConfig")]
    public class AppConfigSetting
    {
        [Key]
        public int AppConfigID { get; set; }

        [Column(TypeName = "varchar(50)")]
        [Required]
        public string ConfigName { get; set; }

        [Column(TypeName = "varchar(50)")]
        [Required]
        public string AliasName { get; set; }

        [Column(TypeName = "varchar(200)")]
        [Required]
        public string Description { get; set; }

        [Required(ErrorMessage = "ConfigValue is required.")]
        [Column(TypeName = "varchar(100)")]
        public string ConfigValue { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string ConfigType { get; set; }

        [Column(TypeName = "varchar(10)")]
        [Required]
        public string ConfigDataType { get; set; }           

        [Column(TypeName = "varchar(50)")]
        [Required]
        public string ConfigUIType { get; set; }

        public int ConfigMinLength { get; set; }

        public int ConfigMaxLength { get; set; }

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
        //public byte[] RowVersion { get; set; }
    }
}
