using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DuAn03_HaiDang.POJO;
using DuAn03_HaiDang.DAO;
using DuAn03_HaiDang.DATAACCESS;
using System.Configuration;
using PMS.Business;
using PMS.Data;

namespace DuAn03_HaiDang
{
    public partial class frmDangNhap : FormBase
    {
        TaiKhoanDAO taikhoanDAO = new TaiKhoanDAO();
        public frmDangNhap()
        {
            InitializeComponent();
        }
        private void testUser()
        {
            string strUser = txtTaiKhoan.Text;
            string strPass = txtMatKhau.Text;
            if (strUser != "" && strPass != "")
            {
                try
                {
                    var result = BLLAccount.FindAccount(strUser, strPass);
                    if (result.IsSuccess)
                    {
                        var user = (PMS.Data.TaiKhoan)result.Data;
                        AccountSuccess.TenTK = user.UserName;
                        AccountSuccess.TenChuTK = user.Name;
                        AccountSuccess.ThanhPham = user.ThanhPham;
                        AccountSuccess.BTP = user.BTP;
                        AccountSuccess.ThaoTac = user.ThaoTac ?? 0;
                        AccountSuccess.IdFloor = (user.Floor ?? 0).ToString();
                        AccountSuccess.IsOwner = user.IsOnwer;
                        AccountSuccess.IsCompleteAcc = user.IsCompleteAcc;
                        if (!string.IsNullOrEmpty(user.ListChuyenId))
                        {
                            AccountSuccess.strListChuyenId = user.ListChuyenId;
                            AccountSuccess.listChuyenId = user.ListChuyenId.Split(',').ToList();
                        }
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(result.Messages[0].msg, result.Messages[0].Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtTaiKhoan.Text = "";
                        txtTaiKhoan.Focus();
                        txtMatKhau.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("finish" + ex.Message);
                }

            }
            else
                MessageBox.Show("Tên tài khoản và mật khẩu không được rỗng", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            testUser();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHienMatKhau.Checked)
            {
                txtMatKhau.UseSystemPasswordChar = false;
            }
            else
            {
                txtMatKhau.UseSystemPasswordChar = true;
            }
        }

        private void frmDangNhap_Load(object sender, EventArgs e)
        {
            txtTaiKhoan.Focus();
        }

        private void txtMatKhau_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                testUser();
        }

        private void txtTaiKhoan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                testUser();
        }

        private void txtMatKhau_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                testUser();
        }

        private void frmDangNhap_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
