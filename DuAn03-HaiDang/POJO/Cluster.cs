using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuAn03_HaiDang.POJO
{
    public class Cluster
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int LineId { get; set; }
        public int FloorId { get; set; }
        public string FloorName { get; set; }
        public string LineName { get; set; }
        public bool IsEndOfLine { get; set; }
    }
}
