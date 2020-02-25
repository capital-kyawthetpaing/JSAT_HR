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
                sm.NRC = dt.Rows[0]["NRC"].ToString();
                sm.DOB = dt.Rows[0]["DOB"].ToString();
                sm.JoinDate = dt.Rows[0]["JoinDate"].ToString();
                sm.PermanentDate = dt.Rows[0]["PermanentDate"].ToString();
                sm.Gender = dt.Rows[0]["Gender"].ToString();
                sm.BankInformation = dt.Rows[0]["BankInformation"].ToString();
                sm.Currency = Convert.ToByte(dt.Rows[0]["Currency"]);
                sm.BasicSalary = Convert.ToDecimal(dt.Rows[0]["BasicSalary"]);
                sm.Effort = dt.Rows[0]["Effort"].ToString();
                sm.UniformCharges = dt.Rows[0]["UniformCharges"].ToString();
                sm.CompanyCD = dt.Rows[0]["CompanyCD"].ToString();
                sm.DepartmentCD = dt.Rows[0]["DepartmentCD"].ToString();
                sm.SubDivisionCD = dt.Rows[0]["SubDivisionCD"].ToString();
                sm.PositionCD = dt.Rows[0]["PositionCD"].ToString();

                sm.OfficeCD = dt.Rows[0]["OfficeCD"].ToString();
                sm.FingerPrintID = Convert.ToInt32(dt.Rows[0]["FingerPrintID"]);

                sm.PhoneNo = dt.Rows[0]["PhoneNo"].ToString();
                sm.EmergencyPhoneNo = dt.Rows[0]["EmergencyPhoneNo"].ToString();
                sm.TransportationCD = dt.Rows[0]["TransportationCD"].ToString();
                sm.EmailAddress = dt.Rows[0]["EmailAddress"].ToString();
                sm.PermanentAddress = dt.Rows[0]["PermanentAddress"].ToString();
                sm.TemporaryAddress = dt.Rows[0]["TemporaryAddress"].ToString();

                sm.TransportationCD = dt.Rows[0]["TransportationCD"].ToString();
                sm.MD = Convert.ToBoolean(dt.Rows[0]["MD"]);
                sm.Director = Convert.ToBoolean(dt.Rows[0]["Director"]);
                sm.Manager = Convert.ToBoolean(dt.Rows[0]["Manager"]);
                sm.N1 = Convert.ToBoolean(dt.Rows[0]["N1"]);
                sm.N2 = Convert.ToBoolean(dt.Rows[0]["N2"]);
                sm.N3 = Convert.ToBoolean(dt.Rows[0]["N3"]);
                sm.Local1stInterviewer = Convert.ToBoolean(dt.Rows[0]["Local1stInterviewer"]);
                sm.Local2ndInterviewer = Convert.ToBoolean(dt.Rows[0]["Local2ndInterviewer"]);
                sm.Overseas1stInterviewer = Convert.ToBoolean(dt.Rows[0]["Overseas1stInterviewer"]);
                sm.Overseas2ndInterviewer = Convert.ToBoolean(dt.Rows[0]["Overseas2ndInterviewer"]);
                sm.MarketingTeamAllowance = Convert.ToBoolean(dt.Rows[0]["MarketingTeamAllowance"]);
                sm.MentorAllowance = Convert.ToBoolean(dt.Rows[0]["MentorAllowance"]);
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
            ms.DOB = Convert.ToDateTime(model.DOB);
            ms.NRC = model.NRC;
            ms.JoinDate = Convert.ToDateTime(model.JoinDate);
            ms.PermanentDate = Convert.ToDateTime(model.PermanentDate);
            ms.BankInformation = model.BankInformation;
            ms.PermanentAddress = model.PermanentAddress;
            ms.TemporaryAddress = model.TemporaryAddress;
            ms.PhoneNo = model.PhoneNo;
            ms.EmergencyPhoneNo = model.EmergencyPhoneNo;
            ms.EmailAddress = model.EmailAddress;
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

        public string Staff_Update(StaffModel model)
        {
            JSAT_HREntities db = new JSAT_HREntities();
            string msg = string.Empty;

            M_Staff updatestaff = db.M_Staff.Where(s => s.StaffID.Equals(model.StaffID)).SingleOrDefault();
            Staff_Allowance updateallow = db.Staff_Allowance.Where(a => a.StaffID.Equals(model.StaffID)).SingleOrDefault();
            updatestaff.ChangeDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            updatestaff.Name = model.Name;
            updatestaff.Gender = Convert.ToByte(model.Gender);
            updatestaff.DOB = Convert.ToDateTime(model.DOB);
            updatestaff.NRC = model.NRC;
            updatestaff.JoinDate = Convert.ToDateTime(model.JoinDate);
            updatestaff.PermanentDate = Convert.ToDateTime(model.PermanentDate);
            updatestaff.BankInformation = model.BankInformation;
            updatestaff.PermanentAddress = model.PermanentAddress;
            updatestaff.TemporaryAddress = model.TemporaryAddress;
            updatestaff.PhoneNo = model.PhoneNo;
            updatestaff.EmergencyPhoneNo = model.EmergencyPhoneNo;
            updatestaff.EmailAddress = model.EmailAddress;
            model.UniformCharges = model.UniformCharges.Replace(",", "");
            updatestaff.UniformCharges = Convert.ToDecimal(model.UniformCharges);
            updatestaff.FingerPrintID = model.FingerPrintID;
            updatestaff.OfficeCD = Convert.ToByte(model.OfficeCD);
            updatestaff.CompanyCD = model.CompanyCD;
            updatestaff.DepartmentCD = model.DepartmentCD;
            updatestaff.SubDivisionCD = model.SubDivisionCD;
            updatestaff.PositionCD = model.PositionCD;
            updatestaff.TransportationCD = model.TransportationCD;
            updatestaff.Currency = model.Currency;
            updatestaff.Photo = model.Photo;
            if (model.BasicSalary == 1)
                updatestaff.BasicSalary = 250000;
            else
                updatestaff.BasicSalary = 150000;
            if (model.Effort.Contains(","))
                model.Effort = model.Effort.Replace(",", "");
            updatestaff.Effort = Convert.ToDecimal(model.Effort);
            updatestaff.DeleteFlg = model.DeleteFlg;
            updatestaff.InsertedDate = DateTime.Now;
            updatestaff.InsertedBy = HttpContext.Current.Session["UserID"].ToString();

            updateallow.StaffID = Convert.ToInt32(model.StaffID);
            updateallow.ChangeDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            updateallow.MD = model.MD;
            updateallow.Director = model.Director;
            updateallow.Manager = model.Manager;
            updateallow.N1 = model.N1;
            updateallow.N2 = model.N2;
            updateallow.N3 = model.N3;
            updateallow.JpnUniGradurate = model.JpnUniGradurate;
            updateallow.Local1stInterviewer = model.Local1stInterviewer;
            updateallow.Local2ndInterviewer = model.Local2ndInterviewer;
            updateallow.Overseas1stInterviewer = model.Overseas1stInterviewer;
            updateallow.Overseas2ndInterviewer = model.Overseas2ndInterviewer;
            updateallow.MarketingTeamAllowance = model.MarketingTeamAllowance;
            updateallow.MentorAllowance = model.MentorAllowance;

            updatestaff.UpdatedDate = DateTime.Now;
            updatestaff.UpdatedBy = HttpContext.Current.Session["UserID"].ToString();
            try
            {
                db.SaveChanges();
                msg = "OK";
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }
            return msg;
        }

        public string Check_StaffCD(StaffModel model)
        {
            JSAT_HREntities db = new JSAT_HREntities();
            M_Staff md = new M_Staff();
            int staffID = Convert.ToInt32(model.StaffID);
            string scd;
            M_Staff staffcd =  db.M_Staff.Where(ms => ms.StaffID.Equals(staffID)).SingleOrDefault();
            if (staffcd != null)
                scd = staffcd.StaffID.ToString();
            else
                scd ="";

            return scd;
        }
    }
}
