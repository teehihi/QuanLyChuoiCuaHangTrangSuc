using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer
{
    public class DAL
    {
        //   private readonly string connectionString = "Data Source=TEE\\TEE;Initial Catalog=JwelrySystemDBMSFinal;Integrated Security=True;";

        private readonly string connectionString = ConnectionHelper.CurrentConnectionString;

        public DataSet ExecuteQueryDataSet(string query, CommandType commandType, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = commandType;
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }

        public int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure; // Thêm dòng này

                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    return cmd.ExecuteNonQuery();
                }
            }
        }


        public object ExecuteScalar(string query, SqlParameter[] parameters = null, bool isStoredProcedure = false)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (isStoredProcedure)
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                    }

                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    return cmd.ExecuteScalar();
                }
            }
        }

    }
}

