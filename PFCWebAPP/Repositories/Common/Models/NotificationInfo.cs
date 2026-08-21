
using PFCWebAPP.Repositories.Common.Enums;

namespace PFCWebAPP.Repositories.Common.Models
{
    public class NotificationInfo
    {
        public long NotificationID { get; set; }
        public int NotificationTemplateID { get; set; }
        public long PriceFileHeaderID { get; set; }
        public DateTime NotificationDate { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Attachments { get; set; }
        public string SentTo { get; set; }
        public string CcTo { get; set; }
        public string BccTo { get; set; }
        public NotificationStatus Status { get; set; }
        public NotificationPriority Priority { get; set; } = 0;
        public DateTime StatusDate { get; set; }
        public int ResendCount { get; set; }

        public bool IsResend { get; set; }

    }
}
