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
        public DataTable GETL_Log()
        {
            BaseDL bdl = new BaseDL();
            SqlParameter[] prms = new SqlParameter[0];
            return bdl.SelectData("M_Company_SelectAll", prms);
        }
    }
}
