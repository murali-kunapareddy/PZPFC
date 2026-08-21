using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using PFCWebAPP.Repositories.Common.Interfaces;
using PFCWebAPP.Repositories.Common.ServiceProviders;
using System.Data;
using System.Drawing;

namespace PFCWebAPP.Utilities
{
    public static class Common
    {
        private static Dictionary<string, string> _lstMasterCountryList;
        private static List<CountryCodeModel> _lstPFCCountryCode;
        private static List<SalesOrganisationModel> _lstSalesOrganisation;


        /// <summary>
        /// lstMasterCountryList
        /// </summary>
        private static Dictionary<string, string> lstMasterCountryList
        {
            get
            {
                return _lstMasterCountryList;
            }
            set
            {
                _lstMasterCountryList = value;
            }
        }


        /// <summary>
        /// lstPFCCountryCode
        /// </summary>
        private static List<CountryCodeModel> lstPFCCountryCode
        {
            get
            {
                return _lstPFCCountryCode;
            }
            set
            {
                _lstPFCCountryCode = value;
            }
        }


        /// <summary>
        /// lstSalesOrganisation
        /// </summary>
        public static List<SalesOrganisationModel> lstSalesOrganisation
        {
            get
            {
                return _lstSalesOrganisation;
            }
            set
            {
                _lstSalesOrganisation = value;
            }
        }


