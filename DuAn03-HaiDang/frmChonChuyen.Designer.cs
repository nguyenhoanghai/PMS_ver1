namespace QuanLyNangSuat
{
    partial class frmChonChuyen
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
            this.cbLine = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.butCopy = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbLine
            // 
            this.cbLine.FormattingEnabled = true;
            this.cbLine.Items.AddRange(new object[] {
            "Đọc số",
            "Đọc âm thanh"});
            this.cbLine.Location = new System.Drawing.Point(92, 13);
            this.cbLine.Margin = new System.Windows.Forms.Padding(4);
            this.cbLine.Name = "cbLine";
            this.cbLine.Size = new System.Drawing.Size(253, 24);
            this.cbLine.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 16);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 17);
            this.label4.TabIndex = 2;
            this.label4.Text = "Chuyền";
            // 
            // butCopy
            // 
            this.butCopy.Location = new System.Drawing.Point(137, 45);
            this.butCopy.Margin = new System.Windows.Forms.Padding(4);
            this.butCopy.Name = "butCopy";
            this.butCopy.Size = new System.Drawing.Size(100, 28);
            this.butCopy.TabIndex = 4;
            this.butCopy.Text = "Sao chép";
            this.butCopy.UseVisualStyleBackColor = true;
            this.butCopy.Click += new System.EventHandler(this.butCopy_Click);
            // 
            // butCancel
            // 
            this.butCancel.Location = new System.Drawing.Point(245, 45);
            this.butCancel.Margin = new System.Windows.Forms.Padding(4);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(100, 28);
            this.butCancel.TabIndex = 5;
            this.butCancel.Text = "Huỷ";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // frmChonChuyen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 86);
            this.Controls.Add(this.butCopy);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.cbLine);
            this.Controls.Add(this.label4);
            this.MaximumSize = new System.Drawing.Size(372, 133);
            this.MinimumSize = new System.Drawing.Size(372, 133);
            this.Name = "frmChonChuyen";
            this.Text = "Chọn chuyền";
            this.Load += new System.EventHandler(this.frmChonChuyen_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbLine;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button butCopy;
        private System.Windows.Forms.Button butCancel;
    }
}