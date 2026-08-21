using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PFCRepository.DatabaseContext.Models.ExtractionTables
{
    [PrimaryKey("Kappl", "Kschl", "Vkorg", "Vtweg", "Spart", "Prodh1", "Prodh2", "Prodh3", "Kfrst", "Datbi")]
    [Table("A609")]
    public partial class A609
    {
        [Key]
        [Column("KAPPL")]
        [StringLength(100)]
        public string Kappl { get; set; } = null!;

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
        [Column("KFRST")]
        [StringLength(100)]
        public string Kfrst { get; set; } = null!;

        [Key]
        [Column("DATBI", TypeName = "datetime")]
        public DateTime Datbi { get; set; }

        [Column("DATAB", TypeName = "datetime")]
        public DateTime? Datab { get; set; }

        [Column("KBSTAT")]
        [StringLength(2)]
        public string? Kbstat { get; set; }

        [Column("KNUMH")]
        [StringLength(10)]
        public string? Knumh { get; set; }
    }

}
