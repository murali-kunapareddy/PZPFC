using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ExcelUploadValidator.Interfaces;
using ExcelUploadValidator.Models;
using ExcelUploadValidator.ServiceProviders;
using PFCWebAPP.Filters;
using PFCWebAPP.Repositories.Common.Enums;
using PFCWebAPP.Repositories.Common.Interfaces;
using PFCWebAPP.Repositories.Configure.Interfaces;
using PFCWebAPP.Repositories.Configure.Models;
using System.Data;
using System.Text;
using PFCWebAPP.Repositories.PriceList.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PFCWebAPP.Utilities;
using PFCWebAPP.DatabaseContext.Models.CustomTables;

namespace PFCWebAPP.Controllers
{
    [PFCExceptionFilter]
    public class ConfigureController : BaseController
    {
        private readonly ILoggingProvider _objLoggingProvider;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IConfigureProvider _objConfigureProvider;
        public readonly ICommonProvider _objCommonProvider;
        public readonly IPriceListProvider _objPriceListProvider;


        public ConfigureController(ILoggingProvider objLoggingProvider, IHttpContextAccessor contextAccessor, IConfigureProvider objConfigureProvider, ICommonProvider objCommonProvider,IPriceListProvider objPriceListProvider)
        {
            httpContextAccessor = contextAccessor;
            _objLoggingProvider = objLoggingProvider;
            _objConfigureProvider = objConfigureProvider;
            _objCommonProvider = objCommonProvider;
            _objPriceListProvider = objPriceListProvider;
            _objLoggingProvider.LogMessage(LogType.Info, "Configure Controller");
        }


