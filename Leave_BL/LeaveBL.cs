using System.Data;
using System.Data.SqlClient;
using JH_DL;

namespace Leave_BL
{
    public class LeaveBL
    {
        public DataTable M_Leave_Select()
        {
            BaseDL bdl = new BaseDL();
            DataTable dtleave = new DataTable();
            SqlParameter[] prms = new SqlParameter[0];
            dtleave = bdl.SelectData("M_Leave_Select", prms);
            return dtleave;
        }
    }
}
