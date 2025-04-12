using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcessLayer;

namespace BusinessAccessLayer
{
    public class DBChiTietHoaDon
    {
        private DAL dal = new DAL();

        // Lấy thông tin chi tiết hóa đơn từ view v_OrderDetails
        public DataTable GetOrderDetails(int orderId)
        {
            string query = "SELECT * FROM v_OrderDetails WHERE OrderID = @OrderID";
            SqlParameter[] parameters = { new SqlParameter("@OrderID", orderId) };
            DataSet ds = dal.ExecuteQueryDataSet(query, CommandType.Text, parameters);
            return ds.Tables[0];
        }

        // Lấy danh sách sản phẩm trong hóa đơn từ bảng OrderDetail
        public DataTable GetOrderItems(int orderId)
        {
            string query = @"
                SELECT od.ProductID, p.Name AS ProductName, od.Quantity, od.UnitPrice, od.SubTotal
                FROM OrderDetail od
                JOIN Product p ON od.ProductID = p.ProductID
                WHERE od.OrderID = @OrderID";
            SqlParameter[] parameters = { new SqlParameter("@OrderID", orderId) };
            DataSet ds = dal.ExecuteQueryDataSet(query, CommandType.Text, parameters);
            return ds.Tables[0];
        }

        // Lấy thông tin giảm giá từ view v_OrderWithPromotion
        public DataTable GetOrderDiscount(int orderId)
        {
            string query = "SELECT DiscountValue FROM v_OrderWithPromotion WHERE OrderID = @OrderID";
            SqlParameter[] parameters = { new SqlParameter("@OrderID", orderId) };
            DataSet ds = dal.ExecuteQueryDataSet(query, CommandType.Text, parameters);
            return ds.Tables[0];
        }

        public int InsertOrder(int customerId, DateTime orderDate, decimal totalAmount, string paymentMethod, decimal discount)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionHelper.CurrentConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
            INSERT INTO Orders (CustomerID, OrderDate, TotalAmount, PaymentMethod, Discount) 
            OUTPUT INSERTED.OrderID 
            VALUES (@CustomerID, @OrderDate, @TotalAmount, @PaymentMethod, @Discount)", conn);

                cmd.Parameters.AddWithValue("@CustomerID", customerId);
                cmd.Parameters.AddWithValue("@OrderDate", orderDate);
                cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                cmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
                cmd.Parameters.AddWithValue("@Discount", discount);

                return (int)cmd.ExecuteScalar();
            }
        }

        public void InsertOrderItem(int orderId, string productId, int quantity, decimal unitPrice)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionHelper.CurrentConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
            INSERT INTO OrderItems (OrderID, ProductID, Quantity, UnitPrice, SubTotal) 
            VALUES (@OrderID, @ProductID, @Quantity, @UnitPrice, @SubTotal)", conn);

                cmd.Parameters.AddWithValue("@OrderID", orderId);
                cmd.Parameters.AddWithValue("@ProductID", productId);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Parameters.AddWithValue("@UnitPrice", unitPrice);
                cmd.Parameters.AddWithValue("@SubTotal", unitPrice * quantity);

                cmd.ExecuteNonQuery();
            }
        }


    }

}



