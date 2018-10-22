using DuAn03_HaiDang.DAO;
using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.KeyPad_Chuyen.dao;
using DuAn03_HaiDang.Model;
using DuAn03_HaiDang.POJO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DuAn03_HaiDang.Helper;
using DuAn03_HaiDang.KeyPad_Chuyen.pojo;
using DuAn03_HaiDang.Enum;
using QuanLyNangSuat.Model;
using PMS.Business;
using PMS.Business.Enum;
using PMS.Business.Models;


namespace DuAn03_HaiDang
{
    public partial class FrmHienThiLCD : FormBase
    {
        LabelConfigDAO labelConfigDAO = new LabelConfigDAO();
        PanelConfigDAO panelConfigDAO = new PanelConfigDAO();
        TableLayoutPanelConfigDAO tableLayoutPanelConfigDAO = new TableLayoutPanelConfigDAO();
        ThoiGianTinhNhipDoTTDAO thoigiantinhndttDAO = new ThoiGianTinhNhipDoTTDAO();
        //  ConfigDAO configDAO = new ConfigDAO();
        ChuyenDAO chuyenDAO = new ChuyenDAO();
        DenDAO denDAO = new DenDAO();
        //   ShiftDAO shiftDAO = new ShiftDAO();
        int countMaHangInDate = 0;
        int maChuyen = 0;
        int manHinhShow = 1;
        int timeShowMH1 = 1;
        int timeShowMH2 = 1;
        int countShowMH = 0;
        //   List<Shift> shifts = new List<Shift>();
        List<WorkingTimeModel> shifts = new List<WorkingTimeModel>();
        List<LineWorkingShiftModel> shifts_ = new List<LineWorkingShiftModel>();
        double intWorkTime = 0;
        double intWorkMinuter = 0;
        LabelConfig labelConfigTopOftblpanelFooter;
        LabelConfig labelConfigDownOftblpanelFooter;
        DataTable dtLoadDataAll = new DataTable();
        DataTable dtLaoDongVaThucHienNgay = new DataTable();
        DataTable dtchuyenSanPham = new DataTable();
        DataTable dtThongTinThang = new DataTable();
        List<string> listDataAll = new List<string>();
        List<Label> listLableNangSuatGio = new List<Label>();
        int column = 0;
        DataTable dtCountThanhPham = new DataTable();
        List<TableLayoutPanelConfig> listConfig = new List<TableLayoutPanelConfig>();
        List<ModelLabelOfTBLPanelContent> listLabel = new List<ModelLabelOfTBLPanelContent>();
        //Cac bien luu value tam cua cac o hien thi
        string thucHienVaDinhMuc1 = "";
        string btpTrenChuyen1 = "";
        string laoDong1 = "";
        string nhipChuyen1 = "";
        string maHang1 = "";
        string sanLuongKeHoach1 = "";
        string luyKeThucHien1 = "";
        string doanhThuThang1 = "";
        string doanhThuNgay1 = "";
        string thuNhapBQNgay1 = "";
        string thuNhapBQThang1 = "";
        string doanhThuNgayTrenDinhMuc1 = "";
        string tiLeThucHien1 = "";
        string doanhThuKHThang1 = "";
        string thucHienKHThang1 = "";

        string thucHienVaDinhMuc2 = "";
        string btpTrenChuyen2 = "";
        string laoDong2 = "";
        string nhipChuyen2 = "";
        string maHang2 = "";
        string sanLuongKeHoach2 = "";
        string luyKeThucHien2 = "";
        string doanhThuThang2 = "";
        string doanhThuNgay2 = "";
        string thuNhapBQNgay2 = "";
        string thuNhapBQThang2 = "";
        string doanhThuNgayTrenDinhMuc2 = "";
        string tiLeThucHien2 = "";
        string doanhThuKHThang2 = "";
        string thucHienKHThang2 = "";
        int hienThiNSGio = 0;
        string tongThucHienNgay1 = "";
        string tongThucHienNgay2 = "";
        int intMaxError = 10;
        int intCurrentError = 0;

        string ThoatChuyen = "";
        int ThoatChuyenNgay = 0;

        //danh sach chuyen show thong tin ra man hinh
        public List<Chuyen> listChuyen = new List<Chuyen>();

        //Tong thoi gian lam viec trong ngay cua chuyen
        TimeSpan tongTimeOfWorkInDay = new TimeSpan();
        //Số giây làm việc trong ngày cua chuyền
        int totalSecond = 0;
        List<InformationChuyen> listChuyenInf;

        //Loai doanh thu thang
        int loaiDoanhThuThang = 1;

        //Danh dach cac thong tin hien thi trong phan content
        List<ShowLCDLabelForPanelContent> listLableForContent = new List<ShowLCDLabelForPanelContent>();

        private int soThongHienThi = 0;
        private int soLanLatTrang = 0;
        private int countRowInContentOfPage = 0;
        private int tableType = 1;
        private string currentColorDen = string.Empty;
        private System.Windows.Forms.TableLayoutPanel tblpanelFooter;
        //  private List<ModelWorkHours> listModelWorkHours;
        private List<WorkingTimeModel> listModelWorkHours;

