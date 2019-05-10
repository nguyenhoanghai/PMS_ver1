namespace DuAn03_HaiDang
{
    partial class FrmCauHinhDocAmThanh_Create
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkIsActive = new System.Windows.Forms.CheckBox();
            this.txtDescription = new System.Windows.Forms.RichTextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.butDeleteItem = new System.Windows.Forms.Button();
            this.butHuyItem = new System.Windows.Forms.Button();
            this.butUpdateItem = new System.Windows.Forms.Button();
            this.butAddItem = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.chkIsActiveOfDetail = new System.Windows.Forms.CheckBox();
            this.txtThuTuDoc = new System.Windows.Forms.MaskedTextBox();
            this.cbbIntConfig = new System.Windows.Forms.ComboBox();
            this.cbbSound = new System.Windows.Forms.ComboBox();
            this.cbbLoaiFile = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.dgListItem = new System.Windows.Forms.DataGridView();
            this.IndexTypeSelect = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IdObj = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ValueFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ThuTuDoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsActiveOfDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.butSave = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgListItem)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkIsActive);
            this.groupBox1.Controls.Add(this.txtDescription);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 4);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(753, 106);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thông tin cấu hình";
            // 
            // chkIsActive
            // 
            this.chkIsActive.AutoSize = true;
            this.chkIsActive.Checked = true;
            this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsActive.Location = new System.Drawing.Point(141, 65);
            this.chkIsActive.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(83, 21);
            this.chkIsActive.TabIndex = 3;
            this.chkIsActive.Text = "Sử dụng";
            this.chkIsActive.UseVisualStyleBackColor = true;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(459, 30);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(268, 59);
            this.txtDescription.TabIndex = 2;
            this.txtDescription.Text = "";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(141, 30);
            this.txtName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(233, 22);
            this.txtName.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(397, 37);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Mô tả:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(57, 65);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Sử dụng:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 34);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tên cấu hình:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox6);
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Location = new System.Drawing.Point(4, 112);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(752, 409);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Chi tiết cấu hình";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.butDeleteItem);
            this.groupBox6.Controls.Add(this.butHuyItem);
            this.groupBox6.Controls.Add(this.butUpdateItem);
            this.groupBox6.Controls.Add(this.butAddItem);
            this.groupBox6.Location = new System.Drawing.Point(13, 124);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox6.Size = new System.Drawing.Size(726, 61);
            this.groupBox6.TabIndex = 6;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Thao tác";
            // 
            // butDeleteItem
            // 
            this.butDeleteItem.Location = new System.Drawing.Point(341, 23);
            this.butDeleteItem.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.butDeleteItem.Name = "butDeleteItem";
            this.butDeleteItem.Size = new System.Drawing.Size(100, 28);
            this.butDeleteItem.TabIndex = 0;
            this.butDeleteItem.Text = "Xoá";
            this.butDeleteItem.UseVisualStyleBackColor = true;
            this.butDeleteItem.Click += new System.EventHandler(this.butDeleteItem_Click);
            // 
            // butHuyItem
            // 
            this.butHuyItem.Location = new System.Drawing.Point(450, 23);
            this.butHuyItem.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.butHuyItem.Name = "butHuyItem";
            this.butHuyItem.Size = new System.Drawing.Size(100, 28);
            this.butHuyItem.TabIndex = 0;
            this.butHuyItem.Text = "Huỷ";
            this.butHuyItem.UseVisualStyleBackColor = true;
            // 
            // butUpdateItem
            // 
            this.butUpdateItem.Location = new System.Drawing.Point(233, 23);
            this.butUpdateItem.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.butUpdateItem.Name = "butUpdateItem";
            this.butUpdateItem.Size = new System.Drawing.Size(100, 28);
            this.butUpdateItem.TabIndex = 0;
            this.butUpdateItem.Text = "Sửa";
            this.butUpdateItem.UseVisualStyleBackColor = true;
            this.butUpdateItem.Click += new System.EventHandler(this.butUpdateItem_Click);
            // 
            // butAddItem
            // 
            this.butAddItem.Location = new System.Drawing.Point(125, 23);
            this.butAddItem.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.butAddItem.Name = "butAddItem";
            this.butAddItem.Size = new System.Drawing.Size(100, 28);
            this.butAddItem.TabIndex = 0;
            this.butAddItem.Text = "Thêm";
            this.butAddItem.UseVisualStyleBackColor = true;
            this.butAddItem.Click += new System.EventHandler(this.butAddItem_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.chkIsActiveOfDetail);
            this.groupBox5.Controls.Add(this.txtThuTuDoc);
            this.groupBox5.Controls.Add(this.cbbIntConfig);
            this.groupBox5.Controls.Add(this.cbbSound);
            this.groupBox5.Controls.Add(this.cbbLoaiFile);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Location = new System.Drawing.Point(15, 22);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox5.Size = new System.Drawing.Size(724, 95);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Thông tin của thành phần";
            // 
            // chkIsActiveOfDetail
            // 
            this.chkIsActiveOfDetail.AutoSize = true;
            this.chkIsActiveOfDetail.Checked = true;
            this.chkIsActiveOfDetail.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsActiveOfDetail.Location = new System.Drawing.Point(517, 55);
            this.chkIsActiveOfDetail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkIsActiveOfDetail.Name = "chkIsActiveOfDetail";
            this.chkIsActiveOfDetail.Size = new System.Drawing.Size(83, 21);
            this.chkIsActiveOfDetail.TabIndex = 3;
            this.chkIsActiveOfDetail.Text = "Sử dụng";
            this.chkIsActiveOfDetail.UseVisualStyleBackColor = true;
            // 
            // txtThuTuDoc
            // 
            this.txtThuTuDoc.Location = new System.Drawing.Point(517, 23);
            this.txtThuTuDoc.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtThuTuDoc.Mask = "00000";
            this.txtThuTuDoc.Name = "txtThuTuDoc";
            this.txtThuTuDoc.Size = new System.Drawing.Size(132, 22);
            this.txtThuTuDoc.TabIndex = 2;
            this.txtThuTuDoc.ValidatingType = typeof(int);
            // 
            // cbbIntConfig
            // 
            this.cbbIntConfig.FormattingEnabled = true;
            this.cbbIntConfig.Items.AddRange(new object[] {
            "Đọc số",
            "Đọc âm thanh"});
            this.cbbIntConfig.Location = new System.Drawing.Point(119, 60);
            this.cbbIntConfig.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbbIntConfig.Name = "cbbIntConfig";
            this.cbbIntConfig.Size = new System.Drawing.Size(253, 24);
            this.cbbIntConfig.TabIndex = 1;
            // 
            // cbbSound
            // 
            this.cbbSound.FormattingEnabled = true;
            this.cbbSound.Items.AddRange(new object[] {
            "Đọc số",
            "Đọc âm thanh"});
            this.cbbSound.Location = new System.Drawing.Point(119, 60);
            this.cbbSound.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbbSound.Name = "cbbSound";
            this.cbbSound.Size = new System.Drawing.Size(253, 24);
            this.cbbSound.TabIndex = 1;
            // 
            // cbbLoaiFile
            // 
            this.cbbLoaiFile.FormattingEnabled = true;
            this.cbbLoaiFile.Items.AddRange(new object[] {
            "Đọc số",
            "Đọc âm thanh"});
            this.cbbLoaiFile.Location = new System.Drawing.Point(119, 25);
            this.cbbLoaiFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbbLoaiFile.Name = "cbbLoaiFile";
            this.cbbLoaiFile.Size = new System.Drawing.Size(253, 24);
            this.cbbLoaiFile.TabIndex = 1;
            this.cbbLoaiFile.SelectedIndexChanged += new System.EventHandler(this.cbbLoaiFile_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(439, 57);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 17);
            this.label7.TabIndex = 0;
            this.label7.Text = "Sử dụng:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(424, 28);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 17);
            this.label6.TabIndex = 0;
            this.label6.Text = "Thứ tự đọc:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 63);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 17);
            this.label5.TabIndex = 0;
            this.label5.Text = "Chọn giá trị:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(41, 31);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "Loại file:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.dgListItem);
            this.groupBox4.Location = new System.Drawing.Point(8, 193);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox4.Size = new System.Drawing.Size(739, 208);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Danh sách các thành phần";
            // 
            // dgListItem
            // 
            this.dgListItem.AllowUserToAddRows = false;
            this.dgListItem.AllowUserToDeleteRows = false;
            this.dgListItem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgListItem.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IndexTypeSelect,
            this.FileType,
            this.IdObj,
            this.ValueFile,
            this.ThuTuDoc,
            this.IsActiveOfDetail});
            this.dgListItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgListItem.Location = new System.Drawing.Point(4, 19);
            this.dgListItem.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgListItem.Name = "dgListItem";
            this.dgListItem.ReadOnly = true;
            this.dgListItem.Size = new System.Drawing.Size(731, 185);
            this.dgListItem.TabIndex = 0;
            this.dgListItem.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgListItem_CellClick);
            // 
            // IndexTypeSelect
            // 
            this.IndexTypeSelect.HeaderText = "IndexTypeSelect";
            this.IndexTypeSelect.Name = "IndexTypeSelect";
            this.IndexTypeSelect.ReadOnly = true;
            this.IndexTypeSelect.Visible = false;
            // 
            // FileType
            // 
            this.FileType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FileType.HeaderText = "Loại file";
            this.FileType.Name = "FileType";
            this.FileType.ReadOnly = true;
            // 
            // IdObj
            // 
            this.IdObj.HeaderText = "IdObj";
            this.IdObj.Name = "IdObj";
            this.IdObj.ReadOnly = true;
            this.IdObj.Visible = false;
            // 
            // ValueFile
            // 
            this.ValueFile.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ValueFile.HeaderText = "Giá trị";
            this.ValueFile.Name = "ValueFile";
            this.ValueFile.ReadOnly = true;
            // 
            // ThuTuDoc
            // 
            this.ThuTuDoc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ThuTuDoc.HeaderText = "Thứ tự đọc";
            this.ThuTuDoc.Name = "ThuTuDoc";
            this.ThuTuDoc.ReadOnly = true;
            // 
            // IsActiveOfDetail
            // 
            this.IsActiveOfDetail.HeaderText = "Sử dụng";
            this.IsActiveOfDetail.Name = "IsActiveOfDetail";
            this.IsActiveOfDetail.ReadOnly = true;
            this.IsActiveOfDetail.Width = 70;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.butSave);
            this.groupBox3.Controls.Add(this.butCancel);
            this.groupBox3.Location = new System.Drawing.Point(460, 529);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Size = new System.Drawing.Size(296, 62);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Thao tác";
            // 
            // butSave
            // 
            this.butSave.Location = new System.Drawing.Point(39, 22);
            this.butSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.butSave.Name = "butSave";
            this.butSave.Size = new System.Drawing.Size(100, 28);
            this.butSave.TabIndex = 0;
            this.butSave.Text = "Lưu";
            this.butSave.UseVisualStyleBackColor = true;
            this.butSave.Click += new System.EventHandler(this.butSave_Click);
            // 
            // butCancel
            // 
            this.butCancel.Location = new System.Drawing.Point(168, 22);
            this.butCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(100, 28);
            this.butCancel.TabIndex = 0;
            this.butCancel.Text = "Huỷ";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // FrmCauHinhDocAmThanh_Create
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 600);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(779, 647);
            this.MinimumSize = new System.Drawing.Size(779, 647);
            this.Name = "FrmCauHinhDocAmThanh_Create";
            this.Text = "Thiết lập cấu hình đọc âm thanh";
            this.Load += new System.EventHandler(this.FrmCauHinhDocAmThanh_Create_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgListItem)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkIsActive;
        private System.Windows.Forms.RichTextBox txtDescription;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView dgListItem;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button butSave;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button butDeleteItem;
        private System.Windows.Forms.Button butHuyItem;
        private System.Windows.Forms.Button butUpdateItem;
        private System.Windows.Forms.Button butAddItem;
        private System.Windows.Forms.CheckBox chkIsActiveOfDetail;
        private System.Windows.Forms.MaskedTextBox txtThuTuDoc;
        private System.Windows.Forms.ComboBox cbbIntConfig;
        private System.Windows.Forms.ComboBox cbbSound;
        private System.Windows.Forms.ComboBox cbbLoaiFile;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridViewTextBoxColumn IndexTypeSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileType;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdObj;
        private System.Windows.Forms.DataGridViewTextBoxColumn ValueFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn ThuTuDoc;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsActiveOfDetail;
    }
}