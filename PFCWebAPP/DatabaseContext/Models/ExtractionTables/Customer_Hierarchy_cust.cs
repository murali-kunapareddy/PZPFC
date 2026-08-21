using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFCWebAPP.DatabaseContext.Models.ExtractionTables
{
    [Table("Customer_Hierarchy_cust")]
    [PrimaryKey("KUNNR", "VKORG", "VTWEG", "SPART")]
    public class CustomerHierarchyCust
    {

        [Key]
        [StringLength(100)]
        [Column("KUNNR")]
        public string? KUNNR { get; set; }

        [Key]
        [StringLength(100)]
        [Column("VKORG")]
        public string? VKORG { get; set; }

        [Key]
        [StringLength(100)]
        [Column("VTWEG")]
        public string? VTWEG { get; set; }

        [Key]
        [StringLength(100)]
        [Column("SPART")]
        public string?  SPART { get; set; }

        [StringLength(100)]
        [Column("Level1")]
        public string? Level1 { get; set; }

        [StringLength(100)]
        [Column("Level2")]
        public string? Level2 { get; set; }

        [StringLength(100)]
        [Column("Level3")]
        public string? Level3 { get; set; }

        [StringLength(100)]
        [Column("Level4")]
        public string? Level4 { get; set; }

        [StringLength(100)]
        [Column("Level5")]
        public string? Level5 { get; set; }

        [StringLength(100)]
        [Column("Level6")]
        public string? Level6 { get; set; }

        [StringLength(100)]
        [Column("Level7")]
        public string?  Level7 { get; set; }

        [StringLength(100)]
        [Column("Level8")]
        public string? Level8 { get; set; }

        [StringLength(100)]
        [Column("Level9")]
        public string? Level9 { get; set; }

        [StringLength(100)]
        [Column("Level10")]
        public string? Level10 { get; set; }

        [StringLength(255)]
        [Column("Level1_name")]
        public string? Level1Name { get; set; }

        [StringLength(255)]
        [Column("Level2_name")]
        public string? Level2Name { get; set; }

        [StringLength(255)]
        [Column("Level3_name")]
        public string? Level3Name { get; set; }

        [StringLength(255)]
        [Column("Level4_name")]
        public string? Level4Name { get; set; }

        [StringLength(255)]
        [Column("Level5_name")]
        public string? Level5Name { get; set; }

        [StringLength(255)]
        [Column("Level6_name")]
        public string? Level6Name { get; set; }

        [StringLength(255)]
        [Column("Level7_name")]
        public string? Level7Name { get; set; }

        [StringLength(255)]
        [Column("Level8_name")]
        public string? Level8Name { get; set; }

        [StringLength(255)]
        [Column("Level9_name")]
        public string? Level9Name { get; set; }

        [StringLength(255)]
        [Column("Level10_name")]
        public string? Level10Name { get; set; }

    }
}
