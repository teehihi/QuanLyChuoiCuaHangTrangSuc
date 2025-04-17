using BusinessAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Windows.Shapes;

namespace QuanLyChuoiCuaHangTrangSuc.SubForm
{

    public partial class frmChiTietHoaDon : Form
    {
        private DBOrder dbOrder = new DBOrder();
        DBCustomer dBCustomer = new DBCustomer();
        private List<CartItem> cartItems; // Lưu trữ giỏ hàng
        private DBChiTietHoaDon orderBAL = new DBChiTietHoaDon(); // Thực thi các thao tác với cơ sở dữ liệu
        private double discount;
        private decimal tongTien;
        private int promotionId;
        private double toTal;

        private int orderId;


        private PrintDocument printDocument;
        private PrintPreviewDialog printPreviewDialog;
        public frmChiTietHoaDon(List<CartItem> cartItems, double discountAmount, int promotionId)
        {
            InitializeComponent();
            this.cartItems = cartItems;
            discount = discountAmount; // Giảm giá (nếu có)
            LoadNewOrderInfo();  // Gọi phương thức hiển thị giỏ hàng khi form được tạo
            txtKhachHang.Focus();
            this.promotionId = promotionId;


            printDocument = new PrintDocument();
            printPreviewDialog = new PrintPreviewDialog();

            // Gán sự kiện in
            printDocument.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);
            printPreviewDialog.Size = new Size(800, 600); // Thiết lập kích thước hộp thoại
            printPreviewDialog.StartPosition = FormStartPosition.CenterScreen; // Căn giữa màn hình

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
            tongTien = 0;
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
            toTal = Convert.ToDouble(tongTien) - discount;
            // Hiển thị thông tin tổng tiền và các thông tin khác
            lblTamTinh.Text = tongTien.ToString("N0") + " VNĐ";
            lblGiamGia.Text = discount.ToString("N0") + " VNĐ"; 
            lblTotal.Text = toTal.ToString("N0") + " VNĐ"; ; 
            txtNgayLap.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            txtMaHD.Text = "";

        }



        private void btnDong_Click(object sender, EventArgs e)
        {
            // Nếu Owner là frmInvoices và chưa bị Dispose
            if (this.Owner is frmInvoices parentForm && !parentForm.IsDisposed)
            {
                parentForm.ClearCart(); // Gọi hàm dọn giỏ hàng
            }

            this.Close(); // Đóng form hiện tại
        }


