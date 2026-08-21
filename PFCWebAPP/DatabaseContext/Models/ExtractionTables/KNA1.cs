using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PFCWebAPP.DatabaseContext.Models.ExtractionTables
{
    [Table("KNA1")]
    public partial class Kna1
    {
        [Key]
        [Column("KUNNR")]
        [StringLength(100)]
        public string Kunnr { get; set; } = null!;

        [Column("LAND1")]
        [StringLength(100)]
        public string? Land1 { get; set; }

        [Column("NAME1")]
        [StringLength(100)]
        public string? Name1 { get; set; }

        [Column("ORT01")]
        [StringLength(100)]
        public string? Ort01 { get; set; }

        [Column("PSTLZ")]
        [StringLength(100)]
        public string? Pstlz { get; set; }

        [Column("REGIO")]
        [StringLength(100)]
        public string? Regio { get; set; }

        [Column("STRAS")]
        [StringLength(100)]
        public string? Stras { get; set; }

        [Column("TELF1")]
        [StringLength(100)]
        public string? Telf1 { get; set; }

        [Column("ADRNR")]
        [StringLength(100)]
        public string? Adrnr { get; set; }

        [Column("KTOKD")]
        [StringLength(100)]
        public string? Ktokd { get; set; }

        [Column("LOEVM")]
        [StringLength(100)]
        public string? Loevm { get; set; }

        [Column("ORT02")]
        [StringLength(100)]
        public string? Ort02 { get; set; }

        [Column("PFACH")]
        [StringLength(100)]
        public string? Pfach { get; set; }

        [Column("LZONE")]
        [StringLength(100)]
        public string? Lzone { get; set; }

        [Column("KATR4")]
        [StringLength(100)]
        public string? Katr4 { get; set; }

        [Column("KATR6")]
        [StringLength(100)]
        public string? Katr6 { get; set; }

        [Column("KATR7")]
        [StringLength(100)]
        public string? Katr7 { get; set; }

        [Column("KATR9")]
        [StringLength(100)]
        public string? Katr9 { get; set; }

        [Column("KATR10")]
        [StringLength(100)]
        public string? Katr10 { get; set; }

        [Column("NODEL")]
        [StringLength(100)]
        public string? Nodel { get; set; }

        [Column("YYSESAID")]
        [StringLength(100)]
        public string? Yysesaid { get; set; }

        [Column("YYNAME")]
        [StringLength(100)]
        public string? Yyname { get; set; }

        [Column("YYCUGOLDID")]
        [StringLength(100)]
        public string? Yycugoldid { get; set; }

        [Column("VBUND")]
        [StringLength(100)]
        public string? Vbund { get; set; }

        [Column("YKATR4")]
        [StringLength(100)]
        public string? Ykatr4 { get; set; }

        [Column("YKATR6")]
        [StringLength(100)]
        public string? Ykatr6 { get; set; }

        [Column("YKATR7")]
        [StringLength(100)]
        public string? Ykatr7 { get; set; }

        [Column("YKATR9")]
        [StringLength(100)]
        public string? Ykatr9 { get; set; }

        [Column("YKATR10")]
        [StringLength(100)]
        public string? Ykatr10 { get; set; }
    }

}
