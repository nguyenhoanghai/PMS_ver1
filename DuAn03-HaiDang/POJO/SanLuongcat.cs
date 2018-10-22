using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuAn03_HaiDang.POJO
{
    public class SanLuongCat
    {
        public int Id { get; set; }
        public int IdToCat { get; set; }
        public int IdSanPham { get; set; }
        public int SanLuong { get; set; }
        public TimeSpan ThoiGianNapSL { get; set; }
        public DateTime NgayNapSL { get; set; }
        public bool IsDeleted { get; set; }
    }
}
