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

namespace Department_BL
{
    public class DepartmentBL
    {
        public DataTable GetDeparment()
        {
            BaseDL bdl = new BaseDL();
            SqlParameter[] prms = new SqlParameter[0];
            return bdl.SelectData("M_Department_SelectAll", prms);
        }

        public string Department_Save(DempartmentModel model)
        {
            JSAT_HREntities db = new JSAT_HREntities();
            M_Department md = new M_Department();

            md.DepartmentCD = model.DepartmentCD;
            md.Department = model.DepartmentName;
            md.InsertedDate = DateTime.Now;
            md.InsertedBy = HttpContext.Current.Session["UserID"].ToString();

            db.M_Department.Add(md);
            db.SaveChanges();

            string msg = "OK";
            return msg;
        }

        public string Department_Update(DempartmentModel model)
        {
            JSAT_HREntities db = new JSAT_HREntities();
            M_Department md = new M_Department();
            string msg = string.Empty;
            M_Department update = db.M_Department.Where(s => s.DepartmentCD.Equals(model.DepartmentCD)).SingleOrDefault();
            update.DepartmentCD = model.DepartmentCD;
            update.Department = model.DepartmentName;
            update.UpdatedDate = DateTime.Now;
            update.UpdatedBy = HttpContext.Current.Session["UserID"].ToString();
            try
            {
                db.SaveChanges();
                msg = "OK";
            }
            catch(Exception ex)
            {
                msg = ex.ToString();
            }
            return msg;
        }

        public string Check_DeptCD(DempartmentModel model)
        {
            JSAT_HREntities db = new JSAT_HREntities();
            M_Department md = new M_Department();
            string msg = string.Empty;
            M_Department update = db.M_Department.Where(s => s.DepartmentCD.Equals(model.DepartmentCD)).SingleOrDefault();
            if (update != null)
                msg = update.DepartmentCD;
            else
                msg = "";
           
            return msg;
        }

        //public async Task<DempartmentModel> DepartmentEdit(string deptcd)
        //{
        //    JSAT_HREntities context = new JSAT_HREntities();
        //    DempartmentModel dm = new DempartmentModel();
        //    M_Department dept = await context.M_Department.Where(s => s.DepartmentCD == deptcd).SingleOrDefaultAsync();
        //    dm.DepartmentCD = dept.DepartmentCD;
        //    dm.DepartmentName = dept.Department;
        //    return dm;
        //}
    }
}
