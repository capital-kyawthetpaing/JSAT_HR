using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using JH_DL;
using JH_Model;
using NPOI.XSSF.UserModel;
using NPOI.XSSF.Model;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Model;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.Web;
using Leave_BL;

namespace Attendance_BL
{
    public class AttendanceBL
    {
        public DataTable AttendanceData(string excelfile,string officeid)
        {
            try
            {

                string filename = excelfile;
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
                    DataSet AttendanceDataSet = new DataSet();
                    AttendanceDataSet.Locale = CultureInfo.InvariantCulture;
                    DataTable table = AttendanceDataSet.Tables.Add("AttendanceData");

                    table.Columns.Add("YYYYMM", typeof(string));
                    table.Columns.Add("DD", typeof(string));
                    table.Columns.Add("FingerPrintID", typeof(string));
                    table.Columns.Add("OfficeCD", typeof(string));
                    table.Columns.Add("AttandenceDate", typeof(string));
                    table.Columns.Add("TimeIn", typeof(string));
                    table.Columns.Add("TimeOut", typeof(string));
                    if (sheet.SheetName == "Att.log report")
                    {
                        //body
                        string attdate = string.Empty;
                        string newattdate = string.Empty;
                        string yyymm = string.Empty;
                        string dd = string.Empty;


                        for (int j = sheet.FirstRowNum + 2; j <= 2; j++)
                        {
                            var row = sheet.GetRow(j);
                            attdate = GetCellValue(row.GetCell(2));
                            string[] lines = Regex.Split(attdate, "-");
                            newattdate = lines[0] + "-" + lines[1];
                            yyymm = lines[0] + lines[1];
                            dd = lines[4];

                        }
                        for (int j = sheet.FirstRowNum + 4; j <= Convert.ToUInt32(rowCount); j = j + 2)
                        {
                            DataRow AttendanceDataRow;
                            var row = sheet.GetRow(j);

                            for (int i = row.FirstCellNum + 1; i <= Convert.ToUInt32(dd); i++)
                            {
                                AttendanceDataRow = table.NewRow();

                                AttendanceDataRow["AttandenceDate"] = newattdate + "-" + i;

                                AttendanceDataRow["DD"] = i;

                                AttendanceDataRow["YYYYMM"] = yyymm;

                                AttendanceDataRow["OfficeCD"] = officeid;

                                if (j % 2 == 0)
                                {
                                    var row1 = sheet.GetRow(j);
                                    AttendanceDataRow["FingerPrintID"] = GetCellValue(row1.GetCell(2));

                                    var row2 = sheet.GetRow(j + 1);
                                    if (GetCellValue(row2.GetCell(i - 1)).ToString() != "")
                                    {
                                        AttendanceDataRow["TimeIn"] = GetCellValue(row2.GetCell(i - 1)).Substring(0, 5);
                                    }
                                    //else
                                    //{
                                    //    AttendanceDataRow["TimeIn"] = GetCellValue(row2.GetCell(i - 1));
                                    //}

                                    if (GetCellValue(row2.GetCell(i - 1)).ToString() != "")
                                    {
                                        AttendanceDataRow["TimeOut"] = GetCellValue(row2.GetCell(i - 1)).Substring(GetCellValue(row2.GetCell(i - 1)).Length - 5, 5);
                                    }
                                    //else
                                    //{
                                    //    AttendanceDataRow["TimeOut"] = GetCellValue(row2.GetCell(i - 1));
                                    //}
                                }
                                else
                                {
                                    var row1 = sheet.GetRow(j - 1);
                                    AttendanceDataRow["FingerPrintID"] = GetCellValue(row1.GetCell(2));

                                    var row2 = sheet.GetRow(j);
                                    if (GetCellValue(row2.GetCell(i - 1)).ToString() != "")
                                    {
                                        AttendanceDataRow["TimeIn"] = GetCellValue(row2.GetCell(i - 1)).Substring(0, 5); ;
                                    }
                                    //else
                                    //{
                                    //    AttendanceDataRow["TimeIn"] = GetCellValue(row2.GetCell(i - 1));
                                    //}
                                    if (GetCellValue(row2.GetCell(i - 1)).ToString() != "")
                                    {
                                        AttendanceDataRow["TimeOut"] = GetCellValue(row2.GetCell(i - 1)).Substring(GetCellValue(row2.GetCell(i - 1)).Length - 5, 5);                        
                                    }
                                    //else
                                    //{
                                    //    AttendanceDataRow["TimeOut"] = GetCellValue(row2.GetCell(i - 1));
                                    //}
                                }

                                table.Rows.Add(AttendanceDataRow);
                            }

                        }
                    }

                    return AttendanceDataSet.Tables[0];
                }
                else
                {
                    DataSet AttendanceDataSet = new DataSet();
                    AttendanceDataSet.Locale = CultureInfo.InvariantCulture;
                    DataTable table = AttendanceDataSet.Tables.Add("AttendanceData");

                    table.Columns.Add("YYYYMM", typeof(string));
                    table.Columns.Add("DD", typeof(string));
                    table.Columns.Add("FingerPrintID", typeof(string));
                    table.Columns.Add("OfficeCD", typeof(string));
                    table.Columns.Add("AttandenceDate", typeof(string));
                    table.Columns.Add("TimeIn", typeof(string));
                    table.Columns.Add("TimeOut", typeof(string));
                    return AttendanceDataSet.Tables[0];
                }
            }
            catch(Exception ex)
            {
                string error = ex.ToString();
                return null;
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

        public void Insert_Attendance_Data(DataTable dttest, string filename,string id)
        {
            DataTable dt = new DataTable();
            BaseDL bdl = new BaseDL();
            SqlParameter[] prms = new SqlParameter[4];
            if (!string.IsNullOrWhiteSpace(filename))
            {
                prms[0] = new SqlParameter("@FileName", SqlDbType.VarChar) { Value = filename };
            }
            else
            {
                prms[0] = new SqlParameter("@FileName", SqlDbType.VarChar) { Value = System.DBNull.Value };
            }


            prms[1] = new SqlParameter("@ImportType", SqlDbType.VarChar) { Value = id };
            prms[2] = new SqlParameter("@ImportedBy", SqlDbType.VarChar) { Value = HttpContext.Current.Session["UserID"].ToString() };


            dttest.TableName = "M_Attendance";
            System.IO.StringWriter writer = new System.IO.StringWriter();
            dttest.WriteXml(writer, XmlWriteMode.WriteSchema, false);
            string result = writer.ToString();
            prms[3] = new SqlParameter("@xml", SqlDbType.Xml) { Value = result };
            bdl.InsertUpdateDeleteData("M_Attendance_Insert", prms);
        }

        public DataTable M_Attendance_Select(AttendanceModel am)
        {
            BaseDL bdl = new BaseDL();
            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@YYYYMM", SqlDbType.Int) { Value = am.YYYYMM };
            prms[1] = new SqlParameter("@StaffID", SqlDbType.VarChar) { Value = am.StaffID };

            DataTable dtAttendance = new DataTable();
            dtAttendance = bdl.SelectData("M_Attendance_Select", prms);
        
            return dtAttendance;
        }
       
        public DataTable GetLeave(string id)
        {
            LeaveBL lbl = new LeaveBL();
            DataTable dtleave = new DataTable();
            dtleave = lbl.M_Leave_Select(id);
            return dtleave;
        }

        public DataTable Get_Import_List_View()
        {
            BaseDL bdl = new BaseDL();
            DataTable dtImportList = new DataTable();
            dtImportList = bdl.SelectData("L_Import_Select", null);
            return dtImportList;
        }

        public DataTable Get_Attendance_List(string id)
        {
            BaseDL bdl = new BaseDL();
            DataTable dtImportList = new DataTable();
            string[] arr;
            string YYYMM = string.Empty;
            string StaffID = string.Empty;
            if (id != null)
            {
                arr = id.Split('_');
                YYYMM = arr[0]+arr[1];
                StaffID = arr[2];

            }

            SqlParameter[] prm = new SqlParameter[2];
            prm[0] = new SqlParameter("@YYYMM", SqlDbType.Int) { Value = YYYMM };

            prm[1] = new SqlParameter("@StaffID", SqlDbType.VarChar) { Value = StaffID };

            dtImportList = bdl.SelectData("Attendance_Setting_Select", prm);
            return dtImportList;
        }

        public DataTable ExcelToTable(string filename)
        {
            FileStream excelStream = new FileStream(filename, FileMode.Open);
            var book = new XSSFWorkbook(excelStream);
            excelStream.Close();

            var sheet = book.GetSheetAt(0);
            var headerRow = sheet.GetRow(0);//第一行为标题行
            var cellCount = headerRow.LastCellNum;//LastCellNum = PhysicalNumberOfCells
            var rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1

            DataSet IncomeTaxDataSet = new DataSet();
            IncomeTaxDataSet.Locale = CultureInfo.InvariantCulture;
            DataTable table = IncomeTaxDataSet.Tables.Add("IncomeTaxData");

            //header
            table.Columns.Add("StaffID", typeof(string));
            table.Columns.Add("StaffName", typeof(string));
            table.Columns.Add("IncomeTax", typeof(string));

            //body
            for (var i = sheet.FirstRowNum + 1; i <= rowCount; i++)
            {
                var row = sheet.GetRow(i);
                var dataRow = table.NewRow();
                if (row != null)
                {
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                            dataRow["StaffID"] = GetCellValue(row.GetCell(0));

                        if (row.GetCell(j) != null)
                            dataRow["StaffName"] = GetCellValue(row.GetCell(1));

                        if(row.GetCell(j)!=null)
                            dataRow["IncomeTax"] = GetCellValue(row.GetCell(2));

                    }
                }
                table.Rows.Add(dataRow);
            }

            return IncomeTaxDataSet.Tables[0];
        }

