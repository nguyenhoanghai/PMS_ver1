using DuAn03_HaiDang.DATAACCESS;
using PMS.Business;
using PMS.Business.Enum;
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
    public partial class frmTiLeDenCat : Form
    {
        int ObjId = 0;
        public frmTiLeDenCat()
        {
            InitializeComponent();
        }

        private void frmTiLeDenCat_Load(object sender, EventArgs e)
        {
            loadPhanCongTyLe();
        }

        private void butDelete_Click(object sender, EventArgs e)
        {
            var item = (LightPercentModel)cbLightPer.SelectedItem;
            if (item.Id != 0)
            {
                if (MessageBox.Show("Bạn có muốn xoá tỷ lệ này không?", "Xoá tỷ lệ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                { 
                    BLLLightPercent.Instance.Delete(item.Id);
                    loadPhanCongTyLe();
                }
            }
        }

        private void butClick(int type)
        {
            switch (type)
            {
                case 1: //add
                case 2: //update
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnAdd.Enabled = false;
                    break;
                case 3: //save
                case 4: //cancel
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnDelete.Enabled = true;
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    break;
            }
        }

        private void loadPhanCongTyLe()
        {
            try
            {
                cbLightPer.DataSource = null;
                var objs = BLLLightPercent.Instance.GetAll((int)eLightType.BTPConLai);

                cbLightPer.DataSource = objs;
                cbLightPer.DisplayMember = "Name";
                cbLightPer.ValueMember = "Id";
                cbLightPer.SelectedIndex = 0;
                cbLightPer.Refresh();

                repCBTiLe.DataSource = null;
                repCBTiLe.DataSource = objs;
                repCBTiLe.DisplayMember = "Name";
                repCBTiLe.ValueMember = "Id";
                repCBTiLe.PopulateViewColumns();
                repCBTiLe.View.Columns[0].Caption = "Id";
                repCBTiLe.View.Columns[0].Visible = false;
                repCBTiLe.View.Columns[2].Visible = false;
                repCBTiLe.View.Columns[3].Visible = false;
                repCBTiLe.View.Columns[4].Visible = false;
                repCBTiLe.View.Columns[5].Visible = false;
                repCBTiLe.View.Columns[6].Visible = false;
                repCBTiLe.View.Columns[7].Visible = false;
                repCBTiLe.View.Columns[1].Caption = "Tỉ lệ";

                loadDSTyLe();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void loadDSTyLe()
        {
            try
            {
                gridLine.DataSource = null;
                var listChuyen = BLLLightPercent.Instance.GetPhanCongBTPConLai(AccountSuccess.strListChuyenId.Split(',').Select(x => Convert.ToInt32(x)).ToArray());
                gridLine.DataSource = listChuyen;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnsavePC_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridView1.RowCount > 0)
                {
                    var list = new List<ModelSelectItem>();
                    for (var i = 0; i < gridView1.RowCount; i++)
                    {
                        var row = gridView1.GetRow(gridView1.GetRowHandle(i));
                        list.Add((ModelSelectItem)row);
                    }
                    if (BLLLightPercent.Instance.SavePhanCongTiLeBTPConLai(list))
                        MessageBox.Show("Lưu thông tin thành công.");
                    else
                        MessageBox.Show("Lưu thông tin thất bại.");
                    loadDSTyLe();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            loadDSTyLe();
        }

        private void cbLightPer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbLightPer.DataSource != null)
            {
                var item = (LightPercentModel)cbLightPer.SelectedItem;
                BindData(item.Childs);
            }
        }

        private void BindData(List<LightPercentDetailModel> list)
        {
            try
            {
                gridDe.DataSource = null;
                gridDe.DataSource = list;
            }
            catch (Exception)
            {
            }
        }

        private void btnedit_Click(object sender, EventArgs e)
        {
            var item = (LightPercentModel)cbLightPer.SelectedItem;
            if (item.Id != 0)
            {
                ObjId = item.Id;
                cbLightPer.Text = item.Name;
                gridViewDe.OptionsBehavior.Editable = true;
                butClick(2);
            }
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            butClick(1);
            ObjId = 0;
            cbLightPer.Text = "";
            var source = new List<P_LightPercent_De>();
            source.Add(new P_LightPercent_De() { Id = 0, ColorName = "Đỏ", From = 0, To = 0 });
            source.Add(new P_LightPercent_De() { Id = 0, ColorName = "Vàng", From = 0, To = 0 });
            source.Add(new P_LightPercent_De() { Id = 0, ColorName = "Xanh", From = 0, To = 0 });
            gridDe.DataSource = null;
            gridDe.DataSource = source;
            gridViewDe.OptionsBehavior.Editable = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                LightPercentModel obj = new LightPercentModel();
                obj.Id = ObjId;
                obj.Name = cbLightPer.Text;
                obj.Type = (int)eLightType.BTPConLai;
                for (var i = 0; i < gridViewDe.RowCount; i++)
                {
                    var row = gridViewDe.GetRow(gridViewDe.GetRowHandle(i));
                    obj.Childs.Add((LightPercentDetailModel)row);
                }
                if (BLLLightPercent.Instance.InsertOrUpdate(obj).IsSuccess)
                {
                    MessageBox.Show("Lưu thông tin thành công.");
                    loadPhanCongTyLe();
                    gridViewDe.OptionsBehavior.Editable = false;
                    butClick(3);
                }
                else
                    MessageBox.Show("Lưu thông tin thất bại.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            butClick(4);
            loadPhanCongTyLe();
        }

    }
}
