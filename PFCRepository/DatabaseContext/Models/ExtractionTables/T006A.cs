using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PFCRepository.DatabaseContext.Models.ExtractionTables
{
    [Table("T006A")]
    public partial class T006a
    {
        [Key]
        [Column("MSEHI")]
        [StringLength(100)]
        public string Msehi { get; set; } = null!;

        [Column("MSEH3")]
        [StringLength(100)]
        public string? Mseh3 { get; set; }

        [Column("MSEHT")]
        [StringLength(100)]
        public string? Mseht { get; set; }

        [Column("MSEHL")]
        [StringLength(100)]
        public string? Msehl { get; set; }
    }

}
