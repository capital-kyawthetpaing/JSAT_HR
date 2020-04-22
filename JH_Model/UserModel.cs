using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JH_Model
{
    public class UserModel : BaseModel
    {
        public string UserID { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string ViewID { get; set; }
        public string ViewName { get; set; }
        public bool ShowView { get; set; }
        public bool IsReadOnly { get; set; }
        public string SaveUpdateFlag { get; set; }
    }
}
