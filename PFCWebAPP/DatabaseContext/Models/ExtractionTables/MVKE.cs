using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFCWebAPP.DatabaseContext.Models.ExtractionTables
{
    [PrimaryKey("Matnr", "Vkorg", "Vtweg")]
    [Table("MVKE")]
    public partial class Mvke
    {
        [Key]
        [Column("MATNR")]
        [StringLength(100)]
        public string Matnr { get; set; } = null!;

        [Key]
        [Column("VKORG")]
        [StringLength(100)]
        public string Vkorg { get; set; } = null!;

        [Key]
        [Column("VTWEG")]
        [StringLength(100)]
        public string Vtweg { get; set; } = null!;

        [Column("LVORM")]
        [StringLength(100)]
        public string? Lvorm { get; set; }

        [Column("VERSG")]
        [StringLength(100)]
        public string? Versg { get; set; }

        [Column("VMSTA")]
        [StringLength(100)]
        public string? Vmsta { get; set; }

        [Column("VMSTD", TypeName = "datetime")]
        public DateTime? Vmstd { get; set; }

        [Column("AUMNG")]
        public double? Aumng { get; set; }

        [Column("LFMNG")]
        public double? Lfmng { get; set; }

        [Column("SCMNG")]
        public double? Scmng { get; set; }

        [Column("MTPOS")]
        [StringLength(100)]
        public string? Mtpos { get; set; }

        [Column("DWERK")]
        [StringLength(100)]
        public string? Dwerk { get; set; }

        [Column("PRODH")]
        [StringLength(100)]
        public string? Prodh { get; set; }

        [Column("KONDM")]
        [StringLength(100)]
        public string? Kondm { get; set; }

        [Column("KTGRM")]
        [StringLength(100)]
        public string? Ktgrm { get; set; }

        [Column("MVGR2")]
        [StringLength(100)]
        public string? Mvgr2 { get; set; }

        [Column("MVGR3")]
        [StringLength(100)]
        public string? Mvgr3 { get; set; }

        [Column("MVGR5")]
        [StringLength(100)]
        public string? Mvgr5 { get; set; }

        [Column("PRAT1")]
        [StringLength(100)]
        public string? Prat1 { get; set; }

        [Column("PRAT2")]
        [StringLength(100)]
        public string? Prat2 { get; set; }

        [Column("PRAT5")]
        [StringLength(100)]
        public string? Prat5 { get; set; }

        [Column("PRAT6")]
        [StringLength(100)]
        public string? Prat6 { get; set; }

        [Column("PRAT8")]
        [StringLength(100)]
        public string? Prat8 { get; set; }

        [Column("VAVME")]
        [StringLength(100)]
        public string? Vavme { get; set; }

        [Column("PLGTP")]
        [StringLength(100)]
        public string? Plgtp { get; set; }

        [Column("YYMARKETID")]
        [StringLength(100)]
        public string? Yymarketid { get; set; }

        [Column("YYPRDDATE", TypeName = "datetime")]
        public DateTime? Yyprddate { get; set; }
    }

}
