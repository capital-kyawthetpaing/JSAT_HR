using System;
using System.Collections.Generic;
using System.Data;
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
    }
}