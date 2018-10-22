namespace DuAn03_HaiDang
{
    partial class FrmConnectDatabase
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
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.butConnect = new DevComponents.DotNetBar.ButtonX();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.cboServerName = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cboAuthenticaion = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem1 = new DevComponents.Editors.ComboItem();
            this.comboItem2 = new DevComponents.Editors.ComboItem();
            this.txtUsername = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtPassword = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.cboDatabase = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelX1.Location = new System.Drawing.Point(16, 14);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "Server Name:";
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelX2.Location = new System.Drawing.Point(16, 47);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(86, 23);
            this.labelX2.TabIndex = 0;
            this.labelX2.Text = "Authentication:";
            // 
            // labelX3
            // 
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = "";
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelX3.Location = new System.Drawing.Point(16, 81);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(75, 23);
            this.labelX3.TabIndex = 0;
            this.labelX3.Text = "User Name:";
            // 
            // labelX4
            // 
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.Class = "";
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelX4.Location = new System.Drawing.Point(16, 118);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(60, 23);
            this.labelX4.TabIndex = 0;
            this.labelX4.Text = "Password:";
            // 
            // butConnect
            // 
            this.butConnect.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.butConnect.ColorTable = DevComponents.DotNetBar.eButtonColor.Office2007WithBackground;
            this.butConnect.Location = new System.Drawing.Point(68, 196);
            this.butConnect.Name = "butConnect";
            this.butConnect.Size = new System.Drawing.Size(75, 23);
            this.butConnect.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.butConnect.TabIndex = 1;
            this.butConnect.Text = "Connect";
            this.butConnect.Click += new System.EventHandler(this.butConnect_Click);
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.Office2007WithBackground;
            this.buttonX1.Location = new System.Drawing.Point(173, 196);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(75, 23);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1.TabIndex = 1;
            this.buttonX1.Text = "Cancel";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // cboServerName
            // 
            this.cboServerName.DisplayMember = "Text";
            this.cboServerName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboServerName.FormattingEnabled = true;
            this.cboServerName.ItemHeight = 15;
            this.cboServerName.Location = new System.Drawing.Point(108, 14);
            this.cboServerName.Name = "cboServerName";
            this.cboServerName.Size = new System.Drawing.Size(180, 21);
            this.cboServerName.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cboServerName.TabIndex = 2;
            this.cboServerName.DropDown += new System.EventHandler(this.cboServerName_DropDown);
            // 
            // cboAuthenticaion
            // 
            this.cboAuthenticaion.DisplayMember = "Text";
            this.cboAuthenticaion.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboAuthenticaion.FormattingEnabled = true;
            this.cboAuthenticaion.ItemHeight = 15;
            this.cboAuthenticaion.Items.AddRange(new object[] {
            this.comboItem1,
            this.comboItem2});
            this.cboAuthenticaion.Location = new System.Drawing.Point(108, 48);
            this.cboAuthenticaion.Name = "cboAuthenticaion";
            this.cboAuthenticaion.Size = new System.Drawing.Size(180, 21);
            this.cboAuthenticaion.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cboAuthenticaion.TabIndex = 2;
            this.cboAuthenticaion.SelectionChangeCommitted += new System.EventHandler(this.cboAuthenticaion_SelectionChangeCommitted);
            // 
            // comboItem1
            // 
            this.comboItem1.Text = "Windows Authentication";
            // 
            // comboItem2
            // 
            this.comboItem2.Text = "SQL Server Authentication";
            // 
            // txtUsername
            // 
            // 
            // 
            // 
            this.txtUsername.Border.Class = "TextBoxBorder";
            this.txtUsername.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtUsername.Location = new System.Drawing.Point(108, 84);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(180, 21);
            this.txtUsername.TabIndex = 3;
            // 
            // txtPassword
            // 
            // 
            // 
            // 
            this.txtPassword.Border.Class = "TextBoxBorder";
            this.txtPassword.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtPassword.Location = new System.Drawing.Point(108, 120);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '●';
            this.txtPassword.Size = new System.Drawing.Size(180, 21);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // labelX5
            // 
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.Class = "";
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelX5.Location = new System.Drawing.Point(11, 156);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(91, 23);
            this.labelX5.TabIndex = 0;
            this.labelX5.Text = "Database Name:";
            // 
            // cboDatabase
            // 
            this.cboDatabase.DisplayMember = "Text";
            this.cboDatabase.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboDatabase.FormattingEnabled = true;
            this.cboDatabase.ItemHeight = 15;
            this.cboDatabase.Location = new System.Drawing.Point(108, 156);
            this.cboDatabase.Name = "cboDatabase";
            this.cboDatabase.Size = new System.Drawing.Size(180, 21);
            this.cboDatabase.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cboDatabase.TabIndex = 2;
            this.cboDatabase.DropDown += new System.EventHandler(this.cboDatabase_DropDown);
            // 
            // FrmConnectDatabase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 227);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.cboAuthenticaion);
            this.Controls.Add(this.cboDatabase);
            this.Controls.Add(this.cboServerName);
            this.Controls.Add(this.buttonX1);
            this.Controls.Add(this.butConnect);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.labelX5);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.MaximizeBox = false;
            this.Name = "FrmConnectDatabase";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kết nối Cơ Sở Dữ Liệu";
            this.Load += new System.EventHandler(this.FrmConnectDatabase_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.ButtonX butConnect;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboServerName;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboAuthenticaion;
        private DevComponents.Editors.ComboItem comboItem1;
        private DevComponents.Editors.ComboItem comboItem2;
        private DevComponents.DotNetBar.Controls.TextBoxX txtUsername;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPassword;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboDatabase;
    }
}