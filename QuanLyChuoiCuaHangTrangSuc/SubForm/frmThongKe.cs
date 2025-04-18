using BusinessAccessLayer;
using ClosedXML.Excel;
using QuanLyChuoiCuaHangTrangSuc.MainForm;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Guna.Charts.WinForms;
using Guna.UI2.WinForms;

namespace QuanLyChuoiCuaHangTrangSuc
{
    public partial class frmThongKe : Form
    {
        private readonly Random random = new Random();

        public frmThongKe()
        {
            InitializeComponent();
            this.Text = "Thống kê doanh thu - TeeNStyle";
            
        }

        private void frmThongKe_Load(object sender, EventArgs e)
        {
            LoadTopSellingProducts();
            LoadRevenueTarget_Donut();
            LoadMonthlyStats();
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                var data = ProductBusiness.GetTopSellingProducts();
                if (data == null || data.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Top Selling Products");
                worksheet.Cell(1, 1).Value = "Tên Sản Phẩm";
                worksheet.Cell(1, 2).Value = "Số Lượng Đã Bán";
                worksheet.Cell(1, 3).Value = "Doanh Thu (VNĐ)";

                int row = 2;
                foreach (DataRow item in data.Rows)
                {
                    worksheet.Cell(row, 1).Value = item["Name"].ToString();
                    worksheet.Cell(row, 2).Value = Convert.ToInt32(item["TotalSold"]);
                    worksheet.Cell(row, 3).Value = Convert.ToDecimal(item["TotalRevenue"]);
                    row++;
                }

                var saveFileDialog = new SaveFileDialog
                {
                    FileName = "TopSellingProducts.xlsx",
                    Filter = "Excel Files|*.xlsx",
                    Title = "Lưu tệp Excel"
                };
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    workbook.SaveAs(saveFileDialog.FileName);
                    MessageBox.Show("Xuất Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTopSellingProducts()
        {
            var data = ProductBusiness.GetTopSellingProducts();
            if (data == null || data.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để hiển thị biểu đồ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            topSaleChart.Datasets.Clear();
            topSaleChart.BackColor = SystemColors.ControlLight;
            var barDataset = new GunaBarDataset { CornerRadius = 5, BarPercentage = 0.5, Label = "Số lượng bán" };

            foreach (DataRow row in data.Rows)
            {
                if (row["Name"] is string productName && double.TryParse(row["TotalSold"].ToString(), out double totalSold) && totalSold >= 0)
                {
                    barDataset.DataPoints.Add(new LPoint { Label = productName, Y = totalSold });
                }
            }

            if (barDataset.DataPoints.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu hợp lệ để hiển thị biểu đồ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            topSaleChart.Datasets.Add(barDataset);
            topSaleChart.Update();
        }

        private void LoadRevenueTarget_Donut()
        {
            var data = ProductBusiness.GetTopSellingProducts();
            if (data == null || data.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để hiển thị biểu đồ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double totalRevenue = 0;
            foreach (DataRow row in data.Rows)
                totalRevenue += Convert.ToDouble(row["TotalRevenue"]);

            pieChart.Datasets.Clear();
            pieChart.BackColor = SystemColors.ControlLight;

            var donutDataset = new GunaDoughnutDataset { Label = "Tiến độ doanh thu" };
            donutDataset.DataPoints.Add(new LPoint { Label = "Đã đạt", Y = Math.Min(totalRevenue, 100_000_000) });

            double remaining = 100_000_000 - totalRevenue;
            if (remaining > 0)
                donutDataset.DataPoints.Add(new LPoint { Label = "Còn thiếu", Y = remaining });

            pieChart.Datasets.Add(donutDataset);
            pieChart.XAxes.Display = false;
            pieChart.YAxes.Display = false;
            pieChart.Update();
            lblDoanhSoCanDat.Text = $"Đã đạt: {totalRevenue.ToString("C0", CultureInfo.GetCultureInfo("vi-VN"))} / {100000000.ToString("C0", CultureInfo.GetCultureInfo("vi-VN"))}";
        }

        private void LoadMonthlyStats()
        {
            var data = ProductBusiness.GetMonthlyStatistics();
            if (data == null || data.Rows.Count == 0)
            {
                SetStats(0, 0, lblTongSP, lblTangGiam, picTangGiam);
                SetStats(0, 0, lblTotal, lblTangGiamTotal, picTangGiamDT, true);
                SetStats(0, 0, lblTotalCustomer, lblTangGiamCustomer, picTangGiamKH);
                return;
            }

            var (currMonth, currYear) = (DateTime.Now.Month, DateTime.Now.Year);
            var (prevMonth, prevYear) = currMonth == 1 ? (12, currYear - 1) : (currMonth - 1, currYear);

            int currentSold = 0, previousSold = 0;
            int currentRevenue = 0, previousRevenue = 0;

            foreach (DataRow row in data.Rows)
            {
                int month = Convert.ToInt32(row["Month"]);
                int year = Convert.ToInt32(row["Year"]);
                int sold = Convert.ToInt32(row["TotalSold"]);
                int revenue = Convert.ToInt32(row["TotalRevenue"]);

                if (month == currMonth && year == currYear)
                {
                    currentSold = sold;
                    currentRevenue = revenue;
                }
                else if (month == prevMonth && year == prevYear)
                {
                    previousSold = sold;
                    previousRevenue = revenue;
                }
            }

            SetStats(previousSold, currentSold, lblTongSP, lblTangGiam, picTangGiam);
            SetStats(previousRevenue, currentRevenue, lblTotal, lblTangGiamTotal, picTangGiamDT, true);
            SetStats(previousSold, currentSold, lblTotalCustomer, lblTangGiamCustomer, picTangGiamKH);
        }

        private void SetStats(int previous, int current, Guna2HtmlLabel totalLabel, Guna2HtmlLabel changeLabel, Guna2PictureBox icon, bool isCurrency = false)
        {
            totalLabel.Text = isCurrency ? current.ToString("C0", CultureInfo.GetCultureInfo("vi-VN")) : current.ToString();

            if (previous == 0 && current == 0)
            {
                changeLabel.Text = "Không có thay đổi";
                changeLabel.Location = new Point(22, 109);
                icon.Image = null;
            }
            else if (previous == 0)
            {
                changeLabel.Text = "Tăng 100% so với tháng trước";
                icon.Image = Properties.Resources.iconArrowUp;
            }
            else
            {
                double percent = ((double)(current - previous) / previous) * 100;
                if (percent > 0)
                {
                    changeLabel.Text = $"Tăng {Math.Round(percent)}% so với tháng trước";
                    icon.Image = Properties.Resources.iconArrowUp;
                }
                else if (percent < 0)
                {
                    changeLabel.Text = $"Giảm {Math.Abs(Math.Round(percent))}% so với tháng trước";
                    icon.Image = Properties.Resources.iconArrowDown;
                }
                else
                {
                    changeLabel.Text = "Không thay đổi so với tháng trước";
                    changeLabel.Location = new Point(22, 109);
                    icon.Image = null;
                }
            }
        }
    }
}