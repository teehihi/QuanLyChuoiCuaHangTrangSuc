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
            if (txtUsername.Text == "" && txtPassword.Text == "")
            {
                UIHelper.SwitchForm(this, new frmMenu());
                
            }
            else
            {
                lblWrongAccount.Visible = true;
                txtPassword.Focus();
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

        private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnSignIn.PerformClick();
            }
        }
    }
}
