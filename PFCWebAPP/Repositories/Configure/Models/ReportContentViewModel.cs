using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using static PFCWebAPP.Utilities.Common;

namespace PFCWebAPP.Repositories.Configure.Models
{
    public class ReportContentViewModel
    {
        [Display(Name = "Template Name")]
        [Required(ErrorMessage = "{0} is required.")]
        [Remote("IsTemplateAvailable", "Configure", ErrorMessage = "Template name already exists")]
        public string TemplateName { get; set; }

        [Display(Name = "Country Code")]
        [Required(ErrorMessage = "{0} is required.")]
        public string CountryCode { get; set; }

        public List<CountryCodeModel> DrpCountryCodes
        {
            get
            {
                return Utilities.Common.PFCCountryCode();
            }
        }

        public ReportContentModel reportContentModel { get; set; }

        public TemplateModel templateModel { get; set; }
    }

    public class ReportContentModel
    {
        [Display(Name = "Template Name")]
        [Required(ErrorMessage = "{0} is required.")]
        [Remote("IsTemplateAvailable", "Configure", ErrorMessage = "Template name already exists")]
        public string TemplateName { get; set; }

        [Display(Name = "Country Code")]
        [Required(ErrorMessage = "{0} is required.")]
        public string CountryCode { get; set; }

        public List<CountryCodeModel> DrpCountryCodes
        {
            get
            {
                return Utilities.Common.PFCCountryCode();
            }
        }
    }

}
