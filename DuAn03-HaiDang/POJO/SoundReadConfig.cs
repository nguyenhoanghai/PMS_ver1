using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuAn03_HaiDang.POJO
{
    public class SoundReadConfig
    {
        public SoundReadConfig()
        {
            listItem = new List<SoundReadItem>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int IdChuyen { get; set; }
        public int ConfigType { get; set; }
        public List<SoundReadItem> listItem { get; set; }       
    }
}
