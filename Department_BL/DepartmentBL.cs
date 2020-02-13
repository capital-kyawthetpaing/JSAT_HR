﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JH_DL;
using JH_Model;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity;

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

        public string Department_Save(DempartmentModel model)
        {
            JSAT_HREntities db = new JSAT_HREntities();
            M_Department md = new M_Department();

            md.DepartmentCD = model.DepartmentCD;
            md.Department = model.DepartmentName;

            db.M_Department.Add(md);
            db.SaveChanges();

            string msg = "OK";
            return msg;
        }

        public async Task<DempartmentModel> DepartmentEdit(string deptcd)
        {
            JSAT_HREntities context = new JSAT_HREntities();
            DempartmentModel dm = new DempartmentModel();
            M_Department dept = await context.M_Department.Where(s => s.DepartmentCD == deptcd).SingleOrDefaultAsync();
            dm.DepartmentCD = dept.DepartmentCD;
            dm.DepartmentName = dept.Department;
            return dm;
        }
    }
}
