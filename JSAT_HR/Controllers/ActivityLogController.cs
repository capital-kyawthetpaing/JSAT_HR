using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommonFunction;
using System.Threading.Tasks;
using JH_Model;
using ActivityLog_BL;

namespace JSAT_HR.Controllers
{
    public class ActivityLogController : Controller
    {
        Activity_Log_BL albl = new Activity_Log_BL();
        // GET: ActivityLog
        public ActionResult Activity_Log()
        {
            return View();
        }

        [HttpGet]
        public string GETL_Log()
        {
            Function fun = new Function();
            return fun.DataTableToJSONWithJSONNet(albl.GETL_Log());
        }
    }
}