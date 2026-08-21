using PFCRepository.Repositories.Common.Models;
using PFCRepository.Repositories.Common.ServiceProviders;
using System.Collections;

namespace PFCRepository.Repositories.Common.Interfaces
{
    public interface IMailSenderProvider : IDisposable
    {
        Task<string> SendMailAsync(NotificationInfo data, Hashtable attachments);
    }
}
