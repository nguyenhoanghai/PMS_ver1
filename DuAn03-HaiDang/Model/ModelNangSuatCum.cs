using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuAn03_HaiDang.Model
{
    public class ModelNangSuatCum
    {
        public string chuyen { get; set; }
        public string maHang { get; set; }
        public string sanLuongKeHoach { get; set; }
        public List<NSCum> listNangSuatCum { get; set; }

    }

    public class NSCum
    {
        public int clusterId { get; set; }
        public string cum { get; set; }
        public string sanLuong { get; set; }
    }
}
