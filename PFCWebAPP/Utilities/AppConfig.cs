
namespace PFCWebAPP.Utilities
{
    public class AppConfig
    {


        public static string CanLogWithNLogger
        {
            get { return AppSettings.AppSetting["CanLogWithNLogger"].ToString().Trim(); }
        }
        public static string ApplicationName
        {
            get { return AppSettings.AppSetting["ApplicationName"].ToString().Trim(); }
        }
        public static string ApplicationShortName
        {
            get { return AppSettings.AppSetting["ApplicationShortName"].ToString().Trim(); }
        }
        public static string BildVersion
        {
            get { return AppSettings.AppSetting["BildVersion"].ToString().Trim(); }
        }
        public static string SessionExpireUrl
        {
            get { return AppSettings.AppSetting["SessionExpireUrl"].ToString().Trim(); }
        }

        public static string TemplateFilepath
        {
            get { return AppSettings.AppSetting["TemplateFilepath"].ToString().Trim(); }
        }

        public static string PFCDownloadedFileLoaction
        {
            get { return AppSettings.AppSetting["PFCDownloadedFileLoaction"].ToString().Trim(); }
        }
        public static string PFCDownloadedFileExtraction
        {
            get { return AppSettings.AppSetting["PFCDownloadedFileLoaction"].ToString().Trim(); }
        }
        public static string WhitelistingEnvironment
        {
            get { return AppSettings.AppSetting["WhitelistingEnvironment"].ToString().Trim(); }
        }
        public static string WhitelistIPAddresses
        {
            get { return AppSettings.AppSetting["WhitelistIPAddresses"].ToString().Trim(); }
        }
        public static string ConnectionString
        {
            get { return ConnectionStringSettings.ConnectionSetting["ConnectionString"].ToString().Trim(); }
        }



        public static string TransactionScopeTimeoutInSec
        {
            get { return AppSettings.AppSetting["TransactionScopeTimeoutInSec"].ToString().Trim(); }
        }

        public static string ApplicationMode
        {
            get { return AppSettings.AppSetting["ApplicationMode"].ToString().Trim(); }
        }
        public static string ShowBanner
        {
            get { return AppSettings.AppSetting["ShowBanner"].ToString().Trim(); }
        }


        public static string NotificationMode
        {
            get { return NotificationSettings.NotificationSetting["NotificationMode"].ToString().Trim(); }
        }

        public static string DefaultSenderName
        {
            get { return NotificationSettings.NotificationSetting["DefaultSenderName"].ToString().Trim(); }
        }

        public static string DefaultSenderEmail
        {
            get { return NotificationSettings.NotificationSetting["DefaultSenderEmail"].ToString().Trim(); }
        }

        public static string DefaultRecipientName
        {
            get { return NotificationSettings.NotificationSetting["DefaultRecipientName"].ToString().Trim(); }
        }

        public static string DefaultRecipientEmail
        {
            get { return NotificationSettings.NotificationSetting["DefaultRecipientEmail"].ToString().Trim(); }
        }

        public static string MailPickUpLocation
        {
            get { return NotificationSettings.NotificationSetting["MailPickUpLocation"].ToString().Trim(); }
        }

        public static string SMTPServer
        {
            get { return NotificationSettings.NotificationSetting["SmtpServer"].ToString().Trim(); }
        }

        public static int SmtpPort
        {
            get { return Convert.ToInt32(NotificationSettings.NotificationSetting["SmtpPort"].ToString().Trim()); }
        }

        public static string SmtpUsername
        {
            get { return NotificationSettings.NotificationSetting["SmtpUsername"].ToString().Trim(); }
        }

        public static string SmtpPassword
        {
            get { return NotificationSettings.NotificationSetting["SmtpPassword"].ToString().Trim(); }
        }

        public static string SmtpNetworkDeliveryMethodEnabled
        {
            get { return NotificationSettings.NotificationSetting["SmtpNetworkDeliveryMethodEnabled"].ToString().Trim(); }
        }

