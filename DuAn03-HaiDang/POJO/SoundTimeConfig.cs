using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuAn03_HaiDang.POJO
{
    public class SoundTimeConfig
    {
        public int Id { get; set; }
        public TimeSpan Time { get; set; }
        public int SoLanDoc { get; set; }
        public int ConfigType { get; set; }
        public bool IsActive { get; set; }
    }
}
