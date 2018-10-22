using QuanLyNangSuat.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QuanLyNangSuat.Model;
using DuAn03_HaiDang.POJO;
using DuAn03_HaiDang.DAO;
using PMS.Business;

namespace QuanLyNangSuat
{
    public partial class FrmLCDConfig : Form
    {
        private LCDConfigDAO lcdConfigDAO;
     //   private ConfigDAO configDAO; 
        private int tableType;
        public FrmLCDConfig(int _tableType)
        {
            InitializeComponent();
            this.lcdConfigDAO = new LCDConfigDAO();
       //     this.configDAO = new ConfigDAO();
            this.tableType = _tableType;
        }

        private void FrmLCDConfig_Load(object sender, EventArgs e)
        {
            try
            {
                LoadDataTableLayoutPanel();
                LoadDataPanel();
                LoadDataLabelArea();
                LoadDataLabelForPanelContent();
                LoadConfig();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Đã xảy ra lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDataTableLayoutPanel()
        {
            try
            {
                var listModel = lcdConfigDAO.GetTableLayoutPanelConfig(tableType);
                gridControlTableLayoutPanel.DataSource = listModel;
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        private void LoadDataPanel()
        {
            try
            {
                var listModel = lcdConfigDAO.GetPanelConfig(tableType);
                gridControlPanel.DataSource = listModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadDataLabelArea()
        {
            try
            {
                var listModel = lcdConfigDAO.GetLabelAreaConfig(tableType);
                gridControlLabelArea.DataSource = listModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadDataLabelForPanelContent()
        {
            try
            {
                var listModel = lcdConfigDAO.GetLabelForPanelContentConfig(tableType);
                gridControlLabelForPanelContent.DataSource = listModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadConfig()
        {
            try
            {
              //  var listModel = configDAO.GetConfig();
                var listModel = BLLConfig.Instance.GetShowLCDConfig();
                gridControlLCDConfig.DataSource = listModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void butSaveTableLayout_Click(object sender, EventArgs e)
        {
            try
            {
                List<ModelTableLayoutPanel> listModel = new List<ModelTableLayoutPanel>();
                for (int i=0; i < gridViewTableLayoutPanel.RowCount; i++)
                {
                    var model = new ModelTableLayoutPanel();
                    model.Id = int.Parse(gridViewTableLayoutPanel.GetRowCellValue(i, "Id").ToString());
                    model.ColumnInt = int.Parse(gridViewTableLayoutPanel.GetRowCellValue(i, "ColumnInt").ToString());
                    model.RowInt = int.Parse(gridViewTableLayoutPanel.GetRowCellValue(i, "RowInt").ToString());
                    model.IsShow = bool.Parse(gridViewTableLayoutPanel.GetRowCellValue(i, "IsShow").ToString());
                    model.SizePercent = float.Parse(gridViewTableLayoutPanel.GetRowCellValue(i, "SizePercent").ToString());
                    model.TableLayoutPanelName = gridViewTableLayoutPanel.GetRowCellValue(i, "TableLayoutPanelName").ToString();
                    model.TableType = tableType;                    
                    listModel.Add(model);
                }
                lcdConfigDAO.SaveTableLayoutPanelConfig(listModel);
                LoadDataTableLayoutPanel();
            }
            catch (Exception ex)
            {                
                MessageBox.Show("Lỗi: " + ex.Message, "Đã xảy ra lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void butSavePanel_Click(object sender, EventArgs e)
        {
            try
            {
                List<ModelPanel> listModel = new List<ModelPanel>();
                for (int i = 0; i < gridViewPanel.RowCount; i++)
                {
                    var model = new ModelPanel();
                    model.Id = int.Parse(gridViewPanel.GetRowCellValue(i, "Id").ToString());
                    model.Name = gridViewPanel.GetRowCellValue(i, "Name").ToString();
                    model.BackColor = gridViewPanel.GetRowCellValue(i, "BackColor").ToString();                   
                    model.TableType = tableType;
                    listModel.Add(model);
                }
                lcdConfigDAO.SavePanelConfig(listModel);
                LoadDataPanel();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Đã xảy ra lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void butSaveLabelArea_Click(object sender, EventArgs e)
        {
            try
            {
                List<ModelLabelArea> listModel = new List<ModelLabelArea>();
                for (int i = 0; i < gridViewLabelArea.RowCount; i++)
                {
                    var model = new ModelLabelArea();
                    model.Id = int.Parse(gridViewLabelArea.GetRowCellValue(i, "Id").ToString());
                    model.Italic = bool.Parse(gridViewLabelArea.GetRowCellValue(i, "Italic").ToString());
                    model.Bold = bool.Parse(gridViewLabelArea.GetRowCellValue(i, "Bold").ToString());
                    model.Color = gridViewLabelArea.GetRowCellValue(i, "Color").ToString();
                    model.Font = gridViewLabelArea.GetRowCellValue(i, "Font").ToString();
                    model.Position = int.Parse(gridViewLabelArea.GetRowCellValue(i, "Position").ToString());
                    model.Size = float.Parse(gridViewLabelArea.GetRowCellValue(i, "Size").ToString());
                    model.TableLayoutPanelName = gridViewLabelArea.GetRowCellValue(i, "TableLayoutPanelName").ToString();
                    model.TableType = tableType;                   
                    listModel.Add(model);
                }
                lcdConfigDAO.SaveLabelAreaConfig(listModel);
                LoadDataLabelArea();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Đã xảy ra lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void butSaveLabelForPanelContent_Click(object sender, EventArgs e)
        {
            try
            {
                List<ModelLabelForPanelContent> listModel = new List<ModelLabelForPanelContent>();
                for (int i = 0; i < gridViewLabelForPanelContent.RowCount; i++)
                {
                    var model = new ModelLabelForPanelContent();
                    model.Id = int.Parse(gridViewLabelForPanelContent.GetRowCellValue(i, "Id").ToString());
                    model.IntRowTBLPanelContent = int.Parse(gridViewLabelForPanelContent.GetRowCellValue(i, "IntRowTBLPanelContent").ToString());
                    model.IsShow = bool.Parse(gridViewLabelForPanelContent.GetRowCellValue(i, "IsShow").ToString());
                    model.LabelName = gridViewLabelForPanelContent.GetRowCellValue(i, "LabelName").ToString();
                    model.SttNext = int.Parse(gridViewLabelForPanelContent.GetRowCellValue(i, "SttNext").ToString());
                    model.TableType = tableType;
                    listModel.Add(model);
                }
                lcdConfigDAO.SaveLabelForPanelContentConfig(listModel);
                LoadDataLabelForPanelContent();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Đã xảy ra lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void butSaveLCDConfig_Click(object sender, EventArgs e)
        {
            try
            {
                List<Config> listModel = new List<Config>();
                for (int i = 0; i < gridViewLCDConfig.RowCount; i++)
                {
                    var model = new Config();
                    model.Id = int.Parse(gridViewLCDConfig.GetRowCellValue(i, "Id").ToString());
                    model.Name = gridViewLCDConfig.GetRowCellValue(i, "Name").ToString();
                    model.Value = gridViewLCDConfig.GetRowCellValue(i, "Value").ToString();
                    listModel.Add(model);
                }
                lcdConfigDAO.SaveLCDConfig(listModel);
                LoadConfig();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Đã xảy ra lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
