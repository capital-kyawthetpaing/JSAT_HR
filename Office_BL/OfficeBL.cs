using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JH_DL;

namespace Office_BL
{
    public class OfficeBL
    {
        public DataTable GETOffice()
        {
            BaseDL bdl = new BaseDL();
            SqlParameter[] prms = new SqlParameter[0];
            return bdl.SelectData("M_Office_SelectAll", prms);
        }
    }
}
