namespace JH_Model
{
    public class AttendanceModel : BaseModel
    {
        public string YYYYMM { get; set; }
        public string YYYY { get; set; }
        public string MM { get; set; }
        public string DD { get; set; }
        public string StaffID { get; set; }
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }
        public string LeaveType { get; set; }
        public string EarlyOut { get; set; }
    }
}
