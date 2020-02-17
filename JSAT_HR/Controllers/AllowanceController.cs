﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JH_Model;
using JH_DL;
using Allowance_BL;
using CommonFunction;
using System.Threading.Tasks;

namespace JSAT_HR.Controllers
{
    public class AllowanceController : Controller
    {
        Allowance_SettingBL asbl = new Allowance_SettingBL();
        // GET: Allowance
        public ActionResult Allowance_Setting(int id)
        {
            if(id==1)
            {
                ViewBag.Data = "MMK";
            }
            else
            {
                ViewBag.Data = "USD";
            }
            AllowanceModel am = new AllowanceModel();
            am.Currency = id;
            asbl.GetAllowance(am);
            return View(am);
        }
        public async Task<ActionResult> Allowance_Setting_Save(AllowanceModel model)
        {
            string flag = string.Empty;
            if (model != null)
            {
                await asbl.Allowance_Setting_Save(model);
            }
            return RedirectToAction("Allowance_Setting");
        }


    }
}