        /// <summary>
        /// MasterCountryList
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> MasterCountryList()
        {
            try
            {
                if (Common.lstMasterCountryList != null)
                {
                    return Common.lstMasterCountryList;
                }
                var MasterList = new Dictionary<string, string>();

                MasterList.Add("AF", "Afghanistan");
                MasterList.Add("AX", "Aland Islands");
                MasterList.Add("AL", "Albania");
                MasterList.Add("DZ", "Algeria");
                MasterList.Add("AS", "American Samoa");
                MasterList.Add("AD", "Andorra");
                MasterList.Add("AO", "Angola");
                MasterList.Add("AI", "Anguilla");
                MasterList.Add("AQ", "Antarctica");
                MasterList.Add("AG", "Antigua and Barbuda");
                MasterList.Add("AR", "Argentina");
                MasterList.Add("AM", "Armenia");
                MasterList.Add("AW", "Aruba");
                MasterList.Add("AU", "Australia");
                MasterList.Add("AT", "Austria");
                MasterList.Add("AZ", "Azerbaijan");
                MasterList.Add("BS", "Bahamas");
                MasterList.Add("BH", "Bahrain");
                MasterList.Add("BD", "Bangladesh");
                MasterList.Add("BB", "Barbados");
                MasterList.Add("BY", "Belarus");
                MasterList.Add("BE", "Belgium");
                MasterList.Add("BZ", "Belize");
                MasterList.Add("BJ", "Benin");
                MasterList.Add("BM", "Bermuda");
                MasterList.Add("BT", "Bhutan");
                MasterList.Add("BO", "Bolivia");
                MasterList.Add("BA", "Bosnia and Herzegovina");
                MasterList.Add("BW", "Botswana");
                MasterList.Add("BV", "Bouvet Island");
                MasterList.Add("BR", "Brazil");
                MasterList.Add("VG", "British Virgin Islands");
                MasterList.Add("IO", "British Indian Ocean Territory");
                MasterList.Add("BN", "Brunei Darussalam");
                MasterList.Add("BG", "Bulgaria");
                MasterList.Add("BF", "Burkina Faso");
                MasterList.Add("BI", "Burundi");
                MasterList.Add("KH", "Cambodia");
                MasterList.Add("CM", "Cameroon");
                MasterList.Add("CA", "Canada");
                MasterList.Add("CV", "Cape Verde");
                MasterList.Add("KY", "Cayman Islands");
                MasterList.Add("CF", "Central African Republic");
                MasterList.Add("TD", "Chad");
                MasterList.Add("CL", "Chile");
                MasterList.Add("CN", "China");
                MasterList.Add("HK", "Hong Kong, SAR China");
                MasterList.Add("MO", "Macao, SAR China");
                MasterList.Add("CX", "Christmas Island");
                MasterList.Add("CC", "Cocos(Keeling) Islands");
                MasterList.Add("CO", "Colombia");
                MasterList.Add("KM", "Comoros");
                MasterList.Add("CG", "Congo(Brazzaville)");
                MasterList.Add("CD", "Congo, (Kinshasa)");
                MasterList.Add("CK", "Cook Islands");
                MasterList.Add("CR", "Costa Rica");
                MasterList.Add("CI", "Côte d'Ivoire");
                MasterList.Add("HR", "Croatia");
                MasterList.Add("CU", "Cuba");
                MasterList.Add("CY", "Cyprus");
                MasterList.Add("CZ", "Czech Republic");
                MasterList.Add("DK", "Denmark");
                MasterList.Add("DJ", "Djibouti");
                MasterList.Add("DM", "Dominica");
                MasterList.Add("DO", "Dominican Republic");
                MasterList.Add("EC", "Ecuador");
                MasterList.Add("EG", "Egypt");
                MasterList.Add("SV", "El Salvador");
                MasterList.Add("GQ", "Equatorial Guinea");
                MasterList.Add("ER", "Eritrea");
                MasterList.Add("EE", "Estonia");
                MasterList.Add("ET", "Ethiopia");
                MasterList.Add("FK", "Falkland Islands(Malvinas)");
                MasterList.Add("FO", "Faroe Islands");
                MasterList.Add("FJ", "Fiji");
                MasterList.Add("FI", "Finland");
                MasterList.Add("FR", "France");
                MasterList.Add("GF", "French Guiana");
                MasterList.Add("PF", "French Polynesia");
                MasterList.Add("TF", "French Southern Territories");
                MasterList.Add("GA", "Gabon");
                MasterList.Add("GM", "Gambia");
                MasterList.Add("GE", "Georgia");
                MasterList.Add("DE", "Germany");
                MasterList.Add("GH", "Ghana");
                MasterList.Add("GI", "Gibraltar");
                MasterList.Add("GR", "Greece");
                MasterList.Add("GL", "Greenland");
                MasterList.Add("GD", "Grenada");
                MasterList.Add("GP", "Guadeloupe");
                MasterList.Add("GU", "Guam");
                MasterList.Add("GT", "Guatemala");
                MasterList.Add("GG", "Guernsey");
                MasterList.Add("GN", "Guinea");
                MasterList.Add("GW", "Guinea - Bissau");
                MasterList.Add("GY", "Guyana");
                MasterList.Add("HT", "Haiti");
                MasterList.Add("HM", "Heard and Mcdonald Islands");
                MasterList.Add("VA", "Holy See(Vatican City State)");
                MasterList.Add("HN", "Honduras");
                MasterList.Add("HU", "Hungary");
                MasterList.Add("IS", "Iceland");
                MasterList.Add("IN", "India");
                MasterList.Add("ID", "Indonesia");
                MasterList.Add("IR", "Iran, Islamic Republic of");
                MasterList.Add("IQ", "Iraq");
                MasterList.Add("IE", "Ireland");
                MasterList.Add("IM", "Isle of Man");
                MasterList.Add("IL", "Israel");
                MasterList.Add("IT", "Italy");
                MasterList.Add("JM", "Jamaica");
                MasterList.Add("JP", "Japan");
                MasterList.Add("JE", "Jersey");
                MasterList.Add("JO", "Jordan");
                MasterList.Add("KZ", "Kazakhstan");
                MasterList.Add("KE", "Kenya");
                MasterList.Add("KI", "Kiribati");
                MasterList.Add("KP", "Korea(North)");
                MasterList.Add("KR", "Korea(South)");
                MasterList.Add("KW", "Kuwait");
                MasterList.Add("KG", "Kyrgyzstan");
                MasterList.Add("LA", "Lao PDR");
                MasterList.Add("LV", "Latvia");
                MasterList.Add("LB", "Lebanon");
                MasterList.Add("LS", "Lesotho");
                MasterList.Add("LR", "Liberia");
                MasterList.Add("LY", "Libya");
                MasterList.Add("LI", "Liechtenstein");
                MasterList.Add("LT", "Lithuania");
                MasterList.Add("LU", "Luxembourg");
                MasterList.Add("MK", "Macedonia, Republic of");
                MasterList.Add("MG", "Madagascar");
                MasterList.Add("MW", "Malawi");
                MasterList.Add("MY", "Malaysia");
                MasterList.Add("MV", "Maldives");
                MasterList.Add("ML", "Mali");
                MasterList.Add("MT", "Malta");
                MasterList.Add("MH", "Marshall Islands");
                MasterList.Add("MQ", "Martinique");
                MasterList.Add("MR", "Mauritania");
                MasterList.Add("MU", "Mauritius");
                MasterList.Add("YT", "Mayotte");
                MasterList.Add("MX", "Mexico");
                MasterList.Add("FM", "Micronesia, Federated States of");
                MasterList.Add("MD", "Moldova");
                MasterList.Add("MC", "Monaco");
                MasterList.Add("MN", "Mongolia");
                MasterList.Add("ME", "Montenegro");
                MasterList.Add("MS", "Montserrat");
                MasterList.Add("MA", "Morocco");
                MasterList.Add("MZ", "Mozambique");
                MasterList.Add("MM", "Myanmar");
                MasterList.Add("NA", "Namibia");
                MasterList.Add("NR", "Nauru");
                MasterList.Add("NP", "Nepal");
                MasterList.Add("NL", "Netherlands");
                MasterList.Add("AN", "Netherlands Antilles");
                MasterList.Add("NC", "New Caledonia");
                MasterList.Add("NZ", "New Zealand");
                MasterList.Add("NI", "Nicaragua");
                MasterList.Add("NE", "Niger");
                MasterList.Add("NG", "Nigeria");
                MasterList.Add("NU", "Niue");
                MasterList.Add("NF", "Norfolk Island");
                MasterList.Add("MP", "Northern Mariana Islands");
                MasterList.Add("NO", "Norway");
                MasterList.Add("OM", "Oman");
                MasterList.Add("PK", "Pakistan");
                MasterList.Add("PW", "Palau");
                MasterList.Add("PS", "Palestinian Territory");
                MasterList.Add("PA", "Panama");
                MasterList.Add("PG", "Papua New Guinea");
                MasterList.Add("PY", "Paraguay");
                MasterList.Add("PE", "Peru");
                MasterList.Add("PH", "Philippines");
                MasterList.Add("PN", "Pitcairn");
                MasterList.Add("PL", "Poland");
                MasterList.Add("PT", "Portugal");
                MasterList.Add("PR", "Puerto Rico");
                MasterList.Add("QA", "Qatar");
                MasterList.Add("RE", "Réunion");
                MasterList.Add("RO", "Romania");
                MasterList.Add("RU", "Russian Federation");
                MasterList.Add("RW", "Rwanda");
                MasterList.Add("BL", "Saint - Barthélemy");
                MasterList.Add("SH", "Saint Helena");
                MasterList.Add("KN", "Saint Kitts and Nevis");
                MasterList.Add("LC", "Saint Lucia");
                MasterList.Add("MF", "Saint - Martin(French part)");
                MasterList.Add("PM", "Saint Pierre and Miquelon");
                MasterList.Add("VC", "Saint Vincent and Grenadines");
                MasterList.Add("WS", "Samoa");
                MasterList.Add("SM", "San Marino");
                MasterList.Add("ST", "Sao Tome and Principe");
                MasterList.Add("SA", "Saudi Arabia");
                MasterList.Add("SN", "Senegal");
                MasterList.Add("RS", "Serbia");
                MasterList.Add("SC", "Seychelles");
                MasterList.Add("SL", "Sierra Leone");
                MasterList.Add("SG", "Singapore");
                MasterList.Add("SK", "Slovakia");
                MasterList.Add("SI", "Slovenia");
                MasterList.Add("SB", "Solomon Islands");
                MasterList.Add("SO", "Somalia");
                MasterList.Add("ZA", "South Africa");
                MasterList.Add("GS", "South Georgia and the South Sandwich Islands");
                MasterList.Add("SS", "South Sudan");
                MasterList.Add("ES", "Spain");
                MasterList.Add("LK", "Sri Lanka");
                MasterList.Add("SD", "Sudan");
                MasterList.Add("SR", "Suriname");
                MasterList.Add("SJ", "Svalbard and Jan Mayen Islands");
                MasterList.Add("SZ", "Swaziland");
                MasterList.Add("SE", "Sweden");
                MasterList.Add("CH", "Switzerland");
                MasterList.Add("SY", "Syrian Arab Republic(Syria)");
                MasterList.Add("TW", "Taiwan, Republic of China");
                MasterList.Add("TJ", "Tajikistan");
                MasterList.Add("TZ", "Tanzania, United Republic of");
                MasterList.Add("TH", "Thailand");
                MasterList.Add("TL", "Timor - Leste");
                MasterList.Add("TG", "Togo");
                MasterList.Add("TK", "Tokelau");
                MasterList.Add("TO", "Tonga");
                MasterList.Add("TT", "Trinidad and Tobago");
                MasterList.Add("TN", "Tunisia");
                MasterList.Add("TR", "Turkey");
                MasterList.Add("TM", "Turkmenistan");
                MasterList.Add("TC", "Turks and Caicos Islands");
                MasterList.Add("TV", "Tuvalu");
                MasterList.Add("UG", "Uganda");
                MasterList.Add("UA", "Ukraine");
                MasterList.Add("AE", "United Arab Emirates");
                MasterList.Add("GB", "United Kingdom");
                MasterList.Add("US", "United States of America");
                MasterList.Add("UN", "UnKnown");
                MasterList.Add("UM", "US Minor Outlying Islands");
                MasterList.Add("UY", "Uruguay");
                MasterList.Add("UZ", "Uzbekistan");
                MasterList.Add("VU", "Vanuatu");
                MasterList.Add("VE", "Venezuela(Bolivarian Republic)");
                MasterList.Add("VN", "Viet Nam");
                MasterList.Add("VI", "Virgin Islands, US");
                MasterList.Add("WF", "Wallis and Futuna Islands");
                MasterList.Add("EH", "Western Sahara");
                MasterList.Add("YE", "Yemen");
                MasterList.Add("ZM", "Zambia");
                MasterList.Add("ZW", "Zimbabwe");

                Common.lstMasterCountryList = MasterList;

                return MasterList;
            }
            catch(Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// PFCCountryCode
        /// </summary>
        /// <returns></returns>
        public static List<CountryCodeModel> PFCCountryCode()
        {
            try
            {
                if(Common.lstPFCCountryCode != null)
                {
                    return Common.lstPFCCountryCode;
                }
                var MasterList = new Dictionary<string, string>();
                List<CountryCodeModel> lstCCM = new List<CountryCodeModel>();
                CountryCodeModel CCM = new CountryCodeModel();

                lstCCM.Add(new CountryCodeModel { CountryCode = "AU" });
                lstCCM.Add(new CountryCodeModel { CountryCode = "NZ" });

                Common.lstPFCCountryCode = lstCCM;

                return lstCCM;
            }
            catch(Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// SalesOrganisations
        /// </summary>
        /// <returns></returns>
        public static List<SalesOrganisationModel> SalesOrganisations()
        {
            try
            {
                if (Common.lstSalesOrganisation != null)
                {
                    return Common.lstSalesOrganisation;
                }
                var MasterList = new Dictionary<string, string>();
                List<SalesOrganisationModel> lstSOM = new List<SalesOrganisationModel>();
                SalesOrganisationModel SOM = new SalesOrganisationModel();

                lstSOM.Add(new SalesOrganisationModel { SalesOrganisationCode = "AU01" });
                lstSOM.Add(new SalesOrganisationModel { SalesOrganisationCode = "NZ01" });

                Common.lstSalesOrganisation = lstSOM;

                return lstSOM;
            }
            catch (Exception)
            {
                throw;
            }
        }




        /// <summary>
        ///  ConvertDataTableDataInfoExcel
        /// </summary>
        /// <param name="SheetName"></param>
        /// <param name="DT"></param>
        /// <param name="fname"></param>
        /// <returns></returns>

        

        public static bool ConvertDataTableDataInfoExcel(string SheetName, DataTable DT, string fname)
        {
            try
            {
                if (DT != null && DT.Rows.Count > 0)
                {
                    if (!Directory.Exists(PFCWebAPP.Utilities.AppConfig.TemplateFilepath))
                        Directory.CreateDirectory(PFCWebAPP.Utilities.AppConfig.TemplateFilepath);

                    using (IWorkbook excelFile = new XSSFWorkbook())
                    {
                        ISheet sheet1 = (ISheet)excelFile.CreateSheet(SheetName);
                        using (FileStream file = new FileStream(fname, FileMode.Create, FileAccess.ReadWrite))
                        {
                            //make a header row 
                            NPOI.SS.UserModel.IRow rowOne = sheet1.CreateRow(0);

                            var font = excelFile.CreateFont();
                            font.FontHeightInPoints = 11;
                            font.FontName = "Calibri";
                            font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;


                            for (int j = 0; j < DT.Columns.Count; j++)
                            {
                                NPOI.SS.UserModel.ICell cell = rowOne.CreateCell(j);
                                string columnName = DT.Columns[j].ToString();
                                cell.SetCellValue(columnName);
                                cell.CellStyle = excelFile.CreateCellStyle();
                                cell.CellStyle.SetFont(font);
                                sheet1.AutoSizeColumn(j);

                                int headerTextLength = columnName.Length;
                                double requiredWidth = (headerTextLength + 1) * 256;
                                int autoFitWidth = sheet1.GetColumnWidth(j);
                                int finalWidth = (int)Math.Max(requiredWidth, autoFitWidth);

                                //sheet1.SetColumnWidth(j, sheet1.GetColumnWidth(j) + 2);
                                sheet1.SetColumnWidth(j, finalWidth);
                                // sheet1.AutoSizeRow(j);
                                //GC.Collect(); // due to NPOI (2.0.1) is not disposing the BitMap objects
                            }
                            sheet1.CreateFreezePane(0, 1, 0, 1);


                            int i = 0;
                            foreach (DataRow rr in DT.Rows)
                            {
                                NPOI.SS.UserModel.IRow row = sheet1.CreateRow(i + 1);
                                for (int j = 0; j < DT.Columns.Count; j++)
                                {
                                    NPOI.SS.UserModel.ICell cell = row.CreateCell(j);
                                    string columnName = DT.Columns[j].ToString();
                                    cell.SetCellValue(rr[columnName].ToString());
                                }
                                i++;
                            }
                            excelFile.Write(file, true);
                            file.Close();
                        }

                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return true;
        }

        /// <summary>
        /// CountryCodeModel
        /// </summary>
        public class CountryCodeModel
        {
            public string CountryCode { get; set;}
        }

        /// <summary>
        /// SalesOrganisationModel
        /// </summary>
        public class SalesOrganisationModel
        {
            public string SalesOrganisationCode { get; set; }
        }

        public static string RoundStringToDecimal(string input)
        {
            if(decimal.TryParse(input, out decimal number))
            {
                decimal roundedNumber = Math.Round(number,Convert.ToInt32(AppConfig.DecimalPosition));
                return roundedNumber.ToString("0.00");

            }
            else
            {
                return input;
            }
        }

        public static string RoundStringToDecimalNew(string input)
        {
            if (decimal.TryParse(input, out decimal number))
            {
                decimal roundedNumber = Math.Round(number, Convert.ToInt32(AppConfig.DecimalPosition));
                return roundedNumber.ToString("0.000");

            }
            else
            {
                return input;
            }
        }


    }
}
