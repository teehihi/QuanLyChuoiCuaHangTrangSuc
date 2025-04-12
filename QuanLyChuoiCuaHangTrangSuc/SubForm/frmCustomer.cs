using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessAccessLayer;
using Guna.UI2.WinForms;
using QuanLyChuoiCuaHangTrangSuc.MainForm;

namespace QuanLyChuoiCuaHangTrangSuc
{

    public partial class frmCustomer : Form
    {
       
        DBCustomer dbCustomer;
        private bool isAdding = false;
        public frmCustomer()
        {
            InitializeComponent();
            dbCustomer = new DBCustomer();
            panelThemSuaXoa.Visible = true;
            panelLuuHuy.Visible = false;
            cboThanhPho.SelectedIndex = 0; // Chọn thành phố đầu tiên trong danh sách
            dgvCustomer.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12.25F, FontStyle.Bold);


        }

        private void frmCustomer_Load(object sender, EventArgs e)
        {
            
            LoadData();
            cboThanhPho.Enabled = false;
            txtCustomerType.Enabled = false;
            dgvCustomer.Font = new Font("Segoe UI", 13); 
            ClearFields();
            DisableInput();

        }


        private void LoadData()
        {
            DataSet ds = dbCustomer.LayKhachHang();
            if (ds != null && ds.Tables.Count > 0)
            {
               
                dgvCustomer.DataSource = ds.Tables[0];

                dgvCustomer.Columns["CustomerID"].HeaderText = "Mã KH";
                dgvCustomer.Columns["FullName"].HeaderText = "Họ và Tên";
                dgvCustomer.Columns["CustomerType"].HeaderText = "Loại khách hàng";
                dgvCustomer.Columns["Address"].HeaderText = "Địa chỉ";
                dgvCustomer.Columns["Phone"].HeaderText = "Số điện thoại";

            }
        }

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (dgvCustomer.CurrentRow != null && dgvCustomer.CurrentRow.Index >= 0)
            {
                txtCustomerID.Text = dgvCustomer.CurrentRow.Cells["CustomerID"].Value?.ToString();
                txtCustomerName.Text = dgvCustomer.CurrentRow.Cells["FullName"].Value?.ToString();
                txtCustomerType.Text = dgvCustomer.CurrentRow.Cells["CustomerType"].Value?.ToString();

                // Lấy địa chỉ từ CSDL
                string fullAddress = dgvCustomer.CurrentRow.Cells["Address"].Value?.ToString();
                txtCustomerAddress.Text = "";
                cboThanhPho.SelectedIndex = -1;

                if (!string.IsNullOrWhiteSpace(fullAddress))
                {
                    int lastCommaIndex = fullAddress.LastIndexOf(',');
                    if (lastCommaIndex >= 0)
                    {
                        txtCustomerAddress.Text = fullAddress.Substring(0, lastCommaIndex).Trim();
                        cboThanhPho.Text = fullAddress.Substring(lastCommaIndex + 1).Trim();
                    }
                    else
                    {
                        // Không có dấu phẩy, gán toàn bộ vào địa chỉ
                        txtCustomerAddress.Text = fullAddress.Trim();
                    }
                }


                txtCustomerPhone.Text = dgvCustomer.CurrentRow.Cells["Phone"].Value?.ToString();
            }


