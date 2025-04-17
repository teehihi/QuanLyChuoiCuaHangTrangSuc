using BusinessAccessLayer;
using Guna.UI2.WinForms;
using QuanLyChuoiCuaHangTrangSuc.MainForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyChuoiCuaHangTrangSuc
{
    public partial class frmSupplier : Form
    {
        DBSupplier dbSupplier = new DBSupplier();
        bool isAddingSupplier = false;
        
        private Guna2Panel selectedPanel = null;
        public frmSupplier()
        {
            InitializeComponent();
            this.Text = "Quản lý nhà cung cấp - TeeNStyle";
        }

        private void frmSupplier_Load(object sender, EventArgs e)
        {
            LoadSuppliers();
        }

        private void LoadSuppliers()
        {
            DataTable dt = dbSupplier.LayNhaCungCap().Tables[0];
            flpSupplier.Controls.Clear();

            foreach (DataRow row in dt.Rows)
            {
                int supplierId = Convert.ToInt32(row["SupplierID"]);
                string supplierName = row["Name"].ToString();
                string supplierAddress = row["Address"].ToString();
                string supplierPhone = row["Phone"].ToString();

                // Panel chứa ảnh và tên
                Guna2Panel supplierPanel = new Guna2Panel();
                supplierPanel.Size = new Size(150, 200);
                supplierPanel.BorderRadius = 10;
                supplierPanel.FillColor = Color.FromArgb(20, 35, 60);
                supplierPanel.Margin = new Padding(10);
                supplierPanel.Cursor = Cursors.Hand;

                // Ảnh nhà cung cấp
                Guna2PictureBox pictureBox = new Guna2PictureBox();
                pictureBox.Size = new Size(120, 100);
                pictureBox.Location = new Point(15, 10);
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox.UseTransparentBackground = true;
                pictureBox.BorderRadius = 10;

                // Load ảnh từ resource
                Image image = null;
                image = Properties.Resources.picSupplier;
                
                pictureBox.Image = image;

                // Gán thông tin vào Tag (bao gồm cả ảnh)
                supplierPanel.Tag = new
                {
                    SupplierID = supplierId,
                    Name = supplierName,
                    Address = supplierAddress,
                    Phone = supplierPhone,
                    Image = image
                };

                // Tên nhà cung cấp - wrap text
                Guna2HtmlLabel label = new Guna2HtmlLabel();
                label.Text = supplierName;
                label.ForeColor = Color.White;
                label.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                label.Location = new Point(15, 120);
                label.MaximumSize = new Size(120, 40);
                label.AutoSize = true;
                label.Margin = new Padding(10);
                label.TextAlignment = ContentAlignment.MiddleCenter;
                label.BackColor = Color.Transparent;
                

                // Thêm control con
                supplierPanel.Controls.Add(pictureBox);
                supplierPanel.Controls.Add(label);

                // Gán sự kiện click
                supplierPanel.Click += SupplierPanel_Click;

                // Click ảnh hoặc tên cũng như click panel
                pictureBox.Click += (s, e) => SupplierPanel_Click(supplierPanel, e);
                label.Click += (s, e) => SupplierPanel_Click(supplierPanel, e);

                // Thêm vào flow layout
                flpSupplier.Controls.Add(supplierPanel);
            }




        }




        private void SupplierPanel_Click(object sender, EventArgs e)
        {
            var panel = sender as Guna2Panel;
            if (panel == null || panel.Tag == null) return;

            // Nếu đang chọn lại chính panel đó -> hủy chọn
            if (selectedPanel == panel)
            {
                ClearPanelSelection(panel);
                selectedPanel = null;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnThem.Enabled = true;
                return;
            }

            // Nếu có panel cũ đang chọn -> xóa viền và nút check
            if (selectedPanel != null)
            {
                ClearPanelSelection(selectedPanel);
            }

            // Gán panel mới là panel đang được chọn
            selectedPanel = panel;

            // Hiển thị dữ liệu
            dynamic data = panel.Tag;
            txtSupplierID.Text = data.SupplierID.ToString();
            txtSupplierName.Text = data.Name;
            txtSupplierAddress.Text = data.Address;
            txtSupplierPhone.Text = data.Phone;
            txtSupplierName.Focus();

            // Cập nhật trạng thái nút
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            
            // Hiển thị viền chọn
            panel.BorderColor = Color.LimeGreen;
            panel.BorderThickness = 3;

            // Tìm hoặc tạo nút check
            Guna2Button btnCheck = panel.Controls.OfType<Guna2Button>().FirstOrDefault(b => b.Name == "btnCheck");
            if (btnCheck == null)
            {
                btnCheck = new Guna2Button
                {
                    Name = "btnCheck",
                    Size = new Size(30, 30),
                    Image = Properties.Resources.iconCheck,
                    ImageSize = new Size(16, 16),
                    BorderRadius = 6,
                    FillColor = Color.LimeGreen,
                    HoverState = { FillColor = Color.LimeGreen },
                    PressedColor = Color.LimeGreen,
                    BorderThickness = 0,
                    BackColor = Color.Transparent,
                    Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                    Location = new Point(120, 170),
                    Visible = true
                };
                panel.Controls.Add(btnCheck);
                btnCheck.BringToFront();
            }
            else
            {
                btnCheck.Visible = true;
            }
        }

        private void ClearPanelSelection(Guna2Panel panel)
        {
            panel.BorderThickness = 0;

            var btnCheck = panel.Controls.OfType<Guna2Button>().FirstOrDefault(b => b.Name == "btnCheck");
            if (btnCheck != null)
            {
                btnCheck.Visible = false;
            }
        }


        private void btnThem_Click(object sender, EventArgs e)
        {
            ClearSupplierFields();
            EnableSupplierInput();

            // Bỏ chọn panel nếu có
            if (selectedPanel != null)
            {
                ClearPanelSelection(selectedPanel);
                selectedPanel = null;
            }
            isAddingSupplier = true;
            panelLuuHuy.Visible = true;
            panelThemSuaXoa.Visible = false;
            txtSupplierID.Focus();
        }
        
        private void btnSua_Click(object sender, EventArgs e)
        {

            EnableSupplierInput();
            isAddingSupplier = false;
            panelLuuHuy.Visible = true;
            panelThemSuaXoa.Visible = false;
            txtSupplierName.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            int supplierID;
            
            string name = txtSupplierName.Text.Trim();
            string phone = txtSupplierPhone.Text.Trim();
            string address = txtSupplierAddress.Text.Trim();
            

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Vui lòng nhập tên nhà cung cấp.", "Thông báo");
                return;
            }



            if (isAddingSupplier)
            {
                if (dbSupplier.ThemNhaCungCap(name, address, phone))
                {
                    MessageBox.Show("Thêm nhà cung cấp thành công.");
                }
                else
                {
                    MessageBox.Show("Thêm nhà cung cấp thất bại.");
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(txtSupplierID.Text))
                {
                    supplierID = Convert.ToInt32(txtSupplierID.Text);
                    if (dbSupplier.SuaNhaCungCap(supplierID, name, address, phone))
                    {
                        MessageBox.Show("Sửa nhà cung cấp thành công.");
                    }
                    else
                    {
                        MessageBox.Show("Sửa nhà cung cấp thất bại.");
                    }
                }

                
            }

            LoadSuppliers();
            DisableSupplierInput();
            ClearSupplierFields();
            panelLuuHuy.Visible = false;
            panelThemSuaXoa.Visible = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }


        private void btnHuy_Click(object sender, EventArgs e)
        {
           
            LoadSuppliers();
            ClearSupplierFields();
            DisableSupplierInput();
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            panelThemSuaXoa.Visible = true;
            panelLuuHuy.Visible = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            
            if (!int.TryParse(txtSupplierID.Text, out int id))
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp cần xóa.");
                return;
            }

            DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa nhà cung cấp {txtSupplierName.Text}?", "Xác nhận", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                if (dbSupplier.XoaNhaCungCap(id))
                {
                    MessageBox.Show("Xóa thành công.");
                    LoadSuppliers();
                    ClearSupplierFields();
                    // Bỏ chọn panel nếu có
                    if (selectedPanel != null)
                    {
                        ClearPanelSelection(selectedPanel);
                        selectedPanel = null;
                    }
                    btnSua.Enabled = false;
                    btnXoa.Enabled = false;
                    LoadSuppliers();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại.");
                }
            }
        }

        private void ClearSupplierFields()
        {
            flpSupplier.Enabled = true;
            txtSupplierID.Text = "";
            txtSupplierName.Text = "";
            txtSupplierAddress.Text = "";
            txtSupplierPhone.Text = "";
            

        }

        private void EnableSupplierInput()
        {
            //txtSupplierID.Enabled = true;
            txtSupplierName.Enabled = true;
            txtSupplierAddress.Enabled = true;
            txtSupplierPhone.Enabled = true;

            flpSupplier.Enabled = false;

        }

        private void DisableSupplierInput()
        {
            txtSupplierID.Enabled = false;
            txtSupplierName.Enabled = false;
            txtSupplierAddress.Enabled = false;
            txtSupplierPhone.Enabled = false;
            flpSupplier.Enabled = true;
        }
    }
}
