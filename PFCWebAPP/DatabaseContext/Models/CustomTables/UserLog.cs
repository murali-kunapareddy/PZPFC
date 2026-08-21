using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PFCWebAPP.DatabaseContext.Models.CustomTables
{
    [Table("TRN_UserLog")]
    public class UserLog
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UserLogID { get; set; }

        [StringLength(15)]
        [Column(TypeName = "varchar(15)")]
        [Required]
        public string UserSESA { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime AttemptedOn { get; set; } = DateTime.UtcNow;
        public bool IsSuccess { get; set; } = true;
        [StringLength(25)]
        [Column(TypeName = "varchar(25)")]
        public string IPAddress { get; set; }

        [Column(TypeName = "varchar(50)")]
        [StringLength(50)]
        public string MachineName { get; set; }

        [Column(TypeName = "varchar(50)")]
        [StringLength(50)]
        public string OperatingSystem { get; set; }

        [Column(TypeName = "varchar(100)")]
        [StringLength(100)]
        public string UserHostAddress { get; set; }

        [Column(TypeName = "varchar(100)")]
        [StringLength(100)]
        public string UserAgent { get; set; }
        //public bool IsActive { get; set; } = true;

        //[Timestamp]
        //public byte[] RowVersion { get; set; }
    }
}