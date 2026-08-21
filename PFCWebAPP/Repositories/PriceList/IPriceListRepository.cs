using PFCWebAPP.DatabaseContext.Models.CustomTables;
using PFCWebAPP.DatabaseContext.Models.ExtractionTables;

namespace PFCWebAPP.Repositories.PriceList
{
    public interface IPriceListRepository: IDisposable
    {
        /// <summary>
        /// Get Property for  A507 repository.
        /// </summary>
        GenericRepository<A507> A507Repository { get; }
        /// <summary>
        /// Get Property for  A604 repository.
        /// </summary>
        GenericRepository<A604> A604Repository { get; }
        /// <summary>
        /// Get Property for  A606 repository.
        /// </summary>
        GenericRepository<A606> A606Repository { get; }
        /// <summary>
        /// Get Property for  A607 repository.
        /// </summary>
        GenericRepository<A607> A607Repository { get; }
        /// <summary>
        /// Get  Property for  A608 repository.
        /// </summary>
        GenericRepository<A608> A608Repository { get; }
        /// <summary>
        /// Get  Property for  A609 repository.
        /// </summary>
        GenericRepository<A609> A609Repository { get; }
        /// <summary>
        /// Get  Property for  A652 repository.
        /// </summary>
        GenericRepository<A652> A652Repository { get; }
        /// <summary>
        /// Get  Property for  A653 repository.
        /// </summary>
        GenericRepository<A653> A653Repository { get; }
        /// <summary>
        /// Get  Property for  A655 repository.
        /// </summary>
        GenericRepository<A655> A655Repository { get; }
        /// <summary>
        /// Get  Property for  A657 repository.
        /// </summary>
        GenericRepository<A657> A657Repository { get; }
        /// <summary>
        /// Get  Property for  A979 repository.
        /// </summary>
        GenericRepository<A979> A979Repository { get; }
        /// <summary>
        /// Get  Property for  A996 repository.
        /// </summary>
        GenericRepository<A996> A996Repository { get; }
        /// <summary>
        /// Get  Property for  Kna1 repository.
        /// </summary>
        GenericRepository<Kna1> Kna1Repository { get; }
        /// <summary>
        /// Get  Property for  Knvv repository.
        /// </summary>
        GenericRepository<Knvv> KnvvRepository { get; }
        /// <summary>
        /// Get  Property for  KonmSum repository.
        /// </summary>
        GenericRepository<KonmSum> KonmSumRepository { get; }
        /// <summary>
        /// Get  Property for  Konp repository.
        /// </summary>
        GenericRepository<Konp> KonpRepository { get; }
        /// <summary>
        /// Get  Property for  Makt repository.
        /// </summary>
        GenericRepository<Makt> MaktRepository { get; }
        /// <summary>
        /// Get  Property for  A604 repository.
        /// </summary>
        GenericRepository<Mara> MaraRepository { get; }
        /// <summary>
        /// Get  Property for  Marc repository.
        /// </summary>
        GenericRepository<Marc> MarcRepository { get; }
        /// <summary>
        /// Get  Property for  MarmSum repository.
        /// </summary>
        GenericRepository<MarmSum> MarmSumRepository { get; }
        /// <summary>
        /// Get  Property for  Mvke repository.
        /// </summary>
        GenericRepository<Mvke> MvkeRepository { get; }
        /// <summary>
        /// Get  Property for  T006a repository.
        /// </summary>
        GenericRepository<T006a> T006aRepository { get; }
        /// <summary>
        ///  Get Property for  CustomerHierarchyCust repository.
        /// </summary>
        GenericRepository<CustomerHierarchyCust> CustomerHierarchyCustRepository { get; }

        /// <summary>
        ///  Get Property for  PriceFile Header repository.
        /// </summary>
        GenericRepository<PriceFileHeader> PriceFileHeaderRepository { get; }

        /// <summary>
        ///  Get Property for  PriceFile Details repository.
        /// </summary>
        GenericRepository<PriceFileDetails> PriceFileDetailsRepository { get; }

        /// <summary>
        ///  Get Property for  PriceFile Location Details repository.
        /// </summary>
        GenericRepository<PriceFileLocationDetails> PriceFileLocationDetailsRepository { get; }
        /// <summary>
        ///  Get Property for  Customer Settings Repository.
        /// </summary>
        GenericRepository<CustomerSettings> CustomerSettingsRepository { get; }

        /// <summary>
        ///  Get Property for queue Repository.
        /// </summary>
        GenericRepository<QueueModel> QueueModelRepository { get; }

        /// <summary>
        ///  Get Property for queue history Repository.
        /// </summary>
        GenericRepository<QueueHistory> QueueHistoryRepository { get; }


    }
}
