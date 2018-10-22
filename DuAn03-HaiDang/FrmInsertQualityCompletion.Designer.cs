namespace QuanLyNangSuat
{
    partial class FrmInsertQualityCompletion
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
            System.Windows.Forms.PictureBox btnRefreshAssign;
            System.Windows.Forms.PictureBox btnRefreshPhase;
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblSanLuongKeHoach = new System.Windows.Forms.Label();
            this.txtsl = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.btnDelete_s = new System.Windows.Forms.Button();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            this.btnCancel_s = new System.Windows.Forms.Button();
            this.cbPhase = new System.Windows.Forms.ComboBox();
            this.btnUpdate_s = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.btnAdd_s = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpNgayThucHien = new System.Windows.Forms.DateTimePicker();
            this.cboSanPham_0 = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ProductName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ProductivityWorker = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            btnRefreshAssign = new System.Windows.Forms.PictureBox();
            btnRefreshPhase = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(btnRefreshAssign)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(btnRefreshPhase)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtsl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRefreshAssign
            // 
            btnRefreshAssign.BackgroundImage = global::QuanLyNangSuat.Properties.Resources.refresh;
            btnRefreshAssign.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            btnRefreshAssign.Cursor = System.Windows.Forms.Cursors.Hand;
            btnRefreshAssign.ImageLocation = "";
            btnRefreshAssign.Location = new System.Drawing.Point(311, 36);
            btnRefreshAssign.Name = "btnRefreshAssign";
            btnRefreshAssign.Size = new System.Drawing.Size(30, 27);
            btnRefreshAssign.TabIndex = 52;
            btnRefreshAssign.TabStop = false;
            btnRefreshAssign.Click += new System.EventHandler(this.btnRefreshAssign_Click);
            // 
            // btnRefreshPhase
            // 
            btnRefreshPhase.BackgroundImage = global::QuanLyNangSuat.Properties.Resources.refresh;
            btnRefreshPhase.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            btnRefreshPhase.Cursor = System.Windows.Forms.Cursors.Hand;
            btnRefreshPhase.ImageLocation = "";
            btnRefreshPhase.Location = new System.Drawing.Point(311, 146);
            btnRefreshPhase.Name = "btnRefreshPhase";
            btnRefreshPhase.Size = new System.Drawing.Size(30, 27);
            btnRefreshPhase.TabIndex = 53;
            btnRefreshPhase.TabStop = false;
            btnRefreshPhase.Click += new System.EventHandler(this.btnRefreshPhase_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(btnRefreshPhase);
            this.groupBox3.Controls.Add(btnRefreshAssign);
            this.groupBox3.Controls.Add(this.lblSanLuongKeHoach);
            this.groupBox3.Controls.Add(this.txtsl);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.btnDelete_s);
            this.groupBox3.Controls.Add(this.radioGroup1);
            this.groupBox3.Controls.Add(this.btnCancel_s);
            this.groupBox3.Controls.Add(this.cbPhase);
            this.groupBox3.Controls.Add(this.btnUpdate_s);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.btnAdd_s);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.dtpNgayThucHien);
            this.groupBox3.Controls.Add(this.cboSanPham_0);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.groupBox3.Location = new System.Drawing.Point(12, 11);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(361, 441);
            this.groupBox3.TabIndex = 40;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Thông Tin";
            // 
            // lblSanLuongKeHoach
            // 
            this.lblSanLuongKeHoach.AutoSize = true;
            this.lblSanLuongKeHoach.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblSanLuongKeHoach.ForeColor = System.Drawing.Color.Red;
            this.lblSanLuongKeHoach.Location = new System.Drawing.Point(119, 113);
            this.lblSanLuongKeHoach.Name = "lblSanLuongKeHoach";
            this.lblSanLuongKeHoach.Size = new System.Drawing.Size(17, 19);
            this.lblSanLuongKeHoach.TabIndex = 51;
            this.lblSanLuongKeHoach.Text = "0";
            // 
            // txtsl
            // 
            this.txtsl.Location = new System.Drawing.Point(123, 232);
            this.txtsl.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.txtsl.Name = "txtsl";
            this.txtsl.Size = new System.Drawing.Size(83, 26);
            this.txtsl.TabIndex = 49;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label4.ForeColor = System.Drawing.Color.MediumBlue;
            this.label4.Location = new System.Drawing.Point(36, 234);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 19);
            this.label4.TabIndex = 47;
            this.label4.Text = "Sản lượng";
            // 
            // btnDelete_s
            // 
            this.btnDelete_s.Enabled = false;
            this.btnDelete_s.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete_s.Image = global::QuanLyNangSuat.Properties.Resources.deletee;
            this.btnDelete_s.Location = new System.Drawing.Point(23, 388);
            this.btnDelete_s.Name = "btnDelete_s";
            this.btnDelete_s.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnDelete_s.Size = new System.Drawing.Size(140, 40);
            this.btnDelete_s.TabIndex = 40;
            this.btnDelete_s.Text = "Xoá";
            this.btnDelete_s.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDelete_s.UseVisualStyleBackColor = true;
            // 
            // radioGroup1
            // 
            this.radioGroup1.EditValue = true;
            this.radioGroup1.Location = new System.Drawing.Point(125, 182);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.radioGroup1.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(true, "Tăng"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(false, "Giảm")});
            this.radioGroup1.Size = new System.Drawing.Size(67, 45);
            this.radioGroup1.TabIndex = 46;
            // 
            // btnCancel_s
            // 
            this.btnCancel_s.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel_s.Image = global::QuanLyNangSuat.Properties.Resources._1464012624_Cancel;
            this.btnCancel_s.Location = new System.Drawing.Point(181, 388);
            this.btnCancel_s.Name = "btnCancel_s";
            this.btnCancel_s.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnCancel_s.Size = new System.Drawing.Size(140, 40);
            this.btnCancel_s.TabIndex = 41;
            this.btnCancel_s.Text = "Huỷ bỏ";
            this.btnCancel_s.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel_s.UseVisualStyleBackColor = true;
            // 
            // cbPhase
            // 
            this.cbPhase.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.cbPhase.FormattingEnabled = true;
            this.cbPhase.Location = new System.Drawing.Point(123, 146);
            this.cbPhase.Name = "cbPhase";
            this.cbPhase.Size = new System.Drawing.Size(187, 27);
            this.cbPhase.TabIndex = 42;
            // 
            // btnUpdate_s
            // 
            this.btnUpdate_s.Enabled = false;
            this.btnUpdate_s.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate_s.Image = global::QuanLyNangSuat.Properties.Resources.edit;
            this.btnUpdate_s.Location = new System.Drawing.Point(181, 338);
            this.btnUpdate_s.Name = "btnUpdate_s";
            this.btnUpdate_s.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnUpdate_s.Size = new System.Drawing.Size(140, 40);
            this.btnUpdate_s.TabIndex = 42;
            this.btnUpdate_s.Text = "Cập nhật";
            this.btnUpdate_s.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUpdate_s.UseVisualStyleBackColor = true;
            this.btnUpdate_s.Click += new System.EventHandler(this.btnUpdate_s_Click);
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label8.ForeColor = System.Drawing.Color.MediumBlue;
            this.label8.Location = new System.Drawing.Point(29, 149);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 19);
            this.label8.TabIndex = 41;
            this.label8.Text = "Công Đoạn";
            // 
            // btnAdd_s
            // 
            this.btnAdd_s.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd_s.Image = global::QuanLyNangSuat.Properties.Resources.add;
            this.btnAdd_s.Location = new System.Drawing.Point(23, 338);
            this.btnAdd_s.Name = "btnAdd_s";
            this.btnAdd_s.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnAdd_s.Size = new System.Drawing.Size(140, 40);
            this.btnAdd_s.TabIndex = 43;
            this.btnAdd_s.Text = "Thêm";
            this.btnAdd_s.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAdd_s.UseVisualStyleBackColor = true;
            this.btnAdd_s.Click += new System.EventHandler(this.btnAdd_s_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label2.ForeColor = System.Drawing.Color.MediumBlue;
            this.label2.Location = new System.Drawing.Point(39, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 19);
            this.label2.TabIndex = 20;
            this.label2.Text = "Mặt hàng";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label6.ForeColor = System.Drawing.Color.MediumBlue;
            this.label6.Location = new System.Drawing.Point(2, 79);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(111, 19);
            this.label6.TabIndex = 19;
            this.label6.Text = "Ngày thực hiện";
            // 
            // dtpNgayThucHien
            // 
            this.dtpNgayThucHien.CustomFormat = "d/M/yyyy";
            this.dtpNgayThucHien.Enabled = false;
            this.dtpNgayThucHien.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.dtpNgayThucHien.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgayThucHien.Location = new System.Drawing.Point(123, 73);
            this.dtpNgayThucHien.Name = "dtpNgayThucHien";
            this.dtpNgayThucHien.Size = new System.Drawing.Size(84, 26);
            this.dtpNgayThucHien.TabIndex = 25;
            // 
            // cboSanPham_0
            // 
            this.cboSanPham_0.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.cboSanPham_0.FormattingEnabled = true;
            this.cboSanPham_0.Location = new System.Drawing.Point(123, 36);
            this.cboSanPham_0.Name = "cboSanPham_0";
            this.cboSanPham_0.Size = new System.Drawing.Size(187, 27);
            this.cboSanPham_0.TabIndex = 22;
            this.cboSanPham_0.SelectedIndexChanged += new System.EventHandler(this.cboSanPham_0_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label7.ForeColor = System.Drawing.Color.MediumBlue;
            this.label7.Location = new System.Drawing.Point(22, 113);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(91, 19);
            this.label7.TabIndex = 18;
            this.label7.Text = "SL kế hoạch";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.gridControl1);
            this.groupBox1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.groupBox1.Location = new System.Drawing.Point(379, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(839, 441);
            this.groupBox1.TabIndex = 41;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Lịch sử trong Ngày";
            // 
            // gridControl1
            // 
            this.gridControl1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(3, 22);
            this.gridControl1.MainView = this.gridView;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(833, 416);
            this.gridControl1.TabIndex = 11;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            // 
            // gridView
            // 
            this.gridView.ColumnPanelRowHeight = 35;
            this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn5,
            this.ProductName,
            this.ProductivityWorker,
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn4});
            this.gridView.GridControl = this.gridControl1;
            this.gridView.Name = "gridView";
            this.gridView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView.OptionsBehavior.AutoSelectAllInEditor = false;
            this.gridView.OptionsBehavior.Editable = false;
            this.gridView.OptionsView.RowAutoHeight = true;
            this.gridView.OptionsView.ShowAutoFilterRow = true;
            this.gridView.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "Id";
            this.gridColumn5.FieldName = "MaChuyen";
            this.gridColumn5.Name = "gridColumn5";
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
            this.ProductName.Caption = "Công Đoạn";
            this.ProductName.FieldName = "PhaseName";
            this.ProductName.Name = "ProductName";
            this.ProductName.Visible = true;
            this.ProductName.VisibleIndex = 1;
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
            this.ProductivityWorker.Caption = "Hình thức";
            this.ProductivityWorker.FieldName = "TypeName";
            this.ProductivityWorker.Name = "ProductivityWorker";
            this.ProductivityWorker.Visible = true;
            this.ProductivityWorker.VisibleIndex = 2;
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
            this.gridColumn1.Caption = "Sản lượng";
            this.gridColumn1.FieldName = "Quantity";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 3;
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
            this.gridColumn2.Caption = "Thời Gian";
            this.gridColumn2.FieldName = "Time";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 4;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn4.AppearanceCell.Options.UseFont = true;
            this.gridColumn4.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn4.AppearanceHeader.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn4.AppearanceHeader.Options.UseFont = true;
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn4.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridColumn4.Caption = "Mã Hàng";
            this.gridColumn4.FieldName = "CommoName";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 0;
            // 
            // FrmInsertQualityCompletion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1230, 462);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Name = "FrmInsertQualityCompletion";
            this.Text = "Nhập sản lượng công đoạn Hoàn thành";
            this.Load += new System.EventHandler(this.FrmInsertQualityCompletion_Load);
            ((System.ComponentModel.ISupportInitialize)(btnRefreshAssign)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(btnRefreshPhase)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtsl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lblSanLuongKeHoach;
        private System.Windows.Forms.NumericUpDown txtsl;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnDelete_s;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
        private System.Windows.Forms.Button btnCancel_s;
        private System.Windows.Forms.ComboBox cbPhase;
        private System.Windows.Forms.Button btnUpdate_s;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnAdd_s;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtpNgayThucHien;
        private System.Windows.Forms.ComboBox cboSanPham_0;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn ProductName;
        private DevExpress.XtraGrid.Columns.GridColumn ProductivityWorker;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
    }
}