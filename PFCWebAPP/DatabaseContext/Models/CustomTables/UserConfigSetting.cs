using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PFCWebAPP.DatabaseContext.Models.CustomTables
{
    [Table("TRN_UserConfigSetting")]
    public class UserConfigSetting
    {
        [Key]
        public long UserConfigSettingID { get; set; }

        [Column(TypeName = "varchar(30)")]
        [Required]
        public string UserSESA { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string SalesOrganization { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string SelectedCustomers { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PricesActiveDate { get; set; }
        public bool CanUseAutoReportContent { get; set; }
        
        public long ReportContentTemplateID { get; set; }
        public long ReportFormatTemplateID { get; set; }
        public long SelectedCustomersTemplateID { get; set; }
        public bool CanIncludeTradePrices { get; set; }
        public bool CanIncludeCustomerNetPrices { get; set; }
        public bool CanIncludeCustomerHierarchyNetPrices { get; set; }
        public bool CanIncludeOverallNetPrices { get; set; }
        public bool CanIncludePriceGroupNets { get; set; }
        public bool CanIncludeSellOffPrices { get; set; }
        public bool CanIncludeDiscount1 { get; set; }

        //[Column(TypeName = "varchar(4)")]
        //public string Discount1 { get; set; }
        public bool CanIncludeDiscount2 { get; set; }

        //[Column(TypeName = "varchar(4)")]
        //public string Discount2 { get; set; }
        public bool CanIncludeDiscount3 { get; set; }

        //[Column(TypeName = "varchar(4)")]
        //public string Discount3 { get; set; }
        public bool CanIncludeDiscount4 { get; set; }

        //[Column(TypeName = "varchar(4)")]
        //public string Discount4 { get; set; }
        public bool CanIncludeDiscount5 { get; set; }

        //[Column(TypeName = "varchar(4)")]
        //public string Discount5 { get; set; }
        public bool CanIncludeDiscount6 { get; set; }

        //[Column(TypeName = "varchar(4)")]
        //public string Discount6 { get; set; }
        public bool CanIncludeDiscount7 { get; set; }

        //[Column(TypeName = "varchar(4)")]
        //public string Discount7 { get; set; }
        public bool CanIncludeDiscount8 { get; set; }

        //[Column(TypeName = "varchar(4)")]
        //public string Discount8 { get; set; }
        public bool CanIncludePromoPrice { get; set; }
        public bool CanUseShiftBreaks { get; set; }
        public bool CanUseMOQAsBrk1 { get; set; }
        public bool CanUseGlobalCOSForProductHierarchy { get; set; }
        public bool CanUseLocalCOSForProductHierarchy { get; set; }
        public bool CanAddSODInFinalPrice { get; set; }
        public double SODInFinalPriceValue { get; set; }
        public bool CanUseAlternateValidFromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? AlternateValidFromDate { get; set; }
        public bool CanShowTemplateMaterialOnly { get; set; }
        public bool CanSendEmail { get; set; }
        public bool CanShowNotFoundTemplateMaterials { get; set; }

        public bool CanIncludeProductHierarchyOverride { get; set; }
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
