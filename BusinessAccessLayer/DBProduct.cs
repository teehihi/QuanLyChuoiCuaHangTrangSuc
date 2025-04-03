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

    public class DBProduct
    {
        private DAL dal;
        public DBProduct()
        {
            dal = new DAL();
        }
        // Lấy danh sách sản phẩm
        public DataSet LayDanhSachTrangSuc()
        {
            string query = "SELECT p.*, pg.Name FROM Product p JOIN ProductGroup pg ON p.GroupID = pg.GroupID";
            return dal.ExecuteQueryDataSet(query, CommandType.Text);
        }
        public DataSet LayTrangSucTheoLoai(string loai)
        {
            if (string.IsNullOrEmpty(loai))
            {
                return LayDanhSachTrangSuc();
            }

            string query = "sp_GetProductsByGroup";
            return dal.ExecuteQueryDataSet(query, CommandType.StoredProcedure,
                new SqlParameter[] { new SqlParameter("@GroupName", loai) });
        }

        // Lấy Name từ GroupID
        public string LayGroupNameTuGroupID(int groupId)
        {
            string query = "SELECT Name FROM ProductGroup WHERE GroupID = @GroupID";
            DataSet ds = dal.ExecuteQueryDataSet(query, CommandType.Text, new SqlParameter[] { new SqlParameter("@GroupID", groupId) });

            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0]["Name"].ToString();
            }
            return string.Empty;
        }

        // Lấy GroupID từ Name
        public int LayGroupIDTuGroupName(string groupName)
        {
            string query = "SELECT GroupID FROM ProductGroup WHERE Name = @GroupName";
            DataSet ds = dal.ExecuteQueryDataSet(query, CommandType.Text, new SqlParameter[] { new SqlParameter("@GroupName", groupName) });

            if (ds.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(ds.Tables[0].Rows[0]["GroupID"]);
            }
            return 0;
        }


        // Thêm sản phẩm
        public bool ThemTrangSuc(string name, string material, int stockQuantity, decimal price, float weight, string description, int groupId, int branchId, byte[] prodImage)
        {
            return dal.ExecuteNonQuery("sp_AddProduct", new SqlParameter[]
            {
                new SqlParameter("@Name", name),
                new SqlParameter("@Material", material),
                new SqlParameter("@StockQuantity", stockQuantity),
                new SqlParameter("@Price", price),
                new SqlParameter("@Weight", weight),
                new SqlParameter("@Description", description),
                new SqlParameter("@GroupID", groupId),
                new SqlParameter("@BranchID", branchId), // Không thể null
                new SqlParameter("@ProdImage", (object)prodImage ?? DBNull.Value) // Hình ảnh có thể null
            }) > 0;
        }

        // Sửa sản phẩm
        public bool SuaTrangSuc(int productId, string name, string material, int stockQuantity, decimal price, float weight, string description, int groupId, int branchId, byte[] prodImage)
        {
            return dal.ExecuteNonQuery("sp_UpdateProduct", new SqlParameter[]
            {
                new SqlParameter("@ProductID", productId),
                new SqlParameter("@Name", name),
                new SqlParameter("@Material", material),
                new SqlParameter("@StockQuantity", stockQuantity),
                new SqlParameter("@Price", price),
                new SqlParameter("@Weight", weight),
                new SqlParameter("@Description", description),
                new SqlParameter("@GroupID", groupId),
                new SqlParameter("@BranchID", branchId), // Không thể null
                new SqlParameter("@ProdImage", (object)prodImage ?? DBNull.Value) // Hình ảnh có thể null
            }) > 0;
        }


        // Xóa sản phẩm
        public bool XoaTrangSuc(int productId)
        {
            return dal.ExecuteNonQuery("sp_DeleteProduct", new SqlParameter[]
            {
                new SqlParameter("@ProductID", productId)
            }) > 0;
        }


    }
}
