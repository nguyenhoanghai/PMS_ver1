using PMS.Business;
using PMS.Business.Models;
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
    public partial class FrmBTP_HCStruct : Form
    {
        int phaseType = 1;
        public FrmBTP_HCStruct(int _phaseType)
        {
            phaseType = _phaseType;
            InitializeComponent(); 
        }

        private void FrmBTP_HCStruct_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void gridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Id").ToString(), out Id);
                if (Id == 0 && string.IsNullOrEmpty(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Name").ToString()))
                    goto End;
                else if (Id == 0 && string.IsNullOrEmpty(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Index").ToString()))
                    goto End;

                if (Id != 0 && string.IsNullOrEmpty(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Name").ToString()))
                    MessageBox.Show("Vui lòng nhập tên .", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Id != 0 && string.IsNullOrEmpty(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Index").ToString()))
                    MessageBox.Show("Vui lòng nhập số thứ tự.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var obj = new P_Phase();
                    obj.Id = Id;
                    obj.Name = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Name").ToString();
                    obj.Index = int.Parse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Index").ToString());
                    obj.IsShow = bool.Parse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "IsShow").ToString());
                    obj.Note = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Note") != null ? gridView.GetRowCellValue(gridView.FocusedRowHandle, "Note").ToString() : "";
                    obj.Type = phaseType;
                    var kq = BLLBTP_HCStructure.Instance.InsertOrUpdate(obj);
                    if (!kq.IsSuccess)
                    {
                        MessageBox.Show(kq.Messages[0].msg, kq.Messages[0].Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        goto End;
                    }
                    else
                        LoadData();
                }
            }
            catch (Exception ex)
            {
            }
        End: { }
        }

        private void repbtn_deleteCounter_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = int.Parse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                BLLBTP_HCStructure.Instance.Delete(Id);
                LoadData();
            }
        }

        private void LoadData()
        {
            try
            {
                var list = BLLBTP_HCStructure.Instance.Gets(phaseType);
                list.Add(new PhaseModel() { Id = 0, Index = list.Count + 1, Name = "", Note = "" });
                gridControl.DataSource = list;
            }
            catch (Exception)
            {
                MessageBox.Show("Vui lòng kiểm tra lại cấu trúc dữ liệu", "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } 
        }
    }
}
