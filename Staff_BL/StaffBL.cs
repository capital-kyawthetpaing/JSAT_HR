using System.Data;
using System.Data.SqlClient;
using JH_DL;

namespace Staff_BL
{
    public class StaffBL
    {
        public DataTable GetAllStaff()
        {
            BaseDL bdl = new BaseDL();
            SqlParameter[] prms = new SqlParameter[0];
            //prms[0] = new SqlParameter("@StaffCD", SqlDbType.VarChar) { Value = sm.StaffCD };
            //prms[1] = new SqlParameter("@StaffName", SqlDbType.VarChar) { Value = sm.StaffName };
            return bdl.SelectData("M_Staff_SelectAll", prms);
        }
    }
}
