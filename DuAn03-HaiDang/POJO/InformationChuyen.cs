using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuAn03_HaiDang.POJO
{
    class InformationChuyen
    {
        public string MaChuyen { get; set; }
        public string TenChuyen { get; set; }
        public int Labor { get; set; }
        public List<InformationPosition> listPosition = new List<InformationPosition>();
        public List<Shift> listShift = new List<Shift>();
    }
}
