using DuAn03_HaiDang.DAO;
using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.KeyPad_Chuyen.dao;
using DuAn03_HaiDang.KeyPad_Chuyen.pojo;
using DuAn03_HaiDang.Model;
using DuAn03_HaiDang.POJO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DuAn03_HaiDang.HelperControl;
using PMS.Business;
using PMS.Business.Models;

namespace DuAn03_HaiDang
{
    public partial class FrmHienThiLCDKANBAN : FormBase
    {
        //Init object
        #region Init object
        TableLayoutPanelConfigDAO tableLayoutPanelConfigDAO = new TableLayoutPanelConfigDAO();
        PanelConfigDAO panelConfigDAO = new PanelConfigDAO();
        LabelConfigDAO labelConfigDAO = new LabelConfigDAO();
        //  ConfigDAO configDAO = new ConfigDAO();
        NangXuatDAO nangSuatDAO;
        BTPDAO btpDAO = new BTPDAO();
        ChuyenDAO chuyenDAO = new ChuyenDAO();
        List<TableLayoutPanelConfig> listTableLayoutPanelConfig;
        List<PanelConfig> listPanelConfig;
        List<LabelConfig> listLabelConfig;
        List<LabelForTablePanel> listLabelForTablePanel;
        // List<Chuyen> listChuyen;
        List<AssignmentForLineModel> listChuyen;
        List<ChuyenLabelKB> listLabelForContent;
        int tableType = 2;
        DataTable dtDataChuyen;
        private System.Windows.Forms.Timer timerShow;
        int soRowTrenMH = 1;
        int indexChuyenShow = 0;
        #endregion Init object


        //Layout
        #region Layout
        private System.Windows.Forms.TableLayoutPanel tblpanelBody;
        private System.Windows.Forms.TableLayoutPanel tblpanelHeader;
        private System.Windows.Forms.TableLayoutPanel tblpanelContent;
        #endregion Layout

        public FrmHienThiLCDKANBAN()
        {
            InitializeComponent();
            //Get list Chuyen
            listChuyen = BLLAssignmentForLine.Instance.GetAssignmentForLines(AccountSuccess.strListChuyenId.Split(',').Select(x => Convert.ToInt32(x)).ToList()).OrderBy(x => x.MaChuyen).ToList();// chuyenDAO.GetListDSChuyen(AccountSuccess.strListChuyenId).OrderBy(x => x.MaChuyen).ToList();
            this.timerShow = new System.Windows.Forms.Timer(this.components);
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
            //tblpanelHeader and tblpanelContent
            BuildPanelHeaderAndContent();
            //tblpanelBody
            BuildPanelBody();
            //Load Data for Chuyens
            LoadDataForChuyens();
        }

        private void FrmHienThiLCDKANBAN_Load(object sender, EventArgs e)
        {

        }

