using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyChuoiCuaHangTrangSuc
{
    public static class UIHelper
    {

        // Thiết lập giao diện ban đầu
        public static void InitializeUI(Panel panelLeft, Guna2Button btnHome, Guna2Button btnCustomer, Guna2Button btnInvoices,
            Guna2Button btnProduct, Guna2Button btnStonk, Guna2Button btnSupplier, Guna2Separator sephLine, Guna2Button btnNotification,
            Guna2Button btnSetting, Guna2Separator topSeph)
        {
            int btnWidthBegin = 50;
            panelLeft.Width = 80;
            btnHome.Width = btnWidthBegin;
            btnCustomer.Width = btnWidthBegin;
            btnInvoices.Width = btnWidthBegin;
            btnProduct.Width = btnWidthBegin;
            btnStonk.Width = btnWidthBegin;
            btnSupplier.Width = btnWidthBegin;
            sephLine.Width = btnWidthBegin - 10;
            btnNotification.Width = btnWidthBegin;
            btnSetting.Width = btnWidthBegin;
            topSeph.Visible = false;
        }
        // Đóng mở panel sidebar
        public static void TogglePannelVisibility(Panel panelLeft,Label lblAdmin, PictureBox picLogo, PictureBox picMenu, Guna2Button btnHome, Guna2Button btnCustomer,
            Guna2Button btnProduct, Guna2Button btnInvoices, Guna2Button btnStonk, Guna2Separator seph, Guna2Button btnNotification,
            Guna2Button btnSetting, Guna2Separator topSeph, Guna2Button btnSupplier)
        {
            int a = 285, b = 50;
            picLogo.Visible = false;
            lblAdmin.Visible = false;
            btnHome.Width = a;
            btnCustomer.Width = a;
            btnProduct.Width = a;
            btnInvoices.Width = a;
            btnStonk.Width = a;
            btnSupplier.Width = a;
            seph.Width = 40;
            btnNotification.Width = a;
            btnSetting.Width = a;
            topSeph.Visible = false;

            if (panelLeft.Width == 80)
            {
                // Mở rộng panel
                panelLeft.Width = 320;
                picLogo.Visible = true;
                lblAdmin.Visible = true;
                picMenu.Image = Properties.Resources.menu_icon_closed; // Đổi icon
                btnHome.Width = a;
                btnCustomer.Width = a;
                btnProduct.Width = a;
                btnInvoices.Width = a;
                btnStonk.Width = a;
                btnSupplier.Width = a;
                seph.Width = a - 10;
                btnNotification.Width = a;
                btnSetting.Width = a;
                
                topSeph.Visible = true;
            }
            else if (panelLeft.Width == 320)
            {
                // Thu nhỏ panel
                panelLeft.Width = 80;
                picLogo.Visible = false;
                lblAdmin.Visible= false;
                btnHome.Width = b;
                btnCustomer.Width = b;
                btnProduct.Width = b;
                btnInvoices.Width = b;
                btnStonk.Width = b;
                btnSupplier.Width = b;
                seph.Width = 40 ;
                btnNotification.Width = b;
                btnSetting.Width = b;
                picMenu.Image = Properties.Resources.menu_icon; // Đổi icon
                
                topSeph.Visible = false;
            }
        }


            // Chuyển đổi Form
            public static void SwitchForm(Form currentForm, Form newForm)
        {
            currentForm.BeginInvoke(new Action(() =>
            {
                newForm.Shown += (s, e) => currentForm.Hide(); // Chỉ đóng khi form mới đã hiển thị hoàn toàn
                newForm.ShowDialog();
                currentForm.Close();
            }));
        }

        // Xử lý sự kiện đăng xuất
        public static void HandleLogout(Form currentForm)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn ĐĂNG XUẤT?", "CẢNH BÁO", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (result == DialogResult.OK)
            {
                currentForm.Hide();

                frmLogin loginForm = new frmLogin();
                loginForm.ShowDialog();

                currentForm.Close();
            }
        }



        //Thêm hover cho panel
        public static void AddHoverEffect(Guna2Panel panel, Color hoverColor, Color normalColor)
        {
            // Lưu kích thước ban đầu của panel và các phần tử con
            Size originalSize = panel.Size;
            Dictionary<Control, Size> originalSizes = new Dictionary<Control, Size>();

            foreach (Control ctrl in panel.Controls)
            {
                originalSizes[ctrl] = ctrl.Size; // Lưu kích thước gốc của các phần tử con
            }

            // Kích thước khi phóng to
            Size hoverSize = new Size(originalSize.Width + 10, originalSize.Height + 10);

            // Tạo Timer để làm mượt animation
            Timer expandTimer = new Timer { Interval = 10 };
            expandTimer.Tick += (s, e) =>
            {
                if (panel.Width < hoverSize.Width)
                {
                    panel.Width += 2;
                    panel.Height += 2;

                    foreach (Control ctrl in panel.Controls)
                    {
                        ctrl.Width += 1; // Phóng to từng control nhỏ bên trong
                        ctrl.Height += 1;
                    }
                }
                else
                {
                    expandTimer.Stop();
                }
            };

            Timer shrinkTimer = new Timer { Interval = 10 };
            shrinkTimer.Tick += (s, e) =>
            {
                if (panel.Width > originalSize.Width)
                {
                    panel.Width -= 2;
                    panel.Height -= 2;

                    foreach (Control ctrl in panel.Controls)
                    {
                        ctrl.Width -= 1; // Thu nhỏ từng control bên trong
                        ctrl.Height -= 1;
                    }
                }
                else
                {
                    shrinkTimer.Stop();
                }
            };

            // Sự kiện khi chuột đi vào panel hoặc phần tử con
            EventHandler mouseEnter = (s, e) =>
            {
                panel.FillColor = hoverColor;
                shrinkTimer.Stop();
                expandTimer.Start();
            };

            // Sự kiện khi chuột rời khỏi panel hoặc phần tử con
            EventHandler mouseLeave = (s, e) =>
            {
                panel.FillColor = normalColor;
                expandTimer.Stop();
                shrinkTimer.Start();
            };

            // Gán sự kiện cho panel chính
            panel.MouseEnter += mouseEnter;
            panel.MouseLeave += mouseLeave;

            // Gán sự kiện cho tất cả phần tử con bên trong panel
            foreach (Control ctrl in panel.Controls)
            {
                ctrl.BackColor = Color.Transparent;
                ctrl.MouseEnter += mouseEnter;
                ctrl.MouseLeave += mouseLeave;
            }
        }



        //FORM PRODUCT
        private static Guna2Button currentSelectedButton = null;


        // Thay đổi trạng thái của nút được chọn
        public static void SetSelectedButton(Guna2Button button)
        {
            // Nếu nút mới được chọn khác nút hiện tại
            if (currentSelectedButton != button)
            {
                // Bỏ chọn nút trước đó nếu có
                if (currentSelectedButton != null)
                {
                    currentSelectedButton.Checked = false;
                }

                // Đặt nút hiện tại thành nút mới được chọn
                currentSelectedButton = button;
                currentSelectedButton.Checked = true;
            }
        }




        // Chuyển số tiền thành chữ
        public static string NumberToVietnameseText(decimal number)
        {
            if (number == 0) return "(Không đồng)";

            string[] unitNames = { "", "nghìn", "triệu", "tỷ", "nghìn tỷ", "triệu tỷ" };
            string[] digitNames = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };

            string result = "";
            int unitIndex = 0;

            while (number > 0)
            {
                int block = (int)(number % 1000); // Lấy từng khối 3 số
                if (block > 0)
                {
                    string blockText = ConvertBlockToText(block, digitNames);
                    if (unitIndex > 0) blockText += " " + unitNames[unitIndex];
                    result = blockText + " " + result;
                }

                number /= 1000;
                unitIndex++;
            }

            result = result.Trim() + " đồng";

            // Bọc trong ngoặc đơn và viết hoa chữ cái đầu tiên
            return FormatVietnameseText(result);
        }

        private static string ConvertBlockToText(int number, string[] digitNames)
        {
            int tram = number / 100;
            int chuc = (number / 10) % 10;
            int donvi = number % 10;
            string result = "";

            if (tram > 0)
                result += digitNames[tram] + " trăm";
            
            if (chuc > 0)
            {
                if (chuc == 1)
                    result += " mười";
                else
                    result += " " + digitNames[chuc] + " mươi";
            }

            if (donvi > 0)
            {
                if (chuc == 0 && tram > 0)
                    result += " lẻ";
                if (donvi == 1 && chuc > 1)
                    result += " mốt";
                else if (donvi == 5 && chuc > 0)
                    result += " lăm";
                else
                    result += " " + digitNames[donvi];
            }

            return result.Trim();
        }






        // Thêm dấu ngoặc đơn và viết hoa chữ cái đầu
        private static string FormatVietnameseText(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return "(" + char.ToUpper(input[0]) + input.Substring(1) + ")";
        }
    }
}
