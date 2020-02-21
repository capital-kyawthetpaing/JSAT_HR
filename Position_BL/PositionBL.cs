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
            position.PositionCD = pm.PostitionCD;
            position.Position = pm.PositionName;
            position.InsertedDate = DateTime.Now;
            position.InsertedBy = HttpContext.Current.Session["UserID"].ToString();

            return position;
        }

        public async Task<string> Position_Update(PositionModel pm)
        {
            JSAT_HREntities db = new JSAT_HREntities();
            string msg = string.Empty;
            
            M_Position update = await db.M_Position.Where(p => p.PositionCD.Equals(pm.PostitionCD)).SingleOrDefaultAsync();
            update = setPositionValue(update, pm);
            update.PositionCD = pm.PostitionCD;
            update.Position = pm.PositionName;
            update.UpdatedDate = DateTime.Now;
            update.UpdatedBy = HttpContext.Current.Session["UserID"].ToString();

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

        public async Task<string> Check_Position(PositionModel pm)
        {
            JSAT_HREntities db = new JSAT_HREntities();
            string msg = string.Empty;
            M_Position update = await db.M_Position.Where(p => p.PositionCD.Equals(pm.PostitionCD)).SingleOrDefaultAsync();
            if (update != null)
                msg = update.PositionCD;
            else
                msg = "";
            return msg;
        }
    }
}
