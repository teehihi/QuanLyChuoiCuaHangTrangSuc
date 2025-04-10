using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataAcessLayer;

namespace QuanLyChuoiCuaHangTrangSuc.SubForm.NhanVien
{
    public partial class frmHomeNV : Form
    {
        // Sự kiện để yêu cầu mở form con
        public event Action<string> RequestFormChange;
        public frmHomeNV()
        {
            InitializeComponent();
            lblHello.Text = $"Xin chào, {ConnectionHelper.CurrentUserName}!";
        }

        private void lblLogOut_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            UIHelper.HandleLogout(this);
        }



        private void btnCustomer_Click(object sender, EventArgs e)
        {

            RequestFormChange?.Invoke("frmCustomer");
        }

        private void btnSuppiler_Click(object sender, EventArgs e)
        {
            RequestFormChange?.Invoke("frmSupplier");

        }
        private void btnProduct_Click(object sender, EventArgs e)
        {
            RequestFormChange?.Invoke("frmProduct");

            //UIHelper.SwitchForm(this, new frmProduct());
        }

        private void btnInvoices_Click(object sender, EventArgs e)
        {
            RequestFormChange?.Invoke("frmInvoices");
        }

        private void btnStonk_Click(object sender, EventArgs e)
        {

            RequestFormChange?.Invoke("frmThongKe");
        }
    }
}
