﻿using System;
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
            return View();
        }

        [HttpGet]
        public string GETCompany()
        {
            Function fun = new Function();
            return fun.DataTableToJSONWithJSONNet(cbl.GETCompany());
        }

        public async Task<ActionResult> Company_Save(CompanyModel cm)
        {
            string flag = string.Empty;
            CompanyBL cbl = new CompanyBL();
            if(cm != null)
            {
                var company_CD = await cbl.Check_Company(cm);
                if(company_CD == "")
                {
                    cbl.Company_Save(cm);
                }
                else
                {
                    flag = await cbl.Company_Update(cm);
                }              
            }
            return RedirectToAction("Company_List");
        }
    }
}