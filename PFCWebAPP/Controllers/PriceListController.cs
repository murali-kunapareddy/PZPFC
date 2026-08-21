

using Microsoft.AspNetCore.Mvc;
using PFCWebAPP.Filters;
using PFCWebAPP.Repositories.Common.Enums;
using PFCWebAPP.Repositories.Common.Interfaces;
using PFCWebAPP.Repositories.Configure.Interfaces;
using PFCWebAPP.Repositories.PriceList.Interfaces;
using PFCWebAPP.Repositories.PriceList.Models;
using PFCWebAPP.Utilities;
using System.Data;

namespace PFCWebAPP.Controllers
{
    [PFCExceptionFilter]
    public class PriceListController : BaseController
    {
        private readonly ILoggingProvider _objLoggingProvider;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IPriceListProvider _priceListpProvider;
        private readonly IConfigureProvider _configureProvider;
        public readonly ICommonProvider _objCommonProvider;

        public PriceListController(ILoggingProvider objLoggingProvider, IHttpContextAccessor contextAccessor, IPriceListProvider priceListpProvider, IConfigureProvider configureProvider, ICommonProvider objCommonProvider)
        {
            httpContextAccessor = contextAccessor;
            _objLoggingProvider = objLoggingProvider;
            _priceListpProvider = priceListpProvider;
            _configureProvider = configureProvider;
            _objCommonProvider = objCommonProvider;
        }

        public IActionResult Index()
        {
            return View();
        }

        [PFCRoleBasedAuthorizeFilter]
        [PFCSessionExpireFilter]
        public IActionResult GeneratePriceFile()
        {
            return View();
        }

