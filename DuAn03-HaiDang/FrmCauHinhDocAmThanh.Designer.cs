namespace DuAn03_HaiDang
{
    partial class FrmCauHinhDocAmThanh
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dgListConfig = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chonXoa = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ConfigName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsActive = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.butClose = new System.Windows.Forms.Button();
            this.butDelete = new System.Windows.Forms.Button();
            this.butAdd = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgListConfig)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.Controls.Add(this.dgListConfig, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(771, 369);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // dgListConfig
            // 
            this.dgListConfig.AllowUserToAddRows = false;
            this.dgListConfig.AllowUserToDeleteRows = false;
            this.dgListConfig.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgListConfig.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.chonXoa,
            this.ConfigName,
            this.Description,
            this.IsActive});
            this.dgListConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgListConfig.Location = new System.Drawing.Point(4, 53);
            this.dgListConfig.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgListConfig.Name = "dgListConfig";
            this.dgListConfig.Size = new System.Drawing.Size(763, 312);
            this.dgListConfig.TabIndex = 1;
            this.dgListConfig.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgListConfig_CellClick);
            // 
            // Id
            // 
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.Visible = false;
            // 
            // chonXoa
            // 
            this.chonXoa.HeaderText = "Xoá";
            this.chonXoa.Name = "chonXoa";
            this.chonXoa.Width = 50;
            // 
            // ConfigName
            // 
            this.ConfigName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Blue;
            this.ConfigName.DefaultCellStyle = dataGridViewCellStyle1;
            this.ConfigName.FillWeight = 89.0863F;
            this.ConfigName.HeaderText = "Tên cấu hình";
            this.ConfigName.Name = "ConfigName";
            // 
            // Description
            // 
            this.Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Description.FillWeight = 89.0863F;
            this.Description.HeaderText = "Mô tả";
            this.Description.Name = "Description";
            // 
            // IsActive
            // 
            this.IsActive.FillWeight = 121.8274F;
            this.IsActive.HeaderText = "Sử dụng";
            this.IsActive.Name = "IsActive";
            this.IsActive.Width = 80;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.btnCopy);
            this.panel1.Controls.Add(this.butClose);
            this.panel1.Controls.Add(this.butDelete);
            this.panel1.Controls.Add(this.butAdd);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(4, 4);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(763, 41);
            this.panel1.TabIndex = 2;
            // 
            // butClose
            // 
            this.butClose.Location = new System.Drawing.Point(651, 6);
            this.butClose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.butClose.Name = "butClose";
            this.butClose.Size = new System.Drawing.Size(100, 28);
            this.butClose.TabIndex = 3;
            this.butClose.Text = "Đóng";
            this.butClose.UseVisualStyleBackColor = true;
            this.butClose.Click += new System.EventHandler(this.butClose_Click);
            // 
            // butDelete
            // 
            this.butDelete.Location = new System.Drawing.Point(120, 6);
            this.butDelete.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.butDelete.Name = "butDelete";
            this.butDelete.Size = new System.Drawing.Size(100, 28);
            this.butDelete.TabIndex = 3;
            this.butDelete.Text = "Xoá";
            this.butDelete.UseVisualStyleBackColor = true;
            this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
            // 
            // butAdd
            // 
            this.butAdd.Location = new System.Drawing.Point(12, 6);
            this.butAdd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.butAdd.Name = "butAdd";
            this.butAdd.Size = new System.Drawing.Size(100, 28);
            this.butAdd.TabIndex = 3;
            this.butAdd.Text = "Thêm mới";
            this.butAdd.UseVisualStyleBackColor = true;
            this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(228, 6);
            this.btnCopy.Margin = new System.Windows.Forms.Padding(4);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(210, 28);
            this.btnCopy.TabIndex = 4;
            this.btnCopy.Text = "Sao chép cấu hình đọc từ ...";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(543, 6);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 28);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "Làm mới lưới";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // FrmCauHinhDocAmThanh
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(771, 369);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "FrmCauHinhDocAmThanh";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Danh sách cấu hình đọc âm thanh";
            this.Load += new System.EventHandler(this.FrmCauHinhDocAmThanh_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgListConfig)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dgListConfig;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button butClose;
        private System.Windows.Forms.Button butDelete;
        private System.Windows.Forms.Button butAdd;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chonXoa;
        private System.Windows.Forms.DataGridViewTextBoxColumn ConfigName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsActive;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnRefresh;
    }
}