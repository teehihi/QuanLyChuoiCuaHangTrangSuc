using BusinessAccessLayer;
using DataAcessLayer;
using Guna.UI2.WinForms;
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
        private byte[] imageData; // Biến lưu ảnh dưới dạng byte[]

        public frmInvoices()
        {
            InitializeComponent();
            this.Text = "Quản lý hóa đơn - TeeNStyle";
        }


        private void frmInvoices_Load(object sender, EventArgs e)
        {

            LoadLoaiTrangSuc();
            cbLoaiSP.SelectedIndex = 0;

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

            // Gán dữ liệu vào panel Cart

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


        //Xóa dữ liệu
        
    }
}
