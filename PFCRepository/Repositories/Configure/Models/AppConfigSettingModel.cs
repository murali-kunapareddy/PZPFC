using PFCRepository.DatabaseContext.Models.CustomTables;
using System.Security.Policy;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PFCRepository.Repositories.Configure.Models
{
    //public class AppConfigSettingModel
    //{
    //    public List<AppConfigSettingVM> lstAppConfigSetting { get; set; }

    //    //public List<ConfigOptions> lstConfigOptions { get; set; }
    //}

    public class AppConfigSettingVM
    {
        public int AppConfigID { get; set; }
        public string ConfigName { get; set; }
        public string AliasName { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "ConfigValue is required.")]
        public string ConfigValue { get; set; }
        public string ConfigType { get; set; }
        public string ConfigDataType { get; set; }
        public string ConfigUIType { get; set; }
        public int ConfigMinLength { get; set; }
        public int ConfigMaxLength { get; set; }
        public int SequenceNo { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<ConfigOptions> configOptions { get; set; }

    }
}
