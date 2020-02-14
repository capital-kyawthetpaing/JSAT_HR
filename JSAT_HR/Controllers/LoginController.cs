using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JSAT_HR.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login(int? errorId)
        {
            if (errorId > 0)
                ViewBag.ErrorMessage = "Incorrect UserID or Password!";
            else
            {
                ViewBag.ErrorMessage = "";
            }
            return View();
        }
    }
}