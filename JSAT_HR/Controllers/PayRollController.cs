using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommonFunction;
using JH_DL;
using Newtonsoft.Json;
using PayRoll_BL;


namespace JSAT_HR.Controllers
{
    public class PayRollController : Controller
    {
        Payroll_BL pbl = new Payroll_BL();
        DataTable AllowanceDt = new DataTable();
        DataTable DeductionDt = new DataTable();
        // GET: PayRoll
        public ActionResult PayRoll_Setting()
        {
            return View();
        }

        [HttpGet]
        public string GetPayRoll()
        {
            Function fun = new Function();
            return fun.DataTableToJSONWithJSONNet(pbl.PayRoll_Search("",1));
        }



        [HttpGet]
        public string PayRoll_Search(string id )
        {
            DataTable dtpay = new DataTable();
            if (!String.IsNullOrWhiteSpace(id))
            {
                dtpay = pbl.PayRoll_Search(id,0);
                if(dtpay !=null && dtpay.Rows.Count>0)
                {
                    string jsonresult;
                    jsonresult = JsonConvert.SerializeObject(dtpay);
                    return jsonresult;
                }
                else
                {
                    return JsonConvert.SerializeObject(dtpay);
                }
            }
            else
                return JsonConvert.SerializeObject(dtpay);
        }

        public FileStreamResult PayRoll_Report(string id)
        {
            DataSet ds = new DataSet();
            string savefilename = "PayRoll_Report" + (DateTime.Now).ToShortDateString() + ".pdf";

            DataTable dt = new DataTable();
            dt = pbl.PayRoll_Setting_Report(id);
            if (dt.Rows.Count < 20)
            {
                for (int i = 0; dt.Rows.Count < 20; i++)
                {
                    dt.Rows.Add();
                    dt.AcceptChanges();
                }
            }
            ds.Tables.Add(dt);

            Report.PayRoll_List rpt = new Report.PayRoll_List();
            rpt.Database.Tables["Pay_Roll"].SetDataSource(ds.Tables[0]);
            rpt.SetParameterValue("Date", DateTime.Now.ToString("yyyy-MM-dd"));

            Stream str = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            str.Seek(0, SeekOrigin.Begin);

            return File(str, "application/pdf");
        }

        public string PayRoll_Process(string id)
        {
            DataTable dtpay = new DataTable();
            if (id != "00")
            {
                string date = id.Substring(0, 4) + "-" + id.Substring(4,2) + "-" + "01";
                if (!String.IsNullOrWhiteSpace(id))
                {
                    dtpay = pbl.PayRoll_Calculate(id, date);
                    if (dtpay != null && dtpay.Rows.Count > 0)
                    {
                        string jsonresult;
                        jsonresult = JsonConvert.SerializeObject(dtpay);
                        return jsonresult;
                    }
                }
                return JsonConvert.SerializeObject(dtpay);
            }
            return JsonConvert.SerializeObject(dtpay);
        }

        public FileStreamResult PayRoll_Detail_Report(string id)
        {
            DataSet ds = new DataSet();
            string savefilename = "PayRoll_Report_Detail" + (DateTime.Now).ToShortDateString() + ".pdf";

            DataTable dt = new DataTable();
            dt = pbl.PayRoll_Detail_Report(id.Substring(6,1),id.Substring(0,6));
            if (dt.Rows.Count > 0)
            {
                ds.Tables.Add(dt);

                Report.PayRoll_Detail rpt = new Report.PayRoll_Detail();
                rpt.Database.Tables["PayRoll_Detail_tb"].SetDataSource(ds.Tables[0]);

                Stream str = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                str.Seek(0, SeekOrigin.Begin);

                return File(str, "application/pdf");
            }
            else
                return null;
        }

        public ActionResult PayRoll_Detail()
        {
            
            Payroll_BL bl = new Payroll_BL();
            var staffName = bl.SelectStaff();
            SelectList list = new SelectList(staffName, "StaffID", "Name");
            ViewBag.staffNameList = list;

            return View();
        }

