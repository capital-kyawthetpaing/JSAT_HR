using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JH_Model;
using JH_DL;
using System.Data;
using System.Data.SqlClient;
using System.Web;

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

        public DataTable GetAllUser()
        {
            BaseDL bdl = new BaseDL();
            return bdl.SelectData("M_User_SelectAll", null);
        }

        public DataTable M_View_Select()
        {
            BaseDL bdl = new BaseDL();
            return bdl.SelectData("M_View_SelectAll", null);
        }

        public DataTable M_View_Select_ForEdit(string id)
        {
            BaseDL bdl = new BaseDL();
            SqlParameter[] prms = new SqlParameter[1];

            prms[0] = new SqlParameter("@UserID", SqlDbType.VarChar) { Value = id };

            return bdl.SelectData("User_SelectAll_ForEdit", prms);
        }

        public void Update_User_Authorization(DataTable dtUserlist, MultiModel mModel)
        {
            BaseDL dl = new BaseDL();
            try
            {
                SqlParameter[] prms = new SqlParameter[6];

                prms[0] = new SqlParameter("@UserID", SqlDbType.VarChar) { Value = mModel.userModel.UserID };

                prms[1] = new SqlParameter("@UserName", SqlDbType.VarChar) { Value = mModel.userModel.UserName };

                prms[2] = new SqlParameter("@Password", SqlDbType.VarChar) { Value = mModel.userModel.Password };

                prms[3] = new SqlParameter("@ImportedBy", SqlDbType.VarChar) { Value = HttpContext.Current.Session["UserID"].ToString().Split('_')[0] };

                prms[4] = new SqlParameter("@saveUpdateFlag", SqlDbType.VarChar) { Value = mModel.userModel.SaveUpdateFlag };

                dtUserlist.TableName = "user";
                System.IO.StringWriter writer = new System.IO.StringWriter();
                dtUserlist.WriteXml(writer, XmlWriteMode.WriteSchema, false);
                string result = writer.ToString();
                prms[5] = new SqlParameter("@xml", SqlDbType.Xml) { Value = result };
                dl.InsertUpdateDeleteData("User_List_Save_Update", prms);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UserExists(MultiModel model)
        {
            JSAT_HREntities db = new JSAT_HREntities();
            M_User md = new M_User();
            string UserID = model.userModel.UserID;
            M_User userid = db.M_User.Where(mu => mu.UserID.Equals(UserID)).SingleOrDefault();
            if (userid != null)
                return true;
            return false;
        }

        public DataTable UserRead(string id)
        {
            BaseDL bdl = new BaseDL();
            DataTable dt = new DataTable();
            string viewname = id;
            string userid = HttpContext.Current.Session["UserID"].ToString().Split('_')[0];
            SqlParameter[] prms = new SqlParameter[3];
                prms[0] = new SqlParameter("@UserID", SqlDbType.VarChar) { Value = userid };
                prms[1] = new SqlParameter("@ViewName", SqlDbType.VarChar) { Value = viewname };
                prms[2] = new SqlParameter("@Option", SqlDbType.Int) { Value = 2 };
                dt = bdl.SelectData("User_Authorization_SelectAll", prms);

            return dt;
        }
    }
}
