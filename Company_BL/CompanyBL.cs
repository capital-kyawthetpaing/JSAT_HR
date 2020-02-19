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
            JSAT_HREntities db = new JSAT_HREntities();
            M_Company company = new M_Company();
            company.CompanyCD = cm.CompanyCD;
            company.CompanyName = cm.CompanyName;
            company.InsertedDate = DateTime.Now;
            company.InsertedBy = HttpContext.Current.Session["UserID"].ToString();

            db.M_Company.Add(company);
            db.SaveChanges();
            string msg = "OK";
            return msg;
        }

        public async Task<string> Company_Update(CompanyModel cm)
        {
            JSAT_HREntities db = new JSAT_HREntities();
            string msg = string.Empty;
            M_Company update = await db.M_Company.Where(c => c.CompanyCD.Equals(cm.CompanyCD)).SingleOrDefaultAsync();
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

        public async Task<string> Check_Company(CompanyModel cm)
        {
            JSAT_HREntities db = new JSAT_HREntities();
            string msg = string.Empty;
            M_Company update = await db.M_Company.Where(c => c.CompanyCD.Equals(cm.CompanyCD)).SingleOrDefaultAsync();
            if (update != null)
                msg = update.CompanyCD;
            else
                msg = "";
            return msg;
        }
    }
}
