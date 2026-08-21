using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PFCWebAPP.DatabaseContext.Models.CustomTables
{
    [Table("MST_UserRoleMapping")]
    public class UserRoleMapping
    {
        [Key]
        // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserRoleMappingID { get; set; }

        [Column(TypeName = "varchar(15)")]
        [Required]
        public string UserSESA { get; set; }
        public int? RoleID { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        [Column(TypeName = "varchar(100)")]
        public string CreatedBy { get; set; } = "";

        [Column(TypeName = "varchar(100)")]
        public string ModifiedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "datetime")]
        public DateTime ModifiedDate { get; set; }

        //[Timestamp]
        //public byte[] RowVersion { get; set; }

        public virtual Role Roles { get; set; }
    }
}