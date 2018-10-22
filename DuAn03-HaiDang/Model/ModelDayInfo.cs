using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuanLyNangSuat.Model
{
    public class ModelDayInfo
    {
        public int STTChuyenSanPham { get; set; }
        public int ThucHienNgay { get; set; }
        public int DinhMucNgay { get; set; }
        public int SoLaoDong { get; set; }
        public float NangSuatLaoDong { get; set; }
        public float ThoiGianCheTao { get; set; }
        public bool IsStopOnDay { get; set; }
    }
}
