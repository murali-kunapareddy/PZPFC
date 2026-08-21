using ExcelUploadValidator.Models;

namespace PFCWebAPP.Repositories.PriceList.Models
{
    public class PriceFileCalucationResponse
    {
        public List<PriceFileCalucationFields> lstPriceFileCalucationFields { get; set; }
        public PriceFileCalucationStatus PriceFileCalucationStatus { get; set; }
        public string ResultMessage { get; set; }
    }
    public enum PriceFileCalucationStatus
    {
        Failed = 0,
        Success = 1


    }


    public class PriceFileCalucationFields

    {
        public string CustNo { get; set; }
        public string Prefix { get; set; }
        public string CustomerCatNo { get; set; }
        public string ColourCode { get; set; }
        public string CustomerItemNo { get; set; }
        public string SchneiderElectricMaterialReference { get; set; }
        public string MaterialDescription { get; set; }
        public float WholesaleListPrice_exclGST { get; set; }
        public float WholesaleListPrice_inclGST { get; set; }
        public float Per { get; set; }
        public string UOM { get; set; }
        public int MOQ_MinimumOrderQuantity { get; set; }
        public float OrderMultiple { get; set; }
        public float RRP_RecommendedRetailPrice { get; set; }
        public float ARRP_AdvertisedRecommendedRetailPrice { get; set; }
        public string PriceDerivedFrom { get; set; }
        public int PriceBreak1_CUSTOMERQTY { get; set; }
        public float PriceBreak1_CUSTOMERDiscount { get; set; }
        public float PriceBreak1_CUSTOMERCost_exclGST { get; set; }
        public float PriceBreak1_CUSTOMERCost_inclGST { get; set; }
        public int PriceBreak2_CUSTOMERQTY { get; set; }
        public float PriceBreak2_CUSTOMERDiscount { get; set; }
        public float PriceBreak2_CUSTOMERCost_exclGST { get; set; }
        public float PriceBreak2_CUSTOMERCost_inclGST { get; set; }
        public int PriceBreak3_CUSTOMERQTY { get; set; }
        public float PriceBreak3_CUSTOMERDiscount { get; set; }
        public float PriceBreak3_CUSTOMERCost_exclGST { get; set; }
        public float PriceBreak3_CUSTOMERCost_inclGST { get; set; }
        public int PriceBreak4_CUSTOMERQTY { get; set; }
        public float PriceBreak4_CUSTOMERDiscount { get; set; }
        public float PriceBreak4_CUSTOMERCost_exclGST { get; set; }
        public float PriceBreak4_CUSTOMERCost_inclGST { get; set; }
        public int PriceBreak5_CUSTOMERQTY { get; set; }
        public float PriceBreak5_CUSTOMERDiscount { get; set; }
        public float PriceBreak5_CUSTOMERCost_exclGST { get; set; }
        public float PriceBreak5_CUSTOMERCost_inclGST { get; set; }
        public string Barcode { get; set; }
        public string SAPLocalCOS { get; set; }
        public string CartonQty { get; set; }
        public string StockStatus { get; set; }
        public DateOnly ValidFrom { get; set; }
        public DateOnly ValidTo { get; set; }
        public string FileReferenceData { get; set; }
        public string Currency { get; set; }
        public string VRG { get; set; }
        public string VRGDescription { get; set; }
        public string MainGroup { get; set; }
        public string MainGroupDescription { get; set; }
        public string Group { get; set; }
        public string GroupDescription { get; set; }
        public string SubGroupDescription { get; set; }
    }
}
