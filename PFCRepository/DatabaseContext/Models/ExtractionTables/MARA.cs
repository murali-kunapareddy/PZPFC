using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PFCRepository.DatabaseContext.Models.ExtractionTables
{
    [Table("MARA")]
    public partial class Mara
    {
        [Key]
        [Column("MATNR")]
        [StringLength(100)]
        public string Matnr { get; set; } = null!;

        [Column("ERSDA", TypeName = "datetime")]
        public DateTime? Ersda { get; set; }

        [Column("MTART")]
        [StringLength(4)]
        public string? Mtart { get; set; }

        [Column("MATKL")]
        [StringLength(9)]
        public string? Matkl { get; set; }

        [Column("MEINS")]
        [StringLength(3)]
        public string? Meins { get; set; }

        [Column("BRGEW")]
        public double? Brgew { get; set; }

        [Column("NTGEW")]
        public double? Ntgew { get; set; }

        [Column("GEWEI")]
        [StringLength(3)]
        public string? Gewei { get; set; }

        [Column("VOLUM")]
        public double? Volum { get; set; }

        [Column("VOLEH")]
        [StringLength(3)]
        public string? Voleh { get; set; }

        [Column("EAN11")]
        [StringLength(18)]
        public string? Ean11 { get; set; }

        [Column("LAENG")]
        public double? Laeng { get; set; }

        [Column("BREIT")]
        public double? Breit { get; set; }

        [Column("HOEHE")]
        public double? Hoehe { get; set; }

        [Column("MEABM")]
        [StringLength(3)]
        public string? Meabm { get; set; }

        [Column("PRDHA")]
        [StringLength(18)]
        public string? Prdha { get; set; }

        [Column("EXTWG")]
        [StringLength(18)]
        public string? Extwg { get; set; }

        [Column("MSTAV")]
        [StringLength(2)]
        public string? Mstav { get; set; }

        [Column("YYPMO")]
        [StringLength(18)]
        public string? Yypmo { get; set; }

        [Column("YYMTART")]
        [StringLength(4)]
        public string? Yymtart { get; set; }

        [Column("PSTAT")]
        [StringLength(15)]
        public string? Pstat { get; set; }

        [Column("TRAGR")]
        [StringLength(4)]
        public string? Tragr { get; set; }

        [Column("MSTDV", TypeName = "datetime")]
        public DateTime? Mstdv { get; set; }

        [Column("YYSYS")]
        [StringLength(10)]
        public string? Yysys { get; set; }

        [Column("YYMIGPRO")]
        [StringLength(6)]
        public string? Yymigpro { get; set; }
    }

}
