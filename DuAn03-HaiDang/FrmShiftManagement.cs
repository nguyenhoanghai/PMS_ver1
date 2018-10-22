using DevExpress.XtraGrid.Views.Grid;
using PMS.Business;
using PMS.Data;
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
    public partial class FrmShiftManagement : Form
    { 
        int shiftId = 0;
        public FrmShiftManagement()
        {
            InitializeComponent(); 
        }

        private void FrmShiftManagement_Load(object sender, EventArgs e)
        {
            GetShiftToGrid();
        }
         

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SaveShift();
        }

        private void SaveShift()
        {
            if (string.IsNullOrEmpty(txtCaLamViec.Text))
                MessageBox.Show("Vui lòng nhập tên ca làm việc", "Lỗi nhập liệu");
            else
            {
                var shift = new P_WorkingShift();
                shift.Id = shiftId;
                shift.Name = txtCaLamViec.Text;
                shift.TimeStart = DateTime.Parse(teditTimeStart.EditValue.ToString()).TimeOfDay;
                shift.TimeEnd = DateTime.Parse(teditTimeEnd.EditValue.ToString()).TimeOfDay;
                var kq = BLLShift.InsertOrUpdateShift(shift);
                if (kq.IsSuccess)
                {
                    GetShiftToGrid();
                }
                MessageBox.Show(kq.Messages[0].msg, kq.Messages[0].Title);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SaveShift();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (shiftId != 0)
            {
                var kq = BLLShift.DeleteShift(shiftId);
                if (kq.IsSuccess)
                {
                    MessageBox.Show("Xoá ca làm việc của chuyền thành công.", "xóa thành công", MessageBoxButtons.OK, MessageBoxIcon.None);
                    ResetShiftForm();
                }
                else
                    MessageBox.Show("Không thể xoá ca làm việc.", "xóa thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn ca làm việc muốn xoá , Vui lòng thực hiện thao tác này.", "Lỗi thực hiện", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void ResetShiftForm()
        {
            txtCaLamViec.Text = "";
            btnAdd.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ResetShiftForm();
        } 

        private void GetShiftToGrid()
        {
            try
            {
                var shifts = BLLShift.GetShift();
                gridControl.DataSource = null;
                gridControl.DataSource = shifts;
            }
            catch (Exception)
            {
            }
        }
         

        private void gridView_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {
                int.TryParse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Id").ToString(), out shiftId);
                txtCaLamViec.Text = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Name").ToString();
                teditTimeStart.EditValue = TimeSpan.Parse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "TimeStart").ToString());
                teditTimeEnd.EditValue = TimeSpan.Parse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "TimeEnd").ToString());
                btnAdd.Enabled = false;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
            catch (Exception)
            { }
        }
    }
}
