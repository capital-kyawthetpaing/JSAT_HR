using System;
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
            String UImsg = Session["UImsg"] as string;
            String UEmsg = Session["UEmsg"] as string;
            ViewBag.UImsg = UImsg;
            ViewBag.UEmsg = UEmsg;
            Session["UImsg"] = "";
            Session["UEmsg"] = "";
            if (id == 1)
            {
                ViewBag.Data = "MMK";
            }
            else
            {
                ViewBag.Data = "USD";
            }
            AllowanceModel am = new AllowanceModel();
            am.Currency = Convert.ToByte(id);
            asbl.GetAllowance(am);
            return View(am);
        }
        public ActionResult Allowance_Setting_Save(AllowanceModel model)
        {
            try
            {
                string flag = string.Empty;
                if (model != null)
                {
                    flag = asbl.Allowance_Setting_Save(model);
                }
                if(flag=="OK")
                {
                    Session["UImsg"] = "OK";
                }
                return RedirectToAction("Allowance_Setting", "Allowance", new { @id = model.Currency });
            }
            catch (Exception ex)
            {
                string st = ex.ToString();
                Session["UEmsg"] = "NotOK";
                return RedirectToAction("Allowance_Setting", "Allowance", new { @id = model.Currency });
            }
        }


    }
}