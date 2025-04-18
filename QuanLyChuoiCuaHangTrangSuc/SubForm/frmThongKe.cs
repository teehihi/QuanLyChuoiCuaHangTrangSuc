using BusinessAccessLayer;
using ClosedXML.Excel;
using QuanLyChuoiCuaHangTrangSuc.MainForm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Guna.Charts.WinForms;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Globalization;

namespace QuanLyChuoiCuaHangTrangSuc
{
    public partial class frmThongKe : Form
    {
        private readonly Random random = new Random(); // Khởi tạo Random một lần

        public frmThongKe()
        {
            InitializeComponent();
            this.Text = "Thống kê doanh thu - TeeNStyle";
            LoadMonthlyProductStats();
            LoadMonthlyRevuStats();
            LoadMonthlyCustomerStats();
        }

        private void frmThongKe_Load(object sender, EventArgs e)
        {
            LoadTopSellingProducts();
            LoadRevenueTarget_Donut();
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Top Selling Products");

                    worksheet.Cell(1, 1).Value = "Tên Sản Phẩm";
                    worksheet.Cell(1, 2).Value = "Số Lượng Đã Bán";
                    worksheet.Cell(1, 3).Value = "Doanh Thu (VNĐ)";

                    var data = ProductBusiness.GetTopSellingProducts();
                    if (data == null || data.Rows.Count == 0)
                    {
                        MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    int row = 2;
                    foreach (DataRow item in data.Rows)
                    {
                        worksheet.Cell(row, 1).Value = item["Name"].ToString();
                        worksheet.Cell(row, 2).Value = Convert.ToInt32(item["TotalSold"]);
                        worksheet.Cell(row, 3).Value = Convert.ToDecimal(item["TotalRevenue"]);
                        row++;
                    }

                    SaveFileDialog saveFileDialog = new SaveFileDialog
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
            var barDataset = new GunaBarDataset
            {
                CornerRadius = 5,
                BarPercentage = 0.5,
                
                Label = "Số lượng bán"
            };

            foreach (DataRow row in data.Rows)
            {
                string productName;
                double totalSold;

                try
                {
                    productName = row["Name"]?.ToString();
                    totalSold = Convert.ToDouble(row["TotalSold"]);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    continue;
                }

                if (string.IsNullOrEmpty(productName) || totalSold < 0)
                {
                    continue;
                }

                // Tạo LPoint với thuộc tính Y thay vì Value
                var point = new LPoint
                {
                    Label = productName,
                    Y = totalSold // Sử dụng Y để lưu giá trị số
                };
                barDataset.DataPoints.Add(point);
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

            const double revenueTarget = 100_000_000; // 100 triệu VND
            double totalRevenue = 0;

            try
            {
                foreach (DataRow row in data.Rows)
                {
                    double revenue = Convert.ToDouble(row["TotalRevenue"]);
                    totalRevenue += revenue;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tính tổng doanh thu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Làm sạch dữ liệu cũ
            pieChart.Datasets.Clear();
            
            pieChart.BackColor = SystemColors.ControlLight;
            var donutDataset = new GunaDoughnutDataset
            {

                Label = "Tiến độ doanh thu",
                
            };

            // Phần đã đạt
            donutDataset.DataPoints.Add(new LPoint
            {
                Label = "Đã đạt",
                Y = Math.Min(totalRevenue, revenueTarget) // không vượt quá mục tiêu
            });

            // Phần còn lại
            double remaining = revenueTarget - totalRevenue;
            if (remaining > 0)
            {
                donutDataset.DataPoints.Add(new LPoint
                {
                    Label = "Còn thiếu",
                    Y = remaining
                });
            }

            

            pieChart.Datasets.Add(donutDataset);
            // Ẩn trục X
            pieChart.XAxes.Display = false;
            // Ẩn trục Y
            pieChart.YAxes.Display = false;


            pieChart.Update();
        }

        private void LoadMonthlyProductStats()
        {
            var data = ProductBusiness.GetMonthlyStatistics();

            if (data == null || data.Rows.Count == 0)
            {
                lblTongSP.Text = "0";
                lblTangGiam.Text = "Không có dữ liệu";
                lblTangGiam.Location = new Point(22, 109);
                picTangGiam.Image = null;
                return;
            }

            // Lấy tháng hiện tại và tháng trước
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;
            int previousMonth = currentMonth == 1 ? 12 : currentMonth - 1;
            int previousYear = currentMonth == 1 ? currentYear - 1 : currentYear;

            // Tìm dữ liệu theo tháng
            int totalCurrent = 0, totalPrevious = 0;

            foreach (DataRow row in data.Rows)
            {
                int month = Convert.ToInt32(row["Month"]);
                int year = Convert.ToInt32(row["Year"]);
                int totalProducts = Convert.ToInt32(row["TotalSold"]);

                if (month == currentMonth && year == currentYear)
                    totalCurrent = totalProducts;

                else if (month == previousMonth && year == previousYear)
                    totalPrevious = totalProducts;
            }

            // Gán tổng sản phẩm cho label
            lblTongSP.Text = totalCurrent.ToString();

            // So sánh tăng giảm
            if (totalPrevious == 0 && totalCurrent == 0)
            {
                lblTangGiam.Text = "Không có thay đổi";
                lblTangGiam.Location = new Point(22, 109);
                picTangGiam.Image = null;
            }
            else if (totalPrevious == 0)
            {
                lblTangGiam.Text = "Tăng 100% so với tháng trước";
                picTangGiam.Image = Properties.Resources.iconArrowUp;
            }
            else
            {
                double percentChange = ((double)(totalCurrent - totalPrevious) / totalPrevious) * 100;
                if (percentChange > 0)
                {
                    lblTangGiam.Text = $"Tăng {Math.Round(percentChange)}% so với tháng trước";
                    picTangGiam.Image = Properties.Resources.iconArrowUp;
                }
                else if (percentChange < 0)
                {
                    lblTangGiam.Text = $"Giảm {Math.Abs(Math.Round(percentChange))}% so với tháng trước";
                    picTangGiam.Image = Properties.Resources.iconArrowDown;
                }
                else
                {
                    lblTangGiam.Text = "Không thay đổi so với tháng trước";
                    lblTangGiam.Location = new Point(22, 109);
                    picTangGiam.Image = null;

                }
            }

        }


        private void LoadMonthlyRevuStats()
        {
            var data = ProductBusiness.GetMonthlyStatistics();

            if (data == null || data.Rows.Count == 0)
            {
                lblTotal.Text = "0";
                lblTangGiamTotal.Text = "Không có dữ liệu";
                lblTangGiamTotal.Location = new Point(22, 109);
                picTangGiamDT.Image = null;
                return;
            }

            // Lấy tháng hiện tại và tháng trước
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;
            int previousMonth = currentMonth == 1 ? 12 : currentMonth - 1;
            int previousYear = currentMonth == 1 ? currentYear - 1 : currentYear;

            // Tìm dữ liệu theo tháng
            int totalCurrent = 0, totalPrevious = 0;

            foreach (DataRow row in data.Rows)
            {
                int month = Convert.ToInt32(row["Month"]);
                int year = Convert.ToInt32(row["Year"]);
                int totalProducts = Convert.ToInt32(row["TotalRevenue"]);

                if (month == currentMonth && year == currentYear)
                    totalCurrent = totalProducts;

                else if (month == previousMonth && year == previousYear)
                    totalPrevious = totalProducts;
            }

            // Gán tổng sản phẩm cho label
            lblTotal.Text = totalCurrent.ToString("C0", CultureInfo.GetCultureInfo("vi-VN"));


            // So sánh tăng giảm
            if (totalPrevious == 0 && totalCurrent == 0)
            {
                lblTangGiamTotal.Text = "Không có thay đổi";
                lblTangGiamTotal.Location = new Point(22, 109);
                picTangGiamDT.Image = null;
            }
            else if (totalPrevious == 0)
            {
                lblTangGiamTotal.Text = "Tăng 100% so với tháng trước";
                picTangGiamDT.Image = Properties.Resources.iconArrowUp;
            }
            else
            {
                double percentChange = ((double)(totalCurrent - totalPrevious) / totalPrevious) * 100;
                if (percentChange > 0)
                {
                    lblTangGiamTotal.Text = $"Tăng {Math.Round(percentChange)}% so với tháng trước";
                    picTangGiamDT.Image = Properties.Resources.iconArrowUp;
                }
                else if (percentChange < 0)
                {
                    lblTangGiamTotal.Text = $"Giảm {Math.Abs(Math.Round(percentChange))}% so với tháng trước";
                    picTangGiamDT.Image = Properties.Resources.iconArrowDown;
                }
                else
                {
                    lblTangGiamTotal.Text = "Không thay đổi so với tháng trước";
                    lblTangGiamTotal.Location = new Point(22, 109);
                    picTangGiamDT.Image = null;

                }
            }

        }
        private void LoadMonthlyCustomerStats()
        {
            var data = ProductBusiness.GetMonthlyStatistics();

            if (data == null || data.Rows.Count == 0)
            {
                lblTotalCustomer.Text = "0";
                lblTangGiamCustomer.Text = "Không có dữ liệu";
                lblTangGiamCustomer.Location = new Point(22, 109);
                picTangGiamKH.Image = null;
                return;
            }

            // Lấy tháng hiện tại và tháng trước
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;
            int previousMonth = currentMonth == 1 ? 12 : currentMonth - 1;
            int previousYear = currentMonth == 1 ? currentYear - 1 : currentYear;

            // Tìm dữ liệu theo tháng
            int totalCurrent = 0, totalPrevious = 0;

            foreach (DataRow row in data.Rows)
            {
                int month = Convert.ToInt32(row["Month"]);
                int year = Convert.ToInt32(row["Year"]);
                int totalProducts = Convert.ToInt32(row["TotalSold"]);

                if (month == currentMonth && year == currentYear)
                    totalCurrent = totalProducts;

                else if (month == previousMonth && year == previousYear)
                    totalPrevious = totalProducts;
            }

            // Gán tổng sản phẩm cho label
            lblTotalCustomer.Text = totalCurrent.ToString();

            // So sánh tăng giảm
            if (totalPrevious == 0 && totalCurrent == 0)
            {
                lblTangGiamCustomer.Text = "Không có thay đổi";
                lblTangGiamCustomer.Location = new Point(22, 109);
                picTangGiamKH.Image = null;
            }
            else if (totalPrevious == 0)
            {
                lblTangGiamCustomer.Text = "Tăng 100% so với tháng trước";
                picTangGiamKH.Image = Properties.Resources.iconArrowUp;
            }
            else
            {
                double percentChange = ((double)(totalCurrent - totalPrevious) / totalPrevious) * 100;
                if (percentChange > 0)
                {
                    lblTangGiamCustomer.Text = $"Tăng {Math.Round(percentChange)}% so với tháng trước";
                    picTangGiamKH.Image = Properties.Resources.iconArrowUp;
                }
                else if (percentChange < 0)
                {
                    lblTangGiamCustomer.Text = $"Giảm {Math.Abs(Math.Round(percentChange))}% so với tháng trước";
                    picTangGiamKH.Image = Properties.Resources.iconArrowDown;
                }
                else
                {
                    lblTangGiamCustomer.Text = "Không thay đổi so với tháng trước";
                    lblTangGiamCustomer.Location = new Point(22, 109);
                    picTangGiamKH.Image = null;

                }
            }

        }
    }
}