using PMS.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 

namespace PMS.Business.Models
{
   public class NangSuat_Report_H
    {
        public string LineName { get; set; }
        public string Labors { get; set; }
        public string CommoditityName { get; set; }
        public double SanLuongKeHoach { get; set; }
        public double LuyKeTH { get; set; }
        public double LuyKeBTP { get; set; }
        public double BTPNgay { get; set; }
        public double BTPTrenChuyen { get; set; }
        public double Von { get; set; }
        public double DinhMucGio { get; set; }
        public double DinhMucNgay { get; set; }
        public double ThuNhapNgay { get; set; }
        public double ThuNhapThang { get; set; }
        public double ThuNhapBQ { get; set; }
        public double NhipTT { get; set; }
        public double NhipNC { get; set; }
        public double TongTHNgay { get; set; }
        public double TongTCNgay { get; set; }
        public int TGDaLV { get; set; }
        public List<WorkingTimeModel> LineWorkingTimes { get; set; }
    }
}
