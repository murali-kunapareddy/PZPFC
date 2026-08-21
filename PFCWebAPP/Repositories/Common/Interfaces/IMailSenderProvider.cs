using PFCWebAPP.Repositories.Common.Models;
using PFCWebAPP.Repositories.Common.ServiceProviders;
using System.Collections;

namespace PFCWebAPP.Repositories.Common.Interfaces
{
    public interface IMailSenderProvider : IDisposable
    {
        Task<string> SendMailAsync(NotificationInfo data, Hashtable attachments);
    }
}
