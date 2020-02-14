using System.Data;
using System.Data.SqlClient;
using System.Linq;
using JH_DL;
using JH_Model;


namespace Attendance_BL
{
    public class AttendanceBL
    {
        public void Insert_Attendance_Data(DataTable dttest, JH_Model.M_AttandenceModel attmodel)
        {
            DataTable dt = new DataTable();
            BaseDL bdl = new BaseDL();
            SqlParameter[] prms = new SqlParameter[4];
            if (!string.IsNullOrWhiteSpace(attmodel.FileName))
            {
                prms[0] = new SqlParameter("@FileName", SqlDbType.VarChar) { Value = attmodel.FileName };
            }
            else
            {
                prms[0] = new SqlParameter("@FileName", SqlDbType.VarChar) { Value = System.DBNull.Value };
            }


            prms[1] = new SqlParameter("@ImportType", SqlDbType.VarChar) { Value = 1 };
            prms[2] = new SqlParameter("@ImportedBy", SqlDbType.VarChar) { Value = 1 };


            dttest.TableName = "M_Attandance";
            System.IO.StringWriter writer = new System.IO.StringWriter();
            dttest.WriteXml(writer, XmlWriteMode.WriteSchema, false);
            string result = writer.ToString();
            prms[3] = new SqlParameter("@xml", SqlDbType.Xml) { Value = result };
            bdl.InsertUpdateDeleteData("M_Attandance_Insert", prms);
        }
        public DataSet M_Attendance_Select(AttendanceModel am)
        {
            BaseDL bdl = new BaseDL();
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@YYYYMM", SqlDbType.Int) { Value = am.YYYYMM };
            //prms[1] = new SqlParameter("@StaffName", SqlDbType.VarChar) { Value = sm.StaffName };
            return bdl.SelectDataSet("M_Attendance_Select", prms);
        }
        public DataTable Get_Import_List_View()
        {
            BaseDL bdl = new BaseDL();
            DataTable dtImportList = new DataTable();
            dtImportList = bdl.SelectData("L_Import_Select", null);
            return dtImportList;
        }
    }
}
