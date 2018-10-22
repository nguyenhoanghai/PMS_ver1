using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Models
{
    public class ModelProductivity
    {
        public string thucHienVaDinhMuc { get; set; }
        public string btpTrenChuyen { get; set; }
        public string laoDong { get; set; }
        public string nhipChuyen { get; set; }
        public string maHang { get; set; }
        public string sanLuongKeHoach { get; set; }
        public string luyKeThucHien { get; set; }
        public string doanhThuThang { get; set; }
        public string thuNhapBQThang { get; set; }
        public string doanhThuNgayTrenDinhMuc { get; set; }
        public string chuyen { get; set; }
        public string btpGiaoChuyenNgay { get; set; }
        public string luyKeBTP { get; set; }
        public string btpBinhQuanBTPTrenNgay { get; set; }
        public string tinhTrangBTP { get; set; }
        public string tiLeThucHien { get; set; }
        public string doanhThuKHThang { get; set; }
        public string thucHienKHThang { get; set; }
        public string tongThucHienNgay { get; set; }
        public string tenLoi { get; set; }
        public string nangSuatGioTruoc { get; set; }
        public string nangSuatGioHienTai { get; set; }
        public string tongNangSuat { get; set; }
        public string mauDen { get; set; }
        public List<WorkingTimeModel> listWorkHours { get; set; }
        public int ThucHienNgay { get; set; }
        public int ThoatChuyenNgay { get; set; }
        public int ErrorNgay { get; set; }
        public int DinhMucNgay { get; set; }
        public int LaborBase { get; set; }

        //
        public string ThoatChuyen { get; set; }
        public string KieuHienThiNangSuatGio { get; set; }
        public string DoanhThuBQ { get; set; }
        public string LuyKeKCS { get; set; }
        public string LKTH_SLKH { get; set; }
        public int TCInHour { get; set; }
        public int KCSInHour { get; set; }
    } 
}
