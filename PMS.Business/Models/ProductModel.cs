using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Models
{
  public  class ProductModel
    {
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string DinhNghia { get; set; }
        public int Floor { get; set; }
        public double DonGia { get; set; }
        public double DonGiaCM { get; set; }
        public double ProductionTime { get; set; }
    }
}
