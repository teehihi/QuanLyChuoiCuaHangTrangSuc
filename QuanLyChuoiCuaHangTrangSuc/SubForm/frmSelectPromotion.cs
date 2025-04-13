using BusinessAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyChuoiCuaHangTrangSuc.SubForm
{
    public partial class frmSelectPromotion : Form
    {
        public double? DiscountRate { get; set; }
        public string PromotionName { get; set; }
        public int PromotionID { get; set; }
        DBPromotion dbPromotion = new DBPromotion();

        public frmSelectPromotion()
        {
            InitializeComponent();
        }

        private void frmSelectPromotion_Load(object sender, EventArgs e)
        {
            dgvPromotion.DataSource = dbPromotion.LayDanhSachKhuyenMai();
        }

        private void dgvPromotion_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                double discountRate = 0;
                double.TryParse(dgvPromotion.Rows[e.RowIndex].Cells["DiscountRate"].Value.ToString(), out discountRate);

                DiscountRate = discountRate;
                PromotionName = dgvPromotion.Rows[e.RowIndex].Cells["Name"].Value.ToString();

                PromotionID = Convert.ToInt32(dgvPromotion.Rows[e.RowIndex].Cells["PromotionID"].Value.ToString());

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
