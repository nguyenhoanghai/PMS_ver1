using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.POJO;
using DuAn03_HaiDang.KeyPad_Chuyen.pojo;
using DuAn03_HaiDang.DAO;
using System.Data.SqlClient;
using System.Configuration;
using DuAn03_HaiDang.KeyPad_Chuyen.dao;
using DuAn03_HaiDang.Model;
using PMS.Business;
using PMS.Business.Enum;

namespace DuAn03_HaiDang
{
    public partial class FrmTyLeDen_PhanChuyen : FormBase
    {
        DataTable dtDStyle = new DataTable();
        List<Den> listDen = new List<Den>();
        List<Den> listDen_PhanCong = new List<Den>();
        ChuyenDAO chuyenDAO;
        string eventclick = "";
        string idTable = "";
        int appId = 0;
        public FrmTyLeDen_PhanChuyen()
        {
            InitializeComponent();
            int.TryParse(ConfigurationManager.AppSettings["AppId"].ToString(), out appId);
            idTable = BLLConfig.Instance.FindConfigValue(appId, eAppConfigName.TABLE); // dbclass.listAppConfig.Where(c => c.Name.Trim().ToUpper().Equals("TABLE")).Select(c => c.Value).FirstOrDefault().ToString();
        }
        private void loadDSTyLe()
        {
            cboTentyle.DataSource = null;
            //cboTentyle.Items.Clear();
            cboTentyle.Refresh();
            dgTenTyLe.DataSource = null;
            chuyenDAO = new ChuyenDAO();
            dtDStyle.Clear();
            listDen.Clear();
            listDen_PhanCong.Clear();
            string sql = "";
            if (idTable == "1")
            {
                sql = "select STT, Color from Den where IdCatalogTable =1 and STTParent Is NULL";
            }
            else if (idTable == "2")
            {
                sql = "select STT, Color from Den where IdCatalogTable =2 and STTParent Is NULL";
            }
            else
            {
                MessageBox.Show("Lỗi: bạn chưa gán hoặc gán sai id bảng trong file config. Cần kiểm tra lại thông tin.", "Lỗi cấu hình", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            dtDStyle = dbclass.TruyVan_TraVe_DataTable(sql);
            listDen_PhanCong.Add(new Den { STT = "0", Color = "Chọn tỷ lệ đèn" });
            if (dtDStyle.Rows.Count > 0)
            {
                for (int i = 0; i < dtDStyle.Rows.Count; i++)
                {
                    Den den = new Den { STT = dtDStyle.Rows[i][0].ToString(), Color = dtDStyle.Rows[i][1].ToString() };
                    listDen.Add(den);
                    listDen_PhanCong.Add(den);
                }
            }
            cboTentyle.DataSource = listDen;
            cboTentyle.DisplayMember = "Color";
            cboTentyle.ValueMember = "STT";
            cboTentyle.Refresh();
            dgTenTyLe.DataSource = listDen_PhanCong;
            dgTenTyLe.DisplayMember = "Color";
            dgTenTyLe.ValueMember = "STT";
        }

        private void loadPhanCongTyLe()
        {
            dgthongtinphantyle.Rows.Clear();
            var listChuyen = chuyenDAO.GetListChuyenInfByListId(AccountSuccess.strListChuyenId);
            if (listChuyen != null && listChuyen.Count > 0)
            {
                DataGridViewRow row;
                for (int i = 0; i < listChuyen.Count; i++)
                {
                    row = (DataGridViewRow)dgthongtinphantyle.Rows[i].Clone();
                    row.Cells[0].Value = listChuyen[i].MaChuyen;
                    row.Cells[1].Value = listChuyen[i].TenChuyen;
                    if (idTable == "1")
                        row.Cells[2].Value = listChuyen[i].IdDenNangSuat.ToString();
                    else if (idTable == "2")
                        row.Cells[2].Value = listChuyen[i].IdDen.ToString();
                    
                    dgthongtinphantyle.Rows.Add(row);
                }
            }
        }

        private void chiTietTyLe(string STTDen)
        {
            try
            {
                dtDStyle.Clear();
                dgThongtintyle.Rows.Clear();
                string strSQL = "";
                if (idTable == "1")
                {
                    strSQL = "select STT, Color, ValueFrom, ValueTo, MaMauDen from Den Where IdCatalogTable =1 and STTParent ='" + STTDen + "'";
                }
                else if (idTable == "2")
                {
                    strSQL = "select STT, Color, ValueFrom, ValueTo, MaMauDen from Den Where IdCatalogTable =2 and STTParent ='" + STTDen + "'";
                }
                else
                {
                    MessageBox.Show("Lỗi: bạn chưa gán hoặc gán sai id bảng trong file config. Cần kiểm tra lại thông tin.", "Lỗi cấu hình", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                dtDStyle = dbclass.TruyVan_TraVe_DataTable(strSQL);
                if (dtDStyle.Rows.Count > 0)
                {
                    DataGridViewRow row;
                    for (int i = 0; i < dtDStyle.Rows.Count; i++)
                    {
                        row = (DataGridViewRow)dgThongtintyle.Rows[i].Clone();
                        row.Cells[0].Value = dtDStyle.Rows[i][0].ToString();
                        row.Cells[1].Value = dtDStyle.Rows[i][1].ToString();
                        row.Cells[2].Value = int.Parse(dtDStyle.Rows[i][2].ToString());
                        row.Cells[3].Value = int.Parse(dtDStyle.Rows[i][3].ToString());
                        row.Cells[4].Value = dtDStyle.Rows[i][4].ToString();
                        dgThongtintyle.Rows.Add(row);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void FrmTyLeDen_PhanChuyen_Load(object sender, EventArgs e)
        {
            try
            {
                loadDSTyLe();
                loadPhanCongTyLe();
                dgThongtintyle.Enabled = false;
                butThemTyLe.Enabled = true;
                butSua.Enabled = true;
                butDeleteTyLe.Enabled = true;
                butLuutyle.Enabled = false;
                butHuy.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
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
                foreach (DataGridViewRow item in dgthongtinphantyle.Rows)
                {
                    string MaChuyen = "";
                    string MaDen = "";
                    try
                    {
                        MaChuyen = item.Cells[0].Value.ToString();
                        MaDen = item.Cells[2].Value.ToString();
                    }
                    catch (Exception)
                    {

                    }

                    if (MaChuyen != "")
                    {
                        string strSql = "";
                        if (idTable == "2")
                        {
                            strSql = "update Chuyen set IdDen='" + MaDen + "' where MaChuyen='" + MaChuyen + "'";
                        }
                        else if (idTable == "1")
                        {
                            strSql = "update Chuyen set IdDenNangSuat='" + MaDen + "' where MaChuyen='" + MaChuyen + "'";
                        }
                        else
                        {
                            MessageBox.Show("Lỗi: bạn chưa gán hoặc gán sai id bảng trong file config. Cần kiểm tra lại thông tin.", "Lỗi cấu hình", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        dbclass.TruyVan_XuLy(strSql);
                    }
                }
            }
            catch (Exception)
            {

            }

        }























        private void butSua_Click(object sender, EventArgs e)
        {
            dgThongtintyle.Enabled = true;
            cboTentyle.Enabled = false;
            butThemTyLe.Enabled = false;
            butSua.Enabled = false;
            butDeleteTyLe.Enabled = false;
            butLuutyle.Enabled = true;
            butHuy.Enabled = true;
            eventclick = "update";
        }

        private void butThemTyLe_Click(object sender, EventArgs e)
        {
            dgThongtintyle.Enabled = true;
            dgThongtintyle.Rows.Clear();
            //Insert information 
            DataGridViewRow drow;
            drow = (DataGridViewRow)dgThongtintyle.Rows[0].Clone();
            drow.Cells[0].Value = "";
            drow.Cells[1].Value = "Đỏ";
            drow.Cells[2].Value = "";
            drow.Cells[3].Value = "";
            if (idTable == "2")
            {
                drow.Cells[4].Value = "4";
            }
            else if (idTable == "1")
            {
                drow.Cells[4].Value = "1";
            }
            dgThongtintyle.Rows.Add(drow);
            drow = (DataGridViewRow)dgThongtintyle.Rows[1].Clone();
            drow.Cells[0].Value = "";
            drow.Cells[1].Value = "Vàng";
            drow.Cells[2].Value = "";
            drow.Cells[3].Value = "";
            if (idTable == "2")
            {
                drow.Cells[4].Value = "5";
            }
            else if (idTable == "1")
            {
                drow.Cells[4].Value = "2";
            }
            dgThongtintyle.Rows.Add(drow);
            drow = (DataGridViewRow)dgThongtintyle.Rows[2].Clone();
            drow.Cells[0].Value = "";
            drow.Cells[1].Value = "Xanh";
            drow.Cells[2].Value = "";
            drow.Cells[3].Value = "";
            if (idTable == "2")
            {
                drow.Cells[4].Value = "6";
            }
            else if (idTable == "1")
            {
                drow.Cells[4].Value = "3";
            }
            dgThongtintyle.Rows.Add(drow);
            //
            cboTentyle.Enabled = true;
            butThemTyLe.Enabled = false;
            butSua.Enabled = false;
            butDeleteTyLe.Enabled = false;
            butLuutyle.Enabled = true;
            butHuy.Enabled = true;
            eventclick = "insert";
            cboTentyle.Text = "";
            cboTentyle.Focus();
        }
        DenDAO denDAO = new DenDAO();
        private void butLuutyle_Click(object sender, EventArgs e)
        {

            Den den;
            if (eventclick == "update")
            {
                try
                {
                    string STT;

                    foreach (DataGridViewRow item in dgThongtintyle.Rows)
                    {
                        STT = "";
                        try
                        {
                            STT = item.Cells[0].Value.ToString();

                        }
                        catch (Exception)
                        {

                        }

                        if (STT != "")
                        {
                            den = new Den { STT = STT, ValueFrom = int.Parse(item.Cells[2].Value.ToString()), ValueTo = int.Parse(item.Cells[3].Value.ToString()) };
                            int kq = denDAO.SuaThongTinDen(den);

                        }
                    }

                    EnableButton2();



                }
                catch (Exception)
                {

                }
            }
            else
            {
                try
                {
                    string Name = cboTentyle.Text;
                    if (Name != "")
                    {
                        string IdTest = denDAO.TestName(Name);
                        if (IdTest == "")
                        {
                            int kq = 0;
                            if (idTable == "2")
                            {
                                den = new Den { Color = Name, ValueFrom = 0, ValueTo = 0, IdCatalogTable = "2", STTParent = null };
                                kq = denDAO.ThemTTDen(den);
                            }
                            else if (idTable == "1")
                            {
                                den = new Den { Color = Name, ValueFrom = 0, ValueTo = 0, IdCatalogTable = "1", STTParent = null };
                                kq = denDAO.ThemTTDen(den);
                            }

                            if (kq > 0)
                            {
                                string STT = denDAO.FindToFinalId();
                                if (STT != "")
                                {
                                    foreach (DataGridViewRow item in dgThongtintyle.Rows)
                                    {

                                        try
                                        {

                                            if (item.Cells[1].Value != null)
                                            {

                                                if (idTable == "2")
                                                {
                                                    den = new Den { Color = item.Cells[1].Value.ToString(), ValueFrom = int.Parse(item.Cells[2].Value.ToString()), ValueTo = int.Parse(item.Cells[3].Value.ToString()), IdCatalogTable = "2", STTParent = STT, MaMauDen = int.Parse(item.Cells[4].Value.ToString()) };
                                                    kq = denDAO.ThemTTDen(den);
                                                }
                                                else if (idTable == "1")
                                                {
                                                    den = new Den { Color = item.Cells[1].Value.ToString(), ValueFrom = int.Parse(item.Cells[2].Value.ToString()), ValueTo = int.Parse(item.Cells[3].Value.ToString()), IdCatalogTable = "1", STTParent = STT, MaMauDen = int.Parse(item.Cells[4].Value.ToString()) };
                                                    kq = denDAO.ThemTTDen(den);
                                                }
                                            }
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                    loadDSTyLe();
                                    EnableButton2();
                                }
                                else
                                {
                                    MessageBox.Show("Lỗi: Không tìm thấy id tên tỷ lệ!", "Lỗi xử lý", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Lỗi: Tên loại tỷ lệ đã tồn tại vui lòng chọn tên tỉ lệ khác!", "Lỗi thao tác", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            cboTentyle.Text = "";
                            cboTentyle.Focus();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Lỗi: Tên loại tỷ lệ không được để trống.", "Lỗi thao tác", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                }
                catch (Exception)
                {


                }
            }

        }
        private void EnableButton2()
        {
            dgThongtintyle.Enabled = false;
            //dgThongtintyle.Rows.Clear();
            //cboTentyle.SelectedIndex = 0;
            cboTentyle.Enabled = true;
            butThemTyLe.Enabled = true;
            butSua.Enabled = true;
            butDeleteTyLe.Enabled = true;
            butLuutyle.Enabled = false;
            butHuy.Enabled = false;
        }
        private void butHuy_Click(object sender, EventArgs e)
        {

            loadDSTyLe();
            EnableButton2();
        }

        private void cboTentyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTentyle.DataSource != null)
            {
                Den den = (Den)cboTentyle.SelectedItem;
                chiTietTyLe(den.STT);
            }
        }

        private void butDeleteTyLe_Click(object sender, EventArgs e)
        {
            try
            {

                if (MessageBox.Show("Bạn có muốn xoá tỷ lệ đèn được chọn không?", "Xoá tỷ lệ đèn", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Den den = (Den)cboTentyle.SelectedItem;
                    int kq = denDAO.XoaTyLeDen(den.STT);
                    if (kq > 0)
                    {
                        loadDSTyLe();
                        EnableButton2();
                        cboTentyle.SelectedIndex = 0;
                        cboTentyle.Text = ((Den)cboTentyle.SelectedItem).Color;
                    }
                }
            }
            catch (Exception)
            { }
        }

        private void dgthongtinphantyle_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgthongtinphantyle_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }



    }
}
