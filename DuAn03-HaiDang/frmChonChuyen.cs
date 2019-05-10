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

namespace QuanLyNangSuat
{
    public partial class frmChonChuyen : Form
    {
        int lineId = 0;
        public frmChonChuyen(int _lineId)
        {
            InitializeComponent();
            lineId = _lineId;
        }

        private void frmChonChuyen_Load(object sender, EventArgs e)
        {
            GetCBLine();
        }

        private void GetCBLine()
        {
            cbLine.DataSource = null;
            cbLine.DataSource = BLLSound.GetLinesHaveReadSoundConfig();
            cbLine.ValueMember = "Data";
            cbLine.DisplayMember = "Name";
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void butCopy_Click(object sender, EventArgs e)
        {
            var model = (ModelSelectItem)cbLine.SelectedItem;
            if (model != null)
                BLLSound.CopyReadSoundConfig(lineId, model.Data);
            else
                MessageBox.Show("Vui lòng chọn cấu hình của chuyền cần sao chép.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
