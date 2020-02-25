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
                sm.Mode = "update";
                sm = sbl.SearchStaff(sm);                
            }
            else
            {
                sm.Mode = "save";
            }
            return View(sm);
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
                bool exists = sbl.StaffExists(model);
                if (id == "" && model.Mode == "save")
                {
                    msg = sbl.Staff_Save(model);
                    TempData["Smsg"] = msg;
                    return RedirectToAction("StaffList");
                }
                else if(id != "" && model.Mode == "update")
                {
                    msg = sbl.Staff_Update(model);
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