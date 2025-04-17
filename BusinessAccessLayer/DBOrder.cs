using DataAcessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace BusinessAccessLayer
{
    public class DBOrder
    {
        private DAL db;

        public DBOrder()
        {
            db = new DAL();
        }

        public int SaveOrder_UsingSP(decimal totalAmount, string shippingMethod, int branchID, int customerID,
            int appID, int promotionID, decimal discountValue, List<OrderItem> orderItems, out string error)
        {
            error = "";
            List<SqlCommand> commands = new List<SqlCommand>();

            // 1. Thêm hóa đơn (gọi stored procedure sp_AddOrder)
            SqlCommand cmdOrder = new SqlCommand("[dbo].[sp_AddOrder]");
            cmdOrder.CommandType = CommandType.StoredProcedure;
            cmdOrder.Parameters.AddWithValue("@CustomerID", customerID);
            cmdOrder.Parameters.AddWithValue("@BranchID", branchID);
            cmdOrder.Parameters.AddWithValue("@AppID", appID);
            cmdOrder.Parameters.AddWithValue("@TotalAmount", totalAmount);
            cmdOrder.Parameters.AddWithValue("@PaymentMethod", "Cash");
            cmdOrder.Parameters.AddWithValue("@ShippingMethod", shippingMethod);
            cmdOrder.Parameters.AddWithValue("@OrderStatus", "Pending");
            commands.Add(cmdOrder);

            // 2. Thêm chi tiết hóa đơn (OrderDetail)
            foreach (var item in orderItems)
            {
                SqlCommand cmdDetail = new SqlCommand();
                cmdDetail.CommandText = @"
            INSERT INTO OrderDetail (OrderID, ProductID, Quantity, UnitPrice, SubTotal)
            VALUES (@OrderID, @ProductID, @Quantity, @UnitPrice, @SubTotal);";
                cmdDetail.Parameters.AddWithValue("@OrderID", 0); // Sẽ được thay thế sau khi có OrderID
                cmdDetail.Parameters.AddWithValue("@ProductID", item.ProductID);
                cmdDetail.Parameters.AddWithValue("@Quantity", item.Quantity);
                cmdDetail.Parameters.AddWithValue("@UnitPrice", item.UnitPrice);
                cmdDetail.Parameters.AddWithValue("@SubTotal", item.SubTotal);
                commands.Add(cmdDetail);
            }

            // 3. Chỉ thêm khuyến mãi nếu promotionID > 0 (coi như 0 là không chọn)
            if (promotionID > 0)
            {
                SqlCommand cmdPromotion = new SqlCommand();
                cmdPromotion.CommandText = @"
            INSERT INTO OrderPromotion (OrderID, PromotionID, DiscountValue)
            VALUES (@OrderID, @PromotionID, @DiscountValue);";
                cmdPromotion.Parameters.AddWithValue("@OrderID", 0); // Sẽ được thay thế sau khi có OrderID
                cmdPromotion.Parameters.AddWithValue("@PromotionID", promotionID);
                cmdPromotion.Parameters.AddWithValue("@DiscountValue", discountValue);
                commands.Add(cmdPromotion);
            }

            // Thực thi giao dịch
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionHelper.CurrentConnectionString))
                {
                    conn.Open();
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Thực thi lệnh đầu tiên (sp_AddOrder) để lấy OrderID
                            cmdOrder.Connection = conn;
                            cmdOrder.Transaction = transaction;
                            object result = cmdOrder.ExecuteScalar();
                            if (result == null)
                            {
                                throw new Exception("Không thể lấy OrderID từ stored procedure sp_AddOrder.");
                            }
                            int orderID = Convert.ToInt32(result);

                            // Cập nhật OrderID cho các lệnh còn lại
                            foreach (var cmd in commands.Skip(1)) // Bỏ qua lệnh đầu tiên (sp_AddOrder)
                            {
                                cmd.Connection = conn;
                                cmd.Transaction = transaction;
                                foreach (SqlParameter param in cmd.Parameters)
                                {
                                    if (param.ParameterName == "@OrderID")
                                    {
                                        param.Value = orderID;
                                    }
                                }
                                cmd.ExecuteNonQuery();
                            }

                            // Commit giao dịch
                            transaction.Commit();
                            return orderID;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            error = "Lỗi khi lưu hóa đơn: " + ex.Message;
                            return -1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error = "Lỗi kết nối cơ sở dữ liệu: " + ex.Message;
                return -1;
            }
        }

        public bool UpdateOrder_UsingSP(int orderID, decimal totalAmount, string shippingMethod, int branchID, int customerID,
            int appID, int promotionID, decimal discountValue, List<OrderItem> orderItems, out string error)
        {
            error = "";
            List<SqlCommand> commands = new List<SqlCommand>();

            // 1. Gọi stored procedure cập nhật đơn hàng
            SqlCommand cmdUpdateOrder = new SqlCommand("[dbo].[sp_UpdateOrder]");
            cmdUpdateOrder.CommandType = CommandType.StoredProcedure;
            cmdUpdateOrder.Parameters.AddWithValue("@OrderID", orderID);
            cmdUpdateOrder.Parameters.AddWithValue("@CustomerID", customerID);
            cmdUpdateOrder.Parameters.AddWithValue("@OrderDate", DateTime.Now); // cập nhật thời gian mới
            cmdUpdateOrder.Parameters.AddWithValue("@TotalAmount", totalAmount);
            cmdUpdateOrder.Parameters.AddWithValue("@OrderStatus", "Pending");
            cmdUpdateOrder.Parameters.AddWithValue("@ShippingMethod", shippingMethod);
            cmdUpdateOrder.Parameters.AddWithValue("@PaymentMethod", "Cash");
            cmdUpdateOrder.Parameters.AddWithValue("@BranchID", branchID);
            cmdUpdateOrder.Parameters.AddWithValue("@AppID", appID);
            commands.Add(cmdUpdateOrder);

            // 2. Xoá chi tiết đơn hàng cũ
            SqlCommand cmdDeleteDetails = new SqlCommand("DELETE FROM OrderDetail WHERE OrderID = @OrderID;");
            cmdDeleteDetails.Parameters.AddWithValue("@OrderID", orderID);
            commands.Add(cmdDeleteDetails);

            // 3. Xoá khuyến mãi cũ (nếu có)
            SqlCommand cmdDeletePromo = new SqlCommand("DELETE FROM OrderPromotion WHERE OrderID = @OrderID;");
            cmdDeletePromo.Parameters.AddWithValue("@OrderID", orderID);
            commands.Add(cmdDeletePromo);

            // 4. Thêm lại OrderDetail mới
            foreach (var item in orderItems)
            {
                SqlCommand cmdDetail = new SqlCommand(@"
            INSERT INTO OrderDetail (OrderID, ProductID, Quantity, UnitPrice, SubTotal)
            VALUES (@OrderID, @ProductID, @Quantity, @UnitPrice, @SubTotal);");
                cmdDetail.Parameters.AddWithValue("@OrderID", orderID);
                cmdDetail.Parameters.AddWithValue("@ProductID", item.ProductID);
                cmdDetail.Parameters.AddWithValue("@Quantity", item.Quantity);
                cmdDetail.Parameters.AddWithValue("@UnitPrice", item.UnitPrice);
                cmdDetail.Parameters.AddWithValue("@SubTotal", item.SubTotal);
                commands.Add(cmdDetail);
            }

            // 5. Thêm lại khuyến mãi nếu có
            if (promotionID > 0)
            {
                SqlCommand cmdPromo = new SqlCommand(@"
            INSERT INTO OrderPromotion (OrderID, PromotionID, DiscountValue)
            VALUES (@OrderID, @PromotionID, @DiscountValue);");
                cmdPromo.Parameters.AddWithValue("@OrderID", orderID);
                cmdPromo.Parameters.AddWithValue("@PromotionID", promotionID);
                cmdPromo.Parameters.AddWithValue("@DiscountValue", discountValue);
                commands.Add(cmdPromo);
            }

            // 6. Thực thi giao dịch
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionHelper.CurrentConnectionString))
                {
                    conn.Open();
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            foreach (var cmd in commands)
                            {
                                cmd.Connection = conn;
                                cmd.Transaction = transaction;
                                cmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            error = "Lỗi khi cập nhật đơn hàng: " + ex.Message;
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error = "Lỗi kết nối cơ sở dữ liệu: " + ex.Message;
                return false;
            }
        }


        public bool DeleteOrder_UsingSP(int orderID, out string error)
        {
            error = "";
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionHelper.CurrentConnectionString))
                {
                    conn.Open();

                    // Tạo lệnh gọi stored procedure
                    using (SqlCommand cmd = new SqlCommand("[dbo].[sp_DeleteOrder]", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@OrderID", orderID);

                        // Thực thi stored procedure
                        cmd.ExecuteNonQuery();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                error = "Lỗi khi xóa đơn hàng: " + ex.Message;
                return false;
            }
        }

        //PAYMENT

        //Cập nhật PaymentMethod và OrderStatus
        public bool UpdatePaymentAndStatus(int orderID, string paymentMethod, out string error)
        {
            error = "";
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionHelper.CurrentConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("UPDATE OrderTable SET PaymentMethod = @PaymentMethod, OrderStatus = 'Completed' WHERE OrderID = @OrderID", conn))
                    {
                        cmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
                        cmd.Parameters.AddWithValue("@OrderID", orderID);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return true;
                        }
                        else
                        {
                            error = "Không tìm thấy đơn hàng để cập nhật!";
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error = "Lỗi khi cập nhật thanh toán: " + ex.Message;
                return false;
            }
        }

        public DataSet LayHoaDonTheoChiNhanh(string branchID)
        {
            string query = "SELECT * FROM OrderTable WHERE BranchID = @BranchID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@BranchID", branchID)
            };
            return db.ExecuteQueryDataSet(query, CommandType.Text, parameters);
        }


        public DataSet LayHoaDonTheoBoLoc(
            string branchID,
            string paymentMethod,
            string shippingMethod,
            string appID,
            string orderStatus)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@BranchID", branchID),
                new SqlParameter("@PaymentMethod", paymentMethod),
                new SqlParameter("@ShippingMethod", shippingMethod),
                new SqlParameter("@AppID", appID),
                new SqlParameter("@OrderStatus", orderStatus)
            };

            return db.ExecuteQueryDataSet("sp_LayHoaDonTheoBoLoc", CommandType.StoredProcedure, parameters);
        }




        public DataSet LayChiTietHoaDon(int orderID)
        {
            DataSet ds = new DataSet();

            using (SqlConnection conn = new SqlConnection(ConnectionHelper.CurrentConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_GetOrderFullInfo", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrderID", orderID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                
                ds.Tables[0].TableName = "Order";
                ds.Tables[1].TableName = "OrderDetail";
                ds.Tables[2].TableName = "Promotion";
                ds.Tables[3].TableName = "ShippingMethod";
                ds.Tables[4].TableName = "Application";
            }

            return ds;
        }

    }

    public class OrderItem
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal => Quantity * UnitPrice;
    }

    

}


