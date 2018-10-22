namespace QuanLyNangSuat
{
    partial class frmMainComplete
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMainComplete));
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barLargeButtonItem5 = new DevExpress.XtraBars.BarLargeButtonItem();
            this.btnCompletionPhase = new DevExpress.XtraBars.BarLargeButtonItem();
            this.btnAssignment = new DevExpress.XtraBars.BarLargeButtonItem();
            this.btnInsertQuality = new DevExpress.XtraBars.BarLargeButtonItem();
            this.btnExit = new DevExpress.XtraBars.BarLargeButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barLargeButtonItem1 = new DevExpress.XtraBars.BarLargeButtonItem();
            this.barLargeButtonItem4 = new DevExpress.XtraBars.BarLargeButtonItem();
            this.barLargeButtonItem2 = new DevExpress.XtraBars.BarLargeButtonItem();
            this.barLargeButtonItem3 = new DevExpress.XtraBars.BarLargeButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnCompletionPhase,
            this.btnAssignment,
            this.btnInsertQuality,
            this.btnExit,
            this.barLargeButtonItem1,
            this.barLargeButtonItem4,
            this.barLargeButtonItem5});
            this.barManager1.MaxItemId = 7;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barLargeButtonItem5),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnCompletionPhase),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnAssignment),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnInsertQuality),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnExit)});
            this.bar1.Text = "Tools";
            // 
            // barLargeButtonItem5
            // 
            this.barLargeButtonItem5.Caption = "Mã Hàng";
            this.barLargeButtonItem5.Id = 6;
            this.barLargeButtonItem5.LargeGlyph = global::QuanLyNangSuat.Properties.Resources.info;
            this.barLargeButtonItem5.Name = "barLargeButtonItem5";
            this.barLargeButtonItem5.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barLargeButtonItem5_ItemClick);
            // 
            // btnCompletionPhase
            // 
            this.btnCompletionPhase.Caption = "Quản lý Công Đoạn";
            this.btnCompletionPhase.Id = 0;
            this.btnCompletionPhase.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("btnCompletionPhase.LargeGlyph")));
            this.btnCompletionPhase.Name = "btnCompletionPhase";
            this.btnCompletionPhase.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCompletionPhase_ItemClick);
            // 
            // btnAssignment
            // 
            this.btnAssignment.Caption = "Phân Hàng Sản Xuất";
            this.btnAssignment.Id = 1;
            this.btnAssignment.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("btnAssignment.LargeGlyph")));
            this.btnAssignment.Name = "btnAssignment";
            this.btnAssignment.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAssignment_ItemClick);
            // 
            // btnInsertQuality
            // 
            this.btnInsertQuality.Caption = "Nhập Sản Lượng";
            this.btnInsertQuality.Id = 2;
            this.btnInsertQuality.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("btnInsertQuality.LargeGlyph")));
            this.btnInsertQuality.Name = "btnInsertQuality";
            this.btnInsertQuality.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnInsertQuality_ItemClick);
            // 
            // btnExit
            // 
            this.btnExit.Caption = "Thoát";
            this.btnExit.Id = 3;
            this.btnExit.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("btnExit.LargeGlyph")));
            this.btnExit.Name = "btnExit";
            this.btnExit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnExit_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(1258, 97);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 378);
            this.barDockControlBottom.Size = new System.Drawing.Size(1258, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 97);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 281);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1258, 97);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 281);
            // 
            // barLargeButtonItem1
            // 
            this.barLargeButtonItem1.Caption = "Mã Hàng";
            this.barLargeButtonItem1.Glyph = global::QuanLyNangSuat.Properties.Resources._1467720683_pie_chart;
            this.barLargeButtonItem1.Id = 4;
            this.barLargeButtonItem1.Name = "barLargeButtonItem1";
            // 
            // barLargeButtonItem4
            // 
            this.barLargeButtonItem4.Caption = "Mã Hang";
            this.barLargeButtonItem4.Id = 5;
            this.barLargeButtonItem4.Name = "barLargeButtonItem4";
            this.barLargeButtonItem4.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barLargeButtonItem4_ItemClick);
            // 
            // barLargeButtonItem2
            // 
            this.barLargeButtonItem2.Caption = "Nhập Sản Lượng";
            this.barLargeButtonItem2.Id = 2;
            this.barLargeButtonItem2.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barLargeButtonItem2.LargeGlyph")));
            this.barLargeButtonItem2.Name = "barLargeButtonItem2";
            // 
            // barLargeButtonItem3
            // 
            this.barLargeButtonItem3.Caption = "Nhập Sản Lượng";
            this.barLargeButtonItem3.Id = 2;
            this.barLargeButtonItem3.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barLargeButtonItem3.LargeGlyph")));
            this.barLargeButtonItem3.Name = "barLargeButtonItem3";
            // 
            // frmMainComplete
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1258, 378);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.IsMdiContainer = true;
            this.Name = "frmMainComplete";
            this.Text = "QUẢN LÝ NĂNG SUẤT 7.8";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMainComplete_FormClosing);
            this.Load += new System.EventHandler(this.frmMainComplete_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarLargeButtonItem btnAssignment;
        private DevExpress.XtraBars.BarLargeButtonItem btnCompletionPhase;
        private DevExpress.XtraBars.BarLargeButtonItem btnInsertQuality;
        private DevExpress.XtraBars.BarLargeButtonItem btnExit;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarLargeButtonItem barLargeButtonItem4;
        private DevExpress.XtraBars.BarLargeButtonItem barLargeButtonItem1;
        private DevExpress.XtraBars.BarLargeButtonItem barLargeButtonItem2;
        private DevExpress.XtraBars.BarLargeButtonItem barLargeButtonItem3;
        private DevExpress.XtraBars.BarLargeButtonItem barLargeButtonItem5;

    }
}