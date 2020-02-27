using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JH_Model;
using JH_DL;

namespace User_BL
{
    public class UserBL
    {
        public UserModel CheckLogin(UserModel umodel)
        {
            JSAT_HREntities db = new JSAT_HREntities();
            M_User um = db.M_User.Where(m => m.UserID == umodel.UserID && m.Password == umodel.Password).SingleOrDefault();

            if (um == null)
                return null;
            else
            {
                umodel.UserID = um.UserID;
                umodel.UserName = um.UserName;
                return umodel;
            }
        }
    }
}
