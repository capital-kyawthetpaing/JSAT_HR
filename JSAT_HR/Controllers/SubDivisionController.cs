using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SubDivision_BL;
using CommonFunction;

namespace JSAT_HR.Controllers
{
    public class SubDivisionController : Controller
    {
        SubDivisionBL sdbl = new SubDivisionBL();

        // GET: SubDivision
        public ActionResult SubDivisionList()
        {
            return View();
        }

        [HttpGet]
        public string GETSubDivision()
        {
            Function fun = new Function();
            return fun.DataTableToJSONWithJSONNet(sdbl.GETSubDivision());
        }
    }
}