        public static string UserDetailsUpdatedSuccess
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["UserDetailsUpdatedSuccess"].ToString().Trim(); }
           // get { return "User Details Updated Successfully"; }
        }
        public static string UserDeletedSuccess
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["UserDeletedSuccess"].ToString().Trim(); }
           // get { return "User Deleted Successfully"; }
        }
        public static string ConfirmDeletePortalUser
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["ConfirmDeletePortalUser"].ToString().Trim(); }
           // get { return "Are you sure, you want to delete portal user ?"; }
        }
        public static string ReActivePortalAccess
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["ReActivePortalAccess"].ToString().Trim(); }
           // get { return "Would you like to re-activate portal access for "; }
        }
        public static string UserDeleteWithOutRole
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["UserDeleteWithOutRole"].ToString().Trim(); }
            //get { return "Updating User without any role, User will be Deleted. Are you Sure ?"; }
        }
        public static string SelectAtleastSingleCustomer
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["SelectAtleastSingleCustomer"].ToString().Trim(); }
            // get { return "Select Atleast Single Customer"; }
        }
        public static string SelectSalesOrganisation
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["SelectSalesOrganisation"].ToString().Trim(); }
            //get { return "Select any sales organisation"; }
        }
        public static string PopUpHeaderValidation
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["PopUpHeaderValidation"].ToString().Trim(); }
            //get { return "Validation"; }
        }
        public static string PopUpHeaderInformation
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["PopUpHeaderInformation"].ToString().Trim(); }
            //get { return "Information"; }
        }
        public static string CustomerSelectionCountFromSettings
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["CustomerSelectionCountFromSettings"].ToString().Trim(); }
            // get { return "Selection of Customers Exceeded should be less than or equal to "; }
        }
        public static string ExcludeNetTradePricesAndchkOverallNetPrices
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["ExcludeNetTradePricesAndchkOverallNetPrices"].ToString().Trim(); }
            //get { return "Excluding Trade Prices will result in empty price list(s) since Net Prices and Overall Net Prices are also excluded "; }
        }
        public static string ExcludechkTradePricesAndchkOverallNetPrices
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["ExcludechkTradePricesAndchkOverallNetPrices"].ToString().Trim(); }
            //get { return "Excluding Net Prices will result in empty price list(s) since Trade Prices and Overall Net Prices are also excluded "; }
        }
        public static string chkOverallNetPrices
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["chkOverallNetPrices"].ToString().Trim(); }
            // get { return "Excluding Overall Net Prices will result in empty price list(s) since Trade Prices and Net Prices are also excluded "; }
        }
        public static string SelectTradeTemplate
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["SelectTradeTemplate"].ToString().Trim(); }
            // get { return "Please Select TradeTemplate"; }
        }
        public static string SelectTradeListFormat
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["SelectTradeListFormat"].ToString().Trim(); }
            // get { return "Please Select Trade List Format"; }
        }
        public static string PartialDownloadOfExcel
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["PartialDownloadOfExcel"].ToString().Trim(); }
            // get { return "Excel Files For Customers are Generated partially, Please Re-Download from Archived Location"; }
        }
        public static string FullDownloadOfExcelSuccess
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["FullDownloadOfExcelSuccess"].ToString().Trim(); }
            // get { return "All Excel Files For Customers are Generated Successfully"; }
        }
        public static string DataSavedSuccessfully
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["DataSavedSuccessfully"].ToString().Trim(); }
            //get { return "Data Saved Successfully"; }
        }
        public static string FailedToUpload
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["FailedToUpload"].ToString().Trim(); }
            // get { return "Failed to Upload Data, Please try Again"; }
        }
        public static string CustomerTemplateSaved
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["CustomerTemplateSaved"].ToString().Trim(); }
            //get { return "Customer Template Saved Successfully"; }
        }
        public static string CustomerTemplateDeleteConformation
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["CustomerTemplateDeleteConformation"].ToString().Trim(); }
            // get { return "Are you sure, you want to Delete Customer Template ?"; }
        }
        public static string CustomerTemplateDelete
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["CustomerTemplateDelete"].ToString().Trim(); }
            //get { return "Customer Template Deleted Successfully"; }
        }
        public static string CustomerExistInOrganisation
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["CustomerExistInOrganisation"].ToString().Trim(); }
            //get { return "Customer Template Exists in Other Organisation"; }
        }
        public static string TemplateSaved
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["TemplateSaved"].ToString().Trim(); }
            //get { return "Template Saved Successfully"; }
        }
        public static string TemplateDelete
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["TemplateDelete"].ToString().Trim(); }
            // get { return "Customer Template Deleted Successfully"; }
        }
        public static string TemplateDeleteConformation
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["TemplateDeleteConformation"].ToString().Trim(); }
            // get { return "Are you sure, you want to Delete Template ?"; }
        }
        public static string TemplateExistInOrganisation
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["TemplateExistInOrganisation"].ToString().Trim(); }
            // get { return "Template Exists in Other Organisation"; }
        }
        public static string TemplateExistInOrganisationTryNewName
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["TemplateExistInOrganisationTryNewName"].ToString().Trim(); }
            //get { return "Template already exists in the other Organisation, Please Enter New Template Name."; }
        }
        public static string TemplateReActive1
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["TemplateReActive1"].ToString().Trim(); }
            // get { return "Would you like to re-activate Template - "; }
        }
        public static string TemplateReActive2
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["TemplateReActive2"].ToString().Trim(); }
            //get { return "existing in the Organisation -"; }
        }
        public static string NotificationReSentSuccess
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["NotificationReSentSuccess"].ToString().Trim(); }
            // get { return "Notification Re-Sent Successfully"; }
        }
        public static string NotificationReSentFailed
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["NotificationReSentFailed"].ToString().Trim(); }
            //get { return "Failed to Re-Send Notification"; }
        }
        public static string SessionExpire
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["SessionExpire"].ToString().Trim(); }
            //get { return "Session Expired...!  Sorry, you were gone for too long!  When you are ready and have time to focus, relaunch to continue"; }
        }
        public static string UnauthzorisedUser
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["UnauthzorisedUser"].ToString().Trim(); }
            //get { return "Unauthzorised User"; }
        }
        public static string UnauthzorisedUserRole
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["UnauthzorisedUserRole"].ToString().Trim(); }
            //get { return "Unauthzorised User Role"; }
        }

        public static string DirectoryName
        {
            get { return Directory.GetCurrentDirectory(); }
        }
        public static string EmailSentSuccess
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["EmailSentSuccess"].ToString().Trim(); }
            //get { return "Unauthzorised User Role"; }
        }
        public static string EmailSentFail
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["EmailSentFail"].ToString().Trim(); }
            //get { return "Unauthzorised User Role"; }
        }
        public static string TimeInterval
        {
            get { return AppSettings.AppSetting["TimeInterval"].ToString().Trim(); }
        }

        public static string ResendNotificationConfirmation
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["ResendNotificationConfirmation"].ToString().Trim(); }
        }
        public static string DifferentOrgCustomers
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["DifferentOrgCustomers"].ToString().Trim(); }
        }
        public static string DecimalPosition
        {
            get { return AppSettings.AppSetting["DecimalPosition"].ToString().Trim(); }
        }
        public static string CustomersDidNotMatchOrg
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["CustomersDidNotMatchOrg"].ToString().Trim(); }
        }
        public static string NoteForNonAMDUsers
        {
            get { return PopUpMessageSettings.PopUpMessageSetting["NoteForNonAMDUsers"].ToString().Trim(); }
            // get { return "Template Exists in Other Organisation"; }
        }
        public static string SupportBcc
        {
            get { return AppSettings.AppSetting["SupportBCC"].ToString().Trim(); }
        }
        public static string SupportBccMails
        {
            get { return AppSettings.AppSetting["SupportBCCEmails"].ToString().Trim(); }
        }
        public static string PricingFutureDate
        {
            get { return AppSettings.AppSetting["PricingFutureDate"].ToString().Trim(); }
        }
        public static int EtlStartHour
        {
            get { return Convert.ToInt32(EtlSettings.EtlSetting["EtlStartHour"].ToString().Trim()); }
        }
        public static int EtlCurrentHour
        {
            get { return Convert.ToInt32(EtlSettings.EtlSetting["EtlCurrentHour"].ToString().Trim()); }
        }
        public static string EtlInProgressMsg
        {
            get { return EtlSettings.EtlSetting["EtlInProgressMsg"].ToString().Trim(); }
        }
        public static string EtlYetToStartMsg
        {
            get { return EtlSettings.EtlSetting["EtlYetToStartMsg"].ToString().Trim(); }
        }
    }

    static class AuthSettings
    {
        public static IConfiguration AuthSetting { get; }
        static AuthSettings()
        {
            AuthSetting = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build().GetSection("PingAuthSettings");

        }
    }
    static class AppSettings
    {
        public static IConfiguration AppSetting { get; }
        static AppSettings()
        {
            AppSetting = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build().GetSection("AppSettings");

        }
    }
    static class ConnectionStringSettings
    {
        public static IConfiguration ConnectionSetting { get; }
        static ConnectionStringSettings()
        {
            ConnectionSetting = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build().GetSection("ConnectionStrings");

        }
    }
    static class NotificationSettings
    {
        public static IConfiguration NotificationSetting { get; }
        static NotificationSettings()
        {
            NotificationSetting = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build().GetSection("NotificationSettings");

        }
    }

    static class PopUpMessageSettings
    {
        public static IConfiguration PopUpMessageSetting { get; }
        static PopUpMessageSettings()
        {
            PopUpMessageSetting = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build().GetSection("PopUpMessageSettings");

        }
    }

    static class EtlSettings
    {
        public static IConfiguration EtlSetting { get; }
        static EtlSettings()
        {
            EtlSetting = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build().GetSection("EtlSettings");

        }
    }



}
