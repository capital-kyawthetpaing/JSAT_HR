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
                    pbl.Position_Save(pm);
                }
                else
                {
                    flag = pbl.Position_Update(pm);
                }
            }
            return RedirectToAction("PositionList");
        }
    }
}