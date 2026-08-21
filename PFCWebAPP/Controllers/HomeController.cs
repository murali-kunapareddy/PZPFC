using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PFCWebAPP.DatabaseContext.Models.CustomTables;
using PFCWebAPP.Models;
using PFCWebAPP.Repositories.BackOps.Interfaces;
using PFCWebAPP.Repositories.Common.Enums;
using PFCWebAPP.Repositories.Common.Interfaces;
using PFCWebAPP.Repositories.Common.Models;
using PFCWebAPP.Repositories.Configure;
using PFCWebAPP.Repositories.PriceList;
using PFCWebAPP.Repositories.PriceList.Interfaces;
using PFCWebAPP.Utilities;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using System.Diagnostics;
using System.IO.Compression;
using System.Reflection;
using Microsoft.IdentityModel.Logging;
using PFCWebAPP.Repositories.PriceList.Models.IntermediateModels;
using static Org.BouncyCastle.Math.EC.ECCurve;
using PFCWebAPP.Filters;

namespace PFCWebAPP.Controllers
{
    [PFCExceptionFilter]
    public class HomeController : BaseController
    {
        private readonly ILoggingProvider _objLoggingProvider;
        private readonly IHttpContextAccessor httpContextAccessor;
        public readonly ICommonProvider _objCommonProvider;
        private readonly IPriceListProvider _objPriceListProvider;
        private readonly IPriceListRepository _objPriceListRepository;
        private readonly IConfigureRepository _objConfigureRepository;

        public HomeController(ILoggingProvider objLoggingProvider, IHttpContextAccessor contextAccessor, IBackOpsProvider objBackOpsProvider, ICommonProvider objCommonProvider, IPriceListProvider objPriceListProvider, IConfigureRepository objConfigureRepository, IPriceListRepository objPriceListRepository)
        {
            httpContextAccessor = contextAccessor;
            _objLoggingProvider = objLoggingProvider;
            _objCommonProvider = objCommonProvider;
            _objPriceListProvider = objPriceListProvider;
            _objConfigureRepository = objConfigureRepository;
            _objPriceListRepository = objPriceListRepository;
            _objLoggingProvider.LogMessage(LogType.Info, "Home Page");
        }

        public IActionResult Index(string Code = "")
        {
            try
            {
                UserDashboardViewModel uDVM = new();
                uDVM.objUserInfo = new();
                uDVM.objPFCSummaryInfo = new();
                uDVM.objUserInfo = _objCommonProvider.GetLoginUserDetails();
                if (uDVM.objUserInfo != null)
                {
                    uDVM.IsPFCUser = _objCommonProvider.IsValidUser(uDVM.objUserInfo.EmployeeSESA);
                }

                return View(uDVM);
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Home :", ex);
                throw;
            }
        }

        public ActionResult GetDashboardData()
        {
            try
            {
                UserDashboardViewModel uDVM = new();
                uDVM.objUserInfo = new();
                uDVM.objPFCSummaryInfo = new();
                uDVM.objUserInfo = _objCommonProvider.GetLoginUserDetails();
                if (uDVM.objUserInfo != null)
                {
                    uDVM.IsPFCUser = _objCommonProvider.IsValidUser(uDVM.objUserInfo.EmployeeSESA);
                }

                if (uDVM.objUserInfo != null && uDVM.IsPFCUser)
                {
                    uDVM.objPFCSummaryInfo = _objCommonProvider.GetPFCSummaryInfoByUserSESA().ToList();
                }
                return Json(uDVM, new Newtonsoft.Json.JsonSerializerSettings());
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Error while GetDashboardData ", ex);
                throw;
            }
        }

