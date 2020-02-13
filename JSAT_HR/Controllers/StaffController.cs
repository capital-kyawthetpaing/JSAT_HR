using System.Web.Mvc;
using CommonFunction;
using Staff_BL;
using JH_Model;

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
        public void Staff_Save(StaffModel model)
        {
            sbl.Staff_Save(model);

        }
    }
}