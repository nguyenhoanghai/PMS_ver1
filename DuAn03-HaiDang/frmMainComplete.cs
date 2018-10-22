using DuAn03_HaiDang;
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
    public partial class frmMainComplete : Form
    {
        public frmMainComplete()
        {
            InitializeComponent();
        }

        private bool ActiveForm(Type type)
        {
            bool result = false;
            foreach (Form fm in MdiChildren)
            {
                if (fm.GetType() == type)
                {
                    fm.Activate();
                    result = true;
                }
            }
            return result;
        }

        private void btnCompletionPhase_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var result = ActiveForm(typeof(frmCompletionPhaseMana));
                if (!result)
                {
                    var f = new frmCompletionPhaseMana();
                  f.MdiParent = this;                
                    f.Show();
                }
            }
            catch (Exception )
            {
            }
        }

        private void frmMainComplete_Load(object sender, EventArgs e)
        {

        }

        private void btnAssignment_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var result = ActiveForm(typeof(FrmAssignCompletion));
                if (!result)
                {
                    var f = new FrmAssignCompletion();
                    f.MdiParent = this;
                    f.Show();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btnInsertQuality_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var result = ActiveForm(typeof(FrmInsertQualityCompletion));
                if (!result)
                {
                    var f = new FrmInsertQualityCompletion();
                    f.MdiParent = this;
                    f.Show();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                Application.Exit();
                //AccountSuccess.TenTK = "(none)";
                //AccountSuccess.BTP = 0;
                //AccountSuccess.TenChuTK = null;
                //AccountSuccess.ThanhPham = 0;
                //AccountSuccess.IdFloor = string.Empty;
                //AccountSuccess.IsAll = 0;
                //this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void barLargeButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barLargeButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var result = ActiveForm(typeof(FrmProduct_N));
                if (!result)
                {
                    var f = new FrmProduct_N();
                    f.MdiParent = this;
                    f.Show();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void frmMainComplete_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
