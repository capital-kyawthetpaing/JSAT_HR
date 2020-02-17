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

                    dt = abl.AttendanceData(AttendanceFile + filename);
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

        public ActionResult Import_Log_List()
        {
            return View();
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
    }
}