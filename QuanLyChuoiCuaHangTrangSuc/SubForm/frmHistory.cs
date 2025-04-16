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
        private DBApplication dbApp = new DBApplication();
        private DataTable originalData;

        public frmHistory()
        {
            InitializeComponent();
            cboPayment.SelectedIndex = 0;
            cboDeli.SelectedIndex = 0;
            cboApp.SelectedIndex = 0;
            cboStatus.SelectedIndex = 0;
            dtpStart.Value = Convert.ToDateTime("01/01/2024");
            dtpEnd.Value = DateTime.Now;
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
                    var item = CreateOrderHistoryItem(row);

                    flpOrderHistory.Controls.Add(item);
                }
            }
            originalData = ds.Tables[0];
            
        }

        private void DisplayFilteredData()
        {
            flpOrderHistory.Controls.Clear();

            DateTime startDate = dtpStart.Value.Date;
            DateTime endDate = dtpEnd.Value.Date;

            var filteredRows = originalData.AsEnumerable()
                .Where(row =>
                {
                    DateTime orderDate = row.Field<DateTime>("OrderDate").Date;
                    return orderDate >= startDate && orderDate <= endDate;
                });

            foreach (var row in filteredRows)
            {
                var item = CreateOrderHistoryItem(row);
                flpOrderHistory.Controls.Add(item);
            }
        }

        private UCController.UCOrderHistory CreateOrderHistoryItem(DataRow row)
        {
            var item = new UCController.UCOrderHistory();

            item.OrderID = row["OrderID"].ToString();
            item.CustomerName = dbCustomer.LayTenKhachHangTheoID(Convert.ToInt32(row["CustomerID"]));
            item.PaymentMethod = row["PaymentMethod"].ToString();
            item.OrderDelivery = row["ShippingMethod"].ToString();
            item.OrderApp = dbApp.GetAppNameByID(Convert.ToInt32(row["AppID"]));
            item.OrderDate = Convert.ToDateTime(row["OrderDate"]).ToString("dd/MM/yyyy");
            item.OrderStatus = row["OrderStatus"].ToString();
            item.TotalAmount = $"{Convert.ToDecimal(row["TotalAmount"]):N0} VNĐ";

            // Gán sự kiện click
            item.OrderClicked += Item_OrderClicked;

            return item;
        }

        private void Item_OrderClicked(object sender, string orderID)
        {
            // Mở form chi tiết
            var chiTietForm = new frmChiTietHoaDon(Convert.ToInt32(orderID));
            chiTietForm.ShowDialog();
        }


        private void dtpStart_ValueChanged(object sender, EventArgs e)
        {
            DisplayFilteredData();
        }

        private void dtpEnd_ValueChanged(object sender, EventArgs e)
        {
            DisplayFilteredData();
        }

        private void FilterOrders()
        {
            string branchID = "1";

            // Nếu SelectedIndex = 0 (mặc định) => bỏ qua
            string payment = cboPayment.SelectedIndex > 0 ? cboPayment.SelectedItem.ToString() : "";
            string shipping = cboDeli.SelectedIndex > 0 ? cboDeli.SelectedItem.ToString() : "";
            string status = cboStatus.SelectedIndex > 0 ? cboStatus.SelectedItem.ToString() : "";

            string appID = "";
            if (cboApp.SelectedIndex > 0)
            {
                try { appID = DBApplication.GetAppIDByName(cboApp.SelectedItem.ToString()).ToString(); }
                catch { appID = ""; }
            }

            DataSet ds = dbOrder.LayHoaDonTheoBoLoc(branchID, payment, shipping, appID, status);


            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                flpOrderHistory.Controls.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    var item = CreateOrderHistoryItem(row);
                    flpOrderHistory.Controls.Add(item);
                }

                originalData = dt;
                DisplayFilteredData(); // nếu bạn muốn kết hợp lọc theo ngày
            }
        }

        private void cbFilter_Changed(object sender, EventArgs e)
        {
            FilterOrders();
        }

        private void btnClearFilters_Click(object sender, EventArgs e)
        {
            cboPayment.SelectedIndex = 0;
            cboDeli.SelectedIndex = 0;
            cboApp.SelectedIndex = 0;
            cboStatus.SelectedIndex = 0;
            dtpStart.Value = Convert.ToDateTime("01/01/2024");
            dtpEnd.Value = DateTime.Now;
            LoadHistory();
        }
    }
}
