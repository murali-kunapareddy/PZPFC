using PFCWebAPP.Repositories.PriceList.Models.IntermediateModels;
using SE.CA.PingComponent.Entities;

namespace PFCWebAPP.Repositories.Common.Models
{
    public class UserDashboardViewModel
    {
        public List<PFCSummaryInfo> objPFCSummaryInfo { get; set; }
        public UserInfo objUserInfo { get; set; }
        public bool IsPFCUser { get; set; }
    }

    public class PFCSummaryInfo
    {
        public long PriceFileHeaderID { get; set; }
        public string PriceFileCreatedDate { get; set; }
        public string SalesOrganization { get; set; }
        public string PricesActiveDate { get; set; }
        public string TradeListTemplate { get; set; }
        public string TradeListFormat { get; set; }
        public string Customers { get; set; }
        public string UserSESA { get; set; }
        public List<SelectedCustomers> lstSelectedCustomers { get; set; }
        public List<SelectedCustomersByHeaderID> lstSelectedCustomersByHeaderID { get; set; }


    }

    public class SelectedCustomersByHeaderID
    {
        public long PriceFileHeaderID { get; set; }
        public string PFCZipFileName { get; set; }
        public int CustomerSNO { get; set; }
        public string CustomerNumber { get; set; }
        public string CustomerName { get; set; }
        public string zKUNNR { get; set; }
        public string PC1 { get; set; }
        public string PC2 { get; set; }
        public string PC3 { get; set; }
    }
}
