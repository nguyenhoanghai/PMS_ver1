using DuAn03_HaiDang.DATAACCESS;
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
    public partial class FrmProcessLog : Form
    {
        public FrmProcessLog()
        {
            InitializeComponent();
        }
        private void LoadProcessLog()
        {
            try
            {
                txtLog.Text = AccountSuccess.strError;
            }
            catch (Exception ex)
            {                
                MessageBox.Show("Lỗi:"+ex.Message);
            }
        }

        private void butReload_Click(object sender, EventArgs e)
        {
            LoadProcessLog();
        }

        private void butClear_Click(object sender, EventArgs e)
        {
            AccountSuccess.strError=string.Empty;
        }

        private void butOnOffLog_Click(object sender, EventArgs e)
        {
            try
            {
                if (AccountSuccess.isWriteLog)
                {
                    butOnOffLog.Text = "Write Log (Off)";
                    AccountSuccess.isWriteLog = false;
                }
                else
                {
                    butOnOffLog.Text = "Write Log (On)";
                    AccountSuccess.isWriteLog = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi:" + ex.Message);
            }
        }

        private void FrmProcessLog_Load(object sender, EventArgs e)
        {
            try
            {
                if (AccountSuccess.isWriteLog)
                {
                    butOnOffLog.Text = "Write Log (Off)";                    
                }
                else
                {
                    butOnOffLog.Text = "Write Log (On)";                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi:" + ex.Message);
            }
        }
    }
}
