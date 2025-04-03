
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyChuoiCuaHangTrangSuc.MainForm
{
    public partial class frmMenu : Form
    {
        // Dictionary lưu các Form đã tạo để dùng lại
        private Dictionary<string, Form> openForms = new Dictionary<string, Form>();

        public frmMenu()
        {
            InitializeComponent();
            


            // Khởi tạo giao diện và form đầu tiên
            UIHelper.InitializeUI(panelLeft, btnHome, btnCustomer, btnInvoices,
                         btnProduct, btnStonk, btnSuppiler, sephLine,
                         btnNotification, btnSetting, topSeph);
            UIHelper.SetSelectedButton(btnHome);

            
            // Mở mặc định trang Home
            OpenChildForm("frmHome");
        }

        private void frmMenu_Load(object sender, EventArgs e)
        {
            // Load trước các Form con
            LoadAllChildForms();
        }

        private void LoadAllChildForms()
        {
            // Tạo sẵn các Form để tránh delay khi mở
            openForms["frmHome"] = new frmHome();
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

            // Ẩn Form hiện tại (nếu có)
            if (panelMid.Controls.Count > 0)
            {
                Form currentForm = (Form)panelMid.Controls[0];
                currentForm.Hide();
            }

            // Thêm Form con vào Panel nếu chưa có
            if (!panelMid.Controls.Contains(childForm))
            {
                panelMid.Controls.Clear();
                panelMid.Controls.Add(childForm);
            }

            panelMid.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void picMenu_Click(object sender, EventArgs e)
        {
            UIHelper.TogglePannelVisibility(panelLeft, lblAdmin, picLogo, picMenu, btnHome, btnCustomer,
                btnProduct, btnInvoices, btnStonk, sephLine, btnNotification, btnSetting, topSeph, btnSuppiler);
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            OpenChildForm("frmHome");
            UIHelper.SetSelectedButton(btnHome);
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            OpenChildForm("frmCustomer");
            UIHelper.SetSelectedButton(btnCustomer);
        }

        private void btnSuppiler_Click(object sender, EventArgs e)
        {
            OpenChildForm("frmSupplier");
            UIHelper.SetSelectedButton(btnSuppiler);
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            OpenChildForm("frmProduct");
            UIHelper.SetSelectedButton(btnProduct);
        }

        private void btnInvoices_Click(object sender, EventArgs e)
        {
            OpenChildForm("frmInvoices");
            UIHelper.SetSelectedButton(btnInvoices);
        }

        private void btnStonk_Click(object sender, EventArgs e)
        {
            OpenChildForm("frmThongKe");
            UIHelper.SetSelectedButton(btnStonk);
        }

        private void lblLogOut_Click(object sender, EventArgs e)
        {
            UIHelper.HandleLogout(this);
        }

        
    }
}