        private void LoadLCDConfig()
        {
            try
            {
                var configs = BLLConfig.Instance.GetShowLCDConfig();
                if (configs != null && configs.Count > 0)
                {
                    foreach (var config in configs)
                    {
                        if (config.Name.Trim().Equals("ThoiGianLatTrangLCDKanBan"))
                        {
                            int interval = 1000;
                            int.TryParse(config.Value, out interval);
                            initTimerShow(interval);
                        }
                        else if (config.Name.Trim().Equals("SoRowTrenLCDKanBan"))
                            int.TryParse(config.Value, out soRowTrenMH);
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
            if (listChuyen.Count > soRowTrenMH)
                this.timerShow.Enabled = true;
            else
                this.timerShow.Enabled = false;
        }

        private void timerShow_Tick(object sender, EventArgs e)
        {
            try
            {
                this.tblpanelContent.Controls.Clear();
                int indexStart = indexChuyenShow + 1;
                int indexMax = indexChuyenShow + soRowTrenMH;
                int indexRow = -1;
                for (int j = indexStart; j <= indexMax; j++)
                {
                    if (j < listChuyen.Count)
                    {
                        indexRow++;
                        var chuyen = listChuyen[j];
                        var listLabelForContentOfChuyen = listLabelForContent.Where(c => c.chuyenId == chuyen.MaChuyen).ToList();
                        if (listLabelForContentOfChuyen.Count > 0)
                        {
                            var lbs = new List<ChuyenLabelKB>();
                            foreach (var item in listLabelForContentOfChuyen)
                            {
                                if (lbs.FirstOrDefault(x => x.systemName.Trim().ToUpper().Equals(item.systemName.Trim().ToUpper())) == null)
                                    lbs.Add(item);
                            }

                            for (int i = 0; i < listLabelForTablePanel.Count; i++)
                            {
                                if (!listLabelForTablePanel[i].SystemValueName.Trim().Equals("tinhTrangBTP"))
                                    this.tblpanelContent.Controls.Add(lbs[i].label, i, indexRow);
                                else
                                    this.tblpanelContent.Controls.Add(lbs[i].panel, i, indexRow);
                            }
                            this.tblpanelContent.ResumeLayout(false);
                            this.tblpanelContent.PerformLayout();
                        }
                        if (j == listChuyen.Count - 1)
                        {
                            indexChuyenShow = -1;
                            break;
                        }
                        else
                            indexChuyenShow = j;
                    }
                }
            }
            catch (Exception ex)
            {
                //   MessageBox.Show("Lỗi timer getTime: " + ex.Message, "Lỗi xử lý", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // this.timerShow.Enabled = false;
            }

        }

        //tblpanelHeader and tblpanelContent
        private void BuildPanelHeaderAndContent()
        {
            try
            {
                if (listTableLayoutPanelConfig != null && listTableLayoutPanelConfig.Count > 0)
                {
                    var listtblpanelHeader = listTableLayoutPanelConfig.Where(c => c.TableLayoutTableName.Trim() == "tblpanelHeader" && c.IsShow).OrderBy(c => c.RowInt).ToList();
                    if (listtblpanelHeader.Count > 0)
                    {
                        var panelHeaderConfig = listPanelConfig.Where(c => c.Name == "panelHeader").FirstOrDefault();
                        var panelContentConfig = listPanelConfig.Where(c => c.Name == "panelContent").FirstOrDefault();
                        List<Label> listLabelForHeader = new List<Label>();
                        listLabelForContent = new List<ChuyenLabelKB>();

                        if (listLabelForTablePanel != null && listLabelForTablePanel.Count > 0)
                        {
                            listLabelForTablePanel = listLabelForTablePanel.OrderBy(c => c.IntRowTBLPanelContent).ToList();
                            //Create to label for tablePanelHeader
                            var labelConfigHeader = listLabelConfig.Where(c => c.TableLayoutPanelName.Trim() == "tblPanelHeader").FirstOrDefault();
                            if (labelConfigHeader != null)
                            {
                                for (int i = 0; i < listLabelForTablePanel.Count; i++)
                                {
                                    Label label = new Label();
                                    label.Anchor = System.Windows.Forms.AnchorStyles.None;
                                    label.AutoSize = true;
                                    DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigHeader, label);
                                    label.Location = new System.Drawing.Point(16, 17);
                                    label.Name = "labelH" + i.ToString();
                                    label.Size = new System.Drawing.Size(91, 18);
                                    label.TabIndex = 0;
                                    label.Text = listLabelForTablePanel[i].LabelName;
                                    label.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                                    //Add label to list label of tablepanel Header
                                    listLabelForHeader.Add(label);
                                }
                            }
                            //Create to label for tablePanelContent
                            var labelConfigContent = listLabelConfig.Where(c => c.TableLayoutPanelName.Trim() == "tblPanelContent").FirstOrDefault();
                            if (labelConfigContent != null)
                            {
                                if (listChuyen.Count > 0)
                                {
                                    for (int i = 0; i < listLabelForTablePanel.Count; i++)
                                    {
                                        for (int j = 0; j < listChuyen.Count; j++)
                                        {
                                            //  int chuyenId = 0;
                                            //  int.TryParse(listChuyen[j].MaChuyen, out chuyenId);
                                            if (!listLabelForTablePanel[i].SystemValueName.Trim().Equals("tinhTrangBTP"))
                                            {
                                                Label label = new Label();
                                                label.Anchor = System.Windows.Forms.AnchorStyles.None;
                                                label.AutoSize = true;
                                                DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigContent, label);
                                                label.Location = new System.Drawing.Point(16, 17);
                                                label.Name = "labelC" + i.ToString() + j.ToString();
                                                label.Size = new System.Drawing.Size(91, 18);
                                                label.TabIndex = 0;
                                                if (i == 0)
                                                    label.Text = listChuyen[j].LineName.ToUpper();
                                                else
                                                    label.Text = string.Empty;
                                                label.TextAlign = System.Drawing.ContentAlignment.TopCenter;

                                                listLabelForContent.Add(new ChuyenLabelKB()
                                                {
                                                    chuyenId = listChuyen[j].MaChuyen,
                                                    label = label,
                                                    systemName = listLabelForTablePanel[i].SystemValueName
                                                });
                                            }
                                            else
                                            {
                                                Panel panel = new Panel();
                                                panel.BackColor = DuAn03_HaiDang.Helper.HelperControl.GetColor("Black");
                                                panel.Dock = System.Windows.Forms.DockStyle.Fill;
                                                panel.Location = new System.Drawing.Point(832, 2);
                                                panel.Margin = new System.Windows.Forms.Padding(0);
                                                panel.Name = "panel" + i.ToString() + j.ToString();
                                                panel.Size = new System.Drawing.Size(168, 48);
                                                panel.TabIndex = 1;

                                                listLabelForContent.Add(new ChuyenLabelKB()
                                                {
                                                    chuyenId = listChuyen[j].MaChuyen,
                                                    panel = panel,
                                                    systemName = listLabelForTablePanel[i].SystemValueName
                                                });
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        //tblpanelHeader
                        #region tblpanelHeader
                        this.tblpanelHeader = new TableLayoutPanel();
                        this.tblpanelHeader.SuspendLayout();
                        this.tblpanelHeader.BackColor = DuAn03_HaiDang.Helper.HelperControl.GetColor(panelHeaderConfig.BackColor);
                        this.tblpanelHeader.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
                        this.tblpanelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
                        this.tblpanelHeader.ColumnCount = listtblpanelHeader.Count;
                        foreach (var item in listtblpanelHeader)
                        {
                            float size = 0;
                            float.TryParse(item.SizePercent, out size);
                            this.tblpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, size));
                        }
                        if (listLabelForHeader.Count > 0)
                        {
                            for (int i = 0; i < listLabelForHeader.Count; i++)
                            {
                                this.tblpanelHeader.Controls.Add(listLabelForHeader[i], i, 0);
                            }
                        }
                        this.tblpanelHeader.Location = new System.Drawing.Point(0, 0);
                        this.tblpanelHeader.Name = "tblpanelHeader";
                        this.tblpanelHeader.RowCount = 1;
                        this.tblpanelHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
                        this.tblpanelHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 98F));
                        this.tblpanelHeader.Margin = new System.Windows.Forms.Padding(0);
                        this.tblpanelHeader.Size = new System.Drawing.Size(739, 100);
                        this.tblpanelHeader.TabIndex = 0;

                        this.tblpanelHeader.ResumeLayout(false);
                        this.tblpanelHeader.PerformLayout();
                        #endregion tblpanelHeader

                        //tblpanelContent
                        #region tblpanelContent
                        this.tblpanelContent = new TableLayoutPanel();
                        this.tblpanelContent.SuspendLayout();
                        this.tblpanelContent.BackColor = DuAn03_HaiDang.Helper.HelperControl.GetColor(panelContentConfig.BackColor);
                        this.tblpanelContent.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
                        this.tblpanelContent.ColumnCount = listtblpanelHeader.Count;
                        this.tblpanelContent.Dock = System.Windows.Forms.DockStyle.Fill;
                        foreach (var item in listtblpanelHeader)
                        {
                            float size = 0;
                            float.TryParse(item.SizePercent, out size);
                            this.tblpanelContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, size));
                        }
                        this.tblpanelContent.Margin = new System.Windows.Forms.Padding(0);
                        this.tblpanelContent.Location = new System.Drawing.Point(0, 0);
                        this.tblpanelContent.Name = "tblpanelContent";
                        if (listChuyen.Count > 0)
                        {
                            //this.tblpanelContent.RowCount = listChuyen.Count;
                            //float sizeRow = 100 / listChuyen.Count;
                            //foreach (var item in listChuyen)
                            //{
                            //    this.tblpanelContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, sizeRow));
                            //}
                            this.tblpanelContent.RowCount = soRowTrenMH;
                            float sizeRow = 100 / soRowTrenMH;
                            for (int irow = 0; irow < soRowTrenMH; irow++)
                            {
                                this.tblpanelContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, sizeRow));
                            }
                        }
                        if (listLabelForContent.Count > 0)
                        {
                            int index = -1;
                            //for (int i = 0; i < listLabelForTablePanel.Count; i++)
                            //{
                            //    for (int j = 0; j < listChuyen.Count; j++)
                            //    {
                            //        index++;
                            //        if (!listLabelForTablePanel[i].SystemValueName.Trim().Equals("tinhTrangBTP"))
                            //            this.tblpanelContent.Controls.Add(listLabelForContent[index].label, i, j);
                            //        else
                            //            this.tblpanelContent.Controls.Add(listLabelForContent[index].panel, i, j);
                            //    }

                            //}                           

                            for (int j = 0; j < soRowTrenMH; j++)
                            {
                                if (j < listChuyen.Count)
                                {
                                    var chuyen = listChuyen[j];
                                    // int chuyenId = 0;
                                    // int.TryParse(chuyen.MaChuyen, out chuyenId);
                                    var listLabelForContentOfChuyen = listLabelForContent.Where(c => c.chuyenId == chuyen.MaChuyen).ToList();
                                    if (listLabelForContentOfChuyen.Count > 0)
                                    {
                                        for (int i = 0; i < listLabelForTablePanel.Count; i++)
                                        {
                                            if (!listLabelForTablePanel[i].SystemValueName.Trim().Equals("tinhTrangBTP"))
                                                this.tblpanelContent.Controls.Add(listLabelForContentOfChuyen[i].label, i, j);
                                            else
                                                this.tblpanelContent.Controls.Add(listLabelForContentOfChuyen[i].panel, i, j);
                                        }
                                    }
                                    indexChuyenShow = j;
                                }
                                else
                                    break;
                            }
                        }
                        this.tblpanelContent.Size = new System.Drawing.Size(739, 100);
                        this.tblpanelContent.TabIndex = 0;

                        this.tblpanelContent.ResumeLayout(false);
                        this.tblpanelContent.PerformLayout();
                        #endregion tblpanelContent
                    }
                }
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
                        this.tblpanelBody.Controls.Add(this.tblpanelHeader, 0, 0);
                        this.tblpanelBody.Controls.Add(this.tblpanelContent, 0, 1);
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

        //Set information for label Chuyen
        private void SetInfoForChuyen(ModelInfoChuyenOfKANBAN modelInfoChuyenKB)
        {
            try
            {
                List<ChuyenLabelKB> listChuyenLabel = null;
                if (listLabelForContent != null && listLabelForContent.Count > 0 && modelInfoChuyenKB != null)
                {
                    listChuyenLabel = listLabelForContent.Where(c => c.chuyenId == modelInfoChuyenKB.chuyenId).ToList();
                }
                if (listChuyenLabel != null && listChuyenLabel.Count > 0)
                {
                    foreach (var item in listChuyenLabel)
                    {
                        if (item.systemName.Trim().Equals("maHang"))
                            item.label.Text = modelInfoChuyenKB.maHang;
                        else if (item.systemName.Trim().Equals("btpGiaoChuyenNgay"))
                            item.label.Text = modelInfoChuyenKB.btpGiaoChuyenNgay;
                        else if (item.systemName.Trim().Equals("luyKeBTP"))
                            item.label.Text = modelInfoChuyenKB.luyKeBTP;
                        else if (item.systemName.Trim().Equals("btpBinhQuanBTPTrenNgay"))
                            item.label.Text = modelInfoChuyenKB.btpBinhQuanBTPTrenNgay;
                        else if (item.systemName.Trim().Equals("tinhTrangBTP"))
                            item.panel.BackColor = DuAn03_HaiDang.Helper.HelperControl.GetColor(modelInfoChuyenKB.tinhTrangBTP);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        //Load Data Of Chuyen
        private void LoadDataOfChuyen(string chuyenId, ChuyenSanPham ChuyenSanPham)
        {
            try
            {
                var data = btpDAO.GetDataChuyen(chuyenId, tableType, ChuyenSanPham);
                SetInfoForChuyen(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Load Data for mutiChuyen
        private void LoadDataForChuyens()
        {
            try
            {
                if (listChuyen != null && listChuyen.Count > 0)
                    foreach (var chuyen in listChuyen)
                    {
                        LoadDataOfChuyen(chuyen.MaChuyen.ToString(), null);
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void LoadDataInChange()
        {
            try
            {
                var listChuyenSanPhamChange = nangSuatDAO.CheckIsBTP();
                if (listChuyenSanPhamChange != null && listChuyenSanPhamChange.Count > 0)
                {
                    foreach (var item in listChuyenSanPhamChange)
                    {
                        LoadDataOfChuyen(item.MaChuyen, item);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void timerCheckBTPChange_Tick(object sender, EventArgs e)
        {
            try
            {
                LoadDataInChange();
            }
            catch (Exception ex)
            {
                timerCheckBTPChange.Enabled = false;
                MessageBox.Show("Lỗi: " + ex.Message);
            }

        }
    }
}
