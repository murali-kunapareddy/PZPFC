using PFCRepository.Repositories.Common;
using PFCRepository.Repositories.Common.Interfaces;
using PFCRepository.Repositories.Common.ServiceProviders;
using PFCRepository.Repositories.Configure;
using PFCRepository.Repositories.Configure.Interfaces;
using PFCRepository.Repositories.PriceList;
using PFCRepository.Repositories.PriceList.ServiceProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceFileGenerator
{
    internal class CustomerFile
    {
        private readonly ILoggingProvider _objLoggingProvider;
        private readonly IPriceListRepository _objPriceListRepository;
        public readonly ICommonProvider _objCommonProvider;
        private readonly IConfigureRepository _objConfigureRepository;
        private readonly INotificationProvider _objNotificationProvider;
        private readonly IMailSenderProvider _objMailSenderProvider;
        private readonly IConfigureProvider _objConfigureProvider;
        private readonly ICommonRepository _objCommonRepository;

        public CustomerFile(ILoggingProvider objLoggingProvider, IPriceListRepository objPriceListRepository, IConfigureRepository objConfigureRepository, ICommonProvider objCommonProvider, INotificationProvider objNotificationProvider, IMailSenderProvider objMailSenderProvider,
            IConfigureProvider objConfigureProvider, ICommonRepository objCommonRepository)
        {
            _objLoggingProvider = objLoggingProvider;
            _objPriceListRepository = objPriceListRepository;
            _objCommonProvider = objCommonProvider;
            _objConfigureRepository = objConfigureRepository;
            _objNotificationProvider = objNotificationProvider;
            _objMailSenderProvider = objMailSenderProvider;
            _objConfigureProvider = objConfigureProvider;
            _objCommonRepository = objCommonRepository;

        }
        public PFCRepository.DatabaseContext.Models.CustomTables.CustomerSettings getCustomer(string customerId, string SalesOrg)
        {
            PriceListProvider _priceListProvider = new PriceListProvider(_objLoggingProvider, _objPriceListRepository, _objConfigureRepository, _objCommonProvider, _objNotificationProvider);
            NotificationProvider _notificationProvider = new NotificationProvider(_objLoggingProvider, _objMailSenderProvider, _objPriceListRepository, _objCommonProvider, _objConfigureRepository, _objConfigureProvider,_objCommonRepository);
            var customerSettings = _priceListProvider.GetCustomerSettings(customerId, SalesOrg);

            return customerSettings;

        }
    }
}
