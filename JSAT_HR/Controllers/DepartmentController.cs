using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Department_BL;
using CommonFunction;
using JH_Model;
using System.Data;
using Newtonsoft.Json;
using System.Threading.Tasks;

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
            string flag = string.Empty;
            DepartmentBL dbl = new DepartmentBL();
            if(model!=null)
            {
                var deptCD = dbl.Check_DeptCD(model);
                if (deptCD == "")
                {
                    dbl.Department_Save(model);
                }
                else
                {
                    flag = dbl.Department_Update(model);
                }
            }
            return RedirectToAction("DepartmentList");
        }
    }
}