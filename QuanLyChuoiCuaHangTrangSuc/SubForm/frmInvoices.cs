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
    public partial class frmInvoices : Form
    {
        public frmInvoices()
        {
            InitializeComponent();
            this.Text = "Quản lý hóa đơn - TeeNStyle";
        }


        private void frmInvoices_Load(object sender, EventArgs e)
        {
            UIHelper.InitializeUI(panelLeft, btnHome, btnCustomer, btnInvoices,
                         btnProduct, btnStonk, btnSuppiler, sephLine,
                         btnNotification, btnSetting, topSeph);

        }
        private void picMenu_Click(object sender, EventArgs e)
        {
            UIHelper.TogglePannelVisibility(panelLeft, lblAdmin, picLogo, picMenu, btnHome, btnCustomer,
                btnProduct, btnInvoices, btnStonk, sephLine, btnNotification, btnSetting, topSeph, btnSuppiler);

        }
        private void lblLogOut_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            UIHelper.HandleLogout(this);
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            UIHelper.SwitchForm(this, new frmHome());
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            UIHelper.SwitchForm(this, new frmCustomer());
        }
        private void btnSuppiler_Click(object sender, EventArgs e)
        {
            UIHelper.SwitchForm(this, new frmSupplier());
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            UIHelper.SwitchForm(this, new frmProduct());
        }

        private void btnInvoices_Click(object sender, EventArgs e)
        {
            UIHelper.SwitchForm(this, new frmInvoices());
        }

        private void btnStonk_Click(object sender, EventArgs e)
        {
            UIHelper.SwitchForm(this, new frmThongKe());
        }
    }
}
