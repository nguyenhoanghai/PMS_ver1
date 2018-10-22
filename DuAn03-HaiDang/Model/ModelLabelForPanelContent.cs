using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuanLyNangSuat.Model
{
    public class ModelLabelForPanelContent
    {
        public int Id { get; set; }
        public string LabelName { get; set; }
        public int IntRowTBLPanelContent { get; set; }
        public int SttNext { get; set; }
        public int TableType { get; set; }
        public bool IsShow { get; set; }
        public string SystemValueName { get; set; }
    }
}