        public JsonResult GetPayRollDetail_AllowanceLabel(string id)
        {
            Payroll_BL bl = new Payroll_BL();
            
            AllowanceDt = bl.PayRoll_Detail_Allow(id);
            return Json(PRD_ALabelCol(AllowanceDt), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPayRollDetail_AllowanceData(string id)
        {

            Payroll_BL bl = new Payroll_BL();

           AllowanceDt = bl.PayRoll_Detail_Allow(id);

            return Json(PRD_ADataCol(AllowanceDt), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPayRollDetail_DeductionLabel(string id)
        {
            Payroll_BL bl = new Payroll_BL();
            DeductionDt = bl.PayRoll_Detail_Deduction(id);
            return Json(PRD_DLabelCol(DeductionDt), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPayRollDetail_DeductionData(string id)
        {
            Payroll_BL bl = new Payroll_BL();
            DeductionDt = bl.PayRoll_Detail_Deduction(id);
            return Json(PRD_DDataCol(DeductionDt), JsonRequestBehavior.AllowGet);
        }

        public string PRD_Row(string id)
        {
            return "<tr><td>" + id + "</td></tr>";
        }

        public string PRD_ALabelCol(DataTable dt)
        {
            string Table_Header = string.Empty;
            if (dt.Rows.Count > 0)
            {

                var StaffID = dt.Rows[0]["StaffID"].ToString();
                Table_Header += PRD_Row("StaffID");
                var StaffName = dt.Rows[0]["Name"].ToString();
                Table_Header += PRD_Row("Name");
                var YYYYMM = dt.Rows[0]["YYYYMM"].ToString();
                Table_Header += PRD_Row("YYYYMM");
                var Currency = dt.Rows[0]["Currency"].ToString();
                Table_Header += PRD_Row("Currency");
                var BasicSalary = dt.Rows[0]["BasicSalary"].ToString();
                Table_Header += PRD_Row("BasicSalary");
                var Effort = dt.Rows[0]["Effort"].ToString();
                Table_Header += PRD_Row("Effort");
                var MD = dt.Rows[0]["MD"].ToString();
                if (MD.Split('.')[0] != "0")
                {
                    Table_Header += PRD_Row("MD");
                }
                var Director = dt.Rows[0]["Director"].ToString();
                if (Director.Split('.')[0] != "0")
                {
                    Table_Header += PRD_Row("Director");
                }
                var Manager = dt.Rows[0]["Manager"].ToString();
                if (Manager.Split('.')[0] != "0")
                {
                    Table_Header += PRD_Row("Manager");
                }
                var N1 = dt.Rows[0]["N1"].ToString();
                if (N1.Split('.')[0] != "0")
                {
                    Table_Header += PRD_Row("N1");
                }
                var N2 = dt.Rows[0]["N2"].ToString();
                if (N2.Split('.')[0] != "0")
                {
                    Table_Header += PRD_Row("N2");
                }
                var N3 = dt.Rows[0]["N3"].ToString();

                if (N3.Split('.')[0] != "0")
                {
                    Table_Header += PRD_Row("N3");
                }
                var JpnUNiGradurate = dt.Rows[0]["JpnUNiGradurate"].ToString();
                if (JpnUNiGradurate.Split('.')[0] != "0")
                {
                    Table_Header += PRD_Row("JpnUNiGradurate");
                }
                var Local1stInterviewer = dt.Rows[0]["Local1stInterviewer"].ToString();
                if (Local1stInterviewer.Split('.')[0] != "0")
                {
                    Table_Header += PRD_Row("Local1stInterviewer");
                }

                var Local2dstInterviewer = dt.Rows[0]["Local2dstInterviewer"].ToString();
                if (Local2dstInterviewer.Split('.')[0] != "0")
                {
                    Table_Header += PRD_Row("Local2dstInterviewer");
                }

                var Overseas1stInterviewer = dt.Rows[0]["Overseas1stInterviewer"].ToString();
                if (Overseas1stInterviewer.Split('.')[0] != "0")
                {
                    Table_Header += PRD_Row("Overseas1stInterviewer");
                }

                var Overseas2ndInterviewer = dt.Rows[0]["Overseas2ndInterviewer"].ToString();
                if (Overseas2ndInterviewer.Split('.')[0] != "0")
                {
                    Table_Header += PRD_Row("Overseas2ndInterviewer");
                }

                var MarketingTeam = dt.Rows[0]["MarketingTeam"].ToString();
                if (MarketingTeam.Split('.')[0] != "0")
                {
                    Table_Header += PRD_Row("MarketingTeam");
                }

                var Mentor = dt.Rows[0]["Mentor"].ToString();
                if (Mentor.Split('.')[0] != "0")
                {
                    Table_Header += PRD_Row("Mentor");
                }

                var Bus = dt.Rows[0]["Bus"].ToString();
                if (Bus.Split('.')[0] != "0")
                {
                    Table_Header += PRD_Row("Bus");
                }

                var Train = dt.Rows[0]["Train"].ToString();
                if (Train.Split('.')[0] != "0")
                {
                    Table_Header += PRD_Row("Train");
                }


                var TotalSalary = dt.Rows[0]["TotalSalary"].ToString();
                Table_Header += "<br><tr><td><b>TotalSalary</b></td></tr>";

            }


            return Table_Header;

        }

        public string PRD_DLabelCol(DataTable dt)
        {
            string DTable_Header = string.Empty;
            if (dt.Rows.Count > 0)
            {

                //var StaffID = dt.Rows[0]["StaffID"].ToString();
                //DTable_Header += PRD_Row("StaffID");
                //var StaffName = dt.Rows[0]["Name"].ToString();
                //DTable_Header += PRD_Row("Name");
                //var YYYYMM = dt.Rows[0]["YYYYMM"].ToString();
                //DTable_Header += PRD_Row("YYYYMM");
                var LateMinutes = dt.Rows[0]["LateMinutes"].ToString();
                DTable_Header += PRD_Row("LateMinutes");
                var HourlyLeave = dt.Rows[0]["HourlyLeave"].ToString();
                DTable_Header += PRD_Row("HourlyLeave");
                var EarlyOut = dt.Rows[0]["EarlyOut"].ToString();
                DTable_Header += PRD_Row("EarlyOut");

                var UnpaidLeave = dt.Rows[0]["UnpaidLeave"].ToString();
                if (UnpaidLeave.Split('.')[0] != "0")
                {
                    DTable_Header += PRD_Row("UnpaidLeave");
                }

                var LateTimeFees = dt.Rows[0]["LateTimeFees"].ToString();
                if (LateTimeFees.Split('.')[0] != "0")
                {
                    DTable_Header += PRD_Row("LateTimeFees");
                }

                var HourlyLeaveFees = dt.Rows[0]["HourlyLeaveFees"].ToString();
                if (HourlyLeaveFees.Split('.')[0] != "0")
                {
                    DTable_Header += PRD_Row("HourlyLeaveFees");
                }

                var EarlyOutFees = dt.Rows[0]["EarlyOutFees"].ToString();
                if (EarlyOutFees.Split('.')[0] != "0")
                {
                    DTable_Header += PRD_Row("EarlyOutFees");
                }

                var UnpaidLeaveFees = dt.Rows[0]["UnpaidLeaveFees"].ToString();
                if (UnpaidLeaveFees.Split('.')[0] != "0")
                {
                    DTable_Header += PRD_Row("UnpaidLeaveFees");
                }

                var SocialWelfare = dt.Rows[0]["SocialWelfare"].ToString();
                if (SocialWelfare.Split('.')[0] != "0")
                {
                    DTable_Header += PRD_Row("SocialWelfare");
                }

                var IncomeTax = dt.Rows[0]["IncomeTax"].ToString();
                if (IncomeTax.Split('.')[0] != "0")
                {
                    DTable_Header += PRD_Row("IncomeTax");
                }

                var UniformCharges = dt.Rows[0]["UniformCharges"].ToString();
                if (UniformCharges.Split('.')[0] != "0")
                {
                    DTable_Header += PRD_Row("UniformCharges");
                }

                var TotalDeduction = dt.Rows[0]["TotalDeduction"].ToString();
                DTable_Header += "<tr><td><b>TotalDeduction</b></td></tr>";

            }


            return DTable_Header;
        }

        public string PRD_DDataCol(DataTable dt)
        {
            string Deduction_Data = string.Empty;
            if (dt.Rows.Count > 0)
            {

                //var StaffID = dt.Rows[0]["StaffID"].ToString();
                //Deduction_Data += PRD_Row(StaffID);
                //var StaffName = dt.Rows[0]["Name"].ToString();
                //Deduction_Data += PRD_Row(StaffName);
                //var YYYYMM = dt.Rows[0]["YYYYMM"].ToString();
                //Deduction_Data += PRD_Row(YYYYMM);
                var LateMinutes = dt.Rows[0]["LateMinutes"].ToString();
                Deduction_Data += PRD_Row(LateMinutes);
                var HourlyLeave = dt.Rows[0]["HourlyLeave"].ToString();
                Deduction_Data += PRD_Row(HourlyLeave);
                var EarlyOut = dt.Rows[0]["EarlyOut"].ToString();
                Deduction_Data += PRD_Row(EarlyOut);

                var UnpaidLeave = dt.Rows[0]["UnpaidLeave"].ToString();
                if (UnpaidLeave.Split('.')[0] != "0")
                {
                    Deduction_Data += PRD_Row(UnpaidLeave);
                }

                var LateTimeFees = dt.Rows[0]["LateTimeFees"].ToString();
                if (LateTimeFees.Split('.')[0] != "0")
                {
                    Deduction_Data += PRD_Row(LateTimeFees);
                }

                var HourlyLeaveFees = dt.Rows[0]["HourlyLeaveFees"].ToString();
                if (HourlyLeaveFees.Split('.')[0] != "0")
                {
                    Deduction_Data += PRD_Row(HourlyLeaveFees);
                }

                var EarlyOutFees = dt.Rows[0]["EarlyOutFees"].ToString();
                if (EarlyOutFees.Split('.')[0] != "0")
                {
                    Deduction_Data += PRD_Row(EarlyOutFees);
                }

                var UnpaidLeaveFees = dt.Rows[0]["UnpaidLeaveFees"].ToString();
                if (UnpaidLeaveFees.Split('.')[0] != "0")
                {
                    Deduction_Data += PRD_Row(UnpaidLeaveFees);
                }

                var SocialWelfare = dt.Rows[0]["SocialWelfare"].ToString();
                if (SocialWelfare.Split('.')[0] != "0")
                {
                    Deduction_Data += PRD_Row(SocialWelfare);
                }

                var IncomeTax = dt.Rows[0]["IncomeTax"].ToString();
                if (IncomeTax.Split('.')[0] != "0")
                {
                    Deduction_Data += PRD_Row(IncomeTax);
                }

                var UniformCharges = dt.Rows[0]["UniformCharges"].ToString();
                if (UniformCharges.Split('.')[0] != "0")
                {
                    Deduction_Data += PRD_Row(UniformCharges);
                }

                var TotalDeduction = dt.Rows[0]["TotalDeduction"].ToString();
                Deduction_Data += "<tr><td><b>" + TotalDeduction + "</b></td></tr>";

            }


            return Deduction_Data;

        }
             

        public string PRD_ADataCol(DataTable dt)
        {
            string AllowanceData = string.Empty;
            if (dt.Rows.Count > 0)
            {

                var StaffID = dt.Rows[0]["StaffID"].ToString();
                AllowanceData += "<tr><td>" + StaffID + "</td></tr>";
                var StaffName = dt.Rows[0]["Name"].ToString();
                AllowanceData += "<tr><td>" + StaffName + "</td></tr>";
                var YYYYMM = dt.Rows[0]["YYYYMM"].ToString();
                AllowanceData += "<tr><td>" + YYYYMM + "</td></tr>";
                var Currency = dt.Rows[0]["Currency"].ToString();
                AllowanceData += "<tr><td>" + Currency + "</td></tr>";
                var BasicSalary = dt.Rows[0]["BasicSalary"].ToString();
                AllowanceData += "<tr><td>" + BasicSalary + "</td></tr>";

                var Effort = dt.Rows[0]["Effort"].ToString();
                AllowanceData += "<tr><td>" + Effort + "</td></tr>";

                var MD = dt.Rows[0]["MD"].ToString();
                if (MD.Split('.')[0] != "0")
                {
                    AllowanceData += "<tr><td>" + MD + "</td></tr>";
                }

                var Director = dt.Rows[0]["Director"].ToString();
                if (Director.Split('.')[0] != "0")
                {
                    AllowanceData += "<tr><td>" + Director + "</td></tr>";
                }

                var Manager = dt.Rows[0]["Manager"].ToString();
                if (Manager.Split('.')[0] != "0")
                {
                    AllowanceData += "<tr><td>" + Manager + "</td></tr>";
                }

                var N1 = dt.Rows[0]["N1"].ToString();
                if (N1.Split('.')[0] != "0")
                {
                    AllowanceData += "<tr><td>" + N1 + "</td></tr>";
                }

                var N2 = dt.Rows[0]["N2"].ToString();
                if (N2.Split('.')[0] != "0")
                {
                    AllowanceData += "<tr><td>" + N2 + "</td></tr>";
                }

                var N3 = dt.Rows[0]["N3"].ToString();
                if (N3.Split('.')[0] != "0")
                {
                    AllowanceData += "<tr><td>" + N3 + "</td></tr>";
                }

                var JpnUNiGradurate = dt.Rows[0]["JpnUNiGradurate"].ToString();
                if (JpnUNiGradurate.Split('.')[0] != "0")
                {
                    AllowanceData += "<tr><td>" + JpnUNiGradurate + "</td></tr>";
                }

                var Local1stInterviewer = dt.Rows[0]["Local1stInterviewer"].ToString();
                if (Local1stInterviewer.Split('.')[0] != "0")
                {
                    AllowanceData += "<tr><td>" + Local1stInterviewer + "</td></tr>";
                }

                var Local2dstInterviewer = dt.Rows[0]["Local2dstInterviewer"].ToString();
                if (Local2dstInterviewer.Split('.')[0] != "0")
                {
                    AllowanceData += "<tr><td>" + Local2dstInterviewer + "</td></tr>";
                }

                var Overseas1stInterviewer = dt.Rows[0]["Overseas1stInterviewer"].ToString();
                if (Overseas1stInterviewer.Split('.')[0] != "0")
                {
                    AllowanceData += "<tr><td>" + Overseas1stInterviewer + "</td></tr>";
                }

                var Overseas2ndInterviewer = dt.Rows[0]["Overseas2ndInterviewer"].ToString();
                if (Overseas2ndInterviewer.Split('.')[0] != "0")
                {
                    AllowanceData += "<tr><td>" + Overseas2ndInterviewer + "</td></tr>";
                }

                var MarketingTeam = dt.Rows[0]["MarketingTeam"].ToString();
                if (MarketingTeam.Split('.')[0] != "0")
                {
                    AllowanceData += "<tr><td>" + MarketingTeam + "</td></tr>";
                }

                var Mentor = dt.Rows[0]["Mentor"].ToString();
                if (Mentor.Split('.')[0] != "0")
                {
                    AllowanceData += "<tr><td>" + Mentor + "</td></tr>";
                }

                var Bus = dt.Rows[0]["Bus"].ToString();
                if (Bus.Split('.')[0] != "0")
                {
                    AllowanceData += "<tr><td>" + Bus + "</td></tr>";
                }

                var Train = dt.Rows[0]["Train"].ToString();
                if (Train.Split('.')[0] != "0")
                {
                    AllowanceData += "<tr><td>" + Train + "</td></tr>";
                }



                var TotalSalary = dt.Rows[0]["TotalSalary"].ToString();
                AllowanceData += "<br><tr><td><b>" + TotalSalary + "</b></td></tr>";


            }
            return AllowanceData;
        }
    }
}