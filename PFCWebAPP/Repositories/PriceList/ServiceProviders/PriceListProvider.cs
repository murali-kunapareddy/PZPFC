
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;
using PFCWebAPP.DatabaseContext.Models.CustomTables;
using PFCWebAPP.Repositories.Common.Enums;
using PFCWebAPP.Repositories.Common.Interfaces;
using PFCWebAPP.Repositories.Common.ServiceProviders;
using PFCWebAPP.Repositories.Configure;
using PFCWebAPP.Repositories.PriceList.Interfaces;
using PFCWebAPP.Repositories.PriceList.Models;
using PFCWebAPP.Repositories.PriceList.Models.IntermediateModels;
using PFCWebAPP.Repositories.PriceList.Models.Masters;
using PFCWebAPP.Utilities;
using System.Data;
using System.Drawing;
using System.Reflection;
using PFCWebAPP.Repositories.Common.Models;
using NPOI.SS.Formula.Functions;
using System.Text.RegularExpressions;
using System.Globalization;
using Humanizer;
using System.Transactions;
using SE.CA.PingComponent.Entities;
using PFCWebAPP.Repositories.PriceList.Models.API;
using Microsoft.AspNetCore.Http;

namespace PFCWebAPP.Repositories.PriceList.ServiceProviders
{
    public class PriceListProvider : IPriceListProvider
    {

        #region Private member variables...

        private readonly ILoggingProvider _objLoggingProvider;
        private readonly IPriceListRepository _objPriceListRepository;
        private readonly IHttpContextAccessor _objHttpContextAccessor;
        public readonly ICommonProvider _objCommonProvider;
        private readonly IConfigureRepository _objConfigureRepository;
        private readonly INotificationProvider _objNotificationProvider;

        #endregion

        public PriceListProvider(ILoggingProvider objLoggingProvider, IPriceListRepository objPriceListRepository, IConfigureRepository objConfigureRepository, IHttpContextAccessor objHttpContextAccessor, ICommonProvider objCommonProvider, INotificationProvider objNotificationProvider)
        {
            _objLoggingProvider = objLoggingProvider;
            _objPriceListRepository = objPriceListRepository;
            _objHttpContextAccessor = objHttpContextAccessor;
            _objCommonProvider = objCommonProvider;
            _objConfigureRepository = objConfigureRepository;
            _objNotificationProvider = objNotificationProvider;
        }


        #region  Countries dropdown ....

        /// <summary>
        /// GetCountryList
        /// </summary>
        /// <param></param>
        /// <returns>Returns List of Country list</returns>

        public List<CountryListOutput> GetCountryList()
        {
            try
            {
                var lst_Ctry = _objPriceListRepository.A507Repository.GetManyQueryable();
                var countryLst = (lst_Ctry
                                  .Where(item => item.Kappl.Equals("V") && item.Kschl.Equals("ZPR0"))
                                  .GroupBy(item => new { item.Vkorg, item.Vtweg, item.Spart })
                                  .Select(a => new CountryListOutput
                                  {
                                      VKORG = a.Key.Vkorg,
                                      VTWEG = a.Key.Vtweg,
                                      SPART = a.Key.Spart
                                  })
                                  .OrderBy(a => a.VKORG)
                                  ).ToList();
                return countryLst;

            }
            catch
            {
                throw;
            }

        }



        #endregion

        #region Customers list based on Org

        public List<CustomersListOutput> GetCustomerByCountry(string ctryCode)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: fetch Customers by country start");
                var get_kna1 = _objPriceListRepository.Kna1Repository.GetManyQueryable();
                var get_knvv = _objPriceListRepository.KnvvRepository.GetManyQueryable();
                var lst_Ctry = _objPriceListRepository.A507Repository.GetManyQueryable();
                var get_a507 = (lst_Ctry
                                  .Where(item => item.Kappl.Equals("V") && item.Kschl.Equals("ZPR0"))
                                  .GroupBy(item => new { item.Vkorg, item.Vtweg, item.Spart })
                                  .Select(a => new CountryListOutput
                                  {
                                      VKORG = a.Key.Vkorg,
                                      VTWEG = a.Key.Vtweg,
                                      SPART = a.Key.Spart
                                  })
                                  .OrderBy(a => a.VKORG)
                                  );

                var custLst = (from kn1 in get_kna1
                               join k in get_knvv on kn1.Kunnr equals k.Kunnr
                               //from knv in res.DefaultIfEmpty()
                               join a in get_a507 on k.Spart equals a.SPART into res1
                               from actry in res1.DefaultIfEmpty()
                               where k.Vkorg == ctryCode && k.Vtweg == "OG"
                               group actry by new { k.Vkorg, kn1.Kunnr, kn1.Land1, kn1.Name1, k.Kvgr1, k.Kvgr2, k.Kvgr3 } into g
                               select new CustomersListOutput
                               {
                                   VKORG = g.Key.Vkorg,
                                   KUNNR = g.Key.Kunnr.TrimStart('0'),
                                   LAND1 = g.Key.Land1,
                                   NAME1 = g.Key.Name1,
                                   xVKORG = g.Key.Kunnr,
                                   KVGR1 = g.Key.Kvgr1,
                                   KVGR2 = g.Key.Kvgr2,
                                   KVGR3 = g.Key.Kvgr3,
                                   Organization = ctryCode
                               }).ToList();

                _objLoggingProvider.LogMessage(LogType.Info, "end: fetch Customers by country end");

                var finalres = custLst.Where(c => !c.NAME1.StartsWith('.') && !c.NAME1.ToLower().Contains("DO NOT".ToLower()) && c.VKORG != null).ToList();

