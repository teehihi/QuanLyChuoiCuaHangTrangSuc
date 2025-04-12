using DataAcessLayer;
using System;
using System.Collections.Generic;
using System.Data;
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
    }
}
