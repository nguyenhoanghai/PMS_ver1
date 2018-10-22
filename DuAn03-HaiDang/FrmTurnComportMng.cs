using QuanLyNangSuat.DAO;
using QuanLyNangSuat.POJO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QuanLyNangSuat
{
    public partial class FrmTurnComportMng : Form
    {
        private TurnCOMMngDAO turnCOMMngDAO; 
        private int configId=0;
        public FrmTurnComportMng()
        {
            InitializeComponent();
            this.turnCOMMngDAO = new TurnCOMMngDAO();
        }

        private void FrmTurnComportMng_Load(object sender, EventArgs e)
        {
            try 
	        {                       
                LoadDataToGridView();
                EnableControl(true, false, false, false, false);                
                EnableInput(false);               
                ClearInput();
	        }
	        catch (Exception ex)
	        {
		        MessageBox.Show("Lỗi: "+ex.Message);
	        }
        }

        #region User Event

        private void EnableControl(bool isButAdd, bool isButUpdate, bool isButSave, bool isButDelete, bool isButCancel)
        {
            try
            {
                butAdd.Enabled = isButAdd;
                butUpdate.Enabled = isButUpdate;
                butSave.Enabled = isButSave;
                butDelete.Enabled = isButDelete;
                butHuy.Enabled = isButCancel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ClearInput()
        {
            try
            {
                cbbComport.SelectedIndex = 0;
                cbbStatus.SelectedIndex = 0;
                tTimeAction.EditValue = DateTime.Now;
                txtDescription.Text = string.Empty;
                chkIsUse.Checked = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void EnableInput(bool enable)
        {
            try
            {
                cbbComport.Enabled = enable;
                cbbStatus.Enabled = enable;
                txtDescription.Enabled = enable;                
                tTimeAction.Enabled = enable;
                chkIsUse.Enabled = enable;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void butSave_Click(object sender, EventArgs e)
        {
            try
            {
                TurnCOMMng config = BuildModel();
                if (config != null)
                {
                    int result = 0;
                    if (configId == 0)
                    {
                        result = turnCOMMngDAO.AddObj(config);
                    }
                    else
                    {
                        result = turnCOMMngDAO.UpdateObj(config);
                    }
                    if (result == 1)
                    {
                        MessageBox.Show("Lưu thông tin thành công.");
                        configId = 0;
                        EnableControl(true, false, false, false, false);
                        EnableInput(false);
                        LoadDataToGridView();
                        ClearInput();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void butAdd_Click(object sender, EventArgs e)
        {
            try
            {
                EnableInput(true);
                ClearInput();
                EnableControl(false, false, true, false, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void butUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                EnableInput(true);
                EnableControl(false, false, true, false, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void butDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (configId != 0)
                {
                    if (MessageBox.Show("Bạn có muốn xoá không?", "Xoá đối tượng", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var result = turnCOMMngDAO.DeleteObj(configId);
                        if (result != 0)
                        {
                            MessageBox.Show("Xoá thành công.");
                            LoadDataToGridView();
                            EnableControl(true, false, false, false, false);
                            EnableInput(false);
                            ClearInput();
                            LoadDataToGridView();
                            configId = 0;
                        }
                        else
                            MessageBox.Show("Xoá thất bại.");
                    }
                }
                else
                    MessageBox.Show("Bạn chưa chọn đối tượng để xoá");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void butHuy_Click(object sender, EventArgs e)
        {
            try
            {
                if (configId == 0)
                    EnableControl(true, false, false, false, false);
                else
                {
                    if (butSave.Enabled)
                        EnableControl(false, true, false, true, true);
                    else
                        EnableControl(true, false, false, false, false);
                }
                EnableInput(false);
                ClearInput();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        #endregion
        
        #region Private Function
        private void LoadDataToGridView()
        {
            try
            {                
                var lisConfig = turnCOMMngDAO.GetListTurnCOMConfig();
                if (lisConfig != null && lisConfig.Count > 0)
                {
                    foreach (var c in lisConfig)
                    {
                        if (c.Status == 0)
                            c.StatusName = "Tắt";
                        else
                            c.StatusName = "Mở";
                        if (c.COMTypeId == 0)
                            c.COMTypeName = "Cổng COM gửi dữ liệu xuống bảng điện tử";
                        else
                            c.COMTypeName = "Cổng COM truyền nhân số liệu";
                    }
                    gridControl.DataSource = lisConfig;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private TurnCOMMng BuildModel()
        {
            TurnCOMMng config = null;
            try
            {                
                config = new TurnCOMMng();
                config.Id = configId;
                config.Description = txtDescription.Text;
                config.IsActive = chkIsUse.Checked;
                config.COMTypeId = cbbComport.SelectedIndex;
                config.Status = cbbStatus.SelectedIndex;
                DateTime dateTimeAction = DateTime.Parse(tTimeAction.EditValue.ToString());
                TimeSpan timeAction = dateTimeAction.TimeOfDay;
                config.TimeAction = new TimeSpan(timeAction.Hours, timeAction.Minutes, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return config;
        }

        #endregion  

       

        private void gridView_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                configId = 0;
                if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "Id") != null)
                    int.TryParse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Id").ToString(), out configId);
                if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "COMTypeId") != null)
                    cbbComport.SelectedIndex = int.Parse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "COMTypeId").ToString());
                if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "Status") != null)
                    cbbStatus.SelectedIndex = int.Parse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Status").ToString());
                if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "Description") != null)
                    txtDescription.Text = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Description").ToString();
                if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "IsActive") != null)
                    chkIsUse.Checked = bool.Parse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "IsActive").ToString());
                if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "TimeAction") != null)
                {
                    TimeSpan timeAction = TimeSpan.Parse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "TimeAction").ToString());
                    DateTime dateNow = DateTime.Now;
                    tTimeAction.EditValue = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, timeAction.Hours, timeAction.Minutes, timeAction.Seconds);
                }
                EnableControl(false, true, false, true, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}
