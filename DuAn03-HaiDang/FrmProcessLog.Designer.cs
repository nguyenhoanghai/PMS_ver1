namespace QuanLyNangSuat
{
    partial class FrmProcessLog
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
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.butClear = new System.Windows.Forms.Button();
            this.butReload = new System.Windows.Forms.Button();
            this.butOnOffLog = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(3, 2);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(645, 319);
            this.txtLog.TabIndex = 0;
            this.txtLog.Text = "";
            // 
            // butClear
            // 
            this.butClear.Location = new System.Drawing.Point(299, 330);
            this.butClear.Name = "butClear";
            this.butClear.Size = new System.Drawing.Size(75, 23);
            this.butClear.TabIndex = 1;
            this.butClear.Text = "Clear";
            this.butClear.UseVisualStyleBackColor = true;
            this.butClear.Click += new System.EventHandler(this.butClear_Click);
            // 
            // butReload
            // 
            this.butReload.Location = new System.Drawing.Point(431, 330);
            this.butReload.Name = "butReload";
            this.butReload.Size = new System.Drawing.Size(75, 23);
            this.butReload.TabIndex = 1;
            this.butReload.Text = "Reload";
            this.butReload.UseVisualStyleBackColor = true;
            this.butReload.Click += new System.EventHandler(this.butReload_Click);
            // 
            // butOnOffLog
            // 
            this.butOnOffLog.Location = new System.Drawing.Point(162, 330);
            this.butOnOffLog.Name = "butOnOffLog";
            this.butOnOffLog.Size = new System.Drawing.Size(93, 23);
            this.butOnOffLog.TabIndex = 2;
            this.butOnOffLog.Text = "Write Log (Off)";
            this.butOnOffLog.UseVisualStyleBackColor = true;
            this.butOnOffLog.Click += new System.EventHandler(this.butOnOffLog_Click);
            // 
            // FrmProcessLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 365);
            this.Controls.Add(this.butOnOffLog);
            this.Controls.Add(this.butReload);
            this.Controls.Add(this.butClear);
            this.Controls.Add(this.txtLog);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(667, 404);
            this.MinimumSize = new System.Drawing.Size(667, 404);
            this.Name = "FrmProcessLog";
            this.Text = "FrmProcessLog";
            this.Load += new System.EventHandler(this.FrmProcessLog_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtLog;
        private System.Windows.Forms.Button butClear;
        private System.Windows.Forms.Button butReload;
        private System.Windows.Forms.Button butOnOffLog;
    }
}