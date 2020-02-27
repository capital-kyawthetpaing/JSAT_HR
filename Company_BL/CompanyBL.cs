using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JH_DL;
using JH_Model;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Web;


namespace Company_BL
{
    public class CompanyBL
    {
        public DataTable GETCompany()
        {
            BaseDL bdl = new BaseDL();
            SqlParameter[] prms = new SqlParameter[0];
            return bdl.SelectData("M_Company_SelectAll", prms);
        }

        public string Company_Save(CompanyModel cm)
        {
            string updatedby = string.Empty;
            updatedby = HttpContext.Current.Session["UserID"].ToString();
            updatedby = updatedby.Split('_')[0];

            JSAT_HREntities db = new JSAT_HREntities();
            string msg = string.Empty;
            M_Company company = new M_Company();
            company.CompanyCD = cm.CompanyCD;
            company.CompanyName = cm.CompanyName;
            company.InsertedDate = DateTime.Now;
            company.InsertedBy = updatedby;

            //db.M_Company.Add(company);
            //db.SaveChanges();
            //string msg = "OK";
            //return msg;

            try
            {
                db.M_Company.Add(company);
                db.SaveChanges();
                msg = "OK";
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }
            return msg;
        }

    //}

    public string Company_Update(CompanyModel cm)
        {
            JSAT_HREntities db = new JSAT_HREntities();
            string msg = string.Empty;
            M_Company update = db.M_Company.Where(c => c.CompanyCD.Equals(cm.CompanyCD)).SingleOrDefault();
            update.CompanyCD = cm.CompanyCD;
            update.CompanyName = cm.CompanyName;
            update.UpdatedDate = DateTime.Now;
            update.UpdatedBy = HttpContext.Current.Session["UserID"].ToString();

             try
            {
                db.SaveChanges();
                msg = "OK";
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }
            return msg;
        }

        public string Check_Company(CompanyModel cm)
        {
            JSAT_HREntities db = new JSAT_HREntities();
            string msg = string.Empty;
            M_Company update = db.M_Company.Where(c => c.CompanyCD.Equals(cm.CompanyCD)).SingleOrDefault();
            if (update != null)
                msg = update.CompanyCD;
            else
                msg = "";
            return msg;
        }
    }
}
