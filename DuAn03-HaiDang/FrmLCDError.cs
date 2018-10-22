using DuAn03_HaiDang.DAO;
using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.Enum;
using DuAn03_HaiDang.KeyPad_Chuyen.dao;
using DuAn03_HaiDang.KeyPad_Chuyen.pojo;
using DuAn03_HaiDang.POJO;
using PMS.Business;
using PMS.Business.Enum;
using PMS.Business.Models;
using QuanLyNangSuat.DAO;
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

namespace QuanLyNangSuat
{
    public partial class FrmLCDError : Form
    {
        TableLayoutPanelConfigDAO tableLayoutPanelConfigDAO = new TableLayoutPanelConfigDAO();
        PanelConfigDAO panelConfigDAO = new PanelConfigDAO();
        LabelConfigDAO labelConfigDAO = new LabelConfigDAO();
      //  ConfigDAO configDAO = new ConfigDAO();
        ChuyenDAO chuyenDAO = new ChuyenDAO();
      //  GroupErrorDAO groupErrorDAO = new GroupErrorDAO();
        List<TableLayoutPanelConfig> listTableLayoutPanelConfig;
        List<PanelConfig> listPanelConfig;
        List<LabelConfig> listLabelConfig;
        List<LabelForTablePanel> listLabelForTablePanel;
        List<Chuyen> listChuyen;
        string titleLCD = string.Empty;
        int soRowTrenMH = 1;
        int tableType = 4;
        string logo = string.Empty;
        Label labelTime;
        Label labelDate;
        SqlConnection sqlConnection;
        SqlDataAdapter sqlDataAdapter;
        int maxWorkTimeOfLines = 0;
        TimeSpan timeStartWork = new TimeSpan(0, 0, 0);
      //  ModelLCDError modelLCDError;
       List<GroupErrorModel> modelLCDError;
        private System.Windows.Forms.Timer timerShow;
        private System.Windows.Forms.Timer timerDateTime;
        Chuyen line;
        List<ModelShowLCDError> listModelShowLCDError = new List<ModelShowLCDError>();
        List<int> listIndexRowIsGroupError = new List<int>();
        //Layout
        #region Layout
        private System.Windows.Forms.TableLayoutPanel tblpanelBody;
        private System.Windows.Forms.TableLayoutPanel tblpanelHeader;
        private System.Windows.Forms.TableLayoutPanel tblpanelContent;
        private System.Windows.Forms.TableLayoutPanel tblpanelTitle1;

        #endregion Layout
        public FrmLCDError(SqlConnection _sqlConnection)
        {
            InitializeComponent();
            this.timerShow = new System.Windows.Forms.Timer();
            this.timerDateTime = new System.Windows.Forms.Timer();
            this.sqlConnection = _sqlConnection;
        }

