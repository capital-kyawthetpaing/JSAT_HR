using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Department_BL;
using CommonFunction;
using JH_Model;

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

        public ActionResult Department_Save(DempartmentModel model)
        {
            DepartmentBL dbl = new DepartmentBL();
            if(model !=null)
            {
                dbl.Department_Save(model);
            }
            return RedirectToAction("DepartmentList");
        }
    }
}