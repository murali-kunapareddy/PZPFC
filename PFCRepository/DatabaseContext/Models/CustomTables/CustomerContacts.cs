using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFCRepository.DatabaseContext.Models.CustomTables
{
    public class CustomerContacts
    {
        [Key]
        public int CustomerContactID { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string AccountNumber { get; set; }

        [Column(TypeName = "varchar(250)")]
        public string AccountName { get; set; }

        [Column(TypeName = "varchar(250)")]
        public string ContactPerson { get; set; }

        [Column(TypeName = "varchar(1024)")]
        public string ToEmailID { get; set; } //we can give multiple EmailID's with SemiColan separator

        [Column(TypeName = "varchar(1024)")]
        public string CcEmailID { get; set; }

        [Column(TypeName = "varchar(1024)")]
        public string BccEmailID { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; } = false;

        [Column(TypeName = "varchar(100)")]
        public string CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
    }
}
