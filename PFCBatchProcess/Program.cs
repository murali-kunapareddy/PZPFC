using Microsoft.EntityFrameworkCore;
using PFCRepository.DatabaseContext;
using PFCRepository.DatabaseContext.Models.CustomTables;
using Newtonsoft.Json;
using PFCRepository.Repositories.Common;
using PFCRepository.Repositories.Common.Enums;
using PFCRepository.Repositories.Common.Interfaces;
using PFCRepository.Repositories.Common.ServiceProviders;
using PFCRepository.Repositories.Configure;
using PFCRepository.Repositories.Configure.Interfaces;
using PFCRepository.Repositories.Configure.ServiceProviders;
using PFCRepository.Repositories.PriceList;
using PFCRepository.Repositories.PriceList.Interfaces;
using PFCRepository.Repositories.PriceList.ServiceProviders;
using PFCRepository.Repositories.PriceList.Models;
using PFCRepository.Utilities;
using System.Reflection;
using NuGet.Protocol;
using NPOI.SS.UserModel;
using static Org.BouncyCastle.Math.EC.ECCurve;
using PFCRepository.Repositories.Common.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using NuGet.Protocol.Core.Types;

namespace PriceFileGenerator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PFCDBContext>();
            optionsBuilder.UseSqlServer(AppConfig.ConnectionString);

            PFCDBContext dbContext = new PFCDBContext(optionsBuilder.Options);
            ILoggingProvider loggingProvider = new LoggingProvider();
            IPriceListRepository priceListRepository = new PriceListRepository(dbContext);
            IConfigureRepository configureRepository = new ConfigureRepository(dbContext);
            ICommonRepository commonRepository = new CommonRepository(dbContext);
            ICommonProvider commonProvider = new CommonProvider(loggingProvider, commonRepository, configureRepository, priceListRepository);
            IMailSenderProvider mailSenderProvider = new MailSenderProvider(loggingProvider, commonRepository, commonProvider);
            IConfigureProvider configureProvider = new ConfigureProvider(loggingProvider, configureRepository, commonProvider, priceListRepository);
            INotificationProvider notificationProvider = new NotificationProvider(loggingProvider, mailSenderProvider, priceListRepository, commonProvider, configureRepository, configureProvider, commonRepository);

            PriceListProvider _priceListProvider = new PriceListProvider(loggingProvider, priceListRepository, configureRepository, commonProvider, notificationProvider);
            loggingProvider.LogMessage(LogType.Info, "START: Programm");

            int WebInProgressCount;
            var startOfToday = DateTime.UtcNow.Date;

            //To check if ETL job is in Progress 
            var EtlStartEndDateTimeObj = configureRepository.ETLJobProcessHistoryRepository.GetManyQueryable()
                   .Where(j => j.ETLStartDateTime.Date <= startOfToday
                   && j.ETLEndDateTime.Date >= startOfToday)
                   .OrderBy(j => j.ETLJobProcessHistoryId)
                   .FirstOrDefault();

            TimeZoneInfo aestZone = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time"); //considering aest timezone for now(temporary).
            DateTime currentSystemTime = DateTime.Now;
            DateTime currentAESTTime = TimeZoneInfo.ConvertTime(currentSystemTime, TimeZoneInfo.Local, aestZone);

            bool isEtlInProgress = currentAESTTime >= EtlStartEndDateTimeObj.ETLStartDateTime && currentAESTTime <= EtlStartEndDateTimeObj.ETLEndDateTime;

            if (isEtlInProgress)
            {
                loggingProvider.LogMessage(LogType.Warn, $"Scheduler Skipped : ETL Job is in progress");
                return;
            }

            WebInProgressCount = priceListRepository.PriceFileHeaderRepository.GetManyQueryable()
            .Where(t => t.CreatedBy.StartsWith("SESA")
            && t.Status == "In-Progress"
            && t.PercentCompleted < 100
            && t.CreatedDate > DateTime.UtcNow.AddMinutes(-15))
            .Count();

            if (WebInProgressCount == 0)
            {
                WebInProgressCount = priceListRepository.PriceFileLocationDetailsRepository.GetManyQueryable()
                    .Where(t => t.CreatedBy.StartsWith("SESA")
                    && t.Status == "In-Progress"
                    && t.PercentCompleted < 100
                    && t.CreatedDate > DateTime.UtcNow.AddMinutes(-15))
                    .Count();
            }

            if (WebInProgressCount > 0)
            {
                loggingProvider.LogMessage(LogType.Info, $"Web In Progress Requests Count is {WebInProgressCount}");
                return;
            }

            loggingProvider.LogMessage(LogType.Info, "START: Reading from Queue Model");
            var Queue = _priceListProvider.GetQueueModel();

            if (Queue == null)
            {
                loggingProvider.LogMessage(LogType.Info, "END: Queue Model returns Null");
                return;
            }
            loggingProvider.LogMessage(LogType.Info, "END: Reading from Queue Model");
            DateTime parsedFutureDate;
            PFCApiRequestDetails.CreatedBy = $"{Queue.CreatedBy}{Queue.QueueId}";
            DateTime.TryParse(commonProvider.GetAppSettingByName(Constants.PricingFutureDate), out parsedFutureDate);
            int RateLimitToday = Convert.ToInt32(commonProvider.GetAppSettingByName(Constants.ApiRequestLimitPerDay));
            int RateLimitMonth = Convert.ToInt32(commonProvider.GetAppSettingByName(Constants.ApiRequestLimitPerMonth));

            if (parsedFutureDate.Date <= DateTime.UtcNow.Date)
            {
                Queue.PriceStatus = "current";
            }


            List<(string PriceStatus, DateTime PricesActiveDate, bool SendEmail)> QueuePriceStatusList = new List<(string PriceStatus, DateTime PricesActiveDate, bool SendEmail)>();
            string QueueMessage, QueueStatus;

           
            var startOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

            var RequestCount = priceListRepository.QueueHistoryRepository
                .GetManyQueryable()
                .Where(o => o.CustomerEmail.ToLower().Trim() == Queue.CustomerEmail.ToLower().Trim() && 
                o.CustomerId.ToLower().Trim() == Queue.CustomerId.ToLower().Trim() && 
                o.SalesOrganization.ToLower().Trim() == Queue.SalesOrganization.ToLower().Trim() &&
                o.QueueStatus.ToLower().Trim() == "success")
                .GroupBy(o => 1)
                .Select(g => new
                {
                    CountToday = g.Where(x => x.CreatedDate >= startOfToday && x.CreatedDate < startOfToday.AddDays(1)).Select(x => x.CreatedBy).Distinct().Count(),
                    CountThisMonth = g.Where(x => x.CreatedDate >= startOfMonth && x.CreatedDate < startOfMonth.AddMonths(1)).Select(x => x.CreatedBy).Distinct().Count()
                }).FirstOrDefault() ?? new { CountToday = 0, CountThisMonth = 0 };



            if (RequestCount.CountToday >= RateLimitToday)
            {
                QueueMessage = "Request limit exceeded for the Day " + Queue.CustomerEmail;
                QueueStatus = "RATE LIMIT EXCEEDED";
                loggingProvider.LogMessage(LogType.Info, QueueMessage);
                notificationProvider.SendNotificationForMissingCustomerSettings(Queue.CustomerId, "", AppConfig.DailyLimitExceeded, Queue.CustomerEmail);
                var queueHistory = PrepareQueueHistory(Queue, QueueStatus, QueueMessage);
                _priceListProvider.SaveQueueHistory(queueHistory);
                priceListRepository.QueueModelRepository.DeleteEntity(Queue);
                return;
            }
            else if (RequestCount.CountThisMonth >= RateLimitMonth)
            {
                QueueMessage = "Request limit exceeded for this Month " + Queue.CustomerEmail;
                QueueStatus = "RATE LIMIT EXCEEDED";
                loggingProvider.LogMessage(LogType.Info, QueueMessage);
                notificationProvider.SendNotificationForMissingCustomerSettings(Queue.CustomerId, "", AppConfig.MonthlyLimitExceeded, Queue.CustomerEmail);
                var queueHistory = PrepareQueueHistory(Queue, QueueStatus, QueueMessage);
                _priceListProvider.SaveQueueHistory(queueHistory);
                priceListRepository.QueueModelRepository.DeleteEntity(Queue);
                return;
            }


            switch (Queue.PriceStatus.ToLower())
            {
                case "all":
                    QueuePriceStatusList.Add(CreateQueuePriceStatus("current", DateTime.UtcNow, false));
                    QueuePriceStatusList.Add(CreateQueuePriceStatus("pending", parsedFutureDate.Date, false));
                    break;
                case "pending":
                    QueuePriceStatusList.Add(CreateQueuePriceStatus("pending", parsedFutureDate.Date, true));
                    break;
                case "current":
                    QueuePriceStatusList.Add(CreateQueuePriceStatus("current", DateTime.UtcNow, true));
                    break;
            }



            var CustomerSetting = _priceListProvider.GetCustomerSettings(Queue.CustomerId, Queue.SalesOrganization);
            if (CustomerSetting == null)
            {
                QueueMessage = "Customer Settings Not Configured for " + Queue.CustomerId;
                QueueStatus = "INVALID";
                loggingProvider.LogMessage(LogType.Info, QueueMessage);
                notificationProvider.SendNotificationForMissingCustomerSettings(Queue.CustomerId, "", AppConfig.MissingCustomerSettings, Queue.CustomerEmail);
                var queueHistory = PrepareQueueHistory(Queue, QueueStatus, QueueMessage);
                _priceListProvider.SaveQueueHistory(queueHistory);
                priceListRepository.QueueModelRepository.DeleteEntity(Queue);
                return;
            }

            loggingProvider.LogMessage(LogType.Info, "GetCustomerByCountry : Start");
            var Customer = _priceListProvider
                .GetCustomerByCountry(Queue.SalesOrganization)
                .Find(o => o.KUNNR.ToLower().Trim() == Queue.CustomerId.TrimStart('0').ToLower().Trim());
            loggingProvider.LogMessage(LogType.Info, "GetCustomerByCountry : End");

            if (Customer == null)
            {
                QueueStatus = "INVALID";
                QueueMessage = "Customer " + Queue.CustomerId + " is Not Available in PZ PFC";
                loggingProvider.LogMessage(LogType.Info, QueueMessage);
                notificationProvider.SendNotificationForMissingCustomerSettings(Queue.CustomerId, "", AppConfig.MissingCustomer, Queue.CustomerEmail);
                var queueHistory = PrepareQueueHistory(Queue, QueueStatus, QueueMessage);
                _priceListProvider.SaveQueueHistory(queueHistory);
                priceListRepository.QueueModelRepository.DeleteEntity(Queue);
                return;
            }
            var CustomerDetails = new
            {
                CustomerSNO = 1,
                CustomerNumber = Customer.KUNNR,
                CustomerName = Customer.NAME1,
                zKUNNR = Customer.xVKORG,
                PC1 = Customer.KVGR1,
                PC2 = Customer.KVGR2,
                PC3 = Customer.KVGR3,
                Customer.Organization
            };
            List<object> CustomerDetailsObj = new List<object>();
            CustomerDetailsObj.Add(CustomerDetails);

            // Get Trade Trade List Format based on Country and PC1 (KVGR1)
            var TradeListFormat = _priceListProvider.GetTradeFormatListByCustomerCountry(Customer.VKORG.Substring(0, 2), Customer.KVGR1);

            DateTime productHirearchyOverrideDate;
            DateTime.TryParse(commonProvider.GetAppSettingByName(Constants.ProductHirearchyOverrideDate), out productHirearchyOverrideDate);
            PriceFileSaveConfig priceFileSaveConfig = new()
            {
                UserSESA = PFCApiRequestDetails.CreatedBy,
                SalesOrganization = Queue.SalesOrganization,
                SelectedCustomers = CustomerDetailsObj.ToJson(),
                PricesActiveDate = DateTime.UtcNow,
                CanUseAutoReportContent = CustomerSetting.CanUseAutoReportContent,
                ReportContentTemplateID = CustomerSetting.ReportContentTemplateID,
                ReportFormatTemplateID = TradeListFormat,
                CanIncludeTradePrices = CustomerSetting.CanIncludeTradePrices,
                CanIncludeCustomerNetPrices = CustomerSetting.CanIncludeCustomerNetPrices,
                CanIncludeCustomerHierarchyNetPrices = CustomerSetting.CanIncludeCustomerHierarchyNetPrices,
                CanIncludeOverallNetPrices = CustomerSetting.CanIncludeOverallNetPrices,
                CanIncludePriceGroupNets = CustomerSetting.CanIncludePriceGroupNets,
                CanIncludeSellOffPrices = CustomerSetting.CanIncludeSellOffPrices,
                CanIncludeDiscount1 = CustomerSetting.CanIncludeDiscount1,
                CanIncludeDiscount2 = CustomerSetting.CanIncludeDiscount2,
                CanIncludeDiscount3 = CustomerSetting.CanIncludeDiscount3,
                CanIncludeDiscount4 = CustomerSetting.CanIncludeDiscount4,
                CanIncludeDiscount5 = CustomerSetting.CanIncludeDiscount5,
                CanIncludeDiscount6 = CustomerSetting.CanIncludeDiscount6,
                CanIncludeDiscount7 = CustomerSetting.CanIncludeDiscount7,
                CanIncludeDiscount8 = CustomerSetting.CanIncludeDiscount8,
                CanIncludePromoPrice = CustomerSetting.CanIncludePromoPrice,
                CanUseShiftBreaks = CustomerSetting.CanUseShiftBreaks,
                CanUseMOQAsBrk1 = CustomerSetting.CanUseMOQAsBrk1,
                CanUseGlobalCOSForProductHierarchy = CustomerSetting.CanUseGlobalCOSForProductHierarchy,
                CanUseLocalCOSForProductHierarchy = CustomerSetting.CanUseLocalCOSForProductHierarchy,
                CanAddSODInFinalPrice = CustomerSetting.CanAddSODInFinalPrice,
                SODInFinalPriceValue = CustomerSetting.SODInFinalPriceValue,
                CanUseAlternateValidFromDate = CustomerSetting.CanUseAlternateValidFromDate,
                AlternateValidFromDate = CustomerSetting.AlternateValidFromDate,
                CanShowTemplateMaterialOnly = CustomerSetting.CanShowTemplateMaterialOnly,
                SendEmail = CustomerSetting.CanSendEmail,
                ShowNotFoundTemplateMaterials = CustomerSetting.CanShowNotFoundTemplateMaterials,
                CanIncludeProductHierarchyOverride = CustomerSetting.CanIncludeProductHierarchyOverride,
    };

            foreach (var item in QueuePriceStatusList)
            {
                priceFileSaveConfig.PricesActiveDate = item.PricesActiveDate;
                priceFileSaveConfig.SendEmail = item.SendEmail;

                if (CustomerSetting.CanIncludeProductHierarchyOverride)
                {
                    if (priceFileSaveConfig.PricesActiveDate < productHirearchyOverrideDate)
                    {
                        priceFileSaveConfig.CanIncludeProductHierarchyOverride = !CustomerSetting.CanIncludeProductHierarchyOverride;
                    }
                    else
                    {
                        priceFileSaveConfig.CanIncludeProductHierarchyOverride = CustomerSetting.CanIncludeProductHierarchyOverride;
                    }
                }
                else
                {
                    priceFileSaveConfig.CanIncludeProductHierarchyOverride = CustomerSetting.CanIncludeProductHierarchyOverride;
                }
                loggingProvider.LogMessage(LogType.Info, "UserPriceFileSaveConfig : Start");
                var configid = _priceListProvider.UserPriceFileSaveConfig(priceFileSaveConfig);
                loggingProvider.LogMessage(LogType.Info, "UserPriceFileSaveConfig : End");

                QueueStatus = "IN PROGRESS";
                QueueMessage = "Excel File Generation is InProgress";

                var queueHistory = PrepareQueueHistory(Queue, QueueStatus, QueueMessage, configid);
                _priceListProvider.SaveQueueHistory(queueHistory);

                loggingProvider.LogMessage(LogType.Info, "GetExcelDetails : Start");
                var DataResult = _priceListProvider.GetExcelDetails(configid, priceFileSaveConfig.SendEmail, priceFileSaveConfig.ShowNotFoundTemplateMaterials);
                if(DataResult == "DATA_NOT_FOUND" || DataResult == "UNSUCCESSFUL")
                {                    
                    QueueMessage = "Customer " + Queue.CustomerId + " Data is Not Available for Excel Generation";                    
                    loggingProvider.LogMessage(LogType.Error, QueueMessage);
                    queueHistory.ModifiedDate = DateTime.UtcNow;
                    queueHistory.IsActive = false;
                    queueHistory.IsDeleted = true;
                    queueHistory.ModifiedBy = queueHistory.CreatedBy;
                    queueHistory.QueueStatus = DataResult;
                    queueHistory.QueueMessage = QueueMessage;
                    priceListRepository.QueueHistoryRepository.UpdateEntity(queueHistory);
                    loggingProvider.LogMessage(LogType.Info, "QueueHistory Table Info Updated Status : " + DataResult);
                    notificationProvider.SendNotificationForMissingCustomerSettings(Queue.CustomerId, "", AppConfig.MissingExcelData, Queue.CustomerEmail);
                    var ReInsertQueue = PrepareQueue(Queue);
                    loggingProvider.LogMessage(LogType.Info, "Queue Table Data Delete : Start");
                    priceListRepository.QueueModelRepository.DeleteEntity(Queue);
                    loggingProvider.LogMessage(LogType.Info, "Queue Table Data Delete : End");
                    loggingProvider.LogMessage(LogType.Info, "Queue Table Data Insert : Start");
                    priceListRepository.QueueModelRepository.InsertEntity(ReInsertQueue);
                    loggingProvider.LogMessage(LogType.Info, "Queue Table Data Insert : End");
                    return;
                }
                loggingProvider.LogMessage(LogType.Info, "GetExcelDetails : End");


                if (Queue.PriceStatus.ToLower().ToString() == "current" || Queue.PriceStatus.ToLower().ToString() == "pending")
                {
                    var pfhDetails = priceListRepository.PriceFileHeaderRepository.GetFirst(O => O.UserConfigSettingID == configid);
                    var pflNotificationDetails = commonRepository.NotificationHistoryRepository.GetFirst(O => O.PriceFileHeaderID == pfhDetails.PriceFileHeaderID);

                    if (pflNotificationDetails.Status.ToLower() == "success")
                    {
                        queueHistory.ModifiedDate = DateTime.UtcNow;
                        queueHistory.IsActive = false;
                        queueHistory.IsDeleted = true;
                        queueHistory.ModifiedBy = queueHistory.CreatedBy;
                        queueHistory.QueueStatus = pflNotificationDetails.Status;
                        queueHistory.QueueMessage = "Mail Sent Successfully";
                        priceListRepository.QueueHistoryRepository.UpdateEntity(queueHistory);
                        loggingProvider.LogMessage(LogType.Info, "SUCCESS: QueueHistoryRepository Update For : " + queueHistory.CreatedBy + " - " + queueHistory.CustomerId + " - " + queueHistory.UserConfigId);
                    }
                    else
                    {
                        queueHistory.ModifiedDate = DateTime.UtcNow;
                        queueHistory.QueueStatus = pflNotificationDetails.Status;
                        queueHistory.QueueMessage = "Unknown Error in Sending Mail";
                        priceListRepository.QueueHistoryRepository.UpdateEntity(queueHistory);
                        loggingProvider.LogMessage(LogType.Info, "ERROR: QueueHistoryRepository Update For : " + queueHistory.CreatedBy + " - " + queueHistory.CustomerId + " - " + queueHistory.UserConfigId);
                    }
                }
            }

            if (Queue.PriceStatus.ToLower() == "all")
            {
                notificationProvider.SendEmailWithMultipleAttachments(PFCApiRequestDetails.CreatedBy, CustomerDetails.CustomerName);

                var QueueHistory = priceListRepository.QueueHistoryRepository.GetManyQueryable(o => o.CreatedBy == PFCApiRequestDetails.CreatedBy);
                var LastQueueNotificationDetail = commonRepository.NotificationHistoryRepository.GetFirst(O => O.CreatedBy == PFCApiRequestDetails.CreatedBy && O.Status != "");
                var FirstQueueNotificationDetails = commonRepository.NotificationHistoryRepository.GetFirst(O => O.CreatedBy == PFCApiRequestDetails.CreatedBy && O.Status == "");

                FirstQueueNotificationDetails.Status = LastQueueNotificationDetail.Status;
                FirstQueueNotificationDetails.ModifiedBy = PFCApiRequestDetails.CreatedBy;
                FirstQueueNotificationDetails.ModifiedDate = DateTime.UtcNow;
                commonRepository.NotificationHistoryRepository.UpdateEntity(FirstQueueNotificationDetails);
                loggingProvider.LogMessage(LogType.Info, "NotificationHistoryRepository Update For :" + PFCApiRequestDetails.CreatedBy);
                foreach (var item in QueueHistory)
                {
                    List<SqlParameter> lstParameters = new List<SqlParameter>();

                    using (ISqlHelper objSqlHelper = new SqlHelper(AppConfig.ConnectionString))
                    {
                        string strUpdateQuery = "Update dbo.TRN_Queue_History ";
                        if (LastQueueNotificationDetail.Status.ToLower() == "success")
                        {
                            strUpdateQuery += "SET IsActive = @IsActive, IsDeleted = @IsDeleted, QueueStatus = @QueueStatus, QueueMessage = @QueueMessage,ModifiedBy = @ModifiedBy, ModifiedDate = @ModifiedDate WHERE UserConfigId = @UserConfigId";
                            lstParameters.Add(new SqlParameter() { ParameterName = "@IsActive", Value = false });
                            lstParameters.Add(new SqlParameter() { ParameterName = "@IsDeleted", Value = true });
                            lstParameters.Add(new SqlParameter() { ParameterName = "@QueueMessage", Value = "Mail Sent Successfully" });
                        }
                        else
                        {
                            strUpdateQuery += "SET QueueStatus = @QueueStatus, QueueMessage = @QueueMessage,ModifiedBy = @ModifiedBy, ModifiedDate = @ModifiedDate WHERE UserConfigId = @UserConfigId";
                            item.QueueMessage = "Unknown Error in Sending Mail";
                        }
                        lstParameters.Add(new SqlParameter() { ParameterName = "@QueueStatus", Value = LastQueueNotificationDetail.Status });
                        lstParameters.Add(new SqlParameter() { ParameterName = "@ModifiedBy", Value = PFCApiRequestDetails.CreatedBy });
                        lstParameters.Add(new SqlParameter() { ParameterName = "@ModifiedDate", Value = DateTime.UtcNow });
                        lstParameters.Add(new SqlParameter() { ParameterName = "@UserConfigId", Value = item.UserConfigId });

                        objSqlHelper.ExecuteNonQuery(CommandType.Text, strUpdateQuery, lstParameters.ToArray());
                    }
                }
            }


            priceListRepository.QueueModelRepository.DeleteEntity(Queue);
            loggingProvider.LogMessage(LogType.Info, "END: Programm");

            //} while (StartOfDay == DateTime.UtcNow.Date);
        }
        public static (string PriceStatus, DateTime PricesActiveDate, bool SendEmail) CreateQueuePriceStatus(string status, DateTime date, bool SendEmail)
        {
            return (status, date, SendEmail);
        }

        public static QueueModel PrepareQueue(QueueModel Queue)
        {            
            QueueModel queueModel = new()
            {                
                SalesOrganization   = Queue.SalesOrganization,
                Distributionchannel = Queue.Distributionchannel,
                CustomerId          = Queue.CustomerId,
                PricingFiletype     = Queue.PricingFiletype,
                PriceStatus         = Queue.PriceStatus,
                PricingDate         = Queue.PricingDate,
                CustomerEmail       = Queue.CustomerEmail,
                IsActive            = true,
                CreatedBy           = Queue.CreatedBy,
                CreatedDate         = DateTime.UtcNow
            };
            return queueModel;
        }

        public static QueueHistory PrepareQueueHistory(QueueModel Queue, string QueueStatus, string QueueMessage, long congigId = 0)
        {
            QueueHistory queueHistory = new()
            {
                SalesOrganization = Queue.SalesOrganization,
                UserConfigId = congigId,
                Distributionchannel = Queue.Distributionchannel,
                CustomerId = Queue.CustomerId,
                PricingFiletype = Queue.PricingFiletype,
                PriceStatus = Queue.PriceStatus,
                PricingDate = Queue.PricingDate,
                CustomerEmail = Queue.CustomerEmail,
                QueueStatus = QueueStatus,
                QueueMessage = QueueMessage,
                IsActive = true,
                CreatedBy = PFCApiRequestDetails.CreatedBy,
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false,
                ModifiedDate = DateTime.UtcNow,
                ModifiedBy = "",
            };
            return queueHistory;
        }
    }
}