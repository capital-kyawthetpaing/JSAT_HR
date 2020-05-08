using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using JH_DL;
using JH_Model;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Web;

namespace Position_BL
{
    public class PositionBL
    {
        public DataTable GETPosition()
        {
            BaseDL bdl = new BaseDL();
            SqlParameter[] prms = new SqlParameter[0];
            return bdl.SelectData("M_Position_SelectAll", prms);
        }

        public string Position_Save(PositionModel pm)
        {
            string msg = string.Empty;
            M_Position position = new M_Position();
            position = setPositionValue(position,pm);
            JSAT_HREntities db = new JSAT_HREntities();
            db.M_Position.Add(position);
            db.SaveChanges();
            msg = "OK";
            return msg;
        }

        public M_Position setPositionValue(M_Position position,PositionModel pm)
        {
            string updatedby = string.Empty;
            updatedby = HttpContext.Current.Session["UserID"].ToString();
            updatedby = updatedby.Split('_')[0];

            position.PositionCD = pm.PostitionCD;
            position.Position = pm.PositionName;
            position.InsertedDate = DateTime.Now;
            position.InsertedBy = updatedby;

            return position;
        }

        public string Position_Update(PositionModel pm)
        {
            string updatedby = string.Empty;
            updatedby = HttpContext.Current.Session["UserID"].ToString();
            updatedby = updatedby.Split('_')[0];
            JSAT_HREntities db = new JSAT_HREntities();
            string msg = string.Empty;
            
            M_Position update = db.M_Position.Where(p => p.PositionCD.Equals(pm.PostitionCD)).SingleOrDefault();
            update = setPositionValue(update, pm);
            update.PositionCD = pm.PostitionCD;
            update.Position = pm.PositionName;
            update.UpdatedDate = DateTime.Now;
            update.UpdatedBy = updatedby;

            try
            {
                db.SaveChanges();
                msg = "OK";
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }
            return msg;

        }

        public string Check_Position(PositionModel pm)
        {
            JSAT_HREntities db = new JSAT_HREntities();
            string msg = string.Empty;
            M_Position update = db.M_Position.Where(p => p.PositionCD.Equals(pm.PostitionCD)).SingleOrDefault();
            if (update != null)
                msg = update.PositionCD;
            else
                msg = "";
            return msg;
        }

        public DataTable PositionCheck(string id)
        {
            BaseDL bdl = new BaseDL();
            DataTable dt = new DataTable();
            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@id", SqlDbType.VarChar) { Value = id };
            prms[1] = new SqlParameter("@option", SqlDbType.VarChar) { Value = 4 };
            dt = bdl.SelectData("M_Organization_Select_byID", prms);
            return dt;
        }
    }
}
