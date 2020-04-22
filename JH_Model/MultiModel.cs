using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JH_Model
{
   public  class MultiModel:BaseModel
    {
        public List<AttendanceModel> attlistModel { get; set; }

        public AttendanceModel attModel { get; set; }
        public StaffModel smodel { get; set; }

        public List<UserModel> UserlistModel { get; set; }
        public UserModel userModel { get; set; }
    }
}
