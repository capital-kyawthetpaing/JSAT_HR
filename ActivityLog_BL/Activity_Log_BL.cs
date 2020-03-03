using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using JH_DL;
using JH_Model;
using System.Data;
using System.Data.SqlClient;

namespace ActivityLog_BL
{
    public class Activity_Log_BL
    {
        public DataTable GETL_Log_ByID(string userid)
        {
            BaseDL bdl = new BaseDL();
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@UserID", SqlDbType.VarChar) { Value = userid };
            return bdl.SelectData("L_Log_SelectByID", prms);
        }
    }
}
