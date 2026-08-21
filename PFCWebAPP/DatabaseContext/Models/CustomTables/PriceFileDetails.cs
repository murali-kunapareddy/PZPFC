using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFCWebAPP.DatabaseContext.Models.CustomTables
{
    [Table("TRN_PriceFileDetails")]
    public class PriceFileDetails
    {
        [Key]
        public long PriceFileDetailID { get; set; }
        public long PriceFileHeaderID { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string CustomerNo { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Prefix { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string CustomerCatNo { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string ColourCode { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string CustomerItemNo { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string SchneiderElectricMaterialReference { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string MaterialDescription { get; set; }

        public double WholesaleListPriceExclGST { get; set; }
        public double WholesaleListPriceInclGST { get; set; }
        public double Per { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string UOM { get; set; }

        public int MOQ { get; set; }
        public double OrderMultiple { get; set; }
        public double RecommendedRetailPrice { get; set; }
        public double AdvertisedRecommendedRetailPrice { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string PriceDerivedFrom { get; set; }
        public int PriceBreak1CustomerQty { get; set; }
        public double PriceBreak1CustomerDiscount { get; set; }
        public double PriceBreak1CustomerCostExclGST { get; set; }
        public double PriceBreak1CustomerCostInclGST { get; set; }
        public int PriceBreak2CustomerQty { get; set; }
        public double PriceBreak2CustomerDiscount { get; set; }
        public double PriceBreak2CustomerCostExclGST { get; set; }
        public double PriceBreak2CustomerCostInclGST { get; set; }
        public int PriceBreak3CustomerQty { get; set; }
        public double PriceBreak3CustomerDiscount { get; set; }
        public double PriceBreak3CustomerCostExclGST { get; set; }
        public double PriceBreak3CustomerCostInclGST { get; set; }
        public int PriceBreak4CustomerQty { get; set; }
        public double PriceBreak4CustomerDiscount { get; set; }
        public double PriceBreak4CustomerCostExclGST { get; set; }
        public double PriceBreak4CustomerCostInclGST { get; set; }
        public int PriceBreak5CustomerQty { get; set; }
        public double PriceBreak5CustomerDiscount { get; set; }
        public double PriceBreak5CustomerCostExclGST { get; set; }
        public double PriceBreak5CustomerCostInclGST { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Barcode { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string ProductHierarchy { get; set; } // 1=Global, 2=Local

        [Column(TypeName = "varchar(100)")]
        public string SAPCOS { get; set; } //SubGroup

        [Column(TypeName = "varchar(100)")]
        public string CartonQty { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string StockStatus { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ValidFrom { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ValidTo { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string FileReferenceData { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Currency { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string VRG { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string VRGDescription { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string MaterialStatus { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string MainGroup { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string MainGroupDescription { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Group { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string GroupDescription { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string SubGroup { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string SubGroupDescription { get; set; }
        public bool IsFound { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; } = false;

        [Column(TypeName = "varchar(100)")]
        public string CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        public virtual PriceFileHeader PriceFileHeader { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string DiscGroup { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string DiscGroupDescription { get; set; }
    }
}
