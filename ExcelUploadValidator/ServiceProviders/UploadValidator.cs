using ExcelUploadValidator.Interfaces;
using ExcelUploadValidator.Models;
using NLog;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using System.Text;

namespace ExcelUploadValidator.ServiceProviders
{
    /// <summary>
    /// UploadValidator
    /// </summary>
    public class UploadValidator : IUploadValidator
    {
        private static ILogger _objLoggingProvider = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// ValidateDataWithTemplateStructure
        /// </summary>
        /// <param name="objUploadValidatorRequest"></param>
        /// <returns></returns>
        public UploadValidatorResponse ValidateDataWithTemplateStructure(UploadValidatorRequest objUploadValidatorRequest)
        {
            UploadValidatorResponse objUploadValidatorResponse = new UploadValidatorResponse();
            objUploadValidatorResponse.TemplateMasterID = objUploadValidatorRequest.TemplateID;
            try
            {
                #region Validations
                if (objUploadValidatorRequest.lstTemplateDataStructure == null || objUploadValidatorRequest.lstTemplateDataStructure.Count() == 0)
                {
                    objUploadValidatorResponse.UploadValidatorStatus = UploadValidatorStatus.FailedDueToInvalidRequestPayload;
                    objUploadValidatorResponse.ResultMessage = "Invalid TemplateDataStructure Data, Please Provide Valid Data.";
                    return objUploadValidatorResponse;

                }
                else if (objUploadValidatorRequest.TemplateID <= 0)
                {
                    objUploadValidatorResponse.UploadValidatorStatus = UploadValidatorStatus.FailedDueToInvalidRequestPayload; 
                    objUploadValidatorResponse.ResultMessage = "TemplateID Must be GreaterThan Zero";
                    return objUploadValidatorResponse;

                }
                else if (objUploadValidatorRequest.TemplateName  == null  || objUploadValidatorRequest.TemplateName.Trim() == "")
                {
                    objUploadValidatorResponse.UploadValidatorStatus = UploadValidatorStatus.FailedDueToInvalidRequestPayload; 
                    objUploadValidatorResponse.ResultMessage = "TemplateName Must not be Empty";
                    return objUploadValidatorResponse;

                }
                else if (objUploadValidatorRequest.TemplateFilepath == null || objUploadValidatorRequest.TemplateFilepath.Trim() == "")
                {
                    objUploadValidatorResponse.UploadValidatorStatus = UploadValidatorStatus.FailedDueToInvalidRequestPayload;
                    objUploadValidatorResponse.ResultMessage = "TemplateFilepath Must not be Empty";
                    return objUploadValidatorResponse;

                }
                else if (objUploadValidatorRequest.TemplateFilepath == null || objUploadValidatorRequest.TemplateFilepath.Trim() == "")
                {
                    objUploadValidatorResponse.UploadValidatorStatus = UploadValidatorStatus.FailedDueToInvalidRequestPayload;
                    objUploadValidatorResponse.ResultMessage = "TemplateFilepath Must not be Empty";
                    return objUploadValidatorResponse;

                }
                else if (objUploadValidatorRequest.ExcelFileName == null || objUploadValidatorRequest.ExcelFileName.Trim() == "")
                {
                    objUploadValidatorResponse.UploadValidatorStatus = UploadValidatorStatus.FailedDueToInvalidRequestPayload;
                    objUploadValidatorResponse.ResultMessage = "ExcelFileName Must not be Empty";
                    return objUploadValidatorResponse;

                }
                else if (objUploadValidatorRequest.lstFiles == null)
                {
                    objUploadValidatorResponse.UploadValidatorStatus = UploadValidatorStatus.FailedDueToInvalidRequestPayload;
                    objUploadValidatorResponse.ResultMessage = "lstFiles Must not be null";
                    return objUploadValidatorResponse;

                }
                #endregion


                string fileExt = Path.GetExtension(objUploadValidatorRequest.ExcelFileName);

                #region Copy File in Temporary Location
                string temporaryFileName = Guid.NewGuid().ToString() + fileExt;
                string fpath = Path.Combine(Directory.GetCurrentDirectory(), objUploadValidatorRequest.TemplateFilepath);
                string fname = Path.Combine(objUploadValidatorRequest.TemplateFilepath, temporaryFileName);
      

                if (!Directory.Exists(objUploadValidatorRequest.TemplateFilepath))
                    Directory.CreateDirectory(objUploadValidatorRequest.TemplateFilepath);

                
                using (var stream = new FileStream(fname, FileMode.Create))
                {
                    objUploadValidatorRequest.lstFiles[0].CopyTo(stream);
                }

                #endregion



                var dt = ConvertExcelDataIntoDataTable(fname, fpath);
                if (dt != null)
                {



                    objUploadValidatorResponse.TemplateFileName = temporaryFileName;

                    List<string> columnHeads = new List<string>();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        columnHeads.Add(dt.Columns[i].ColumnName.Trim().ToUpper().ToString());
                    }
                    if (CompareColumns(columnHeads, objUploadValidatorRequest.lstTemplateDataStructure))
                    {
                        objUploadValidatorResponse.UploadValidatorStatus = UploadValidatorStatus.Success;
                        objUploadValidatorResponse.TemplateData = dt;
                        objUploadValidatorResponse.ResultMessage = "Success";
                        return objUploadValidatorResponse;
                    }
                    else
                    {
                        objUploadValidatorResponse.UploadValidatorStatus = UploadValidatorStatus.FailedDueToMismatchedColumns;
                        objUploadValidatorResponse.ResultMessage = MismatchedColumns(columnHeads, objUploadValidatorRequest.TemplateName, objUploadValidatorRequest.lstTemplateDataStructure);
                        return objUploadValidatorResponse;
                    }
                }
                else
                {
                    _objLoggingProvider.Info("dtTemplateData== null");
                    objUploadValidatorResponse.UploadValidatorStatus = UploadValidatorStatus.FailedDueToInvalidRequestPayload;
                    objUploadValidatorResponse.ResultMessage = "Invalid Excel File";
                    return objUploadValidatorResponse;

                }

               

            }
            catch (Exception ex)
            {
                _objLoggingProvider.Error("Error while MismatchedColumns", ex);

                objUploadValidatorResponse.UploadValidatorStatus = UploadValidatorStatus.FailedDuetoUnKnowError;
                objUploadValidatorResponse.ResultMessage = "Failed";
                return objUploadValidatorResponse;
            }
            //return objUploadValidatorResponse;
        }

