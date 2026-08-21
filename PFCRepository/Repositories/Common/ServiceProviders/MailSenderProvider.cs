using Microsoft.Data.SqlClient;
using PFCRepository.DatabaseContext.Models.CustomTables;
using PFCRepository.Repositories.Common.Enums;
using PFCRepository.Repositories.Common.Interfaces;
using PFCRepository.Repositories.Common.Models;
using PFCRepository.Utilities;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Net.Mime;
using NPOI.POIFS.Crypt.Dsig;
using System.Reflection;
using NPOI.XWPF.UserModel;
using SE.CA.EmailHelper;

namespace PFCRepository.Repositories.Common.ServiceProviders
{
    public class MailSenderProvider : IMailSenderProvider
    {
        private readonly ILoggingProvider _objLoggingProvider;
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






        public MailSenderProvider(ILoggingProvider objLoggingProvider,  ICommonRepository objCommonRepository, ICommonProvider objCommonProvider)
        {
            _objLoggingProvider = objLoggingProvider;
            
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

                if (!string.IsNullOrEmpty(data.CcTo))
                {
                    CarbonCopy = new MailAddressCollection();
                    foreach (string email in data.CcTo.Split(';'))
                    {
                        if (email != "")
                        {
                            CarbonCopy.Add(email);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(data.BccTo))
                {
                    BlindCarbonCopy = new MailAddressCollection();
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
                    _objLoggingProvider.LogMessage(LogType.Info, string.Format("Recipient {0} with email {1} added", MA.DisplayName, MA.Address));
                }
                _objLoggingProvider.LogMessage(LogType.Info, "Adding \'To\' completed");
                if (CarbonCopy != null)
                {
                    foreach (MailAddress MA in CarbonCopy)
                    {
                        msg.CC.Add(MA);
                        _objLoggingProvider.LogMessage(LogType.Info, string.Format( "Recipient {0} with email {1} added", MA.DisplayName, MA.Address));
                    }
                }
                _objLoggingProvider.LogMessage(LogType.Info, "Adding \'Cc\' completed");

                if (BlindCarbonCopy != null )
                {
                    foreach (MailAddress MA in BlindCarbonCopy)
                    {
                        msg.Bcc.Add(MA);
                        _objLoggingProvider.LogMessage(LogType.Info, string.Format("Recipient {0} with email {1} added", MA.DisplayName, MA.Address));
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
                            _objLoggingProvider.LogMessage(LogType.Info, string.Format( "Recipient with BccSupport email {0} added", MA));
                        }
                       
                    }
                }
                _objLoggingProvider.LogMessage(LogType.Info, "Adding \'BCcSupport \' completed");

                //msg.Bcc.Add(new MailAddress("shiva.komaparathi@non.se.com"));
                //msg.Bcc.Add(new MailAddress("nareshkumar.challa@non.se.com"));
                //msg.Bcc.Add(new MailAddress("Murali.Kunapareddy@se.com"));


                // add attachments if any
                // if it is from remote location, give full URL
                List<string> lstAttachment = new List<string>();

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
                            _objLoggingProvider.LogMessage(LogType.Info, string.Format("File {0} of type {1} attached", filName, mime));

                            string directory = Path.GetDirectoryName(encFile);
                            string newFilePath = Path.Combine(directory, filName);

                            // Check if the file already exists and append a sequence number if it does
                            int sequence = 1;
                            string fileWithoutExtension = Path.GetFileNameWithoutExtension(filName);
                            string extension = Path.GetExtension(filName);
                            while (File.Exists(newFilePath))
                            {
                                string tempFileName = $"{fileWithoutExtension}_{sequence}{extension}";
                                newFilePath = Path.Combine(directory, tempFileName);
                                sequence++;
                            }

                            // Copy the encrypted file and give it the new filename
                            File.Copy(encFile, newFilePath, overwrite: false);

                            // Update your List<string> with the new file path
                            // Assuming 'filePaths' is your List<string>
                            
                            lstAttachment.Add(newFilePath);
                        }
                        catch (Exception ex)
                        {
                            _objLoggingProvider.LogException("Error while attaching via memory stream", ex);
                        }
                    }
                }
                _objLoggingProvider.LogMessage(LogType.Info, string.Format("{0} Attachments added", MailAttachments.Count));

                msg.Subject = MailSubject;
                msg.Body = MailBody;
                msg.Priority = Priority;


                // Create an AlternateView for HTML content
                //AlternateView htmlView = AlternateView.CreateAlternateViewFromString(msg.Body, null, MediaTypeNames.Text.Html);

                // Add the image as a LinkedResource
                //var outPutDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
                //var logoImagePath = Path.Combine(outPutDirectory, @"Images\SignatureImage.jpg");
                //string relLogoPath = new Uri(logoImagePath).LocalPath;
                //LinkedResource signatureImage = new LinkedResource("Images/SignatureImage.jpg", MediaTypeNames.Image.Jpeg);
                //signatureImage.ContentId = "signatureImage";
                //htmlView.LinkedResources.Add(signatureImage);

                //// Add the AlternateView to the MailMessage
                //msg.AlternateViews.Add(htmlView);


