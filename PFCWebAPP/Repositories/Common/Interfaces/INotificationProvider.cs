using PFCWebAPP.DatabaseContext.Models.CustomTables;

namespace PFCWebAPP.Repositories.Common.Interfaces
{
    public interface INotificationProvider : IDisposable
    {
        Task<bool> SendMailByPriceFileID(long PriceFileHeaderID);
        Task<bool> SendMailByPriceFileLocationDetails(List<PriceFileLocationDetails> lstPFCAttachments);

        IQueryable<NotificationHistory> GetNotificationHistory();
        Task<bool> ReSendEmailNotification(long notificationId);
        Task<List<long>> SendMailToPriceFileLocationCustomers(long PriceFileHeaderID, string CustomersWithPriceHeaderId);
        bool GetMailStatusInNotificationHistory(List<long> ids);

        Task<List<long>> SendNotificationForMissingCustomerSettings(string CustomerNo, string CustomerName, string SalesOrg);
        Task<List<long>> SendNotificationForUnAuthorized(string SalesOrg, string Createdby);
    }
}
