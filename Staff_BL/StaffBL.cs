using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using JH_DL;
using JH_Model;


namespace Staff_BL
{
    public class StaffBL
    {
        public DataTable GetAllStaff()
        {
            BaseDL bdl = new BaseDL();
            SqlParameter[] prms = new SqlParameter[0];
            //prms[0] = new SqlParameter("@StaffCD", SqlDbType.VarChar) { Value = sm.StaffCD };
            //prms[1] = new SqlParameter("@StaffName", SqlDbType.VarChar) { Value = sm.StaffName };
            return bdl.SelectData("M_Staff_SelectAll", prms);
        }

        public StaffModel SearchStaff(StaffModel sm)
        {
            BaseDL bdl = new BaseDL();
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@StaffID", SqlDbType.Int) { Value = sm.StaffID };
            //prms[1] = new SqlParameter("@StaffName", SqlDbType.VarChar) { Value = sm.StaffName };
            DataTable dt = bdl.SelectData("M_Staff_Search", prms);
            if (dt.Rows.Count > 0)
            {
                sm.StaffID =  dt.Rows[0]["StaffID"].ToString();
                sm.Name = dt.Rows[0]["Name"].ToString();
                sm.Gender = dt.Rows[0]["Gender"].ToString();
                sm.CompanyCD = dt.Rows[0]["CompanyCD"].ToString();
                sm.DepartmentCD = dt.Rows[0]["DepartmentCD"].ToString();
                sm.SubDivisionCD = dt.Rows[0]["SubDivisionCD"].ToString();
                sm.PositionCD = dt.Rows[0]["PositionCD"].ToString();
                sm.OfficeCD = dt.Rows[0]["OfficeCD"].ToString();
                sm.UniformCharges = dt.Rows[0]["UniformCharges"].ToString();
                sm.TransportationCD = dt.Rows[0]["TransportationCD"].ToString();
                sm.MD = Convert.ToBoolean(dt.Rows[0]["MD"]);
            }

            return sm;
        }

        public string Staff_Save(StaffModel model)
        {
            JSAT_HREntities db = new JSAT_HREntities();
            M_Staff ms = new M_Staff();
            Staff_Allowance sa = new Staff_Allowance();
            BaseDL dl = new BaseDL();
            string msg = "";

            ms.StaffID = Convert.ToInt32(model.StaffID);
            ms.ChangeDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            ms.Name = model.Name;
            ms.Gender = Convert.ToByte(model.Gender);
            ms.DOB = model.DOB;
            ms.NRC = model.NRC;
            ms.JoinDate = model.JoinDate;
            ms.PermanentDate = model.PermanentDate;
            ms.BankInformation = model.BankInformation;
            ms.PermanentAddress = model.PermanentAddress;
            ms.TemporaryAddress = model.TemporaryAddress;
            ms.PhoneNo = model.PhoneNo;
            ms.EmergencyPhoneNo = model.EmergencyPhoneNo;
            ms.EmailAddress = model.EmailAddress;
            if (model.UniformCharges.Contains(","))
                model.UniformCharges = model.UniformCharges.Replace(",","");
            ms.UniformCharges = Convert.ToDecimal(model.UniformCharges);
            ms.FingerPrintID = model.FingerPrintID;
            ms.OfficeCD = Convert.ToByte(model.OfficeCD);
            ms.CompanyCD = model.CompanyCD;
            ms.DepartmentCD = model.DepartmentCD;
            ms.SubDivisionCD = model.SubDivisionCD;
            ms.PositionCD = model.PositionCD;
            ms.TransportationCD = model.TransportationCD;
            ms.Currency = model.Currency;
            ms.Photo = model.Photo;
            if (model.BasicSalary == 1)
                ms.BasicSalary = 250000;
            else
                ms.BasicSalary = 150000;
            if (model.Effort.Contains(","))
                model.Effort = model.Effort.Replace(",","");
            ms.Effort = Convert.ToDecimal(model.Effort);
            ms.DeleteFlg = model.DeleteFlg;
            ms.InsertedDate = DateTime.Now;
            ms.InsertedBy = HttpContext.Current.Session["UserID"].ToString();

            sa.StaffID = Convert.ToInt32(model.StaffID);
            sa.ChangeDate= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            sa.MD = model.MD;
            sa.Director = model.Director;
            sa.Manager = model.Manager;
            sa.N1 = model.N1;
            sa.N2 = model.N2;
            sa.N3 = model.N3;
            sa.JpnUniGradurate = model.JpnUniGradurate;
            sa.Local1stInterviewer = model.Local1stInterviewer;
            sa.Local2ndInterviewer = model.Local2ndInterviewer;
            sa.Overseas1stInterviewer = model.Overseas1stInterviewer;
            sa.Overseas2ndInterviewer = model.Overseas2ndInterviewer;
            sa.MarketingTeamAllowance = model.MarketingTeamAllowance;
            sa.MentorAllowance = model.MentorAllowance;

           // db.M_Staff.Add(ms);
           // db.Staff_Allowance.Add(sa);
            try
            {
                //db.SaveChanges();
                msg = "Insert Success";
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }

            return msg;
        }

        public async Task<string> Check_StaffCD(StaffModel model)
        {
            JSAT_HREntities db = new JSAT_HREntities();
            M_Staff md = new M_Staff();
            string scd;
            M_Staff staffcd = await db.M_Staff.Where(ms => ms.StaffID.Equals(model.StaffID)).SingleOrDefaultAsync();
            if (staffcd != null)
                scd = staffcd.StaffID.ToString();
            else
                scd ="";

            return scd;
        }
    }
}
