using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using JH_DL;
using System.Data.SqlClient;

namespace Position_BL
{
    public class PositionBL
    {
        public DataTable GETPosition()
        {
            BaseDL bdl = new BaseDL();
            SqlParameter[] prms = new SqlParameter[0];
            return bdl.SelectData("M_Position_SelectAll", prms);
        }
    }
}
