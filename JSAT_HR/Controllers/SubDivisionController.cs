using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SubDivision_BL;
using CommonFunction;
using System.Threading.Tasks;
using JH_Model;

namespace JSAT_HR.Controllers
{
    public class SubDivisionController : Controller
    {
        SubDivisionBL sdbl = new SubDivisionBL();

        // GET: SubDivision
        public ActionResult SubDivisionList()
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
        public string GETSubDivision()
        {
            Function fun = new Function();
            return fun.DataTableToJSONWithJSONNet(sdbl.GETSubDivision());
        }

        public ActionResult SubDivision_Save(SubDivisionModel model)
        {
            string flag = string.Empty;
            if (model != null)
            {
                var subDivCD = sdbl.Check_SubDivisionCD(model);
                if (subDivCD == "")
                {
                    flag = sdbl.SubDivision_Save(model);
                    if (flag == "OK")
                    {
                        Session["MessageSave"] = "OK";
                    }
                    else
                    {
                        Session["MessageSave"] = "NOT OK";
                    }
                }
                else
                {
                    flag = sdbl.SubDivision_Update(model);
                    if (flag == "OK")

                    {
                        Session["Message"] = "OK";

                    }
                    else
                    {
                        Session["Message"] = "Not OK";
                    }
                }
            }
            return RedirectToAction("SubDivisionList");
        }

        public string SubDivisionCheck(string id)
        {
            Function fun = new Function();
            return fun.DataTableToJSONWithJSONNet(sdbl.SubDivisionCheck(id));
        }
    }
}