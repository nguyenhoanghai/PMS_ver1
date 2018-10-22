using DuAn03_HaiDang.DAO;
using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.Enum;
using DuAn03_HaiDang.KeyPad_Chuyen.dao;
using DuAn03_HaiDang.KeyPad_Chuyen.pojo;
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

namespace QuanLyNangSuat
{
    public partial class FrmLCDNangSuatCum : Form
    {
        public FrmLCDNangSuatCum(SqlConnection _sqlConnection)
        {
            InitializeComponent();
            this.timerShow = new System.Windows.Forms.Timer();
            this.timerDateTime = new System.Windows.Forms.Timer();
            this.sqlConnection = _sqlConnection;
        }

        TableLayoutPanelConfigDAO tableLayoutPanelConfigDAO = new TableLayoutPanelConfigDAO();
        PanelConfigDAO panelConfigDAO = new PanelConfigDAO();
        LabelConfigDAO labelConfigDAO = new LabelConfigDAO();
      //  ConfigDAO configDAO = new ConfigDAO();          
        ChuyenDAO chuyenDAO = new ChuyenDAO();      
        CumDAO cumDAO = new CumDAO();
      //  ShiftDAO shiftDAO = new ShiftDAO();
        List<TableLayoutPanelConfig> listTableLayoutPanelConfig;
        List<PanelConfig> listPanelConfig;
        List<LabelConfig> listLabelConfig;
        List<LabelForTablePanel> listLabelForTablePanel;
        List<Chuyen> listChuyen;
        List<ModelLCDNangSuatCum> listModelLCDNangSuatCum;
        List<Cluster> listClusterOfLine;
        string titleLCD = string.Empty;        
        int soRowTrenMH = 1;
        int tableType = 5;
        string logo = string.Empty;
        Label labelTime;
        Label labelDate;
        SqlConnection sqlConnection;
        SqlDataAdapter sqlDataAdapter;
        int maxWorkTimeOfLines = 0;
        TimeSpan timeStartWork = new TimeSpan(0, 0, 0);
        Chuyen line;
        List<Shift> listShiftPublic = new List<Shift>();
        private System.Windows.Forms.Timer timerShow;
        private System.Windows.Forms.Timer timerDateTime;
       // private List<ModelWorkHours> listModelWorkHours;
        private List<WorkingTimeModel> listModelWorkHours;
        //Layout
        #region Layout
        private System.Windows.Forms.TableLayoutPanel tblpanelBody;
        private System.Windows.Forms.TableLayoutPanel tblpanelHeader;        
        private System.Windows.Forms.TableLayoutPanel tblpanelContent;
        private System.Windows.Forms.TableLayoutPanel tblpanelTitle1;
        #endregion Layout
        
