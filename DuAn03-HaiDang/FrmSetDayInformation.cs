using DuAn03_HaiDang;
using DuAn03_HaiDang.DAO;
using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.Helper;
using DuAn03_HaiDang.KeyPad_Chuyen.dao;
using DuAn03_HaiDang.Model;
using PMS.Business;
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
    public partial class FrmSetDayInformation : Form
    {
        Chuyen_SanPhamDAO chuyen_sanphamDAO = new Chuyen_SanPhamDAO();
        ThanhPhamDAO thanhphamDAO = new ThanhPhamDAO();
        BTPDAO btpDAO = new BTPDAO();
        ChuyenDAO chuyenDAO = new ChuyenDAO();
        NangXuatDAO nangxuatDAO = new NangXuatDAO();
        CumDAO cumDAO = new CumDAO();
        ClusterDAO clusterDAO = new ClusterDAO();
        NangSuatCumDAO nangSuatCumDAO = new NangSuatCumDAO();
        //  ShiftDAO shiftDAO = new ShiftDAO();
        private int dailyWorkerInfoId = 0;
        private List<ChuyenSanPhamModel> listPCC;
        FrmMainNew frmMainNew;
        bool hieusuatChange = false;
        bool dinhmucChange = false;
        double thoigianchetao = 0;
        public FrmSetDayInformation(FrmMainNew _frmMainNew)
        {
            InitializeComponent();
            this.frmMainNew = _frmMainNew;
        }

        private void FrmSetDayInformation_Load(object sender, EventArgs e)
        {
            try
            {
                LoadDSChuyen();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void butAdd_Click(object sender, EventArgs e)
        {
            try
            {
                SetProductivityWorker();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }


        private void gridView_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                int.TryParse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Id").ToString(), out dailyWorkerInfoId);
                txtNangSuatLaoDong.Text = gridView.GetRowCellValue(gridView.FocusedRowHandle, "NangXuatLaoDong").ToString();
                txtLaoDongChuyen.Value = int.Parse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "LaoDongChuyen").ToString());
                cbbSanPham.Text = gridView.GetRowCellValue(gridView.FocusedRowHandle, "CommoName").ToString();
                txtLean.Text = gridView.GetRowCellValue(gridView.FocusedRowHandle, "LeanKH").ToString();
                chkbShowLCD.Checked = bool.Parse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "ShowLCD").ToString());
                numHieuSuat.Text = gridView.GetRowCellValue(gridView.FocusedRowHandle, "HieuSuat").ToString();
                //   numDinhMucNgay.Value = decimal.Parse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "DinhMucNgay").ToString());
                btnAdd.Enabled = false;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void cbbChuyen_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtLaoDongChuyen.Value = 0;
                txtNangSuatLaoDong.Text = "0";
                chkIsStopOnDay.Checked = false;
                LoadPCCToCbbSanPham();
                GetDayInformationToGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void LoadPCCToCbbSanPham()
        {
            cbbSanPham.DataSource = null;
            cbbSanPham.Text = "";
            var chuyen = ((LineModel)cbbChuyen.SelectedItem);
            if (chuyen != null)
            {
                listPCC = null;
                listPCC = BLLAssignmentForLine.Instance.GetListChuyenSanPham(chuyen.MaChuyen, false);
                if (listPCC != null && listPCC.Count > 0)
                {
                    cbbSanPham.DataSource = listPCC;
                    cbbSanPham.DisplayMember = "CommoName";
                    //Gán mã chuyền cho form main để gửi dữ liệu xuống bảng
                    FrmMainNew.strMaChuyenTatca = chuyen.MaChuyen;
                }
                else
                    MessageBox.Show("Bạn chưa phân công mặt hàng cho chuyền này, Vui lòng thực hiện thao tác Phân Công Cho Chuyên", "Lỗi Thực Hiện", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show("Lỗi: Không tìm thấy mã chuyền.");
        }

        private void LoadDSChuyen()
        {
            cbbChuyen.DataSource = null;
            cbbChuyen.Text = "(--Chọn Chuyền--)";
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

        private void GetDayInformationToGridView()
        {
            try
            {
                Chuyen line = (Chuyen)cbbChuyen.SelectedItem;
                if (line != null)
                {
                    gridControl.DataSource = null;
                    var listInformation = BLLProductivity.GetDailyWorkerInformation(line.MaChuyen, dtpNgayLamViec.Value);
                    if (listInformation != null && listInformation.Count > 0)
                        gridControl.DataSource = listInformation;
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// new method date : 14/9/2016 - Hai
        /// </summary>
        /// <param name="sttChuyen_SanPham"></param>
        /// <param name="thoiGianCheTaoSanPham"></param>
        /// <param name="maChuyen"></param>
        private void SaveData(int sttChuyen_SanPham, double thoiGianCheTaoSanPham, int maChuyen)
        {
            try
            {
                // Kiem tra tinh trang cap nhap trang thai IsFinishBTPThoatChuyen
                // Update nhung row cap nhap sai hoac khong cap nhap
                chuyen_sanphamDAO.UpdateIsFinishBTPChuyen();
                if (maChuyen != 0)
                {
                    var thanhpham = new ThanhPhamModel();
                    thanhpham.Id = dailyWorkerInfoId;
                    thanhpham.Ngay = dtpNgayLamViec.Value.Day + "/" + dtpNgayLamViec.Value.Month + "/" + dtpNgayLamViec.Value.Year;
                    thanhpham.STTChuyen_SanPham = sttChuyen_SanPham;
                    thanhpham.CreatedDate = DateTime.Now;
                    thanhpham.ShowLCD = chkbShowLCD.Checked;
                    thanhpham.HieuSuat = double.Parse(numHieuSuat.Text);

                    double nangSuatLaoDong = 0;
                    double.TryParse(txtNangSuatLaoDong.Text, out nangSuatLaoDong);
                    thanhpham.NangXuatLaoDong = nangSuatLaoDong;

                    nangSuatLaoDong = 0;
                    double.TryParse(txtLean.Text, out nangSuatLaoDong);
                    thanhpham.LeanKH = nangSuatLaoDong;
                    thanhpham.LaoDongChuyen = (int)txtLaoDongChuyen.Value;
                    thanhpham.LineId = maChuyen;

                    // Thông tin nang xuat                
                    var nangxuat = new NangXuat();
                    nangxuat.Ngay = dtpNgayLamViec.Value.Day + "/" + dtpNgayLamViec.Value.Month + "/" + dtpNgayLamViec.Value.Year;
                    nangxuat.STTCHuyen_SanPham = thanhpham.STTChuyen_SanPham;
                    nangxuat.DinhMucNgay = (float)Math.Round((thanhpham.NangXuatLaoDong * thanhpham.LaoDongChuyen), 1);
                    nangxuat.NhipDoSanXuat = (float)Math.Round((((thoiGianCheTaoSanPham * 100) / double.Parse(numHieuSuat.Text)) / thanhpham.LaoDongChuyen), 1);
                    nangxuat.TimeLastChange = DateTime.Now.TimeOfDay;
                    nangxuat.IsStopOnDay = chkIsStopOnDay.Checked;
                    if (nangxuat.IsStopOnDay)
                        nangxuat.TimeStopOnDay = DateTime.Now.TimeOfDay;

                    var chuyenSanPham = BLLAssignmentForLine.Instance.LayLuyKeTHandKeHoachTheoSTT(sttChuyen_SanPham);
                    bool isEndDate = false;
                    int soLuongConLai = chuyenSanPham.SanLuongKeHoach - chuyenSanPham.LuyKeTH;
                    if (chuyenSanPham != null && soLuongConLai <= nangxuat.DinhMucNgay)
                    {
                        /// check lai DMNgay
                        isEndDate = true;
                        nangxuat.DinhMucNgay = soLuongConLai;
                        // thanhpham.NangXuatLaoDong = nangxuat.DinhMucNgay / thanhpham.LaoDongChuyen;
                    }
                    nangxuat.IsEndDate = isEndDate;
                    thanhpham.NangSuatObj = nangxuat;
                    nangxuat.TGCheTaoSP = (int)((thoigianchetao * 100) / thanhpham.HieuSuat);

                    if (thanhpham.NangXuatLaoDong > 0 && thanhpham.LaoDongChuyen > 0)
                    {
                        var rs = BLLProductivity.InsertOrUpdate_TP(thanhpham, frmMainNew.getBTPInLineByType, frmMainNew.calculateNormsdayType, frmMainNew.TypeOfCaculateDayNorms);
                        if (rs.IsSuccess)
                        {
                            GetDayInformationToGridView();
                            ResetForm();
                            dailyWorkerInfoId = 0;
                            SetProductivityWorker();
                            HelperControl.ResetKeypad(maChuyen, false, frmMainNew);
                        }
                        MessageBox.Show(rs.Messages[0].msg, rs.Messages[0].Title);
                    }
                    else
                        MessageBox.Show("Lỗi: Bạn chưa nhập đúng thông tin Năng Suất Lao Động hoặc Số Lao Động", "Lỗi thao tác", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("Bạn chưa chọn chuyền sản xuất. Vui lòng thực hiện thao tác này...", "Lỗi thao tác", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void dtpNgayLamViec_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                GetDayInformationToGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void cbbSanPham_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                SetProductivityWorker();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void SetProductivityWorker()
        {
            try
            {
                //  Chuyen chuyen = ((Chuyen)cbbChuyen.SelectedItem);
                var chuyen = ((LineModel)cbbChuyen.SelectedItem);
                if (chuyen != null)
                {
                    if (listPCC != null && listPCC.Count > 0)
                    {
                        var chuyenSanPham = (ChuyenSanPhamModel)cbbSanPham.SelectedItem;
                        if (chuyenSanPham != null)
                        {
                            thoigianchetao = chuyenSanPham.ProductionTime;
                            //TimeSpan timeinWork = shiftDAO.TimeIsWorkAllDayOfLine(chuyen.MaChuyen);
                            TimeSpan timeinWork = HelperControl.TimeIsWorkAllDayOfLine(chuyen.Shifts);
                            if (timeinWork.TotalSeconds == 0)
                                MessageBox.Show("Chuyền chưa có thông tin giờ làm việc trong ngày. Vui lòng kiểm tra ca làm việc của chuyền", "Lỗi ca làm việc", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            else
                            {
                                int TotalSecond = (int)timeinWork.TotalSeconds;
                                var listCSP = listPCC.Where(c => c.STTThucHien < chuyenSanPham.STTThucHien).OrderBy(c => c.STTThucHien).ToList();

                                if (listCSP.Count > 0)
                                {
                                    foreach (var csp in listCSP)
                                    {
                                        var ngay = dtpNgayLamViec.Value.Day + "/" + dtpNgayLamViec.Value.Month + "/" + dtpNgayLamViec.Value.Year;
                                        // var nangSuatNgay = nangxuatDAO.TTNangXuatTrongNgay(ngay, csp.STT);
                                        var nangSuatNgay = BLLProductivity.TTNangXuatTrongNgay(ngay, csp.STT);
                                        if (nangSuatNgay != null)
                                        {
                                            if (nangSuatNgay.IsEndDate)
                                            {
                                                //   var thanhPhamNgay = thanhphamDAO.GetThanhPhamByNgayAndSTT(ngay, csp.STT);
                                                var thanhPhamNgay = BLLProductivity.GetThanhPhamByNgayAndSTT(ngay, csp.STT);
                                                if (thanhPhamNgay == null)
                                                {
                                                    MessageBox.Show("Không tìm thấy thông tin ngày của măt hàng trước.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                    return;
                                                }
                                                //else
                                                //{
                                                //    TotalSecond = TotalSecond - (int)(thanhPhamNgay.NangXuatLaoDong * csp.NangXuatSanXuat);
                                                //}
                                            }
                                            else
                                            {
                                                MessageBox.Show("Định mức sản xuất của chuyền trong ngày đã đủ. Bạn không cần nhập thêm thông tin ngày", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Vui lòng nhập thông tin ngày tuần tự theo đúng thứ tự sản xuất trong phân công chuyền", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            return;
                                        }
                                    }
                                }
                                var spcuachuyen = ((ChuyenSanPhamModel)cbbSanPham.SelectedItem);
                                txtNangSuatLaoDong.Text = Math.Round(TotalSecond / ((spcuachuyen.ProductionTime * 100) / double.Parse(numHieuSuat.Text)), 2).ToString();
                               
                                numDinhMucNgay.Value = (decimal)Math.Round((double)(float.Parse(txtNangSuatLaoDong.Text) * (double)txtLaoDongChuyen.Value), 0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dailyWorkerInfoId != 0)
                {
                    if (MessageBox.Show("Bạn có muốn xoá không?", "Xoá đối tượng", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var spcuachuyen = ((ChuyenSanPhamModel)cbbSanPham.SelectedItem);
                        // var result = thanhphamDAO.XoaOBJ(spcuachuyen.STT.ToString(), dtpNgayLamViec.Value);
                        var result = BLLDayInfo.Delete(spcuachuyen.STT, (dtpNgayLamViec.Value.Day + "/" + dtpNgayLamViec.Value.Month + "/" + dtpNgayLamViec.Value.Year));
                        if (result)
                        {
                            MessageBox.Show("Xoá thành công.");
                            GetDayInformationToGridView();
                            ResetForm();
                            dailyWorkerInfoId = 0;
                            SetProductivityWorker();
                        }
                        else
                            MessageBox.Show("Xoá thất bại.");
                    }
                }
                else
                    MessageBox.Show("Bạn chưa chọn đối tượng để xoá");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void Save()
        {
            try
            {
                var spcuachuyen = ((ChuyenSanPhamModel)cbbSanPham.SelectedItem);
                var chuyen = ((LineModel)cbbChuyen.SelectedItem);
                if (spcuachuyen != null && chuyen != null)
                    SaveData(spcuachuyen.STT, spcuachuyen.ProductionTime, chuyen.MaChuyen);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void ResetForm()
        {
            txtNangSuatLaoDong.Text = "0";
            txtLaoDongChuyen.Value = 0;
            txtLean.Text = "0";

            btnAdd.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;

            //cbbChuyen.SelectedIndex = 0;
            dailyWorkerInfoId = 0;

            chkIsStopOnDay.Checked = false;
            dtpNgayLamViec.Value = DateTime.Now;
        }

        private void btnReLine_Click(object sender, EventArgs e)
        {
            LoadDSChuyen();
        }

        private void btnReCommo_Click(object sender, EventArgs e)
        {
            LoadPCCToCbbSanPham();
        }

        private void cbbSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void numHieuSuat_ValueChanged(object sender, EventArgs e)
        { 
                SetProductivityWorker(); 
        }

        private void numDinhMucNgay_ValueChanged(object sender, EventArgs e)
        { 
                var chuyen = ((LineModel)cbbChuyen.SelectedItem);
                if (chuyen != null)
                {
                    TimeSpan timeinWork = HelperControl.TimeIsWorkAllDayOfLine(chuyen.Shifts);
                    if (timeinWork.TotalSeconds == 0)
                        MessageBox.Show("Chuyền chưa có thông tin giờ làm việc trong ngày. Vui lòng kiểm tra ca làm việc của chuyền", "Lỗi ca làm việc", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {

                        int TotalSecond = (int)timeinWork.TotalSeconds;
                        var spcuachuyen = ((ChuyenSanPhamModel)cbbSanPham.SelectedItem);
                        int laodong = int.Parse(txtLaoDongChuyen.Text);
                        int dinhmuc = (int)numDinhMucNgay.Value;

                        if (dinhmuc > 0 && laodong > 0)
                        {
                            thoigianchetao = spcuachuyen.ProductionTime;
                            double sanluong1laodong = dinhmuc / laodong;
                            double sanluongtheoTGchetao = (TotalSecond / spcuachuyen.ProductionTime);
                           
                            numHieuSuat.Text = ((sanluong1laodong / sanluongtheoTGchetao) * 100).ToString();
                            double nsld = (TotalSecond / ((spcuachuyen.ProductionTime * 100) / double.Parse(numHieuSuat.Text)));
                            txtNangSuatLaoDong.Text = nsld+"";
                         }
                    }
                } 

        }

        private void numHieuSuat_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
       (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

         
    }
}