        public Boolean CheckColumn(string[] colName, DataTable dt)
        {
            try
            {
                DataColumnCollection col = dt.Columns;
                for (int i = 0; i < colName.Length; i++)
                {
                    if (!col.Contains(colName[i]))
                        return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return false;
            }
        }
        public void Insert_IncomeTax_Data(DataTable dttest,string YYYMM, string filename)
        {
            DataTable dt = new DataTable();
            BaseDL bdl = new BaseDL();
            SqlParameter[] prms = new SqlParameter[5];
            if (!string.IsNullOrWhiteSpace(filename))
            {
                prms[0] = new SqlParameter("@FileName", SqlDbType.VarChar) { Value = filename };
            }
            else
            {
                prms[0] = new SqlParameter("@FileName", SqlDbType.VarChar) { Value = System.DBNull.Value };
            }
            if (!string.IsNullOrWhiteSpace(YYYMM))
            {
                prms[1] = new SqlParameter("@YYYMM", SqlDbType.VarChar) { Value = YYYMM };
            }
            else
            {
                prms[1] = new SqlParameter("@YYYMM", SqlDbType.VarChar) { Value = System.DBNull.Value };
            }

            prms[2] = new SqlParameter("@ImportType", SqlDbType.VarChar) { Value = 3 };
            prms[3] = new SqlParameter("@ImportedBy", SqlDbType.VarChar) { Value = HttpContext.Current.Session["UserID"].ToString() };


            dttest.TableName = "Payroll_Deduction";
            System.IO.StringWriter writer = new System.IO.StringWriter();
            dttest.WriteXml(writer, XmlWriteMode.WriteSchema, false);
            string result = writer.ToString();
            prms[4] = new SqlParameter("@xml", SqlDbType.Xml) { Value = result };
            bdl.InsertUpdateDeleteData("Payroll_Deduction_IncomeTax_Insert", prms);
        }

        public void Update_Attendance_Data(DataTable dtattendance, MultiModel mModel)
        {
            BaseDL dl = new BaseDL();
            try
            {
                string YYYMM = string.Empty;
                YYYMM = mModel.attModel.YYYY + mModel.attModel.MM;
                SqlParameter[] prms = new SqlParameter[3];
                if (!string.IsNullOrWhiteSpace(mModel.attModel.StaffID))
                {
                    prms[0] = new SqlParameter("@StaffID", SqlDbType.Int) { Value = mModel.attModel.StaffID };
                }
                else
                {
                    prms[0] = new SqlParameter("@StaffID", SqlDbType.Int) { Value = System.DBNull.Value };
                }
                if (!string.IsNullOrWhiteSpace(YYYMM))
                {
                    prms[1] = new SqlParameter("@YYYMM", SqlDbType.Int) { Value=Convert.ToInt32(YYYMM) };
                }
                else
                {
                    prms[1] = new SqlParameter("@YYYMM", SqlDbType.Int) { Value=System.DBNull.Value };
                }

                dtattendance.TableName = "attendance";
                System.IO.StringWriter writer = new System.IO.StringWriter();
                dtattendance.WriteXml(writer, XmlWriteMode.WriteSchema, false);
                string result = writer.ToString();
                prms[2] = new SqlParameter("@xml", SqlDbType.Xml) { Value = result };
                dl.InsertUpdateDeleteData("M_Attendance_Update", prms);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update_Attendance_List(DataTable dtAttlist, MultiModel mModel)
        {
            BaseDL dl = new BaseDL();
            try
            {
                string YYYMM = string.Empty;
                YYYMM = mModel.attModel.YYYY + mModel.attModel.MM;
                SqlParameter[] prms = new SqlParameter[3];
                if (!string.IsNullOrWhiteSpace(mModel.attModel.StaffID))
                {
                    prms[0] = new SqlParameter("@StaffID", SqlDbType.Int) { Value = mModel.attModel.StaffID };
                }
                else
                {
                    prms[0] = new SqlParameter("@StaffID", SqlDbType.Int) { Value = System.DBNull.Value };
                }
                if (!string.IsNullOrWhiteSpace(YYYMM))
                {
                    prms[1] = new SqlParameter("@YYYMM", SqlDbType.Int) { Value = Convert.ToInt32(YYYMM) };
                }
                else
                {
                    prms[1] = new SqlParameter("@YYYMM", SqlDbType.Int) { Value = System.DBNull.Value };
                }

                dtAttlist.TableName = "attendance";
                System.IO.StringWriter writer = new System.IO.StringWriter();
                dtAttlist.WriteXml(writer, XmlWriteMode.WriteSchema, false);
                string result = writer.ToString();
                prms[2] = new SqlParameter("@xml", SqlDbType.Xml) { Value = result };
                dl.InsertUpdateDeleteData("Attendance_List_Update", prms);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void QuickAttendance_Update(string staffid, string date)
        {
            BaseDL bdl = new BaseDL();
            string result = string.Empty;
            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@staffid", SqlDbType.VarChar) { Value = staffid };
            prms[1] = new SqlParameter("@date", SqlDbType.VarChar) { Value = date };
            bdl.InsertUpdateDeleteData("QuickAttendance_Update", prms);
        }

    }
}