        private void LoadLCDConfig()
        {
            try
            {
              //  List<Config> listConfig = configDAO.GetConfig();
                var listConfig = BLLConfig.Instance.GetShowLCDConfig();
                if (listConfig != null && listConfig.Count > 0)
                {
                    foreach (var config in listConfig)
                    {
                        switch (config.Name.Trim())
                        {
                            case "ThoiGianLayDuLieuLCDNSCum":
                                int interval = 1000;
                                int.TryParse(config.Value, out interval);
                                initTimerShow(interval);
                                break;
                            case "SoRowTrenLCDNSCum":
                                int.TryParse(config.Value, out soRowTrenMH);
                                break;
                            case "Logo":
                                logo = config.Value;
                                break;
                            case "TieuDeLCDNangSuatCum":
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
                        labelDate.Text = "Ngày " + dateNow.Day.ToString() + "/" + dateNow.Month.ToString() + "/" + dateNow.Year.ToString();
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
                float sizeColumn1 = 15;
                float sizeColumn2 = 85;
                if (listTableLayoutPanelConfig != null && listTableLayoutPanelConfig.Count > 0)
                {
                    var listtblpanelContent = listTableLayoutPanelConfig.Where(c => c.TableLayoutTableName.Trim() == "tblpanelContent" && c.IsShow).OrderBy(c => c.RowInt).ToList();
                    if (listtblpanelContent.Count > 0)
                    {
                        var column1Config = listtblpanelContent.Where(c => c.ColumnInt.Trim() == "1").FirstOrDefault();
                        if (column1Config != null)
                            float.TryParse(column1Config.SizePercent, out sizeColumn1);
                        var column2Config = listtblpanelContent.Where(c => c.ColumnInt.Trim() == "2").FirstOrDefault();
                        if (column2Config != null)
                            float.TryParse(column2Config.SizePercent, out sizeColumn2);
                    }
                }
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
                this.tblpanelContent.ColumnCount = this.tblpanelTitle1.ColumnCount;
                float sizeColumn = sizeColumn2 / maxWorkTimeOfLines;
                this.tblpanelContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, sizeColumn1));
                for (int i = 0; i < maxWorkTimeOfLines; i++)
                {
                    this.tblpanelContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, sizeColumn));
                }
                this.tblpanelContent.Margin = new System.Windows.Forms.Padding(0);
                this.tblpanelContent.Size = new System.Drawing.Size(739, 405);
                this.tblpanelContent.TabIndex = 0;
                if (listClusterOfLine != null && listClusterOfLine.Count > 0)
                {
                    listModelLCDNangSuatCum = new List<ModelLCDNangSuatCum>();

                    this.tblpanelContent.RowCount = listClusterOfLine.Count;
                    float sireRow = 100 / listClusterOfLine.Count;
                    for (int j = 0; j < listClusterOfLine.Count; j++)
                    {
                        this.tblpanelContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, sireRow));

                        var item = listClusterOfLine[j];
                        Label label = new Label();
                        label.Anchor = System.Windows.Forms.AnchorStyles.None;
                        label.AutoSize = true;
                        label.Location = new System.Drawing.Point(16, 17);
                        label.Name = "label" + j;
                        label.Size = new System.Drawing.Size(91, 18);
                        label.TabIndex = 0;
                        label.Text = item.Name;
                        label.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                        this.tblpanelContent.Controls.Add(label, 0, j);
                        ModelLCDNangSuatCum modelTitleCluster = new ModelLCDNangSuatCum();
                        modelTitleCluster.ClusterId = item.Id;
                        modelTitleCluster.ClusterName = item.Name;
                        modelTitleCluster.STTHour = 0;
                        modelTitleCluster.Label = label;
                        listModelLCDNangSuatCum.Add(modelTitleCluster);
                        var labelConfigContentTitle = listLabelConfig.Where(c => c.TableLayoutPanelName.Trim() == "tblPanelContent" && c.Position == 1).FirstOrDefault();
                        if (labelConfigContentTitle != null)
                        {
                            DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigContentTitle, label);
                        }
                        var labelConfigContent = listLabelConfig.Where(c => c.TableLayoutPanelName.Trim() == "tblPanelContent" && c.Position == 2).FirstOrDefault();
                        
                        for (int k = 1; k <= maxWorkTimeOfLines; k++)
                        {
                            Label labelNS = new Label();
                            labelNS.Anchor = System.Windows.Forms.AnchorStyles.None;
                            labelNS.AutoSize = true;
                            labelNS.Location = new System.Drawing.Point(16, 17);
                            labelNS.Name = "label" + j + "_" + k;
                            labelNS.Size = new System.Drawing.Size(91, 18);
                            labelNS.TabIndex = 0;
                            labelNS.Text = "0";
                            labelNS.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                            this.tblpanelContent.Controls.Add(labelNS, k, j);
                            if (labelConfigContent != null)
                            {
                                DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigContent, labelNS);
                            }

