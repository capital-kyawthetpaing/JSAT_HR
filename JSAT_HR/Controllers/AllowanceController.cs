using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JH_Model;
using JH_DL;
using Allowance_BL;



namespace JSAT_HR.Controllers
{
    public class AllowanceController : Controller
    {
        Allowance_SettingBL asbl = new Allowance_SettingBL();
        // GET: Allowance
        public ActionResult Allowance_Setting()
        {
            return View();
        }

        public ActionResult Allowance_Setting_Save(AllowanceModel am)
        {
            asbl.Allowance_Setting_Save(am);

            return View("Allowance_Setting");
        }
    }
}