using QuanLyChuoiCuaHangTrangSuc.MainForm;
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
using DataAcessLayer;

namespace QuanLyChuoiCuaHangTrangSuc
{
    public partial class frmLogin : Form
    {
       
        private bool isPasswordVisible = false;
        public frmLogin()
        {
            InitializeComponent();
        }

        

        private void frmLogin_Load(object sender, EventArgs e)
        {

            txtUsername.Focus();
            // Đảm bảo txtPassword ban đầu là ẩn
            txtPassword.UseSystemPasswordChar = true;
             
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            string connectionString = $"Data Source=TEE\\TEE;Initial Catalog=JwelrySystemDBMSFinal;User ID={username};Password={password}";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open(); // Nếu mở được => đúng tài khoản SQL

                // Gán connection string cho toàn bộ chương trình
                DataAcessLayer.ConnectionHelper.CurrentConnectionString = connectionString;
                DataAcessLayer.ConnectionHelper.CurrentUserName = username; // Lưu tên đăng nhập hiện tại


                // Kiểm tra role bằng IS_MEMBER
                string roleQuery = "SELECT IS_MEMBER('db_owner')";
                using (SqlCommand cmd = new SqlCommand(roleQuery, conn))
                {
                    int isManager = Convert.ToInt32(cmd.ExecuteScalar());
                    DataAcessLayer.ConnectionHelper.IsManager = (isManager == 1);
                }

                // Mở form Menu (màn hình chính)
                UIHelper.SwitchForm(this, new frmMenu());
            }

        }



        //Hàm đóng mở mật khẩu
        private void TogglePasswordVisibility(object sender, EventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;
            txtPassword.UseSystemPasswordChar = !isPasswordVisible;

            // Cập nhật hình ảnh cho nút Toggle Password
            if (isPasswordVisible)
            {
                btnTogglePassword.Image = Properties.Resources.eye_opend;  // Hình ảnh "mở mắt"
            }
            else
            {
                btnTogglePassword.Image = Properties.Resources.eye_closed; // Hình ảnh "đóng mắt"
            }
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            lblWrongAccount.Visible = false;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            lblWrongAccount.Visible = false;
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                btnSignIn.PerformClick();
            }
        }
    }
}
