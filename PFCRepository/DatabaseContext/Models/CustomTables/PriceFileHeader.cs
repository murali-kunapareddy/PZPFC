using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFCRepository.DatabaseContext.Models.CustomTables
{
    [Table("TRN_PriceFileHeader")]
    public class PriceFileHeader
    {
        [Key]
        public long PriceFileHeaderID { get; set; }
        public long UserConfigSettingID { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Status { get; set; }

        [Column(TypeName = "varchar(512)")]
        public string StatusText { get; set; }

        public double PercentCompleted { get; set; }
        public bool IsCompleted { get; set; }
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
        public virtual ICollection<PriceFileDetails> PriceFileDetails { get; set; }
        public virtual ICollection<PriceFileLocationDetails> PriceFileLocationDetails { get; set; }
    }
}
