using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommonFunction;
using Newtonsoft.Json;
using PayRoll_BL;


namespace JSAT_HR.Controllers
{
    public class PayRollController : Controller
    {
        Payroll_BL pbl = new Payroll_BL();
        // GET: PayRoll
        public ActionResult PayRoll_Setting()
        {
            return View();
        }

        public ActionResult PayRoll_Detail()
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
            }
            return JsonConvert.SerializeObject(dtpay);
        }

        public FileStreamResult PayRoll_Report(string id)
        {
            DataSet ds = new DataSet();
            string savefilename = "PayRoll_Report" + (DateTime.Now).ToShortDateString() + ".pdf";

            DataTable dt = new DataTable();
            dt = pbl.PayRoll_Search(id,1);
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
                string date = id.Substring(0, 4) + "-" + id.Substring(4) + "-" + "01";
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

        public FileStreamResult PayRoll_Detail_Report(string yyyymm)
        {
            DataSet ds = new DataSet();
            string savefilename = "PayRoll_Report_Detail" + (DateTime.Now).ToShortDateString() + ".pdf";

            DataTable dt = new DataTable();
            dt = pbl.PayRoll_Detail_Report(yyyymm.Substring(3,1),yyyymm.Substring(0,4));
            //if (dt.Rows.Count > 0)
            //{
                //for (int i = 0; dt.Rows.Count < 20; i++)
                //{
                //    dt.Rows.Add();
                //    dt.AcceptChanges();
                //}

                ds.Tables.Add(dt);

                Report.PayRoll_Detail rpt = new Report.PayRoll_Detail();
                rpt.Database.Tables["Pay_Roll_Detail_tb"].SetDataSource(ds.Tables[0]);

                Stream str = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                str.Seek(0, SeekOrigin.Begin);

                return File(str, "application/pdf");
            //}
        }
    }
}