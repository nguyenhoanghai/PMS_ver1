using DevExpress.XtraGrid.Views.Grid;
using DuAn03_HaiDang;
using DuAn03_HaiDang.DATAACCESS;
using PMS.Business;
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
    public partial class FrmDayInfo_View : Form
    {
        FrmMainNew frmMain;
        public FrmDayInfo_View(FrmMainNew _frmMain)
        {
            InitializeComponent();
            frmMain = _frmMain;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
             GetNew();
        }

        private void GetNew()
        {
            try
            {
                var data = BLLProductivity.GetProductivitiesInDay(AccountSuccess.strListChuyenId.Split(',').Select(x => Convert.ToInt32(x)).ToList(), frmMain.appId);
                gridControl1.DataSource = data;
            }
            catch (Exception )
            {
                //  frmMain.GhiFileLog(DateTime.Now + "loi GetNew form dayinfo_view ex :" + ex.Message + " \n");
            }
        }

        private void Frm1_Load(object sender, EventArgs e)
        {
            timer1.Interval = frmMain.TimeRefreshFromDayInfoView;
            timer2.Interval = frmMain.TimeCloseFromDayInfoViewIfNotUse;
            timer2.Enabled = false;
            GetNew();
        }

        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0)
                {
                    var a = (Boolean)gridView1.GetRowCellValue(e.RowHandle, "IsFinish");
                    if (a)
                    {
                        e.Appearance.BackColor = Color.Red;
                        e.Appearance.ForeColor = Color.White;
                    }
                }
            }
            catch (Exception ex)
            {
                //  frmMain.GhiFileLog(DateTime.Now + "loi gridView1_RowStyle form dayinfo_view ex :" + ex.Message + " \n");
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridControl1_MouseHover(object sender, EventArgs e)
        {
            timer2.Enabled = false;
        }

        private void gridControl1_MouseLeave(object sender, EventArgs e)
        {
            timer2.Enabled = true;
        }
    }
}
