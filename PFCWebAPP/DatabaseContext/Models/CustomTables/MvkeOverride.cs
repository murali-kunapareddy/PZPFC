using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFCWebAPP.DatabaseContext.Models.CustomTables
{
    [PrimaryKey("MvkeOverrideId")]
    [Table("MVKE_Override")]
    public class MvkeOverride
    {
        [Key]
        public int MvkeOverrideId { get; set; }

        [Column("VKORG")]
        public string Vkorg { get; set; }

        [Column("MATNR")]
        public string Matnr { get; set; }

        [Column("PRODH")]
        public string Prodh { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
