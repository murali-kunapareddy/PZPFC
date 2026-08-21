using NPOI.SS.UserModel;
using PFCWebAPP.DatabaseContext.Models.CustomTables;
using PFCWebAPP.Repositories.PriceList.Models;
using PFCWebAPP.Repositories.PriceList.Models.API;
using PFCWebAPP.Repositories.PriceList.Models.Masters;

namespace PFCWebAPP.Repositories.PriceList.Interfaces
{
    public interface IPriceListProvider : IDisposable
    {
        List<CountryListOutput> GetCountryList();
        List<CustomersListOutput> GetCustomerByCountry(string ctryCode);
        List<CustomersListOutput> GetCustomerByCountryV2(string ctryCode);
        List<CustomersListOutput> GetCustomerByCustomerList(string ctryCode, string[] custList);
        List<DiscountParameters> GetDiscountsByCountry();
        List<TradeTemplate> GetTradeListTemplate(string CtryName);
        List<TradeTemplate> GetCustomerListTemplate(string CtryName);
        List<TradeTemplateOutputFormate> GetTradeListOutputFormate(string CtryName);
        int UserPriceFileSaveConfig(PriceFileSaveConfig UserFileConfig);
        SelectedUserConfigSetting GetPriceFileSaveConfig();
        SelectedUserConfigSetting GetPriceFileSaveConfigV2();
        void GetExcelDetails(long id,bool SendEmail,bool showNotFoundMaterials);
        ProcessBar GetGenerationStatus(long ConfigId);
        ProcessBar GetStatusForIndividualFiles(long PricFileHeaderId, string Customers, long ReDownloadCount);
        //string GetGenerationExcelFiles();
        IEnumerable<PriceFileDetails> GetPriceFileDetailsForCustomer(long PriceFileHeaderID, string CustomerId,bool showNotFoundMaterials);
        long GetTotalRecordsCount(long id, string CustomerId);
        IFont CreateHeaderFont(IWorkbook workbook);
        ICellStyle CreateHeaderCellStyle(IWorkbook workbook); 
        IFont CreateHeaderLastFont(IWorkbook workbook);
        ICellStyle CreateHeaderLastCellStyle(IWorkbook workbook);
        IFont CreateDataFont(IWorkbook workbook);
        ICellStyle CreateDataCellStyle(IWorkbook workbook);
        ICellStyle CreateDataDigitCellStyle(IWorkbook workbook);
        ICellStyle CreateDataDarkCellStyle(IWorkbook workbook);
        ICellStyle CreateDataLightCellStyle(IWorkbook workbook);
        List<Dictionary<string, string>> DownloadExcelForCustomersPrices_V2(long ConfigId, long Param_PriceFileHeaderId = 0, string ConfigCustomers = "",string ArchivedMode="", bool SendEmail=false, bool showNotFoundMaterials = false);
        ProcessBar GetGenerationHomeStatus(long PriceFileHeaderID);
        bool MessageForAllExcelDownload(long PriceFileHeaderId);
        int GetTradeFormatListByCustomerCountry(string CountryCode, string PC1);

        CustomerSettings GetCustomerSettings(string CustomerNumber, string SalesOrganization);

        QueueModel SaveApiRequests(QueueModel queueModel);
    }
}
