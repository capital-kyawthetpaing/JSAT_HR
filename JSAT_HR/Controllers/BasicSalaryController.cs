﻿using CommonFunction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BasicSalary_BL;
using System.Threading.Tasks;
using JH_Model;

namespace JSAT_HR.Controllers
{
    public class BasicSalaryController : Controller
    {
        BasicSalaryBL bsbl = new BasicSalaryBL();
        // GET: BasicSalary
        public ActionResult BasicSalary()
        {
            return View();
        }

        [HttpGet]
        public string GETBasicSalary()
        {
            Function fun = new Function();
            return fun.DataTableToJSONWithJSONNet(bsbl.GETBasicSalary());
        }

        [HttpGet]
        public string GETBasicSalaryByID(string id)
        {
            Function fun = new Function();
            return fun.DataTableToJSONWithJSONNet(bsbl.GETBasicSalaryByID(id));
        }

        public ActionResult BasicSalary_Save(SalaryModel sm)
        {
            string flag =  bsbl.BasicSalary_Save(sm);

            return RedirectToAction("BasicSalary");
        }
    }
}