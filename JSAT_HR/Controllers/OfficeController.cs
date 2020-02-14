using CommonFunction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Office_BL;
using JH_Model;
using System.Threading.Tasks;

namespace JSAT_HR.Controllers
{
    public class OfficeController : Controller
    {
        OfficeBL obl = new OfficeBL();
        // GET: Office
        public ActionResult Office_Hours_Setting()
        {
            return View();
        }
        [HttpGet]
        public string GETOffice()
        {
            Function fun = new Function();
            return fun.DataTableToJSONWithJSONNet(obl.GETOffice());
        }

        public async Task<ActionResult> Hours_Setting_Save(OfficeModel om)
        {
            string flag = await obl.Hours_Setting_Save(om);

            return RedirectToAction("Office_Hours_Setting");
        }
    }
}