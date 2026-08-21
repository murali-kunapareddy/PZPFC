using PFCWebAPP.DatabaseContext.Models.CustomTables;
using PFCWebAPP.Repositories.Common.Enums;
using PFCWebAPP.Repositories.Common.Models;
using SE.CA.PingComponent.Entities;
using System.Data;

namespace PFCWebAPP.Repositories.Common.Interfaces
{
    public interface ICommonProvider : IDisposable
    {
        string GetLoginUserSESA();
        UserInfo GetLoginUserDetails();
        List<PFCSummaryInfo> GetPFCSummaryInfoByUserSESA();
        List<PriceFileLocationDetails> PriceFileLocationInfoByHeaderID(long PriceFileHeaderID);
        dynamic GetAppSettingByName(string ConfigName);
        string GetNotificationTemplateNameByOrg(string OrgName);
        bool IsValidUser(string UserSESA);
        bool IsAdminUser(string UserSESA);

        string GetCustomerValidationNotificationTemplate(string OrgName);
    }

}
