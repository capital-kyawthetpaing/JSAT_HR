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

        public string Hours_Setting_Save(OfficeModel am)
        {
            string msg = string.Empty;
            JSAT_HREntities db = new JSAT_HREntities();
            

            M_Office tb =  db.M_Office.Where(s => s.OfficeCD.Equals(2)).SingleOrDefault();
            if (!String.IsNullOrWhiteSpace(am.OfficeTimeIn))
                tb.OfficeHourFrom = TimeSpan.Parse(am.OfficeTimeIn);
            if (!String.IsNullOrWhiteSpace(am.OfficeTimeOut))
                tb.OfficeHourTo = TimeSpan.Parse(am.OfficeTimeOut);
           
            db.SaveChanges();

            M_Office tb1 =  db.M_Office.Where(s => s.OfficeCD.Equals(1)).SingleOrDefault();
            if(!String.IsNullOrWhiteSpace(am.AcademyTimeIn))
                tb1.OfficeHourFrom = TimeSpan.Parse(am.AcademyTimeIn);
            if (!String.IsNullOrWhiteSpace(am.AcademyTimeOut))
                tb1.OfficeHourTo = TimeSpan.Parse(am.AcademyTimeOut);
          
            db.SaveChanges();

            msg = "OK";
            return msg;


        }
    }
}
