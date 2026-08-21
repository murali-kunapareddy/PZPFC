using PFCRepository.DatabaseContext.Models.CustomTables;
using PFCRepository.Repositories.Common.Enums;
using PFCRepository.Repositories.Common.Models;
using System.Data;

namespace PFCRepository.Repositories.Common.Interfaces
{
    public interface ICommonProvider : IDisposable
    {
        string GetLoginUserSESA();
        //List<PFCSummaryInfo> GetPFCSummaryInfoByUserSESA();
        List<PriceFileLocationDetails> PriceFileLocationInfoByHeaderID(long PriceFileHeaderID);
        dynamic GetAppSettingByName(string ConfigName);
        string GetNotificationTemplateNameByOrg(string OrgName);
        bool IsValidUser(string UserSESA);
        bool IsAdminUser(string UserSESA);
        NotificationTemplates GetPriceFileAPINotificationTemplate(string TemplateName);
    }
}