            btnSua.Enabled = true;
            btnXoa.Enabled = true;
        }

        private void txtSearch_IconRightClick(object sender, EventArgs e)
        {
            string tuKhoa = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(tuKhoa))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DataSet ds = dbCustomer.TimKhachHangTongHop(tuKhoa);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dgvCustomer.DataSource = ds.Tables[0];
                }
                else
                {
                    MessageBox.Show("Không tìm thấy khách hàng phù hợp.", "Kết quả", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtSearch_IconRightClick(sender, e);
                e.SuppressKeyPress = true; // Chặn tiếng beep hoặc hành động mặc định
            }
        }

        private void btnXoaBoLoc_Click(object sender, EventArgs e)
        {
            LoadData();
            txtSearch.Clear();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            ClearFields();
            EnableInput();
            isAdding = true;
            txtCustomerType.SelectedIndex = 0;
            panelLuuHuy.Visible = true;
            panelThemSuaXoa.Visible = false;
            txtCustomerName.Focus();


        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            
            if (!int.TryParse(txtCustomerID.Text, out _))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần sửa.", "Thông báo");
                return;
            }

            EnableInput();
            txtCustomerName.Focus();
            isAdding = false;
            panelLuuHuy.Visible = true;
            panelThemSuaXoa.Visible = false;

        }
        private void btnLuu_Click(object sender, EventArgs e)
        {
            string fullName = txtCustomerName.Text.Trim();
            string customerType = txtCustomerType.Text.Trim();
            //string address = txtCustomerAddress.Text.Trim();
            string address = string.IsNullOrWhiteSpace(txtCustomerAddress.Text)
                ? cboThanhPho.Text
                : txtCustomerAddress.Text.Trim() + ", " + cboThanhPho.Text;

            string phone = txtCustomerPhone.Text.Trim();

            if (txtCustomerType.Text == "Chọn" || string.IsNullOrWhiteSpace(txtCustomerType.Text))
            {
                MessageBox.Show("Vui lòng chọn loại khách hàng (VIP hoặc Regular)!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(fullName))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng.", "Thông báo");
                return;
            }

            if (isAdding)
            {
                if (dbCustomer.ThemKhachHang(fullName, customerType, address, phone))
                {
                    MessageBox.Show("Thêm khách hàng thành công.", "Thông báo");
                }
                else
                {
                    MessageBox.Show("Thêm khách hàng thất bại.", "Lỗi");
                }
            }
            else
            {
                if (!int.TryParse(txtCustomerID.Text, out int customerId))
                {
                    MessageBox.Show("ID không hợp lệ.", "Lỗi");
                    return;
                }

                if (dbCustomer.SuaKhachHang(customerId, fullName, customerType, address, phone))
                {
                    MessageBox.Show("Sửa khách hàng thành công.", "Thông báo");
                }
                else
                {
                    MessageBox.Show("Sửa khách hàng thất bại.", "Lỗi");
                }
            }

            LoadData();
            DisableInput();
            ClearFields();
            panelLuuHuy.Visible = false;
            panelThemSuaXoa.Visible = true;

            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtCustomerID.Text, out int customerId))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa.", "Thông báo");
                return;
            }

            DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa khách hàng {txtCustomerName.Text}?", "Xác nhận", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                if (dbCustomer.XoaKhachHang(customerId))
                {
                    MessageBox.Show("Xóa khách hàng thành công.", "Thông báo");
                    LoadData();
                    ClearFields();

                    btnSua.Enabled = false;
                    btnXoa.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Xóa khách hàng thất bại.", "Lỗi");
                }
            }
        }
        
        private void btnHuy_Click(object sender, EventArgs e)
        {
            LoadData();
            ClearFields();
            DisableInput();
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            panelThemSuaXoa.Visible = true;
            panelLuuHuy.Visible = false;
        }


        private void ClearFields()
        {
            this.txtCustomerID.Text = "";
            this.txtCustomerAddress.Text = "";
            this.txtCustomerName.Text = "";
            this.txtCustomerPhone.Text = "";
            this.txtCustomerType.Text = "";
            this.cboThanhPho.SelectedIndex = 0; // Chọn thành phố đầu tiên trong danh sách
            this.txtCustomerType.SelectedIndex = 0; // Chọn loại khách hàng đầu tiên trong danh sách

        }
        private void EnableInput()
        {
            //this.txtCustomerID.Enabled = true;
            this.txtCustomerAddress.Enabled = true;
            this.txtCustomerName.Enabled = true;
            this.txtCustomerPhone.Enabled = true;
            this.txtCustomerType.Enabled = true;
            this.cboThanhPho.Enabled = true;
        }

        private void DisableInput()
        {
            this.txtCustomerID.Enabled = false;
            this.txtCustomerAddress.Enabled = false;
            this.txtCustomerName.Enabled = false;
            this.txtCustomerPhone.Enabled = false;
            this.txtCustomerType.Enabled = false;
            this.cboThanhPho.Enabled = false;
        }


    }
}
