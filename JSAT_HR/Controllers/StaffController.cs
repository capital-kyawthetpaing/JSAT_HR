using System.Web.Mvc;
using CommonFunction;
using Staff_BL;
using JH_Model;
using JH_DL;
using System.Linq;
using Newtonsoft.Json;
using System;

namespace JSAT_HR.Controllers
{
    public class StaffController : Controller
    {
        StaffBL sbl = new StaffBL();
        // GET: Staff
        public ActionResult StaffList()
        {
            if(Session["UserID"]!=null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult StaffEntry()
        {
            return View();
        }

        [HttpGet]
        public string GetStaff()
        {
            Function fun = new Function();
            return fun.DataTableToJSONWithJSONNet(sbl.GetAllStaff());
        }

        [HttpPost]
        public ActionResult Staff_Save(StaffModel model)
        {
            try
            {
                string msg = string.Empty;
                JSAT_HREntities db = new JSAT_HREntities();
                var id = db.M_Staff.Where(ms => ms.StaffID.Equals(model.StaffID)).Select(s => s.StaffID).FirstOrDefault();
               
                if (id == 0)
                {
                    msg = sbl.Staff_Save(model);
                    TempData["Smsg"] = msg;
                    return RedirectToAction("StaffList");
                }
                else
                {
                    TempData["Imsg"] = "Duplicate";
                    return RedirectToAction("StaffEntry");
                }
            }
            catch (Exception ex)
            {
                string st = ex.ToString();
                return RedirectToAction("StaffEntry");
            }
        }       
    }
}