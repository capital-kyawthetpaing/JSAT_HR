using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Position_BL;
using CommonFunction;

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
    }
}