using DevExpress.XtraEditors.Repository;
using PMS.Business;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QuanLyNangSuat
{
    public partial class frmDepartmentDailyLabours : Form
    {
        public frmDepartmentDailyLabours()
        {
            InitializeComponent();
        }

        private void frmDepartmentDailyLabours_Load(object sender, EventArgs e)
        {
            GetDepartments();
            LoadGrid();
        }

        private void GetDepartments()
        {
            repLKDepartment.DataSource = null;
            repLKDepartment.DataSource = BLLDepartment.Instance.Gets();
            repLKDepartment.DisplayMember = "Name";
            repLKDepartment.ValueMember = "Id";
            repLKDepartment.PopulateViewColumns();
            repLKDepartment.View.Columns[0].Visible = false;
            repLKDepartment.View.Columns[2].Visible = false;
            repLKDepartment.View.Columns[3].Visible = false;
            repLKDepartment.View.Columns[4].Visible = false;
            repLKDepartment.View.Columns[1].Caption = "Bộ phận";
        }

        private void LoadGrid()
        {
            try
            {
                gridProduct.DataSource = null;
                var pro = new List<P_DepartmentDailyLabour>();
                pro.Add(new P_DepartmentDailyLabour() { Id = 0, LDCurrent = 0, LDNew = 0, LDOff = 0, LDPregnant = 0, LDVacation = 0 });
                pro.AddRange(BLLDepartmentDailyLabour.Instance.Gets(dtDate.Value.ToString("dd/MM/yyyy")));
                gridProduct.DataSource = pro;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy danh sách mã hàng.\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridView_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (Convert.ToInt32(gridView.GetRowCellValue(e.RowHandle, "Id")) == 0 && e.Column.Caption == "")
            {
                //  e.RepositoryItem.Appearance.Image = 
                RepositoryItemButtonEdit ritem = new RepositoryItemButtonEdit();
                ritem.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
                ritem.ReadOnly = true;
                ritem.Buttons[0].Image = global::QuanLyNangSuat.Properties.Resources.if_plus_sign_173078;
                ritem.Buttons[0].ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
                ritem.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
                ritem.Buttons[0].Enabled = true;

                ritem.Click += ritem_Click;
                e.RepositoryItem = ritem;
            }
        }

        private void ritem_Click(object sender, EventArgs e)
        {
            Save ();
        }

        private void Save()
        {
            int Id = 0;
            int.TryParse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Id").ToString(), out Id);
            if (string.IsNullOrEmpty(gridView.GetRowCellValue(gridView.FocusedRowHandle, "DepartmentId").ToString()))
                MessageBox.Show("Vui lòng chọn bộ phận.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (string.IsNullOrEmpty(gridView.GetRowCellValue(gridView.FocusedRowHandle, "LDCurrent").ToString()) &&
                              Convert.ToDouble(gridView.GetRowCellValue(gridView.FocusedRowHandle, "LDCurrent").ToString()) <= 0)
                MessageBox.Show("lao động định biên phải lớn hơn 0, hoặc bạn nhập sai định dạng dữ liệu.\n", "Lỗi nhập liệu");
            else
            {
                var obj = new P_DepartmentDailyLabour();
                obj.Id = Id; 
                obj.Date = dtDate.Value.ToString("dd/MM/yyyy");
                obj.CreatedAt = dtDate.Value ;
                obj.DepartmentId = Convert.ToInt32(gridView.GetRowCellValue(gridView.FocusedRowHandle, "DepartmentId").ToString());
                obj.LDCurrent = Convert.ToInt32(gridView.GetRowCellValue(gridView.FocusedRowHandle, "LDCurrent").ToString());
                obj.LDNew = Convert.ToInt32(gridView.GetRowCellValue(gridView.FocusedRowHandle, "LDNew").ToString());
                obj.LDOff = Convert.ToInt32(gridView.GetRowCellValue(gridView.FocusedRowHandle, "LDOff").ToString());
                obj.LDPregnant = Convert.ToInt32(gridView.GetRowCellValue(gridView.FocusedRowHandle, "LDPregnant").ToString());
                obj.LDVacation = Convert.ToInt32(gridView.GetRowCellValue(gridView.FocusedRowHandle, "LDVacation").ToString());

                var rs = BLLDepartmentDailyLabour.Instance.InsertOrUpdate(obj);
                if (rs.IsSuccess)
                {
                    LoadGrid();
                }
                else
                    MessageBox.Show(rs.Messages[0].msg, rs.Messages[0].Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Id").ToString(), out Id);
                if (Id != 0)
                    Save();
            }
            catch (Exception ex)
            {
            }
        End:
            {

            }
        }

        private void repbtnDelete_Click(object sender, EventArgs e)
        {
            int Id = int.Parse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                if (MessageBox.Show("Bạn có muốn xoá không?", "Xoá đối tượng", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var result = BLLDepartmentDailyLabour.Instance.Delete(Id);
                    if (result.IsSuccess)
                        LoadGrid();
                    else
                        MessageBox.Show(result.Messages[0].msg, result.Messages[0].Title);
                }
            }
        }

        private void dtDate_ValueChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }
    }
}
