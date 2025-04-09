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
using DataAcessLayer;
using BusinessAccessLayer;
using Guna.UI2.WinForms;

namespace QuanLyChuoiCuaHangTrangSuc
{
    public partial class frmProduct : Form
    {
        DBProduct dbProduct = new DBProduct();
        private byte[] imageData; // Biến lưu ảnh dưới dạng byte[]

        public frmProduct()
        {
            InitializeComponent();
        }

        private void frmProduct_Load(object sender, EventArgs e)
        {

            if (!ConnectionHelper.IsManager)
            {
                btnThem.Visible = false;
                btnSua.Visible = false;
                btnXoa.Visible = false;
                btnChoosePic.Visible = false;
            }


            pictureBoxProduct.Image = Properties.Resources.picProduct1;
            LoadLoaiTrangSuc();
            panelLuuHuy.Visible = false;
            cbLoaiSP.SelectedIndex = 0;
            cbLoaiSP.Enabled = false;

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

            }
        }

        private void LoadLoaiTrangSuc()
        {
            btnAll.Click += (s, e) => { UIHelper.SetSelectedButton(btnAll); LoadProducts(""); };
            btnRing.Click += (s, e) => { UIHelper.SetSelectedButton(btnRing); LoadProducts("Nhẫn"); };
            btnNecklace.Click += (s, e) => { UIHelper.SetSelectedButton(btnNecklace); LoadProducts("Dây chuyền"); };
            btnEarrings.Click += (s, e) => { UIHelper.SetSelectedButton(btnEarrings); LoadProducts("Bông tai"); };
            btnBracelet.Click += (s, e) => { UIHelper.SetSelectedButton(btnBracelet); LoadProducts("Lắc tay"); };
            btnPendant.Click += (s, e) => { UIHelper.SetSelectedButton(btnPendant); LoadProducts("Mặt dây chuyền"); };

            // Ban đầu hiển thị tất cả
            LoadProducts("");
        }


