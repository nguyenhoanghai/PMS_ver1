using DevExpress.XtraEditors.Controls;
using DuAn03_HaiDang.DAO;
using DuAn03_HaiDang.Model;
using DuAn03_HaiDang.POJO;
using PMS.Business;
using PMS.Business.Models;
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
    public partial class FrmMailTemplate : Form
    {
        private int mailTemplateId = 0;
        public FrmMailTemplate()
        {
            InitializeComponent();

        }

        private void FrmMailTemplate_Load(object sender, EventArgs e)
        {
            try
            {
                LoadDataForGridView();
                LoadDataForCbbMailSend();
                LoadDataForCbbMailReceives();
                LoadDataForListCHKMailFile();
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
                var listMail = BLLMailSend.GetAll();// mailSendDAO.GetListSelectItem();
                if (listMail != null && listMail.Count > 0)
                {
                    cbbMailSend.DataSource = listMail.ToList();
                    cbbMailSend.DisplayMember = "Address";
                    cbbMailSend.ValueMember = "Id";
                    cbbMailSend.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadDataForCbbMailReceives()
        {
            try
            {
                var listMail = BLLMailReceive.GetAll(); // mailReceiveDAO.GetListSelectItem();
                if (listMail != null && listMail.Count > 0)
                {
                    foreach (var item in listMail)
                    {
                        this.cbbMailReceives.Properties.Items.Add(new DevExpress.XtraEditors.Controls.CheckedListBoxItem(item.Id, item.Address));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadDataForListCHKMailFile()
        {
            try
            {
                var listMailFile = BLLMailFile.GetAll();// mailFileDAO.GetListSelectItem();
                if (listMailFile != null && listMailFile.Count > 0)
                {
                    foreach (var item in listMailFile)
                    {
                        this.listchkFileAttack.Items.Add(new DevExpress.XtraEditors.Controls.CheckedListBoxItem(item.Id, item.Name));

                    }
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
                gridMail.DataSource = BLLMailTemplate.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void butAdd_Click(object sender, EventArgs e)
        {
            if (CheckValidate())
                Save();
        }

        private void butUpdate_Click(object sender, EventArgs e)
        {
            if (CheckValidate())
                Save();
        }

        private void butDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (mailTemplateId != 0)
                {
                    if (MessageBox.Show("Bạn có muốn xoá không?", "Xoá đối tượng", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var result = BLLMailTemplate.Delete(mailTemplateId);
                        MessageBox.Show(result.Messages[0].msg, result.Messages[0].Title);
                        if (result.IsSuccess)
                        {
                            LoadDataForGridView();
                            ResetForm();
                        }
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

        private void ResetForm()
        {
            try
            {
                mailTemplateId = 0;
                txtName.Text = string.Empty;
                txtSubject.Text = string.Empty;
                txtContent.Text = string.Empty;
                txtDescription.Text = string.Empty;
                chkIsActive.Checked = false;
                cbbMailSend.SelectedIndex = 0;
                cbbMailReceives.SetEditValue(string.Empty);
                listchkFileAttack.UnCheckAll();

                butAdd.Enabled = true;
                butDelete.Enabled = false;
                butUpdate.Enabled = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Save()
        {
            try
            {
                var mail = new MailTemplateModel();
                mail.Id = mailTemplateId;
                mail.Name = txtName.Text;
                mail.Description = txtDescription.Text;
                mail.IsActive = chkIsActive.Checked;
                mail.Subject = txtSubject.Text;
                mail.Content = txtContent.Text;
                var mailTSelect = ((MailSend_Model)cbbMailSend.SelectedItem);
                if (mail != null)
                    mail.MailSendId = mailTSelect.Id;
                if (this.cbbMailReceives.Properties.Items != null && this.cbbMailReceives.Properties.Items.Count > 0)
                {
                    foreach (CheckedListBoxItem item in this.cbbMailReceives.Properties.Items)
                    {
                        if (item.CheckState == CheckState.Checked)
                            mail.MailReceiveIds += (!string.IsNullOrEmpty(mail.MailReceiveIds) ? "|" + item.Value : item.Value);
                    }
                }
                List<int> listFileId = new List<int>();
                if (this.listchkFileAttack.Items != null && this.listchkFileAttack.Items.Count > 0)
                {
                    foreach (CheckedListBoxItem item in this.listchkFileAttack.Items)
                    {
                        if (item.CheckState == CheckState.Checked)
                            mail.MailFileIds += (!string.IsNullOrEmpty(mail.MailFileIds) ? "|" + item.Value : item.Value);   
                    }
                } 
                var result = BLLMailTemplate.CreateOrUpdate(mail);
                MessageBox.Show(result.Messages[0].msg, result.Messages[0].Title);
                if (result.IsSuccess)
                {
                    ResetForm();
                    LoadDataForGridView();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool CheckValidate()
        {
            var flag = true;
            try
            {
                if (string.IsNullOrEmpty(txtName.Text))
                {
                    MessageBox.Show("Vui lòng nhập Tên.");
                    flag = false;
                }
                else if (string.IsNullOrEmpty(txtSubject.Text))
                {
                    MessageBox.Show("Vui lòng nhập Tiêu đề.");
                    flag = false;
                }
                else if (string.IsNullOrEmpty(txtContent.Text))
                {
                    MessageBox.Show("Vui lòng nhập nội dung.");
                    flag = false;
                }
                else if (cbbMailSend.SelectedItem == null)
                {
                    MessageBox.Show("vui lòng chọn mail gửi.");
                    flag = false;
                }
                else if (cbbMailReceives.Properties.GetItems().GetCheckedValues().Count == 0)
                {
                    MessageBox.Show("vui lòng chọn mail nhận Báo cáo.");
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi : " + ex);
            }
            return flag;
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void gridView_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                butAdd.Enabled = false;
                butDelete.Enabled = true;
                butUpdate.Enabled = true;
                listchkFileAttack.UnCheckAll();
                int.TryParse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Id").ToString(), out mailTemplateId);
                txtName.Text = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Name").ToString();
                txtDescription.Text = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Description").ToString();
                txtSubject.Text = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Subject").ToString();
                txtContent.Text = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Content").ToString();
                chkIsActive.Checked = bool.Parse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "IsActive").ToString());
                chkIsActive.Enabled = true;
                var t = cbbMailSend.Items.Cast<MailSend_Model>().FirstOrDefault(x => x.Id == int.Parse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "MailSendId").ToString()));
                if (t != null)
                    cbbMailSend.SelectedItem = t;

                cbbMailReceives.SetEditValue(string.Empty);
                var mailRe = gridView.GetRowCellValue(gridView.FocusedRowHandle, "MailReceiveIds") != null ?gridView.GetRowCellValue(gridView.FocusedRowHandle, "MailReceiveIds").ToString(): string.Empty;
                if (!string.IsNullOrEmpty(mailRe))
                {
                    var arrReceiveId = mailRe.Split('|');
                    if (arrReceiveId != null && arrReceiveId.Count() > 0)
                    {
                        if (this.cbbMailReceives.Properties.Items != null && this.cbbMailReceives.Properties.Items.Count > 0)
                        {
                            foreach (CheckedListBoxItem item in this.cbbMailReceives.Properties.Items)
                            {
                                foreach (var id in arrReceiveId)
                                {
                                    int receiveId = 0;
                                    if (!string.IsNullOrEmpty(id))
                                        int.TryParse(id, out receiveId);
                                    if ((int)item.Value == receiveId)
                                    {
                                        item.CheckState = CheckState.Checked;
                                        break;
                                    }
                                    else
                                        item.CheckState = CheckState.Unchecked;
                                }
                            }
                        }
                    }
                }

                var mFiles = gridView.GetRowCellValue(gridView.FocusedRowHandle, "MailFileIds") != null ? gridView.GetRowCellValue(gridView.FocusedRowHandle, "MailFileIds").ToString() : string.Empty;
                if (!string.IsNullOrEmpty(mFiles))
                {
                    var fileids = mFiles.Split('|').Select(x => Convert.ToInt32(x));
                    if (fileids != null && fileids.Count() > 0)
                    {
                        if (this.listchkFileAttack.Items != null && this.listchkFileAttack.Items.Count > 0)
                        {
                            foreach (CheckedListBoxItem item in this.listchkFileAttack.Items)
                            {
                                foreach (var id in fileids)
                                {
                                    if ((int)item.Value == id)
                                    {
                                        item.CheckState = CheckState.Checked;
                                        break;
                                    }
                                    else
                                        item.CheckState = CheckState.Unchecked;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: Bind dữ liệu bị lỗi\n" + ex.Message);
            }
        }
    }
}
