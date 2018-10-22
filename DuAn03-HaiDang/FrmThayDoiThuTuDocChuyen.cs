using DuAn03_HaiDang.KeyPad_Chuyen.dao;
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
    public partial class FrmThayDoiThuTuDocChuyen : Form
    {
        private ChuyenDAO chuyenDAO;
        private int idChuyen;
        private string tenChuyen;
        private int thuTuDoc;
        public delegate void SEND();
        public SEND sender;
        public FrmThayDoiThuTuDocChuyen(int _idChuyen, string _tenChuyen, int _thuTuDoc)
        {
            InitializeComponent();
            chuyenDAO = new ChuyenDAO();
            this.idChuyen = _idChuyen;
            this.tenChuyen = _tenChuyen;
            this.thuTuDoc = _thuTuDoc;
            lblTenChuyen.Text = tenChuyen;
            txtThuTuDoc.Text = thuTuDoc.ToString();
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
                    int intThuTuDoc = 0;
                    if(!string.IsNullOrEmpty(txtThuTuDoc.Text))
                        int.TryParse(txtThuTuDoc.Text, out intThuTuDoc);
                    var result = chuyenDAO.UpdateThuTuDocAmThanh(idChuyen, intThuTuDoc);
                    if (result)
                    {
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

    }
}
