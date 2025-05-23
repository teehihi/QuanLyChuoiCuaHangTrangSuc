﻿using System;
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
            lblHello.Text = $"Xin chào, {GetPrefixBeforeAt(ConnectionHelper.CurrentUserName)}!";
        }

        private void lblLogOut_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            UIHelper.HandleLogout(this);
        }



        private void btnCustomer_Click(object sender, EventArgs e)
        {
            this.Text = "Quản lý khách hàng - TeeNStyle";

            RequestFormChange?.Invoke("frmCustomer");
        }

        private void btnSuppiler_Click(object sender, EventArgs e)
        {
            this.Text = "Quản lý nhà cung cấp - TeeNStyle";
            RequestFormChange?.Invoke("frmSupplier");

        }
        private void btnProduct_Click(object sender, EventArgs e)
        {
            this.Text = "Quản lý sản phẩm - TeeNStyle";
            RequestFormChange?.Invoke("frmProduct");

            //UIHelper.SwitchForm(this, new frmProduct());
        }

        private void btnInvoices_Click(object sender, EventArgs e)
        {
            this.Text = "Quản lý hóa đơn - TeeNStyle";
            RequestFormChange?.Invoke("frmInvoices");
        }

        private void btnStonk_Click(object sender, EventArgs e)
        {

            this.Text = "Thống kê doanh thu - TeeNStyle";
            RequestFormChange?.Invoke("frmThongKe");
        }

        private string GetPrefixBeforeAt(string email)
        {
            if (string.IsNullOrEmpty(email))
                return email;

            int atIndex = email.IndexOf('@');
            if (atIndex >= 0)
                return email.Substring(0, atIndex);

            return email;
        }
    }
}
