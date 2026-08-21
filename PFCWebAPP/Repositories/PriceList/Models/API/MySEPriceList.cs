using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace PFCWebAPP.Repositories.PriceList.Models.API
{
    public class MySEPriceList
    {
        [Required]
        [XmlElement(Namespace = "")]
        public string SalesOrganization { get; set; }
        [Required]
        [XmlElement(Namespace = "")]
        public string DistributionChannel { get; set; }
        [Required]
        [XmlElement(Namespace = "")]
        public string CustomerId { get; set; }
        [Required]
        [XmlElement(Namespace = "")]
        public string PricingFileType { get; set; }
        [Required]
        [XmlElement(Namespace = "")]
        public string PriceStatus { get; set; }
        [Required]
        [XmlElement(Namespace = "")]
        public string PricingDate { get; set; }
        [Required]
        [XmlElement(Namespace = "")]
        [EmailAddress]
        public string CustomerEmail { get; set; }
    }
}
