using CommonFunction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Office_BL;

namespace JSAT_HR.Controllers
{
    public class OfficeController : Controller
    {
        OfficeBL obl = new OfficeBL();
        // GET: Office
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public string GETOffice()
        {
            Function fun = new Function();
            return fun.DataTableToJSONWithJSONNet(obl.GETOffice());
        }
    }
}