using Microsoft.Data.SqlClient;
using PFCWebAPP.DatabaseContext.Models.CustomTables;
using PFCWebAPP.Repositories.Common.Enums;
using PFCWebAPP.Repositories.Common.Interfaces;
using PFCWebAPP.Repositories.Common.Models;
using PFCWebAPP.Utilities;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace PFCWebAPP.Repositories.Common.ServiceProviders
{
    public class MailSenderProvider : IMailSenderProvider
    {
        private readonly ILoggingProvider _objLoggingProvider;
        private readonly IHttpContextAccessor _objHttpContextAccessor;
        private readonly ICommonRepository _objCommonRepository;
        public readonly ICommonProvider _objCommonProvider;

        public MailAddress Sender { get; set; }
        public MailAddressCollection Recipient { get; set; }
        public MailAddressCollection CarbonCopy { get; set; }
        public MailAddressCollection BlindCarbonCopy { get; set; }
        public string MailSubject { get; set; }
        public string MailBody { get; set; }
        public Hashtable MailAttachments { get; set; }
        public MailPriority Priority { get; set; }

        public long notificationId { get; set; }
        public int resendCount { get; set; }

        public string UserSESA { get; set; }

        public bool IsResend { get; set; }






        public MailSenderProvider(ILoggingProvider objLoggingProvider, IHttpContextAccessor objHttpContextAccessor, ICommonRepository objCommonRepository, ICommonProvider objCommonProvider)
        {
            _objLoggingProvider = objLoggingProvider;
            _objHttpContextAccessor = objHttpContextAccessor;
            _objCommonRepository = objCommonRepository;
            _objCommonProvider = objCommonProvider;
            UserSESA = _objCommonProvider.GetLoginUserSESA();

        }



        public async Task<string> SendMailAsync(NotificationInfo data, Hashtable attachments)
        {
            try
            {
                notificationId = data.NotificationID;
                resendCount = data.ResendCount;
                var log = new StringBuilder();
                IsResend = data.IsResend;
                var status = String.Empty;

                Sender = new MailAddress(AppConfig.DefaultSenderEmail, AppConfig.DefaultSenderName);
                
                Recipient = new MailAddressCollection();
                foreach (string email in data.SentTo.Split(';'))
                {
                    if (email != "")
                    {
                        Recipient.Add(email);
                    }
                }

                CarbonCopy = new MailAddressCollection();
                if (!string.IsNullOrEmpty(data.CcTo))
                {
                    foreach (string email in data.CcTo.Split(';'))
                    {
                        if (email != "")
                        {
                            CarbonCopy.Add(email);
                        }
                    }
                }

                BlindCarbonCopy = new MailAddressCollection();
                if (!string.IsNullOrEmpty(data.BccTo))
                {
                    foreach (string email in data.BccTo.Split(';'))
                    {
                        if (email != "")
                        {
                            BlindCarbonCopy.Add(email);
                        }
                    }
                }


                MailSubject = data.Subject;
                MailBody = data.Body;
                Priority = (MailPriority)data.Priority;
                // attachments
                MailAttachments = attachments;

                await SendMails();

                //
                //
                return log.ToString();
            }
            catch(Exception ex)
            {
                _objLoggingProvider.LogException("SendMailAsync", ex);
                throw;
            }
        }



        private async Task<string> SendMails()
        {
            try
            {
                _objLoggingProvider.LogMessage(LogType.Info, "Collecting Mail settings");
                // build the message
                var msg = new MailMessage();
                // keep the required
                msg.IsBodyHtml = true;
                msg.BodyEncoding = Encoding.UTF8;
                msg.From = Sender;

                foreach (MailAddress MA in Recipient)
                {
                    msg.To.Add(MA);
                    _objLoggingProvider.LogMessage(LogType.Info, "Recipient {0} with email {1} added", MA.DisplayName, MA.Address);
                }
                _objLoggingProvider.LogMessage(LogType.Info, "Adding \'To\' completed");
                if (CarbonCopy != null)
                {
                    foreach (MailAddress MA in CarbonCopy)
                    {
                        msg.CC.Add(MA);
                        _objLoggingProvider.LogMessage(LogType.Info, "Recipient {0} with email {1} added", MA.DisplayName, MA.Address);
                    }
                }
                _objLoggingProvider.LogMessage(LogType.Info, "Adding \'Cc\' completed");

                if (BlindCarbonCopy != null )
                {
                    foreach (MailAddress MA in BlindCarbonCopy)
                    {
                        msg.Bcc.Add(MA);
                        _objLoggingProvider.LogMessage(LogType.Info, "Recipient {0} with email {1} added", MA.DisplayName, MA.Address);
                    }

                }
                _objLoggingProvider.LogMessage(LogType.Info, "Adding \'BCc\' completed");
                if (AppConfig.SupportBcc.ToUpper() == "True".ToUpper())
                {
                    foreach (var MA in AppConfig.SupportBccMails.Split(';'))
                    {
                        if (MA.Trim() != "")
                        {
                            msg.Bcc.Add(MA);
                            _objLoggingProvider.LogMessage(LogType.Info, "Recipient with BccSupport email {0} added", MA);
                        }
                       
                    }
                }
                _objLoggingProvider.LogMessage(LogType.Info, "Adding \'BCcSupport \' completed");

                //msg.Bcc.Add(new MailAddress("shiva.komaparathi@non.se.com"));
                //msg.Bcc.Add(new MailAddress("nareshkumar.challa@non.se.com"));
                //msg.Bcc.Add(new MailAddress("Murali.Kunapareddy@se.com"));


                // add attachments if any
                // if it is from remote location, give full URL
                if (MailAttachments.Count > 0)
                {
                    foreach (DictionaryEntry pair in MailAttachments)
                    {
                        try
                        {
                            var filName = pair.Key.ToString();
                            var isFullPath = false;
                            if (filName.Contains("\\"))
                            {
                                var fi = new FileInfo(filName);
                                isFullPath = true;
                                filName = fi.Name;
                            }
                            var ext = filName.Substring(filName.Length - 3);
                            var mime = GetMime(ext);
                            var encFile = pair.Value.ToString();
                            byte[] data = GetData(encFile, isFullPath);
                            MemoryStream ms = new MemoryStream(data);
                            msg.Attachments.Add(new Attachment(ms, filName, mime));
                            _objLoggingProvider.LogMessage(LogType.Info, "File {0} of type {1} attached", filName, mime);
                        }
                        catch (Exception ex)
                        {
                            _objLoggingProvider.LogException("Error while attaching via memory stream", ex);
                        }
                    }
                }
                _objLoggingProvider.LogMessage(LogType.Info, "{0} Attachments added", MailAttachments.Count);
                msg.Subject = MailSubject;
                msg.Body = MailBody;
                msg.Priority = Priority;
                //
                if (AppConfig.NotificationMode == "LOCAL")
                {
                    // instanciate smtp client
                    using (var client = new SmtpClient(AppConfig.SMTPServer))
                    {
                        client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                        //client.PickupDirectoryLocation = AppConfig.MailPickUpLocation;

                        string fpath = Path.Combine(Directory.GetCurrentDirectory(), PFCWebAPP.Utilities.AppConfig.MailPickUpLocation);
                        if (!Directory.Exists(fpath))
                            Directory.CreateDirectory(fpath);
                        client.PickupDirectoryLocation = fpath;
                        client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
                        await client.SendMailAsync(msg);
                    }
                }
                else if (AppConfig.NotificationMode == "PROD")
                {
                    // reading SMTP details from web.config
                    //var SmtpServer = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                    //var SmtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"].ToString());
                    //var SmtpEMailFrom = ConfigurationManager.AppSettings["SmtpEMailFrom"].ToString();
                    //var SmtpPassword = ConfigurationManager.AppSettings["SmtpPassword"].ToString();
                    // instanciate smtp client
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    using (var client = new SmtpClient())
                    {
                        client.Host = AppConfig.SMTPServer;
                        client.Port = AppConfig.SmtpPort;
                        // specify smtp options
                        client.EnableSsl = true;
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.DeliveryFormat = SmtpDeliveryFormat.International;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential(AppConfig.SmtpUsername, AppConfig.SmtpPassword);
                        client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
                        await client.SendMailAsync(msg);
                    }
                }
                // _objLoggingProvider.LogMessage(LogType.Info,"Successfully Sent");
                //Send("--SUCCESS--");
                //return "SUCCESS";

            }
            catch (Exception ex)
            {
               // var x = ex.Message;
                //  TODO: save error message
                _objLoggingProvider.LogException("Error while sending mail", ex);
               
                // send error mail to 
                //Send(ex.Message);
                //return "FAIL";
            }
            return "";

        }
        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {

            try
            {

                var log = new StringBuilder();
                var status = String.Empty;
                if (e.Cancelled)
                {
                    status = "FAIL";

                }
                else if (e.Error != null)
                {
                    status = "FAIL";
                }
                else
                {
                    status = "SUCCESS";

                }

                if (IsResend == true)
                {
                    int updatedResendCount;
                    if (status == "SUCCESS")
                    {
                        updatedResendCount = resendCount + 1;
                    }
                    else
                    {
                        updatedResendCount = resendCount;
                    }

                    using (ISqlHelper objSqlHelper = new SqlHelper(AppConfig.ConnectionString))
                    {
                        string strUpdateQuery = "Update dbo.TRN_NotificationHistory SET ResendStatus = @ResendStatus, ResendCount = @ResendCount, ModifiedBy = @ModifiedBy, ModifiedDate = @ModifiedDate WHERE NotificationHistoryID = @NotificationHistoryID";

                        List<SqlParameter> lstParameters = new List<SqlParameter>();
                        lstParameters.Add(new SqlParameter() { ParameterName = "@ResendStatus", Value = status });
                        lstParameters.Add(new SqlParameter() { ParameterName = "@ResendCount", Value = updatedResendCount });
                        lstParameters.Add(new SqlParameter() { ParameterName = "@ModifiedBy", Value = UserSESA });
                        lstParameters.Add(new SqlParameter() { ParameterName = "@ModifiedDate", Value = DateTime.UtcNow });
                        lstParameters.Add(new SqlParameter() { ParameterName = "@NotificationHistoryID", Value = notificationId });

                        int output = objSqlHelper.ExecuteNonQuery(CommandType.Text, strUpdateQuery, lstParameters.ToArray());


                    }
                }
                else if (notificationId != 0)
                {
                    using (ISqlHelper objSqlHelper = new SqlHelper(AppConfig.ConnectionString))
                    {
                        string strUpdateQuery = "UPDATE dbo.TRN_NotificationHistory SET Status = @Status, StatusDate = @StatusDate, ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate WHERE NotificationHistoryID = @NotificationHistoryID";
                        string UserSESA = _objCommonProvider.GetLoginUserSESA();
                        List<SqlParameter> lstSqlParameters = new List<SqlParameter>();
                        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@Status", Value = status });
                        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@StatusDate", Value = DateTime.UtcNow });
                        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@ModifiedBy", Value = UserSESA });
                        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@ModifiedDate", Value = DateTime.UtcNow });
                        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@NotificationHistoryID", Value = notificationId });
                        objSqlHelper.ExecuteNonQuery(CommandType.Text, strUpdateQuery, lstSqlParameters.ToArray());
                    }
                }
            }
            catch(Exception ex)
            {
                _objLoggingProvider.LogException("SendCompletedCallback", ex);

            }

        }


        private string GetMime(string extension)
        {
            switch (extension.ToLower())
            {
                case "txt":
                    return "text/plain";
                case "jpg":
                    return "image/jpeg";
                case "peg": // jpeg     // TODO: handle text after last (.)
                    return "image/jpeg";
                case "gif":
                    return "image/gif";
                case "png":
                    return "image/png";
                case "pdf":
                    return "application/pdf";
                case "xls":
                    return "application/vnd.ms-excel";
                case "lsx": // xlsx     // TODO: handle text after last (.)
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case "doc":
                    return "application/msword";
                case "ocx": // docx     // TODO: handle text after last (.)
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case "ppt":
                    return "application/vnd.ms-powerpoint";
                case "ptx": // pptx     // TODO: handle text after last (.)
                    return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                default:
                    return "application/octet-stream";
            }
        }

        private byte[] GetData(string filePath, bool isFullPath = false)
        {
            // TODO: temp code - just retrieve the encfile
            //var fileparts = fileFullPath.Split('/');
            //return new APACWP.Common.WorkflowCommon().DownloadAttachment(fileparts[fileparts.Length-1]);
            var fileFullPath = string.Empty;
            if (!isFullPath)
            {
                var baseFolder = AppConfig.PFCDownloadedFileLoaction;
                if (filePath.Substring(0, 1) == "/")
                {
                    filePath = filePath.Substring(1);
                }
                fileFullPath = Path.Combine(baseFolder, filePath);
            }
            else
            {
                fileFullPath = filePath;
            }
            //
            if (File.Exists(fileFullPath))
            {
                return File.ReadAllBytes(fileFullPath);
            }
            return null;
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
            if (_objCommonProvider != null && isDispose)
            {
                _objCommonProvider.Dispose();
            }
        }

        #endregion
    }




}
