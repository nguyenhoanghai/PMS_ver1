using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuAn03_HaiDang.POJO
{
    class SanPhamCuaChuyen
    {
        public string STT { get; set; }
        public string TenSanPham { get; set; }
        public string MaSanPham { get; set; }
        public float NangXuatSanXuat { get; set; }
        public int SanLuongKeHoach { get; set; }
        public int LuyKeTH { get; set; }
        public int LuyKeTPThoatChuyen { get; set; }
        public int BTPLoi { get; set; }
        public bool IsFinish { get; set; }
        
    }
}
