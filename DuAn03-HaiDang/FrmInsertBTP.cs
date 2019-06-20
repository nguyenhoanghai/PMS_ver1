using DuAn03_HaiDang;
using DuAn03_HaiDang.DATAACCESS;
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
    public partial class FrmInsertBTP : Form
    {
        int slkh = 0, lkht = 0, nangsuatId = 0;
        FrmMainNew frmMainNew;
        public FrmInsertBTP(FrmMainNew _frmMainNew)
        {
            InitializeComponent();
            frmMainNew = _frmMainNew;
        }

        private void FrmInsertBTP_Load(object sender, EventArgs e)
        {
            LoadLines();
            LoadAssignments(0);
            LoadBTPHC();

            cbProType.DisplayMember = "Text";
            cbProType.ValueMember = "Value";
            cbProType.Items.Add(new { Text = "Bán Thành Phẩm", Value = "3" });
            cbProType.Items.Add(new { Text = "BTP Phối bộ hoàn chỉnh", Value = "4" });
            cbProType.SelectedIndex = 0;
        }

        private void btnReLine_Click(object sender, EventArgs e)
        {
            LoadLines();
        }

        private void btnReCommo_Click(object sender, EventArgs e)
        {
            LoadAssignments(0);
        }

        private void cboLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAssignments(0);
        }

        private void cboCommo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var commo = (AssignmentForLineModel)cboCommo.SelectedItem;
                if (commo != null && commo.STT != 0)
                {
                    lbslkh.Text = commo.SanLuongKeHoach.ToString();
                    lblkbtp.Text = commo.LK_BTP.ToString();
                    lblkbtppbhc.Text = commo.LK_BTP_HC.ToString();
                    slkh = commo.SanLuongKeHoach;
                    nangsuatId = commo.NangSuatId;
                }
                else
                {
                    lbslkh.Text = "0";
                    lblkbtp.Text = "0";
                    lblkbtppbhc.Text = "0";
                    slkh = 0;
                }
            }
            catch (Exception)
            {
            }
        }

        private void LoadBTPHC()
        {
            try
            {
                cbbtp_hcStruct.DataSource = null;
                cbbtp_hcStruct.DataSource = BLLBTP_HCStructure.Instance.Gets((int)ePhaseType.BTP_HC);
                cbbtp_hcStruct.DisplayMember = "Name";
                cbbtp_hcStruct.ValueMember = "Id";
            }
            catch (Exception) { }
        }

        private void LoadLines()
        {
            try
            {
                var listChuyen = new List<LineModel>();
                listChuyen.Add(new LineModel() { MaChuyen = 0, TenChuyen = "(None)" });
                var abc = BLLLine.GetLines_s(AccountSuccess.strListChuyenId.Split(',').Select(x => Convert.ToInt32(x)).ToList());
                cboLine.DataSource = null;
                cboLine.DataSource = abc;
                cboLine.DisplayMember = "TenChuyen";
                cboLine.SelectedIndex = 0;
            }
            catch (Exception) { }
        }

        private void LoadAssignments(int selectIndex)
        {
            try
            {
                LineModel chuyen = (LineModel)cboLine.SelectedItem;
                if (chuyen != null && chuyen.MaChuyen != 0)
                {
                    var source = new List<AssignmentForLineModel>();
                    if (chuyen.MaChuyen != 0)
                    {
                        var csp = BLLAssignmentForLine.Instance.GetAssignmentForLine(chuyen.MaChuyen, frmMainNew.todayStr);
                        if (csp != null && csp.Count > 0)
                            source.AddRange(csp);
                        else
                        {
                            MessageBox.Show("Bạn chưa phân công mặt hàng cho chuyền này, Vui lòng thực hiện thao tác Phân Công Cho Chuyên", "Lỗi Thực Hiện", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            var spcuachuyen = new AssignmentForLineModel
                            {
                                STT = 0,
                                CommoName = "(None)",
                                LuyKeTH = 0,
                                SanLuongKeHoach = 0,
                                MaSanPham = 0,
                               // NangXuatSanXuat = 0,
                                LuyKeBTPThoatChuyen = 0
                            };
                            source.Add(spcuachuyen);
                        }
                    }
                    else
                    {
                        var spcuachuyen = new AssignmentForLineModel
                        {
                            STT = 0,
                            CommoName = "(None)",
                            LuyKeTH = 0,
                            SanLuongKeHoach = 0,
                            MaSanPham = 0,
                           // NangXuatSanXuat = 0,
                            LuyKeBTPThoatChuyen = 0
                        };
                        source.Add(spcuachuyen);
                    }
                    cboCommo.DataSource = null;
                    cboCommo.DataSource = source;
                    //   gridControlListNS.DataSource = null;
                    cboCommo.DisplayMember = "CommoName";
                    cboCommo.SelectedIndex = selectIndex;
                }
            }
            catch (Exception) { }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            try
            {
                var chuyen = ((LineModel)cboLine.SelectedItem);
                var sanpham = ((AssignmentForLineModel)cboCommo.SelectedItem);
                if (chuyen.MaChuyen != 0)
                {
                    if (cbProType.SelectedIndex == 0)
                        SaveBTP(chuyen, sanpham);
                    else
                        SaveBTP_HC(chuyen, sanpham);
                }
            }
            catch (Exception)
            {
            }
        }
        private void SaveBTP_HC(LineModel chuyen, AssignmentForLineModel sanpham)
        {
            if (slkh < (lkht + txtQuantity.Value))
                MessageBox.Show("Số lương bạn nhập hiện tại đã vượt sản lượng kế hoạch. Vui lòng nhập trong phạm vi sản lượng kế hoạch", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            { 
                var btpStructObj = ((PhaseModel)cbbtp_hcStruct.SelectedItem);

                var newObj = new  PhaseDailyModel(); 
                newObj.CommandTypeId = (int)eCommandRecive.ProductIncrease;
                  newObj.PhaseId = btpStructObj.Id;
                newObj.AssignId = sanpham.STT;
                newObj.Quantity = (int)txtQuantity.Value; 
                var rs = BLLBTP_HCStructure.Instance.InsertBTPDay(newObj);
                if (rs.IsSuccess)
                {
                    Reset();
                    LoadAssignments(cboCommo.SelectedIndex);
                    if (rs.DataSendKeyPad != null)
                        frmMainNew.listDataSendKeyPad.Add(rs.DataSendKeyPad);
                    if (rs.Records != null)
                    {
                        BLLProductivity.ResetNormsDayAndBTPInLine(frmMainNew.getBTPInLineByType, frmMainNew.calculateNormsdayType, frmMainNew.TypeOfCaculateDayNorms, chuyen.MaChuyen, false, frmMainNew.todayStr);
                        DuAn03_HaiDang.Helper.HelperControl.ResetKeypad(chuyen.MaChuyen, false, frmMainNew);
                    }
                }
                MessageBox.Show(rs.Messages[0].msg, rs.Messages[0].Title);
                lbQuantitiesBTPHC.Text = newObj.Quantity.ToString();
            }
        }

        private void SaveBTP(LineModel chuyen, AssignmentForLineModel sanpham)
        {
            if ((int.Parse(lblkbtp.Text) + txtQuantity.Value) > int.Parse(lblkbtppbhc.Text) && frmMainNew.isUseBTP_HC==1)
                MessageBox.Show("Sản lượng lũy kế BTP hoàn chỉnh chưa đủ. Vui lòng nhập thêm sản lượng BTP hoàn chỉnh.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                var tdn = new PMS.Data.TheoDoiNgay();
                tdn.STT = 0;
                tdn.MaChuyen = chuyen.MaChuyen;
                tdn.MaSanPham = sanpham.MaSanPham;
                tdn.CumId = chuyen.LastClusterId;
                tdn.STTChuyenSanPham = sanpham.STT;
                tdn.ThanhPham = (int)txtQuantity.Value;
                tdn.CommandTypeId = radioGroup1.SelectedIndex == 0 ? (int)eCommandRecive.ProductIncrease : (int)eCommandRecive.ProductReduce;
                tdn.ProductOutputTypeId = (int)eProductOutputType.BTP;
                var rs = BLLDayInfo.InsertOrUpdate(tdn, frmMainNew.appId, true, frmMainNew.TypeOfCheckFinishProduction);
                if (rs.IsSuccess)
                {
                    Reset();
                    LoadAssignments(cboCommo.SelectedIndex);
                    if (rs.DataSendKeyPad != null)
                        frmMainNew.listDataSendKeyPad.Add(rs.DataSendKeyPad);
                    if (rs.Records != null)
                    {
                        DuAn03_HaiDang.Helper.HelperControl.ResetKeypad(chuyen.MaChuyen, false, frmMainNew);
                    }
                    BLLProductivity.ResetNormsDayAndBTPInLine(frmMainNew.getBTPInLineByType, frmMainNew.calculateNormsdayType, frmMainNew.TypeOfCaculateDayNorms, chuyen.MaChuyen, false, frmMainNew.todayStr);

                }
                MessageBox.Show(rs.Messages[0].msg, rs.Messages[0].Title);
            }
        }

        private void Reset()
        {
            txtQuantity.Value = 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void cbProType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbProType.SelectedIndex == 1)
            {
                lbstruc.Visible = true;
                cbbtp_hcStruct.Visible = true;
                groupBox1.Visible = true;
            }
            else
            {
                lbstruc.Visible = false;
                cbbtp_hcStruct.Visible = false;
                groupBox1.Visible = false;
            }
        }

        private void FrmInsertBTP_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (AccountSuccess.BTP == 1)
                Application.Exit();
        }

        private void btnReBTP_HC_Click(object sender, EventArgs e)
        {
            LoadBTPHC();
        }

        private void cbbtp_hcStruct_SelectedIndexChanged(object sender, EventArgs e)
        {
            var structObj = (PhaseModel)cbbtp_hcStruct.SelectedItem;
            var sanpham = ((AssignmentForLineModel)cboCommo.SelectedItem);
            if (structObj != null && sanpham != null)
            {
                var rs = BLLBTP_HCStructure.Instance.CountQuantities(sanpham.STT, structObj.Id, DateTime.Now);
                lbQuantitiesBTPHC.Text = rs.Records.ToString();
                lbLKngaytruoc.Text = rs.Data.ToString();
                lkht = rs.Data;
            }
        }
    }
}
