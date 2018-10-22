using DuAn03_HaiDang.DAO;
using DuAn03_HaiDang.KeyPad_Chuyen.dao;
using DuAn03_HaiDang.KeyPad_Chuyen.pojo;
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
    public partial class FrmCluster : Form
    {
        private ClusterDAO clusterDAO;
        private FloorDAO floorDAO;
        private ChuyenDAO chuyenDAO;
        private int clusterId = 0;
        public FrmCluster()
        {
            InitializeComponent();
            clusterDAO = new ClusterDAO();
            floorDAO = new FloorDAO();
            chuyenDAO = new ChuyenDAO();
        }

        private void FrmCluster_Load(object sender, EventArgs e)
        {
            try
            {
                LoadDataToCbbFloor();
                LoadDataToGridView();
                EnableControl(true, false, false, false, false);
                EnableInput(false);
                ClearInput();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
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
                txtCode.Text = string.Empty;
                txtName.Text = string.Empty;
                txtDescription.Text = string.Empty;
                chkIsEndLine.Checked = false;
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
                txtCode.Enabled = enable;
                txtName.Enabled = enable;
                txtDescription.Enabled = enable;
                cbbLine.Enabled = enable;
                cbbFloor.Enabled = enable;
                chkIsEndLine.Enabled = enable;
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
                Cum cluster = BuildModel();
                if (cluster != null)
                {
                    var result = BLLCluster.InsertOrUpdate(cluster);
                    if (result.IsSuccess)
                    {
                        clusterId = 0;
                        EnableControl(true, false, false, false, false);
                        EnableInput(false);
                        LoadDataToGridView();
                        ClearInput();
                    }
                    MessageBox.Show(result.Messages[0].msg, result.Messages[0].Title);
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
                if (clusterId != 0)
                {
                    if (MessageBox.Show("Bạn có muốn xoá không?", "Xoá đối tượng", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var result = clusterDAO.DeleteObj(clusterId);
                        if (result != 0)
                        {
                            MessageBox.Show("Xoá thành công.");
                            LoadDataToGridView();
                            EnableControl(true, false, false, false, false);
                            EnableInput(false);
                            ClearInput();
                            LoadDataToGridView();
                            clusterId = 0;
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
                if (clusterId == 0)
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

        private void LoadDataToCbbFloor()
        {
            try
            {
                var listFloor = new List<Floor>();
                listFloor.Add(new Floor() { IdFloor = 0, Name = "(Chọn lầu của chuyền)" });
                var listFloorInDB = BLLFloor.GetAll(); // floorDAO.GetListFloor();
                if (listFloorInDB != null && listFloorInDB.Count > 0)
                    listFloor.AddRange(listFloorInDB);
                cbbFloor.DataSource = listFloor;
                cbbFloor.DisplayMember = "Name";
                cbbFloor.ValueMember = "IdFloor";

                var fl = listFloor.Where(x => x.IsDefault).FirstOrDefault();
                cbbFloor.SelectedValue = fl != null ? fl.IdFloor : 0;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadDataToCbbLine()
        {
            try
            {
                Floor floor = (Floor)cbbFloor.SelectedItem;
                if (floor != null)
                {
                    var listLine = new List<LineModel>();
                    listLine.Add(new LineModel() { MaChuyen = 0, TenChuyen = "(Chọn chuyền)" });
                    var listLineInDB = BLLLine.GetLines(floor.IdFloor); //( chuyenDAO.GetListDSChuyenByFloorId(floor.IdFloor.ToString());
                    if (listLineInDB != null && listLineInDB.Count > 0)
                        listLine.AddRange(listLineInDB);
                    cbbLine.DataSource = listLine;
                    cbbLine.DisplayMember = "TenChuyen";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadDataToGridView()
        {
            try
            {
                var listCluster = clusterDAO.GetListCluster();
                if (listCluster != null && listCluster.Count > 0)
                {
                    gridControl.DataSource = listCluster;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Cum BuildModel()
        {
            Cum cluster = null;
            try
            {
                if (string.IsNullOrEmpty(txtCode.Text))
                    MessageBox.Show("Lỗi: Mã cụm không được để trống");
                else if (clusterDAO.CheckExistCode(txtCode.Text.Trim(), clusterId))
                    MessageBox.Show("Lỗi: Mã cụm đã tồn tại.Vui lòng chọn mã khác.");
                else if (string.IsNullOrEmpty(txtName.Text))
                    MessageBox.Show("Lỗi: Tên cụm không được để trống");
                else
                {
                    var floor = (Floor)cbbFloor.SelectedItem;
                    if (floor == null || floor.IdFloor == 0)
                        MessageBox.Show("Lỗi: Bạn chưa chọn lầu cho chuyền");
                    else
                    {
                        var chuyen = (LineModel)cbbLine.SelectedItem;
                        if (chuyen == null || chuyen.MaChuyen == 0)
                            MessageBox.Show("Lỗi: Bạn chưa chọn chuyền");
                        else
                        {
                            cluster = new Cum();
                            cluster.Id = clusterId;
                            cluster.Code = txtCode.Text;
                            cluster.TenCum = txtName.Text;
                            cluster.MoTa = txtDescription.Text;
                            cluster.FloorId = floor.IdFloor;
                            cluster.IdChuyen = chuyen.MaChuyen;
                            cluster.IsEndOfLine = chkIsEndLine.Checked;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return cluster;
        }

        #endregion



        private void gridView_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                clusterId = 0;
                if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "Id") != null)
                    int.TryParse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Id").ToString(), out clusterId);
                if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "Code") != null)
                    txtCode.Text = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Code").ToString();
                if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "Name") != null)
                    txtName.Text = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Name").ToString();
                if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "Description") != null)
                    txtDescription.Text = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Description").ToString();
                if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "FloorName") != null)
                    cbbFloor.Text = gridView.GetRowCellValue(gridView.FocusedRowHandle, "FloorName").ToString();
                if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "LineName") != null)
                    cbbLine.Text = gridView.GetRowCellValue(gridView.FocusedRowHandle, "LineName").ToString();
                if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "IsEndOfLine") != null)
                    chkIsEndLine.Checked = bool.Parse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "IsEndOfLine").ToString());

                EnableControl(false, true, false, true, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void cbbFloor_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                LoadDataToCbbLine();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}
