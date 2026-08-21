using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFCWebAPP.DatabaseContext.Models.CustomTables
{
    [Table("TRN_Queue_History")]
    public class QueueHistory
    {
        [Key]
        public int QueueHistoryId { get; set; }

        [Required]
        [Column(TypeName = "varchar(10)")]
        public string SalesOrganization { get; set; }

        
        public long UserConfigId { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Distributionchannel { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string CustomerId { get; set; }

        [Required]
        [Column(TypeName = "varchar(10)")]
        public string PricingFiletype { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string PriceStatus { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime PricingDate { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string CustomerEmail { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string QueueStatus { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string QueueMessage { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }

        public bool IsDeleted { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string ModifiedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime ModifiedDate { get; set; }
    }
}
