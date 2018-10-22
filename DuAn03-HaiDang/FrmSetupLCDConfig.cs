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
    public partial class FrmSetupLCDConfig : Form
    {
        public FrmSetupLCDConfig()
        {
            InitializeComponent();
        }

        private void FrmSetupLCDConfig_Load(object sender, EventArgs e)
        {
            cbbLCDType.SelectedIndex = 0;
        }

        private void butEdit_Click(object sender, EventArgs e)
        {
            try
            {
                int tableType = 1;
                tableType = cbbLCDType.SelectedIndex + 1;
                FrmLCDConfig f = new FrmLCDConfig(tableType);
                f.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: "+ex.Message);
            }
        }
    }
}
