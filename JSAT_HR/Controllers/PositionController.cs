using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Position_BL;
using CommonFunction;
using JH_Model;
using System.Threading.Tasks;

namespace JSAT_HR.Controllers
{
    public class PositionController : Controller
    {
        PositionBL pbl = new PositionBL();
        // GET: Position
        public ActionResult PositionList()
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
        public string GETPosition()
        {
            Function fun = new Function();
            return fun.DataTableToJSONWithJSONNet(pbl.GETPosition());
        }

        public ActionResult Position_Save(PositionModel pm)
        {
            string flag = string.Empty;
            PositionBL pbl = new PositionBL();
            if (pm != null)
            {
                var position_CD = pbl.Check_Position(pm);
                if (position_CD == "")
                {
                    flag = pbl.Position_Save(pm);
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
                    flag = pbl.Position_Update(pm);
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
            return RedirectToAction("PositionList");
        }
    }
}