using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuAn03_HaiDang.POJO
{
    public class NangSuat_Cum
    {
        public int Id { get; set; }
        public DateTime Ngay { get; set; }
        public int STTChuyen_SanPham { get; set; }
        public int IdCum { get; set; }
        public int SanLuongKCSTang { get; set; }
        public int SanLuongKCSGiam { get; set; }
        public int SanLuongTCTang { get; set; }
        public int SanLuongTCGiam { get; set; }
        public int BTPTang { get; set; }
        public int BTPGiam { get; set; }        
    }
}
