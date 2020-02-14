using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Company_BL;
using CommonFunction;

namespace JSAT_HR.Controllers
{
    public class CompanyController : Controller
    {
        CompanyBL cbl = new CompanyBL();
        // GET: Company
        public ActionResult Company_List()
        {
            return View();
        }

        [HttpGet]
        public string GETCompany()
        {
            Function fun = new Function();
            return fun.DataTableToJSONWithJSONNet(cbl.GETCompany());
        }
    }
}