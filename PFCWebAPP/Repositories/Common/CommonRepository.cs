using PFCWebAPP.DatabaseContext;
using PFCWebAPP.DatabaseContext.Models.CustomTables;

namespace PFCWebAPP.Repositories.Common
{
    public class CommonRepository : ICommonRepository
    {
        #region Private member variables...

        private readonly PFCDBContext _dbContext;
        private GenericRepository<PriceFileHeader> _PriceFileHeaderRepository;
        private GenericRepository<NotificationTemplates> _NotificationTemplatesRepository;
        private GenericRepository<NotificationHistory> _NotificationHistoryRepository;
        private GenericRepository<CustomerContacts> _CustomerContactsRepository;

        #endregion


        /// <summary>
        /// CommonRepository
        /// </summary>
        /// <param name="dbContext"></param>
        public CommonRepository(PFCDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GenericRepository<NotificationTemplates> NotificationTemplatesRepository => _NotificationTemplatesRepository ?? (_NotificationTemplatesRepository = new GenericRepository<NotificationTemplates>(_dbContext));
        public GenericRepository<NotificationHistory> NotificationHistoryRepository => _NotificationHistoryRepository ?? (_NotificationHistoryRepository = new GenericRepository<NotificationHistory>(_dbContext));
        public GenericRepository<PriceFileHeader> PriceFileHeaderRepository => _PriceFileHeaderRepository ?? (_PriceFileHeaderRepository = new GenericRepository<PriceFileHeader>(_dbContext));
        public GenericRepository<CustomerContacts> CustomerContactsRepository => _CustomerContactsRepository ?? (_CustomerContactsRepository = new GenericRepository<CustomerContacts>(_dbContext));

        #region Implementing IDiosposable...

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
