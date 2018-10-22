namespace QuanLyNangSuat
{
    partial class FrmSetupLCDConfig
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
            this.label1 = new System.Windows.Forms.Label();
            this.cbbLCDType = new System.Windows.Forms.ComboBox();
            this.butEdit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Chọn Màn Hình LCD:";
            // 
            // cbbLCDType
            // 
            this.cbbLCDType.FormattingEnabled = true;
            this.cbbLCDType.Items.AddRange(new object[] {
            "Màn Hình Năng Suất Chuyền",
            "Màn Hình Bảng KanBan",
            "Màn Hình Tổng Hợp",
            "Màn Hình Lỗi",
            "Màn Hình Năng Suất Cụm"});
            this.cbbLCDType.Location = new System.Drawing.Point(119, 16);
            this.cbbLCDType.Name = "cbbLCDType";
            this.cbbLCDType.Size = new System.Drawing.Size(197, 21);
            this.cbbLCDType.TabIndex = 1;
            // 
            // butEdit
            // 
            this.butEdit.Location = new System.Drawing.Point(322, 15);
            this.butEdit.Name = "butEdit";
            this.butEdit.Size = new System.Drawing.Size(75, 23);
            this.butEdit.TabIndex = 2;
            this.butEdit.Text = "Chỉnh Sửa";
            this.butEdit.UseVisualStyleBackColor = true;
            this.butEdit.Click += new System.EventHandler(this.butEdit_Click);
            // 
            // FrmSetupLCDConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 53);
            this.Controls.Add(this.butEdit);
            this.Controls.Add(this.cbbLCDType);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(422, 92);
            this.MinimumSize = new System.Drawing.Size(422, 92);
            this.Name = "FrmSetupLCDConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cấu Hình Giao Diện Màn Hình LCD";
            this.Load += new System.EventHandler(this.FrmSetupLCDConfig_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbbLCDType;
        private System.Windows.Forms.Button butEdit;

    }
}