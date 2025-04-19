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
    public class DBPromotion
    {
        private DAL db = new DAL();

        public DataTable LayDanhSachKhuyenMai()
        {
            return db.ExecuteQueryDataSet("sp_LayDanhSachKhuyenMai", CommandType.StoredProcedure).Tables[0];
        }

        public DataSet TimKhuyenMaiTongHop(string tuKhoa)
        {
            return db.ExecuteQueryDataSet("sp_TimKhuyenMaiTongHop", CommandType.StoredProcedure,
                new SqlParameter[] { new SqlParameter("@TuKhoa", tuKhoa) });
        }

    }
}
