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



        LineModel selectedLine = null;
        double secondsWorkOfLine = 0;
        ChuyenSanPhamModel selectedProduct = null;
        string selectedDate = null;
        double nangSuatLaoDong100PhanTram = 0;
        public FrmSetDayInformation(FrmMainNew _frmMainNew)
        {
            InitializeComponent();
            this.frmMainNew = _frmMainNew;
        }

        private void FrmSetDayInformation_Load(object sender, EventArgs e)
        {
            try
            {
                selectedDate = dtpNgayLamViec.Value.ToString("d/M/yyyy");
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

        bool bindFormGrid = false;
        private void gridView_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                bindFormGrid = true;
                int.TryParse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Id").ToString(), out dailyWorkerInfoId);
                txtNangSuatLaoDong.Text = gridView.GetRowCellValue(gridView.FocusedRowHandle, "NangXuatLaoDong").ToString();
                txtLaoDongChuyen.Value = int.Parse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "LaoDongChuyen").ToString());
                numOff.Value = int.Parse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "LDOff").ToString());
                numVacation.Value = int.Parse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "LDVacation").ToString());
                numPregnant.Value = int.Parse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "LDPregnant").ToString());
                numNew.Value = int.Parse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "LDNew").ToString());
                cbbSanPham.Text = gridView.GetRowCellValue(gridView.FocusedRowHandle, "CommoName").ToString();
                txtLean.Text = gridView.GetRowCellValue(gridView.FocusedRowHandle, "LeanKH").ToString();
                chkbShowLCD.Checked = bool.Parse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "ShowLCD").ToString());
                numHieuSuat.Text = gridView.GetRowCellValue(gridView.FocusedRowHandle, "HieuSuat").ToString();

                txtNangSuatLaoDong.Text = Math.Round(nangSuatLaoDong100PhanTram * double.Parse(numHieuSuat.Text) / 100,2).ToString();
                numDinhMucNgay.Value = (decimal)Math.Round((double)(float.Parse(txtNangSuatLaoDong.Text) * (double)txtLaoDongChuyen.Value), 0);

              //  var nsld = Math.Round(double.Parse(txtNangSuatLaoDong.Text), 2);
             //   numDinhMucNgay.Value = (decimal)(Math.Round(nsld * (double)txtLaoDongChuyen.Value));
                btnAdd.Enabled = false;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true; 
                bindFormGrid = false;
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
                numOff.Value = 0;
                numVacation.Value = 0;
                numPregnant.Value = 0;
                numNew.Value = 0;
                txtNangSuatLaoDong.Text = "0";
                chkIsStopOnDay.Checked = false;
                LoadPCCToCbbSanPham();
                GetDayInformationToGridView();

                selectedLine = ((LineModel)cbbChuyen.SelectedItem);
                if (selectedLine != null)
                {
                    TimeSpan timeinWork = HelperControl.TimeIsWorkAllDayOfLine(selectedLine.Shifts);
                    if (timeinWork.TotalSeconds == 0)
                        MessageBox.Show("Chuyền chưa có thông tin giờ làm việc trong ngày. Vui lòng kiểm tra ca làm việc của chuyền", "Lỗi ca làm việc", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        secondsWorkOfLine = timeinWork.TotalSeconds;
                }

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
                    thanhpham.Ngay = selectedDate;
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
                    thanhpham.LDOff = (int)numOff.Value;
                    thanhpham.LDNew = (int)numNew.Value;
                    thanhpham.LDPregnant = (int)numPregnant.Value;
                    thanhpham.LDVacation = (int)numVacation.Value;
                    thanhpham.LineId = maChuyen;

                    // Thông tin nang xuat                
                    var nangxuat = new NangXuat();
                    nangxuat.Ngay = selectedDate;
                    nangxuat.STTCHuyen_SanPham = thanhpham.STTChuyen_SanPham;
                    nangxuat.DinhMucNgay = (double)numDinhMucNgay.Value; // (float)Math.Round((thanhpham.NangXuatLaoDong * thanhpham.LaoDongChuyen), 1);
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

                    nangxuat.TGCheTaoSP =  ((thoigianchetao * 100) / thanhpham.HieuSuat);
                    thanhpham.NangSuatObj = nangxuat;
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
                selectedDate = dtpNgayLamViec.Value.ToString("d/M/yyyy");
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
             //   SetProductivityWorker();
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
                if (selectedLine != null)
                {
                    if (listPCC != null && listPCC.Count > 0)
                    {
                        if (selectedProduct != null)
                        {
                            thoigianchetao = selectedProduct.ProductionTime;

                            if (secondsWorkOfLine == 0)
                                MessageBox.Show("Chuyền chưa có thông tin giờ làm việc trong ngày. Vui lòng kiểm tra ca làm việc của chuyền", "Lỗi ca làm việc", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            else
                            {
                                var listCSP = listPCC.Where(c => c.STTThucHien < selectedProduct.STTThucHien).OrderBy(c => c.STTThucHien).ToList();
                                if (listCSP.Count > 0)
                                {
                                    foreach (var csp in listCSP)
                                    {
                                        var nangSuatNgay = BLLProductivity.TTNangXuatTrongNgay(selectedDate, csp.STT);
                                        if (nangSuatNgay != null)
                                        {
                                            if (nangSuatNgay.IsEndDate)
                                            {
                                                var thanhPhamNgay = BLLProductivity.GetThanhPhamByNgayAndSTT(selectedDate, csp.STT);
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
                                            //    MessageBox.Show("Định mức sản xuất của chuyền trong ngày đã đủ. Bạn không cần nhập thêm thông tin ngày", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                //return;
                                             }
                                        }
                                        else
                                        {
                                          //  MessageBox.Show("Vui lòng nhập thông tin ngày tuần tự theo đúng thứ tự sản xuất trong phân công chuyền", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                           // return;
                                        }
                                    }
                                }
                                tinhtheohieuxuat = true;
                                nangSuatLaoDong100PhanTram = Math.Round(secondsWorkOfLine / selectedProduct.ProductionTime, 2);
                                txtNSLDChuan.Text = nangSuatLaoDong100PhanTram.ToString();
                                txtNangSuatLaoDong.Text = Math.Round(nangSuatLaoDong100PhanTram * double.Parse(numHieuSuat.Text) / 100, 2).ToString();
                                numDinhMucNgay.Value = (decimal)Math.Round((double)(float.Parse(txtNangSuatLaoDong.Text) * (double)txtLaoDongChuyen.Value), 0);

                                //  txtNangSuatLaoDong.Text = Math.Round(secondsWorkOfLine / ((selectedProduct.ProductionTime * 100) / double.Parse(numHieuSuat.Text)), 2).ToString();
                                // var oldNSLD = Math.Round(secondsWorkOfLine / ((selectedProduct.ProductionTime * 100) / double.Parse(numHieuSuat.Text)), 2).ToString();
                                //  numDinhMucNgay.Value = (decimal)Math.Round((double)(float.Parse(txtNangSuatLaoDong.Text) * (double)txtLaoDongChuyen.Value), 0);
                                //  var oldDMN = (decimal)Math.Round((double)(float.Parse(txtNangSuatLaoDong.Text) * (double)txtLaoDongChuyen.Value), 0);
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
            numOff.Value = 0;
            numVacation.Value = 0;
            numPregnant.Value = 0;
            numNew.Value = 0;
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
            selectedProduct = (ChuyenSanPhamModel)cbbSanPham.SelectedItem;
            SetProductivityWorker();
        }

        private void numHieuSuat_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
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
            catch (Exception)
            {
            }

        }


        bool tinhtheohieuxuat = false,
                   tinhtheodinhmuc = false;
        private void TinhTheoHieuXuat()
        {
            try
            {
                //if (tinhtheodinhmuc)
                //    tinhtheodinhmuc = false;
                //else
                //{
                    tinhtheohieuxuat = true;
                    if (txtLaoDongChuyen.Value <= 0)
                        MessageBox.Show("Vui lòng nhập số lao động chuyền lớn hơn 0", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else if (string.IsNullOrEmpty(numHieuSuat.Text) && Convert.ToInt32(numHieuSuat.Text) <= 0)
                        MessageBox.Show("Vui lòng nhập hiệu xuất sản suất lớn hơn 0", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        txtNangSuatLaoDong.Text = Math.Round(nangSuatLaoDong100PhanTram * double.Parse(numHieuSuat.Text) / 100, 1).ToString();
                        numDinhMucNgay.Value = (decimal)Math.Round((double)(float.Parse(txtNangSuatLaoDong.Text) * (double)txtLaoDongChuyen.Value), 0);
                    }
               // }
            }
            catch (Exception)
            {
            }

        }

        private void TinhTheoDinhMuc()
        {
            try
            {
                //if (tinhtheohieuxuat)
                //    tinhtheohieuxuat = false;
                //else
                //{
                    tinhtheodinhmuc = true;
                    if (txtLaoDongChuyen.Value <= 0)
                        MessageBox.Show("Vui lòng nhập số lao động chuyền lớn hơn 0", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else if (numDinhMucNgay.Value <= 0)
                        MessageBox.Show("Vui lòng nhập định mức ngày lớn hơn 0", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        var nsld = Math.Round(numDinhMucNgay.Value / txtLaoDongChuyen.Value, 1);
                        txtNangSuatLaoDong.Text = nsld.ToString();
                        numHieuSuat.Text = Math.Round((double)nsld / nangSuatLaoDong100PhanTram * 100, 1).ToString();
                    }
                //}
            }
            catch (Exception)
            {
            }

        }

        private void numHieuSuat_TextChanged(object sender, EventArgs e)
        {
            //if (!bindFormGrid)
            //{
            //    tinhtheohieuxuat = true;
                
            //}
        }

        private void txtLaoDongChuyen_ValueChanged(object sender, EventArgs e)
        {
            SetProductivityWorker();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnTinhTheoHieuSuat_Click(object sender, EventArgs e)
        {
TinhTheoHieuXuat();
        }

        private void btnTinhTheoDinhMuc_Click(object sender, EventArgs e)
        {
TinhTheoDinhMuc();
        }

        private void numDinhMucNgay_ValueChanged(object sender, EventArgs e)
        {
            //if (!bindFormGrid)
            //{
            //    tinhtheodinhmuc = true;
                
            //}
        }


    }
}
