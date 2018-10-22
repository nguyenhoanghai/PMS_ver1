using DuAn03_HaiDang;
using DuAn03_HaiDang.DATAACCESS;
using PMS.Business;
using PMS.Business.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QuanLyNangSuat
{
    public partial class frmMainShow : Form
    {
        public static SqlConnection sqlCon = new SqlConnection();
        public frmMainShow()
        {
            InitializeComponent();
            ConnectDatabase();
        }

        private void barLargeButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var appId = 0;
            int.TryParse(ConfigurationManager.AppSettings["AppId"].ToString(), out appId);
            var configs = BLLConfig.Instance.GetAll(appId);
            var idTable = configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.TABLE)).Value.Trim();
            if (idTable == "1")
            {
                var form = new FrmHienThiLCD();
                form.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                form.Show();
                //   AccountSuccess.ListFormLCD.Add(form);
            }
            else
            {
                var frm = new FrmHienThiLCDKANBAN();
                frm.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                frm.Show();
                //  AccountSuccess.ListFormLCD.Add(frm);
            }
            //if (MessageBox.Show("Bạn có muốn ẩn màn mình chính của chương trình?", "Ẩn màn hình chính", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //    this.Hide();
            //else
            //    this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
        }

        private void barLargeButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmHienThiLCDTongHop form = new FrmHienThiLCDTongHop(sqlCon);
            form.Show();
        }
        private static void ConnectDatabase()
        {
            string strConnectionString = dbclass.GetConnectionString();
            try
            {
                sqlCon = new SqlConnection(strConnectionString);
                sqlCon.Open();
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể kết nối với CSDL, Vui lòng thử cấu hình lại kết nối", "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FrmConnectDatabase form = new FrmConnectDatabase();
                form.Show();
            }
        }

        private void btnNS_Click(object sender, EventArgs e)
        {
            try
            {
                var appId = 0;
                int.TryParse(ConfigurationManager.AppSettings["AppId"].ToString(), out appId);
                var configs = BLLConfig.Instance.GetAll(appId);
                var idTable = configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.TABLE)).Value.Trim();
                if (idTable == "1")
                {
                    var form = new FrmHienThiLCD();
                    form.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                    form.Show();
                    //   AccountSuccess.ListFormLCD.Add(form);
                }
                else
                {
                    var frm = new FrmHienThiLCDKANBAN();
                    frm.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                    frm.Show();
                    //  AccountSuccess.ListFormLCD.Add(frm);
                }
                //if (MessageBox.Show("Bạn có muốn ẩn màn mình chính của chương trình?", "Ẩn màn hình chính", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //    this.Hide();
                //else
                //    this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            }
            catch (Exception)
            {            }
        }

        private void btnCollec_Click(object sender, EventArgs e)
        {
            FrmHienThiLCDTongHop form = new FrmHienThiLCDTongHop(sqlCon);
            form.Show();
        }
    }
}
