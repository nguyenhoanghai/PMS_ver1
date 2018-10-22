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

namespace DuAn03_HaiDang
{
    public partial class FrmMailSend : Form
    {   
        private int mailSendId = 0;
        private int mailReceiveId = 0; 
        public FrmMailSend()
        {
            InitializeComponent();   
        }

        private void FrmMailSend_Load(object sender, EventArgs e)
        {
            try
            {
                LoadDataForCbbMailType();
                LoadListMailSendToGridView();
                LoadListMailReceiveToGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        #region     MAILRECEIVE
        #region Event
        private void btnAdd_r_Click(object sender, EventArgs e)
        {
            if (CheckValidate_Re())
                SaveMail_R();
        }

        private void btnUpdate_r_Click(object sender, EventArgs e)
        {
            if (CheckValidate_Re())
                SaveMail_R();
        }

        private void btnDelete_r_Click(object sender, EventArgs e)
        {
            try
            {
                if (mailReceiveId > 0)
                {
                    if (MessageBox.Show("Bạn có muốn xoá?", "Xoá Mail Nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        BLLMailReceive.Delete(mailReceiveId);
                        ResetMailReceiveForm();
                        LoadListMailReceiveToGridView();
                    }
                }
                else
                    MessageBox.Show("Lỗi: Bạn chưa chọn đối tượng để xoá.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnCancel_r_Click(object sender, EventArgs e)
        {
            ResetMailReceiveForm();
        }

        private void dgvListMailReceive_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int row = e.RowIndex;
                int.TryParse(dgvListMailReceive.Rows[row].Cells["IdR"].Value.ToString(), out mailReceiveId);
                txtAddressReceive.Text = dgvListMailReceive.Rows[row].Cells["AddressR"].Value.ToString();
                txtNoteReceive.Text = dgvListMailReceive.Rows[row].Cells["NoteR"].Value.ToString();
                bool isCheck = false;
                bool.TryParse(dgvListMailReceive.Rows[row].Cells["IsActiveR"].Value.ToString(), out isCheck);
                chkIsActiveReceive.Checked = isCheck;
                chkIsActiveReceive.Enabled = true;

                btnAdd_r.Enabled = false;
                btnUpdate_r.Enabled = true;
                btnDelete_r.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        #endregion

        #region Function
        private void LoadListMailReceiveToGridView()
        {
            try
            {
              //  mailReceiveDAO.LoadMailToDataGirdview(dgvListMailReceive);
                dgvListMailReceive.DataSource = BLLMailReceive.GetAll();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool CheckValidate_Re()
        {
            var flag = true;
            try
            {
                if (string.IsNullOrEmpty(txtAddressReceive.Text))
                {
                    MessageBox.Show("Vui lòng nhập địa chỉ Mail Nhận.");
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi : " + ex);
            }
            return flag;
        }

        private void SaveMail_R()
        {
            try
            {
                var mail_R = new MAIL_RECEIVE();
                mail_R.Id = mailReceiveId;
                mail_R.Address = txtAddressReceive.Text;
                mail_R.IsActive = chkIsActiveReceive.Checked;
                mail_R.Note = txtNoteReceive.Text;
                var result = BLLMailReceive.CreateOrUpdate(mail_R);
                MessageBox.Show(result.Messages[0].msg, result.Messages[0].Title);
                if (result.IsSuccess)
                {
                    ResetMailReceiveForm();
                    LoadListMailReceiveToGridView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi :" + ex.Message);
            }
        }

        private void ResetMailReceiveForm()
        {
            mailReceiveId = 0;
            txtAddressReceive.Text = string.Empty;
            chkIsActiveReceive.Checked = false;
            txtNoteReceive.Text = string.Empty;
            btnUpdate_r.Enabled = false;
            btnDelete_r.Enabled = false;
            btnAdd_r.Enabled = true;
        }
        #endregion
        #endregion

        #region  MAILSEND
        #region Event
        private void btnAdd_s_Click(object sender, EventArgs e)
        {
            if (CheckValidate_Send())
                SaveMail_Send();
        }

        private void btnUpdate_s_Click(object sender, EventArgs e)
        {
            if (CheckValidate_Send())
                SaveMail_Send();
        }

        private void btnDelete_s_Click(object sender, EventArgs e)
        {
            try
            {
                if (mailSendId > 0)
                {
                    if (MessageBox.Show("Bạn có muốn xoá?", "Xoá Mail", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        BLLMailSend.Delete(mailSendId);
                        ResetMailSendForm();
                        LoadListMailSendToGridView();
                    }
                }
                else
                    MessageBox.Show("Lỗi: Bạn chưa chọn đối tượng để xoá.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnCancel_s_Click(object sender, EventArgs e)
        {
            ResetMailSendForm();
        }

        private void dgvListMailSend_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int row = e.RowIndex;
                int.TryParse(dgvListMailSend.Rows[row].Cells["Id"].Value.ToString(), out mailSendId);
                txtAddress.Text = dgvListMailSend.Rows[row].Cells["Address"].Value.ToString();
                txtNote.Text = dgvListMailSend.Rows[row].Cells["Note"].Value.ToString();
                cbbMailTypeSend.Text = dgvListMailSend.Rows[row].Cells["MailType"].Value.ToString();
                bool isCheck = false;
                bool.TryParse(dgvListMailSend.Rows[row].Cells["IsActive"].Value.ToString(), out isCheck);
                chkIsActive.Checked = isCheck;
                chkIsActive.Enabled = true;

                btnAdd_s.Enabled = false;
                btnUpdate_s.Enabled = true;
                btnDelete_s.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        #endregion
        #region Function
        private void SaveMail_Send()
        {
            try
            {
                var mail = new MAIL_SEND();
                mail.Id = mailSendId;
                mail.Address = txtAddress.Text;
                mail.IsActive = chkIsActive.Checked;
                mail.PassWord = txtPassword.Text;
                var mailTypeSelect = ((MAIL_TYPE)cbbMailTypeSend.SelectedItem);
                mail.MailTypeId = mailTypeSelect != null ? mailTypeSelect.id : 0;
                mail.Note = txtNote.Text;
                var result = BLLMailSend.CreateOrUpdate(mail);
                MessageBox.Show(result.Messages[0].msg, result.Messages[0].Title);
                if (result.IsSuccess)
                {
                    ResetMailSendForm();
                    LoadListMailSendToGridView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi :" + ex.Message);
            }
        }

        private bool CheckValidate_Send()
        {
            var flag = true;
            try
            {
                if (string.IsNullOrEmpty(txtAddress.Text))
                {
                    MessageBox.Show("Vui lòng nhập địa chỉ mail gửi.");
                    flag = false;
                }
                else if (mailSendId == 0 && string.IsNullOrEmpty(txtPassword.Text))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu.");
                    flag = false;
                }
                else if (!string.IsNullOrEmpty(txtPassword.Text) && string.IsNullOrEmpty(txtRePassword.Text))
                {
                    MessageBox.Show("Vui lòng nhập xác nhận mật khẩu.");
                    flag = false;
                }
                else if (!string.IsNullOrEmpty(txtPassword.Text) && txtPassword.Text.CompareTo(txtRePassword.Text) == -1)
                {
                    MessageBox.Show("Xác nhận mật khẩu không đúng với mật khẩu.");
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi : " + ex);
            }
            return flag;
        }

        private void ResetMailSendForm()
        {
            cbbMailTypeSend.SelectedIndex = 0;
            txtAddress.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtRePassword.Text = string.Empty;
            txtNote.Text = string.Empty;
            chkIsActive.Checked = false;
            btnAdd_s.Enabled = true;
            btnUpdate_s.Enabled = false;
            btnDelete_s.Enabled = false;
            mailSendId = 0;
        }

        private void LoadListMailSendToGridView()
        {
            try
            { 
                dgvListMailSend.DataSource = BLLMailSend.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void LoadDataForCbbMailType()
        {
            try
            {
                var listMailType = BLLMailType.GetAll(); 
                if (listMailType != null && listMailType.Count > 0)
                {
                    cbbMailTypeSend.DataSource = listMailType.ToList();
                    cbbMailTypeSend.DisplayMember = "TypeName";
                    cbbMailTypeSend.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        #endregion
 
    }
}
