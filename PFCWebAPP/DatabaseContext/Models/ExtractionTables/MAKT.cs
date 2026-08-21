using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PFCWebAPP.DatabaseContext.Models.ExtractionTables
{
    [Table("MAKT")]
    public partial class Makt
    {
        [Key]
        [Column("MATNR")]
        [StringLength(100)]
        public string Matnr { get; set; } = null!;

        [Column("MAKTX")]
        [StringLength(100)]
        public string? Maktx { get; set; }
    }
}
