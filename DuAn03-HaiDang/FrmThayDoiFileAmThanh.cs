using DuAn03_HaiDang.KeyPad_Chuyen.dao;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DuAn03_HaiDang
{
    public partial class FrmThayDoiFileAmThanh : Form
    {
        private ChuyenDAO chuyenDAO;
        private int idChuyen;
        private string tenChuyen;
        private string fileAmThanh;
        private OpenFileDialog dlg=null;
        public delegate void SEND();
        public SEND sender;
        public FrmThayDoiFileAmThanh(int _idChuyen, string _tenChuyen, string _fileAmThanh)
        {
            InitializeComponent();
            chuyenDAO = new ChuyenDAO();
            this.idChuyen = _idChuyen;
            this.tenChuyen = _tenChuyen;
            this.fileAmThanh = _fileAmThanh;
            lblTenChuyen.Text = tenChuyen;
            txtFileAmThanh.Text = fileAmThanh;
        }
        
        private void butCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void butSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (idChuyen > 0)
                {
                    var result = chuyenDAO.UpdateFileAmThanh(idChuyen, txtFileAmThanh.Text);
                    if (result)
                    {
                        if (dlg != null)
                        {
                            string[] tmp = dlg.FileNames;
                            foreach (string i in tmp)
                            {
                                FileInfo fi = new FileInfo(i);
                                string des = Application.StartupPath + @"\Sound\" + txtFileAmThanh.Text;
                                File.Delete(des);
                                fi.CopyTo(des);
                                dlg = null;
                                break;
                            }
                        }
                        MessageBox.Show("Lưu thông tin thành công.");
                        this.sender();
                        this.Close();
                    }
                    else
                        MessageBox.Show("Lưu thông tin thất bại.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void txtFileAmThanh_Click(object sender, EventArgs e)
        {
            try
            {
                dlg = new OpenFileDialog();
                dlg.Filter = "file hinh|*.wav|all file|*.*";
                dlg.InitialDirectory = @"C:\";
                dlg.Multiselect = false;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string[] tmp = dlg.FileNames;
                    foreach (string i in tmp)
                    {
                        FileInfo fi = new FileInfo(i);
                        string[] xxx = i.Split('\\');
                        string tenFile = xxx[xxx.Length - 1];
                        txtFileAmThanh.Text = tenFile;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

       
    }
}
