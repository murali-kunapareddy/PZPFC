using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using NPOI.OpenXml4Net.Exceptions;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Utilities;
using PFCWebAPP.DatabaseContext.Models.CustomTables;
using PFCWebAPP.Filters;
using PFCWebAPP.Repositories.Common;
using PFCWebAPP.Repositories.Common.Enums;
using PFCWebAPP.Repositories.Common.Interfaces;
using PFCWebAPP.Repositories.Common.Models;
using PFCWebAPP.Repositories.Common.ServiceProviders;
using PFCWebAPP.Repositories.Configure;
using PFCWebAPP.Repositories.PriceList;
using PFCWebAPP.Repositories.PriceList.Interfaces;
using PFCWebAPP.Repositories.PriceList.Models.IntermediateModels;
using PFCWebAPP.Utilities;
using System.Data;

namespace PFCWebAPP.Controllers
{
    [PFCExceptionFilter]
    public class NotificationController : BaseController
    {
        private readonly ILoggingProvider _objLoggingProvider;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly INotificationProvider _notificationProvider;
        private readonly ICommonRepository _commonRepository;
        private readonly ICommonProvider _commonProvider;
        private readonly IPriceListRepository _priceListRepository;
        private readonly IPriceListProvider _priceListProvider;
        private readonly IConfigureRepository _configureRepository;
       


        public NotificationController(ILoggingProvider objLoggingProvider, IHttpContextAccessor contextAccessor, INotificationProvider notificationProvider,ICommonRepository commonRepository,ICommonProvider commonProvider,IPriceListRepository priceListRepository,IPriceListProvider priceListProvider, IConfigureRepository configureRepository)
        {
            httpContextAccessor = contextAccessor;
            _objLoggingProvider = objLoggingProvider;
            _objLoggingProvider.LogMessage(LogType.Info, "Notification Page");
            _notificationProvider = notificationProvider;
            _commonRepository = commonRepository;
            _commonProvider = commonProvider;  
            _priceListRepository = priceListRepository;
            _priceListProvider = priceListProvider;
            _configureRepository = configureRepository;
           

        }

        [PFCRoleBasedAuthorizeFilter]
        [PFCSessionExpireFilter]
        public IActionResult History()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMailToPriceFileLocationCustomers(long PriceFileHeaderID,string CustomersWithPriceHeaderId)
        {
            try
            {
                var returnval = await _notificationProvider.SendMailToPriceFileLocationCustomers(PriceFileHeaderID, CustomersWithPriceHeaderId);
                return Json(returnval, new Newtonsoft.Json.JsonSerializerSettings());
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Error while DownLoadAll Files ", ex);
                throw;
            }
        }


