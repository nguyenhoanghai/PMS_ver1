using DevComponents.DotNetBar;
using DevExpress.XtraTab;
using DuAn03_HaiDang.DAO;
using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.KeyPad_Chuyen.dao;
using DuAn03_HaiDang.KeyPad_Chuyen.pojo;
using DuAn03_HaiDang.POJO;
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
using System.Globalization;
using DuAn03_HaiDang;

namespace QuanLyNangSuat
{
    public partial class FrmDayInfo : Form
    {
        private ChuyenDAO chuyenDAO;
        private Chuyen_SanPhamDAO chuyen_sanphamDAO;
        // private DataTable dtSanPham = null;
        private NangXuatDAO nangSuatDAO;
        private BTPDAO btpDAO;
        private int STTC_SP = 0;
        private int appId = 0;
        private string seletedDate = string.Empty;
        FrmMainNew frmMainNew;
        public FrmDayInfo(FrmMainNew _frmMainNew)
        {
            InitializeComponent();
            chuyenDAO = new ChuyenDAO();
            chuyen_sanphamDAO = new Chuyen_SanPhamDAO();
            nangSuatDAO = new NangXuatDAO();
            //   dtSanPham = new DataTable();
            btpDAO = new BTPDAO();
            frmMainNew = _frmMainNew;
        }

