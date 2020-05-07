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
            String Imsg = Session["Imsg"] as string;
            String Umsg = Session["Umsg"] as string;
            String EImsg = Session["EImsg"] as string;
            String EUmsg = Session["EUmsg"] as string;
            ViewBag.Imsg = Imsg;
            ViewBag.Umsg = Umsg;
            ViewBag.EImsg = EImsg;
            ViewBag.IUmsg = EUmsg;

            Session["Imsg"] = "";
            Session["Umsg"] = "";
            Session["EImsg"] = "";
            Session["EUmsg"] = "";
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
                        Session["Imsg"] = "OK";
                    }
                    else
                    {
                        Session["IEmsg"] = "NotoK";
                    }
                    
                }
                else
                {
                    flag = cbl.Company_Update(cm);

                    if (flag == "OK")
                    {
                        Session["Umsg"] = "OK";
                    }
                    else
                    {
                        Session["UEmsg"] = "NotoK";
                    }
                }              
            }
            return RedirectToAction("Company_List");
        }

        public string CompanyCheck(string id)
        {
            Function fun = new Function();
            return fun.DataTableToJSONWithJSONNet(cbl.CompanyCheck(id));
        }
    }
}