        public IActionResult GetNotificationHistory()
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: GetNotificationHistory using Ajax Call");
                var notificationHistory = _notificationProvider.GetNotificationHistory();
                _objLoggingProvider.LogMessage(LogType.Info, "End: GetNotificationHistory using Ajax Call");
                return Json(notificationHistory, new Newtonsoft.Json.JsonSerializerSettings());
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("GetNotificationHistory :", ex);
                throw;
            }
        }

        public async Task<IActionResult> ReSendNotificationAsync(long notificationId)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: ReSendNotification using Ajax Call");
                var resendNotificationStatus = await _notificationProvider.ReSendEmailNotification(notificationId);
                _objLoggingProvider.LogMessage(LogType.Info, "End: ReSendNotification using Ajax Call");
                return Json(resendNotificationStatus, new Newtonsoft.Json.JsonSerializerSettings());
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("ReSendNotification :", ex);
                throw;
            }
        }

        public FileResult DownloadFile(string EncryptedFileName, string ActualFileName, int NotificationHistoryId)
        {
            try
            {
                string fpath = Path.Combine(Directory.GetCurrentDirectory(), AppConfig.PFCDownloadedFileLoaction);
                string fname = Path.Combine(fpath, EncryptedFileName);
                byte[] bytes = System.IO.File.ReadAllBytes(fname);
                return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, ActualFileName);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [HttpPost]
        public IActionResult GetMailStatusInNotificationHistory(List<long> ids)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: GetMailStatusInNotificationHistory using Ajax Call");
                var boolResult = _notificationProvider.GetMailStatusInNotificationHistory(ids);
                _objLoggingProvider.LogMessage(LogType.Info, "End: GetMailStatusInNotificationHistory using Ajax Call");
                return Json(boolResult, new Newtonsoft.Json.JsonSerializerSettings());
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("GetMailStatusInNotificationHistory :", ex);
                throw;
            }
        }

        public IActionResult CheckIfGeneratedExcelFileExists(int NotificationHistoryId, string EncryptedFileName, string ActualFileName)
        {
            try
            {
                string customerNumber = ActualFileName.Split('-')[0];
                string fpath = Path.Combine(Directory.GetCurrentDirectory(), AppConfig.PFCDownloadedFileLoaction);
                string fname = Path.Combine(fpath, EncryptedFileName);

                if (System.IO.File.Exists(fname))
                {
                    var fileNames = new
                    {
                       PFCEncryptedFileName = EncryptedFileName,
                       PFCActualFileName = ActualFileName
                    }; 
                    return Json(fileNames, new Newtonsoft.Json.JsonSerializerSettings());
                }
                else
                {
                    var customers = (from res in _commonRepository.NotificationHistoryRepository.GetManyQueryable()
                                     join pfh in _priceListRepository.PriceFileHeaderRepository.GetManyQueryable() on res.PriceFileHeaderID equals pfh.PriceFileHeaderID
                                     join ucs in _configureRepository.UserConfigSettingRepository.GetManyQueryable() on pfh.UserConfigSettingID equals ucs.UserConfigSettingID
                                     join tm in _configureRepository.TemplateMasterRepository.GetManyQueryable() on ucs.ReportContentTemplateID equals tm.TemplateMasterID
                                     where res.NotificationHistoryID == NotificationHistoryId
                                     select new
                                     {
                                         selectedcustomers = ucs.SelectedCustomers,
                                         PFCZipFileName = tm.TemplateName,
                                         PriceFileHeaderId = pfh.PriceFileHeaderID,
                                         configurationSetingID = ucs.UserConfigSettingID
                                     }
                   ).FirstOrDefault();
                    var lst_cust = JsonConvert.DeserializeObject<List<SelectedCustomers>>(customers.selectedcustomers);
                    var customerDetails = lst_cust.Where(x => x.CustomerNumber == customerNumber.Trim());
                    var objCustomers = (from cust in customerDetails
                                        select new SelectedCustomersByHeaderID
                                        {
                                            PriceFileHeaderID = customers.PriceFileHeaderId,
                                            PFCZipFileName = customers.PFCZipFileName,
                                            CustomerSNO = customerDetails.Select(a => a.CustomerSNO).First(),
                                            CustomerNumber = customerDetails.Select(a => a.CustomerNumber).First(),
                                            CustomerName = customerDetails.Select(a => a.CustomerName).First(),
                                            zKUNNR = customerDetails.Select(a => a.zKUNNR).First(),
                                            PC1 = customerDetails.Select(a => a.PC1).First(),
                                            PC2 = customerDetails.Select(a => a.PC2).First(),
                                            PC3 = customerDetails.Select(a => a.PC3).First()
                                        }).ToList();
                    var customers_obj = JsonConvert.SerializeObject(objCustomers);
                    var fileNames = _priceListProvider.DownloadExcelForCustomersPrices_V2(customers.configurationSetingID, customers.PriceFileHeaderId, customers_obj, Constants.DataBase.Trim().ToLower());
                    using (ISqlHelper objSqlHelper = new SqlHelper(AppConfig.ConnectionString))
                    {
                        string strUpdateQuery = "Update dbo.TRN_NotificationHistory SET EncryptedFileName = @EncryptedFileName WHERE NotificationHistoryID = @NotificationHistoryID";
                        List<SqlParameter> lstParameters = new List<SqlParameter>();
                        lstParameters.Add(new SqlParameter() { ParameterName = "@EncryptedFileName", Value = fileNames[0]["PFCEncryptedFileName"] });
                        lstParameters.Add(new SqlParameter() { ParameterName = "@NotificationHistoryID", Value = NotificationHistoryId });

                        int output = objSqlHelper.ExecuteNonQuery(CommandType.Text, strUpdateQuery, lstParameters.ToArray());
                    }


                    return Json(fileNames[0], new Newtonsoft.Json.JsonSerializerSettings());
                }
            }
            catch (Exception e)
            {
                throw;
            }

        }

    }
}
