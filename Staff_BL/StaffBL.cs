using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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

        public string Staff_Save(StaffModel model)
        {
            JSAT_HREntities db = new JSAT_HREntities();
            M_Staff ms = new M_Staff();
            Staff_Allowance sa = new Staff_Allowance();
            BaseDL dl = new BaseDL();
            string msg = "";

            ms.StaffID = model.StaffID;
            ms.ChangeDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            ms.Name = model.Name;
            ms.Gender = model.Gender;
            ms.NRC = model.NRC;
            ms.JoinDate = model.JoinDate;
            ms.PermanentDate = model.PermanentDate;
            ms.BankInformation = model.BankInformation;
            ms.Address = model.Address;
            ms.PhoneNo = model.PhoneNo;
            ms.EmergencyPhoneNo = model.EmergencyPhoneNo;
            ms.EmailAddress = model.EmailAddress;
            if (model.UniformCharges == true)
                ms.UniformCharges = 5000;
            else
                ms.UniformCharges = 0;
            ms.FingerPrintID = model.FingerPrintID;
            ms.StaffType = model.StaffType;
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
            ms.Effort = model.Effort;
            ms.DeleteFlg = model.DeleteFlg;
            ms.InsertedDate = DateTime.Now;
            ms.InsertedBy = "1";

            sa.StaffID = model.StaffID;
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

            db.M_Staff.Add(ms);
            db.Staff_Allowance.Add(sa);
            try
            {
                db.SaveChanges();
                msg = "Insert Success";
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }

            return msg;
        }
    }
}
