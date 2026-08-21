using PFCRepository.DatabaseContext.Models.CustomTables;

namespace PFCRepository.Repositories.Common
{
    public interface ICommonRepository : IDisposable
    {
        /// <summary>
        /// Get/Set Property for  PriceFileHeader  repository.
        /// </summary>
        GenericRepository<PriceFileHeader> PriceFileHeaderRepository { get; }
        GenericRepository<NotificationTemplates> NotificationTemplatesRepository { get; }
        GenericRepository<NotificationHistory> NotificationHistoryRepository { get; }
        GenericRepository<CustomerContacts> CustomerContactsRepository { get; }

    }
}
