using PFCRepository.Repositories.Common.Interfaces;
using System.Data;
using System.Text;
using PFCRepository.Repositories.Common.Models;
using PFCRepository.Repositories.Configure;
using PFCRepository.Repositories.PriceList.Models.IntermediateModels;
using Newtonsoft.Json;
using PFCRepository.DatabaseContext.Models.CustomTables;
using PFCRepository.Repositories.PriceList;
using System.Collections.Generic;
using PFCRepository.Repositories.Common.Enums;
using PFCRepository.Utilities;
using static PFCRepository.Utilities.Common;
using Microsoft.Data.SqlClient;

namespace PFCRepository.Repositories.Common.ServiceProviders
{
    public class CommonProvider : ICommonProvider
    {

        private readonly ILoggingProvider _objLoggingProvider;
        private readonly ICommonRepository _objCommonRepository;
        private readonly IConfigureRepository _objConfigureRepository;
        private readonly IPriceListRepository _objPriceListRepository;

        public CommonProvider(ILoggingProvider objLoggingProvider, ICommonRepository objCommonRepository, IConfigureRepository objConfigureRepository, IPriceListRepository objPriceListRepository)
        {
            _objLoggingProvider = objLoggingProvider;
            _objCommonRepository = objCommonRepository;
            _objConfigureRepository = objConfigureRepository;
            _objPriceListRepository = objPriceListRepository;
        }

        public string GetLoginUserSESA()
        {
            try
            {
                return PFCApiRequestDetails.CreatedBy;          
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("GetDashboardData :" , ex);
                throw;
            }
        }



        /// <summary>
        /// GetPFCSummaryInfoByUserSESA
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<PFCSummaryInfo> GetPFCSummaryInfoByUserSESA()
        {
            try
            {

                int MaxCount = 5000;
                try
                {
                    string DisplayMaxRecords = GetAppSettingByName(Constants.DisplayMaxRecords);
                    if (DisplayMaxRecords != "" && DisplayMaxRecords != null)
                    {
                        MaxCount = Convert.ToInt32(DisplayMaxRecords);
                    }
                }
                catch (Exception ex1)
                {
                    _objLoggingProvider.LogException("GetPFCSummaryInfoByUserSESA(DisplayMaxRecords) :", ex1);
                }

                List<PFCSummaryInfo> objPFCSummaryInfo = new();
                string UserSESA = GetLoginUserSESA();
                _objLoggingProvider.LogMessage(LogType.Info, "Start: Get PFC Summary by UserSESA");
                if (IsValidUser(UserSESA))
                {
                    if (IsAdminUser(UserSESA))
                    {
                        objPFCSummaryInfo = (from ph in _objCommonRepository.PriceFileHeaderRepository.GetManyQueryable()
                                             join uc in _objConfigureRepository.UserConfigSettingRepository.GetManyQueryable()
                                             on ph.UserConfigSettingID equals uc.UserConfigSettingID
                                             join tm in _objConfigureRepository.TemplateMasterRepository.GetManyQueryable()
                                             on uc.ReportContentTemplateID equals tm.TemplateMasterID into tml
                                             from template in tml.DefaultIfEmpty()
                                             join rm in _objConfigureRepository.ReportFormatMasterRepository.GetManyQueryable()
                                             on uc.ReportFormatTemplateID equals rm.ReportFormatMasterID into rml
                                             from reportFormat in rml.DefaultIfEmpty()
                                             where ph.IsCompleted == true && ph.Status == "Completed" && ph.IsActive == true
                                             orderby ph.PriceFileHeaderID descending

                                             select new PFCSummaryInfo
                                             {
                                                 PriceFileHeaderID = Convert.ToInt32(ph.PriceFileHeaderID),
                                                 PriceFileCreatedDate = DateTime.Parse(ph.CreatedDate.ToString()).ToString("dd-MM-yyy"),
                                                 SalesOrganization = uc.SalesOrganization,
                                                 PricesActiveDate = DateTime.Parse(uc.PricesActiveDate.ToString()).ToString("dd-MM-yyy"),
                                                 TradeListTemplate = uc.CanUseAutoReportContent == true ? "AutoTemplate" : (template != null ? template.AliasName : ""),
                                                 TradeListFormat = reportFormat != null ? reportFormat.AliasName : "",
                                                 lstSelectedCustomers = JsonConvert.DeserializeObject<List<SelectedCustomers>>(uc.SelectedCustomers),
                                                 Customers = uc.SelectedCustomers,
                                                 UserSESA = uc.UserSESA
                                             }).Take(MaxCount).ToList();

                    }
                    else
                    {
                        objPFCSummaryInfo = (from ph in _objCommonRepository.PriceFileHeaderRepository.GetManyQueryable()
                                             join uc in _objConfigureRepository.UserConfigSettingRepository.GetManyQueryable()
                                             on ph.UserConfigSettingID equals uc.UserConfigSettingID
                                             join tm in _objConfigureRepository.TemplateMasterRepository.GetManyQueryable()
                                             on uc.ReportContentTemplateID equals tm.TemplateMasterID into tml
                                             from template in tml.DefaultIfEmpty()
                                             join rm in _objConfigureRepository.ReportFormatMasterRepository.GetManyQueryable()
                                             on uc.ReportFormatTemplateID equals rm.ReportFormatMasterID into rml
                                             from reportFormat in rml.DefaultIfEmpty()
                                             where ph.IsCompleted == true && ph.Status == "Completed" && uc.UserSESA.ToUpper() == UserSESA.ToUpper() && ph.IsActive == true
                                             orderby ph.PriceFileHeaderID descending

                                             select new PFCSummaryInfo
                                             {
                                                 PriceFileHeaderID = Convert.ToInt32(ph.PriceFileHeaderID),
                                                 PriceFileCreatedDate = DateTime.Parse(ph.CreatedDate.ToString()).ToString("dd-MM-yyy"),
                                                 SalesOrganization = uc.SalesOrganization,
                                                 PricesActiveDate = DateTime.Parse(uc.PricesActiveDate.ToString()).ToString("dd-MM-yyy"),
                                                 TradeListTemplate = uc.CanUseAutoReportContent == true ? "AutoTemplate" : (template != null ? template.AliasName : ""),
                                                 TradeListFormat = reportFormat != null ? reportFormat.AliasName : "",
                                                 lstSelectedCustomers = JsonConvert.DeserializeObject<List<SelectedCustomers>>(uc.SelectedCustomers),
                                                 Customers = uc.SelectedCustomers,
                                                 UserSESA = uc.UserSESA
                                             }).Take(MaxCount).ToList();
                    }
                    objPFCSummaryInfo.Select(c => { c.lstSelectedCustomersByHeaderID = DeserializeSelectedCustomersByHeaderID(c.Customers, c.PriceFileHeaderID, c.TradeListTemplate); return c; }).ToList();

                    objPFCSummaryInfo.Select(c => { c.Customers = DeserializeSelectedCustomer(c.Customers); return c; }).ToList();
                }
                      

                return objPFCSummaryInfo;
                
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("GetPFCSummaryInfoByUserSESA :" , ex);
                throw;
            }
        }

