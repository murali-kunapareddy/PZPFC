namespace PFCWebAPP.Repositories.PriceList.Models
{
    public class PriceFileSaveConfig
    {
        public string UserSESA { get; set; }
        public string SalesOrganization { get; set; }
        public string SelectedCustomers { get; set; }
        public DateTime? PricesActiveDate { get; set; }
        public bool CanUseAutoReportContent { get; set; }
        public long ReportContentTemplateID { get; set; }
        public long ReportFormatTemplateID { get; set; }
        public bool CanIncludeTradePrices { get; set; }
        public bool CanIncludeCustomerNetPrices { get; set; }
        public bool CanIncludeCustomerHierarchyNetPrices { get; set; }
        public bool CanIncludeOverallNetPrices { get; set; }
        public bool CanIncludePriceGroupNets { get; set; }
        public bool CanIncludeSellOffPrices { get; set; }
        public bool CanIncludeDiscount1 { get; set; }
        public bool CanIncludeDiscount2 { get; set; }
        public bool CanIncludeDiscount3 { get; set; }
        public bool CanIncludeDiscount4 { get; set; }
        public bool CanIncludeDiscount5 { get; set; }
        public bool CanIncludeDiscount6 { get; set; }
        public bool CanIncludeDiscount7 { get; set; }
        public bool CanIncludeDiscount8 { get; set; }
        public bool CanIncludePromoPrice { get; set; }
        public bool CanUseShiftBreaks { get; set; }
        public bool CanUseMOQAsBrk1 { get; set; }
        public bool CanUseGlobalCOSForProductHierarchy { get; set; }
        public bool CanUseLocalCOSForProductHierarchy { get; set; }
        public bool CanAddSODInFinalPrice { get; set; }
        public double SODInFinalPriceValue { get; set; }
        public bool CanUseAlternateValidFromDate { get; set; }
        public DateTime? AlternateValidFromDate { get; set; }
        public bool CanShowTemplateMaterialOnly { get; set; }
        public bool SendEmail { get; set; }
        public bool ShowNotFoundTemplateMaterials { get; set;}

        public bool CanIncludeProductHierarchyOverride { get; set; }

    }
}
