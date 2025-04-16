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
        public UCOrderHistory()
        {
            InitializeComponent();
            //foreach (Control control in this.Controls)
            //{
            //    control.MouseHover += new EventHandler(UCOrderHistory_MouseEnter);
            //    control.MouseLeave += new EventHandler(UCOrderHistory_MouseLeave);
            //}
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
