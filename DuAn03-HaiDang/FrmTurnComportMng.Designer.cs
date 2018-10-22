namespace QuanLyNangSuat
{
    partial class FrmTurnComportMng
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
            this.butDelete = new System.Windows.Forms.Button();
            this.butHuy = new System.Windows.Forms.Button();
            this.butSave = new System.Windows.Forms.Button();
            this.butAdd = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.butUpdate = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkIsUse = new System.Windows.Forms.CheckBox();
            this.tTimeAction = new DevExpress.XtraEditors.TimeEdit();
            this.cbbStatus = new System.Windows.Forms.ComboBox();
            this.cbbComport = new System.Windows.Forms.ComboBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.butCluster = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.COMTypeId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.COMTypeName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.StatusId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Status = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TimeAction = new DevExpress.XtraGrid.Columns.GridColumn();
            this.IsActive = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.Description = new DevExpress.XtraGrid.Columns.GridColumn();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tTimeAction.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.butCluster)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // butDelete
            // 
            this.butDelete.Location = new System.Drawing.Point(372, 13);
            this.butDelete.Name = "butDelete";
            this.butDelete.Size = new System.Drawing.Size(75, 23);
            this.butDelete.TabIndex = 6;
            this.butDelete.Text = "Xoá";
            this.butDelete.UseVisualStyleBackColor = true;
            this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
            // 
            // butHuy
            // 
            this.butHuy.Location = new System.Drawing.Point(464, 13);
            this.butHuy.Name = "butHuy";
            this.butHuy.Size = new System.Drawing.Size(75, 23);
            this.butHuy.TabIndex = 7;
            this.butHuy.Text = "Huỷ";
            this.butHuy.UseVisualStyleBackColor = true;
            this.butHuy.Click += new System.EventHandler(this.butHuy_Click);
            // 
            // butSave
            // 
            this.butSave.Location = new System.Drawing.Point(276, 13);
            this.butSave.Name = "butSave";
            this.butSave.Size = new System.Drawing.Size(75, 23);
            this.butSave.TabIndex = 5;
            this.butSave.Text = "Lưu";
            this.butSave.UseVisualStyleBackColor = true;
            this.butSave.Click += new System.EventHandler(this.butSave_Click);
            // 
            // butAdd
            // 
            this.butAdd.Location = new System.Drawing.Point(75, 14);
            this.butAdd.Name = "butAdd";
            this.butAdd.Size = new System.Drawing.Size(75, 23);
            this.butAdd.TabIndex = 3;
            this.butAdd.Text = "Thêm";
            this.butAdd.UseVisualStyleBackColor = true;
            this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.butDelete);
            this.groupBox3.Controls.Add(this.butHuy);
            this.groupBox3.Controls.Add(this.butSave);
            this.groupBox3.Controls.Add(this.butUpdate);
            this.groupBox3.Controls.Add(this.butAdd);
            this.groupBox3.Location = new System.Drawing.Point(3, 123);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(610, 44);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Thao tác";
            // 
            // butUpdate
            // 
            this.butUpdate.Location = new System.Drawing.Point(177, 13);
            this.butUpdate.Name = "butUpdate";
            this.butUpdate.Size = new System.Drawing.Size(75, 23);
            this.butUpdate.TabIndex = 4;
            this.butUpdate.Text = "Sửa";
            this.butUpdate.UseVisualStyleBackColor = true;
            this.butUpdate.Click += new System.EventHandler(this.butUpdate_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkIsUse);
            this.groupBox1.Controls.Add(this.tTimeAction);
            this.groupBox1.Controls.Add(this.cbbStatus);
            this.groupBox1.Controls.Add(this.cbbComport);
            this.groupBox1.Controls.Add(this.txtDescription);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(610, 114);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thông tin tắt/mở cổng COM";
            // 
            // chkIsUse
            // 
            this.chkIsUse.AutoSize = true;
            this.chkIsUse.Checked = true;
            this.chkIsUse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsUse.Location = new System.Drawing.Point(121, 85);
            this.chkIsUse.Name = "chkIsUse";
            this.chkIsUse.Size = new System.Drawing.Size(64, 17);
            this.chkIsUse.TabIndex = 7;
            this.chkIsUse.Text = "sử dụng";
            this.chkIsUse.UseVisualStyleBackColor = true;
            // 
            // tTimeAction
            // 
            this.tTimeAction.EditValue = new System.DateTime(2015, 9, 15, 0, 0, 0, 0);
            this.tTimeAction.Location = new System.Drawing.Point(121, 54);
            this.tTimeAction.Name = "tTimeAction";
            this.tTimeAction.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.tTimeAction.Size = new System.Drawing.Size(175, 20);
            this.tTimeAction.TabIndex = 6;
            // 
            // cbbStatus
            // 
            this.cbbStatus.FormattingEnabled = true;
            this.cbbStatus.Items.AddRange(new object[] {
            "Tắt",
            "Mở"});
            this.cbbStatus.Location = new System.Drawing.Point(380, 20);
            this.cbbStatus.Name = "cbbStatus";
            this.cbbStatus.Size = new System.Drawing.Size(151, 21);
            this.cbbStatus.TabIndex = 5;
            // 
            // cbbComport
            // 
            this.cbbComport.FormattingEnabled = true;
            this.cbbComport.Items.AddRange(new object[] {
            "Cổng COM gửi dữ liệu xuống bảng điện tử",
            "Cổng COM truyền nhân số liệu"});
            this.cbbComport.Location = new System.Drawing.Point(121, 20);
            this.cbbComport.Name = "cbbComport";
            this.cbbComport.Size = new System.Drawing.Size(175, 21);
            this.cbbComport.TabIndex = 4;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(380, 52);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(216, 48);
            this.txtDescription.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Sử dụng cài đặt:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(315, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Trạng thái:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Thời gian thực hiện:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(318, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Mô tả:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Cổng COM:";
            // 
            // butCluster
            // 
            this.butCluster.AutoHeight = false;
            this.butCluster.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.butCluster.Name = "butCluster";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(813, 476);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.gridControl);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 173);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(807, 300);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Danh sách thông tin tắt mở cổng COM";
            // 
            // gridControl
            // 
            this.gridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl.Location = new System.Drawing.Point(3, 16);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Name = "gridControl";
            this.gridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.butCluster,
            this.repositoryItemCheckEdit1});
            this.gridControl.Size = new System.Drawing.Size(801, 281);
            this.gridControl.TabIndex = 9;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            // 
            // gridView
            // 
            this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.COMTypeId,
            this.COMTypeName,
            this.StatusId,
            this.Status,
            this.TimeAction,
            this.IsActive,
            this.Description});
            this.gridView.GridControl = this.gridControl;
            this.gridView.Name = "gridView";
            this.gridView.OptionsBehavior.Editable = false;
            this.gridView.OptionsView.ShowAutoFilterRow = true;
            this.gridView.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.gridView_RowCellClick);
            // 
            // COMTypeId
            // 
            this.COMTypeId.Caption = "gridColumn1";
            this.COMTypeId.FieldName = "COMTypeId";
            this.COMTypeId.Name = "COMTypeId";
            // 
            // COMTypeName
            // 
            this.COMTypeName.Caption = "Loại cổng COM";
            this.COMTypeName.FieldName = "COMTypeName";
            this.COMTypeName.Name = "COMTypeName";
            this.COMTypeName.Visible = true;
            this.COMTypeName.VisibleIndex = 0;
            // 
            // StatusId
            // 
            this.StatusId.Caption = "StatusId";
            this.StatusId.FieldName = "Status";
            this.StatusId.Name = "StatusId";
            // 
            // Status
            // 
            this.Status.Caption = "Trạng thái";
            this.Status.FieldName = "StatusName";
            this.Status.Name = "Status";
            this.Status.Visible = true;
            this.Status.VisibleIndex = 2;
            // 
            // TimeAction
            // 
            this.TimeAction.Caption = "Thời gian thực hiện";
            this.TimeAction.FieldName = "TimeAction";
            this.TimeAction.Name = "TimeAction";
            this.TimeAction.Visible = true;
            this.TimeAction.VisibleIndex = 1;
            // 
            // IsActive
            // 
            this.IsActive.Caption = "Sử dụng";
            this.IsActive.ColumnEdit = this.repositoryItemCheckEdit1;
            this.IsActive.FieldName = "IsActive";
            this.IsActive.Name = "IsActive";
            this.IsActive.Visible = true;
            this.IsActive.VisibleIndex = 3;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // Description
            // 
            this.Description.Caption = "Mô tả";
            this.Description.FieldName = "Description";
            this.Description.Name = "Description";
            this.Description.Visible = true;
            this.Description.VisibleIndex = 4;
            // 
            // FrmTurnComportMng
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(813, 476);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FrmTurnComportMng";
            this.Text = "Quản lý tắt/mở cổng COM";
            this.Load += new System.EventHandler(this.FrmTurnComportMng_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tTimeAction.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.butCluster)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button butDelete;
        private System.Windows.Forms.Button butHuy;
        private System.Windows.Forms.Button butSave;
        private System.Windows.Forms.Button butAdd;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button butUpdate;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.TimeEdit tTimeAction;
        private System.Windows.Forms.ComboBox cbbStatus;
        private System.Windows.Forms.ComboBox cbbComport;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit butCluster;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraGrid.Columns.GridColumn COMTypeId;
        private DevExpress.XtraGrid.Columns.GridColumn COMTypeName;
        private DevExpress.XtraGrid.Columns.GridColumn Status;
        private DevExpress.XtraGrid.Columns.GridColumn TimeAction;
        private DevExpress.XtraGrid.Columns.GridColumn IsActive;
        private DevExpress.XtraGrid.Columns.GridColumn Description;
        private System.Windows.Forms.CheckBox chkIsUse;
        private DevExpress.XtraGrid.Columns.GridColumn StatusId;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
    }
}