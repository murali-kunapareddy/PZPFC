using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFCWebAPP.DatabaseContext.Models.CustomTables
{
    [Table("TRN_NotificationHistory")]
    public class NotificationHistory
    {
        [Key]
        public long NotificationHistoryID { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime NotificationDate { get; set; }

        [Column(TypeName = "varchar(1024)")]
        public string Subject { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string Body { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string SentTo { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string CcTo { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string BccTo { get; set; }
        public int Priority { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Status { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime StatusDate { get; set; }

        [Column(TypeName = "nvarchar(1024)")]
        public string AttachmentPath { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        public string ActualFileName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string EncryptedFileName { get; set; }
        public int NotificationTemplateID { get; set; }
        public long PriceFileHeaderID { get; set; }
        public long PriceFileLocationID { get; set; }
        public long ResendCount { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string ResendStatus { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; } = false;

        [Column(TypeName = "varchar(100)")]
        public string CreatedBy { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string ModifiedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime ModifiedDate { get; set; }
    }
}
