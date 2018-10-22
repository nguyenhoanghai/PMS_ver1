namespace QuanLyNangSuat
{
    partial class FrmReportTableNSLine
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.butView = new System.Windows.Forms.Button();
            this.butExportExcel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Chuyen = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ThucHien = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DinhMuc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DoanhThuThucHien = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DoanhThuDinhMuc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SoLoi = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.gridControl1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(716, 413);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label3);
            this.flowLayoutPanel1.Controls.Add(this.dtpDate);
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.dtpToDate);
            this.flowLayoutPanel1.Controls.Add(this.butView);
            this.flowLayoutPanel1.Controls.Add(this.butExportExcel);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.MinimumSize = new System.Drawing.Size(993, 44);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(993, 44);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // dtpDate
            // 
            this.dtpDate.Location = new System.Drawing.Point(72, 10);
            this.dtpDate.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(200, 20);
            this.dtpDate.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(278, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 14, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Đến Ngày:";
            // 
            // dtpToDate
            // 
            this.dtpToDate.Location = new System.Drawing.Point(342, 10);
            this.dtpToDate.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(200, 20);
            this.dtpToDate.TabIndex = 3;
            // 
            // butView
            // 
            this.butView.Location = new System.Drawing.Point(548, 10);
            this.butView.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.butView.Name = "butView";
            this.butView.Size = new System.Drawing.Size(75, 23);
            this.butView.TabIndex = 1;
            this.butView.Text = "Xem";
            this.butView.UseVisualStyleBackColor = true;
            this.butView.Click += new System.EventHandler(this.butView_Click);
            // 
            // butExportExcel
            // 
            this.butExportExcel.Location = new System.Drawing.Point(629, 10);
            this.butExportExcel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.butExportExcel.Name = "butExportExcel";
            this.butExportExcel.Size = new System.Drawing.Size(75, 23);
            this.butExportExcel.TabIndex = 1;
            this.butExportExcel.Text = "Xuất excel";
            this.butExportExcel.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 14);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 14, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Chọn Ngày:";
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(3, 53);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(710, 357);
            this.gridControl1.TabIndex = 4;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Chuyen,
            this.ThucHien,
            this.DinhMuc,
            this.DoanhThuThucHien,
            this.DoanhThuDinhMuc,
            this.SoLoi});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            // 
            // Chuyen
            // 
            this.Chuyen.Caption = "Chuyền";
            this.Chuyen.FieldName = "Chuyen";
            this.Chuyen.Name = "Chuyen";
            this.Chuyen.Visible = true;
            this.Chuyen.VisibleIndex = 0;
            // 
            // ThucHien
            // 
            this.ThucHien.Caption = "Sản Lượng Thực Hiện";
            this.ThucHien.FieldName = "ThucHien";
            this.ThucHien.Name = "ThucHien";
            this.ThucHien.Visible = true;
            this.ThucHien.VisibleIndex = 1;
            // 
            // DinhMuc
            // 
            this.DinhMuc.Caption = "Sản Lượng Định Mức";
            this.DinhMuc.FieldName = "DinhMuc";
            this.DinhMuc.Name = "DinhMuc";
            this.DinhMuc.Visible = true;
            this.DinhMuc.VisibleIndex = 2;
            // 
            // DoanhThuThucHien
            // 
            this.DoanhThuThucHien.Caption = "Doanh Thu Thực Hiện";
            this.DoanhThuThucHien.FieldName = "DoanhThuThucHien";
            this.DoanhThuThucHien.Name = "DoanhThuThucHien";
            this.DoanhThuThucHien.Visible = true;
            this.DoanhThuThucHien.VisibleIndex = 3;
            // 
            // DoanhThuDinhMuc
            // 
            this.DoanhThuDinhMuc.Caption = "Doanh Thu Định Mức";
            this.DoanhThuDinhMuc.FieldName = "DoanhThuDinhMuc";
            this.DoanhThuDinhMuc.Name = "DoanhThuDinhMuc";
            this.DoanhThuDinhMuc.Visible = true;
            this.DoanhThuDinhMuc.VisibleIndex = 4;
            // 
            // SoLoi
            // 
            this.SoLoi.Caption = "Số Lỗi";
            this.SoLoi.FieldName = "SoLoi";
            this.SoLoi.Name = "SoLoi";
            this.SoLoi.Visible = true;
            this.SoLoi.VisibleIndex = 5;
            // 
            // FrmReportTableNSLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 413);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FrmReportTableNSLine";
            this.Text = "Báo Cáo Năng Suất Chuyền";
            this.Load += new System.EventHandler(this.FrmReportTableNSLine_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.Button butView;
        private System.Windows.Forms.Button butExportExcel;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn Chuyen;
        private DevExpress.XtraGrid.Columns.GridColumn ThucHien;
        private DevExpress.XtraGrid.Columns.GridColumn DinhMuc;
        private DevExpress.XtraGrid.Columns.GridColumn DoanhThuThucHien;
        private DevExpress.XtraGrid.Columns.GridColumn DoanhThuDinhMuc;
        private DevExpress.XtraGrid.Columns.GridColumn SoLoi;
    }
}