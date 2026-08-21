using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PFCRepository.DatabaseContext.Models.ExtractionTables
{
    [Table("MARM_sum")]
    public partial class MarmSum
    {
        [Key]
        [Column("MATNR")]
        [StringLength(100)]
        public string Matnr { get; set; } = null!;

        [Column("UoM_Text")]
        [StringLength(255)]
        public string? UoMText { get; set; }

        [Column("UoM_Qty_Text")]
        [StringLength(255)]
        public string? UoMQtyText { get; set; }
    }

}