                if (AppConfig.NotificationMode == "LOCAL")
                {
                    // instanciate smtp client
                    using (var client = new SmtpClient(AppConfig.SMTPServer))
                    {
                        client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                        //client.PickupDirectoryLocation = AppConfig.MailPickUpLocation;

                        string fpath = Path.Combine(Directory.GetCurrentDirectory(), PFCRepository.Utilities.AppConfig.MailPickUpLocation);
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
                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    //using (var client = new SmtpClient())
                    //{
                    //    client.Host = AppConfig.SMTPServer;
                    //    client.Port = AppConfig.SmtpPort;
                    //    // specify smtp options
                    //    client.EnableSsl = true;
                    //    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //    client.DeliveryFormat = SmtpDeliveryFormat.International;
                    //    client.UseDefaultCredentials = false;
                    //    client.Credentials = new NetworkCredential(AppConfig.SmtpUsername, AppConfig.SmtpPassword);
                    //    client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
                    //    await client.SendMailAsync(msg);
                    //}
                    var semailer = new SE.CA.EmailService();
                    semailer.StatusUpdated += SEMailer_StatusUpdated;
                    SE.CA.EmailHelper.AppSettings.SmtpUser = AppConfig.SmtpUsername;
                    SE.CA.EmailHelper.AppSettings.SmtpPass = AppConfig.SmtpPassword;
                    SE.CA.EmailHelper.AppSettings.HtmlSupport = true;
                    var from = new EmailAddress() { Email = msg.From.Address, Name = msg.From.DisplayName };

                    List<EmailAddress> toList = msg.To?.Select(address => new EmailAddress { Email = address.Address, Name = address.DisplayName }).ToList() ?? new List<EmailAddress>();
                    List<EmailAddress> ccList = msg.CC?.Select(address => new EmailAddress { Email = address.Address, Name = address.DisplayName }).ToList() ?? new List<EmailAddress>();
                    List<EmailAddress> bccList = msg.Bcc?.Select(address => new EmailAddress { Email = address.Address, Name = address.DisplayName }).ToList() ?? new List<EmailAddress>();

                    int bodyHash;
                    var result = semailer.Send(toList, msg.Subject, msg.Body, out bodyHash, lstAttachment, null, from, ccList, bccList);
                    SEMailer_StatusUpdated(result);
                    _objLoggingProvider.LogMessage(LogType.Info, $"SendMail Status : {result}");
                    if (result != null)
                    {
                        foreach (string filePath in lstAttachment)
                        {
                            try
                            {
                                if (File.Exists(filePath))
                                {
                                    File.Delete(filePath);
                                    _objLoggingProvider.LogMessage(LogType.Info, $"Renamed File : {filePath} Deleted Successfully");
                                }
                            }
                            catch (IOException ex)
                            {
                                // Handle the exception (e.g., log the error or notify the user)                            
                                _objLoggingProvider.LogException($"An error occurred while deleting the file: {ex.Message}", ex);
                            }
                        }
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

        private void SEMailer_StatusUpdated(object sender,string status)
        {
            try
            {
                if (IsResend)
                {
                    _objLoggingProvider.LogMessage(LogType.Info, $"TRN_NotificationHistory Id: {notificationId} Resend Status Updated : {status}");
                }
                else if (notificationId != 0)
                {
                    _objLoggingProvider.LogMessage(LogType.Info, $"TRN_NotificationHistory Id: {notificationId} Status Updated : {status}");
                }
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("SEMailer_StatusUpdated_CallbackEvent", ex);
            }
        }

        private void SEMailer_StatusUpdated(string status)
        {
            try
            {   
                if (IsResend)
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
                        UserSESA = _objCommonProvider.GetLoginUserSESA();
                        List<SqlParameter> lstParameters = new List<SqlParameter>();
                        lstParameters.Add(new SqlParameter() { ParameterName = "@ResendStatus", Value = status });
                        lstParameters.Add(new SqlParameter() { ParameterName = "@ResendCount", Value = updatedResendCount });
                        lstParameters.Add(new SqlParameter() { ParameterName = "@ModifiedBy", Value = UserSESA });
                        lstParameters.Add(new SqlParameter() { ParameterName = "@ModifiedDate", Value = DateTime.UtcNow });
                        lstParameters.Add(new SqlParameter() { ParameterName = "@NotificationHistoryID", Value = notificationId });
                        objSqlHelper.ExecuteNonQuery(CommandType.Text, strUpdateQuery, lstParameters.ToArray());
                        _objLoggingProvider.LogMessage(LogType.Info, $"TRN_NotificationHistory Id: {notificationId} Resend Status Updated : {status}");
                    }
                }
                else if (notificationId != 0)
                {
                    using (ISqlHelper objSqlHelper = new SqlHelper(AppConfig.ConnectionString))
                    {
                        string strUpdateQuery = "UPDATE dbo.TRN_NotificationHistory SET Status = @Status, StatusDate = @StatusDate, ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate WHERE NotificationHistoryID = @NotificationHistoryID";
                        UserSESA = _objCommonProvider.GetLoginUserSESA();
                        List<SqlParameter> lstSqlParameters = new List<SqlParameter>();
                        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@Status", Value = status });
                        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@StatusDate", Value = DateTime.UtcNow });
                        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@ModifiedBy", Value = UserSESA });
                        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@ModifiedDate", Value = DateTime.UtcNow });
                        lstSqlParameters.Add(new SqlParameter() { ParameterName = "@NotificationHistoryID", Value = notificationId });
                        objSqlHelper.ExecuteNonQuery(CommandType.Text, strUpdateQuery, lstSqlParameters.ToArray());
                        _objLoggingProvider.LogMessage(LogType.Info, $"TRN_NotificationHistory Id: {notificationId} Status Updated : {status}");
                    }
                }
            }
            catch (Exception ex)
            {
                _objLoggingProvider.LogException("SEMailer_StatusUpdated", ex);
            }            
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