        public string DeserializeSelectedCustomer(string customersListJson)
        {
            List<SelectedCustomers> customers = JsonConvert.DeserializeObject<List<SelectedCustomers>>(customersListJson);

            string commaSeparatedCustomersString = string.Join(", ", customers.Select(c => $"{c.CustomerNumber} - {c.CustomerName}"));

            return commaSeparatedCustomersString.ToString();
        }
        public List<SelectedCustomersByHeaderID> DeserializeSelectedCustomersByHeaderID(string customersListJson,long PriceFileHeaderID,string FileName)
        {
            List<SelectedCustomersByHeaderID> lstPFCSelectedCustomersByHeaderID = new List<SelectedCustomersByHeaderID>();
            List<SelectedCustomers> lstcustomers = JsonConvert.DeserializeObject<List<SelectedCustomers>>(customersListJson);
          
            foreach (var SelectedCustomer in lstcustomers)
            {
                SelectedCustomersByHeaderID objPFCSelectedCustomersByHeaderID = new SelectedCustomersByHeaderID();
                objPFCSelectedCustomersByHeaderID.CustomerSNO = SelectedCustomer.CustomerSNO;
                objPFCSelectedCustomersByHeaderID.CustomerNumber= SelectedCustomer.CustomerNumber;
                objPFCSelectedCustomersByHeaderID.CustomerName = SelectedCustomer.CustomerName;
                objPFCSelectedCustomersByHeaderID.zKUNNR = SelectedCustomer.zKUNNR;
                objPFCSelectedCustomersByHeaderID.PC1 = SelectedCustomer.PC1;
                objPFCSelectedCustomersByHeaderID.PC2 = SelectedCustomer.PC2;
                objPFCSelectedCustomersByHeaderID.PC3 = SelectedCustomer.PC3;
                objPFCSelectedCustomersByHeaderID.PriceFileHeaderID = PriceFileHeaderID;
                objPFCSelectedCustomersByHeaderID.PFCZipFileName = FileName;
                lstPFCSelectedCustomersByHeaderID.Add(objPFCSelectedCustomersByHeaderID);

            }

            

            return lstPFCSelectedCustomersByHeaderID;
        }

