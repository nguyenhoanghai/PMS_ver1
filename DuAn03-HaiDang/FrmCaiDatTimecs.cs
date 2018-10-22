using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using DuAn03_HaiDang.DATAACCESS;

namespace DuAn03_HaiDang
{
    public partial class FrmCaiDatTimecs : FormBase
    {
        public FrmCaiDatTimecs()
        {
            InitializeComponent();
        }

        private void butThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
        private void FrmCaiDatTimecs_Load(object sender, EventArgs e)
        {
            txtWaitingACK.Value = int.Parse(dbclass.listAppConfig.Where(c => c.Name.Trim().ToUpper().Equals("TIMEOUTACK")).Select(c => c.Value).FirstOrDefault().ToString());
            TimeSpan timecheck = TimeSpan.Parse(dbclass.listAppConfig.Where(c => c.Name.Trim().ToUpper().Equals("TIMECHECK")).Select(c => c.Value).FirstOrDefault().ToString());
            timeEditTimeCheck.EditValue = (DateTime.Now.Date.AddSeconds(timecheck.TotalSeconds));
        }

        private void butLuu_Click_1(object sender, EventArgs e)
        {
            Configuration _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            DateTime time = DateTime.Parse(timeEditTimeCheck.EditValue.ToString());
            _config.AppSettings.Settings["TimeCheck"].Value = time.TimeOfDay.ToString();
            _config.AppSettings.Settings["TimeOutACK"].Value = txtWaitingACK.Value.ToString();            
            _config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            MessageBox.Show("Cài đặt thời gian thành công.", "Cài đặt thành công", MessageBoxButtons.OK, MessageBoxIcon.None);
            this.Close();
        }

        private void butThoat_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        

        
    }
}
