using ExcelUploadValidator.Interfaces;
using ExcelUploadValidator.ServiceProviders;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using NPOI.XSSF.Streaming.Values;
using Org.BouncyCastle.Ocsp;
using PFCWebAPP.DatabaseContext.Models.CustomTables;
using PFCWebAPP.Extensions;
using PFCWebAPP.Repositories.BackOps;
using PFCWebAPP.Repositories.Common.Enums;
using PFCWebAPP.Repositories.Common.Interfaces;
using PFCWebAPP.Repositories.Common.ServiceProviders;
using PFCWebAPP.Repositories.Configure.Interfaces;
using PFCWebAPP.Repositories.Configure.Models;
using PFCWebAPP.Repositories.PriceList;
using PFCWebAPP.Utilities;
using System.Data;
using System.Linq;
using System.Transactions;

namespace PFCWebAPP.Repositories.Configure.ServiceProviders
{
    public class ConfigureProvider : IConfigureProvider
    {
        private readonly ILoggingProvider _objLoggingProvider;
        private readonly IConfigureRepository _objConfigureRepository;
        private readonly IHttpContextAccessor _objHttpContextAccessor;
        public readonly ICommonProvider _objCommonProvider;
        public readonly IPriceListRepository _objPriceListRepository;
        public readonly IBackOpsRepository _objBackOpsRepository;

        public ConfigureProvider(ILoggingProvider objLoggingProvider, IConfigureRepository objConfigureRepository, IHttpContextAccessor objHttpContextAccessor, ICommonProvider objCommonProvider, IPriceListRepository objPriceListRepository, IBackOpsRepository objBackOpsRepository)
        {
            _objLoggingProvider = objLoggingProvider;
            _objConfigureRepository = objConfigureRepository;
            _objHttpContextAccessor = objHttpContextAccessor;
            _objCommonProvider = objCommonProvider;
            _objPriceListRepository = objPriceListRepository;
            _objBackOpsRepository = objBackOpsRepository;
        }


