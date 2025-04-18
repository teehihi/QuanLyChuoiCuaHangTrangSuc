using DataAcessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer
{
    public class ProductBusiness
    {
        private static readonly DAL dal = new DAL();

        public static DataTable GetTopSellingProducts()
        {
            string storedProcedureName = "sp_GetTopSellingProducts";

            // Gọi stored procedure không có tham số
            DataSet ds = dal.ExecuteQueryDataSet(storedProcedureName, CommandType.StoredProcedure, null);

            // Trả về bảng đầu tiên
            return ds.Tables[0];
        }

        public static DataTable GetMonthlyStatistics()
        {
            string storedProcedureName = "sp_GetMonthlyStatistics";

            // Gọi stored procedure không có tham số
            DataSet ds = dal.ExecuteQueryDataSet(storedProcedureName, CommandType.StoredProcedure, null);

            // Trả về bảng đầu tiên
            return ds.Tables[0];
        }

    }
}
