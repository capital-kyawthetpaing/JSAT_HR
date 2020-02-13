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
            DepartmentBL dbl = new DepartmentBL();
            if(model !=null)
            {
                dbl.Department_Save(model);
            }
            return RedirectToAction("DepartmentList");
        }


        public async Task<ActionResult> Smart_Template_New_Edit(string id)
        {
            JSAT_HREntities context = new JSAT_HREntities();
            DempartmentModel model = new DempartmentModel();
            DepartmentBL dbl = new DepartmentBL();
            model = await dbl.DepartmentEdit(id);
            return View(model);
        }
    }
}