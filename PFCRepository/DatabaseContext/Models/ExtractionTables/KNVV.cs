using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PFCRepository.DatabaseContext.Models.ExtractionTables
{
    [PrimaryKey("Kunnr", "Vkorg", "Vtweg", "Spart")]
    [Table("KNVV")]
    public partial class Knvv
    {
        [Key]
        [Column("KUNNR")]
        [StringLength(100)]
        public string Kunnr { get; set; } = null!;

        [Key]
        [Column("VKORG")]
        [StringLength(100)]
        public string Vkorg { get; set; } = null!;

        [Key]
        [Column("VTWEG")]
        [StringLength(100)]
        public string Vtweg { get; set; } = null!;

        [Key]
        [Column("SPART")]
        [StringLength(100)]
        public string Spart { get; set; } = null!;

        [Column("LOEVM")]
        [StringLength(100)]
        public string? Loevm { get; set; }

        [Column("VERSG")]
        [StringLength(100)]
        public string? Versg { get; set; }

        [Column("KALKS")]
        [StringLength(100)]
        public string? Kalks { get; set; }

        [Column("BZIRK")]
        [StringLength(100)]
        public string? Bzirk { get; set; }

        [Column("KONDA")]
        [StringLength(100)]
        public string? Konda { get; set; }

        [Column("PLTYP")]
        [StringLength(100)]
        public string? Pltyp { get; set; }

        [Column("KZTLF")]
        [StringLength(100)]
        public string? Kztlf { get; set; }

        [Column("KZAZU")]
        [StringLength(100)]
        public string? Kzazu { get; set; }

        [Column("VSBED")]
        [StringLength(100)]
        public string? Vsbed { get; set; }

        [Column("KTGRD")]
        [StringLength(100)]
        public string? Ktgrd { get; set; }

        [Column("ZTERM")]
        [StringLength(100)]
        public string? Zterm { get; set; }

        [Column("VWERK")]
        [StringLength(100)]
        public string? Vwerk { get; set; }

        [Column("VKGRP")]
        [StringLength(100)]
        public string? Vkgrp { get; set; }

        [Column("VKBUR")]
        [StringLength(100)]
        public string? Vkbur { get; set; }

        [Column("KVGR1")]
        [StringLength(100)]
        public string? Kvgr1 { get; set; }

        [Column("KVGR2")]
        [StringLength(100)]
        public string? Kvgr2 { get; set; }

        [Column("KVGR3")]
        [StringLength(100)]
        public string? Kvgr3 { get; set; }

        [Column("KVGR4")]
        [StringLength(100)]
        public string? Kvgr4 { get; set; }

        [Column("KVGR5")]
        [StringLength(100)]
        public string? Kvgr5 { get; set; }

        [Column("PRFRE")]
        [StringLength(100)]
        public string? Prfre { get; set; }

        [Column("YYINVGROUP")]
        [StringLength(100)]
        public string? Yyinvgroup { get; set; }

        [Column("YYSLT")]
        [StringLength(100)]
        public string? Yyslt { get; set; }

        [Column("YYSTOCKRES")]
        [StringLength(100)]
        public string? Yystockres { get; set; }
    }

}
