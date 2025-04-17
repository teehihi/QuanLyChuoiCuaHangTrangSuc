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

    public class DBSupplier
    {
        private DAL dal;
        public DBSupplier()
        {
            dal = new DAL();
        }
        public DataSet LayNhaCungCap()
        {
            string query = "SELECT * From Supplier";
            return dal.ExecuteQueryDataSet(query, CommandType.Text);
        }
        // Thêm nhà cung cấp
        public bool ThemNhaCungCap(string tenNCC, string diaChi, string soDienThoai)
        {
            return dal.ExecuteNonQuery("sp_AddSupplier", new SqlParameter[]
            {
        new SqlParameter("@Name", tenNCC),
        new SqlParameter("@Address", diaChi),
        new SqlParameter("@Phone", soDienThoai)
            }) > 0;
        }
        // Sửa nhà cung cấp
        public bool SuaNhaCungCap(int supplierId, string tenNCC, string diaChi, string soDienThoai)
        {
            return dal.ExecuteNonQuery("sp_UpdateSupplier", new SqlParameter[]
            {
        new SqlParameter("@SupplierID", supplierId),
        new SqlParameter("@Name", tenNCC),
        new SqlParameter("@Address", diaChi),
        new SqlParameter("@Phone", soDienThoai)
            }) > 0;
        }

        // Xóa nhà cung cấp
        public bool XoaNhaCungCap(int supplierId)
        {
            return dal.ExecuteNonQuery("sp_DeleteSupplier", new SqlParameter[]
            {
        new SqlParameter("@SupplierID", supplierId)
            }) > 0;
        }



    }
}
