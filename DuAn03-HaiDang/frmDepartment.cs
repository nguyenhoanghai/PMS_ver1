using DevExpress.XtraEditors.Repository;
using PMS.Business;
using PMS.Business.Models;
using PMS.Data;
using QuanLyNangSuat.Model;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QuanLyNangSuat
{
    public partial class frmDepartment : Form
    {
        public frmDepartment()
        {
            InitializeComponent();
        }

        private void frmDepartment_Load(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void LoadGrid()
        {
            try
            {
                gridProduct.DataSource = null;
                var pro = new List<DepartmentModel>();
                pro.Add(new DepartmentModel() {Id= 0, BaseLabours = 0, Name = "" });
                pro.AddRange(BLLDepartment.Instance.Gets());
                gridProduct.DataSource = pro;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy danh sách Bộ phận.\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Id").ToString(), out Id);
                if (Id != 0)
                    Save ();
            }
            catch (Exception ex)
            {
            }
        End:
            {

            }
        }

        private void Save()
        {
            int Id = 0;
            int.TryParse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Id").ToString(), out Id);
            if (string.IsNullOrEmpty(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Name").ToString()))
                MessageBox.Show("Vui lòng nhập tên bộ phận.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            else if (string.IsNullOrEmpty(gridView.GetRowCellValue(gridView.FocusedRowHandle, "BaseLabours").ToString()) &&
                              Convert.ToDouble(gridView.GetRowCellValue(gridView.FocusedRowHandle, "BaseLabours").ToString()) <= 0)
                MessageBox.Show("lao động định biên phải lớn hơn 0, hoặc bạn nhập sai định dạng dữ liệu.\n", "Lỗi nhập liệu");
            else
            {
                var obj = new P_Department();
                obj.Id = Id; 
                obj.Name = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Name").ToString();
                obj.BaseLabours = Convert.ToInt32(gridView.GetRowCellValue(gridView.FocusedRowHandle, "BaseLabours").ToString()); 

                var rs = BLLDepartment.Instance.InsertOrUpdate(obj);
                if (rs.IsSuccess)
                {
                    LoadGrid();
                }
                else
                    MessageBox.Show(rs.Messages[0].msg, rs.Messages[0].Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void repbtnDelete_Click(object sender, EventArgs e)
        {
            int Id = int.Parse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                if (MessageBox.Show("Bạn có muốn xoá không?", "Xoá đối tượng", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var result = BLLDepartment.Instance.Delete(Id);
                    if (result.IsSuccess)
                        LoadGrid();
                    else
                        MessageBox.Show(result.Messages[0].msg, result.Messages[0].Title);
                }
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
            Save();
        }
    }
}
