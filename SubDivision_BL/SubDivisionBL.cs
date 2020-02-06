using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JH_DL;
using System.Data;
using System.Data.SqlClient;

namespace SubDivision_BL
{
    public class SubDivisionBL
    {
        public DataTable GETSubDivision()
        {
            BaseDL bdl = new BaseDL();
            SqlParameter[] prms = new SqlParameter[0];
            return bdl.SelectData("M_SubDivision_SelectAll", prms);
        }
    }
}
