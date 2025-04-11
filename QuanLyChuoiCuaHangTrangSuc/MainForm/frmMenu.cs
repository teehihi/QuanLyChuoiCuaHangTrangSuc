
using DataAcessLayer;
using Guna.UI2.WinForms;
using QuanLyChuoiCuaHangTrangSuc.SubForm.NhanVien;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using QuanLyChuoiCuaHangTrangSuc.SubForm.ChatForm;
using System.Linq;
namespace QuanLyChuoiCuaHangTrangSuc.MainForm
{
    public interface IReloadable
    {
        void ReloadData();
    }
    public partial class frmMenu : Form
    {

        // Dictionary lưu các Form đã tạo để dùng lại
        private Dictionary<string, Form> openForms = new Dictionary<string, Form>();
        private frmChat chatForm;

        // Định nghĩa sự kiện để gọi từ frmHome
        public event Action<string> OnChildFormRequested;

        public frmMenu()
        {
            InitializeComponent();
            this.ShowInTaskbar = true;
            this.ShowIcon = true;
            




            // Khởi tạo giao diện và form đầu tiên
            UIHelper.InitializeUI(panelLeft, btnHome, btnCustomer, btnInvoices,
                         btnProduct, btnStonk, btnSuppiler, sephLine,
                         btnNotification, btnSetting, topSeph);
            UIHelper.SetSelectedButton(btnHome);

            
        }

        private void frmMenu_Load(object sender, EventArgs e)
        {
            // Load trước các Form con
            LoadAllChildForms();

            if (ConnectionHelper.IsManager)
            {
                OpenChildForm("frmHome"); // Giao diện Quản lý
               
            }
            else
            {
                OpenChildForm("frmHomeNV"); // Giao diện Nhân viên
                btnSuppiler.Visible = false; // Ẩn nút nhà cung cấp
                // Di chuyển các nút lên 50px
                btnStonk.Location = new Point(btnStonk.Location.X, btnStonk.Location.Y - 50);
                btnProduct.Location = new Point(btnProduct.Location.X, btnProduct.Location.Y - 50);
                btnInvoices.Location = new Point(btnInvoices.Location.X, btnInvoices.Location.Y - 50);
                lblAdmin.Text = "Employee Tools"; // Đổi tên admin thành Employee

            }




            // Kết nối sự kiện từ frmHome
            if (openForms["frmHome"] is frmHome homeForm)
            {
                homeForm.RequestFormChange += (formName) =>
                {
                    OpenChildForm(formName);
                    UpdateSelectedButton(formName);
                };
            }
            if (openForms["frmHomeNV"] is frmHomeNV homeNVForm)
            {
                homeNVForm.RequestFormChange += (formName) =>
                {
                    OpenChildForm(formName);
                    UpdateSelectedButton(formName);
                };
            }


        }

        // Hàm để cập nhật nút được chọn
        private void UpdateSelectedButton(string formName)
        {
            // Gọi UIHelper.SetSelectedButton với nút tương ứng
            switch (formName)
            {
                case "frmHomeNV":
                    UIHelper.SetSelectedButton(btnHome);
                    break;
                case "frmHome":
                    UIHelper.SetSelectedButton(btnHome);
                    break;
                case "frmCustomer":
                    UIHelper.SetSelectedButton(btnCustomer);
                    break;
                case "frmProduct":
                    UIHelper.SetSelectedButton(btnProduct);
                    break;
                case "frmSupplier":
                    UIHelper.SetSelectedButton(btnSuppiler);
                    break;
                case "frmInvoices":
                    UIHelper.SetSelectedButton(btnInvoices);
                    break;
                case "frmThongKe":
                    UIHelper.SetSelectedButton(btnStonk);
                    break;
                default:
                    // Mặc định nếu formName không hợp lệ
                    break;
            }
        }

        private void LoadAllChildForms()
        {
            // Tạo sẵn các Form để tránh delay khi mở
            openForms["frmHome"] = new frmHome();
            openForms["frmHomeNV"] = new frmHomeNV();
            openForms["frmCustomer"] = new frmCustomer();
            openForms["frmProduct"] = new frmProduct();
            openForms["frmSupplier"] = new frmSupplier();
            openForms["frmInvoices"] = new frmInvoices();
            openForms["frmThongKe"] = new frmThongKe();

            // Thiết lập Form con để có thể thêm vào Panel
            foreach (var form in openForms.Values)
            {
                form.TopLevel = false;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Dock = DockStyle.Fill;
            }
        }



