namespace DuAn03_HaiDang
{
    partial class FrmBaoHetHang
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
            this.panelEx2 = new DevComponents.DotNetBar.PanelEx();
            this.butHuyThayDoiHienThiDen = new DevComponents.DotNetBar.ButtonX();
            this.butLuuHienThiDen = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.dataGridBaoHetHang = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.dgSTT = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
            this.dgSoSanPhamCon = new DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn();
            this.dgSoLanBao = new DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelEx2.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridBaoHetHang)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelEx2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupPanel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(521, 332);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panelEx2
            // 
            this.panelEx2.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx2.Controls.Add(this.butHuyThayDoiHienThiDen);
            this.panelEx2.Controls.Add(this.butLuuHienThiDen);
            this.panelEx2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx2.Location = new System.Drawing.Point(0, 282);
            this.panelEx2.Margin = new System.Windows.Forms.Padding(0);
            this.panelEx2.Name = "panelEx2";
            this.panelEx2.Size = new System.Drawing.Size(521, 50);
            this.panelEx2.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx2.Style.BackColor1.Color = System.Drawing.Color.DodgerBlue;
            this.panelEx2.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx2.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx2.Style.GradientAngle = 90;
            this.panelEx2.TabIndex = 59;
            // 
            // butHuyThayDoiHienThiDen
            // 
            this.butHuyThayDoiHienThiDen.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.butHuyThayDoiHienThiDen.ColorTable = DevComponents.DotNetBar.eButtonColor.BlueOrb;
            this.butHuyThayDoiHienThiDen.Location = new System.Drawing.Point(286, 8);
            this.butHuyThayDoiHienThiDen.Name = "butHuyThayDoiHienThiDen";
            this.butHuyThayDoiHienThiDen.Size = new System.Drawing.Size(111, 32);
            this.butHuyThayDoiHienThiDen.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.butHuyThayDoiHienThiDen.TabIndex = 48;
            this.butHuyThayDoiHienThiDen.Text = "Huỷ";
            this.butHuyThayDoiHienThiDen.Click += new System.EventHandler(this.butHuyThayDoiHienThiDen_Click);
            // 
            // butLuuHienThiDen
            // 
            this.butLuuHienThiDen.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.butLuuHienThiDen.ColorTable = DevComponents.DotNetBar.eButtonColor.BlueOrb;
            this.butLuuHienThiDen.Location = new System.Drawing.Point(122, 9);
            this.butLuuHienThiDen.Name = "butLuuHienThiDen";
            this.butLuuHienThiDen.Size = new System.Drawing.Size(111, 32);
            this.butLuuHienThiDen.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.butLuuHienThiDen.TabIndex = 48;
            this.butLuuHienThiDen.Text = "Lưu";
            this.butLuuHienThiDen.Click += new System.EventHandler(this.butLuuHienThiDen_Click);
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.dataGridBaoHetHang);
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupPanel1.Location = new System.Drawing.Point(0, 0);
            this.groupPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(521, 282);
            // 
            // 
            // 
            this.groupPanel1.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel1.Style.BackColorGradientAngle = 90;
            this.groupPanel1.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderBottomWidth = 1;
            this.groupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderLeftWidth = 1;
            this.groupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderRightWidth = 1;
            this.groupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderTopWidth = 1;
            this.groupPanel1.Style.Class = "";
            this.groupPanel1.Style.CornerDiameter = 4;
            this.groupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseDown.Class = "";
            this.groupPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseOver.Class = "";
            this.groupPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel1.TabIndex = 60;
            this.groupPanel1.Text = "Thông tin phát âm thanh báo";
            // 
            // dataGridBaoHetHang
            // 
            this.dataGridBaoHetHang.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridBaoHetHang.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgSTT,
            this.dgSoSanPhamCon,
            this.dgSoLanBao});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridBaoHetHang.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridBaoHetHang.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridBaoHetHang.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.dataGridBaoHetHang.Location = new System.Drawing.Point(0, 0);
            this.dataGridBaoHetHang.Name = "dataGridBaoHetHang";
            this.dataGridBaoHetHang.Size = new System.Drawing.Size(515, 261);
            this.dataGridBaoHetHang.TabIndex = 0;
            this.dataGridBaoHetHang.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridBaoHetHang_UserDeletedRow);
            // 
            // dgSTT
            // 
            this.dgSTT.HeaderText = "STT";
            this.dgSTT.Name = "dgSTT";
            this.dgSTT.Visible = false;
            this.dgSTT.Width = 50;
            // 
            // dgSoSanPhamCon
            // 
            this.dgSoSanPhamCon.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            // 
            // 
            // 
            this.dgSoSanPhamCon.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window;
            this.dgSoSanPhamCon.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.dgSoSanPhamCon.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dgSoSanPhamCon.BackgroundStyle.TextColor = System.Drawing.SystemColors.ControlText;
            this.dgSoSanPhamCon.HeaderText = "Số Mặt Hàng";
            this.dgSoSanPhamCon.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left;
            this.dgSoSanPhamCon.Name = "dgSoSanPhamCon";
            this.dgSoSanPhamCon.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // dgSoLanBao
            // 
            this.dgSoLanBao.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            // 
            // 
            // 
            this.dgSoLanBao.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window;
            this.dgSoLanBao.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.dgSoLanBao.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dgSoLanBao.BackgroundStyle.TextColor = System.Drawing.SystemColors.ControlText;
            this.dgSoLanBao.HeaderText = "Số lần báo";
            this.dgSoLanBao.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left;
            this.dgSoLanBao.Name = "dgSoLanBao";
            this.dgSoLanBao.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // FrmBaoHetHang
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 332);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmBaoHetHang";
            this.Text = "Báo đạt sản lượng";
            this.Load += new System.EventHandler(this.FrmBaoHetHang_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelEx2.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridBaoHetHang)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevComponents.DotNetBar.PanelEx panelEx2;
        private DevComponents.DotNetBar.ButtonX butHuyThayDoiHienThiDen;
        private DevComponents.DotNetBar.ButtonX butLuuHienThiDen;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridBaoHetHang;
        private DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn dgSTT;
        private DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn dgSoSanPhamCon;
        private DevComponents.DotNetBar.Controls.DataGridViewIntegerInputColumn dgSoLanBao;
    }
}