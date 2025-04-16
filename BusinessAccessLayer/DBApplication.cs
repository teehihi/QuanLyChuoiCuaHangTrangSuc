using DataAcessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer
{
    public class DBApplication
    {
        private DAL db = new DAL();

        public static int GetAppIDByName(string appName)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionHelper.CurrentConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT AppID FROM Application WHERE Name = @Name", conn);
                cmd.Parameters.AddWithValue("@Name", appName);

                object result = cmd.ExecuteScalar();
                if (result != null)
                    return Convert.ToInt32(result);
                else
                    throw new Exception("Không tìm thấy ứng dụng với tên: " + appName);
            }
        }
        public string GetAppNameByID(int appId)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionHelper.CurrentConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Name FROM Application WHERE AppID = @AppID", conn);
                cmd.Parameters.AddWithValue("@AppID", appId);

                object result = cmd.ExecuteScalar();
                return result != null ? result.ToString() : null;
            }
        }
    }
}
