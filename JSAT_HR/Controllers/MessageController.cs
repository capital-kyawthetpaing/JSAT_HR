using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommonFunction;

namespace JSAT_HR.Controllers
{
    public class MessageController : Controller
    {
        // GET: Message
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetMessage(string id)
        {
            Function fun = new Function();
            return Json(new { message = fun.GetMessage(id) });
        }
    }
}