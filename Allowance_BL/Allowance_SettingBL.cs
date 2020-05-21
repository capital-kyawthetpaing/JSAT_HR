using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JH_Model;
using JH_DL;
using System.Data.Entity;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace Allowance_BL
{
    public class Allowance_SettingBL
    {

        public string  Allowance_Setting_Save(AllowanceModel am)
        {
            string updatedby = string.Empty;
            updatedby = HttpContext.Current.Session["UserID"].ToString();
            updatedby = updatedby.Split('_')[0];

            string msg = string.Empty;
            JSAT_HREntities db = new JSAT_HREntities();
            M_Allowance tb =  db.M_Allowance.Where(s => s.Currency == am.Currency).SingleOrDefault();

            tb.ChangeDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            tb.Currency = am.Currency;
            tb.MD = Convert.ToDecimal(am.MD);
            tb.Manager = Convert.ToDecimal(am.Manager);
            tb.Director = Convert.ToDecimal(am.Director);
            tb.N1 = Convert.ToDecimal(am.N1);
            tb.N2 = Convert.ToDecimal(am.N2);
            tb.N3 = Convert.ToDecimal(am.N3);
            tb.JpnUniGrade = Convert.ToDecimal(am.JpnUniGrade);
            tb.MarketingTeamAllowance = Convert.ToDecimal(am.MarketingTeamAllowance);
            tb.MentorAllowance = Convert.ToDecimal(am.MentorAllowance);
            tb.Local1stInterviewer = Convert.ToDecimal(am.Local1stInterviewer);
            tb.Local2ndInterviewer = Convert.ToDecimal(am.Local2ndInterviewer);
            tb.Overseas1stInterviewer = Convert.ToDecimal(am.Oversea1stInterviewer);
            tb.Overseas2ndInterviewer = Convert.ToDecimal(am.Oversea2ndInterviewer);
            if(string.IsNullOrWhiteSpace(am.Maternity))
            {
                am.Maternity = "0";
                tb.Maternity = Convert.ToByte(am.Maternity);
            }
            else
            {
                if (am.Maternity.ToString().Contains(("%")))
                    am.Maternity = am.Maternity.Replace("%", "");
                tb.Maternity = Convert.ToByte(am.Maternity);
            }
           if(string.IsNullOrWhiteSpace(am.Medical))
            {
                am.Medical = "0";
                tb.Medical = Convert.ToByte(am.Medical);
            }
           else
            {
                if (am.Medical.ToString().Contains(("%")))
                    am.Medical = am.Medical.Replace("%", "");
                tb.Medical = Convert.ToByte(am.Medical);
            }

            tb.InsertedDate = DateTime.Now;
            tb.InsertedBy = updatedby;

            db.SaveChanges();

            msg = "OK";
            return msg;
        }


        public AllowanceModel GetAllowance(AllowanceModel am)
        {
            JSAT_HREntities db = new JSAT_HREntities();
            M_Allowance model = db.M_Allowance.Where(m => m.Currency == am.Currency).SingleOrDefault();

            if (model != null)
            {
                am.MD = String.Format("{0:n0}", model.MD);
                am.Director = String.Format("{0:n0}", model.Director);
                am.Manager = String.Format("{0:n0}", model.Manager);
                am.N1 = String.Format("{0:n0}", model.N1);
                am.N2 = String.Format("{0:n0}", model.N2);
                am.N3 = String.Format("{0:n0}", model.N3);
                am.JpnUniGrade = String.Format("{0:n0}", model.JpnUniGrade);
                am.MarketingTeamAllowance = String.Format("{0:n0}", model.MarketingTeamAllowance);
                am.MentorAllowance = String.Format("{0:n0}", model.MentorAllowance);
                am.Local1stInterviewer = String.Format("{0:n0}", model.Local1stInterviewer);
                am.Local2ndInterviewer = String.Format("{0:n0}", model.Local2ndInterviewer);
                am.Oversea1stInterviewer = String.Format("{0:n0}", model.Overseas1stInterviewer);
                am.Oversea2ndInterviewer = String.Format("{0:n0}", model.Overseas2ndInterviewer);

                am.Maternity = String.Format("{0:n0}", model.Maternity) +"%";
                am.Medical = String.Format("{0:n0}", model.Medical) + "%";
                return am;
            }
                
            else
            {
                return am;
            }
        }



    }
}
