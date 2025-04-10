using DataAcessLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer
{
    public class DBLogin
    {
        private readonly string server = "TEE\\TEE";
        private readonly string saUser = "sa";
        private readonly string saPassword = "2105";
        private readonly string targetDb = "JwelrySystemDBMSFinal";
        private readonly string defaultPassword = "2105";

        public void EnsureUserForEmail(string email)
        {
            string loginName = email; // giữ nguyên
            string userName = "Nhan_Vien_" + GetPrefixBeforeAt(email);
            ConnectionHelper.CurrentUserName = GetPrefixBeforeAt(email);
            using (SqlConnection conn = new SqlConnection(
                $"Server={server};Database=master;User Id={saUser};Password={saPassword};"))
            {
                conn.Open();

                // 1. Tạo LOGIN nếu chưa có
                if (!LoginExists(conn, loginName))
                {
                    CreateLogin(conn, loginName);
                }

                // 2. Tạo USER trong DB + gán role nếu chưa có
                CreateUserAndGrant(conn, loginName, userName);
            }
        }

        private bool LoginExists(SqlConnection conn, string loginName)
        {
            string query = "SELECT COUNT(*) FROM sys.sql_logins WHERE name = @LoginName";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@LoginName", loginName);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        private void CreateLogin(SqlConnection conn, string loginName)
        {
            string sql = $@"
                    CREATE LOGIN [{loginName}] 
                    WITH PASSWORD = '{defaultPassword}', CHECK_POLICY = OFF;";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private void CreateUserAndGrant(SqlConnection conn, string loginName, string userName)
        {
            ConnectionHelper.IsManager = false;

            string sql = $@"
                USE [{targetDb}];

                IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = '{userName}')
                BEGIN
                    CREATE USER [{userName}] FOR LOGIN [{loginName}];
                    EXEC sp_addrolemember 'NhanVienRole', '{userName}';
                END";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private string GetPrefixBeforeAt(string email)
        {
            int atIndex = email.IndexOf("@");
            return (atIndex > 0) ? email.Substring(0, atIndex) : email;
        }
    }
}

