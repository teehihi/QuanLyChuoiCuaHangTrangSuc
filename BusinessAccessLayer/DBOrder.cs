using DataAcessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer
{
    public class DBOrder
    {
        private DAL db = new DAL();
        public DBOrder()
        {
            db = new DAL(); // Khởi tạo DAL
        }

    }
}
