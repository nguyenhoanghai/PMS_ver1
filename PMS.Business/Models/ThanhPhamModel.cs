using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Models
{
    public class ThanhPhamModel : ThanhPham
    {
        public int CommoId { get; set; }
        public string CommoName { get; set; }
        public NangXuat NangSuatObj { get; set; }
        public int LineId { get; set; } 
    }
}
