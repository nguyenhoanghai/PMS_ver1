namespace QuanLyNangSuat
{
    partial class FrmImportPCCFromExcel
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
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.STT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TenChuyen = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TenSanPham = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TGCheTaoSP = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SanLuongKeHoach = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Thang = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Nam = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.butSave = new System.Windows.Forms.Button();
            this.butCreateTemplateExcel = new System.Windows.Forms.Button();
            this.butChooseFile = new System.Windows.Forms.Button();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.gridControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(768, 311);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(3, 43);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(762, 265);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.STT,
            this.TenChuyen,
            this.TenSanPham,
            this.TGCheTaoSP,
            this.SanLuongKeHoach,
            this.Thang,
            this.Nam});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            // 
            // STT
            // 
            this.STT.Caption = "STT";
            this.STT.FieldName = "STTThucHien";
            this.STT.Name = "STT";
            this.STT.Visible = true;
            this.STT.VisibleIndex = 0;
            // 
            // TenChuyen
            // 
            this.TenChuyen.Caption = "Tên Chuyền";
            this.TenChuyen.FieldName = "TenChuyen";
            this.TenChuyen.Name = "TenChuyen";
            this.TenChuyen.Visible = true;
            this.TenChuyen.VisibleIndex = 1;
            // 
            // TenSanPham
            // 
            this.TenSanPham.Caption = "Tên Mặt Hàng";
            this.TenSanPham.FieldName = "TenSanPham";
            this.TenSanPham.Name = "TenSanPham";
            this.TenSanPham.Visible = true;
            this.TenSanPham.VisibleIndex = 2;
            // 
            // TGCheTaoSP
            // 
            this.TGCheTaoSP.Caption = "Thời Gian Chế Tạo";
            this.TGCheTaoSP.FieldName = "NangXuatSanXuat";
            this.TGCheTaoSP.Name = "TGCheTaoSP";
            this.TGCheTaoSP.Visible = true;
            this.TGCheTaoSP.VisibleIndex = 3;
            // 
            // SanLuongKeHoach
            // 
            this.SanLuongKeHoach.Caption = "Sản Lượng Kế Hoạch";
            this.SanLuongKeHoach.FieldName = "SanLuongKeHoach";
            this.SanLuongKeHoach.Name = "SanLuongKeHoach";
            this.SanLuongKeHoach.Visible = true;
            this.SanLuongKeHoach.VisibleIndex = 4;
            // 
            // Thang
            // 
            this.Thang.Caption = "Tháng";
            this.Thang.FieldName = "Thang";
            this.Thang.Name = "Thang";
            this.Thang.Visible = true;
            this.Thang.VisibleIndex = 5;
            // 
            // Nam
            // 
            this.Nam.Caption = "Năm";
            this.Nam.FieldName = "Nam";
            this.Nam.Name = "Nam";
            this.Nam.Visible = true;
            this.Nam.VisibleIndex = 6;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.butSave);
            this.panel1.Controls.Add(this.butCreateTemplateExcel);
            this.panel1.Controls.Add(this.butChooseFile);
            this.panel1.Controls.Add(this.txtFile);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(768, 40);
            this.panel1.TabIndex = 1;
            // 
            // butSave
            // 
            this.butSave.Location = new System.Drawing.Point(595, 7);
            this.butSave.Name = "butSave";
            this.butSave.Size = new System.Drawing.Size(152, 22);
            this.butSave.TabIndex = 1;
            this.butSave.Text = "Lưu thông tin phân công";
            this.butSave.UseVisualStyleBackColor = true;
            this.butSave.Click += new System.EventHandler(this.butSave_Click);
            // 
            // butCreateTemplateExcel
            // 
            this.butCreateTemplateExcel.Enabled = false;
            this.butCreateTemplateExcel.Location = new System.Drawing.Point(462, 8);
            this.butCreateTemplateExcel.Name = "butCreateTemplateExcel";
            this.butCreateTemplateExcel.Size = new System.Drawing.Size(120, 22);
            this.butCreateTemplateExcel.TabIndex = 1;
            this.butCreateTemplateExcel.Text = "Tạo file excel mẫu";
            this.butCreateTemplateExcel.UseVisualStyleBackColor = true;
            this.butCreateTemplateExcel.Click += new System.EventHandler(this.butCreateTemplateExcel_Click);
            // 
            // butChooseFile
            // 
            this.butChooseFile.Location = new System.Drawing.Point(350, 8);
            this.butChooseFile.Name = "butChooseFile";
            this.butChooseFile.Size = new System.Drawing.Size(100, 22);
            this.butChooseFile.TabIndex = 1;
            this.butChooseFile.Text = "Chọn file excel";
            this.butChooseFile.UseVisualStyleBackColor = true;
            this.butChooseFile.Click += new System.EventHandler(this.butChooseFile_Click);
            // 
            // txtFile
            // 
            this.txtFile.Enabled = false;
            this.txtFile.Location = new System.Drawing.Point(14, 10);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(322, 20);
            this.txtFile.TabIndex = 0;
            this.txtFile.Text = "Đường dẫn đến file excel...";
            // 
            // FrmImportPCCFromExcel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(768, 311);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FrmImportPCCFromExcel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmImportPCCFromExcel";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button butSave;
        private System.Windows.Forms.Button butCreateTemplateExcel;
        private System.Windows.Forms.Button butChooseFile;
        private System.Windows.Forms.TextBox txtFile;
        private DevExpress.XtraGrid.Columns.GridColumn STT;
        private DevExpress.XtraGrid.Columns.GridColumn TenChuyen;
        private DevExpress.XtraGrid.Columns.GridColumn TenSanPham;
        private DevExpress.XtraGrid.Columns.GridColumn TGCheTaoSP;
        private DevExpress.XtraGrid.Columns.GridColumn SanLuongKeHoach;
        private DevExpress.XtraGrid.Columns.GridColumn Nam;
        private DevExpress.XtraGrid.Columns.GridColumn Thang;
    }
}