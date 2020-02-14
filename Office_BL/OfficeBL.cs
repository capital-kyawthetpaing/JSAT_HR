using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using JH_DL;
using JH_Model;


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

        public async Task<string> Hours_Setting_Save(OfficeModel am)
        {

            string msg = string.Empty;
            JSAT_HREntities db = new JSAT_HREntities();
            

            M_Office tb = await db.M_Office.Where(s => s.OfficeCD.Equals("1")).SingleOrDefaultAsync();
            tb.OfficeHourFrom = TimeSpan.Parse(am.OfficeTimeIn);
            tb.OfficeHourTo = TimeSpan.Parse(am.OfficeTimeOut);
            db.M_Office.Add(tb);
            db.SaveChanges();

            M_Office tb1 = await db.M_Office.Where(s => s.OfficeCD.Equals("2")).SingleOrDefaultAsync();
            tb1.OfficeHourFrom = TimeSpan.Parse(am.AcademyTimeIn);
            tb1.OfficeHourTo = TimeSpan.Parse(am.AcademyTimeOut);
            db.M_Office.Add(tb1);
            db.SaveChanges();

            msg = "OK";
            return msg;


        }
    }
}
