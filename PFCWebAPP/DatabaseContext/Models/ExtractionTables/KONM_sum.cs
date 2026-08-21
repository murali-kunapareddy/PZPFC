using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PFCWebAPP.DatabaseContext.Models.ExtractionTables
{
    [Table("KONM_sum")]
    public partial class KonmSum
    {
        [Key]
        [Column("KNUMH")]
        [StringLength(100)]
        public string Knumh { get; set; } = null!;

        [Column("KSTBM1")]
        public double? Kstbm1 { get; set; }

        [Column("KSTBM2")]
        public double? Kstbm2 { get; set; }

        [Column("KSTBM3")]
        public double? Kstbm3 { get; set; }

        [Column("KSTBM4")]
        public double? Kstbm4 { get; set; }

        [Column("KSTBM5")]
        public double? Kstbm5 { get; set; }

        [Column("KSTBM6")]
        public double? Kstbm6 { get; set; }

        [Column("KSTBM7")]
        public double? Kstbm7 { get; set; }

        [Column("KSTBM8")]
        public double? Kstbm8 { get; set; }

        [Column("KBETR1")]
        public double? Kbetr1 { get; set; }

        [Column("KBETR2")]
        public double? Kbetr2 { get; set; }

        [Column("KBETR3")]
        public double? Kbetr3 { get; set; }

        [Column("KBETR4")]
        public double? Kbetr4 { get; set; }

        [Column("KBETR5")]
        public double? Kbetr5 { get; set; }

        [Column("KBETR6")]
        public double? Kbetr6 { get; set; }

        [Column("KBETR7")]
        public double? Kbetr7 { get; set; }

        [Column("KBETR8")]
        public double? Kbetr8 { get; set; }

        [StringLength(255)]
        public string? Prices { get; set; }

        [StringLength(255)]
        public string? Source { get; set; }
    }

}
