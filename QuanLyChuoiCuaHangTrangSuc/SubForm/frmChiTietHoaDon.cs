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
        DBCustomer dBCustomer = new DBCustomer();
        private List<CartItem> cartItems; // Lưu trữ giỏ hàng
        private DBChiTietHoaDon orderBAL = new DBChiTietHoaDon(); // Thực thi các thao tác với cơ sở dữ liệu
        private double discount;
        public frmChiTietHoaDon(List<CartItem> cartItems, double discountAmount)
        {
            InitializeComponent();
            this.cartItems = cartItems;
            discount = discountAmount; // Giảm giá (nếu có)
            LoadNewOrderInfo();  // Gọi phương thức hiển thị giỏ hàng khi form được tạo

        }

        private void frmChiTietHoaDon_Load(object sender, EventArgs e)
        {
            txtKhachHang.Focus();

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
            lblTamTinh.Text = tongTien.ToString("N0") + " VNĐ";
            lblGiamGia.Text = discount.ToString("N0") + " VNĐ";  // Mặc định là 0 (có thể chỉnh sửa sau)
            lblTotal.Text = tongTien.ToString("N0") + " VNĐ"; ;  // Mặc định hoặc có thể sử dụng ComboBox để chọn phương thức thanh toán
            txtNgayLap.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            txtMaHD.Text = "(Tự tạo)";

        }

        private void btnLuuHoaDon_Click(object sender, EventArgs e)
        {
            try
            {
                // Các thông tin cần thiết để lưu hóa đơn
                int customerId = 1;  // Mặc định hoặc lấy từ ComboBox
                string paymentMethod = "Tiền mặt";  // Mặc định hoặc lấy từ ComboBox
                decimal discount = decimal.Parse(lblGiamGia.Text);
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

        private void txtKhachHang_TextChanged(object sender, EventArgs e)
        {
            if (txtKhachHang.Text.Length == 10)
            {
                // Tìm kiếm khách hàng theo mã
                DataSet ds = dBCustomer.TimKhachHangTongHop(txtKhachHang.Text);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    txtKhachHang.Text = row["FullName"].ToString();
                }
                else
                {
                    tlpAddCustomer.Visible = true;
                    seph2.Visible = true;
                    txtCustomerPhone.Text = txtKhachHang.Text;
                    txtCustomerName.Focus();
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            dBCustomer.ThemKhachHang(txtCustomerName.Text, txtCustomerType.Text, txtCustomerAddress.Text, txtCustomerPhone.Text);
            
            seph2.Visible = false;
            tlpAddCustomer.Visible = false;
            ClearField();
            txtKhachHang_TextChanged(sender, e);
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            txtKhachHang.Clear();
            ClearField();
            tlpAddCustomer.Visible = false;
            seph2.Visible = false;
        }
        private void ClearField()
        {
            
            txtCustomerName.Clear();
            txtCustomerType.SelectedIndex = 0;
            cboThanhPho.SelectedIndex = 0;
            txtCustomerAddress.Clear();
            txtCustomerPhone.Clear();
            txtKhachHang.Focus();
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            frmPayment paymentForm = new frmPayment();
            paymentForm.ShowDialog();
        }
    }
}
