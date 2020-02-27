using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Company_BL;
using CommonFunction;
using System.Threading.Tasks;
using JH_Model;

namespace JSAT_HR.Controllers
{
    public class CompanyController : Controller
    {
        CompanyBL cbl = new CompanyBL();
        // GET: Company
        public ActionResult Company_List()
        {
            String message= Session["Message"] as string;
            String msgSave = Session["MessageSave"] as string;
            ViewBag.mesg = message;
            ViewBag.mesgsave = msgSave;

            Session["Message"] = "";
            Session["MessageSave"] = "";
            return View();
        }

        [HttpGet]
        public string GETCompany()
        {
            Function fun = new Function();
            return fun.DataTableToJSONWithJSONNet(cbl.GETCompany());
        }

        public ActionResult Company_Save(CompanyModel cm)
        {
            string flag = string.Empty;
            CompanyBL cbl = new CompanyBL();
            if(cm != null)
            {
                var company_CD = cbl.Check_Company(cm);
                if(company_CD == "")
                {
                   flag= cbl.Company_Save(cm);
                    if (flag == "OK")
                    {
                        Session["MessageSave"] = "OK";
                    }
                    else
                    {

                    }
                    
                }
                else
                {
                    flag = cbl.Company_Update(cm);

                    if (flag == "OK")
                    {
                        Session["Message"] = "OK";
                    }
                    else
                    {
                        Session["Message"] = "NOT OK";
                    }
                }              
            }
            return RedirectToAction("Company_List");
        }
    }
}