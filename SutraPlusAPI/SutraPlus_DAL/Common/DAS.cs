using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SutraPlus_DAL.Common
{
    public class DAS
    {

        SqlConnection con;

        public DAS()
        {
            try
            {
                //Online
                //string cs = "Data Source=103.50.212.163;Initial Catalog="+ HttpContext.Current.Session["customercode"] + ";Persist Security Info=True;User ID=sa;password=root@123";

                //Developer
                string cs = "Server=DESKTOP-QH67LSN\\SQLEXPRESS;Database=K2223RGP;Trusted_Connection=True;TrustServerCertificate=True;";


                con = new SqlConnection(cs);


                con.Open();
            }
            catch (SqlException ex)
            {
            }
            con.Close();

        }

        public string ExecSQLQuery(string Query)
        {

            if (this.con.State == ConnectionState.Closed)
                this.con.Open();

            SqlCommand SQLCmd = new SqlCommand(Query, con);
            SQLCmd.CommandTimeout = 0;
            string returnValue = "";
            try
            {
                returnValue = (SQLCmd.ExecuteScalar()).ToString();
            }
            catch (InvalidOperationException ex)
            {
                returnValue = ex.Message;
            }
            catch (SqlException ex1)
            {
                returnValue = ex1.Number.ToString();
            }

            catch (NullReferenceException ex2)
            {
                returnValue = "";
            }
            finally
            {
                SQLCmd.Connection.Close();
                SQLCmd = null;
                if (this.con.State == ConnectionState.Open)
                    this.con.Close();
            }
            return returnValue;
        }


        public DataTable GetDataTable(string Query)
        {
            if (this.con.State == ConnectionState.Closed)
                this.con.Open();
            //Query = Query + sCompanyID_FinYear;
            DataTable dt = new DataTable();
            SqlDataAdapter SQLDA = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 300;
            cmd.Connection = con;
            cmd.CommandText = Query;
            cmd.CommandType = CommandType.Text;
            SQLDA.SelectCommand = cmd;

            SQLDA.SelectCommand.ExecuteNonQuery();




            try
            {
                SQLDA.Fill(dt);

            }
            catch (SqlException ex)
            {
                //System.Windows.Forms.MessageBox.Show("Failed to Execute Command! \n\tError Detail:\n \t" + ex.Message, "Data Access Error!", MessageBoxButtons.OK);
            }
            finally
            {
                SQLDA.SelectCommand.Connection.Close();
                SQLDA = null;
                if (this.con.State == ConnectionState.Open)
                    this.con.Close();
            }
            return dt;
        }
    }

}
