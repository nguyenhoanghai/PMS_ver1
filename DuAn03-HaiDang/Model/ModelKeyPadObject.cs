using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuAn03_HaiDang.Model
{
    public class ModelKeyPadObject
    {
        public int LineId { get; set; }
        public int KeyPadId { get; set; }
        public int EquipmentId { get; set; }
        public int ClusterId { get; set; }
        public bool IsEndOfLine { get; set; }
        public int UseTypeId { get; set; }
    }
}
