namespace DuAn03_HaiDang
{
    partial class frmPhaHangChoChuyen
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
            System.Windows.Forms.PictureBox btnReLine;
            System.Windows.Forms.PictureBox btnReCommo;
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.butImportFromExcel = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtSanLuongKeHoach = new System.Windows.Forms.NumericUpDown();
            this.txtSTTThucHien = new System.Windows.Forms.NumericUpDown();
            this.lueSanPham = new DevExpress.XtraEditors.LookUpEdit();
            this.cbbYear = new System.Windows.Forms.ComboBox();
            this.cbbMorth = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbSanLuongKeHoach = new System.Windows.Forms.Label();
            this.cbbChuyen = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.STTThucHien = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TenSanPham = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.SanLuongKeHoach = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LuyKeTH = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Thang = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Nam = new DevExpress.XtraGrid.Columns.GridColumn();
            this.STT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.IsFinish = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemMemoEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            btnReLine = new System.Windows.Forms.PictureBox();
            btnReCommo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(btnReLine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(btnReCommo)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSanLuongKeHoach)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSTTThucHien)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueSanPham.Properties)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit2)).BeginInit();
            this.SuspendLayout();
            // 
            // btnReLine
            // 
            btnReLine.BackgroundImage = global::QuanLyNangSuat.Properties.Resources.refresh;
            btnReLine.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            btnReLine.Cursor = System.Windows.Forms.Cursors.Hand;
            btnReLine.ImageLocation = "";
            btnReLine.Location = new System.Drawing.Point(376, 87);
            btnReLine.Name = "btnReLine";
            btnReLine.Size = new System.Drawing.Size(30, 27);
            btnReLine.TabIndex = 53;
            btnReLine.TabStop = false;
            btnReLine.Click += new System.EventHandler(this.btnReLine_Click);
            // 
            // btnReCommo
            // 
            btnReCommo.BackgroundImage = global::QuanLyNangSuat.Properties.Resources.refresh;
            btnReCommo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            btnReCommo.Cursor = System.Windows.Forms.Cursors.Hand;
            btnReCommo.ImageLocation = "";
            btnReCommo.Location = new System.Drawing.Point(376, 131);
            btnReCommo.Name = "btnReCommo";
            btnReCommo.Size = new System.Drawing.Size(30, 27);
            btnReCommo.TabIndex = 54;
            btnReCommo.TabStop = false;
            btnReCommo.Click += new System.EventHandler(this.btnReCommo_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(btnReCommo);
            this.groupBox1.Controls.Add(btnReLine);
            this.groupBox1.Controls.Add(this.butImportFromExcel);
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.btnUpdate);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.txtSanLuongKeHoach);
            this.groupBox1.Controls.Add(this.txtSTTThucHien);
            this.groupBox1.Controls.Add(this.lueSanPham);
            this.groupBox1.Controls.Add(this.cbbYear);
            this.groupBox1.Controls.Add(this.cbbMorth);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lbSanLuongKeHoach);
            this.groupBox1.Controls.Add(this.cbbChuyen);
            this.groupBox1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(433, 533);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thông tin chung";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // butImportFromExcel
            // 
            this.butImportFromExcel.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butImportFromExcel.Image = global::QuanLyNangSuat.Properties.Resources.excel;
            this.butImportFromExcel.Location = new System.Drawing.Point(61, 442);
            this.butImportFromExcel.Name = "butImportFromExcel";
            this.butImportFromExcel.Padding = new System.Windows.Forms.Padding(60, 0, 0, 0);
            this.butImportFromExcel.Size = new System.Drawing.Size(298, 40);
            this.butImportFromExcel.TabIndex = 13;
            this.butImportFromExcel.Text = "Nhập từ excel";
            this.butImportFromExcel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.butImportFromExcel.UseVisualStyleBackColor = true;
            this.butImportFromExcel.Click += new System.EventHandler(this.butImportFromExcel_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Image = global::QuanLyNangSuat.Properties.Resources.deletee;
            this.btnDelete.Location = new System.Drawing.Point(61, 391);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnDelete.Size = new System.Drawing.Size(140, 40);
            this.btnDelete.TabIndex = 15;
            this.btnDelete.Text = "Xoá";
            this.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_s_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Image = global::QuanLyNangSuat.Properties.Resources._1464012624_Cancel;
            this.btnCancel.Location = new System.Drawing.Point(219, 391);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnCancel.Size = new System.Drawing.Size(140, 40);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Huỷ bỏ";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_s_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Enabled = false;
            this.btnUpdate.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.Image = global::QuanLyNangSuat.Properties.Resources.edit;
            this.btnUpdate.Location = new System.Drawing.Point(219, 340);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnUpdate.Size = new System.Drawing.Size(140, 40);
            this.btnUpdate.TabIndex = 17;
            this.btnUpdate.Text = "Cập nhật";
            this.btnUpdate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_s_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Image = global::QuanLyNangSuat.Properties.Resources.add;
            this.btnAdd.Location = new System.Drawing.Point(61, 340);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnAdd.Size = new System.Drawing.Size(140, 40);
            this.btnAdd.TabIndex = 18;
            this.btnAdd.Text = "Thêm";
            this.btnAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_s_Click);
            // 
            // txtSanLuongKeHoach
            // 
            this.txtSanLuongKeHoach.Location = new System.Drawing.Point(146, 177);
            this.txtSanLuongKeHoach.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.txtSanLuongKeHoach.Name = "txtSanLuongKeHoach";
            this.txtSanLuongKeHoach.Size = new System.Drawing.Size(133, 26);
            this.txtSanLuongKeHoach.TabIndex = 14;
            this.txtSanLuongKeHoach.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // txtSTTThucHien
            // 
            this.txtSTTThucHien.Location = new System.Drawing.Point(146, 44);
            this.txtSTTThucHien.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.txtSTTThucHien.Name = "txtSTTThucHien";
            this.txtSTTThucHien.Size = new System.Drawing.Size(77, 26);
            this.txtSTTThucHien.TabIndex = 12;
            // 
            // lueSanPham
            // 
            this.lueSanPham.Location = new System.Drawing.Point(146, 132);
            this.lueSanPham.Name = "lueSanPham";
            this.lueSanPham.Properties.Appearance.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lueSanPham.Properties.Appearance.Options.UseFont = true;
            this.lueSanPham.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueSanPham.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("MaSanPham", "Mã Mặt Hàng", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("TenSanPham", "Tên Mặt Hàng", 30, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Center, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.True),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("DonGiaCM", "Đơn giá CM", 30, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Center),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("DonGia", "Đơn giá", 30, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Center),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("DinhNghia", "Mô tả", 40, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Center)});
            this.lueSanPham.Properties.PopupFormMinSize = new System.Drawing.Size(300, 300);
            this.lueSanPham.Size = new System.Drawing.Size(224, 26);
            this.lueSanPham.TabIndex = 2;
            // 
            // cbbYear
            // 
            this.cbbYear.FormattingEnabled = true;
            this.cbbYear.Location = new System.Drawing.Point(267, 221);
            this.cbbYear.Name = "cbbYear";
            this.cbbYear.Size = new System.Drawing.Size(103, 27);
            this.cbbYear.TabIndex = 7;
            // 
            // cbbMorth
            // 
            this.cbbMorth.FormattingEnabled = true;
            this.cbbMorth.Location = new System.Drawing.Point(146, 221);
            this.cbbMorth.Name = "cbbMorth";
            this.cbbMorth.Size = new System.Drawing.Size(115, 27);
            this.cbbMorth.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.MediumBlue;
            this.label3.Location = new System.Drawing.Point(27, 221);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 19);
            this.label3.TabIndex = 11;
            this.label3.Text = "Kế hoạch tháng";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.MediumBlue;
            this.label1.Location = new System.Drawing.Point(62, 135);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 19);
            this.label1.TabIndex = 3;
            this.label1.Text = "Mặt Hàng";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.MediumBlue;
            this.label4.Location = new System.Drawing.Point(24, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 19);
            this.label4.TabIndex = 4;
            this.label4.Text = "Thứ tự sản xuất";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.MediumBlue;
            this.label2.Location = new System.Drawing.Point(20, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 19);
            this.label2.TabIndex = 4;
            this.label2.Text = "Chuyền sản xuất";
            // 
            // lbSanLuongKeHoach
            // 
            this.lbSanLuongKeHoach.AutoSize = true;
            this.lbSanLuongKeHoach.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSanLuongKeHoach.ForeColor = System.Drawing.Color.MediumBlue;
            this.lbSanLuongKeHoach.Location = new System.Drawing.Point(49, 179);
            this.lbSanLuongKeHoach.Name = "lbSanLuongKeHoach";
            this.lbSanLuongKeHoach.Size = new System.Drawing.Size(91, 19);
            this.lbSanLuongKeHoach.TabIndex = 6;
            this.lbSanLuongKeHoach.Text = "SL kế hoạch";
            // 
            // cbbChuyen
            // 
            this.cbbChuyen.FormattingEnabled = true;
            this.cbbChuyen.Location = new System.Drawing.Point(146, 87);
            this.cbbChuyen.Name = "cbbChuyen";
            this.cbbChuyen.Size = new System.Drawing.Size(224, 27);
            this.cbbChuyen.TabIndex = 1;
            this.cbbChuyen.SelectedIndexChanged += new System.EventHandler(this.cbbChuyen_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.gridControl1);
            this.groupBox2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(456, 12);
            this.groupBox2.MinimumSize = new System.Drawing.Size(710, 188);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(785, 533);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Danh sách phân công";
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(3, 22);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemMemoEdit1,
            this.repositoryItemMemoEdit2});
            this.gridControl1.Size = new System.Drawing.Size(779, 508);
            this.gridControl1.TabIndex = 14;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.ColumnPanelRowHeight = 50;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.STTThucHien,
            this.TenSanPham,
            this.SanLuongKeHoach,
            this.LuyKeTH,
            this.Thang,
            this.Nam,
            this.STT,
            this.IsFinish});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsBehavior.AutoSelectAllInEditor = false;
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsView.RowAutoHeight = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.gridView1_RowCellClick);
            // 
            // STTThucHien
            // 
            this.STTThucHien.AppearanceCell.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.STTThucHien.AppearanceCell.Options.UseFont = true;
            this.STTThucHien.AppearanceCell.Options.UseTextOptions = true;
            this.STTThucHien.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.STTThucHien.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.STTThucHien.AppearanceHeader.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.STTThucHien.AppearanceHeader.Options.UseFont = true;
            this.STTThucHien.AppearanceHeader.Options.UseTextOptions = true;
            this.STTThucHien.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.STTThucHien.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.STTThucHien.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.STTThucHien.Caption = "STT Sản Xuất";
            this.STTThucHien.FieldName = "STT_TH";
            this.STTThucHien.Name = "STTThucHien";
            this.STTThucHien.Visible = true;
            this.STTThucHien.VisibleIndex = 0;
            // 
            // TenSanPham
            // 
            this.TenSanPham.AppearanceCell.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TenSanPham.AppearanceCell.Options.UseFont = true;
            this.TenSanPham.AppearanceCell.Options.UseTextOptions = true;
            this.TenSanPham.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TenSanPham.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.TenSanPham.AppearanceHeader.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TenSanPham.AppearanceHeader.Options.UseFont = true;
            this.TenSanPham.AppearanceHeader.Options.UseTextOptions = true;
            this.TenSanPham.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TenSanPham.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.TenSanPham.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.TenSanPham.Caption = "Mặt Hàng";
            this.TenSanPham.ColumnEdit = this.repositoryItemMemoEdit1;
            this.TenSanPham.FieldName = "CommoName";
            this.TenSanPham.Name = "TenSanPham";
            this.TenSanPham.Visible = true;
            this.TenSanPham.VisibleIndex = 1;
            // 
            // repositoryItemMemoEdit1
            // 
            this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
            // 
            // SanLuongKeHoach
            // 
            this.SanLuongKeHoach.AppearanceCell.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SanLuongKeHoach.AppearanceCell.Options.UseFont = true;
            this.SanLuongKeHoach.AppearanceCell.Options.UseTextOptions = true;
            this.SanLuongKeHoach.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.SanLuongKeHoach.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.SanLuongKeHoach.AppearanceHeader.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SanLuongKeHoach.AppearanceHeader.Options.UseFont = true;
            this.SanLuongKeHoach.AppearanceHeader.Options.UseTextOptions = true;
            this.SanLuongKeHoach.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.SanLuongKeHoach.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.SanLuongKeHoach.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.SanLuongKeHoach.Caption = "SL Kế Hoạch";
            this.SanLuongKeHoach.FieldName = "ProductionPlans";
            this.SanLuongKeHoach.Name = "SanLuongKeHoach";
            this.SanLuongKeHoach.Visible = true;
            this.SanLuongKeHoach.VisibleIndex = 2;
            // 
            // LuyKeTH
            // 
            this.LuyKeTH.AppearanceCell.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LuyKeTH.AppearanceCell.Options.UseFont = true;
            this.LuyKeTH.AppearanceCell.Options.UseTextOptions = true;
            this.LuyKeTH.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.LuyKeTH.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.LuyKeTH.AppearanceHeader.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LuyKeTH.AppearanceHeader.Options.UseFont = true;
            this.LuyKeTH.AppearanceHeader.Options.UseTextOptions = true;
            this.LuyKeTH.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.LuyKeTH.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.LuyKeTH.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.LuyKeTH.Caption = "LK Thực Hiện";
            this.LuyKeTH.FieldName = "LK_TH";
            this.LuyKeTH.Name = "LuyKeTH";
            this.LuyKeTH.Visible = true;
            this.LuyKeTH.VisibleIndex = 3;
            // 
            // Thang
            // 
            this.Thang.AppearanceCell.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Thang.AppearanceCell.Options.UseFont = true;
            this.Thang.AppearanceCell.Options.UseTextOptions = true;
            this.Thang.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Thang.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Thang.AppearanceHeader.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Thang.AppearanceHeader.Options.UseFont = true;
            this.Thang.AppearanceHeader.Options.UseTextOptions = true;
            this.Thang.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Thang.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Thang.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.Thang.Caption = "Tháng";
            this.Thang.FieldName = "Month";
            this.Thang.Name = "Thang";
            this.Thang.Visible = true;
            this.Thang.VisibleIndex = 4;
            // 
            // Nam
            // 
            this.Nam.AppearanceCell.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Nam.AppearanceCell.Options.UseFont = true;
            this.Nam.AppearanceCell.Options.UseTextOptions = true;
            this.Nam.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Nam.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Nam.AppearanceHeader.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Nam.AppearanceHeader.Options.UseFont = true;
            this.Nam.AppearanceHeader.Options.UseTextOptions = true;
            this.Nam.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Nam.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.Nam.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.Nam.Caption = "Năm";
            this.Nam.FieldName = "Year";
            this.Nam.Name = "Nam";
            this.Nam.Visible = true;
            this.Nam.VisibleIndex = 5;
            // 
            // STT
            // 
            this.STT.AppearanceCell.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.STT.AppearanceCell.Options.UseFont = true;
            this.STT.AppearanceCell.Options.UseTextOptions = true;
            this.STT.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.STT.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.STT.AppearanceHeader.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.STT.AppearanceHeader.Options.UseFont = true;
            this.STT.AppearanceHeader.Options.UseTextOptions = true;
            this.STT.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.STT.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.STT.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.STT.Caption = "STT";
            this.STT.FieldName = "STT";
            this.STT.Name = "STT";
            // 
            // IsFinish
            // 
            this.IsFinish.AppearanceCell.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IsFinish.AppearanceCell.Options.UseFont = true;
            this.IsFinish.AppearanceCell.Options.UseTextOptions = true;
            this.IsFinish.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.IsFinish.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.IsFinish.AppearanceHeader.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IsFinish.AppearanceHeader.Options.UseFont = true;
            this.IsFinish.AppearanceHeader.Options.UseTextOptions = true;
            this.IsFinish.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.IsFinish.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.IsFinish.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.IsFinish.Caption = "Trạng Thái";
            this.IsFinish.FieldName = "IsFinishStr";
            this.IsFinish.Name = "IsFinish";
            this.IsFinish.Visible = true;
            this.IsFinish.VisibleIndex = 6;
            // 
            // repositoryItemMemoEdit2
            // 
            this.repositoryItemMemoEdit2.Name = "repositoryItemMemoEdit2";
            // 
            // frmPhaHangChoChuyen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1251, 557);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(732, 403);
            this.Name = "frmPhaHangChoChuyen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Phân hàng cho chuyền";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmPhaHangChoChuyen_Load);
            ((System.ComponentModel.ISupportInitialize)(btnReLine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(btnReCommo)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSanLuongKeHoach)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSTTThucHien)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueSanPham.Properties)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbbYear;
        private System.Windows.Forms.ComboBox cbbMorth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbSanLuongKeHoach;
        private System.Windows.Forms.ComboBox cbbChuyen;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button butImportFromExcel;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn STTThucHien;
        private DevExpress.XtraGrid.Columns.GridColumn TenSanPham;
        private DevExpress.XtraGrid.Columns.GridColumn SanLuongKeHoach;
        private DevExpress.XtraGrid.Columns.GridColumn LuyKeTH;
        private DevExpress.XtraGrid.Columns.GridColumn Thang;
        private DevExpress.XtraGrid.Columns.GridColumn Nam;
        private DevExpress.XtraGrid.Columns.GridColumn STT;
        private DevExpress.XtraEditors.LookUpEdit lueSanPham;
        private DevExpress.XtraGrid.Columns.GridColumn IsFinish;
        private System.Windows.Forms.NumericUpDown txtSanLuongKeHoach;
        private System.Windows.Forms.NumericUpDown txtSTTThucHien;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAdd;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit2;

    }
}