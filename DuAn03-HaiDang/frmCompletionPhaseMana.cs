using PMS.Business;
using PMS.Data;
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
    public partial class frmCompletionPhaseMana : Form
    {
        private int PId = 0;
        public frmCompletionPhaseMana()
        {
            InitializeComponent();
        }

        private void frmCompletionPhaseMana_Load(object sender, EventArgs e)
        {
            LoadCompletionPhase();
        }

        private void LoadCompletionPhase()
        {
            try
            {
                gridPhase.DataSource = BLLCompletionPhase.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy danh sách công đoạn hoàn thành.\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_g_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void btnUpdate_g_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void btnDelete_g_Click(object sender, EventArgs e)
        {
            try
            {
                if (PId != 0)
                {
                    if (MessageBox.Show("Bạn có muốn xoá không?", "Xoá đối tượng", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var result = BLLCompletionPhase.Delete(PId);
                        if (result.IsSuccess)
                        {
                            LoadCompletionPhase();
                            ResetForm();
                        }
                        else
                            MessageBox.Show(result.Messages[0].msg, result.Messages[0].Title);
                    }
                }
                else
                    MessageBox.Show("Bạn chưa chọn đối tượng để xoá");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnCancel_g_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void gridView_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                int.TryParse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Id").ToString(), out PId);
                var str = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Code");
                txtCode.Text = str != null ? str.ToString() : string.Empty;
                txtName.Text = (string)gridView.GetRowCellValue(gridView.FocusedRowHandle, "Name");
                txtNote.Text = (string)gridView.GetRowCellValue(gridView.FocusedRowHandle, "Note");
                cbShow.Checked = (Boolean)gridView.GetRowCellValue(gridView.FocusedRowHandle, "IsShow");
                txtOrderIndex.Value = (int)gridView.GetRowCellValue(gridView.FocusedRowHandle, "OrderIndex");
                btnAdd_g.Enabled = false;
                btnDelete_g.Enabled = true;
                btnUpdate_g.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi exception gridView_RowCellClick: " + ex.Message);
            }
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(txtName.Text))
                MessageBox.Show("Vui lòng nhập tên công đoạn.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                var obj = new P_CompletionPhase();
                obj.Id = PId;
                obj.OrderIndex = (int)txtOrderIndex.Value;
                obj.Code = txtCode.Text;
                obj.Name = txtName.Text;
                obj.Note = txtNote.Text;
                obj.IsShow = cbShow.Checked;
                obj.CreatedDate = DateTime.Now;
                var rs = BLLCompletionPhase.InsertOrUpdate(obj);
                if (rs.IsSuccess)
                {
                    //  MessageBox.Show(rs.Messages[0].msg, rs.Messages[0].Title, MessageBoxButtons.OK, MessageBoxIcon.None);
                    LoadCompletionPhase();
                    ResetForm();
                }
                else
                    MessageBox.Show(rs.Messages[0].msg, rs.Messages[0].Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetForm()
        {
            PId = 0;
            txtOrderIndex.Value = 0;
            txtCode.Text = string.Empty;
            txtName.Text = string.Empty;
            txtNote.Text = string.Empty;
            cbShow.Checked = false;
            btnAdd_g.Enabled = true;
            btnUpdate_g.Enabled = false;
            btnDelete_g.Enabled = false;
        }
    }
}
