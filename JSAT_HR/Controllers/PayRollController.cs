using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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


        [HttpGet]
        public string PayRoll_Search(string id )
        {
            DataTable dtpay = new DataTable();
            if (!String.IsNullOrWhiteSpace(id))
            {
                dtpay = pbl.PayRoll_Search(id);
                if(dtpay.Rows.Count>0)
                {
                    string jsonresult;
                    jsonresult = JsonConvert.SerializeObject(dtpay);
                    return jsonresult;
                }
            }
            return null;
        }

        //public FileStreamResult Staff_Report(string id)
        //{
        //    string fullMonthName = DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture);

        //    DataSet ds = new DataSet();
        //    string savedFileName = "StaffReport_" + (DateTime.Now).ToShortDateString() + ".pdf";
        //    var staffid = id;

        //    DataTable dtpay = new DataTable();
        //    dtpay = pbl.PayRoll_Search(id);
        //    if (dtpay.Rows.Count < 20)
        //    {
        //        for (int i = 0; dtpay.Rows.Count < 20; i++)
        //        {
        //            dtpay.Rows.Add();
        //            dtpay.AcceptChanges();
        //        }
        //    }
        //    ds.Tables.Add(dtpay);
        //    Report pr = new Report.PayRoll_Report();
        //    srp.Database.Tables["Staff_TB"].SetDataSource(ds.Tables[0]);
        //    srp.SetParameterValue("Month", fullMonthName);
        //    srp.SetParameterValue("Date", DateTime.Now.ToString("yyyy-MM-dd"));

        //    Stream str = srp.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        //    Response.Buffer = false;
        //    Response.ClearContent();
        //    Response.ClearHeaders();
        //    str.Seek(0, SeekOrigin.Begin);

        //    return File(str, "application/pdf");
        //}
    }
}