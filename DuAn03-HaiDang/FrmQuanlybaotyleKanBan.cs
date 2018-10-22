using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using DuAn03_HaiDang.DAO;
//using DuAn03_HaiDang.POJO;
using System.IO;
//using DuAn03_HaiDang.KeyPad_Chuyen.pojo;
using DuAn03_HaiDang.DATAACCESS;
//using DuAn03_HaiDang.KeyPad_Chuyen.dao;
using PMS.Business;
using PMS.Business.Models;
using PMS.Data;
using Microsoft.VisualBasic;

namespace DuAn03_HaiDang
{
    public partial class FrmQuanlybaotyleKanBan : FormBase
    {
        //  DataTable dtDStyle = new DataTable();       
        //  List<ReadPercent> listDen = new List<ReadPercent>();
        //  List<ReadPercent> listDen_PhanCong = new List<ReadPercent>();
        //  ChuyenDAO chuyenDAO;        
        private int idReadPercent = 0;
        List<ReadPercentModel> listreadPercents = new List<ReadPercentModel>();
        //  ReadPercentDAO readpercentDAO = new ReadPercentDAO(); 
        public FrmQuanlybaotyleKanBan()
        {
            InitializeComponent();
            //   chuyenDAO = new ChuyenDAO();
        }

        private void butHuyThayDoiTyLe_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmQuanlybaotyleKanBan_Load(object sender, EventArgs e)
        {
            try
            {
                enableButton(true);
                dgTyLe.Enabled = false;
                loadDSTyLe();
                loadPhanCongTyLe();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        private void loadData(int Id)
        {
            dgTyLe.Rows.Clear();
            dgTyLe.Refresh();
            //   readpercentDAO.LoadOBJToDataGirdview(Id, dgTyLe);
        }

        private void dgTyLe_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            int Id = int.Parse(e.Row.Cells[0].Value.ToString());
            // readpercentDAO.XoaOBJ(Id);
            BLLReadPercent.Instance.Delete(Id);

        }

        private void dgTyLe_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                try
                {
                    OpenFileDialog dlg = new OpenFileDialog();
                    dlg.Filter = "file hinh|*.wav|all file|*.*";
                    dlg.InitialDirectory = @"C:\";
                    dlg.Multiselect = true;
                    string a = null;
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        string[] tmp = dlg.FileNames;
                        foreach (string i in tmp)
                        {
                            FileInfo fi = new FileInfo(i);
                            string[] xxx = i.Split('\\');
                            string des = Application.StartupPath + @"\Sound\" + xxx[xxx.Length - 1];
                            a = xxx[xxx.Length - 1];
                            File.Delete(des);
                            //over.
                            fi.CopyTo(des);
                        }
                        dgTyLe.Rows[e.RowIndex].Cells[4].Value = a;
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void loadPhanCongTyLe()
        {
            dgthongtinphantyle.Rows.Clear();
            // var listChuyen = chuyenDAO.GetListChuyenInfByListId(AccountSuccess.strListChuyenId);
            //    = BLLLine.GetLines_s(AccountSuccess.strListChuyenId.Split(',').Select(x => Convert.ToInt32(x)).ToList());
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
                    row.Cells[5].Value = listChuyen[i].ReadPercentId == null ? "Chọn tỷ lệ đọc thông báo" : listChuyen[i].ReadPercentName.ToString();
                    dgthongtinphantyle.Rows.Add(row);
                }
            }
        }


        //private void loadDSTyLe()
        //{
        //    cboTentyle.DataSource = null;            
        //    cboTentyle.Refresh();
        //    dgTenTyLe.DataSource = null;
        //    dtDStyle.Clear();
        //    listDen.Clear();
        //    listDen_PhanCong.Clear();
        //    string sql = "select Id, Name from ReadPercent where IsDeleted=0 and (IdParent Is NULL OR IdParent = 0) ";
        //    dtDStyle = dbclass.TruyVan_TraVe_DataTable(sql);
        //    listDen_PhanCong.Add(new ReadPercent { Id = 0, Name = "Chọn tỷ lệ đọc thông báo" });
        //    if (dtDStyle != null && dtDStyle.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dtDStyle.Rows.Count; i++)
        //        {
        //            ReadPercent item = new ReadPercent { Id = int.Parse(dtDStyle.Rows[i]["Id"].ToString()), Name = dtDStyle.Rows[i]["Name"].ToString() };
        //            listDen.Add(item);
        //            listDen_PhanCong.Add(item);
        //        }

