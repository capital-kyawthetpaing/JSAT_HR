//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JH_DL
{
    using System;
    using System.Collections.Generic;
    
    public partial class M_Attendance
    {
        public int YYYYMM { get; set; }
        public byte DD { get; set; }
        public int FingerPrintID { get; set; }
        public byte OfficeCD { get; set; }
        public string AttandenceDate { get; set; }
        public Nullable<System.TimeSpan> TimeIn { get; set; }
        public Nullable<System.TimeSpan> TimeOut { get; set; }
        public Nullable<byte> MorningLeaveType { get; set; }
        public Nullable<byte> EveningLeaveType { get; set; }
        public Nullable<System.TimeSpan> LateTime { get; set; }
        public Nullable<int> LateMinutes { get; set; }
        public Nullable<byte> EarlyOut { get; set; }
        public Nullable<bool> PayTransportation { get; set; }
        public Nullable<byte> ImportFlag { get; set; }
    }
}
