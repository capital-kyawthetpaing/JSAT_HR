using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using NPOI.XSSF.UserModel;
using NPOI.XSSF.Model;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Model;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using Attendance_BL;
using System.Configuration;
using Newtonsoft.Json;

namespace JSAT_HR.Controllers
{
    public class AttendanceController : Controller
    {
        string AttendanceFile = ConfigurationManager.AppSettings["AttendanceFile"].ToString();

        // GET: Attendance
        public ActionResult AttendanceImport()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AttendanceDataSave(HttpPostedFileBase uploadFile, JH_Model.M_AttandenceModel model)
        {
            try
            {
                if (uploadFile != null)
                {
                    string filename = string.Empty;
                    AttendanceBL abl = new AttendanceBL();
                    System.Data.DataTable dt = new System.Data.DataTable();

                    filename = uploadFile.FileName;
                    if (!Directory.Exists(AttendanceFile))
                    {
                        Directory.CreateDirectory(AttendanceFile);
                    }
                    if (filename.Contains(".xlsx"))
                    {
                        filename = filename.Replace(".xlsx", "");
                        filename = filename + "$" + DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.ToString("HHmmss") + ".xlsx";
                    }
                    uploadFile.SaveAs(AttendanceFile + filename);

                    dt = AttendanceData(AttendanceFile + filename);
                    model.FileName = uploadFile.FileName;
                    if (dt.Rows.Count > 0)
                    {
                        abl.Insert_Attendance_Data(dt, model);
                    }
                }

            }

            catch (Exception ex) { string error = ex.ToString(); }

            return RedirectToAction("AttendanceImport");
        }

