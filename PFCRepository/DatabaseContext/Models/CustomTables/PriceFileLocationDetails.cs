using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFCRepository.DatabaseContext.Models.CustomTables
{
    [Table("TRN_PriceFileLocationDetails")]
    public class PriceFileLocationDetails
    {
        [Key]
        public long PriceFileLocationID { get; set; }
        public long PriceFileHeaderID { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string CustomerNo { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        public string PFCActualFileName { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string PFCEncryptedFileName { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string PFCFileLocationMode { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string PFCFileType { get; set; } 

        [Column(TypeName = "varchar(250)")]
        public string PFCFilePath { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string PFCFileSize { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Status { get; set; }

        [Column(TypeName = "varchar(512)")]
        public string StatusText { get; set; }

        public double PercentCompleted { get; set; }

        public bool IsCompleted { get; set; }

        public int ReDownloadCount { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string ReDownloadStatus { get; set; }

        [Column(TypeName = "varchar(512)")]
        public string ReDownloadStatusText { get; set; }
        public double ReDownloadPercentCompleted { get; set; }
        public bool IsReDownloadCompleted { get; set; }
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

        public virtual PriceFileHeader PriceFileHeader { get; set; }
    }
}
