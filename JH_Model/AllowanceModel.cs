﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JH_Model
{
    public class AllowanceModel:BaseModel
    {
        public string MD { get; set; }

        public string Director { get; set; }

        public string Manager { get; set; }

        public string N1 { get; set; }

        public string N2 { get; set; }

        public string N3 { get; set; }

        public string JpnUniGrade { get; set; }

        public string Local1stInterviewer { get; set; }

        public string Local2ndInterviewer { get; set; }

        public string Oversea1stInterviewer { get; set; }

        public string Oversea2ndInterviewer { get; set; }

        public string MarketingTeamAllowance { get; set; }

        public string MentorAllowance { get; set; }

        public byte Currency { get; set; }

        public string Medical { get; set; }

        public string Maternity { get; set; }

    }
}
