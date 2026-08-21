using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PFCRepository.DatabaseContext.Models.CustomTables
{
    [Table("MST_Roles")]
    public class Role
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleID { get; set; }
        //public Guid? RoleGUID { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Required]
        public string RoleName { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; } = false;

        [Column(TypeName = "varchar(100)")]
        public string CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }

        //[Timestamp]
        //public byte[] RowVersion { get; set; }
        public virtual ICollection<RoleMenu> RoleMenus { get; set; }
        public virtual ICollection<UserRoleMapping> UserRoleMapping { get; set; }
    }
}