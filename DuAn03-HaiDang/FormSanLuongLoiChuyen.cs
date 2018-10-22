using DuAn03_HaiDang.DAO;
using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.KeyPad_Chuyen.dao;
using DuAn03_HaiDang.POJO;
using PMS.Business;
using PMS.Business.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DuAn03_HaiDang
{
    public partial class FormSanLuongLoiChuyen : Form
    {
       // ErrorDAO errorDAO = new ErrorDAO();
        ChuyenDAO chuyenDAO = new ChuyenDAO();
        List<ErrorModel> listError = null;
        public FormSanLuongLoiChuyen()
        {
            InitializeComponent();
        }

        public void BuildGridView()
        {
            try
            { 
                if (listError != null && listError.Count > 0)
                {
                    for (int i = 0; i < listError.Count; i++)
                    {
                        var error = listError[i];
                        var columnError = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
                        columnError.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
                        columnError.HeaderText = error.Name;
                        columnError.Name = error.Name;
                        columnError.ReadOnly = true;
                        this.dgTTNangXuat.Columns.Add(columnError);
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void LoadData()
        {
            try
            {
                dgTTNangXuat.Rows.Clear();
                dgTTNangXuat.Refresh();
                var listChuyen = chuyenDAO.GetListChuyenInfByListId(AccountSuccess.strListChuyenId);
                if (listChuyen != null && listChuyen.Count > 0)
                {
                    foreach (var chuyen in listChuyen)
                    {
                        var chuyenSanPham = chuyenDAO.GetChuyenSanPhamInfByChuyenId(chuyen.MaChuyen);
                        {
                            if (chuyenSanPham != null)
                            {
                                DataGridViewRow row = new DataGridViewRow();

                                DataGridViewCell cellChuyen = new DataGridViewTextBoxCell();
                                cellChuyen.Value = chuyen.TenChuyen;
                                row.Cells.Add(cellChuyen);

                                DataGridViewCell cellMaHang = new DataGridViewTextBoxCell();
                                cellMaHang.Value = chuyenSanPham.TenSanPham;
                                row.Cells.Add(cellMaHang);

                                if (listError != null && listError.Count > 0)
                                {
                                    foreach (var error in listError)
                                    {
                                        int sanLuong = chuyenDAO.GetSanLuongLoiCuaChuyen(error.Id, int.Parse(chuyen.MaChuyen), int.Parse(chuyenSanPham.STT));
                                        DataGridViewCell cellSLLoi = new DataGridViewTextBoxCell();
                                        cellSLLoi.Value = sanLuong;
                                        row.Cells.Add(cellSLLoi);
                                    }
                                }

                                dgTTNangXuat.Rows.Add(row);
                                if (dgTTNangXuat.Rows.Count % 2 == 0)
                                {
                                    dgTTNangXuat.Rows[dgTTNangXuat.Rows.Count - 1].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void butUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                LoadData();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void FormSanLuongLoiChuyen_Load(object sender, EventArgs e)
        {
            try
            {

                listError = BLLError.GetAll();//errorDAO.GetListError();
                BuildGridView();
                LoadData();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}
