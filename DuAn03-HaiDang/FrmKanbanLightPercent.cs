using DuAn03_HaiDang.DATAACCESS;
using Microsoft.VisualBasic;
using PMS.Business;
using PMS.Business.Enum;
using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QuanLyNangSuat
{
    public partial class FrmKanbanLightPercent : Form
    {
        int Id = 0;
        string idTable = "";
        int appId = 0;
        List<LightPercentModel> listKCSModel = new List<LightPercentModel>();
        public FrmKanbanLightPercent()
        {

            InitializeComponent();
            int.TryParse(ConfigurationManager.AppSettings["AppId"].ToString(), out appId);
            idTable = BLLConfig.Instance.FindConfigValue(appId, eAppConfigName.TABLE);
        }

        private void FrmKanbanLightPercent_Load(object sender, EventArgs e)
        {
            try
            {
                loadPhanCongTyLe();
                loadDSTyLe();
                dgTyLe.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void loadDSTyLe()
        {
            try
            {
                dgthongtinphantyle.Rows.Clear();
                var listChuyen = BLLReadPercent.Instance.GetAll(AccountSuccess.strListChuyenId.Split(',').Select(x => Convert.ToInt32(x)).ToArray());
                if (listChuyen != null && listChuyen.Count > 0)
                {
                    DataGridViewRow row;
                    for (int i = 0; i < listChuyen.Count; i++)
                    {
                        row = (DataGridViewRow)dgthongtinphantyle.Rows[i].Clone();
                        row.Cells[0].Value = listChuyen[i].Id;
                        row.Cells[1].Value = listChuyen[i].LineId;
                        row.Cells[2].Value = listChuyen[i].LineName;
                        row.Cells[3].Value = listChuyen[i].AssignmentId;
                        row.Cells[4].Value = listChuyen[i].CommoName;
                        row.Cells[5].Value = listChuyen[i].KanbanLightPercentId == null ? "Chọn tỷ lệ đọc thông báo" : listChuyen[i].KanbanLightReadPercentName;
                        // row.Cells[5].Value = listChuyen[i].ReadPercent_KCSInventoryId == null ? "0" : listChuyen[i].ReadPercent_KCSInventoryId.ToString();
                        dgthongtinphantyle.Rows.Add(row);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void loadPhanCongTyLe()
        {
            try
            {
                cbtile.DataSource = null;
                cbtile.Refresh();
                dgTenTyLe.DataSource = null;

                listKCSModel.Clear();
                listKCSModel.AddRange(BLLLightPercent.Instance.GetAll((int)eLightType.KanBan));
                if (listKCSModel.Count > 0)
                {
                    cbtile.DataSource = listKCSModel;
                    cbtile.DisplayMember = "Name";
                    cbtile.ValueMember = "Id";
                    cbtile.SelectedIndex = 0;
                    cbtile.Refresh();
                }
                dgTenTyLe.DataSource = listKCSModel;
                dgTenTyLe.DisplayMember = "Name";
                dgTenTyLe.ValueMember = "Id";
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void dgthongtinphantyle_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            dgTyLe.Enabled = true;
            dgTyLe.Rows.Clear();
            //Insert information 
            DataGridViewRow drow;
            for (int i = 0; i < 3; i++)
            {
                drow = (DataGridViewRow)dgTyLe.Rows[i].Clone();
                drow.Cells[0].Value = "";
                switch (i)
                {
                    case 0: drow.Cells[1].Value = "Đỏ"; break;
                    case 1: drow.Cells[1].Value = "Vàng"; break;
                    case 2: drow.Cells[1].Value = "Xanh"; break;
                }
                drow.Cells[2].Value = "";
                drow.Cells[3].Value = "";
                dgTyLe.Rows.Add(drow);
            }
            //
            cbtile.Enabled = true;
            btnAdd.Enabled = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = true;
            btncancel.Enabled = true;
            //  eventclick = "insert";
            cbtile.Text = "";
            cbtile.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cbtile.Text))
            {
                ResponseBase rs;
                var obj = new LightPercentModel()
                {
                    Id = Id,
                    Name = cbtile.Text,
                    Type  =(int)eLightType.KanBan 
                };
                var listReadPercent = new List<LightPercentDetailModel>();
                if (dgTyLe.Rows.Count > 0)
                {
                    LightPercentDetailModel rp;
                    for (int i = 0; i < dgTyLe.Rows.Count - 1; i++)
                    {
                        rp = new  LightPercentDetailModel();
                        DataGridViewRow row = dgTyLe.Rows[i];
                        if (row != null)
                        {
                            float percentFrom = 0;
                            if (row.Cells["dgPercentFrom"].Value != null)
                                float.TryParse(row.Cells["dgPercentFrom"].Value.ToString(), out percentFrom);
                            rp.From = percentFrom;

                            float percentTo = 0;
                            if (row.Cells["dgPercentTo"].Value != null)
                                float.TryParse(row.Cells["dgPercentTo"].Value.ToString(), out percentTo);
                            rp.To = percentTo;

                            string ColorName = string.Empty;
                            if (row.Cells["lightColor"].Value != null)
                                ColorName = row.Cells["lightColor"].Value.ToString();
                            rp.ColorName = ColorName;
                            listReadPercent.Add(rp);
                        }
                    }
                    obj.Childs.AddRange(listReadPercent);
                }
                if (Id == 0)
                    rs = BLLLightPercent.Instance.InsertOrUpdate(obj);
                else
                    rs = BLLLightPercent.Instance.Update(Id, cbtile.Text, listReadPercent);
                if (rs.IsSuccess)
                {
                    ProcessButton(4);
                    dgTyLe.Enabled = false;
                    loadPhanCongTyLe();
                    loadDSTyLe();
                    Id = 0;
                    MessageBox.Show("Lưu thông tin thành công");
                }
            }
            else
                MessageBox.Show("Tên tỷ lệ không được để trống", "Lỗi thao tác", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void ProcessButton(int type)
        {
            switch (type)
            {
                case 1: //them
                    btnAdd.Enabled = false;
                    btnUpdate.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    break;
                case 2: //update
                    btnAdd.Enabled = false;
                    btnUpdate.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    break;
                case 3: //delete
                    btnAdd.Enabled = false;
                    btnUpdate.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    break;
                case 4: //cancel
                    btnAdd.Enabled = true;
                    btnUpdate.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    break;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                dgTyLe.Enabled = true;
                ProcessButton(2);
                var item = (P_LightPercent)cbtile.SelectedItem;
                Id = item.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (cbtile.DataSource != null)
            {
                if (MessageBox.Show("Bạn có muốn xoá tỷ lệ này không?", "Xoá tỷ lệ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    var item = (LightPercentModel)cbtile.SelectedItem;
                    BLLLightPercent.Instance.Delete(item.Id);
                    loadPhanCongTyLe();
                    loadDSTyLe();
                }
            }
        }

        private void cbtile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbtile.DataSource != null)
            {
                var item = (LightPercentModel)cbtile.SelectedItem;
                Id = item.Id;
                BindData(item.Childs);
            }
        }

        private void BindData(List<LightPercentDetailModel> list)
        {
            try
            {
                dgTyLe.Rows.Clear();
                dgTyLe.Refresh();

                if (list != null && list.Count > 0)
                {
                    DataGridViewRow row;
                    for (int i = 0; i < list.Count; i++)
                    {
                        row = (DataGridViewRow)dgTyLe.Rows[i].Clone();
                        row.Cells[0].Value = list[i].Id;
                        row.Cells[1].Value = list[i].ColorName;
                        row.Cells[2].Value = list[i].From;
                        row.Cells[3].Value = list[i].To; 
                        dgTyLe.Rows.Add(row);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            ProcessButton(4);
            dgTyLe.Enabled = false;
            loadDSTyLe();
            Id = 0;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            loadDSTyLe();
        }

        private void btnSave__Click(object sender, EventArgs e)
        {
            try
            {
                if (dgthongtinphantyle.Rows != null && dgthongtinphantyle.Rows.Count > 0)
                {
                    var list = new List<P_ReadPercentOfLine>();
                    int lineId = 0, assignId = 0, Id = 0, readPercentId = 0;
                    foreach (DataGridViewRow item in dgthongtinphantyle.Rows)
                    {
                        try
                        {
                            lineId = 0; assignId = 0; Id = 0; readPercentId = 0;
                            int.TryParse(item.Cells[0].Value.ToString(), out Id);
                            int.TryParse(item.Cells[1].Value.ToString(), out lineId);
                            int.TryParse(item.Cells[3].Value.ToString(), out assignId);
                            if (Information.IsNumeric(item.Cells[5].Value.ToString()))
                                int.TryParse(item.Cells[5].Value.ToString(), out readPercentId);
                            else
                            {
                                var obj = listKCSModel.FirstOrDefault(x => x.Name.Trim().Equals(item.Cells[5].Value.ToString().Trim()));
                                readPercentId = obj != null ? obj.Id : 0;
                            }
                            list.Add(new P_ReadPercentOfLine()
                            {
                                Id = Id,
                                LineId = lineId,
                                AssignmentId = assignId,
                                KanbanLightPercentId = readPercentId,
                            });
                        }
                        catch (Exception)
                        { }
                    }
                    BLLReadPercent_KCSInventory.Instance.Update(list, 2);
                    MessageBox.Show("Lưu thông tin thành công.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}