        private void txtKhachHang_TextChanged(object sender, EventArgs e)
        {
            cboThanhPho.SelectedIndex = 0;
            txtCustomerType.SelectedIndex = 0;
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
            else
            {
                tlpAddCustomer.Visible = false;
                seph2.Visible = false;
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (!KiemTraThongTinDayDu(tlpAddCustomer))
                return;
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
            if(string.IsNullOrWhiteSpace(txtKhachHang.Text))
            {
                MessageBox.Show("Vui lòng nhập thông tin khách hàng trước khi thanh toán!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtKhachHang.Focus();
            }
            else
            {
                frmPayment paymentForm = new frmPayment(Convert.ToInt32(toTal),orderId);
                paymentForm.ShowDialog();
            }
        }

        private void btnLuuHD_Click(object sender, EventArgs e)
        {
            try
            {
                string tenKH = txtKhachHang.Text.Trim();
                int customerID = DBCustomer.GetCustomerIDByName(tenKH);
                int branchID = 1; // Cũng mặc định luôn
                string selectedAppName = cboApp.SelectedItem.ToString();
                int appID = DBApplication.GetAppIDByName(selectedAppName);
                int promotionID = promotionId; // Truyền từ form frmOrder
                string discountText = lblGiamGia.Text.Replace("VNĐ", "").Replace(",", "").Trim();
                decimal discountValue = Convert.ToDecimal(discountText);
                decimal total = cartItems.Sum(i => i.Price * i.Quantity) - discountValue;
                string shippingMethod = cboShippingMethod.SelectedItem?.ToString() ?? "";

                // Convert CartItem sang OrderItem
                List<OrderItem> orderItems = cartItems.Select(ci => new OrderItem
                {
                    ProductID = Convert.ToInt32(ci.ProductID),
                    Quantity = ci.Quantity,
                    UnitPrice = ci.Price,
                    
                }).ToList();

                string error;
                int orderID = dbOrder.SaveOrder_UsingSP(total, shippingMethod, branchID, customerID, appID, promotionID, discountValue, orderItems, out error);

                if (orderID == -1)
                {
                    MessageBox.Show(error, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // Gán mã hóa đơn vừa tạo vào txtMaHD.Text
                    orderId = orderID;
                    txtMaHD.Text = orderID.ToString();
                    txtKhachHang.Enabled = false;
                    cboApp.Enabled = false;
                    cboShippingMethod.Enabled = false;
                    btnLuu.Enabled = false;
                    btnPay.Enabled = true;
                    MessageBox.Show("Lưu hóa đơn thành công! Mã HD: " + orderID, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Hàm kiểm tra tự động các ô nhập liệu trong tlpAddCustomer
        private bool KiemTraThongTinDayDu(TableLayoutPanel tlp)
        {
            foreach (Control ctrl in tlp.Controls)
            {
                if (ctrl is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Focus();
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                else if (ctrl is ComboBox comboBox && comboBox.SelectedIndex == 0)
                {
                    comboBox.Focus();
                    MessageBox.Show("Vui lòng chọn đầy đủ thông tin khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                else if (ctrl is RichTextBox richTextBox && string.IsNullOrWhiteSpace(richTextBox.Text))
                {
                    richTextBox.Focus();
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            return true;
        }


        public frmChiTietHoaDon(int orderID)
        {
            InitializeComponent();
            this.orderId = orderID;
            LoadOldOrderInfo(); // Hàm mới để hiển thị hóa đơn cũ

            // Disable thao tác chỉnh sửa
            DisableEditing();

            printDocument = new PrintDocument();
            printPreviewDialog = new PrintPreviewDialog();

            // Gán sự kiện in
            printDocument.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);
            printPreviewDialog.Size = new Size(800, 600); // Thiết lập kích thước hộp thoại
            printPreviewDialog.StartPosition = FormStartPosition.CenterScreen; // Căn giữa màn hình
        }
        private void LoadOldOrderInfo()
        {
            DataSet ds = dbOrder.LayChiTietHoaDon(orderId);

            if (ds.Tables["Order"].Rows.Count > 0)
            {
                DataRow row = ds.Tables["Order"].Rows[0];
                txtKhachHang.Text = dBCustomer.LayTenKhachHangTheoID(Convert.ToInt32(row["CustomerID"]));
                txtNgayLap.Text = Convert.ToDateTime(row["OrderDate"]).ToString("dd/MM/yyyy HH:mm:ss");
                txtMaHD.Text = orderId.ToString();
                lblTotal.Text = Convert.ToDecimal(row["TotalAmount"]).ToString("N0") + " VNĐ";

                // Đổ dữ liệu vào ComboBox cboShippingMethod từ cột ShippingMethod trong OrderTable
                string shippingMethod = ds.Tables["ShippingMethod"].Rows[0]["ShippingMethod"].ToString();
                cboShippingMethod.Items.Add(shippingMethod);
                cboShippingMethod.SelectedItem = shippingMethod;

                // Đổ dữ liệu vào ComboBox cboApp từ bảng Application
                cboApp.DataSource = ds.Tables["Application"];
                cboApp.DisplayMember = "Name";  // Hiển thị tên ứng dụng
                cboApp.ValueMember = "AppID";  // Sử dụng AppID làm giá trị

                // Set giá trị ứng dụng đã chọn
                int appId = Convert.ToInt32(row["AppID"]);
                cboApp.SelectedValue = appId;
            }

            dgvChiTiet.Rows.Clear();
            dgvChiTiet.Columns.Clear();

            dgvChiTiet.Columns.Add("ProductName", "Tên sản phẩm");
            dgvChiTiet.Columns.Add("Quantity", "Số lượng");
            dgvChiTiet.Columns.Add("Price", "Giá");
            dgvChiTiet.Columns.Add("SubTotal", "Thành tiền");

            foreach (DataRow detailRow in ds.Tables["OrderDetail"].Rows)
            {
                string productName = detailRow["Name"].ToString();
                int quantity = Convert.ToInt32(detailRow["Quantity"]);
                decimal unitPrice = Convert.ToDecimal(detailRow["UnitPrice"]);
                decimal subTotal = Convert.ToDecimal(detailRow["SubTotal"]);

                dgvChiTiet.Rows.Add(productName, quantity, unitPrice.ToString("N0"), subTotal.ToString("N0"));
            }

            // Giảm giá (nếu có)
            if (ds.Tables.Contains("Promotion") && ds.Tables["Promotion"].Rows.Count > 0)
            {
                decimal discountAmount = Convert.ToDecimal(ds.Tables["Promotion"].Rows[0]["DiscountValue"]);
                lblGiamGia.Text = discountAmount.ToString("N0") + " VNĐ";
            }
            else
            {
                lblGiamGia.Text = "0 VNĐ";
            }

            // Tạm tính
            decimal tamTinh = dgvChiTiet.Rows.Cast<DataGridViewRow>()
                .Sum(r => Convert.ToDecimal(r.Cells["SubTotal"].Value.ToString().Replace(",", "")));

            lblTamTinh.Text = tamTinh.ToString("N0") + " VNĐ";
        }



        private void DisableEditing()
        {
            txtKhachHang.ReadOnly = true;
            txtMaHD.ReadOnly = true;
            txtNgayLap.ReadOnly = true;

            dgvChiTiet.ReadOnly = true;
            dgvChiTiet.AllowUserToAddRows = false;

            
            btnLuu.Enabled = false;
            btnPay.Enabled = false;
            btnHuy.Enabled = false;
        }

        private void btnInHoaDon_Click(object sender, EventArgs e)
        {
            printPreviewDialog.Document = printDocument;
            printPreviewDialog.ShowDialog();
        }
        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            
            Graphics g = e.Graphics;
            Font fontHOADONBANHANG = new Font("Arial", 20, FontStyle.Bold);
            Font fontTitle = new Font("Arial", 16, FontStyle.Bold);
            Font fontHeader = new Font("Arial", 12, FontStyle.Bold);
            Font fontContent = new Font("Arial", 12);
            Font fontXuatBill = new Font("Arial", 8);

            int pageWidth = e.PageBounds.Width; // Lấy chiều rộng toàn bộ khổ giấy
            float x = 50, y = 50;

            Image logo = Properties.Resources.MediumLogo;
            g.DrawImage(logo, 50, 85, 125, 40);

            // Tiêu đề cửa hàng (căn giữa so với toàn bộ trang)
            string title = "CỬA HÀNG VÀNG BẠC ĐÁ QUÝ TEENSTYLE";
            float titleX = (pageWidth - g.MeasureString(title, fontTitle).Width) / 2;
            g.DrawString(title, fontTitle, Brushes.Black, titleX, y);
            y += 35;

            string diachi = "Địa chỉ: 32A Xuân Thủy, phường Thảo Điền, Thành phố Thủ Đức, TP.HCM";
            float diachiX = (pageWidth - g.MeasureString(diachi, fontXuatBill).Width) / 2;
            g.DrawString(diachi, fontXuatBill, Brushes.Black, diachiX, y);
            y += 30;

            // Tiêu đề hóa đơn (căn giữa)
            string header = "HÓA ĐƠN BÁN HÀNG";
            float headerX = (pageWidth - g.MeasureString(header, fontHOADONBANHANG).Width) / 2;
            g.DrawString(header, fontHOADONBANHANG, Brushes.Black, headerX, y);
            y += 40;
            string gioXuat = "Thời gian xuất hóa đơn: " + DateTime.Now;
            float alaignX = (pageWidth - g.MeasureString(gioXuat, fontXuatBill).Width) / 2;
            g.DrawString(gioXuat, fontXuatBill, Brushes.Black, alaignX, y);
            y += 50;

            string line ="Hóa đơn: #" + txtMaHD.Text;
            g.DrawString(line, fontContent, Brushes.Black, x, y);
            y += 25;
            string lineCustomer = "Khách hàng: " + txtKhachHang.Text;
            g.DrawString(lineCustomer, fontContent, Brushes.Black, x, y);
            y += 25;
            string lineNV = "Nhân viên: Thu ngân 1";
            g.DrawString(lineNV, fontContent, Brushes.Black, x, y);
            y += 25;
           
            y += 10;


            //Tiêu đề bảng
            g.DrawString("Danh sách sản phẩm:", fontHeader, Brushes.Black, x, y);
            y += 30;
            // Bảng danh sách sản phẩm

            // Định dạng căn phải
            StringFormat formatRight = new StringFormat();
            formatRight.Alignment = StringAlignment.Far;

            // Lề trang
            float margin = 50;
            float tableWidth = pageWidth - (margin * 2); // Bảng chiếm hết chiều rộng trừ lề
            float tableX = margin; // Vị trí bắt đầu của bảng
            float rowHeight = 30;
            float tableY = y + 20; // Vị trí bảng dưới phần tiêu đề

            // Định nghĩa vị trí các cột dựa trên tỷ lệ bảng
            float col1 = tableX;
            float col2 = tableX + (tableWidth * 0.4f);
            float col3 = tableX + (tableWidth * 0.6f);
            float col4 = tableX + (tableWidth * 0.8f);

            // Vẽ tiêu đề bảng
            g.DrawRectangle(Pens.Black, tableX, tableY, tableWidth, rowHeight);
            g.DrawString("Sản phẩm", fontHeader, Brushes.Black, col1 + 10, tableY + 5);
            g.DrawString("Số lượng", fontHeader, Brushes.Black, col2 + 10, tableY + 5);
            g.DrawString("Đơn giá", fontHeader, Brushes.Black, col3 + 10, tableY + 5);
            g.DrawString("Thành tiền", fontHeader, Brushes.Black, col4 + 10, tableY + 5);
            tableY += rowHeight;

            //Độ rộng cột số lượng
            float soLuongWidth = g.MeasureString("Số luọng", fontHeader).Width;

            // Vẽ dữ liệu sản phẩm
            decimal tongTien = 0;
            int tongSoLuong = 0;
            foreach (DataGridViewRow row in dgvChiTiet.Rows)
            {
                    string tenSP = row.Cells["ProductName"].Value.ToString();
                    int soLuong = Convert.ToInt32(row.Cells["Quantity"].Value);
                    tongSoLuong += soLuong;
                    decimal donGia = Convert.ToDecimal(row.Cells["Price"].Value);
                    decimal thanhTien = soLuong * donGia;

                    tongTien += thanhTien;

                    g.DrawRectangle(Pens.Black, tableX, tableY, tableWidth, rowHeight);
                    g.DrawString(tenSP, fontContent, Brushes.Black, col1 + 10, tableY + 5);
                    g.DrawString(soLuong.ToString(), fontContent, Brushes.Black, col2 + (soLuongWidth / 2) + 50 - g.MeasureString(soLuong.ToString(), fontContent).Width, tableY + 5);
                    g.DrawString(donGia.ToString("N0"), fontContent, Brushes.Black, col3 + 10, tableY + 5);
                    g.DrawString(thanhTien.ToString("N0"), fontContent, Brushes.Black, col4 + 10, tableY + 5);
                    tableY += rowHeight;

            
            }

            y = tableY + 20;
            string totalSL = $"Tổng số lượng: {tongSoLuong}";
            g.DrawString(totalSL, fontHeader, Brushes.Black, col1, y);


            y = tableY + 20;

            // Tính tiền trước thuế từ tổng tiền đã giảm

            decimal thanhTienIN = decimal.Parse(lblTamTinh.Text.Replace("VNĐ", "").Replace(",", "").Trim());
            decimal giamGia = decimal.Parse(lblGiamGia.Text.Replace("VNĐ", "").Replace(",", "").Trim());
            decimal tongTienIn = decimal.Parse(lblTotal.Text.Replace("VNĐ", "").Replace(",", "").Trim());

            decimal truocThue = tongTienIn / 1.1m;
            decimal tinhThue = truocThue * 0.1m;      // Thuế 10%

           
            // THÀNH TIỀN
            string thanhTienText = "Thành tiền: ";
            g.DrawString(thanhTienText, fontHeader, Brushes.Black, col3 + 10, y);
            string thanhTienNUM = $"{thanhTienIN:N0} VNĐ";
            g.DrawString(thanhTienNUM, fontHeader, Brushes.Black, new RectangleF(tableX, y, tableWidth, rowHeight), formatRight);
            y += 30;
            // GIẢM GIÁ
            string giamGiaStr = "Giảm giá: ";
            g.DrawString(giamGiaStr, fontHeader, Brushes.Black, col3 + 10, y);
            string giamGiaNUM = $"{giamGia:N0} VNĐ";
            g.DrawString(giamGiaNUM, fontHeader, Brushes.Black, new RectangleF(tableX, y, tableWidth, rowHeight), formatRight);
            y += 30;

            // GIÁ TRƯỚC THUẾ
            string giaTruocThue = "Giá trước thuế: ";
            g.DrawString(giaTruocThue, fontHeader, Brushes.Black, col3 + 10, y);
            string giaTruocThueNUM = $"{truocThue:N0} VNĐ";
            g.DrawString(giaTruocThueNUM, fontHeader, Brushes.Black, new RectangleF(tableX, y, tableWidth, rowHeight), formatRight);
            y += 30;

            // THUẾ
            string thue = "Thuế VAT (10%): ";
            g.DrawString(thue, fontHeader, Brushes.Black, col3 + 10, y);
            string thueNUM = $"{tinhThue:N0} VNĐ";
            g.DrawString(thueNUM, fontHeader, Brushes.Black, new RectangleF(tableX, y, tableWidth, rowHeight), formatRight);
            y += 30;

            
            // TỔNG TIỀN THANH TOÁN
            string totalString = "Tổng tiền: ";
            g.DrawString(totalString, fontHeader, Brushes.Black, col3 + 10, y);
            string totalNUM = $"{tongTienIn:N0} VNĐ";
            g.DrawString(totalNUM, fontHeader, Brushes.Black, new RectangleF(tableX, y, tableWidth, rowHeight), formatRight);
            y += 30;

            
            Font fontSoTienBangChu = new Font("Arial", 12, FontStyle.Italic);
            string soTienBangChu = UIHelper.NumberToVietnameseText(Convert.ToDecimal(tongTienIn));
            g.DrawString(soTienBangChu, fontSoTienBangChu, Brushes.Black, col1, y);
            y += 30;
            // Font nhỏ hơn để hiển thị ghi chú
            Font fontNote = new Font("Arial", 10, FontStyle.Italic);

            // Ghi chú và lời cảm ơn (căn giữa)
            y += 40;
            string note1 = "Chất lượng đi đôi với giá tiền";
            float note1X = (pageWidth - g.MeasureString(note1, fontNote).Width) / 2;
            g.DrawString(note1, fontNote, Brushes.Black, note1X, y);

            y += 20;
            string note2 = "Cam kết bảo hành 1 đổi 1 trong vòng 6 tháng nếu có hư hỏng do cửa hàng";
            float note2X = (pageWidth - g.MeasureString(note2, fontNote).Width) / 2;
            g.DrawString(note2, fontNote, Brushes.Black, note2X, y);

            y += 20;
            string note3 = "TeeNStyle chân thành cảm ơn Quý khách!";
            float note3X = (pageWidth - g.MeasureString(note3, fontNote).Width) / 2;
            g.DrawString(note3, fontNote, Brushes.Black, note3X, y);

            y += 30;


            // Mã QR (hiển thị bằng text)
            y += 30;
            // Mã QR và thông tin tài khoản
            float qrSize = 200;
            float qrX = x;
            float textX = qrX + qrSize + 20;
            float qrY = y;

            // Dữ liệu VietQR tối giản (cần theo chuẩn nếu dùng thực tế)
            string bankAcc = "1040489156";
            string bankCode = "VCB"; // Mã ngân hàng
            string accName = "NGUYEN NHAT THIEN";
            string content = "THANHTOANHOADONTEENSTYLE";
            string qrContent = $"https://img.vietqr.io/image/{bankCode}-{bankAcc}-qr_only.png?amount={tongTienIn}&addInfo={content}&accountName={Uri.EscapeDataString(accName)}";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(qrContent);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                {
                    Image qrImage = Image.FromStream(stream);
                    g.DrawImage(qrImage, qrX, qrY, qrSize, qrSize);
                }
            }
            catch (Exception ex)
            {
                g.DrawString("[QR không khả dụng]", fontContent, Brushes.Black, qrX, qrY);
            }

            string qrInfo = "STK: 1040489156\nTK: NGUYEN NHAT THIEN\nAlias: VANGBACTEENSTYLE21\nNgân hàng: Vietcombank";
            g.DrawString(qrInfo, fontContent, Brushes.Black, textX, qrY + 75);



        }
       
    }
}
