using BusinessAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace QuanLyChuoiCuaHangTrangSuc.SubForm
{
    public partial class frmChiTietHoaDon : Form
    {
        private DBChiTietHoaDon orderBAL = new DBChiTietHoaDon();
        private int orderId;
        public frmChiTietHoaDon()
        {
            InitializeComponent();
            this.orderId = orderId;
            TaiThongTinHoaDon();
        }

        private void frmChiTietHoaDon_Load(object sender, EventArgs e)
        {
            //chỉnh sửa tiêu đề form
            lblHoaDonInf.Left = (this.ClientSize.Width - lblHoaDonInf.Width) / 2;


            dgvChiTiet.ColumnHeadersDefaultCellStyle.BackColor = Color.MediumSlateBlue;
            dgvChiTiet.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvChiTiet.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvChiTiet.EnableHeadersVisualStyles = false;
        }
        private void TaiThongTinHoaDon()
        {
            try
            {
                // Lấy thông tin hóa đơn
                DataTable dtOrder = orderBAL.GetOrderDetails(orderId);
                if (dtOrder.Rows.Count > 0)
                {
                    DataRow row = dtOrder.Rows[0];
                    txtMaHD.Text = row["OrderID"].ToString();
                    txtNgayLap.Text = Convert.ToDateTime(row["OrderDate"]).ToString("dd/MM/yyyy HH:mm:ss");
                    txtKhachHang.Text = row["CustomerName"].ToString();
                    txtTong.Text = Convert.ToDecimal(row["TotalAmount"]).ToString("N2");
                    txtThanhToan.Text = row["PaymentMethod"].ToString();
                }

                // Lấy danh sách sản phẩm
                DataTable dtItems = orderBAL.GetOrderItems(orderId);
                dgvChiTiet.Rows.Clear();
                foreach (DataRow row in dtItems.Rows)
                {
                    dgvChiTiet.Rows.Add(row["ProductName"], row["Quantity"], Convert.ToDecimal(row["UnitPrice"]).ToString("N2"), Convert.ToDecimal(row["SubTotal"]).ToString("N2"));
                }

                // Lấy thông tin giảm giá
                DataTable dtDiscount = orderBAL.GetOrderDiscount(orderId);
                if (dtDiscount.Rows.Count > 0)
                {
                    txtGiamGia.Text = Convert.ToDecimal(dtDiscount.Rows[0]["DiscountValue"]).ToString("N2");
                }
                else
                {
                    txtGiamGia.Text = "0.00";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}