        int TimesGetNS = 1;
        int KhoangCachGetNSOnDay = 1;
        int calculateNormsdayType = 0;
        public FrmHienThiLCD()
        {
            InitializeComponent();
            try
            {
                //Get list label cua content
                listLableForContent = tableLayoutPanelConfigDAO.GetLabelForTBLPanelContent().Where(c => c.IsShow).ToList();
                if (listLableForContent != null)
                    soThongHienThi = listLableForContent.Count();
                //Load config for LCD
                LoadLCDConfig();
                if (!string.IsNullOrEmpty(AccountSuccess.strListChuyenId))
                    listChuyen = chuyenDAO.GetListChuyenInfByListId(AccountSuccess.strListChuyenId);

                // get cau hinh danh sach table layout
                listConfig = tableLayoutPanelConfigDAO.GetTableLayoutPanelConfig(tableType);
                //Ve table layout
                this.tblpanelFooter = new System.Windows.Forms.TableLayoutPanel();
                this.tblpanelFooter.SuspendLayout();
                LoadTableLayoutPanel();
                LoadTableLayoutPanelContent();
                //tinh so lan lat trang
                if (countRowInContentOfPage > 0)
                {
                    float soDuTiLeLatTrang = soThongHienThi % countRowInContentOfPage;
                    if (soDuTiLeLatTrang == 0)
                        soLanLatTrang = soThongHienThi / countRowInContentOfPage;
                    else
                        soLanLatTrang = (soThongHienThi / countRowInContentOfPage) + 1;
                }

                if (listChuyen.Count > 0)
                {

                    int.TryParse(listChuyen[0].MaChuyen, out maChuyen);
                    lblTableName.Text = "BẢNG NĂNG SUẤT " + listChuyen[0].TenChuyen.ToUpper();

                    ////TTAll(maChuyen);

                   
                   // // GetTongGiayCuaThoiGianLamViecTrongNgay();
                    
                   GetProductivitiesOfLine(maChuyen);
                }

                //Load design content
                LoadContent(countMaHangInDate);

                //Load config Panel
                LoadPanel();
                //Load config Label
               LoadLabel();

                //Set time and date
               SetTimeAndDate();

                //design footer
                designNangSuatGio();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void GetProductivitiesOfLine(int maChuyen)
        {
            try
            {
                SumHoursOfShifts();
                if (!IsDesign)
                {
                    countMaHangInDate = 1;
                    LoadTableLayoutPanelContent();
                    LoadContent(countMaHangInDate);
                    LoadLabel();
                    ColumnNumber = 1;
                    IsDesign = true;

                }
                ModelProductivity product;
                if (calculateNormsdayType == 0)
                    product = BLLProductivity.GetProductivityByLineId_0(maChuyen, tableType, hienThiNSGio, TimesGetNS, KhoangCachGetNSOnDay);
                else
                    product = BLLProductivity.GetProductivityByLineId_1(maChuyen, tableType, hienThiNSGio, TimesGetNS, KhoangCachGetNSOnDay);
                if (product != null)
                {
                    SetValueForLabelOfTBLPanelContent(0, manHinhShow, "thucHienVaDinhMuc", "");
                    SetValueForLabelOfTBLPanelContent(0, manHinhShow, "btpTrenChuyen", "");
                    SetValueForLabelOfTBLPanelContent(0, manHinhShow, "laoDong", "");
                    SetValueForLabelOfTBLPanelContent(0, manHinhShow, "nhipChuyen", "");

                    SetValueForLabelOfTBLPanelContent(0, manHinhShow, "maHang", "");
                    SetValueForLabelOfTBLPanelContent(0, manHinhShow, "sanLuongKeHoach", "");
                    SetValueForLabelOfTBLPanelContent(0, manHinhShow, "luyKeThucHien", "");
                    SetValueForLabelOfTBLPanelContent(0, manHinhShow, "doanhThuThang", "");
                    SetValueForLabelOfTBLPanelContent(0, manHinhShow, "thuNhapBQThang", "");
                    SetValueForLabelOfTBLPanelContent(0, manHinhShow, "doanhThuNgayTrenDinhMuc", "");
                    SetValueForLabelOfTBLPanelContent(0, manHinhShow, "tiLeThucHien", "");
                    SetValueForLabelOfTBLPanelContent(0, manHinhShow, "doanhThuKHThang", "");
                    SetValueForLabelOfTBLPanelContent(0, manHinhShow, "thucHienKHThang", "");
                    SetValueForLabelOfTBLPanelContent(0, manHinhShow, "tongThucHienNgay", "");
                    SetValueForLabelOfTBLPanelContent(0, manHinhShow, "ThoatChuyen", "");
                    SetValueForLabelOfTBLPanelContent(1, manHinhShow, "ProductionPlans", "");          

                    indexFinish = indexFinish + 1;

                    var lb = listLabel.FirstOrDefault(x => x.intColumn == 0 && x.label.Text.Equals("NĂNG SUẤT") || x.label.Text.Equals("THỰC HIỆN / ĐỊNH MỨC"));
                    switch (hienThiNSGio)
                    {
                        case (int)eShowNSType.PercentTH_OnDay:
                        case (int)eShowNSType.TH_DM_OnDay:
                        case (int)eShowNSType.PercentTH_FollowHour:
                        case (int)eShowNSType.PercentTH_FollowConfig:
                        case (int)eShowNSType.TH_DM_FollowHour: // TH - ĐM -> theo tung gio
                        case (int)eShowNSType.TH_DM_FollowConfig: // TH - ĐM -> theo so lan chia
                            if (lb != null)
                                lb.label.Text = "NĂNG SUẤT (TH/ĐM)";
                            SetValueForLabelOfTBLPanelContent(1, manHinhShow, "thucHienVaDinhMuc", product.thucHienVaDinhMuc);
                            break;
                        case (int)eShowNSType.TH_TC_FollowHour:
                        case (int)eShowNSType.TH_TC_FollowConfig:
                        case (int)eShowNSType.TH_TC_OnDay:
                            if (lb != null)
                                lb.label.Text = "NĂNG SUẤT (TH/TC)";
                            SetValueForLabelOfTBLPanelContent(1, manHinhShow, "thucHienVaDinhMuc", product.ThucHienNgay + "/" + product.ThoatChuyenNgay);
                            break;
                        case (int)eShowNSType.TH_Error_OnDay:
                        case (int)eShowNSType.TH_Err_FollowHour:
                        case (int)eShowNSType.TH_Err_FollowConfig: // th-error
                            if (lb != null)
                                lb.label.Text = "NĂNG SUẤT (TH/LỖI)";
                            SetValueForLabelOfTBLPanelContent(1, manHinhShow, "thucHienVaDinhMuc", product.ThucHienNgay + "/" + product.ErrorNgay);
                            break;
                    }


                    if (indexFinish == 0)
                    {
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "btpTrenChuyen", product.btpTrenChuyen);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "laoDong", (product.laoDong));
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "nhipChuyen", product.nhipChuyen);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "maHang", product.maHang);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "sanLuongKeHoach", product.sanLuongKeHoach);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "luyKeThucHien", product.luyKeThucHien);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "doanhThuThang", product.doanhThuThang);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "thuNhapBQThang", product.thuNhapBQThang);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "doanhThuNgayTrenDinhMuc", product.doanhThuNgayTrenDinhMuc);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "tiLeThucHien", product.tiLeThucHien);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "doanhThuKHThang", product.doanhThuKHThang);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "thucHienKHThang", product.thucHienKHThang);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "tongThucHienNgay", product.tongThucHienNgay);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "ThoatChuyen", product.ThoatChuyen);          
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "ProductionPlans", product.DinhMucNgay +"|"+product.sanLuongKeHoach);          
                        SetColorForDen(product.mauDen);
                    }
                    else
                    {
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "btpTrenChuyen", product.btpTrenChuyen);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "laoDong", (product.laoDong));
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "nhipChuyen", product.nhipChuyen);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "maHang", product.maHang);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "sanLuongKeHoach", product.sanLuongKeHoach);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "luyKeThucHien", product.luyKeThucHien);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "doanhThuThang", product.doanhThuThang);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "thuNhapBQThang", product.thuNhapBQThang);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "doanhThuNgayTrenDinhMuc", product.doanhThuNgayTrenDinhMuc);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "tiLeThucHien", product.tiLeThucHien);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "doanhThuKHThang", product.doanhThuKHThang);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "thucHienKHThang", product.thucHienKHThang);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "tongThucHienNgay", product.tongThucHienNgay);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "ThoatChuyen", product.ThoatChuyen);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "ProductionPlans", product.DinhMucNgay + "|" + product.sanLuongKeHoach);          

                        SetColorForDen(product.mauDen);
                    }

                    #region Phan khung duoi
                    double NangSuatPhutKH = 0;
                    int NangSuatGioKH = 0;
                    var dateNow = DateTime.Now.Date;
                    if (intWorkTime > 0)
                    {
                        NangSuatPhutKH = product.DinhMucNgay / intWorkMinuter;
                        NangSuatGioKH = (int)(product.DinhMucNgay / intWorkTime);
                        if (product.DinhMucNgay % intWorkTime != 0)
                            NangSuatGioKH++;
                    }
                    if (hienThiNSGio == (int)eShowNSType.PercentTH_OnDay)
                    {
                        #region  hiển thị một ô năng suất hiện tại duy nhất
                        double phutToNow = GetSoPhutLamViecTrongNgay_(DateTime.Now.TimeOfDay, shifts_);
                        double nsKHToNow = (phutToNow / intWorkMinuter) * product.DinhMucNgay;
                        double tiLePhanTram = 0;
                        tiLePhanTram = (product.ThucHienNgay > 0 && nsKHToNow > 0) ? (Math.Round((double)((product.ThucHienNgay * 100) / nsKHToNow), 2)) : 0;
                        if (listLableNangSuatGio.Count > 0)
                            listLableNangSuatGio[0].Text = product.ThucHienNgay + "/" + (int)nsKHToNow + "  (" + tiLePhanTram + "%)";
                        #endregion
                    }
                    else
                    {
                        #region Thực Hiện / Định Mức Or Hiển thị theo %
                        if (product.listWorkHours != null && product.listWorkHours.Count > 0)
                        {
                            foreach (var item in product.listWorkHours)
                            {
                                switch (hienThiNSGio)
                                {
                                    case (int)eShowNSType.TH_DM_FollowHour:
                                    case (int)eShowNSType.TH_DM_FollowConfig:
                                    case (int)eShowNSType.TH_DM_OnDay:
                                        //San luong gio - ke hoach gio
                                        listLableNangSuatGio[item.IntHours - 1].Text = product.listWorkHours[item.IntHours - 1].KCS + "/" + product.listWorkHours[item.IntHours - 1].NormsHour;
                                        break;
                                    case (int)eShowNSType.PercentTH_FollowConfig:
                                    case (int)eShowNSType.PercentTH_FollowHour:
                                        // % thuc hien = sanluong gio * 100 / ke hoach gio
                                        var nsGioKH = product.listWorkHours[item.IntHours - 1].NormsHour;
                                        listLableNangSuatGio[item.IntHours - 1].Text = (nsGioKH > 0 && product.listWorkHours[item.IntHours - 1].KCS > 0 ? Math.Round((double)((product.listWorkHours[item.IntHours - 1].KCS / nsGioKH) * 100)) : 0) + "%";
                                        break;
                                    case (int)eShowNSType.TH_TC_FollowConfig:
                                    case (int)eShowNSType.TH_TC_FollowHour:
                                    case (int)eShowNSType.TH_TC_OnDay:
                                        // thuc hien - thoat chuyen
                                        listLableNangSuatGio[item.IntHours - 1].Text = product.listWorkHours[item.IntHours - 1].KCS + "/" + product.listWorkHours[item.IntHours - 1].TC;
                                        if (listLableNangSuatGio[item.IntHours - 1].Text.Length > 6)
                                        {
                                            listLableNangSuatGio[item.IntHours - 1].Font = new Font(listLableNangSuatGio[item.IntHours - 1].Font.FontFamily, 24, listLableNangSuatGio[item.IntHours - 1].Font.Style);
                                            listLableNangSuatGio[item.IntHours - 1].Size = new System.Drawing.Size(210, 37);
                                            listLableNangSuatGio[item.IntHours - 1].AutoSize = false;
                                        }
                                        else
                                        {
                                            listLableNangSuatGio[item.IntHours - 1].AutoSize = true;
                                            listLableNangSuatGio[item.IntHours - 1].Font = new Font(listLableNangSuatGio[item.IntHours - 1].Font.FontFamily, 32, listLableNangSuatGio[item.IntHours - 1].Font.Style);
                                        }
                                        break;
                                    case (int)eShowNSType.TH_Err_FollowConfig:
                                    case (int)eShowNSType.TH_Err_FollowHour:
                                    case (int)eShowNSType.TH_Error_OnDay:
                                        // thuc hien - loi
                                        listLableNangSuatGio[item.IntHours - 1].Text = product.listWorkHours[item.IntHours - 1].KCS + "/" + product.listWorkHours[item.IntHours - 1].Error;
                                        if (listLableNangSuatGio[item.IntHours - 1].Text.Length > 6)
                                        {
                                            listLableNangSuatGio[item.IntHours - 1].Font = new Font(listLableNangSuatGio[item.IntHours - 1].Font.FontFamily, 24, listLableNangSuatGio[item.IntHours - 1].Font.Style);
                                            listLableNangSuatGio[item.IntHours - 1].Size = new System.Drawing.Size(210, 37);
                                            listLableNangSuatGio[item.IntHours - 1].AutoSize = false;
                                        }
                                        else
                                        {
                                            listLableNangSuatGio[item.IntHours - 1].AutoSize = true;
                                            listLableNangSuatGio[item.IntHours - 1].Font = new Font(listLableNangSuatGio[item.IntHours - 1].Font.FontFamily, 32, listLableNangSuatGio[item.IntHours - 1].Font.Style);
                                        }
                                        break;
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion
                }
            }
            catch (Exception)
            { }
        }

        private void FrmHienThiLCD_Load(object sender, EventArgs e)
        {
            try
            {
                listChuyen = chuyenDAO.GetListChuyenInfByListId(AccountSuccess.strListChuyenId);
                if (listChuyen != null && listChuyen.Count > 0)
                {
                    var line = listChuyen.First();
                    listModelWorkHours = BLLShift.GetListWorkHoursOfLineByLineId(int.Parse(line.MaChuyen));// shiftDAO.GetListWorkHoursOfLineByLineId(line.MaChuyen);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }

        }

        private void ShowLogo(string addressImg)
        {
            try
            {
                this.panelLogo.BackgroundImage = Image.FromFile(Application.StartupPath + @addressImg);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi : " + ex.Message, "Lỗi xử lý", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTableLayoutPanel()
        {
            try
            {
                if (listConfig != null && listConfig.Count > 0)
                {
                    var listConfigByGroup = listConfig.GroupBy(c => c.TableLayoutTableName).ToList();
                    foreach (var config in listConfigByGroup.OrderBy(c => c.Key))
                    {
                        if (config.Key.Trim().Equals("tblpanelBody"))
                        {
                            foreach (var item in config.OrderBy(c => c.RowInt))
                            {
                                this.tblpanelBody.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, float.Parse(item.SizePercent)));
                            }

                        }
                        else if (config.Key.Trim().Equals("tblpanelHeader"))
                        {
                            foreach (var item in config.OrderBy(c => c.ColumnInt))
                            {
                                this.tblpanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, float.Parse(item.SizePercent)));
                            }
                        }
                        else if (config.Key.Trim().Equals("tblpanelContent"))
                        {
                            this.tblpanelContent.ColumnCount = 1;
                            foreach (var item in config.OrderBy(c => c.ColumnInt))
                            {
                                if (item.ColumnInt.Trim().Equals("1"))
                                {
                                    this.tblpanelContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, float.Parse(item.SizePercent)));
                                }
                            }
                        }
                        else if (config.Key.Trim().Equals("tblpanelFooter"))
                        {
                            if (hienThiNSGio == 2)
                            {
                                foreach (var item in config.OrderBy(c => c.ColumnInt))
                                {
                                    this.tblpanelFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, float.Parse(item.SizePercent)));
                                }
                            }
                        }
                        else
                            MessageBox.Show("Lỗi lấy cấu hình tablelayoutpanel: Tồn tại cấu hình của phần tử không được sử dụng trong hệ thống", "Lỗi cấu hình", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi : " + ex.Message, "Lỗi xử lý", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTableLayoutPanelContent()
        {
            try
            {
                if (this.tblpanelContent.ColumnStyles != null && this.tblpanelContent.ColumnStyles.Count > 0)
                {
                    for (int i = 0; i < this.tblpanelContent.ColumnStyles.Count; i++)
                    {
                        this.tblpanelContent.ColumnStyles.Remove(this.tblpanelContent.ColumnStyles[i]);
                        i = i - 1;
                    }
                }
                if (this.tblpanelContent.RowStyles != null && this.tblpanelContent.RowStyles.Count > 0)
                {
                    for (int i = 0; i < this.tblpanelContent.RowStyles.Count; i++)
                    {
                        this.tblpanelContent.RowStyles.Remove(this.tblpanelContent.RowStyles[i]);
                        i = i - 1;
                    }
                }
                if (listConfig != null && listConfig.Count > 0)
                {
                    var listConfigByGroup = listConfig.GroupBy(c => c.TableLayoutTableName).ToList();
                    foreach (var config in listConfigByGroup.OrderBy(c => c.Key))
                    {
                        if (config.Key.Trim().Equals("tblpanelContent"))
                        {
                            this.tblpanelContent.ColumnCount = 1;
                            foreach (var item in config.Where(c => c.ColumnInt != "" && c.IsShow).OrderBy(c => c.ColumnInt))
                            {
                                if (item.ColumnInt.Trim().Equals("1"))
                                    this.tblpanelContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, float.Parse(item.SizePercent)));
                                else
                                {
                                    if (countMaHangInDate > 0)
                                    {
                                        this.tblpanelContent.ColumnCount = this.tblpanelContent.ColumnCount + countMaHangInDate;
                                        float sizeColumn = float.Parse(item.SizePercent) / countMaHangInDate;
                                        for (int i = 0; i < countMaHangInDate; i++)
                                        {
                                            this.tblpanelContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, sizeColumn));
                                        }
                                    }
                                }
                            }
                            var rowConfig = config.Where(c => c.RowInt != "" && c.IsShow).OrderBy(c => c.IntRowInt).ToList();
                            countRowInContentOfPage = rowConfig.Count;
                            this.tblpanelContent.RowCount = countRowInContentOfPage;
                            foreach (var item in rowConfig)
                            {
                                this.tblpanelContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, float.Parse(item.SizePercent)));
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi : " + ex.Message, "Lỗi xử lý", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPanel()
        {
            try
            {
                List<PanelConfig> listConfig = panelConfigDAO.GetPanelConfig(tableType);
                if (listConfig != null && listConfig.Count > 0)
                {
                    foreach (var config in listConfig)
                    {
                        if (config.Name.Trim().Equals("panelHeader"))
                            this.panelHeader.BackColor = DuAn03_HaiDang.Helper.HelperControl.GetColor(config.BackColor);
                        else if (config.Name.Trim().Equals("panelContent"))
                            this.panelContent.BackColor = DuAn03_HaiDang.Helper.HelperControl.GetColor(config.BackColor);
                        else if (config.Name.Trim().Equals("panelFooter"))
                            this.panelFooter.BackColor = DuAn03_HaiDang.Helper.HelperControl.GetColor(config.BackColor);
                        else
                            MessageBox.Show("Lỗi lấy cấu hình panle: Tồn tại cấu hình của phần tử không được sử dụng trong hệ thống", "Lỗi cấu hình", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi : " + ex.Message, "Lỗi xử lý", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadLabel()
        {
            try
            {
                List<LabelConfig> listConfig = labelConfigDAO.GetLabelConfig(tableType);
                if (listConfig != null && listConfig.Count > 0)
                {
                    var listConfigByGroup = listConfig.GroupBy(c => c.TableLayoutPanelName).ToList();
                    foreach (var config in listConfigByGroup)
                    {
                        if (config.Key.Trim().Equals("tblpanelHeader"))
                        {
                            foreach (var item in config.OrderBy(c => c.Position))
                            {
                                if (item.Position == 2)
                                    DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(item, this.lblTableName);
                                else if (item.Position == 3)
                                {
                                    DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(item, this.lblTime);
                                    DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(item, this.lblDate);
                                }
                                else
                                    MessageBox.Show("Lỗi lấy cấu hình label: không tìm thấy vi trí được cấu hình trong phần Header hoặc cấu hình vị trí trong DB sai.", "Lỗi cấu hình", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else if (config.Key.Trim().Equals("tblpanelContent"))
                        {
                            if (listLabel.Count > 0)
                            {
                                var listLableAtColumn0 = listLabel.Where(c => c.intColumn == 0).ToList();
                                var listLableATColumOther = listLabel.Where(c => c.intColumn != 0).ToList();
                                foreach (var item in config.OrderBy(c => c.Position))
                                {
                                    if (item.Position == 1)
                                    {
                                        if (listLableAtColumn0.Count > 0)
                                        {
                                            foreach (var labelModel in listLableAtColumn0)
                                            {
                                                DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(item, labelModel.label);
                                            }
                                        }
                                    }
                                    else if (item.Position == 2)
                                    {
                                        if (listLableATColumOther.Count > 0)
                                        {
                                            foreach (var labelmodel in listLableATColumOther)
                                            {
                                                DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(item, labelmodel.label);
                                            }
                                        }
                                    }
                                    else
                                        MessageBox.Show("Lỗi lấy cấu hình label: không tìm thấy vi trí được cấu hình trong phần Content hoặc cấu hình vị trí trong DB sai.", "Lỗi cấu hình", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        else if (config.Key.Trim().Equals("tblpanelFooter"))
                        {
                            foreach (var item in config.OrderBy(c => c.Position))
                            {
                                if (item.Position == 1)
                                    labelConfigTopOftblpanelFooter = item;
                                else if (item.Position == 2)
                                    labelConfigDownOftblpanelFooter = item;
                                else
                                    MessageBox.Show("Lỗi lấy cấu hình label: không tìm thấy vi trí được cấu hình trong phần Footer hoặc cấu hình vị trí trong DB sai.", "Lỗi cấu hình", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi : " + ex.Message, "Lỗi xử lý", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Add label vao cac row va column trong tblpanelContent
        List<TableLayoutPanelConfig> rowTBLPanelContents = new List<TableLayoutPanelConfig>();
        private void LoadContent(int countMaHangInDate)
        {
            listLabel.Clear();
            this.tblpanelContent.Controls.Clear();
            rowTBLPanelContents = listConfig.Where(c => c.TableLayoutTableName.Trim() == "tblpanelContent" && c.RowInt != "" && c.IsShow).OrderBy(c => c.IntRowInt).ToList();

            if (rowTBLPanelContents.Count > 0)
            {
                for (int i = 0; i < rowTBLPanelContents.Count(); i++)
                {
                    var row = rowTBLPanelContents[i];
                    int rowInt = -1;
                    int.TryParse(row.RowInt, out rowInt);
                    var listShowLCDLabelOfRow = listLableForContent.Where(c => c.IntRow == rowInt).OrderBy(c => c.IntRow).ToList();
                    if (listShowLCDLabelOfRow.Count > 0)
                    {
                        for (int j = 0; j < countMaHangInDate + 1; j++)
                        {
                            ModelLabelOfTBLPanelContent modelLabelOfTBLContent = new ModelLabelOfTBLPanelContent();
                            modelLabelOfTBLContent.intColumn = j;

                            var label = new System.Windows.Forms.Label();
                            label.Anchor = System.Windows.Forms.AnchorStyles.None;
                            label.AutoSize = true;
                            label.Location = new System.Drawing.Point(4, 9);
                            label.Name = "lbl" + row.Id + j.ToString();
                            label.Size = new System.Drawing.Size(14, 3);
                            label.TabIndex = 0;
                            if (listShowLCDLabelOfRow.Count > 0)
                            {
                                if (j == 0)
                                {
                                    label.Text = listShowLCDLabelOfRow[0].LabelName;
                                }

                                modelLabelOfTBLContent.listShowLCDLabel = listShowLCDLabelOfRow;
                            }
                            modelLabelOfTBLContent.label = label;
                            listLabel.Add(modelLabelOfTBLContent);
                            this.tblpanelContent.Controls.Add(label, j, i);
                        }
                    }
                }
            }
        }

        private void LoadLCDConfig()
        {
            try
            {
                var configs = BLLConfig.Instance.GetShowLCDConfig();
                if (configs != null && configs.Count > 0)
                {
                    foreach (var config in configs)
                    {
                        switch (config.Name.Trim().ToUpper())
                        {
                            case eShowLCDConfigName.IntervalGetTime: initTimerGetTime(int.Parse(config.Value)); break;
                            case eShowLCDConfigName.IntervalShow: initTimerShow(int.Parse(config.Value)); break;
                            case eShowLCDConfigName.TimeShowMH1_TimeShowMH2: setTimeShowMH1OrMH2(config.Value); break;
                            case eShowLCDConfigName.IntervalLoadData: initTimerLoadData(int.Parse(config.Value)); break;
                            case eShowLCDConfigName.Logo: ShowLogo(config.Value); break;
                            case eShowLCDConfigName.LoaiDoanhThuThang: int.TryParse(config.Value, out loaiDoanhThuThang); break;
                            case eShowLCDConfigName.TypeShowProductivitiesPerHour: int.TryParse(config.Value, out hienThiNSGio); break;
                            case eShowLCDConfigName.TimesGetNSInDay: int.TryParse(config.Value, out TimesGetNS); break;
                            case eShowLCDConfigName.KhoangCachLayNangSuat: int.TryParse(config.Value, out KhoangCachGetNSOnDay); break;
                        }
                    }
                }

                int appId = 0;
                int.TryParse(ConfigurationManager.AppSettings["AppId"].ToString(), out appId);
                var Configs = BLLConfig.Instance.GetAll(appId);
                int.TryParse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.CalculateNormsdayType)).Value.Trim(), out calculateNormsdayType);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi : " + ex.Message, "Lỗi xử lý", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region timer get time
        //Khoi tao timer lấy thời gian
        private void initTimerGetTime(int interval)
        {
            this.timerGetTime.Interval = interval;
            this.timerGetTime.Tick += new System.EventHandler(this.timerGetTime_Tick);
            this.timerGetTime.Enabled = true;
        }

        private void timerGetTime_Tick(object sender, EventArgs e)
        {
            try
            {
                SetTimeAndDate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi timer getTime: " + ex.Message, "Lỗi xử lý", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.timerGetTime.Enabled = false;
            }

        }

        private void SetTimeAndDate()
        {
            try
            {
                var dateNow = DateTime.Now;
                lblTime.Text = dateNow.Hour.ToString() + " : " + dateNow.Minute.ToString() + " : " + dateNow.Second.ToString();
                lblDate.Text = "NGÀY " + dateNow.Day.ToString() + "/" + dateNow.Month.ToString() + "/" + dateNow.Year.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region timer show
        //Khoi tao timer show
        private void initTimerShow(int interval)
        {
            this.timerShow.Interval = interval;
            this.timerShow.Tick += new System.EventHandler(this.timerShow_Tick);
            this.timerShow.Enabled = true;
        }

        private void timerShow_Tick(object sender, EventArgs e)
        {
            try
            {
                ShowContent();
            }
            catch (Exception ex)
            {
                intCurrentError++;
                if (intCurrentError > intMaxError)
                {
                    this.timerShow.Enabled = false;
                    MessageBox.Show("Lỗi: Timer lật trang. " + ex.Message + ". Hệ thống sẽ tắt tiến trình tự động");
                }
            }

        }

        private void GetTongGiayCuaThoiGianLamViecTrongNgay()
        {

            tongTimeOfWorkInDay = TimeIsWork(maChuyen.ToString());
            totalSecond = (int)tongTimeOfWorkInDay.TotalSeconds;
        }

        private void ShowContent()
        {
            try
            {
                if (countShowMH >= timeShowMH2)
                {
                    // ClearValueOfLable();

                    SetValueForLabelOfTBLPanelContent(0, manHinhShow, "thucHienVaDinhMuc", "");
                    SetValueForLabelOfTBLPanelContent(0, manHinhShow, "btpTrenChuyen", "");
                    SetValueForLabelOfTBLPanelContent(0, manHinhShow, "laoDong", "");
                    SetValueForLabelOfTBLPanelContent(0, manHinhShow, "nhipChuyen", "");

                    SetValueForLabelOfTBLPanelContent(0, manHinhShow, "maHang", "");
                    SetValueForLabelOfTBLPanelContent(0, manHinhShow, "sanLuongKeHoach", "");
                    SetValueForLabelOfTBLPanelContent(0, manHinhShow, "luyKeThucHien", "");
                    SetValueForLabelOfTBLPanelContent(0, manHinhShow, "doanhThuThang", "");
                    SetValueForLabelOfTBLPanelContent(0, manHinhShow, "thuNhapBQThang", "");
                    SetValueForLabelOfTBLPanelContent(0, manHinhShow, "tongThucHienNgay", "");

                    SetValueForLabelOfTBLPanelContent(0, manHinhShow, "ThoatChuyen", "");
                    SetValueForLabelOfTBLPanelContent(1, manHinhShow, "ProductionPlans", "");          


                    if (indexFinish == 0)
                    {
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "thucHienVaDinhMuc", thucHienVaDinhMuc1);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "btpTrenChuyen", btpTrenChuyen1);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "laoDong", laoDong1);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "nhipChuyen", nhipChuyen1);


                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "maHang", maHang1);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "sanLuongKeHoach", sanLuongKeHoach1);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "luyKeThucHien", luyKeThucHien1);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "doanhThuThang", doanhThuThang1);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "thuNhapBQThang", thuNhapBQThang1);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "tongThucHienNgay", tongThucHienNgay1);

                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "ThoatChuyen", ThoatChuyen);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "ProductionPlans", "");          


                    }
                    else
                    {

                        SetValueForLabelOfTBLPanelContent(2, manHinhShow, "thucHienVaDinhMuc", thucHienVaDinhMuc2);
                        SetValueForLabelOfTBLPanelContent(2, manHinhShow, "btpTrenChuyen", btpTrenChuyen2);
                        SetValueForLabelOfTBLPanelContent(2, manHinhShow, "laoDong", laoDong2);
                        SetValueForLabelOfTBLPanelContent(2, manHinhShow, "nhipChuyen", nhipChuyen2);

                        SetValueForLabelOfTBLPanelContent(2, manHinhShow, "maHang", maHang2);
                        SetValueForLabelOfTBLPanelContent(2, manHinhShow, "sanLuongKeHoach", sanLuongKeHoach2);
                        SetValueForLabelOfTBLPanelContent(2, manHinhShow, "luyKeThucHien", luyKeThucHien2);
                        SetValueForLabelOfTBLPanelContent(2, manHinhShow, "doanhThuThang", doanhThuThang2);
                        SetValueForLabelOfTBLPanelContent(2, manHinhShow, "thuNhapBQThang", thuNhapBQThang2);
                        SetValueForLabelOfTBLPanelContent(2, manHinhShow, "tongThucHienNgay", tongThucHienNgay2);

                        SetValueForLabelOfTBLPanelContent(2, manHinhShow, "ThoatChuyen", ThoatChuyen);
                        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "ProductionPlans", "");          

                    }

                    if (manHinhShow < soLanLatTrang)
                        manHinhShow++;
                    else
                    {
                        manHinhShow = 1;
                        if (listChuyen.Count > 1)
                        {
                            for (int i = 0; i < listChuyen.Count; i++)
                            {
                                if (listChuyen[i].MaChuyen.Equals(maChuyen.ToString()))
                                {
                                    if (i == listChuyen.Count - 1)
                                    {
                                        int.TryParse(listChuyen[0].MaChuyen, out maChuyen);
                                        lblTableName.Text = "BẢNG NĂNG SUẤT " + listChuyen[0].TenChuyen.ToUpper();
                                    }
                                    else
                                    {
                                        int.TryParse(listChuyen[i + 1].MaChuyen, out maChuyen);
                                        lblTableName.Text = "BẢNG NĂNG SUẤT " + listChuyen[i + 1].TenChuyen.ToUpper();
                                    }
                                    //GetTongGiayCuaThoiGianLamViecTrongNgay();
                                    //IsDesign = false;
                                    break;
                                }
                            }
                        }
                    }

                    //manHinhShow = 2;
                    countShowMH = 0;
                }
                else
                    countShowMH++;

                //if (manHinhShow == 1)
                //{
                //    if (countShowMH >= timeShowMH2)
                //    {
                //        ClearValueOfLable();

                //        SetValueForLabelOfTBLPanelContent(0, manHinhShow, "thucHienVaDinhMuc", "");
                //        SetValueForLabelOfTBLPanelContent(0, manHinhShow, "btpTrenChuyen", "");
                //        SetValueForLabelOfTBLPanelContent(0, manHinhShow, "laoDong", "");
                //        SetValueForLabelOfTBLPanelContent(0, manHinhShow, "nhipChuyen", "");

                //        SetValueForLabelOfTBLPanelContent(0, manHinhShow, "maHang", "");
                //        SetValueForLabelOfTBLPanelContent(0, manHinhShow, "sanLuongKeHoach", "");
                //        SetValueForLabelOfTBLPanelContent(0, manHinhShow, "luyKeThucHien", "");
                //        SetValueForLabelOfTBLPanelContent(0, manHinhShow, "doanhThuThang", "");
                //        SetValueForLabelOfTBLPanelContent(0, manHinhShow, "thuNhapBQThang", "");


                //        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "thucHienVaDinhMuc", thucHienVaDinhMuc1);
                //        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "btpTrenChuyen", btpTrenChuyen1);
                //        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "laoDong", laoDong1);
                //        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "nhipChuyen", nhipChuyen1);


                //        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "maHang", maHang1);
                //        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "sanLuongKeHoach", sanLuongKeHoach1);
                //        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "luyKeThucHien", luyKeThucHien1);
                //        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "doanhThuThang", doanhThuThang1);
                //        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "thuNhapBQThang", thuNhapBQThang1);

                //        SetValueForLabelOfTBLPanelContent(2, manHinhShow, "thucHienVaDinhMuc", thucHienVaDinhMuc2);
                //        SetValueForLabelOfTBLPanelContent(2, manHinhShow, "btpTrenChuyen", btpTrenChuyen2);
                //        SetValueForLabelOfTBLPanelContent(2, manHinhShow, "laoDong", laoDong2);
                //        SetValueForLabelOfTBLPanelContent(2, manHinhShow, "nhipChuyen", nhipChuyen2);

                //        SetValueForLabelOfTBLPanelContent(2, manHinhShow, "maHang", maHang2);
                //        SetValueForLabelOfTBLPanelContent(2, manHinhShow, "sanLuongKeHoach", sanLuongKeHoach2);
                //        SetValueForLabelOfTBLPanelContent(2, manHinhShow, "luyKeThucHien", luyKeThucHien2);
                //        SetValueForLabelOfTBLPanelContent(2, manHinhShow, "doanhThuThang", doanhThuThang2);
                //        SetValueForLabelOfTBLPanelContent(2, manHinhShow, "thuNhapBQThang", thuNhapBQThang2);

                //        if (manHinhShow < soLanLatTrang)
                //            manHinhShow++;
                //        else
                //        {
                //            manHinhShow = 1;

                //        }

                //        //manHinhShow = 2;
                //        countShowMH = 0;
                //    }
                //    else
                //        countShowMH++;
                //}
                //else
                //{
                //    if (countShowMH >= timeShowMH1)
                //    {
                //        ClearValueOfLable();

                //        SetValueForLabelOfTBLPanelContent(0, manHinhShow, "thucHienVaDinhMuc", "");
                //        SetValueForLabelOfTBLPanelContent(0, manHinhShow, "btpTrenChuyen", "");
                //        SetValueForLabelOfTBLPanelContent(0, manHinhShow, "laoDong", "");
                //        SetValueForLabelOfTBLPanelContent(0, manHinhShow, "nhipChuyen", "");

                //        SetValueForLabelOfTBLPanelContent(0, manHinhShow, "maHang", "");
                //        SetValueForLabelOfTBLPanelContent(0, manHinhShow, "sanLuongKeHoach", "");
                //        SetValueForLabelOfTBLPanelContent(0, manHinhShow, "luyKeThucHien", "");
                //        SetValueForLabelOfTBLPanelContent(0, manHinhShow, "doanhThuThang", "");
                //        SetValueForLabelOfTBLPanelContent(0, manHinhShow, "thuNhapBQThang", "");


                //        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "thucHienVaDinhMuc", thucHienVaDinhMuc1);
                //        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "btpTrenChuyen", btpTrenChuyen1);
                //        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "laoDong", laoDong1);
                //        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "nhipChuyen", nhipChuyen1);


                //        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "maHang", maHang1);
                //        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "sanLuongKeHoach", sanLuongKeHoach1);
                //        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "luyKeThucHien", luyKeThucHien1);
                //        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "doanhThuThang", doanhThuThang1);
                //        SetValueForLabelOfTBLPanelContent(1, manHinhShow, "thuNhapBQThang", thuNhapBQThang1);

                //        SetValueForLabelOfTBLPanelContent(2, manHinhShow, "thucHienVaDinhMuc", thucHienVaDinhMuc2);
                //        SetValueForLabelOfTBLPanelContent(2, manHinhShow, "btpTrenChuyen", btpTrenChuyen2);
                //        SetValueForLabelOfTBLPanelContent(2, manHinhShow, "laoDong", laoDong2);
                //        SetValueForLabelOfTBLPanelContent(2, manHinhShow, "nhipChuyen", nhipChuyen2);

                //        SetValueForLabelOfTBLPanelContent(2, manHinhShow, "maHang", maHang2);
                //        SetValueForLabelOfTBLPanelContent(2, manHinhShow, "sanLuongKeHoach", sanLuongKeHoach2);
                //        SetValueForLabelOfTBLPanelContent(2, manHinhShow, "luyKeThucHien", luyKeThucHien2);
                //        SetValueForLabelOfTBLPanelContent(2, manHinhShow, "doanhThuThang", doanhThuThang2);
                //        SetValueForLabelOfTBLPanelContent(2, manHinhShow, "thuNhapBQThang", thuNhapBQThang2);


                //        manHinhShow = 1;
                //        countShowMH = 0;

                //    }
                //    else
                //        countShowMH++;
                //}
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion

        private void ClearValueOfLable()
        {
            try
            {
                if (listLabel != null && listLabel.Count > 0)
                {
                    foreach (var label in listLabel)
                    {
                        label.label.Text = "";

                    }
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void SetValueForLabelOfTBLPanelContent(int intColumn, int intLuotShow, string systemValueName, string value)
        {
            try
            {
                if (listLabel != null && listLabel.Count > 0)
                {
                    if (intColumn == 0)
                    {
                        foreach (var label in listLabel)
                        {
                            if (label.intColumn == 0)
                            {
                                if (label.listShowLCDLabel != null)
                                {
                                    bool findResult = false;
                                    foreach (var obj in label.listShowLCDLabel)
                                    {
                                        if (obj.SystemValueName.Trim().ToUpper().Equals(systemValueName.Trim().ToUpper()))
                                        {
                                            if (obj.SttNext == intLuotShow)
                                            {
                                                label.label.Text = obj.LabelName;
                                                findResult = true;
                                                break;
                                            }
                                        }
                                    }
                                    if (findResult)
                                        break;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var label in listLabel)
                        {
                            if (label.intColumn == intColumn)
                            {
                                if (label.listShowLCDLabel != null)
                                {
                                    bool findResult = false;
                                    foreach (var obj in label.listShowLCDLabel)
                                    {
                                        if (obj.SystemValueName.Trim().ToUpper().Equals(systemValueName.Trim().ToUpper()))
                                        {
                                            if (obj.SttNext == intLuotShow)
                                            {
                                                label.label.Text = value;
                                                findResult = true;
                                                break;
                                            }
                                        }
                                    }
                                    if (findResult)
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            { }
        }

        #region timer load Data
        //Khoi tao timer load Data
        private void initTimerLoadData(int interval)
        {
            this.timerLoadData.Interval = interval;
            this.timerLoadData.Tick += new System.EventHandler(this.timerLoadData_Tick);
            this.timerLoadData.Enabled = true;
        }

        private void timerLoadData_Tick(object sender, EventArgs e)
        {
            try
            {
                //  TTAll(maChuyen); 
                GetProductivitiesOfLine(maChuyen);
                // GetNangSuatGio();
            }
            catch (Exception ex)
            {
                intCurrentError++;
                if (intCurrentError > intMaxError)
                {
                    this.timerLoadData.Enabled = false;
                    MessageBox.Show("Lỗi: Timer lấy dữ liệu mới." + ex.Message + ". Hệ thống sẽ tắt tiến trình tự động");
                }
            }
        }
        #endregion

        private void setTimeShowMH1OrMH2(string tileShow_MH1MH2)
        {
            if (!string.IsNullOrEmpty(tileShow_MH1MH2))
            {
                var arrTimeShow = Regex.Split(tileShow_MH1MH2, "/");
                if (arrTimeShow != null && arrTimeShow.Length > 0)
                {
                    int.TryParse(arrTimeShow[0], out timeShowMH1);
                    int.TryParse(arrTimeShow[1], out timeShowMH2);
                }
            }
        }

        private List<Shift> TTCaCuaChuyen(int MaChuyen)
        {
            List<Shift> listShift = new List<Shift>();
            DataTable dtAllShift = new DataTable();
            string strSQL = "select IdShift, Name, TimeStart, TimeEnd from Shift where MaChuyen =" + MaChuyen + " order by IdShift";
            dtAllShift = dbclass.TruyVan_TraVe_DataTable(strSQL);
            if (dtAllShift.Rows.Count > 0)
            {
                for (int i = 0; i < dtAllShift.Rows.Count; i++)
                {
                    Shift shift = new Shift()
                    {
                        IdShift = int.Parse(dtAllShift.Rows[i][0].ToString()),
                        Name = dtAllShift.Rows[i][1].ToString(),
                        TimeStart = TimeSpan.Parse(dtAllShift.Rows[i][2].ToString()),
                        TimeEnd = TimeSpan.Parse(dtAllShift.Rows[i][3].ToString())
                    };
                    listShift.Add(shift);
                }
            }
            return listShift;
        }

        private int SumHoursOfShifts()
        {
            int countHours = 0;
            double sumHours = 0;
            double sumMinuterWork = 0;
            try
            {
                shifts_ = BLLShift.GetShiftsOfLine(maChuyen);
                shifts = BLLShift.GetListWorkHoursOfLineByLineId(maChuyen);// TTCaCuaChuyen(maChuyen);
                var shiftsOfLine = shifts_; // BLLShift.GetListWorkHoursOfLineByLineId(maChuyen);
                if (shiftsOfLine != null && shiftsOfLine.Count > 0)
                {
                    foreach (var shift in shiftsOfLine)
                    {
                        sumHours += shift.End.TotalHours - shift.Start.TotalHours;
                        sumMinuterWork += shift.End.TotalMinutes - shift.Start.TotalMinutes;
                    }
                }
                intWorkTime = sumHours;
                intWorkMinuter = sumMinuterWork;
                int intSumHours = (int)sumHours;
                if (intSumHours == sumHours)
                    countHours = intSumHours;
                else
                    countHours = intSumHours + 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tính tổng số giờ làm việc trong ngày: " + ex.Message, "Lỗi xử lý", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return countHours;
        }

        private List<ModelWorkHours> GetThongTinGioLVCuaChuyen()
        {
            var listWorkHours = new List<ModelWorkHours>();
            try
            {
                #region HoangHai
                shifts = BLLShift.GetListWorkHoursOfLineByLineId(maChuyen);// TTCaCuaChuyen(maChuyen);
                if (shifts != null && shifts.Count > 0)
                {
                    int intHours = 1;
                    TimeSpan timeEnd = new TimeSpan(0, 0, 0);
                    TimeSpan timeStart = new TimeSpan(0, 0, 0);
                    bool isWaitingTimeEnd = false;
                    double dHoursShiftOld = 0;
                    for (int i = 0; i < shifts.Count; i++)
                    {
                        var shift = shifts[i];
                        while (true)
                        {
                            if (!isWaitingTimeEnd)
                            {
                                if (timeStart == new TimeSpan(0, 0, 0))
                                    timeStart = shift.TimeStart;
                                else
                                    timeStart = timeEnd;
                            }
                            else
                            {
                                if (dHoursShiftOld == 0)
                                    timeStart = shift.TimeStart;
                            }
                            if (timeStart > shift.TimeEnd)
                            {
                                break;
                            }
                            else
                            {
                                if (!isWaitingTimeEnd)
                                    timeEnd = timeStart.Add(new TimeSpan(1, 0, 0));
                                else
                                {
                                    if (dHoursShiftOld > 0)
                                    {
                                        double hour = 1 - dHoursShiftOld;
                                        int minuter = (int)(hour * 60);
                                        timeEnd = shift.TimeStart.Add(new TimeSpan(0, minuter, 0));

                                    }
                                    else
                                        timeEnd = timeStart.Add(new TimeSpan(1, 0, 0));
                                    isWaitingTimeEnd = false;
                                }
                                if (timeEnd <= shift.TimeEnd)
                                {
                                    listWorkHours.Add(new ModelWorkHours() { IntHours = intHours, TimeStart = timeStart, TimeEnd = timeEnd, Name = "Giờ " + intHours + " (" + timeStart + "-" + timeEnd + ")" });
                                    intHours++;
                                }
                                else
                                {
                                    isWaitingTimeEnd = true;
                                    dHoursShiftOld = shift.TimeEnd.TotalHours - timeStart.TotalHours;
                                    if (dHoursShiftOld != 0 && i == shifts.Count - 1)
                                    {
                                        listWorkHours.Add(new ModelWorkHours() { IntHours = intHours, TimeStart = timeStart, TimeEnd = shift.TimeEnd, Name = "Giờ " + intHours + " (" + timeStart + "-" + shift.TimeEnd + ")" });
                                        intHours++;
                                    }
                                    break;
                                }
                            }
                            if (intHours > 30)
                                break;
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy thông tin giờ Làm việc của chuyền:" + ex.Message, "Lỗi xử lý", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return listWorkHours;
        }

        private double GetSoPhutLamViecTrongNgay(TimeSpan timeNow)
        {
            double soPhut = 1;
            try
            {
                if (shifts != null && shifts.Count > 0)
                {
                    foreach (var sh in shifts)
                    {
                        if (sh.TimeStart <= timeNow)
                        {
                            if (timeNow <= sh.TimeEnd)
                                soPhut += timeNow.TotalMinutes - sh.TimeStart.TotalMinutes;
                            else
                                soPhut += sh.TimeEnd.TotalMinutes - sh.TimeStart.TotalMinutes;
                        }
                    }
                }
                int intSoPhut = (int)soPhut;
                if (intSoPhut < soPhut)
                    soPhut++;
            }
            catch (Exception)
            { }
            return soPhut;
        }

        private double GetSoPhutLamViecTrongNgay(TimeSpan timeNow, List<PMS.Business.Models.WorkingTimeModel> workShift)
        {
            double soPhut = 1;
            try
            {
                if (workShift != null && workShift.Count > 0)
                {
                    foreach (var sh in workShift)
                    {
                        if (sh.TimeStart <= timeNow)
                        {
                            if (timeNow <= sh.TimeEnd)
                                soPhut += (timeNow.TotalMinutes - sh.TimeStart.TotalMinutes) > 60 ? ((timeNow.TotalMinutes - sh.TimeStart.TotalMinutes) - 60) : (timeNow.TotalMinutes - sh.TimeStart.TotalMinutes);
                            else
                                soPhut += (sh.TimeEnd.TotalMinutes - sh.TimeStart.TotalMinutes) > 60 ? ((sh.TimeEnd.TotalMinutes - sh.TimeStart.TotalMinutes) - 60) : (sh.TimeEnd.TotalMinutes - sh.TimeStart.TotalMinutes);
                        }
                    }
                }
                int intSoPhut = (int)soPhut;
                if (intSoPhut < soPhut)
                    soPhut++;
            }
            catch (Exception)
            { }
            return soPhut;
        }

        private double GetSoPhutLamViecTrongNgay_(TimeSpan timeNow, List<PMS.Business.Models.LineWorkingShiftModel> workShift)
        {
            double soPhut = 0;
            try
            {
                if (workShift != null && workShift.Count > 0)
                {
                    foreach (var sh in workShift)
                    {
                        if (timeNow >= sh.Start)
                        {
                            if (timeNow < sh.End)
                            {
                                var h = timeNow.Hours - sh.Start.Hours;
                                soPhut += (h * 60 + timeNow.Minutes) - sh.Start.Minutes;
                            }
                            else if (timeNow >= sh.End)
                                soPhut += ((sh.End - sh.Start).TotalMinutes);
                        }
                    }
                }
            }
            catch (Exception)
            { }
            return soPhut;
        }


        private void designNangSuatGio()
        {

            this.panelFooter.Controls.Add(this.tblpanelFooter);
            this.tblpanelFooter.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tblpanelFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblpanelFooter.Location = new System.Drawing.Point(0, 0);
            this.tblpanelFooter.Name = "tblpanelFooter";
            this.tblpanelFooter.Size = new System.Drawing.Size(386, 162);
            //  this.tblpanelFooter.Size = new System.Drawing.Size(886, 162);
            this.tblpanelFooter.TabIndex = 1;

            if (hienThiNSGio == 2)
            {
                this.tblpanelFooter.RowCount = 1;
                this.tblpanelFooter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
                tblpanelFooter.ColumnCount = 2;
                Label lbl = new Label();
                lbl.Anchor = System.Windows.Forms.AnchorStyles.None;
                lbl.AutoSize = true;
                DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigTopOftblpanelFooter, lbl);
                lbl.Text = "NĂNG SUẤT HIỆN TẠI";
                tblpanelFooter.Controls.Add(lbl, 0, 0);

                Label lblNangSuatGio = new Label();
                lblNangSuatGio.Anchor = System.Windows.Forms.AnchorStyles.None;
                lblNangSuatGio.AutoSize = true;
                DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigDownOftblpanelFooter, lblNangSuatGio);
                lblNangSuatGio.Size = new System.Drawing.Size(31, 17);
                lblNangSuatGio.Name = "lblNangSuatGio";
                lblNangSuatGio.Text = "";
                listLableNangSuatGio.Add(lblNangSuatGio);
                tblpanelFooter.Controls.Add(lblNangSuatGio, 1, 0);
            }
            else
            {

                this.tblpanelFooter.RowCount = 2;
                this.tblpanelFooter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                this.tblpanelFooter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
                //get theo ca lam viec
                List<WorkingTimeModel> lineWorkingTimes = new List<WorkingTimeModel>();
                switch (hienThiNSGio)
                {
                    case (int)eShowNSType.PercentTH_FollowHour:
                    case (int)eShowNSType.TH_Err_FollowHour:
                    case (int)eShowNSType.TH_DM_FollowHour:
                    case (int)eShowNSType.TH_TC_FollowHour:
                        lineWorkingTimes = BLLShift.GetListWorkHoursOfLineByLineId(maChuyen);
                        break;
                    case (int)eShowNSType.PercentTH_FollowConfig:
                    case (int)eShowNSType.TH_Err_FollowConfig:
                    case (int)eShowNSType.TH_DM_FollowConfig:
                    case (int)eShowNSType.TH_TC_FollowConfig:
                        lineWorkingTimes = BLLShift.GetListWorkHoursOfLineByLineId(maChuyen, TimesGetNS);
                        break;
                    case (int)eShowNSType.TH_DM_OnDay:
                    case (int)eShowNSType.TH_TC_OnDay:
                    case (int)eShowNSType.TH_Error_OnDay:
                        lineWorkingTimes.Add(new WorkingTimeModel()
                        {
                            TimeStart = DateTime.Now.AddHours(-KhoangCachGetNSOnDay).TimeOfDay,
                            TimeEnd = DateTime.Now.TimeOfDay,
                            IntHours = 1,
                        });

                        lineWorkingTimes.Add(new WorkingTimeModel()
                        {
                            TimeStart = DateTime.Now.AddHours(-(KhoangCachGetNSOnDay + KhoangCachGetNSOnDay)).TimeOfDay,
                            TimeEnd = DateTime.Now.AddHours(-KhoangCachGetNSOnDay).TimeOfDay,
                            IntHours = 2,
                        });
                        break;
                }

                if (lineWorkingTimes.Count > 0)
                {
                    tblpanelFooter.ColumnCount = lineWorkingTimes.Count;
                    float columnPercent = (100 / lineWorkingTimes.Count);
                    for (int i = 0; i < lineWorkingTimes.Count; i++)
                    {
                        if (i == 0 && ((int)eShowNSType.TH_Error_OnDay == hienThiNSGio || (int)eShowNSType.TH_TC_OnDay == hienThiNSGio || (int)eShowNSType.TH_DM_OnDay == hienThiNSGio))
                            DesignLabel("NS " + KhoangCachGetNSOnDay + "H hiện tại", columnPercent, i);
                        else if (i == 1 && ((int)eShowNSType.TH_Error_OnDay == hienThiNSGio || (int)eShowNSType.TH_TC_OnDay == hienThiNSGio || (int)eShowNSType.TH_DM_OnDay == hienThiNSGio))
                            DesignLabel("NS " + KhoangCachGetNSOnDay + "H trước", columnPercent, i);
                        else
                            DesignLabel((lineWorkingTimes[i].TimeStart.ToString("h':'m") + " - " + lineWorkingTimes[i].TimeEnd.ToString("h':'m")), columnPercent, i);
                    }
                }

            }
            this.tblpanelFooter.ResumeLayout(false);
            this.tblpanelFooter.PerformLayout();
        }

        private void DesignLabel(string labelName, float columnPercent, int i)
        {
            tblpanelFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, columnPercent));
            Label lblSo = new Label();
            lblSo.Anchor = System.Windows.Forms.AnchorStyles.None;
            lblSo.AutoSize = true;
            DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigTopOftblpanelFooter, lblSo);
            switch (hienThiNSGio)
            {
                //Theo ca lam viec chia ra tung giờ                
                case (int)eShowNSType.PercentTH_FollowHour:
                    lblSo.Text = (i + 1).ToString() + "H"; break;

                case (int)eShowNSType.TH_DM_FollowHour:
                    lblSo.Text = (i + 1).ToString() + "H (%)"; break;

                case (int)eShowNSType.TH_DM_FollowConfig:
                case (int)eShowNSType.PercentTH_FollowConfig:
                case (int)eShowNSType.TH_TC_FollowHour:
                case (int)eShowNSType.TH_TC_FollowConfig:
                case (int)eShowNSType.TH_Err_FollowHour:
                case (int)eShowNSType.TH_Err_FollowConfig:
                case (int)eShowNSType.TH_DM_OnDay:
                case (int)eShowNSType.TH_TC_OnDay:
                case (int)eShowNSType.TH_Error_OnDay:
                    lblSo.Text = labelName; break;
            }

            tblpanelFooter.Controls.Add(lblSo, i, 0);

            Label lblNangSuatGio = new Label();
            lblNangSuatGio.Anchor = System.Windows.Forms.AnchorStyles.None;
            lblNangSuatGio.AutoSize = true;
            DuAn03_HaiDang.Helper.HelperControl.SetConfigForLable(labelConfigDownOftblpanelFooter, lblNangSuatGio);
            lblNangSuatGio.Size = new System.Drawing.Size(131, 17);

            lblNangSuatGio.Name = "lblNangSuatGio" + (i + 1).ToString();
            lblNangSuatGio.Tag = i + 1;
            listLableNangSuatGio.Add(lblNangSuatGio);
            tblpanelFooter.Controls.Add(lblNangSuatGio, i, 1);
        }

        private int GetSoThanhPhamTheoGio(TimeSpan timeStart, TimeSpan timeEnd)
        {
            dtCountThanhPham.Clear();
            int SoLuong = 0;
            try
            {
                string sqlGetThanhPham = "select ThanhPham, CommandTypeId from TheoDoiNgay where CommandTypeId in (" + (int)eCommandRecive.ProductIncrease + "," + (int)eCommandRecive.ProductReduce + ") and MaChuyen=" + maChuyen + " and Time >= '" + timeStart + "' and Time <='" + timeEnd + "' and Date='" + DateTime.Now + "' and IsEndOfLine=1 and ProductOutputTypeId=" + (int)eProductOutputType.KCS;
                dtCountThanhPham = dbclass.TruyVan_TraVe_DataTable(sqlGetThanhPham);
                if (dtCountThanhPham != null && dtCountThanhPham.Rows.Count > 0)
                {
                    int soLuongTang = 0;
                    int soLuongGiam = 0;
                    foreach (DataRow row in dtCountThanhPham.Rows)
                    {
                        int commandTypeId = 0;
                        int.TryParse(row["CommandTypeId"].ToString(), out commandTypeId);
                        int sl = 0;
                        int.TryParse(row["ThanhPham"].ToString(), out sl);
                        switch (commandTypeId)
                        {
                            case (int)eCommandRecive.ProductIncrease:
                                soLuongTang += sl;
                                break;
                            case (int)eCommandRecive.ProductReduce:
                                soLuongGiam += sl;
                                break;
                        }
                    }
                    SoLuong = soLuongTang - soLuongGiam;
                    if (SoLuong < 0)
                        SoLuong = 0;
                }
            }
            catch (Exception ex)
            {

            }
            return SoLuong;
        }

        private int GetSoThanhPhamTheoThoiGianHienTai(TimeSpan timeNow)
        {
            dtCountThanhPham.Clear();
            int SoLuong = 0;
            try
            {
                string sqlGetThanhPham = "select ThanhPham, CommandTypeId from TheoDoiNgay where CommandTypeId in (" + (int)eCommandRecive.ProductIncrease + "," + (int)eCommandRecive.ProductReduce + ") and MaChuyen=" + maChuyen + " and Time <='" + timeNow + "' and Date='" + DateTime.Now + "' and IsEndOfLine=1 and ProductOutputTypeId=" + (int)eProductOutputType.KCS;
                dtCountThanhPham = dbclass.TruyVan_TraVe_DataTable(sqlGetThanhPham);
                if (dtCountThanhPham != null && dtCountThanhPham.Rows.Count > 0)
                {
                    int soLuongTang = 0;
                    int soLuongGiam = 0;
                    foreach (DataRow row in dtCountThanhPham.Rows)
                    {
                        int commandTypeId = 0;
                        int.TryParse(row["CommandTypeId"].ToString(), out commandTypeId);
                        int sl = 0;
                        int.TryParse(row["ThanhPham"].ToString(), out sl);
                        switch (commandTypeId)
                        {
                            case (int)eCommandRecive.ProductIncrease:
                                soLuongTang += sl;
                                break;
                            case (int)eCommandRecive.ProductReduce:
                                soLuongGiam += sl;
                                break;
                        }
                    }
                    SoLuong = soLuongTang - soLuongGiam;
                    if (SoLuong < 0)
                        SoLuong = 0;
                }
            }
            catch (Exception ex)
            {
            }
            return SoLuong;
        }

        bool IsDesign = false;
        int ColumnNumber = 0;
        DataTable dtSanLuongGio = new DataTable();

        //HoangHai
        private void GetNangSuatGio()
        {
            try
            {
                double NangSuatPhutKH = 0;
                int NangSuatGioKH = 0;
                var dateNow = DateTime.Now.Date;
                if (intWorkTime > 0)
                {
                    NangSuatPhutKH = DinhMucNgayCuaChuyen / intWorkMinuter;
                    NangSuatGioKH = (int)(DinhMucNgayCuaChuyen / intWorkTime);
                    if (DinhMucNgayCuaChuyen % intWorkTime != 0)
                        NangSuatGioKH++;
                }
                switch (hienThiNSGio)
                {
                    case 0:
                    case 1:
                    case 3:
                        #region Thực Hiện / Định Mức Or Hiển thị theo %
                        if (listModelWorkHours != null && listModelWorkHours.Count > 0)
                        {
                            foreach (var item in listModelWorkHours)
                            {
                                dtSanLuongGio.Clear();
                                int sanLuongGioTang = 0, sanLuongGioGiam = 0, sanLuongGio = 0, TCTang = 0, TCGiam = 0, tongTC = 0;

                                string sqlSanLuongGio = "select (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + maChuyen + " and Time >= '" + item.TimeStart + "' and Time <='" + item.TimeEnd + "' and Date='" + dateNow + "' and CommandTypeId=" + (int)eCommandRecive.ProductIncrease + " and ProductOutputTypeId=" + (int)eProductOutputType.KCS + " and IsEndOfLine=1) AS SanLuongTang,";
                                sqlSanLuongGio += "(select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + maChuyen + " and Time >= '" + item.TimeStart + "' and Time <='" + item.TimeEnd + "' and Date='" + dateNow + "' and CommandTypeId=" + (int)eCommandRecive.ProductReduce + " and ProductOutputTypeId=" + (int)eProductOutputType.KCS + " and IsEndOfLine=1) AS SanLuongGiam,";

                                if (hienThiNSGio == 3)
                                {
                                    sqlSanLuongGio += " (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + maChuyen + " and Time >= '" + item.TimeStart + "' and Time <='" + item.TimeEnd + "' and Date='" + dateNow + "' and CommandTypeId=" + (int)eCommandRecive.ProductIncrease + " and ProductOutputTypeId=" + (int)eProductOutputType.TC + " and IsEndOfLine=1) AS TCTang,";
                                    sqlSanLuongGio += " (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + maChuyen + " and Time >= '" + item.TimeStart + "' and Time <='" + item.TimeEnd + "' and Date='" + dateNow + "' and CommandTypeId=" + (int)eCommandRecive.ProductReduce + " and ProductOutputTypeId=" + (int)eProductOutputType.TC + " and IsEndOfLine=1) AS TCGiam";
                                }

                                dtSanLuongGio = dbclass.TruyVan_TraVe_DataTable(sqlSanLuongGio);
                                if (dtSanLuongGio != null && dtSanLuongGio.Rows.Count > 0)
                                {
                                    DataRow rowSanLuongGio = dtSanLuongGio.Rows[0];
                                    if (rowSanLuongGio["SanLuongTang"] != null)
                                        int.TryParse(rowSanLuongGio["SanLuongTang"].ToString(), out sanLuongGioTang);
                                    if (rowSanLuongGio["SanLuongGiam"] != null)
                                        int.TryParse(rowSanLuongGio["SanLuongGiam"].ToString(), out sanLuongGioGiam);
                                    sanLuongGio = sanLuongGioTang - sanLuongGioGiam;

                                    if (hienThiNSGio == 3)
                                    {
                                        if (rowSanLuongGio["TCTang"] != null)
                                            int.TryParse(rowSanLuongGio["TCTang"].ToString(), out TCTang);
                                        if (rowSanLuongGio["TCGiam"] != null)
                                            int.TryParse(rowSanLuongGio["TCGiam"].ToString(), out TCGiam);
                                        tongTC = TCTang - TCGiam;
                                    }
                                }

                                switch (hienThiNSGio)
                                {
                                    case 0: listLableNangSuatGio[item.IntHours - 1].Text = (sanLuongGio < 0 ? 0 : sanLuongGio) + "/" + NangSuatGioKH.ToString();
                                        break;
                                    case 1: listLableNangSuatGio[item.IntHours - 1].Text = Math.Round((double)(NangSuatGioKH > 0 ? ((sanLuongGio * 100) / NangSuatGioKH) : 0), 2).ToString();
                                        break;
                                    case 3:
                                        listLableNangSuatGio[item.IntHours - 1].Text = (sanLuongGio < 0 ? 0 : sanLuongGio) + "/" + (tongTC < 0 ? 0 : tongTC);
                                        if (listLableNangSuatGio[item.IntHours - 1].Text.Length > 6)
                                        {
                                            listLableNangSuatGio[item.IntHours - 1].Font = new Font(listLableNangSuatGio[item.IntHours - 1].Font.FontFamily, 24, listLableNangSuatGio[item.IntHours - 1].Font.Style);
                                            listLableNangSuatGio[item.IntHours - 1].Size = new System.Drawing.Size(210, 37);
                                            listLableNangSuatGio[item.IntHours - 1].AutoSize = false;
                                        }
                                        else
                                        {
                                            listLableNangSuatGio[item.IntHours - 1].AutoSize = true;
                                            listLableNangSuatGio[item.IntHours - 1].Font = new Font(listLableNangSuatGio[item.IntHours - 1].Font.FontFamily, 32, listLableNangSuatGio[item.IntHours - 1].Font.Style);
                                        }
                                        break;
                                }
                            }
                        }
                        #endregion
                        break;
                    case 2:
                        #region  hiển thị một ô năng suất hiện tại duy nhất
                        TimeSpan timeNow = DateTime.Now.TimeOfDay;
                        int soLuongHienTai = GetSoThanhPhamTheoThoiGianHienTai(timeNow);
                        //  double nangSuatKHGioHienTai = GetSoPhutLamViecTrongNgay(timeNow) * NangSuatPhutKH;
                        double nangSuatKHGioHienTai = GetSoPhutLamViecTrongNgay(timeNow) * NangSuatPhutKH;
                        int intNangSuatHienTai = (int)nangSuatKHGioHienTai;
                        if (intNangSuatHienTai < nangSuatKHGioHienTai)
                            intNangSuatHienTai++;
                        double tiLePhanTram = 0;
                        if (nangSuatKHGioHienTai > 0)
                            tiLePhanTram = Math.Round((double)((soLuongHienTai * 100) / nangSuatKHGioHienTai), 2);

                        listLableNangSuatGio[0].Text = soLuongHienTai.ToString() + "/" + intNangSuatHienTai.ToString() + "  (" + tiLePhanTram.ToString() + "%)";
                        #endregion
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Hàm lay thời gian làm việc trong mot ngay cua chuyen
        InformationChuyen chuyenTimeWork = new InformationChuyen();
        private TimeSpan TimeIsWork(string MaChuyen)
        {
            DateTime daynow = DateTime.Now.Date;
            TimeSpan timeStartTT = thoigiantinhndttDAO.LayTimeBatDau(daynow, int.Parse(MaChuyen));
            TimeSpan timeWork = new TimeSpan();
            timeWork = TimeSpan.Parse("00:00:00");
            TimeSpan timeNow = DateTime.Now.TimeOfDay;
            if (listChuyen.Count > 0)
            {
                chuyenTimeWork = null;
                for (int i = 0; i < listChuyen.Count; i++)
                {
                    if (listChuyen[i].MaChuyen == MaChuyen)
                    {
                        chuyenTimeWork = listChuyenInf[i];
                        break;
                    }
                }
                foreach (var item in chuyenTimeWork.listShift)
                {
                    if (item.TimeStart < timeStartTT && timeStartTT < item.TimeEnd)
                    {
                        item.TimeStart = timeStartTT;
                    }
                    else if (item.TimeEnd <= timeStartTT)
                    {
                        chuyenTimeWork.listShift.Remove(item);
                    }
                }
                if (chuyenTimeWork.listShift.Count > 0)
                {
                    for (int j = 0; j < chuyenTimeWork.listShift.Count; j++)
                    {
                        if (timeNow > chuyenTimeWork.listShift[j].TimeStart)
                        {
                            if (timeNow < chuyenTimeWork.listShift[j].TimeEnd)
                            {
                                timeWork += (timeNow - chuyenTimeWork.listShift[j].TimeStart);
                            }
                            else
                            {
                                timeWork += (chuyenTimeWork.listShift[j].TimeEnd - chuyenTimeWork.listShift[j].TimeStart);
                            }
                        }
                    }
                }

            }
            return timeWork;
        }

        int DinhMucNgayCuaChuyen = 0;
       // DataTable dtNangSuatChuyenSanPham = new DataTable();
       int indexFinish = -1;
        //private void TTAll(int maChuyen)
        //{
        //    try
        //    {
        //        dtLoadDataAll.Clear();
        //        dtchuyenSanPham.Clear();
        //        dtThongTinThang.Clear();
        //        dtLaoDongVaThucHienNgay.Clear();
        //        listDataAll.Clear();
        //        string dateNow = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
        //        int month = DateTime.Now.Month;
        //        int year = DateTime.Now.Year;
        //        int sanLuongKeHoachThang = 0;
        //        int thucHienThang = 0;
        //        double doanhThuKeHoachThang = 0;
        //        double doanhThuThucHienThang = 0;
        //        int tongTHNgay = 0;
        //        indexFinish = -1;

        //        string strGetKHThangCuaChuyen = "Select c.LaoDongDinhBien, csp.STT, csp.SanLuongKeHoach, csp.NangXuatSanXuat, csp.LuyKeTH, sp.TenSanPham, sp.DonGia, sp.DonGiaCM, c.IdDenNangSuat from Chuyen_SanPham csp, SanPham sp, Chuyen c where csp.MaChuyen=" + maChuyen + " and  csp.IsDelete=0 and sp.MaSanPham=csp.MaSanPham and c.MaChuyen=csp.MaChuyen and csp.Thang=" + month + " and csp.Nam=" + year;
        //        dtThongTinThang.Clear();
        //        dtThongTinThang = dbclass.TruyVan_TraVe_DataTable(strGetKHThangCuaChuyen);
        //        if (dtThongTinThang != null && dtThongTinThang.Rows.Count > 0)
        //        {
        //            foreach (DataRow row in dtThongTinThang.Rows)
        //            {
        //                string STTChuyen_SanPham = row["STT"].ToString();
        //                int sanLuongKH = 0;
        //                int.TryParse(row["SanLuongKeHoach"].ToString(), out sanLuongKH);
        //                sanLuongKeHoachThang += sanLuongKH;
        //                float donGiaCM = 0;
        //                float.TryParse(row["DonGiaCM"].ToString(), out donGiaCM);
        //                float donGia = 0;
        //                float.TryParse(row["DonGia"].ToString(), out donGia);
        //                doanhThuKeHoachThang += (donGiaCM * sanLuongKH);
        //                string sqlNangSuatOfChuyenSanPham = "select ThucHienNgay, ThucHienNgayGiam from NangXuat where STTCHuyen_SanPham='" + STTChuyen_SanPham + "'";
        //                dtNangSuatChuyenSanPham.Clear();
        //                dtNangSuatChuyenSanPham = dbclass.TruyVan_TraVe_DataTable(sqlNangSuatOfChuyenSanPham);
        //                if (dtNangSuatChuyenSanPham != null && dtNangSuatChuyenSanPham.Rows.Count > 0)
        //                {
        //                    foreach (DataRow drow in dtNangSuatChuyenSanPham.Rows)
        //                    {
        //                        int thucHienNgay = 0;
        //                        int thucHienNgayGiam = 0;
        //                        int.TryParse(drow["ThucHienNgay"].ToString(), out thucHienNgay);
        //                        int.TryParse(drow["ThucHienNgayGiam"].ToString(), out thucHienNgayGiam);
        //                        thucHienNgay = thucHienNgay - thucHienNgayGiam;
        //                        thucHienThang += thucHienNgay;
        //                        doanhThuThucHienThang += (donGia * thucHienNgay);
        //                    }
        //                }
        //            }
        //        }
        //        string strGetSPCuaChuyen = "Select c.LaoDongDinhBien, tp.NangXuatLaoDong, tp.LaoDongChuyen, csp.STT, csp.SanLuongKeHoach, csp.NangXuatSanXuat, csp.LuyKeTH, sp.TenSanPham, sp.DonGia, sp.DonGiaCM, c.IdDenNangSuat, csp.IsFinish from Chuyen_SanPham csp, SanPham sp, ThanhPham tp, Chuyen c where csp.MaChuyen=" + maChuyen + " and  csp.IsDelete=0 and sp.MaSanPham=csp.MaSanPham and csp.STT = tp.STTChuyen_SanPham and tp.Ngay ='" + dateNow + "' and c.MaChuyen=csp.MaChuyen";

        //        #region Start Clear cac bien luu tru
        //        thucHienVaDinhMuc1 = "";
        //        thucHienVaDinhMuc2 = "";

        //        luyKeThucHien1 = "";
        //        luyKeThucHien2 = "";

        //        maHang1 = "";
        //        maHang2 = "";

        //        btpTrenChuyen1 = "";
        //        btpTrenChuyen2 = "";

        //        laoDong1 = "";
        //        laoDong2 = "";

        //        doanhThuThang1 = "";
        //        doanhThuThang2 = "";

        //        doanhThuNgay1 = "";
        //        doanhThuNgay2 = "";

        //        nhipChuyen1 = "";
        //        nhipChuyen2 = "";

        //        sanLuongKeHoach1 = "";
        //        sanLuongKeHoach2 = "";

        //        thuNhapBQNgay1 = "";
        //        thuNhapBQNgay2 = "";

        //        thuNhapBQThang1 = "";
        //        thuNhapBQThang2 = "";

        //        doanhThuNgayTrenDinhMuc1 = "";
        //        doanhThuNgayTrenDinhMuc2 = "";

        //        tiLeThucHien1 = "";
        //        tiLeThucHien2 = "";

        //        doanhThuKHThang1 = "";
        //        thucHienKHThang1 = "";

        //        doanhThuKHThang2 = "";
        //        thucHienKHThang2 = "";

        //        tongThucHienNgay1 = "";
        //        tongThucHienNgay2 = "";

        //        ThoatChuyen = "";
        //        ThoatChuyenNgay = 0;
        //        //End clear
        //        #endregion

        //        dtchuyenSanPham = dbclass.TruyVan_TraVe_DataTable(strGetSPCuaChuyen);
        //        if (dtchuyenSanPham != null && dtchuyenSanPham.Rows.Count > 0)
        //        {
        //            string idDen = dtchuyenSanPham.Rows[0]["IdDenNangSuat"].ToString();
        //            int tyLeDenThucTe = 0;
        //            string colorDen = "";
        //            //countMaHangInDate = dtchuyenSanPham.Rows.Count;
        //            countMaHangInDate = 1;

        //            //HoangHai
        //            double TongTPThoatChuyen = 0, THNgay_Temp = 0;
        //            if (dtchuyenSanPham.Rows.Count == 1)
        //            {
        //                #region
        //                if (!IsDesign)
        //                {
        //                    LoadTableLayoutPanelContent();
        //                    LoadContent(countMaHangInDate);
        //                    LoadLabel();
        //                    ColumnNumber = 1;
        //                    IsDesign = true;
        //                }
        //                string STTChuyen_ThanhPham = dtchuyenSanPham.Rows[0]["STT"].ToString();
        //                string strSQL = "select ROUND(nx.DinhMucNgay,0) DinhMucNgay, (nx.ThucHienNgay-nx.ThucHienNgayGiam) ThucHienNgay, case when nx.DinhMucNgay = 0 then 0 else (ROUND((((nx.ThucHienNgay-nx.ThucHienNgayGiam)/nx.DinhMucNgay)*100),0))end TyLeThucHien, ((SELECT SUM(BTPNgay) FROM BTP b WHERE b.STTChuyen_SanPham = '" + STTChuyen_ThanhPham + "'AND b.Ngay ='" + dateNow + "' and b.CommandTypeId=" + (int)eCommandRecive.BTPIncrease + " and b.IsEndOfLine=1)-(SELECT SUM(BTPNgay) FROM BTP b WHERE b.STTChuyen_SanPham = '" + STTChuyen_ThanhPham + "'AND b.Ngay ='" + dateNow + "'and b.CommandTypeId=" + (int)eCommandRecive.BTPReduce + " and b.IsEndOfLine=1)) BTPNgay, ((SELECT SUM(BTPNgay) FROM BTP b WHERE  b.STTChuyen_SanPham = '" + STTChuyen_ThanhPham + "' and b.CommandTypeId=" + (int)eCommandRecive.BTPIncrease + " and b.IsEndOfLine=1 )-(SELECT SUM(BTPNgay) FROM BTP b WHERE  b.STTChuyen_SanPham = '" + STTChuyen_ThanhPham + "' and b.CommandTypeId=" + (int)eCommandRecive.BTPReduce + " and b.IsEndOfLine=1 )-(SELECT SUM(nx.BTPLoi) FROM NangXuat nx WHERE nx.STTCHuyen_SanPham = '" + STTChuyen_ThanhPham + "')) LuyKeBTP, nx.BTPTrenChuyen, nx.NhipDoSanXuat, nx.NhipDoThucTe, (nx.BTPThoatChuyenNgay-nx.BTPThoatChuyenNgayGiam) BTPThoatChuyenNgay, csp.LuyKeBTPThoatChuyen FROM SanPham sp, ThanhPham tp, Chuyen_SanPham csp, NangXuat nx WHERE csp.STT ='" + STTChuyen_ThanhPham + "' AND nx.STTCHuyen_SanPham = csp.STT  and nx.Ngay ='" + dateNow + "' AND csp.MaSanPham = sp.MaSanPham AND tp.STTChuyen_SanPham = csp.STT AND tp.Ngay = nx.Ngay";
        //                dtLoadDataAll = dbclass.TruyVan_TraVe_DataTable(strSQL);
        //                if (dtLoadDataAll != null && dtLoadDataAll.Rows.Count > 0)
        //                {
        //                    var a = 0;
        //                    int.TryParse(dtLoadDataAll.Rows[0]["BTPThoatChuyenNgay"].ToString(), out a);
        //                    ThoatChuyenNgay += a > 0 ? a : 0;

        //                    float donGia = 0;
        //                    float.TryParse(dtchuyenSanPham.Rows[0]["DonGia"].ToString(), out donGia);

        //                    int laoDongChuyen = 0;
        //                    int.TryParse(dtchuyenSanPham.Rows[0]["LaoDongChuyen"].ToString(), out laoDongChuyen);

        //                    int btpTrenChuyen = 0;
        //                    int.TryParse(dtLoadDataAll.Rows[0]["BTPTrenChuyen"].ToString(), out btpTrenChuyen);

        //                    int sanLuongKeHoach = 0;
        //                    int.TryParse(dtchuyenSanPham.Rows[0]["SanLuongKeHoach"].ToString(), out sanLuongKeHoach);


        //                    int thucHienNgay = 0;
        //                    int.TryParse(dtLoadDataAll.Rows[0]["ThucHienNgay"].ToString(), out thucHienNgay);
        //                    if (thucHienNgay < 0) thucHienNgay = 0;
        //                    tongTHNgay += thucHienNgay;

        //                    float donGiaCM = 0;
        //                    float.TryParse(dtchuyenSanPham.Rows[0]["DonGiaCM"].ToString(), out donGiaCM);

        //                    int laoDongDinhBien = 0;
        //                    int.TryParse(dtchuyenSanPham.Rows[0]["LaoDongDinhBien"].ToString(), out laoDongDinhBien);

        //                    double luongBQNgay = 0;
        //                    double doanhThuNgay = 0;
        //                    if (laoDongChuyen > 0)
        //                    {
        //                        luongBQNgay = Math.Round(((donGia * thucHienNgay) / laoDongChuyen), 1);
        //                        doanhThuNgay = Math.Round((donGiaCM * thucHienNgay), 1);
        //                    }

        //                    int luyKeTH = 0;
        //                    int.TryParse(dtchuyenSanPham.Rows[0]["LuyKeTH"].ToString(), out luyKeTH);

        //                    int dinhMucNgay = 0;
        //                    int.TryParse(dtLoadDataAll.Rows[0]["DinhMucNgay"].ToString(), out dinhMucNgay);
        //                    DinhMucNgayCuaChuyen = dinhMucNgay;

        //                    double doanhThuNgay_ThucHienNgay = 0;
        //                    doanhThuNgay_ThucHienNgay = Math.Round((donGia * thucHienNgay), 1);

        //                    double doanhThuNgay_DinhMucNgay = 0;
        //                    doanhThuNgay_DinhMucNgay = Math.Round((donGia * dinhMucNgay), 1);

        //                    double tiLeThucHien = 0;
        //                    if (dinhMucNgay != 0)
        //                    {
        //                        tiLeThucHien = (thucHienNgay * 100) / dinhMucNgay;
        //                        tiLeThucHien = Math.Round(tiLeThucHien, 2);
        //                    }

        //                    double luongBQThang = 0;
        //                    double doanhThuThang = 0;
        //                    if (loaiDoanhThuThang == 1)
        //                    {
        //                        string sqlGetNXOfSTT = "select tp.LaoDongChuyen, (nx.ThucHienNgay-nxThucHienNgayGiam) ThucHienNgay  from ThanhPham tp, NangXuat nx where nx.STTChuyen_SanPham = tp.STTChuyen_SanPham and tp.STTChuyen_SanPham='" + STTChuyen_ThanhPham + "' and nx.Ngay = tp.Ngay";
        //                        dtLaoDongVaThucHienNgay = dbclass.TruyVan_TraVe_DataTable(sqlGetNXOfSTT);
        //                        if (dtLaoDongVaThucHienNgay != null && dtLaoDongVaThucHienNgay.Rows.Count > 0)
        //                        {
        //                            foreach (DataRow row in dtLaoDongVaThucHienNgay.Rows)
        //                            {
        //                                int ldchuyen = 0;
        //                                int.TryParse(row["LaoDongChuyen"].ToString(), out ldchuyen);
        //                                int thNgay = 0;
        //                                int.TryParse(row["ThucHienNgay"].ToString(), out thNgay);
        //                                double luong = 0;
        //                                double doanhThu = 0;
        //                                if (ldchuyen > 0)
        //                                {
        //                                    luong = (donGia * thNgay) / ldchuyen;
        //                                    doanhThu = (donGiaCM * thNgay);
        //                                }
        //                                luongBQThang += luong;
        //                                doanhThuThang += doanhThu;
        //                            }
        //                            luongBQThang = Math.Round(luongBQThang, 1);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        string sqlGetNXOfSTT = "select tp.LaoDongChuyen,(nx.BTPThoatChuyenNgay-nx.BTPThoatChuyenNgayGiam) BTPThoatChuyenNgay, (nx.ThucHienNgay-nx.ThucHienNgayGiam) ThucHienNgay  from ThanhPham tp, NangXuat nx where nx.STTChuyen_SanPham = tp.STTChuyen_SanPham and tp.STTChuyen_SanPham='" + STTChuyen_ThanhPham + "' and nx.Ngay = tp.Ngay and datediff(mm,'" + DateTime.Now + "',nx.Ngay) = 0  ";
        //                        dtLaoDongVaThucHienNgay = dbclass.TruyVan_TraVe_DataTable(sqlGetNXOfSTT);
        //                        if (dtLaoDongVaThucHienNgay != null && dtLaoDongVaThucHienNgay.Rows.Count > 0)
        //                        {
        //                            foreach (DataRow row in dtLaoDongVaThucHienNgay.Rows)
        //                            {
        //                                THNgay_Temp = 0;
        //                                double.TryParse(row["BTPThoatChuyenNgay"].ToString(), out THNgay_Temp);
        //                                TongTPThoatChuyen += (THNgay_Temp > 0 ? THNgay_Temp : 0);

        //                                int ldchuyen = 0;
        //                                int.TryParse(row["LaoDongChuyen"].ToString(), out ldchuyen);
        //                                int thNgay = 0;
        //                                int.TryParse(row["ThucHienNgay"].ToString(), out thNgay);
        //                                double luong = 0;
        //                                double doanhThu = 0;
        //                                if (ldchuyen > 0)
        //                                {
        //                                    luong = (donGia * thNgay) / ldchuyen;
        //                                    doanhThu = (donGiaCM * thNgay);
        //                                }
        //                                luongBQThang += luong;
        //                                doanhThuThang += doanhThu;
        //                            }
        //                            luongBQThang = Math.Round(luongBQThang, 1);
        //                        }
        //                    }

        //                    thucHienVaDinhMuc1 = dtLoadDataAll.Rows[0]["ThucHienNgay"].ToString() + " / " + dinhMucNgay.ToString();
        //                    btpTrenChuyen1 = Math.Round((double)(laoDongChuyen == 0 ? 0 : (btpTrenChuyen / laoDongChuyen)), 2).ToString() + " / " + btpTrenChuyen.ToString();
        //                    laoDong1 = laoDongChuyen.ToString() + " / " + laoDongDinhBien.ToString();
        //                    nhipChuyen1 = dtLoadDataAll.Rows[0]["NhipDoThucTe"].ToString() + " / " + dtLoadDataAll.Rows[0]["NhipDoSanXuat"].ToString();
        //                    maHang1 = dtchuyenSanPham.Rows[0]["TenSanPham"].ToString().ToUpper();
        //                    sanLuongKeHoach1 = sanLuongKeHoach.ToString();
        //                    luyKeThucHien1 = luyKeTH.ToString() + " / " + (sanLuongKeHoach - luyKeTH).ToString();
        //                    doanhThuThang1 = (doanhThuThang == 0 ? "0" : String.Format("{0:#,###}", doanhThuThang));
        //                    thuNhapBQNgay1 = (luongBQNgay == 0 ? "0" : String.Format("{0:#,###}", luongBQNgay));
        //                    thuNhapBQThang1 = (luongBQThang == 0 ? "0" : String.Format("{0:#,###}", luongBQThang));
        //                    tiLeThucHien1 = tiLeThucHien.ToString();
        //                    doanhThuNgayTrenDinhMuc1 = (doanhThuNgay_ThucHienNgay == 0 ? "0" : String.Format("{0:#,###}", doanhThuNgay_ThucHienNgay)) + " / " + (doanhThuNgay_DinhMucNgay == 0 ? "0" : String.Format("{0:#,###}", doanhThuNgay_DinhMucNgay));

        //                    if (loaiDoanhThuThang == 1)
        //                        doanhThuThang1 = (doanhThuThang == 0 ? "0" : String.Format("{0:#,###}", doanhThuThang));
        //                    else
        //                        doanhThuThang1 = (doanhThuNgay == 0 ? "0" : String.Format("{0:#,###}", doanhThuNgay)) + " / " + (doanhThuThang == 0 ? "0" : String.Format("{0:#,###}", doanhThuThang));

        //                    thuNhapBQThang1 = (luongBQNgay == 0 ? "0" : String.Format("{0:#,###}", luongBQNgay)) + "/" + (luongBQThang == 0 ? "0" : String.Format("{0:#,###}", luongBQThang));
        //                    //doanhThuNgay1 = (doanhThuNgay == 0 ? "0" : String.Format("{0:#,###}", doanhThuNgay));
        //                    //lblLuongBQ.Text = (luongBQNgay == 0 ? "0" : String.Format("{0:#,###}", luongBQNgay)) + "/" + (luongBQThang == 0 ? "0" : String.Format("{0:#,###}", luongBQThang));
        //                    //lblDoanhThuBQ.Text = (doanhThuNgay == 0 ? "0" : String.Format("{0:#,###}", doanhThuNgay)) + "/" + (doanhThuThang == 0 ? "0" : String.Format("{0:#,###}", doanhThuThang));
        //                    float nhipDoSanXuat = 0;
        //                    float.TryParse(dtLoadDataAll.Rows[0]["NhipDoSanXuat"].ToString(), out nhipDoSanXuat);
        //                    float nhipDoThucTe = 0;
        //                    float.TryParse(dtLoadDataAll.Rows[0]["NhipDoThucTe"].ToString(), out nhipDoThucTe);
        //                    tyLeDenThucTe = nhipDoThucTe == 0 ? 0 : (int)(nhipDoThucTe == 0 ? 0 : ((nhipDoSanXuat * 100) / nhipDoThucTe));
        //                    colorDen = denDAO.GetColorDen(idDen, tableType, tyLeDenThucTe);
        //                    SetColorForDen(colorDen);
        //                    //this.panelDen.BackColor = HelperControl.GetColor(colorDen);
        //                    tongThucHienNgay1 = tongTHNgay.ToString() + " / " + DinhMucNgayCuaChuyen.ToString();
        //                    doanhThuKHThang1 = (doanhThuThucHienThang == 0 ? "0" : String.Format("{0:#,###}", doanhThuThucHienThang)) + " / " + (doanhThuKeHoachThang == 0 ? "0" : String.Format("{0:#,###}", doanhThuKeHoachThang));
        //                    thucHienKHThang1 = (thucHienThang == 0 ? "0" : String.Format("{0:#,###}", thucHienThang)) + " / " + (sanLuongKeHoachThang == 0 ? "0" : String.Format("{0:#,###}", sanLuongKeHoachThang));

        //                    ThoatChuyen = ThoatChuyenNgay + "/" + TongTPThoatChuyen;
        //                }
        //                #endregion
        //            }
        //            else
        //            {
        //                #region
        //                if (ColumnNumber == 1)
        //                {
        //                    IsDesign = false;
        //                }
        //                if (!IsDesign)
        //                {
        //                    LoadTableLayoutPanelContent();
        //                    LoadContent(countMaHangInDate);
        //                    LoadLabel();
        //                    IsDesign = true;
        //                    ColumnNumber = 1;
        //                }


        //                int thucHienNgay1 = 0;
        //                int thucHienNgay2 = 0;
        //                int tongDinhMucNgay = 0;
        //                double tongDoanhThuNgay = 0;
        //                double tongDoangThuThang = 0;
        //                bool isReadTyLeDen = true;
        //                int isFinish = 0;


        //                for (int i = 0; i < dtchuyenSanPham.Rows.Count; i++)
        //                {

        //                    int.TryParse(dtchuyenSanPham.Rows[i]["IsFinish"].ToString(), out isFinish);
        //                    if (isFinish == 1)
        //                        indexFinish = i;
        //                    string STTChuyen_ThanhPham = dtchuyenSanPham.Rows[i]["STT"].ToString();
        //                    string strSQL = "select ROUND(nx.DinhMucNgay,0) DinhMucNgay, (nx.ThucHienNgay-nx.ThucHienNgayGiam) ThucHienNgay, case when nx.DinhMucNgay = 0 then 0 else (ROUND((((nx.ThucHienNgay-nx.ThucHienNgayGiam)/nx.DinhMucNgay)*100),0))end TyLeThucHien, ((SELECT SUM(BTPNgay) FROM BTP b WHERE b.STTChuyen_SanPham = '" + STTChuyen_ThanhPham + "'AND b.Ngay ='" + dateNow + "' and b.CommandTypeId=" + (int)eCommandRecive.BTPIncrease + " and b.IsEndOfLine=1)-(SELECT SUM(BTPNgay) FROM BTP b WHERE b.STTChuyen_SanPham = '" + STTChuyen_ThanhPham + "'AND b.Ngay ='" + dateNow + "'and b.CommandTypeId=" + (int)eCommandRecive.BTPReduce + " and b.IsEndOfLine=1)) BTPNgay, ((SELECT SUM(BTPNgay) FROM BTP b WHERE  b.STTChuyen_SanPham = '" + STTChuyen_ThanhPham + "' and b.CommandTypeId=" + (int)eCommandRecive.BTPIncrease + " and b.IsEndOfLine=1 )-(SELECT SUM(BTPNgay) FROM BTP b WHERE  b.STTChuyen_SanPham = '" + STTChuyen_ThanhPham + "' and b.CommandTypeId=" + (int)eCommandRecive.BTPReduce + " and b.IsEndOfLine=1 )-(SELECT SUM(nx.BTPLoi) FROM NangXuat nx WHERE nx.STTCHuyen_SanPham = '" + STTChuyen_ThanhPham + "')) LuyKeBTP, nx.BTPTrenChuyen, nx.NhipDoSanXuat, nx.NhipDoThucTe, (nx.BTPThoatChuyenNgay-nx.BTPThoatChuyenNgayGiam) BTPThoatChuyenNgay, csp.LuyKeBTPThoatChuyen FROM SanPham sp, ThanhPham tp, Chuyen_SanPham csp, NangXuat nx WHERE csp.STT ='" + STTChuyen_ThanhPham + "' AND nx.STTCHuyen_SanPham = csp.STT  and nx.Ngay ='" + dateNow + "' AND csp.MaSanPham = sp.MaSanPham AND tp.STTChuyen_SanPham = csp.STT AND tp.Ngay = nx.Ngay";
        //                    dtLoadDataAll = dbclass.TruyVan_TraVe_DataTable(strSQL);
        //                    if (dtLoadDataAll != null && dtLoadDataAll.Rows.Count > 0)
        //                    {

        //                        var a = 0;
        //                        int.TryParse(dtLoadDataAll.Rows[0]["BTPThoatChuyenNgay"].ToString(), out a);
        //                        ThoatChuyenNgay += a > 0 ? a : 0;

        //                        float donGia = 0;
        //                        float.TryParse(dtchuyenSanPham.Rows[i]["DonGia"].ToString(), out donGia);

        //                        int laoDongChuyen = 0;
        //                        int.TryParse(dtchuyenSanPham.Rows[i]["LaoDongChuyen"].ToString(), out laoDongChuyen);

        //                        float btpTrenChuyen = 0;
        //                        float.TryParse(dtLoadDataAll.Rows[0]["BTPTrenChuyen"].ToString(), out btpTrenChuyen);

        //                        int sanLuongKeHoach = 0;
        //                        int.TryParse(dtchuyenSanPham.Rows[i]["SanLuongKeHoach"].ToString(), out sanLuongKeHoach);

        //                        int thucHienNgay = 0;
        //                        int.TryParse(dtLoadDataAll.Rows[0]["ThucHienNgay"].ToString(), out thucHienNgay);
        //                        if (thucHienNgay < 0) thucHienNgay = 0;
        //                        tongTHNgay += thucHienNgay;

        //                        float donGiaCM = 0;
        //                        float.TryParse(dtchuyenSanPham.Rows[i]["DonGiaCM"].ToString(), out donGiaCM);

        //                        int laoDongDinhBien = 0;
        //                        int.TryParse(dtchuyenSanPham.Rows[i]["LaoDongDinhBien"].ToString(), out laoDongDinhBien);

        //                        double luongBQNgay = 0;
        //                        double doanhThuNgay = 0;
        //                        if (laoDongChuyen > 0)
        //                        {
        //                            luongBQNgay = Math.Round(laoDongChuyen == 0 ? 0 : ((donGia * thucHienNgay) / laoDongChuyen), 1);
        //                            doanhThuNgay = (donGiaCM * thucHienNgay);
        //                            tongDoanhThuNgay += doanhThuNgay;
        //                        }

        //                        int luyKeTH = 0;
        //                        int.TryParse(dtchuyenSanPham.Rows[i]["LuyKeTH"].ToString(), out luyKeTH);

        //                        int dinhMucNgay = 0;
        //                        int.TryParse(dtLoadDataAll.Rows[0]["DinhMucNgay"].ToString(), out dinhMucNgay);
        //                        tongDinhMucNgay += dinhMucNgay;

        //                        double doanhThuNgay_ThucHienNgay = 0;
        //                        doanhThuNgay_ThucHienNgay = Math.Round((donGia * thucHienNgay), 1);

        //                        double doanhThuNgay_DinhMucNgay = 0;
        //                        doanhThuNgay_DinhMucNgay = Math.Round((donGia * dinhMucNgay), 1);

        //                        double luongBQThang = 0;
        //                        double doanhThuThang = 0;

        //                        double tiLeThucHien = 0;
        //                        if (dinhMucNgay != 0)
        //                        {
        //                            tiLeThucHien = (thucHienNgay * 100) / dinhMucNgay;
        //                            tiLeThucHien = Math.Round(tiLeThucHien, 2);
        //                        }

        //                        if (loaiDoanhThuThang == 1)
        //                        {
        //                            #region
        //                            string sqlGetNXOfSTT = "select tp.LaoDongChuyen, (nx.ThucHienNgay-nx.ThucHienNgayGiam) ThucHienNgay,(nx.BTPThoatChuyenNgay-nx.BTPThoatChuyenNgayGiam) TPThoatChuyenNgay  from ThanhPham tp, NangXuat nx where nx.STTChuyen_SanPham = tp.STTChuyen_SanPham and tp.STTChuyen_SanPham='" + STTChuyen_ThanhPham + "' and nx.Ngay = tp.Ngay";
        //                            dtLaoDongVaThucHienNgay = dbclass.TruyVan_TraVe_DataTable(sqlGetNXOfSTT);
        //                            if (dtLaoDongVaThucHienNgay != null && dtLaoDongVaThucHienNgay.Rows.Count > 0)
        //                            {
        //                                foreach (DataRow row in dtLaoDongVaThucHienNgay.Rows)
        //                                {
        //                                    THNgay_Temp = 0;
        //                                    double.TryParse(row["TPThoatChuyenNgay"].ToString(), out THNgay_Temp);
        //                                    TongTPThoatChuyen += (THNgay_Temp > 0 ? THNgay_Temp : 0);

        //                                    int ldchuyen = 0;
        //                                    int.TryParse(row["LaoDongChuyen"].ToString(), out ldchuyen);
        //                                    int thNgay = 0;
        //                                    int.TryParse(row["ThucHienNgay"].ToString(), out thNgay);
        //                                    double luong = 0;
        //                                    double doanhThu = 0;
        //                                    if (ldchuyen > 0)
        //                                    {
        //                                        luong = (donGia * thNgay) / ldchuyen;
        //                                        doanhThu = (donGiaCM * thNgay);
        //                                    }
        //                                    luongBQThang += luong;
        //                                    doanhThuThang += doanhThu;
        //                                }
        //                                luongBQThang = Math.Round(luongBQThang, 1);
        //                                tongDoangThuThang += doanhThuThang;
        //                            }
        //                            #endregion
        //                        }
        //                        else
        //                        {
        //                            #region
        //                            string sqlGetNXOfSTT = "select tp.LaoDongChuyen, (nx.ThucHienNgay-nx.ThucHienNgayGiam) ThucHienNgay, (nx.BTPThoatChuyenNgay-nx.BTPThoatChuyenNgayGiam) TPThoatChuyenNgay  from ThanhPham tp, NangXuat nx where nx.STTChuyen_SanPham = tp.STTChuyen_SanPham and tp.STTChuyen_SanPham='" + STTChuyen_ThanhPham + "' and nx.Ngay = tp.Ngay and datediff(mm,'" + DateTime.Now + "',nx.Ngay) = 0  ";
        //                            dtLaoDongVaThucHienNgay = dbclass.TruyVan_TraVe_DataTable(sqlGetNXOfSTT);
        //                            if (dtLaoDongVaThucHienNgay != null && dtLaoDongVaThucHienNgay.Rows.Count > 0)
        //                            {
        //                                foreach (DataRow row in dtLaoDongVaThucHienNgay.Rows)
        //                                {
        //                                    THNgay_Temp = 0;
        //                                    double.TryParse(row["TPThoatChuyenNgay"].ToString(), out THNgay_Temp);
        //                                    TongTPThoatChuyen += (THNgay_Temp > 0 ? THNgay_Temp : 0);

        //                                    int ldchuyen = 0;
        //                                    int.TryParse(row["LaoDongChuyen"].ToString(), out ldchuyen);
        //                                    int thNgay = 0;
        //                                    int.TryParse(row["ThucHienNgay"].ToString(), out thNgay);
        //                                    double luong = 0;
        //                                    double doanhThu = 0;
        //                                    if (ldchuyen > 0)
        //                                    {
        //                                        luong = (donGia * thNgay) / ldchuyen;
        //                                        doanhThu = (donGiaCM * thNgay);
        //                                    }
        //                                    luongBQThang += luong;
        //                                    doanhThuThang += doanhThu;
        //                                }
        //                                luongBQThang = Math.Round(luongBQThang, 1);
        //                                tongDoangThuThang += doanhThuThang;
        //                            }
        //                            #endregion
        //                        }

        //                        if (i == 0)
        //                        {
        //                            thucHienVaDinhMuc1 = dtLoadDataAll.Rows[0]["ThucHienNgay"].ToString() + " / " + dinhMucNgay.ToString();
        //                            btpTrenChuyen1 = Math.Round((double)(laoDongChuyen == 0 ? 0 : (btpTrenChuyen / laoDongChuyen)), 2).ToString() + " / " + dtLoadDataAll.Rows[0]["BTPTrenChuyen"].ToString();
        //                            laoDong1 = laoDongChuyen.ToString() + " / " + laoDongDinhBien.ToString();
        //                            nhipChuyen1 = dtLoadDataAll.Rows[0]["NhipDoThucTe"].ToString() + " / " + dtLoadDataAll.Rows[0]["NhipDoSanXuat"].ToString();
        //                            maHang1 = dtchuyenSanPham.Rows[i]["TenSanPham"].ToString().ToUpper();
        //                            sanLuongKeHoach1 = sanLuongKeHoach.ToString();
        //                            luyKeThucHien1 = luyKeTH.ToString() + " / " + (sanLuongKeHoach - luyKeTH).ToString();
        //                            if (loaiDoanhThuThang == 1)
        //                            {
        //                                doanhThuThang1 = (doanhThuThang == 0 ? "0" : String.Format("{0:#,###}", doanhThuThang));
        //                            }
        //                            else
        //                            {
        //                                doanhThuThang1 = (doanhThuNgay == 0 ? "0" : String.Format("{0:#,###}", doanhThuNgay)) + " / " + (doanhThuThang == 0 ? "0" : String.Format("{0:#,###}", doanhThuThang));
        //                            }


        //                            thucHienNgay1 = thucHienNgay;
        //                            doanhThuNgay1 = (doanhThuNgay == 0 ? "0" : String.Format("{0:#,###}", doanhThuNgay));
        //                            thuNhapBQThang1 = (luongBQNgay == 0 ? "0" : String.Format("{0:#,###}", luongBQNgay)) + "/" + (luongBQThang == 0 ? "0" : String.Format("{0:#,###}", luongBQThang));

        //                            doanhThuNgayTrenDinhMuc1 = (doanhThuNgay_ThucHienNgay == 0 ? "0" : String.Format("{0:#,###}", doanhThuNgay_ThucHienNgay)) + " / " + (doanhThuNgay_DinhMucNgay == 0 ? "0" : String.Format("{0:#,###}", doanhThuNgay_DinhMucNgay));
        //                            tiLeThucHien1 = tiLeThucHien.ToString();
        //                        }
        //                        else
        //                        {
        //                            thucHienVaDinhMuc2 = dtLoadDataAll.Rows[0]["ThucHienNgay"].ToString() + " / " + dinhMucNgay.ToString();
        //                            btpTrenChuyen2 = Math.Round((double)(laoDongChuyen == 0 ? 0 : (btpTrenChuyen / laoDongChuyen)), 2).ToString() + " / " + dtLoadDataAll.Rows[0]["BTPTrenChuyen"].ToString();
        //                            laoDong2 = laoDongChuyen.ToString() + " / " + laoDongDinhBien.ToString();
        //                            nhipChuyen2 = dtLoadDataAll.Rows[0]["NhipDoThucTe"].ToString() + " / " + dtLoadDataAll.Rows[0]["NhipDoSanXuat"].ToString();
        //                            maHang2 = dtchuyenSanPham.Rows[i]["TenSanPham"].ToString().ToUpper();
        //                            sanLuongKeHoach2 = sanLuongKeHoach.ToString();
        //                            luyKeThucHien2 = luyKeTH.ToString() + " / " + (sanLuongKeHoach - luyKeTH).ToString();
        //                            if (loaiDoanhThuThang == 1)
        //                            {
        //                                doanhThuThang2 = (doanhThuThang == 0 ? "0" : String.Format("{0:#,###}", doanhThuThang));
        //                            }
        //                            else
        //                            {
        //                                doanhThuThang2 = (doanhThuNgay == 0 ? "0" : String.Format("{0:#,###}", doanhThuNgay)) + " / " + (doanhThuThang == 0 ? "0" : String.Format("{0:#,###}", doanhThuThang));
        //                            }


        //                            thucHienNgay2 = thucHienNgay;
        //                            thuNhapBQThang2 = (luongBQNgay == 0 ? "0" : String.Format("{0:#,###}", luongBQNgay)) + "/" + (luongBQThang == 0 ? "0" : String.Format("{0:#,###}", luongBQThang));

        //                            doanhThuNgayTrenDinhMuc2 = (doanhThuNgay_ThucHienNgay == 0 ? "0" : String.Format("{0:#,###}", doanhThuNgay_ThucHienNgay)) + " / " + (doanhThuNgay_DinhMucNgay == 0 ? "0" : String.Format("{0:#,###}", doanhThuNgay_DinhMucNgay));
        //                            tiLeThucHien2 = tiLeThucHien.ToString();
        //                        }
        //                    }
        //                    if (isFinish == 0 && isReadTyLeDen)
        //                    {
        //                        isReadTyLeDen = false;
        //                        int nhipDoSanXuat = 0;
        //                        int.TryParse(dtLoadDataAll.Rows[0]["NhipDoSanXuat"].ToString(), out nhipDoSanXuat);
        //                        int nhipDoThucTe = 0;
        //                        int.TryParse(dtLoadDataAll.Rows[0]["NhipDoThucTe"].ToString(), out nhipDoThucTe);
        //                        tyLeDenThucTe = nhipDoThucTe == 0 ? 0 : (nhipDoSanXuat * 100) / nhipDoThucTe;
        //                        colorDen = denDAO.GetColorDen(idDen, tableType, tyLeDenThucTe);
        //                        SetColorForDen(colorDen);
        //                    }
        //                }
        //                tongDoanhThuNgay = Math.Round(tongDoanhThuNgay, 1);
        //                tongDoangThuThang = Math.Round(tongDoangThuThang, 1);
        //                //lblThucHienNgay.Text = thucHienNgay1.ToString() + "/" + (thucHienNgay1 + thucHienNgay2).ToString();
        //                //lblThucHienNgay2.Text = thucHienNgay2.ToString() + "/" + (thucHienNgay1 + thucHienNgay2).ToString();
        //                //lblDoanhThuBQ.Text = (tongDoangThuThang == 0 ? "0" : String.Format("{0:#,###}", tongDoangThuThang));
        //                //lblDoanhThuBQ2.Text = (tongDoangThuThang == 0 ? "0" : String.Format("{0:#,###}", tongDoangThuThang));
        //                DinhMucNgayCuaChuyen = tongDinhMucNgay;
        //                //if (loaiDoanhThuThang == 2)
        //                //{
        //                //    doanhThuThang1 = (tongDoangThuThang == 0 ? "0" : String.Format("{0:#,###}", tongDoangThuThang));
        //                //    doanhThuThang2 = (tongDoangThuThang == 0 ? "0" : String.Format("{0:#,###}", tongDoangThuThang));
        //                //}                           
        //                //this.panelDen.BackColor = HelperControl.GetColor(colorDen);
        //                doanhThuKHThang2 = doanhThuKHThang1 = (doanhThuThucHienThang == 0 ? "0" : String.Format("{0:#,###}", doanhThuThucHienThang)) + " / " + (doanhThuKeHoachThang == 0 ? "0" : String.Format("{0:#,###}", doanhThuKeHoachThang));
        //                thucHienKHThang2 = thucHienKHThang1 = (thucHienThang == 0 ? "0" : String.Format("{0:#,###}", thucHienThang)) + " / " + (sanLuongKeHoachThang == 0 ? "0" : String.Format("{0:#,###}", sanLuongKeHoachThang));
        //                tongThucHienNgay1 = tongThucHienNgay2 = tongTHNgay.ToString() + " / " + DinhMucNgayCuaChuyen.ToString();

        //                ThoatChuyen = ThoatChuyenNgay + "/" + TongTPThoatChuyen;
        //                #endregion
        //            }
        //        }

        //        SetValueForLabelOfTBLPanelContent(0, manHinhShow, "thucHienVaDinhMuc", "");
        //        SetValueForLabelOfTBLPanelContent(0, manHinhShow, "btpTrenChuyen", "");
        //        SetValueForLabelOfTBLPanelContent(0, manHinhShow, "laoDong", "");
        //        SetValueForLabelOfTBLPanelContent(0, manHinhShow, "nhipChuyen", "");

        //        SetValueForLabelOfTBLPanelContent(0, manHinhShow, "maHang", "");
        //        SetValueForLabelOfTBLPanelContent(0, manHinhShow, "sanLuongKeHoach", "");
        //        SetValueForLabelOfTBLPanelContent(0, manHinhShow, "luyKeThucHien", "");
        //        SetValueForLabelOfTBLPanelContent(0, manHinhShow, "doanhThuThang", "");
        //        SetValueForLabelOfTBLPanelContent(0, manHinhShow, "thuNhapBQThang", "");
        //        SetValueForLabelOfTBLPanelContent(0, manHinhShow, "doanhThuNgayTrenDinhMuc", "");
        //        SetValueForLabelOfTBLPanelContent(0, manHinhShow, "tiLeThucHien", "");
        //        SetValueForLabelOfTBLPanelContent(0, manHinhShow, "doanhThuKHThang", "");
        //        SetValueForLabelOfTBLPanelContent(0, manHinhShow, "thucHienKHThang", "");
        //        SetValueForLabelOfTBLPanelContent(0, manHinhShow, "tongThucHienNgay", "");

        //        SetValueForLabelOfTBLPanelContent(0, manHinhShow, "ThoatChuyen", "");
        //        indexFinish = indexFinish + 1;

        //        if (indexFinish == 0)
        //        {
        //            SetValueForLabelOfTBLPanelContent(1, manHinhShow, "thucHienVaDinhMuc", thucHienVaDinhMuc1);
        //            SetValueForLabelOfTBLPanelContent(1, manHinhShow, "btpTrenChuyen", btpTrenChuyen1);
        //            SetValueForLabelOfTBLPanelContent(1, manHinhShow, "laoDong", laoDong1);
        //            SetValueForLabelOfTBLPanelContent(1, manHinhShow, "nhipChuyen", nhipChuyen1);

        //            SetValueForLabelOfTBLPanelContent(1, manHinhShow, "maHang", maHang1);
        //            SetValueForLabelOfTBLPanelContent(1, manHinhShow, "sanLuongKeHoach", sanLuongKeHoach1);
        //            SetValueForLabelOfTBLPanelContent(1, manHinhShow, "luyKeThucHien", luyKeThucHien1);
        //            SetValueForLabelOfTBLPanelContent(1, manHinhShow, "doanhThuThang", doanhThuThang1);
        //            SetValueForLabelOfTBLPanelContent(1, manHinhShow, "thuNhapBQThang", thuNhapBQThang1);
        //            SetValueForLabelOfTBLPanelContent(1, manHinhShow, "doanhThuNgayTrenDinhMuc", doanhThuNgayTrenDinhMuc1);
        //            SetValueForLabelOfTBLPanelContent(1, manHinhShow, "tiLeThucHien", tiLeThucHien1);
        //            SetValueForLabelOfTBLPanelContent(1, manHinhShow, "doanhThuKHThang", doanhThuKHThang1);
        //            SetValueForLabelOfTBLPanelContent(1, manHinhShow, "thucHienKHThang", thucHienKHThang1);
        //            SetValueForLabelOfTBLPanelContent(1, manHinhShow, "tongThucHienNgay", tongThucHienNgay1);

        //            SetValueForLabelOfTBLPanelContent(1, manHinhShow, "ThoatChuyen", ThoatChuyen);
        //        }
        //        else
        //        {
        //            SetValueForLabelOfTBLPanelContent(1, manHinhShow, "thucHienVaDinhMuc", thucHienVaDinhMuc2);
        //            SetValueForLabelOfTBLPanelContent(1, manHinhShow, "btpTrenChuyen", btpTrenChuyen2);
        //            SetValueForLabelOfTBLPanelContent(1, manHinhShow, "laoDong", laoDong2);
        //            SetValueForLabelOfTBLPanelContent(1, manHinhShow, "nhipChuyen", nhipChuyen2);

        //            SetValueForLabelOfTBLPanelContent(1, manHinhShow, "maHang", maHang2);
        //            SetValueForLabelOfTBLPanelContent(1, manHinhShow, "sanLuongKeHoach", sanLuongKeHoach2);
        //            SetValueForLabelOfTBLPanelContent(1, manHinhShow, "luyKeThucHien", luyKeThucHien2);
        //            SetValueForLabelOfTBLPanelContent(1, manHinhShow, "doanhThuThang", doanhThuThang2);
        //            SetValueForLabelOfTBLPanelContent(1, manHinhShow, "thuNhapBQThang", thuNhapBQThang2);
        //            SetValueForLabelOfTBLPanelContent(1, manHinhShow, "doanhThuNgayTrenDinhMuc", doanhThuNgayTrenDinhMuc2);
        //            SetValueForLabelOfTBLPanelContent(1, manHinhShow, "tiLeThucHien", tiLeThucHien2);
        //            SetValueForLabelOfTBLPanelContent(1, manHinhShow, "doanhThuKHThang", doanhThuKHThang2);
        //            SetValueForLabelOfTBLPanelContent(1, manHinhShow, "thucHienKHThang", thucHienKHThang2);
        //            SetValueForLabelOfTBLPanelContent(1, manHinhShow, "tongThucHienNgay", tongThucHienNgay2);

        //            SetValueForLabelOfTBLPanelContent(1, manHinhShow, "ThoatChuyen", ThoatChuyen);
        //        }
        //    }
        //    catch (Exception)
        //    { }
        //}

        private void SetColorForDen(string colorDen)
        {
            if (!colorDen.Equals(currentColorDen))
            {
                switch (colorDen)
                {
                    case "ĐỎ": currentColorDen = colorDen; colorDen = "Red"; break;
                    case "VÀNG": currentColorDen = colorDen; colorDen = "Yellow"; break;
                    case "XANH": currentColorDen = colorDen; colorDen = "Green"; break;
                }

                if (colorDen.Equals("Red"))
                    this.panelDen.BackgroundImage = global::QuanLyNangSuat.Properties.Resources.circle_red;
                else if (colorDen.Equals("Yellow"))
                    this.panelDen.BackgroundImage = global::QuanLyNangSuat.Properties.Resources.circle_yellow;
                else if (colorDen.Equals("Green"))
                    this.panelDen.BackgroundImage = global::QuanLyNangSuat.Properties.Resources.circle_green;

            }
        }

        private void FrmHienThiLCD_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                DuAn03_HaiDang.Helper.HelperControl.ClearFormActiveLCD(this.Name);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}
