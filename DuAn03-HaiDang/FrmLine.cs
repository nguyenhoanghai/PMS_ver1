using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTab;
using DuAn03_HaiDang.DAO;
using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.KeyPad_Chuyen.dao;
using DuAn03_HaiDang.KeyPad_Chuyen.pojo;
using DuAn03_HaiDang.Model;
using PMS.Business;
using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DuAn03_HaiDang
{
    public partial class FrmLine : Form
    {
        private ChuyenDAO chuyenDAO;
        private FloorDAO floorDAO;
        private int chuyenId = 0;
        private int Id = 0;
        public FrmLine()
        {
            InitializeComponent();
            chuyenDAO = new ChuyenDAO();
            floorDAO = new FloorDAO();
        }

        private void FrmLine_Load(object sender, EventArgs e)
        {
            try
            {
                LoadDataToCbbFloor();
                LoadDataToGridView();
                if (!AccountSuccess.IsOwner)
                    cbbFloor.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        #region User Event
        private void cbbFloor_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                LoadDataToGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void gridView_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                ResetForm();
                chuyenId = 0;
                if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "MaChuyen") != null)
                    int.TryParse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "MaChuyen").ToString(), out chuyenId);
                if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "Code") != null)
                    txtCode.Text = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Code").ToString();
                if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "TenChuyen") != null)
                    txtName.Text = gridView.GetRowCellValue(gridView.FocusedRowHandle, "TenChuyen").ToString();
                if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "DinhNghia") != null)
                    txtDescription.Text = gridView.GetRowCellValue(gridView.FocusedRowHandle, "DinhNghia").ToString();
                if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "Sound") != null)
                    txtSound.Text = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Sound").ToString();
                if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "FloorName") != null)
                    cbbFloor.Text = gridView.GetRowCellValue(gridView.FocusedRowHandle, "FloorName").ToString();
                if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "LaoDongDinhBien") != null)
                    txtLabor.Value = (int)gridView.GetRowCellValue(gridView.FocusedRowHandle, "LaoDongDinhBien");
                btnAdd.Enabled = false;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SaveLine();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SaveLine();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (chuyenId != 0)
                {
                    if (MessageBox.Show("Bạn có muốn xoá không?", "Xoá đối tượng", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var rs = BLLLine.Delete(chuyenId);
                        if (rs.IsSuccess)
                            LoadDataToGridView();
                        MessageBox.Show(rs.Messages[0].msg, rs.Messages[0].Title);
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
            ResetForm();
        }

        #endregion

        #region Private Function

        private void LoadDataToCbbFloor()
        {
            try
            {
                var list = BLLFloor.GetFloorForComBoBox();
                cbFloor2.DataSource = list.SelectList;
                cbFloor2.DisplayMember = "Name";
                cbFloor2.ValueMember = "IdFloor";
                cbFloor2.SelectedValue = list.DefaultValue;

                cbbFloor.DataSource = list.SelectList;
                cbbFloor.DisplayMember = "Name";
                cbbFloor.ValueMember = "IdFloor";
                cbbFloor.SelectedValue = list.DefaultValue;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Exception : lấy thông tin Lầu.\n" + ex.Message);
            }
        }

        private void LoadDataToGridView()
        {
            try
            {
                if (AccountSuccess.IsOwner)
                {
                    Floor floor = (Floor)cbbFloor.SelectedItem;
                    gridControl1.DataSource = null;
                    if (floor.IdFloor != 0)
                    {
                        var lines = BLLLine.GetLines(AccountSuccess.strListChuyenId.Split(',').Select(x => Convert.ToInt32(x)).ToList(), floor.IdFloor, true);
                        if (lines != null && lines.Count > 0)
                            gridControl1.DataSource = lines;
                    }
                }
                else
                {
                    gridControl1.DataSource = null;
                    var lines = BLLLine.GetLines(AccountSuccess.strListChuyenId.Split(',').Select(x => Convert.ToInt32(x)).ToList(), int.Parse(AccountSuccess.IdFloor), false);
                    if (lines != null && lines.Count > 0)
                        gridControl1.DataSource = lines;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private PMS.Data.Chuyen BuildModel()
        {
            PMS.Data.Chuyen chuyen = null;
            try
            {
                if (string.IsNullOrEmpty(txtCode.Text))
                    MessageBox.Show("Lỗi: Mã chuyền không được để trống");
                else if (string.IsNullOrEmpty(txtName.Text))
                    MessageBox.Show("Lỗi: Tên chuyền không được để trống");
                else if (txtLabor.Value == 0)
                    MessageBox.Show("Lỗi: Số lao động định biên phải lớn hơn 0");
                else
                {
                    Floor floor = (Floor)cbbFloor.SelectedItem;
                    if (floor == null || floor.IdFloor == 0)
                        MessageBox.Show("Lỗi: Bạn chưa chọn lầu cho chuyền");
                    else
                    {
                        chuyen = new PMS.Data.Chuyen();
                        chuyen.MaChuyen = chuyenId;
                        chuyen.Code = txtCode.Text;
                        chuyen.TenChuyen = txtName.Text;
                        chuyen.DinhNghia = txtDescription.Text;
                        chuyen.Sound = txtSound.Text;
                        chuyen.LaoDongDinhBien = (int)txtLabor.Value;
                        chuyen.FloorId = floor.IdFloor;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return chuyen;
        }

        private void ResetForm()
        {
            btnAdd.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            txtCode.Text = "";
            txtName.Text = "";
            txtDescription.Text = "";
            txtLabor.Value = 0;
            txtSound.Text = "";
            chuyenId = 0;
        }

        private void SaveLine()
        {
            try
            {
                var chuyen = BuildModel();
                if (chuyen != null)
                {
                    var rs = BLLLine.InsertOrUpdate(chuyen);
                    if (rs.IsSuccess)
                    {
                        LoadDataToGridView();
                        ResetForm();
                    }
                    MessageBox.Show(rs.Messages[0].msg, rs.Messages[0].Title);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        #endregion

        #region  Phan ca Cho Chuyen
        private void gridView1_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {
                ResetForm_Ass();
                Id = 0;
                if (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Id") != null)
                    int.TryParse(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Id").ToString(), out Id);
                if (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ShiftName") != null)
                    lueShift.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ShiftName").ToString();
                if (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ShiftOrder") != null)
                    txtOrder.Value = int.Parse(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ShiftOrder").ToString());

                btnAdd_s.Enabled = false;
                btnUpdate_s.Enabled = true;
                btnDelete_s.Enabled = true;
                cbFloor2.Enabled = false;
                cbLine.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnAdd_s_Click(object sender, EventArgs e)
        {
            Save_Assign();
        }

        private void btnUpdate_s_Click(object sender, EventArgs e)
        {
            Save_Assign();
        }

        private void btnDelete_s_Click(object sender, EventArgs e)
        {
            try
            {
                if (Id != 0)
                {
                    if (MessageBox.Show("Bạn có muốn xoá không?", "Xoá đối tượng", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var rs = BLLLineWorkingShift.Delete(Id);
                        if (rs.IsSuccess)
                            GetDataForLineShiftGridView();
                        MessageBox.Show(rs.Messages[0].msg, rs.Messages[0].Title);
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

        private void btnCancel_s_Click(object sender, EventArgs e)
        {
            ResetForm_Ass();
        }

        private void Save_Assign()
        {
            var floor = (Floor)cbFloor2.SelectedItem;
            var line = (LineModel)cbLine.SelectedItem;
            var shift = (P_WorkingShift)lueShift.GetSelectedDataRow();
            if (floor == null || floor.IdFloor == 0)
                MessageBox.Show("Vui lòng chọn Lầu.", "Lỗi Thao tác");
            else if (line == null)
                MessageBox.Show("Vui lòng chọn Chuyền.", "Lỗi Thao tác");
            else if (shift == null)
                MessageBox.Show("Vui lòng chọn Ca làm việc.", "Lỗi Thao tác");
            else
            {
                var lineShift = new LineWorkingShiftModel();
                lineShift.Id = Id;
                lineShift.LineId = line.MaChuyen;
                lineShift.LineName = line.TenChuyen;
                lineShift.ShiftId = shift.Id;
                lineShift.ShiftName = shift.Name;
                lineShift.ShiftOrder = (int)txtOrder.Value;
                var rs = BLLLineWorkingShift.InsertOrUpdate(lineShift);
                if (rs.IsSuccess)
                {
                    ResetForm_Ass();
                    GetDataForLineShiftGridView();
                }
                MessageBox.Show(rs.Messages[0].msg, rs.Messages[0].Title);
            }

        }

        private void ResetForm_Ass()
        {
            btnAdd_s.Enabled = true;
            btnUpdate_s.Enabled = false;
            btnDelete_s.Enabled = false;
            txtOrder.Value = 0;
            Id = 0;
            cbLine.Enabled = true;
            cbFloor2.Enabled = true;
        }

        private void GetDataForCBLine()
        {
            try
            {
                if (AccountSuccess.IsOwner)
                {
                    Floor floor = (Floor)cbFloor2.SelectedItem;
                    cbLine.DataSource = null;
                    if (floor.IdFloor != 0)
                    {
                        var lines = BLLLine.GetLines(AccountSuccess.strListChuyenId.Split(',').Select(x => Convert.ToInt32(x)).ToList(), floor.IdFloor, true);
                        if (lines != null && lines.Count > 0)
                        {
                            cbLine.DataSource = lines;
                            cbLine.DisplayMember = "TenChuyen";
                        }
                    }
                }
                else
                {
                    cbLine.DataSource = null;
                    var lines = BLLLine.GetLines(AccountSuccess.strListChuyenId.Split(',').Select(x => Convert.ToInt32(x)).ToList(), int.Parse(AccountSuccess.IdFloor), false);
                    if (lines != null && lines.Count > 0)
                    {
                        cbLine.DataSource = lines;
                        cbLine.DisplayMember = "TenChuyen";
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        private void xtraTabControl1_SelectedPageChanging(object sender, TabPageChangingEventArgs e)
        {
            if (!AccountSuccess.IsOwner)
                cbbFloor.Enabled = false;
            else
                LoadDataToCbbFloor();

            if (xtraTabControl1.SelectedTabPageIndex == 1)
            {
                LoadDataToGridView();
                ResetForm();
            }
            else
            {
                ResetForm_Ass();
                GetDataForCBLine();
                GetShift_GridView();
            }
        }

        private void cbFloor2_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetDataForCBLine();
        }

        private void cbLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetDataForLineShiftGridView();
        }

        private void GetDataForLineShiftGridView()
        {
            try
            {
                var line = (LineModel)cbLine.SelectedItem;
                gridControl2.DataSource = null;
                if (line != null)
                {
                    var lines = BLLLineWorkingShift.GetWorkingShiftOfLine(line.MaChuyen);
                    if (lines != null && lines.Count > 0)
                        gridControl2.DataSource = lines;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetShift_GridView()
        {
            try
            {
                lueShift.Text = "(-- Chọn Ca Làm Việc --)";
                var shifts = BLLShift.GetShift();
                if (shifts != null && shifts.Count > 0)
                {
                    lueShift.Properties.DataSource = shifts;
                    lueShift.Properties.DisplayMember = "Name";
                    lueShift.Properties.ValueMember = "Id";
                }
            }
            catch (Exception)
            { }
        }

        private void lueShift_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                var shift = (P_WorkingShift)lueShift.GetSelectedDataRow();
                lbTime.Text = "Giờ : " + shift.TimeStart + " => " + shift.TimeEnd;
            }
            catch (Exception)
            { }
        }

    }
}