        /// <summary>
        /// ConvertExcelDataIntoDataTable
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fpath"></param>
        /// <returns></returns>
        public DataTable ConvertExcelDataIntoDataTable(string fileName, string fpath)
        {
            try
            {
                var dt = new DataTable();
                var fileExt = Path.GetExtension(fileName);

                //
                ISheet sheet;
                //
                if (File.Exists(fileName))
                {
                    using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                    {
                        try
                        {
                            if (fileExt == ".xls")
                            {

                                HSSFWorkbook hssfwb = new HSSFWorkbook(fs);
                                hssfwb.MissingCellPolicy = MissingCellPolicy.RETURN_BLANK_AS_NULL;
                                sheet = hssfwb.GetSheetAt(0);
                            }
                            else
                            //XSSFWorkBook will read 2007 Excel format  
                            {
                                XSSFWorkbook hssfwb = new XSSFWorkbook(fs);
                                hssfwb.MissingCellPolicy = MissingCellPolicy.CREATE_NULL_AS_BLANK;
                                sheet = hssfwb.GetSheetAt(0);
                            }
                            //

                            // get headers
                            IRow headerRow = sheet.GetRow(0);   // always first row is header row
                            List<string> columnHeads = new List<string>();
                            foreach (ICell headerCell in headerRow)
                            {
                                var colName = headerCell.ToString();
                                columnHeads.Add(colName);
                                dt.Columns.Add(colName);
                            }
                            // get the rest
                            int rowIndex = 0;
                            foreach (IRow row in sheet)
                            {
                                // skip header row
                                if (rowIndex++ == 0) continue;
                                DataRow dr = dt.NewRow();
                                int colCount = (row.Cells.Count < dt.Columns.Count) ? row.Cells.Count : dt.Columns.Count;

                                for (int i = 0; i < colCount; i++)
                                {
                                    dr[i] = row.Cells[i].ToString();
                                }

                                dr.ItemArray = row.Cells.Select(c => c == null ? "" : c.ToString()).ToArray();
                                dt.Rows.Add(dr);
                            }
                        }
                        catch (Exception ex)
                        {
                            // push exception to database
                            _objLoggingProvider.Error("Error while reading sheet into datatable", ex);
                            return dt;
                        }
                    }
                }
                else
                {
                    return null;
                }
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }



        /// <summary>
        /// CompareColumns
        /// </summary>
        /// <param name="excelCols"></param>
        /// <param name="lstTemplateDataStructure"></param>
        /// <returns></returns>
        private bool CompareColumns(List<string> excelCols, List<TemplateDataStructure> lstTemplateDataStructure)
        {
            try
            {
                bool diff = false;

                var dbCols = (from n in lstTemplateDataStructure orderby n.SequenceNo
                              select n.FieldName.Trim().ToUpper()).ToList();

                    diff = dbCols.SequenceEqual(excelCols);
                    if (diff)
                    {
                        // both matched ... continue further

                        return true;
                    }
                    else
                    {
                        return false;
                    }
            }
            catch (Exception ex)
            {
                _objLoggingProvider.Error("Error while CompareColumns", ex);
                throw;
            }

        }


        /// <summary>
        /// MismatchedColumns
        /// </summary>
        /// <param name="excelCols"></param>
        /// <param name="TemplateName"></param>
        /// <param name="lstTemplateDataStructure"></param>
        /// <returns></returns>
        private string MismatchedColumns(List<string> excelCols, string TemplateName, List<TemplateDataStructure> lstTemplateDataStructure)
        {
            try
            {
                
                    var dbCols = (from n in lstTemplateDataStructure
                                  orderby n.SequenceNo
                                  select n.FieldName).ToList();

                    var result = new StringBuilder();
                    result.Append("<div class=\"panel-heading\">");
                    result.Append("<h3 class=\"panel-title\">Mismatched columns for " + TemplateName + "</h3>");
                    result.Append("</div>");
                    result.Append("<div class=\"panel-body\">");
                    result.Append("<table class=\"table table-striped\">");
                    result.Append("<tr><th>Database Columns</th><th>Excel Columns</th></tr>");
                    if (dbCols.Count > excelCols.Count)
                    {
                        for (var i = 0; i < dbCols.Count; i++)
                        {
                            result.Append("<tr>");
                            result.Append("<td>");
                            result.Append(dbCols[i].ToString());
                            result.Append("</td>");
                            result.Append("<td>");
                            if (excelCols.Count <= i)
                                result.Append("");
                            else
                                result.Append(excelCols[i].ToString());
                            result.Append("</td>");
                            result.Append("</tr>");
                        }
                    }
                    else
                    {
                        for (var i = 0; i < excelCols.Count; i++)
                        {
                            result.Append("<tr>");
                            result.Append("<td>");
                            if (dbCols.Count <= i)
                                result.Append("");
                            else
                                result.Append(dbCols[i].ToString());
                            result.Append("</td>");
                            result.Append("<td>");
                            result.Append(excelCols[i].ToString());
                            result.Append("</td>");
                            result.Append("</tr>");
                        }
                    }
                    result.Append("</table>");
                    result.Append("</div>");

                    return result.ToString();
                




            }
            catch (Exception ex)
            {
                _objLoggingProvider.Error("Error while MismatchedColumns", ex);
                throw;

            }
        }





        /// <summary>
        /// Dispose
        /// </summary>

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}