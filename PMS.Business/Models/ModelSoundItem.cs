using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Models
{
    public class ModelSoundItem
    {
        public string SoundPath { get; set; }
        public string Formula { get; set; }
        public int FileType { get; set; }
        public bool IsProductivity { get; set; }
    }
}
