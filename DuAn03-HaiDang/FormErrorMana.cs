using DevExpress.XtraGrid.Views.Grid;
using GPRO.Ultilities;
using PMS.Business;
using PMS.Data;
using QuanLyNangSuat.GridView_Model;
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
    public partial class FrmErrorMana : Form
    {
        private int errorId = 0;
        private int groupErrId = 0;
          
        public FrmErrorMana()
        {
            InitializeComponent(); 
        }
        private void FrmErrorMana_Load(object sender, EventArgs e)
        {
            LoadDataForGridGroupError();
        }

        private void xtraTabControl1_SelectedPageChanging(object sender, DevExpress.XtraTab.TabPageChangingEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 1)
            {
                LoadDataForGridGroupError();
                ResetGroupErrorForm();
            }
            else
            {
                LoadDataForGridError();
                ResetErrorForm();
                LoadDataToCbbGroupError();
            } 
        }

        #region ERROR
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (errorId != 0)
                {
                    if (MessageBox.Show("Bạn có muốn xoá không?", "Xoá đối tượng", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var result = BLLError.Delete(errorId);
                        if (result.IsSuccess)
                        {
                            LoadDataForGridError();
                            ResetErrorForm();
                        }
                        MessageBox.Show(result.Messages[0].msg, result.Messages[0].Title);
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ResetErrorForm();
        }

        private void btnRefreshGroup_Click(object sender, EventArgs e)
        {
            LoadDataToCbbGroupError();
        }
         
        private void LoadDataForGridError()
        {
            try
            { 
                gridError.DataSource = BLLError.GetAll() ; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Error BuildModel()
        {
            Error error = null;
            try
            {
                var groupError = (GroupError)cbbGroupError.SelectedItem;
                if (string.IsNullOrEmpty(txtCode.Value.ToString()))
                    MessageBox.Show("Lỗi: Mã lỗi không được để trống");
                else if (string.IsNullOrEmpty(txtName.Text))
                    MessageBox.Show("Lỗi: Tên lỗi không được để trống");
                else if (groupError == null)
                    MessageBox.Show("Lỗi: Bạn chưa chọn nhóm lỗi");
                else
                {
                    error = new Error();
                    error.Id = errorId;
                    error.Code = (int)txtCode.Value;
                    error.Name = txtName.Text;
                    error.Description = txtNote.Text;
                    error.GroupErrorId = groupError.Id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return error;
        }

        private void LoadDataToCbbGroupError()
        {
            try
            {
                cbbGroupError.DataSource = null;
                var listGError = BLLGroupError.GetAll();
                if (listGError != null)
                {
                    cbbGroupError.DataSource = listGError;
                    cbbGroupError.ValueMember = "Id";
                    cbbGroupError.DisplayMember = "Name";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ResetErrorForm()
        {
            errorId = 0;
            btnAdd.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            if (cbbGroupError.Items.Count > 0)
                cbbGroupError.SelectedIndex = 0;
            txtCode.Value = 0;
            txtName.Text = string.Empty;
            txtNote.Text = string.Empty;
        }

        private void Save()
        {
            try
            {
                Error error = BuildModel();
                if (error != null)
                {
                    var result = BLLError.InsertOrUpdate(error);
                    if (result.IsSuccess)
                    {
                        ResetErrorForm();
                        LoadDataForGridError();
                    }
                    MessageBox.Show(result.Messages[0].msg, result.Messages[0].Title);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        private void gridView2_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {
                int.TryParse(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "Id").ToString(), out errorId);
                txtCode.Value = decimal.Parse(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "Code").ToString());
                txtName.Text = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "Name").ToString();
                txtNote.Text = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "Description").ToString();
                cbbGroupError.Text = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "GroupName").ToString();
                btnAdd.Enabled = false;
                btnDelete.Enabled = true;
                btnUpdate.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        #endregion
        
        #region GROUP ERROR
        private void ResetGroupErrorForm()
        {
            btnAdd_g.Enabled = true;
            btnUpdate_g.Enabled = false;
            btnDelete_g.Enabled = false;
            txtCode_g.Text = string.Empty;
            txtName_g.Text = string.Empty;
            txtDescription_g.Text = string.Empty;
            groupErrId = 0;
        }

        private void LoadDataForGridGroupError()
        {
            try
            {
                gridErrorGroup.DataSource = BLLGroupError.GetAll();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnAdd_g_Click(object sender, EventArgs e)
        {
            SaveGroupError();
        }

        private void btnUpdate_g_Click(object sender, EventArgs e)
        {
            SaveGroupError();
        }

        private void btnDelete_g_Click(object sender, EventArgs e)
        {
            try
            {
                if (groupErrId != 0)
                {
                    if (MessageBox.Show("Bạn có muốn xoá không?", "Xoá đối tượng", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var result = BLLGroupError.Delete(groupErrId);
                        if (result.IsSuccess)
                        {
                            LoadDataForGridGroupError();
                            ResetGroupErrorForm();
                        }
                        MessageBox.Show(result.Messages[0].msg, result.Messages[0].Title);
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

        private void btnCancel_g_Click(object sender, EventArgs e)
        {
            ResetGroupErrorForm();
        }

        private void SaveGroupError()
        {
            try
            {
                GroupError groupError = GetGroupErrorModel();
                var result = BLLGroupError.InsertOrUpdate(groupError);
                if (result.IsSuccess)
                {
                    LoadDataForGridGroupError();
                    ResetGroupErrorForm();
                }
                MessageBox.Show(result.Messages[0].msg, result.Messages[0].Title);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private GroupError GetGroupErrorModel()
        {
            try
            {
                GroupError error = null;
                try
                {
                    if (string.IsNullOrEmpty(txtCode_g.Text))
                        MessageBox.Show("Lỗi: Tên Nhóm lỗi không được để trống");
                    else if (string.IsNullOrEmpty(txtName_g.Text))
                        MessageBox.Show("Lỗi: Tên Nhóm lỗi không được để trống");
                    else
                    {
                        error = new GroupError();
                        error.Id = groupErrId;
                        error.Code = txtCode_g.Text;
                        error.Name = txtName_g.Text;
                        error.Description = txtDescription_g.Text;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return error;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void gridView_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {
                int.TryParse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Id").ToString(), out groupErrId );
                txtCode_g.Text =  (gridView.GetRowCellValue(gridView.FocusedRowHandle, "Code").ToString());
                txtName_g.Text = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Name").ToString();
                txtDescription_g.Text = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Description").ToString(); 
                btnAdd_g.Enabled = false;
                btnDelete_g.Enabled = true;
                btnUpdate_g.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        #endregion

        
    }
}
