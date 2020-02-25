using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using JH_DL;

namespace CommonFunction
{
    public class Function
    {
        public string DataTableToJSONWithJSONNet(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }

        public string GetMessage(string id)
        {
            BaseDL dl = new BaseDL();
            string ID = string.Empty;
            string key = string.Empty;
            if (id != null)
            {
                var msgarr = id.Split('_');
                ID = msgarr[0];
                key = msgarr[1];
            }

            SqlParameter[] prm = new SqlParameter[2];
            prm[0] = new SqlParameter("@ID", SqlDbType.VarChar) { Value = ID };
            prm[1] = new SqlParameter("@KEY", SqlDbType.VarChar) { Value = key };
            DataTable dt = dl.SelectData("M_Message_Select", prm);
            var msg = dt.Rows[0]["Message1"].ToString();
            return msg;
        }
    }
}
