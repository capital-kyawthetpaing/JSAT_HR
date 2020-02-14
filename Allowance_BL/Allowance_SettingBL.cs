using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JH_Model;
using JH_DL;
using System.Data.Entity;
using System.Web;

namespace Allowance_BL
{
    public class Allowance_SettingBL
    {

        public string Allowance_Setting_Save(AllowanceModel am)
        {
            
            string msg = string.Empty;
            JSAT_HREntities db = new JSAT_HREntities();
            M_Allowance tb = new M_Allowance();

            tb.ChangeDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            tb.MD = am.MD;
            tb.Manager = am.Manager;
            tb.Director = am.Director;
            tb.N1 = am.N1;
            tb.N2 = am.N2;
            tb.N3 = am.N3;
            tb.JpnUniGrade = am.JpnUniGrade;
            tb.Bus = am.Bus;
            tb.Train = am.Train;
            tb.MarketingTeamAllowance = am.MarketingTeamAllowance;
            tb.MentorAllowance = am.MentorAllowance;
            tb.Local1stInterviewer = am.Local1stInterviewer;
            tb.Local2ndInterviewer = am.Local2ndInterviewer;
            tb.Overseas1stInterviewer = am.Oversea1stInterviewer;
            tb.Overseas2ndInterviewer = am.Oversea2ndInterviewer;
            tb.OfficeTimeIn = TimeSpan.Parse(am.OfficeTimeIn);
            tb.OfficeTimeOut = TimeSpan.Parse(am.OfficeTimeOut);
            tb.AcademyTimeIn = TimeSpan.Parse(am.AcademyTimeIn);
            tb.AcademyTimeOut = TimeSpan.Parse(am.AcademyTimeOut);
            tb.InsertedDate = DateTime.Now;
            tb.InsertedBy = HttpContext.Current.Session["UserID"].ToString();

            db.M_Allowance.Add(tb);
            db.SaveChanges();

            msg = "OK";
            return msg;


        }
    }
}
