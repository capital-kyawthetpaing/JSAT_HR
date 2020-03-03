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
using FastMember;
using Spire.Xls;

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
                                abl.Insert_Attendance_Data(dt, file.FileName,id);
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

                            dt = abl.ExcelToTable(IncomeTaxFile + filename,id);
                            if (dt.Rows.Count > 0)
                            {
                                abl.Insert_IncomeTax_Data(dt, file.FileName);
                                Session["Imsg"] = "OK";
                            }
                        }                      
                    }

                }
            }
                        

            catch (Exception ex) { string error = ex.ToString(); Session["Emsg"] = "NotOK"; }

            //return RedirectToAction("AttendanceImport");
            return JsonConvert.SerializeObject("OK");
        }

        public ActionResult Import_Log_List()
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
            String UImsg = Session["UImsg"] as string;
            String UEmsg = Session["UEmsg"] as string;
            ViewBag.Imsg = UImsg;
            ViewBag.Emsg = UEmsg;
            Session["UImsg"] = "";
            Session["UEmsg"] = "";
            MultiModel mm = new MultiModel();
            mm.attModel = new AttendanceModel();
            mm.attModel.YYYY = DateTime.Now.Year.ToString();
            mm.attModel.MM = DateTime.Now.Month.ToString().PadLeft(2,'0');
            return View(mm);

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

        public ActionResult Attendance_Setting_Save(MultiModel model)
        {
            try {

                DataTable dtattendance= new DataTable();
                using (var reader = ObjectReader.Create(model.attlistModel, "TimeIn", "TimeOut", "LeaveType", "EarlyOut"))
                {
                    dtattendance.Load(reader);
                }
                dtattendance.Columns.Add("DD", typeof(System.Int32));
                if (dtattendance.Rows.Count>0)
                {
                    int count = 1;
                    foreach (DataRow dr in dtattendance.Rows)
                    {
                        dr["DD"] = count;
                        count++;
                        if (dr["LeaveType"].ToString() == "")
                            dr["LeaveType"] = 0;
                        if (dr["EarlyOut"].ToString() == "")
                            dr["EarlyOut"] = 0;
                    }
                    abl.Update_Attendance_Data(dtattendance,model);
                }
                Session["UImsg"] = "OK";
                return RedirectToAction("AttendanceSetting");
            }
            catch (Exception ex)
            {
                string st = ex.ToString();
                Session["UEmsg"] = "NotOK";
                return RedirectToAction("AttendanceSetting");
            }
        }
    }
}