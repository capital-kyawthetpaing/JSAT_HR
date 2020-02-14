using System.Web.Mvc;
using CommonFunction;
using Staff_BL;
using JH_Model;
using JH_DL;
using System.Linq;

namespace JSAT_HR.Controllers
{
    public class StaffController : Controller
    {
        StaffBL sbl = new StaffBL();
        // GET: Staff
        public ActionResult StaffList()
        {
            return View();
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
            string msg = string.Empty;
            JSAT_HREntities db = new JSAT_HREntities();
            var id = db.M_Staff.Where(ms => ms.StaffID.Equals(model.StaffID)).Select(s => s.StaffID).FirstOrDefault();
            if (id == 0)
            {
                msg=sbl.Staff_Save(model);
                TempData["msg"] = msg;
                return RedirectToAction("StaffList");
            }
            else
            {
                return RedirectToAction("StaffList");
            }

        }
    }
}