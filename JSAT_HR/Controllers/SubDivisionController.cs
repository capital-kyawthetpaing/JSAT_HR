using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SubDivision_BL;
using CommonFunction;
using System.Threading.Tasks;
using JH_Model;

namespace JSAT_HR.Controllers
{
    public class SubDivisionController : Controller
    {
        SubDivisionBL sdbl = new SubDivisionBL();

        // GET: SubDivision
        public ActionResult SubDivisionList()
        {
            return View();
        }

        [HttpGet]
        public string GETSubDivision()
        {
            Function fun = new Function();
            return fun.DataTableToJSONWithJSONNet(sdbl.GETSubDivision());
        }

        public async Task<ActionResult> SubDivision_Save(SubDivisionModel model)
        {
            string flag = string.Empty;
            if (model != null)
            {
                var subDivCD = await sdbl.Check_SubDivisionCD(model);
                if (subDivCD == "")
                {
                    sdbl.SubDivision_Save(model);
                }
                else
                {
                    flag = await sdbl.SubDivision_Update(model);
                }
            }
            return RedirectToAction("SubDivisionList");
        }
    }
}