        private void OpenChildForm(string formName)
        {
            if (!openForms.ContainsKey(formName)) return;

            Form childForm = openForms[formName];

            // Ẩn các Form hiện có trong panelMid
            foreach (Control control in panelMid.Controls)
            {
                if (control is Form form)
                {
                    form.Hide();
                }
            }

            // Thêm Form con vào Panel nếu chưa có
            if (!panelMid.Controls.Contains(childForm))
            {
                childForm.TopLevel = false;
                childForm.FormBorderStyle = FormBorderStyle.None;
                childForm.Dock = DockStyle.Fill;
                panelMid.Controls.Add(childForm);
            }

            panelMid.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            btnChat.BringToFront();
        }

        private void ReloadChildForm(string formName)
        {
            if (openForms.TryGetValue(formName, out Form form) && form is IReloadable reloadable)
            {
                reloadable.ReloadData();
            }
        }

        private void picMenu_Click(object sender, EventArgs e)
        {
            UIHelper.TogglePannelVisibility(panelLeft, lblAdmin, picLogo, picMenu, btnHome, btnCustomer,
                btnProduct, btnInvoices, btnStonk, sephLine, btnNotification, btnSetting, topSeph, btnSuppiler);
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Text = "Trang chủ - TeeNStyle";
            if (panelLeft.Width != 80)
            {
                picMenu_Click(sender, e); // Đóng menu bên trái nếu đang mở

            }
            if (ConnectionHelper.IsManager)
            {
                OpenChildForm("frmHome"); // Giao diện Quản lý
            }
            else
            {
                OpenChildForm("frmHomeNV"); // Giao diện Nhân viên
                

            }

            UIHelper.SetSelectedButton(btnHome);
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            this.Text = "Quản lý khách hàng - TeeNStyle";
            if (panelLeft.Width != 80)
            {
                picMenu_Click(sender, e); // Đóng menu bên trái nếu đang mở

            }
            
            OpenChildForm("frmCustomer");
            ReloadChildForm("frmCustomer");
            UIHelper.SetSelectedButton(btnCustomer);
        }

        private void btnSuppiler_Click(object sender, EventArgs e)
        {
            this.Text = "Quản lý nhà cung cấp - TeeNStyle";
            if (panelLeft.Width != 80)
            {
                picMenu_Click(sender, e); // Đóng menu bên trái nếu đang mở

            }
            
            OpenChildForm("frmSupplier");
            ReloadChildForm("frmSupplier");
            UIHelper.SetSelectedButton(btnSuppiler);
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            this.Text = "Quản lý sản phẩm - TeeNStyle";
            if (panelLeft.Width != 80)
            {
                picMenu_Click(sender, e); // Đóng menu bên trái nếu đang mở

            }
            
            OpenChildForm("frmProduct");
            ReloadChildForm("frmProduct");
            UIHelper.SetSelectedButton(btnProduct);
        }

        private void btnInvoices_Click(object sender, EventArgs e)
        {
            this.Text = "Quản lý hóa đơn - TeeNStyle";
            if (panelLeft.Width != 80)
            {
                picMenu_Click(sender, e); // Đóng menu bên trái nếu đang mở

            }
           
            OpenChildForm("frmInvoices");
            ReloadChildForm("frmInvoices");
            UIHelper.SetSelectedButton(btnInvoices);
        }

        private void btnStonk_Click(object sender, EventArgs e)
        {
            this.Text = "Thống kê doanh thu - TeeNStyle";
            if (panelLeft.Width != 80)
            {
                picMenu_Click(sender, e); // Đóng menu bên trái nếu đang mở

            }
            
            OpenChildForm("frmThongKe");
            ReloadChildForm("frmThongKe");
            UIHelper.SetSelectedButton(btnStonk);
        }

        private void lblLogOut_Click(object sender, EventArgs e)
        {
           
            UIHelper.HandleLogout(this);
            
        }

        private void btnChat_Click(object sender, EventArgs e)
        {
            frmChat chatForm = new frmChat();
            chatForm.TopMost = true;
            chatForm.StartPosition = FormStartPosition.Manual;
            chatForm.Location = new Point(this.Right - 360, this.Bottom - 450); // điều chỉnh vị trí cho phù hợp
            chatForm.Show();

        }

      

    }
}
