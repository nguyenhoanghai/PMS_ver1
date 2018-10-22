using DuAn03_HaiDang.DAO;
using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.Enum;
using DuAn03_HaiDang.KeyPad_Chuyen.dao;
using DuAn03_HaiDang.KeyPad_Chuyen.pojo;
using DuAn03_HaiDang.Model;
using DuAn03_HaiDang.POJO;
using PMS.Business;
using PMS.Business.Enum;
using PMS.Business.Models;
using QuanLyNangSuat.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DuAn03_HaiDang
{
    public partial class FrmHienThiLCDTongHop : Form
    {
        //Init object
        #region Init object
        TableLayoutPanelConfigDAO tableLayoutPanelConfigDAO = new TableLayoutPanelConfigDAO();
        PanelConfigDAO panelConfigDAO = new PanelConfigDAO();
        LabelConfigDAO labelConfigDAO = new LabelConfigDAO();
        //  ConfigDAO configDAO = new ConfigDAO();
        NangXuatDAO nangSuatDAO;
        //  ShiftDAO shiftDAO = new ShiftDAO();
        BTPDAO btpDAO = new BTPDAO();
        ChuyenDAO chuyenDAO = new ChuyenDAO();
        List<TableLayoutPanelConfig> listTableLayoutPanelConfig;
        List<PanelConfig> listPanelConfig;
        List<LabelConfig> listLabelConfig;
        List<LabelForTablePanel> listLabelForTablePanel;
        List<Chuyen> listChuyen;
        ModelLCDTongHopInfo modelLCDTongHopInfo;
        int tableType = 3;
        DataTable dtDataChuyen;
        private System.Windows.Forms.Timer timerShow;
        private System.Windows.Forms.Timer timerDateTime;
        int soRowTrenMH = 1;
        int indexShow = 0;
        string logo = string.Empty;
        string titleLCD = string.Empty;
        TimeSpan timeStartWork = new TimeSpan(0, 0, 0);
        int maxWorkTimeOfLines = 0;
        Label labelTime;
        Label labelDate;
        SqlConnection sqlConnection;
        SqlDataAdapter sqlDataAdapter;
        string lineIdHasMaxWorkHours;
        int maxWorkTime = 0;
        //  private List<ModelWorkHours> listModelWorkHours;
        private List<WorkingTimeModel> listModelWorkHours;
        private int interval = 1000;
        #endregion Init object


        //Layout
        #region Layout
        private System.Windows.Forms.TableLayoutPanel tblpanelBody;
        private System.Windows.Forms.TableLayoutPanel tblpanelHeader;
        private System.Windows.Forms.TableLayoutPanel tblpanelTitle1;
        private System.Windows.Forms.TableLayoutPanel tblpanelTitle2;
        private System.Windows.Forms.TableLayoutPanel tblpanelContent;
        private System.Windows.Forms.TableLayoutPanel tblpanelFooter;
        #endregion Layout

        public FrmHienThiLCDTongHop(SqlConnection _sqlConnection)
        {
            InitializeComponent();
            this.timerShow = new System.Windows.Forms.Timer();
            this.timerDateTime = new System.Windows.Forms.Timer();
            this.sqlConnection = _sqlConnection;
        }

        private void FrmHienThiLCDTongHop_Load(object sender, EventArgs e)
        {
            try
            {
                modelLCDTongHopInfo = new ModelLCDTongHopInfo();
                //Get list Chuyen
                listChuyen = chuyenDAO.GetListChuyenInfByListId(AccountSuccess.strListChuyenId);

                maxWorkTime = GetMaxWorkTimeOfLines();
                if (!string.IsNullOrEmpty(lineIdHasMaxWorkHours))
                    listModelWorkHours = BLLShift.GetListWorkHoursOfLineByLineId(int.Parse(lineIdHasMaxWorkHours));// shiftDAO.GetListWorkHoursOfLineByLineId(lineIdHasMaxWorkHours);
                LoadLCDConfig();
                //Get config information
                listTableLayoutPanelConfig = tableLayoutPanelConfigDAO.GetTableLayoutPanelConfig(tableType);
                //Get config panel
                listPanelConfig = panelConfigDAO.GetPanelConfig(tableType);
                //Get label config
                listLabelConfig = labelConfigDAO.GetLabelConfig(tableType);
                //Get label for tablepanel
                listLabelForTablePanel = labelConfigDAO.GetLabelForTablePanel(tableType);
                //Init
                dtDataChuyen = new DataTable();
                nangSuatDAO = new NangXuatDAO();

                BuildPanelHeader();
                BuildPanelTitle1();
                BuildPanelTitle2();
                BuildPanelContent();
                BuildPanelFooter();
                BuildPanelBody();
                initTimerShow(interval);
                initTimerDateTime();
                //  LoadDataForChuyens();                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void LoadLCDConfig()
        {
            try
            {
                //List<Config> listConfig = configDAO.GetConfig();
                var listConfig = BLLConfig.Instance.GetShowLCDConfig();

                if (listConfig != null && listConfig.Count > 0)
                {
                    foreach (var config in listConfig)
                    {
                        switch (config.Name.Trim())
                        {
                            case "ThoiGianLayDuLieuLCDTong":
                                int.TryParse(config.Value, out interval);
                                break;
                            case "SoRowTrenLCDTong":
                                int.TryParse(config.Value, out soRowTrenMH);
                                break;
                            case "Logo":
                                logo = config.Value;
                                break;
                            case "TieuDeLCDTong":
                                titleLCD = config.Value;
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Lỗi : " + ex.Message, "Lỗi xử lý", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Khoi tao timer show
        private void initTimerShow(int interval)
        {
            this.timerShow.Interval = interval;
            this.timerShow.Tick += new System.EventHandler(this.timerShow_Tick);
            this.timerShow.Enabled = true;
            //if (listChuyen.Count > soRowTrenMH)

            //else
            //    this.timerShow.Enabled = false;

        }
        int k = 0;
        private void timerShow_Tick(object sender, EventArgs e)
        {
            try
            {
                //LoadDataForChuyens();
                LoadDataForChuyens_N();
            }
            catch (Exception ex)
            {
                //  this.timerShow.Enabled = false;
                MessageBox.Show("Lỗi timer getTime: " + ex.Message, "Lỗi xử lý", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        //Khoi tao timer DateTime
        private void initTimerDateTime()
        {
            this.timerDateTime.Interval = 1000;
            this.timerDateTime.Tick += new System.EventHandler(this.timerDateTime_Tick);
            this.timerDateTime.Enabled = true;
        }
        private void timerDateTime_Tick(object sender, EventArgs e)
        {
            try
            {
                DateTime dateNow = DateTime.Now;
                labelTime.Text = dateNow.Hour.ToString() + ":" + dateNow.Minute.ToString() + ":" + dateNow.Second.ToString();
                labelDate.Text = "Ngày " + dateNow.Day.ToString() + "/" + dateNow.Month.ToString() + "/" + dateNow.Year.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi timer getTime: " + ex.Message, "Lỗi xử lý", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.timerDateTime.Enabled = false;
            }

        }

        //tblpanelHeader
        private void BuildPanelHeader()
        {
            try
            {
                if (listTableLayoutPanelConfig != null && listTableLayoutPanelConfig.Count > 0)
                {
                    var listtblpanelHeader = listTableLayoutPanelConfig.Where(c => c.TableLayoutTableName.Trim() == "tblpanelHeader" && c.IsShow).OrderBy(c => c.RowInt).ToList();
                    if (listtblpanelHeader.Count > 0)
                    {
                        this.tblpanelHeader = new TableLayoutPanel();
                        if (listPanelConfig != null && listPanelConfig.Count > 0)
                        {
                            var panelConfig = listPanelConfig.Where(c => c.Name.Trim() == "panelHeader").FirstOrDefault();
                            if (panelConfig != null)
                                this.tblpanelHeader.BackColor = DuAn03_HaiDang.Helper.HelperControl.GetColor(panelConfig.BackColor);
                        }
                        this.tblpanelHeader.SuspendLayout();
                        this.tblpanelHeader.RowCount = 1;
                        this.tblpanelHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
                        this.tblpanelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
                        this.tblpanelHeader.Location = new System.Drawing.Point(0, 0);
                        this.tblpanelHeader.Name = "tblpanelHeader";
                        this.tblpanelHeader.ColumnCount = listtblpanelHeader.Count;
                        foreach (var item in listtblpanelHeader)
                        {
                            float size = 0;
                            float.TryParse(item.SizePercent, out size);
                            this.tblpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, size));
                        }
                        this.tblpanelHeader.Margin = new System.Windows.Forms.Padding(0);
                        this.tblpanelHeader.Size = new System.Drawing.Size(739, 405);
                        this.tblpanelHeader.TabIndex = 0;

                        Panel panel = new Panel();

                        panel.Dock = System.Windows.Forms.DockStyle.Fill;
                        panel.Location = new System.Drawing.Point(832, 2);
                        panel.Margin = new System.Windows.Forms.Padding(0);
                        panel.Name = "panelLogo";
                        panel.Size = new System.Drawing.Size(168, 48);
                        panel.TabIndex = 1;
                        try
                        {
                            panel.BackgroundImage = Image.FromFile(Application.StartupPath + @logo);
                            panel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
                        }
                        catch (Exception)
                        {
                            //  MessageBox.Show("Lỗi không tìm thấy logo với đường dẫn : " + (Application.StartupPath + @logo));
                        }

                        Label labelTitle = new Label();
                        labelTitle.Anchor = System.Windows.Forms.AnchorStyles.None;
                        labelTitle.AutoSize = true;
                        labelTitle.Location = new System.Drawing.Point(16, 17);
                        labelTitle.Name = "labelTitle";
                        labelTitle.Size = new System.Drawing.Size(91, 18);
                        labelTitle.TabIndex = 0;
                        labelTitle.Text = titleLCD;
                        labelTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;

                        var tblpanelDateTime = new TableLayoutPanel();
                        tblpanelDateTime.SuspendLayout();
                        tblpanelDateTime.RowCount = 2;
                        tblpanelDateTime.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                        tblpanelDateTime.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                        tblpanelDateTime.ColumnCount = 1;
                        tblpanelDateTime.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
                        tblpanelDateTime.Dock = System.Windows.Forms.DockStyle.Fill;
                        tblpanelDateTime.Location = new System.Drawing.Point(0, 0);
                        tblpanelDateTime.Name = "tblpanelDateTime";
                        tblpanelDateTime.Margin = new System.Windows.Forms.Padding(0);
                        tblpanelDateTime.Size = new System.Drawing.Size(739, 405);
                        tblpanelDateTime.TabIndex = 0;

                        DateTime dateNow = DateTime.Now;
                        labelTime = new Label();
                        labelTime.Anchor = System.Windows.Forms.AnchorStyles.None;
                        labelTime.AutoSize = true;
                        labelTime.Location = new System.Drawing.Point(16, 17);
                        labelTime.Name = "labelTime";
                        labelTime.Size = new System.Drawing.Size(91, 18);
                        labelTime.TabIndex = 0;
                        labelTime.Text = dateNow.Hour.ToString() + ":" + dateNow.Minute.ToString() + ":" + dateNow.Second.ToString();
                        labelTime.TextAlign = System.Drawing.ContentAlignment.TopCenter;

                        labelDate = new Label();
                        labelDate.Anchor = System.Windows.Forms.AnchorStyles.None;
                        labelDate.AutoSize = true;
                        labelDate.Location = new System.Drawing.Point(16, 17);
                        labelDate.Name = "labelDate";
                        labelDate.Size = new System.Drawing.Size(91, 18);
                        labelDate.TabIndex = 0;
                        labelDate.Text = "Ngày" + dateNow.Day.ToString() + "/" + dateNow.Month.ToString() + "/" + dateNow.Year.ToString();
                        labelDate.TextAlign = System.Drawing.ContentAlignment.TopCenter;

                        var labelConfigHeader = listLabelConfig.Where(c => c.TableLayoutPanelName.Trim() == "tblPanelHeader" && c.Position == 1).FirstOrDefault();
                        if (labelConfigHeader != null)
                        {
                            DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigHeader, labelTitle);
                        }

                        var labelConfigHeaderDatetime = listLabelConfig.Where(c => c.TableLayoutPanelName.Trim() == "tblPanelHeader" && c.Position == 2).FirstOrDefault();
                        if (labelConfigHeaderDatetime != null)
                        {
                            DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigHeaderDatetime, labelTime);
                            DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigHeaderDatetime, labelDate);
                        }
                        tblpanelDateTime.Controls.Add(labelTime, 0, 0);
                        tblpanelDateTime.Controls.Add(labelDate, 0, 1);
                        tblpanelDateTime.ResumeLayout(false);

                        this.tblpanelHeader.Controls.Add(panel, 0, 0);
                        this.tblpanelHeader.Controls.Add(labelTitle, 1, 0);
                        this.tblpanelHeader.Controls.Add(tblpanelDateTime, 2, 0);
                        this.tblpanelHeader.ResumeLayout(false);
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        //tblpanelTitle1
        private void BuildPanelTitle1()
        {
            try
            {
                if (listTableLayoutPanelConfig != null && listTableLayoutPanelConfig.Count > 0)
                {
                    var listtblpanelTitle1 = listTableLayoutPanelConfig.Where(c => c.TableLayoutTableName.Trim() == "tblpanelTitle1" && c.IsShow).OrderBy(c => c.RowInt).ToList();
                    if (listtblpanelTitle1.Count > 0)
                    {
                        this.tblpanelTitle1 = new TableLayoutPanel();
                        this.tblpanelTitle1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
                        if (listPanelConfig != null && listPanelConfig.Count > 0)
                        {
                            var panelConfig = listPanelConfig.Where(c => c.Name.Trim() == "panelTitle1").FirstOrDefault();
                            if (panelConfig != null)
                                this.tblpanelTitle1.BackColor = DuAn03_HaiDang.Helper.HelperControl.GetColor(panelConfig.BackColor);
                        }
                        this.tblpanelTitle1.SuspendLayout();
                        this.tblpanelTitle1.RowCount = 1;
                        this.tblpanelTitle1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
                        this.tblpanelTitle1.Dock = System.Windows.Forms.DockStyle.Fill;
                        this.tblpanelTitle1.Location = new System.Drawing.Point(0, 0);
                        this.tblpanelTitle1.Name = "tblpanelTitle1";
                        this.tblpanelTitle1.ColumnCount = listtblpanelTitle1.Count;
                        foreach (var item in listtblpanelTitle1)
                        {
                            float size = 0;
                            float.TryParse(item.SizePercent, out size);
                            this.tblpanelTitle1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, size));
                        }
                        this.tblpanelTitle1.Margin = new System.Windows.Forms.Padding(0);
                        this.tblpanelTitle1.Size = new System.Drawing.Size(739, 405);
                        this.tblpanelTitle1.TabIndex = 0;

                        Label labelMorth = new Label();
                        labelMorth.Anchor = System.Windows.Forms.AnchorStyles.None;
                        labelMorth.AutoSize = true;
                        labelMorth.Location = new System.Drawing.Point(16, 17);
                        labelMorth.Name = "labelMorth";
                        labelMorth.Size = new System.Drawing.Size(91, 18);
                        labelMorth.TabIndex = 0;
                        DateTime dateNow = DateTime.Now;
                        labelMorth.Text = "THÁNG " + dateNow.Month.ToString() + "/" + dateNow.Year.ToString();
                        labelMorth.TextAlign = System.Drawing.ContentAlignment.TopCenter;

                        Label labelKH = new Label();
                        labelKH.Anchor = System.Windows.Forms.AnchorStyles.None;
                        labelKH.AutoSize = true;
                        labelKH.Location = new System.Drawing.Point(16, 17);
                        labelKH.Name = "labelKH";
                        labelKH.Size = new System.Drawing.Size(91, 18);
                        labelKH.TabIndex = 0;
                        labelKH.Text = "SẢN LƯỢNG (TH/KH):";
                        labelKH.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                        modelLCDTongHopInfo.LabelKeHoachTong = labelKH;

                        Label labelDT = new Label();
                        labelDT.Anchor = System.Windows.Forms.AnchorStyles.None;
                        labelDT.AutoSize = true;
                        labelDT.Location = new System.Drawing.Point(16, 17);
                        labelDT.Name = "labelDT";
                        labelDT.Size = new System.Drawing.Size(91, 18);
                        labelDT.TabIndex = 0;
                        labelDT.Text = "DOANH THU (TH/KH):";
                        labelDT.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                        modelLCDTongHopInfo.LabelDoanhThuTong = labelDT;

                        Label labelNSX = new Label();
                        labelNSX.Anchor = System.Windows.Forms.AnchorStyles.None;
                        labelNSX.AutoSize = true;
                        labelNSX.Location = new System.Drawing.Point(16, 17);
                        labelNSX.Name = "labelNSX";
                        labelNSX.Size = new System.Drawing.Size(91, 18);
                        labelNSX.TabIndex = 0;
                        labelNSX.Text = "NHỊP SẢN XUẤT (TH/KH):";
                        labelNSX.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                        modelLCDTongHopInfo.LabelNhipSanXuat = labelNSX;

                        var labelConfigHeader = listLabelConfig.Where(c => c.TableLayoutPanelName.Trim() == "tblPanelTitle1").FirstOrDefault();
                        if (labelConfigHeader != null)
                        {
                            DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigHeader, labelMorth);
                            DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigHeader, modelLCDTongHopInfo.LabelKeHoachTong);
                            DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigHeader, modelLCDTongHopInfo.LabelDoanhThuTong);
                            DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigHeader, modelLCDTongHopInfo.LabelNhipSanXuat);
                        }

                        this.tblpanelTitle1.Controls.Add(labelMorth, 0, 0);
                        this.tblpanelTitle1.Controls.Add(labelKH, 1, 0);
                        this.tblpanelTitle1.Controls.Add(labelDT, 2, 0);
                        this.tblpanelTitle1.Controls.Add(labelNSX, 3, 0);
                        this.tblpanelTitle1.ResumeLayout(false);
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        //tblpanelTitle2
        private void BuildPanelTitle2()
        {
            try
            {
                if (listTableLayoutPanelConfig != null && listTableLayoutPanelConfig.Count > 0)
                {
                    var listtblpanelTitle2 = listTableLayoutPanelConfig.Where(c => c.TableLayoutTableName.Trim() == "tblpanelTitle2" && c.IsShow).OrderBy(c => c.RowInt).ToList();
                    if (listtblpanelTitle2.Count > 0)
                    {
                        this.tblpanelTitle2 = new TableLayoutPanel();
                        this.tblpanelTitle2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
                        if (listPanelConfig != null && listPanelConfig.Count > 0)
                        {
                            var panelConfig = listPanelConfig.Where(c => c.Name.Trim() == "panelTitle2").FirstOrDefault();
                            if (panelConfig != null)
                                this.tblpanelTitle2.BackColor = DuAn03_HaiDang.Helper.HelperControl.GetColor(panelConfig.BackColor);
                        }
                        this.tblpanelTitle2.SuspendLayout();
                        this.tblpanelTitle2.RowCount = 1;
                        this.tblpanelTitle2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
                        this.tblpanelTitle2.Dock = System.Windows.Forms.DockStyle.Fill;
                        this.tblpanelTitle2.Location = new System.Drawing.Point(0, 0);
                        this.tblpanelTitle2.Name = "tblpanelTitle2";
                        this.tblpanelTitle2.ColumnCount = listtblpanelTitle2.Count;
                        foreach (var item in listtblpanelTitle2)
                        {
                            float size = 0;
                            float.TryParse(item.SizePercent, out size);
                            this.tblpanelTitle2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, size));
                        }
                        this.tblpanelTitle2.Margin = new System.Windows.Forms.Padding(0);
                        this.tblpanelTitle2.Size = new System.Drawing.Size(739, 405);
                        this.tblpanelTitle2.TabIndex = 0;

                        Label labelTHKHThang = new Label();
                        labelTHKHThang.Anchor = System.Windows.Forms.AnchorStyles.None;
                        labelTHKHThang.AutoSize = true;
                        labelTHKHThang.Location = new System.Drawing.Point(16, 17);
                        labelTHKHThang.Name = "labelTHKHThang";
                        labelTHKHThang.Size = new System.Drawing.Size(91, 18);
                        labelTHKHThang.TabIndex = 0;
                        DateTime dateNow = DateTime.Now;
                        labelTHKHThang.Text = "TH/KH THÁNG";
                        labelTHKHThang.TextAlign = System.Drawing.ContentAlignment.TopCenter;

                        Label labelTHKHNgay = new Label();
                        labelTHKHNgay.Anchor = System.Windows.Forms.AnchorStyles.None;
                        labelTHKHNgay.AutoSize = true;
                        labelTHKHNgay.Location = new System.Drawing.Point(16, 17);
                        labelTHKHNgay.Name = "labelTHKHNgay";
                        labelTHKHNgay.Size = new System.Drawing.Size(91, 18);
                        labelTHKHNgay.TabIndex = 0;
                        labelTHKHNgay.Text = "TH/KH NGÀY";
                        labelTHKHNgay.TextAlign = System.Drawing.ContentAlignment.TopCenter;

                        Label labelDoanhThuTHKH = new Label();
                        labelDoanhThuTHKH.Anchor = System.Windows.Forms.AnchorStyles.None;
                        labelDoanhThuTHKH.AutoSize = true;
                        labelDoanhThuTHKH.Location = new System.Drawing.Point(16, 17);
                        labelDoanhThuTHKH.Name = "labelDoanhThuTHKH";
                        labelDoanhThuTHKH.Size = new System.Drawing.Size(91, 18);
                        labelDoanhThuTHKH.TabIndex = 0;
                        labelDoanhThuTHKH.Text = "DOANH THU (TH/KH)";
                        labelDoanhThuTHKH.TextAlign = System.Drawing.ContentAlignment.TopCenter;

                        Label labelPTTH = new Label();
                        labelPTTH.Anchor = System.Windows.Forms.AnchorStyles.None;
                        labelPTTH.AutoSize = true;
                        labelPTTH.Location = new System.Drawing.Point(16, 17);
                        labelPTTH.Name = "labelPTTH";
                        labelPTTH.Size = new System.Drawing.Size(91, 18);
                        labelPTTH.TabIndex = 0;
                        labelPTTH.Text = "% THỰC HIỆN";
                        labelPTTH.TextAlign = System.Drawing.ContentAlignment.TopCenter;

                        var labelConfigHeader = listLabelConfig.Where(c => c.TableLayoutPanelName.Trim() == "tblPanelTitle2").FirstOrDefault();
                        if (labelConfigHeader != null)
                        {
                            DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigHeader, labelTHKHThang);
                            DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigHeader, labelTHKHNgay);
                            DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigHeader, labelDoanhThuTHKH);
                            DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigHeader, labelPTTH);
                        }
                        this.tblpanelTitle2.Controls.Add(labelTHKHNgay, 1, 0);
                        this.tblpanelTitle2.Controls.Add(labelTHKHThang, 2, 0);
                        this.tblpanelTitle2.Controls.Add(labelDoanhThuTHKH, 3, 0);
                        this.tblpanelTitle2.Controls.Add(labelPTTH, 4, 0);
                        this.tblpanelTitle2.ResumeLayout(false);
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        //tblpanelContent
        private void BuildPanelContent()
        {
            try
            {
                if (listTableLayoutPanelConfig != null && listTableLayoutPanelConfig.Count > 0)
                {
                    var listtblpanelContent = listTableLayoutPanelConfig.Where(c => c.TableLayoutTableName.Trim() == "tblpanelContent" && c.IsShow).OrderBy(c => c.RowInt).ToList();
                    if (listtblpanelContent.Count > 0)
                    {
                        this.tblpanelContent = new TableLayoutPanel();
                        this.tblpanelContent.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
                        if (listPanelConfig != null && listPanelConfig.Count > 0)
                        {
                            var panelConfig = listPanelConfig.Where(c => c.Name.Trim() == "panelContent").FirstOrDefault();
                            if (panelConfig != null)
                                this.tblpanelContent.BackColor = DuAn03_HaiDang.Helper.HelperControl.GetColor(panelConfig.BackColor);
                        }
                        this.tblpanelContent.SuspendLayout();
                        this.tblpanelContent.Dock = System.Windows.Forms.DockStyle.Fill;
                        this.tblpanelContent.Location = new System.Drawing.Point(0, 0);
                        this.tblpanelContent.Name = "tblpanelContent";
                        this.tblpanelContent.ColumnCount = listtblpanelContent.Count;
                        foreach (var item in listtblpanelContent)
                        {
                            float size = 0;
                            float.TryParse(item.SizePercent, out size);
                            this.tblpanelContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, size));
                        }
                        this.tblpanelContent.Margin = new System.Windows.Forms.Padding(0);
                        this.tblpanelContent.Size = new System.Drawing.Size(739, 405);
                        this.tblpanelContent.TabIndex = 0;

                        if (listChuyen != null && listChuyen.Count > 0)
                        {
                            this.tblpanelContent.RowCount = listChuyen.Count;
                            float size = 100 / listChuyen.Count;
                            for (int i = 0; i < listChuyen.Count; i++)
                            {
                                this.tblpanelContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, size));
                                var chuyen = listChuyen[i];

                                ModelLCDLineInfo modelLCDLineInfo = new ModelLCDLineInfo();
                                modelLCDLineInfo.MaChuyen = int.Parse(chuyen.MaChuyen);
                                modelLCDLineInfo.TenChuyen = chuyen.TenChuyen;

                                Label labelLineName = new Label();
                                labelLineName.Anchor = System.Windows.Forms.AnchorStyles.None;
                                labelLineName.AutoSize = true;
                                labelLineName.Location = new System.Drawing.Point(16, 17);
                                labelLineName.Name = "labelLineName" + chuyen.MaChuyen;
                                labelLineName.Size = new System.Drawing.Size(91, 18);
                                labelLineName.TabIndex = 0;
                                DateTime dateNow = DateTime.Now;
                                labelLineName.Text = chuyen.TenChuyen;
                                labelLineName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                                ModelLabel modelLabelLineName = new ModelLabel();
                                modelLabelLineName.SystemName = "LineName";
                                modelLabelLineName.Label = labelLineName;
                                modelLCDLineInfo.ListLabelNSInfo.Add(modelLabelLineName);

                                Label labelTHMorth = new Label();
                                labelTHMorth.Anchor = System.Windows.Forms.AnchorStyles.None;
                                labelTHMorth.AutoSize = true;
                                labelTHMorth.Location = new System.Drawing.Point(16, 17);
                                labelTHMorth.Name = "labelTHMorth" + chuyen.MaChuyen;
                                labelTHMorth.Size = new System.Drawing.Size(91, 18);
                                labelTHMorth.TabIndex = 0;
                                labelTHMorth.Text = "0";
                                labelTHMorth.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                                ModelLabel modelLabelTHMorth = new ModelLabel();
                                modelLabelTHMorth.SystemName = "THMorth";
                                modelLabelTHMorth.Label = labelTHMorth;
                                modelLCDLineInfo.ListLabelNSInfo.Add(modelLabelTHMorth);

                                Label labelKHMorth = new Label();
                                labelKHMorth.Anchor = System.Windows.Forms.AnchorStyles.None;
                                labelKHMorth.AutoSize = true;
                                labelKHMorth.Location = new System.Drawing.Point(16, 17);
                                labelKHMorth.Name = "labelKHMorth" + chuyen.MaChuyen;
                                labelKHMorth.Size = new System.Drawing.Size(91, 18);
                                labelKHMorth.TabIndex = 0;
                                labelKHMorth.Text = "0";
                                labelKHMorth.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                                ModelLabel modelLabelKHMorth = new ModelLabel();
                                modelLabelKHMorth.SystemName = "KHMorth";
                                modelLabelKHMorth.Label = labelKHMorth;
                                modelLCDLineInfo.ListLabelNSInfo.Add(modelLabelKHMorth);

                                Label labelTHDay = new Label();
                                labelTHDay.Anchor = System.Windows.Forms.AnchorStyles.None;
                                labelTHDay.AutoSize = true;
                                labelTHDay.Location = new System.Drawing.Point(16, 17);
                                labelTHDay.Name = "labelTHDay" + chuyen.MaChuyen;
                                labelTHDay.Size = new System.Drawing.Size(91, 18);
                                labelTHDay.TabIndex = 0;
                                labelTHDay.Text = "0";
                                labelTHDay.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                                ModelLabel modelLabelTHDay = new ModelLabel();
                                modelLabelTHDay.SystemName = "THDay";
                                modelLabelTHDay.Label = labelTHDay;
                                modelLCDLineInfo.ListLabelNSInfo.Add(modelLabelTHDay);

                                Label labelKHDay = new Label();
                                labelKHDay.Anchor = System.Windows.Forms.AnchorStyles.None;
                                labelKHDay.AutoSize = true;
                                labelKHDay.Location = new System.Drawing.Point(16, 17);
                                labelKHDay.Name = "labelKHDay" + chuyen.MaChuyen;
                                labelKHDay.Size = new System.Drawing.Size(91, 18);
                                labelKHDay.TabIndex = 0;
                                labelKHDay.Text = "0";
                                labelKHDay.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                                ModelLabel modelLabelKHDay = new ModelLabel();
                                modelLabelKHDay.SystemName = "KHDay";
                                modelLabelKHDay.Label = labelKHDay;
                                modelLCDLineInfo.ListLabelNSInfo.Add(modelLabelKHDay);

                                Label labelLuyKeSP = new Label();
                                labelLuyKeSP.Anchor = System.Windows.Forms.AnchorStyles.None;
                                labelLuyKeSP.AutoSize = true;
                                labelLuyKeSP.Location = new System.Drawing.Point(16, 17);
                                labelLuyKeSP.Name = "labelLuyKeSP" + chuyen.MaChuyen;
                                labelLuyKeSP.Size = new System.Drawing.Size(91, 18);
                                labelLuyKeSP.TabIndex = 0;
                                labelLuyKeSP.Text = "0";
                                labelLuyKeSP.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                                ModelLabel modelLabelLuyKeSP = new ModelLabel();
                                modelLabelLuyKeSP.SystemName = "LuyKeSP";
                                modelLabelLuyKeSP.Label = labelLuyKeSP;
                                modelLCDLineInfo.ListLabelNSInfo.Add(modelLabelLuyKeSP);

                                Label labelLuyKeDT = new Label();
                                labelLuyKeDT.Anchor = System.Windows.Forms.AnchorStyles.None;
                                labelLuyKeDT.AutoSize = true;
                                labelLuyKeDT.Location = new System.Drawing.Point(16, 17);
                                labelLuyKeDT.Name = "labelLuyKeDT" + chuyen.MaChuyen;
                                labelLuyKeDT.Size = new System.Drawing.Size(91, 18);
                                labelLuyKeDT.TabIndex = 0;
                                labelLuyKeDT.Text = "0";
                                labelLuyKeDT.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                                ModelLabel modelLabelLuyKeDT = new ModelLabel();
                                modelLabelLuyKeDT.SystemName = "LuyKeDT";
                                modelLabelLuyKeDT.Label = labelLuyKeDT;
                                modelLCDLineInfo.ListLabelNSInfo.Add(modelLabelLuyKeDT);

                                Label labelPTThucHien = new Label();
                                labelPTThucHien.Anchor = System.Windows.Forms.AnchorStyles.None;
                                labelPTThucHien.AutoSize = true;
                                labelPTThucHien.Location = new System.Drawing.Point(16, 17);
                                labelPTThucHien.Name = "labelPTThucHien" + chuyen.MaChuyen;
                                labelPTThucHien.Size = new System.Drawing.Size(91, 18);
                                labelPTThucHien.TabIndex = 0;
                                labelPTThucHien.Text = "0";
                                labelPTThucHien.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                                ModelLabel modelLabelPTThucHien = new ModelLabel();
                                modelLabelPTThucHien.SystemName = "PTThucHien";
                                modelLabelPTThucHien.Label = labelPTThucHien;
                                modelLCDLineInfo.ListLabelNSInfo.Add(modelLabelPTThucHien);

                                modelLCDTongHopInfo.ListLineNS.Add(modelLCDLineInfo);

                                var labelConfigHeader = listLabelConfig.Where(c => c.TableLayoutPanelName.Trim() == "tblPanelContent").FirstOrDefault();
                                if (labelConfigHeader != null)
                                {
                                    DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigHeader, labelLineName);
                                    DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigHeader, labelTHDay);
                                    DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigHeader, labelKHDay);
                                    DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigHeader, labelTHMorth);
                                    DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigHeader, labelKHMorth);
                                    DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigHeader, labelLuyKeSP);
                                    DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigHeader, labelLuyKeDT);
                                    DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigHeader, labelPTThucHien);
                                }

                                this.tblpanelContent.Controls.Add(labelLineName, 0, i);
                                this.tblpanelContent.Controls.Add(labelTHDay, 1, i);
                                this.tblpanelContent.Controls.Add(labelKHDay, 2, i);
                                this.tblpanelContent.Controls.Add(labelTHMorth, 3, i);
                                this.tblpanelContent.Controls.Add(labelKHMorth, 4, i);
                                this.tblpanelContent.Controls.Add(labelLuyKeSP, 5, i);
                                this.tblpanelContent.Controls.Add(labelLuyKeDT, 6, i);
                                this.tblpanelContent.Controls.Add(labelPTThucHien, 7, i);
                            }
                        }
                        this.tblpanelContent.ResumeLayout(false);
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private List<Shift> TTCaCuaChuyen(int MaChuyen)
        {
            List<Shift> listShift = new List<Shift>();
            DataTable dtAllShift = new DataTable();
            string strSQL = "select IdShift, Name, TimeStart, TimeEnd from Shift where MaChuyen =" + MaChuyen + " order by IdShift";
            dtAllShift = dbclass.TruyVan_TraVe_DataTable(strSQL);
            if (dtAllShift.Rows.Count > 0)
            {
                for (int i = 0; i < dtAllShift.Rows.Count; i++)
                {
                    Shift shift = new Shift()
                    {
                        IdShift = int.Parse(dtAllShift.Rows[i][0].ToString()),
                        Name = dtAllShift.Rows[i][1].ToString(),
                        TimeStart = TimeSpan.Parse(dtAllShift.Rows[i][2].ToString()),
                        TimeEnd = TimeSpan.Parse(dtAllShift.Rows[i][3].ToString())
                    };
                    listShift.Add(shift);
                    if (timeStartWork == new TimeSpan(0, 0, 0))
                        timeStartWork = shift.TimeStart;
                    else
                    {
                        if (shift.TimeStart < timeStartWork)
                            timeStartWork = shift.TimeStart;
                    }
                }
            }
            return listShift;
        }

        private int SumHoursOfShifts(int maChuyen)
        {
            int countHours = 0;
            double sumHours = 0;
            double sumMinuterWork = 0;
            try
            {
                var shifts = BLLShift.GetShiftsOfLine(maChuyen);//TTCaCuaChuyen(maChuyen);
                if (shifts != null && shifts.Count > 0)
                {
                    foreach (var shift in shifts)
                    {
                        sumHours += shift.End.TotalHours - shift.Start.TotalHours;
                        sumMinuterWork += shift.End.TotalMinutes - shift.Start.TotalMinutes;
                    }
                }
                int intSumHours = (int)sumHours;
                if (intSumHours == sumHours)
                    countHours = intSumHours;
                else
                    countHours = intSumHours + 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tính tổng số giờ làm việc trong ngày: " + ex.Message, "Lỗi xử lý", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return countHours;
        }

        private int GetMaxWorkTimeOfLines()
        {
            try
            {
                int maxWorkTime = 0;
                if (listChuyen != null && listChuyen.Count > 0)
                {
                    foreach (var chuyen in listChuyen)
                    {
                        int workTime = SumHoursOfShifts(int.Parse(chuyen.MaChuyen));
                        if (workTime > maxWorkTime)
                        {
                            lineIdHasMaxWorkHours = chuyen.MaChuyen;
                            maxWorkTime = workTime;
                        }
                    }
                }
                return maxWorkTime;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //tblpanelFooter
        private void BuildPanelFooter()
        {
            try
            {
                this.tblpanelFooter = new TableLayoutPanel();
                this.tblpanelFooter.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
                if (listPanelConfig != null && listPanelConfig.Count > 0)
                {
                    var panelConfig = listPanelConfig.Where(c => c.Name.Trim() == "panelFooter").FirstOrDefault();
                    if (panelConfig != null)
                        this.tblpanelFooter.BackColor = DuAn03_HaiDang.Helper.HelperControl.GetColor(panelConfig.BackColor);
                }
                this.tblpanelFooter.SuspendLayout();
                this.tblpanelFooter.RowCount = 2;
                this.tblpanelFooter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                this.tblpanelFooter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                this.tblpanelFooter.Dock = System.Windows.Forms.DockStyle.Fill;
                this.tblpanelFooter.Location = new System.Drawing.Point(0, 0);
                this.tblpanelFooter.Name = "tblpanelFooter";


                maxWorkTimeOfLines = maxWorkTime;
                if (maxWorkTime > 0)
                {
                    this.tblpanelFooter.ColumnCount = maxWorkTime;
                    float size = 100 / maxWorkTime;
                    for (int i = 0; i < maxWorkTime; i++)
                    {
                        this.tblpanelFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, size));

                        Label labelTitleHourNS = new Label();
                        labelTitleHourNS.Anchor = System.Windows.Forms.AnchorStyles.None;
                        labelTitleHourNS.AutoSize = true;
                        labelTitleHourNS.Location = new System.Drawing.Point(16, 17);
                        labelTitleHourNS.Name = "labelTitleHourNS" + i.ToString();
                        labelTitleHourNS.Size = new System.Drawing.Size(91, 18);
                        labelTitleHourNS.TabIndex = 0;
                        DateTime dateNow = DateTime.Now;
                        labelTitleHourNS.Text = "GIỜ " + (i + 1).ToString();
                        labelTitleHourNS.TextAlign = System.Drawing.ContentAlignment.TopCenter;

                        Label labelValueHourNS = new Label();
                        labelValueHourNS.Anchor = System.Windows.Forms.AnchorStyles.None;
                        labelValueHourNS.AutoSize = true;
                        labelValueHourNS.Location = new System.Drawing.Point(16, 17);
                        labelValueHourNS.Name = "labelValueHourNS" + i.ToString();
                        labelValueHourNS.Size = new System.Drawing.Size(91, 18);
                        labelValueHourNS.TabIndex = 0;
                        labelValueHourNS.Text = "0";
                        labelValueHourNS.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                        ModelLCDHourNSInfo modelLCDHourNSInfo = new ModelLCDHourNSInfo();
                        modelLCDHourNSInfo.intHour = i + 1;
                        modelLCDHourNSInfo.LabelValue = labelValueHourNS;
                        modelLCDTongHopInfo.ListNSGio.Add(modelLCDHourNSInfo);

                        var labelConfigFooter1 = listLabelConfig.Where(c => c.TableLayoutPanelName.Trim() == "tblPanelFooter" && c.Position == 1).FirstOrDefault();
                        if (labelConfigFooter1 != null)
                            DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigFooter1, labelTitleHourNS);

                        var labelConfigFooter2 = listLabelConfig.Where(c => c.TableLayoutPanelName.Trim() == "tblPanelFooter" && c.Position == 2).FirstOrDefault();
                        if (labelConfigFooter2 != null)
                        {
                            DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigFooter2, labelValueHourNS);

                        }

                        this.tblpanelFooter.Controls.Add(labelTitleHourNS, i, 0);
                        this.tblpanelFooter.Controls.Add(labelValueHourNS, i, 1);
                    }
                }
                this.tblpanelFooter.Margin = new System.Windows.Forms.Padding(0);
                this.tblpanelFooter.Size = new System.Drawing.Size(739, 405);
                this.tblpanelFooter.TabIndex = 0;
                this.tblpanelFooter.ResumeLayout(false);
            }
            catch (Exception ex)
            {

                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        //tblpanelBody
        private void BuildPanelBody()
        {
            try
            {
                if (listTableLayoutPanelConfig != null && listTableLayoutPanelConfig.Count > 0)
                {
                    var listtblpanelBody = listTableLayoutPanelConfig.Where(c => c.TableLayoutTableName.Trim() == "tblpanelBody" && c.IsShow).OrderBy(c => c.RowInt).ToList();
                    if (listtblpanelBody.Count > 0)
                    {
                        this.tblpanelBody = new TableLayoutPanel();
                        this.tblpanelBody.SuspendLayout();
                        this.tblpanelBody.ColumnCount = 1;
                        this.tblpanelBody.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
                        this.tblpanelBody.Dock = System.Windows.Forms.DockStyle.Fill;
                        this.tblpanelBody.Location = new System.Drawing.Point(0, 0);
                        this.tblpanelBody.Name = "tblpanelBody";
                        this.tblpanelBody.RowCount = listtblpanelBody.Count;
                        foreach (var item in listtblpanelBody)
                        {
                            float size = 0;
                            float.TryParse(item.SizePercent, out size);
                            this.tblpanelBody.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, size));
                        }
                        this.tblpanelBody.Controls.Add(this.tblpanelHeader, 0, 0);
                        this.tblpanelBody.Controls.Add(this.tblpanelTitle1, 0, 1);
                        this.tblpanelBody.Controls.Add(this.tblpanelTitle2, 0, 2);
                        this.tblpanelBody.Controls.Add(this.tblpanelContent, 0, 3);
                        this.tblpanelBody.Controls.Add(this.tblpanelFooter, 0, 4);
                        this.tblpanelBody.Margin = new System.Windows.Forms.Padding(0);
                        this.tblpanelBody.Size = new System.Drawing.Size(739, 405);
                        this.tblpanelBody.TabIndex = 0;
                        this.Controls.Add(this.tblpanelBody);
                        this.tblpanelBody.ResumeLayout(false);
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        //Load Data for mutiChuyen
        DataTable dtChuyenSanPham = new DataTable();
        DataTable dtNangSuat = new DataTable();
        DataTable dtSanLuongGio = new DataTable();
        private void LoadDataForChuyens()
        {
            try
            {
                if (listChuyen != null && listChuyen.Count > 0)
                {
                    string dateNow = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                    int morth = DateTime.Now.Month;
                    int year = DateTime.Now.Year;
                    int sanLuongKHTong = 0;
                    int sanLuongTHTong = 0;
                    double doanhThuKHTong = 0;
                    double doanhThuTHTong = 0;
                    int dinhMucMoiGio = 0;
                    int dinhMucNgayTong = 0;
                    float nhipDoThucTeTong = 0;
                    float nhipDoSanXuatTong = 0;
                    int soNangXuatTrongNgayCuaTCChuyen = 0;
                    string strListSTTChuyenSanPham = string.Empty;
                    foreach (var chuyen in listChuyen)
                    {
                        dtChuyenSanPham.Clear();
                        int maChuyen = 0;
                        int.TryParse(chuyen.MaChuyen, out maChuyen);
                        int thucHienThang = 0;
                        int keHoachThang = 0;
                        int thucHienNgayTong = 0;
                        int keHoachNgayTong = 0;
                        int luyKeSP = 0;
                        double luyKeDT = 0;
                        double doanhThuKH = 0;
                        double ptThucHien = 0;
                        string strSQL = "SELECT csp.STT, csp.SanLuongKeHoach, csp.LuyKeTH, csp.NangXuatSanXuat, sp.DonGiaCM FROM Chuyen_SanPham csp, SanPham sp Where csp.MaChuyen='" + maChuyen + "' and csp.IsDelete=0 and sp.IsDelete=0 and csp.MaSanPham=sp.MaSanPham and csp.Thang='" + morth + "' and csp.Nam='" + year + "'";
                        //string strSQL = "SELECT csp.STT, csp.SanLuongKeHoach, csp.LuyKeTH, csp.NangXuatSanXuat, sp.DonGiaCM FROM Chuyen_SanPham csp, SanPham sp Where csp.MaChuyen='"+maChuyen+"' and csp.IsDelete=0 and sp.IsDelete=0 and csp.IsFinish=0 and csp.MaSanPham=sp.MaSanPham and csp.Nam='"+year+"'";
                        if (sqlConnection.State == ConnectionState.Open)
                        {
                            sqlConnection.Close();
                        }
                        sqlConnection.Open();
                        sqlDataAdapter = new SqlDataAdapter(strSQL, sqlConnection);
                        sqlDataAdapter.Fill(dtChuyenSanPham);
                        if (dtChuyenSanPham != null && dtChuyenSanPham.Rows.Count > 0)
                        {
                            foreach (DataRow row in dtChuyenSanPham.Rows)
                            {
                                int sanLuongKeHoach = 0;
                                int.TryParse(row["SanLuongKeHoach"].ToString(), out sanLuongKeHoach);
                                int luyKeTH = 0;
                                int.TryParse(row["LuyKeTH"].ToString(), out luyKeTH);
                                double donGiaCM = 0;
                                double.TryParse(row["DonGiaCM"].ToString(), out donGiaCM);
                                int sttChuyenSanPham = 0;
                                int.TryParse(row["STT"].ToString(), out sttChuyenSanPham);
                                strListSTTChuyenSanPham += sttChuyenSanPham.ToString() + ",";
                                thucHienThang += luyKeTH;
                                keHoachThang += sanLuongKeHoach;
                                luyKeSP += luyKeTH;
                                luyKeDT += (luyKeTH * donGiaCM);
                                doanhThuKH += (sanLuongKeHoach * donGiaCM);
                                string sqlNangSuat = "Select nx.DinhMucNgay, nx.ThucHienNgay, nx.ThucHienNgayGiam, nx.NhipDoThucTe, nx.NhipDoSanXuat  From NangXuat nx Where nx.STTChuyen_SanPham='" + sttChuyenSanPham + "' and nx.Ngay='" + dateNow + "'";
                                if (sqlConnection.State == ConnectionState.Open)
                                {
                                    sqlConnection.Close();
                                }
                                sqlConnection.Open();
                                sqlDataAdapter = new SqlDataAdapter(sqlNangSuat, sqlConnection);
                                dtNangSuat.Clear();
                                sqlDataAdapter.Fill(dtNangSuat);
                                if (dtNangSuat != null && dtNangSuat.Rows.Count > 0)
                                {
                                    foreach (DataRow rowNangSuat in dtNangSuat.Rows)
                                    {
                                        double dinhMucNgay = 0;
                                        double.TryParse(rowNangSuat["DinhMucNgay"].ToString(), out dinhMucNgay);
                                        int thucHienNgay = 0;
                                        int.TryParse(rowNangSuat["ThucHienNgay"].ToString(), out thucHienNgay);
                                        int thuHienNgayGiam = 0;
                                        int.TryParse(rowNangSuat["ThucHienNgayGiam"].ToString(), out thuHienNgayGiam);
                                        thucHienNgayTong += (thucHienNgay - thuHienNgayGiam);
                                        keHoachNgayTong += (int)dinhMucNgay;
                                        float nhipDoThucTe = 0;
                                        float nhipDoSanXuat = 0;
                                        float.TryParse(rowNangSuat["NhipDoThucTe"].ToString(), out nhipDoThucTe);
                                        float.TryParse(rowNangSuat["NhipDoSanXuat"].ToString(), out nhipDoSanXuat);
                                        nhipDoSanXuatTong += nhipDoSanXuat;
                                        nhipDoThucTeTong += nhipDoThucTe;
                                        soNangXuatTrongNgayCuaTCChuyen++;
                                    }
                                }
                            }
                            ptThucHien += (thucHienThang * 100) / keHoachThang;
                            sanLuongKHTong += keHoachThang;
                            sanLuongTHTong += luyKeSP;
                            doanhThuKHTong += doanhThuKH;
                            doanhThuTHTong += luyKeDT;
                            dinhMucNgayTong += keHoachNgayTong;
                            for (int index = 0; index < modelLCDTongHopInfo.ListLineNS.Count; index++)
                            {
                                if (modelLCDTongHopInfo.ListLineNS[index].MaChuyen == maChuyen)
                                {
                                    if (modelLCDTongHopInfo.ListLineNS[index].ListLabelNSInfo.Count > 0)
                                    {
                                        for (int ilabel = 0; ilabel < modelLCDTongHopInfo.ListLineNS[index].ListLabelNSInfo.Count; ilabel++)
                                        {
                                            switch (modelLCDTongHopInfo.ListLineNS[index].ListLabelNSInfo[ilabel].SystemName)
                                            {
                                                case "LineName": modelLCDTongHopInfo.ListLineNS[index].ListLabelNSInfo[ilabel].Label.Text = chuyen.TenChuyen; break;
                                                case "THMorth": modelLCDTongHopInfo.ListLineNS[index].ListLabelNSInfo[ilabel].Label.Text = (thucHienThang == 0 ? "0" : String.Format("{0:#,###}", thucHienThang)); break;
                                                case "KHMorth": modelLCDTongHopInfo.ListLineNS[index].ListLabelNSInfo[ilabel].Label.Text = (keHoachThang == 0 ? "0" : String.Format("{0:#,###}", keHoachThang)); break;
                                                case "THDay": modelLCDTongHopInfo.ListLineNS[index].ListLabelNSInfo[ilabel].Label.Text = (thucHienNgayTong == 0 ? "0" : String.Format("{0:#,###}", thucHienNgayTong)); break;
                                                case "KHDay": modelLCDTongHopInfo.ListLineNS[index].ListLabelNSInfo[ilabel].Label.Text = (keHoachNgayTong == 0 ? "0" : String.Format("{0:#,###}", keHoachNgayTong)); break;
                                                case "LuyKeSP": modelLCDTongHopInfo.ListLineNS[index].ListLabelNSInfo[ilabel].Label.Text = (luyKeDT == 0 ? "0" : String.Format("{0:#,###}", luyKeDT)); break;
                                                case "LuyKeDT": modelLCDTongHopInfo.ListLineNS[index].ListLabelNSInfo[ilabel].Label.Text = (doanhThuKH == 0 ? "0" : String.Format("{0:#,###}", doanhThuKH)); break;
                                                case "PTThucHien": modelLCDTongHopInfo.ListLineNS[index].ListLabelNSInfo[ilabel].Label.Text = (ptThucHien == 0 ? "0" : String.Format("{0:#,###}", ptThucHien)); break;
                                            }
                                        }
                                    }
                                    break;
                                }
                            }
                        }

                    }
                    modelLCDTongHopInfo.LabelKeHoachTong.Text = "SẢN LƯỢNG (TH/KH): " + (sanLuongTHTong == 0 ? "0" : String.Format("{0:#,###}", sanLuongTHTong)) + " / " + (sanLuongKHTong == 0 ? "0" : String.Format("{0:#,###}", sanLuongKHTong));
                    modelLCDTongHopInfo.LabelDoanhThuTong.Text = "DOANH THU (TH/KH): " + (doanhThuTHTong == 0 ? "0" : String.Format("{0:#,###}", doanhThuTHTong)) + " / " + (doanhThuKHTong == 0 ? "0" : String.Format("{0:#,###}", doanhThuKHTong));
                    if (soNangXuatTrongNgayCuaTCChuyen == 0)
                        modelLCDTongHopInfo.LabelNhipSanXuat.Text = "NHỊP SẢN XUẤT (TH/KH): 0 / 0";
                    else
                    {
                        nhipDoThucTeTong = (int)(nhipDoThucTeTong / soNangXuatTrongNgayCuaTCChuyen);
                        nhipDoSanXuatTong = (int)(nhipDoSanXuatTong / soNangXuatTrongNgayCuaTCChuyen);
                        modelLCDTongHopInfo.LabelNhipSanXuat.Text = "NHỊP SẢN XUẤT (TH/KH): " + (nhipDoThucTeTong == 0 ? "0" : String.Format("{0:#,###}", nhipDoThucTeTong)) + " / " + (nhipDoSanXuatTong == 0 ? "0" : String.Format("{0:#,###}", nhipDoSanXuatTong));
                    }
                    dinhMucMoiGio = dinhMucNgayTong / maxWorkTimeOfLines;

                    if (!string.IsNullOrEmpty(strListSTTChuyenSanPham))
                    {
                        strListSTTChuyenSanPham = strListSTTChuyenSanPham.Substring(0, strListSTTChuyenSanPham.Length - 1);
                        foreach (var nsGio in modelLCDTongHopInfo.ListNSGio)
                        {
                            string sqlSanLuongGio = string.Empty;
                            if (listModelWorkHours != null && listModelWorkHours.Count > 0)
                            {
                                foreach (var model in listModelWorkHours)
                                {
                                    if (model.IntHours == nsGio.intHour)
                                    {
                                        sqlSanLuongGio = "select (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen in (" + AccountSuccess.strListChuyenId.Trim() + ") and Time >= '" + model.TimeStart + "' and Time <='" + model.TimeEnd + "' and Date='" + dateNow + "' and STTChuyenSanPham in (" + strListSTTChuyenSanPham + ") and CommandTypeId=" + (int)eCommandRecive.ProductIncrease + " and ProductOutputTypeId=" + (int)eProductOutputType.KCS + " and IsEndOfLine=1) AS SanLuongTang, (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen in (" + AccountSuccess.strListChuyenId.Trim() + ") and Time >= '" + model.TimeStart + "' and Time <='" + model.TimeEnd + "' and Date='" + dateNow + "' and STTChuyenSanPham in (" + strListSTTChuyenSanPham + ") and CommandTypeId=" + (int)eCommandRecive.ProductReduce + " and ProductOutputTypeId=" + (int)eProductOutputType.KCS + " and IsEndOfLine=1) AS SanLuongGiam";
                                        break;
                                    }

                                }
                            }
                            int sanLuongGio = 0;
                            int sanLuongGioTang = 0;
                            int sanLuongGioGiam = 0;
                            if (sqlConnection.State == ConnectionState.Open)
                            {
                                sqlConnection.Close();
                            }
                            sqlConnection.Open();
                            sqlDataAdapter = new SqlDataAdapter(sqlSanLuongGio, sqlConnection);
                            dtSanLuongGio.Clear();
                            sqlDataAdapter.Fill(dtSanLuongGio);
                            if (dtSanLuongGio != null && dtSanLuongGio.Rows.Count > 0)
                            {
                                DataRow rowSanLuongGio = dtSanLuongGio.Rows[0];
                                if (rowSanLuongGio["SanLuongTang"] != null)
                                    int.TryParse(rowSanLuongGio["SanLuongTang"].ToString(), out sanLuongGioTang);
                                if (rowSanLuongGio["SanLuongGiam"] != null)
                                    int.TryParse(rowSanLuongGio["SanLuongGiam"].ToString(), out sanLuongGioGiam);
                                sanLuongGio = sanLuongGioTang - sanLuongGioGiam;
                                if (sanLuongGio < 0)
                                    sanLuongGio = 0;
                            }
                            nsGio.LabelValue.Text = sanLuongGio.ToString() + "/" + dinhMucMoiGio.ToString();

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }











        private void LoadDataForChuyens_O()
        {
            try
            {
                if (listChuyen != null && listChuyen.Count > 0)
                {
                    string dateNow = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                    int sanLuongKHTong = 0, sanLuongTHTong = 0, dinhMucMoiGio = 0, dinhMucNgayTong = 0, soNangXuatTrongNgayCuaTCChuyen = 0;
                    double doanhThuKHTong = 0, doanhThuTHTong = 0, nhipDoThucTeTong = 0, nhipDoSanXuatTong = 0;
                    string strListSTTChuyenSanPham = string.Empty;

                    var nxInDay = BLLProductivity.GetProductivitiesInDay(dateNow, AccountSuccess.strListChuyenId.Split(',').Select(x => Convert.ToInt32(x)).ToList());

                    if (nxInDay != null && nxInDay.Count > 0)
                    {
                        #region
                        foreach (var chuyen in listChuyen)
                        {
                            int thucHienThang = 0, keHoachThang = 0, thucHienNgayTong = 0, keHoachNgayTong = 0, luyKeSP = 0;
                            double luyKeDT = 0, doanhThuKH = 0, ptThucHien = 0;

                            #region
                            var nxOfLine = nxInDay.Where(x => x.LineId == int.Parse(chuyen.MaChuyen)).ToList();
                            if (nxOfLine != null && nxOfLine.Count > 0)
                            {
                                foreach (var item in nxOfLine)
                                {
                                    strListSTTChuyenSanPham += item.STTCHuyen_SanPham + ",";
                                    thucHienThang += item.LK_TH;
                                    keHoachThang += item.ProductionPlans;
                                    luyKeSP += item.LK_TH;
                                    luyKeDT += (item.LK_TH * item.ProductPriceCM);
                                    doanhThuKH += (item.ProductionPlans * item.ProductPriceCM);
                                    thucHienNgayTong += (item.ThucHienNgay - item.ThucHienNgayGiam);
                                    keHoachNgayTong += (int)item.DinhMucNgay;
                                    nhipDoSanXuatTong += item.NhipDoSanXuat;
                                    nhipDoThucTeTong += item.NhipDoThucTe;
                                    soNangXuatTrongNgayCuaTCChuyen++;
                                }

                                ptThucHien += (thucHienThang * 100) / keHoachThang;
                                sanLuongKHTong += keHoachThang;
                                sanLuongTHTong += luyKeSP;
                                doanhThuKHTong += doanhThuKH;
                                doanhThuTHTong += luyKeDT;
                                dinhMucNgayTong += keHoachNgayTong;

                                for (int index = 0; index < modelLCDTongHopInfo.ListLineNS.Count; index++)
                                {
                                    if (modelLCDTongHopInfo.ListLineNS[index].MaChuyen == int.Parse(chuyen.MaChuyen))
                                    {
                                        if (modelLCDTongHopInfo.ListLineNS[index].ListLabelNSInfo.Count > 0)
                                        {
                                            for (int ilabel = 0; ilabel < modelLCDTongHopInfo.ListLineNS[index].ListLabelNSInfo.Count; ilabel++)
                                            {
                                                switch (modelLCDTongHopInfo.ListLineNS[index].ListLabelNSInfo[ilabel].SystemName)
                                                {
                                                    case "LineName": modelLCDTongHopInfo.ListLineNS[index].ListLabelNSInfo[ilabel].Label.Text = chuyen.TenChuyen; break;
                                                    case "THMorth": modelLCDTongHopInfo.ListLineNS[index].ListLabelNSInfo[ilabel].Label.Text = (thucHienThang == 0 ? "0" : String.Format("{0:#,###}", thucHienThang)); break;
                                                    case "KHMorth": modelLCDTongHopInfo.ListLineNS[index].ListLabelNSInfo[ilabel].Label.Text = (keHoachThang == 0 ? "0" : String.Format("{0:#,###}", keHoachThang)); break;
                                                    case "THDay": modelLCDTongHopInfo.ListLineNS[index].ListLabelNSInfo[ilabel].Label.Text = (thucHienNgayTong == 0 ? "0" : String.Format("{0:#,###}", thucHienNgayTong)); break;
                                                    case "KHDay": modelLCDTongHopInfo.ListLineNS[index].ListLabelNSInfo[ilabel].Label.Text = (keHoachNgayTong == 0 ? "0" : String.Format("{0:#,###}", keHoachNgayTong)); break;
                                                    case "LuyKeSP": modelLCDTongHopInfo.ListLineNS[index].ListLabelNSInfo[ilabel].Label.Text = (luyKeDT == 0 ? "0" : String.Format("{0:#,###}", luyKeDT)); break;
                                                    case "LuyKeDT": modelLCDTongHopInfo.ListLineNS[index].ListLabelNSInfo[ilabel].Label.Text = (doanhThuKH == 0 ? "0" : String.Format("{0:#,###}", doanhThuKH)); break;
                                                    case "PTThucHien": modelLCDTongHopInfo.ListLineNS[index].ListLabelNSInfo[ilabel].Label.Text = (ptThucHien == 0 ? "0" : String.Format("{0:#,###}", ptThucHien)); break;
                                                }
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                            #endregion
                        }
                        modelLCDTongHopInfo.LabelKeHoachTong.Text = "SẢN LƯỢNG (TH/KH): " + (sanLuongTHTong == 0 ? "0" : String.Format("{0:#,###}", sanLuongTHTong)) + " / " + (sanLuongKHTong == 0 ? "0" : String.Format("{0:#,###}", sanLuongKHTong));
                        modelLCDTongHopInfo.LabelDoanhThuTong.Text = "DOANH THU (TH/KH): " + (doanhThuTHTong == 0 ? "0" : String.Format("{0:#,###}", doanhThuTHTong)) + " / " + (doanhThuKHTong == 0 ? "0" : String.Format("{0:#,###}", doanhThuKHTong));
                        if (soNangXuatTrongNgayCuaTCChuyen == 0)
                            modelLCDTongHopInfo.LabelNhipSanXuat.Text = "NHỊP SẢN XUẤT (TH/KH): 0 / 0";
                        else
                        {
                            nhipDoThucTeTong = (int)(nhipDoThucTeTong / soNangXuatTrongNgayCuaTCChuyen);
                            nhipDoSanXuatTong = (int)(nhipDoSanXuatTong / soNangXuatTrongNgayCuaTCChuyen);
                            modelLCDTongHopInfo.LabelNhipSanXuat.Text = "NHỊP SẢN XUẤT (TH/KH): " + (nhipDoThucTeTong == 0 ? "0" : String.Format("{0:#,###}", nhipDoThucTeTong)) + " / " + (nhipDoSanXuatTong == 0 ? "0" : String.Format("{0:#,###}", nhipDoSanXuatTong));
                        }
                        dinhMucMoiGio = dinhMucNgayTong / maxWorkTimeOfLines;

                        if (!string.IsNullOrEmpty(strListSTTChuyenSanPham))
                        {
                            #region
                            strListSTTChuyenSanPham = strListSTTChuyenSanPham.Substring(0, strListSTTChuyenSanPham.Length - 1);
                            var tdns = BLLDayInfo.GetByStt_CSP(strListSTTChuyenSanPham.Split(',').Select(x => Convert.ToInt32(x)).ToList(), dateNow);
                            foreach (var nsGio in modelLCDTongHopInfo.ListNSGio)
                            {
                                int sanLuongGioTang = 0, sanLuongGioGiam = 0;
                                if (listModelWorkHours != null && listModelWorkHours.Count > 0 && tdns != null && tdns.Count > 0)
                                {
                                    foreach (var model in listModelWorkHours)
                                    {
                                        if (model.IntHours == nsGio.intHour)
                                        {
                                            sanLuongGioTang = tdns.Where(x => x.Time >= model.TimeStart && x.Time <= model.TimeEnd && x.CommandTypeId == (int)eCommandRecive.ProductIncrease && x.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(x => x.ThanhPham);
                                            sanLuongGioGiam = tdns.Where(x => x.Time >= model.TimeStart && x.Time <= model.TimeEnd && x.CommandTypeId == (int)eCommandRecive.ProductReduce && x.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(x => x.ThanhPham);
                                            sanLuongGioTang = sanLuongGioTang - sanLuongGioGiam;
                                            break;
                                        }
                                    }
                                }
                                nsGio.LabelValue.Text = (sanLuongGioTang > 0 ? sanLuongGioTang : 0) + "/" + dinhMucMoiGio;
                            }
                            #endregion
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get data theo Timer
        /// </summary>
        private void LoadDataForChuyens_N()
        {
            try
            {
                if (listChuyen != null && listChuyen.Count > 0)
                {
                    string dateNow = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                    var LCDData = BLLProductivity.GetProductivitiesForLCDCollection(dateNow, AccountSuccess.strListChuyenId.Split(',').Select(x => Convert.ToInt32(x)).ToList());
                    if (LCDData != null && LCDData.Lines.Count > 0)
                    {
                        #region
                        foreach (var line in LCDData.Lines)
                        {
                            for (int index = 0; index < modelLCDTongHopInfo.ListLineNS.Count; index++)
                            {
                                if (modelLCDTongHopInfo.ListLineNS[index].MaChuyen == line.LineId)
                                {
                                    if (modelLCDTongHopInfo.ListLineNS[index].ListLabelNSInfo.Count > 0)
                                    {
                                        for (int ilabel = 0; ilabel < modelLCDTongHopInfo.ListLineNS[index].ListLabelNSInfo.Count; ilabel++)
                                        {
                                            switch (modelLCDTongHopInfo.ListLineNS[index].ListLabelNSInfo[ilabel].SystemName)
                                            {
                                                case "LineName": modelLCDTongHopInfo.ListLineNS[index].ListLabelNSInfo[ilabel].Label.Text = line.LineName; break;
                                                case "THMorth": modelLCDTongHopInfo.ListLineNS[index].ListLabelNSInfo[ilabel].Label.Text = (line.LK_TH == 0 ? "0" : String.Format("{0:#,###}", line.LK_TH)); break;
                                                case "KHMorth": modelLCDTongHopInfo.ListLineNS[index].ListLabelNSInfo[ilabel].Label.Text = (line.ProductionPlans == 0 ? "0" : String.Format("{0:#,###}", line.ProductionPlans)); break;
                                                case "THDay": modelLCDTongHopInfo.ListLineNS[index].ListLabelNSInfo[ilabel].Label.Text = (line.ThucHienNgay == 0 ? "0" : String.Format("{0:#,###}",  line.ThucHienNgay )); break;
                                                case "KHDay": modelLCDTongHopInfo.ListLineNS[index].ListLabelNSInfo[ilabel].Label.Text = (line.DinhMucNgay == 0 ? "0" : String.Format("{0:#,###}", line.DinhMucNgay)); break;
                                                case "LuyKeSP": modelLCDTongHopInfo.ListLineNS[index].ListLabelNSInfo[ilabel].Label.Text = (line.RevenueTH == 0 ? "0" : String.Format("{0:#,###}", line.RevenueTH)); break;
                                                case "LuyKeDT": modelLCDTongHopInfo.ListLineNS[index].ListLabelNSInfo[ilabel].Label.Text = (line.RevenuePlan == 0 ? "0" : String.Format("{0:#,###}", line.RevenuePlan)); break;
                                                case "PTThucHien": modelLCDTongHopInfo.ListLineNS[index].ListLabelNSInfo[ilabel].Label.Text = (line.PercentTH == 0 ? "0" : String.Format("{0:#,###}", line.PercentTH)); break;
                                            }
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        #endregion

                        modelLCDTongHopInfo.LabelKeHoachTong.Text = "SẢN LƯỢNG (TH/KH): " + String.Format("{0:#,###}", LCDData.Lines.Sum(x => x.LK_TH)) + " / " + String.Format("{0:#,###}", LCDData.Lines.Sum(x => x.ProductionPlans));
                        modelLCDTongHopInfo.LabelDoanhThuTong.Text = "DOANH THU (TH/KH): " + String.Format("{0:#,###}", LCDData.Lines.Sum(x => x.RevenueTH)) + " / " + String.Format("{0:#,###}", LCDData.Lines.Sum(x => x.RevenuePlan));
                        modelLCDTongHopInfo.LabelNhipSanXuat.Text = "NHỊP SẢN XUẤT (TH/KH): " + String.Format("{0:#,###}", LCDData.NhipTT) + " / " + String.Format("{0:#,###}", LCDData.NhipSX);

                        #region tt nang xuat gio
                        double dinhMucMoiGio = Math.Round(LCDData.Lines.Sum(x => x.DinhMucNgay) / maxWorkTimeOfLines);
                        foreach (var nsGio in modelLCDTongHopInfo.ListNSGio)
                        {
                            int sanLuongGioTang = 0, sanLuongGioGiam = 0;
                            if (listModelWorkHours != null && listModelWorkHours.Count > 0 && LCDData.DayInfos.Count > 0)
                            {
                                foreach (var model in listModelWorkHours)
                                {
                                    if (model.IntHours == nsGio.intHour)
                                    {
                                        sanLuongGioTang = LCDData.DayInfos.Where(x => x.Time >= model.TimeStart && x.Time <= model.TimeEnd && x.CommandTypeId == (int)eCommandRecive.ProductIncrease && x.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(x => x.ThanhPham);
                                        sanLuongGioGiam = LCDData.DayInfos.Where(x => x.Time >= model.TimeStart && x.Time <= model.TimeEnd && x.CommandTypeId == (int)eCommandRecive.ProductReduce && x.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(x => x.ThanhPham);
                                        sanLuongGioTang = sanLuongGioTang - sanLuongGioGiam;
                                        break;
                                    }
                                }
                            }
                            nsGio.LabelValue.Text = (sanLuongGioTang > 0 ? sanLuongGioTang : 0) + "/" + dinhMucMoiGio;
                        }
                        #endregion
                    }                     
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
