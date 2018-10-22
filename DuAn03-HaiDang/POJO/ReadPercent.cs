using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuAn03_HaiDang.POJO
{
    class ReadPercent
    {
        public int Id { get; set; }
        public int PercentFrom { get; set; }
        public int PercentTo { get; set; }
        public int CountRepeat { get; set; }
        public string Sound { get; set; }
        public string Name { get; set; }
        public int IdParent { get; set; }
    }
}