        private void Panel_Click(object sender, EventArgs e)
        {
            btnSua.Enabled = true;
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

            // Gán dữ liệu vào TextBox
            txtProductID.Text = row["ProductID"].ToString();
            txtProductName.Text = row["Name"].ToString();
            txtMaterial.Text = row["Material"].ToString();
            txtStockQuantity.Text = row["StockQuantity"].ToString();
            txtPrice.Text = string.Format("{0:N0} VNĐ", Convert.ToDecimal(row["Price"]));
            txtWeight.Text = row["Weight"].ToString();
            txtDescription.Text = row["Description"].ToString();

            string groupName = dbProduct.LayGroupNameTuGroupID(Convert.ToInt32(row["GroupID"]));

            // Kiểm tra xem combobox có chứa groupName đó hay không
            if (cbLoaiSP.Items.Contains(groupName))
            {
                cbLoaiSP.SelectedItem = groupName;  // Chọn mục tương ứng trong ComboBox
            }
            else
            {
                cbLoaiSP.SelectedIndex = 0;
            }

            txtBranchId.Text = row["BranchID"].ToString();

            // Kiểm tra và hiển thị ảnh sản phẩm
            if (row["ProdImage"] != DBNull.Value)
            {
                byte[] imgBytes = (byte[])row["ProdImage"];
                using (MemoryStream ms = new MemoryStream(imgBytes))
                {
                    pictureBoxProduct.Image = Image.FromStream(ms);
                }
            }
            else
            {
                pictureBoxProduct.Image = Properties.Resources.picProduct1; // Ảnh mặc định
            }
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




        private void btnChoosePic_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBoxProduct.Image = Image.FromFile(openFileDialog.FileName);
                    imageData = ImageToByteArray(pictureBoxProduct.Image);
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            EnableInput();
            ClearFields();
            flpProduct.Enabled = false;
            panelLuuHuy.Visible = true;
            panelThemSuaXoa.Visible = false;
            txtBranchId.Focus();


        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            EnableInput();
            flpProduct.Enabled = false;
            panelThemSuaXoa.Visible = false;
            panelLuuHuy.Visible = true;
            txtBranchId.Focus();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            ClearFields();
            panelLuuHuy.Enabled = true;
            panelLuuHuy.Visible = false;
            panelThemSuaXoa.Visible = true;
            DisableInput();
            flpProduct.Enabled = true;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {

            panelThemSuaXoa.Visible = true;
            panelLuuHuy.Visible = false;
            flpProduct.Enabled = true;

            string name = txtProductName.Text;
            string material = txtMaterial.Text;
            int stockQuantity = int.TryParse(txtStockQuantity.Text, out int sq) ? sq : 0;
            decimal price = decimal.TryParse(txtPrice.Text.Replace(" VNĐ", "").Replace(",", ""), out decimal p) ? p : 0;
            float weight = float.TryParse(txtWeight.Text, out float w) ? w : 0;
            string description = txtDescription.Text;
            int groupId = dbProduct.LayGroupIDTuGroupName(cbLoaiSP.SelectedItem.ToString());
            int branchId = Convert.ToInt32(txtBranchId.Text);
            byte[] prodImage = ImageToByteArray(pictureBoxProduct.Image);

            if (string.IsNullOrWhiteSpace(txtProductID.Text))
            {
                // Thêm sản phẩm
                if (dbProduct.ThemTrangSuc(name, material, stockQuantity, price, weight, description, groupId, branchId, prodImage))
                {
                    MessageBox.Show("Thêm sản phẩm thành công!");
                    ClearFields();
                    DisableInput();
                    LoadProducts("");
                }
                else
                {
                    MessageBox.Show("Thêm sản phẩm thất bại!");
                }
            }
            else
            {
                // Sửa sản phẩm
                int productId = Convert.ToInt32(txtProductID.Text);
                if (dbProduct.SuaTrangSuc(productId, name, material, stockQuantity, price, weight, description, groupId, branchId, prodImage))
                {
                    MessageBox.Show("Cập nhật sản phẩm thành công!");
                    DisableInput();
                    ClearFields();
                    LoadProducts("");
                }
                else
                {
                    MessageBox.Show("Cập nhật sản phẩm thất bại!");
                }
            }

        }




        private void btnXoa_Click(object sender, EventArgs e)
        {
            DisableInput();
            if (string.IsNullOrWhiteSpace(txtProductID.Text))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm để xóa!");
                return;
            }

            int productId = Convert.ToInt32(txtProductID.Text);

            DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa sản phẩm {txtProductName.Text}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                if (dbProduct.XoaTrangSuc(productId))
                {
                    MessageBox.Show("Xóa sản phẩm thành công!");
                    ClearFields(); // Xóa dữ liệu sau khi xóa sản phẩm
                    LoadProducts("");
                }
                else
                {
                    MessageBox.Show("Xóa sản phẩm thất bại!");
                }
            }

        }


        //Xóa dữ liệu
        private void ClearFields()
        {
            txtProductID.Text = string.Empty;
            txtProductName.Text = "";
            txtMaterial.Text = "";
            txtStockQuantity.Text = "";
            txtPrice.Text = "";
            txtWeight.Text = "";
            txtDescription.Text = "";
            txtBranchId.Text = "";
            cbLoaiSP.SelectedIndex = 0;
            pictureBoxProduct.Image = Properties.Resources.picProduct1; // Ảnh mặc định
        }

        private void EnableInput()
        {
            txtProductName.Enabled = true;
            txtMaterial.Enabled = true;
            txtStockQuantity.Enabled = true;
            txtPrice.Enabled = true;
            txtWeight.Enabled = true;
            txtDescription.Enabled = true;
            cbLoaiSP.Enabled = true;
            txtBranchId.Enabled = true;
            btnChoosePic.Enabled = true;
        }

        private void DisableInput()
        {
            txtProductName.Enabled = false;
            txtMaterial.Enabled = false;
            txtStockQuantity.Enabled = false;
            txtPrice.Enabled = false;
            txtWeight.Enabled = false;
            txtDescription.Enabled = false;
            cbLoaiSP.Enabled = false;
            txtBranchId.Enabled = false;
            btnChoosePic.Enabled = false;
        }


        private void txtPrice_Leave(object sender, EventArgs e)
        {
            txtPrice.Text = string.Format("{0:N0} VNĐ", Convert.ToDecimal(txtPrice.Text));
        }

        private void txtPrice_Enter(object sender, EventArgs e)
        {
            txtPrice.Text = txtPrice.Text.Replace(" VNĐ", "").Replace(",", "");

        }

    }
}

