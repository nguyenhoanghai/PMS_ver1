using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DuAn03_HaiDang.KeyPad_Chuyen.dao;
using DuAn03_HaiDang.DAO;
using DuAn03_HaiDang.DATAACCESS;
using System.Configuration;
using DuAn03_HaiDang.Model;
using System.IO;
using QuanLyNangSuat.Model;
using QuanLyNangSuat;
using PMS.Business;
using PMS.Data;
using PMS.Business.Models;

namespace DuAn03_HaiDang
{
    public partial class FrmAssignCompletion : Form
    {
        int Id = 0;
        public FrmAssignCompletion()
        {
            InitializeComponent();
        }

        private void frmPhaHangChoChuyen_Load(object sender, EventArgs e)
        {
            try
            {
                //san pham
                LoadDSSamPham();
                //
                LoadPhanCongRaDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void LoadDSSamPham()
        {
            lueSanPham.Text = "(--Chọn Mặt Hàng--)";
            var listProduct = BLLCommodity.GetAll(int.Parse(AccountSuccess.IdFloor), AccountSuccess.IsAll);
            if (listProduct != null && listProduct.Count > 0)
            {
                lueSanPham.Properties.DataSource = listProduct;
                lueSanPham.Properties.DisplayMember = "TenSanPham";
                lueSanPham.Properties.ValueMember = "MaSanPham";
            }
        }

        private void LoadPhanCongRaDataGridView()
        {
            gridControl1.DataSource = null;
            gridControl1.DataSource = BLLAssignCompletion.GetAll(true);
        }

        private void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                btnAdd.Enabled = false;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
                Id = int.Parse(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Id").ToString());
                txtOrderIndex.Value = (int)gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "OrderIndex");
                lueSanPham.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CommoName").ToString();
                txtSanLuongKeHoach.Value = (int)gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ProductionsPlan");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnAdd_s_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void btnUpdate_s_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void btnDelete_s_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Việc xoá dữ liệu trong mục Phân Hàng Cho Chuyền sẽ xoá hết toàn bộ dữ liệu có liên quan khác. \n Bạn có thực sự muốn xoá?", "Cảnh báo xoá", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (Id != 0)
                    {
                        var result = BLLAssignCompletion.Delete(Id);
                        if (result.IsSuccess)
                        {
                            LoadPhanCongRaDataGridView();

                        }
                        else
                            MessageBox.Show(result.Messages[0].msg, result.Messages[0].Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                        MessageBox.Show("Lỗi: Bạn chưa chọn phân công muốn xoá.", "Lỗi thao tác", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnCancel_s_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void butImportFromExcel_Click(object sender, EventArgs e)
        {
            try
            {
                btnAdd.Enabled = false;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;

                FrmImportPCCFromExcel frm = new FrmImportPCCFromExcel();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void Save()
        {
            try
            {
                SanPham sanPham = (SanPham)lueSanPham.GetSelectedDataRow();
                if (sanPham == null || sanPham.MaSanPham == 0)
                    MessageBox.Show("Vui lòng chọn Mã Hàng", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (txtSanLuongKeHoach.Value <= 0)
                    MessageBox.Show("Sản lượng kế hoạch của mặt hàng phải lớn hơn 0", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var csp = new P_AssignCompletion();
                    csp.Id = Id;
                    csp.CommoId = sanPham.MaSanPham;
                    csp.OrderIndex = (int)txtOrderIndex.Value;
                    csp.ProductionsPlan = (int)txtSanLuongKeHoach.Value;
                    csp.IsFinish = cbIsFinish.Checked;

                    var finishAssign = BLLAssignCompletion.GetAssignByCommoId(sanPham.MaSanPham, true);
                    if (finishAssign != null)
                    {
                        if (MessageBox.Show("Bạn đã phân công mã hàng " + sanPham.TenSanPham + " vào thời gian " + finishAssign.CreatedDate.ToShortDateString() + " với Sản lượng kế hoạch : " + finishAssign.ProductionsPlan + "(sp).Kết thúc vào ngày " + finishAssign.FinishedDate + ". Bạn có muốn cập nhập thông tin cho phân công này để tiếp tục sản xuất không ?", "Cập nhập dữ liệu", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            #region da ket thuc update lai san luong ke hoach de san xuat tiep
                            //if (csp.ProductionsPlan < finishAssign.ProductionsPlan)
                            //    MessageBox.Show("Sản lượng kế hoạch của Phân công mới không được nhỏ hơn Sản lượng kế hoạch của Phân công cũ.\nVui lòng nhập lại sản lượng kế hoạch.", "Lỗi nhập liệu");
                            //else
                            //{
                            // update lai san luong ke hoach san xuat tiep
                             csp.Id = finishAssign.Id;
                            csp.OrderIndex = finishAssign.OrderIndex;
                            csp.UpdatedDate = DateTime.Now;
                            var kq = BLLAssignCompletion.Update(csp);
                            if (kq.IsSuccess)
                            {
                                LoadPhanCongRaDataGridView();
                                ResetForm();
                            }
                            else
                                MessageBox.Show(kq.Messages[0].msg, kq.Messages[0].Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //}
                            #endregion
                        }
                    }
                    else
                    {
                        #region ADD
                        var findObj = BLLAssignCompletion.GetAssignByCommoId(sanPham.MaSanPham, false);
                        if (findObj != null)
                        {
                            #region
                            if (MessageBox.Show("Thông tin phân công đã tồn tại, bạn muốn thay đổi thông tin Phân công không ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                csp.Id = findObj.Id;
                                csp.UpdatedDate = DateTime.Now;
                                var kq = BLLAssignCompletion.Update(csp);
                                if (kq.IsSuccess)
                                {
                                    LoadPhanCongRaDataGridView();
                                    ResetForm();
                                }
                                else
                                    MessageBox.Show(kq.Messages[0].msg, kq.Messages[0].Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            #endregion
                        }
                        else
                        {
                            #region
                            csp.CreatedDate = DateTime.Now;
                            var rs = BLLAssignCompletion.Insert(csp);
                            if (rs.IsSuccess)
                            {
                                LoadPhanCongRaDataGridView();
                                ResetForm();
                            }
                            else
                                MessageBox.Show(rs.Messages[0].msg, rs.Messages[0].Title, MessageBoxButtons.OK, MessageBoxIcon.Error);

                            #endregion
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ResetForm()
        {
            btnAdd.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            txtOrderIndex.Value = 0;
            txtSanLuongKeHoach.Value = 0;
            Id = 0;
        }

        private void btnRefreshPro_Click(object sender, EventArgs e)
        {
            //san pham
            LoadDSSamPham();
        }
    }
}
