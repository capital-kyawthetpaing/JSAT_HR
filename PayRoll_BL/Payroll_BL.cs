using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JH_Model;
using JH_DL;
using System.Data.Entity;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace PayRoll_BL
{
    public class Payroll_BL
    {
        public DataTable PayRoll_Search(string yearmonth)
        {
            BaseDL bdl = new BaseDL();
            DataTable dtpay = new DataTable();
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@yearmonth", SqlDbType.VarChar) { Value = yearmonth };
            dtpay = bdl.SelectData("PayRoll_Search", prms);
            if (dtpay.Rows.Count > 0)
                return dtpay;
            else
                return null;
        }

        public Boolean PayRoll_Allowance_Insert(string yearmonth)
        {
            try
            {
                BaseDL bdl = new BaseDL();
                DataTable dtinfo = new DataTable();
                SqlParameter[] prms = new SqlParameter[1];
                prms[0] = new SqlParameter("@YYYYMM", SqlDbType.VarChar) { Value = yearmonth };
                bdl.InsertUpdateDeleteData("PayRoll_Allowance_InsertUpdate", prms);
                return true;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return false;
            }
        }


        public Boolean PayRoll_Deduction_Insert(string yearmonth)
        {
            try
            {
                BaseDL bdl = new BaseDL();
                DataTable dtinfo = new DataTable();
                SqlParameter[] prms = new SqlParameter[1];
                prms[0] = new SqlParameter("@YYYYMM", SqlDbType.VarChar) { Value = yearmonth };
                bdl.InsertUpdateDeleteData("PayRoll_Deduction_InsertUpdate", prms);
                return true;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return false;
            }
        }




    }
}