                return finalres;

            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Error while fetch Customers by country ", ex);
                throw;
            }

        }
        public List<CustomersListOutput> GetCustomerByCountryV2(string ctryCode)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: fetch Customers by country");
                                
                var kna1Query = _objPriceListRepository.Kna1Repository.GetManyQueryable().AsNoTracking();
                var knvvQuery = _objPriceListRepository.KnvvRepository.GetManyQueryable().AsNoTracking();
                var a507Query = _objPriceListRepository.A507Repository.GetManyQueryable()
                    .AsNoTracking()
                    .Where(item => item.Kappl == "V" && item.Kschl == "ZPR0")
                    .GroupBy(item => new { item.Vkorg, item.Vtweg, item.Spart })
                    .Select(group => new CountryListOutput
                    {
                        VKORG = group.Key.Vkorg,
                        VTWEG = group.Key.Vtweg,
                        SPART = group.Key.Spart
                    });


                var customerQuery = from kn1 in kna1Query
                                    join knvv in knvvQuery on kn1.Kunnr equals knvv.Kunnr
                                    join a507 in a507Query on knvv.Spart equals a507.SPART into a507Group
                                    from a507Item in a507Group.DefaultIfEmpty()
                                    where knvv.Vkorg == ctryCode && knvv.Vtweg == "OG"
                                    group a507Item by new
                                    {
                                        knvv.Vkorg,
                                        kn1.Kunnr,
                                        kn1.Land1,
                                        kn1.Name1,
                                        knvv.Kvgr1,
                                        knvv.Kvgr2,
                                        knvv.Kvgr3
                                    } into grouped
                                    select new CustomersListOutput
                                    {
                                        VKORG = grouped.Key.Vkorg,
                                        KUNNR = grouped.Key.Kunnr.TrimStart('0'),
                                        LAND1 = grouped.Key.Land1,
                                        NAME1 = grouped.Key.Name1,
                                        xVKORG = grouped.Key.Kunnr,
                                        KVGR1 = grouped.Key.Kvgr1,
                                        KVGR2 = grouped.Key.Kvgr2,
                                        KVGR3 = grouped.Key.Kvgr3,
                                        Organization = ctryCode
                                    };

                var finalResult = customerQuery
                    .AsEnumerable()
                    .Where(c => !c.NAME1.StartsWith('.') &&
                                !c.NAME1.Contains("DO NOT", StringComparison.OrdinalIgnoreCase) &&
                                c.VKORG != null)
                    .ToList();
                
                _objLoggingProvider.LogMessage(LogType.Info, "End: fetch Customers by country");

                return finalResult;
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Error while fetching Customers by country", ex);
                throw;
            }
        }



        public List<CustomersListOutput> GetCustomerByCustomerList(string ctryCode, string[] custList)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: fetch Customers by Customer List");
                var get_kna1 = _objPriceListRepository.Kna1Repository.GetManyQueryable();
                var get_knvv = _objPriceListRepository.KnvvRepository.GetManyQueryable();
                var lst_Ctry = _objPriceListRepository.A507Repository.GetManyQueryable();
                var get_a507 = (lst_Ctry
                                  .Where(item => item.Kappl.Equals("V") && item.Kschl.Equals("ZPR0"))
                                  .GroupBy(item => new { item.Vkorg, item.Vtweg, item.Spart })
                                  .Select(a => new CountryListOutput
                                  {
                                      VKORG = a.Key.Vkorg,
                                      VTWEG = a.Key.Vtweg,
                                      SPART = a.Key.Spart
                                  })
                                  .OrderBy(a => a.VKORG)
                                  );

                var custLst = (from kn1 in get_kna1
                               join k in get_knvv on kn1.Kunnr equals k.Kunnr
                               //from knv in res.DefaultIfEmpty()
                               join a in get_a507 on k.Spart equals a.SPART into res1
                               from actry in res1.DefaultIfEmpty()
                               where k.Vkorg == ctryCode && k.Vtweg == "OG" && custList.Contains(kn1.Kunnr.TrimStart('0'))
                               group actry by new { k.Vkorg, kn1.Kunnr, kn1.Land1, kn1.Name1, k.Kvgr1, k.Kvgr2, k.Kvgr3 } into g
                               select new CustomersListOutput
                               {
                                   VKORG = g.Key.Vkorg,
                                   KUNNR = g.Key.Kunnr.TrimStart('0'),
                                   LAND1 = g.Key.Land1,
                                   NAME1 = g.Key.Name1,
                                   xVKORG = g.Key.Kunnr,
                                   KVGR1 = g.Key.Kvgr1,
                                   KVGR2 = g.Key.Kvgr2,
                                   KVGR3 = g.Key.Kvgr3,
                                   Organization = ctryCode
                               }).ToList();

                _objLoggingProvider.LogMessage(LogType.Info, "end: fetch Customers by country end");

                var finalres = custLst.Where(c => !c.NAME1.StartsWith('.') && !c.NAME1.ToLower().Contains("DO NOT".ToLower()) && c.VKORG != null).ToList();

                return finalres;

            }
            catch
            {
                throw;
            }

        }

        #endregion

        public List<DiscountParameters> GetDiscountsByCountry()
        {
            try
            {
                List<DiscountParameters> lstDiscountParameters = new List<DiscountParameters>();
                var temp_mstr = _objConfigureRepository.TemplateMasterRepository.GetQueryable(s => s.TemplateName == "DiscountParameters").FirstOrDefault();
                var temp_data = _objConfigureRepository.TemplateDataRepository.GetManyQueryable();
                var Discount_json = temp_data
                                .Where(s => s.TemplateMasterID == temp_mstr.TemplateMasterID)
                                .Select(a => a.Data).FirstOrDefault();
                if (Discount_json != null)
                {
                    lstDiscountParameters = JsonConvert.DeserializeObject<List<DiscountParameters>>(Discount_json);
                }
                return lstDiscountParameters;
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Error while GetDiscountsByCountry ", ex);
                throw;
            }
        }


        #region Template Formats ........

        public List<TradeTemplate> GetTradeListTemplate(string CtryName)
        {
            try
            {
                var CountryCode = string.Empty;
                CountryCode = CtryName == "AU01" ? "AU" : "NZ";
                var trade_templ = (from tdetails in _objConfigureRepository.TemplateDataRepository.GetManyQueryable()
                                   join tmaster in _objConfigureRepository.TemplateMasterRepository.GetManyQueryable()
                                   on tdetails.TemplateMasterID equals tmaster.TemplateMasterID
                                   where tmaster.IsActive == true && tmaster.CountryCode == CountryCode && tmaster.TemplateCategoryID == 1
                                   select new TradeTemplate
                                   {
                                       TemplateMasterID = tmaster.TemplateMasterID,
                                       TemplateName = tmaster.TemplateName,
                                       AliasName = tmaster.AliasName
                                   }
                             ).ToList();

                //var trade_templ = _objConfigureRepository.TemplateMasterRepository
                //    .GetAllEntities(s => s.CountryCode == CountryCode && s.TemplateCategoryID == 1 && s.IsActive == true && s.IsDeleted == false)
                //    .Select(sl => new TradeTemplate
                //    {
                //        TemplateMasterID = sl.TemplateMasterID,
                //        TemplateName = sl.TemplateName,
                //        AliasName = sl.AliasName
                //    }).ToList();

                return trade_templ;




            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Error while GetTradeListTemplate ", ex);
                throw;
            }
        }

        public List<TradeTemplate> GetCustomerListTemplate(string CtryName)
        {
            try
            {
                var CountryCode = string.Empty;
                CountryCode = CtryName == "AU01" ? "AU" : "NZ";
                var trade_templ = (from tdetails in _objConfigureRepository.TemplateDataRepository.GetManyQueryable()
                                   join tmaster in _objConfigureRepository.TemplateMasterRepository.GetManyQueryable()
                                   on tdetails.TemplateMasterID equals tmaster.TemplateMasterID
                                   where tmaster.IsActive == true && tmaster.CountryCode == CountryCode && tmaster.TemplateCategoryID == 3
                                   select new TradeTemplate
                                   {
                                       TemplateMasterID = tmaster.TemplateMasterID,
                                       TemplateName = tmaster.TemplateName,
                                       AliasName = tmaster.AliasName
                                   }
                             ).ToList();

                return trade_templ;
            }
            catch
            {
                throw;
            }
        }

        public List<TradeTemplateOutputFormate> GetTradeListOutputFormate(string CtryName)
        {
            try
            {
                var CountryCode = string.Empty;

                CountryCode = CtryName == "AU01" ? "AU" : "NZ";

                var trade_templ = _objConfigureRepository.ReportFormatMasterRepository
                    .GetAllEntities(s => s.CountryCode == CountryCode && s.IsActive == true && s.IsDeleted == false)
                    .Select(sl => new TradeTemplateOutputFormate
                    {
                        ReportFormatMasterID = sl.ReportFormatMasterID,
                        FormatName = sl.FormatName,
                        AliasName = sl.AliasName
                    }).ToList();

                return trade_templ;
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Error while GetTradeListOutputFormate ", ex);
                throw;
            }
        }

        #endregion

        public int UserPriceFileSaveConfig(PriceFileSaveConfig UserFileConfig)
        {
            try
            {
                string UserSESA = _objCommonProvider.GetLoginUserSESA();
                var userconf = _objConfigureRepository.UserConfigSettingRepository
                                .GetQueryable(s => s.UserSESA == UserSESA)
                                .OrderByDescending(s => s.CreatedDate)
                                .FirstOrDefault();
                if (userconf != null)
                {
                    userconf.IsActive = false;
                    userconf.IsDeleted = true;
                    _objConfigureRepository.UserConfigSettingRepository.UpdateEntity(userconf);

                }

                UserConfigSetting confObj = new UserConfigSetting();
                confObj.UserSESA = UserSESA;
                confObj.SalesOrganization = UserFileConfig.SalesOrganization;
                confObj.SelectedCustomers = UserFileConfig.SelectedCustomers;
                confObj.PricesActiveDate = UserFileConfig.PricesActiveDate;
                confObj.CanUseAutoReportContent = UserFileConfig.CanUseAutoReportContent;
                confObj.ReportContentTemplateID = UserFileConfig.ReportContentTemplateID;
                confObj.ReportFormatTemplateID = UserFileConfig.ReportFormatTemplateID;
                confObj.CanIncludeTradePrices = UserFileConfig.CanIncludeTradePrices;
                confObj.CanIncludeCustomerNetPrices = UserFileConfig.CanIncludeCustomerNetPrices;
                confObj.CanIncludeCustomerHierarchyNetPrices = UserFileConfig.CanIncludeCustomerHierarchyNetPrices;
                confObj.CanIncludeOverallNetPrices = UserFileConfig.CanIncludeOverallNetPrices;
                confObj.CanIncludePriceGroupNets = UserFileConfig.CanIncludePriceGroupNets;
                confObj.CanIncludeSellOffPrices = UserFileConfig.CanIncludeSellOffPrices;
                confObj.CanIncludeDiscount1 = UserFileConfig.CanIncludeDiscount1;
                confObj.CanIncludeDiscount2 = UserFileConfig.CanIncludeDiscount2;
                confObj.CanIncludeDiscount3 = UserFileConfig.CanIncludeDiscount3;
                confObj.CanIncludeDiscount4 = UserFileConfig.CanIncludeDiscount4;
                confObj.CanIncludeDiscount5 = UserFileConfig.CanIncludeDiscount5;
                confObj.CanIncludeDiscount6 = UserFileConfig.CanIncludeDiscount6;
                confObj.CanIncludeDiscount7 = UserFileConfig.CanIncludeDiscount7;
                confObj.CanIncludeDiscount8 = UserFileConfig.CanIncludeDiscount8;
                confObj.CanIncludePromoPrice = UserFileConfig.CanIncludePromoPrice;
                confObj.CanUseShiftBreaks = UserFileConfig.CanUseShiftBreaks;
                confObj.CanUseMOQAsBrk1 = UserFileConfig.CanUseMOQAsBrk1;
                confObj.CanUseGlobalCOSForProductHierarchy = UserFileConfig.CanUseGlobalCOSForProductHierarchy;
                confObj.CanUseLocalCOSForProductHierarchy = UserFileConfig.CanUseLocalCOSForProductHierarchy;
                confObj.CanAddSODInFinalPrice = UserFileConfig.CanAddSODInFinalPrice;
                confObj.SODInFinalPriceValue = (float)UserFileConfig.SODInFinalPriceValue;
                confObj.CanUseAlternateValidFromDate = UserFileConfig.CanUseAlternateValidFromDate;
                confObj.AlternateValidFromDate = UserFileConfig.AlternateValidFromDate;
                confObj.CanShowTemplateMaterialOnly = UserFileConfig.CanShowTemplateMaterialOnly;
                confObj.CreatedBy = UserSESA;
                confObj.CreatedDate = DateTime.UtcNow;
                confObj.CanSendEmail = UserFileConfig.SendEmail;
                confObj.CanShowNotFoundTemplateMaterials = UserFileConfig.ShowNotFoundTemplateMaterials;
                confObj.CanIncludeProductHierarchyOverride = UserFileConfig.CanIncludeProductHierarchyOverride;
                _objConfigureRepository.UserConfigSettingRepository.InsertEntity(confObj);
                var id = Convert.ToInt32(confObj.UserConfigSettingID);
                return id;

            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Error while UserPriceFileSaveConfig ", ex);
                throw;
            }
        }


        public SelectedUserConfigSetting GetPriceFileSaveConfig()
        {
            try
            {
                string UserSESA = _objCommonProvider.GetLoginUserSESA();
                var userConfig = _objConfigureRepository.UserConfigSettingRepository.GetQueryable(x => x.IsActive == true && x.UserSESA == UserSESA)
                                .FirstOrDefault();
                if (userConfig != null)
                {
                    var selected_cust = JsonConvert.DeserializeObject<List<SelectedCustomers>>(userConfig.SelectedCustomers);

                    var data = _objConfigureRepository.UserConfigSettingRepository.GetQueryable(x => x.SelectedCustomers != "" && x.UserSESA == UserSESA && x.IsActive == true)
                                .Select(s => new SelectedUserConfigSetting
                                {
                                    SalesOrganization = s.SalesOrganization,
                                    SelectedCustomers = selected_cust,
                                    PricesActiveDate = s.PricesActiveDate,
                                    CanUseAutoReportContent = s.CanUseAutoReportContent,
                                    ReportContentTemplateID = s.ReportContentTemplateID,
                                    ReportFormatTemplateID = s.ReportFormatTemplateID,
                                    CanIncludeTradePrices = s.CanIncludeTradePrices,
                                    CanIncludeCustomerNetPrices = s.CanIncludeCustomerNetPrices,
                                    CanIncludeCustomerHierarchyNetPrices = s.CanIncludeCustomerHierarchyNetPrices,
                                    CanIncludeOverallNetPrices = s.CanIncludeOverallNetPrices,
                                    CanIncludePriceGroupNets = s.CanIncludePriceGroupNets,
                                    CanIncludeSellOffPrices = s.CanIncludeSellOffPrices,
                                    CanIncludeDiscount1 = s.CanIncludeDiscount1,
                                    CanIncludeDiscount2 = s.CanIncludeDiscount2,
                                    CanIncludeDiscount3 = s.CanIncludeDiscount3,
                                    CanIncludeDiscount4 = s.CanIncludeDiscount4,
                                    CanIncludeDiscount5 = s.CanIncludeDiscount5,
                                    CanIncludeDiscount6 = s.CanIncludeDiscount6,
                                    CanIncludeDiscount7 = s.CanIncludeDiscount7,
                                    CanIncludeDiscount8 = s.CanIncludeDiscount8,
                                    CanIncludePromoPrice = s.CanIncludePromoPrice,
                                    CanUseShiftBreaks = s.CanUseShiftBreaks,
                                    CanUseMOQAsBrk1 = s.CanUseMOQAsBrk1,
                                    CanUseGlobalCOSForProductHierarchy = s.CanUseGlobalCOSForProductHierarchy,
                                    CanUseLocalCOSForProductHierarchy = s.CanUseLocalCOSForProductHierarchy,
                                    CanAddSODInFinalPrice = s.CanAddSODInFinalPrice,
                                    SODInFinalPriceValue = Convert.ToSingle(s.SODInFinalPriceValue),
                                    CanUseAlternateValidFromDate = s.CanUseAlternateValidFromDate,
                                    AlternateValidFromDate = s.AlternateValidFromDate,
                                    CanShowTemplateMaterialOnly = s.CanShowTemplateMaterialOnly,
                                    CanIncludeProductHierarchyOverride = s.CanIncludeProductHierarchyOverride

                                }).FirstOrDefault();
                    return data;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Error while GetPriceFileSaveConfig ", ex);
                throw;
            }
        }

        public SelectedUserConfigSetting GetPriceFileSaveConfigV2()
        {
            try
            {
                string userSESA = _objCommonProvider.GetLoginUserSESA();

                var userConfig = _objConfigureRepository.UserConfigSettingRepository
                    .GetQueryable(x => x.IsActive && x.UserSESA == userSESA && !string.IsNullOrEmpty(x.SelectedCustomers))
                    .AsNoTracking()
                    .FirstOrDefault();

                if (userConfig == null)
                    return null;

                var selectedCustomers = string.IsNullOrWhiteSpace(userConfig.SelectedCustomers)
                    ? new List<SelectedCustomers>()
                    : JsonConvert.DeserializeObject<List<SelectedCustomers>>(userConfig.SelectedCustomers);

                return new SelectedUserConfigSetting
                {
                    SalesOrganization = userConfig.SalesOrganization,
                    SelectedCustomers = selectedCustomers,
                    PricesActiveDate = userConfig.PricesActiveDate,
                    CanUseAutoReportContent = userConfig.CanUseAutoReportContent,
                    ReportContentTemplateID = userConfig.ReportContentTemplateID,
                    ReportFormatTemplateID = userConfig.ReportFormatTemplateID,
                    CanIncludeTradePrices = userConfig.CanIncludeTradePrices,
                    CanIncludeCustomerNetPrices = userConfig.CanIncludeCustomerNetPrices,
                    CanIncludeCustomerHierarchyNetPrices = userConfig.CanIncludeCustomerHierarchyNetPrices,
                    CanIncludeOverallNetPrices = userConfig.CanIncludeOverallNetPrices,
                    CanIncludePriceGroupNets = userConfig.CanIncludePriceGroupNets,
                    CanIncludeSellOffPrices = userConfig.CanIncludeSellOffPrices,
                    CanIncludeDiscount1 = userConfig.CanIncludeDiscount1,
                    CanIncludeDiscount2 = userConfig.CanIncludeDiscount2,
                    CanIncludeDiscount3 = userConfig.CanIncludeDiscount3,
                    CanIncludeDiscount4 = userConfig.CanIncludeDiscount4,
                    CanIncludeDiscount5 = userConfig.CanIncludeDiscount5,
                    CanIncludeDiscount6 = userConfig.CanIncludeDiscount6,
                    CanIncludeDiscount7 = userConfig.CanIncludeDiscount7,
                    CanIncludeDiscount8 = userConfig.CanIncludeDiscount8,
                    CanIncludePromoPrice = userConfig.CanIncludePromoPrice,
                    CanUseShiftBreaks = userConfig.CanUseShiftBreaks,
                    CanUseMOQAsBrk1 = userConfig.CanUseMOQAsBrk1,
                    CanUseGlobalCOSForProductHierarchy = userConfig.CanUseGlobalCOSForProductHierarchy,
                    CanUseLocalCOSForProductHierarchy = userConfig.CanUseLocalCOSForProductHierarchy,
                    CanAddSODInFinalPrice = userConfig.CanAddSODInFinalPrice,
                    SODInFinalPriceValue = Convert.ToSingle(userConfig.SODInFinalPriceValue),
                    CanUseAlternateValidFromDate = userConfig.CanUseAlternateValidFromDate,
                    AlternateValidFromDate = userConfig.AlternateValidFromDate,
                    CanShowTemplateMaterialOnly = userConfig.CanShowTemplateMaterialOnly,
                    CanIncludeProductHierarchyOverride = userConfig.CanIncludeProductHierarchyOverride
                };
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Error while GetPriceFileSaveConfig", ex);
                throw;
            }
        }


        public void GetExcelDetails(long id, bool SendEmail, bool showNotFoundMaterials)
        {

            string val = string.Empty;

            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "USPT_GeneratePriceFileCreation: Start with UserConfigSettingID: " + id);
                using (ISqlHelper objSqlHelper = new SqlHelper(AppConfig.ConnectionString))
                {
                    List<SqlParameter> lstSqlParameters = new List<SqlParameter>();
                    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@UserConfigSettingID", Value = id });
                    SqlParameter Result = new SqlParameter("@ResultFlag", SqlDbType.NVarChar, 250);
                    Result.Direction = ParameterDirection.Output;
                    lstSqlParameters.Add(Result);
                    objSqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "USPT_GeneratePriceFileCreation", lstSqlParameters.ToArray());
                    val = Result.Value.ToString();

                }
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Error while GetExcelDetails ", ex);
                using (ISqlHelper objSqlHelper = new SqlHelper(AppConfig.ConnectionString))
                {
                    string strUpdateQuery = "UPDATE dbo.TRN_PriceFileHeader SET Status = @Status, StatusText = @StatusText, PercentCompleted= @PercentCompleted  WHERE UserConfigSettingID = @UserConfigSettingID";

                    List<SqlParameter> lstParameters = new List<SqlParameter>();
                    lstParameters.Add(new SqlParameter() { ParameterName = "@StatusText", Value = "Failed due to some technical reason. Please Try Later. " });
                    lstParameters.Add(new SqlParameter() { ParameterName = "@PercentCompleted", Value = 0 });
                    lstParameters.Add(new SqlParameter() { ParameterName = "@Status", Value = "Failed" });
                    lstParameters.Add(new SqlParameter() { ParameterName = "@UserConfigSettingID", Value = id });

                    int output = objSqlHelper.ExecuteNonQuery(CommandType.Text, strUpdateQuery, lstParameters.ToArray());

                }
            }

            if (val == "Success")
            {
                DownloadExcelForCustomersPrices_V2(id, 0, "", "", SendEmail, showNotFoundMaterials);
            }

        }

        private void UpdatePriceFileLocationInfo(long PriceFileHeaderID, string CustomerNo, float PercentCompleted_Msg, string status_Text_Msg)
        {
            try
            {
                using (ISqlHelper objSqlHelper = new SqlHelper(AppConfig.ConnectionString))
                {
                    string strUpdateQuery = "UPDATE dbo.TRN_PriceFileLocationDetails SET StatusText = @StatusText, PercentCompleted= @PercentCompleted WHERE PriceFileHeaderID = @PriceFileHeaderID and CustomerNo = @CustomerNo ";

                    List<SqlParameter> lstSqlParameters = new List<SqlParameter>();
                    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@StatusText", Value = status_Text_Msg });
                    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@PercentCompleted", Value = PercentCompleted_Msg });
                    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@PriceFileHeaderID", Value = PriceFileHeaderID });
                    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@CustomerNo", Value = CustomerNo });

                    int output = objSqlHelper.ExecuteNonQuery(CommandType.Text, strUpdateQuery, lstSqlParameters.ToArray());
                    Console.WriteLine("updated query count");
                    Console.WriteLine(output);
                }
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Error while UpdatePriceFileLocationInfo ", ex);
                throw;
            }
        }

        private void UpdatePriceFileLocationInfoForDatabaseMode(long PriceFileHeaderID, string CustomerNo, float PercentCompleted_Msg, string status_Text_Msg)
        {
            try
            {
                using (ISqlHelper objSqlHelper = new SqlHelper(AppConfig.ConnectionString))
                {
                    string strUpdateQuery = "UPDATE dbo.TRN_PriceFileLocationDetails SET ReDownloadStatusText = @ReDownloadStatusText, ReDownloadPercentCompleted= @ReDownloadPercentCompleted WHERE PriceFileHeaderID = @PriceFileHeaderID and CustomerNo = @CustomerNo ";

                    List<SqlParameter> lstSqlParameters = new List<SqlParameter>();
                    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@ReDownloadStatusText", Value = status_Text_Msg });
                    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@ReDownloadPercentCompleted", Value = PercentCompleted_Msg });
                    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@PriceFileHeaderID", Value = PriceFileHeaderID });
                    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@CustomerNo", Value = CustomerNo });

                    int output = objSqlHelper.ExecuteNonQuery(CommandType.Text, strUpdateQuery, lstSqlParameters.ToArray());
                    Console.WriteLine("updated query count");
                    Console.WriteLine(output);
                }
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Error while UpdatePriceFileLocationInfo ", ex);
                throw;
            }
        }

        public ProcessBar GetGenerationStatus(long ConfigId)
        {
            try
            {
                ProcessBar rec = new();
                rec = _objPriceListRepository.PriceFileHeaderRepository
                            .GetQueryable(s => s.UserConfigSettingID == ConfigId && s.IsActive == true)
                            .Select(s => new ProcessBar
                            {
                                StatusPercentage = s.PercentCompleted,
                                StatusData = s.StatusText,
                                PriceFileHeaderID = s.PriceFileHeaderID,
                                Status = s.Status
                            }).FirstOrDefault();

                if (rec == null)
                {
                    ProcessBar objProcessBar = new ProcessBar();
                    objProcessBar.StatusPercentage = 15;
                    objProcessBar.StatusData = "processing Request";
                    return objProcessBar;
                }

                if (rec.StatusPercentage == 100 && rec.Status.ToLower() == "Completed".ToLower())
                {
                    var PriceFileLocationDetails = _objPriceListRepository.PriceFileLocationDetailsRepository
                                        .GetManyQueryable(a => a.IsActive == true && a.PriceFileHeaderID == rec.PriceFileHeaderID)
                                        .ToList();

                    var total_Cnt = PriceFileLocationDetails.Count();
                    var completed_Cnt = PriceFileLocationDetails.Where(x => x.PercentCompleted == 100).Count();

                    ProcessBar objProcessBar = new ProcessBar();
                    objProcessBar.ZipFileName = "Zip";
                    objProcessBar.PriceFileHeaderID = rec.PriceFileHeaderID;
                    if (completed_Cnt == total_Cnt)
                    {
                        objProcessBar.StatusPercentage = 100;
                        objProcessBar.StatusData = completed_Cnt + "/" + total_Cnt + " Generating Excel File Completed ";
                        objProcessBar.Status = "Completed";
                        var userConfig = _objConfigureRepository.UserConfigSettingRepository
                          .GetQueryable(x => x.UserConfigSettingID == ConfigId).FirstOrDefault();
                        if (userConfig != null)
                        {
                            var objTemplateMaster = _objConfigureRepository.TemplateMasterRepository
                               .GetQueryable(x => x.TemplateMasterID == userConfig.ReportContentTemplateID).FirstOrDefault();
                            if (objTemplateMaster != null)
                            {
                                objProcessBar.ZipFileName = objTemplateMaster.TemplateName;
                            }
                        }
                        _objLoggingProvider.LogMessage(LogType.Info, "ProcessBar Status:=" + objProcessBar.StatusData + " StatusPercentage Completed:=" + objProcessBar.StatusPercentage + " ,UserConfigSettingID:=" + ConfigId);
                        //return objProcessBar;
                    }
                    else
                    {
                        var current_Rec_Status = PriceFileLocationDetails.Where(x => x.PercentCompleted < 100).OrderByDescending(x => x.PercentCompleted).FirstOrDefault();
                        objProcessBar.StatusPercentage = current_Rec_Status.PercentCompleted;
                        objProcessBar.StatusData = completed_Cnt + 1 + "/" + total_Cnt + " Generating Excel File For " + current_Rec_Status.CustomerNo + " is " + current_Rec_Status.Status;
                        _objLoggingProvider.LogMessage(LogType.Info, "ProcessBar Status:=" + objProcessBar.StatusData + " StatusPercentage Complated:=" + objProcessBar.StatusPercentage + " ,UserConfigSettingID:=" + ConfigId);
                        //return objProcessBar;
                    }
                    return objProcessBar;
                }
                else if (rec.StatusPercentage != 100 && rec.Status.ToLower() == "Failed".ToLower())
                {
                    ProcessBar objProcessBar = new ProcessBar();
                    objProcessBar.StatusPercentage = 0;
                    objProcessBar.StatusData = "Failed due to some technical reason. Please Try Later.";
                    return objProcessBar;
                }
                else
                {
                    _objLoggingProvider.LogMessage(LogType.Info, "ProcessBar Status:=" + rec.StatusData + " StatusPercentage Complated:=" + rec.StatusPercentage + " ,UserConfigSettingID:=" + ConfigId);
                    return rec;
                }

            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Error while GetGenerationStatus ", ex);
                //throw; // while staus check not required to throw exception even if you get any exception
                ProcessBar objProcessBar = new ProcessBar();
                objProcessBar.StatusPercentage = 0;
                objProcessBar.StatusData = "Failed due to some technical reason. Please Try Later.";
                return objProcessBar;
            }
        }

        public ProcessBar GetStatusForIndividualFiles(long PriceFileHeaderId, string Sel_Customers, long ReDownloadCount)
        {
            List<PriceFileLocationDetails> PriceFileLocationDetails = new();
            if (Sel_Customers != "" || Sel_Customers == null)
            {
                var selected_cust = JsonConvert.DeserializeObject<List<string>>(Sel_Customers);
                var sel_cust = string.Join(",", selected_cust);
                PriceFileLocationDetails = _objPriceListRepository.PriceFileLocationDetailsRepository
                        .GetManyQueryable(a => a.IsActive == true && a.PriceFileHeaderID == PriceFileHeaderId).
                        Where(s => Sel_Customers.Contains(s.CustomerNo))
                        .ToList();
                var priceFileRedownloadcount = _objPriceListRepository.PriceFileLocationDetailsRepository
                        .GetManyQueryable(a => a.IsActive == true && a.PriceFileHeaderID == PriceFileHeaderId).
                        Where(s => Sel_Customers.Contains(s.CustomerNo))
                        .ToList();
                long reCountValue = priceFileRedownloadcount.Sum(s => s.ReDownloadCount);
                if (ReDownloadCount == reCountValue)
                {
                    using (ISqlHelper objSqlHelper = new SqlHelper(AppConfig.ConnectionString))
                    {
                        string strUpdateQuery = "UPDATE dbo.TRN_PriceFileLocationDetails SET ReDownloadStatus= @ReDownloadStatus, ReDownloadStatusText = @ReDownloadStatusText, ReDownloadPercentCompleted= @ReDownloadPercentCompleted, IsReDownloadCompleted=@IsReDownloadCompleted WHERE PriceFileHeaderID = @PriceFileHeaderID and CustomerNo IN (@CustomerNo) ";

                        List<SqlParameter> lstSqlParameters = new List<SqlParameter>();
                        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@ReDownloadStatus", Value = "" });
                        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@ReDownloadStatusText", Value = "" });
                        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@ReDownloadPercentCompleted", Value = 0 });
                        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@IsReDownloadCompleted", Value = 0 });
                        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@PriceFileHeaderID", Value = PriceFileHeaderId });
                        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@CustomerNo", Value = sel_cust });

                        int output = objSqlHelper.ExecuteNonQuery(CommandType.Text, strUpdateQuery, lstSqlParameters.ToArray());
                        Console.WriteLine("updated query count");
                        Console.WriteLine(output);
                    }
                }

                var total_Cnt = PriceFileLocationDetails.Count();
                var completed_Cnt = PriceFileLocationDetails.Where(x => x.ReDownloadPercentCompleted == 100).Count();
                bool checkstat = false;
                int statCount = 0;
                PriceFileLocationDetails.ForEach(s =>
                {
                    if (s.ReDownloadPercentCompleted == 100)
                    {
                        statCount++;
                    }
                });
                if (statCount == total_Cnt)
                {
                    checkstat = true;
                }
                else
                {
                    checkstat = false;
                }

                ProcessBar objProcessBar = new ProcessBar();
                objProcessBar.ZipFileName = "Zip";
                objProcessBar.PriceFileHeaderID = PriceFileHeaderId;
                if (checkstat)
                {
                    objProcessBar.StatusPercentage = 100;
                    objProcessBar.StatusData = completed_Cnt + "/" + total_Cnt + " Generating Excel File Completed ";
                    objProcessBar.Status = "Completed";
                    objProcessBar.IsCompleted = true;
                    _objLoggingProvider.LogMessage(LogType.Info, "ProcessBar Status:=" + objProcessBar.StatusData + " StatusPercentage Completed:=" + objProcessBar.StatusPercentage);
                }
                else
                {
                    var current_Rec_Status = PriceFileLocationDetails.Where(x => x.ReDownloadPercentCompleted < 100).OrderByDescending(x => x.ReDownloadPercentCompleted).FirstOrDefault();
                    objProcessBar.StatusPercentage = current_Rec_Status.PercentCompleted;
                    objProcessBar.StatusData = completed_Cnt + 1 + "/" + total_Cnt + " Generating Excel File For " + current_Rec_Status.CustomerNo + " is " + current_Rec_Status.Status;
                    objProcessBar.IsCompleted = false;
                    _objLoggingProvider.LogMessage(LogType.Info, "ProcessBar Status:=" + objProcessBar.StatusData + " StatusPercentage Complated:=" + objProcessBar.StatusPercentage);
                }
                return objProcessBar;
            }
            else
            {
                ProcessBar objProcessBar = new ProcessBar();
                objProcessBar.StatusPercentage = 0;
                objProcessBar.StatusData = "Failed";
                objProcessBar.ZipFileName = "Zip";
                objProcessBar.PriceFileHeaderID = PriceFileHeaderId;
                objProcessBar.IsCompleted = false;

                return objProcessBar;
            }

        }

        public IFont CreateHeaderFont(IWorkbook workbook)
        {
            IFont font = workbook.CreateFont();
            font.IsBold = true;
            font.FontHeightInPoints = 11;
            font.FontName = "Calibri";
            return font;
        }

        public ICellStyle CreateHeaderCellStyle(IWorkbook workbook)
        {
            ICellStyle cellStyle = workbook.CreateCellStyle();
            cellStyle.Alignment = HorizontalAlignment.Left;
            cellStyle.WrapText = true;
            return cellStyle;
        }

        public IFont CreateHeaderLastFont(IWorkbook workbook)
        {
            IFont font = workbook.CreateFont();
            font.IsBold = false;
            font.FontHeightInPoints = 8;
            font.FontName = "Calibri";
            return font;
        }

        public ICellStyle CreateHeaderLastCellStyle(IWorkbook workbook)
        {
            ICellStyle cellStyle = workbook.CreateCellStyle();
            cellStyle.Alignment = HorizontalAlignment.Left;
            cellStyle.WrapText = true;
            return cellStyle;
        }

        public ICellStyle CreateHeaderDarkCellStyle(IWorkbook workbook)
        {
            var color = new XSSFColor(new byte[] { 216, 228, 188 });
            var cellStyle = (XSSFCellStyle)workbook.CreateCellStyle();
            cellStyle.SetFillForegroundColor(color);
            cellStyle.Alignment = HorizontalAlignment.Left;
            cellStyle.FillPattern = FillPattern.SolidForeground;
            cellStyle.BorderBottom = BorderStyle.Medium;
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderTop = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;
            cellStyle.WrapText = true;
            return cellStyle;
        }
        public ICellStyle CreateHeaderLightCellStyle(IWorkbook workbook)
        {
            var color = new XSSFColor(new byte[] { 235, 241, 222 });
            var cellStyle = (XSSFCellStyle)workbook.CreateCellStyle();
            cellStyle.SetFillForegroundColor(color);
            cellStyle.Alignment = HorizontalAlignment.Left;
            cellStyle.FillPattern = FillPattern.SolidForeground;
            cellStyle.BorderBottom = BorderStyle.Medium;
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderTop = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;
            cellStyle.WrapText = true;
            return cellStyle;
        }
        public IFont CreateDataFont(IWorkbook workbook)
        {
            IFont font = workbook.CreateFont();
            font.IsBold = false;
            font.FontHeightInPoints = 11;
            return font;
        }

        public ICellStyle CreateDataCellStyle(IWorkbook workbook)
        {
            ICellStyle cellStyle = workbook.CreateCellStyle();
            cellStyle.Alignment = HorizontalAlignment.Left;
            cellStyle.BorderBottom = BorderStyle.Dotted;
            cellStyle.BorderLeft = BorderStyle.Dotted;
            cellStyle.BorderTop = BorderStyle.Dotted;
            cellStyle.BorderRight = BorderStyle.Dotted;
            IDataFormat textformat = workbook.CreateDataFormat();
            cellStyle.DataFormat = textformat.GetFormat("@");
            return cellStyle;
        }
        public ICellStyle CreateDataDigitCellStyle(IWorkbook workbook)
        {
            ICellStyle cellStyle = workbook.CreateCellStyle();
            cellStyle.Alignment = HorizontalAlignment.Right;
            cellStyle.BorderBottom = BorderStyle.Dotted;
            cellStyle.BorderLeft = BorderStyle.Dotted;
            cellStyle.BorderTop = BorderStyle.Dotted;
            cellStyle.BorderRight = BorderStyle.Dotted;
            IDataFormat textformat = workbook.CreateDataFormat();
            cellStyle.DataFormat = textformat.GetFormat("@");
            return cellStyle;
        }
        public ICellStyle CreateTextDataCellStyle(IWorkbook workbook)
        {
            ICellStyle cellStyle = workbook.CreateCellStyle();
            cellStyle.Alignment = HorizontalAlignment.Left;
            cellStyle.BorderBottom = BorderStyle.Dotted;
            cellStyle.BorderLeft = BorderStyle.Dotted;
            cellStyle.BorderTop = BorderStyle.Dotted;
            cellStyle.BorderRight = BorderStyle.Dotted;
            IDataFormat textformat = workbook.CreateDataFormat();
            cellStyle.DataFormat = textformat.GetFormat("@");
            return cellStyle;
        }
        public ICellStyle CreateTextDataValueCellStyle(IWorkbook workbook)
        {
            ICellStyle cellStyle = workbook.CreateCellStyle();
            cellStyle.Alignment = HorizontalAlignment.Right;
            cellStyle.BorderBottom = BorderStyle.Dotted;
            cellStyle.BorderLeft = BorderStyle.Dotted;
            cellStyle.BorderTop = BorderStyle.Dotted;
            cellStyle.BorderRight = BorderStyle.Dotted;
            IDataFormat textformat = workbook.CreateDataFormat();
            cellStyle.DataFormat = textformat.GetFormat("@");
            return cellStyle;
        }
        public ICellStyle CreateDataDarkCellStyle(IWorkbook workbook)
        {
            var color = new XSSFColor(new byte[] { 216, 228, 188 });
            var cellStyle = (XSSFCellStyle)workbook.CreateCellStyle();
            cellStyle.SetFillForegroundColor(color);
            cellStyle.Alignment = HorizontalAlignment.Right;
            cellStyle.FillPattern = FillPattern.SolidForeground;
            cellStyle.BorderBottom = BorderStyle.Dotted;
            cellStyle.BorderLeft = BorderStyle.Dotted;
            cellStyle.BorderTop = BorderStyle.Dotted;
            cellStyle.BorderRight = BorderStyle.Dotted;
            IDataFormat textformat = workbook.CreateDataFormat();
            cellStyle.DataFormat = textformat.GetFormat("@");
            return cellStyle;
        }
        public ICellStyle CreateDataLightCellStyle(IWorkbook workbook)
        {
            var color = new XSSFColor(new byte[] { 235, 241, 222 });
            var cellStyle = (XSSFCellStyle)workbook.CreateCellStyle();
            cellStyle.SetFillForegroundColor(color);
            cellStyle.Alignment = HorizontalAlignment.Right;
            cellStyle.FillPattern = FillPattern.SolidForeground;
            cellStyle.BorderBottom = BorderStyle.Dotted;
            cellStyle.BorderLeft = BorderStyle.Dotted;
            cellStyle.BorderTop = BorderStyle.Dotted;
            cellStyle.BorderRight = BorderStyle.Dotted;
            IDataFormat textformat = workbook.CreateDataFormat();
            cellStyle.DataFormat = textformat.GetFormat("@");
            return cellStyle;
        }
        public static void RemoveCellComment(ICell cell)
        {
            if (cell.CellComment != null)
            {
                cell.CellComment = null;
            }
        }

        // parallel processing V2
        public List<Dictionary<string, string>> DownloadExcelForCustomersPrices_V2(long ConfigId, long Param_PriceFileHeaderId = 0, string ConfigCustomers = "", string ArchivedMode = "", bool SendEmail = false, bool showNotFoundMaterials = false)
        {
            string ArchivedRecords = _objCommonProvider.GetAppSettingByName(Constants.ArchivedFileLocationMode);
            List<SelectedCustomers> selected_cust = new();
            UserConfigSetting userConfig = new();
            List<ReportFormatCustom> totres = new();
            long Stored_priceFileHeaderId = 0;
            List<Dictionary<string, string>> createdFileNames = new();
            List<SelectedCustomersByHeaderID> emailListCustomerWithPFId = new();

            var priceHeader = _objPriceListRepository.PriceFileHeaderRepository
            .GetQueryable(a => a.UserConfigSettingID == ConfigId && a.IsCompleted == true && a.IsActive == true).FirstOrDefault();
            try
            {

                _objLoggingProvider.LogMessage(LogType.Info, "PriceFileExcelCreationStart: DownloadExcelForCustomersPrices_V2 with UserConfigSettingID: " + ConfigId);
                if (ConfigId != 0 && Param_PriceFileHeaderId == 0)
                {

                    userConfig = _objConfigureRepository.UserConfigSettingRepository.GetQueryable(x => x.SelectedCustomers != "" && x.UserConfigSettingID == ConfigId).FirstOrDefault();
                    selected_cust = JsonConvert.DeserializeObject<List<SelectedCustomers>>(userConfig.SelectedCustomers);

                    var rfmaster = _objConfigureRepository.ReportFormatMasterRepository
                                .GetQueryable(a => a.ReportFormatMasterID == userConfig.ReportFormatTemplateID && a.IsActive == true);
                    var rfmastermapping = _objConfigureRepository.ReportFormatFieldMappingRepository
                                    .GetManyQueryable(x => x.IsActive == true);
                    var rffieldmaster = _objConfigureRepository.ReportFormatFieldMasterRepository.GetQueryable(s => s.IsActive == true);

                    _objLoggingProvider.LogMessage(LogType.Info, "ReportFormatFieldMaster Details: Start using  UserConfigSettingID: " + ConfigId);
                    totres = (from rfm in rfmaster
                              join rfmap in rfmastermapping on rfm.ReportFormatMasterID equals rfmap.ReportFormatMasterID
                              join rffm in rffieldmaster on rfmap.ReportFormatFieldMasterID equals rffm.ReportFormatFieldMasterID
                              where rfmap.ReportFormatMasterID == userConfig.ReportFormatTemplateID
                              select (new ReportFormatCustom
                              {
                                  ReportFormatMasterId = rfm.ReportFormatMasterID,
                                  ReportFormatName = rfm.FormatName,
                                  ReportFormatAliasName = rfm.AliasName,
                                  FormatFieldMasterID = rfmap.ReportFormatFieldMasterID,
                                  ReportFieldAliasName = rfmap.AliasName,
                                  ReportFieldSequence = rfmap.SequenceNo,
                                  ReportFieldMasterFieldName = rffm.FieldName,
                                  ReportFieldMasterDescription = rffm.FieldDescription,
                                  ReportFormatFieldMasterDataType = rffm.DataType,
                              })
                               ).OrderBy(a=>a.ReportFieldSequence).ToList();
                    _objLoggingProvider.LogMessage(LogType.Info, "ReportFormatFieldMaster Details: End");

                    Stored_priceFileHeaderId = priceHeader.PriceFileHeaderID;
                }
                else if (ConfigId != 0 && Param_PriceFileHeaderId != 0)
                {
                    ProcessBar ss = new ProcessBar();
                    ss.Status = "Success";
                    ss.StatusPercentage = 20;

                    var userConfigId = _objPriceListRepository.PriceFileHeaderRepository.GetQueryable(x => x.PriceFileHeaderID == Param_PriceFileHeaderId && x.IsActive == true).FirstOrDefault().UserConfigSettingID;

                    userConfig = _objConfigureRepository.UserConfigSettingRepository.GetQueryable(x => x.UserConfigSettingID == userConfigId).FirstOrDefault();

                    List<SelectedCustomersByHeaderID> lstcustomers = JsonConvert.DeserializeObject<List<SelectedCustomersByHeaderID>>(ConfigCustomers);

                    selected_cust = (from cust in lstcustomers
                                     select new SelectedCustomers
                                     {
                                         CustomerName = cust.CustomerName,
                                         CustomerNumber = cust.CustomerNumber,
                                         PC1 = cust.PC1,
                                         PC2 = cust.PC2,
                                         PC3 = cust.PC3,
                                         zKUNNR = cust.zKUNNR,
                                         CustomerSNO = cust.CustomerSNO
                                     }).ToList();

                    var rfmaster = _objConfigureRepository.ReportFormatMasterRepository
                                .GetQueryable(a => a.ReportFormatMasterID == userConfig.ReportFormatTemplateID && a.IsActive == true);
                    var rfmastermapping = _objConfigureRepository.ReportFormatFieldMappingRepository
                                    .GetManyQueryable(x => x.IsActive == true);
                    var rffieldmaster = _objConfigureRepository.ReportFormatFieldMasterRepository.GetQueryable(s => s.IsActive == true);

                    _objLoggingProvider.LogMessage(LogType.Info, "ReportFormatFieldMaster Details: Start using  UserConfigSettingID: " + userConfigId);
                    totres = (from rfm in rfmaster
                              join rfmap in rfmastermapping on rfm.ReportFormatMasterID equals rfmap.ReportFormatMasterID
                              join rffm in rffieldmaster on rfmap.ReportFormatFieldMasterID equals rffm.ReportFormatFieldMasterID
                              where rfmap.ReportFormatMasterID == userConfig.ReportFormatTemplateID
                              select (new ReportFormatCustom
                              {
                                  ReportFormatMasterId = rfm.ReportFormatMasterID,
                                  ReportFormatName = rfm.FormatName,
                                  ReportFormatAliasName = rfm.AliasName,
                                  FormatFieldMasterID = rfmap.ReportFormatFieldMasterID,
                                  ReportFieldAliasName = rfmap.AliasName,
                                  ReportFieldSequence = rfmap.SequenceNo,
                                  ReportFieldMasterFieldName = rffm.FieldName,
                                  ReportFieldMasterDescription = rffm.FieldDescription,
                                  ReportFormatFieldMasterDataType = rffm.DataType,
                              })
                               ).ToList();
                    _objLoggingProvider.LogMessage(LogType.Info, "ReportFormatFieldMaster Details: End");

                    Stored_priceFileHeaderId = priceHeader.PriceFileHeaderID;
                }

                foreach (var cust in selected_cust)
                {

                    _objLoggingProvider.LogMessage(LogType.Info, "PriceFileDetails for " + cust.zKUNNR + ": Start using  UserConfigSettingID:=" + ConfigId);

                    var respdet = GetPriceFileDetailsForCustomer(Stored_priceFileHeaderId, cust.zKUNNR, showNotFoundMaterials);

                    var responsedetails = respdet.OrderBy(x=>x.SchneiderElectricMaterialReference).ToList();
                    //var responsedetails = from re in respdet
                    //                      orderby re.SchneiderElectricMaterialReference ascending
                    //                      select re;

                    _objLoggingProvider.LogMessage(LogType.Info, "PriceFileDetails for " + cust.zKUNNR + ": End using  UserConfigSettingID:=" + ConfigId);

                    if (ConfigId != 0 && Param_PriceFileHeaderId == 0)
                    {
                        UpdatePriceFileLocationInfo(Param_PriceFileHeaderId, cust.zKUNNR, 15, "In-Progress");
                    }
                    else if (ConfigId != 0 && Param_PriceFileHeaderId != 0)
                    {
                        UpdatePriceFileLocationInfoForDatabaseMode(Param_PriceFileHeaderId, cust.zKUNNR, 15, "In-Progress");
                    }


                    _objLoggingProvider.LogMessage(LogType.Info, "PriceFileDetails: UpdatePriceFileLocationInfo Status");


                    _objLoggingProvider.LogMessage(LogType.Info, "PriceFileDetails: Customising Excel Variables for UserConfigSettingID:=" + ConfigId);
                    var cosvalue = string.Empty;
                    if (responsedetails.Count !=  0)
                    {
                        cosvalue = responsedetails.First().ProductHierarchy.ToString();
                    }
                    else
                    {
                        if(userConfig.CanUseLocalCOSForProductHierarchy == true)
                        {
                            cosvalue = "LocalCOS";
                        }
                        else
                        {
                            cosvalue = "GlobalCOS";
                        }
                    }
                    
                    var excelFileName = "";
                    var lastColumnName = "";
                    bool canUseAltValidDate = userConfig.CanUseAlternateValidFromDate;
                    DateTime dt = Convert.ToDateTime(DateTime.UtcNow);
                    if (canUseAltValidDate == false)
                    {
                        var appendstr = userConfig.CanAddSODInFinalPrice == true ? " - Inc SOD" : "";
                        DateTime pactive = Convert.ToDateTime(userConfig.PricesActiveDate.ToString());
                        excelFileName = cust.CustomerNumber + " - " + cust.CustomerName.Replace("/", "") + " - Created on " + dt.ToString("dd-MMM-yy") + " - Pricing Valid on " + pactive.ToString("dd-MMM-yy")+ appendstr;
                        CultureInfo cultureInfo = new CultureInfo("en-US");
                        cultureInfo.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                        cultureInfo.DateTimeFormat.LongTimePattern = "HH:mm:ss tt";
                        lastColumnName = "File created on " + dt.ToString("dd/MM/yyyy HH:mm:ss tt",cultureInfo) + "\nPricing Valid on " + pactive.ToString("dd/MM/yyyy",cultureInfo) + "\n(Schneider Electric internal ref - List Price File - " + AppConfig.ApplicationMode + " )";
                    }
                    else
                    {
                        var appendstr = userConfig.CanAddSODInFinalPrice == true ? " - Inc SOD" : "";
                        DateTime pactive = Convert.ToDateTime(userConfig.AlternateValidFromDate.ToString());
                        excelFileName = cust.CustomerNumber + " - " + cust.CustomerName.Replace("/", "") + " - Created on " + dt.ToString("dd-MMM-yy") + " - Pricing Valid on " + pactive.ToString("dd-MMM-yy")+ appendstr;
                        //lastColumnName = "File created on " + dt.ToString("dd/MM/yyyy HH:mm:ss tt") + "\nPricing Valid on " + pactive.ToString("dd/MM/yyyy") + "\n(Schneider Electric internal ref - List Price File - " + AppConfig.ApplicationMode + " )";
                        CultureInfo cultureInfo = new CultureInfo("en-US");
                        cultureInfo.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                        cultureInfo.DateTimeFormat.LongTimePattern = "HH:mm:ss tt";
                        lastColumnName = "File created on " + dt.ToString("dd/MM/yyyy HH:mm:ss tt", cultureInfo) + "\nPricing Valid on " + pactive.ToString("dd/MM/yyyy", cultureInfo) + "\n(Schneider Electric internal ref - List Price File - " + AppConfig.ApplicationMode + " )";
                    }

                    List<string> excludedColumnText = new();

                    var priceBreak1 = responsedetails.Sum(a => a.PriceBreak1CustomerQty + a.PriceBreak1CustomerDiscount + a.PriceBreak1CustomerCostInclGST + a.PriceBreak1CustomerCostExclGST);
                    if (priceBreak1 == 0)
                    {
                        string[] col_text = { "PriceBreak1CustomerQty", "PriceBreak1CustomerDiscount", "PriceBreak1CustomerCostExclGST", "PriceBreak1CustomerCostInclGST" };
                        excludedColumnText.AddRange(col_text);
                    }
                    var priceBreak2 = responsedetails.Sum(a => a.PriceBreak2CustomerQty + a.PriceBreak2CustomerDiscount + a.PriceBreak2CustomerCostInclGST + a.PriceBreak2CustomerCostExclGST);
                    if (priceBreak2 == 0)
                    {
                        string[] col_text = { "PriceBreak2CustomerQty", "PriceBreak2CustomerDiscount", "PriceBreak2CustomerCostExclGST", "PriceBreak2CustomerCostInclGST" };
                        excludedColumnText.AddRange(col_text);
                    }
                    var priceBreak3 = responsedetails.Sum(a => a.PriceBreak3CustomerQty + a.PriceBreak3CustomerDiscount + a.PriceBreak3CustomerCostInclGST + a.PriceBreak3CustomerCostExclGST);
                    if (priceBreak3 == 0)
                    {
                        string[] col_text = { "PriceBreak3CustomerQty", "PriceBreak3CustomerDiscount", "PriceBreak3CustomerCostExclGST", "PriceBreak3CustomerCostInclGST" };
                        excludedColumnText.AddRange(col_text);
                    }
                    var priceBreak4 = responsedetails.Sum(a => a.PriceBreak4CustomerQty + a.PriceBreak4CustomerDiscount + a.PriceBreak4CustomerCostInclGST + a.PriceBreak4CustomerCostExclGST);
                    if (priceBreak4 == 0)
                    {
                        string[] col_text = { "PriceBreak4CustomerQty", "PriceBreak4CustomerDiscount", "PriceBreak4CustomerCostExclGST", "PriceBreak4CustomerCostInclGST" };
                        excludedColumnText.AddRange(col_text);
                    }
                    var priceBreak5 = responsedetails.Sum(a => a.PriceBreak5CustomerQty + a.PriceBreak5CustomerDiscount + a.PriceBreak5CustomerCostInclGST + a.PriceBreak5CustomerCostExclGST);
                    if (priceBreak5 == 0)
                    {
                        string[] col_text = { "PriceBreak5CustomerQty", "PriceBreak5CustomerDiscount", "PriceBreak5CustomerCostExclGST", "PriceBreak5CustomerCostInclGST" };
                        excludedColumnText.AddRange(col_text);
                    }

                    if (ConfigId != 0 && Param_PriceFileHeaderId == 0)
                    {
                        UpdatePriceFileLocationInfo(Stored_priceFileHeaderId, cust.zKUNNR, 35, "In-Progress");
                    }
                    else if (ConfigId != 0 && Param_PriceFileHeaderId != 0)
                    {
                        UpdatePriceFileLocationInfoForDatabaseMode(Param_PriceFileHeaderId, cust.zKUNNR, 35, "In-Progress");
                    }

                    try
                    {
                        // Updating of Alias Name
                        JArray resarray = new();
                        for (int i = 0; i < totres.ToList().Count; i++)
                        {
                            var exists = excludedColumnText.Find(s => s.Contains(totres[i].ReportFieldMasterFieldName.ToString()));
                            if (exists == null)
                            {
                                if (totres[i].ReportFieldMasterFieldName.ToString().Replace(" ", "") == "SAPCOS" && cosvalue == "LocalCOS")
                                {
                                    JObject resobj = new();
                                    resobj["Name"] = totres[i].ReportFieldMasterFieldName.ToString();
                                    resobj["AliasName"] = "SAP Local COS";
                                    resobj["IsText"] = true;
                                    resobj["AlignmentLeft"] = totres[i].ReportFormatFieldMasterDataType.ToLower() == "VARCHAR".ToLower()?true:false;
                                    resobj["Color"] = "White";
                                    resobj["Discount"] = false;
                                    resarray.Add(resobj);
                                }
                                else if (totres[i].ReportFieldMasterFieldName.ToString().Replace(" ", "") == "SAPCOS" && cosvalue == "GlobalCOS")
                                {
                                    JObject resobj = new();
                                    resobj["Name"] = totres[i].ReportFieldMasterFieldName.ToString();
                                    resobj["AliasName"] = "SAP Global COS";
                                    resobj["IsText"] = true;
                                    resobj["AlignmentLeft"] = totres[i].ReportFormatFieldMasterDataType.ToLower() == "VARCHAR".ToLower() ? true : false;
                                    resobj["Color"] = "White";
                                    resobj["Discount"] = false;
                                    resarray.Add(resobj);
                                }
                                else if (totres[i].ReportFieldMasterFieldName.ToString() == "StockStatus")
                                {
                                    JObject resobj = new();
                                    resobj["Name"] = totres[i].ReportFieldMasterFieldName.ToString();
                                    resobj["IsText"] = true;
                                    resobj["AlignmentLeft"] = totres[i].ReportFormatFieldMasterDataType.ToLower() == "VARCHAR".ToLower() ? true : false;
                                    resobj["Color"] = "White";
                                    resobj["Discount"] = false;
                                    if(userConfig.SalesOrganization == "AU01")
                                    {
                                        var firsttext = totres[i].ReportFieldAliasName.ToString().Substring(0,12).Trim();
                                        var secondtext = totres[i].ReportFieldAliasName.ToString().Substring(12).Trim();
                                        //string[] splitstr = totres[i].ReportFormatAliasName.ToString().Substring(0,12);
                                        resobj["AliasName"] = string.Concat(firsttext,Environment.NewLine, secondtext);
                                    }
                                    else
                                    {
                                        resobj["AliasName"] = totres[i].ReportFieldAliasName.ToString();
                                    }
                                    resarray.Add(resobj);
                                }
                                else if (totres[i].ReportFieldMasterFieldName.ToString() == "FileReferenceData")
                                {
                                    JObject resobj = new();
                                    resobj["Name"] = totres[i].ReportFieldMasterFieldName.ToString();
                                    resobj["AliasName"] = lastColumnName;
                                    resobj["IsText"] = true;
                                    resobj["AlignmentLeft"] = totres[i].ReportFormatFieldMasterDataType.ToLower() == "VARCHAR".ToLower() ? true : false;
                                    resobj["Color"] = "Yellow";
                                    resobj["Discount"] = false;
                                    resarray.Add(resobj);
                                }

                                else if (priceBreak1 != 0 && (totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak1CustomerQty".ToLower()
                                   || totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak1CustomerDiscount".ToLower() ||
                                   totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak1CustomerCostExclGST".ToLower() ||
                                   totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak1CustomerCostInclGST".ToLower()))
                                {
                                    JObject resobj = new();
                                    resobj["Name"] = totres[i].ReportFieldMasterFieldName.ToString();
                                    resobj["AliasName"] = totres[i].ReportFieldAliasName.ToString();
                                    resobj["AlignmentLeft"] = totres[i].ReportFormatFieldMasterDataType.ToLower() == "VARCHAR".ToLower() ? true : false;
                                    resobj["Color"] = "Green1";
                                    if (totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak1CustomerDiscount".ToLower())
                                    {
                                        resobj["Discount"] = true;
                                    }
                                    else
                                    {
                                        resobj["Discount"] = false;
                                    }
                                    resarray.Add(resobj);
                                }
                                else if (priceBreak2 != 0 && (totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak2CustomerQty".ToLower()
                                   || totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak2CustomerDiscount".ToLower() ||
                                   totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak2CustomerCostExclGST".ToLower() ||
                                   totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak2CustomerCostInclGST".ToLower()))
                                {
                                    JObject resobj = new();
                                    resobj["Name"] = totres[i].ReportFieldMasterFieldName.ToString();
                                    resobj["AliasName"] = totres[i].ReportFieldAliasName.ToString();
                                    resobj["AlignmentLeft"] = totres[i].ReportFormatFieldMasterDataType.ToLower() == "VARCHAR".ToLower() ? true : false;
                                    resobj["Color"] = "Green2";
                                    if (totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak2CustomerDiscount".ToLower())
                                    {
                                        resobj["Discount"] = true;
                                    }
                                    else
                                    {
                                        resobj["Discount"] = false;
                                    }
                                    resarray.Add(resobj);
                                }
                                else if (priceBreak3 != 0 && (totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak3CustomerQty".ToLower()
                                  || totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak3CustomerDiscount".ToLower() ||
                                  totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak3CustomerCostExclGST".ToLower() ||
                                  totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak3CustomerCostInclGST".ToLower()))
                                {
                                    JObject resobj = new();
                                    resobj["Name"] = totres[i].ReportFieldMasterFieldName.ToString();
                                    resobj["AliasName"] = totres[i].ReportFieldAliasName.ToString();
                                    resobj["AlignmentLeft"] = totres[i].ReportFormatFieldMasterDataType.ToLower() == "VARCHAR".ToLower() ? true : false;
                                    resobj["Color"] = "Green3";
                                    if (totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak3CustomerDiscount".ToLower())
                                    {
                                        resobj["Discount"] = true;
                                    }
                                    else
                                    {
                                        resobj["Discount"] = false;
                                    }
                                    resarray.Add(resobj);
                                }
                                else if (priceBreak4 != 0 && (totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak4CustomerQty".ToLower()
                                  || totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak4CustomerDiscount".ToLower() ||
                                  totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak4CustomerCostExclGST".ToLower() ||
                                  totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak4CustomerCostInclGST".ToLower()))
                                {
                                    JObject resobj = new();
                                    resobj["Name"] = totres[i].ReportFieldMasterFieldName.ToString();
                                    resobj["AliasName"] = totres[i].ReportFieldAliasName.ToString();
                                    resobj["AlignmentLeft"] = totres[i].ReportFormatFieldMasterDataType.ToLower() == "VARCHAR".ToLower() ? true : false;
                                    resobj["Color"] = "Green4";
                                    if (totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak4CustomerDiscount".ToLower())
                                    {
                                        resobj["Discount"] = true;
                                    }
                                    else
                                    {
                                        resobj["Discount"] = false;
                                    }
                                    resarray.Add(resobj);
                                }
                                else if (priceBreak5 != 0 && (totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak5CustomerQty".ToLower()
                                  || totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak5CustomerDiscount".ToLower() ||
                                  totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak5CustomerCostExclGST".ToLower() ||
                                  totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak5CustomerCostInclGST".ToLower()))
                                {
                                    JObject resobj = new();
                                    resobj["Name"] = totres[i].ReportFieldMasterFieldName.ToString();
                                    resobj["AliasName"] = totres[i].ReportFieldAliasName.ToString();
                                    resobj["AlignmentLeft"] = totres[i].ReportFormatFieldMasterDataType.ToLower() == "VARCHAR".ToLower() ? true : false;
                                    resobj["Color"] = "Green5";
                                    if (totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak5CustomerDiscount".ToLower())
                                    {
                                        resobj["Discount"] = true;
                                    }
                                    else
                                    {
                                        resobj["Discount"] = false;
                                    }
                                    resarray.Add(resobj);
                                }
                                else if (priceBreak2 != 0 && (totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak6CustomerQty".ToLower()
                                   || totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak6CustomerDiscount".ToLower() ||
                                   totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak6CustomerCostExclGST".ToLower() ||
                                   totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak6CustomerCostInclGST".ToLower()))
                                {
                                    JObject resobj = new();
                                    resobj["Name"] = totres[i].ReportFieldMasterFieldName.ToString();
                                    resobj["AliasName"] = totres[i].ReportFieldAliasName.ToString();
                                    resobj["AlignmentLeft"] = totres[i].ReportFormatFieldMasterDataType.ToLower() == "VARCHAR".ToLower() ? true : false;
                                    resobj["Color"] = "Green2";
                                    if (totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak6CustomerDiscount".ToLower())
                                    {
                                        resobj["Discount"] = true;
                                    }
                                    else
                                    {
                                        resobj["Discount"] = false;
                                    }
                                    resarray.Add(resobj);
                                }
                                else if (priceBreak3 != 0 && (totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak7CustomerQty".ToLower()
                                  || totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak7CustomerDiscount".ToLower() ||
                                  totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak7CustomerCostExclGST".ToLower() ||
                                  totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak7CustomerCostInclGST".ToLower()))
                                {
                                    JObject resobj = new();
                                    resobj["Name"] = totres[i].ReportFieldMasterFieldName.ToString();
                                    resobj["AliasName"] = totres[i].ReportFieldAliasName.ToString();
                                    resobj["AlignmentLeft"] = totres[i].ReportFormatFieldMasterDataType.ToLower() == "VARCHAR".ToLower() ? true : false;
                                    resobj["Color"] = "Green3";
                                    if (totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak7CustomerDiscount".ToLower())
                                    {
                                        resobj["Discount"] = true;
                                    }
                                    else
                                    {
                                        resobj["Discount"] = false;
                                    }
                                    resarray.Add(resobj);
                                }
                                else if (priceBreak4 != 0 && (totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak8CustomerQty".ToLower()
                                  || totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak8CustomerDiscount".ToLower() ||
                                  totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak8CustomerCostExclGST".ToLower() ||
                                  totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak8CustomerCostInclGST".ToLower()))
                                {
                                    JObject resobj = new();
                                    resobj["Name"] = totres[i].ReportFieldMasterFieldName.ToString();
                                    resobj["AliasName"] = totres[i].ReportFieldAliasName.ToString();
                                    resobj["AlignmentLeft"] = totres[i].ReportFormatFieldMasterDataType.ToLower() == "VARCHAR".ToLower() ? true : false;
                                    resobj["Color"] = "Green4";
                                    if (totres[i].ReportFieldMasterFieldName.ToString().ToLower() == "PriceBreak8CustomerDiscount".ToLower())
                                    {
                                        resobj["Discount"] = true;
                                    }
                                    else
                                    {
                                        resobj["Discount"] = false;
                                    }
                                    resarray.Add(resobj);
                                }
                                else
                                {
                                    JObject resobj = new();
                                    resobj["Name"] = totres[i].ReportFieldMasterFieldName.ToString();
                                    resobj["AliasName"] = totres[i].ReportFieldAliasName.ToString();
                                    resobj["AlignmentLeft"] = totres[i].ReportFormatFieldMasterDataType.ToLower() == "VARCHAR".ToLower() ? true : totres[i].ReportFormatFieldMasterDataType.ToLower() == "Date".ToLower()? true :false;
                                    resobj["Color"] = "White";
                                    resobj["Discount"] = false;
                                    resarray.Add(resobj);
                                }
                            }

                        }
                        _objLoggingProvider.LogMessage(LogType.Info, "PriceFileDetails: Excel Headerobject done");
                        var boundTable = responsedetails.ToList();
                        IWorkbook workbook = new XSSFWorkbook();

                        // Assigning name for sheet & restricting to 31 charaters
                        // (Excel doesn't support sheet name more than 31 charaters)
                        // included because excel sheet does't support '/'
                        var sheetName = cust.CustomerNumber + " - " + cust.CustomerName.Replace("/", "");
                        
                        if (sheetName.Length > 31)
                        {
                            sheetName = sheetName.Substring(0, 31);
                        }
                        ISheet sheet = workbook.CreateSheet(sheetName);
                        IRow headerRow = sheet.CreateRow(0);
                        headerRow.HeightInPoints = 45.5F;
                        // Create Header style 1 (Header row)
                        ICellStyle headerCellStyle = CreateHeaderCellStyle(workbook);
                        headerCellStyle.BorderBottom = BorderStyle.Medium;
                        headerCellStyle.BorderLeft = BorderStyle.Thin;
                        headerCellStyle.BorderTop = BorderStyle.Thin;
                        headerCellStyle.BorderRight = BorderStyle.Thin;

                        // Create Header cell style for last column
                        ICellStyle headerLastStyle = CreateHeaderLastCellStyle(workbook);
                        headerLastStyle.CloneStyleFrom(headerCellStyle);
                        headerLastStyle.FillForegroundColor = IndexedColors.LightYellow.Index;
                        headerLastStyle.FillPattern = FillPattern.SolidForeground;

                        // Create Header cell style2 for Price Columns (Dark)
                        ICellStyle headerPriceStyle1 = CreateHeaderDarkCellStyle(workbook);


                        // Create Header cell style2 for Price Columns (Light)
                        ICellStyle headerPriceStyle2 = CreateHeaderLightCellStyle(workbook);


                        IFont headerFont = CreateHeaderFont(workbook);
                        IFont headerLastFont = CreateHeaderLastFont(workbook);

                        int headerRowIndexCnt = 0;
                        _objLoggingProvider.LogMessage(LogType.Info, "PriceFileDetails: Writting Excel Header Start");
                        //Handling of data in excel for Header
                        foreach (JObject res in resarray)
                        {
                            int headerTextLength = (res["AliasName"].ToString()).Length;
                            double requiredWidth = (headerTextLength + 1) * 256;
                            int autoFitWidth = sheet.GetColumnWidth(headerRowIndexCnt);
                            int finalWidth = (int)Math.Max(requiredWidth, autoFitWidth);

                            if (res["Name"].ToString().ToLower() == "FileReferenceData".ToLower())
                            {
                                NPOI.SS.UserModel.ICell cell = headerRow.CreateCell(headerRowIndexCnt);
                                cell.SetCellValue(res["AliasName"].ToString());
                                cell.CellStyle = headerLastStyle;
                                cell.CellStyle.SetFont(headerLastFont);
                            }
                            else if (res["Name"].ToString().ToLower() == "StockStatus".ToLower())
                            {
                                NPOI.SS.UserModel.ICell cell = headerRow.CreateCell(headerRowIndexCnt);
                                cell.SetCellValue(res["AliasName"].ToString());
                                cell.CellStyle = headerCellStyle;
                                cell.CellStyle.SetFont(headerFont);
                            }
                            else if (priceBreak1 != 0 && res["Color"].ToString().ToLower() == "Green1".ToLower())
                            {
                                ICell cell = headerRow.CreateCell(headerRowIndexCnt);
                                cell.SetCellValue(res["AliasName"].ToString());
                                cell.CellStyle = headerPriceStyle1;
                                cell.CellStyle.SetFont(headerFont);
                            }
                            else if (priceBreak2 != 0 && res["Color"].ToString().ToLower() == "Green2".ToLower())
                            {
                                ICell cell = headerRow.CreateCell(headerRowIndexCnt);
                                cell.SetCellValue(res["AliasName"].ToString());
                                cell.CellStyle = headerPriceStyle2;
                                cell.CellStyle.SetFont(headerFont);
                            }
                            else if (priceBreak3 != 0 && res["Color"].ToString().ToLower() == "Green3".ToLower())
                            {
                                ICell cell = headerRow.CreateCell(headerRowIndexCnt);
                                cell.SetCellValue(res["AliasName"].ToString());
                                cell.CellStyle = headerPriceStyle1;
                                cell.CellStyle.SetFont(headerFont);
                            }
                            else if (priceBreak4 != 0 && res["Color"].ToString().ToLower() == "Green4".ToLower())
                            {
                                ICell cell = headerRow.CreateCell(headerRowIndexCnt);
                                cell.SetCellValue(res["AliasName"].ToString());
                                cell.CellStyle = headerPriceStyle2;
                                cell.CellStyle.SetFont(headerFont);
                            }
                            else if (priceBreak5 != 0 && res["Color"].ToString().ToLower() == "Green5".ToLower())
                            {
                                ICell cell = headerRow.CreateCell(headerRowIndexCnt);
                                cell.SetCellValue(res["AliasName"].ToString());
                                cell.CellStyle = headerPriceStyle1;
                                cell.CellStyle.SetFont(headerFont);
                            }
                            else
                            {
                                ICell cell = headerRow.CreateCell(headerRowIndexCnt);
                                cell.SetCellValue(res["AliasName"].ToString());
                                cell.CellStyle = headerCellStyle;
                                cell.CellStyle.SetFont(headerFont);
                            }
                            //sheet.AutoSizeColumn(headerRowIndexCnt);
                            sheet.SetColumnWidth(headerRowIndexCnt, finalWidth);
                            headerRowIndexCnt++;
                        }
                        _objLoggingProvider.LogMessage(LogType.Info, "PriceFileDetails: Writting Excel Header End");

                        if (ConfigId != 0 && Param_PriceFileHeaderId == 0)
                        {
                            UpdatePriceFileLocationInfo(Stored_priceFileHeaderId, cust.zKUNNR, 65, "In-Progress");
                        }
                        else if (ConfigId != 0 && Param_PriceFileHeaderId != 0)
                        {
                            UpdatePriceFileLocationInfoForDatabaseMode(Param_PriceFileHeaderId, cust.zKUNNR, 65, "In-Progress");
                        }

                        _objLoggingProvider.LogMessage(LogType.Info, "PriceFileDetails: UpdatePriceFileLocationInfo satus with 65 %");
                        _objLoggingProvider.LogMessage(LogType.Info, "PriceFileDetails: Assigning of cell formats for data start");
                        // Handling of data in excel excluding header row
                        int rowIndex = 1;
                        int indxcnt = 0;
                        // Data row style for invalid matching row
                        ICellStyle dataHighlightStyle = CreateDataCellStyle(workbook);
                        dataHighlightStyle.FillForegroundColor = IndexedColors.Red.Index;
                        dataHighlightStyle.FillPattern = FillPattern.SolidForeground;

                        // Data row for normal rows
                        ICellStyle dataStrStyle = CreateDataCellStyle(workbook);
                        ICellStyle dataCellStyleRight = CreateDataDigitCellStyle(workbook);

                        // Data row for barcode cells
                        ICellStyle dataBarcodeStyle = CreateTextDataCellStyle(workbook);
                        ICellStyle dataBarcodeCellStyle = CreateTextDataValueCellStyle(workbook);

                        // Data row for Price discount color (Dark)
                        ICellStyle dataRowStylePriceDisc1 = CreateDataDarkCellStyle(workbook);

                        // Data row for Price discount color (Light)
                        ICellStyle dataRowStylePriceDisc2 = CreateDataLightCellStyle(workbook);

                        IFont dataFont = CreateDataFont(workbook);
                        IDataFormat decimalDataFormat = workbook.CreateDataFormat();
                        IDataFormat dateDataFormat = workbook.CreateDataFormat();

                        _objLoggingProvider.LogMessage(LogType.Info, "PriceFileDetails: Assigning of cell formats for data End");
                        _objLoggingProvider.LogMessage(LogType.Info, "PriceFileDetails: Writting data in to excel start");
                        headerRowIndexCnt = 0;
                        foreach (var dyobject in boundTable)
                        {
                            bool highlighColor = dyobject.IsFound;
                            IRow dataRow = sheet.CreateRow(rowIndex);
                            int internalCnt = 0;
                            foreach (JObject res in resarray)
                            {
                                Object obj = (object)dyobject;
                                Type type = obj.GetType();
                                PropertyInfo propertyinfo = type.GetProperty(res["Name"].ToString().Replace(" ", ""));
                                if (propertyinfo != null)
                                {
                                    var align = (bool)res["AlignmentLeft"];
                                    object value = propertyinfo.GetValue(obj, null);
                                    ICell dataCell = dataRow.CreateCell(internalCnt);

                                    if (res["Name"].ToString().ToLower() == "WholesaleListPriceExclGST".ToLower() || res["Name"].ToString().ToLower() == "WholesaleListPriceInclGST".ToLower())
                                    {
                                        if (!highlighColor)
                                        {
                                            dataCell.CellStyle = dataHighlightStyle;
                                            dataCell.SetCellValue(string.Empty);
                                            dataCell.SetCellType(CellType.String);
                                            RemoveCellComment(dataCell);
                                        }
                                        else
                                        {
                                            var value_txt = string.Empty;
                                            if (value == null || value.ToString() == "0")
                                            {
                                                value_txt = string.Empty;
                                            }
                                            else
                                            {
                                                value_txt = Utilities.Common.RoundStringToDecimal(value.ToString());
                                            }

                                            if (align)
                                            {
                                                dataCell.CellStyle = dataStrStyle;

                                            }
                                            else
                                            {
                                                dataCell.CellStyle = dataCellStyleRight;

                                            } 
                                            dataCell.SetCellValue(value_txt);
                                            dataCell.SetCellType(CellType.String);
                                            RemoveCellComment(dataCell);
                                        }

                                    }
                                    else if (res["Name"].ToString().ToLower() == "ValidFrom".ToLower() || res["Name"].ToString().ToLower() == "ValidTo".ToLower())
                                    {  // SAP COS
                                        if (!highlighColor)
                                        {
                                            dataCell.CellStyle = dataHighlightStyle;
                                            dataCell.SetCellValue(string.Empty);
                                            dataCell.SetCellType(CellType.String);
                                            RemoveCellComment(dataCell);
                                        }
                                        else
                                        {
                                            if (align)
                                            {
                                                dataCell.CellStyle = dataStrStyle;
                                            }
                                            else
                                            {
                                                dataCell.CellStyle = dataCellStyleRight;
                                            }
                                            string datevalue = value.ToString().Split(" ")[0];
                                            DateTime dtvalue = Convert.ToDateTime(datevalue.ToString());
                                            CultureInfo cultureInfodt = new CultureInfo("en-US");
                                            cultureInfodt.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                                            cultureInfodt.DateTimeFormat.LongTimePattern = "HH:mm:ss tt";
                                            dataCell.SetCellValue(value == null ? string.Empty : dtvalue.ToString("dd/MM/yyyy", cultureInfodt));
                                            dataCell.SetCellType(CellType.String);
                                            RemoveCellComment(dataCell);
                                        }

                                    }
                                    else if (res["Name"].ToString().Replace(" ", "").ToLower() == "SAPCOS".ToLower())
                                    {
                                        if (!highlighColor)
                                        {
                                            dataCell.CellStyle = dataHighlightStyle;
                                            dataCell.SetCellValue(string.Empty);
                                            dataCell.SetCellType(CellType.String);
                                            RemoveCellComment(dataCell);
                                        }
                                        else
                                        {
                                            var value_txt = string.Empty;
                                            if (value == null || value.ToString() == "0")
                                            {
                                                value_txt = string.Empty;
                                            }
                                            else
                                            {
                                                value_txt = value.ToString();
                                            }
                                            if (align)
                                            {
                                                dataCell.CellStyle = dataStrStyle;
                                            }
                                            else
                                            {
                                                dataCell.CellStyle = dataCellStyleRight;
                                            }
                                            dataCell.SetCellValue(value_txt);
                                            dataCell.SetCellType(CellType.String);
                                            RemoveCellComment(dataCell);
                                        }

                                    }
                                    else if (res["Name"].ToString().ToLower() == "PriceBreak1CustomerQty".ToLower() ||
                                       res["Name"].ToString().ToLower() == "PriceBreak1CustomerDiscount".ToLower() ||
                                       res["Name"].ToString().ToLower() == "PriceBreak1CustomerCostExclGST".ToLower() ||
                                       res["Name"].ToString().ToLower() == "PriceBreak1CustomerCostInclGST".ToLower())
                                    {
                                        if (!highlighColor)
                                        {
                                            dataCell.CellStyle = dataHighlightStyle;
                                            dataCell.SetCellValue(value == null ? string.Empty : value.ToString() == "0" ? "" : string.Empty);
                                            dataCell.SetCellType(CellType.String);
                                            RemoveCellComment(dataCell);
                                        }
                                        else
                                        {
                                            if (res["Name"].ToString().ToLower() == "PriceBreak1CustomerDiscount".ToLower())
                                            {
                                                dataCell.CellStyle = dataRowStylePriceDisc1;
                                                dataCell.SetCellValue(value == null ? string.Empty : value.ToString() == "0" ? "" : Utilities.Common.RoundStringToDecimalNew((Convert.ToSingle(value)*100).ToString()) + "%");
                                                dataCell.SetCellType(CellType.String);
                                                RemoveCellComment(dataCell);
                                            }
                                            else if (res["Name"].ToString().ToLower() == "PriceBreak1CustomerQty".ToLower())
                                            {
                                                dataCell.CellStyle = dataRowStylePriceDisc1;
                                                dataCell.SetCellValue(value == null ? string.Empty : value.ToString() == "0" ? "" : value.ToString());
                                                dataCell.SetCellType(CellType.String);
                                                RemoveCellComment(dataCell);
                                            }
                                            else
                                            {
                                                dataCell.CellStyle = dataRowStylePriceDisc1;
                                                dataCell.SetCellValue(value == null ? string.Empty : value.ToString() == "0" ? "" : Utilities.Common.RoundStringToDecimal(value.ToString()));
                                                dataCell.SetCellType(CellType.String);
                                                RemoveCellComment(dataCell);
                                            }
                                        }

                                    }
                                    else if (res["Name"].ToString().ToLower() == "PriceBreak2CustomerQty".ToLower() ||
                                       res["Name"].ToString().ToLower() == "PriceBreak2CustomerDiscount".ToLower() ||
                                       res["Name"].ToString().ToLower() == "PriceBreak2CustomerCostExclGST".ToLower() ||
                                       res["Name"].ToString().ToLower() == "PriceBreak2CustomerCostInclGST".ToLower())
                                    {
                                        if (!highlighColor)
                                        {
                                            dataCell.CellStyle = dataHighlightStyle;
                                            dataCell.SetCellValue(value == null ? string.Empty : value.ToString() == "0" ? "" : string.Empty);
                                            dataCell.SetCellType(CellType.String);
                                            RemoveCellComment(dataCell);
                                        }
                                        else
                                        {
                                            if (res["Name"].ToString().ToLower() == "PriceBreak2CustomerDiscount".ToLower())
                                            {
                                                dataCell.CellStyle = dataRowStylePriceDisc2;
                                                dataCell.SetCellValue(value == null ? string.Empty : value.ToString() == "0" ? "" : Utilities.Common.RoundStringToDecimalNew((Convert.ToSingle(value) * 100).ToString()) + "%");
                                                dataCell.SetCellType(CellType.String);
                                                RemoveCellComment(dataCell);
                                            }
                                            else if (res["Name"].ToString().ToLower() == "PriceBreak2CustomerQty".ToLower())
                                            {
                                                dataCell.CellStyle = dataRowStylePriceDisc2;
                                                dataCell.SetCellValue(value == null ? string.Empty : value.ToString() == "0" ? "" : value.ToString());
                                                dataCell.SetCellType(CellType.String);
                                                RemoveCellComment(dataCell);
                                            }
                                            else
                                            {
                                                dataCell.CellStyle = dataRowStylePriceDisc2;
                                                dataCell.SetCellValue(value == null ? string.Empty : value.ToString() == "0" ? "" : Utilities.Common.RoundStringToDecimal(value.ToString()));
                                                dataCell.SetCellType(CellType.String);
                                                RemoveCellComment(dataCell);
                                            }
                                        }

                                    }
                                    else if (res["Name"].ToString().ToLower() == "PriceBreak3CustomerQty".ToLower() ||
                                      res["Name"].ToString().ToLower() == "PriceBreak3CustomerDiscount".ToLower() ||
                                      res["Name"].ToString().ToLower() == "PriceBreak3CustomerCostExclGST".ToLower() ||
                                      res["Name"].ToString().ToLower() == "PriceBreak3CustomerCostInclGST".ToLower())
                                    {
                                        if (!highlighColor)
                                        {
                                            dataCell.CellStyle = dataHighlightStyle;
                                            dataCell.SetCellValue(value == null ? string.Empty : value.ToString() == "0" ? "" : string.Empty);
                                            dataCell.SetCellType(CellType.String);
                                            RemoveCellComment(dataCell);
                                        }
                                        else
                                        {
                                            if (res["Name"].ToString().ToLower() == "PriceBreak3CustomerDiscount".ToLower())
                                            {
                                                dataCell.CellStyle = dataRowStylePriceDisc1;
                                                dataCell.SetCellValue(value == null ? string.Empty : value.ToString() == "0" ? "" : Utilities.Common.RoundStringToDecimalNew((Convert.ToSingle(value) * 100).ToString()) + "%");
                                                dataCell.SetCellType(CellType.String);
                                                RemoveCellComment(dataCell);
                                            }
                                            else if (res["Name"].ToString().ToLower() == "PriceBreak3CustomerQty".ToLower())
                                            {
                                                dataCell.CellStyle = dataRowStylePriceDisc1;
                                                dataCell.SetCellValue(value == null ? string.Empty : value.ToString() == "0" ? "" : value.ToString());
                                                dataCell.SetCellType(CellType.String);
                                                RemoveCellComment(dataCell);
                                            }
                                            else
                                            {
                                                dataCell.CellStyle = dataRowStylePriceDisc1;
                                                dataCell.SetCellValue(value == null ? string.Empty : value.ToString() == "0" ? "" : Utilities.Common.RoundStringToDecimal(value.ToString()));
                                                dataCell.SetCellType(CellType.String);
                                                RemoveCellComment(dataCell);
                                            }
                                        }

                                    }
                                    else if (res["Name"].ToString().ToLower() == "PriceBreak4CustomerQty".ToLower() ||
                                      res["Name"].ToString().ToLower() == "PriceBreak4CustomerDiscount".ToLower() ||
                                      res["Name"].ToString().ToLower() == "PriceBreak4CustomerCostExclGST".ToLower() ||
                                      res["Name"].ToString().ToLower() == "PriceBreak4CustomerCostInclGST".ToLower())
                                    {
                                        if (!highlighColor)
                                        {
                                            dataCell.CellStyle = dataHighlightStyle;
                                            dataCell.SetCellValue(value == null ? string.Empty : value.ToString() == "0" ? "" : string.Empty);
                                            dataCell.SetCellType(CellType.String);
                                            RemoveCellComment(dataCell);
                                        }
                                        else
                                        {
                                            if (res["Name"].ToString().ToLower() == "PriceBreak4CustomerDiscount".ToLower())
                                            {
                                                dataCell.CellStyle = dataRowStylePriceDisc2;
                                                dataCell.SetCellValue(value == null ? string.Empty : value.ToString() == "0" ? "" : Utilities.Common.RoundStringToDecimalNew((Convert.ToSingle(value) * 100).ToString()) + "%");
                                                dataCell.SetCellType(CellType.String);
                                                RemoveCellComment(dataCell);
                                            }
                                            else if (res["Name"].ToString().ToLower() == "PriceBreak4CustomerQty".ToLower())
                                            {
                                                dataCell.CellStyle = dataRowStylePriceDisc2;
                                                dataCell.SetCellValue(value == null ? string.Empty : value.ToString() == "0" ? "" : value.ToString());
                                                dataCell.SetCellType(CellType.String);
                                                RemoveCellComment(dataCell);
                                            }
                                            else
                                            {
                                                dataCell.CellStyle = dataRowStylePriceDisc2;
                                                dataCell.SetCellValue(value == null ? string.Empty : value.ToString() == "0" ? "" : Utilities.Common.RoundStringToDecimal(value.ToString()));
                                                dataCell.SetCellType(CellType.String);
                                                RemoveCellComment(dataCell);
                                            }
                                        }

                                    }
                                    else if (res["Name"].ToString().ToLower() == "PriceBreak5CustomerQty".ToLower() ||
                                      res["Name"].ToString().ToLower() == "PriceBreak5CustomerDiscount".ToLower() ||
                                      res["Name"].ToString().ToLower() == "PriceBreak5CustomerCostExclGST".ToLower() ||
                                      res["Name"].ToString().ToLower() == "PriceBreak5CustomerCostInclGST".ToLower())
                                    {
                                        if (!highlighColor)
                                        {
                                            dataCell.CellStyle = dataHighlightStyle;
                                            dataCell.SetCellValue(value == null ? string.Empty : value.ToString() == "0" ? "" : string.Empty);
                                            dataCell.SetCellType(CellType.String);
                                            RemoveCellComment(dataCell);
                                        }
                                        else
                                        {
                                            if (res["Name"].ToString().ToLower() == "PriceBreak5CustomerDiscount".ToLower())
                                            {
                                                dataCell.CellStyle = dataRowStylePriceDisc1;
                                                dataCell.SetCellValue(value == null ? string.Empty : value.ToString() == "0" ? "" : Utilities.Common.RoundStringToDecimalNew((Convert.ToSingle(value) * 100).ToString()) + "%");
                                                dataCell.SetCellType(CellType.String);
                                                RemoveCellComment(dataCell);
                                            }
                                            else if (res["Name"].ToString().ToLower() == "PriceBreak5CustomerQty".ToLower())
                                            {
                                                dataCell.CellStyle = dataRowStylePriceDisc1;
                                                dataCell.SetCellValue(value == null ? string.Empty : value.ToString() == "0" ? "" : value.ToString());
                                                dataCell.SetCellType(CellType.String);
                                                RemoveCellComment(dataCell);
                                            }
                                            else
                                            {
                                                dataCell.CellStyle = dataRowStylePriceDisc1;
                                                dataCell.SetCellValue(value == null ? string.Empty : value.ToString() == "0" ? "" : Utilities.Common.RoundStringToDecimal(value.ToString()));
                                                dataCell.SetCellType(CellType.String);
                                                RemoveCellComment(dataCell);
                                            }
                                        }

                                    }
                                    else if (res["Name"].ToString().ToLower() == "PriceBreak6CustomerQty".ToLower() ||
                                      res["Name"].ToString().ToLower() == "PriceBreak6CustomerDiscount".ToLower() ||
                                      res["Name"].ToString().ToLower() == "PriceBreak6CustomerCostExclGST".ToLower() ||
                                      res["Name"].ToString().ToLower() == "PriceBreak6CustomerCostInclGST".ToLower())
                                    {
                                        if (!highlighColor)
                                        {
                                            dataCell.CellStyle = dataHighlightStyle;
                                            dataCell.SetCellValue(value == null ? string.Empty : value.ToString() == "0" ? "" : string.Empty);
                                            dataCell.SetCellType(CellType.String);
                                            RemoveCellComment(dataCell);
                                        }
                                        else
                                        {
                                            if (res["Name"].ToString().ToLower() == "PriceBreak6CustomerDiscount".ToLower())
                                            {
                                                dataCell.CellStyle = dataRowStylePriceDisc2;
                                                dataCell.SetCellValue(value == null ? string.Empty : value.ToString() == "0" ? "" : Utilities.Common.RoundStringToDecimalNew((Convert.ToSingle(value) * 100).ToString()) + "%");
                                                dataCell.SetCellType(CellType.String);
                                                RemoveCellComment(dataCell);
                                            }
                                            else if (res["Name"].ToString().ToLower() == "PriceBreak6CustomerQty".ToLower())
                                            {
                                                dataCell.CellStyle = dataRowStylePriceDisc2;
                                                dataCell.SetCellValue(value == null ? string.Empty : value.ToString() == "0" ? "" : value.ToString());
                                                dataCell.SetCellType(CellType.String);
                                                RemoveCellComment(dataCell);
                                            }
                                            else
                                            {
                                                dataCell.CellStyle = dataRowStylePriceDisc2;
                                                dataCell.SetCellValue(value == null ? string.Empty : value == "0" ? "" : Utilities.Common.RoundStringToDecimal(value.ToString()));
                                                dataCell.SetCellType(CellType.String);
                                                RemoveCellComment(dataCell);
                                            }
                                        }

                                    }
                                    else if (res["Name"].ToString().ToLower() == "PriceBreak7CustomerQty".ToLower() ||
                                      res["Name"].ToString().ToLower() == "PriceBreak7CustomerDiscount".ToLower() ||
                                      res["Name"].ToString().ToLower() == "PriceBreak7CustomerCostExclGST".ToLower() ||
                                      res["Name"].ToString().ToLower() == "PriceBreak7CustomerCostInclGST".ToLower())
                                    {
                                        if (!highlighColor)
                                        {
                                            dataCell.CellStyle = dataHighlightStyle;
                                            dataCell.SetCellValue(value == null ? string.Empty : value.ToString() == "0" ? "" : string.Empty);
                                            dataCell.SetCellType(CellType.String);
                                            RemoveCellComment(dataCell);
                                        }
                                        else
                                        {
                                            if (res["Name"].ToString().ToLower() == "PriceBreak7CustomerDiscount".ToLower())
                                            {
                                                dataCell.CellStyle = dataRowStylePriceDisc1;
                                                dataCell.SetCellValue(value == null ? string.Empty : value.ToString() == "0" ? "" : Utilities.Common.RoundStringToDecimalNew((Convert.ToSingle(value) * 100).ToString()) + "%");
                                                dataCell.SetCellType(CellType.String);
                                                RemoveCellComment(dataCell);
                                            }
                                            else if (res["Name"].ToString().ToLower() == "PriceBreak7CustomerQty".ToLower())
                                            {
                                                dataCell.CellStyle = dataRowStylePriceDisc1;
                                                dataCell.SetCellValue(value == null ? string.Empty : value.ToString() == "0" ? "" : value.ToString());
                                                dataCell.SetCellType(CellType.String);
                                                RemoveCellComment(dataCell);
                                            }
                                            else
                                            {
                                                dataCell.CellStyle = dataRowStylePriceDisc1;
                                                dataCell.SetCellValue(value == null ? string.Empty : value.ToString() == "0" ? "" : Utilities.Common.RoundStringToDecimal(value.ToString()));
                                                dataCell.SetCellType(CellType.String);
                                                RemoveCellComment(dataCell);
                                            }
                                        }

                                    }
                                    else if (res["Name"].ToString().ToLower() == "PriceBreak8CustomerQty".ToLower() ||
                                      res["Name"].ToString().ToLower() == "PriceBreak8CustomerDiscount".ToLower() ||
                                      res["Name"].ToString().ToLower() == "PriceBreak8CustomerCostExclGST".ToLower() ||
                                      res["Name"].ToString().ToLower() == "PriceBreak8CustomerCostInclGST".ToLower())
                                    {
                                        if (!highlighColor)
                                        {
                                            dataCell.CellStyle = dataHighlightStyle;
                                            dataCell.SetCellValue(value == null ? string.Empty : value.ToString() == "0" ? "" : string.Empty);
                                            dataCell.SetCellType(CellType.String);
                                            RemoveCellComment(dataCell);
                                        }
                                        else
                                        {
                                            if (res["Name"].ToString().ToLower() == "PriceBreak8CustomerDiscount".ToLower())
                                            {
                                                dataCell.CellStyle = dataRowStylePriceDisc2;
                                                dataCell.SetCellValue(value == null ? string.Empty : value.ToString() == "0" ? "" : Utilities.Common.RoundStringToDecimalNew((Convert.ToSingle(value) * 100).ToString()) + "%");
                                                dataCell.SetCellType(CellType.String);
                                                RemoveCellComment(dataCell);
                                            }
                                            else if (res["Name"].ToString().ToLower() == "PriceBreak8CustomerQty".ToLower())
                                            {
                                                dataCell.CellStyle = dataRowStylePriceDisc2;
                                                dataCell.SetCellValue(value == null ? string.Empty : value.ToString() == "0" ? "" : value.ToString());
                                                dataCell.SetCellType(CellType.String);
                                                RemoveCellComment(dataCell);
                                            }
                                            else
                                            {
                                                dataCell.CellStyle = dataRowStylePriceDisc2;
                                                dataCell.SetCellValue(value == null ? string.Empty : value == "0" ? "" : Utilities.Common.RoundStringToDecimal(value.ToString()));
                                                dataCell.SetCellType(CellType.String);
                                                RemoveCellComment(dataCell);
                                            }
                                        }

                                    }
                                    else if(res["Name"].ToString().ToLower() == "RecommendedRetailPrice".ToLower() || res["Name"].ToString().ToLower() == "AdvertisedRecommendedRetailPrice".ToLower())
                                    {
                                        dataCell.CellStyle = dataCellStyleRight;
                                        dataCell.SetCellValue(value == null ? string.Empty : value.ToString() == "0" ? "" : Utilities.Common.RoundStringToDecimal(value.ToString()));
                                        dataCell.SetCellType(CellType.String);
                                        RemoveCellComment(dataCell);
                                    }
                                    else if (res["Name"].ToString().ToLower() == "Barcode".ToLower())
                                    {
                                        dataCell.CellStyle = dataBarcodeStyle;
                                        dataCell.SetCellValue(value == null ? string.Empty : value.ToString() == "0" ? "" :  value.ToString());
                                        dataCell.SetCellType(CellType.String);
                                        
                                    }
                                    else
                                    {
                                        if (!highlighColor)
                                        {
                                            var value_txt = string.Empty;
                                            if (value == null || value.ToString() == "0")
                                            {
                                                value_txt = string.Empty;
                                            }
                                            else
                                            {
                                                value_txt = value.ToString();
                                            }
                                            dataCell.CellStyle = dataHighlightStyle;
                                            dataCell.SetCellValue(value_txt);
                                            dataCell.SetCellType(CellType.String);
                                            RemoveCellComment(dataCell);
                                        }
                                        else
                                        {
                                            var value_txt = string.Empty;
                                            if (value == null || value.ToString() == "0")
                                            {
                                                value_txt = string.Empty;
                                            }
                                            else
                                            {
                                                value_txt = value.ToString();
                                            }
                                            var testString = value == null ? string.Empty : value.Equals(0) ? "" : value;
                                            if (align)
                                            {
                                                dataCell.CellStyle = dataStrStyle;
                                            }
                                            else
                                            {
                                                dataCell.CellStyle = dataCellStyleRight;
                                            }
                                            dataCell.SetCellValue(value_txt);
                                            dataCell.SetCellType(CellType.String);
                                            RemoveCellComment(dataCell);
                                        }

                                    }

                                    dataStrStyle.SetFont(dataFont);
                                    indxcnt++;
                                    internalCnt++;
                                }

                            }
                            rowIndex++;
                        }

                        sheet.CreateFreezePane(0, 1, 0, 1);
                        _objLoggingProvider.LogMessage(LogType.Info, "PriceFileDetails: Writting data in to excel end");

                        // defining file name for excel
                        string PFCEncryptedFileName = Guid.NewGuid().ToString() + ".xlsx";
                        Dictionary<string, string> fobj = new();
                        fobj.Add("PFCActualFileName", excelFileName + ".xlsx");
                        fobj.Add("PFCEncryptedFileName", PFCEncryptedFileName);
                        createdFileNames.Add(fobj);

                        if (!Directory.Exists(AppConfig.PFCDownloadedFileLoaction))
                            Directory.CreateDirectory(AppConfig.PFCDownloadedFileLoaction);

                        string fpath = Path.Combine(Directory.GetCurrentDirectory(), AppConfig.PFCDownloadedFileLoaction);
                        string fname = Path.Combine(AppConfig.PFCDownloadedFileLoaction, PFCEncryptedFileName);


                        using (FileStream filestream = new FileStream(fname, FileMode.Create, FileAccess.Write))
                        {
                            workbook.Write(filestream, true);
                        }
                        _objLoggingProvider.LogMessage(LogType.Info, "PriceFileDetails: Creating excel file in the path" + fname);

                        if (ConfigId != 0 && Param_PriceFileHeaderId == 0)
                        {
                            _objLoggingProvider.LogMessage(LogType.Info, "PriceFileDetails: uploaded Excel File for Customer:" + cust.zKUNNR + "and PriceFileHeaderID" + Param_PriceFileHeaderId);

                            using (ISqlHelper objSqlHelper = new SqlHelper(AppConfig.ConnectionString))
                            {
                                string strUpdateQuery = "UPDATE dbo.TRN_PriceFileLocationDetails SET Status = @Status, StatusText = @StatusText, PercentCompleted= @PercentCompleted, IsCompleted=@IsCompleted,  PFCFilePath =@PFCFilePath, PFCActualFileName= @PFCActualFileName,PFCEncryptedFileName= @PFCEncryptedFileName,PFCFileType=@PFCFileType, ModifiedDate = GetUTCDate(), PFCFileLocationMode = @PFCFileLocationMode  WHERE PriceFileHeaderID = @PriceFileHeaderID and CustomerNo = @CustomerNo ";

                                List<SqlParameter> lstSqlParameters = new List<SqlParameter>();
                                lstSqlParameters.Add(new SqlParameter() { ParameterName = "@StatusText", Value = "Excel File Generation Completed" });
                                lstSqlParameters.Add(new SqlParameter() { ParameterName = "@PercentCompleted", Value = 100 });
                                lstSqlParameters.Add(new SqlParameter() { ParameterName = "@PriceFileHeaderID", Value = Stored_priceFileHeaderId });
                                lstSqlParameters.Add(new SqlParameter() { ParameterName = "@CustomerNo", Value = cust.zKUNNR });
                                lstSqlParameters.Add(new SqlParameter() { ParameterName = "@Status", Value = "Completed" });
                                lstSqlParameters.Add(new SqlParameter() { ParameterName = "@PFCFilePath", Value = AppConfig.PFCDownloadedFileLoaction });
                                lstSqlParameters.Add(new SqlParameter() { ParameterName = "@PFCActualFileName", Value = excelFileName + ".xlsx" });
                                lstSqlParameters.Add(new SqlParameter() { ParameterName = "@PFCEncryptedFileName", Value = PFCEncryptedFileName });
                                lstSqlParameters.Add(new SqlParameter() { ParameterName = "@PFCFileType", Value = ".xlsx" });
                                lstSqlParameters.Add(new SqlParameter() { ParameterName = "@IsCompleted", Value = true });
                                lstSqlParameters.Add(new SqlParameter() { ParameterName = "@PFCFileLocationMode", Value = Constants.ApplicationServer });

                                int output = objSqlHelper.ExecuteNonQuery(CommandType.Text, strUpdateQuery, lstSqlParameters.ToArray());

                            }
                            if (SendEmail == true)
                            {
                                var customers = _objConfigureRepository.UserConfigSettingRepository.GetQueryable(q => q.UserConfigSettingID == ConfigId).FirstOrDefault().SelectedCustomers;
                                selected_cust = JsonConvert.DeserializeObject<List<SelectedCustomers>>(customers);

                                emailListCustomerWithPFId = (from pfh in _objPriceListRepository.PriceFileHeaderRepository.GetManyQueryable()
                                                             join usrconf in _objConfigureRepository.UserConfigSettingRepository.GetManyQueryable()
                                                             on pfh.UserConfigSettingID equals usrconf.UserConfigSettingID
                                                             join tmpl in _objConfigureRepository.TemplateMasterRepository.GetManyQueryable()
                                                             on usrconf.ReportContentTemplateID equals tmpl.TemplateMasterID into temptableres
                                                             from temp in temptableres.DefaultIfEmpty()
                                                             where pfh.PriceFileHeaderID == Stored_priceFileHeaderId
                                                             select new SelectedCustomersByHeaderID
                                                             {
                                                                 PriceFileHeaderID = Stored_priceFileHeaderId,
                                                                 PFCZipFileName = temp.TemplateName,
                                                                 CustomerSNO = cust.CustomerSNO,
                                                                 CustomerNumber = cust.CustomerNumber,
                                                                 CustomerName = cust.CustomerName,
                                                                 zKUNNR = cust.zKUNNR,
                                                                 PC1 = cust.PC1,
                                                                 PC2 = cust.PC2,
                                                                 PC3 = cust.PC3
                                                             }).ToList();
                                string str_CustomersWithPFID = JsonConvert.SerializeObject(emailListCustomerWithPFId);
                                var resnotify = _objNotificationProvider.SendMailToPriceFileLocationCustomers(Stored_priceFileHeaderId, str_CustomersWithPFID);

                            }
                        }
                        else if (ConfigId != 0 && Param_PriceFileHeaderId != 0)
                        {
                            _objLoggingProvider.LogMessage(LogType.Info, "PriceFileDetails: uploaded Excel File for Customer:" + cust.zKUNNR + "and PriceFileHeaderID" + Param_PriceFileHeaderId);
                            var recount = _objPriceListRepository.PriceFileLocationDetailsRepository
                                .GetQueryable(a => a.PriceFileHeaderID == Stored_priceFileHeaderId && a.CustomerNo == cust.zKUNNR)
                                .FirstOrDefault().ReDownloadCount;
                            using (ISqlHelper objSqlHelper = new SqlHelper(AppConfig.ConnectionString))
                            {
                                string strUpdateQuery = "UPDATE dbo.TRN_PriceFileLocationDetails SET ReDownloadStatus = @ReDownloadStatus, ReDownloadStatusText = @ReDownloadStatusText, ReDownloadPercentCompleted= @ReDownloadPercentCompleted, PFCFilePath =@PFCFilePath, PFCActualFileName= @PFCActualFileName,PFCEncryptedFileName= @PFCEncryptedFileName,PFCFileType=@PFCFileType, ModifiedDate = GetUTCDate(), PFCFileLocationMode = @PFCFileLocationMode,ReDownloadCount =@ReDownloadCount,IsReDownloadCompleted =@IsReDownloadCompleted  WHERE PriceFileHeaderID = @PriceFileHeaderID and CustomerNo = @CustomerNo ";

                                List<SqlParameter> lstSqlParameters = new List<SqlParameter>();
                                lstSqlParameters.Add(new SqlParameter() { ParameterName = "@ReDownloadStatusText", Value = "Excel File Generation Completed" });
                                lstSqlParameters.Add(new SqlParameter() { ParameterName = "@ReDownloadPercentCompleted", Value = 100 });
                                lstSqlParameters.Add(new SqlParameter() { ParameterName = "@PriceFileHeaderID", Value = Stored_priceFileHeaderId });
                                lstSqlParameters.Add(new SqlParameter() { ParameterName = "@CustomerNo", Value = cust.zKUNNR });
                                lstSqlParameters.Add(new SqlParameter() { ParameterName = "@ReDownloadStatus", Value = "Completed" });
                                lstSqlParameters.Add(new SqlParameter() { ParameterName = "@PFCFilePath", Value = AppConfig.PFCDownloadedFileLoaction });
                                lstSqlParameters.Add(new SqlParameter() { ParameterName = "@PFCActualFileName", Value = excelFileName + ".xlsx" });
                                lstSqlParameters.Add(new SqlParameter() { ParameterName = "@PFCEncryptedFileName", Value = PFCEncryptedFileName });
                                lstSqlParameters.Add(new SqlParameter() { ParameterName = "@PFCFileType", Value = ".xlsx" });
                                lstSqlParameters.Add(new SqlParameter() { ParameterName = "@ReDownloadCount", Value = recount + 1 });
                                lstSqlParameters.Add(new SqlParameter() { ParameterName = "@IsReDownloadCompleted", Value = true });
                                lstSqlParameters.Add(new SqlParameter() { ParameterName = "@PFCFileLocationMode", Value = ArchivedRecords });

                                int output = objSqlHelper.ExecuteNonQuery(CommandType.Text, strUpdateQuery, lstSqlParameters.ToArray());

                            }
                        }
                        _objLoggingProvider.LogMessage(LogType.Info, "PriceFileExcelCreationEnd: End of Excel Method");

                    }
                    catch (Exception ex1)
                    {
                        _objLoggingProvider.LogException("Error while Processing of Excels ", ex1);
                        double percent = _objPriceListRepository.PriceFileHeaderRepository.GetQueryable(c => c.UserConfigSettingID == ConfigId && c.IsCompleted == true).FirstOrDefault().PercentCompleted;
                        if (percent != 100)
                        {
                            using (ISqlHelper objSqlHelper = new SqlHelper(AppConfig.ConnectionString))
                            {
                                string strUpdateQuery = "UPDATE dbo.TRN_PriceFileHeader SET Status = @Status, StatusText = @StatusText, PercentCompleted= @PercentCompleted  WHERE UserConfigSettingID = @UserConfigSettingID";
                                List<SqlParameter> lstParameters = new List<SqlParameter>();
                                lstParameters.Add(new SqlParameter() { ParameterName = "@StatusText", Value = "Failed due to some technical reason. Please Try Later. " });
                                lstParameters.Add(new SqlParameter() { ParameterName = "@PercentCompleted", Value = 0 });
                                lstParameters.Add(new SqlParameter() { ParameterName = "@Status", Value = "Failed" });
                                lstParameters.Add(new SqlParameter() { ParameterName = "@UserConfigSettingID", Value = ConfigId });

                                int output = objSqlHelper.ExecuteNonQuery(CommandType.Text, strUpdateQuery, lstParameters.ToArray());

                                string UserSESA = _objCommonProvider.GetLoginUserSESA();
                                string logPriceStatement = "INSERT INTO dbo.TRN_PriceFileLog (PriceFileHeaderID,LogType,FunctionName,LogInformation,IsActive,IsDeleted,CreatedBy,CreatedDate) values ( @PriceFileHeaderID,@LogType,@FunctionName,@LogInformation,@IsActive,@IsDeleted,@CreatedBy,GETUTCDATE())";
                                List<SqlParameter> lstSqlParams = new List<SqlParameter>();
                                lstSqlParams.Add(new SqlParameter() { ParameterName = "@PriceFileHeaderID", Value = priceHeader.PriceFileHeaderID });
                                lstSqlParams.Add(new SqlParameter() { ParameterName = "@LogType", Value = "Error" });
                                lstSqlParams.Add(new SqlParameter() { ParameterName = "@FunctionName", Value = "DownloadExcelForCustomersPrices_V2" });
                                lstSqlParams.Add(new SqlParameter() { ParameterName = "@LogInformation", Value = "Error Due to Excel Generation" });
                                lstSqlParams.Add(new SqlParameter() { ParameterName = "@IsActive", Value = true });
                                lstSqlParams.Add(new SqlParameter() { ParameterName = "@IsDeleted", Value = false });
                                lstSqlParams.Add(new SqlParameter() { ParameterName = "@CreatedBy", Value = UserSESA });
                                int output1 = objSqlHelper.ExecuteNonQuery(CommandType.Text, logPriceStatement, lstSqlParams.ToArray());

                            }
                        }

                    }

                }

                return createdFileNames;
            }

            catch (Exception ex3)
            {
                _objLoggingProvider.LogException("Error while DownloadExcelForCustomersPrices ", ex3);
                using (ISqlHelper objSqlHelper = new SqlHelper(AppConfig.ConnectionString))
                {
                    string strUpdateQuery = "UPDATE dbo.TRN_PriceFileHeader SET Status = @Status, StatusText = @StatusText, PercentCompleted= @PercentCompleted  WHERE UserConfigSettingID = @UserConfigSettingID";

                    List<SqlParameter> lstParameters = new List<SqlParameter>();
                    lstParameters.Add(new SqlParameter() { ParameterName = "@StatusText", Value = "Failed due to some technical reason. Please Try Later. " });
                    lstParameters.Add(new SqlParameter() { ParameterName = "@PercentCompleted", Value = 0 });
                    lstParameters.Add(new SqlParameter() { ParameterName = "@Status", Value = "Failed" });
                    lstParameters.Add(new SqlParameter() { ParameterName = "@UserConfigSettingID", Value = ConfigId });

                    int output = objSqlHelper.ExecuteNonQuery(CommandType.Text, strUpdateQuery, lstParameters.ToArray());

                    string UserSESA = _objCommonProvider.GetLoginUserSESA();
                    string logPriceStatement = "INSERT INTO dbo.TRN_PriceFileLog (PriceFileHeaderID,LogType,FunctionName,LogInformation,IsActive,IsDeleted,CreatedBy,CreatedDate) values ( @PriceFileHeaderID,@LogType,@FunctionName,@LogInformation,@IsActive,@IsDeleted,@CreatedBy,GETUTCDATE())";
                    List<SqlParameter> lstSqlParams = new List<SqlParameter>();
                    lstSqlParams.Add(new SqlParameter() { ParameterName = "@PriceFileHeaderID", Value = priceHeader.PriceFileHeaderID });
                    lstSqlParams.Add(new SqlParameter() { ParameterName = "@LogType", Value = "Error" });
                    lstSqlParams.Add(new SqlParameter() { ParameterName = "@FunctionName", Value = "DownloadExcelForCustomersPrices_V2" });
                    lstSqlParams.Add(new SqlParameter() { ParameterName = "@LogInformation", Value = "Error Due to Excel Generation getting initial details" });
                    lstSqlParams.Add(new SqlParameter() { ParameterName = "@IsActive", Value = true });
                    lstSqlParams.Add(new SqlParameter() { ParameterName = "@IsDeleted", Value = false });
                    lstSqlParams.Add(new SqlParameter() { ParameterName = "@CreatedBy", Value = UserSESA });
                    int output1 = objSqlHelper.ExecuteNonQuery(CommandType.Text, logPriceStatement, lstSqlParams.ToArray());
                }
                throw;
            }
        }

        // Prepare/ Fetch pricefiledetails based on customer and headerid
        public IEnumerable<PriceFileDetails> GetPriceFileDetailsForCustomer(long PriceFileHeaderID, string CustomerId, bool showNotFoundMaterials)
        {
            _objLoggingProvider.LogMessage(LogType.Info, "GetPriceFileDetailsForCustomer PriceFileHeaderID:=" + PriceFileHeaderID + ", CustomerId:= " + CustomerId);

            int maxProcessorCount = Environment.ProcessorCount;
            int batchSize = 1000;
            DataTable resultDataTable = new DataTable();
            double totalRecords = GetTotalRecordsCount(PriceFileHeaderID, CustomerId);
            int numBatches = (int)Math.Ceiling((double)totalRecords / batchSize);
            Parallel.For(0, numBatches, new ParallelOptions { MaxDegreeOfParallelism = maxProcessorCount }, batchIndex =>
            {
                int offset = batchIndex * batchSize;
                DataTable batchDataTable;
                using (ISqlHelper objSqlHelper = new SqlHelper(AppConfig.ConnectionString))
                {
                    if(showNotFoundMaterials == true)
                    {
                        string sqltxt = "select * from dbo.TRN_PriceFileDetails where PriceFileHeaderID = @PriceFileHeaderID AND CustomerNo = @CustomerNo  order by PriceFileDetailID OFFSET @Offset ROWS FETCH NEXT @batchSize ROWS ONLY";
                        List<SqlParameter> lstSqlParameters = new List<SqlParameter>();
                        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@PriceFileHeaderID", Value = PriceFileHeaderID });
                        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@CustomerNo", Value = CustomerId });
                        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@Offset", Value = offset });
                        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@batchSize", Value = batchSize });
                        batchDataTable = objSqlHelper.ExecuteTable(CommandType.Text, sqltxt, lstSqlParameters.ToArray());
                    }
                    else
                    {
                        string sqltxt = "select * from dbo.TRN_PriceFileDetails where PriceFileHeaderID = @PriceFileHeaderID AND CustomerNo = @CustomerNo AND IsFound = @IsFound  order by PriceFileDetailID OFFSET @Offset ROWS FETCH NEXT @batchSize ROWS ONLY";
                        List<SqlParameter> lstSqlParameters = new List<SqlParameter>();
                        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@PriceFileHeaderID", Value = PriceFileHeaderID });
                        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@CustomerNo", Value = CustomerId });
                        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@Offset", Value = offset });
                        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@batchSize", Value = batchSize });
                        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@IsFound", Value = 1 });
                        batchDataTable = objSqlHelper.ExecuteTable(CommandType.Text, sqltxt, lstSqlParameters.ToArray());
                    }
                    

                }

                lock (resultDataTable)
                {
                    resultDataTable.Merge(batchDataTable);
                }
            });

            List<PriceFileDetails> priceDetailsList = new List<PriceFileDetails>();

            priceDetailsList = (from DataRow reader in resultDataTable.Rows
                                select new PriceFileDetails
                                {
                                    PriceFileDetailID = (long)(reader["PriceFileDetailID"]),
                                    PriceFileHeaderID = (long)(reader["PriceFileHeaderID"]),
                                    CustomerNo = reader["CustomerNo"].ToString(),
                                    Prefix = reader["Prefix"].ToString(),
                                    CustomerCatNo = reader["CustomerCatNo"].ToString(),
                                    ColourCode = reader["ColourCode"].ToString(),
                                    CustomerItemNo = reader["CustomerItemNo"].ToString(),
                                    SchneiderElectricMaterialReference = reader["SchneiderElectricMaterialReference"].ToString(),
                                    MaterialDescription = reader["MaterialDescription"].ToString(),
                                    WholesaleListPriceExclGST = (double)reader["WholesaleListPriceExclGST"],
                                    WholesaleListPriceInclGST = (double)reader["WholesaleListPriceInclGST"],
                                    Per = (double)reader["Per"],
                                    UOM = reader["UOM"].ToString(),
                                    MOQ = (int)reader["MOQ"],
                                    OrderMultiple = (double)reader["OrderMultiple"],
                                    RecommendedRetailPrice = (double)reader["RecommendedRetailPrice"],
                                    AdvertisedRecommendedRetailPrice = (double)reader["AdvertisedRecommendedRetailPrice"],
                                    PriceDerivedFrom = reader["PriceDerivedFrom"].ToString(),
                                    PriceBreak1CustomerQty = (int)reader["PriceBreak1CustomerQty"],
                                    PriceBreak1CustomerDiscount = (double)reader["PriceBreak1CustomerDiscount"],
                                    PriceBreak1CustomerCostExclGST = (double)reader["PriceBreak1CustomerCostExclGST"],
                                    PriceBreak1CustomerCostInclGST = (double)reader["PriceBreak1CustomerCostInclGST"],
                                    PriceBreak2CustomerQty = (int)reader["PriceBreak2CustomerQty"],
                                    PriceBreak2CustomerDiscount = (double)reader["PriceBreak2CustomerDiscount"],
                                    PriceBreak2CustomerCostExclGST = (double)reader["PriceBreak2CustomerCostExclGST"],
                                    PriceBreak2CustomerCostInclGST = (double)reader["PriceBreak2CustomerCostInclGST"],
                                    PriceBreak3CustomerQty = (int)reader["PriceBreak3CustomerQty"],
                                    PriceBreak3CustomerDiscount = (double)reader["PriceBreak3CustomerDiscount"],
                                    PriceBreak3CustomerCostExclGST = (double)reader["PriceBreak3CustomerCostExclGST"],
                                    PriceBreak3CustomerCostInclGST = (double)reader["PriceBreak3CustomerCostInclGST"],
                                    PriceBreak4CustomerQty = (int)reader["PriceBreak4CustomerQty"],
                                    PriceBreak4CustomerDiscount = (double)reader["PriceBreak4CustomerDiscount"],
                                    PriceBreak4CustomerCostExclGST = (double)reader["PriceBreak4CustomerCostExclGST"],
                                    PriceBreak4CustomerCostInclGST = (double)reader["PriceBreak4CustomerCostInclGST"],
                                    PriceBreak5CustomerQty = (int)reader["PriceBreak5CustomerQty"],
                                    PriceBreak5CustomerDiscount = (double)reader["PriceBreak5CustomerDiscount"],
                                    PriceBreak5CustomerCostExclGST = (double)reader["PriceBreak5CustomerCostExclGST"],
                                    PriceBreak5CustomerCostInclGST = (double)reader["PriceBreak5CustomerCostInclGST"],
                                    Barcode = reader["Barcode"].ToString(),
                                    ProductHierarchy = reader["ProductHierarchy"].ToString(),
                                    SAPCOS = reader["SAPCOS"].ToString(),
                                    CartonQty = reader["CartonQty"].ToString(),
                                    StockStatus = reader["StockStatus"].ToString(),
                                    ValidFrom = (DateTime)reader["ValidFrom"],
                                    ValidTo = (DateTime)reader["ValidTo"],
                                    FileReferenceData = reader["FileReferenceData"].ToString(),
                                    Currency = reader["Currency"].ToString(),
                                    VRG = reader["VRG"].ToString(),
                                    VRGDescription = reader["VRGDescription"].ToString(),
                                    MaterialStatus = reader["MaterialStatus"].ToString(),
                                    MainGroup = reader["MainGroup"].ToString(),
                                    MainGroupDescription = reader["MainGroupDescription"].ToString(),
                                    Group = reader["Group"].ToString(),
                                    GroupDescription = reader["GroupDescription"].ToString(),
                                    SubGroup = reader["SubGroup"].ToString(),
                                    SubGroupDescription = reader["SubGroupDescription"].ToString(),
                                    DiscGroup = reader["DiscGroup"].ToString(),
                                    DiscGroupDescription = reader["DiscGroupDescription"].ToString(),
                                    IsActive = (bool)reader["IsActive"],
                                    IsDeleted = (bool)reader["IsDeleted"],
                                    IsFound = (bool)reader["IsFound"],
                                    CreatedBy = reader["CreatedBy"].ToString(),
                                    CreatedDate = (DateTime)reader["CreatedDate"],
                                   
                                }
                                ).ToList();

            return priceDetailsList;

        }

        public ProcessBar GetGenerationHomeStatus(long PriceFileHeaderID)
        {
            try
            {
                ProcessBar rec = new();

                var PriceFileLocationDetails = _objPriceListRepository.PriceFileLocationDetailsRepository
                                    .GetManyQueryable(a => a.IsActive == true && a.PriceFileHeaderID == PriceFileHeaderID)
                                    .ToList();

                var total_Cnt = PriceFileLocationDetails.Count();
                var completed_Cnt = PriceFileLocationDetails.Where(x => x.PercentCompleted == 100).Count();


                ProcessBar objProcessBar = new ProcessBar();
                objProcessBar.ZipFileName = "Zip";
                objProcessBar.PriceFileHeaderID = rec.PriceFileHeaderID;
                var ConfigId = _objPriceListRepository.PriceFileHeaderRepository
                            .GetQueryable(s => s.PriceFileHeaderID == PriceFileHeaderID).FirstOrDefault().UserConfigSettingID;
                if (completed_Cnt == total_Cnt)
                {
                    objProcessBar.StatusPercentage = 100;
                    objProcessBar.StatusData = completed_Cnt + "/" + total_Cnt + " Generating Excel File Completed ";
                    objProcessBar.Status = "Completed";

                    var userConfig = _objConfigureRepository.UserConfigSettingRepository
                      .GetQueryable(x => x.UserConfigSettingID == ConfigId).FirstOrDefault();

                    var objTemplateMaster = _objConfigureRepository.TemplateMasterRepository
                       .GetQueryable(x => x.TemplateMasterID == userConfig.ReportContentTemplateID).FirstOrDefault();
                    if (objTemplateMaster != null)
                    {
                        objProcessBar.ZipFileName = objTemplateMaster.TemplateName;
                    }

                    _objLoggingProvider.LogMessage(LogType.Info, "ProcessBar Status:=" + objProcessBar.StatusData + " StatusPercentage Completed:=" + objProcessBar.StatusPercentage + " ,UserConfigSettingID:=" + ConfigId);
                    //return objProcessBar;
                }
                else
                {
                    var current_Rec_Status = PriceFileLocationDetails.Where(x => x.PercentCompleted < 100).OrderByDescending(x => x.PercentCompleted).FirstOrDefault();
                    objProcessBar.StatusPercentage = current_Rec_Status.PercentCompleted;
                    objProcessBar.StatusData = completed_Cnt + 1 + "/" + total_Cnt + " Generating Excel File For " + current_Rec_Status.CustomerNo + " is " + current_Rec_Status.Status;
                    _objLoggingProvider.LogMessage(LogType.Info, "ProcessBar Status:=" + objProcessBar.StatusData + " StatusPercentage Completed:=" + objProcessBar.StatusPercentage + " ,UserConfigSettingID:=" + ConfigId);

                }
                return objProcessBar;


            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Error while GetGenerationStatus ", ex);
                //throw; // while staus check not required to throw exception even if you get any exception
                ProcessBar objProcessBar = new ProcessBar();
                objProcessBar.StatusPercentage = 0;
                objProcessBar.StatusData = "Failed due to some technical reason. Please Try Later.";
                return objProcessBar;
            }
        }

        public bool MessageForAllExcelDownload(long PriceFileHeaderID)
        {
            try
            {
                var priceFileLocDetails = _objPriceListRepository.PriceFileLocationDetailsRepository
                                        .GetManyQueryable(s => s.PriceFileHeaderID == PriceFileHeaderID)
                                        .ToList();
                bool status = priceFileLocDetails.All(a => a.IsCompleted == true && a.PercentCompleted == 100);
                return status;


            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Error while showing of successfull excel download ", ex);
                throw;

            }
        }

        public int GetTradeFormatListByCustomerCountry(string CountryCode, string PC1)
        {            
            string template;
            switch (CountryCode)
            {
                case "AU":
                    switch (PC1)
                    {
                        case "ED":
                        case "EC":
                            template = "AUWholesalerOutputTemplete";
                            break;
                        default:
                            template = "AUOtherChannelOutputTemplete";
                            break;
                    }
                    break;
                case "NZ":
                    switch (PC1)
                    {
                        case "ED":
                        case "EC":
                            template = "NZRebateCustomerOutputTemplete";
                            break;
                        default:
                            template = "NZNonRebateCustomerOutputTemplete";
                            break;
                    }
                    break;
                default:
                    template = "";
                    break;
            }

            return _objConfigureRepository.ReportFormatMasterRepository
                .GetQueryable(x => x.FormatName.ToLower().Trim() == template.ToLower().Trim())                
                .Select(t => t.ReportFormatMasterID)
                .FirstOrDefault();
        }

        public CustomerSettings GetCustomerSettings(string CustomerNumber, string SalesOrganization)
        {
            var CustomerSetting = _objPriceListRepository.CustomerSettingsRepository
                .GetQueryable(x => x.CustomerNumber.Trim().ToLower() == CustomerNumber.Trim().ToLower() && x.SalesOrganization.Trim().ToUpper() == SalesOrganization.Trim().ToUpper() && x.IsActive == true)
                .FirstOrDefault();
            return CustomerSetting;
        }

        public QueueModel SaveApiRequests(QueueModel queueModel)
        {
            try
            {                
                _objLoggingProvider.LogMessage(LogType.Info, "Start: Save API requests in Queue");
                using var scope = new TransactionScope();
                
                queueModel.CreatedDate = DateTime.UtcNow;                
                queueModel.IsActive = true;

                queueModel = _objPriceListRepository.QueueModelRepository.InsertEntity(queueModel);
                scope.Complete();
                _objLoggingProvider.LogMessage(LogType.Info, "Start: Save API requests in Queue");
                return queueModel;
            }
            catch(Exception ex)
            {
                _objLoggingProvider.LogException("Error while saving API requests in Queue: ", ex);
                throw;
            }
        }


        #region active prallel processing using datatable

        //used in parallel start
        public long GetTotalRecordsCount(long id, string CustomerId)
        {
            Int32 total = 0;
            try
            {
                using (ISqlHelper objSqlHelper = new SqlHelper(AppConfig.ConnectionString))
                {
                    string strQuery = "select count(*) from dbo.TRN_PriceFileDetails where PriceFileHeaderID = @PriceFileHeaderID and CustomerNo = @CustomerNo ";
                    List<SqlParameter> lstParameters = new List<SqlParameter>();
                    lstParameters.Add(new SqlParameter() { ParameterName = "@PriceFileHeaderID", Value = id });
                    lstParameters.Add(new SqlParameter() { ParameterName = "@CustomerNo", Value = CustomerId });
                    object totalRows = objSqlHelper.ExecuteScalarQuery(CommandType.Text, strQuery, lstParameters.ToArray());
                    total = (Int32)totalRows;
                }
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Error while DownloadExcelForCustomersPrices ", ex);
                throw;
            }

            return total;
        }

        public DataTable FetchParallelRecords()
        {
            int maxProcessorCount = Environment.ProcessorCount;
            int batchSize = 1000;
            DataTable resultDataTable = new DataTable();
            double totalRecords = GetTotalRecordsCount(13, "0000024736");
            int numBatches = (int)Math.Ceiling((double)totalRecords / batchSize);
            Parallel.For(0, numBatches, new ParallelOptions { MaxDegreeOfParallelism = maxProcessorCount }, batchIndex =>
            {
                int offset = batchIndex * batchSize;
                DataTable batchDataTable;
                using (ISqlHelper objSqlHelper = new SqlHelper(AppConfig.ConnectionString))
                {
                    //string sqltxt = "select * from (select *, ROW_NUMBER() OVER (ORDER BY PriceFileDetailID) As RowNum From [dbo].[TRN_PriceFileDetails]) As T where PriceFileHeaderID = @PriceFileHeaderID AND RowNum > @StartIndex AND RowNum<= @EndIndex";
                    string sqltxt = "select * from dbo.TRN_PriceFileDetails where PriceFileHeaderID = @PriceFileHeaderID order by PriceFileDetailID OFFSET @Offset ROWS FETCH NEXT @batchSize ROWS ONLY";
                    List<SqlParameter> lstSqlParameters = new List<SqlParameter>();
                    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@PriceFileHeaderID", Value = 13 });
                    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@Offset", Value = offset });
                    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@batchSize", Value = batchSize });
                    batchDataTable = objSqlHelper.ExecuteTable(CommandType.Text, sqltxt, lstSqlParameters.ToArray());

                }

                lock (resultDataTable)
                {
                    resultDataTable.Merge(batchDataTable);
                }
            });
            return resultDataTable;


        }

        //used in parallel end


        #endregion

        #region testing parallel processing methods
        //private DataTable FetchDataChunk(long StartIndex, long endIndex)
        //{
        //    DataTable dtRes;
        //    List<PriceFileDetails> prfdetails = new List<PriceFileDetails>();
        //    using (ISqlHelper objSqlHelper = new SqlHelper(AppConfig.ConnectionString))
        //    {
        //        //string sqltxt = "select * from (select *, ROW_NUMBER() OVER (ORDER BY PriceFileDetailID) As RowNum From [dbo].[TRN_PriceFileDetails]) As T where PriceFileHeaderID = @PriceFileHeaderID AND RowNum > @StartIndex AND RowNum<= @EndIndex";
        //        string sqltxt = "select * From dbo.TRN_PriceFileDetails where PriceFileHeaderID = @PriceFileHeaderID AND PriceFileDetailID between @StartIndex AND @EndIndex";
        //        List<SqlParameter> lstSqlParameters = new List<SqlParameter>();
        //        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@PriceFileHeaderID", Value = 9 });
        //        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@StartIndex", Value = StartIndex });
        //        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@EndIndex", Value = endIndex });
        //        dtRes = objSqlHelper.ExecuteTable(CommandType.Text, sqltxt, lstSqlParameters.ToArray());
        //        return dtRes;
        //    }
        //}

        //public DataTable FetchDataInParallel()
        //{
        //    DataTable resultDataTable = new DataTable();
        //    using (ISqlHelper objSqlHelper = new SqlHelper(AppConfig.ConnectionString))
        //    {
        //        string strQuery = "select count(*) from dbo.TRN_PriceFileDetails where PriceFileHeaderID = @PriceFileHeaderID";
        //        List<SqlParameter> lstParameters = new List<SqlParameter>();
        //        lstParameters.Add(new SqlParameter() { ParameterName = "@PriceFileHeaderID", Value = 9 });
        //        object totalRows = objSqlHelper.ExecuteScalarQuery(CommandType.Text, strQuery, lstParameters.ToArray());

        //        string strQy = "select top 1 * from dbo.TRN_PriceFileDetails where PriceFileHeaderID = @PriceFileHeaderID order by 1 asc";
        //        List<SqlParameter> lstParames = new List<SqlParameter>();
        //        lstParames.Add(new SqlParameter() { ParameterName = "@PriceFileHeaderID", Value = 9 });
        //        object startRows = objSqlHelper.ExecuteScalarQuery(CommandType.Text, strQy, lstParames.ToArray());

        //        int chunkSize = (Int32)totalRows / Environment.ProcessorCount;
        //        int taskCount = Environment.ProcessorCount;
        //        long startIndex = (Int64)startRows;
        //        long endIndex = startIndex + chunkSize - 1;
        //        long remainingRows = (Int32)totalRows;

        //        Task<DataTable>[] tasks = new Task<DataTable>[taskCount];
        //        for (int i = 0; i < taskCount; i++)
        //        {
        //            if (remainingRows <= 0)
        //            {
        //                break;
        //            }

        //            tasks[i] = Task.Run(() => FetchDataChunk(startIndex, endIndex));
        //            //tasks[i] = Task.Run(() =>
        //            //{
        //            //    DataTable chunkDataTable = new DataTable();
        //            //    using (ISqlHelper objSqlHelper = new SqlHelper(AppConfig.ConnectionString))
        //            //    {
        //            //        string sqltxt = "select * From dbo.TRN_PriceFileDetails where PriceFileHeaderID = @PriceFileHeaderID AND PriceFileDetailID between @StartIndex AND @EndIndex";
        //            //        List<SqlParameter> lstSqlParameters = new List<SqlParameter>();
        //            //        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@PriceFileHeaderID", Value = 9 });
        //            //        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@StartIndex", Value = startIndex });
        //            //        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@EndIndex", Value = endIndex });
        //            //        chunkDataTable = objSqlHelper.ExecuteTable(CommandType.Text, sqltxt, lstSqlParameters.ToArray());
        //            //    }
        //            //    return chunkDataTable;
        //            //});

        //            startIndex = endIndex + 1;
        //            endIndex = startIndex + chunkSize - 1;
        //            remainingRows -= chunkSize;
        //        }
        //        Task.WaitAll(tasks);
        //        foreach (var task in tasks)
        //        {

        //            resultDataTable.Merge(task.Result);
        //        }
        //    }
        //    return resultDataTable;
        //}

        //public IEnumerable<PriceFileDetails> FetchParallelReaderRecords()
        //{
        //    List<PriceFileDetails> resultList = new List<PriceFileDetails>();

        //    using (ISqlHelper objSqlHelper = new SqlHelper(AppConfig.ConnectionString))
        //    {
        //        string strQuery = "select count(*) from dbo.TRN_PriceFileDetails where PriceFileHeaderID = @PriceFileHeaderID";
        //        List<SqlParameter> lstParameters = new List<SqlParameter>();
        //        lstParameters.Add(new SqlParameter() { ParameterName = "@PriceFileHeaderID", Value = 9 });
        //        object totalRows = objSqlHelper.ExecuteScalarQuery(CommandType.Text, strQuery, lstParameters.ToArray());

        //        int chunkSize = (Int32)totalRows / Environment.ProcessorCount;
        //        int taskCount = Environment.ProcessorCount;

        //        Task[] tasks = new Task[taskCount];
        //        int startIndex = 1;
        //        for (int i = 0; i < taskCount; i++)
        //        {
        //            int endIndex = (i == taskCount - 1) ? (Int32)totalRows : startIndex + chunkSize - 1;
        //            tasks[i] = Task.Run(() => FetchDataChunk(startIndex, endIndex, resultList));
        //            startIndex = endIndex + 1;
        //        }
        //        Task.WaitAll(tasks);

        //    }

        //    return resultList;

        //}

        //private void FetchDataChunk(int startIndex, int endIndex, List<PriceFileDetails> resultList)
        //{
        //    using (ISqlHelper objSqlHelper = new SqlHelper(AppConfig.ConnectionString))
        //    {
        //        string sqltxt = "select * from (select  ROW_NUMBER() OVER (ORDER BY PriceFileDetailID) As RowNum,* From [dbo].[TRN_PriceFileDetails] where PriceFileHeaderID = 9) AS T WHERE RowNum>= @StartIndex and RowNum<= @EndIndex";
        //        List<SqlParameter> lstSqlParameters = new List<SqlParameter>();
        //        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@PriceFileHeaderID", Value = 9 });
        //        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@StartIndex", Value = startIndex });
        //        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@EndIndex", Value = endIndex });
        //        SqlDataReader queryResult = objSqlHelper.ExecuteReaderQuery(CommandType.Text, sqltxt, lstSqlParameters.ToArray());

        //        using (SqlDataReader reader = queryResult)
        //        {
        //            while (reader.Read())
        //            {
        //                PriceFileDetails model = new();
        //                model.PriceFileDetailID = (long)reader["PriceFileDetailID"];
        //                model.PriceFileHeaderID = (long)reader["PriceFileHeaderID"];
        //                model.CustomerNo = reader["CustomerNo"].ToString();
        //                model.Prefix = reader["Prefix"].ToString();
        //                model.CustomerCatNo = reader["CustomerCatNo"].ToString();
        //                model.ColourCode = reader["ColourCode"].ToString();
        //                model.CustomerItemNo = reader["CustomerItemNo"].ToString();
        //                model.SchneiderElectricMaterialReference = reader["SchneiderElectricMaterialReference"].ToString();
        //                model.MaterialDescription = reader["MaterialDescription"].ToString();
        //                model.WholesaleListPriceExclGST = (double)reader["WholesaleListPriceExclGST"];
        //                model.WholesaleListPriceInclGST = (double)reader["WholesaleListPriceInclGST"];
        //                model.Per = (double)reader["Per"];
        //                model.UOM = reader["UOM"].ToString();
        //                model.MOQ = (int)reader["MOQ"];
        //                model.OrderMultiple = (double)reader["OrderMultiple"];
        //                model.RecommendedRetailPrice = (double)reader["RecommendedRetailPrice"];
        //                model.AdvertisedRecommendedRetailPrice = (double)reader["AdvertisedRecommendedRetailPrice"];
        //                model.PriceDerivedFrom = reader["PriceDerivedFrom"].ToString();
        //                model.PriceBreak1CustomerQty = (int)reader["PriceBreak1CustomerQty"];
        //                model.PriceBreak1CustomerDiscount = (double)reader["PriceBreak1CustomerDiscount"];
        //                model.PriceBreak1CustomerCostExclGST = (double)reader["PriceBreak1CustomerCostExclGST"];
        //                model.PriceBreak1CustomerCostInclGST = (double)reader["PriceBreak1CustomerCostInclGST"];
        //                model.PriceBreak2CustomerQty = (int)reader["PriceBreak2CustomerQty"];
        //                model.PriceBreak2CustomerDiscount = (double)reader["PriceBreak2CustomerDiscount"];
        //                model.PriceBreak2CustomerCostExclGST = (double)reader["PriceBreak2CustomerCostExclGST"];
        //                model.PriceBreak2CustomerCostInclGST = (double)reader["PriceBreak2CustomerCostInclGST"];
        //                model.PriceBreak3CustomerQty = (int)reader["PriceBreak3CustomerQty"];
        //                model.PriceBreak3CustomerDiscount = (double)reader["PriceBreak3CustomerDiscount"];
        //                model.PriceBreak3CustomerCostExclGST = (double)reader["PriceBreak3CustomerCostExclGST"];
        //                model.PriceBreak3CustomerCostInclGST = (double)reader["PriceBreak3CustomerCostInclGST"];
        //                model.PriceBreak4CustomerQty = (int)reader["PriceBreak4CustomerQty"];
        //                model.PriceBreak4CustomerDiscount = (double)reader["PriceBreak4CustomerDiscount"];
        //                model.PriceBreak4CustomerCostExclGST = (double)reader["PriceBreak4CustomerCostExclGST"];
        //                model.PriceBreak4CustomerCostInclGST = (double)reader["PriceBreak4CustomerCostInclGST"];
        //                model.PriceBreak5CustomerQty = (int)reader["PriceBreak5CustomerQty"];
        //                model.PriceBreak5CustomerDiscount = (double)reader["PriceBreak5CustomerDiscount"];
        //                model.PriceBreak5CustomerCostExclGST = (double)reader["PriceBreak5CustomerCostExclGST"];
        //                model.PriceBreak5CustomerCostInclGST = (double)reader["PriceBreak5CustomerCostInclGST"];
        //                model.Barcode = reader["Barcode"].ToString();
        //                model.ProductHierarchy = reader["ProductHierarchy"].ToString();
        //                model.SAPCOS = reader["SAPCOS"].ToString();
        //                model.CartonQty = reader["CartonQty"].ToString();
        //                model.StockStatus = reader["StockStatus"].ToString();
        //                model.ValidFrom = (DateTime)reader["ValidFrom"];
        //                model.ValidTo = (DateTime)reader["ValidTo"];
        //                model.FileReferenceData = reader["FileReferenceData"].ToString();
        //                model.Currency = reader["Currency"].ToString();
        //                model.VRG = reader["VRG"].ToString();
        //                model.VRGDescription = reader["VRGDescription"].ToString();
        //                model.MaterialStatus = reader["MaterialStatus"].ToString();
        //                model.MainGroup = reader["MainGroup"].ToString();
        //                model.MainGroupDescription = reader["MainGroupDescription"].ToString();
        //                model.Group = reader["Group"].ToString();
        //                model.GroupDescription = reader["GroupDescription"].ToString();
        //                model.SubGroup = reader["SubGroup"].ToString();
        //                model.SubGroupDescription = reader["SubGroupDescription"].ToString();
        //                model.IsActive = (bool)reader["IsActive"];
        //                model.IsDeleted = (bool)reader["IsDeleted"];
        //                model.CreatedBy = reader["CreatedBy"].ToString();
        //                model.CreatedDate = (DateTime)reader["CreatedDate"];

        //                lock (resultList)
        //                {
        //                    resultList.Add(model);
        //                }
        //            }
        //        }

        //    }
        //}

        #endregion

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
            if (_objPriceListRepository != null && isDispose)
            {
                _objPriceListRepository.Dispose();
            }
            if (_objLoggingProvider != null && isDispose)
            {
                _objLoggingProvider.Dispose();
            }
            if (_objCommonProvider != null && isDispose)
            {
                _objCommonProvider.Dispose();
            }
            if (_objConfigureRepository != null && isDispose)
            {
                _objConfigureRepository.Dispose();
            }
            if (_objNotificationProvider != null && isDispose)
            {
                _objNotificationProvider.Dispose();
            }
        }

        #endregion
    }
}
