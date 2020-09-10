using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Models
{
    public class LCDCollectionModel
    {
        public List<ProductivitiesModel> Lines { get; set; }
        public List<TheoDoiNgay> DayInfos { get; set; }
        public List<ModelSelectItem> Times { get; set; }
        public double NhipTT { get; set; }
        public double NhipSX { get; set; }
        public string Now { get; set; }

        public string KCS { get; set; }
        public string KH { get; set; }
        public string DoanhThu { get; set; }
        public string DoanhThuKH { get; set; } 
        public string Nhip { get; set; }

        public LCDCollectionModel()
        {
            Lines = new List<ProductivitiesModel>();
            DayInfos = new List<TheoDoiNgay>();
            Times = new List<ModelSelectItem>();
        }
    }
}