        /// <summary>
        /// Get Organizations List
        /// </summary>
        /// <param></param>
        /// <returns>json</returns>
        public IActionResult GetCountryList()
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: GetRoles using Ajax Call");
                var lstcontry = _priceListpProvider.GetCountryList();
                string MaxCustomers = _objCommonProvider.GetAppSettingByName(Constants.SelectMaxCustomers);
                _objLoggingProvider.LogMessage(LogType.Info, "End: GetRoles using Ajax Call");
                var result = new { MaxSelectCustomers = MaxCustomers, data = lstcontry };
                return Json(result, new Newtonsoft.Json.JsonSerializerSettings());
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Get Customers by organization List
        /// </summary>
        /// <param></param>
        /// <returns>json</returns>
        public IActionResult GetCustomerByCountry(string ctrycode)
        {
            try
            {
                if (ctrycode != null)
                {
                    _objLoggingProvider.LogMessage(LogType.Info, "Start: GetRoles using Ajax Call");
                    var lstcontry = _priceListpProvider.GetCustomerByCountryV2(ctrycode);
                    _objLoggingProvider.LogMessage(LogType.Info, "End: GetRoles using Ajax Call");
                    return Json(lstcontry, new Newtonsoft.Json.JsonSerializerSettings());
                }
                else
                {
                    return Json(false, new Newtonsoft.Json.JsonSerializerSettings());
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// Get Customers by customer Number
        /// </summary>
        /// <param></param>
        /// <returns>json</returns>
        public IActionResult GetCustomerByCustomerNo(string ctrycode, string custList)
        {
            try
            {
                if (ctrycode != null)
                {
                    string[] customerListSel = custList.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                    _objLoggingProvider.LogMessage(LogType.Info, "Start: GetRoles using Ajax Call");
                    List<CustomersListOutput> lstcontry = _priceListpProvider.GetCustomerByCountry(ctrycode);
                    //lstcontry = lstcontry.Where(x=>x.KUNNR == )
                    var lstCustList = lstcontry.Where(x => customerListSel.Contains(x.KUNNR));

                    _objLoggingProvider.LogMessage(LogType.Info, "End: GetRoles using Ajax Call");
                    return Json(lstCustList, new Newtonsoft.Json.JsonSerializerSettings());
                }
                else
                {
                    return Json(false, new Newtonsoft.Json.JsonSerializerSettings());
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Get Organizations Discount values
        /// </summary>
        /// <param></param>
        /// <returns>json</returns>
        public IActionResult GetDiscountsByCountry()
        {
            try
            {

                _objLoggingProvider.LogMessage(LogType.Info, "Start: GetDiscounts using Ajax Call");
                var lstcontry = _priceListpProvider.GetDiscountsByCountry();
                _objLoggingProvider.LogMessage(LogType.Info, "End: GetDiscounts using Ajax Call");
                return Json(lstcontry, new Newtonsoft.Json.JsonSerializerSettings());
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Get customer template data by ID
        /// </summary>
        /// <param></param>
        /// <returns>json</returns>
        public IActionResult GetCustomerTemplateDatabyID(int masterTemplateID)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: GetCustomerTemplateDatabyID using Ajax Call");
                var lstcontry = _configureProvider.GetTemplateDataByTemplateIDV2(masterTemplateID);
                DataTable Dt = new DataTable();
                if (lstcontry != null && lstcontry.Tables != null && lstcontry.Tables.Count > 0 && lstcontry.Tables[1] != null && lstcontry.Tables[1].Rows.Count > 0)
                {
                    Dt = lstcontry.Tables[1];
                }
                _objLoggingProvider.LogMessage(LogType.Info, "End: GetCustomerTemplateDatabyID using Ajax Call");
                return Json(Dt, new Newtonsoft.Json.JsonSerializerSettings());
            }
            catch (Exception)
            {
                throw;
            }        

        }

        /// <summary>
        /// Get Organizations TradeList Template
        /// </summary>
        /// <param OrgName="CtryName"></param>
        /// <returns>json</returns>
        public IActionResult GetTradeListTemplate(string CtryName)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: GetTradeListTemplate using Ajax Call");
                var lst_template = _priceListpProvider.GetTradeListTemplate(CtryName);
                _objLoggingProvider.LogMessage(LogType.Info, "End: GetTradeListTemplate using Ajax Call");
                return Json(lst_template, new Newtonsoft.Json.JsonSerializerSettings());
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Get Customer list Template
        /// </summary>
        /// <param OrgName="CtryName"></param>
        /// <returns>json</returns>
        public IActionResult GetCustomerListTemplate(string CtryName)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: GetCustomerListTemplate using Ajax Call");
                var lst_template = _priceListpProvider.GetCustomerListTemplate(CtryName);
                _objLoggingProvider.LogMessage(LogType.Info, "End: GetCustomerListTemplate using Ajax Call");
                return Json(lst_template, new Newtonsoft.Json.JsonSerializerSettings());
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get Organizations TradeList Formate Outputs
        /// </summary>
        /// <param OrgName="CtryName"></param>
        /// <returns>json</returns>
        public IActionResult GetTradeListOutputFormate(string CtryName)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: GetTradeListOutputFormate using Ajax Call");
                var lst_template = _priceListpProvider.GetTradeListOutputFormate(CtryName);
                _objLoggingProvider.LogMessage(LogType.Info, "End: GetTradeListOutputFormate using Ajax Call");
                return Json(lst_template, new Newtonsoft.Json.JsonSerializerSettings());
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Save/ Update User PriceFile Configuration
        /// </summary>
        /// <param PriceFileSaveConfig="CtryName"></param>
        /// <returns>json</returns>
        public IActionResult UserPriceFileSaveConfig(PriceFileSaveConfig UserFileConfig)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: UpdatePriceFileConfig using Ajax Call");
                var lst_template = _priceListpProvider.UserPriceFileSaveConfig(UserFileConfig);
                _objLoggingProvider.LogMessage(LogType.Info, "End: UpdatePriceFileConfig using Ajax Call");
                return Json(lst_template, new Newtonsoft.Json.JsonSerializerSettings());
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Save/ Update User PriceFile Configuration
        /// </summary>
        /// <param PriceFileSaveConfig="CtryName"></param>
        /// <returns>json</returns>
        public IActionResult GetPriceFileSaveConfig()
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: GetPriceFileSaveConfig using Ajax Call");
                var lst_template = _priceListpProvider.GetPriceFileSaveConfigV2();
                _objLoggingProvider.LogMessage(LogType.Info, "End: GetPriceFileSaveConfig using Ajax Call");
                return Json(lst_template, new Newtonsoft.Json.JsonSerializerSettings());
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// GetExcelDetails By UserConfig
        /// </summary>
        /// <param ></param>
        /// <returns>json</returns>
        public IActionResult GetExcelDetails(long id, bool SendEmail,bool showNotFoundMaterials)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: GetExcel Details using Ajax Call");
                _priceListpProvider.GetExcelDetails(id, SendEmail, showNotFoundMaterials);
                _objLoggingProvider.LogMessage(LogType.Info, "End: GetExcel Details using Ajax Call");
                return Ok(null);
            }
            catch
            {
                throw;
            }

        }


        /// <summary>
        /// GetGenerationStatus
        /// </summary>
        /// <param ></param>
        /// <returns>ProcessBar</returns>
        public IActionResult GetGenerationStatus(long ConfigId)
        {
            try
            {
                //_objLoggingProvider.LogMessage(LogType.Info, "Start: Status of PriceFile Caluculation using Ajax Call");
                var lst_template = _priceListpProvider.GetGenerationStatus(ConfigId);
                //_objLoggingProvider.LogMessage(LogType.Info, "End: Status of PriceFile Caluculation using Ajax Call");
                return Json(lst_template, new Newtonsoft.Json.JsonSerializerSettings());
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// GetGenerationStatus
        /// </summary>
        /// <param ></param>
        /// <returns>ProcessBar</returns>
        public IActionResult GetStatusForIndividualFiles(long PriceFileHeaderId, string CustomersLst,long ReDownloadCount)
        {
            try
            {
                //_objLoggingProvider.LogMessage(LogType.Info, "Start: Status of PriceFile Caluculation using Ajax Call");
                var lst_template = _priceListpProvider.GetStatusForIndividualFiles(PriceFileHeaderId,CustomersLst,ReDownloadCount);
                //_objLoggingProvider.LogMessage(LogType.Info, "End: Status of PriceFile Caluculation using Ajax Call");
                return Json(lst_template, new Newtonsoft.Json.JsonSerializerSettings());
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// PopupStatusView
        /// </summary>
        /// <param ></param>
        /// <returns>Percentage in Partial view</returns>
        public ActionResult PopupStatusView(ProcessBar processdata)
        {
            return PartialView("_PopupStatusView", processdata);
        }


        ///// <summary>
        ///// GetGenerationExcelFiles
        ///// </summary>
        ///// <param ></param>
        ///// <returns>ProcessBar</returns>
        //public IActionResult GetGenerationExcelFiles()
        //{
        //    try
        //    {
        //        _objLoggingProvider.LogMessage(LogType.Info, "Start: Status of PriceFile Caluculation using Ajax Call");
        //        var lst_template = _priceListpProvider.GetGenerationExcelFiles();
        //        _objLoggingProvider.LogMessage(LogType.Info, "End: Status of PriceFile Caluculation using Ajax Call");
        //        return Json(lst_template, new Newtonsoft.Json.JsonSerializerSettings());
        //    }
        //    catch
        //    {
        //        throw;
        //    }

        //}

        /// <summary>
        /// Re-Generate files to server from database if it is missed
        /// </summary>
        /// <param></param>
        /// <returns>List of filename and encryptedfile name</returns>
        [HttpPost]
        public IActionResult ReDownloadExcelForCustomersFromDB(long ConfigId, long Param_PriceFileHeaderId = 0, string ConfigCustomers = "", string ArchivedMode = "")
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: Status of PriceFile ReDownloadExcelForCustomersFromDB using Ajax Call");
                var lst_template = _priceListpProvider.DownloadExcelForCustomersPrices_V2(ConfigId, Param_PriceFileHeaderId, ConfigCustomers, ArchivedMode);
                _objLoggingProvider.LogMessage(LogType.Info, "End: Status of PriceFile ReDownloadExcelForCustomersFromDB using Ajax Call");
                return Json(lst_template, new Newtonsoft.Json.JsonSerializerSettings());
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// Checks status of excel files based on pricefile header id
        /// </summary>
        /// <param></param>
        /// <returns>true if all excel files downloaded</returns>
        [HttpPost]
        public IActionResult MessageForAllExcelDownload(long PriceFileHeaderId)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: Status of PriceFile MessageForAllExcelDownload using Ajax Call");
                var status = _priceListpProvider.MessageForAllExcelDownload(PriceFileHeaderId);
                _objLoggingProvider.LogMessage(LogType.Info, "End: Status of PriceFile MessageForAllExcelDownload using Ajax Call");
                return Json(status, new Newtonsoft.Json.JsonSerializerSettings());
            }
            catch
            {
                throw;
            }

        }

    }
}
