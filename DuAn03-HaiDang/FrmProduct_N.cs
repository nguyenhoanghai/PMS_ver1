using DevExpress.XtraEditors.Repository;
using DuAn03_HaiDang.DATAACCESS;
using PMS.Business;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QuanLyNangSuat
{
    public partial class FrmProduct_N : Form
    {
        private int ProId = 0;
        private int floorDefault = 0;
        public FrmProduct_N()
        {
            InitializeComponent();
        }

        private void FrmProduct_N_Load(object sender, EventArgs e)
        {
            GetFloor();
            LoadProduct_Grid();
        }

        private void GetFloor()
        {
            var listFloor = BLLFloor.GetFloorForComBoBox();
            cbFloor.DataSource = listFloor.SelectList;
            cbFloor.DisplayMember = "Name";
            cbFloor.ValueMember = "IdFloor";
            cbFloor.SelectedValue = listFloor.DefaultValue;
            floorDefault = listFloor.DefaultValue;
        }

        private void LoadProduct_Grid()
        {
            try
            {
                gridProduct.DataSource = null;
                var item = (Floor)cbFloor.SelectedItem;

                if (item != null && item.IdFloor != 0)
                {

                    var pro = new List<SanPham>();
                    pro.Add(new SanPham() { MaSanPham = 0, TenSanPham = "" });
                    pro.AddRange(BLLCommodity.GetAll(item.IdFloor, AccountSuccess.IsAll));

                    gridProduct.DataSource = pro;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy danh sách mã hàng.\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "MaSanPham").ToString(), out Id);
                if (Id != 0)
                    SaveSanPham(); 
            }
            catch (Exception ex)
            {
            }
        End:
            {

            }
        }

        private void repbtnDelete_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int Id = int.Parse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "MaSanPham").ToString());
            if (Id != 0)
            {
                if (MessageBox.Show("Bạn có muốn xoá không?", "Xoá đối tượng", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var result = BLLCommodity.Delete(Id);
                    if (result.IsSuccess)
                        LoadProduct_Grid();
                    else
                        MessageBox.Show(result.Messages[0].msg, result.Messages[0].Title);
                }
            }
        }

        private void gridView_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //if (e.Column.Caption == "")
            //{
            //    RepositoryItemButtonEdit ritem = new RepositoryItemButtonEdit();
            //    ritem.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            //    ritem.ReadOnly = true;
            //    ritem.Buttons[0].Enabled = true;
            //    e.RepositoryItem = ritem;
            //}

            if (Convert.ToInt32(gridView.GetRowCellValue(e.RowHandle, "MaSanPham")) == 0 && e.Column.Caption == "")
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


        void ritem_Click(object sender, EventArgs e)
        {
            SaveSanPham();
        }

        private void SaveSanPham()
        {
            int Id = 0;
            int.TryParse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "MaSanPham").ToString(), out Id);
            if (string.IsNullOrEmpty(gridView.GetRowCellValue(gridView.FocusedRowHandle, "TenSanPham").ToString()))
                MessageBox.Show("Vui lòng nhập tên sản phẫm.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (string.IsNullOrEmpty(gridView.GetRowCellValue(gridView.FocusedRowHandle, "DonGia").ToString()))
                MessageBox.Show("Vui lòng nhập đơn giá.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (string.IsNullOrEmpty(gridView.GetRowCellValue(gridView.FocusedRowHandle, "DonGiaCM").ToString()))
                MessageBox.Show("Vui lòng nhập đơn giá CM.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (string.IsNullOrEmpty(gridView.GetRowCellValue(gridView.FocusedRowHandle, "ProductionTime").ToString()))
                MessageBox.Show("Vui lòng thời gian chế tạo sản phẫm.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (string.IsNullOrEmpty(gridView.GetRowCellValue(gridView.FocusedRowHandle, "ProductionTime").ToString()) &&
                              Convert.ToDouble(gridView.GetRowCellValue(gridView.FocusedRowHandle, "ProductionTime").ToString()) <= 0)
                MessageBox.Show("Thời gian chế tạo Mặt Hàng phải lớn hơn 0, hoặc bạn nhập sai định dạng dữ liệu.\n", "Lỗi nhập liệu");
            else
            {
                var obj = new SanPham();
                obj.MaSanPham = Id;
                obj.Floor = floorDefault;
                obj.TenSanPham = gridView.GetRowCellValue(gridView.FocusedRowHandle, "TenSanPham").ToString();
                obj.DonGia = Convert.ToDouble(gridView.GetRowCellValue(gridView.FocusedRowHandle, "DonGia").ToString());
                obj.DonGiaCM = Convert.ToDouble(gridView.GetRowCellValue(gridView.FocusedRowHandle, "DonGiaCM").ToString());
                obj.ProductionTime = Convert.ToDouble(gridView.GetRowCellValue(gridView.FocusedRowHandle, "ProductionTime").ToString());
                obj.DinhNghia = gridView.GetRowCellValue(gridView.FocusedRowHandle, "DinhNghia") != null ? gridView.GetRowCellValue(gridView.FocusedRowHandle, "DinhNghia").ToString() : "";
                if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "MaKhachHang") != null)
                    obj.MaKhachHang = gridView.GetRowCellValue(gridView.FocusedRowHandle, "MaKhachHang").ToString();

                if (gridView.GetRowCellValue(gridView.FocusedRowHandle, "DonGiaCat") != null)
                    obj.DonGiaCat = Convert.ToDouble(gridView.GetRowCellValue(gridView.FocusedRowHandle, "DonGiaCat").ToString());

                var rs = BLLCommodity.InsertOrUpdate(obj);
                if (rs.IsSuccess)
                {
                    LoadProduct_Grid();
                }
                else
                    MessageBox.Show(rs.Messages[0].msg, rs.Messages[0].Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        int i = 0;
        private void gridView_RowLoaded(object sender, DevExpress.XtraGrid.Views.Base.RowEventArgs e)
        {
            //if (e.Column.Caption == "")
            //{
            //    RepositoryItemButtonEdit ritem = new RepositoryItemButtonEdit();
            //    ritem.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            //    ritem.ReadOnly = true;
            //    ritem.Buttons[0].Enabled = true;
            //    e.RepositoryItem = ritem;
            //}
        }


    }
}
