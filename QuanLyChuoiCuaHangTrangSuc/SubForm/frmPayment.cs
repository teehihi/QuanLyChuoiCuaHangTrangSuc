using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyChuoiCuaHangTrangSuc.SubForm
{
    public partial class frmPayment : Form
    {

        private int amount;
        private string content;
        private string paymentMethod;
        public frmPayment(int amountTotal)
        {
            InitializeComponent();
            amount = amountTotal;
            content = Uri.EscapeDataString($"TEENNSTYLEHOADON");
    }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCash_Click(object sender, EventArgs e)
        {
            paymentMethod = "Cash";
            picQR.Visible = false; // Ẩn ảnh QR
        }

        private void btnVNPAY_Click(object sender, EventArgs e)
        {
            paymentMethod = "VNPAY";
            string qrContent = "";
            // Dữ liệu VietQR tối giản (cần theo chuẩn nếu dùng thực tế)
            string bankAcc = "1040489156";
            string bankCode = "VCB"; // Mã ngân hàng
            string accName = "NGUYEN NHAT THIEN";

            qrContent = $"https://img.vietqr.io/image/{bankCode}-{bankAcc}-qr_only.png?amount={amount}&addInfo={content}&accountName={Uri.EscapeDataString(accName)}";
            picQR.Load(qrContent); // Vì VietQR hỗ trợ ảnh URL

            picQR.Visible = true; // Hiện ảnh QR
        }

        private void btnMoMo_Click(object sender, EventArgs e)
        {
            paymentMethod = "MoMo";
            picQR.Image = Properties.Resources.momoQR;
            picQR.Visible = true;

        }

        private void btnZaloPay_Click(object sender, EventArgs e)
        {
            paymentMethod = "ZaloPay";
            string qrContent = "";
            // Dữ liệu VietQR tối giản (cần theo chuẩn nếu dùng thực tế)
            string bankAcc = "1040489156";
            string bankCode = "VCB"; // Mã ngân hàng
            string accName = "NGUYEN NHAT THIEN";

            qrContent = $"https://img.vietqr.io/image/{bankCode}-{bankAcc}-qr_only.png?amount={amount}&addInfo={content}&accountName={Uri.EscapeDataString(accName)}";
            picQR.Load(qrContent); // Vì VietQR hỗ trợ ảnh URL

            picQR.Visible = true; // Hiện ảnh QR
        }
    }
}
