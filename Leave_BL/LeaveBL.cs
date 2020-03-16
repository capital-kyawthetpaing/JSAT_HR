using System.Data;
using System.Data.SqlClient;
using JH_DL;

namespace Leave_BL
{
    public class LeaveBL
    {
        public DataTable M_Leave_Select(string option)
        {
            BaseDL bdl = new BaseDL();
            DataTable dtleave = new DataTable();
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@option", SqlDbType.Int) { Value = option };
            dtleave = bdl.SelectData("M_Leave_Select", prms);
            return dtleave;
        }
    }
}
