using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PFCWebAPP.DatabaseContext.Models.CustomTables
{
    [Table("MST_UserMaster")]
    public class UserMaster
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserMasterID { get; set; }

        [Remote("IsUserNameAvailable", "BackOps", ErrorMessage = "Existing User")]
        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "UserSESA")]
        [StringLength(15, MinimumLength = 7, ErrorMessage = "Minimum 7 characters should be entered")]
        [Column(TypeName = "varchar(15)")]
        //[RegularExpression(@"^(SESA|SESI|ADM)[a-zA-Z0-9]+$", ErrorMessage = "Please enter Valid UserSESA")]
        [RegularExpression(@"^(?:[A-Za-z]{3}\d{6}|[A-Za-z]{4}\d{6}|[A-Za-z]{3}\d{5}|[A-Za-z]{4}\d{5}|[A-Za-z]{3}\d{7}|[A-Za-z]{4}\d{7})$", ErrorMessage = "Please enter Valid UserSESA")]
        public string UserSESA { get; set; }

        [StringLength(100)]
        [Column(TypeName = "varchar(100)")]
        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(100)]
        [Column(TypeName = "varchar(100)")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;


        [StringLength(250)]
        [Column(TypeName = "varchar(250)")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        //[StringLength(250)]
        //[Column(TypeName = "nvarchar(250)")]
        //[Display(ResourceType = typeof(Users), Name = "Company")]
        //public string Company { get; set; } = string.Empty;



        [StringLength(250)]
        [Column(TypeName = "nvarchar(250)")]
        [Display(Name = "Department")]
        public string Department { get; set; } = string.Empty;


        //[StringLength(250)]
        //[Column(TypeName = "nvarchar(250)")]
        //[Display(ResourceType = typeof(Users), Name = "Location")]
        //public string Location { get; set; } = string.Empty;

        //[StringLength(15)]
        //[Column(TypeName = "varchar(15)")]
        //[Required(ErrorMessage = "{0} is required.")]
        //[Display(ResourceType = typeof(Users), Name = "ManagerSESA")]
        //public string ManagerSESA { get; set; }

        [StringLength(2)]
        [Column(TypeName = "varchar(2)")]
        [Required(ErrorMessage = "{0} is required.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Country Code cannot be Numeric")]
        [Remote("IsCountyCodeAvailable", "BackOps", ErrorMessage = "Invalid {0}")]
        [Display(Name = "Country")]
        public string Country { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        [Column(TypeName = "varchar(100)")]
        public string CreatedBy { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string ModifiedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime ModifiedDate { get; set; }

        //[Timestamp]
        //public byte[] RowVersion { get; set; }
    }
}