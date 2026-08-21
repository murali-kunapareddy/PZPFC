using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PFCWebAPP.DatabaseContext.Models.CustomTables
{
    [Table("MST_Menus")]
    public class Menu
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MenuID { get; set; }

        //public Guid? MenuGUID { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string MenuName { get; set; }

        public int ParentID { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string ControllerName { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string ActionName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string AliasName { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string HrefVal { get; set; }
        public int SortOrder { get; set; }
        public bool CanShowMenu { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; } = false;

        [Column(TypeName = "varchar(100)")]
        public string CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }




        //[Timestamp]
        //public byte[] RowVersion { get; set; }

        public virtual ICollection<RoleMenu> RoleMenus { get; set; }

    }
}