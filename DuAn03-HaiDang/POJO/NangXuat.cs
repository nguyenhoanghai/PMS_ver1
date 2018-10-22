using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuAn03_HaiDang.POJO
{
    class NangXuat
    {
        public string Ngay { get; set; }
        public string STTChuyen_SanPham { get; set; }
        public float DinhMucNgay { get; set; }
        public int ThucHienNgay { get; set; }
        public int ThucHienNgayGiam { get; set; }
        public int BTPThoatChuyenNgay { get; set; }
        public int BTPThoatChuyenNgayGiam { get; set; }
        public int BTPNgay { get; set; }
        public int BTPNgayGiam { get; set; }
        public int BTPTrenChuyen { get; set; }
        public float NhipDoThucTe { get; set; }
        public float NhipDoThucTeBTPThoatChuyen { get; set; }
        public float NhipDoSanXuat { get; set; }
        public int IsChange { get; set; }
        public int IsChangeBTP { get; set; }
        public int IsBTP { get; set; }
        public TimeSpan TimeLastChange { get; set; }
        public int BTPLoi { get; set; }
        public bool IsEndDate { get; set; }
        public int SanLuongLoi { get; set; }
        public int SanLuongLoiGiam { get; set; }
        public bool IsStopOnDay { get; set; }
        public Nullable<TimeSpan> TimeStopOnDay { get; set; }
    }
}
