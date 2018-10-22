using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuanLyNangSuat.Model
{
    public class ModelShowLCDError
    {
        public int ErrorId { get; set; }
        public int GroupErrorId { get; set; }
        public int LabelType { get; set; }
        public System.Windows.Forms.Label Label { get; set; }
    }
}
