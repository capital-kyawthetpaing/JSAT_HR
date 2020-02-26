using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Base_BL;
using JH_Model;
using Newtonsoft.Json;
using User_BL;


namespace JSAT_HR.Controllers
{
    public class UserController : Controller
    {
        UserBL ubl = new UserBL();
        DataTable dt = new DataTable();
        BaseBL bbl = new BaseBL();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Checklogin(UserModel um)
        {
            UserModel umodel = ubl.CheckLogin(um);
            if(umodel == null)
            {
                return RedirectToAction("Login", "Login", new { @errorId = 1 });
                
            }
            else
            {
                Session["UserID"] = um.UserID.ToString();
                Session["UserName"] = um.UserName.ToString();
               
                return RedirectToAction("StaffList", "Staff");
            }
        }
    }
}