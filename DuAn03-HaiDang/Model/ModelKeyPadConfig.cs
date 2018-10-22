using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuAn03_HaiDang.Model
{
    public class ModelKeyPadConfig
    {
        public int KeyPadId { get; set; }
        public string KeyPadName { get; set; }
        public int EquipmentId { get; set; }
        public int UseTypeId { get; set; }
        public  List<ModelKeyPadObjectConfig> ListObjectConfig { get; set; }
    }

    public class ModelKeyPadObjectConfig
    {
        public int STTNut { get; set; }
        public int ClusterId { get; set; }       
        public int CommandTypeId { get; set; }
        public int LineId { get; set; }
        public bool IsEndOfLine { get; set; }
        public string LineSound { get; set; }
    }

}
