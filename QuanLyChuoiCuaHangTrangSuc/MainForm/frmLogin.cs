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
using BusinessAccessLayer;
using Google.Apis.Auth;
using System.Net.Http;
using Google.Apis.Auth.OAuth2.Responses;
using System.Net;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Diagnostics;

namespace QuanLyChuoiCuaHangTrangSuc
{
    public partial class frmLogin : Form
    {


        DBLogin dbLogin = new DBLogin();

        private readonly string clientId = "486562133750-4s1po3odkk718ak2bks92q9kgm296eh4.apps.googleusercontent.com";
        private readonly string clientSecret = "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";
        private readonly string redirectUri = "http://localhost:5000/";

        private bool isPasswordVisible = false;
        public frmLogin()
        {

            InitializeComponent();

        }



        private void frmLogin_Load(object sender, EventArgs e)
        {
            this.ShowInTaskbar = true;
            this.ShowIcon = true;
            this.Icon = Properties.Resources.fvicon;


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

            string connectionString = $"Data Source=CT\\SQLEXPRESS02;Initial Catalog=JwelrySystemDBMSFinal;User ID={username};Password={password}";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); // Nếu mở được => đúng tài khoản SQL

                    // Gán connection string cho toàn bộ chương trình
                    DataAcessLayer.ConnectionHelper.CurrentConnectionString = connectionString;
                    DataAcessLayer.ConnectionHelper.CurrentUserName = username;

                    // Kiểm tra role bằng IS_MEMBER
                    string roleQuery = "SELECT IS_MEMBER('db_owner')";
                    using (SqlCommand cmd = new SqlCommand(roleQuery, conn))
                    {
                        int isManager = Convert.ToInt32(cmd.ExecuteScalar());
                        DataAcessLayer.ConnectionHelper.IsManager = (isManager == 1);
                    }

                    // Ẩn label nếu đang hiển thị lỗi
                    lblWrongAccount.Visible = false;

                    // Mở form Menu (màn hình chính)
                    UIHelper.SwitchForm(this, new frmMenu());
                }
            }
            catch (SqlException)
            {
                // Nếu có lỗi mở kết nối => sai tài khoản hoặc mật khẩu
                txtPassword.Clear(); // Xóa mật khẩu
                lblWrongAccount.Visible = true;

                txtPassword.Focus(); // Đặt con trỏ vào ô mật khẩu
            }
        }


        private async void btnGGSign_Click(object sender, EventArgs e)
        {
            try
            {
                string email = await LoginWithGoogle();
                if (email != null)
                {
                    dbLogin.EnsureUserForEmail(email);
                    UIHelper.SwitchForm(this, new frmMenu());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đăng nhập: " + ex.Message);
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



        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSignIn.PerformClick();
                e.SuppressKeyPress = true; // Chặn tiếng beep hoặc hành động mặc định

            }
        }

        public async Task<string> LoginWithGoogle()
        {
            try
            {
                string authUrl = $"https://accounts.google.com/o/oauth2/v2/auth" +
                                 $"?response_type=code" +
                                 $"&scope=email" +
                                 $"&redirect_uri={redirectUri}" +
                                 $"&client_id={clientId}";

                // 1. Mở trình duyệt bằng tiến trình riêng
                var browserProcess = new System.Diagnostics.Process();
                browserProcess.StartInfo.FileName = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
                browserProcess.StartInfo.Arguments = $"--new-window \"{authUrl}\"";
                browserProcess.Start();

                // 🛡️ Listener nội bộ nhận phản hồi từ Google
                var http = new HttpListener();
                http.Prefixes.Add(redirectUri); // Ví dụ: http://localhost:5000/
                http.Start();

                var context = await http.GetContextAsync();
                string code = context.Request.QueryString["code"];

                // Phản hồi đơn giản
                string responseString = @"
                <html>
                  <head>
                    <title>Đăng nhập thành công</title>
                    <meta charset='UTF-8'>
                    <script>
                      setTimeout(function() {
                        window.open('', '_self');
                        window.close();
                      }, 2000); // tự đóng sau 2 giây
                    </script>
                  </head>
                  <body>
                    <h3>Đăng nhập Google thành công!</h3>
                    <p>Bạn có thể đóng tab này và quay lại ứng dụng.</p>
                  </body>
                </html>";


                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                context.Response.ContentLength64 = buffer.Length;
                await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                context.Response.OutputStream.Close();
                http.Stop();

                // ✅ Đóng trình duyệt nếu còn mở
                if (!browserProcess.HasExited)
                    browserProcess.Kill();

                if (string.IsNullOrEmpty(code))
                {
                    MessageBox.Show("Không lấy được mã xác thực từ Google.");
                    return null;
                }

                // 📩 Gửi request lấy access token
                using (var client = new HttpClient())
                {
                    var requestBody = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("code", code),
                        new KeyValuePair<string, string>("client_id", clientId),
                        new KeyValuePair<string, string>("client_secret", clientSecret),
                        new KeyValuePair<string, string>("redirect_uri", redirectUri),
                        new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    });

                    var tokenResponse = await client.PostAsync("https://oauth2.googleapis.com/token", requestBody);
                    var responseBody = await tokenResponse.Content.ReadAsStringAsync();

                    if (!tokenResponse.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Lỗi khi gửi yêu cầu lấy access token:\n" + responseBody);
                        return null;
                    }

                    var tokenResult = Newtonsoft.Json.JsonConvert.DeserializeObject<Google.Apis.Auth.OAuth2.Responses.TokenResponse>(responseBody);
                    var payload = await GoogleJsonWebSignature.ValidateAsync(tokenResult.IdToken);

                    return payload.Email;
                }
            }
            catch (HttpListenerException ex)
            {
                MessageBox.Show("Không thể khởi động HTTP listener (có thể bị chặn bởi firewall hoặc port bị chiếm).\n" + ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đăng nhập Google:\n" + ex.Message);
                return null;
            }

        }
    }
}
