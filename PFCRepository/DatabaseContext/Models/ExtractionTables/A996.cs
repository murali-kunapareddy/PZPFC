using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PFCRepository.DatabaseContext.Models.ExtractionTables
{
    [PrimaryKey("Kschl", "Vkorg", "Vtweg", "Spart", "Yykvgr3", "Konda", "Prodh1", "Prodh2", "Prodh3", "Datbi")]
    [Table("A996")]
    public partial class A996
    {
        [Column("KAPPL")]
        [StringLength(2)]
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
        [Column("YYKVGR3")]
        [StringLength(100)]
        public string Yykvgr3 { get; set; } = null!;

        [Key]
        [Column("KONDA")]
        [StringLength(100)]
        public string Konda { get; set; } = null!;

        [Key]
        [Column("PRODH1")]
        [StringLength(100)]
        public string Prodh1 { get; set; } = null!;

        [Key]
        [Column("PRODH2")]
        [StringLength(100)]
        public string Prodh2 { get; set; } = null!;

        [Key]
        [Column("PRODH3")]
        [StringLength(100)]
        public string Prodh3 { get; set; } = null!;

        [Key]
        [Column("DATBI")]
        [StringLength(100)]
        public string Datbi { get; set; } = null!;

        [Column("DATAB", TypeName = "datetime")]
        public DateTime? Datab { get; set; }

        [Column("KNUMH")]
        [StringLength(10)]
        public string? Knumh { get; set; }
    }

}
