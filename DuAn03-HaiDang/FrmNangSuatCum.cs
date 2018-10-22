using DuAn03_HaiDang.DAO;
using DuAn03_HaiDang.DATAACCESS;
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
    public partial class FrmNangSuatCum : FormBase
    {
        private CumDAO cumDAO;
        private int maxCountCum;
        public FrmNangSuatCum()
        {
            InitializeComponent();
            cumDAO = new CumDAO();
            maxCountCum = cumDAO.GetMaxCountOfChuyen();
            BuildGridView();
        }

        public void BuildGridView()
        {
            try
            {
                if (maxCountCum > 0)
                {
                    for (int i = 0; i < maxCountCum; i++)
                    {
                        var columnTram = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
                        columnTram.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
                        columnTram.HeaderText = "Trạm" + (i + 1).ToString();
                        columnTram.Name = "lblTram" + (i + 1).ToString();
                        columnTram.ReadOnly = true;
                        this.dgTTNangXuat.Columns.Add(columnTram);

                        var columnLuyKeTram = new DevComponents.DotNetBar.Controls.DataGridViewLabelXColumn();
                        columnLuyKeTram.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
                        columnLuyKeTram.HeaderText = "Lũy kế trạm" + (i + 1).ToString();
                        columnLuyKeTram.Name = "lblLuyKeTram" + (i + 1).ToString();
                        columnLuyKeTram.ReadOnly = true;
                        this.dgTTNangXuat.Columns.Add(columnLuyKeTram);
                    }
                }                
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Lỗi: "+ ex.Message);
            }
        }
        
        private void LoadData()
        {           
            try
            {
                var listModelNangSuatCum = cumDAO.GetTTNangSuatCum(AccountSuccess.strListChuyenId);
                if (listModelNangSuatCum != null && listModelNangSuatCum.Count > 0)
                {
                    foreach (var item in listModelNangSuatCum)
                    {
                        DataGridViewRow row = new DataGridViewRow();

                        DataGridViewCell cellChuyen = new DataGridViewTextBoxCell();
                        cellChuyen.Value = item.chuyen;
                        row.Cells.Add(cellChuyen);

                        DataGridViewCell cellMaHang = new DataGridViewTextBoxCell();
                        cellMaHang.Value = item.maHang;
                        row.Cells.Add(cellMaHang);

                        DataGridViewCell cellKeHoach = new DataGridViewTextBoxCell();
                        cellKeHoach.Value = item.sanLuongKeHoach;
                        row.Cells.Add(cellKeHoach);

                        if (item.listNangSuatCum != null)
                        {
                            foreach (var nscum in item.listNangSuatCum)
                            {
                                DataGridViewCell cellTram = new DataGridViewTextBoxCell();
                                cellTram.Value = nscum.cum;
                                row.Cells.Add(cellTram);

                                DataGridViewCell cellSanLuong = new DataGridViewTextBoxCell();
                                cellSanLuong.Value = nscum.sanLuong;
                                row.Cells.Add(cellSanLuong);
                            }
                        }
                        else
                        {
                            if (maxCountCum > 0)
                            {
                                for (int i = 0; i < maxCountCum; i++)
                                {
                                    DataGridViewCell cellTram = new DataGridViewTextBoxCell();
                                    cellTram.Value = "";
                                    row.Cells.Add(cellTram);

                                    DataGridViewCell cellSanLuong = new DataGridViewTextBoxCell();
                                    cellSanLuong.Value = "";
                                    row.Cells.Add(cellSanLuong);
                                }
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void butUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                dgTTNangXuat.Rows.Clear();
                dgTTNangXuat.Refresh();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: "+ex.Message);
            }
            
        }

        private void FrmNangSuatCum_Load(object sender, EventArgs e)
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
       
    }
}