        public List<PriceFileLocationDetails> PriceFileLocationInfoByHeaderID(long PriceFileHeaderID)
        {
            try
            {
                List<PriceFileLocationDetails> lstPriceFileLocationDetails = _objPriceListRepository.PriceFileLocationDetailsRepository.GetManyQueryable(x => x.PriceFileHeaderID == PriceFileHeaderID && x.IsActive == true).ToList();
                return lstPriceFileLocationDetails;

            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Error while PriceFileLocationInfoByHeaderID ", ex);
                throw;
            }
        }


        public dynamic GetAppSettingByName(string ConfigName)
        {
            try
            {                
                return GetorSaveAppSettingsIntoFromSession(ConfigName);
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Error while GetAppSettingByName ", ex);
                throw;
            }
        }

        public bool IsValidUser(string UserSESA)
        {
            try
            {
                using (ISqlHelper objSqlHelper = new SqlHelper())
                {
                    string strQuery = "select top 1 1 from dbo.MST_UserMaster where UserSESA = @UserSESA and IsActive = @IsActive";
                    List<SqlParameter> lstSqlParameters = new List<SqlParameter>();

                    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@UserSESA", Value = UserSESA });
                    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@IsActive", Value = true });

                    var dtIsValidUser = objSqlHelper.ExecuteTable(CommandType.Text, strQuery, lstSqlParameters.ToArray());
                    if (dtIsValidUser != null && dtIsValidUser.Rows.Count > 0)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch(Exception ex)
            {
                _objLoggingProvider.LogException("IsValidUser ", ex);
                return false;
            }
        }



        public bool IsAdminUser(string UserSESA)
        {
            try
            {
                return false;
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("IsAdminUser ", ex);
                return false;
            }
        }

        private dynamic GetorSaveAppSettingsIntoFromSession(string ConfigName ="")
        {
            List<ApplicationSetting> lstApplicationSetting = new List<ApplicationSetting>();
            string strQuery = "select isnull(ConfigName,'') as Name, isnull(ConfigValue,'') as Value, isnull(ConfigDataType,'') as DataType from [dbo].[MST_AppConfig] where isnull(IsActive,0) =1 and isnull(ConfigName,'') != '' ";
            DataTable dtAppConfigSetting;

            using (ISqlHelper objSqlHelper = new SqlHelper(AppConfig.ConnectionString))
            {
                dtAppConfigSetting = objSqlHelper.ExecuteTable(CommandType.Text, strQuery);
                if (dtAppConfigSetting != null)
                {
                    for (var i = 0; i < dtAppConfigSetting.Rows.Count; i++)
                    {
                        ApplicationSetting AS = new ApplicationSetting();
                        AS.Name = dtAppConfigSetting.Rows[i]["Name"].ToString();
                        AS.Value = dtAppConfigSetting.Rows[i]["Value"].ToString();
                        AS.DataType = dtAppConfigSetting.Rows[i]["DataType"].ToString();
                        lstApplicationSetting.Add(AS);
                    }

                }

            }
            var jsonApplicationSetting = JsonConvert.SerializeObject(lstApplicationSetting);
            
            if (ConfigName != "")
            {
                ApplicationSetting objApplicationSetting = lstApplicationSetting.Where(x => x.Name.ToUpper() == ConfigName.ToUpper()).FirstOrDefault();
                if (objApplicationSetting != null)
                {
                    return objApplicationSetting.Value;

                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }


        }

        public  string GetNotificationTemplateNameByOrg(string OrgName)
        {
            string templateName = string.Empty;
            if(OrgName != "")
            {
                templateName = OrgName == "AU01" ? "AU01PriceFileDistribution" : "NZ01PriceFileDistribution";

            }
            return templateName;
        }

        public NotificationTemplates GetPriceFileAPINotificationTemplate(string TemplateName)
        {
            return _objCommonRepository.NotificationTemplatesRepository
                               .GetManyQueryable(s => s.TemplateName.ToString().ToUpper() == TemplateName.ToString().ToUpper())
                               .FirstOrDefault();
        }





        #region Dispose

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="isDispose"></param>
        private void Dispose(bool isDispose)
        {
            if (_objLoggingProvider != null && isDispose)
            {
                _objLoggingProvider.Dispose();
            }
            if (_objCommonRepository != null && isDispose)
            {
                _objCommonRepository.Dispose();
            }
            if (_objConfigureRepository != null && isDispose)
            {
                _objConfigureRepository.Dispose();
            }
            if (_objPriceListRepository != null && isDispose)
            {
                _objPriceListRepository.Dispose();
            }
            
        }

        #endregion
    }
}
