using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JH_Model
{
    public class StaffModel : BaseModel
    {
        public string StaffID { get; set; }
        public string ChangeDate { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string NRC { get; set; }
        public string JoinDate { get; set; }
        public string PermanentDate { get; set; }
        public string BankInformation { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string EmergencyPhoneNo { get; set; }
        public string EmailAddress { get; set; }
        public string UniformCharges { get; set; }
        public string FingerPrintID { get; set; }
        public string StaffType { get; set; }
        public string DepartmentCD { get; set; }
        public string SubDivisionCD { get; set; }
        public string PositionCD { get; set; }
        public string TransportationCD { get; set; }
        public string Currency { get; set; }
        public string Photo { get; set; }
        public string BasicSalary { get; set; }
        public string Effort { get; set; }
        public string DeleteFlg { get; set; }
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

    }
}
