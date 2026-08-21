using NPOI.SS.Formula.Functions;
using PFCWebAPP.DatabaseContext;
using PFCWebAPP.DatabaseContext.Models.CustomTables;
using PFCWebAPP.DatabaseContext.Models.ExtractionTables;
using static NPOI.POIFS.Crypt.CryptoFunctions;


namespace PFCWebAPP.Repositories.PriceList
{
    public class PriceListRepository: IPriceListRepository
    {
        #region Private member variables...

        private readonly PFCDBContext _dbContext;

        private GenericRepository<A507> _A507Repository;
        private GenericRepository<A604> _A604Repository;
        private GenericRepository<A606> _A606Repository;
        private GenericRepository<A607> _A607Repository;
        private GenericRepository<A608> _A608Repository;
        private GenericRepository<A609> _A609Repository;
        private GenericRepository<A652> _A652Repository;
        private GenericRepository<A653> _A653Repository;
        private GenericRepository<A655> _A655Repository;
        private GenericRepository<A657> _A657Repository;
        private GenericRepository<A979> _A979Repository;
        private GenericRepository<A996> _A996Repository;
        private GenericRepository<Kna1> _KNA1Repository;
        private GenericRepository<Knvv> _KNVVRepository;
        private GenericRepository<KonmSum> _KONM_sumRepository;
        private GenericRepository<Konp> _KONPRepository;
        private GenericRepository<Makt> _MAKTRepository;
        private GenericRepository<Mara> _MARARepository;
        private GenericRepository<Marc> _MARCRepository;
        private GenericRepository<MarmSum> _MARM_sumRepository;
        private GenericRepository<Mvke> _MVKERepository;
        private GenericRepository<T006a> _T006ARepository;
        private GenericRepository<CustomerHierarchyCust> _CustomerHierarchyCustRepository;
        private GenericRepository<PriceFileHeader> _PriceFileHeaderRepository;
        private GenericRepository<PriceFileDetails> _PriceFileDetailsRepository;
        private GenericRepository<PriceFileLocationDetails> _PriceFileLocationDetailsRepository;
        private GenericRepository<CustomerSettings> _CustomerSettingsRepository;
        private GenericRepository<QueueModel> _QueueModelRepository;
        private GenericRepository<QueueHistory> _QueueHistoryRepository;

        #endregion
        public PriceListRepository(PFCDBContext dbContext) 
        {
            _dbContext = dbContext;
        }



        #region Public Repository Creation properties for Extraction...

        public GenericRepository<A507> A507Repository => _A507Repository ?? (_A507Repository = new GenericRepository<A507>(_dbContext));
        public GenericRepository<A604> A604Repository => _A604Repository ?? (_A604Repository = new GenericRepository<A604>(_dbContext));
        public GenericRepository<A606> A606Repository => _A606Repository ?? (_A606Repository = new GenericRepository<A606>(_dbContext));
        public GenericRepository<A607> A607Repository => _A607Repository ?? (_A607Repository = new GenericRepository<A607>(_dbContext));
        public GenericRepository<A608> A608Repository => _A608Repository ?? (_A608Repository = new GenericRepository<A608>(_dbContext));
        public GenericRepository<A609> A609Repository => _A609Repository ?? (_A609Repository = new GenericRepository<A609>(_dbContext));
        public GenericRepository<A652> A652Repository => _A652Repository ?? (_A652Repository = new GenericRepository<A652>(_dbContext));
        public GenericRepository<A653> A653Repository => _A653Repository ?? (_A653Repository = new GenericRepository<A653>(_dbContext));
        public GenericRepository<A655> A655Repository => _A655Repository ?? (_A655Repository = new GenericRepository<A655>(_dbContext));
        public GenericRepository<A657> A657Repository => _A657Repository ?? (_A657Repository = new GenericRepository<A657>(_dbContext));
        public GenericRepository<A979> A979Repository => _A979Repository ?? (_A979Repository = new GenericRepository<A979>(_dbContext));
        public GenericRepository<A996> A996Repository => _A996Repository ?? (_A996Repository = new GenericRepository<A996>(_dbContext));
        public GenericRepository<Kna1> Kna1Repository => _KNA1Repository ?? (_KNA1Repository = new GenericRepository<Kna1>(_dbContext));
        public GenericRepository<Knvv> KnvvRepository => _KNVVRepository ?? (_KNVVRepository = new GenericRepository<Knvv>(_dbContext));
        public GenericRepository<KonmSum> KonmSumRepository => _KONM_sumRepository ?? (_KONM_sumRepository = new GenericRepository<KonmSum>(_dbContext));
        public GenericRepository<Konp> KonpRepository => _KONPRepository ?? (_KONPRepository = new GenericRepository<Konp>(_dbContext));
        public GenericRepository<Makt> MaktRepository => _MAKTRepository ?? (_MAKTRepository = new GenericRepository<Makt>(_dbContext));
        public GenericRepository<Mara> MaraRepository => _MARARepository ?? (_MARARepository = new GenericRepository<Mara>(_dbContext));
        public GenericRepository<Marc> MarcRepository => _MARCRepository ?? (_MARCRepository = new GenericRepository<Marc>(_dbContext));
        public GenericRepository<MarmSum> MarmSumRepository => _MARM_sumRepository ?? (_MARM_sumRepository = new GenericRepository<MarmSum>(_dbContext));
        public GenericRepository<Mvke> MvkeRepository => _MVKERepository ?? (_MVKERepository = new GenericRepository<Mvke>(_dbContext));
        public GenericRepository<T006a> T006aRepository => _T006ARepository ?? (_T006ARepository = new GenericRepository<T006a>(_dbContext));
        public GenericRepository<CustomerHierarchyCust> CustomerHierarchyCustRepository => _CustomerHierarchyCustRepository ?? (_CustomerHierarchyCustRepository = new GenericRepository<CustomerHierarchyCust>(_dbContext));
        public GenericRepository<PriceFileHeader> PriceFileHeaderRepository => _PriceFileHeaderRepository ?? (_PriceFileHeaderRepository = new GenericRepository<PriceFileHeader>(_dbContext));
        public GenericRepository<PriceFileDetails> PriceFileDetailsRepository => _PriceFileDetailsRepository ?? (_PriceFileDetailsRepository = new GenericRepository<PriceFileDetails>(_dbContext));
        public GenericRepository<PriceFileLocationDetails> PriceFileLocationDetailsRepository => _PriceFileLocationDetailsRepository ?? (_PriceFileLocationDetailsRepository = new GenericRepository<PriceFileLocationDetails>(_dbContext));
        public GenericRepository<CustomerSettings> CustomerSettingsRepository => _CustomerSettingsRepository ?? (_CustomerSettingsRepository = new GenericRepository<CustomerSettings>(_dbContext));
        public GenericRepository<QueueModel> QueueModelRepository => _QueueModelRepository ?? (_QueueModelRepository = new GenericRepository<QueueModel>(_dbContext));
        public GenericRepository<QueueHistory> QueueHistoryRepository => _QueueHistoryRepository ?? (_QueueHistoryRepository = new GenericRepository<QueueHistory>(_dbContext));

        #endregion




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
