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
            String message = Session["Message"] as string;
            String msgSave = Session["MessageSave"] as string;
            ViewBag.mesg = message;
            ViewBag.mesgsave = msgSave;
            Session["Message"] = "";
            Session["MessageSave"] = "";
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
                //var dept = deptCD.Split(',');
                //var cd = dept[0].ToString();
                //var name = dept[1].ToString();

                if (deptCD == "")
                {
                    flag = dbl.Department_Save(model);

                    if (flag == "OK")
                    {
                        Session["MessageSave"] = "OK";
                    }
                    else
                    {

                    }

                }
                else
                {
                    flag = dbl.Department_Update(model);
                    if (flag == "OK")
                    {
                        Session["Message"] = "OK";
                    }
                    else
                    {
                        Session["Message"] = "NOT OK";
                    }
                }
            }
            return RedirectToAction("DepartmentList");
        }
    }
}

