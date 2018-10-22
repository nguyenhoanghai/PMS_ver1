using DuAn03_HaiDang.POJO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuAn03_HaiDang.Model
{
    public class ChuyenSanPham: Chuyen_SanPham
    {
        public string TenSanPham { get; set; }
        public string MaSanPham { get; set; }
        public string IdDen { get; set; }
        public string TenChuyen { get; set; }
        public string IsFinishStr { get; set; }
    }
}
