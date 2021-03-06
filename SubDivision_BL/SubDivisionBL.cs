﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JH_DL;
using System.Data;
using System.Data.SqlClient;
using JH_Model;
using System.Data.Entity;
using System.Web;

namespace SubDivision_BL
{
    public class SubDivisionBL
    {
        public DataTable GETSubDivision()     
        {
            BaseDL bdl = new BaseDL();
            SqlParameter[] prms = new SqlParameter[0];
            return bdl.SelectData("M_SubDivision_SelectAll", prms);
        }

        public string Check_SubDivisionCD(SubDivisionModel model)
        {
            JSAT_HREntities db = new JSAT_HREntities();
            M_SubDivision md = new M_SubDivision();
            string msg = string.Empty;
            M_SubDivision update = db.M_SubDivision.Where(s => s.SubDivisionCD.Equals(model.SubDivisionCD)).SingleOrDefault();
            if (update != null)
                msg = update.SubDivisionCD;
            else
                msg = "";

            return msg;
        }

        public string SubDivision_Save(SubDivisionModel model)
        {
            string updatedby = string.Empty;
            updatedby = HttpContext.Current.Session["UserID"].ToString();
            updatedby = updatedby.Split('_')[0];

            JSAT_HREntities db = new JSAT_HREntities();
            M_SubDivision msd = new M_SubDivision();

            msd.SubDivisionCD = model.SubDivisionCD;
            msd.SubDivision = model.SubDivisionName;
            msd.InsertedDate = DateTime.Now;
            msd.InsertedBy = updatedby;

            db.M_SubDivision.Add(msd);
            db.SaveChanges();

            string msg = "OK";
            return msg;
        }

        public string SubDivision_Update(SubDivisionModel model)
        {
            string updatedby = string.Empty;
            updatedby = HttpContext.Current.Session["UserID"].ToString();
            updatedby = updatedby.Split('_')[0];
            JSAT_HREntities db = new JSAT_HREntities();
            M_SubDivision md = new M_SubDivision();
            string msg = string.Empty;
            M_SubDivision update = db.M_SubDivision.Where(s => s.SubDivisionCD.Equals(model.SubDivisionCD)).SingleOrDefault();
            update.SubDivisionCD = model.SubDivisionCD;
            update.SubDivision = model.SubDivisionName;
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

        public DataTable SubDivisionCheck(string id)
        {
            BaseDL bdl = new BaseDL();
            DataTable dt = new DataTable();
            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@id", SqlDbType.VarChar) { Value = id };
            prms[1] = new SqlParameter("@option", SqlDbType.VarChar) { Value = 3 };
            dt = bdl.SelectData("M_Organization_Select_byID", prms);
            return dt;
        }

    }
}
