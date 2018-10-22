using DuAn03_HaiDang.DATAACCESS;
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

namespace QuanLyNangSuat
{
    public partial class FrmVideoShedule : Form
    {
        private int oId = 0;
        private List<VideoScheduleModel> listObj = new List<VideoScheduleModel>();
        public FrmVideoShedule()
        {
            InitializeComponent();
        }

        private void FrmVideoShedule_Load(object sender, EventArgs e)
        {
            gridDetail.Enabled = true;
            cbVideo.DataSource = null;
            cbVideo.DataSource = BLLVideoLibrary.Gets();
            cbVideo.DisplayMember = "Name";
            cbVideo.ValueMember = "Id";

            cbLine.DataSource = null;
            cbLine.DataSource = BLLLine.GetLines_s(AccountSuccess.strListChuyenId.Split(',').Select(x => Convert.ToInt32(x)).ToList());
            cbLine.ValueMember = "Machuyen";
            cbLine.DisplayMember = "TenChuyen";
        }

        private void GetDataToGridSchedule()
        {
            var obj = (LineModel)cbLine.SelectedItem;
            if (obj != null)
            {
                gridSchedule.DataSource = null;
                var objs = BLLPlayVideoSchedule.Gets(obj.MaChuyen);
                gridSchedule.DataSource = objs;
                listObj.Clear();
                listObj.AddRange(objs);
            }
        }

        private void cbLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetDataToGridSchedule();
            gridDetail.Rows.Clear();
            ResetForm();
        }

        private void gridVideo_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                txtStart.EditValue = gridVideo.GetRowCellValue(gridVideo.FocusedRowHandle, "TimeStart").ToString();
                txtEnd.EditValue = gridVideo.GetRowCellValue(gridVideo.FocusedRowHandle, "TimeEnd").ToString();
                oId = 0;
                int.TryParse(gridVideo.GetRowCellValue(gridVideo.FocusedRowHandle, "Id").ToString(), out oId);

                bool chk = false;
                bool.TryParse(gridVideo.GetRowCellValue(gridVideo.FocusedRowHandle, "IsActive").ToString(), out chk);
                chbIsActive.Checked = chk;
                chbIsActive.Enabled = true;
                btnAdd.Enabled = false;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;

                gridDetail.Rows.Clear();
                var obj = listObj.FirstOrDefault(x => x.Id == oId);
                if (obj != null && obj.Detail.Count > 0)
                {
                    DataGridViewRow row;
                    for (int i = 0; i < obj.Detail.Count; i++)
                    {
                        row = (DataGridViewRow)gridDetail.Rows[i].Clone();
                        row.Cells[0].Value = obj.Detail[i].Id;
                        row.Cells[1].Value = obj.Detail[i].OrderIndex;
                        row.Cells[2].Value = obj.Detail[i].VideoId;
                        gridDetail.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private bool CheckValidate()
        {
            var flag = true;
            try
            {
                if (cbLine.SelectedItem == null)
                {
                    MessageBox.Show("Bạn chưa chọn chuyền.");
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi : " + ex);
            }
            return flag;
        }

        private void Save()
        {
            try
            {
                var line = (LineModel)cbLine.SelectedItem;
                var obj = new VideoScheduleModel();
                obj.Id = oId;
                TimeSpan time = new TimeSpan(0, 0, 0);
                try
                {
                    DateTime datetime = DateTime.Parse(txtStart.EditValue.ToString());
                    time = datetime.TimeOfDay;
                }
                catch
                {
                    TimeSpan.TryParse(txtStart.EditValue.ToString(), out time);
                }
                obj.TimeStart = time;

                try
                {
                    DateTime datetime = DateTime.Parse(txtEnd.EditValue.ToString());
                    time = datetime.TimeOfDay;
                }
                catch
                {
                    TimeSpan.TryParse(txtEnd.EditValue.ToString(), out time);
                }
                obj.TimeEnd = time;
                obj.IsActive = chbIsActive.Checked;
                obj.LineId = line.MaChuyen;


                foreach (DataGridViewRow item in gridDetail.Rows)
                {
                    int index = 0, videoId = 0;
                    try
                    {
                        int.TryParse(item.Cells[1].Value.ToString(), out index);
                        int.TryParse(item.Cells[2].Value.ToString(), out videoId);

                        obj.Detail.Add(new P_PlayVideoSheduleDetail()
                        {
                            Id = 0,
                            OrderIndex = index,
                            VideoId = videoId
                        });
                    }
                    catch (Exception)
                    { }
                }

                var result = BLLPlayVideoSchedule.CreateOrUpdate(obj);

                if (result.IsSuccess)
                {
                    ResetForm();
                    GetDataToGridSchedule();
                }
                else
                    MessageBox.Show(result.Messages[0].msg, result.Messages[0].Title);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi :" + ex.Message);
            }
        }

        private void ResetForm()
        {
            TimeSpan time = DateTime.Now.TimeOfDay;
            txtStart.EditValue = TimeSpan.Parse(time.Hours + ":" + time.Minutes + ":00");
            txtEnd.EditValue = TimeSpan.Parse(time.Hours + ":" + time.Minutes + ":00");
            chbIsActive.Checked = true;
            btnAdd.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            chbIsActive.Enabled = true; 
            gridDetail.Rows.Clear();
            oId = 0;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (CheckValidate())
                Save();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (CheckValidate())
                Save();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Bạn có muốn xoá dữ liệu?", "Xoá dữa liệu", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var result = BLLPlayVideoSchedule.Delete(oId);

                    if (result.IsSuccess)
                    {
                        GetDataToGridSchedule();
                        ResetForm();
                    }
                    else
                        MessageBox.Show(result.Messages[0].msg, result.Messages[0].Title);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}
