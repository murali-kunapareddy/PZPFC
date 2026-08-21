using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PFCRepository.DatabaseContext.Models.CustomTables
    {
        [Table("TRN_Queue")]
        public class QueueModel
        {
            [Key]
            public int QueueId { get; set; }

            [Required]
            [Column(TypeName = "varchar(10)")]
            public string SalesOrganization { get; set; }

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

            public bool IsActive { get; set; }

            [Column(TypeName = "varchar(100)")]
            public string CreatedBy { get; set; }

            [Column(TypeName = "datetime")]
            public DateTime CreatedDate { get; set; }
        }
    }