        public IActionResult DownLoadAll(long PriceFileHeaderID, string FileName)
        {
            try
            {
                bool IsFilesExits = false;
                var zipName = $"{FileName}-{DateTime.UtcNow.ToString("yyyyMMddHHmmss").Replace(" / ", " - ")}.zip";
                List<PriceFileLocationDetails> lstPriceFileLocationDetails = _objCommonProvider.PriceFileLocationInfoByHeaderID(PriceFileHeaderID).ToList();
                using (MemoryStream ms = new MemoryStream())
                {
                    //required: using System.IO.Compression;  
                    using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, true))
                    {
                        //QUery the Products table and get all image content  
                        lstPriceFileLocationDetails.ForEach(file =>
                        {
                            string ExcelfileURL = PFCWebAPP.Utilities.AppConfig.PFCDownloadedFileLoaction + "\\" + file.PFCEncryptedFileName;
                            if (System.IO.File.Exists(ExcelfileURL))
                            {
                                IsFilesExits = true;
                                string PFCDownloadedFileLoaction = PFCWebAPP.Utilities.AppConfig.PFCDownloadedFileLoaction;
                                byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(PFCDownloadedFileLoaction, file.PFCEncryptedFileName));

                                var entry = zip.CreateEntry(file.PFCActualFileName);
                                using (var fileStream = new MemoryStream(fileBytes))
                                using (var entryStream = entry.Open())
                                {
                                    fileStream.CopyTo(entryStream);
                                }
                            }
                        });
                    }
                    return File(ms.ToArray(), "application/zip", zipName);
                    //if(IsFilesExits)
                    //{
                    //    return File(ms.ToArray(), "application/zip", zipName);
                    //}
                    //else
                    //{
                    //    return Json("");
                    //}
                }


            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Error while DownLoadAll Files ", ex);
                throw;
            }
        }

        public IActionResult DownLoadPriceFilesForSelectedCustomers(string SelectedCustomersInfo,string Mode = "")
        {
            try
            {
                List<SelectedCustomersByHeaderID> lstcustomers = JsonConvert.DeserializeObject<List<SelectedCustomersByHeaderID>>(SelectedCustomersInfo);
                long PriceFileHeaderID = lstcustomers.FirstOrDefault().PriceFileHeaderID;
                var userConfigId = _objPriceListRepository.PriceFileHeaderRepository.GetQueryable(x => x.PriceFileHeaderID == PriceFileHeaderID).FirstOrDefault().UserConfigSettingID;
                string FileName = lstcustomers.FirstOrDefault().PFCZipFileName;
                string ArchivedRecords = _objCommonProvider.GetAppSettingByName(Constants.ArchivedFileLocationMode);
                string ArchivedExtractionMode = _objCommonProvider.GetAppSettingByName(Constants.ArchivedFileExtractionMode);
                var zipName = $"{FileName}-{DateTime.UtcNow.ToString("yyyyMMddHHmmss").Replace(" / ", " - ")}.zip";

                if (Mode == Constants.File.Trim().ToLower())
                {
                    //List<SelectedCustomersByHeaderID> missed_cust = new();
                    //List<string> listCustomersNos = new();
                    //List<SelectedCustomersByHeaderID> selected_cust = new();

                    //List<PriceFileLocationDetails> lstPriceFileLocation = _objCommonProvider.PriceFileLocationInfoByHeaderID(PriceFileHeaderID).ToList();
                    //lstPriceFileLocation.ForEach(file =>
                    //{
                    //    string ExcelfileURL = PFCWebAPP.Utilities.AppConfig.PFCDownloadedFileLoaction + "\\" + file.PFCEncryptedFileName;
                    //    if (System.IO.File.Exists(ExcelfileURL))
                    //    {

                    //    }
                    //    else
                    //    {
                    //        listCustomersNos.Add(file.CustomerNo);
                    //    }
                    //});
                    //missed_cust = lstcustomers.Where(s => listCustomersNos.Contains(s.zKUNNR)).ToList();
                    //string passingCust = JsonConvert.SerializeObject(missed_cust);
                    //List<Dictionary<string, string>> filenames = _objPriceListProvider.DownloadExcelForCustomersPrices_V2(userConfigId, PriceFileHeaderID, passingCust, "Database");

                    bool IsFilesExits = false;

                    List<PriceFileLocationDetails> lstPriceFileLocationDetails = _objCommonProvider.PriceFileLocationInfoByHeaderID(PriceFileHeaderID).ToList();
                    if (lstPriceFileLocationDetails.Count() != lstcustomers.Count())
                    {
                        zipName = $"{FileName}-SelectedCustomers-{lstcustomers.Count()}-{DateTime.UtcNow.ToString("yyyyMMddHHmmss").Replace(" / ", " - ")}.zip";
                    }
                    lstPriceFileLocationDetails = (from n in lstPriceFileLocationDetails join m in lstcustomers on n.CustomerNo equals m.zKUNNR select n).ToList();

                    using (MemoryStream ms = new MemoryStream())
                    {
                        //required: using System.IO.Compression;  
                        using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, true))
                        {
                            //QUery the Products table and get all image content  
                            lstPriceFileLocationDetails.ForEach(file =>
                            {
                                string ExcelfileURL = PFCWebAPP.Utilities.AppConfig.PFCDownloadedFileLoaction + "\\" + file.PFCEncryptedFileName;
                                if (System.IO.File.Exists(ExcelfileURL))
                                {
                                    IsFilesExits = true;
                                    string PFCDownloadedFileLoaction = PFCWebAPP.Utilities.AppConfig.PFCDownloadedFileLoaction;
                                    byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(PFCDownloadedFileLoaction, file.PFCEncryptedFileName));

                                    var entry = zip.CreateEntry(file.PFCActualFileName);
                                    using (var fileStream = new MemoryStream(fileBytes))
                                    using (var entryStream = entry.Open())
                                    {
                                        fileStream.CopyTo(entryStream);
                                    }
                                }
                            });
                        }
                        return File(ms.ToArray(), "application/zip", zipName);
                    }
                }
                else if (Mode == Constants.DataBase.Trim().ToLower())
                {
                    List<Dictionary<string, string>> filenames = _objPriceListProvider.DownloadExcelForCustomersPrices_V2(userConfigId, PriceFileHeaderID, SelectedCustomersInfo, ArchivedRecords);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, true))
                        {
                            filenames.ForEach(file =>
                            {
                                string PFCDownloadedFileLoaction = PFCWebAPP.Utilities.AppConfig.PFCDownloadedFileLoaction;
                                byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(PFCDownloadedFileLoaction, file["PFCEncryptedFileName"]));
                                var entry = zip.CreateEntry(file["PFCActualFileName"]);
                                using (var fileStream = new MemoryStream(fileBytes))
                                using (var entryStream = entry.Open())
                                {
                                    fileStream.CopyTo(entryStream);
                                }

                            });
                        }
                        return File(ms.ToArray(), "application/zip", zipName);
                    }
                }
                else
                {
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Error while DownLoadAll Files ", ex);
                throw;
            }
        }

        // To Download all PriceFilesDetails by clicking download button in Home page
        public IActionResult DownloadFilesCustom(long PriceFileHeaderID, string FileName, string Customers = "",string Mode="")
        {
            try
            {
                string ArchivedRecords = _objCommonProvider.GetAppSettingByName(Constants.ArchivedFileLocationMode);
                string ArchivedExtractionMode = _objCommonProvider.GetAppSettingByName(Constants.ArchivedFileExtractionMode);
                var zipName = $"{FileName}-{DateTime.UtcNow.ToString("yyyyMMddHHmmss").Replace(" / ", " - ")}.zip";
                if (Constants.File.Trim().ToLower() == Mode)
                {
                    //List<SelectedCustomersByHeaderID> missed_cust = new();
                    //List<string> listCustomersNos = new();
                    //List<SelectedCustomersByHeaderID> selected_cust = new();
                    //List<SelectedCustomers> selectedCustomers = new();
                    //string passingCust = string.Empty;
                    //List<PriceFileLocationDetails> lstPriceFileLocation = _objCommonProvider.PriceFileLocationInfoByHeaderID(PriceFileHeaderID).ToList();
                    //var userConfigId = _objPriceListRepository.PriceFileHeaderRepository.GetQueryable(x => x.PriceFileHeaderID == PriceFileHeaderID).FirstOrDefault().UserConfigSettingID;
                    //lstPriceFileLocation.ForEach(file =>
                    //{
                    //    string ExcelfileURL = PFCWebAPP.Utilities.AppConfig.PFCDownloadedFileLoaction + "\\" + file.PFCEncryptedFileName;
                    //    if (System.IO.File.Exists(ExcelfileURL))
                    //    {

                    //    }
                    //    else
                    //    {
                    //        listCustomersNos.Add(file.CustomerNo);
                    //    }
                    //});
                    //if (Customers != "")
                    //{
                    //    selectedCustomers = JsonConvert.DeserializeObject<List<SelectedCustomers>>(Customers);
                    //    missed_cust = (from selcust in selectedCustomers

                    //                   select new SelectedCustomersByHeaderID
                    //                   {
                    //                       PriceFileHeaderID = PriceFileHeaderID,
                    //                       PFCZipFileName = FileName,
                    //                       CustomerSNO = selcust.CustomerSNO,
                    //                       CustomerNumber = selcust.CustomerNumber,
                    //                       CustomerName = selcust.CustomerName,
                    //                       zKUNNR = selcust.zKUNNR,
                    //                       PC1 = selcust.PC1,
                    //                       PC2 = selcust.PC2,
                    //                       PC3 = selcust.PC3
                    //                   }
                    //               ).ToList();

                    //    passingCust = JsonConvert.SerializeObject(missed_cust);
                    //}
                    //List<Dictionary<string, string>> filenames = _objPriceListProvider.DownloadExcelForCustomersPrices_V2(userConfigId, PriceFileHeaderID, passingCust, "Database");
                   
                    bool IsFilesExits = false;

                    List<PriceFileLocationDetails> lstPriceFileLocationDetails = _objCommonProvider.PriceFileLocationInfoByHeaderID(PriceFileHeaderID).ToList();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        //required: using System.IO.Compression;  
                        using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, true))
                        {
                            //QUery the Products table and get all image content  
                            lstPriceFileLocationDetails.ForEach(file =>
                            {
                                string ExcelfileURL = PFCWebAPP.Utilities.AppConfig.PFCDownloadedFileLoaction + "\\" + file.PFCEncryptedFileName;
                                if (System.IO.File.Exists(ExcelfileURL))
                                {
                                    IsFilesExits = true;
                                    string PFCDownloadedFileLoaction = PFCWebAPP.Utilities.AppConfig.PFCDownloadedFileLoaction;
                                    byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(PFCDownloadedFileLoaction, file.PFCEncryptedFileName));

                                    var entry = zip.CreateEntry(file.PFCActualFileName);
                                    using (var fileStream = new MemoryStream(fileBytes))
                                    using (var entryStream = entry.Open())
                                    {
                                        fileStream.CopyTo(entryStream);
                                    }
                                }
                            });
                        }
                        return File(ms.ToArray(), "application/zip", zipName);
                    }
                }
                else if(Constants.DataBase.Trim().ToLower() == Mode)
                {
                    List<SelectedCustomers> selectedCustomers = new();
                    string passingCust = string.Empty;
                    var userConfigId = _objPriceListRepository.PriceFileHeaderRepository.GetQueryable(x => x.PriceFileHeaderID == PriceFileHeaderID).FirstOrDefault().UserConfigSettingID;
                    var userConfig = _objConfigureRepository.UserConfigSettingRepository.GetQueryable(x => x.SelectedCustomers != "" && x.UserConfigSettingID == userConfigId).FirstOrDefault();
                    var SelectedCustomersInfo = userConfig.SelectedCustomers;
                    selectedCustomers = JsonConvert.DeserializeObject<List<SelectedCustomers>>(SelectedCustomersInfo);
                    var s_cust = (from selcust in selectedCustomers

                                   select new SelectedCustomersByHeaderID
                                   {
                                       PriceFileHeaderID = PriceFileHeaderID,
                                       PFCZipFileName = FileName,
                                       CustomerSNO = selcust.CustomerSNO,
                                       CustomerNumber = selcust.CustomerNumber,
                                       CustomerName = selcust.CustomerName,
                                       zKUNNR = selcust.zKUNNR,
                                       PC1 = selcust.PC1,
                                       PC2 = selcust.PC2,
                                       PC3 = selcust.PC3
                                   }
                               ).ToList();

                    passingCust = JsonConvert.SerializeObject(s_cust);

                    List<Dictionary<string, string>> filenames = _objPriceListProvider.DownloadExcelForCustomersPrices_V2(userConfigId, PriceFileHeaderID, passingCust, ArchivedRecords);
                   // Thread.Sleep(2000);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, true))
                        {
                            filenames.ForEach(file =>
                            {
                                string PFCDownloadedFileLoaction = PFCWebAPP.Utilities.AppConfig.PFCDownloadedFileLoaction;
                                byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(PFCDownloadedFileLoaction, file["PFCEncryptedFileName"]));
                                var entry = zip.CreateEntry(file["PFCActualFileName"]);
                                using (var fileStream = new MemoryStream(fileBytes))
                                using (var entryStream = entry.Open())
                                {
                                    fileStream.CopyTo(entryStream);
                                }

                            });
                        }
                        return File(ms.ToArray(), "application/zip", zipName);
                    }

                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Error while DownLoadAll Files ", ex);
                throw;
            }
        }

        // To check wether all files exists or not in server path
        public IActionResult CheckStatusOfFiles(long PriceFileHeaderID, string FileName, string Customers)
        {
            try
            {
                string ArchivedRecords = _objCommonProvider.GetAppSettingByName(Constants.ArchivedFileLocationMode);
                string ArchivedExtractionMode = _objCommonProvider.GetAppSettingByName(Constants.ArchivedFileExtractionMode);
                List<SelectedCustomers> selected_cust = new();
                List<SelectedCustomers> missed_cust = new();
                List<string> listCustomersNos = new();
                long ConfigId = _objPriceListRepository.PriceFileHeaderRepository.GetQueryable(c => c.PriceFileHeaderID == PriceFileHeaderID).FirstOrDefault().UserConfigSettingID;
                if (Customers != "")
                    selected_cust = JsonConvert.DeserializeObject<List<SelectedCustomers>>(Customers);

                List<PriceFileLocationDetails> lstPriceFileLocationDetails = _objCommonProvider.PriceFileLocationInfoByHeaderID(PriceFileHeaderID).ToList();
                lstPriceFileLocationDetails.ForEach(file =>
                {
                    string ExcelfileURL = PFCWebAPP.Utilities.AppConfig.PFCDownloadedFileLoaction + "\\" + file.PFCEncryptedFileName;
                    if (System.IO.File.Exists(ExcelfileURL))
                    {

                    }
                    else
                    {
                        listCustomersNos.Add(file.CustomerNo);
                    }
                });
                missed_cust = selected_cust.Where(s => listCustomersNos.Contains(s.zKUNNR)).ToList();
                var redownloadquery = _objPriceListRepository.PriceFileLocationDetailsRepository
                    .GetManyQueryable(a => a.IsActive == true && a.PriceFileHeaderID == PriceFileHeaderID).
                    Where(s => listCustomersNos.Contains(s.CustomerNo))
                    .ToList();
                long redownloadcnt = redownloadquery.Sum(a => a.ReDownloadCount);
                object exl = new
                {
                    ArchivedFileLocationMode = ArchivedRecords.Trim().ToLower(),
                    ArchivedFileExtractionMode = ArchivedExtractionMode.Trim().ToLower(),
                    PriceFileHeaderID = PriceFileHeaderID,
                    FileName = FileName,
                    Customers = missed_cust,
                    listCustomers = listCustomersNos,
                    ReDownloadCount= redownloadcnt,
                    AllFilesExists = missed_cust.Count == 0 ? true : false,
                    UserConfigId = ConfigId
                };
                return Json(exl, new Newtonsoft.Json.JsonSerializerSettings());


            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Error while DownLoadAll Files ", ex);
                throw;
            }
        }

        public IActionResult DownloadMissedFilesFromServerForMail(long PriceFileHeaderID, string FileName, string Customers = "")
        {
            try
            {
                string ArchivedRecords = _objCommonProvider.GetAppSettingByName(Constants.ArchivedFileLocationMode);
                List<Dictionary<string, string>> filenames = new();
                if (ArchivedRecords.Trim().ToLower() == "ApplicationServer".ToLower())
                {
                    List<SelectedCustomersByHeaderID> missed_cust = new();
                    List<string> listCustomersNos = new();
                    List<SelectedCustomersByHeaderID> selected_cust = new();
                    List<SelectedCustomers> selectedCustomers = new();
                    string passingCust = string.Empty;
                    List<PriceFileLocationDetails> lstPriceFileLocation = _objCommonProvider.PriceFileLocationInfoByHeaderID(PriceFileHeaderID).ToList();
                    var userConfigId = _objPriceListRepository.PriceFileHeaderRepository.GetQueryable(x => x.PriceFileHeaderID == PriceFileHeaderID).FirstOrDefault().UserConfigSettingID;
                    lstPriceFileLocation.ForEach(file =>
                    {
                        string ExcelfileURL = PFCWebAPP.Utilities.AppConfig.PFCDownloadedFileLoaction + "\\" + file.PFCEncryptedFileName;
                        if (System.IO.File.Exists(ExcelfileURL))
                        {

                        }
                        else
                        {
                            listCustomersNos.Add(file.CustomerNo);
                        }
                    });

                    if (Customers != "")
                    {
                        selectedCustomers = JsonConvert.DeserializeObject<List<SelectedCustomers>>(Customers);
                        missed_cust = (from selcust in selectedCustomers

                                       select new SelectedCustomersByHeaderID
                                       {
                                           PriceFileHeaderID = PriceFileHeaderID,
                                           PFCZipFileName = FileName,
                                           CustomerSNO = selcust.CustomerSNO,
                                           CustomerNumber = selcust.CustomerNumber,
                                           CustomerName = selcust.CustomerName,
                                           zKUNNR = selcust.zKUNNR,
                                           PC1 = selcust.PC1,
                                           PC2 = selcust.PC2,
                                           PC3 = selcust.PC3
                                       }
                                   ).ToList();

                        passingCust = JsonConvert.SerializeObject(missed_cust);
                    }


                    filenames = _objPriceListProvider.DownloadExcelForCustomersPrices_V2(userConfigId, PriceFileHeaderID, passingCust, "Database");

                }
                return Json(filenames, new Newtonsoft.Json.JsonSerializerSettings()); ;

            }
            catch(Exception ex) 
            {
                _objLoggingProvider.LogException("Error while DownLoading Missed Files for Mail", ex);
                throw;
            }
        }

        public static void RemoveCellComment(ICell cell)
        {
            if (cell.CellComment != null)
            {
                cell.CellComment = null;
            }
        }


        public EmptyResult EmptyData()
        {
            return new EmptyResult();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}