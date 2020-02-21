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

        public ActionResult StaffEntry(string id)
        {
            StaffModel sm = new StaffModel();
            if (!string.IsNullOrWhiteSpace(id))
            {
                sm.StaffID = id;
                sm = sbl.SearchStaff(sm);
                return View(sm);
            }
            else 
                return View();
        }

        [HttpGet]
        public string GetStaff()
        {
            Function fun = new Function();
            return fun.DataTableToJSONWithJSONNet(sbl.GetAllStaff());
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Staff_SaveAsync(StaffModel model)
        {
            try
            {
                string msg = string.Empty;
                var id = await sbl.Check_StaffCD(model);
                if (id == "")
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