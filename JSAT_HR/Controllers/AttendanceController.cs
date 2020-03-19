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
using Staff_BL;
using Leave_BL;
using System.Configuration;
using Newtonsoft.Json;
using JH_Model;
using CommonFunction;
using FastMember;
using Spire.Xls;
using System.Data.SqlClient;
using JH_DL;



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
            String UImsg = Session["UImsg"] as string;
            String UEmsg = Session["UEmsg"] as string;
            string S_Display = Session["SearchDisplay"] as string;
            ViewBag.UImsg = UImsg;
            ViewBag.UEmsg = UEmsg;
            ViewBag.S_Display = S_Display;
            Session["UImsg"] = "";
            Session["UEmsg"] = "";
            Session["SearchDisplay"] = "";

            return View();
        }

        [HttpPost]
        public string _AttendanceListSearch(string id)
        {
            string jsonresult;
            AttendanceBL abl = new AttendanceBL();
            AttendanceModel am = new AttendanceModel();

            string[] arr;
            string YYYMM = string.Empty;
            string StaffID = string.Empty;
            if (id != null)
            {
                arr = id.Split('_');
                YYYMM = arr[0] + arr[1];
                StaffID = arr[2];

            }
            am.YYYYMM = YYYMM;
            am.StaffID = StaffID;
            DataTable dt = abl.M_Attendance_Select(am);

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

        [HttpGet]
        public string GetLeave(string id)
        {
            Function fun = new Function();
            return fun.DataTableToJSONWithJSONNet(abl.GetLeave(id));
        }

        public ActionResult Attendance_List_Save(MultiModel model)
        {
            try
            {

                DataTable dtAttlist = new DataTable();
                using (var reader = ObjectReader.Create(model.attlistModel, "TimeIn", "TimeOut", "EarlyOut", "MorningLeaveType", "EveningLeaveType"))
                {
                    dtAttlist.Load(reader);
                }
                dtAttlist.Columns.Add("DD", typeof(System.Int32));
                if (dtAttlist.Rows.Count > 0)
                {
                    int count = 1;
                    foreach (DataRow dr in dtAttlist.Rows)
                    {
                        dr["DD"] = count;
                        count++;
                    }
                    abl.Update_Attendance_List(dtAttlist, model);
                }
                Session["UImsg"] = "OK";
                Session["SearchDisplay"]= model.attModel.YYYY +'_'+ model.attModel.MM+'_'+ model.attModel.StaffID;
                return RedirectToAction("AttendanceList");
            }
            catch (Exception ex)
            {
                string st = ex.ToString();
                Session["UEmsg"] = "NotOK";
                return RedirectToAction("AttendanceList");
            }
        }

        public ActionResult AttendanceSetting()
        {
            String UImsg = Session["UImsg"] as string;
            String UEmsg = Session["UEmsg"] as string;
            ViewBag.UImsg = UImsg;
            ViewBag.UEmsg = UEmsg;
            Session["UImsg"] = "";
            Session["UEmsg"] = "";
            MultiModel mm = new MultiModel();
            mm.attModel = new AttendanceModel();
            mm.attModel.YYYY = DateTime.Now.Year.ToString();
            mm.attModel.MM = DateTime.Now.Month.ToString().PadLeft(2, '0');
            return View(mm);

        }

        [HttpPost]
        public string _AttendanceSearch(string id)
        {
            string jsonresult;
            DataTable dt = abl.Get_Attendance_List(id);
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
            try
            {

                DataTable dtattendance = new DataTable();
                using (var reader = ObjectReader.Create(model.attlistModel, "TimeIn", "TimeOut", "LeaveType", "EarlyOut"))
                {
                    dtattendance.Load(reader);
                }
                dtattendance.Columns.Add("DD", typeof(System.Int32));
                if (dtattendance.Rows.Count > 0)
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
                    abl.Update_Attendance_Data(dtattendance, model);
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

       
        public ActionResult QuickAttendance(StaffModel model)
        {
            try
            {
                if (model.SelectedMultiStaffId != null)
                {
                    List<StaffObj> stafflist = this.LoadData();
                    model.SelectedStaffName = stafflist.Where(p => model.SelectedMultiStaffId.Contains(p.StaffID)).Select(q => q).ToList();
                }
                this.ViewBag.StaffList = this.GetStaffList();
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return this.View(model);
        }

        private IEnumerable<SelectListItem> GetStaffList()
        {

            SelectList lstobj = null;
            try
            {
                var list = this.LoadData()
                                  .Select(p =>
                                            new SelectListItem
                                            {
                                                Value = Convert.ToInt32(p.StaffID).ToString(),
                                                Text = p.StaffName,
                                            });
                lstobj = new SelectList(list, "Value", "Text");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstobj;
        }

        private List<StaffObj> LoadData()
        {
            StaffBL sbl = new StaffBL();
            List<StaffObj> lst = new List<StaffObj>();
            //List<DataRow> list = dt.AsEnumerable().ToList();
            try
            {
                
                DataTable dt = new DataTable();
                dt = sbl.GetAllStaff("1");

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        StaffObj infoObj = new StaffObj();
                        infoObj.StaffID = Convert.ToInt32(dt.Rows[i]["StaffID"].ToString());
                        infoObj.StaffName = dt.Rows[i]["Name"].ToString();
                        lst.Add(infoObj);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return lst;
        }

        public ActionResult QuickSetting(StaffModel model)
        {
            String message = Session["Message"] as string;
            ViewBag.mesg = message;
            Session["Message"] = "";
         
            try
            {
                if (model.SelectedMultiStaffId != null)
                {
                    List<StaffObj> stafflist = this.LoadData();
                    model.SelectedStaffName = stafflist.Where(p => model.SelectedMultiStaffId.Contains(p.StaffID)).Select(q => q).ToList();
                }
                this.ViewBag.StaffList = this.GetStaffList();
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return this.View(model);
        }

       
        public string QuickAttendance_Update(string id, string fromdate,string todate, string leavetype , string morningleave, string eveleave)
        {
            string jsonresult; string flag = string.Empty;
            AttendanceBL abl = new AttendanceBL();

            if (!String.IsNullOrWhiteSpace(id))
            {
                string[] list; list = id.Split(',');
                foreach (DateTime day in EachDay(DateTime.Parse(fromdate), DateTime.Parse(todate)))
                {
                    string date = day.ToString("yyyyMMdd").Replace("/", "");
                    foreach (string st in list)
                    {
                        flag = abl.QuickAttendance_Update(st, date,leavetype,morningleave,eveleave);
                        if (flag == "OK")
                        {
                            Session["Message"] = "OK";
                        }
                        else
                        {
                            Session["Message"] = "NOT OK";
                        }
                    }
                }
            }
            jsonresult = JsonConvert.SerializeObject(flag);
            return jsonresult;
            
        }

        public IEnumerable<DateTime> EachDay(DateTime from, DateTime to)
        {
            for (var day = from.Date; day.Date <= to.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}