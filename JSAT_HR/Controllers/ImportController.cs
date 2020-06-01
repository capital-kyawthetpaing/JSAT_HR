using Attendance_BL;
using CommonFunction;
using JH_Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JSAT_HR.Controllers
{
    public class ImportController : Controller
    {
        string AttendanceFile = ConfigurationManager.AppSettings["AttendanceFile"].ToString();
        string IncomeTaxFile = ConfigurationManager.AppSettings["IncomeTaxFile"].ToString();
        string HolidayFile = ConfigurationManager.AppSettings["HolidayFile"].ToString();
        // GET: Import
        public ActionResult Import_List()
        {
            String Imsg = Session["Imsg"] as string;
            String Emsg = Session["Emsg"] as string;
            ViewBag.Imsg = Imsg;
            ViewBag.Emsg = Emsg;
            Session["Imsg"] = "";
            Session["Emsg"] = "";
            PayrollModel pm = new PayrollModel();
            return View(pm);
        }

        [HttpGet]
        public string Get_Import_List()
        {
            DataTable dt = new DataTable();
            string jsonresult;
            AttendanceBL atbl = new AttendanceBL();
            Function fn = new Function();
            dt = atbl.Get_Import_List_View();
            jsonresult = fn.DataTableToJSONWithJSONNet(dt);
            return jsonresult;
        }

        [HttpPost]
        public string AttendanceDataSave(string id)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = null;
                    for (int i = 0; i < files.Count; i++)
                    {

                        file = files[i];
                    }
                    if (file != null)
                    {
                        string filename = string.Empty;
                        string newfilename = string.Empty;
                        filename = file.FileName;
                        string[] arr;
                        string OfficeCD = string.Empty;
                        if (id != null)
                        {
                            arr = id.Split('_');
                            OfficeCD = arr[0];

                        }
                        if (Convert.ToInt32(OfficeCD) == 1 || Convert.ToInt32(OfficeCD) == 2)
                        {

                            AttendanceBL abl = new AttendanceBL();
                            System.Data.DataTable dt = new System.Data.DataTable();

                            if (!Directory.Exists(AttendanceFile))
                            {
                                Directory.CreateDirectory(AttendanceFile);
                            }
                            if (filename.Contains(".xlsx"))
                            {
                                filename = filename.Replace(" ", "_").Replace(".xls", "");
                                filename = filename + "$" + DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.ToString("HHmmss") + ".xlsx";

                            }
                            file.SaveAs(AttendanceFile + filename);
                            //Workbook workbook1 = new Workbook();
                            //workbook1.LoadFromFile(AttendanceFile + filename,ExcelVersion.Version97to2003);
                            //filename = filename.Replace(".xls", "");
                            //newfilename = filename + ".xlsx";
                            //workbook1.SaveToFile(AttendanceFile + newfilename , ExcelVersion.Version2016);

                            dt = abl.AttendanceData(AttendanceFile + filename, id);
                            if (dt.Rows.Count > 0)
                            {
                                abl.Insert_Attendance_Data(dt, file.FileName, id);
                                Session["Imsg"] = "OK";
                            }
                        }
                        else
                        {
                            AttendanceBL abl = new AttendanceBL();
                            System.Data.DataTable dt = new System.Data.DataTable();

                            if (!Directory.Exists(IncomeTaxFile))
                            {
                                Directory.CreateDirectory(IncomeTaxFile);
                            }
                            if (filename.Contains(".xlsx"))
                            {
                                filename = filename.Replace(".xlsx", "");
                                filename = filename + "$" + DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.ToString("HHmmss") + ".xlsx";
                            }
                            file.SaveAs(IncomeTaxFile + filename);

                            dt = abl.ExcelToTable(IncomeTaxFile + filename);
                            String[] colName = { "StaffID", "StaffName", "IncomeTax" };
                            if (abl.CheckColumn(colName, dt))
                            {
                                string[] arr1;
                                string YYYMM = string.Empty;
                                string officeid = string.Empty;
                                if (id != null)
                                {
                                    arr1 = id.Split('_');
                                    officeid = arr1[0];
                                    YYYMM = arr1[1] + arr1[2];

                                }
                                if (dt.Rows.Count > 0)
                                {
                                    abl.Insert_IncomeTax_Data(dt, YYYMM, file.FileName);
                                    Session["Imsg"] = "OK";
                                }
                            }
                            else
                            {
                                Session["Emsg"] = "NotOK";
                            }

                        }
                    }

                }
            }


            catch (Exception ex) { string error = ex.ToString(); Session["Emsg"] = "NotOK"; }

            //return RedirectToAction("AttendanceImport");
            return JsonConvert.SerializeObject("OK");
        }

        public ActionResult Holiday_Import()
        {
            return View();
        }

        [HttpPost]
        public string HolidaySave(string id)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = null;
                    for (int i = 0; i < files.Count; i++)
                    {
                        file = files[i];
                    }
                    if (file != null)
                    {
                        string filename = string.Empty;
                        string newfilename = string.Empty;
                        filename = file.FileName;
                        AttendanceBL abl = new AttendanceBL();
                        System.Data.DataTable dt = new System.Data.DataTable();

                        if (!Directory.Exists(HolidayFile))
                        {
                            Directory.CreateDirectory(HolidayFile);
                        }
                        if (filename.Contains(".xlsx"))
                        {
                            filename = filename.Replace(".xlsx", "");
                            filename = filename + "$" + DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.ToString("HHmmss") + ".xlsx";
                        }
                        file.SaveAs(HolidayFile + filename);

                        dt = abl.Holiday_ExcelToTable(HolidayFile + filename);
                        String[] colName = {"Holiday_Date", "Description" };
                        if (abl.CheckColumn(colName, dt))
                        {
                            string YYY = string.Empty;
                            if (id != null)
                            {
                                YYY = id;

                            }
                            if (dt.Rows.Count > 0)
                            {
                                abl.Insert_Holiday_Data(dt, YYY, file.FileName);
                                Session["Imsg"] = "OK";
                            }
                        }
                        else
                        {
                            Session["Emsg"] = "NotOK";
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                string error = ex.ToString();
                Session["Emsg"] = "NotOK";
            }
            return JsonConvert.SerializeObject("OK");
        }

        [HttpGet]
        public string Get_Holiday_List()
        {
            DataTable dt = new DataTable();
            string jsonresult;
            AttendanceBL atbl = new AttendanceBL();
            Function fn = new Function();
            dt = atbl.Get_Holiday();
            jsonresult = fn.DataTableToJSONWithJSONNet(dt);
            return jsonresult;
        }
    }
}