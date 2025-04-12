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
    }

}



