using DataAcessLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer
{
    public class DBTransaction
    {
        private DAL db;

        public DBTransaction()
        {
            db = new DAL();
        }

        public bool AddTransaction(DateTime transactionDate, decimal amount, string type, string description, int branchID, int orderID, out string error)
        {
            error = "";
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionHelper.CurrentConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_AddTransaction", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TransactionDate", transactionDate);
                        cmd.Parameters.AddWithValue("@Amount", amount);
                        cmd.Parameters.AddWithValue("@Type", type);
                        cmd.Parameters.AddWithValue("@Description", description);
                        cmd.Parameters.AddWithValue("@BranchID", branchID);
                        cmd.Parameters.AddWithValue("@OrderID", orderID);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                error = "Lỗi khi thêm giao dịch: " + ex.Message;
                return false;
            }
        }
    }
}
