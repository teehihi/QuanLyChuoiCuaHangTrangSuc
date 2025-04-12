using BusinessAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyChuoiCuaHangTrangSuc.SubForm
{
    public partial class frmChiTietHoaDon : Form
    {
        private List<CartItem> cartItems; // Lưu trữ giỏ hàng
        private DBChiTietHoaDon orderBAL = new DBChiTietHoaDon(); // Thực thi các thao tác với cơ sở dữ liệu

        public frmChiTietHoaDon(List<CartItem> cartItems)
        {
            InitializeComponent();
            this.cartItems = cartItems;
            LoadNewOrderInfo();  // Gọi phương thức hiển thị giỏ hàng khi form được tạo

        }

        private void frmChiTietHoaDon_Load(object sender, EventArgs e)
        {

            // Chỉnh sửa tiêu đề form
            lblHoaDonInf.Left = (this.ClientSize.Width - lblHoaDonInf.Width) / 2;

            // Thiết lập giao diện cho DataGridView
            dgvChiTiet.ColumnHeadersDefaultCellStyle.BackColor = Color.MediumSlateBlue;
            dgvChiTiet.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvChiTiet.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvChiTiet.EnableHeadersVisualStyles = false;
        }

        private void LoadNewOrderInfo()
        {
            dgvChiTiet.Rows.Clear();  // Xóa các dòng cũ
            decimal tongTien = 0;
            // Thêm các cột đã có
            dgvChiTiet.Columns.Add("ProductName", "Tên sản phẩm");
            dgvChiTiet.Columns.Add("Quantity", "Số lượng");
            dgvChiTiet.Columns.Add("Price", "Giá");

            // Thêm cột mới (cột Thành Tiền)
            dgvChiTiet.Columns.Add("SubTotal", "Thành tiền");
            
            foreach (var item in cartItems)
            {
                decimal thanhTien = item.Quantity * item.Price;
                
                dgvChiTiet.Rows.Add(item.Name, item.Quantity, item.Price.ToString("N0"), thanhTien.ToString("N0"));
                tongTien += thanhTien;
            }

            // Hiển thị thông tin tổng tiền và các thông tin khác
            txtTong.Text = tongTien.ToString("N0");
            txtGiamGia.Text = "0.00";  // Mặc định là 0 (có thể chỉnh sửa sau)
            txtThanhToan.Text = tongTien.ToString("N0"); ;  // Mặc định hoặc có thể sử dụng ComboBox để chọn phương thức thanh toán
            txtNgayLap.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            txtMaHD.Text = "(Tự tạo)";
            txtKhachHang.Text = "Khách lẻ";  // Mặc định hoặc có thể chọn khách hàng từ ComboBox
        }

        private void btnLuuHoaDon_Click(object sender, EventArgs e)
        {
            try
            {
                // Các thông tin cần thiết để lưu hóa đơn
                int customerId = 1;  // Mặc định hoặc lấy từ ComboBox
                string paymentMethod = "Tiền mặt";  // Mặc định hoặc lấy từ ComboBox
                decimal discount = decimal.Parse(txtGiamGia.Text);
                decimal total = cartItems.Sum(i => i.Price * i.Quantity) - discount;

                // Lưu hóa đơn mới
                int newOrderId = orderBAL.InsertOrder(customerId, DateTime.Now, total, paymentMethod, discount);

                // Lưu các sản phẩm trong hóa đơn
                foreach (var item in cartItems)
                {
                    orderBAL.InsertOrderItem(newOrderId, item.ProductID, item.Quantity, item.Price);
                }

                MessageBox.Show("Lưu hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
