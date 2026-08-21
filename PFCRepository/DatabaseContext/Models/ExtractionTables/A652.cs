using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PFCRepository.DatabaseContext.Models.ExtractionTables
{
    [PrimaryKey("Kappl", "Kschl", "Vkorg", "Vtweg", "Spart", "Kunnr", "Matnr", "Kfrst", "Datbi")]
    [Table("A652")]
    public partial class A652
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
        [Column("KUNNR")]
        [StringLength(100)]
        public string Kunnr { get; set; } = null!;

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