        public DataTable AttendanceData(string excelfile)
        {
            string filename =excelfile;
            FileStream excelStream = new FileStream(filename, FileMode.Open);
            var book = new XSSFWorkbook(excelStream);
            excelStream.Close();

            var sheetcount = book.NumberOfSheets;
            if (Convert.ToInt32(sheetcount) >= 3)
            {

                var sheet = book.GetSheetAt(2);
                var headerRow = sheet.GetRow(2);
                var cellCount = headerRow.LastCellNum;
                var rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1

                //header
                DataSet sampleDataSet = new DataSet();
                sampleDataSet.Locale = CultureInfo.InvariantCulture;
                DataTable table = sampleDataSet.Tables.Add("SampleData");

                table.Columns.Add("YYYYMM", typeof(string));
                table.Columns.Add("FingerPrintID", typeof(string));
                table.Columns.Add("StaffType", typeof(string));
                table.Columns.Add("AttandenceDate", typeof(string));
                table.Columns.Add("TimeIn", typeof(string));
                table.Columns.Add("TimeOut", typeof(string));
                if (sheet.SheetName == "Att.log report")
                {
                    //body
                    string attdate = string.Empty;
                    string newattdate = string.Empty;
                    string yyymm = string.Empty;
                    for (int j = sheet.FirstRowNum + 2; j <= 2; j++)
                    {
                        var row = sheet.GetRow(j);
                        attdate = GetCellValue(row.GetCell(2));
                        string[] lines = Regex.Split(attdate, "-");
                        newattdate = lines[0] + "-" + lines[1];
                        yyymm = lines[0] + lines[1];
                    }
                    for (int j = sheet.FirstRowNum + 4; j <= Convert.ToUInt32(rowCount); j = j + 2)
                    {
                        DataRow sampleDataRow;
                        var row = sheet.GetRow(j);

                        for (int i = row.FirstCellNum + 1; i <= Convert.ToUInt32(cellCount); i++)
                        {
                            sampleDataRow = table.NewRow();

                            sampleDataRow["AttandenceDate"] = newattdate + "-" + i;

                            sampleDataRow["YYYYMM"] = yyymm;

                            sampleDataRow["StaffType"] = 1;

                            if (j % 2 == 0)
                            {
                                var row1 = sheet.GetRow(j);
                                sampleDataRow["FingerPrintID"] = GetCellValue(row1.GetCell(2));

                                var row2 = sheet.GetRow(j + 1);
                                if (GetCellValue(row2.GetCell(i - 1)).ToString() != "")
                                {
                                    sampleDataRow["TimeIn"] = GetCellValue(row2.GetCell(i - 1)).Substring(0, 5);
                                }
                                else
                                {
                                    sampleDataRow["TimeIn"] = GetCellValue(row2.GetCell(i - 1));
                                }

                                if (GetCellValue(row2.GetCell(i - 1)).ToString() != "")
                                {
                                    sampleDataRow["TimeOut"] = GetCellValue(row2.GetCell(i - 1)).Substring(GetCellValue(row2.GetCell(i - 1)).Length - 5, 5);
                                }
                                else
                                {
                                    sampleDataRow["TimeIn"] = GetCellValue(row2.GetCell(i - 1));
                                }
                            }
                            else
                            {
                                var row1 = sheet.GetRow(j - 1);
                                sampleDataRow["FingerPrintID"] = GetCellValue(row1.GetCell(2));

                                var row2 = sheet.GetRow(j);
                                if (GetCellValue(row2.GetCell(i - 1)).ToString() != "")
                                {
                                    sampleDataRow["TimeIn"] = GetCellValue(row2.GetCell(i - 1)).Substring(0, 5); ;
                                }
                                else
                                {
                                    sampleDataRow["TimeIn"] = GetCellValue(row2.GetCell(i - 1));
                                }
                                if (GetCellValue(row2.GetCell(i - 1)).ToString() != "")
                                {
                                    sampleDataRow["TimeOut"] = GetCellValue(row2.GetCell(i - 1));
                                }
                                else
                                {
                                    sampleDataRow["TimeOut"] = GetCellValue(row2.GetCell(i - 1)).Substring(GetCellValue(row2.GetCell(i - 1)).Length - 5, 5);
                                }
                            }

                            table.Rows.Add(sampleDataRow);
                        }

                    }
                }

                return sampleDataSet.Tables[0];
            }
            else
            {
                DataSet sampleDataSet = new DataSet();
                sampleDataSet.Locale = CultureInfo.InvariantCulture;
                DataTable table = sampleDataSet.Tables.Add("SampleData");

                table.Columns.Add("YYYYMM", typeof(string));
                table.Columns.Add("FingerPrintID", typeof(string));
                table.Columns.Add("StaffType", typeof(string));
                table.Columns.Add("AttandenceDate", typeof(string));
                table.Columns.Add("TimeIn", typeof(string));
                table.Columns.Add("TimeOut", typeof(string));
                return sampleDataSet.Tables[0];
            }
        }

        private static string GetCellValue(ICell cell)
        {


            if (cell == null)
                return string.Empty;
            switch (cell.CellType)
            {
                case CellType.Blank:
                    return string.Empty;
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();
                case CellType.Error:
                    return cell.ErrorCellValue.ToString();
                case CellType.Numeric:
                case CellType.Unknown:
                default:
                    return cell.ToString();//This is a trick to get the correct value of the cell. NumericCellValue will return a numeric value no matter the cell value is a date or a number
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Formula:
                    try
                    {
                        var e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                        e.EvaluateInCell(cell);
                        return cell.ToString();
                    }
                    catch
                    {
                        return cell.NumericCellValue.ToString();
                    }
            }
        }

        public ActionResult Import_Log_List()
        {
            return View();
        }
        public string Bind_Import_List()
        {
            DataTable dt = new DataTable();
            string jsonresult;
            AttendanceBL atbl = new AttendanceBL();
            dt = atbl.Get_Import_List_View();
            jsonresult = DataTableToJSONWithJSONNet(dt);
            return jsonresult;
        }
        public string DataTableToJSONWithJSONNet(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }
    }
}