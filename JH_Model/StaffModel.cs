using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JH_Model
{
    public class StaffModel : BaseModel
    {
        public int StaffID { get; set; }
        public DateTime ChangeDate { get; set; }
        public string Name { get; set; }
        public byte Gender { get; set; }
        public DateTime DOB { get; set; }
        public string NRC { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime PermanentDate { get; set; }
        public string BankInformation { get; set; }
        public string PermanentAddress { get; set; }
        public string TemporaryAddress { get; set; }
        public string PhoneNo { get; set; }
        public string EmergencyPhoneNo { get; set; }
        public string EmailAddress { get; set; }
        public decimal UniformCharges { get; set; }
        public int FingerPrintID { get; set; }
        public byte OfficeCD { get; set; }
        public string CompanyCD { get; set; }
        public string DepartmentCD { get; set; }
        public string SubDivisionCD { get; set; }
        public string PositionCD { get; set; }
        public string TransportationCD { get; set; }
        public byte Currency { get; set; }
        public string Photo { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal Effort { get; set; }
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

    }
}
