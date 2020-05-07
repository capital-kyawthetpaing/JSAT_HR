using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommonFunction;
using FastMember;
using JH_DL;
using JH_Model;
using Newtonsoft.Json;
using User_BL;


namespace JSAT_HR.Controllers
{
    public class UserController : Controller
    {
        UserBL ubl = new UserBL();
        DataTable dt = new DataTable();
       
        // GET: User
        public ActionResult Checklogin(UserModel um)
        {
            UserModel umodel = ubl.CheckLogin(um);
            if(umodel == null)
            {
                return RedirectToAction("Login", "Login", new { @errorId = 1 });
                
            }
            else
            {
                Session["UserID"] = um.UserID.ToString() + "_" + um.UserName.ToString();             
                return RedirectToAction("StaffList", "Staff");
            }
        }

        public ActionResult User_List()
        {
            return View();
        }

        public ActionResult User_Entry(string id)
        {
            String Emsg = Session["Emsg"] as string;
            ViewBag.Emsg = Emsg;
            Session["Emsg"] = "";

            if (!string.IsNullOrWhiteSpace(id))
            {
                ViewBag.Userid = id;
                id = null;
                return View();
            }
            else
            {
                ViewBag.Userid = "";
                return View();
            }
        }

        [HttpGet]
        public string GETUser()
        {
            Function fun = new Function();
            return fun.DataTableToJSONWithJSONNet(ubl.GetAllUser());
        }

        [HttpPost]
        public string _MViewBind(string id)
        {
            string jsonresult;
            DataTable dt = ubl.M_View_Select();

            dt.Columns.Add("Total", typeof(System.Int32));
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    dr["Total"] = dt.Rows.Count;
                }
                jsonresult = JsonConvert.SerializeObject(dt);
                return jsonresult;
            }
            else
                return JsonConvert.SerializeObject(dt);

        }

        [HttpPost]
        public string _MViewBind_ForEdit(string id)
        {
            string jsonresult;
            DataTable dt = ubl.M_View_Select_ForEdit(id);

            dt.Columns.Add("Total", typeof(System.Int32));
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    dr["Total"] = dt.Rows.Count;
                }
                jsonresult = JsonConvert.SerializeObject(dt);
                return jsonresult;
            }
            else
                return JsonConvert.SerializeObject(dt);

        }

        public ActionResult User_Save(MultiModel model)
        {
            try
            {
                string msg = string.Empty;
                if (model.userModel.SaveUpdateFlag == null)
                {
                    model.userModel.SaveUpdateFlag = "Save";
                    bool exists = ubl.UserExists(model);
                    if (!exists)
                    {
                        DataTable dtUserlist = new DataTable();
                        using (var reader = ObjectReader.Create(model.UserlistModel, "ViewID", "ShowView", "IsReadOnly"))
                        {
                            dtUserlist.Load(reader);
                        }
                        if (dtUserlist.Rows.Count > 0)
                        {
                            ubl.Update_User_Authorization(dtUserlist, model);
                        }
                        return RedirectToAction("User_List");
                    }
                    else
                    {
                        Session["Emsg"] = "Duplicate";
                        return RedirectToAction("User_Entry");
                    }
                }
                else
                {
                    DataTable dtUserlist = new DataTable();
                    using (var reader = ObjectReader.Create(model.UserlistModel, "ViewID", "ShowView", "IsReadOnly"))
                    {
                        dtUserlist.Load(reader);
                    }
                    if (dtUserlist.Rows.Count > 0)
                    {
                        ubl.Update_User_Authorization(dtUserlist, model);
                    }
                    return RedirectToAction("User_List");
                }                                       
                
            }
            catch (Exception ex)
            {
                string st = ex.ToString();               
                return RedirectToAction("User_List");
            }
        }


        public string UserRead(string id)
        {            
            Function fun = new Function();
            return fun.DataTableToJSONWithJSONNet(ubl.UserRead(id));
        }

        public string UserCheck(string id)
        {
            Function fun = new Function();
            return fun.DataTableToJSONWithJSONNet(ubl.UserCheck(id));
        }
    }
}