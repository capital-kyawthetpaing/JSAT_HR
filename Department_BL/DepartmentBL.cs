﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JH_DL;
using JH_Model;
using System.Data;
using System.Data.SqlClient;

namespace Department_BL
{
    public class DepartmentBL
    {
        public DataTable GetDeparment()
        {
            BaseDL bdl = new BaseDL();
            SqlParameter[] prms = new SqlParameter[0];
            return bdl.SelectData("M_Department_SelectAll", prms);
        }

      
    }
}