        private void LoadLCDConfig()
        {
            try
            {
               // List<Config> listConfig = configDAO.GetConfig();
                var listConfig = BLLConfig.Instance.GetShowLCDConfig();
                if (listConfig != null && listConfig.Count > 0)
                {
                    foreach (var config in listConfig)
                    {
                        switch (config.Name.Trim())
                        {
                            case "ThoiGianLayDuLieuLCDLoi":
                                int interval = 1000;
                                int.TryParse(config.Value, out interval);
                                initTimerShow(interval);
                                break;
                            case "SoRowTrenLCDLoi":
                                int.TryParse(config.Value, out soRowTrenMH);
                                break;
                            case "Logo":
                                logo = config.Value;
                                break;
                            case "TieuDeLCDLoi":
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
                            MessageBox.Show("Không tìm thấy Logo với đường dẫn : " + (Application.StartupPath + @logo));
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

        //tblpanelContent
        private void BuildPanelContent()
        {
            try
            {
                if (listTableLayoutPanelConfig != null && listTableLayoutPanelConfig.Count > 0)
                {

                    this.tblpanelContent = new TableLayoutPanel();
                    this.tblpanelContent.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
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

                    if (listTableLayoutPanelConfig != null && listTableLayoutPanelConfig.Count > 0)
                    {
                        var listtblpanelContent = listTableLayoutPanelConfig.Where(c => c.TableLayoutTableName.Trim() == "tblpanelContent" && c.IsShow).OrderBy(c => c.ColumnInt).ToList();
                        if (listtblpanelContent.Count > 0)
                        {
                            this.tblpanelContent.ColumnCount = listtblpanelContent.Count;
                            foreach (var item in listtblpanelContent)
                            {
                                float sizeColumn = 0;
                                float.TryParse(item.SizePercent, out sizeColumn);
                                this.tblpanelContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, sizeColumn));
                            }

                        }
                    }
                    this.tblpanelContent.Margin = new System.Windows.Forms.Padding(0);
                    this.tblpanelContent.Size = new System.Drawing.Size(739, 405);
                    this.tblpanelContent.TabIndex = 0;

                    if ( modelLCDError.Count >0)
                    {
                        this.tblpanelContent.RowCount = modelLCDError.Count;

                        float sireRow = 100 / modelLCDError.Count;
                        int index = -1;
                        var labelConfigContent = listLabelConfig.Where(c => c.TableLayoutPanelName.Trim() == "tblPanelContent" && c.Position == 2).FirstOrDefault();
                        var labelConfigContentTitle = listLabelConfig.Where(c => c.TableLayoutPanelName.Trim() == "tblPanelContent" && c.Position == 1).FirstOrDefault();
                        for (int j = 0; j < modelLCDError.Count; j++)
                        {
                            var item = modelLCDError[j];
                            List<TableLayoutPanel> listTableLayoutPanel = new List<TableLayoutPanel>();
                            if (item.ErrorsObj.Count > 0)
                            {
                                for (int k = 0; k < item.ErrorsObj.Count; k++)
                                {
                                    this.tblpanelContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, sireRow));
                                    var itemError = item.ErrorsObj[k];
                                    index++;
                                    DateTime dateNow = DateTime.Now;

                                    var labelTitle = new Label();
                                    labelTitle.Anchor = System.Windows.Forms.AnchorStyles.None;
                                    labelTitle.AutoSize = true;
                                    labelTitle.Location = new System.Drawing.Point(16, 17);
                                    labelTitle.Name = "label";
                                    labelTitle.Size = new System.Drawing.Size(91, 18);
                                    labelTitle.TabIndex = 0;
                                    labelTitle.Text = itemError.Name;
                                    labelTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                                    if (labelConfigContentTitle != null)
                                        DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigContentTitle, labelTitle);
                                    this.tblpanelContent.Controls.Add(labelTitle, 0, index);

                                    var labelGioTruoc = new Label();
                                    labelGioTruoc.Anchor = System.Windows.Forms.AnchorStyles.None;
                                    labelGioTruoc.AutoSize = true;
                                    labelGioTruoc.Location = new System.Drawing.Point(16, 17);
                                    labelGioTruoc.Name = "label";
                                    labelGioTruoc.Size = new System.Drawing.Size(91, 18);
                                    labelGioTruoc.TabIndex = 0;
                                    labelGioTruoc.Text = "0";
                                    labelGioTruoc.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                                    if (labelConfigContent != null)
                                        DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigContent, labelGioTruoc);
                                    this.tblpanelContent.Controls.Add(labelGioTruoc, 1, index);
                                    ModelShowLCDError modelGT = new ModelShowLCDError();
                                    modelGT.ErrorId = itemError.Id;
                                    modelGT.GroupErrorId = item.Id;
                                    modelGT.LabelType = 1;
                                    modelGT.Label = labelGioTruoc;
                                    listModelShowLCDError.Add(modelGT);


                                    var labelGioHienTai = new Label();
                                    labelGioHienTai.Anchor = System.Windows.Forms.AnchorStyles.None;
                                    labelGioHienTai.AutoSize = true;
                                    labelGioHienTai.Location = new System.Drawing.Point(16, 17);
                                    labelGioHienTai.Name = "label";
                                    labelGioHienTai.Size = new System.Drawing.Size(91, 18);
                                    labelGioHienTai.TabIndex = 0;
                                    labelGioHienTai.Text = "0";
                                    labelGioHienTai.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                                    if (labelConfigContent != null)
                                        DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigContent, labelGioHienTai);
                                    this.tblpanelContent.Controls.Add(labelGioHienTai, 2, index);
                                    ModelShowLCDError modelHT = new ModelShowLCDError();
                                    modelHT.ErrorId = itemError.Id;
                                    modelHT.GroupErrorId = item.Id;
                                    modelHT.LabelType = 2;
                                    modelHT.Label = labelGioHienTai;
                                    listModelShowLCDError.Add(modelHT);


                                    var labelTong = new Label();
                                    labelTong.Anchor = System.Windows.Forms.AnchorStyles.None;
                                    labelTong.AutoSize = true;
                                    labelTong.Location = new System.Drawing.Point(16, 17);
                                    labelTong.Name = "label";
                                    labelTong.Size = new System.Drawing.Size(91, 18);
                                    labelTong.TabIndex = 0;
                                    labelTong.Text = "0";
                                    labelTong.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                                    if (labelConfigContent != null)
                                        DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigContent, labelTong);
                                    this.tblpanelContent.Controls.Add(labelTong, 3, index);
                                    ModelShowLCDError modelT = new ModelShowLCDError();
                                    modelT.ErrorId = itemError.Id;
                                    modelT.GroupErrorId = item.Id;
                                    modelT.LabelType = 3;
                                    modelT.Label = labelTong;
                                    listModelShowLCDError.Add(modelT);
                                }

                                index++;
                                this.tblpanelContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, sireRow));
                                var labelNhom = new Label();
                                labelNhom.Anchor = System.Windows.Forms.AnchorStyles.None;
                                labelNhom.AutoSize = true;
                                labelNhom.Location = new System.Drawing.Point(16, 17);
                                labelNhom.Name = "label";
                                labelNhom.Size = new System.Drawing.Size(91, 18);
                                labelNhom.TabIndex = 0;
                                labelNhom.Text = item.Name;
                                labelNhom.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                                if (labelConfigContentTitle != null)
                                    DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigContentTitle, labelNhom);

