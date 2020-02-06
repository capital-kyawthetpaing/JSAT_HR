using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Staff_BL;
using CommonFunction;

namespace JSAT_HR.Controllers
{
    public class DepartmentController : Controller
    {
        DepartmentBL dp_bl = new DepartmentBL();
        // GET: Department
        public ActionResult DepartmentList()
        {
            return View();
        }
        [HttpGet]

        public string GetDepartment()
        {
            Function fun = new Function();
            return fun.DataTableToJSONWithJSONNet(dp_bl.GetDeparment());
        }
    }
}