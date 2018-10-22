using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuAn03_HaiDang.Model
{
    public class ModelLCDTongHopInfo
    {
        public ModelLCDTongHopInfo()
        {
            this.ListLineNS = new List<ModelLCDLineInfo>();
            this.ListNSGio = new List<ModelLCDHourNSInfo>();
        }
        public System.Windows.Forms.Label LabelKeHoachTong { get; set; }
        public System.Windows.Forms.Label LabelDoanhThuTong { get; set; }
        public System.Windows.Forms.Label LabelNhipSanXuat { get; set; }
        public List<ModelLCDLineInfo> ListLineNS { get; set; }
        public List<ModelLCDHourNSInfo> ListNSGio { get; set; }
    }

    public class ModelLCDLineInfo
    {
        public ModelLCDLineInfo()
        {
            this.ListLabelNSInfo = new List<ModelLabel>();
        }
        public int MaChuyen { get; set; }
        public string TenChuyen { get; set; }
        public List<ModelLabel> ListLabelNSInfo { get; set; }
    }

    public class ModelLCDHourNSInfo
    {        
        public int intHour { get; set; }
        public System.Windows.Forms.Label LabelValue { get; set; }
    }

    public class ModelLabel
    {
        public string SystemName { get; set; }
        public System.Windows.Forms.Label Label { get; set; }
    }
}
