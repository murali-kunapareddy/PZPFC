using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PFCRepository.DatabaseContext.Models.ExtractionTables
{
    [PrimaryKey("Knumh", "Kopos")]
    [Table("KONP")]
    public partial class Konp
    {
        [Key]
        [Column("KNUMH")]
        [StringLength(100)]
        public string Knumh { get; set; } = null!;

        [Key]
        [Column("KOPOS")]
        [StringLength(100)]
        public string Kopos { get; set; } = null!;

        [Column("STFKZ")]
        [StringLength(100)]
        public string? Stfkz { get; set; }

        [Column("KZBZG")]
        [StringLength(100)]
        public string? Kzbzg { get; set; }

        [Column("KSTBM")]
        public double? Kstbm { get; set; }

        [Column("KONMS")]
        [StringLength(100)]
        public string? Konms { get; set; }

        [Column("KRECH")]
        [StringLength(100)]
        public string? Krech { get; set; }

        [Column("KBETR")]
        public double? Kbetr { get; set; }

        [Column("KONWA")]
        [StringLength(100)]
        public string? Konwa { get; set; }

        [Column("KPEIN")]
        public double? Kpein { get; set; }

        [Column("KMEIN")]
        [StringLength(100)]
        public string? Kmein { get; set; }

        [Column("LOEVM_KO")]
        [StringLength(100)]
        public string? LoevmKo { get; set; }

        [Column("KNUMA_BO")]
        [StringLength(100)]
        public string? KnumaBo { get; set; }

        [Column("KFRST")]
        [StringLength(100)]
        public string? Kfrst { get; set; }
    }

}
