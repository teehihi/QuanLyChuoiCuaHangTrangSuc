namespace QuanLyChuoiCuaHangTrangSuc
{
    partial class frmSupplier
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSupplier));
            this.guna2AnimateWindow1 = new Guna.UI2.WinForms.Guna2AnimateWindow(this.components);
            this.lblLogOut = new System.Windows.Forms.LinkLabel();
            this.guna2DragControl2 = new Guna.UI2.WinForms.Guna2DragControl(this.components);
            this.panelTop = new Guna.UI2.WinForms.Guna2Panel();
            this.btnMaximize = new Guna.UI2.WinForms.Guna2ControlBox();
            this.btnClose = new Guna.UI2.WinForms.Guna2ControlBox();
            this.btnMinimize = new Guna.UI2.WinForms.Guna2ControlBox();
            this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.panelLeft = new Guna.UI2.WinForms.Guna2GradientPanel();
            this.panelMid = new Guna.UI2.WinForms.Guna2Panel();
            this.panelTop.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2AnimateWindow1
            // 
            this.guna2AnimateWindow1.AnimationType = Guna.UI2.WinForms.Guna2AnimateWindow.AnimateWindowType.AW_ACTIVATE;
            this.guna2AnimateWindow1.Interval = 100;
            this.guna2AnimateWindow1.TargetForm = this;
            // 
            // lblLogOut
            // 
            this.lblLogOut.ActiveLinkColor = System.Drawing.Color.White;
            this.lblLogOut.AutoSize = true;
            this.lblLogOut.BackColor = System.Drawing.Color.Transparent;
            this.lblLogOut.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblLogOut.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogOut.LinkColor = System.Drawing.Color.White;
            this.lblLogOut.Location = new System.Drawing.Point(124, 659);
            this.lblLogOut.Name = "lblLogOut";
            this.lblLogOut.Size = new System.Drawing.Size(61, 13);
            this.lblLogOut.TabIndex = 2;
            this.lblLogOut.TabStop = true;
            this.lblLogOut.Text = "Đăng xuất";
            this.lblLogOut.VisitedLinkColor = System.Drawing.Color.White;
            this.lblLogOut.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblLogOut_LinkClicked);
            // 
            // guna2DragControl2
            // 
            this.guna2DragControl2.DockIndicatorTransparencyValue = 0.6D;
            this.guna2DragControl2.TargetControl = this.panelTop;
            this.guna2DragControl2.UseTransparentDrag = true;
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.btnMaximize);
            this.panelTop.Controls.Add(this.btnClose);
            this.panelTop.Controls.Add(this.btnMinimize);
            this.panelTop.Cursor = System.Windows.Forms.Cursors.Default;
            this.panelTop.CustomBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(123)))), ((int)(((byte)(129)))));
            this.panelTop.CustomBorderThickness = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1264, 25);
            this.panelTop.TabIndex = 14;
            // 
            // btnMaximize
            // 
            this.btnMaximize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMaximize.Animated = true;
            this.btnMaximize.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MaximizeBox;
            this.btnMaximize.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMaximize.FillColor = System.Drawing.Color.Transparent;
            this.btnMaximize.IconColor = System.Drawing.Color.White;
            this.btnMaximize.Location = new System.Drawing.Point(1208, -1);
            this.btnMaximize.Name = "btnMaximize";
            this.btnMaximize.Size = new System.Drawing.Size(25, 25);
            this.btnMaximize.TabIndex = 4;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Animated = true;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.FillColor = System.Drawing.Color.Transparent;
            this.btnClose.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(93)))), ((int)(((byte)(211)))));
            this.btnClose.IconColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(1239, -1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(25, 25);
            this.btnClose.TabIndex = 2;
            // 
            // btnMinimize
            // 
            this.btnMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimize.Animated = true;
            this.btnMinimize.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MinimizeBox;
            this.btnMinimize.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMinimize.FillColor = System.Drawing.Color.Transparent;
            this.btnMinimize.IconColor = System.Drawing.Color.White;
            this.btnMinimize.Location = new System.Drawing.Point(1177, -1);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(25, 25);
            this.btnMinimize.TabIndex = 3;
            // 
            // guna2Elipse1
            // 
            this.guna2Elipse1.BorderRadius = 15;
            this.guna2Elipse1.TargetControl = this;
            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(38)))), ((int)(((byte)(47)))));
            this.panelLeft.BorderRadius = 20;
            this.panelLeft.Controls.Add(this.lblLogOut);
            this.panelLeft.CustomBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(123)))), ((int)(((byte)(129)))));
            this.panelLeft.CustomBorderThickness = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.panelLeft.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
            this.panelLeft.Location = new System.Drawing.Point(-20, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(75, 700);
            this.panelLeft.TabIndex = 13;
            // 
            // panelMid
            // 
            this.panelMid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMid.Location = new System.Drawing.Point(0, 0);
            this.panelMid.Name = "panelMid";
            this.panelMid.Size = new System.Drawing.Size(1264, 681);
            this.panelMid.TabIndex = 15;
            // 
            // frmSupplier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(32)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelLeft);
            this.Controls.Add(this.panelMid);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "frmSupplier";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản lý nhà cung cấp";
            this.Load += new System.EventHandler(this.frmSupplier_Load);
            this.panelTop.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.panelLeft.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Guna.UI2.WinForms.Guna2AnimateWindow guna2AnimateWindow1;
        private System.Windows.Forms.LinkLabel lblLogOut;
        private Guna.UI2.WinForms.Guna2DragControl guna2DragControl2;
        private Guna.UI2.WinForms.Guna2Panel panelTop;
        private Guna.UI2.WinForms.Guna2ControlBox btnMaximize;
        private Guna.UI2.WinForms.Guna2ControlBox btnClose;
        private Guna.UI2.WinForms.Guna2ControlBox btnMinimize;
        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
        private Guna.UI2.WinForms.Guna2GradientPanel panelLeft;
        private Guna.UI2.WinForms.Guna2Panel panelMid;
    }
}