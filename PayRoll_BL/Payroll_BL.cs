﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JH_Model;
using JH_DL;
using System.Data.Entity;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace PayRoll_BL
{
    public class Payroll_BL
    {
        public DataTable PayRoll_Search(string yearmonth,int option)
        {
            BaseDL bdl = new BaseDL();
            DataTable dtpay = new DataTable();
            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@yearmonth", SqlDbType.VarChar) { Value = yearmonth };
            prms[1] = new SqlParameter("@option", SqlDbType.VarChar) { Value =  option};
            dtpay = bdl.SelectData("PayRoll_Search", prms);
            if (dtpay.Rows.Count > 0)
                return dtpay;
            else
                return null;
        }

        public Boolean PayRoll_Allowance_Insert(string yearmonth)
        {
            try
            {
                BaseDL bdl = new BaseDL();
                DataTable dtinfo = new DataTable();
                SqlParameter[] prms = new SqlParameter[1];
                prms[0] = new SqlParameter("@YYYYMM", SqlDbType.VarChar) { Value = yearmonth };
                bdl.InsertUpdateDeleteData("PayRoll_Allowance_InsertUpdate", prms);
                return true;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return false;
            }
        }

        public DataTable PayRoll_Calculate(string yearmonth,string date)
        {
            BaseDL bdl = new BaseDL();
            DataTable dtpay = new DataTable();
            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@YYYYMM", SqlDbType.VarChar) { Value = yearmonth };
            prms[1] = new SqlParameter("@yyyymmdd", SqlDbType.VarChar) { Value = date};
            dtpay = bdl.SelectData("Payroll_Calculation", prms);
            if (dtpay.Rows.Count > 0)
                return dtpay;
            else
                return null;
        }
        public DataTable PayRoll_Detail_Allow(string S_ID)
        {
            var ID = S_ID.Split('_')[0];
            var YM = S_ID.Split('_')[1];
            BaseDL dl = new BaseDL();
            SqlParameter[] prms = new SqlParameter[3];
            prms[0] = new SqlParameter("@staffID", SqlDbType.VarChar) { Value = ID };
            prms[1] = new SqlParameter("@YYYYMM", SqlDbType.Int) { Value = int.Parse(YM) };
            prms[2] = new SqlParameter("@option", SqlDbType.Int) { Value = 1 };
            DataTable dt = dl.SelectData("PayRoll_Detail", prms);
            return dt;
        }
        public DataTable PayRoll_Detail_Deduction(string id)
        {
            var ID = id.Split('_')[0];
            var YM = id.Split('_')[1];
            BaseDL dl = new BaseDL();
            SqlParameter[] prms = new SqlParameter[3];
            prms[0] = new SqlParameter("@staffID", SqlDbType.VarChar) { Value = ID };
            prms[1] = new SqlParameter("@YYYYMM", SqlDbType.Int) { Value = int.Parse(YM) };
            prms[2] = new SqlParameter("@option", SqlDbType.Int) { Value = 2 };
            DataTable dt = dl.SelectData("PayRoll_Detail", prms);
            return dt;

        }

        //public List<staffName> SelectStaff()
        //{
        //    int notresign = 0;
        //    JSAT_HREntities context = new JSAT_HREntities();
        //    var staffName = context.M_Staff.Select(s => new staffName {StaffID=s.StaffID,Name=s.Name  }).ToList();
        //    return staffName;
        //}
        public DataTable PayRoll_Detail_Report(string StaffID, string yyyymm)
        {
            BaseDL bdl = new BaseDL();
            DataTable dtpaydetail = new DataTable();
            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@StaffID", SqlDbType.VarChar) { Value = StaffID };
            prms[1] = new SqlParameter("@YYYYMM", SqlDbType.Int) { Value = yyyymm };
            dtpaydetail = bdl.SelectData("PayRoll_Detail_Report", prms);
            if (dtpaydetail.Rows.Count > 0)
                return dtpaydetail;
            else return dtpaydetail;
        }

        public DataTable PayRoll_Select(string id)
        {
            var ID = id.Split('_')[0];
            var YM = id.Split('_')[1];
            BaseDL dl = new BaseDL();
            SqlParameter[] prms = new SqlParameter[3];
            prms[0] = new SqlParameter("@staffID", SqlDbType.VarChar) { Value = ID };
            prms[1] = new SqlParameter("@YYYYMM", SqlDbType.Int) { Value = int.Parse(YM) };
            prms[2] = new SqlParameter("@option", SqlDbType.Int) { Value = 1 };
            DataTable dt = dl.SelectData("PayRoll_Detail", prms);
            return dt;
        }

        public DataTable PayRoll_Setting_Report(string yyyymm)
        {
            BaseDL bdl = new BaseDL();
            DataTable dtpaydetail = new DataTable();
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@YYYYMM", SqlDbType.Int) { Value = yyyymm };
            dtpaydetail = bdl.SelectData("PayRoll_Setting_Report", prms);
            if (dtpaydetail.Rows.Count > 0)
                return dtpaydetail;
            else return dtpaydetail;
        }
        public DataTable Get_Transportation_Report(string yyyymm)
        {
            BaseDL bdl = new BaseDL();
            DataTable dtpaydetail = new DataTable();
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@YYYYMM", SqlDbType.Int) { Value = yyyymm };
            dtpaydetail = bdl.SelectData("Transportation_Report_Select", prms);
            if (dtpaydetail.Rows.Count > 0)
                return dtpaydetail;
            else return dtpaydetail;
        }

        public DataTable CheckImport(string yyyymm)
        {
            BaseDL bdl = new BaseDL();
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@YYYYMM", SqlDbType.VarChar) { Value = yyyymm };
           
            DataTable dtImport = new DataTable();
            dtImport = bdl.SelectData("L_Import_Select_ForPayroll", prms);
            return dtImport;
        }

        public DataTable Get_PayRoll_Detail_Search(string id)
        {
            BaseDL bdl = new BaseDL();
            DataTable dtImportList = new DataTable();
            string[] arr;
            string YYYMM = string.Empty;
            string StaffID = string.Empty;
            if (id != null)
            {
                arr = id.Split('_');
                YYYMM = arr[0] + arr[1];
                StaffID = arr[2];

            }

            SqlParameter[] prm = new SqlParameter[2];
            prm[0] = new SqlParameter("@YYYYMM", SqlDbType.Int) { Value = YYYMM };

            prm[1] = new SqlParameter("@staffID", SqlDbType.VarChar) { Value = StaffID };

            dtImportList = bdl.SelectData("PayRoll_Detail_Search", prm);
            return dtImportList;
        }
    }
}
