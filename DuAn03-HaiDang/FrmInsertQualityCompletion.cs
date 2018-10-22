using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PMS.Business;
using PMS.Business.Models;
using PMS.Data;
using PMS.Business.Enum;

namespace QuanLyNangSuat
{
    public partial class FrmInsertQualityCompletion : Form
    {
        string date = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
        public FrmInsertQualityCompletion()
        {
            InitializeComponent();
        }

        private void FrmInsertQualityCompletion_Load(object sender, EventArgs e)
        {
            LoadCommodities();
            GetPhases();
        }

        private void GetPhases()
        {
            cbPhase.DataSource = BLLCompletionPhase.GetAll();
            cbPhase.DisplayMember = "Name";
            cbPhase.ValueMember = "Id";
        }

        private void LoadCommodities()
        {
            cboSanPham_0.DataSource = BLLAssignCompletion.GetAll(true);
            cboSanPham_0.DisplayMember = "CommoName";
        }

        private void cboSanPham_0_SelectedIndexChanged(object sender, EventArgs e)
        {
            AssignCompletionModel sp = (AssignCompletionModel)cboSanPham_0.SelectedItem;
            if (sp != null)
            {
                lblSanLuongKeHoach.Text = sp.ProductionsPlan.ToString();
                GetDataForGridView(sp);
            }
        }

        private void GetDataForGridView(AssignCompletionModel sp)
        {
            var data = BLLInsertQuality.GetDetailInDay(date, sp.CommoId);
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    item.Time = item.CreatedDate.ToString("HH:mm:ss");
                }
                gridControl1.DataSource = data;
            }
            else
                gridControl1.DataSource = null;
        }

        private void btnAdd_s_Click(object sender, EventArgs e)
        {
            AssignCompletionModel sp = (AssignCompletionModel)cboSanPham_0.SelectedItem;
            P_CompletionPhase phase = (P_CompletionPhase)cbPhase.SelectedItem;
            var obj = new P_CompletionPhase_Daily();
            obj.AssignId = sp.Id;
            obj.CommandTypeId = radioGroup1.SelectedIndex == 0 ? (int)eCommandRecive.ProductIncrease : (int)eCommandRecive.ProductReduce;
            obj.Date = date;
            obj.CompletionPhaseId = phase.Id;
            obj.CreatedDate = DateTime.Now;
            obj.Quantity = (int)txtsl.Value;
            var rs = BLLInsertQuality.Insert(obj);
            if (rs.IsSuccess)
            {
                GetDataForGridView(sp);
                ResetForm();
            }
            else
                MessageBox.Show(rs.Messages[0].msg, rs.Messages[0].Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ResetForm()
        {
            btnAdd_s.Enabled = true;
            btnUpdate_s.Enabled = false;
            btnDelete_s.Enabled = false;
            txtsl.Value = 0;
        }

        private void btnUpdate_s_Click(object sender, EventArgs e)
        {

        }

        private void btnRefreshAssign_Click(object sender, EventArgs e)
        {
            LoadCommodities();
        }

        private void btnRefreshPhase_Click(object sender, EventArgs e)
        {
            GetPhases();
        }


    }
}
