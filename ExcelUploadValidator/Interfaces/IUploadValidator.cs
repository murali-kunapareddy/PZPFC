using ExcelUploadValidator.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelUploadValidator.Interfaces
{
    public interface IUploadValidator : IDisposable
    {
        UploadValidatorResponse ValidateDataWithTemplateStructure(UploadValidatorRequest objUploadValidatorRequest);
        DataTable ConvertExcelDataIntoDataTable(string fileName, string fpath);
    }
}
