using BusinessAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyChuoiCuaHangTrangSuc.SubForm
{
    public partial class frmHistory : Form
    {
        private DBOrder dbOrder = new DBOrder();
        private DBCustomer dbCustomer = new DBCustomer();
        public frmHistory()
        {
            InitializeComponent();
        }

        private void frmHistory_Load(object sender, EventArgs e)
        {
            LoadHistory();
        }
        private void LoadHistory()
        {
            string BranchID = "1";
            DataSet ds = dbOrder.LayHoaDonTheoChiNhanh(BranchID);
            
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                foreach (DataRow row in dt.Rows)
                {
                    
                    UCController.UCOrderHistory item = new UCController.UCOrderHistory();
                    item.OrderID = row["OrderID"].ToString();
                    item.CustomerName = dbCustomer.LayTenKhachHangTheoID(Convert.ToInt32(row["CustomerID"]));
                    item.PaymentMethod = row["PaymentMethod"].ToString();
                    item.OrderDelivery = row["ShippingMethod"].ToString();
                    item.OrderApp = dbOrder.GetAppNameByID(Convert.ToInt32(row["AppID"]));
                    item.OrderDate = Convert.ToDateTime(row["OrderDate"]).ToString("dd/MM/yyyy");
                    item.OrderStatus = row["OrderStatus"].ToString();
                    item.TotalAmount = $"{Convert.ToDecimal(row["TotalAmount"]):N0} VNĐ";
                    

                    flpOrderHistory.Controls.Add(item);
                }
            }
        }
    }
}
