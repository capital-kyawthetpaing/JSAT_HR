using System.Web.Mvc;
using CommonFunction;
using Staff_BL;

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

        [HttpGet]
        public string GetStaff()
        {
            Function fun = new Function();
            return fun.DataTableToJSONWithJSONNet(sbl.GetAllStaff());
        }
    }
}