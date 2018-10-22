using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuAn03_HaiDang.POJO
{
    class InformationPosition
    {
        public string ViTri { get; set; }
        public List<InformationCell> listCell = new List<InformationCell>();
        public int ChangeTP { get; set; }
        public int ChangeBTP { get; set; }
        public int IsInt { get; set; }
        public int STTDen { get; set; }
    }
}
