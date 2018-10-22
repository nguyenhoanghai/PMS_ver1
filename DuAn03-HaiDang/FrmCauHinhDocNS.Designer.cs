namespace DuAn03_HaiDang
{
    partial class FrmCauHinhDocNS
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgReadSoundConfig = new System.Windows.Forms.DataGridView();
            this.MaChuyen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TenChuyen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sound = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.STTReadNS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ThayDoiThuTu = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ThayDoiFileAmThanh = new System.Windows.Forms.DataGridViewButtonColumn();
            this.CauHinhDocAmThanh = new System.Windows.Forms.DataGridViewButtonColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgListTime = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SoLanDoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsActive = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.timeThoiGianDoc = new DevExpress.XtraEditors.TimeEdit();
            this.chkIsActive = new System.Windows.Forms.CheckBox();
            this.txtSoLanDoc = new System.Windows.Forms.MaskedTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.butDelete = new System.Windows.Forms.Button();
            this.butSave = new System.Windows.Forms.Button();
            this.butUpdate = new System.Windows.Forms.Button();
            this.butAdd = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgReadSoundConfig)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgListTime)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeThoiGianDoc.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(666, 487);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgReadSoundConfig);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 295);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(660, 189);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Danh sách chuyền";
            // 
            // dgReadSoundConfig
            // 
            this.dgReadSoundConfig.AllowUserToAddRows = false;
            this.dgReadSoundConfig.AllowUserToDeleteRows = false;
            this.dgReadSoundConfig.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgReadSoundConfig.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MaChuyen,
            this.TenChuyen,
            this.Sound,
            this.STTReadNS,
            this.ThayDoiThuTu,
            this.ThayDoiFileAmThanh,
            this.CauHinhDocAmThanh});
            this.dgReadSoundConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgReadSoundConfig.Location = new System.Drawing.Point(3, 16);
            this.dgReadSoundConfig.Name = "dgReadSoundConfig";
            this.dgReadSoundConfig.Size = new System.Drawing.Size(654, 170);
            this.dgReadSoundConfig.TabIndex = 0;
            this.dgReadSoundConfig.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgReadSoundConfig_CellContentClick);
            // 
            // MaChuyen
            // 
            this.MaChuyen.HeaderText = "IdChuyen";
            this.MaChuyen.Name = "MaChuyen";
            this.MaChuyen.Visible = false;
            // 
            // TenChuyen
            // 
            this.TenChuyen.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TenChuyen.HeaderText = "Tên chuyền";
            this.TenChuyen.Name = "TenChuyen";
            // 
            // Sound
            // 
            this.Sound.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Sound.HeaderText = "File âm thanh";
            this.Sound.Name = "Sound";
            // 
            // STTReadNS
            // 
            this.STTReadNS.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.STTReadNS.HeaderText = "Thứ tự đọc";
            this.STTReadNS.Name = "STTReadNS";
            // 
            // ThayDoiThuTu
            // 
            this.ThayDoiThuTu.HeaderText = "Thay đổi thứ tự";
            this.ThayDoiThuTu.Name = "ThayDoiThuTu";
            this.ThayDoiThuTu.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ThayDoiThuTu.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ThayDoiThuTu.Width = 106;
            // 
            // ThayDoiFileAmThanh
            // 
            this.ThayDoiFileAmThanh.HeaderText = "Thay đổi file đọc";
            this.ThayDoiFileAmThanh.Name = "ThayDoiFileAmThanh";
            this.ThayDoiFileAmThanh.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ThayDoiFileAmThanh.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ThayDoiFileAmThanh.Visible = false;
            this.ThayDoiFileAmThanh.Width = 126;
            // 
            // CauHinhDocAmThanh
            // 
            this.CauHinhDocAmThanh.HeaderText = "Cấu hình đọc âm thanh";
            this.CauHinhDocAmThanh.Name = "CauHinhDocAmThanh";
            this.CauHinhDocAmThanh.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CauHinhDocAmThanh.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.CauHinhDocAmThanh.Width = 146;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(660, 286);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Thời gian đọc âm thanh";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.groupBox3, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.groupBox4, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(654, 267);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dgListTime);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 83);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(648, 181);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Danh sách thời gian đọc trong ngày";
            // 
            // dgListTime
            // 
            this.dgListTime.AllowUserToAddRows = false;
            this.dgListTime.AllowUserToDeleteRows = false;
            this.dgListTime.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgListTime.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.Time,
            this.SoLanDoc,
            this.IsActive});
            this.dgListTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgListTime.Location = new System.Drawing.Point(3, 16);
            this.dgListTime.Name = "dgListTime";
            this.dgListTime.ReadOnly = true;
            this.dgListTime.Size = new System.Drawing.Size(642, 162);
            this.dgListTime.TabIndex = 0;
            this.dgListTime.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgListTime_CellClick);
            // 
            // Id
            // 
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Visible = false;
            // 
            // Time
            // 
            this.Time.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Time.HeaderText = "Thời gian đọc";
            this.Time.Name = "Time";
            this.Time.ReadOnly = true;
            // 
            // SoLanDoc
            // 
            this.SoLanDoc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SoLanDoc.HeaderText = "Số lần đọc";
            this.SoLanDoc.Name = "SoLanDoc";
            this.SoLanDoc.ReadOnly = true;
            // 
            // IsActive
            // 
            this.IsActive.HeaderText = "Sử dụng";
            this.IsActive.Name = "IsActive";
            this.IsActive.ReadOnly = true;
            this.IsActive.Width = 80;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.timeThoiGianDoc);
            this.groupBox4.Controls.Add(this.chkIsActive);
            this.groupBox4.Controls.Add(this.txtSoLanDoc);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.butDelete);
            this.groupBox4.Controls.Add(this.butSave);
            this.groupBox4.Controls.Add(this.butUpdate);
            this.groupBox4.Controls.Add(this.butAdd);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(3, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(648, 74);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Thông tin thời gian";
            // 
            // timeThoiGianDoc
            // 
            this.timeThoiGianDoc.EditValue = new System.DateTime(2015, 5, 13, 0, 0, 0, 0);
            this.timeThoiGianDoc.Location = new System.Drawing.Point(115, 16);
            this.timeThoiGianDoc.Name = "timeThoiGianDoc";
            this.timeThoiGianDoc.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.timeThoiGianDoc.Size = new System.Drawing.Size(100, 20);
            this.timeThoiGianDoc.TabIndex = 20;
            // 
            // chkIsActive
            // 
            this.chkIsActive.AutoSize = true;
            this.chkIsActive.Checked = true;
            this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsActive.Location = new System.Drawing.Point(500, 19);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(66, 17);
            this.chkIsActive.TabIndex = 19;
            this.chkIsActive.Text = "Sử dụng";
            this.chkIsActive.UseVisualStyleBackColor = true;
            // 
            // txtSoLanDoc
            // 
            this.txtSoLanDoc.Location = new System.Drawing.Point(319, 16);
            this.txtSoLanDoc.Mask = "00000";
            this.txtSoLanDoc.Name = "txtSoLanDoc";
            this.txtSoLanDoc.Size = new System.Drawing.Size(100, 20);
            this.txtSoLanDoc.TabIndex = 18;
            this.txtSoLanDoc.ValidatingType = typeof(int);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(447, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Sử dụng:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(253, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Số lần đọc:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Thời gian đọc:";
            // 
            // butDelete
            // 
            this.butDelete.Location = new System.Drawing.Point(435, 45);
            this.butDelete.Name = "butDelete";
            this.butDelete.Size = new System.Drawing.Size(75, 23);
            this.butDelete.TabIndex = 13;
            this.butDelete.Text = "Xoá";
            this.butDelete.UseVisualStyleBackColor = true;
            this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
            // 
            // butSave
            // 
            this.butSave.Location = new System.Drawing.Point(339, 45);
            this.butSave.Name = "butSave";
            this.butSave.Size = new System.Drawing.Size(75, 23);
            this.butSave.TabIndex = 14;
            this.butSave.Text = "Lưu";
            this.butSave.UseVisualStyleBackColor = true;
            this.butSave.Click += new System.EventHandler(this.butSave_Click);
            // 
            // butUpdate
            // 
            this.butUpdate.Location = new System.Drawing.Point(240, 45);
            this.butUpdate.Name = "butUpdate";
            this.butUpdate.Size = new System.Drawing.Size(75, 23);
            this.butUpdate.TabIndex = 15;
            this.butUpdate.Text = "Sửa";
            this.butUpdate.UseVisualStyleBackColor = true;
            // 
            // butAdd
            // 
            this.butAdd.Location = new System.Drawing.Point(138, 46);
            this.butAdd.Name = "butAdd";
            this.butAdd.Size = new System.Drawing.Size(75, 23);
            this.butAdd.TabIndex = 16;
            this.butAdd.Text = "Thêm";
            this.butAdd.UseVisualStyleBackColor = true;
            // 
            // FrmCauHinhDocNS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 487);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FrmCauHinhDocNS";
            this.Text = "Cấu hình đọc năng suất";
            this.Load += new System.EventHandler(this.FrmCauHinhDocNS_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgReadSoundConfig)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgListTime)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeThoiGianDoc.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgReadSoundConfig;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dgListTime;
        private System.Windows.Forms.GroupBox groupBox4;
        private DevExpress.XtraEditors.TimeEdit timeThoiGianDoc;
        private System.Windows.Forms.CheckBox chkIsActive;
        private System.Windows.Forms.MaskedTextBox txtSoLanDoc;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button butDelete;
        private System.Windows.Forms.Button butSave;
        private System.Windows.Forms.Button butUpdate;
        private System.Windows.Forms.Button butAdd;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn SoLanDoc;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsActive;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaChuyen;
        private System.Windows.Forms.DataGridViewTextBoxColumn TenChuyen;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sound;
        private System.Windows.Forms.DataGridViewTextBoxColumn STTReadNS;
        private System.Windows.Forms.DataGridViewButtonColumn ThayDoiThuTu;
        private System.Windows.Forms.DataGridViewButtonColumn ThayDoiFileAmThanh;
        private System.Windows.Forms.DataGridViewButtonColumn CauHinhDocAmThanh;
    }
}