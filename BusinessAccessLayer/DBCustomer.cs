using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcessLayer;
namespace BusinessAccessLayer
{
   
    public class DBCustomer
    {
        private DAL dal;
        public DBCustomer()
        { 
            dal = new DAL();
        }
        public DataSet LayKhachHang()
        {
            string query = "SELECT * From Customer";
            return dal.ExecuteQueryDataSet(query, CommandType.Text);
        }
        public DataSet TimKhachHangTheoTen(string ten)
        {
            if (string.IsNullOrEmpty(ten))
            {
                return LayKhachHang(); // Hàm này anh có thể đã có rồi
            }

            string query = "sp_TimKhachHangTheoTen";
            return dal.ExecuteQueryDataSet(query, CommandType.StoredProcedure,
                new SqlParameter[] { new SqlParameter("@Name", ten) });
        }
        public DataSet TimKhachHangTheoDiaChi(string diaChi)
        {
            if (string.IsNullOrEmpty(diaChi))
            {
                return LayKhachHang(); // hoặc hàm load full
            }

            string query = "sp_TimKhachHangTheoDiaChi";
            return dal.ExecuteQueryDataSet(query, CommandType.StoredProcedure,
                new SqlParameter[] { new SqlParameter("@DiaChi", diaChi) });
        }
        // Thêm khách hàng
        public bool ThemKhachHang(string fullName, string customerType, string address, string phone)
        {
            return dal.ExecuteNonQuery("sp_AddCustomer", new SqlParameter[]
            {
                new SqlParameter("@FullName", fullName),
                new SqlParameter("@CustomerType", customerType),
                new SqlParameter("@Address", address),
                new SqlParameter("@Phone", phone)
            }) > 0;
        }

        // Sửa khách hàng
        public bool SuaKhachHang(int customerId, string fullName, string customerType, string address, string phone)
        {
            return dal.ExecuteNonQuery("sp_UpdateCustomer", new SqlParameter[]
            {
                new SqlParameter("@CustomerID", customerId),
                new SqlParameter("@FullName", fullName),
                new SqlParameter("@CustomerType", customerType),
                new SqlParameter("@Address", address),
                new SqlParameter("@Phone", phone)
            }) > 0;
        }

        // Xóa khách hàng
        public bool XoaKhachHang(int customerId)
        {
            return dal.ExecuteNonQuery("sp_DeleteCustomer", new SqlParameter[]
            {
                new SqlParameter("@CustomerID", customerId)
            }) > 0;
        }
        //Gộp cả 3 loại tìm 
        public DataSet TimKhachHangTongHop(string tuKhoa)
        {
            return dal.ExecuteQueryDataSet("sp_TimKhachHangTongHop", CommandType.StoredProcedure,
                new SqlParameter[] { new SqlParameter("@TuKhoa", tuKhoa) });
        }

        public static int GetCustomerIDByName(string customerName)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionHelper.CurrentConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT CustomerID FROM Customer WHERE FullName = @FullName", conn);
                cmd.Parameters.AddWithValue("@FullName", customerName);

                object result = cmd.ExecuteScalar();
                if (result != null)
                    return Convert.ToInt32(result);
                else
                    throw new Exception("Không tìm thấy ứng dụng với tên: " + customerName);
            }
        }

        public string LayTenKhachHangTheoID(int customerId)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionHelper.CurrentConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT FullName FROM Customer WHERE CustomerID = @CustomerID", conn);
                cmd.Parameters.AddWithValue("@CustomerID", customerId);

                object result = cmd.ExecuteScalar();
                return result != null ? result.ToString() : null;
            }
        }


    }
}
