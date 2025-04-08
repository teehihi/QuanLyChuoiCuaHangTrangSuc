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
            


        }

        private void frmCustomer_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'stepSampleDataSet.Customer' table. You can move, or remove it, as needed.
           
            LoadData();
            // Gắn sự kiện khi chọn dòng
            dgvCustomer.SelectionChanged += dgvCustomer_SelectionChanged;

            // Nếu có dòng thì chọn dòng đầu tiên để hiện dữ liệu luôn
            if (dgvCustomer.Rows.Count > 0)
                dgvCustomer.Rows[0].Selected = true;
                
                txtCustomerType.Enabled = false;
                dgvCustomer.Font = new Font("Segoe UI", 13); 
                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnLuu.Enabled = false;
                btnHuy.Enabled = false;


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
        private void ClearFields()
        {
            this.txtCustomerID.Text = "";
            this.txtCustomerAddress.Text = "";
            this.txtCustomerName.Text = "";
            this.txtCustomerPhone.Text = "";
            this.txtCustomerType.Text = "";
            
        }
        private void EnableInput()
        {
            this.txtCustomerID.Enabled = true;
            this.txtCustomerAddress.Enabled = true;
            this.txtCustomerName.Enabled = true;
            this.txtCustomerPhone.Enabled = true;
            this.txtCustomerType.Enabled = true;
        }


        private void dgvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblInfor_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void dgvCustomer_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCustomer.CurrentRow != null && dgvCustomer.CurrentRow.Index >= 0)
            {
                txtCustomerID.Text = dgvCustomer.CurrentRow.Cells["CustomerID"].Value?.ToString();
                txtCustomerName.Text = dgvCustomer.CurrentRow.Cells["FullName"].Value?.ToString();
                txtCustomerType.Text = dgvCustomer.CurrentRow.Cells["CustomerType"].Value?.ToString();
                txtCustomerAddress.Text = dgvCustomer.CurrentRow.Cells["Address"].Value?.ToString();
                txtCustomerPhone.Text = dgvCustomer.CurrentRow.Cells["Phone"].Value?.ToString();
                
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            ClearFields();
            EnableInput();
            isAdding = true;
            txtCustomerType.SelectedIndex = 0;
            btnLuu.Enabled = true;
            btnHuy.Enabled = true;


        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            
            if (!int.TryParse(txtCustomerID.Text, out _))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần sửa.", "Thông báo");
                return;
            }

            EnableInput();
           
            isAdding = false;
            btnLuu.Enabled = true;
            btnHuy.Enabled = true;

        }
        private void btnLuu_Click(object sender, EventArgs e)
        {
            string fullName = txtCustomerName.Text.Trim();
            string customerType = txtCustomerType.Text.Trim();
            string address = txtCustomerAddress.Text.Trim();
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
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtCustomerID.Text, out int customerId))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa.", "Thông báo");
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này?", "Xác nhận", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                if (dbCustomer.XoaKhachHang(customerId))
                {
                    MessageBox.Show("Xóa khách hàng thành công.", "Thông báo");
                    LoadData();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Xóa khách hàng thất bại.", "Lỗi");
                }
            }
        }
        private void DisableInput()
        {
            this.txtCustomerID.Enabled = false;
            this.txtCustomerAddress.Enabled = false;
            this.txtCustomerName.Enabled = false;
            this.txtCustomerPhone.Enabled = false;
            this.txtCustomerType.Enabled = false;
        }

      

        private void btnHuy_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void panelThemSuaXoa_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnThem_Click_1(object sender, EventArgs e)
        {


        }

        private void panelRight_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
        }

        private void guna2txtsearch_IconRightClick(object sender, EventArgs e)
        {
            string tuKhoa = guna2txtsearch.Text.Trim();

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

    }
}