        #region ------ ReportFormat -------
        /// <summary>
        /// ReportFormat
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex){
                _objLoggingProvider.LogException("Configure :" , ex);
                throw;
            }
           
        }

        /// <summary>
        /// _ViewReportFormatDataById
        /// </summary>
        /// <returns>reportFormatTM</returns>
        [HttpPost]
        public IActionResult _ViewReportFormatDataById(int ReportFormatMasterId)
        {
            try
            {
                ReportFormatTemplateModel reportFormatTM = new();
                _objLoggingProvider.LogMessage(LogType.Info, "Start: _ViewReportFormatDataById");
                var lstReportFormats = _objConfigureProvider.GetReportFormats().ToList();
                reportFormatTM.SelectedReportFormatTempMasterID = ReportFormatMasterId;
                reportFormatTM.SelectedTemplateDetails = (from n in lstReportFormats where n.ReportFormatMasterID == ReportFormatMasterId select n).FirstOrDefault();
                reportFormatTM.ReportFormatsDataTable = _objConfigureProvider.GetReportFormatDetailsByID(ReportFormatMasterId); ;
                _objLoggingProvider.LogMessage(LogType.Info, "End: _ViewReportFormatDataById");
                return View(reportFormatTM);
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("_ViewReportFormatDataById :" , ex);
                throw;
            }
        }

        /// <summary>
        /// ReportFormat
        /// </summary>
        /// <returns>viewModel</returns>
        [PFCRoleBasedAuthorizeFilter]
        [PFCSessionExpireFilter]
        public ActionResult ReportFormat()
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: ReportFormat");
                var configFormatId = _objPriceListProvider.GetPriceFileSaveConfig();
                var lstReportFormats = _objConfigureProvider.GetReportFormats().ToList();
                _objLoggingProvider.LogMessage(LogType.Info, "Start: ReportFormat");
                var rfViewModel = new ReportFormatTemplateModel
                {
                    ReportFormats = lstReportFormats.Select(c => new SelectListItem
                    {
                        Value = c.ReportFormatMasterID.ToString(),
                        Text = c.AliasName
                    })
                };
                if(configFormatId != null)
                {
                    rfViewModel.SelectedReportFormatTempMasterID = Convert.ToInt32(configFormatId.ReportFormatTemplateID);
                }
                return View(rfViewModel);
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("ReportFormat :" , ex);
                throw;
            }
        }
        #endregion

        #region ***** Master References & Templates*****
        /// <summary>
        /// Master References
        /// </summary>
        /// <returns></returns>
        [PFCRoleBasedAuthorizeFilter]
        [PFCSessionExpireFilter]
        public ActionResult Masters()
        {
            try
            {
                TemplateModel TM = new TemplateModel();
                _objLoggingProvider.LogMessage(LogType.Info, "Start: Master References");
                var lstTemplateMasterDetails = _objConfigureProvider.GetTemplatesByCategory("MasterReferences");
                var ddlTemplateMasterDetails = (from n in lstTemplateMasterDetails
                                              select new DDLTemplateMasterDetails
                                              {
                                                  TemplateMasterID = n.TemplateMasterID,
                                                  TemplateName = (n.CountryCode != "" && n.CountryCode != "00") ? (n.AliasName + " - " + n.CountryCode) : n.AliasName
                                              }).ToList();
                TM.ddlTemplateMasterDetails = ddlTemplateMasterDetails;
                _objLoggingProvider.LogMessage(LogType.Info, "End: Master References");
                return View(TM);
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Masters :" , ex);
                throw;
            }
        }



        /// <summary>
        /// _GetTemplateInfoByID
        /// </summary>
        /// <param name="TemplateMasterID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _GetTemplateDetailsByID(int TemplateMasterID)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: _GetTemplateDetailsByID");
                TemplateModel TM = new TemplateModel();
                try
                {
                    string DisplayMaxRecords = _objCommonProvider.GetAppSettingByName(Constants.DisplayMaxRecords);
                    if (!string.IsNullOrEmpty(DisplayMaxRecords))
                    {
                        TM.DisplayMaxRecords = Convert.ToInt32(DisplayMaxRecords);
                    }

                }
                catch (Exception ex)
                {
                    TM.DisplayMaxRecords = 0;

                }

                TM.SelectedTemplateMasterID = TemplateMasterID;
                TM.SelectedTemplateDetails = _objConfigureProvider.GetTemplateMasterDetailsByTemplateID(TemplateMasterID);
                //TM.SelectedTemplateDataInDataTable = _objConfigureProvider.GetTemplateDataByTemplateIDV2(TemplateMasterID, TM.DisplayMaxRecords);
                DataSet ds = _objConfigureProvider.GetTemplateDataByTemplateIDV2(TemplateMasterID, TM.DisplayMaxRecords);

                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    TM.TotalRecordsCount = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalRecordsCount"].ToString());
                }

                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    TM.SelectedTemplateDataInDataTable = ds.Tables[1];
                }
                else
                {
                    TM.SelectedTemplateDataInDataTable = new DataTable();
                }


                _objLoggingProvider.LogMessage(LogType.Info, "End: _GetTemplateDetailsByID");
                return PartialView(TM);
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("_GetTemplateDetailsByID :" , ex);
                throw;
            }

        }

        /// <summary>
        /// Export
        /// </summary>
        /// <param name="TemplateMasterID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Export(int TemplateMasterID, string TemplateName)
        {
            try
            {
                DataSet ds = _objConfigureProvider.GetTemplateDataByTemplateIDV2(TemplateMasterID, 0);

                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    string saveAsFileName = string.Format(TemplateName + "-{0:d}.xlsx", DateTime.UtcNow.ToString("yyyyMMddHHmmss")).Replace("/", "-");
                    string fpath = Path.Combine(Directory.GetCurrentDirectory(), PFCWebAPP.Utilities.AppConfig.TemplateFilepath);
                    string fname = Path.Combine(PFCWebAPP.Utilities.AppConfig.TemplateFilepath, saveAsFileName);


                    bool result = PFCWebAPP.Utilities.Common.ConvertDataTableDataInfoExcel(TemplateName, ds.Tables[1], fname);

                    if (result)
                    {
                        //Read the File data into Byte Array.
                        byte[] bytes = System.IO.File.ReadAllBytes(fname);

                        //Send the File to Download.
                        return File(bytes, "application/octet-stream", Path.GetFileName(fname));
                    }
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("Export :" , ex);
                throw;
            }
        }


        /// <summary>
        /// _ValidateExcelDataByTemplateID
        /// </summary>
        /// <param name="formdata"></param>
        /// <returns></returns>
        [HttpPost]
        [DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue,ValueLengthLimit = int.MaxValue)]
        public ActionResult _ValidateExcelDataByTemplateID(IFormCollection formdata)
        {
            try
            {
                TemplateModel TM = new TemplateModel();
                _objLoggingProvider.LogMessage(LogType.Info, "Start: _ValidateExcelUploadDataByTemplateID");

                var files = Request.Form.Files;
                var TemplateMasterID = formdata["TemplateMasterID"];
                var TemplateName = formdata["TemplateName"];

                using (IUploadValidator objUploadValidator = new UploadValidator())
                {
                    UploadValidatorRequest objUploadValidatorRequest = new UploadValidatorRequest();
                    objUploadValidatorRequest.TemplateID = Convert.ToInt32(TemplateMasterID);
                    objUploadValidatorRequest.TemplateName = TemplateName;
                    objUploadValidatorRequest.lstFiles = files;
                    objUploadValidatorRequest.TemplateFilepath = PFCWebAPP.Utilities.AppConfig.TemplateFilepath;
                    objUploadValidatorRequest.ExcelFileName = files[0].FileName;

                    var ObjTemplateStructureInto = _objConfigureProvider.GetTemplateStructureIntoByTemplateID(Convert.ToInt32(TemplateMasterID));
                    List<TemplateDataStructure> lstTemplateDataStructure = new List<TemplateDataStructure>();
                    foreach (var TemplateStructureInto in ObjTemplateStructureInto)
                    {
                        TemplateDataStructure objTemplateDataStructure = new TemplateDataStructure();
                        objTemplateDataStructure.FieldName = TemplateStructureInto.PropertyName;
                        objTemplateDataStructure.FieldDescription = TemplateStructureInto.PropertyDescription;
                        objTemplateDataStructure.SequenceNo = TemplateStructureInto.SequenceNo;
                        lstTemplateDataStructure.Add(objTemplateDataStructure);
                    }

                    objUploadValidatorRequest.lstTemplateDataStructure = lstTemplateDataStructure;                  
                    _objLoggingProvider.LogMessage(LogType.Info, "Start: ValidateDataWithTemplateStructure");
                    var objUploadValidatorResponse = objUploadValidator.ValidateDataWithTemplateStructure(objUploadValidatorRequest);


                    TM.objTemplateSourceFileDetails = new TemplateSourceFileDetails();
                    TM.objTemplateSourceFileDetails.TemplateMasterID = Convert.ToInt32(TemplateMasterID[0]);
                    TM.objTemplateSourceFileDetails.TemplateName = Convert.ToString(TemplateName[0]);

                    if (objUploadValidatorResponse.UploadValidatorStatus == UploadValidatorStatus.Success)
                    {
                        _objLoggingProvider.LogMessage(LogType.Info, "UploadValidatorStatus: Success");
                        
                        TM.objTemplateSourceFileDetails.SourceFile = objUploadValidatorResponse.TemplateFileName;                      
                        TM.objTemplateSourceFileDetails.TemplateTable = objUploadValidatorResponse.TemplateData;

                        try
                        {
                            string DisplayMaxRecords = _objCommonProvider.GetAppSettingByName(Constants.DisplayMaxRecords);
                            if (!string.IsNullOrEmpty(DisplayMaxRecords))
                            {
                                TM.DisplayMaxRecords = Convert.ToInt32(DisplayMaxRecords);
                            }

                        }
                        catch (Exception ex)
                        {
                            TM.DisplayMaxRecords = 0;

                        }

                        _objLoggingProvider.LogMessage(LogType.Info, "End: ValidateDataWithTemplateStructure");
                        _objLoggingProvider.LogMessage(LogType.Info, "End: _ValidateExcelUploadDataByTemplateID");
                        return PartialView("_PreviewExcelData", TM);


                    }
                    else if (objUploadValidatorResponse.UploadValidatorStatus == UploadValidatorStatus.FailedDueToMismatchedColumns)
                    {
                        _objLoggingProvider.LogMessage(LogType.Info, "UploadValidatorStatus: FailedDueToMismatchedColumns");
                        TM.ColumnMisMatchDataInfo = objUploadValidatorResponse.ResultMessage;
                        _objLoggingProvider.LogMessage(LogType.Info, "End: ValidateDataWithTemplateStructure");
                        _objLoggingProvider.LogMessage(LogType.Info, "End: _ValidateExcelUploadDataByTemplateID");
                        return PartialView("_ColsMismatch", TM);

                    }
                    else if (objUploadValidatorResponse.UploadValidatorStatus == UploadValidatorStatus.FailedDueToInvalidRequestPayload || 
                        objUploadValidatorResponse.UploadValidatorStatus == UploadValidatorStatus.FailedDuetoUnKnowError)
                    {
                        _objLoggingProvider.LogMessage(LogType.Info, "UploadValidatorStatus:"+ UploadValidatorStatus.FailedDuetoUnKnowError.ToString());
                        _objLoggingProvider.LogMessage(LogType.Info, "End: _ValidateExcelUploadDataByTemplateID");
                        TM.InvalidInfo = MismatchedColumns();
                        return PartialView("_InValidExcelData", TM);
                    }
                    else
                    {
                        _objLoggingProvider.LogMessage(LogType.Info, "UploadValidatorStatus: Unknow Status");
                        _objLoggingProvider.LogMessage(LogType.Info, "End: _ValidateExcelUploadDataByTemplateID");
                        TM.InvalidInfo = MismatchedColumns();
                        return PartialView("_InValidExcelData", TM);
                    }


                }
               

            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("_ValidateExcelDataByTemplateID :" , ex);
                throw;
            }

        }


        /// <summary>
        /// MismatchedColumns
        /// </summary>
        /// <returns></returns>
        private string MismatchedColumns()
        {
            var result = new StringBuilder();
            result.Append("<div class=\"panel-heading\">");
            result.Append("<h3 class=\"panel-title\">Invalid details, Please provide valid details</h3>");
            result.Append("</div>");


            return result.ToString();
        }


        /// <summary>
        /// _UpdateExcelDataIntoTemplateTableBYID
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="TemplateMasterID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _UpdateExcelDataIntoTemplateTableBYID(string FileName, int TemplateMasterID)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: _UpdateExcelDataIntoTemplateTableBYID");
                var flage = _objConfigureProvider.UpdateExcelDataIntoTemplateTables(FileName, TemplateMasterID);
                _objLoggingProvider.LogMessage(LogType.Info, "END: _UpdateExcelDataIntoTemplateTableBYID");
                if (flage == true)
                {
                    return Json("Success");
                }
                else
                {
                    return Json("Failed");
                }


            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("_UpdateExcelDataIntoTemplateTableBYID :" , ex);
                throw;
            }

        }


        #endregion

        #region ---------- Reportcontent ----------

        /// <summary>
        /// ReportContent
        /// </summary>
        /// <returns>rcVM</returns>
        public ActionResult ReportContent()
        {
            try
            {
                ReportContentViewModel rcVM = new();
                rcVM.templateModel = new();
                var TemplateName = "ReportTradeListContent";
                _objLoggingProvider.LogMessage(LogType.Info, "Start: ReportContent");
                var configContentId = _objPriceListProvider.GetPriceFileSaveConfig();
                var reportContents = _objConfigureProvider.GetTemplatesByCategory(TemplateName);
                var ddlTemplateMasterModel = (from n in reportContents
                                              select new DDLTemplateMasterDetails
                                              {
                                                  TemplateMasterID = n.TemplateMasterID,
                                                  TemplateName = n.CountryCode != "" ? (n.TemplateName + " - " + n.CountryCode) : n.TemplateName
                                              }).ToList();
                _objLoggingProvider.LogMessage(LogType.Info, "End: ReportContent");
                rcVM.templateModel.ddlTemplateMasterDetails = ddlTemplateMasterModel;
                if(configContentId != null)
                {
                    rcVM.templateModel.SelectedTemplateMasterID = Convert.ToInt32(configContentId.ReportContentTemplateID);
                    rcVM.templateModel.SelectedTemplateDetails = new();
                    var rptContent = reportContents.AsQueryable().Where(x => x.TemplateMasterID == Convert.ToInt32(configContentId.ReportContentTemplateID)).FirstOrDefault();
                    if (rptContent != null)
                    {
                        rcVM.templateModel.SelectedTemplateDetails.TemplateName = rptContent.TemplateName;
                    }

                }
                return View(rcVM);
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("ReportContent :" , ex);
                throw;
            }
        }

        /// <summary>
        /// AddReportContent
        /// </summary>
        ///<param name="CountryCode"></param>
        ///<param name="TemplateName"></param>
        /// <returns>JSON</returns>
        public ActionResult AddReportContent(ReportContentViewModel rcVM)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    _objLoggingProvider.LogMessage(LogType.Info, "Start: AddReportContent");
                    ReportContentModel rcM = new();
                    rcM.TemplateName = rcVM.TemplateName;
                    rcM.CountryCode = rcVM.CountryCode;
                    var addNewRC = _objConfigureProvider.SaveReportContent(rcM);
                    _objLoggingProvider.LogMessage(LogType.Info, "End: AddReportContent");
                    return Json(addNewRC);
                }
                else
                {
                    return Json(false);
                }
               
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("AddReportContent :" , ex);
                throw;
            }
        }


        /// <summary>
        /// IsTemplateAvailable
        /// </summary>
        /// <param name="TemplateName"></param>
        /// <returns>JSON</returns>
        
        public JsonResult IsTemplateAvailable(string TemplateName)
        {
            try
            {
                if (TemplateName != null)
                {
                    _objLoggingProvider.LogMessage(LogType.Info, "Start: IsTemplateAvailable");
                    var objTemplateInfo = _objConfigureProvider.GetTemplateInfoByTemplateName(TemplateName).FirstOrDefault();
                    if (objTemplateInfo != null)
                        return Json(false, new Newtonsoft.Json.JsonSerializerSettings());
                    _objLoggingProvider.LogMessage(LogType.Info, "End: IsTemplateAvailable");
                    return Json(true, new Newtonsoft.Json.JsonSerializerSettings());
                }
                else
                {
                    _objLoggingProvider.LogMessage(LogType.Info, "Info: Invalid Input");
                    return Json(false, new Newtonsoft.Json.JsonSerializerSettings());
                }
            }
            catch(Exception ex)
            {
                _objLoggingProvider.LogException("IsTemplateAvailable :" , ex);
                throw;
            }   
        }


        /// <summary>
        /// DeleteTemplateMaster
        /// </summary>
        /// <param name="TemplateMasterID"></param>
        /// <returns>JSON</returns>

        public JsonResult DeleteTemplateMaster(string TemplateMasterID)
        {
            try
            {
                int tempMasterID = Convert.ToInt32(TemplateMasterID);
                if(tempMasterID != 0)
                {
                    _objLoggingProvider.LogMessage(LogType.Info, "Start: DeleteTemplateMaster");
                    var templateDeleteInfo = _objConfigureProvider.DeleteTemplateMaster(tempMasterID);
                    _objLoggingProvider.LogMessage(LogType.Info, "End: DeleteTemplateMaster");
                    return Json(true, new Newtonsoft.Json.JsonSerializerSettings());
                }
                else
                {
                    _objLoggingProvider.LogMessage(LogType.Info, "Info: Invalid Selection");
                    return Json(false, new Newtonsoft.Json.JsonSerializerSettings());
                }
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("DeleteTemplateMaster :" , ex);
                throw;
            }
        }


        /// <summary>
        /// IsTemplateDeleted
        /// </summary>
        /// <param name="TemplateName"></param>
        /// /// <param name="CountryCode"></param>
        /// <returns>JSON</returns>
        public JsonResult IsTemplateDeleted(string TemplateName, string CountryCode)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: IsTemplateDeleted");
                var isDeletedTemplate = _objConfigureProvider.IsTemplateDeleted(TemplateName, CountryCode);
                _objLoggingProvider.LogMessage(LogType.Info, "End: IsTemplateDeleted");
                return Json(isDeletedTemplate, new Newtonsoft.Json.JsonSerializerSettings());
            }
            catch (Exception ex) 
            {
                _objLoggingProvider.LogException("IsTemplateDeleted :" , ex);
                throw;
            }
        }

        /// <summary>
        /// ReActivateTemplate
        /// </summary>
        /// <param name="TemplateName"></param>
        /// /// <param name="CountryCode"></param>
        /// <returns>JSON</returns>
        public JsonResult ReActivateTemplate(string TemplateName, string CountryCode)
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: ReActivateTemplate");
                var reActiveTemplate = _objConfigureProvider.ReActivateTemplate(TemplateName, CountryCode);
                _objLoggingProvider.LogMessage(LogType.Info, "End: ReActivateTemplate");
                return Json(reActiveTemplate, new Newtonsoft.Json.JsonSerializerSettings());
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("ReActivateTemplate :" , ex);
                throw;
            }
        }

        #endregion


        #region ---- customer Template -----
        /// <summary>
        /// CustomerTemplates
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerTemplates()
        {
            try
            {
                ReportContentViewModel rcVM = new();
                rcVM.templateModel = new();
                var TemplateName = "CustomerTemplates";
                _objLoggingProvider.LogMessage(LogType.Info, "Start: CustomerTemplates");
                var configContentId = _objPriceListProvider.GetPriceFileSaveConfig();
                var reportContents = _objConfigureProvider.GetTemplatesByCategory(TemplateName);
                var ddlTemplateMasterModel = (from n in reportContents
                                              select new DDLTemplateMasterDetails
                                              {
                                                  TemplateMasterID = n.TemplateMasterID,
                                                  TemplateName = n.CountryCode != "" ? (n.TemplateName + " - " + n.CountryCode) : n.TemplateName
                                              }).ToList();
                _objLoggingProvider.LogMessage(LogType.Info, "End: CustomerTemplates");
                rcVM.templateModel.ddlTemplateMasterDetails = ddlTemplateMasterModel;
                if (configContentId != null)
                {
                    rcVM.templateModel.SelectedTemplateMasterID = Convert.ToInt32(configContentId.ReportContentTemplateID);
                    rcVM.templateModel.SelectedTemplateDetails = new();
                    var rptContent = reportContents.AsQueryable().Where(x => x.TemplateMasterID == Convert.ToInt32(configContentId.ReportContentTemplateID)).FirstOrDefault();
                    if (rptContent != null)
                    {
                        rcVM.templateModel.SelectedTemplateDetails.TemplateName = rptContent.TemplateName;
                    }

                }
                return View(rcVM);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// AddReportContent
        /// </summary>
        ///<param name="CountryCode"></param>
        ///<param name="TemplateName"></param>
        /// <returns>JSON</returns>
        public ActionResult AddCustomerTemplate(ReportContentViewModel rcVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _objLoggingProvider.LogMessage(LogType.Info, "Start: AddReportContent");
                    ReportContentModel rcM = new();
                    rcM.TemplateName = rcVM.TemplateName;
                    rcM.CountryCode = rcVM.CountryCode;
                    var addNewRC = _objConfigureProvider.SaveCustomerTemplate(rcM);
                    _objLoggingProvider.LogMessage(LogType.Info, "End: AddReportContent");
                    return Json(addNewRC);
                }
                else
                {
                    return Json(false);
                }

            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("CustomerTemplates :" , ex);
                throw;
            }
        }

        #endregion


        #region ---- Application Settings -----
        /// <summary>
        /// ApplicationSettings
        /// </summary>
        /// <returns></returns>
        [PFCRoleBasedAuthorizeFilter]
        [PFCSessionExpireFilter]
        public ActionResult ApplicationSettings()
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: AppConfig");
                List<AppConfigSettingVM> appSettingVM = new();
                var lstAppConfigSetting = _objConfigureProvider.GetAppConfigSettings();
                var lstConfigOptions = _objConfigureProvider.GetAppConfigOptions();
                if(lstAppConfigSetting != null && lstConfigOptions != null)
                {
                    for(var i = 0; i< lstAppConfigSetting.Count; i++)
                    {
                        List<ConfigOptions> configOptions = new List<ConfigOptions>();
                        configOptions = lstConfigOptions.Where(x => x.ConfigType == lstAppConfigSetting[i].ConfigType).ToList();
                        appSettingVM.Add(new AppConfigSettingVM
                        {
                            AppConfigID = lstAppConfigSetting[i].AppConfigID,
                            ConfigName = lstAppConfigSetting[i].ConfigName,
                            AliasName = lstAppConfigSetting[i].AliasName,
                            Description = lstAppConfigSetting[i].Description,
                            ConfigValue = lstAppConfigSetting[i].ConfigValue,
                            ConfigType = lstAppConfigSetting[i].ConfigType,
                            ConfigDataType = lstAppConfigSetting[i].ConfigDataType,
                            ConfigUIType = lstAppConfigSetting[i].ConfigUIType,
                            ConfigMinLength = lstAppConfigSetting[i].ConfigMinLength,
                            ConfigMaxLength = lstAppConfigSetting[i].ConfigMaxLength,
                            SequenceNo = lstAppConfigSetting[i].SequenceNo,
                            IsActive = lstAppConfigSetting[i].IsActive,
                            IsDeleted = lstAppConfigSetting[i].IsDeleted,
                            CreatedBy = lstAppConfigSetting[i].CreatedBy,
                            ModifiedBy = lstAppConfigSetting[i].ModifiedBy,
                            CreatedDate = lstAppConfigSetting[i].CreatedDate,
                            ModifiedDate = lstAppConfigSetting[i].ModifiedDate,
                            configOptions = configOptions
                        });
                    }
                }
                _objLoggingProvider.LogMessage(LogType.Info, "End: AppConfig");
                return View(appSettingVM);
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("ApplicationSettings :" , ex);
                throw;
            }
        }

        /// <summary>
        /// ModifyAppConfigSetting
        /// </summary>
        /// <param name="objAppConfigSettingModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PFCSessionExpireFilter]
        public ActionResult ModifyAppConfigSetting(List<AppConfigSettingVM> objAppConfigSettingModel)

        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: ModifyAPPConfigSetting");
                if (ModelState.IsValid)
                {
                    List<AppConfigSetting> _lstappConfigSettings =  new List<AppConfigSetting>();
                    for (var i = 0; i < objAppConfigSettingModel.Count; i++)
                    {
                        _lstappConfigSettings.Add(new AppConfigSetting
                        {
                            AppConfigID = objAppConfigSettingModel[i].AppConfigID,
                            ConfigName = objAppConfigSettingModel[i].ConfigName,
                            AliasName = objAppConfigSettingModel[i].AliasName,
                            Description = objAppConfigSettingModel[i].Description,
                            ConfigValue = objAppConfigSettingModel[i].ConfigValue,
                            ConfigType = objAppConfigSettingModel[i].ConfigType,
                            ConfigDataType = objAppConfigSettingModel[i].ConfigDataType,
                            ConfigUIType = objAppConfigSettingModel[i].ConfigUIType,
                            ConfigMinLength = objAppConfigSettingModel[i].ConfigMinLength,
                            ConfigMaxLength = objAppConfigSettingModel[i].ConfigMaxLength,
                            SequenceNo = objAppConfigSettingModel[i].SequenceNo,
                            IsActive = objAppConfigSettingModel[i].IsActive,
                            IsDeleted = objAppConfigSettingModel[i].IsDeleted,
                            CreatedBy = objAppConfigSettingModel[i].CreatedBy == null ? _objCommonProvider.GetLoginUserSESA() : objAppConfigSettingModel[i].CreatedBy,
                            ModifiedBy = objAppConfigSettingModel[i].ModifiedBy == null ? _objCommonProvider.GetLoginUserSESA() : objAppConfigSettingModel[i].ModifiedBy,
                            CreatedDate = objAppConfigSettingModel[i].CreatedDate,
                            ModifiedDate = objAppConfigSettingModel[i].ModifiedDate
                        });
                    }
                    var lstAPPConfigSettingsDetails = _objConfigureProvider.SaveAppConfigSettingDetails(_lstappConfigSettings);
                    _objLoggingProvider.LogMessage(LogType.Info, "End: ModifyAPPConfigSetting");
                    return RedirectToAction("ApplicationSettings");
                }
                _objLoggingProvider.LogMessage(LogType.Info, "End: ModifyAPPConfigSetting");


                return RedirectToAction("ApplicationSettings");
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("ModifyAppConfigSetting :" , ex);
                throw;
            }
        }

        public ActionResult ApplicationSettingsJson()
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: AppConfig");
                List<AppConfigSettingVM> appSettingVM = new();
                var lstAppConfigSetting = _objConfigureProvider.GetAppConfigSettings();
                var lstConfigOptions = _objConfigureProvider.GetAppConfigOptions();
                if (lstAppConfigSetting != null && lstConfigOptions != null)
                {
                    for (var i = 0; i < lstAppConfigSetting.Count; i++)
                    {
                        List<ConfigOptions> configOptions = new List<ConfigOptions>();
                        configOptions = lstConfigOptions.Where(x => x.ConfigType == lstAppConfigSetting[i].ConfigType).ToList();
                        appSettingVM.Add(new AppConfigSettingVM
                        {
                            AppConfigID = lstAppConfigSetting[i].AppConfigID,
                            ConfigName = lstAppConfigSetting[i].ConfigName,
                            AliasName = lstAppConfigSetting[i].AliasName,
                            Description = lstAppConfigSetting[i].Description,
                            ConfigValue = lstAppConfigSetting[i].ConfigValue,
                            ConfigType = lstAppConfigSetting[i].ConfigType,
                            ConfigDataType = lstAppConfigSetting[i].ConfigDataType,
                            ConfigUIType = lstAppConfigSetting[i].ConfigUIType,
                            ConfigMinLength = lstAppConfigSetting[i].ConfigMinLength,
                            ConfigMaxLength = lstAppConfigSetting[i].ConfigMaxLength,
                            SequenceNo = lstAppConfigSetting[i].SequenceNo,
                            IsActive = lstAppConfigSetting[i].IsActive,
                            IsDeleted = lstAppConfigSetting[i].IsDeleted,
                            CreatedBy = lstAppConfigSetting[i].CreatedBy,
                            ModifiedBy = lstAppConfigSetting[i].ModifiedBy,
                            CreatedDate = lstAppConfigSetting[i].CreatedDate,
                            ModifiedDate = lstAppConfigSetting[i].ModifiedDate,
                            configOptions = configOptions
                        });
                    }
                }
                _objLoggingProvider.LogMessage(LogType.Info, "End: AppConfig");
                return Json(appSettingVM, new Newtonsoft.Json.JsonSerializerSettings());
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("ApplicationSettings :", ex);
                throw;
            }
        }

        #endregion

        #region ----- User Log --------
        /// <summary>
        /// Retrive Users Log Data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetUsersLogData()
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Start: GetUsersLogData using Ajax Call");
                var userslogInfo = _objConfigureProvider.GetUsersLogInfo();
                _objLoggingProvider.LogMessage(LogType.Info, "End: GetUsersLogData using Ajax Call");
                return Json(userslogInfo, new Newtonsoft.Json.JsonSerializerSettings());
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("GetUsersLogData :", ex);
                throw;
            }
        }
        #endregion
    }
}
