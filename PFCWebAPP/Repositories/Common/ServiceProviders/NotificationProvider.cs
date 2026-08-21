using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using PFCWebAPP.DatabaseContext.Models.CustomTables;
using PFCWebAPP.Repositories.Common.Enums;
using PFCWebAPP.Repositories.Common.Interfaces;
using PFCWebAPP.Repositories.Common.Models;
using PFCWebAPP.Repositories.Configure;
using PFCWebAPP.Repositories.Configure.Interfaces;
using PFCWebAPP.Repositories.PriceList;
using PFCWebAPP.Repositories.PriceList.Interfaces;
using PFCWebAPP.Repositories.PriceList.Models.API;
using PFCWebAPP.Repositories.PriceList.Models.IntermediateModels;
using PFCWebAPP.Repositories.PriceList.ServiceProviders;
using PFCWebAPP.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace PFCWebAPP.Repositories.Common.ServiceProviders
{
    public class NotificationProvider : INotificationProvider
    {
        private readonly ILoggingProvider _objLoggingProvider;
        private readonly IHttpContextAccessor _objHttpContextAccessor;
        private readonly IMailSenderProvider _objMailSenderProvider;
        private readonly IPriceListRepository _objPriceListRepository;
        private readonly ICommonProvider _objCommonProvider;
        private readonly IConfigureRepository _objConfigureRepository;
        private readonly IConfigureProvider _objConfigureProvider;
        private readonly ICommonRepository _objCommonRepository;

        public NotificationProvider(ILoggingProvider objLoggingProvider, IHttpContextAccessor objHttpContextAccessor, IMailSenderProvider objMailSenderProvider, IPriceListRepository objPriceListRepository, ICommonProvider objCommonProvider, IConfigureRepository objConfigureRepository, IConfigureProvider objConfigureProvider, ICommonRepository objCommonRepository)
        {
            _objLoggingProvider = objLoggingProvider;
            _objHttpContextAccessor = objHttpContextAccessor;
            _objMailSenderProvider = objMailSenderProvider;
            _objPriceListRepository = objPriceListRepository;
            _objCommonProvider = objCommonProvider;
            _objConfigureRepository = objConfigureRepository;
            _objConfigureProvider = objConfigureProvider;
            _objCommonRepository = objCommonRepository;            
        }

        public async Task<bool> SendMailByPriceFileID(long PriceFileHeaderID)
        {
            try
            {
                NotificationInfo objNotificationInfo = new NotificationInfo();
                List<PriceFileLocationDetails> lstPFCAttachments = new List<PriceFileLocationDetails>();
                //Step1:get PriceFileLocationDetails details using PriceFileHeaderID
                //Step2: Get Notification TemplateInfo Based on SalesOrganization & TemplateName
                //Step3: Replace TemplateVariables in Notification Info
                //Setp4: Get all Required Info for Notification
                foreach (var att in lstPFCAttachments)
                {
                    // we need to add based on PFCFileLocationMode: AWS/ApplicationServer
                    var _attachments = new Hashtable();
                    string ExcelfileURL = PFCWebAPP.Utilities.AppConfig.PFCDownloadedFileLoaction + "\\" + att.PFCEncryptedFileName;
                    _attachments.Add(att.PFCActualFileName, ExcelfileURL);
                    //Step5: SaveNotificationHistory Info
                    //Step6: SendMail
                    //Step7: Update Mail Status in NotificationHistory in SendCompletedCallback method in MailSenderProvider

                    var a = await _objMailSenderProvider.SendMailAsync(objNotificationInfo, _attachments);
                }

                return true;
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("SendMailByPriceFileID", ex);
                return false;
            }
        }

        public async Task<bool> SendMailByPriceFileLocationDetails(List<PriceFileLocationDetails> lstPFCAttachments)
        {
            try
            {
                NotificationInfo objNotificationInfo = new NotificationInfo();
                //get PriceFileLocationDetails details using PriceFileHeaderID
                foreach (var att in lstPFCAttachments)
                {
                    // we need to add based on PFCFileLocationMode: AWS/ApplicationServer
                    //Step1: Get Notification TemplateInfo Based on SalesOrganization & TemplateName
                    //Step2: Replace TemplateVariables in Notification Info
                    //Setp3: Get all Required Info for Notification
                    var _attachments = new Hashtable();
                    string ExcelfileURL = PFCWebAPP.Utilities.AppConfig.PFCDownloadedFileLoaction + "\\" + att.PFCEncryptedFileName;
                    _attachments.Add(att.PFCActualFileName, ExcelfileURL);
                    //Step4: SaveNotificationHistory Info
                    //Step5: SendMail
                    //Step6: Update Mail Status in NotificationHistory in SendCompletedCallback method in MailSenderProvider
                    var a = await _objMailSenderProvider.SendMailAsync(objNotificationInfo, _attachments);
                }

                return true;
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("SendMailByPriceFileID", ex);
                return false;
            }
        }

        public async Task<List<long>> SendMailToPriceFileLocationCustomers(long PriceFileHeaderID, string CustomersWithPriceHeaderId)
        {
            List<long> NotificationHistoryIds = new();
            try
            {
                List<SelectedCustomersByHeaderID> lstcustomers = JsonConvert.DeserializeObject<List<SelectedCustomersByHeaderID>>(CustomersWithPriceHeaderId);
                List<PriceFileLocationDetails> lstPFCAttachments = new();
                List<PriceFileLocationDetails> filteredCustomersLocation = new();
                //get PriceFileLocationDetails details using PriceFileHeaderID
                lstPFCAttachments = _objPriceListRepository.PriceFileLocationDetailsRepository
                                .GetManyQueryable(p => p.PriceFileHeaderID == PriceFileHeaderID && p.IsActive == true)
                                .ToList();
                string ArchivedRecords = _objCommonProvider.GetAppSettingByName(Constants.ArchivedFileLocationMode);
                string ArchivedExtraction = _objCommonProvider.GetAppSettingByName(Constants.ArchivedFileExtractionMode);
                var UserConfig = (from ph in _objPriceListRepository.PriceFileHeaderRepository.GetManyQueryable()
                                  join con in _objConfigureRepository.UserConfigSettingRepository.GetManyQueryable()
                                  on ph.UserConfigSettingID equals con.UserConfigSettingID
                                  where ph.PriceFileHeaderID == PriceFileHeaderID
                                  select new
                                  {
                                      UserConfigSettingID = con.UserConfigSettingID,
                                      Organization = con.SalesOrganization
                                  }
                                    ).FirstOrDefault();
                var NotificationTemplateName = _objCommonProvider.GetNotificationTemplateNameByOrg(UserConfig.Organization);
                var templateInfo = _objCommonRepository.NotificationTemplatesRepository
                                .GetManyQueryable(s => s.SalesOrganization == UserConfig.Organization && s.TemplateName.ToLower() == NotificationTemplateName.ToLower())
                                .FirstOrDefault();
                string[] tempvars = templateInfo.TemplateVars.Split(',').Select(p => p.Trim()).ToArray();
                string tmpsubj = templateInfo.TemplateSubject;

                string defaultEmailToStr = string.Empty;
                string defaultCcToStr = string.Empty;
                string defaultBccToStr = string.Empty;
                if (!string.IsNullOrWhiteSpace(templateInfo.DefaultSentTo))
                {
                    defaultEmailToStr = templateInfo.DefaultSentTo;
                }
                if (!string.IsNullOrWhiteSpace(templateInfo.DefaultCcTo))
                {
                    defaultCcToStr = templateInfo.DefaultCcTo;
                }
                if (!string.IsNullOrWhiteSpace(templateInfo.DefaultBccTo))
                {
                    defaultBccToStr = templateInfo.DefaultBccTo;
                }

                filteredCustomersLocation = (from cust in lstPFCAttachments
                                             join lst in lstcustomers
                                             on cust.CustomerNo equals lst.zKUNNR
                                             select cust
                                             ).ToList();
                if (ArchivedRecords == Constants.ApplicationServer || ArchivedRecords == Constants.AWSS3Bucket)
                {
                    foreach (var att in filteredCustomersLocation)
                    {
                        var custList = _objCommonRepository.CustomerContactsRepository
                                    .GetQueryable(q => q.AccountNumber == att.CustomerNo.TrimStart('0'))
                                    .GroupBy(g => g.AccountNumber)
                                    .Select(s => new
                                    {
                                        Customer_No = s.Key,
                                        Customer_Name = s.First().AccountName,
                                        ToEmail = s.Select(to => to.ToEmailID),
                                        CcEmailID = s.Select(cc => cc.CcEmailID),
                                        BccEmailID = s.Select(bcc => bcc.BccEmailID),
                                        zKUNNR = att.CustomerNo
                                    })
                                    .FirstOrDefault();
                        
                        var ApiCustomerDetails = new
                        {
                            Customer_No = lstcustomers.FirstOrDefault().CustomerNumber,
                            Customer_Name = lstcustomers.FirstOrDefault().CustomerName
                        };

                        NotificationInfo objNotificationInfo = new NotificationInfo();
                        var _attachments = new Hashtable();
                        var isApi = _objHttpContextAccessor.HttpContext.Session.GetString("isFromApi");
                        if (custList != null || isApi == "true")
                        {
                            string subjectString = string.Empty;
                            subjectString = tmpsubj;
                            // Forming subject string

                            if (isApi == "true")
                            {
                                foreach (string key in tempvars)
                                {
                                    string placeholder = $"{{{{{key}}}}}";
                                    object value = ApiCustomerDetails.GetType().GetProperty(key)?.GetValue(ApiCustomerDetails, null);
                                    if (value != null)
                                    {
                                        subjectString = subjectString.Replace(placeholder, value.ToString());
                                    }
                                }
                            }
                            else
                            {
                                foreach (string key in tempvars)
                                {
                                    string placeholder = $"{{{{{key}}}}}";
                                    object value = custList.GetType().GetProperty(key)?.GetValue(custList, null);
                                    if (value != null)
                                    {
                                        subjectString = subjectString.Replace(placeholder, value.ToString());
                                    }
                                }
                            }


                            string finalEamilto = "";
                            string finalCcto = "";
                            string finalBccto = "";
                            // For API Calls
                            if (isApi == "true")
                            {
                                var ApiCustomerEmail = _objHttpContextAccessor.HttpContext.Session.GetString("ApiCustomerEmail");
                                var ApiCustomerName = ApiCustomerDetails.Customer_Name;
                                var ApiCustomerNo = ApiCustomerDetails.Customer_No;
                                finalEamilto = ApiCustomerEmail;
                                finalCcto = "sreeranga.vinjamuri@non.se.com";
                                finalBccto = "bhavana.adari@non.se.com";
                            }
                            else
                            {
                                // Forming Email to
                                string custEmailto = string.Concat(custList.ToEmail.AsEnumerable().Select(s => s).ToArray());
                                finalEamilto = string.Concat(custEmailto, defaultEmailToStr);
                                // Forming CC Email to
                                string custCcto = string.Concat(custList.CcEmailID.AsEnumerable().Select(s => s).ToArray());
                                finalCcto = string.Concat(custCcto, defaultCcToStr);
                                // Forming Bcc Email to
                                string custBccto = string.Concat(custList.BccEmailID.AsEnumerable().Select(s => s).ToArray());
                                finalBccto = string.Concat(custBccto, defaultCcToStr);

                            }

                            objNotificationInfo.NotificationTemplateID = templateInfo.NotificationTemplateID;
                            objNotificationInfo.PriceFileHeaderID = PriceFileHeaderID;
                            objNotificationInfo.NotificationDate = DateTime.Now;
                            objNotificationInfo.Subject = subjectString;
                            objNotificationInfo.Body = "<html><body>" + templateInfo.TemplateBody + "</body></html>";
                            objNotificationInfo.Attachments = "";
                            objNotificationInfo.SentTo = finalEamilto.Replace(",", ";");
                            objNotificationInfo.CcTo = finalCcto.Replace(",", ";");
                            objNotificationInfo.BccTo = finalBccto.Replace(",", ";");
                            objNotificationInfo.Priority = templateInfo.Priority == 1 ? NotificationPriority.Low : templateInfo.Priority == 2 ? NotificationPriority.High : NotificationPriority.Low;
                            objNotificationInfo.StatusDate = DateTime.Now;

                            var filepath = PFCWebAPP.Utilities.AppConfig.PFCDownloadedFileLoaction + "\\" + att.PFCEncryptedFileName;
                            byte[] fileBytes = File.ReadAllBytes(filepath);
                            string filebase64 = Convert.ToBase64String(fileBytes);
                            objNotificationInfo.Attachments += filebase64;

                            string ExcelfileURL = Path.Combine(Directory.GetCurrentDirectory(), AppConfig.PFCDownloadedFileLoaction + "\\" + att.PFCEncryptedFileName);
                            _attachments.Add(att.PFCActualFileName, ExcelfileURL);
                        }

                        using (ISqlHelper objSqlHelper = new SqlHelper(AppConfig.ConnectionString))
                        {
                            string strUpdateQuery = "INSERT INTO dbo.TRN_NotificationHistory(NotificationDate,Subject,Body,SentTo,CcTo,BccTo,Priority,AttachmentPath,ActualFileName,EncryptedFileName,NotificationTemplateID,PriceFileHeaderID,PriceFileLocationID,IsActive,CreatedBy) " +
                                "values(@NotificationDate,@Subject,@Body,@SentTo,@CcTo,@BccTo,@Priority,@AttachmentPath,@ActualFileName,@EncryptedFileName,@NotificationTemplateID,@PriceFileHeaderID,@PriceFileLocationID,@IsActive,@CreatedBy); SELECT SCOPE_IDENTITY(); ";
                            string UserSESA = _objCommonProvider.GetLoginUserSESA();
                            List<SqlParameter> lstSqlParameters = new List<SqlParameter>();
                            lstSqlParameters.Add(new SqlParameter() { ParameterName = "@NotificationDate", Value = DateTime.UtcNow });
                            lstSqlParameters.Add(new SqlParameter() { ParameterName = "@Subject", Value = objNotificationInfo.Subject });
                            lstSqlParameters.Add(new SqlParameter() { ParameterName = "@Body", Value = objNotificationInfo.Body });
                            lstSqlParameters.Add(new SqlParameter() { ParameterName = "@SentTo", Value = objNotificationInfo.SentTo });
                            lstSqlParameters.Add(new SqlParameter() { ParameterName = "@CcTo", Value = objNotificationInfo.CcTo });
                            lstSqlParameters.Add(new SqlParameter() { ParameterName = "@BccTo", Value = objNotificationInfo.BccTo });
                            lstSqlParameters.Add(new SqlParameter() { ParameterName = "@Priority", Value = (int)objNotificationInfo.Priority });
                            lstSqlParameters.Add(new SqlParameter() { ParameterName = "@AttachmentPath", Value = AppConfig.PFCDownloadedFileLoaction });
                            lstSqlParameters.Add(new SqlParameter() { ParameterName = "@ActualFileName", Value = att.PFCActualFileName });
                            lstSqlParameters.Add(new SqlParameter() { ParameterName = "@EncryptedFileName", Value = att.PFCEncryptedFileName });
                            lstSqlParameters.Add(new SqlParameter() { ParameterName = "@NotificationTemplateID", Value = templateInfo.NotificationTemplateID });
                            lstSqlParameters.Add(new SqlParameter() { ParameterName = "@PriceFileHeaderID", Value = PriceFileHeaderID });
                            lstSqlParameters.Add(new SqlParameter() { ParameterName = "@PriceFileLocationID", Value = att.PriceFileLocationID });
                            lstSqlParameters.Add(new SqlParameter() { ParameterName = "@IsActive", Value = true });
                            lstSqlParameters.Add(new SqlParameter() { ParameterName = "@CreatedBy", Value = UserSESA });

                            long output = Convert.ToInt32(objSqlHelper.ExecuteScalarQuery(CommandType.Text, strUpdateQuery, lstSqlParameters.ToArray()));
                            objNotificationInfo.NotificationID = output;
                            NotificationHistoryIds.Add(output);
                        }
                        var a = await _objMailSenderProvider.SendMailAsync(objNotificationInfo, _attachments);
                    }
                }
                //NotificationHistoryIds
                return NotificationHistoryIds;
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("SendMailToPriceFileLocationCustomers", ex);
                return NotificationHistoryIds;
            }
        }

        /// <summary>
        /// GetUsers
        /// </summary>
        /// <returns>lst_UserMaster</returns>
        /// param :
        public IQueryable<NotificationHistory> GetNotificationHistory()
        {
            try
            {
                string UserSESA = _objCommonProvider.GetLoginUserSESA();
                int MaxCount = 5000;
                try
                {
                    string DisplayMaxRecords = _objCommonProvider.GetAppSettingByName(Constants.DisplayMaxRecords);
                    if (DisplayMaxRecords != "" && DisplayMaxRecords != null)
                    {
                        MaxCount = Convert.ToInt32(DisplayMaxRecords);
                    }
                }
                catch (Exception ex1)
                {
                    _objLoggingProvider.LogException("GetNotificationHistory(DisplayMaxRecords) :", ex1);
                }
                _objLoggingProvider.LogMessage(LogType.Info, "Start: GetNotificationHistory List");
                IQueryable<NotificationHistory>? lst_notifyhistory = null;
                if (_objCommonProvider.IsAdminUser(UserSESA))
                {
                    lst_notifyhistory = _objCommonRepository.NotificationHistoryRepository.GetManyQueryable().Where(x => x.IsDeleted == false && x.IsActive == true).OrderByDescending(x => x.NotificationHistoryID).Take(MaxCount);
                }
                else
                {
                    lst_notifyhistory = _objCommonRepository.NotificationHistoryRepository.GetManyQueryable().Where(x => x.IsDeleted == false && x.IsActive == true && x.CreatedBy == UserSESA).OrderByDescending(x => x.NotificationHistoryID).Take(MaxCount);
                }
                _objLoggingProvider.LogMessage(LogType.Info, "End: GetNotificationHistory List");
                return lst_notifyhistory;
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("GetNotificationHistory :", ex);
                throw;
            }
        }

        public async Task<bool> ReSendEmailNotification(long notificationId)
        {
            try
            {
                NotificationInfo objNotificationInfo = new NotificationInfo();
                using (ISqlHelper objSqlHelper = new SqlHelper(AppConfig.ConnectionString))
                {
                    string strUpdateQuery = "Update dbo.TRN_NotificationHistory SET ResendStatus = @ResendStatus WHERE NotificationHistoryID = @NotificationHistoryID";

                    List<SqlParameter> lstParameters = new List<SqlParameter>();
                    lstParameters.Add(new SqlParameter() { ParameterName = "@ResendStatus", Value = "In Progress" });
                    lstParameters.Add(new SqlParameter() { ParameterName = "@NotificationHistoryID", Value = notificationId });

                    int output = objSqlHelper.ExecuteNonQuery(CommandType.Text, strUpdateQuery, lstParameters.ToArray());
                }
                var notificationHistory = _objCommonRepository.NotificationHistoryRepository.GetEntityByID(notificationId);
                if (notificationHistory != null)
                {
                    objNotificationInfo.NotificationID = notificationId;
                    objNotificationInfo.NotificationDate = notificationHistory.NotificationDate;
                    objNotificationInfo.NotificationTemplateID = notificationHistory.NotificationTemplateID;
                    objNotificationInfo.PriceFileHeaderID = notificationHistory.PriceFileHeaderID;
                    objNotificationInfo.NotificationDate = notificationHistory.NotificationDate;
                    objNotificationInfo.Subject = notificationHistory.Subject;
                    objNotificationInfo.Body = notificationHistory.Body;
                    objNotificationInfo.SentTo = notificationHistory.SentTo;
                    objNotificationInfo.CcTo = notificationHistory.CcTo;
                    objNotificationInfo.BccTo = notificationHistory.BccTo;
                    objNotificationInfo.Priority = (NotificationPriority)notificationHistory.Priority;
                    objNotificationInfo.ResendCount = (int)notificationHistory.ResendCount;
                    objNotificationInfo.IsResend = true;
                }
                var _attachments = new Hashtable();
                string ExcelfileURL = Path.Combine(Directory.GetCurrentDirectory(), AppConfig.PFCDownloadedFileLoaction + "\\" + notificationHistory.EncryptedFileName);
                _attachments.Add(notificationHistory.ActualFileName, ExcelfileURL);
                //Step4: SaveNotificationHistory Info
                //Step5: SendMail
                //Step6: Update Mail Status in NotificationHistory in SendCompletedCallback method in MailSenderProvider
                var a = await _objMailSenderProvider.SendMailAsync(objNotificationInfo, _attachments);

                int maximumRetryAttempts = 20;
                bool IsSendmail = false;
                string reSendStatus = "";

                for (int attempted = 0; attempted < maximumRetryAttempts; attempted++)
                {
                    using (ISqlHelper objSqlHelper = new SqlHelper())
                    {
                        string strQuery = "select ResendStatus from dbo.TRN_NotificationHistory WHERE NotificationHistoryID = @NotificationHistoryID";

                        List<SqlParameter> lstSqlParameters = new List<SqlParameter>();

                        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@NotificationHistoryID", Value = notificationId });

                        var dtRoleMenus = objSqlHelper.ExecuteTable(CommandType.Text, strQuery, lstSqlParameters.ToArray());

                        if (dtRoleMenus.Rows.Count > 0)
                        {
                            string resendStatus = dtRoleMenus.Rows[0]["ResendStatus"].ToString();
                            //if (dtRoleMenus == "SUCCESS".ToLower())
                            if (resendStatus.ToLower() == "SUCCESS".ToLower())
                            {
                                await Task.Delay(2000);
                                IsSendmail = true;
                                break;
                            }
                            else
                            {
                                Task.Delay(TimeSpan.FromSeconds(3)).Wait();
                            }
                        }
                    }
                }

                return IsSendmail;
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("ReSendEmailNotification :", ex);
                throw;
            }
        }

        public bool GetMailStatusInNotificationHistory(List<long> ids)
        {
            try
            {
                var result = _objCommonRepository.NotificationHistoryRepository.GetManyQueryable()
                    .Where(s => ids.Contains(s.NotificationHistoryID))
                    .ToList();
                bool status = result.All(a => a.Status.Contains("SUCCESS"));

                return status;
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("GetNotificationHistory :", ex);
                throw;
            }
        }

        public async Task<List<long>> SendNotificationForMissingCustomerSettings(string CustomerNo, string CustomerName, string SalesOrg)
        {
            List<long> NotificationHistoryIds = new();
            try
            {
                var NotificationTemplateName = _objCommonProvider.GetCustomerValidationNotificationTemplate(SalesOrg.ToString().ToUpper());
                var templateInfo = _objCommonRepository.NotificationTemplatesRepository
                                .GetManyQueryable(s => s.SalesOrganization.ToString().ToUpper() == SalesOrg.ToString().ToUpper() && s.TemplateName.ToString().ToUpper() == NotificationTemplateName.ToString().ToUpper())
                                .FirstOrDefault();

                string tmpsubj = templateInfo.TemplateSubject;
                string subjectString = tmpsubj;
                string BodyString = templateInfo.TemplateBody;

                string defaultEmailToStr = string.Empty;
                string defaultCcToStr = string.Empty;
                string defaultBccToStr = string.Empty;
                if (!string.IsNullOrWhiteSpace(templateInfo.DefaultSentTo))
                {
                    defaultEmailToStr = templateInfo.DefaultSentTo;
                }
                if (!string.IsNullOrWhiteSpace(templateInfo.DefaultCcTo))
                {
                    defaultCcToStr = templateInfo.DefaultCcTo;
                }
                if (!string.IsNullOrWhiteSpace(templateInfo.DefaultBccTo))
                {
                    defaultBccToStr = templateInfo.DefaultBccTo;
                }

                NotificationInfo objNotificationInfo = new NotificationInfo();
                var _attachments = new Hashtable();

                Dictionary<string, string> tempVars = new Dictionary<string, string>
                {
                    { "Customer_No", CustomerNo },
                    { "Customer_Name", CustomerName },
                    { "Sales_Org", SalesOrg }
                };

                foreach (string key in tempVars.Keys)
                {
                    string placeholder = $"{{{{{key}}}}}";
                    string value = tempVars[key];
                    subjectString = subjectString.Replace(placeholder, value);
                    BodyString = BodyString.Replace(placeholder, value);
                }

                objNotificationInfo.NotificationTemplateID = templateInfo.NotificationTemplateID;
                objNotificationInfo.PriceFileHeaderID = 0;
                objNotificationInfo.NotificationDate = DateTime.Now;
                objNotificationInfo.Subject = subjectString;
                objNotificationInfo.Body = "<html><body>" + templateInfo.TemplateBody + "</body></html>";
                objNotificationInfo.Attachments = "";
                objNotificationInfo.SentTo = defaultEmailToStr.Replace(",", ";");
                objNotificationInfo.CcTo = defaultCcToStr.Replace(",", ";");
                objNotificationInfo.BccTo = defaultBccToStr.Replace(",", ";");
                objNotificationInfo.Priority = templateInfo.Priority == 1 ? NotificationPriority.Low : templateInfo.Priority == 2 ? NotificationPriority.High : NotificationPriority.Low;
                objNotificationInfo.StatusDate = DateTime.Now;


                using (ISqlHelper objSqlHelper = new SqlHelper(AppConfig.ConnectionString))
                {
                    string strUpdateQuery = "INSERT INTO dbo.TRN_NotificationHistory(NotificationDate,Subject,Body,SentTo,CcTo,BccTo,Priority,AttachmentPath,ActualFileName,EncryptedFileName,NotificationTemplateID,PriceFileHeaderID,PriceFileLocationID,IsActive,CreatedBy) " +
                        "values(@NotificationDate,@Subject,@Body,@SentTo,@CcTo,@BccTo,@Priority,@AttachmentPath,@ActualFileName,@EncryptedFileName,@NotificationTemplateID,@PriceFileHeaderID,@PriceFileLocationID,@IsActive,@CreatedBy); SELECT SCOPE_IDENTITY(); ";
                    string UserSESA = _objCommonProvider.GetLoginUserSESA();
                    List<SqlParameter> lstSqlParameters = new List<SqlParameter>();
                    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@NotificationDate", Value = DateTime.UtcNow });
                    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@Subject", Value = objNotificationInfo.Subject });
                    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@Body", Value = objNotificationInfo.Body });
                    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@SentTo", Value = objNotificationInfo.SentTo });
                    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@CcTo", Value = objNotificationInfo.CcTo });
                    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@BccTo", Value = objNotificationInfo.BccTo });
                    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@Priority", Value = (int)objNotificationInfo.Priority });
                    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@AttachmentPath", Value = AppConfig.PFCDownloadedFileLoaction });
                    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@ActualFileName", Value = "" });
                    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@EncryptedFileName", Value = "" });
                    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@NotificationTemplateID", Value = templateInfo.NotificationTemplateID });
                    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@PriceFileHeaderID", Value = 0 });
                    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@PriceFileLocationID", Value = "" });
                    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@IsActive", Value = true });
                    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@CreatedBy", Value = UserSESA });

                    long output = Convert.ToInt32(objSqlHelper.ExecuteScalarQuery(CommandType.Text, strUpdateQuery, lstSqlParameters.ToArray()));
                    objNotificationInfo.NotificationID = output;
                    NotificationHistoryIds.Add(output);
                }
                var a = await _objMailSenderProvider.SendMailAsync(objNotificationInfo, _attachments);

                return NotificationHistoryIds;
            }
            catch(Exception ex)
            {
                _objLoggingProvider.LogException("SendNotificationForMissingCustomerSettings", ex);
                return NotificationHistoryIds;
            }
        }

        public async Task<List<long>> SendNotificationForUnAuthorized(string SalesOrg, string Createdby)
        {
            List<long> NotificationHistoryIds = new();
            try
            {
                var templateInfo = _objCommonRepository.NotificationTemplatesRepository
                                .GetManyQueryable(s => s.SalesOrganization.ToString().ToUpper() == SalesOrg.ToString().ToUpper())
                                .FirstOrDefault();   
                
                string subjectString = $"{templateInfo.TemplateSubject} - {Createdby}";
                string BodyString = templateInfo.TemplateBody;

                string defaultEmailToStr = string.Empty;
                string defaultCcToStr = string.Empty;
                string defaultBccToStr = string.Empty;
                if (!string.IsNullOrWhiteSpace(templateInfo.DefaultSentTo))
                {
                    defaultEmailToStr = templateInfo.DefaultSentTo;
                }
                if (!string.IsNullOrWhiteSpace(templateInfo.DefaultCcTo))
                {
                    defaultCcToStr = templateInfo.DefaultCcTo;
                }
                if (!string.IsNullOrWhiteSpace(templateInfo.DefaultBccTo))
                {
                    defaultBccToStr = templateInfo.DefaultBccTo;
                }

                NotificationInfo objNotificationInfo = new NotificationInfo();
                var _attachments = new Hashtable();

                objNotificationInfo.NotificationTemplateID = templateInfo.NotificationTemplateID;
                objNotificationInfo.PriceFileHeaderID = 0;
                objNotificationInfo.NotificationDate = DateTime.Now;
                objNotificationInfo.Subject = subjectString;
                objNotificationInfo.Body = "<html><body>" + templateInfo.TemplateBody + "</body></html>";
                objNotificationInfo.Attachments = "";
                objNotificationInfo.SentTo = defaultEmailToStr.Replace(",", ";");
                objNotificationInfo.CcTo = defaultCcToStr.Replace(",", ";");
                objNotificationInfo.BccTo = defaultBccToStr.Replace(",", ";");
                objNotificationInfo.Priority = templateInfo.Priority == 1 ? NotificationPriority.Low : templateInfo.Priority == 2 ? NotificationPriority.High : NotificationPriority.Low;
                objNotificationInfo.StatusDate = DateTime.Now;


                //using (ISqlHelper objSqlHelper = new SqlHelper(AppConfig.ConnectionString))
                //{
                //    string strUpdateQuery = "INSERT INTO dbo.TRN_NotificationHistory(NotificationDate,Subject,Body,SentTo,CcTo,BccTo,Priority,AttachmentPath,ActualFileName,EncryptedFileName,NotificationTemplateID,PriceFileHeaderID,PriceFileLocationID,IsActive,CreatedBy) " +
                //        "values(@NotificationDate,@Subject,@Body,@SentTo,@CcTo,@BccTo,@Priority,@AttachmentPath,@ActualFileName,@EncryptedFileName,@NotificationTemplateID,@PriceFileHeaderID,@PriceFileLocationID,@IsActive,@CreatedBy); SELECT SCOPE_IDENTITY(); ";
                    
                //    List<SqlParameter> lstSqlParameters = new List<SqlParameter>();
                //    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@NotificationDate", Value = DateTime.UtcNow });
                //    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@Subject", Value = objNotificationInfo.Subject });
                //    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@Body", Value = objNotificationInfo.Body });
                //    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@SentTo", Value = objNotificationInfo.SentTo });
                //    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@CcTo", Value = objNotificationInfo.CcTo });
                //    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@BccTo", Value = objNotificationInfo.BccTo });
                //    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@Priority", Value = (int)objNotificationInfo.Priority });
                //    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@AttachmentPath", Value = AppConfig.PFCDownloadedFileLoaction });
                //    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@ActualFileName", Value = "" });
                //    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@EncryptedFileName", Value = "" });
                //    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@NotificationTemplateID", Value = templateInfo.NotificationTemplateID });
                //    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@PriceFileHeaderID", Value = 0 });
                //    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@PriceFileLocationID", Value = "" });
                //    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@IsActive", Value = true });
                //    lstSqlParameters.Add(new SqlParameter() { ParameterName = "@CreatedBy", Value = Createdby });

                //    long output = Convert.ToInt32(objSqlHelper.ExecuteScalarQuery(CommandType.Text, strUpdateQuery, lstSqlParameters.ToArray()));
                //    objNotificationInfo.NotificationID = output;
                //    NotificationHistoryIds.Add(output);
                //}
                var a = await _objMailSenderProvider.SendMailAsync(objNotificationInfo, _attachments);

                return NotificationHistoryIds;
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("SendNotificationForMissingCustomerSettings", ex);
                return NotificationHistoryIds;
            }
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
            if (_objPriceListRepository != null && isDispose)
            {
                _objPriceListRepository.Dispose();
            }
            if (_objCommonProvider != null && isDispose)
            {
                _objCommonProvider.Dispose();
            }
            if (_objConfigureRepository != null && isDispose)
            {
                _objConfigureRepository.Dispose();
            }
            if (_objConfigureProvider != null && isDispose)
            {
                _objConfigureProvider.Dispose();
            }
            if (_objCommonRepository != null && isDispose)
            {
                _objCommonRepository.Dispose();
            }
            if (_objMailSenderProvider != null && isDispose)
            {
                _objMailSenderProvider.Dispose();
            }
        }

        #endregion Dispose
    }
}