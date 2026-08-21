using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PFCRepository.DatabaseContext.Models.ExtractionTables
{
    [PrimaryKey("Kschl", "Vkorg", "Vtweg", "Spart", "Zkvgr1", "Yykvgr2", "Yykvgr3", "Matnr", "Kfrst", "Datbi")]
    [Table("A653")]
    public partial class A653
    {
        [Column("KAPPL")]
        [StringLength(100)]
        public string? Kappl { get; set; }

        [Key]
        [Column("KSCHL")]
        [StringLength(100)]
        public string Kschl { get; set; } = null!;

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

        [Key]
        [Column("ZKVGR1")]
        [StringLength(100)]
        public string Zkvgr1 { get; set; } = null!;

        [Key]
        [Column("YYKVGR2")]
        [StringLength(100)]
        public string Yykvgr2 { get; set; } = null!;

        [Key]
        [Column("YYKVGR3")]
        [StringLength(100)]
        public string Yykvgr3 { get; set; } = null!;

        [Key]
        [Column("MATNR")]
        [StringLength(100)]
        public string Matnr { get; set; } = null!;

        [Key]
        [Column("KFRST")]
        [StringLength(100)]
        public string Kfrst { get; set; } = null!;

        [Key]
        [Column("DATBI", TypeName = "datetime")]
        public DateTime Datbi { get; set; }

        [Column("DATAB", TypeName = "datetime")]
        public DateTime? Datab { get; set; }

        [Column("KBSTAT")]
        [StringLength(100)]
        public string? Kbstat { get; set; }

        [Column("KNUMH")]
        [StringLength(100)]
        public string? Knumh { get; set; }
    }

}
