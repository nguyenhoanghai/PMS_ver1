namespace DuAn03_HaiDang
{
    partial class FrmMailSend
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnDelete_s = new System.Windows.Forms.Button();
            this.btnCancel_s = new System.Windows.Forms.Button();
            this.btnUpdate_s = new System.Windows.Forms.Button();
            this.btnAdd_s = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvListMailSend = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chkIsActive = new System.Windows.Forms.CheckBox();
            this.txtNote = new System.Windows.Forms.RichTextBox();
            this.cbbMailTypeSend = new System.Windows.Forms.ComboBox();
            this.txtRePassword = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnDelete_r = new System.Windows.Forms.Button();
            this.btnCancel_r = new System.Windows.Forms.Button();
            this.btnUpdate_r = new System.Windows.Forms.Button();
            this.btnAdd_r = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.dgvListMailReceive = new System.Windows.Forms.DataGridView();
            this.IdR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AddressR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsActiveR = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.NoteR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsDeleted = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.chkIsActiveReceive = new System.Windows.Forms.CheckBox();
            this.txtNoteReceive = new System.Windows.Forms.RichTextBox();
            this.txtAddressReceive = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MailType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MailTypeId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsActive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Note = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListMailSend)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListMailReceive)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1071, 496);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1063, 467);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Cấu hình mail gửi";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.btnDelete_s);
            this.groupBox3.Controls.Add(this.btnCancel_s);
            this.groupBox3.Controls.Add(this.btnUpdate_s);
            this.groupBox3.Controls.Add(this.btnAdd_s);
            this.groupBox3.Location = new System.Drawing.Point(8, 120);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1047, 76);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Thao tác";
            // 
            // btnDelete_s
            // 
            this.btnDelete_s.Enabled = false;
            this.btnDelete_s.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete_s.Image = global::QuanLyNangSuat.Properties.Resources.deletee;
            this.btnDelete_s.Location = new System.Drawing.Point(549, 22);
            this.btnDelete_s.Name = "btnDelete_s";
            this.btnDelete_s.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnDelete_s.Size = new System.Drawing.Size(140, 40);
            this.btnDelete_s.TabIndex = 0;
            this.btnDelete_s.Text = "Xoá";
            this.btnDelete_s.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDelete_s.UseVisualStyleBackColor = true;
            this.btnDelete_s.Click += new System.EventHandler(this.btnDelete_s_Click);
            // 
            // btnCancel_s
            // 
            this.btnCancel_s.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel_s.Image = global::QuanLyNangSuat.Properties.Resources._1464012624_Cancel;
            this.btnCancel_s.Location = new System.Drawing.Point(707, 22);
            this.btnCancel_s.Name = "btnCancel_s";
            this.btnCancel_s.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnCancel_s.Size = new System.Drawing.Size(140, 40);
            this.btnCancel_s.TabIndex = 0;
            this.btnCancel_s.Text = "Huỷ bỏ";
            this.btnCancel_s.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel_s.UseVisualStyleBackColor = true;
            this.btnCancel_s.Click += new System.EventHandler(this.btnCancel_s_Click);
            // 
            // btnUpdate_s
            // 
            this.btnUpdate_s.Enabled = false;
            this.btnUpdate_s.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate_s.Image = global::QuanLyNangSuat.Properties.Resources.edit;
            this.btnUpdate_s.Location = new System.Drawing.Point(396, 22);
            this.btnUpdate_s.Name = "btnUpdate_s";
            this.btnUpdate_s.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnUpdate_s.Size = new System.Drawing.Size(140, 40);
            this.btnUpdate_s.TabIndex = 0;
            this.btnUpdate_s.Text = "Cập nhật";
            this.btnUpdate_s.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUpdate_s.UseVisualStyleBackColor = true;
            this.btnUpdate_s.Click += new System.EventHandler(this.btnUpdate_s_Click);
            // 
            // btnAdd_s
            // 
            this.btnAdd_s.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd_s.Image = global::QuanLyNangSuat.Properties.Resources.add;
            this.btnAdd_s.Location = new System.Drawing.Point(238, 22);
            this.btnAdd_s.Name = "btnAdd_s";
            this.btnAdd_s.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnAdd_s.Size = new System.Drawing.Size(140, 40);
            this.btnAdd_s.TabIndex = 0;
            this.btnAdd_s.Text = "Thêm";
            this.btnAdd_s.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAdd_s.UseVisualStyleBackColor = true;
            this.btnAdd_s.Click += new System.EventHandler(this.btnAdd_s_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.dgvListMailSend);
            this.groupBox2.Location = new System.Drawing.Point(6, 202);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1049, 257);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Danh sách mail";
            // 
            // dgvListMailSend
            // 
            this.dgvListMailSend.AllowUserToAddRows = false;
            this.dgvListMailSend.AllowUserToDeleteRows = false;
            this.dgvListMailSend.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvListMailSend.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.MailType,
            this.Address,
            this.MailTypeId,
            this.IsActive,
            this.Note,
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4});
            this.dgvListMailSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvListMailSend.Location = new System.Drawing.Point(3, 19);
            this.dgvListMailSend.Name = "dgvListMailSend";
            this.dgvListMailSend.ReadOnly = true;
            this.dgvListMailSend.Size = new System.Drawing.Size(1043, 235);
            this.dgvListMailSend.TabIndex = 0;
            this.dgvListMailSend.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvListMailSend_CellClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.chkIsActive);
            this.groupBox1.Controls.Add(this.txtNote);
            this.groupBox1.Controls.Add(this.cbbMailTypeSend);
            this.groupBox1.Controls.Add(this.txtRePassword);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.txtAddress);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox1.Location = new System.Drawing.Point(8, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1047, 108);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thông tin mail";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Location = new System.Drawing.Point(374, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 16);
            this.label6.TabIndex = 5;
            this.label6.Text = "Mật khẩu";
            // 
            // chkIsActive
            // 
            this.chkIsActive.AutoSize = true;
            this.chkIsActive.Checked = true;
            this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsActive.Enabled = false;
            this.chkIsActive.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkIsActive.Location = new System.Drawing.Point(731, 29);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(94, 23);
            this.chkIsActive.TabIndex = 4;
            this.chkIsActive.Text = "Sử dụng";
            this.chkIsActive.UseVisualStyleBackColor = true;
            // 
            // txtNote
            // 
            this.txtNote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNote.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNote.Location = new System.Drawing.Point(731, 61);
            this.txtNote.Name = "txtNote";
            this.txtNote.Size = new System.Drawing.Size(310, 39);
            this.txtNote.TabIndex = 3;
            this.txtNote.Text = "";
            // 
            // cbbMailTypeSend
            // 
            this.cbbMailTypeSend.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbMailTypeSend.FormattingEnabled = true;
            this.cbbMailTypeSend.Location = new System.Drawing.Point(91, 27);
            this.cbbMailTypeSend.Name = "cbbMailTypeSend";
            this.cbbMailTypeSend.Size = new System.Drawing.Size(203, 24);
            this.cbbMailTypeSend.TabIndex = 2;
            // 
            // txtRePassword
            // 
            this.txtRePassword.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRePassword.Location = new System.Drawing.Point(453, 61);
            this.txtRePassword.Name = "txtRePassword";
            this.txtRePassword.PasswordChar = '*';
            this.txtRePassword.Size = new System.Drawing.Size(171, 23);
            this.txtRePassword.TabIndex = 1;
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(453, 27);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(171, 23);
            this.txtPassword.TabIndex = 1;
            // 
            // txtAddress
            // 
            this.txtAddress.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddress.Location = new System.Drawing.Point(91, 61);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(203, 23);
            this.txtAddress.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(310, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(132, 16);
            this.label5.TabIndex = 0;
            this.label5.Text = "Xác nhận mật khẩu";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(666, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 16);
            this.label4.TabIndex = 0;
            this.label4.Text = "Ghi chú";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(26, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "Loại mail";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(659, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Sử dụng";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(10, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Địa chỉ mail";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Controls.Add(this.groupBox6);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1063, 467);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Cấu hình mail nhận";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.btnDelete_r);
            this.groupBox4.Controls.Add(this.btnCancel_r);
            this.groupBox4.Controls.Add(this.btnUpdate_r);
            this.groupBox4.Controls.Add(this.btnAdd_r);
            this.groupBox4.Location = new System.Drawing.Point(10, 90);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(1045, 65);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Thao tác";
            // 
            // btnDelete_r
            // 
            this.btnDelete_r.Enabled = false;
            this.btnDelete_r.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete_r.Image = global::QuanLyNangSuat.Properties.Resources.deletee;
            this.btnDelete_r.Location = new System.Drawing.Point(547, 17);
            this.btnDelete_r.Name = "btnDelete_r";
            this.btnDelete_r.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnDelete_r.Size = new System.Drawing.Size(140, 40);
            this.btnDelete_r.TabIndex = 0;
            this.btnDelete_r.Text = "Xoá";
            this.btnDelete_r.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDelete_r.UseVisualStyleBackColor = true;
            this.btnDelete_r.Click += new System.EventHandler(this.btnDelete_r_Click);
            // 
            // btnCancel_r
            // 
            this.btnCancel_r.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel_r.Image = global::QuanLyNangSuat.Properties.Resources._1464012624_Cancel;
            this.btnCancel_r.Location = new System.Drawing.Point(715, 17);
            this.btnCancel_r.Name = "btnCancel_r";
            this.btnCancel_r.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnCancel_r.Size = new System.Drawing.Size(140, 40);
            this.btnCancel_r.TabIndex = 0;
            this.btnCancel_r.Text = "Huỷ bỏ";
            this.btnCancel_r.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel_r.UseVisualStyleBackColor = true;
            this.btnCancel_r.Click += new System.EventHandler(this.btnCancel_r_Click);
            // 
            // btnUpdate_r
            // 
            this.btnUpdate_r.Enabled = false;
            this.btnUpdate_r.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate_r.Image = global::QuanLyNangSuat.Properties.Resources.edit;
            this.btnUpdate_r.Location = new System.Drawing.Point(376, 17);
            this.btnUpdate_r.Name = "btnUpdate_r";
            this.btnUpdate_r.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnUpdate_r.Size = new System.Drawing.Size(140, 40);
            this.btnUpdate_r.TabIndex = 0;
            this.btnUpdate_r.Text = "Cập nhật";
            this.btnUpdate_r.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUpdate_r.UseVisualStyleBackColor = true;
            this.btnUpdate_r.Click += new System.EventHandler(this.btnUpdate_r_Click);
            // 
            // btnAdd_r
            // 
            this.btnAdd_r.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd_r.Image = global::QuanLyNangSuat.Properties.Resources.add;
            this.btnAdd_r.Location = new System.Drawing.Point(207, 17);
            this.btnAdd_r.Name = "btnAdd_r";
            this.btnAdd_r.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnAdd_r.Size = new System.Drawing.Size(140, 40);
            this.btnAdd_r.TabIndex = 0;
            this.btnAdd_r.Text = "Thêm";
            this.btnAdd_r.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAdd_r.UseVisualStyleBackColor = true;
            this.btnAdd_r.Click += new System.EventHandler(this.btnAdd_r_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.dgvListMailReceive);
            this.groupBox5.Location = new System.Drawing.Point(8, 161);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(1047, 300);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Danh sách mail";
            // 
            // dgvListMailReceive
            // 
            this.dgvListMailReceive.AllowUserToAddRows = false;
            this.dgvListMailReceive.AllowUserToDeleteRows = false;
            this.dgvListMailReceive.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvListMailReceive.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IdR,
            this.AddressR,
            this.IsActiveR,
            this.NoteR,
            this.IsDeleted});
            this.dgvListMailReceive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvListMailReceive.Location = new System.Drawing.Point(3, 19);
            this.dgvListMailReceive.Name = "dgvListMailReceive";
            this.dgvListMailReceive.ReadOnly = true;
            this.dgvListMailReceive.Size = new System.Drawing.Size(1041, 278);
            this.dgvListMailReceive.TabIndex = 1;
            this.dgvListMailReceive.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvListMailReceive_CellClick);
            // 
            // IdR
            // 
            this.IdR.DataPropertyName = "Id";
            this.IdR.HeaderText = "Id";
            this.IdR.Name = "IdR";
            this.IdR.ReadOnly = true;
            this.IdR.Visible = false;
            // 
            // AddressR
            // 
            this.AddressR.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.AddressR.DataPropertyName = "Address";
            this.AddressR.FillWeight = 93.27411F;
            this.AddressR.HeaderText = "Địa chỉ mail";
            this.AddressR.Name = "AddressR";
            this.AddressR.ReadOnly = true;
            // 
            // IsActiveR
            // 
            this.IsActiveR.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.IsActiveR.DataPropertyName = "IsActive";
            this.IsActiveR.FalseValue = "Không";
            this.IsActiveR.FillWeight = 50F;
            this.IsActiveR.HeaderText = "Trạng thái sử dụng";
            this.IsActiveR.Name = "IsActiveR";
            this.IsActiveR.ReadOnly = true;
            this.IsActiveR.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IsActiveR.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.IsActiveR.TrueValue = "Có";
            this.IsActiveR.Width = 50;
            // 
            // NoteR
            // 
            this.NoteR.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.NoteR.DataPropertyName = "Note";
            this.NoteR.FillWeight = 93.27411F;
            this.NoteR.HeaderText = "Ghi chú";
            this.NoteR.Name = "NoteR";
            this.NoteR.ReadOnly = true;
            // 
            // IsDeleted
            // 
            this.IsDeleted.DataPropertyName = "IsDeleted";
            this.IsDeleted.HeaderText = "Column1";
            this.IsDeleted.Name = "IsDeleted";
            this.IsDeleted.ReadOnly = true;
            this.IsDeleted.Visible = false;
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.chkIsActiveReceive);
            this.groupBox6.Controls.Add(this.txtNoteReceive);
            this.groupBox6.Controls.Add(this.txtAddressReceive);
            this.groupBox6.Controls.Add(this.label9);
            this.groupBox6.Controls.Add(this.label11);
            this.groupBox6.Controls.Add(this.label12);
            this.groupBox6.Location = new System.Drawing.Point(10, 7);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(1045, 77);
            this.groupBox6.TabIndex = 3;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Thông tin mail";
            // 
            // chkIsActiveReceive
            // 
            this.chkIsActiveReceive.AutoSize = true;
            this.chkIsActiveReceive.Checked = true;
            this.chkIsActiveReceive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsActiveReceive.Enabled = false;
            this.chkIsActiveReceive.Location = new System.Drawing.Point(420, 32);
            this.chkIsActiveReceive.Name = "chkIsActiveReceive";
            this.chkIsActiveReceive.Size = new System.Drawing.Size(80, 20);
            this.chkIsActiveReceive.TabIndex = 4;
            this.chkIsActiveReceive.Text = "Sử dụng";
            this.chkIsActiveReceive.UseVisualStyleBackColor = true;
            // 
            // txtNoteReceive
            // 
            this.txtNoteReceive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNoteReceive.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNoteReceive.Location = new System.Drawing.Point(615, 30);
            this.txtNoteReceive.Name = "txtNoteReceive";
            this.txtNoteReceive.Size = new System.Drawing.Size(413, 39);
            this.txtNoteReceive.TabIndex = 3;
            this.txtNoteReceive.Text = "";
            // 
            // txtAddressReceive
            // 
            this.txtAddressReceive.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddressReceive.Location = new System.Drawing.Point(125, 27);
            this.txtAddressReceive.Name = "txtAddressReceive";
            this.txtAddressReceive.Size = new System.Drawing.Size(200, 23);
            this.txtAddressReceive.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Blue;
            this.label9.Location = new System.Drawing.Point(555, 33);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 16);
            this.label9.TabIndex = 0;
            this.label9.Text = "Ghi chú";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.Blue;
            this.label11.Location = new System.Drawing.Point(353, 33);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(61, 16);
            this.label11.TabIndex = 0;
            this.label11.Text = "Sử dụng";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.Blue;
            this.label12.Location = new System.Drawing.Point(39, 33);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(80, 16);
            this.label12.TabIndex = 0;
            this.label12.Text = "Địa chỉ mail";
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Visible = false;
            // 
            // MailType
            // 
            this.MailType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MailType.DataPropertyName = "MailTypeName";
            this.MailType.FillWeight = 116.4129F;
            this.MailType.HeaderText = "Loại mail";
            this.MailType.Name = "MailType";
            this.MailType.ReadOnly = true;
            // 
            // Address
            // 
            this.Address.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Address.DataPropertyName = "Address";
            this.Address.FillWeight = 116.4129F;
            this.Address.HeaderText = "Địa chỉ mail";
            this.Address.Name = "Address";
            this.Address.ReadOnly = true;
            // 
            // MailTypeId
            // 
            this.MailTypeId.DataPropertyName = "MailTypeId";
            this.MailTypeId.HeaderText = "MailTypeId";
            this.MailTypeId.Name = "MailTypeId";
            this.MailTypeId.ReadOnly = true;
            this.MailTypeId.Visible = false;
            // 
            // IsActive
            // 
            this.IsActive.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.IsActive.DataPropertyName = "IsActive";
            this.IsActive.FillWeight = 50F;
            this.IsActive.HeaderText = "Trạng thái sử dụng";
            this.IsActive.Name = "IsActive";
            this.IsActive.ReadOnly = true;
            this.IsActive.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IsActive.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.IsActive.Width = 50;
            // 
            // Note
            // 
            this.Note.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Note.DataPropertyName = "Note";
            this.Note.FillWeight = 116.4129F;
            this.Note.HeaderText = "Ghi chú";
            this.Note.Name = "Note";
            this.Note.ReadOnly = true;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "IsDeleted";
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Visible = false;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "MAIL_TYPE";
            this.Column2.HeaderText = "Column2";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Visible = false;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "MAIL_TEAMPLATE";
            this.Column3.HeaderText = "Column3";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Visible = false;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "PassWord";
            this.Column4.HeaderText = "Column4";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Visible = false;
            // 
            // FrmMailSend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1071, 496);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(640, 475);
            this.Name = "FrmMailSend";
            this.Text = "Cấu hình mail";
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmMailSend_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvListMailSend)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvListMailReceive)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Button btnDelete_s;
        private System.Windows.Forms.Button btnCancel_s;
        private System.Windows.Forms.Button btnUpdate_s;
        private System.Windows.Forms.Button btnAdd_s;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkIsActive;
        private System.Windows.Forms.RichTextBox txtNote;
        private System.Windows.Forms.ComboBox cbbMailTypeSend;
        private System.Windows.Forms.TextBox txtRePassword;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnDelete_r;
        private System.Windows.Forms.Button btnCancel_r;
        private System.Windows.Forms.Button btnUpdate_r;
        private System.Windows.Forms.Button btnAdd_r;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckBox chkIsActiveReceive;
        private System.Windows.Forms.RichTextBox txtNoteReceive;
        private System.Windows.Forms.TextBox txtAddressReceive;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DataGridView dgvListMailSend;
        private System.Windows.Forms.DataGridView dgvListMailReceive;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdR;
        private System.Windows.Forms.DataGridViewTextBoxColumn AddressR;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsActiveR;
        private System.Windows.Forms.DataGridViewTextBoxColumn NoteR;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsDeleted;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn MailType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Address;
        private System.Windows.Forms.DataGridViewTextBoxColumn MailTypeId;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsActive;
        private System.Windows.Forms.DataGridViewTextBoxColumn Note;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
    }
}