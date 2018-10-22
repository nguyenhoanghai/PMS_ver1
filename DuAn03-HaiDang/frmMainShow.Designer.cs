namespace QuanLyNangSuat
{
    partial class frmMainShow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMainShow));
            this.btnNS = new DevExpress.XtraEditors.SimpleButton();
            this.btnCollec = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // btnNS
            // 
            this.btnNS.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNS.Appearance.Options.UseFont = true;
            this.btnNS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnNS.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNS.Image = global::QuanLyNangSuat.Properties.Resources._1470044029_video_television;
            this.btnNS.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
            this.btnNS.Location = new System.Drawing.Point(7, 9);
            this.btnNS.Name = "btnNS";
            this.btnNS.Size = new System.Drawing.Size(206, 170);
            this.btnNS.TabIndex = 4;
            this.btnNS.Text = "Màn Hình Năng Suất";
            this.btnNS.Click += new System.EventHandler(this.btnNS_Click);
            // 
            // btnCollec
            // 
            this.btnCollec.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCollec.Appearance.Options.UseFont = true;
            this.btnCollec.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCollec.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCollec.Image = global::QuanLyNangSuat.Properties.Resources._1470044186_cmyk_06;
            this.btnCollec.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
            this.btnCollec.Location = new System.Drawing.Point(219, 9);
            this.btnCollec.Name = "btnCollec";
            this.btnCollec.Size = new System.Drawing.Size(206, 170);
            this.btnCollec.TabIndex = 5;
            this.btnCollec.Text = "Màn Hình Tổng Hợp";
            this.btnCollec.Click += new System.EventHandler(this.btnCollec_Click);
            // 
            // frmMainShow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 189);
            this.Controls.Add(this.btnCollec);
            this.Controls.Add(this.btnNS);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(451, 228);
            this.MinimumSize = new System.Drawing.Size(451, 228);
            this.Name = "frmMainShow";
            this.Text = "Màn Hình Hiển Thị";
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnCollec;
        private DevExpress.XtraEditors.SimpleButton btnNS;
    }
}