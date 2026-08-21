using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PFCRepository.DatabaseContext.Models.ExtractionTables
{
    [PrimaryKey("Kschl", "Vkorg", "Vtweg", "Spart", "Kunnr", "Prodh1", "Prodh2", "Prodh3", "Kfrst", "Datbi")]
    [Table("A606")]
    public partial class A606
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
        [Column("KUNNR")]
        [StringLength(100)]
        public string Kunnr { get; set; } = null!;

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
        [StringLength(100)]
        public string? Kbstat { get; set; }

        [Column("KNUMH")]
        [StringLength(100)]
        public string? Knumh { get; set; }
    }

}
