using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PFCRepository.DatabaseContext.Models.ExtractionTables
{
    [PrimaryKey("Matnr", "Werks")]
    [Table("MARC")]
    public partial class Marc
    {
        [Key]
        [Column("MATNR")]
        [StringLength(100)]
        public string Matnr { get; set; } = null!;

        [Key]
        [Column("WERKS")]
        [StringLength(100)]
        public string Werks { get; set; } = null!;

        [Column("LVORM")]
        [StringLength(100)]
        public string? Lvorm { get; set; }

        [Column("MMSTA")]
        [StringLength(100)]
        public string? Mmsta { get; set; }

        [Column("MMSTD", TypeName = "datetime")]
        public DateTime? Mmstd { get; set; }

        [Column("MAABC")]
        [StringLength(100)]
        public string? Maabc { get; set; }

        [Column("EKGRP")]
        [StringLength(100)]
        public string? Ekgrp { get; set; }

        [Column("DISMM")]
        [StringLength(100)]
        public string? Dismm { get; set; }

        [Column("DISPO")]
        [StringLength(100)]
        public string? Dispo { get; set; }

        [Column("PLIFZ")]
        public double? Plifz { get; set; }

        [Column("WEBAZ")]
        public short? Webaz { get; set; }

        [Column("DISLS")]
        [StringLength(100)]
        public string? Disls { get; set; }

        [Column("BESKZ")]
        [StringLength(100)]
        public string? Beskz { get; set; }

        [Column("SOBSL")]
        [StringLength(100)]
        public string? Sobsl { get; set; }

        [Column("MINBE")]
        public double? Minbe { get; set; }

        [Column("EISBE")]
        public double? Eisbe { get; set; }

        [Column("BSTMI")]
        public double? Bstmi { get; set; }

        [Column("BSTMA")]
        public double? Bstma { get; set; }

        [Column("BSTRF")]
        public double? Bstrf { get; set; }

        [Column("SBDKZ")]
        [StringLength(100)]
        public string? Sbdkz { get; set; }

        [Column("KZAUS")]
        [StringLength(100)]
        public string? Kzaus { get; set; }

        [Column("AUSDT", TypeName = "datetime")]
        public DateTime? Ausdt { get; set; }

        [Column("NFMAT")]
        [StringLength(100)]
        public string? Nfmat { get; set; }

        [Column("FHORI")]
        [StringLength(100)]
        public string? Fhori { get; set; }

        [Column("RGEKZ")]
        [StringLength(100)]
        public string? Rgekz { get; set; }

        [Column("FEVOR")]
        [StringLength(100)]
        public string? Fevor { get; set; }

        [Column("DZEIT")]
        public double? Dzeit { get; set; }

        [Column("YYLOQ")]
        public double? Yyloq { get; set; }

        [Column("YYMSID")]
        [StringLength(100)]
        public string? Yymsid { get; set; }

        [Column("YYMDSOPIND")]
        [StringLength(100)]
        public string? Yymdsopind { get; set; }

        [Column("YYMDITEMCATGR")]
        [StringLength(100)]
        public string? Yymditemcatgr { get; set; }

        [Column("YYAIRSEA")]
        [StringLength(100)]
        public string? Yyairsea { get; set; }

        [Column("YYQTYCONT")]
        public double? Yyqtycont { get; set; }

        [Column("YYSTOPOL")]
        [StringLength(100)]
        public string? Yystopol { get; set; }

        [Column("YYDWERK")]
        [StringLength(100)]
        public string? Yydwerk { get; set; }

        [Column("LADGR")]
        [StringLength(100)]
        public string? Ladgr { get; set; }

        [Column("USEQU")]
        [StringLength(100)]
        public string? Usequ { get; set; }

        [Column("MTVFP")]
        [StringLength(100)]
        public string? Mtvfp { get; set; }

        [Column("HERKL")]
        [StringLength(100)]
        public string? Herkl { get; set; }

        [Column("PRCTR")]
        [StringLength(100)]
        public string? Prctr { get; set; }

        [Column("TRAME")]
        public double? Trame { get; set; }

        [Column("MRPPP")]
        [StringLength(100)]
        public string? Mrppp { get; set; }

        [Column("DISGR")]
        [StringLength(100)]
        public string? Disgr { get; set; }

        [Column("ABCIN")]
        [StringLength(100)]
        public string? Abcin { get; set; }

        [Column("SERNP")]
        [StringLength(100)]
        public string? Sernp { get; set; }

        [Column("LGFSB")]
        [StringLength(100)]
        public string? Lgfsb { get; set; }

        [Column("SCHGT")]
        [StringLength(100)]
        public string? Schgt { get; set; }

        [Column("EPRIO")]
        [StringLength(100)]
        public string? Eprio { get; set; }

        [Column("SFCPF")]
        [StringLength(100)]
        public string? Sfcpf { get; set; }

        [Column("CASNR")]
        [StringLength(100)]
        public string? Casnr { get; set; }

        [Column("PERKZ")]
        [StringLength(100)]
        public string? Perkz { get; set; }

        [Column("ALTSL")]
        [StringLength(100)]
        public string? Altsl { get; set; }

        [Column("MISKZ")]
        [StringLength(100)]
        public string? Miskz { get; set; }

        [Column("SSQSS")]
        [StringLength(100)]
        public string? Ssqss { get; set; }

        [Column("YYSTOCKRESA")]
        [StringLength(100)]
        public string? Yystockresa { get; set; }

        [Column("KAUTB")]
        [StringLength(100)]
        public string? Kautb { get; set; }

        [Column("STAWN")]
        [StringLength(100)]
        public string? Stawn { get; set; }

        [Column("LOSGR")]
        public double? Losgr { get; set; }

        [Column("LGPRO")]
        [StringLength(100)]
        public string? Lgpro { get; set; }

        [Column("QMATV")]
        [StringLength(100)]
        public string? Qmatv { get; set; }

        [Column("AWSLS")]
        [StringLength(100)]
        public string? Awsls { get; set; }

        [Column("CCFIX")]
        [StringLength(100)]
        public string? Ccfix { get; set; }

        [Column("NCOST")]
        [StringLength(100)]
        public string? Ncost { get; set; }
    }

}
