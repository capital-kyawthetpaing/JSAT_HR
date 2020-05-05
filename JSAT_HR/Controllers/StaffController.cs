using System.Web.Mvc;
using CommonFunction;
using Staff_BL;
using JH_Model;
using JH_DL;
using System.Linq;
using Newtonsoft.Json;
using System;
using System.Web;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace JSAT_HR.Controllers
{
    public class StaffController : Controller
    {
        string StaffPhoto = ConfigurationManager.AppSettings["StaffPhoto"].ToString();
        StaffBL sbl = new StaffBL();
        // GET: Staff
        public ActionResult StaffList()
        {
            if(Session["UserID"]!=null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult StaffEntry(string id)
        {
            StaffModel sm = new StaffModel();
            if (!string.IsNullOrWhiteSpace(id))
            {
                sm.StaffID = id;
               // sm.Mode = "update";
                ViewBag.Staffid = id;
                sm = sbl.SearchStaff(sm);                
            }
            else
            {
                //sm.Mode = "save";
                ViewBag.Staffid = "";
                sm.Photo = "Default.png";
            }
            return View(sm);
        }

        [HttpGet]
        public string GetStaff(string id)
        {
            Function fun = new Function();
            return fun.DataTableToJSONWithJSONNet(sbl.GetAllStaff(id));
        }

        [HttpPost]
        public ActionResult Staff_Save(StaffModel model)
        {
            try
            {
                string msg = string.Empty;
                if ( model.Mode == null)
                {
                    model.Mode = "save";
                    bool exists = sbl.StaffExists(model);
                    if (!exists)
                    {
                        HttpPostedFileBase imgfile = Request.Files["imgdata"];
                        string photoname = string.Empty;
                        photoname = imgfile.FileName;
                        if (!string.IsNullOrWhiteSpace(photoname))
                        {
                            if (!Directory.Exists(StaffPhoto))
                            {
                                Directory.CreateDirectory(StaffPhoto);
                            }
                            string path = StaffPhoto + model.StaffID + Path.GetExtension(photoname);
                            imgfile.SaveAs(path);
                            model.Photo = model.StaffID + Path.GetExtension(photoname);
                        }
                        else
                        {
                            model.Photo = "Default.png";
                        }
                        msg = sbl.Staff_Save(model);
                        TempData["Smsg"] = msg;
                        return RedirectToAction("StaffList");
                    }
                    else
                    {
                      
                        TempData["Imsg"] = "Duplicate";
                        return RedirectToAction("StaffEntry");
                    }
                }
                else
                {
                    HttpPostedFileBase imgfile = Request.Files["imgdata"];
                    string photoname = string.Empty;
                    photoname = imgfile.FileName;
                    if (!string.IsNullOrWhiteSpace(photoname))
                    {
                        if (!Directory.Exists(StaffPhoto))
                        {
                            Directory.CreateDirectory(StaffPhoto);
                        }
                        string path = StaffPhoto + model.StaffID + Path.GetExtension(photoname);
                        imgfile.SaveAs(path);
                        model.Photo= model.StaffID + Path.GetExtension(photoname);
                    }
                    msg = sbl.Staff_Update(model);
                    return RedirectToAction("StaffList");
                }
            }
            catch (Exception ex)
            {
                string st = ex.ToString();
                return RedirectToAction("StaffEntry");
            }
        }       
    }
}