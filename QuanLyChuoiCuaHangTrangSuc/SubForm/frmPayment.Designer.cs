namespace QuanLyChuoiCuaHangTrangSuc.SubForm
{
    partial class frmPayment
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPayment));
            this.guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(this.components);
            this.btnHuy = new Guna.UI2.WinForms.Guna2Button();
            this.btnComplete = new Guna.UI2.WinForms.Guna2Button();
            this.lblPayment = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnZaloPay = new Guna.UI2.WinForms.Guna2Button();
            this.btnMoMo = new Guna.UI2.WinForms.Guna2Button();
            this.btnVNPAY = new Guna.UI2.WinForms.Guna2Button();
            this.btnCash = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2Separator1 = new Guna.UI2.WinForms.Guna2Separator();
            this.picQR = new Guna.UI2.WinForms.Guna2PictureBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.guna2Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picQR)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2BorderlessForm1
            // 
            this.guna2BorderlessForm1.BorderRadius = 10;
            this.guna2BorderlessForm1.ContainerControl = this;
            this.guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6D;
            this.guna2BorderlessForm1.TransparentWhileDrag = true;
            // 
            // btnHuy
            // 
            this.btnHuy.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnHuy.Animated = true;
            this.btnHuy.BackColor = System.Drawing.Color.Transparent;
            this.btnHuy.BorderRadius = 10;
            this.btnHuy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHuy.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnHuy.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnHuy.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnHuy.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnHuy.FillColor = System.Drawing.Color.White;
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnHuy.ForeColor = System.Drawing.Color.Red;
            this.btnHuy.Location = new System.Drawing.Point(397, 13);
            this.btnHuy.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(124, 44);
            this.btnHuy.TabIndex = 92;
            this.btnHuy.Text = "Hủy";
            this.btnHuy.UseTransparentBackground = true;
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // btnComplete
            // 
            this.btnComplete.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnComplete.Animated = true;
            this.btnComplete.BackColor = System.Drawing.Color.Transparent;
            this.btnComplete.BorderRadius = 10;
            this.btnComplete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnComplete.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnComplete.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnComplete.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnComplete.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnComplete.FillColor = System.Drawing.Color.White;
            this.btnComplete.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnComplete.ForeColor = System.Drawing.Color.Lime;
            this.btnComplete.Location = new System.Drawing.Point(4, 13);
            this.btnComplete.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnComplete.Name = "btnComplete";
            this.btnComplete.Size = new System.Drawing.Size(165, 44);
            this.btnComplete.TabIndex = 91;
            this.btnComplete.Text = "Đã thanh toán";
            this.btnComplete.UseTransparentBackground = true;
            this.btnComplete.Click += new System.EventHandler(this.btnComplete_Click);
            // 
            // lblPayment
            // 
            this.lblPayment.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblPayment.BackColor = System.Drawing.Color.Transparent;
            this.lblPayment.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblPayment.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblPayment.Location = new System.Drawing.Point(2, 0);
            this.lblPayment.Margin = new System.Windows.Forms.Padding(2);
            this.lblPayment.Name = "lblPayment";
            this.lblPayment.Size = new System.Drawing.Size(248, 32);
            this.lblPayment.TabIndex = 93;
            this.lblPayment.Text = "Phương thức thanh toán";
            this.lblPayment.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.btnZaloPay, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnMoMo, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnVNPAY, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnCash, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 48);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(509, 168);
            this.tableLayoutPanel1.TabIndex = 99;
            // 
            // btnZaloPay
            // 
            this.btnZaloPay.Animated = true;
            this.btnZaloPay.BackColor = System.Drawing.Color.Transparent;
            this.btnZaloPay.BorderColor = System.Drawing.SystemColors.Control;
            this.btnZaloPay.BorderRadius = 7;
            this.btnZaloPay.BorderThickness = 1;
            this.btnZaloPay.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            this.btnZaloPay.CheckedState.BorderColor = System.Drawing.Color.MidnightBlue;
            this.btnZaloPay.CheckedState.FillColor = System.Drawing.Color.Transparent;
            this.btnZaloPay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnZaloPay.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnZaloPay.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnZaloPay.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnZaloPay.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnZaloPay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnZaloPay.FillColor = System.Drawing.Color.Transparent;
            this.btnZaloPay.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnZaloPay.ForeColor = System.Drawing.Color.Black;
            this.btnZaloPay.HoverState.BorderColor = System.Drawing.Color.Gray;
            this.btnZaloPay.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnZaloPay.Image = global::QuanLyChuoiCuaHangTrangSuc.Properties.Resources.icnZaloPay;
            this.btnZaloPay.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnZaloPay.ImageSize = new System.Drawing.Size(30, 30);
            this.btnZaloPay.Location = new System.Drawing.Point(4, 129);
            this.btnZaloPay.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnZaloPay.Name = "btnZaloPay";
            this.btnZaloPay.Size = new System.Drawing.Size(501, 36);
            this.btnZaloPay.TabIndex = 105;
            this.btnZaloPay.Text = "Thanh toán ZaloPay - QR đa năng";
            this.btnZaloPay.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnZaloPay.UseTransparentBackground = true;
            this.btnZaloPay.Click += new System.EventHandler(this.btnZaloPay_Click);
            // 
            // btnMoMo
            // 
            this.btnMoMo.Animated = true;
            this.btnMoMo.BackColor = System.Drawing.Color.Transparent;
            this.btnMoMo.BorderColor = System.Drawing.SystemColors.Control;
            this.btnMoMo.BorderRadius = 7;
            this.btnMoMo.BorderThickness = 1;
            this.btnMoMo.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            this.btnMoMo.CheckedState.BorderColor = System.Drawing.Color.MidnightBlue;
            this.btnMoMo.CheckedState.FillColor = System.Drawing.Color.Transparent;
            this.btnMoMo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMoMo.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnMoMo.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnMoMo.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnMoMo.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnMoMo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMoMo.FillColor = System.Drawing.Color.Transparent;
            this.btnMoMo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnMoMo.ForeColor = System.Drawing.Color.Black;
            this.btnMoMo.HoverState.BorderColor = System.Drawing.Color.Gray;
            this.btnMoMo.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnMoMo.Image = global::QuanLyChuoiCuaHangTrangSuc.Properties.Resources.icnMomo;
            this.btnMoMo.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnMoMo.ImageSize = new System.Drawing.Size(30, 30);
            this.btnMoMo.Location = new System.Drawing.Point(4, 87);
            this.btnMoMo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnMoMo.Name = "btnMoMo";
            this.btnMoMo.Size = new System.Drawing.Size(501, 36);
            this.btnMoMo.TabIndex = 104;
            this.btnMoMo.Text = "Thanh toán bằng Ví MoMo";
            this.btnMoMo.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnMoMo.UseTransparentBackground = true;
            this.btnMoMo.Click += new System.EventHandler(this.btnMoMo_Click);
            // 
            // btnVNPAY
            // 
            this.btnVNPAY.Animated = true;
            this.btnVNPAY.BackColor = System.Drawing.Color.Transparent;
            this.btnVNPAY.BorderColor = System.Drawing.SystemColors.Control;
            this.btnVNPAY.BorderRadius = 7;
            this.btnVNPAY.BorderThickness = 1;
            this.btnVNPAY.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            this.btnVNPAY.CheckedState.BorderColor = System.Drawing.Color.MidnightBlue;
            this.btnVNPAY.CheckedState.FillColor = System.Drawing.Color.Transparent;
            this.btnVNPAY.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVNPAY.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnVNPAY.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnVNPAY.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnVNPAY.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnVNPAY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnVNPAY.FillColor = System.Drawing.Color.Transparent;
            this.btnVNPAY.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnVNPAY.ForeColor = System.Drawing.Color.Black;
            this.btnVNPAY.HoverState.BorderColor = System.Drawing.Color.Gray;
            this.btnVNPAY.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnVNPAY.Image = global::QuanLyChuoiCuaHangTrangSuc.Properties.Resources.icnVPAYLogo1;
            this.btnVNPAY.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnVNPAY.ImageSize = new System.Drawing.Size(30, 30);
            this.btnVNPAY.Location = new System.Drawing.Point(4, 45);
            this.btnVNPAY.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnVNPAY.Name = "btnVNPAY";
            this.btnVNPAY.Size = new System.Drawing.Size(501, 36);
            this.btnVNPAY.TabIndex = 103;
            this.btnVNPAY.Text = "Thanh toán VNPAY";
            this.btnVNPAY.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnVNPAY.UseTransparentBackground = true;
            this.btnVNPAY.Click += new System.EventHandler(this.btnVNPAY_Click);
            // 
            // btnCash
            // 
            this.btnCash.Animated = true;
            this.btnCash.BackColor = System.Drawing.Color.Transparent;
            this.btnCash.BorderColor = System.Drawing.SystemColors.Control;
            this.btnCash.BorderRadius = 7;
            this.btnCash.BorderThickness = 1;
            this.btnCash.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            this.btnCash.CheckedState.BorderColor = System.Drawing.Color.MidnightBlue;
            this.btnCash.CheckedState.FillColor = System.Drawing.Color.Transparent;
            this.btnCash.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCash.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCash.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnCash.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnCash.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnCash.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCash.FillColor = System.Drawing.Color.Transparent;
            this.btnCash.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCash.ForeColor = System.Drawing.Color.Black;
            this.btnCash.HoverState.BorderColor = System.Drawing.Color.Gray;
            this.btnCash.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnCash.Image = global::QuanLyChuoiCuaHangTrangSuc.Properties.Resources.icnCash;
            this.btnCash.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnCash.ImageSize = new System.Drawing.Size(30, 30);
            this.btnCash.Location = new System.Drawing.Point(4, 3);
            this.btnCash.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCash.Name = "btnCash";
            this.btnCash.Size = new System.Drawing.Size(501, 36);
            this.btnCash.TabIndex = 102;
            this.btnCash.Text = "Thanh toán bằng tiền mặt";
            this.btnCash.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnCash.UseTransparentBackground = true;
            this.btnCash.Click += new System.EventHandler(this.btnCash_Click);
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.Controls.Add(this.lblPayment);
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2Panel1.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(525, 34);
            this.guna2Panel1.TabIndex = 100;
            // 
            // guna2Separator1
            // 
            this.guna2Separator1.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2Separator1.Location = new System.Drawing.Point(0, 34);
            this.guna2Separator1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.guna2Separator1.Name = "guna2Separator1";
            this.guna2Separator1.Size = new System.Drawing.Size(525, 10);
            this.guna2Separator1.TabIndex = 101;
            // 
            // picQR
            // 
            this.picQR.BackColor = System.Drawing.Color.Transparent;
            this.picQR.BorderRadius = 15;
            this.picQR.ImageRotate = 0F;
            this.picQR.Location = new System.Drawing.Point(136, 221);
            this.picQR.Name = "picQR";
            this.picQR.Size = new System.Drawing.Size(250, 250);
            this.picQR.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picQR.TabIndex = 102;
            this.picQR.TabStop = false;
            this.picQR.UseTransparentBackground = true;
            this.picQR.Visible = false;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.btnComplete, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnHuy, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 475);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(525, 70);
            this.tableLayoutPanel2.TabIndex = 103;
            // 
            // frmPayment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 545);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.picQR);
            this.Controls.Add(this.guna2Separator1);
            this.Controls.Add(this.guna2Panel1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "frmPayment";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "s";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picQR)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BorderlessForm1;
        private Guna.UI2.WinForms.Guna2Button btnHuy;
        private Guna.UI2.WinForms.Guna2Button btnComplete;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblPayment;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Guna.UI2.WinForms.Guna2Separator guna2Separator1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2Button btnCash;
        private Guna.UI2.WinForms.Guna2Button btnVNPAY;
        private Guna.UI2.WinForms.Guna2Button btnMoMo;
        private Guna.UI2.WinForms.Guna2Button btnZaloPay;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Guna.UI2.WinForms.Guna2PictureBox picQR;
    }
}