        #region ReportFormat
        /// <summary>
        /// GetReportFormats
        /// </summary>
        /// <returns>lst_reportformats</returns>
        /// param : 
        public IQueryable<ReportFormatMaster> GetReportFormats()
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: GetReportFormats");
                IQueryable<ReportFormatMaster>? lst_reportformats = null;
                lst_reportformats = _objConfigureRepository.ReportFormatMasterRepository.GetManyQueryable().Where(x => x.IsActive == true);
                _objLoggingProvider.LogMessage(LogType.Info, "End: GetReportFormats");
                return lst_reportformats;
            }
            catch (Exception)
            {
                throw;
            }

        }


        /// <summary>
        /// GetReportFormatDetailsByID
        /// </summary>
        /// <returns>rfDetailsById</returns>
        /// param : TemplateMasterID
        public List<ReportFormatDataTableViewModel> GetReportFormatDetailsByID(int TemplateMasterID)
        {
            List<ReportFormatDataTableViewModel> rfDetails = new();
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: GetReportFormatDetailsByID");
                //List<ReportFormatFieldMapping> lst_rfmap = _objConfigureRepository.ReportFormatFieldMappingRepository.GetAllEntities(x => x.IsActive == true).ToList();
                //List<ReportFormatFieldMaster> lst_rfeildmaster = _objConfigureRepository.ReportFormatFieldMasterRepository.GetAllEntities(y => y.IsActive == true).ToList();
                //List<ReportFormatMaster> lst_rfmaster = _objConfigureRepository.ReportFormatMasterRepository.GetAllEntities(z => z.IsActive == true).ToList();

                // int reportFormatMasterID = TemplateMasterID;
                var rfDetailsById = (from rfm in _objConfigureRepository.ReportFormatFieldMappingRepository.GetManyQueryable().Where(x => x.IsActive == true && x.IsDeleted == false)
                                     join rffm in _objConfigureRepository.ReportFormatFieldMasterRepository.GetManyQueryable().Where(y => y.IsActive == true && y.IsDeleted == false) on rfm.ReportFormatFieldMasterID equals rffm.ReportFormatFieldMasterID
                                     join fm in _objConfigureRepository.ReportFormatMasterRepository.GetManyQueryable().Where(z => z.IsActive == true && z.IsDeleted == false) on rfm.ReportFormatMasterID equals fm.ReportFormatMasterID
                                     where rfm.ReportFormatMasterID == TemplateMasterID
                                     orderby rfm.SequenceNo
                                     select new ReportFormatDataTableViewModel
                                     {
                                         ExcelFieldName = rfm.AliasName,
                                         FieldName = rffm.FieldName,
                                         SequenceNo = Convert.ToInt32(rfm.SequenceNo)
                                     }
                            ).ToList();
                _objLoggingProvider.LogMessage(LogType.Info, "End: GetReportFormatDetailsByID");
                return rfDetailsById;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        /// <summary>
        /// GetTemplatesByCategory
        /// </summary>
        /// <param name="CategoryName"></param>
        /// <returns></returns>
        public List<TemplateMasterDetails> GetTemplatesByCategory(string CategoryName)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: GetTemplatesByCategory");

                var lstTemplateMasterDetailsInfo = (from TC in _objConfigureRepository.TemplateCategoryRepository.GetManyQueryable()
                                                    join TM in _objConfigureRepository.TemplateMasterRepository.GetManyQueryable()
                                                    on TC.TemplateCategoryID equals TM.TemplateCategoryID
                                                    where TC.IsActive == true && TM.IsActive == true &&
                                                    TC.IsDeleted == false && TM.IsDeleted == false &&
                                                    TC.CategoryName.Trim().ToUpper() == CategoryName.Trim().ToUpper()
                                                    orderby TM.TemplateMasterID
                                                    select new TemplateMasterDetails
                                                    {
                                                        TemplateMasterID = TM.TemplateMasterID,
                                                        TemplateCategoryID = TM.TemplateCategoryID,
                                                        TemplateCategoryName = TC.CategoryName,
                                                        TemplateName = TM.TemplateName,
                                                        AliasName = TM.AliasName,
                                                        TemplateDataModel = TM.TemplateDataModel,
                                                        CountryCode = TM.CountryCode,
                                                        CanDuplicate = TM.CanDuplicate,
                                                        CanUpload = TM.CanUpload,
                                                        IsActive = TM.IsActive
                                                    }).ToList();

                _objLoggingProvider.LogMessage(LogType.Info, "End: GetTemplatesByCategory");



                return lstTemplateMasterDetailsInfo;

            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Error while GetTemplateMasterDetailsModel", ex);
                throw;
            }
        }

        /// <summary>
        /// GetTemplateMasterDetailsByTemplateID
        /// </summary>
        /// <param name="TemplateMasterID"></param>
        /// <returns></returns>
        public TemplateMasterDetails GetTemplateMasterDetailsByTemplateID(int TemplateMasterID)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: GetTemplateMasterDetailsByTemplateID");

                var objTemplateMasterDetailsModel = (from TC in _objConfigureRepository.TemplateCategoryRepository.GetManyQueryable()
                                                     join TM in _objConfigureRepository.TemplateMasterRepository.GetManyQueryable()
                                                     on TC.TemplateCategoryID equals TM.TemplateCategoryID
                                                     where TM.TemplateMasterID == TemplateMasterID && TC.IsActive == true && TM.IsActive == true &&
                                                     TC.IsDeleted == false && TM.IsDeleted == false
                                                     orderby TM.TemplateMasterID
                                                     select new TemplateMasterDetails
                                                     {
                                                         TemplateMasterID = TM.TemplateMasterID,
                                                         TemplateCategoryID = TM.TemplateCategoryID,
                                                         TemplateCategoryName = TC.CategoryName,
                                                         TemplateName = TM.TemplateName,
                                                         AliasName = TM.AliasName,
                                                         TemplateDataModel = TM.TemplateDataModel,
                                                         CountryCode = TM.CountryCode,
                                                         CanDuplicate = TM.CanDuplicate,
                                                         CanUpload = TM.CanUpload,
                                                         IsActive = TM.IsActive
                                                     }).FirstOrDefault();


                _objLoggingProvider.LogMessage(LogType.Info, "End: GetTemplateMasterDetailsByTemplateID");
                return objTemplateMasterDetailsModel;

            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Error while GetTemplateMasterDetailsModel", ex);
                throw;
            }
        }


        /// <summary>
        /// GetTemplateStructureIntoByTemplateID
        /// </summary>
        /// <param name="TemplateMasterID"></param>
        /// <returns></returns>
        public List<TemplateStructure> GetTemplateStructureIntoByTemplateID(int TemplateMasterID)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: GetTemplatesByCategory");

                var lstTemplateStructure = (from TC in _objConfigureRepository.TemplateStructureRepository.GetManyQueryable()
                                            where TC.TemplateMasterID == TemplateMasterID && TC.IsActive == true && TC.IsDeleted == false
                                            orderby TC.SequenceNo
                                            select TC).ToList();

                _objLoggingProvider.LogMessage(LogType.Info, "End: GetTemplatesByCategory");



                return lstTemplateStructure;

            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Error while GetTemplateMasterDetailsModel", ex);
                throw;
            }
        }


        /// <summary>
        /// GetTemplateDataByTemplateID
        /// </summary>
        /// <param name="TemplateMasterID"></param>
        /// <returns></returns>
        public DataTable GetTemplateDataByTemplateID(int TemplateMasterID,int DisplayMaxRecords =0)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: GetTemplateDataByTemplateID");
                DataTable Dt = new DataTable();
                string LoginUserSESA = _objCommonProvider.GetLoginUserSESA();
                var ObjTemplateMaster = _objConfigureRepository.TemplateMasterRepository.GetManyQueryable().Where(x => x.TemplateMasterID == TemplateMasterID && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                if (ObjTemplateMaster != null)
                {
                    if (ObjTemplateMaster.TemplateDataModel.ToUpper().Trim() == "JSON".ToUpper().Trim())
                    {
                        _objLoggingProvider.LogMessage(LogType.Info, "Start: GetTemplateData In Json");
                        var objTemplateData = _objConfigureRepository.TemplateDataRepository.GetManyQueryable().Where(x => x.TemplateMasterID == TemplateMasterID && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                        if (objTemplateData != null)
                        {
                            if (objTemplateData.Data != null)
                            {
                                Dt = (DataTable)JsonConvert.DeserializeObject(objTemplateData.Data, typeof(DataTable));
                            }


                        }
                        _objLoggingProvider.LogMessage(LogType.Info, "End: GetTemplateData In Json");

                    }

                    else if (ObjTemplateMaster.TemplateDataModel.ToUpper().Trim() == "Table".ToUpper().Trim())
                    {
                        if (ObjTemplateMaster.TemplateName.ToUpper().Trim() == "MaterialMasterList".ToUpper().Trim())
                        {
                            _objLoggingProvider.LogMessage(LogType.Info, "Start: Get MaterialList from MaterialMasterRepository");

                            var lstMaterialList = (from n in _objConfigureRepository.MaterialMasterRepository.GetManyQueryable()
                                                   where n.IsActive == true && n.IsDeleted == false
                                                   select new
                                                   {
                                                       Prefix = n.Prefix,
                                                       ColourCode = n.ColourCode,
                                                       CatNo = n.CatNo,
                                                       ItemNo = n.ItemNo,
                                                       InternalSAPItemNo = n.InternalSAPItemNo,
                                                       SplitPackQty = n.SplitPackQty
                                                   });
                            _objLoggingProvider.LogMessage(LogType.Info, "InProgress: Get MaterialList from MaterialMasterRepository");
                            if (DisplayMaxRecords > 0)
                            {
                                DisplayMaxRecords = DisplayMaxRecords + 1;
                                Dt = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(lstMaterialList.Take(DisplayMaxRecords).ToList()));
                            }
                            else
                            {
                                Dt = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(lstMaterialList.ToList()));
                            }
                            _objLoggingProvider.LogMessage(LogType.Info, "End: Get MaterialList from MaterialMasterRepository");


                        }
                        else if (ObjTemplateMaster.TemplateName.ToUpper().Trim() == "CustomerContacts".ToUpper().Trim())
                        {
                            _objLoggingProvider.LogMessage(LogType.Info, "Start: Get CustomerContacts from CustomerContactsRepository");

                            var lstCustomerContactList = (from n in _objConfigureRepository.CustomerContactRepository.GetManyQueryable()
                                                   where n.IsActive == true && n.IsDeleted == false
                                                   select new
                                                   {
                                                       AccountNumber = n.AccountNumber,
                                                       AccountName = n.AccountName,
                                                       ContactPerson = n.ContactPerson,
                                                       ToEmailID = n.ToEmailID,
                                                       CcEmailID = n.CcEmailID,
                                                       BccEmailID = n.BccEmailID
                                                   });
                            _objLoggingProvider.LogMessage(LogType.Info, "InProgress: Get CustomerContacts from CustomerContactsRepository");
                            if (DisplayMaxRecords > 0)
                            {
                                DisplayMaxRecords = DisplayMaxRecords + 1;
                                Dt = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(lstCustomerContactList.Take(DisplayMaxRecords).ToList()));
                            }
                            else
                            {
                                Dt = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(lstCustomerContactList.ToList()));
                            }
                            _objLoggingProvider.LogMessage(LogType.Info, "End: Get MaterialList from CustomerContactsRepository");


                        }

                    }


                    //else if (ObjTemplateMaster.TemplateDataModel.ToUpper().Trim() == "NONE".ToUpper().Trim())
                    //{

                    //    using (ISqlHelper objSqlHelper = new SqlHelper(AppConfig.ConnectionString))
                    //    {
                    //        // to avoid list to datatable conversion i am using here inline query
                    //        string strQuery = "select [PropertyName] as FieldName ,[PropertyDescription] as FieldDescription,SequenceNo from [dbo].[MST_TemplateStructure] where TemplateMasterID =@TemplateMasterID and isnull(IsActive,0) = 1 and isnull(IsDeleted,0) =0 order by SequenceNo";

                    //        List<SqlParameter> lstSqlParameters = new List<SqlParameter>();
                    //        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@TemplateMasterID", Value = TemplateMasterID });

                    //        Dt = objSqlHelper.ExecuteTable(CommandType.Text, strQuery, lstSqlParameters.ToArray());

                    //    }



                    //}

                }




                _objLoggingProvider.LogMessage(LogType.Info, "End: GetTemplateDataByTemplateID");

                return Dt;

            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Error while GetTemplateDataByTemplateID", ex);
                throw;
            }
        }



        public DataSet GetTemplateDataByTemplateIDV2(int TemplateMasterID, int DisplayMaxRecords = 0)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: GetTemplateDataByTemplateID");
                DataSet Ds = new DataSet();
                string LoginUserSESA = _objCommonProvider.GetLoginUserSESA();
                var ObjTemplateMaster = _objConfigureRepository.TemplateMasterRepository.GetManyQueryable().Where(x => x.TemplateMasterID == TemplateMasterID && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                if (ObjTemplateMaster != null)
                {
                    string ResultFlagValue;
                    string ResultValue;
                    using (ISqlHelper objSqlHelper = new SqlHelper(AppConfig.ConnectionString))
                    {
                        List<SqlParameter> lstSqlParameters = new List<SqlParameter>();
                        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@TemplateMasterID", Value = ObjTemplateMaster.TemplateMasterID });
                        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@TemplateCategoryID", Value = ObjTemplateMaster.TemplateCategoryID });
                        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@TemplateName", Value = ObjTemplateMaster.TemplateName });
                        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@DisplayMaxRecords", Value = DisplayMaxRecords });

                        SqlParameter ResultFlag = new SqlParameter("@ResultFlag", SqlDbType.NVarChar, 25);
                        ResultFlag.Direction = ParameterDirection.Output;
                        lstSqlParameters.Add(ResultFlag);

                        SqlParameter Result = new SqlParameter("@Result", SqlDbType.NVarChar, 250);
                        Result.Direction = ParameterDirection.Output;
                        lstSqlParameters.Add(Result);

                        Ds = objSqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "USPM_GetTemplateDataByTemplateID", lstSqlParameters.ToArray());
                        ResultFlagValue = ResultFlag.Value.ToString();
                        ResultValue = Result.ToString();

                    }
                }

                _objLoggingProvider.LogMessage(LogType.Info, "End: GetTemplateDataByTemplateID");

                return Ds;

            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Error while GetTemplateDataByTemplateID", ex);
                throw;
            }
        }



        /// <summary>
        /// UpdateExcelDataIntoTemplateTables
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="TemplateMasterID"></param>
        /// <returns></returns>
        public bool UpdateExcelDataIntoTemplateTables(string FileName, int TemplateMasterID)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: UpdateExcelDataIntoTemplateTables");
                string LoginUserSESA = _objCommonProvider.GetLoginUserSESA();
                var ObjTemplateMaster = _objConfigureRepository.TemplateMasterRepository.GetManyQueryable().Where(x => x.TemplateMasterID == TemplateMasterID && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                if (ObjTemplateMaster != null)
                {
                    DataTable dtTemplateData;

                    using (IUploadValidator objUploadValidator = new UploadValidator())
                    {
                        string fpath = Path.Combine(Directory.GetCurrentDirectory(), PFCWebAPP.Utilities.AppConfig.TemplateFilepath);
                        string fname = Path.Combine(PFCWebAPP.Utilities.AppConfig.TemplateFilepath, FileName);
                        dtTemplateData = objUploadValidator.ConvertExcelDataIntoDataTable(fname, fpath);
                    }
                    if (dtTemplateData != null)
                    {
                        if (ObjTemplateMaster.TemplateDataModel.ToUpper().Trim() == "JSON".ToUpper().Trim())
                        {
                            var JsonData = dtTemplateData.Serialize();

                            using (var scope = new TransactionScope())
                            {
                                try
                                {
                                    TemplateData objTemplateData = _objConfigureRepository.TemplateDataRepository.GetManyQueryable().Where(x => x.TemplateMasterID == TemplateMasterID && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                                    if (objTemplateData != null)
                                    {
                                        objTemplateData.Data = JsonData;
                                        objTemplateData.ModifiedDate = DateTime.UtcNow;
                                        objTemplateData.ModifiedBy = LoginUserSESA;
                                        _objConfigureRepository.TemplateDataRepository.UpdateEntity(objTemplateData);

                                    }
                                    else
                                    {
                                        TemplateData TD = new TemplateData();
                                        TD.TemplateMasterID = TemplateMasterID;
                                        TD.Data = JsonData;
                                        TD.CreatedBy = LoginUserSESA;
                                        TD.CreatedDate = DateTime.UtcNow;
                                        TD.ModifiedDate = DateTime.UtcNow;
                                        TD.ModifiedBy = LoginUserSESA;
                                        TD.IsActive = true;
                                        _objConfigureRepository.TemplateDataRepository.InsertEntity(TD);

                                    }


                                    scope.Complete();
                                    _objLoggingProvider.LogMessage(LogType.Info, "End: UpdateExcelDataIntoTemplateTables");
                                    return true;
                                }
                                catch (Exception ex)
                                {
                                    scope.Dispose();
                                    _objLoggingProvider.LogException("Error while UpdateExcelDataIntoTemplateTables", ex);
                                    return false;

                                }
                            }

                        }
                        else if (ObjTemplateMaster.TemplateDataModel.ToUpper().Trim() == "Table".ToUpper().Trim())
                        {
                            int BatchUpdateMAX_RECORDS = 0;
                            try
                            {
                                string BulkUpdateRecordCount = _objCommonProvider.GetAppSettingByName(Constants.BulkUpdateRecordCount);
                                if (!string.IsNullOrEmpty(BulkUpdateRecordCount))
                                {
                                    BatchUpdateMAX_RECORDS = Convert.ToInt32(BulkUpdateRecordCount);
                                }

                            }
                            catch (Exception ex)
                            {
                                _objLoggingProvider.LogException("BatchUpdateMAX_RECORDS", ex);

                            }
                            if (BatchUpdateMAX_RECORDS <= 0)
                            {
                                BatchUpdateMAX_RECORDS = 5000;
                            }
                            _objLoggingProvider.LogMessage(LogType.Info, " BatchUpdateMAX_RECORDS" + BatchUpdateMAX_RECORDS);




                            if (ObjTemplateMaster.TemplateName.ToUpper().Trim() == "MATERIALMASTERLIST".ToUpper().Trim())
                            {
                                List<MaterialMaster> lstMaterialMaster = new List<MaterialMaster>();
                                lstMaterialMaster = (from p in dtTemplateData.AsEnumerable()
                                                     select new MaterialMaster
                                                     {
                                                         Prefix = p["Prefix"].ToString().Trim(),
                                                         ColourCode = p["ColourCode"].ToString().Trim(),
                                                         CatNo = p["CatNo"].ToString().Trim(),
                                                         ItemNo = p["ItemNo"].ToString().Trim(),
                                                         InternalSAPItemNo = p["InternalSAPItemNo"].ToString().Trim(),
                                                         SplitPackQty = p["SplitPackQty"].ToString().Trim() != null && p["SplitPackQty"].ToString().Trim() != "" ? Convert.ToInt32(p["SplitPackQty"]) : 0,
                                                         IsActive = true,
                                                         CreatedBy = LoginUserSESA,
                                                         CreatedDate = DateTime.UtcNow

                                                     }).DistinctBy(p => p.InternalSAPItemNo).ToList();

                                if (lstMaterialMaster.Count > 0)
                                {
                                    //var option = new TransactionOptions
                                    //{
                                    //    IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                                    //    Timeout = TimeSpan.FromSeconds(Convert.ToInt32(AppConfig.TransactionScopeTimeoutInSec))
                                    //};

                                    //using (var scope = new TransactionScope(TransactionScopeOption.Required, option))
                                    //{
                                    //    try
                                    //    {


                                    //        var param = new SqlParameter("@TableName", "MST_MaterialMaster");
                                    //        var ExecuteQuery = _objConfigureRepository.MaterialMasterRepository.ExecuteQuery("Truncate Table MST_MaterialMaster", param);
                                    //        _objLoggingProvider.LogMessage(LogType.Info, "Start : InsertMultipleEntities with MaterialMasterList");

                                    //        foreach (var chunkMaterialMasterList in lstMaterialMaster.Chunk(BatchUpdateMAX_RECORDS))
                                    //        {
                                    //            _objConfigureRepository.MaterialMasterRepository.InsertMultipleEntities(chunkMaterialMasterList.ToList());
                                    //            _objLoggingProvider.LogMessage(LogType.Info, "InProgress : InsertMultipleEntities with Chunk");
                                    //        }




                                    //        //_objConfigureRepository.MaterialMasterRepository.InsertMultipleEntities(lstMaterialMaster);
                                    //        _objLoggingProvider.LogMessage(LogType.Info, "End : InsertMultipleEntities with MaterialMasterList");
                                    //        scope.Complete();
                                    //        _objLoggingProvider.LogMessage(LogType.Info, "End : InsertMultipleEntities with MaterialMasterList with scope.Complete");
                                    //        _objLoggingProvider.LogMessage(LogType.Info, "End: UpdateExcelDataIntoTemplateTables");
                                    //        return true;
                                    //    }
                                    //    catch (Exception ex)
                                    //    {
                                    //        scope.Dispose();
                                    //        _objLoggingProvider.LogException("Error while UpdateExcelDataIntoTemplateTables", ex);
                                    //        return false;

                                    //    }
                                    //}
                                    try
                                    {
                                        _objLoggingProvider.LogMessage(LogType.Info, "Start: InsertMultipleEntities with MaterialMasterList");

                                        bool result = _objConfigureRepository.MaterialMasterRepository.InsertMaterialistWithTransaction(lstMaterialMaster, BatchUpdateMAX_RECORDS);

                                        _objLoggingProvider.LogMessage(LogType.Info, $"End: InsertMultipleEntities with MaterialMasterList - Success: {result}");

                                        return result;
                                    }
                                    catch (Exception ex)
                                    {
                                        _objLoggingProvider.LogException("Error in InsertCustomerSettingsWithTransaction", ex);
                                        return false;
                                    }

                                }



                            }
                            else if (ObjTemplateMaster.TemplateName.ToUpper().Trim() == "CustomerContacts".ToUpper().Trim())
                            {
                                List<CustomerContacts> lstCustomerContactDetails = new List<CustomerContacts>();
                                lstCustomerContactDetails = (from p in dtTemplateData.AsEnumerable()
                                                     select new CustomerContacts
                                                     {
                                                         AccountNumber = p["AccountNumber"].ToString().Trim(),
                                                         AccountName = p["AccountName"].ToString().Trim(),
                                                         ContactPerson = p["ContactPerson"].ToString().Trim(),
                                                         ToEmailID = p["ToEmailID"].ToString().Trim(),
                                                         CcEmailID = p["CcEmailID"].ToString().Trim(),
                                                         BccEmailID = p["BccEmailID"].ToString().Trim(),
                                                         IsActive = true,
                                                         CreatedBy = LoginUserSESA,
                                                         CreatedDate = DateTime.UtcNow

                                                     }).ToList();
                                if(lstCustomerContactDetails.Count> 0)
                                {
                                    var option = new TransactionOptions
                                    {
                                        IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                                        Timeout = TimeSpan.FromSeconds(Convert.ToInt32(AppConfig.TransactionScopeTimeoutInSec))
                                    };

                                    using (var scope = new TransactionScope(TransactionScopeOption.Required, option))
                                    {
                                        try
                                        {


                                            var param = new SqlParameter("@TableName", "MST_CustomerContact");
                                            var ExecuteQuery = _objConfigureRepository.MaterialMasterRepository.ExecuteQuery("Truncate Table MST_CustomerContact", param);
                                            _objLoggingProvider.LogMessage(LogType.Info, "Start : InsertMultipleEntities with MST_CustomerContact");

                                            foreach (var chunklstCustomerContactDetails in lstCustomerContactDetails.Chunk(BatchUpdateMAX_RECORDS))
                                            {
                                                _objConfigureRepository.CustomerContactRepository.InsertMultipleEntities(chunklstCustomerContactDetails.ToList());
                                                _objLoggingProvider.LogMessage(LogType.Info, "InProgress : InsertMultipleEntities with Chunk");
                                            }




                                            //_objConfigureRepository.CustomerContactRepository.InsertMultipleEntities(lstCustomerContactDetails);
                                            _objLoggingProvider.LogMessage(LogType.Info, "End : InsertMultipleEntities with MST_CustomerContact");
                                            scope.Complete();
                                            _objLoggingProvider.LogMessage(LogType.Info, "End : InsertMultipleEntities with MaterialMasterList with scope.Complete");
                                            _objLoggingProvider.LogMessage(LogType.Info, "End: UpdateExcelDataIntoTemplateTables");
                                            return true;
                                        }
                                        catch (Exception ex)
                                        {
                                            scope.Dispose();
                                            _objLoggingProvider.LogException("Error while UpdateExcelDataIntoTemplateTables", ex);
                                            return false;

                                        }
                                    }

                                }

                            }
                            else if (ObjTemplateMaster.TemplateName.ToUpper().Trim() == "MVKEOverride".ToUpper().Trim())
                            {
                                List<MvkeOverride> mvkeOverrides = (from p in dtTemplateData.AsEnumerable()
                                                                    select new MvkeOverride
                                                                    {
                                                                        Vkorg = p["Vkorg"].ToString().Trim(),
                                                                        Matnr = p["Matnr"].ToString().Trim(),
                                                                        Prodh = p["Prodh"].ToString().Trim(),
                                                                        CreatedBy = LoginUserSESA,
                                                                        CreatedDate = DateTime.UtcNow,
                                                                    }).ToList();
                                if (mvkeOverrides.Count > 0)
                                {
                                    var option = new TransactionOptions
                                    {
                                        IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                                        Timeout = TimeSpan.FromSeconds(Convert.ToInt32(AppConfig.TransactionScopeTimeoutInSec))
                                    };

                                    using (var scope = new TransactionScope(TransactionScopeOption.Required, option))
                                    {
                                        try
                                        {
                                            var param = new SqlParameter("@TableName", "MVKE_Override");
                                            var ExecuteQuery = _objBackOpsRepository.MvkeOverrideRepository.ExecuteQuery("Truncate Table MVKE_Override", param);
                                            _objLoggingProvider.LogMessage(LogType.Info, "Start : InsertMultipleEntities with MVKE_Override");

                                            foreach (var chunklst in mvkeOverrides.Chunk(BatchUpdateMAX_RECORDS))
                                            {
                                                _objBackOpsRepository.MvkeOverrideRepository.InsertMultipleEntities(chunklst.ToList());
                                                _objLoggingProvider.LogMessage(LogType.Info, "InProgress : InsertMultipleEntities with Chunk");
                                            }
                                            //_objConfigureRepository.CustomerContactRepository.InsertMultipleEntities(lstCustomerContactDetails);
                                            _objLoggingProvider.LogMessage(LogType.Info, "End : InsertMultipleEntities with MVKE_Override");
                                            scope.Complete();
                                            _objLoggingProvider.LogMessage(LogType.Info, "End : InsertMultipleEntities in MVKE_Override with scope.Complete");
                                            _objLoggingProvider.LogMessage(LogType.Info, "End: UpdateExcelDataIntoTemplateTables");
                                            return true;
                                        }
                                        catch (Exception ex)
                                        {
                                            scope.Dispose();
                                            _objLoggingProvider.LogException("Error while UpdateExcelDataIntoTemplateTables", ex);
                                            return false;
                                        }
                                    }
                                }
                            }
                            else if (ObjTemplateMaster.TemplateName.ToUpper().Trim() == "CustomerSettings".ToUpper().Trim())
                            {
                                List<CustomerSettings> lstCustomerSettings = (from p in dtTemplateData.AsEnumerable()
                                                             select new CustomerSettings
                                                             {
                                                                 CustomerNumber                       = p["CustomerNumber"].ToString().Trim(),
                                                                 SalesOrganization                    = p["SalesOrganization"].ToString().Trim(),
                                                                 CanIncludeTradePrices                = Convert.ToBoolean(p["CanIncludeTradePrices"]),
                                                                 CanIncludeCustomerNetPrices          = Convert.ToBoolean(p["CanIncludeCustomerNetPrices"]),
                                                                 CanIncludeCustomerHierarchyNetPrices = Convert.ToBoolean(p["CanIncludeCustomerHierarchyNetPrices"]),
                                                                 CanIncludeOverallNetPrices           = Convert.ToBoolean(p["CanIncludeOverallNetPrices"]),
                                                                 CanIncludePriceGroupNets             = Convert.ToBoolean(p["CanIncludePriceGroupNets"]),
                                                                 CanIncludeSellOffPrices              = Convert.ToBoolean(p["CanIncludeSellOffPrices"]),
                                                                 CanIncludePromoPrice                 = Convert.ToBoolean(p["CanIncludePromoPrice"]),
                                                                 CanUseShiftBreaks                    = Convert.ToBoolean(p["CanUseShiftBreaks"]),
                                                                 CanUseMOQAsBrk1                      = Convert.ToBoolean(p["CanUseMOQAsBrk1"]),
                                                                 CanShowTemplateMaterialOnly          = Convert.ToBoolean(p["CanShowTemplateMaterialOnly"]),
                                                                 CanShowNotFoundTemplateMaterials     = Convert.ToBoolean(p["CanShowNotFoundTemplateMaterials"]),
                                                                 CanIncludeDiscount1                  = Convert.ToBoolean(p["CanIncludeDiscount1"]),
                                                                 CanIncludeDiscount2                  = Convert.ToBoolean(p["CanIncludeDiscount2"]),
                                                                 CanIncludeDiscount3                  = Convert.ToBoolean(p["CanIncludeDiscount3"]),
                                                                 CanIncludeDiscount4                  = Convert.ToBoolean(p["CanIncludeDiscount4"]),
                                                                 CanIncludeDiscount5                  = Convert.ToBoolean(p["CanIncludeDiscount5"]),
                                                                 CanIncludeDiscount6                  = Convert.ToBoolean(p["CanIncludeDiscount6"]),
                                                                 CanIncludeDiscount7                  = Convert.ToBoolean(p["CanIncludeDiscount7"]),
                                                                 CanIncludeDiscount8                  = Convert.ToBoolean(p["CanIncludeDiscount8"]),
                                                                 CanUseAutoReportContent              = true,
                                                                 ReportContentTemplateID              = 0,
                                                                 SelectedCustomersTemplateID          = 0,
                                                                 CanUseGlobalCOSForProductHierarchy   = false,
                                                                 CanUseLocalCOSForProductHierarchy    = true,
                                                                 CanAddSODInFinalPrice                = false,
                                                                 SODInFinalPriceValue                 = 0,
                                                                 CanUseAlternateValidFromDate         = false,
                                                                 AlternateValidFromDate               = null,
                                                                 CanSendEmail                         = true,
                                                                 CanIncludeProductHierarchyOverride = Convert.ToBoolean(p["CanIncludeProductHierarchyOverride"]),
                                                                 IsActive = true,
                                                                 CreatedBy = LoginUserSESA,
                                                                 ModifiedBy = "",
                                                                 CreatedDate = DateTime.UtcNow,
                                                                 ModifiedDate = DateTime.UtcNow
                                                             }).ToList();
                                if (lstCustomerSettings.Count > 0)
                                {
                                    //var option = new TransactionOptions
                                    //{
                                    //    IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                                    //    Timeout = TimeSpan.FromSeconds(Convert.ToInt32(AppConfig.TransactionScopeTimeoutInSec) * 2)
                                    //};

                                    //using (var scope = new TransactionScope(TransactionScopeOption.Required, option))
                                    //{
                                    //    try
                                    //    {
                                    //        var param = new SqlParameter("@TableName", "MST_CustomerSettings");
                                    //        _objPriceListRepository.CustomerSettingsRepository.ExecuteQuery("Truncate Table MST_CustomerSettings", param);
                                    //        _objLoggingProvider.LogMessage(LogType.Info, "Start : InsertMultipleEntities with MST_CustomerSettings");

                                    //        foreach (var chunklstCustomerSettingDetails in lstCustomerSettings.Chunk(BatchUpdateMAX_RECORDS))
                                    //        {
                                    //            _objPriceListRepository.CustomerSettingsRepository.InsertMultipleEntities(chunklstCustomerSettingDetails.ToList());
                                    //            _objLoggingProvider.LogMessage(LogType.Info, "InProgress : InsertMultipleEntities with Chunk");
                                    //        }
                                    //        _objLoggingProvider.LogMessage(LogType.Info, "End : InsertMultipleEntities with MST_CustomerSettings");
                                    //        scope.Complete();
                                    //        _objLoggingProvider.LogMessage(LogType.Info, "End : InsertMultipleEntities in CustomerSettings with scope.Complete");
                                    //        _objLoggingProvider.LogMessage(LogType.Info, "End: UpdateExcelDataIntoTemplateTables");
                                    //        return true;
                                    //    }
                                    //    catch (Exception ex)
                                    //    {
                                    //        scope.Dispose();
                                    //        _objLoggingProvider.LogException("Error while UpdateExcelDataIntoTemplateTables", ex);
                                    //        return false;
                                    //    }
                                    //}

                                    try
                                    {
                                        _objLoggingProvider.LogMessage(LogType.Info, "Start: InsertCustomerSettingsWithTransaction");

                                        bool result = _objPriceListRepository.CustomerSettingsRepository.InsertCustomerSettingsWithTransaction(lstCustomerSettings, BatchUpdateMAX_RECORDS);

                                        _objLoggingProvider.LogMessage(LogType.Info, $"End: InsertCustomerSettingsWithTransaction - Success: {result}");

                                        return result;
                                    }
                                    catch (Exception ex)
                                    {
                                        _objLoggingProvider.LogException("Error in InsertCustomerSettingsWithTransaction", ex);
                                        return false;
                                    }
                                    

                                }
                            }
                            
                            else
                            {
                                return false;
                            }
                            return true;

                        }
                        else if (ObjTemplateMaster.TemplateDataModel.ToUpper().Trim() == "NONE".ToUpper().Trim())
                        {
                            return true;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        _objLoggingProvider.LogMessage(LogType.Info, "End: UpdateExcelDataIntoTemplateTables- dtTemplateData== null");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Error while UpdateExcelDataIntoTemplateTables", ex);
                throw;
            }
        }

        /// <summary>
        /// GetReportFormats
        /// </summary>
        /// <returns>lst_reportformats</returns>
        /// param : 
        //public IQueryable<TemplateMaster> GetReportContents()
        //{
        //    try
        //    {
        //        _objLoggingProvider.LogMessage(LogType.Info, "Start: GetReportContents");
        //        IQueryable<TemplateMaster>? lst_reportcontents = null;
        //        lst_reportcontents = _objConfigureRepository.TemplateMasterRepository.GetManyQueryable().Where(x => x.IsActive == true && x.TemplateCategoryID == 1 && x.IsDeleted == false);
        //        _objLoggingProvider.LogMessage(LogType.Info, "End: GetReportContents");
        //        return lst_reportcontents;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}


        #region ReportContent


        public List<TemplateMaster> GetTemplateInfoByTemplateName(string TemplateName)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: GetTemplateInfoByTemplateName");
                var lstTemplateInfo = _objConfigureRepository.TemplateMasterRepository.GetManyQueryable().Where(x => x.IsActive == true && x.TemplateName.ToUpper().Trim() == TemplateName.ToUpper().Trim() && x.IsDeleted == false).ToList();
                _objLoggingProvider.LogMessage(LogType.Info, "End: GetTemplateInfoByTemplateName");
                return lstTemplateInfo;

            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Error while GetTemplateInfoByTemplateName", ex);
                throw;
            }
        }

        /// <summary>
        /// SaveUser
        /// </summary>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        public int SaveReportContent(ReportContentModel rcM)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: SaveReportContent");
                string UserSESA = _objCommonProvider.GetLoginUserSESA();
                
                using var scope = new TransactionScope();
                try
                {
                    TemplateCategory objTemplateCategory = _objConfigureRepository.TemplateCategoryRepository.GetManyQueryable().Where(x => x.CategoryName.Trim().ToUpper() == "ReportTradeListContent".Trim().ToUpper()).FirstOrDefault();
                    if(objTemplateCategory != null && objTemplateCategory.TemplateCategoryID > 0)
                    {
                        TemplateMaster templateMaster = new();
                        templateMaster.TemplateCategoryID = objTemplateCategory.TemplateCategoryID;
                        templateMaster.TemplateName = rcM.TemplateName.Trim();
                        templateMaster.AliasName = rcM.TemplateName.Trim();
                        templateMaster.TemplateDataModel = "JSON";
                        templateMaster.CountryCode = rcM.CountryCode.ToUpper().Trim();
                        templateMaster.CanDuplicate = true;
                        templateMaster.CanUpload = true;
                        templateMaster.CanEdit = true;
                        templateMaster.IsActive = true;
                        templateMaster.IsDeleted = false;
                        templateMaster.CreatedBy = UserSESA;
                        templateMaster.CreatedDate = DateTime.UtcNow;

                        var objTemplateMaster = _objConfigureRepository.TemplateMasterRepository.InsertEntity(templateMaster);

                        TemplateStructure TS = new();
                        TS.TemplateMasterID = objTemplateMaster.TemplateMasterID;
                        TS.PropertyName = "InternalSAPItemNo";
                        TS.PropertyDescription = "InternalSAPItemNo";
                        TS.PropertyDataType = "VARCHAR";
                        TS.SequenceNo = 1;
                        TS.IsActive = true;
                        TS.CreatedBy = UserSESA;
                        TS.CreatedDate = DateTime.UtcNow;

                        var objTemplateStructure = _objConfigureRepository.TemplateStructureRepository.InsertEntity(TS);


                        scope.Complete();
                        _objLoggingProvider.LogMessage(LogType.Info, "End: SaveReportContent");
                        return objTemplateMaster.TemplateMasterID;
                    }
                    else
                    {
                        return 0;
                    }  
                    
                }
                catch (Exception)
                {
                    scope.Dispose();
                    throw;
                }

            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("SaveReportContent :", ex);
                throw;
            }

        }

        /// <summary>
        /// Save customer template
        /// </summary>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        public int SaveCustomerTemplate(ReportContentModel rcM)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: SaveReportContent");
                string UserSESA = _objCommonProvider.GetLoginUserSESA();

                using var scope = new TransactionScope();
                try
                {
                    TemplateCategory objTemplateCategory = _objConfigureRepository.TemplateCategoryRepository.GetManyQueryable().Where(x => x.CategoryName.Trim().ToUpper() == "CustomerTemplates".Trim().ToUpper()).FirstOrDefault();
                    if (objTemplateCategory != null && objTemplateCategory.TemplateCategoryID > 0)
                    {
                        TemplateMaster templateMaster = new();
                        templateMaster.TemplateCategoryID = objTemplateCategory.TemplateCategoryID;
                        templateMaster.TemplateName = rcM.TemplateName.Trim();
                        templateMaster.AliasName = rcM.TemplateName.Trim();
                        templateMaster.TemplateDataModel = "JSON";
                        templateMaster.CountryCode = rcM.CountryCode.ToUpper().Trim();
                        templateMaster.CanDuplicate = true;
                        templateMaster.CanUpload = true;
                        templateMaster.CanEdit = true;
                        templateMaster.IsActive = true;
                        templateMaster.IsDeleted = false;
                        templateMaster.CreatedBy = UserSESA;
                        templateMaster.CreatedDate = DateTime.UtcNow;

                        var objTemplateMaster = _objConfigureRepository.TemplateMasterRepository.InsertEntity(templateMaster);

                        TemplateStructure TS = new();
                        TS.TemplateMasterID = objTemplateMaster.TemplateMasterID;
                        TS.PropertyName = "CustomerNo";
                        TS.PropertyDescription = "CustomerNo";
                        TS.PropertyDataType = "VARCHAR";
                        TS.SequenceNo = 1;
                        TS.IsActive = true;
                        TS.CreatedBy = UserSESA;
                        TS.CreatedDate = DateTime.UtcNow;

                        var objTemplateStructure = _objConfigureRepository.TemplateStructureRepository.InsertEntity(TS);


                        scope.Complete();
                        _objLoggingProvider.LogMessage(LogType.Info, "End: SaveReportContent");
                        return objTemplateMaster.TemplateMasterID;
                    }
                    else
                    {
                        return 0;
                    }

                }
                catch (Exception)
                {
                    scope.Dispose();
                    throw;
                }

            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// DeleteTemplateMaster
        /// </summary>
        /// <param name="TemplateMasterID"></param>
        /// <returns>true/false</returns>
        public bool DeleteTemplateMaster(int TemplateMasterID)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: DeleteTemplateMaster");

                TemplateMaster templateMaster = _objConfigureRepository.TemplateMasterRepository.GetFirst(u => u.TemplateMasterID == TemplateMasterID);
                List<TemplateStructure> templateStructures = _objConfigureRepository.TemplateStructureRepository.GetAllEntities(m => m.TemplateMasterID == TemplateMasterID).ToList();
                List<TemplateData> templateDatas = _objConfigureRepository.TemplateDataRepository.GetAllEntities(m => m.TemplateMasterID == TemplateMasterID).ToList();

                using var scope = new TransactionScope();
                try
                {
                    #region -- Updating Template Master ---
                    templateMaster.IsActive = false;
                    templateMaster.IsDeleted = true;
                    templateMaster.ModifiedBy = _objCommonProvider.GetLoginUserSESA();
                    templateMaster.ModifiedDate = DateTime.UtcNow;

                    _objConfigureRepository.TemplateMasterRepository.UpdateEntity(templateMaster);

                    #endregion

                    #region -- Updating Templatestructure --
                        if (templateStructures.Count > 0)
                        {
                            foreach (TemplateStructure tempStru in templateStructures)
                            {

                                tempStru.IsActive = false;
                                tempStru.IsDeleted = true;
                           
                            _objConfigureRepository.TemplateStructureRepository.UpdateEntity(tempStru);
                            }
                        }
                   
                    #endregion

                    #region -- Updating Templatedata --
                    foreach (TemplateData tempData in templateDatas)
                    {

                        tempData.IsActive = false;
                        tempData.IsDeleted = true;
                        tempData.ModifiedBy = _objCommonProvider.GetLoginUserSESA();
                        tempData.ModifiedDate = DateTime.UtcNow;

                        _objConfigureRepository.TemplateDataRepository.UpdateEntity(tempData);
                    }
                    #endregion

                    scope.Complete();
                    _objLoggingProvider.LogMessage(LogType.Info, "End: DeleteTemplateMaster");
                    return true;

                }
                catch (Exception)
                {
                    scope.Dispose();
                    throw;
                }
            }

            catch(Exception ex)
            {
                _objLoggingProvider.LogException("DeleteTemplateMaster :", ex);
                throw;
            }
        }

        /// <summary>
        /// IsTemplateDeleted
        /// </summary>
        /// <param name="TemplateName"></param>
        /// <param name="CountryCode"></param>
        /// <returns>1 -> if template name exists in same organisation and IsDeleted == true</returns>
        /// <returns>2 -> if template name exists in different organisation and IsDeleted == true</returns>
        /// <returns>3 -> if template name does not exists in the db</returns>
        public int IsTemplateDeleted(string TemplateName, string CountryCode)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: IsTemplateDeleted");
                TemplateMaster tempMaster = _objConfigureRepository.TemplateMasterRepository.GetQueryable(u => u.TemplateName.ToUpper().Trim() == TemplateName.ToUpper().Trim()).FirstOrDefault();
                if (tempMaster != null && tempMaster.IsActive == false && tempMaster.IsDeleted == true && tempMaster.CountryCode == CountryCode)
                {
                    _objLoggingProvider.LogMessage(LogType.Info, "End: IsTemplateDeleted");
                    return 1;
                }
                else if (tempMaster != null && tempMaster.IsActive == false && tempMaster.IsDeleted == true && tempMaster.CountryCode != CountryCode)
                {
                    _objLoggingProvider.LogMessage(LogType.Info, "End: IsTemplateDeleted");
                    return 2;
                }
                else
                {
                    _objLoggingProvider.LogMessage(LogType.Info, "End: IsTemplateDeleted");
                    return 3;
                }
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("IsTemplateDeleted : ", ex);
                throw;
            }
        }


        /// <summary>
        /// ReActivateTemplate
        /// </summary>
        /// <param name="TemplateName"></param>
        /// <param name="CountryCode"></param>
        /// <returns>true/false</returns>
        public int ReActivateTemplate(string TemplateName, string CountryCode)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: ReActivateTemplate");
                TemplateMaster templateMaster = _objConfigureRepository.TemplateMasterRepository.GetFirst(u => u.TemplateName.ToUpper().Trim() == TemplateName.ToUpper().Trim());
                if (templateMaster != null)
                {
                    List<TemplateStructure> templateStructures = _objConfigureRepository.TemplateStructureRepository.GetAllEntities(m => m.TemplateMasterID == templateMaster.TemplateMasterID).ToList();
                    List<TemplateData> templateDatas = _objConfigureRepository.TemplateDataRepository.GetAllEntities(m => m.TemplateMasterID == templateMaster.TemplateMasterID).ToList();

                    using var scope = new TransactionScope();
                    try
                    {
                        #region -- Updating Template Master --
                        templateMaster.IsActive = true;
                        templateMaster.IsDeleted = false;
                        templateMaster.ModifiedBy = _objCommonProvider.GetLoginUserSESA();
                        templateMaster.ModifiedDate = DateTime.UtcNow;

                        _objConfigureRepository.TemplateMasterRepository.UpdateEntity(templateMaster);
                        #endregion

                        #region -- Updating Templatestructure --
                        if (templateStructures.Count > 0)
                        {
                            foreach (TemplateStructure tempStru in templateStructures)
                            {

                                tempStru.IsActive = true;
                                tempStru.IsDeleted = false;

                                _objConfigureRepository.TemplateStructureRepository.UpdateEntity(tempStru);
                            }
                        }
                        #endregion

                        #region -- Updating Templatedata --
                        foreach (TemplateData tempData in templateDatas)
                        {

                            tempData.IsActive = true;
                            tempData.IsDeleted = false;
                            tempData.ModifiedBy = _objCommonProvider.GetLoginUserSESA();
                            tempData.ModifiedDate = DateTime.UtcNow;

                            _objConfigureRepository.TemplateDataRepository.UpdateEntity(tempData);
                        }
                        #endregion
                        scope.Complete();
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    _objLoggingProvider.LogMessage(LogType.Info, "End: ReActivateTemplate");
                    return templateMaster.TemplateMasterID;
                }
                else
                {
                    return 0;
                }
            }
            catch(Exception ex)
            {
                _objLoggingProvider.LogException("ReActivateTemplate : ", ex);
                throw;
            }
            
        }
        #endregion

        #region AppConfigSettings

        /// <summary>
        /// GetAppConfigSettings
        /// </summary>
        /// <returns></returns>
        public List<AppConfigSetting> GetAppConfigSettings()
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: GetAppConfigSettings");
                var lst_APPConfigSetting = _objConfigureRepository.AppConfigSettingRepository.GetManyQueryable().Where(x => x.IsActive == true)
                    .OrderBy(x => x.SequenceNo).ToList();
                _objLoggingProvider.LogMessage(LogType.Info, "End: GetAppConfigSettings");
                return lst_APPConfigSetting;

            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Error while GetAppConfigSettings", ex);
                throw;
            }

        }

        public List<ConfigOptions> GetAppConfigOptions()
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: GetAppConfigOptions");
                var lst_APPConfigOptions = _objConfigureRepository.ConfigOptionsRepository.GetManyQueryable().Where(x => x.IsActive == true)
                    .OrderBy(x => x.SequenceNo).ToList();
                _objLoggingProvider.LogMessage(LogType.Info, "End: GetAppConfigOptions");
                return lst_APPConfigOptions;
            }
            catch(Exception ex)
            {
                _objLoggingProvider.LogException("Error while GetAppConfigOptions", ex);
                throw;
            }
        }

        /// <summary>
        /// SaveAppConfigSettingDetails
        /// </summary>
        /// <param name="objAppConfigSettingModel"></param>
        /// <returns></returns>
        public bool SaveAppConfigSettingDetails(List<AppConfigSetting> objAppConfigSettingModel)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: SaveAppConfigSettingDetails");
                string UserSESA = _objCommonProvider.GetLoginUserSESA();
                for (var i = 0; i < objAppConfigSettingModel.Count; i++)
                {
                    using (var scope = new TransactionScope())
                    {
                        try
                        {
                            objAppConfigSettingModel[i].ModifiedBy = UserSESA;
                            objAppConfigSettingModel[i].ModifiedDate = DateTime.UtcNow;
                            _objConfigureRepository.AppConfigSettingRepository.UpdateEntity(objAppConfigSettingModel[i]);
                            scope.Complete();
                           
                        }
                        catch (Exception)
                        {
                            scope.Dispose();
                            throw;
                        }
                    }
                }
                _objHttpContextAccessor.HttpContext.Session.Remove(Constants.AppConfigSession.ToString());
                _objLoggingProvider.LogMessage(LogType.Info, "End: SaveAppConfigSettingDetails");
                return true;
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Error while SaveAppConfigSettingDetails", ex);
                throw;
            }
        }

        #endregion

        #region UserLog
        /// <summary>
        /// GetUsers
        /// </summary>
        /// <returns>lst_UserMaster</returns>
        /// param : 
        public IQueryable<UserLog> GetUsersLogInfo()
        {
            try
            {
                int MaxCount = 5000;
                try
                {
                    string DisplayMaxRecords = _objCommonProvider.GetAppSettingByName(Constants.DisplayMaxRecords);
                    if (DisplayMaxRecords != "" && DisplayMaxRecords != null)
                    {
                        MaxCount = Convert.ToInt32(DisplayMaxRecords);
                    }
                }
                catch(Exception ex1)
                {
                    _objLoggingProvider.LogException("GetUsersLogInfo(DisplayMaxRecords) :", ex1);
                }
                _objLoggingProvider.LogMessage(LogType.Info, "Start: Get UsersLog Info ");
                IQueryable<UserLog>? lst_UsersLogInfo = null;
                lst_UsersLogInfo = _objConfigureRepository.UserLogRepository.GetManyQueryable().OrderByDescending(x=>x.UserLogID).Take(MaxCount);
                _objLoggingProvider.LogMessage(LogType.Info, "End: Get UsersLog Info ");
                return lst_UsersLogInfo;

            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("GetUsersLogInfo :", ex);
                throw;
            }

        }
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
            if (_objConfigureRepository != null && isDispose)
            {
                _objConfigureRepository.Dispose();
            }
            if (_objLoggingProvider != null && isDispose)
            {
                _objLoggingProvider.Dispose();
            }
            if (_objCommonProvider != null && isDispose)
            {
                _objCommonProvider.Dispose();
            }
        }

        #endregion
    }
}