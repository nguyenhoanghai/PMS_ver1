using DuAn03_HaiDang.DAO;
using DuAn03_HaiDang.Model;
using DuAn03_HaiDang.POJO;
using PMS.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PMS.Data;
using DuAn03_HaiDang.DATAACCESS;
using PMS.Business.Models;

namespace DuAn03_HaiDang
{
    public partial class FrmMailSchedule : Form
    {
        private int mailScheduleId = 0;
        private int mailTemplateId = 0;
        FrmMainNew frmMainNew;
        public FrmMailSchedule(FrmMainNew _frmMainNew)
        {
            InitializeComponent();
            this.frmMainNew = _frmMainNew;
        }

        private void FrmMailSchedule_Load(object sender, EventArgs e)
        {
            try
            {
                LoadDataForCbbMailSend();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void LoadDataForCbbMailSend()
        {
            try
            {
                var listMailT = BLLMailTemplate.GetAll();
                if (listMailT != null && listMailT.Count > 0)
                {
                    cbbMailTemplate.DataSource = listMailT;
                    cbbMailTemplate.DisplayMember = "Name";
                    cbbMailTemplate.ValueMember = "Id";
                    cbbMailTemplate.SelectedIndex = 0;
                    LoadDataForGridView();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadDataForGridView()
        {
            try
            {
                var select = (MailTemplateModel)cbbMailTemplate.SelectedItem;
                if (select != null)
                {
                    gridMail.DataSource = BLLMailSchedule.GetByTemplateId(select.Id);
                    mailTemplateId = select.Id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void cbbMailTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadDataForGridView();
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
                if (cbbMailTemplate.SelectedItem == null)
                {
                    MessageBox.Show("Bạn chưa chọn hoặc chưa có cấu hình mail.");
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
                var mail = new MAIL_SCHEDULE();
                mail.Id = mailScheduleId;
                TimeSpan time = new TimeSpan(0, 0, 0);
                try
                {
                    DateTime datetime = DateTime.Parse(teTime.EditValue.ToString());
                    time = datetime.TimeOfDay;
                }
                catch
                {
                    TimeSpan.TryParse(teTime.EditValue.ToString(), out time);
                }
                mail.Time = (time.Hours.ToString() + ":" + time.Minutes.ToString() + ":00");
                mail.IsActive = chkIsActive.Checked;
                var select = (MailTemplateModel)cbbMailTemplate.SelectedItem;
                mail.MailTemplateId = select.Id;
                var result = BLLMailSchedule.CreateOrUpdate(mail);
                MessageBox.Show(result.Messages[0].msg, result.Messages[0].Title);
                if (result.IsSuccess)
                {
                    ResetForm();
                    LoadDataForGridView();
                    //if (MessageBox.Show("Bạn có muốn khởi động lại chương trình?", "Khởi động chương trình", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    //{
                    //    Application.Restart(); 
                    //}
                    if (!frmMainNew.IsStopProcess)
                        frmMainNew.frmSendMailAndReadSound.GetMailSchedule(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi :" + ex.Message);
            }
        }

        private void ResetForm()
        {
            TimeSpan time = DateTime.Now.TimeOfDay;
            teTime.EditValue = TimeSpan.Parse(time.Hours + ":" + time.Minutes + ":00");
            chkIsActive.Checked = true;
            btnAdd.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            teTime.Enabled = true;
            chkIsActive.Enabled = true;
            mailScheduleId = 0;
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
                    var result = BLLMailSchedule.Delete(mailScheduleId);
                    MessageBox.Show(result.Messages[0].msg, result.Messages[0].Title);
                    if (result.IsSuccess)
                    {
                        LoadDataForGridView();
                        ResetForm();
                    }
                }
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
                int.TryParse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Id").ToString(), out mailScheduleId);
                teTime.EditValue = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Time").ToString();
                bool isCheck = false;
                bool.TryParse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "IsActive").ToString(), out isCheck);
                chkIsActive.Checked = isCheck;
                chkIsActive.Enabled = true;

                btnAdd.Enabled = false;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}
