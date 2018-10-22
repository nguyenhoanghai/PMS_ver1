namespace QuanLyNangSuat
{
    partial class FrmReportNSLinesByHours
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
            this.chartControl1 = new DevExpress.XtraCharts.ChartControl();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.cbbSearchType = new System.Windows.Forms.ComboBox();
            this.cbbHours = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.butView = new System.Windows.Forms.Button();
            this.butExportExcel = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.chartControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1019, 364);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // chartControl1
            // 
            this.chartControl1.Location = new System.Drawing.Point(3, 53);
            this.chartControl1.Name = "chartControl1";
            this.chartControl1.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.chartControl1.Size = new System.Drawing.Size(300, 200);
            this.chartControl1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.cbbSearchType);
            this.flowLayoutPanel1.Controls.Add(this.cbbHours);
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
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1013, 44);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // cbbSearchType
            // 
            this.cbbSearchType.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbbSearchType.FormattingEnabled = true;
            this.cbbSearchType.Items.AddRange(new object[] {
            "Năng suất theo giờ",
            "Năng suất ngày"});
            this.cbbSearchType.Location = new System.Drawing.Point(3, 11);
            this.cbbSearchType.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.cbbSearchType.Name = "cbbSearchType";
            this.cbbSearchType.Size = new System.Drawing.Size(122, 21);
            this.cbbSearchType.TabIndex = 4;
            this.cbbSearchType.SelectedIndexChanged += new System.EventHandler(this.cbbSearchType_SelectedIndexChanged);
            // 
            // cbbHours
            // 
            this.cbbHours.FormattingEnabled = true;
            this.cbbHours.Location = new System.Drawing.Point(131, 10);
            this.cbbHours.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.cbbHours.Name = "cbbHours";
            this.cbbHours.Size = new System.Drawing.Size(145, 21);
            this.cbbHours.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(282, 14);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 14, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Chọn Ngày:";
            // 
            // dtpDate
            // 
            this.dtpDate.Location = new System.Drawing.Point(351, 10);
            this.dtpDate.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(200, 20);
            this.dtpDate.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(557, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 14, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Đến Ngày:";
            // 
            // dtpToDate
            // 
            this.dtpToDate.Location = new System.Drawing.Point(621, 10);
            this.dtpToDate.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(200, 20);
            this.dtpToDate.TabIndex = 3;
            // 
            // butView
            // 
            this.butView.Location = new System.Drawing.Point(827, 10);
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
            this.butExportExcel.Location = new System.Drawing.Point(908, 10);
            this.butExportExcel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.butExportExcel.Name = "butExportExcel";
            this.butExportExcel.Size = new System.Drawing.Size(75, 23);
            this.butExportExcel.TabIndex = 1;
            this.butExportExcel.Text = "Xuất excel";
            this.butExportExcel.UseVisualStyleBackColor = true;
            this.butExportExcel.Click += new System.EventHandler(this.butExportExcel_Click);
            // 
            // FrmReportNSLinesByHours
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1019, 364);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FrmReportNSLinesByHours";
            this.Text = "Năng Suất Của Các Chuyền";
            this.Load += new System.EventHandler(this.FrmReportNSClusterOfLine_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraCharts.ChartControl chartControl1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ComboBox cbbSearchType;
        private System.Windows.Forms.ComboBox cbbHours;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.Button butView;
        private System.Windows.Forms.Button butExportExcel;
    }
}