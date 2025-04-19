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

        private void txtSearch_IconRightClick(object sender, EventArgs e)
        {
            string tuKhoa = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(tuKhoa))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DataSet ds = dbPromotion.TimKhuyenMaiTongHop(tuKhoa);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dgvPromotion.DataSource = ds.Tables[0];
                }
                else
                {
                    MessageBox.Show("Không tìm thấy khách hàng phù hợp.", "Kết quả", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtSearch_IconRightClick(sender, e);
                e.SuppressKeyPress = true; // Chặn tiếng beep hoặc hành động mặc định
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                dgvPromotion.DataSource = dbPromotion.LayDanhSachKhuyenMai();
            }
        }
    }
}
