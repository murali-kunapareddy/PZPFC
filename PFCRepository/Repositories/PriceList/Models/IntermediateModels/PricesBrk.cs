namespace PFCRepository.Repositories.PriceList.Models.IntermediateModels
{
    public class PricesBrk
    {
        public string CustNo { get; set; }
        public string MATNR { get; set; }
        public string VKORG { get; set; }
        public string VTWEG { get; set; }
        public int KSTBM1 { get; set; }
        public int KSTBM2 { get; set; }
        public int KSTBM3 { get; set; }
        public int KSTBM4 { get; set; }
        public int KSTBM5 { get; set; }
        public int KSTBM6 { get; set; }
        public float KBETR1 { get; set; }
        public float KBETR2 { get; set; }
        public float KBETR3 { get; set; }
        public float KBETR4 { get; set; }
        public float KBETR5 { get; set; }
        public float KBETR6 { get; set; }
        public string PriceTable { get; set; }
        public string CondType { get; set; }
        public DateTime DATBI { get; set; }
        public DateTime DATAB { get; set; }
    }
}