                                labelNhom.BackColor = Color.Transparent;
                                this.tblpanelContent.Controls.Add(labelNhom, 0, index);
                                listIndexRowIsGroupError.Add(index);

                                var labelGioTruocN = new Label();
                                labelGioTruocN.Anchor = System.Windows.Forms.AnchorStyles.None;
                                labelGioTruocN.AutoSize = true;
                                labelGioTruocN.Location = new System.Drawing.Point(16, 17);
                                labelGioTruocN.Name = "label";
                                labelGioTruocN.Size = new System.Drawing.Size(91, 18);
                                labelGioTruocN.TabIndex = 0;
                                labelGioTruocN.Text = "0";
                                labelGioTruocN.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                                labelGioTruocN.BackColor = Color.Transparent;
                                if (labelConfigContent != null)
                                    DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigContent, labelGioTruocN);
                                this.tblpanelContent.Controls.Add(labelGioTruocN, 1, index);
                                ModelShowLCDError modelGTN = new ModelShowLCDError();
                                modelGTN.ErrorId = 0;
                                modelGTN.GroupErrorId = item.Id;
                                modelGTN.LabelType = 1;
                                modelGTN.Label = labelGioTruocN;
                                listModelShowLCDError.Add(modelGTN);

                                var labelGioHienTaiN = new Label();
                                labelGioHienTaiN.Anchor = System.Windows.Forms.AnchorStyles.None;
                                labelGioHienTaiN.AutoSize = true;
                                labelGioHienTaiN.Location = new System.Drawing.Point(16, 17);
                                labelGioHienTaiN.Name = "label";
                                labelGioHienTaiN.Size = new System.Drawing.Size(91, 18);
                                labelGioHienTaiN.TabIndex = 0;
                                labelGioHienTaiN.Text = "0";
                                labelGioHienTaiN.BackColor = Color.Transparent;
                                labelGioHienTaiN.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                                if (labelConfigContent != null)
                                    DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigContent, labelGioHienTaiN);
                                this.tblpanelContent.Controls.Add(labelGioHienTaiN, 2, index);
                                ModelShowLCDError modelHTN = new ModelShowLCDError();
                                modelHTN.ErrorId = 0;
                                modelHTN.GroupErrorId = item.Id;
                                modelHTN.LabelType = 2;
                                modelHTN.Label = labelGioHienTaiN;
                                listModelShowLCDError.Add(modelHTN);


                                var labelTongN = new Label();
                                labelTongN.Anchor = System.Windows.Forms.AnchorStyles.None;
                                labelTongN.AutoSize = true;
                                labelTongN.Location = new System.Drawing.Point(16, 17);
                                labelTongN.Name = "label";
                                labelTongN.Size = new System.Drawing.Size(91, 18);
                                labelTongN.TabIndex = 0;
                                labelTongN.Text = "0";
                                labelTongN.BackColor = Color.Transparent;
                                labelTongN.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                                if (labelConfigContent != null)
                                    DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigContent, labelTongN);
                                this.tblpanelContent.Controls.Add(labelTongN, 3, index);
                                ModelShowLCDError modelTN = new ModelShowLCDError();
                                modelTN.ErrorId = 0;
                                modelTN.GroupErrorId = item.Id;
                                modelTN.LabelType = 3;
                                modelTN.Label = labelTongN;
                                listModelShowLCDError.Add(modelTN);
                            }
                        }
                    }
                    this.tblpanelContent.CellPaint += new System.Windows.Forms.TableLayoutCellPaintEventHandler(this.tableLayoutContent_CellPaint);
                    this.tblpanelContent.ResumeLayout(false);

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
                var shifts = TTCaCuaChuyen(maChuyen);
                if (shifts != null && shifts.Count > 0)
                {
                    foreach (var shift in shifts)
                    {
                        sumHours += shift.TimeEnd.TotalHours - shift.TimeStart.TotalHours;
                        sumMinuterWork += shift.TimeEnd.TotalMinutes - shift.TimeStart.TotalMinutes;
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

        private int GetWorkTimeOfLine()
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
                            maxWorkTime = workTime;
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
        private void BuildPanelTitle1()
        {
            try
            {
                this.tblpanelTitle1 = new TableLayoutPanel();
                this.tblpanelTitle1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
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
                this.tblpanelTitle1.Name = "tblpanelFooter";


                if (listTableLayoutPanelConfig != null && listTableLayoutPanelConfig.Count > 0)
                {
                    var listtblpanelTitle1 = listTableLayoutPanelConfig.Where(c => c.TableLayoutTableName.Trim() == "tblpanelTitle1" && c.IsShow).OrderBy(c => c.ColumnInt).ToList();
                    if (listtblpanelTitle1.Count > 0)
                    {
                        this.tblpanelTitle1.ColumnCount = listtblpanelTitle1.Count;
                        foreach (var item in listtblpanelTitle1)
                        {
                            float sizeColumn = 0;
                            float.TryParse(item.SizePercent, out sizeColumn);
                            this.tblpanelTitle1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, sizeColumn));
                        }

                    }
                }
                if (listLabelForTablePanel != null && listLabelForTablePanel.Count > 0)
                {
                    listLabelForTablePanel = listLabelForTablePanel.OrderBy(c => c.IntRowTBLPanelContent).ToList();
                    for (int i = 0; i < listLabelForTablePanel.Count; i++)
                    {
                        var label = listLabelForTablePanel[i];
                        Label labelTitle = new Label();
                        labelTitle.Anchor = System.Windows.Forms.AnchorStyles.None;
                        labelTitle.AutoSize = true;
                        labelTitle.Location = new System.Drawing.Point(16, 17);
                        labelTitle.Name = "label";
                        labelTitle.Size = new System.Drawing.Size(91, 18);
                        labelTitle.TabIndex = 0;
                        DateTime dateNow = DateTime.Now;
                        labelTitle.Text = label.LabelName;
                        labelTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                        var labelConfigTitle1 = listLabelConfig.Where(c => c.TableLayoutPanelName.Trim() == "tblPanelTitle1" && c.Position == 1).FirstOrDefault();
                        if (labelConfigTitle1 != null)
                            DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigTitle1, labelTitle);
                        this.tblpanelTitle1.Controls.Add(labelTitle, i, 0);
                    }
                }
                this.tblpanelTitle1.Margin = new System.Windows.Forms.Padding(0);
                this.tblpanelTitle1.Size = new System.Drawing.Size(739, 405);
                this.tblpanelTitle1.TabIndex = 0;
                this.tblpanelTitle1.ResumeLayout(false);
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
                        this.tblpanelBody.Controls.Add(this.tblpanelContent, 0, 2);

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

        private void FrmLCDError_Load(object sender, EventArgs e)
        {
            try
            {
                LoadLCDConfig();
                //Get list Chuyen
                listChuyen = chuyenDAO.GetListChuyenInfByListId(AccountSuccess.strListChuyenId);
                if (listChuyen != null && listChuyen.Count > 0)
                {
                    line = listChuyen.First();
                    titleLCD += " " + line.TenChuyen;
                }


                //Get config information
                listTableLayoutPanelConfig = tableLayoutPanelConfigDAO.GetTableLayoutPanelConfig(tableType);
                //Get config panel
                listPanelConfig = panelConfigDAO.GetPanelConfig(tableType);
                //Get label config
                listLabelConfig = labelConfigDAO.GetLabelConfig(tableType);
                //Get label for tablepanel
                listLabelForTablePanel = labelConfigDAO.GetLabelForTablePanel(tableType);

                modelLCDError = BLLError.GetListGroupErrors();// groupErrorDAO.GetListGroupErrorDetail();

                BuildPanelHeader();
                BuildPanelTitle1();
                BuildPanelContent();
                BuildPanelBody();
                initTimerDateTime();
                LoadDataErorForChuyen();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }


        private void LoadDataErorForChuyen()
        {
            try
            {
                if (listModelShowLCDError.Count > 0)
                {
                    string dateNow = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                    TimeSpan timeNow = DateTime.Now.TimeOfDay;
                    TimeSpan timeNSSEnd = timeNow.Add(new TimeSpan(0, 30, 0));
                    TimeSpan timeNSStart = timeNow.Add(new TimeSpan(0, -30, 0));
                    for (int i = 0; i < listModelShowLCDError.Count; i++)
                    {
                        int sanLuongGio = 0;
                        int sanLuongGioTang = 0;
                        int sanLuongGioGiam = 0;
                        var model = listModelShowLCDError[i];
                        string sqlSanLuongGio = string.Empty;
                        if (model.ErrorId > 0)
                        {
                            switch (model.LabelType)
                            {
                                case 1:
                                    var timeNSSEndOld = timeNSSEnd.Add(new TimeSpan(-1, 0, 0));
                                    var timeNSStartOld = timeNSStart.Add(new TimeSpan(-1, 0, 0));
                                    sqlSanLuongGio = "select (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + line.MaChuyen + " and ErrorId=" + model.ErrorId + " and Time >= '" + timeNSStartOld + "' and Time <='" + timeNSSEndOld + "' and Date='" + dateNow + "' and CommandTypeId=" + (int)eCommandRecive.ErrorIncrease + " and IsEndOfLine= 1) AS SanLuongTang, (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + line.MaChuyen + " and ErrorId=" + model.ErrorId + " and Time >= '" + timeNSStart + "' and Time <='" + timeNSSEnd + "' and Date='" + dateNow + "' and CommandTypeId=" + (int)eCommandRecive.ErrorReduce + " and IsEndOfLine= 1) AS SanLuongGiam";
                                    break;
                                case 2:
                                    sqlSanLuongGio = "select (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + line.MaChuyen + " and ErrorId=" + model.ErrorId + " and Time >= '" + timeNSStart + "' and Time <='" + timeNSSEnd + "' and Date='" + dateNow + "' and CommandTypeId=" + (int)eCommandRecive.ErrorIncrease + " and IsEndOfLine= 1) AS SanLuongTang, (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + line.MaChuyen + " and ErrorId=" + model.ErrorId + " and Time >= '" + timeNSStart + "' and Time <='" + timeNSSEnd + "' and Date='" + dateNow + "' and CommandTypeId=" + (int)eCommandRecive.ErrorReduce + " and IsEndOfLine= 1) AS SanLuongGiam";
                                    break;
                                case 3:
                                    sqlSanLuongGio = "select (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + line.MaChuyen + " and ErrorId=" + model.ErrorId + " and Date='" + dateNow + "' and CommandTypeId=" + (int)eCommandRecive.ErrorIncrease + " and IsEndOfLine= 1) AS SanLuongTang, (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + line.MaChuyen + " and ErrorId=" + model.ErrorId + " and Date='" + dateNow + "' and CommandTypeId=" + (int)eCommandRecive.ErrorReduce + " and IsEndOfLine= 1) AS SanLuongGiam";
                                    break;
                            }
                        }
                        else
                        {
                            var listError = listModelShowLCDError.Where(c => c.GroupErrorId == model.GroupErrorId && c.ErrorId > 0).Select(c => c.ErrorId).Distinct().ToList();
                            string strListErrorId = string.Empty;
                            if (listError.Count > 0)
                            {
                                for (int n = 0; n < listError.Count; n++)
                                {
                                    strListErrorId += listError[n];
                                    if (n < listError.Count - 1)
                                        strListErrorId += ",";
                                }
                            }
                            if (!string.IsNullOrEmpty(strListErrorId))
                            {
                                switch (model.LabelType)
                                {
                                    case 1:
                                        var timeNSSEndOld = timeNSSEnd.Add(new TimeSpan(-1, 0, 0));
                                        var timeNSStartOld = timeNSStart.Add(new TimeSpan(-1, 0, 0));
                                        sqlSanLuongGio = "select (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + line.MaChuyen + " and ErrorId in (" + strListErrorId + ") and Time >= '" + timeNSStartOld + "' and Time <='" + timeNSSEndOld + "' and Date='" + dateNow + "' and CommandTypeId=" + (int)eCommandRecive.ErrorIncrease + " and IsEndOfLine= 1) AS SanLuongTang, (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + line.MaChuyen + " and ErrorId in (" + strListErrorId + ") and Time >= '" + timeNSStartOld + "' and Time <='" + timeNSSEndOld + "' and Date='" + dateNow + "' and CommandTypeId=" + (int)eCommandRecive.ErrorReduce + " and IsEndOfLine= 1) AS SanLuongGiam";
                                        break;
                                    case 2:
                                        sqlSanLuongGio = "select (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + line.MaChuyen + " and ErrorId in (" + strListErrorId + ") and Time >= '" + timeNSStart + "' and Time <='" + timeNSSEnd + "' and Date='" + dateNow + "' and CommandTypeId=" + (int)eCommandRecive.ErrorIncrease + " and IsEndOfLine= 1) AS SanLuongTang, (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + line.MaChuyen + " and ErrorId in (" + strListErrorId + ") and Time >= '" + timeNSStart + "' and Time <='" + timeNSSEnd + "' and Date='" + dateNow + "' and CommandTypeId=" + (int)eCommandRecive.ErrorReduce + " and IsEndOfLine= 1) AS SanLuongGiam";
                                        break;
                                    case 3:


                                        sqlSanLuongGio = "select (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + line.MaChuyen + " and ErrorId in (" + strListErrorId + ") and Date='" + dateNow + "' and CommandTypeId=" + (int)eCommandRecive.ErrorIncrease + " and IsEndOfLine= 1) AS SanLuongTang, (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + line.MaChuyen + " and ErrorId in (" + strListErrorId + ") and Date='" + dateNow + "' and CommandTypeId=" + (int)eCommandRecive.ErrorReduce + " and IsEndOfLine= 1) AS SanLuongGiam";
                                        break;
                                }
                            }
                        }

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
                        }
                        listModelShowLCDError[i].Label.Text = sanLuongGio.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                LoadDataErorForChuyen();
            }
            catch (Exception ex)
            {
                this.timerShow.Enabled = false;
                MessageBox.Show("Lỗi timer get data: " + ex.Message, "Lỗi xử lý", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
                this.timerDateTime.Enabled = false;
                MessageBox.Show("Lỗi timer getTime: " + ex.Message, "Lỗi xử lý", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void tableLayoutContent_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            if (listIndexRowIsGroupError.Contains(e.Row))
            {
                if (listPanelConfig != null && listPanelConfig.Count > 0)
                {
                    var panelConfig = listPanelConfig.Where(c => c.Name.Trim() == "panelContent2").FirstOrDefault();
                    if (panelConfig != null)
                    {
                        Brush brush = new SolidBrush(DuAn03_HaiDang.Helper.HelperControl.GetColor(panelConfig.BackColor));
                        Graphics g = e.Graphics;
                        Rectangle r = e.CellBounds;
                        g.FillRectangle(brush, r);
                    }

                }

            }
        }

        private void FrmLCDError_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                this.timerDateTime.Enabled = false;
                this.timerShow.Enabled = false;
                DuAn03_HaiDang.Helper.HelperControl.ClearFormActiveLCD(this.Name);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

    }
}
