using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DuAn03_HaiDang.DAO;
using DuAn03_HaiDang.DATAACCESS;

namespace DuAn03_HaiDang
{
    public partial class frmChangePassWord : Form
    {
        TaiKhoanDAO taikhoanDAO = new TaiKhoanDAO();
        public frmChangePassWord()
        {
            InitializeComponent();
        }

        private void btnThayDoiMK_Click(object sender, EventArgs e)
        {
            if (txtMKMoi.Text != "")
            {
                if (txtNhapLaiMKMoi.Text != "")
                {
                    if (txtMKMoi.Text == txtNhapLaiMKMoi.Text)
                    {
                        try
                        {
                            taikhoanDAO.ThayDoiMatKhau(AccountSuccess.TenTK, txtMKMoi.Text);
                            MessageBox.Show("Thay đổi mật khẩu thành công", "Thay đổi thành công", MessageBoxButtons.OK, MessageBoxIcon.None);
                            this.Close();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Lỗi: truy vấn dữ liệu", "Lỗi truy vấn", MessageBoxButtons.OK, MessageBoxIcon.Error);                            
                        }
                        
                    }
                    else
                    {
                        MessageBox.Show("Việc xác nhận lại mật khẩu không đúng, Vui lòng thử lại.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Bạn chưa xác nhận lại mật khẩu ở ô Nhập Lại Mật Khẩu.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("Mật khẩu không được để trống hay có giá trin NULL, Vui lòng nhập mật khẩu mới", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
