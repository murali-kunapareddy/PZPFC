using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelUploadValidator.Models
{
    /// <summary>
    /// UploadValidatorResponse
    /// </summary>
    public class UploadValidatorResponse
    {      
        public int TemplateMasterID { get; set; }
        public DataTable? TemplateData { get; set; }
        public string TemplateFileName { get; set; }
        public UploadValidatorStatus UploadValidatorStatus { get; set; }
        public string? ResultMessage { get; set; }

    }

    public enum UploadValidatorStatus
    {
        FailedDuetoUnKnowError = 0,
        FailedDueToInvalidRequestPayload= 1,
        FailedDueToMismatchedColumns =2,
        Success = 3


    }
}
