using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JH_DL;
using JH_Model;

namespace BasicSalary_BL
{
    public class BasicSalaryBL
    {
        public DataTable GETBasicSalary()
        {
            BaseDL bdl = new BaseDL();
            SqlParameter[] prms = new SqlParameter[0];
            return bdl.SelectData("M_Salary_SelectAll", prms);
        }
        public DataTable GETBasicSalaryByID(string id)
        {
            BaseDL bdl = new BaseDL();
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@Currency", SqlDbType.Int) { Value = Convert.ToInt32(id) };
            return bdl.SelectData("M_Salary_Select_ByID", prms);
        }

        public string BasicSalary_Save(SalaryModel sm)
        {

            string msg = string.Empty;
            JSAT_HREntities db = new JSAT_HREntities();


            M_Salary tb1 = db.M_Salary.Where(s => s.Currency.Equals(1) && s.Status.Equals(1)).SingleOrDefault();

            if (!String.IsNullOrWhiteSpace(sm.MMKPermanentSalary))
                tb1.Amount = Convert.ToDecimal(sm.MMKPermanentSalary);
            db.SaveChanges();

            M_Salary tb2 = db.M_Salary.Where(s => s.Currency.Equals(1) && s.Status.Equals(2)).SingleOrDefault();
            if (!String.IsNullOrWhiteSpace(sm.MMKProvisionSalary))
                tb2.Amount =Convert.ToDecimal(sm.MMKProvisionSalary);
            db.SaveChanges();


            M_Salary tb3 = db.M_Salary.Where(s => s.Currency.Equals(2) && s.Status.Equals(1)).SingleOrDefault();
            if (!String.IsNullOrWhiteSpace(sm.USDPermanentSalary))
                tb3.Amount = Convert.ToDecimal(sm.USDPermanentSalary);
            db.SaveChanges();

            M_Salary tb4 = db.M_Salary.Where(s => s.Currency.Equals(2) && s.Status.Equals(2)).SingleOrDefault();
            if (!String.IsNullOrWhiteSpace(sm.USDProvisionSalary))
                tb4.Amount = Convert.ToDecimal(sm.USDProvisionSalary);
            db.SaveChanges();

            msg = "OK";
            return msg;

        }
    }
}
