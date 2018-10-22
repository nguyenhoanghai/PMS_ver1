namespace QuanLyNangSuat
{
    partial class FrmKanbanLightPercent
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btncancel = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.dgTyLe = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.cbtile = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgthongtinphantyle = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.dg_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgmachuyen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgTenchuyen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgCommoId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgCommo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgTenTyLe = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.btnSave_ = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.dgId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lightColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgPercentFrom = new DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn();
            this.dgPercentTo = new DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgTyLe)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgthongtinphantyle)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.btncancel);
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.btnUpdate);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.dgTyLe);
            this.groupBox1.Controls.Add(this.cbtile);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(494, 502);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thông tin tỉ lệ";
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Image = global::QuanLyNangSuat.Properties.Resources._1486752393_floppy_disk_save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.Location = new System.Drawing.Point(112, 442);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(108, 41);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Lưu lại";
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btncancel
            // 
            this.btncancel.Image = global::QuanLyNangSuat.Properties.Resources._1464012624_Cancel;
            this.btncancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btncancel.Location = new System.Drawing.Point(243, 442);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(108, 41);
            this.btncancel.TabIndex = 6;
            this.btncancel.Text = "Hủy bỏ";
            this.btncancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btncancel.UseVisualStyleBackColor = true;
            this.btncancel.Click += new System.EventHandler(this.btncancel_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::QuanLyNangSuat.Properties.Resources.deletee;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDelete.Location = new System.Drawing.Point(308, 390);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(108, 41);
            this.btnDelete.TabIndex = 5;
            this.btnDelete.Text = "Xóa";
            this.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Image = global::QuanLyNangSuat.Properties.Resources.edit;
            this.btnUpdate.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnUpdate.Location = new System.Drawing.Point(176, 390);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(108, 41);
            this.btnUpdate.TabIndex = 4;
            this.btnUpdate.Text = "Cập nhật";
            this.btnUpdate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Image = global::QuanLyNangSuat.Properties.Resources.add;
            this.btnAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAdd.Location = new System.Drawing.Point(49, 390);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(108, 41);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "Thêm";
            this.btnAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dgTyLe
            // 
            this.dgTyLe.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgTyLe.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgId,
            this.lightColor,
            this.dgPercentFrom,
            this.dgPercentTo});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgTyLe.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgTyLe.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgTyLe.Location = new System.Drawing.Point(3, 72);
            this.dgTyLe.Margin = new System.Windows.Forms.Padding(0);
            this.dgTyLe.Name = "dgTyLe";
            this.dgTyLe.Size = new System.Drawing.Size(487, 302);
            this.dgTyLe.TabIndex = 2;
            // 
            // cbtile
            // 
            this.cbtile.FormattingEnabled = true;
            this.cbtile.Location = new System.Drawing.Point(124, 34);
            this.cbtile.Name = "cbtile";
            this.cbtile.Size = new System.Drawing.Size(326, 26);
            this.cbtile.TabIndex = 1;
            this.cbtile.SelectedIndexChanged += new System.EventHandler(this.cbtile_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tên loại tỉ lệ ";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgthongtinphantyle);
            this.groupBox2.Controls.Add(this.btnSave_);
            this.groupBox2.Controls.Add(this.btnRefresh);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(512, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(644, 502);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Thông tin phân công tỉ lệ";
            // 
            // dgthongtinphantyle
            // 
            this.dgthongtinphantyle.AllowUserToDeleteRows = false;
            this.dgthongtinphantyle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgthongtinphantyle.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dg_Id,
            this.dgmachuyen,
            this.dgTenchuyen,
            this.dgCommoId,
            this.dgCommo,
            this.dgTenTyLe});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgthongtinphantyle.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgthongtinphantyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgthongtinphantyle.Location = new System.Drawing.Point(6, 28);
            this.dgthongtinphantyle.Name = "dgthongtinphantyle";
            this.dgthongtinphantyle.Size = new System.Drawing.Size(632, 403);
            this.dgthongtinphantyle.TabIndex = 8;
            this.dgthongtinphantyle.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgthongtinphantyle_DataError);
            // 
            // dg_Id
            // 
            this.dg_Id.HeaderText = "Column1";
            this.dg_Id.Name = "dg_Id";
            this.dg_Id.Visible = false;
            // 
            // dgmachuyen
            // 
            this.dgmachuyen.HeaderText = "Mã Chuyền";
            this.dgmachuyen.Name = "dgmachuyen";
            this.dgmachuyen.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgmachuyen.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgmachuyen.Visible = false;
            // 
            // dgTenchuyen
            // 
            this.dgTenchuyen.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dgTenchuyen.HeaderText = "Tên Chuyền";
            this.dgTenchuyen.Name = "dgTenchuyen";
            this.dgTenchuyen.ReadOnly = true;
            this.dgTenchuyen.Width = 150;
            // 
            // dgCommoId
            // 
            this.dgCommoId.HeaderText = "Column1";
            this.dgCommoId.Name = "dgCommoId";
            this.dgCommoId.Visible = false;
            // 
            // dgCommo
            // 
            this.dgCommo.HeaderText = "Mã Hàng";
            this.dgCommo.Name = "dgCommo";
            // 
            // dgTenTyLe
            // 
            this.dgTenTyLe.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgTenTyLe.HeaderText = "Loại Tỷ lệ";
            this.dgTenTyLe.Name = "dgTenTyLe";
            this.dgTenTyLe.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgTenTyLe.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // btnSave_
            // 
            this.btnSave_.Image = global::QuanLyNangSuat.Properties.Resources._1486752393_floppy_disk_save;
            this.btnSave_.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave_.Location = new System.Drawing.Point(205, 444);
            this.btnSave_.Name = "btnSave_";
            this.btnSave_.Size = new System.Drawing.Size(108, 41);
            this.btnSave_.TabIndex = 7;
            this.btnSave_.Text = "Lưu lại";
            this.btnSave_.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave_.UseVisualStyleBackColor = true;
            this.btnSave_.Click += new System.EventHandler(this.btnSave__Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = global::QuanLyNangSuat.Properties.Resources.refresh;
            this.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRefresh.Location = new System.Drawing.Point(341, 444);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(108, 41);
            this.btnRefresh.TabIndex = 6;
            this.btnRefresh.Text = "Làm mới";
            this.btnRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // dgId
            // 
            this.dgId.HeaderText = "Mã";
            this.dgId.Name = "dgId";
            this.dgId.Visible = false;
            // 
            // lightColor
            // 
            this.lightColor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.lightColor.HeaderText = "Màu đèn";
            this.lightColor.Name = "lightColor";
            // 
            // dgPercentFrom
            // 
            // 
            // 
            // 
            this.dgPercentFrom.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window;
            this.dgPercentFrom.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.dgPercentFrom.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dgPercentFrom.BackgroundStyle.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.dgPercentFrom.HeaderText = "tỉ lệ từ";
            this.dgPercentFrom.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left;
            this.dgPercentFrom.Name = "dgPercentFrom";
            this.dgPercentFrom.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgPercentFrom.Width = 80;
            // 
            // dgPercentTo
            // 
            // 
            // 
            // 
            this.dgPercentTo.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window;
            this.dgPercentTo.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.dgPercentTo.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dgPercentTo.BackgroundStyle.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.dgPercentTo.HeaderText = "tỉ lệ đến";
            this.dgPercentTo.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left;
            this.dgPercentTo.Name = "dgPercentTo";
            this.dgPercentTo.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgPercentTo.Width = 80;
            // 
            // FrmKanbanLightPercent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1165, 525);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FrmKanbanLightPercent";
            this.Text = "Cài đặt tỉ lệ đèn Kanban";
            this.Load += new System.EventHandler(this.FrmKanbanLightPercent_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgTyLe)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgthongtinphantyle)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btncancel;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAdd;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgTyLe;
        private System.Windows.Forms.ComboBox cbtile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgthongtinphantyle;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgmachuyen;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgTenchuyen;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgCommoId;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgCommo;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgTenTyLe;
        private System.Windows.Forms.Button btnSave_;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgId;
        private System.Windows.Forms.DataGridViewTextBoxColumn lightColor;
        private DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn dgPercentFrom;
        private DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn dgPercentTo;
    }
}