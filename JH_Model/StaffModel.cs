using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace JH_Model
{
    public class StaffModel : BaseModel
    {
        public string StaffID { get; set; }
        public DateTime ChangeDate { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string NRC { get; set; }
        public string JoinDate { get; set; }
        public string PermanentDate { get; set; }
        public string BankInformation { get; set; }
        public string PermanentAddress { get; set; }
        public string TemporaryAddress { get; set; }
        public string PhoneNo { get; set; }
        public string EmergencyPhoneNo { get; set; }
        public string EmailAddress { get; set; }
        public string UniformCharges { get; set; } = "0";
        public int FingerPrintID { get; set; } = 0;
        public string OfficeCD { get; set; } 
        public string CompanyCD { get; set; } = "0";
        public string DepartmentCD { get; set; } = "0";
        public string SubDivisionCD { get; set; } = "0";
        public string PositionCD { get; set; } = "0";
        public string TransportationFee { get; set; } = "0";
        public byte Currency { get; set; }
        public string Photo { get; set; }
        public string BasicSalary { get; set; } = "0";
        public string Effort { get; set; } = "0";
        public bool IsResign { get; set; }
        public bool DeleteFlg { get; set; }
        public bool MD { get; set; }
        public bool Director { get; set; }
        public bool Manager { get; set; }
        public bool N1 { get; set; }
        public bool N2 { get; set; }
        public bool N3 { get; set; }
        public bool JpnUniGradurate { get; set; }
        public bool Local1stInterviewer { get; set; }
        public bool Local2ndInterviewer { get; set; }
        public bool Overseas1stInterviewer { get; set; }
        public bool Overseas2ndInterviewer { get; set; }
        public bool MarketingTeamAllowance { get; set; }
        public bool MentorAllowance { get; set; }

        public List<int> SelectedMultiStaffId { get; set; }
        public List<StaffObj> SelectedStaffName { get; set; }

    }
    public class staffName
    {
        public int StaffID { get; set; }
        public string Name { get; set; }
    }


    //public class MultiStaff
    //{
    //    public List<int> SelectedMultiStaffId { get; set; }
    //    public List<StaffObj> SelectedStaffName { get; set; }
    //}

    public class StaffObj
    {

        public int StaffID { get; set; }

        public string StaffName { get; set; }


    }
}
