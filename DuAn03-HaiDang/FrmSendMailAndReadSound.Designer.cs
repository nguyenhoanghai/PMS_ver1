namespace DuAn03_HaiDang
{
    partial class FrmSendMailAndReadSound
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
            this.components = new System.ComponentModel.Container();
            this.timerSendMailAndReadSound = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timerSendMailAndReadSound
            // 
            this.timerSendMailAndReadSound.Tick += new System.EventHandler(this.timerSendMailAndReadSound_Tick);
            // 
            // FrmSendMailAndReadSound
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 83);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FrmSendMailAndReadSound";
            this.Text = "Gửi mail và đọc âm thanh";
            this.Load += new System.EventHandler(this.FrmSendMailAndReadSound_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timerSendMailAndReadSound;
    }
}