        //    }
        //    if (listDen.Count > 0)
        //    {
        //        cboTentyle.DataSource = listDen;
        //        cboTentyle.DisplayMember = "Name";
        //        cboTentyle.SelectedIndex = 0;
        //        cboTentyle.Refresh();

        //    }
        //    dgTenTyLe.DataSource = listDen_PhanCong;
        //    dgTenTyLe.DisplayMember = "Name"; 
        //}

        private void loadDSTyLe()
        {
            listreadPercents.Clear();
            cboTentyle.DataSource = null;
            cboTentyle.Refresh();
            dgTenTyLe.DataSource = null;

            listreadPercents.AddRange(BLLReadPercent.Instance.GetAll());
            if (listreadPercents.Count > 0)
            {
                cboTentyle.DataSource = listreadPercents;
                cboTentyle.DisplayMember = "Name";
                cboTentyle.SelectedIndex = 0;
                cboTentyle.Refresh();
            }

            dgTenTyLe.DataSource = listreadPercents;
            dgTenTyLe.DisplayMember = "Name";
            dgTenTyLe.ValueMember = "Id";
        }

        private void cboTentyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTentyle.DataSource != null)
            {
                var item = (ReadPercentModel)cboTentyle.SelectedItem;
                idReadPercent = item.Id;
                // loadData(item.Id);
                BindData(item.Childs);
            }
        }

        private void BindData(List<ReadPercent> list)
        {
            try
            {
                dgTyLe.Rows.Clear();
                dgTyLe.Refresh();
                //   readpercentDAO.LoadOBJToDataGirdview(Id, dgTyLe);

                if (list != null && list.Count > 0)
                {
                    DataGridViewRow row;
                    for (int i = 0; i < list.Count; i++)
                    {
                        row = (DataGridViewRow)dgTyLe.Rows[i].Clone();
                        row.Cells[0].Value = list[i].Id;
                        row.Cells[1].Value = list[i].PercentFrom;
                        row.Cells[2].Value = list[i].PercentTo;
                        row.Cells[3].Value = list[i].CountRepeat;
                        if (!string.IsNullOrEmpty(list[i].Sound))
                            row.Cells[4].Value = list[i].Sound;
                        dgTyLe.Rows.Add(row);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void butNapdulieu_Click(object sender, EventArgs e)
        {
            loadPhanCongTyLe();
        }

        private void butLuuphanchia_Click(object sender, EventArgs e)
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
                            int.TryParse(item.Cells[0].Value.ToString(), out Id);
                            int.TryParse(item.Cells[1].Value.ToString(), out lineId);
                            int.TryParse(item.Cells[3].Value.ToString(), out assignId);
                            if (Information.IsNumeric(item.Cells[5].Value.ToString()))
                                int.TryParse(item.Cells[5].Value.ToString(), out readPercentId);
                            else
                            {
                                var obj = listreadPercents.FirstOrDefault(x => x.Name.Trim().Equals(item.Cells[5].Value.ToString().Trim()));
                                readPercentId = obj != null ? obj.Id : 0;
                            }
                            list.Add(new P_ReadPercentOfLine()
                            {
                                Id = Id,
                                LineId = lineId,
                                AssignmentId = assignId,
                                ReadPercentId = readPercentId,
                            });
                        }
                        catch (Exception)
                        { } 
                    }
                    BLLReadPercent.Instance.Update(list);
                    MessageBox.Show("Lưu thông tin thành công.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void enableButton(bool value)
        {
            butSua.Enabled = value;
            butThemTyLe.Enabled = value;
            butDeleteTyLe.Enabled = value;
            butLuutyle.Enabled = !value;
            butHuy.Enabled = !value;
        }

        private void butThemTyLe_Click(object sender, EventArgs e)
        {
            idReadPercent = 0;
            enableButton(false);
            dgTyLe.Enabled = true;
            dgTyLe.Rows.Clear();
            dgTyLe.Refresh();
            cboTentyle.Text = "";
            cboTentyle.Focus();
        }

        private void butSua_Click(object sender, EventArgs e)
        {
            try
            {
                dgTyLe.Enabled = true;
                enableButton(false);
                ReadPercent item = (ReadPercent)cboTentyle.SelectedItem;
                idReadPercent = item.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void butHuy_Click(object sender, EventArgs e)
        {
            enableButton(true);
            dgTyLe.Enabled = false;
            loadDSTyLe();
            idReadPercent = 0;
        }

        private void butLuutyle_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cboTentyle.Text))
            {
                ResponseBase rs;
                var obj = new ReadPercent()
                {
                    Id = idReadPercent,
                    Name = cboTentyle.Text,
                    PercentFrom = 0,
                    PercentTo = 0,
                    CountRepeat = 0
                };
                List<ReadPercent> listReadPercent = new List<ReadPercent>();
                if (dgTyLe.Rows.Count > 0)
                {
                    ReadPercent rp;
                    for (int i = 0; i < dgTyLe.Rows.Count - 1; i++)
                    {
                        rp = new ReadPercent();
                        DataGridViewRow row = dgTyLe.Rows[i];
                        if (row != null)
                        {
                            int percentFrom = 0;
                            if (row.Cells["dgPercentFrom"].Value != null)
                                int.TryParse(row.Cells["dgPercentFrom"].Value.ToString(), out percentFrom);
                            rp.PercentFrom = percentFrom;

                            int percentTo = 0;
                            if (row.Cells["dgPercentTo"].Value != null)
                                int.TryParse(row.Cells["dgPercentTo"].Value.ToString(), out percentTo);
                            rp.PercentTo = percentTo;

                            int countRepeat = 0;
                            if (row.Cells["dgCountRepeat"].Value != null)
                                int.TryParse(row.Cells["dgCountRepeat"].Value.ToString(), out countRepeat);
                            rp.CountRepeat = countRepeat;

                            string sound = string.Empty;
                            if (row.Cells["tsound"].Value != null)
                                sound = row.Cells["tsound"].Value.ToString();
                            rp.Sound = sound;

                            rp.IdParent = idReadPercent;

                            listReadPercent.Add(rp);
                        }
                    }

                }
                if (idReadPercent == 0)
                    //   result = readpercentDAO.ThemOBJ(obj, listReadPercent);
                    rs = BLLReadPercent.Instance.Insert(obj, listReadPercent);
                else
                    //   result = readpercentDAO.SuaThongTinOBJ(idReadPercent, listReadPercent);
                    rs = BLLReadPercent.Instance.Update(idReadPercent, listReadPercent);
                if (rs.IsSuccess)
                {
                    enableButton(true);
                    dgTyLe.Enabled = false;
                    loadDSTyLe();
                    loadPhanCongTyLe();
                    idReadPercent = 0;
                    MessageBox.Show("Lưu thông tin thành công");
                }
            }
            else
            {
                MessageBox.Show("Tên tỷ lệ không được để trống", "Lỗi thao tác", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void butDeleteTyLe_Click(object sender, EventArgs e)
        {
            if (cboTentyle.DataSource != null)
            {
                if (MessageBox.Show("Bạn có luốn xoá tỷ lệ này không?", "Xoá tỷ lệ", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    var item = (ReadPercent)cboTentyle.SelectedItem;
                    // readpercentDAO.XoaOBJ(item.Id);
                    BLLReadPercent.Instance.Delete(item.Id);

                    loadDSTyLe();
                    loadPhanCongTyLe();
                }

            }
        }

        private void dgthongtinphantyle_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

    }
}
