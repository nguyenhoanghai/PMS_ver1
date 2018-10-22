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
    public partial class frmPhaHangChoChuyen : Form
    {
        HangDAO hangDAO = new HangDAO();
        ChuyenDAO chuyenDAO = new ChuyenDAO();
        Chuyen_SanPhamDAO chuyen_sanphamDAO = new Chuyen_SanPhamDAO();
        ThanhPhamDAO thanhphamDAO = new ThanhPhamDAO();
        NangXuatDAO nangxuatDAO = new NangXuatDAO();
        BTPDAO btpDAO = new BTPDAO();
        DateTime dateNow;
        int sttChuyenSanPham = 0;
        FrmMainNew frmMainNew;
        List<ChuyenSanPham> listPCC;

        List<AssignmentForLine_Grid_Model> asigns;
        public frmPhaHangChoChuyen(FrmMainNew _frmMainNew)
        {
            InitializeComponent();
            this.frmMainNew = _frmMainNew;
        }

        private void LoadDataForCbbMorth()
        {
            try
            {
                List<ModelSelect> listSelect = new List<ModelSelect>();
                int selectIndex = 0;
                int morthNow = dateNow.Month;
                for (int i = 0; i < 12; i++)
                {
                    listSelect.Add(new ModelSelect()
                    {
                        Text = "Tháng " + (i + 1).ToString(),
                        Value = i + 1
                    });
                    if (morthNow == i + 1)
                        selectIndex = i;
                }
                cbbMorth.DataSource = listSelect;
                cbbMorth.DisplayMember = "Text";
                cbbMorth.SelectedIndex = selectIndex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadDataForCbbYear()
        {
            try
            {

                int yearNow = dateNow.Year;
                int selectIndex = 0;
                List<ModelSelect> listSelect = new List<ModelSelect>();
                for (int i = yearNow - 4; i <= yearNow + 4; i++)
                {
                    listSelect.Add(new ModelSelect()
                    {
                        Text = "Năm " + i,
                        Value = i
                    });
                    if (i == yearNow)
                        selectIndex = listSelect.Count - 1;
                }
                cbbYear.DataSource = listSelect;
                cbbYear.DisplayMember = "Text";
                cbbYear.SelectedIndex = selectIndex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void frmPhaHangChoChuyen_Load(object sender, EventArgs e)
        {
            try
            {
                dateNow = DateTime.Now;
                //san pham
                LoadDSSamPham();
                //Load cbbmorth
                LoadDataForCbbMorth();
                //Load cbbyear
                LoadDataForCbbYear();
                //chuyen
                LoadDSChuyen();
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
            lueSanPham.Properties.DataSource = null;
            //  var listProduct = hangDAO.GetListProduct(AccountSuccess.IdFloor, AccountSuccess.IsAll);
            var listProduct = BLLCommodity.GetAll(int.Parse(AccountSuccess.IdFloor), AccountSuccess.IsAll);
            if (listProduct != null && listProduct.Count > 0)
            {
                lueSanPham.Properties.DataSource = listProduct;
                lueSanPham.Properties.DisplayMember = "TenSanPham";
                lueSanPham.Properties.ValueMember = "MaSanPham";

            }
        }

        private void LoadDSChuyen()
        {
            cbbChuyen.DataSource = null;
            cbbChuyen.Text = "(-- Chọn Chuyền --)";
            if (!string.IsNullOrEmpty(AccountSuccess.strListChuyenId))
            {
                var listChuyenByListId = BLLLine.GetLines(AccountSuccess.strListChuyenId.Split(',').Select(x => Convert.ToInt32(x)).ToList());
                if (listChuyenByListId != null && listChuyenByListId.Count > 0)
                {
                    cbbChuyen.DataSource = listChuyenByListId;
                    cbbChuyen.DisplayMember = "TenChuyen";
                }
            }
        }

        private void LoadPhanCongRaDataGridView()
        {
            listPCC = new List<ChuyenSanPham>();
            Chuyen chuyen = ((Chuyen)cbbChuyen.SelectedItem);
            if (chuyen != null)
            {
                gridControl1.DataSource = null;
                asigns = BLLAssignmentForLine.Instance.GetDataForGridView(chuyen.MaChuyen);
                gridControl1.DataSource = asigns;
                if (asigns != null && asigns.Count > 0)
                    txtSTTThucHien.Value = asigns.OrderByDescending(x => x.STT_TH).FirstOrDefault().STT_TH + 1;
            }
        }

        private void cbbChuyen_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadPhanCongRaDataGridView();
                sttChuyenSanPham = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                btnAdd.Enabled = false;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;

                sttChuyenSanPham = int.Parse(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "STT").ToString());
                if (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "STT_TH") != null)
                    txtSTTThucHien.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "STT_TH").ToString();
                //cbbTenSanPham.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "TenSanPham").ToString();
                lueSanPham.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CommoName").ToString();
              //  txtNangXuatSanXuat.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "TimeProductPerCommo").ToString();
                txtSanLuongKeHoach.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ProductionPlans").ToString();
                string morth = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Month").ToString();
                string year = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Year").ToString();
                if (!string.IsNullOrEmpty(morth))
                    cbbMorth.SelectedIndex = cbbMorth.FindStringExact("Tháng " + morth);
                if (!string.IsNullOrEmpty(year))
                    cbbYear.SelectedIndex = cbbYear.FindStringExact("Năm " + year);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }

        }

        private void btnAdd_s_Click(object sender, EventArgs e)
        {
            Save_N();
        }

        private void btnUpdate_s_Click(object sender, EventArgs e)
        {
            Save_N();
        }

        private void btnDelete_s_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Việc xoá dữ liệu trong mục Phân Hàng Cho Chuyền sẽ xoá hết toàn bộ dữ liệu có liên quan khác. \n Bạn có thực sự muốn xoá?", "Cảnh báo xoá", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    if (sttChuyenSanPham != 0)
                    {
                        var isThucHien = BLLProductivity.CheckIsHasProductivity(sttChuyenSanPham);
                        if (isThucHien == null)
                        {
                            var result = BLLAssignmentForLine.Instance.Delete(sttChuyenSanPham);
                            if (result.IsSuccess)
                                LoadPhanCongRaDataGridView();
                            MessageBox.Show(result.Messages[0].msg, result.Messages[0].Title);
                        }
                        else
                            MessageBox.Show("Lỗi: phân công này đã được đưa vào thực hiện ở những ngày trước, bạn không thể xoá nó.");
                    }
                    else
                        MessageBox.Show("Lỗi: Bạn chưa chọn thông tin muốn xoá.", "Lỗi thao tác", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
 
        /// <summary>
        /// 
        /// </summary>
        private void Save_N()
        {
            try
            {
                // Kiem tra tinh trang cap nhap trang thai IsFinishBTPThoatChuyen
                // Update nhung row cap nhap sai hoac khong cap nhap
                string strError = string.Empty;
                chuyen_sanphamDAO.UpdateIsFinishBTPChuyen();
                SanPham sanPham = (SanPham)lueSanPham.GetSelectedDataRow();
                Chuyen chuyen = ((Chuyen)cbbChuyen.SelectedItem);
                ModelSelect modelSelectMorth = (ModelSelect)cbbMorth.SelectedItem;
                ModelSelect modelSelectYear = (ModelSelect)cbbYear.SelectedItem; 
                if (chuyen == null)
                    MessageBox.Show("Bạn chưa chọn chuyền sản xuất. Vui lòng thực hiện thao tác này...", "Lỗi nhập liệu");
                else if (sanPham == null)
                    MessageBox.Show("Bạn chưa chọn Mặt Hàng để phân cho chuyền. Vui lòng thực hiện thao tác này...\n", "Lỗi nhập liệu");
                else if (string.IsNullOrEmpty(txtSTTThucHien.Text))
                    MessageBox.Show("Bạn chưa nhập thứ tự thực hiện mặt hàng của chuyền.\n", "Lỗi nhập liệu");
                else if (modelSelectMorth == null)
                    MessageBox.Show("Bạn chưa chọn thông tin tháng thực hiện.\n", "Lỗi nhập liệu");
                else if (modelSelectYear == null)
                    MessageBox.Show("Bạn chưa chọn thông tin năm thực hiện.\n", "Lỗi nhập liệu");
              //  else if (string.IsNullOrEmpty(txtNangXuatSanXuat.Text))
                   // MessageBox.Show("Bạn chưa nhập thời gian chế tạo mặt hàng.\n", "Lỗi nhập liệu");
              //  else if (string.IsNullOrEmpty(txtNangXuatSanXuat.Text))
               //     MessageBox.Show("Vui lòng nhập năng suất sản xuất", "Lỗi nhập liệu");                 
                else if (txtSanLuongKeHoach.Value <= 0)
                    MessageBox.Show("Sản lượng kế hoạch của mặt hàng phải lớn hơn 0, hoặc bạn nhập sai định dạng dữ liệu.\n", "Lỗi nhập liệu");
                else
                {
                    var csp = new PMS.Data.Chuyen_SanPham();
                    csp.MaChuyen = chuyen.MaChuyen;
                    csp.MaSanPham = sanPham.MaSanPham;
                    csp.Thang = modelSelectMorth.Value;
                    csp.Nam = modelSelectYear.Value;
                    csp.STTThucHien = int.Parse(txtSTTThucHien.Text); 
                    csp.SanLuongKeHoach = (int)txtSanLuongKeHoach.Value;
                    csp.STT = sttChuyenSanPham;

                    #region ADD
                    var oldObj = BLLAssignmentForLine.Instance.CheckExists(sttChuyenSanPham, csp.MaChuyen, csp.MaSanPham);
                    if (oldObj != null)
                    {
                        #region
                        if (!oldObj.IsFinish)
                        {
                            #region  chưa ket thuc update binh thuong
                            if (MessageBox.Show("Thông tin phân công đã tồn tại, bạn có muốn thay đổi thông tin", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                if (csp.SanLuongKeHoach <= oldObj.LuyKeTH)
                                {
                                    csp.IsFinish = true;
                                    csp.IsFinishBTPThoatChuyen = true;
                                }
                                csp.STT = oldObj.STT;
                                csp.UpdatedDate = DateTime.Now;
                                var kq = BLLAssignmentForLine.Instance.Update(csp, false, frmMainNew.getBTPInLineByType);
                                if (kq.IsSuccess)
                                {
                                    LoadPhanCongRaDataGridView();
                                    ResetForm();
                                    // if (!frmMainNew.IsStopProcess)
                                    //     frmMainNew.RunAllProcess();
                                }
                                MessageBox.Show(kq.Messages[0].msg, kq.Messages[0].Title);
                            }
                            #endregion
                        }
                        else
                        {
                            if (MessageBox.Show("Bạn đã phân công mặt hàng " + sanPham.TenSanPham + " cho chuyền " + chuyen.TenChuyen + " vào thời gian " + oldObj.TimeAdd.ToShortDateString() + " với Sản lượng kế hoạch : " + oldObj.SanLuongKeHoach + "(sp) và đã sản xuất được :" + oldObj.LuyKeTH + "(sp).Kết thúc vào ngày " + dateNow + ". Bạn có muốn cập nhập thông tin cho phân công này không ?", "Cập nhập dữ liệu", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                #region da ket thuc update lai san luong ke hoach de san xuat tiep
                                if (csp.SanLuongKeHoach < oldObj.SanLuongKeHoach)
                                    MessageBox.Show("Sản lượng kế hoạch của Phân công mới không được nhỏ hơn Sản lượng kế hoạch của Phân công cũ.\nVui lòng nhập lại sản lượng kế hoạch.", "Lỗi nhập liệu");
                                else
                                {
                                    // update lai san luong ke hoach san xuat tiep
                                    csp.IsFinish = csp.SanLuongKeHoach > oldObj.SanLuongKeHoach ? false : true;
                                    csp.IsFinishBTPThoatChuyen = csp.IsFinish;
                                    csp.IsFinishNow = csp.IsFinish;
                                    csp.STT = oldObj.STT;
                                    csp.UpdatedDate = DateTime.Now;
                                    var kq = BLLAssignmentForLine.Instance.Update(csp, true, frmMainNew.getBTPInLineByType);
                                    if (kq.IsSuccess)
                                    {
                                        LoadPhanCongRaDataGridView();
                                        ResetForm();
                                        //  if (!frmMainNew.IsStopProcess)
                                        //     frmMainNew.RunAllProcess();
                                    }
                                    MessageBox.Show(kq.Messages[0].msg, kq.Messages[0].Title);
                                }
                                #endregion
                            }
                            else
                            {
                                //tao phan cong moi
                                csp.TimeAdd = DateTime.Now;
                                InsertAssignment(csp);
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region
                        csp.TimeAdd = DateTime.Now;
                        InsertAssignment(csp);
                        #endregion
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Exception: " + ex.Message, "Lỗi Ngoại Lệ");
            }
        }

        private void InsertAssignment(Chuyen_SanPham csp)
        {
            var kq = BLLAssignmentForLine.Instance.Insert(csp);
            if (kq.IsSuccess)
            {
                MessageBox.Show("Thêm phân công thành công.");
                LoadPhanCongRaDataGridView();
                ResetForm();
                // if (!frmMainNew.IsStopProcess)
                //       frmMainNew.RunAllProcess();
            }
            MessageBox.Show(kq.Messages[0].msg, kq.Messages[0].Title);
        }

        private void ResetForm()
        {
            btnAdd.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
         //   txtNangXuatSanXuat.Text = "0";
            txtSanLuongKeHoach.Value = 0;
            txtSTTThucHien.Value = 0;
            sttChuyenSanPham = 0;
            //if (string.IsNullOrEmpty(sttChuyenSanPham))
            //    gridControl1.Enabled = true;

            int maxSTT = 0;
            if (listPCC != null && listPCC.Count > 0)
            {
                maxSTT = listPCC.Max(c => c.STTThucHien);
                if (maxSTT < 0) maxSTT = 0;
            }
            maxSTT++;
            txtSTTThucHien.Text = maxSTT.ToString();
        }

        private ResponseBase SaveAssign(Chuyen_SanPham chuyen_sanpham, string TenChuyen, string TenSanPham)
        {
            var result = new ResponseBase();
            try
            {
                var dateNow = DateTime.Now;
                int morthNow = dateNow.Month;
                int yearNow = dateNow.Year;
                if (chuyen_sanpham.Nam >= yearNow)
                {
                    if (chuyen_sanpham.Nam == yearNow)
                    {
                        if (chuyen_sanpham.Thang >= morthNow)
                        {
                            var cspExist = BLLAssignmentForLine.Instance.Find(chuyen_sanpham.MaChuyen, chuyen_sanpham.MaSanPham);
                            if (cspExist == null)
                                result = BLLAssignmentForLine.Instance.Insert(chuyen_sanpham);
                            else
                            {
                                if (cspExist.LuyKeTH == 0)
                                {
                                    if (MessageBox.Show("Bạn đã phân công mặt hàng " + TenSanPham + " cho chuyền " + TenChuyen + " vào thời gian " + chuyen_sanpham.Thang + "/" + chuyen_sanpham.Nam + " rồi. Bạn có muốn cập nhập thông tin này ?", "Cập nhập dữ liệu", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                    {
                                        var reUpdate = BLLAssignmentForLine.Instance.Update(chuyen_sanpham, true, frmMainNew.getBTPInLineByType);
                                        if (!reUpdate.IsSuccess)
                                            MessageBox.Show("Cập nhập thông tin phân công mặt hàng " + TenSanPham + " cho chuyền " + TenChuyen + " vào thời gian " + chuyen_sanpham.Thang + "/" + chuyen_sanpham.Nam + "không thành công.");
                                        else
                                            result = reUpdate;
                                    }
                                }
                                else
                                    MessageBox.Show("Bạn đã phân công mặt hàng " + TenSanPham + " cho chuyền " + TenChuyen + " vào thời gian " + chuyen_sanpham.Thang + "/" + chuyen_sanpham.Nam + " rồi. Việc phân công có thông tin sản xuất nên không thể chỉnh sửa");
                            }
                        }
                        else
                        {
                            result.Messages.Add(new PMS.Business.Models.Message()
                            {
                                Title = "AddListPCC",
                                msg = "Lỗi: Tháng nhập phân công hàng cho chuyền không được nhỏ hơn tháng hiện tại."
                            });
                        }
                    }
                    else
                    {
                        var cspExist = BLLAssignmentForLine.Instance.Find(chuyen_sanpham.MaChuyen, chuyen_sanpham.MaSanPham);
                        if (cspExist == null)
                        {
                            result = BLLAssignmentForLine.Instance.Insert(chuyen_sanpham);
                        }
                        else
                        {
                            if (chuyen_sanpham.LuyKeTH == 0)
                            {
                                if (MessageBox.Show("Bạn đã phân công mặt hàng " + TenSanPham + " cho chuyền " + TenChuyen + " vào thời gian " + chuyen_sanpham.Thang + "/" + chuyen_sanpham.Nam + " rồi. Bạn có muốn cập nhập thông tin này ?", "Cập nhập dữ liệu", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    var reUpdate = BLLAssignmentForLine.Instance.Update(chuyen_sanpham, true, frmMainNew.getBTPInLineByType);
                                    if (!reUpdate.IsSuccess)
                                        MessageBox.Show("Cập nhập thông tin phân công mặt hàng " + TenSanPham + " cho chuyền " + TenChuyen + " vào thời gian " + chuyen_sanpham.Thang + "/" + chuyen_sanpham.Nam + "không thành công.");
                                    else
                                        result = reUpdate;
                                }
                            }
                            else
                                MessageBox.Show("Bạn đã phân công mặt hàng " + TenSanPham + " cho chuyền " + TenChuyen + " vào thời gian " + chuyen_sanpham.Thang + "/" + chuyen_sanpham.Nam + " rồi. Việc phân công có thông tin sản xuất nên không thể chỉnh sửa");
                        }
                    }
                }
                else
                {
                    result.Messages.Add(new PMS.Business.Models.Message()
                    {
                        Title = "AddListPCC",
                        msg = "Lỗi: Năm nhập phân công hàng cho chuyền không được nhỏ hơn năm hiện tại."
                    });
                }
            }
            catch (Exception)
            {
                result.Messages.Add(new PMS.Business.Models.Message()
                {
                    Title = "AddListPCC",
                    msg = "Lỗi: việc phân công Mặt Hàng " + TenSanPham + " cho chuyền " + TenChuyen + " thất bại."
                });
            }
            return result;
        }

        private void btnReLine_Click(object sender, EventArgs e)
        {
            LoadDSChuyen();
        }

        private void btnReCommo_Click(object sender, EventArgs e)
        {
            LoadDSSamPham();
        }
    }
}