        private void FrmDayInfo_Load(object sender, EventArgs e)
        {
            try
            {
                txtNgay.Text = frmMainNew.todayStr;
                LoadListLineForCBB(0);

                cbProType.DisplayMember = "Text";
                cbProType.ValueMember = "Value";
                cbProType.Items.Add(new { Text = "Thoát Chuyền", Value = "2" });
                cbProType.Items.Add(new { Text = "Kiểm Đạt", Value = "1" });
                cbProType.Items.Add(new { Text = "Bán Thành Phẩm", Value = "3" });
                cbProType.Items.Add(new { Text = "BTP Phối bộ hoàn chỉnh", Value = "4" });
                cbProType.Items.Add(new { Text = "Lỗi", Value = "0" });
                cbProType.SelectedIndex = 0;

                LoadError();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void LoadError()
        {
            cbError.DataSource = null;
            var err = BLLError.GetAll();
            cbError.DataSource = err;
            cbError.DisplayMember = "Name";
            cbError.ValueMember = "Id";
        }
        private void xtraTabControl1_SelectedPageChanging(object sender, TabPageChangingEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 1)
            {
                LoadListLineForCBB(0);
                Reset();
            }
            else
                LoadListLineForCBB(1);
        }

        #region NHAP SAN LUONG NGAY

        private void cboChuyen_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadDSSanPham(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void cboSanPham_0_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                LoadALLNSOfPCC();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void LoadALLNSOfPCC()
        {
            try
            {
                var chuyen = ((LineModel)cboChuyen_0.SelectedItem);
                //  var sanpham = ((SanPhamCuaChuyen)cboSanPham.SelectedItem);
                var sanpham = ((AssignmentForLineModel)cboSanPham_0.SelectedItem);
                dgv.Rows.Clear();
                // tc
                int tang = 0, giam = 0;
                lbtc_t.Text = tang.ToString();
                lbtc_g.Text = giam.ToString();
                lbtc_tong.Text = (tang - giam).ToString();

                //kcs 
                lbkcs_t.Text = tang.ToString();
                lbkcs_g.Text = giam.ToString();
                lbkcstong.Text = (tang - giam).ToString();

                //btp
                lbbtp_t.Text = tang.ToString();
                lbbtp_g.Text = giam.ToString();
                lbbtp_tong.Text = (tang - giam).ToString();

                //btp
                lberr_t.Text = tang.ToString();
                lberr_g.Text = giam.ToString();
                lberr_tong.Text = (tang - giam).ToString();
                if (chuyen != null && sanpham != null)
                {
                    lblSanLuongKeHoach.Text = sanpham.SanLuongKeHoach.ToString();

                    // var nangSuatTrongNgay = nangSuatDAO.TTNangXuatTrongNgay(dtpNgayThucHien.Value.Date.ToString(), sanpham.STT);
                    var nangSuatTrongNgay = BLLProductivity.TTNangXuatTrongNgay((dtpNgayThucHien.Value.Day + "/" + dtpNgayThucHien.Value.Month + "/" + dtpNgayThucHien.Value.Year), sanpham.STT);
                    lblDinhMucNgay.Text = (nangSuatTrongNgay != null ? (int)nangSuatTrongNgay.DinhMucNgay : 0).ToString();

                    var info = BLLDayInfo.GetInforByDate(DateTime.Now, chuyen.MaChuyen, sanpham.MaSanPham, sanpham.STT);

                    if (info != null && info.Count > 0)
                    {

                        DataGridViewRow row;
                        DataGridViewCell cell;
                        foreach (var item in info)
                        {
                            row = new DataGridViewRow();
                            cell = new DataGridViewTextBoxCell();
                            cell.Value = item.STT;
                            row.Cells.Add(cell);

                            cell = new DataGridViewTextBoxCell();
                            cell.Value = item.CommoName;
                            row.Cells.Add(cell);

                            cell = new DataGridViewTextBoxCell();
                            cell.Value = item.ClusterName;
                            row.Cells.Add(cell);

                            cell = new DataGridViewTextBoxCell();
                            cell.Value = item.ProductOutputTypeId;
                            row.Cells.Add(cell);

                            cell = new DataGridViewTextBoxCell();
                            cell.Value = item.ProType;
                            row.Cells.Add(cell);

                            cell = new DataGridViewTextBoxCell();
                            cell.Value = item.ErrorId;
                            row.Cells.Add(cell);

                            cell = new DataGridViewTextBoxCell();
                            cell.Value = item.ErrorName;
                            row.Cells.Add(cell);

                            cell = new DataGridViewTextBoxCell();
                            cell.Value = item.CommandTypeId;
                            row.Cells.Add(cell);

                            cell = new DataGridViewTextBoxCell();
                            cell.Value = item.CommandType;
                            row.Cells.Add(cell);

                            cell = new DataGridViewTextBoxCell();
                            cell.Value = item.ThanhPham;
                            row.Cells.Add(cell);

                            cell = new DataGridViewTextBoxCell();
                            cell.Value = item.KeypadType;
                            row.Cells.Add(cell);

                            cell = new DataGridViewTextBoxCell();
                            cell.Value = item.Time.ToString(@"hh\:mm\:ss");
                            row.Cells.Add(cell);

                            cell = new DataGridViewTextBoxCell();
                            cell.Value = item.EquipmentId;
                            row.Cells.Add(cell);

                            dgv.Rows.Add(row);
                        }

                        for (int i = 1; i < dgv.Rows.Count; i += 2)
                        {
                            dgv.Rows[i].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                        }

                        // tc
                        tang = 0; giam = 0;
                        tang = info.Where(x => x.ProductOutputTypeId == (int)eProductOutputType.TC && x.CommandTypeId == (int)eCommandRecive.ProductIncrease).Sum(x => x.ThanhPham);
                        giam = info.Where(x => x.ProductOutputTypeId == (int)eProductOutputType.TC && x.CommandTypeId == (int)eCommandRecive.ProductReduce).Sum(x => x.ThanhPham);
                        lbtc_t.Text = tang.ToString();
                        lbtc_g.Text = giam.ToString();
                        lbtc_tong.Text = (tang - giam).ToString();

                        //kcs
                        tang = 0; giam = 0;
                        tang = info.Where(x => x.ProductOutputTypeId == (int)eProductOutputType.KCS && x.CommandTypeId == (int)eCommandRecive.ProductIncrease).Sum(x => x.ThanhPham);
                        giam = info.Where(x => x.ProductOutputTypeId == (int)eProductOutputType.KCS && x.CommandTypeId == (int)eCommandRecive.ProductReduce).Sum(x => x.ThanhPham);
                        lbkcs_t.Text = tang.ToString();
                        lbkcs_g.Text = giam.ToString();
                        lbkcstong.Text = (tang - giam).ToString();

                        //btp
                        tang = 0; giam = 0;
                        tang = info.Where(x => x.ProductOutputTypeId == (int)eProductOutputType.BTP && x.CommandTypeId == (int)eCommandRecive.BTPIncrease).Sum(x => x.ThanhPham);
                        giam = info.Where(x => x.ProductOutputTypeId == (int)eProductOutputType.BTP && x.CommandTypeId == (int)eCommandRecive.BTPReduce).Sum(x => x.ThanhPham);
                        lbbtp_t.Text = tang.ToString();
                        lbbtp_g.Text = giam.ToString();
                        lbbtp_tong.Text = (tang - giam).ToString();

                        //btp phoi bo hc
                        tang = 0; giam = 0;
                        tang = info.Where(x => x.ProductOutputTypeId == (int)eProductOutputType.BTP_HC && x.CommandTypeId == (int)eCommandRecive.BTPIncrease).Sum(x => x.ThanhPham);
                        giam = info.Where(x => x.ProductOutputTypeId == (int)eProductOutputType.BTP_HC && x.CommandTypeId == (int)eCommandRecive.BTPReduce).Sum(x => x.ThanhPham);
                        lbbtp_hc_t.Text = tang.ToString();
                        lbbtp_hc_g.Text = giam.ToString();
                        lbbtp_hc_tong.Text = (tang - giam).ToString();

                        //btp
                        tang = 0; giam = 0;
                        tang = info.Where(x => x.ProductOutputTypeId == null && x.CommandTypeId == (int)eCommandRecive.ErrorIncrease).Sum(x => x.ThanhPham);
                        giam = info.Where(x => x.ProductOutputTypeId == null && x.CommandTypeId == (int)eCommandRecive.ErrorReduce).Sum(x => x.ThanhPham);
                        lberr_t.Text = tang.ToString();
                        lberr_g.Text = giam.ToString();
                        lberr_tong.Text = (tang - giam).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //try
            //{
            //    int row = e.RowIndex;

            //    btnAdd_s.Enabled = false;
            //    btnUpdate_s.Enabled = true;
            //    btnDelete_s.Enabled = true;
            //    int.TryParse(dgv.Rows[row].Cells["STTc"].Value.ToString(), out STTC_SP);
            //    var vl = dgv.Rows[row].Cells["ProductOutputTypeId"].Value.ToString();
            //    switch (vl)
            //    {
            //        case "2": cbProType.SelectedIndex = 0;
            //            break;
            //        case "1": cbProType.SelectedIndex = 1;
            //            break;
            //        case "3": cbProType.SelectedIndex = 2;
            //            break;
            //        case "0": cbProType.SelectedIndex = 3;
            //            break;
            //    }
            //    if (dgv.Rows[row].Cells["ProductOutputTypeId"].Value.ToString() == "0")
            //    {
            //        cbError.SelectedValue = dgv.Rows[row].Cells["ProductOutputTypeId"].Value.ToString();
            //        cbError.Enabled = true;
            //    }
            //    var index = ((dgv.Rows[row].Cells["CommandTypeId"].Value.ToString() == "4" || dgv.Rows[row].Cells["CommandTypeId"].Value.ToString() == "6") ? 0 : 1);
            //    radioGroup1.SelectedIndex = index;
            //    txtsl.Value = decimal.Parse(dgv.Rows[row].Cells["ThanhPham"].Value.ToString());
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

        }

        private void Reset()
        {
            try
            {
                btnAdd_s.Enabled = true;
                btnUpdate_s.Enabled = false;
                btnDelete_s.Enabled = false;
                txtsl.Value = 0;
                cbProType.SelectedIndex = 0;
                cbError.SelectedIndex = 0;
                radioGroup1.SelectedIndex = 0;
            }
            catch { }
        }

        private void btnCancel_s_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void btnUpdate_s_Click(object sender, EventArgs e)
        {
            switch (cbProType.SelectedIndex)
            {
                case 0:
                case 1:
                case 2:

                    break;
                case 3:
                    break;
            }
        }

        private void btnAdd_s_Click(object sender, EventArgs e)
        {
            try
            {
                var chuyen = ((LineModel)cboChuyen_0.SelectedItem);
                var sanpham = ((AssignmentForLineModel)cboSanPham_0.SelectedItem);
                int.TryParse(ConfigurationManager.AppSettings["AppId"].ToString(), out appId);
                if (chuyen.MaChuyen != 0)
                {
                    STTC_SP = 0;
                    var tdn = new PMS.Data.TheoDoiNgay();
                    tdn.STT = STTC_SP;
                    tdn.MaChuyen = chuyen.MaChuyen;
                    tdn.MaSanPham = sanpham.MaSanPham;
                    tdn.CumId = chuyen.LastClusterId;
                    tdn.STTChuyenSanPham = sanpham.STT;
                    tdn.ThanhPham = (int)txtsl.Value;
                    tdn.CommandTypeId = radioGroup1.SelectedIndex == 0 ? (int)eCommandRecive.ProductIncrease : (int)eCommandRecive.ProductReduce;

                    switch (cbProType.SelectedIndex)
                    {
                        case 0: tdn.ProductOutputTypeId = (int)eProductOutputType.TC; break;
                        case 1: tdn.ProductOutputTypeId = (int)eProductOutputType.KCS; break;
                        case 2: tdn.ProductOutputTypeId = (int)eProductOutputType.BTP; break;
                        case 3: tdn.ProductOutputTypeId = (int)eProductOutputType.BTP_HC; break;
                        case 4: tdn.ProductOutputTypeId = (int)eProductOutputType.Error;
                            tdn.ErrorId = ((PMS.Data.Error)cbError.SelectedItem).Id;
                            break;
                    }

                    var rs = BLLDayInfo.InsertOrUpdate(tdn, appId, false, frmMainNew.TypeOfCheckFinishProduction);
                    if (rs.IsSuccess)
                    {
                        Reset();
                        LoadALLNSOfPCC();
                        frmMainNew.listDataSendKeyPad.Add(rs.DataSendKeyPad);
                        BLLProductivity.ResetNormsDayAndBTPInLine(frmMainNew.getBTPInLineByType, frmMainNew.calculateNormsdayType, frmMainNew.TypeOfCaculateDayNorms, chuyen.MaChuyen, false, frmMainNew.todayStr);
                        if (rs.Records != null)
                            DuAn03_HaiDang.Helper.HelperControl.ResetKeypad(chuyen.MaChuyen, false, frmMainNew);
                    }
                    else
                        MessageBox.Show(rs.Messages[0].msg, rs.Messages[0].Title);
                }
            }
            catch (Exception)
            {
            }
        }

        private void cbProType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbProType.SelectedIndex == 4)
                cbError.Enabled = true;
            else
                cbError.Enabled = false;
        }
        #endregion

        #region THAY DOI LUY KE NGAY
        private void dtpNgay_ValueChanged(object sender, EventArgs e)
        {
            LoadDataNS();
            LoadALLNSOfPCC_();
        }

        private void LoadDataNS()
        {
            var spCuaChuyen = (AssignmentForLineModel)cboSanPham_1.SelectedItem;
            var chuyen = (LineModel)cboChuyen_1.SelectedItem;
            if (chuyen != null && chuyen.MaChuyen != 0 && spCuaChuyen != null && spCuaChuyen.MaSanPham != 0)
            {
                //  butUpdate.Enabled = true;
                var nangSuatTrongNgay = BLLProductivity.TTNangXuatTrongNgay(frmMainNew.todayStr, spCuaChuyen.STT);
                if (nangSuatTrongNgay != null)
                {
                    lblDinhMucNgay_1.Text = ((int)nangSuatTrongNgay.DinhMucNgay).ToString();
                    lblSanLuongKeHoach_1.Text = spCuaChuyen.SanLuongKeHoach.ToString();
                    int thucHienNgay = nangSuatTrongNgay.ThucHienNgay - nangSuatTrongNgay.ThucHienNgayGiam;
                    if (thucHienNgay < 0)
                        thucHienNgay = 0;
                    lblThucHienNgayCu.Text = thucHienNgay.ToString();
                    txtThucHienNgay.Value = thucHienNgay;

                    //int sttChuyenSanPham = spCuaChuyen.STT;
                    //// var chuyenSanPhamOld = chuyen_sanphamDAO.GetChuyenSanPham(dtpNgayThucHien.Value.AddDays(-1), sttChuyenSanPham, chuyen.MaChuyen);

                    //var dt = dtpNgayThucHien.Value.AddDays(-1);

                    //var chuyenSanPhamOld = BLLAssignmentForLine.Instance.GetAssignmentByDay((dt.Day + "/" + dt.Month + "/" + dt.Year), sttChuyenSanPham, chuyen.MaChuyen);
                    //if (chuyenSanPhamOld != null)
                    lblLuyKeThucHienTruoc.Text = BLLAssignmentForLine.Instance.GetKCSToDay(dtpNgayThucHien.Value, spCuaChuyen.STT).ToString();

                    var TPThoatChuyen = (nangSuatTrongNgay.BTPThoatChuyenNgay - nangSuatTrongNgay.BTPThoatChuyenNgayGiam);
                    lbThoatChuyenCu.Text = (TPThoatChuyen < 0 ? 0 : TPThoatChuyen).ToString();
                    txtThoatChuyenMoi.Value = (TPThoatChuyen < 0 ? 0 : TPThoatChuyen);

                    var btp = (nangSuatTrongNgay.BTPTang - nangSuatTrongNgay.BTPGiam);
                    lbBTP_Old.Text = (btp < 0 ? "0" : "" + btp);
                    txtBTP_New.Value = (btp < 0 ? 0 : btp);
                }
                else
                {
                    butUpdate.Enabled = false;
                    if (MessageBox.Show("Không tìm thấy thông tin năng suất ngày của chuyền " + chuyen.TenChuyen + " trong ngày " + dtpNgayThucHien.Value.Date.ToString() + ". Bạn có muốn nhập thông tin ngày của chuyền không?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        FrmSetDayInformation form = new FrmSetDayInformation(frmMainNew);
                        form.ShowDialog();
                        LoadALLNSOfPCC();
                    }
                }
            }
        }

        private void cboChuyen_1_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                LoadDSSanPham(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void cboSanPham_1_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                LoadDataNS();
                LoadALLNSOfPCC_();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void LoadALLNSOfPCC_()
        {
            try
            {
                gridControlListNS.DataSource = null;
                var spCuaChuyen = (AssignmentForLineModel)cboSanPham_1.SelectedItem;
                if (spCuaChuyen != null && spCuaChuyen.MaSanPham != 0)
                {
                    //  var listNS = nangSuatDAO.GetAllNSOfPCC(spCuaChuyen.STT);
                    var listNS = BLLProductivity.GetAllNSOfPCC(spCuaChuyen.STT);
                    if (listNS != null && listNS.Count > 0)
                    {
                        gridControlListNS.DataSource = listNS;
                    }
                }
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
                string dateStr = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Ngay").ToString();
                txtNgay.Text = dateStr;
                var arr = dateStr.Split('/').ToArray();
                DateTime date;
                if (int.Parse(arr[0]) > 12)
                    date = DateTime.ParseExact(dateStr, "d/M/yyyy", CultureInfo.InvariantCulture);
                else
                    date = DateTime.ParseExact(dateStr, "M/d/yyyy", CultureInfo.InvariantCulture);

                lblLuyKeThucHienTruoc.Text = BLLAssignmentForLine.Instance.GetKCSToDay(date, int.Parse(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "STTCHuyen_SanPham").ToString())).ToString();
                int a = 0, b = 0, c = 0;
                int.TryParse(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ThucHienNgay").ToString(), out a);
                int.TryParse(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ThucHienNgayGiam").ToString(), out b);
                c = a - b;
                if (c < 0)
                {
                    lblThucHienNgayCu.Text = "0";
                    txtThucHienNgay.Value = 0;
                }
                else
                {
                    lblThucHienNgayCu.Text = c.ToString();
                    txtThucHienNgay.Value = c;
                }

                c = 0;
                int.TryParse(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "BTPThoatChuyenNgay").ToString(), out a);
                int.TryParse(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "BTPThoatChuyenNgayGiam").ToString(), out b);
                c = a - b;
                lbThoatChuyenCu.Text = c < 0 ? "0" : c.ToString();
                if (c < 0)
                {
                    lbThoatChuyenCu.Text = "0";
                    txtThoatChuyenMoi.Value = 0;
                }
                else
                {
                    lbThoatChuyenCu.Text = c.ToString();
                    txtThoatChuyenMoi.Value = c;
                }

                c = 0;
                int.TryParse(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "BTPTang").ToString(), out a);
                int.TryParse(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "BTPGiam").ToString(), out b);
                c = a - b;
                lbThoatChuyenCu.Text = c < 0 ? "0" : c.ToString();
                if (c < 0)
                {
                    lbBTP_Old.Text = "0";
                    txtBTP_New.Value = 0;
                }
                else
                {
                    lbBTP_Old.Text = c.ToString();
                    txtBTP_New.Value = c;
                }
                butClose.Enabled = true;
                butUpdate.Enabled = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        #endregion
        private void LoadListLineForCBB(int type)
        {
            try
            {
                var listChuyen = new List<LineModel>();
                listChuyen.Add(new LineModel() { MaChuyen = 0, TenChuyen = "(None)" });
                var sbc = BLLLine.GetLines_s(AccountSuccess.strListChuyenId.Split(',').Select(x => Convert.ToInt32(x)).ToList());
                listChuyen.AddRange(sbc);
                if (type == 0)
                {
                    cboChuyen_0.DataSource = null;
                    cboChuyen_0.DataSource = listChuyen;
                    cboChuyen_0.DisplayMember = "TenChuyen";
                    cboChuyen_0.SelectedIndex = 0;
                }
                else
                {
                    cboChuyen_1.DataSource = null;
                    cboChuyen_1.DataSource = sbc;
                    cboChuyen_1.DisplayMember = "TenChuyen";
                    cboChuyen_1.SelectedIndex = 0;
                }
            }
            catch (Exception) { }
        }

        private void LoadDSSanPham(int type)
        {
            try
            {
                LineModel chuyen = (type == 0 ? ((LineModel)cboChuyen_0.SelectedItem) : ((LineModel)cboChuyen_1.SelectedItem));
                cboSanPham_0.DataSource = null;
                cboSanPham_1.DataSource = null;
                if (chuyen != null && chuyen.MaChuyen != 0)
                {
                    var source = new List<AssignmentForLineModel>();
                    if (chuyen.MaChuyen != 0)
                    {
                        var csp = BLLAssignmentForLine.Instance.GetAssignmentForLine(chuyen.MaChuyen, true);
                        if (csp != null && csp.Count > 0)
                            source.AddRange(csp);
                        else
                        {
                            MessageBox.Show("Bạn chưa phân công mặt hàng cho chuyền này, Vui lòng thực hiện thao tác Phân Công Cho Chuyên", "Lỗi Thực Hiện", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            var spcuachuyen = new AssignmentForLineModel { STT = 0, CommoName = "(None)", LuyKeTH = 0, SanLuongKeHoach = 0, MaSanPham = 0, NangXuatSanXuat = 0, LuyKeBTPThoatChuyen = 0 };
                            source.Add(spcuachuyen);
                        }
                    }
                    else
                    {
                        var spcuachuyen = new AssignmentForLineModel { STT = 0, CommoName = "(None)", LuyKeTH = 0, SanLuongKeHoach = 0, MaSanPham = 0, NangXuatSanXuat = 0, LuyKeBTPThoatChuyen = 0 };
                        source.Add(spcuachuyen);
                    }
                    try
                    {
 if (type == 0)
                    {
                        cboSanPham_0.DataSource = source;
                        dgv.Rows.Clear();
                        cboSanPham_0.DisplayMember = "CommoName";
                    }
                    else
                    {
                        cboSanPham_1.DataSource = source;
                        gridControlListNS.DataSource = null;
                        cboSanPham_1.DisplayMember = "CommoName";
                        cboSanPham_1.SelectedIndex = 0;
                    }
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                   
                }
            }
            catch (Exception ex) { }
        }

        private void butUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                var spCuaChuyen = (AssignmentForLineModel)cboSanPham_1.SelectedItem;
                var chuyen = (LineModel)cboChuyen_1.SelectedItem;
                if (chuyen != null && chuyen.MaChuyen != 0 && spCuaChuyen != null && spCuaChuyen.MaSanPham != 0)
                {
                    var rs = BLLProductivity.UpdateLKOnDay(spCuaChuyen.STT, (int)txtThucHienNgay.Value, (int)txtThoatChuyenMoi.Value, (int)txtBTP_New.Value, txtNgay.Text, chuyen.LastClusterId, frmMainNew.getBTPInLineByType, frmMainNew.calculateNormsdayType, frmMainNew.TypeOfCaculateDayNorms, frmMainNew.TypeOfCheckFinishProduction);
                    if (rs.IsSuccess)
                    {
                        LoadDSSanPham(1);
                        BLLProductivity.ResetNormsDayAndBTPInLine(frmMainNew.getBTPInLineByType, frmMainNew.calculateNormsdayType, frmMainNew.TypeOfCaculateDayNorms, chuyen.MaChuyen, false, frmMainNew.todayStr);
                    }
                    MessageBox.Show(rs.Messages[0].msg, rs.Messages[0].Title);
                    butClose.Enabled = false;
                    butUpdate.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void butClose_Click(object sender, EventArgs e)
        {
            cboSanPham_1_SelectedIndexChanged(sender, e);
        }

        private void btnReLine_Click(object sender, EventArgs e)
        {
            LoadListLineForCBB(0);
        }

        private void btnReCommo_Click(object sender, EventArgs e)
        {
            LoadALLNSOfPCC();
        }

        private void btnReErr_Click(object sender, EventArgs e)
        {
            LoadError();
        }


        private void cboSanPham_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            butUpdate.Enabled = false;
            butClose.Enabled = false;
        }

        private void cboChuyen_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboSanPham_1_SelectedIndexChanged(sender, e);
        }

        private void cboSanPham_0_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}
