using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFCWebAPP.DatabaseContext.Models.CustomTables
{
    public class MaterialMaster
    {
        public int MaterialMasterID { get; set; }

        [Column(TypeName = "varchar(3)")]
        [Required]
        public string Prefix { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string ColourCode { get; set; }

        [Column(TypeName = "varchar(50)")]
        [Required]
        public string CatNo { get; set; }

        [Column(TypeName = "varchar(50)")]
        [Required]
        public string ItemNo { get; set; }

        [Column(TypeName = "varchar(50)")]
        [Required]
        public string InternalSAPItemNo { get; set; }
        public int SplitPackQty { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; } = false;

        [Column(TypeName = "varchar(100)")]
        public string CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        //public byte[] RowVersion { get; set; }
    }
}
