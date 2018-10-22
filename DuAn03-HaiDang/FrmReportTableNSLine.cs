using DuAn03_HaiDang.KeyPad_Chuyen.dao;
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
    public partial class FrmReportTableNSLine : Form
    {
        private ChuyenDAO chuyenDAO;
        public FrmReportTableNSLine()
        {
            InitializeComponent();
            this.chuyenDAO = new ChuyenDAO();
        }

        private void butView_Click(object sender, EventArgs e)
        {

        }

        private void GetDataNSLine()
        {
            try
            {

            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        private void FrmReportTableNSLine_Load(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: "+ex.Message, "Lỗi xử ");
            }
        }
    }
}
