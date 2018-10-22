using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuAn03_HaiDang.POJO
{
    public class SoundIntConfig
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Formula { get; set; }
        public bool IsActive { get; set; }
        public bool IsProductivity { get; set; }
    }
}
