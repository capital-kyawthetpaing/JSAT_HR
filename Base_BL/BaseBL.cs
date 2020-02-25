using JH_DL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_BL
{
    public class BaseBL
    {
        DataTable dt = new DataTable();
        BaseDL dl = new BaseDL();
        public String _MessageDialog(string id)
        {
            var ID = string.Empty;
            var key = string.Empty;
            var msgType = string.Empty;
            if (id != null)
            {
                var msgarr = id.Split('_');
                ID = msgarr[0];
                key = msgarr[1];
                msgType = msgarr[2];

            }
            SqlParameter[] prm = new SqlParameter[2];
            prm[0] = new SqlParameter("@key", SqlDbType.VarChar) { Value = key };
            prm[1] = new SqlParameter("@msgType", SqlDbType.VarChar) { Value = ID };
            dt = dl.SelectData("HR_Message_Select", prm);
            var msg = dt.Rows[0][msgType].ToString();
            return msg;
        }
    }

}
    