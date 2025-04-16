using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyChuoiCuaHangTrangSuc.UCController
{
    public partial class UCOrderHistory : UserControl
    {
        private Timer fadeTimer = new Timer();
        private Color startColor = SystemColors.Control;
        private Color targetColor = Color.LightBlue;
        private Color currentColor;
        private bool fadingIn = false;
        private int fadeStep = 0;
        private const int totalSteps = 10;

        public event EventHandler<string> OrderClicked;
        public UCOrderHistory()
        {
            InitializeComponent();
            ApplyMouseEvents(this);

            fadeTimer.Interval = 2; // tốc độ mượt
            fadeTimer.Tick += FadeTimer_Tick;

            this.currentColor = this.BackColor;

            this.Click += UCOrderHistory_Click;
            foreach (Control ctrl in this.Controls) ctrl.Click += UCOrderHistory_Click;
        }


        private void UCOrderHistory_Click(object sender, EventArgs e)
        {
            OrderClicked?.Invoke(this, this.OrderID); // phát sự kiện với OrderID
        }



        private void StartFade(Color fromColor, Color toColor)
        {
            startColor = fromColor;
            targetColor = toColor;
            fadeStep = 0;
            fadeTimer.Start();
        }

        private void FadeTimer_Tick(object sender, EventArgs e)
        {
            fadeStep++;

            int r = Interpolate(startColor.R, targetColor.R, fadeStep, totalSteps);
            int g = Interpolate(startColor.G, targetColor.G, fadeStep, totalSteps);
            int b = Interpolate(startColor.B, targetColor.B, fadeStep, totalSteps);

            guna2Panel1.FillColor = Color.FromArgb(r, g, b);

            if (fadeStep >= totalSteps)
            {
                fadeTimer.Stop();
            }
        }


        private int Interpolate(int start, int end, int step, int maxSteps)
        {
            return start + (end - start) * step / maxSteps;
        }

        private void Control_MouseEnter(object sender, EventArgs e)
        {
            StartFade(guna2Panel1.FillColor, Color.LightBlue);
        }

        private void Control_MouseLeave(object sender, EventArgs e)
        {
            StartFade(guna2Panel1.FillColor, SystemColors.Control);
        }

        
        private void ApplyMouseEvents(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                
                control.MouseEnter += Control_MouseEnter;
                control.MouseLeave += Control_MouseLeave;

                if (control.HasChildren)
                {
                    ApplyMouseEvents(control);
                }
            }
            parent.MouseEnter += Control_MouseEnter;
            parent.MouseLeave += Control_MouseLeave;
            
        }



        public string OrderID
        {
            get => lblOrderID.Text;
            set => lblOrderID.Text = value;
        }
        public string CustomerName
        {
            get => lblCustomer.Text;
            set => lblCustomer.Text = value;
        }
        public string PaymentMethod
        {
            get => lblPayment.Text;
            set => lblPayment.Text = value;
        }
        public string OrderDelivery
        {
            get => lblDeli.Text;
            set => lblDeli.Text = value;
        }
        public string OrderApp
        {
            get => lblApp.Text;
            set => lblApp.Text = value;
        }

        public string OrderDate
        {
            get => lblDate.Text;
            set => lblDate.Text = value;
        }
        public string OrderStatus
        {
            get => lblStatus.Text;
            set => lblStatus.Text = value;
        }
        public string TotalAmount
        {
            get => lblTotal.Text;
            set => lblTotal.Text = value;
        }

      
    }
}
