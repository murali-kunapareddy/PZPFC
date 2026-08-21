using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PFCWebAPP.DatabaseContext.Models.CustomTables
{
    [Table("MST_RoleMenus")]
    public class RoleMenu
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleMenuID { get; set; }

        public int RoleID { get; set; }

        public int MenuID { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; } = false;

        [Column(TypeName = "varchar(100)")]
        public string CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }

        //[Timestamp]
        //public byte[] RowVersion { get; set; }

        public virtual Role Roles { get; set; }
        public virtual Menu Menus { get; set; }

    }
}