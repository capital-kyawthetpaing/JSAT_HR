using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Attendance_BL;
using System.Configuration;
using Newtonsoft.Json;
using JH_Model;
using CommonFunction;

namespace JSAT_HR.Controllers
{
    public class AttendanceController : Controller
    {
        string AttendanceFile = ConfigurationManager.AppSettings["AttendanceFile"].ToString();
        string IncomeTaxFile = ConfigurationManager.AppSettings["IncomeTaxFile"].ToString();

        AttendanceBL abl = new AttendanceBL();
        public ActionResult AttendanceImport()
        {
            return View();
        }

        [HttpPost]
        public string AttendanceDataSave(string id)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file=null;
                    for (int i = 0; i < files.Count; i++)
                    {

                         file = files[i];
                    }
                    if (file != null)
                    {
                        string filename = string.Empty;
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
                                filename = filename.Replace(".xlsx", "");
                                filename = filename + "$" + DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.ToString("HHmmss") + ".xlsx";
                            }
                            file.SaveAs(AttendanceFile + filename);

                            dt = abl.AttendanceData(AttendanceFile + filename, id);
                            if (dt.Rows.Count > 0)
                            {
                                abl.Insert_Attendance_Data(dt, file.FileName,id);
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

                            dt = abl.ExcelToTable(IncomeTaxFile + filename,id);
                            if (dt.Rows.Count > 0)
                            {
                                abl.Insert_IncomeTax_Data(dt, file.FileName);
                            }
                        }                      
                    }

                }
            }
                        

            catch (Exception ex) { string error = ex.ToString(); }

            //return RedirectToAction("AttendanceImport");
            return JsonConvert.SerializeObject("OK");
        }

        public ActionResult Import_Log_List()
        {
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

        public ActionResult AttendanceList()
        {
            AttendanceBL abl = new AttendanceBL();
            AttendanceModel am = new AttendanceModel();
            am.YYYYMM = "201912";
            DataSet ds = abl.M_Attendance_Select(am);
            return View(ds);
        }

        public ActionResult AttendanceSetting()
        {
            return View();

        }

        [HttpPost]
        public string _AttendanceSearch(string id)
        {
            string jsonresult;
            DataTable dt  = abl.Get_Attendance_List(id);
            dt.Columns.Add("Total", typeof(System.Int32));
            if (dt.Rows.Count > 0)
                {
                foreach (DataRow dr in dt.Rows)
                {
                    dr["Total"] = dt.Rows.Count;
                }
                    jsonresult = JsonConvert.SerializeObject(dt);
                    return jsonresult;
                }
            else
                return JsonConvert.SerializeObject(dt);

        }
    }
}