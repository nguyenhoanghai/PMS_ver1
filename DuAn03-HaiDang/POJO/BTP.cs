using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuAn03_HaiDang.POJO
{
    public class BTP
    {
        public string Ngay { get; set; }
        public string STTChuyen_SanPham { get; set; }
        public int STT { get; set; }
        public int BTPNgay { get; set; }
        public TimeSpan TimeUpdate { get; set; }
        public int CumId { get; set; }
        public int CommandTypeId { get; set; }
        public bool IsEndOfLine { get; set; }
    }
}
