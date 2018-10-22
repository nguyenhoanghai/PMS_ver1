namespace QuanLyNangSuat
{
    partial class FrmSetDayInformation
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
            this.numDinhMucNgay = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.chkbShowLCD = new System.Windows.Forms.CheckBox();
            this.txtLean = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtNangSuatLaoDong = new System.Windows.Forms.TextBox();
            this.txtLaoDongChuyen = new System.Windows.Forms.NumericUpDown();
            this.chkIsStopOnDay = new System.Windows.Forms.CheckBox();
            this.dtpNgayLamViec = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.cbbSanPham = new System.Windows.Forms.ComboBox();
            this.cbbChuyen = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ProductName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ProductivityWorker = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CountWorker = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LeanKH = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ShowLCDStr = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numHieuSuat = new System.Windows.Forms.TextBox();
            btnReLine = new System.Windows.Forms.PictureBox();
            btnReCommo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(btnReLine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(btnReCommo)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDinhMucNgay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLaoDongChuyen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnReLine
            // 
            btnReLine.BackgroundImage = global::QuanLyNangSuat.Properties.Resources.refresh;
            btnReLine.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            btnReLine.Cursor = System.Windows.Forms.Cursors.Hand;
            btnReLine.ImageLocation = "";
            btnReLine.Location = new System.Drawing.Point(420, 36);
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
            btnReCommo.Location = new System.Drawing.Point(420, 101);
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
            this.groupBox1.Controls.Add(this.numHieuSuat);
            this.groupBox1.Controls.Add(this.numDinhMucNgay);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.chkbShowLCD);
            this.groupBox1.Controls.Add(btnReCommo);
            this.groupBox1.Controls.Add(btnReLine);
            this.groupBox1.Controls.Add(this.txtLean);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.btnUpdate);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.txtNangSuatLaoDong);
            this.groupBox1.Controls.Add(this.txtLaoDongChuyen);
            this.groupBox1.Controls.Add(this.chkIsStopOnDay);
            this.groupBox1.Controls.Add(this.dtpNgayLamViec);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cbbSanPham);
            this.groupBox1.Controls.Add(this.cbbChuyen);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(9, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(491, 472);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thông tin ngày";
            // 
            // numDinhMucNgay
            // 
            this.numDinhMucNgay.Location = new System.Drawing.Point(176, 262);
            this.numDinhMucNgay.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numDinhMucNgay.Name = "numDinhMucNgay";
            this.numDinhMucNgay.Size = new System.Drawing.Size(148, 26);
            this.numDinhMucNgay.TabIndex = 59;
            this.numDinhMucNgay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numDinhMucNgay.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDinhMucNgay.ValueChanged += new System.EventHandler(this.numDinhMucNgay_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.MediumBlue;
            this.label8.Location = new System.Drawing.Point(60, 264);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(110, 19);
            this.label8.TabIndex = 58;
            this.label8.Text = "Định mức ngày";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.MediumBlue;
            this.label7.Location = new System.Drawing.Point(38, 232);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(133, 19);
            this.label7.TabIndex = 56;
            this.label7.Text = "Hiệu suất sản xuất";
            // 
            // chkbShowLCD
            // 
            this.chkbShowLCD.AutoSize = true;
            this.chkbShowLCD.Location = new System.Drawing.Point(176, 334);
            this.chkbShowLCD.Name = "chkbShowLCD";
            this.chkbShowLCD.Size = new System.Drawing.Size(232, 23);
            this.chkbShowLCD.TabIndex = 55;
            this.chkbShowLCD.Text = "chỉ Hiển thị thông tin mã hàng này.";
            this.chkbShowLCD.UseVisualStyleBackColor = true;
            // 
            // txtLean
            // 
            this.txtLean.Location = new System.Drawing.Point(177, 134);
            this.txtLean.Name = "txtLean";
            this.txtLean.Size = new System.Drawing.Size(84, 26);
            this.txtLean.TabIndex = 24;
            this.txtLean.Text = "0";
            this.txtLean.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.MediumBlue;
            this.label6.Location = new System.Drawing.Point(67, 137);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 19);
            this.label6.TabIndex = 23;
            this.label6.Text = "Vốn Kế Hoạch";
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Image = global::QuanLyNangSuat.Properties.Resources.deletee;
            this.btnDelete.Location = new System.Drawing.Point(82, 421);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnDelete.Size = new System.Drawing.Size(140, 40);
            this.btnDelete.TabIndex = 19;
            this.btnDelete.Text = "Xoá";
            this.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Image = global::QuanLyNangSuat.Properties.Resources._1464012624_Cancel;
            this.btnCancel.Location = new System.Drawing.Point(240, 421);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnCancel.Size = new System.Drawing.Size(140, 40);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "Huỷ bỏ";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Enabled = false;
            this.btnUpdate.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.Image = global::QuanLyNangSuat.Properties.Resources.edit;
            this.btnUpdate.Location = new System.Drawing.Point(240, 370);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnUpdate.Size = new System.Drawing.Size(140, 40);
            this.btnUpdate.TabIndex = 21;
            this.btnUpdate.Text = "Cập nhật";
            this.btnUpdate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Image = global::QuanLyNangSuat.Properties.Resources.add;
            this.btnAdd.Location = new System.Drawing.Point(82, 370);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnAdd.Size = new System.Drawing.Size(140, 40);
            this.btnAdd.TabIndex = 22;
            this.btnAdd.Text = "Thêm";
            this.btnAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtNangSuatLaoDong
            // 
            this.txtNangSuatLaoDong.Location = new System.Drawing.Point(177, 166);
            this.txtNangSuatLaoDong.Name = "txtNangSuatLaoDong";
            this.txtNangSuatLaoDong.Size = new System.Drawing.Size(147, 26);
            this.txtNangSuatLaoDong.TabIndex = 9;
            this.txtNangSuatLaoDong.Text = "0";
            this.txtNangSuatLaoDong.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtLaoDongChuyen
            // 
            this.txtLaoDongChuyen.Location = new System.Drawing.Point(177, 198);
            this.txtLaoDongChuyen.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.txtLaoDongChuyen.Name = "txtLaoDongChuyen";
            this.txtLaoDongChuyen.Size = new System.Drawing.Size(84, 26);
            this.txtLaoDongChuyen.TabIndex = 8;
            this.txtLaoDongChuyen.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtLaoDongChuyen.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // chkIsStopOnDay
            // 
            this.chkIsStopOnDay.AutoSize = true;
            this.chkIsStopOnDay.Location = new System.Drawing.Point(177, 305);
            this.chkIsStopOnDay.Name = "chkIsStopOnDay";
            this.chkIsStopOnDay.Size = new System.Drawing.Size(252, 23);
            this.chkIsStopOnDay.TabIndex = 7;
            this.chkIsStopOnDay.Text = "Dừng sản xuất mã hàng này hôm nay.";
            this.chkIsStopOnDay.UseVisualStyleBackColor = true;
            // 
            // dtpNgayLamViec
            // 
            this.dtpNgayLamViec.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgayLamViec.Location = new System.Drawing.Point(177, 69);
            this.dtpNgayLamViec.Name = "dtpNgayLamViec";
            this.dtpNgayLamViec.Size = new System.Drawing.Size(124, 26);
            this.dtpNgayLamViec.TabIndex = 6;
            this.dtpNgayLamViec.ValueChanged += new System.EventHandler(this.dtpNgayLamViec_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.MediumBlue;
            this.label5.Location = new System.Drawing.Point(67, 71);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 19);
            this.label5.TabIndex = 5;
            this.label5.Text = "Ngày làm việc";
            // 
            // cbbSanPham
            // 
            this.cbbSanPham.FormattingEnabled = true;
            this.cbbSanPham.Location = new System.Drawing.Point(177, 101);
            this.cbbSanPham.Name = "cbbSanPham";
            this.cbbSanPham.Size = new System.Drawing.Size(237, 27);
            this.cbbSanPham.TabIndex = 4;
            this.cbbSanPham.SelectedIndexChanged += new System.EventHandler(this.cbbSanPham_SelectedIndexChanged);
            this.cbbSanPham.SelectedValueChanged += new System.EventHandler(this.cbbSanPham_SelectedValueChanged);
            // 
            // cbbChuyen
            // 
            this.cbbChuyen.FormattingEnabled = true;
            this.cbbChuyen.Location = new System.Drawing.Point(177, 36);
            this.cbbChuyen.Name = "cbbChuyen";
            this.cbbChuyen.Size = new System.Drawing.Size(237, 27);
            this.cbbChuyen.TabIndex = 4;
            this.cbbChuyen.SelectedIndexChanged += new System.EventHandler(this.cbbChuyen_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.MediumBlue;
            this.label3.Location = new System.Drawing.Point(93, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 19);
            this.label3.TabIndex = 0;
            this.label3.Text = "Mặt Hàng";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.MediumBlue;
            this.label4.Location = new System.Drawing.Point(27, 169);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(147, 19);
            this.label4.TabIndex = 0;
            this.label4.Text = "Năng suất Lao Động";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.MediumBlue;
            this.label2.Location = new System.Drawing.Point(18, 200);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(153, 19);
            this.label2.TabIndex = 0;
            this.label2.Text = "Lao động trên chuyền";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.MediumBlue;
            this.label1.Location = new System.Drawing.Point(111, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Chuyền";
            // 
            // ProductName
            // 
            this.ProductName.AppearanceCell.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProductName.AppearanceCell.Options.UseFont = true;
            this.ProductName.AppearanceCell.Options.UseTextOptions = true;
            this.ProductName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ProductName.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ProductName.AppearanceHeader.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProductName.AppearanceHeader.Options.UseFont = true;
            this.ProductName.AppearanceHeader.Options.UseTextOptions = true;
            this.ProductName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ProductName.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ProductName.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.ProductName.Caption = "Mặt Hàng";
            this.ProductName.FieldName = "CommoName";
            this.ProductName.Name = "ProductName";
            this.ProductName.Visible = true;
            this.ProductName.VisibleIndex = 0;
            // 
            // gridView
            // 
            this.gridView.ColumnPanelRowHeight = 35;
            this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.PId,
            this.ProductName,
            this.ProductivityWorker,
            this.CountWorker,
            this.LeanKH,
            this.ShowLCDStr,
            this.gridColumn2});
            this.gridView.GridControl = this.gridControl;
            this.gridView.Name = "gridView";
            this.gridView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView.OptionsBehavior.AutoSelectAllInEditor = false;
            this.gridView.OptionsBehavior.Editable = false;
            this.gridView.OptionsView.RowAutoHeight = true;
            this.gridView.OptionsView.ShowAutoFilterRow = true;
            this.gridView.OptionsView.ShowGroupPanel = false;
            this.gridView.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.gridView_RowCellClick);
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn1.AppearanceCell.Options.UseFont = true;
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn1.AppearanceHeader.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn1.AppearanceHeader.Options.UseFont = true;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn1.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridColumn1.Caption = "Id";
            this.gridColumn1.FieldName = "Id";
            this.gridColumn1.Name = "gridColumn1";
            // 
            // PId
            // 
            this.PId.Caption = "gridColumn1";
            this.PId.FieldName = "ProductId";
            this.PId.Name = "PId";
            // 
            // ProductivityWorker
            // 
            this.ProductivityWorker.AppearanceCell.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProductivityWorker.AppearanceCell.Options.UseFont = true;
            this.ProductivityWorker.AppearanceCell.Options.UseTextOptions = true;
            this.ProductivityWorker.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ProductivityWorker.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ProductivityWorker.AppearanceHeader.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProductivityWorker.AppearanceHeader.Options.UseFont = true;
            this.ProductivityWorker.AppearanceHeader.Options.UseTextOptions = true;
            this.ProductivityWorker.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ProductivityWorker.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ProductivityWorker.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.ProductivityWorker.Caption = "Năng Suất Lao Động";
            this.ProductivityWorker.FieldName = "NangXuatLaoDong";
            this.ProductivityWorker.Name = "ProductivityWorker";
            this.ProductivityWorker.Visible = true;
            this.ProductivityWorker.VisibleIndex = 2;
            // 
            // CountWorker
            // 
            this.CountWorker.AppearanceCell.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CountWorker.AppearanceCell.Options.UseFont = true;
            this.CountWorker.AppearanceCell.Options.UseTextOptions = true;
            this.CountWorker.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.CountWorker.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.CountWorker.AppearanceHeader.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CountWorker.AppearanceHeader.Options.UseFont = true;
            this.CountWorker.AppearanceHeader.Options.UseTextOptions = true;
            this.CountWorker.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.CountWorker.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.CountWorker.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.CountWorker.Caption = "Số Lao Động";
            this.CountWorker.FieldName = "LaoDongChuyen";
            this.CountWorker.Name = "CountWorker";
            this.CountWorker.Visible = true;
            this.CountWorker.VisibleIndex = 3;
            // 
            // LeanKH
            // 
            this.LeanKH.AppearanceCell.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LeanKH.AppearanceCell.Options.UseFont = true;
            this.LeanKH.AppearanceCell.Options.UseTextOptions = true;
            this.LeanKH.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.LeanKH.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.LeanKH.AppearanceHeader.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LeanKH.AppearanceHeader.Options.UseFont = true;
            this.LeanKH.AppearanceHeader.Options.UseTextOptions = true;
            this.LeanKH.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.LeanKH.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.LeanKH.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.LeanKH.Caption = "Vốn Kế Hoạch";
            this.LeanKH.FieldName = "LeanKH";
            this.LeanKH.Name = "LeanKH";
            this.LeanKH.Visible = true;
            this.LeanKH.VisibleIndex = 4;
            // 
            // ShowLCDStr
            // 
            this.ShowLCDStr.AppearanceCell.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowLCDStr.AppearanceCell.Options.UseFont = true;
            this.ShowLCDStr.AppearanceCell.Options.UseTextOptions = true;
            this.ShowLCDStr.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ShowLCDStr.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ShowLCDStr.AppearanceHeader.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowLCDStr.AppearanceHeader.Options.UseFont = true;
            this.ShowLCDStr.AppearanceHeader.Options.UseTextOptions = true;
            this.ShowLCDStr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ShowLCDStr.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ShowLCDStr.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.ShowLCDStr.Caption = "Hiển thị";
            this.ShowLCDStr.FieldName = "ShowLCD";
            this.ShowLCDStr.Name = "ShowLCDStr";
            this.ShowLCDStr.Visible = true;
            this.ShowLCDStr.VisibleIndex = 5;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn2.AppearanceCell.Options.UseFont = true;
            this.gridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn2.AppearanceHeader.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn2.AppearanceHeader.Options.UseFont = true;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn2.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridColumn2.Caption = "Hiệu suất";
            this.gridColumn2.FieldName = "HieuSuat";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            // 
            // gridControl
            // 
            this.gridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl.Location = new System.Drawing.Point(3, 22);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Name = "gridControl";
            this.gridControl.Size = new System.Drawing.Size(702, 443);
            this.gridControl.TabIndex = 9;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.gridControl);
            this.groupBox2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(506, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(708, 468);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Danh sách thông tin ngày của phân công";
            // 
            // numHieuSuat
            // 
            this.numHieuSuat.Location = new System.Drawing.Point(176, 229);
            this.numHieuSuat.Name = "numHieuSuat";
            this.numHieuSuat.Size = new System.Drawing.Size(148, 26);
            this.numHieuSuat.TabIndex = 60;
            this.numHieuSuat.Text = "100";
            this.numHieuSuat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numHieuSuat.TextChanged += new System.EventHandler(this.numHieuSuat_ValueChanged);
            this.numHieuSuat.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numHieuSuat_KeyPress);
            // 
            // FrmSetDayInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1226, 492);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FrmSetDayInformation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Nhập thông tin ngày";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmSetDayInformation_Load);
            ((System.ComponentModel.ISupportInitialize)(btnReLine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(btnReCommo)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDinhMucNgay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLaoDongChuyen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraGrid.Columns.GridColumn ProductName;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraGrid.Columns.GridColumn PId;
        private DevExpress.XtraGrid.GridControl gridControl;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cbbSanPham;
        private System.Windows.Forms.ComboBox cbbChuyen;
        private System.Windows.Forms.DateTimePicker dtpNgayLamViec;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraGrid.Columns.GridColumn ProductivityWorker;
        private DevExpress.XtraGrid.Columns.GridColumn CountWorker;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private System.Windows.Forms.CheckBox chkIsStopOnDay;
        private System.Windows.Forms.TextBox txtNangSuatLaoDong;
        private System.Windows.Forms.NumericUpDown txtLaoDongChuyen;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtLean;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraGrid.Columns.GridColumn LeanKH;
        private System.Windows.Forms.CheckBox chkbShowLCD;
        private DevExpress.XtraGrid.Columns.GridColumn ShowLCDStr;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private System.Windows.Forms.NumericUpDown numDinhMucNgay;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox numHieuSuat;
    }
}