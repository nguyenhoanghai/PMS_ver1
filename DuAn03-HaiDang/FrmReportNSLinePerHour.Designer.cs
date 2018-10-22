namespace QuanLyNangSuat
{
    partial class FrmReportNSLinePerHour
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

        #region
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private DevExpress.XtraEditors.SimpleButton btnExport;
        private DevExpress.XtraEditors.SimpleButton btnOpenFolder;

        private DevComponents.DotNetBar.Controls.DataGridViewX dgTTNangXuat;
        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.btnExport = new DevExpress.XtraEditors.SimpleButton();
            this.btnOpenFolder = new DevExpress.XtraEditors.SimpleButton();
            this.dgTTNangXuat = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.dgchuyen = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.dgsolaodong = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.dgsanpham = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.dgkehoach = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.dgluykethuchen = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.dgdoanhthungay = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.dgdoanhthuthang = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.dgBQ = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.dgluykebtp = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.dgbtptrenchuyen = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.dgbtpngay = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.dgdinhmucngay = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.dgDinhMucGio = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.dgThucHienNgay = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.dgThoatChuyenNgay = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.dgPhanTramRCTrenMucKhoan = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.dgPhanTramKĐTrenMucKhoan = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.dgLoiTrenKQT = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.dgnhipchuyen = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgTTNangXuat)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dgTTNangXuat, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1206, 447);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.dtpDate);
            this.flowLayoutPanel1.Controls.Add(this.btnExport);
            this.flowLayoutPanel1.Controls.Add(this.btnOpenFolder);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1200, 44);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 13);
            this.label1.Margin = new System.Windows.Forms.Padding(10, 13, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Chọn Ngày ";
            // 
            // dtpDate
            // 
            this.dtpDate.CustomFormat = "dd/MM/yyyy";
            this.dtpDate.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDate.Location = new System.Drawing.Point(110, 10);
            this.dtpDate.Margin = new System.Windows.Forms.Padding(5, 10, 3, 3);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(126, 26);
            this.dtpDate.TabIndex = 1;
            this.dtpDate.ValueChanged += new System.EventHandler(this.dtpDate_ValueChanged);
            // 
            // btnExport
            // 
            this.btnExport.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Appearance.Options.UseFont = true;
            this.btnExport.Image = global::QuanLyNangSuat.Properties.Resources.excel;
            this.btnExport.Location = new System.Drawing.Point(246, 7);
            this.btnExport.Margin = new System.Windows.Forms.Padding(7);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(120, 32);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "Xuất Excel";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenFolder.Appearance.Options.UseFont = true;
            this.btnOpenFolder.Image = global::QuanLyNangSuat.Properties.Resources._1463126583_folder_16;
            this.btnOpenFolder.Location = new System.Drawing.Point(380, 7);
            this.btnOpenFolder.Margin = new System.Windows.Forms.Padding(7);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new System.Drawing.Size(153, 32);
            this.btnOpenFolder.TabIndex = 4;
            this.btnOpenFolder.Text = "Mở thư mục Lưu";
            this.btnOpenFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);
            // 
            // dgTTNangXuat
            // 
            this.dgTTNangXuat.AllowUserToAddRows = false;
            this.dgTTNangXuat.AllowUserToDeleteRows = false;
            this.dgTTNangXuat.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgTTNangXuat.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Tahoma", 8.25F);
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgTTNangXuat.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgTTNangXuat.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgTTNangXuat.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgchuyen,
            this.dgsolaodong,
            this.dgsanpham,
            this.dgkehoach,
            this.dgluykethuchen,
            this.dgdoanhthungay,
            this.dgdoanhthuthang,
            this.dgBQ,
            this.dgluykebtp,
            this.dgbtptrenchuyen,
            this.dgbtpngay,
            this.dgdinhmucngay,
            this.dgDinhMucGio,
            this.dgThucHienNgay,
            this.dgThoatChuyenNgay,
            this.dgPhanTramRCTrenMucKhoan,
            this.dgPhanTramKĐTrenMucKhoan,
            this.dgLoiTrenKQT,
            this.dgnhipchuyen});
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Tahoma", 8.25F);
            dataGridViewCellStyle11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgTTNangXuat.DefaultCellStyle = dataGridViewCellStyle11;
            this.dgTTNangXuat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgTTNangXuat.EnableHeadersVisualStyles = false;
            this.dgTTNangXuat.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgTTNangXuat.Location = new System.Drawing.Point(0, 50);
            this.dgTTNangXuat.Margin = new System.Windows.Forms.Padding(0);
            this.dgTTNangXuat.Name = "dgTTNangXuat";
            this.dgTTNangXuat.ReadOnly = true;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Tahoma", 8.25F);
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgTTNangXuat.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.dgTTNangXuat.Size = new System.Drawing.Size(1206, 397);
            this.dgTTNangXuat.TabIndex = 0;
            // 
            // dgchuyen
            // 
            this.dgchuyen.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgchuyen.HeaderText = "Chuyền";
            this.dgchuyen.Name = "dgchuyen";
            this.dgchuyen.ReadOnly = true;
            this.dgchuyen.TextAlignment = System.Drawing.StringAlignment.Center;
            this.dgchuyen.WordWrap = true;
            // 
            // dgsolaodong
            // 
            this.dgsolaodong.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgsolaodong.HeaderText = "Lao động (TT/ĐB)";
            this.dgsolaodong.Name = "dgsolaodong";
            this.dgsolaodong.ReadOnly = true;
            // 
            // dgsanpham
            // 
            this.dgsanpham.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgsanpham.HeaderText = "Mã hàng";
            this.dgsanpham.Name = "dgsanpham";
            this.dgsanpham.ReadOnly = true;
            this.dgsanpham.Width = 49;
            // 
            // dgkehoach
            // 
            this.dgkehoach.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgkehoach.DefaultCellStyle = dataGridViewCellStyle10;
            this.dgkehoach.HeaderText = "SL Kế hoạch";
            this.dgkehoach.Name = "dgkehoach";
            this.dgkehoach.ReadOnly = true;
            this.dgkehoach.TextAlignment = System.Drawing.StringAlignment.Center;
            this.dgkehoach.WordWrap = true;
            // 
            // dgluykethuchen
            // 
            this.dgluykethuchen.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgluykethuchen.HeaderText = "Lũy Kế TH";
            this.dgluykethuchen.Name = "dgluykethuchen";
            this.dgluykethuchen.ReadOnly = true;
            this.dgluykethuchen.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgluykethuchen.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dgdoanhthungay
            // 
            this.dgdoanhthungay.HeaderText = "Doanh thu ngày";
            this.dgdoanhthungay.Name = "dgdoanhthungay";
            this.dgdoanhthungay.ReadOnly = true;
            this.dgdoanhthungay.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgdoanhthungay.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dgdoanhthuthang
            // 
            this.dgdoanhthuthang.HeaderText = "Doanh thu tháng";
            this.dgdoanhthuthang.Name = "dgdoanhthuthang";
            this.dgdoanhthuthang.ReadOnly = true;
            // 
            // dgBQ
            // 
            this.dgBQ.HeaderText = "Thu Nhập BQ";
            this.dgBQ.Name = "dgBQ";
            this.dgBQ.ReadOnly = true;
            this.dgBQ.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgBQ.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dgluykebtp
            // 
            this.dgluykebtp.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgluykebtp.HeaderText = "Lũy Kế BTP";
            this.dgluykebtp.Name = "dgluykebtp";
            this.dgluykebtp.ReadOnly = true;
            // 
            // dgbtptrenchuyen
            // 
            this.dgbtptrenchuyen.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgbtptrenchuyen.HeaderText = "Vốn / BTP trên chuyền";
            this.dgbtptrenchuyen.Name = "dgbtptrenchuyen";
            this.dgbtptrenchuyen.ReadOnly = true;
            // 
            // dgbtpngay
            // 
            this.dgbtpngay.HeaderText = "BTP Ngày";
            this.dgbtpngay.Name = "dgbtpngay";
            this.dgbtpngay.ReadOnly = true;
            // 
            // dgdinhmucngay
            // 
            this.dgdinhmucngay.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgdinhmucngay.HeaderText = "Định mức ngày";
            this.dgdinhmucngay.Name = "dgdinhmucngay";
            this.dgdinhmucngay.ReadOnly = true;
            // 
            // dgDinhMucGio
            // 
            this.dgDinhMucGio.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgDinhMucGio.HeaderText = "Định mức giờ";
            this.dgDinhMucGio.Name = "dgDinhMucGio";
            this.dgDinhMucGio.ReadOnly = true;
            // 
            // dgThucHienNgay
            // 
            this.dgThucHienNgay.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgThucHienNgay.HeaderText = "Thực Hiện Ngày";
            this.dgThucHienNgay.Name = "dgThucHienNgay";
            this.dgThucHienNgay.ReadOnly = true;
            // 
            // dgThoatChuyenNgay
            // 
            this.dgThoatChuyenNgay.HeaderText = "Thoát Chuyền Ngày";
            this.dgThoatChuyenNgay.Name = "dgThoatChuyenNgay";
            this.dgThoatChuyenNgay.ReadOnly = true;
            // 
            // dgPhanTramRCTrenMucKhoan
            // 
            this.dgPhanTramRCTrenMucKhoan.HeaderText = "Tỷ lệ % RC / Mức Khoán";
            this.dgPhanTramRCTrenMucKhoan.Name = "dgPhanTramRCTrenMucKhoan";
            this.dgPhanTramRCTrenMucKhoan.ReadOnly = true;
            this.dgPhanTramRCTrenMucKhoan.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgPhanTramRCTrenMucKhoan.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dgPhanTramKĐTrenMucKhoan
            // 
            this.dgPhanTramKĐTrenMucKhoan.HeaderText = "Tỷ lệ % KĐ / Mức khoán";
            this.dgPhanTramKĐTrenMucKhoan.Name = "dgPhanTramKĐTrenMucKhoan";
            this.dgPhanTramKĐTrenMucKhoan.ReadOnly = true;
            this.dgPhanTramKĐTrenMucKhoan.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // dgLoiTrenKQT
            // 
            this.dgLoiTrenKQT.HeaderText = "Tỷ lệ % lỗi / Kiểm qua tay";
            this.dgLoiTrenKQT.Name = "dgLoiTrenKQT";
            this.dgLoiTrenKQT.ReadOnly = true;
            // 
            // dgnhipchuyen
            // 
            this.dgnhipchuyen.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgnhipchuyen.HeaderText = "Nhịp chuyền (TT / NC)";
            this.dgnhipchuyen.Name = "dgnhipchuyen";
            this.dgnhipchuyen.ReadOnly = true;
            // 
            // FrmReportNSLinePerHour
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1206, 447);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FrmReportNSLinePerHour";
            this.Text = "Báo cáo thông tin năng suất tổng hợp";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgTTNangXuat)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn dgchuyen;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn dgsolaodong;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn dgsanpham;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn dgkehoach;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn dgluykethuchen;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn dgdoanhthungay;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn dgdoanhthuthang;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn dgBQ;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn dgluykebtp;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn dgbtptrenchuyen;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn dgbtpngay;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn dgdinhmucngay;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn dgDinhMucGio;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn dgThucHienNgay;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn dgThoatChuyenNgay;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn dgPhanTramRCTrenMucKhoan;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn dgPhanTramKĐTrenMucKhoan;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn dgLoiTrenKQT;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn dgnhipchuyen;



    }
}