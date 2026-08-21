using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelUploadValidator.Models
{
    /// <summary>
    /// UploadValidatorRequest
    /// </summary>
    public class UploadValidatorRequest
    {
        public int TemplateID { get; set; }
        public string TemplateName { get; set; }
        public string TemplateFilepath { get; set; }
        public string ExcelFileName { get; set; }
        public IFormFileCollection lstFiles { get; set; }
        public List<TemplateDataStructure> lstTemplateDataStructure { get; set; }
    }

    public class TemplateDataStructure
    {
        public string FieldName { get; set; }
        public string FieldDescription { get; set; }
        public int SequenceNo { get; set; }

    }
}
