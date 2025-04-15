using BusinessAccessLayer;
using DataAcessLayer;
using Guna.UI2.WinForms;
using QuanLyChuoiCuaHangTrangSuc.MainForm;
using QuanLyChuoiCuaHangTrangSuc.SubForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyChuoiCuaHangTrangSuc
{
    public partial class frmInvoices : Form
    {
        
        DBProduct dbProduct = new DBProduct();

        private Dictionary<Guna2Panel, bool> selectedPanels = new Dictionary<Guna2Panel, bool>();
        private Dictionary<string, DataRow> selectedProducts = new Dictionary<string, DataRow>();
        private double currentDiscountAmount = 0;
        private double currentDiscountRate = 0;
        private int promotionID;
        private byte[] imageData; // Biến lưu ảnh dưới dạng byte[]
        private List<CartItem> cart;

        public frmInvoices()
        {
            InitializeComponent();
            this.Text = "Quản lý hóa đơn - TeeNStyle";
        }


        private void frmInvoices_Load(object sender, EventArgs e)
        {

            
            LoadLoaiTrangSuc();
            cbLoaiSP.SelectedIndex = 0;
            flpCart.Controls.Clear();

        }

        private void LoadProducts(string loai)
        {
            DataTable dt = dbProduct.LayTrangSucTheoLoai(loai).Tables[0];


            flpProduct.Controls.Clear(); // Xóa sản phẩm cũ
            flpProduct.AutoScroll = true;
            foreach (DataRow row in dt.Rows)
            {
                // Tạo panel chứa sản phẩm
                Guna2Panel productPanel = new Guna2Panel();
                productPanel.Size = new Size(160, 200);
                productPanel.BorderRadius = 15;
                productPanel.FillColor = Color.FromArgb(10, 25, 47);
                productPanel.Tag = row; // Lưu DataRow vào Tag
                productPanel.Cursor = Cursors.Hand;

                // Hiển thị ảnh sản phẩm
                Guna2PictureBox pictureBox = new Guna2PictureBox();
                pictureBox.Size = new Size(140, 110);
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox.BorderRadius = 15;

                // Kiểm tra nếu ảnh NULL
                if (row["ProdImage"] != DBNull.Value)
                {
                    byte[] imgBytes = (byte[])row["ProdImage"];
                    using (MemoryStream ms = new MemoryStream(imgBytes))
                    {
                        pictureBox.Image = Image.FromStream(ms);
                    }
                    //pictureBox.Image = ByteArrayToImage((byte[])row["ProdImage"]);
                }
                else
                {
                    pictureBox.Image = Properties.Resources.picProduct1; // Ảnh mặc định
                }


                // Hiển thị tên sản phẩm
                Label lblName = new Label();
                lblName.Text = row["Name"].ToString();
                lblName.AutoSize = false;
                lblName.ForeColor = Color.White;
                lblName.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                lblName.TextAlign = ContentAlignment.MiddleCenter;
                lblName.BackColor = Color.Transparent;
                lblName.Size = new Size(140, 40);

                // Hiển thị giá sản phẩm
                Label lblPrice = new Label();
                lblPrice.Text = string.Format("{0:N0} VNĐ", Convert.ToDecimal(row["Price"]));
                lblPrice.ForeColor = Color.Orange;
                lblPrice.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                lblPrice.TextAlign = ContentAlignment.MiddleCenter;
                lblPrice.BackColor = Color.Transparent;
                lblPrice.Size = new Size(140, 20);


                // Set vị trí
                pictureBox.Location = new Point(10, 10);
                lblName.Location = new Point(10, 120);
                lblPrice.Location = new Point(10, 165);

                // Thêm vào panel
                productPanel.Controls.Add(pictureBox);
                productPanel.Controls.Add(lblName);
                productPanel.Controls.Add(lblPrice);

                //THêm hover
                UIHelper.AddHoverEffect(productPanel, Color.FromArgb(30, 50, 80), Color.FromArgb(10, 25, 47));


                // Thêm panel vào FlowLayoutPanel
                flpProduct.Controls.Add(productPanel);

                // Gán sự kiện Click
                productPanel.Click += Panel_Click;

                // Gán sự kiện Click cho tất cả control con bên trong panel
                foreach (Control control in productPanel.Controls)
                {
                    control.Click += Panel_Click;
                }

                foreach (Control control in flpProduct.Controls)
                {
                    if (control is Guna2Panel panel && panel.Tag is DataRow row1)
                    {
                        string productId = row1["ProductID"].ToString();
                        if (selectedProducts.ContainsKey(productId))
                        {
                            // Áp lại hiệu ứng viền và dấu tick
                            panel.BorderColor = Color.LimeGreen;
                            panel.BorderThickness = 3;

                            Guna2Button btnCheck = panel.Controls.OfType<Guna2Button>().FirstOrDefault(b => b.Name == "btnCheck");
                            if (btnCheck == null)
                            {
                                btnCheck = new Guna2Button();
                                btnCheck.Size = new Size(30, 30);
                                btnCheck.Image = Properties.Resources.iconCheck;
                                btnCheck.ImageSize = new Size(16, 16);
                                btnCheck.BorderRadius = 6;
                                btnCheck.FillColor = Color.LimeGreen;
                                btnCheck.BackColor = Color.Transparent;
                                btnCheck.Location = new Point(145, 185);
                                btnCheck.Name = "btnCheck";
                                panel.Controls.Add(btnCheck);
                                btnCheck.BringToFront();
                            }
                            btnCheck.Visible = true;

                            selectedPanels[panel] = true;
                        }
                    }
                }

            }
        }

        private void LoadLoaiTrangSuc()
        {
            // Ban đầu hiển thị tất cả
            LoadProducts("");
            cbLoaiSP.SelectedIndexChanged += (s,e) =>
            {
                string selectedGroup = cbLoaiSP.SelectedItem?.ToString();
                if (selectedGroup == "Tất cả")
                {
                    LoadProducts("");
                }
                else
                {
                    LoadProducts(selectedGroup);
                }
                    
            };

            
        }


        private void Panel_Click(object sender, EventArgs e)
        {
            Control control = (Control)sender;

            // Tìm panel cha (nếu sender không phải Guna2Panel)
            while (control != null && !(control is Guna.UI2.WinForms.Guna2Panel))
            {
                control = control.Parent;
            }

            // Nếu không tìm thấy panel, thoát
            if (control == null)
            {
                MessageBox.Show("Lỗi: Không tìm thấy panel chứa sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Guna.UI2.WinForms.Guna2Panel panel = (Guna.UI2.WinForms.Guna2Panel)control;

            // Kiểm tra panel.Tag có dữ liệu không
            if (!(panel.Tag is DataRow row))
            {
                MessageBox.Show("Lỗi: Không thể lấy thông tin sản phẩm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            // Kiểm tra trạng thái đã chọn hay chưa
            bool isSelected = selectedPanels.ContainsKey(panel) && selectedPanels[panel];

            if (!isSelected)
            {
                // Chọn: Hiện viền và ảnh check
                panel.BorderColor = Color.LimeGreen;
                panel.BorderThickness = 3;

                // Tìm hoặc tạo button check
                Guna2Button btnCheck = panel.Controls.OfType<Guna2Button>().FirstOrDefault(b => b.Name == "btnCheck");
                

                if (btnCheck == null)
                {
                    btnCheck = new Guna2Button();
                    btnCheck.Size = new Size(30, 30);
                    btnCheck.Image = Properties.Resources.iconCheck;
                    btnCheck.ImageSize = new Size(16, 16);
                    btnCheck.BorderRadius = 6;
                    btnCheck.FillColor = Color.LimeGreen;
                    btnCheck.HoverState.FillColor = Color.LimeGreen;
                    btnCheck.PressedColor = Color.LimeGreen;
                    btnCheck.BorderThickness = 0;
                    btnCheck.BackColor = Color.Transparent;
                    btnCheck.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                    btnCheck.Location = new Point(145, 185);
                    btnCheck.Visible = false;
                    btnCheck.Name = "btnCheck";
                    panel.Controls.Add(btnCheck);
                    btnCheck.BringToFront();
                }

                btnCheck.Visible = true;

                selectedPanels[panel] = true;
                AddToCart(row);
            }
            else
            {

                // Bỏ chọn: ẩn viền và ảnh check
                panel.BorderThickness = 0;

                var btnCheck = panel.Controls.OfType<Guna2Button>().FirstOrDefault();
                if (btnCheck != null)
                    btnCheck.Visible = false;

                selectedPanels[panel] = false;
                RemoveFromCart(row["ProductID"].ToString());
                selectedProducts.Remove(row["ProductID"].ToString());
                
                CapNhatTamTinh();
                UpdateTotals();

            }



            // Gán dữ liệu vào panel Cart
            string productId = row["ProductID"].ToString(); // Hoặc tên cột phù hợp trong DB

            if (!selectedProducts.ContainsKey(productId))
            {
                selectedProducts[productId] = row;
                
            }
            

        }

        private void RemoveFromCart(string productId)
        {
            foreach (Control cartItem in flpCart.Controls)
            {
                if (cartItem.Tag is DataRow cartRow && cartRow["ProductID"].ToString() == productId)
                {
                    flpCart.Controls.Remove(cartItem);
                    cartItem.Dispose();

                    break;
                }
            }
            if (cart != null)
            {
                cart.Clear();
            }

        }




        private void AddToCart(DataRow row)
        {
            Guna2Panel panelCart = new Guna2Panel();
            panelCart.Size = new Size(430, 70);
            panelCart.BackColor = Color.Transparent;
            panelCart.FillColor = Color.FromArgb(15, 30, 60);
            panelCart.BorderRadius = 10;
            panelCart.Tag = row;

            // Tên sản phẩm
            Label lblName = new Label();
            lblName.Text = row["Name"].ToString();
            lblName.ForeColor = Color.White;
            lblName.BackColor = Color.Transparent;
            lblName.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblName.Location = new Point(10, 10);
            lblName.Size = new Size(220, 30);

            // Giá sản phẩm
            Label lblPrice = new Label();
            lblPrice.Text = string.Format("{0:N0} VNĐ", Convert.ToDecimal(row["Price"]));
            lblPrice.ForeColor = Color.Orange;
            lblPrice.BackColor = Color.Transparent;
            lblPrice.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblPrice.Location = new Point(10, 40);
            lblPrice.Size = new Size(220, 30);

            // Số lượng
            Guna2NumericUpDown nudQuantity = new Guna2NumericUpDown();
            nudQuantity.Value = 1;
            nudQuantity.Minimum = 1;
            nudQuantity.Maximum = 100;
            nudQuantity.Size = new Size(60, 30);
            nudQuantity.Location = new Point(300, 20);
            nudQuantity.BorderRadius = 7;
            nudQuantity.BackColor = Color.Transparent;
            nudQuantity.FillColor = Color.White;
            nudQuantity.BorderColor = Color.Gold;
            nudQuantity.UpDownButtonFillColor = Color.Gold;
            nudQuantity.ValueChanged += (s, e) =>
            {
                CapNhatTamTinh();
                UpdateTotals();
            };


            nudQuantity.Tag = row;

            // Nút xóa
            Guna2Button btnDelete = new Guna2Button();
            btnDelete.Text = "";
            btnDelete.Image = Properties.Resources.icnTrash; 
            btnDelete.ImageAlign = HorizontalAlignment.Center;
            btnDelete.ImageSize = new Size(20, 20);
            btnDelete.Size = new Size(30, 30);
            btnDelete.Location = new Point(380, 20);
            btnDelete.Anchor = AnchorStyles.Right;
            btnDelete.BackColor = Color.Transparent;
            btnDelete.FillColor = Color.Transparent;
            btnDelete.HoverState.FillColor = Color.Transparent;
            btnDelete.Cursor = Cursors.Hand;
            btnDelete.Animated = true;
            btnDelete.Click += (s, e) =>
            {
                flpCart.Controls.Remove(panelCart);
                string pid = row["ProductID"].ToString();
                selectedProducts.Remove(pid);
                // Gỡ hiệu ứng chọn ở panel sản phẩm
                var selectedPanel = selectedPanels.Keys.FirstOrDefault(p => ((DataRow)p.Tag)["ProductID"].ToString() == pid);
                if (selectedPanel != null)
                {
                    selectedPanel.BorderThickness = 0;
                    var btnCheck = selectedPanel.Controls.OfType<Guna2Button>().FirstOrDefault();
                    if (btnCheck != null) btnCheck.Visible = false;
                    selectedPanels[selectedPanel] = false;
                }
                CapNhatTamTinh();
                UpdateTotals();

            };

            // Thêm control vào panelCart
            panelCart.Controls.Add(lblName);
            panelCart.Controls.Add(lblPrice);
            panelCart.Controls.Add(nudQuantity);
            panelCart.Controls.Add(btnDelete);

            // Thêm vào flpCart
            flpCart.Controls.Add(panelCart);
            CapNhatTamTinh();
            UpdateTotals();

        }


        private void CapNhatTamTinh()
        {
            decimal tongTien = 0;

            foreach (Guna2Panel panel in flpCart.Controls.OfType<Guna2Panel>())
            {
                var row = panel.Tag as DataRow;
                if (row != null)
                {
                    decimal gia = Convert.ToDecimal(row["Price"]);
                    var nud = panel.Controls.OfType<Guna2NumericUpDown>().FirstOrDefault();
                    if (nud != null)
                    {
                        tongTien += gia * nud.Value;
                    }
                }
            }

            lblTamTinh.Text = tongTien.ToString("N0") + " VNĐ";
        }


        private void UpdateTotals()
        {
            double tamTinh = 0;

            foreach (Control c in flpCart.Controls)
            {
                if (c is Guna2Panel panel && panel.Tag is DataRow row)
                {
                    decimal price = Convert.ToDecimal(row["Price"]);
                    var nud = panel.Controls.OfType<Guna2NumericUpDown>().FirstOrDefault();
                    if (nud != null)
                    {
                        int quantity = (int)nud.Value;
                        tamTinh += (double)(price * quantity);
                    }
                }
            }

            lblTamTinh.Text = tamTinh.ToString("N0") + " VNĐ";
            currentDiscountAmount = tamTinh * currentDiscountRate;

            lblGiamGia.Text = "-" + currentDiscountAmount.ToString("N0") + " VNĐ";

            double tongTien = tamTinh - currentDiscountAmount;
            if (tongTien < 0) tongTien = 0;
            lblTotal.Text = tongTien.ToString("N0") + " VNĐ";
        }



        //Chuyển đổi ảnh thành chuỗi để lưu vào CSDL
        private byte[] ImageToByteArray(Image img)
        {
            if (img == null) return null;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }

        private void btnSelectPromotion_Click(object sender, EventArgs e)
        {
            frmSelectPromotion frm = new frmSelectPromotion();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                currentDiscountRate = frm.DiscountRate ?? 0;
                txtTenKM.Text = frm.PromotionName;
                promotionID = frm.PromotionID;
                UpdateTotals();
            }
        }


        public List<CartItem> GetCartItems()
        {
            List<CartItem> cartItems = new List<CartItem>();

            foreach (var entry in selectedProducts)
            {
                DataRow row = entry.Value;
                string productId = row["ProductID"].ToString();
                int quantity = 1;

                // Tìm panel trong flpCart
                bool foundInCart = false;
                foreach (Control c in flpCart.Controls)
                {
                    if (c is Guna2Panel panel && panel.Tag is DataRow panelRow && panelRow["ProductID"].ToString() == productId)
                    {
                        var nud = panel.Controls.OfType<Guna2NumericUpDown>().FirstOrDefault();
                        if (nud != null)
                        {
                            quantity = (int)nud.Value;
                        }
                        foundInCart = true;
                        break;
                    }
                }

                if (!foundInCart)
                    continue;

                cartItems.Add(new CartItem
                {
                    ProductID = productId,
                    Name = row["Name"].ToString(),
                    Price = Convert.ToDecimal(row["Price"]),
                    Quantity = quantity
                });
            }

            return cartItems;
        }


        //Xóa giỏ hàng
        public void ClearCart()
        {
            // Xóa tất cả sản phẩm trong flpCart
            foreach (Control control in flpCart.Controls)
            {
                control.Dispose();
            }
            flpCart.Controls.Clear();

            // Xóa trạng thái đã chọn
            foreach (var panel in selectedPanels.Keys.ToList())
            {
                if (selectedPanels[panel])
                {
                    // Bỏ viền và ẩn nút check
                    panel.BorderThickness = 0;
                    var btnCheck = panel.Controls.OfType<Guna2Button>().FirstOrDefault(b => b.Name == "btnCheck");
                    if (btnCheck != null)
                        btnCheck.Visible = false;

                    selectedPanels[panel] = false;
                }
            }

            if (cart != null)
            {
                cart.Clear();
            }

            // Xóa danh sách sản phẩm đã chọn
            selectedProducts.Clear();

            // Reset các label liên quan tiền tệ
            lblTamTinh.Text = "0 VNĐ";
            lblGiamGia.Text = "-0 VNĐ";
            lblTotal.Text = "0 VNĐ";

            // Xóa khuyến mãi đang áp dụng
            currentDiscountRate = 0;
            currentDiscountAmount = 0;
            txtTenKM.Text = "";
        }


        private void btnThem_Click(object sender, EventArgs e)
        {
            List<CartItem> cart = GetCartItems(); // bạn đã có danh sách sản phẩm
            frmChiTietHoaDon frm = new frmChiTietHoaDon(cart,currentDiscountAmount, promotionID);
            frm.ShowDialog();

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa toàn bộ giỏ hàng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ClearCart();
            }       
        }

      
        private void btnHistory_Click(object sender, EventArgs e)
        {
            frmHistory frmHistory = new frmHistory();
            frmHistory.ShowDialog();
        }
    }
}