                            ModelLCDNangSuatCum model = new ModelLCDNangSuatCum();
                            model.ClusterId = item.Id;
                            model.ClusterName = item.Name;
                            model.STTHour = k;
                            model.Label = labelNS;
                            listModelLCDNangSuatCum.Add(model);
                        }
                    }
                }                
                this.tblpanelContent.ResumeLayout(false);
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
                float sizeColumn1 = 15;
                float sizeColumn2 = 85;
                if (listTableLayoutPanelConfig != null && listTableLayoutPanelConfig.Count > 0)
                {
                    var listtblpanelContent = listTableLayoutPanelConfig.Where(c => c.TableLayoutTableName.Trim() == "tblpanelTitle1" && c.IsShow).OrderBy(c => c.RowInt).ToList();
                    if (listtblpanelContent.Count > 0)
                    {
                        var column1Config = listtblpanelContent.Where(c => c.ColumnInt.Trim() == "1").FirstOrDefault();
                        if (column1Config != null)
                            float.TryParse(column1Config.SizePercent, out sizeColumn1);
                        var column2Config = listtblpanelContent.Where(c => c.ColumnInt.Trim() == "2").FirstOrDefault();
                        if (column2Config != null)
                            float.TryParse(column2Config.SizePercent, out sizeColumn2);
                    }
                }
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

                int maxWorkTime = GetWorkTimeOfLine();
                maxWorkTimeOfLines = maxWorkTime;
                if (maxWorkTime > 0)
                {
                    this.tblpanelTitle1.ColumnCount = maxWorkTime + 1;
                    float size = sizeColumn2 / maxWorkTime;

                    this.tblpanelTitle1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, sizeColumn1));

                    Label label = new Label();
                    label.Anchor = System.Windows.Forms.AnchorStyles.None;
                    label.AutoSize = true;
                    label.Location = new System.Drawing.Point(16, 17);
                    label.Name = "label";
                    label.Size = new System.Drawing.Size(91, 18);
                    label.TabIndex = 0;
                    DateTime dateNow = DateTime.Now;
                    label.Text = "CỤM / GIỜ";
                    label.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                    var labelConfigTitle1 = listLabelConfig.Where(c => c.TableLayoutPanelName.Trim() == "tblPanelTitle1" && c.Position == 1).FirstOrDefault();
                    if (labelConfigTitle1 != null)
                        DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigTitle1, label);

                    this.tblpanelTitle1.Controls.Add(label, 0, 0);

                    for (int i = 1; i <= maxWorkTime; i++)
                    {
                        this.tblpanelTitle1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, size));

                        Label labelTitleHourNS = new Label();
                        labelTitleHourNS.Anchor = System.Windows.Forms.AnchorStyles.None;
                        labelTitleHourNS.AutoSize = true;
                        labelTitleHourNS.Location = new System.Drawing.Point(16, 17);
                        labelTitleHourNS.Name = "labelTitleHourNS" + i.ToString();
                        labelTitleHourNS.Size = new System.Drawing.Size(91, 18);
                        labelTitleHourNS.TabIndex = 0;
                        labelTitleHourNS.Text = "GIỜ " + (i).ToString();
                        labelTitleHourNS.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                        if (labelConfigTitle1 != null)
                            DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigTitle1, labelTitleHourNS);

                        this.tblpanelTitle1.Controls.Add(labelTitleHourNS, i, 0);
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
                listShiftPublic = TTCaCuaChuyen(maChuyen);
                if (listShiftPublic != null && listShiftPublic.Count > 0)
                {
                    foreach (var shift in listShiftPublic)
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

        //Load Data for mutiChuyen          
        DataTable dtSanLuongGio = new DataTable();
        private void LoadDataLCDNSCum()
        {
            try
            {
                DateTime dateNow = DateTime.Now;
                if (listModelLCDNangSuatCum != null && listModelLCDNangSuatCum.Count > 0)
                {
                    for (int i = 0; i < listModelLCDNangSuatCum.Count; i++ )
                    {
                        var model = listModelLCDNangSuatCum[i];
                        int sanLuongGio = 0;
                        int sanLuongGioTang = 0;
                        int sanLuongGioGiam = 0;
                        string sqlSanLuongGio = string.Empty;
                        if(model.STTHour>0)
                        {
                            if (listModelWorkHours != null && listModelWorkHours.Count > 0)
                            {
                                foreach (var item in listModelWorkHours)
                                {
                                    if (item.IntHours == model.STTHour)
                                    {
                                        sqlSanLuongGio = "select (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + line.MaChuyen + " and CumId=" + model.ClusterId + " and Time >= '" + item.TimeStart + "' and Time <='" + item.TimeEnd + "' and Date='" + dateNow + "' and CommandTypeId=" + (int)eCommandRecive.ProductIncrease + " and ProductOutputTypeId=" + (int)eProductOutputType.KCS + ") AS SanLuongTang, (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + line.MaChuyen + " and CumId=" + model.ClusterId + " and Time >= '" + item.TimeStart + "' and Time <='" + item.TimeEnd + "' and Date='" + dateNow + "' and CommandTypeId=" + (int)eCommandRecive.ProductReduce + " and ProductOutputTypeId=" + (int)eProductOutputType.KCS + ") AS SanLuongGiam";
                                        break;
                                    }
                                    
                                }
                            }                            
                        }
                        else
                        {
                            sqlSanLuongGio = "select (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + line.MaChuyen + " and CumId=" + model.ClusterId + " and Date='" + dateNow + "' and CommandTypeId=" + (int)eCommandRecive.ProductIncrease + " and ProductOutputTypeId=" + (int)eProductOutputType.KCS + ") AS SanLuongTang, (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + line.MaChuyen + " and CumId=" + model.ClusterId + " and Date='" + dateNow + "' and CommandTypeId=" + (int)eCommandRecive.ProductReduce + " and ProductOutputTypeId=" + (int)eProductOutputType.KCS + ") AS SanLuongGiam";
                        }
                        if (!string.IsNullOrEmpty(sqlSanLuongGio))
                        {
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
                            if (sanLuongGio < 0)
                                sanLuongGio = 0;
                            if (model.STTHour > 0)
                                model.Label.Text = sanLuongGio.ToString();
                            else
                                model.Label.Text = model.ClusterName + " (" + sanLuongGio.ToString() + ")";
                        }
                    }
                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }


        private void FrmLCDNangSuatCum_Load(object sender, EventArgs e)
        {
            try
            {
                LoadLCDConfig();
                //Get list Chuyen
                listChuyen = chuyenDAO.GetListChuyenInfByListId(AccountSuccess.strListChuyenId);
                if (listChuyen != null && listChuyen.Count > 0)
                {
                    line = listChuyen.First();
                    listClusterOfLine = cumDAO.GetListClusterOfLine(line.MaChuyen);
                    titleLCD += " " + line.TenChuyen;
                    listModelWorkHours = BLLShift.GetListWorkHoursOfLineByLineId(int.Parse(line.MaChuyen));// shiftDAO.GetListWorkHoursOfLineByLineId(line.MaChuyen);
                }                
                //Get config information
                listTableLayoutPanelConfig = tableLayoutPanelConfigDAO.GetTableLayoutPanelConfig(tableType);
                //Get config panel
                listPanelConfig = panelConfigDAO.GetPanelConfig(tableType);
                //Get label config
                listLabelConfig = labelConfigDAO.GetLabelConfig(tableType);
                //Get label for tablepanel
                listLabelForTablePanel = labelConfigDAO.GetLabelForTablePanel(tableType);

                BuildPanelHeader();
                BuildPanelTitle1();        
                BuildPanelContent();                
                BuildPanelBody();
                initTimerDateTime();
                LoadDataLCDNSCum();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
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
                LoadDataLCDNSCum();
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

        private void FrmLCDNangSuatCum_FormClosed(object sender, FormClosedEventArgs e)
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
