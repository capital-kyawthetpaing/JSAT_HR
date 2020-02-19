﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
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
                if(dtpay !=null && dtpay.Rows.Count>0)
                {
                    string jsonresult;
                    jsonresult = JsonConvert.SerializeObject(dtpay);
                    return jsonresult;
                }
            }
            return JsonConvert.SerializeObject(null);
        }

        public FileStreamResult PayRoll_Report(string id)
        {
            DataSet ds = new DataSet();
            string savefilename = "PayRoll_Report" + (DateTime.Now).ToShortDateString() + ".pdf";

            DataTable dt = new DataTable();
            dt = pbl.PayRoll_Search(id);
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

            Stream str = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            str.Seek(0, SeekOrigin.Begin);

            return File(str, "application/pdf");
        }
    }
}