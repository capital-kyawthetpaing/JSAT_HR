﻿using System;
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
        AttendanceBL abl = new AttendanceBL();
        public ActionResult AttendanceImport()
        {
            return View();
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
            string AttandenceDate = string.Empty;
            if (id != null)
            {
                arr = id.Split('_');
                YYYMM = arr[0] + arr[1];
                StaffID = arr[2];
                AttandenceDate= arr[0] +"-"+ arr[1]+"-01";

            }
            am.YYYYMM = YYYMM;
            am.StaffID = StaffID;
            am.AttandenceDate = AttandenceDate;

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
                dtAttlist.Columns.Add("AttandenceDate", typeof(System.DateTime));
                if (dtAttlist.Rows.Count > 0)
                {
                    int count = 1;
                    foreach (DataRow dr in dtAttlist.Rows)
                    {
                        dr["DD"] = count;
                        dr["AttandenceDate"] = model.attModel.YYYY + "-" + model.attModel.MM + "-" + count;
                        count++;
                    }
                    abl.Update_Attendance_List(dtAttlist, model);
                }
                Session["UImsg"] = "OK";
                Session["SearchDisplay"]= model.attModel.YYYY +"_"+ model.attModel.MM+"_"+ model.attModel.StaffID;
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
            String Imsg = Session["Imsg"] as string;//insert ok
            String IEmsg = Session["IEmsg"] as string;// insert notok
            String Umsg = Session["Umsg"] as string;// update ok
            String UEmsg = Session["UEmsg"] as string;//update notok
            ViewBag.Imsg = Imsg;
            ViewBag.IEmsg = IEmsg;
            ViewBag.Umsg = Umsg;
            ViewBag.UEmsg = UEmsg;
            Session["Imsg"] = "";
            Session["IEmsg"] = "";
            Session["Umsg"] = "";
            Session["UEmsg"] = "";

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
            
            return View();
        }


        public String QuickAttendance_Update(string id, string fromdate, string todate, string leavetype, string morningleave, string eveleave, string transportation,string holiday)
        {
            string flag = string.Empty; DateTime todaydate = DateTime.Now;
            AttendanceBL abl = new AttendanceBL();

            if (todate == "")
                todate = fromdate;

            if (!String.IsNullOrWhiteSpace(id)&& !String.IsNullOrWhiteSpace(fromdate)&&!String.IsNullOrWhiteSpace(todate))
            {
                string[] list; list = id.Split(',');
                if (holiday == "0")
                {
                    foreach (DateTime day in EachDay(DateTime.Parse(fromdate), DateTime.Parse(todate)))
                    {
                        string date = day.ToString("yyyyMMdd").Replace("/", "");
                        foreach (string st in list)
                        {
                            flag = abl.QuickAttendance_Update(st, date, leavetype, morningleave, eveleave, transportation);
                        }
                        if (flag == "OK")
                        {
                            if (DateTime.Parse(todate) < todaydate)
                            {
                                Session["Umsg"] = flag;
                            }
                            else
                                Session["Imsg"] = flag;
                        }
                        else
                        {
                            if (DateTime.Parse(todate) < todaydate)
                            {
                                Session["UEmsg"] = flag;
                            }
                            else
                                Session["Imsg"] = flag;
                        }
                    }
                }
                else
                {
                    foreach (DateTime day in EachDay(DateTime.Parse(fromdate), DateTime.Parse(todate)))
                    {
                        string holidaydate = day.ToString("yyyy-MM-dd");
                        DataTable dt = abl.Get_Holiday(holidaydate,2);
                        if (dt.Rows.Count>0)
                        {
                        }
                        else
                        {
                            string date = day.ToString("yyyyMMdd").Replace("/", "");
                            foreach (string st in list)
                            {
                                flag = abl.QuickAttendance_Update(st, date, leavetype, morningleave, eveleave, transportation);
                            }
                            if (flag == "OK")
                            {
                                if (DateTime.Parse(todate) < todaydate)
                                {
                                    Session["Umsg"] = flag;
                                }
                                else
                                    Session["Imsg"] = flag;
                            }
                            else
                            {
                                if (DateTime.Parse(todate) < todaydate)
                                {
                                    Session["UEmsg"] = flag;
                                }
                                else
                                    Session["Imsg"] = flag;
                            }
                        }
                        
                    }
                }
               
            }
            return JsonConvert.SerializeObject(flag);

        }

        public string QuickAttendance_LeaveUpdate(string id, string fromdate, string todate, string leavetype, string morningleave, string eveleave, string transportation,string holiday)
        {
            string flag = string.Empty;DateTime todaydate = DateTime.Now;
            AttendanceBL abl = new AttendanceBL();

            if (todate == "")
                todate = fromdate;
            if(leavetype=="")
            {
                morningleave = "-1";eveleave = "-1";
            }
            if (!String.IsNullOrWhiteSpace(id) && !String.IsNullOrWhiteSpace(fromdate) && !String.IsNullOrWhiteSpace(todate))
            {
                string[] list; list = id.Split(',');
                if (holiday == "0")
                {
                    foreach (DateTime day in EachDay(DateTime.Parse(fromdate), DateTime.Parse(todate)))
                    {
                        string date = day.ToString("yyyyMMdd").Replace("/", "");
                        foreach (string st in list)
                        {
                            flag = abl.QuickAttendance_LeaveUpdate(st, date, leavetype, morningleave, eveleave, transportation);
                        }
                    }
                    if (flag == "OK")
                    {
                        if (DateTime.Parse(todate) < todaydate)
                        {
                            Session["Umsg"] = flag;
                        }
                        else
                            Session["Imsg"] = flag;
                    }
                    else
                    {
                        if (DateTime.Parse(todate) < todaydate)
                        {
                            Session["UEmsg"] = flag;
                        }
                        else
                            Session["Imsg"] = flag;
                    }
                }
                else
                {
                    foreach (DateTime day in EachDay(DateTime.Parse(fromdate), DateTime.Parse(todate)))
                    {
                        string holidaydate = day.ToString("yyyy-MM-dd");
                        DataTable dt = abl.Get_Holiday(holidaydate, 2);
                        if (dt.Rows.Count > 0)
                        {
                        }
                        else
                        {
                            string date = day.ToString("yyyyMMdd").Replace("/", "");
                            foreach (string st in list)
                            {
                                flag = abl.QuickAttendance_LeaveUpdate(st, date, leavetype, morningleave, eveleave, transportation);
                            }
                        }
                        if (flag == "OK")
                        {
                            if (DateTime.Parse(todate) < todaydate)
                            {
                                Session["Umsg"] = flag;
                            }
                            else
                                Session["Imsg"] = flag;
                        }
                        else
                        {
                            if (DateTime.Parse(todate) < todaydate)
                            {
                                Session["UEmsg"] = flag;
                            }
                            else
                                Session["Imsg"] = flag;
                        }
                    }
                     
                }
              
            }
            return JsonConvert.SerializeObject(flag);


        }
       
        public IEnumerable<DateTime> EachDay(DateTime from, DateTime to)
        {
            for (var day = from.Date; day.Date <= to.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}