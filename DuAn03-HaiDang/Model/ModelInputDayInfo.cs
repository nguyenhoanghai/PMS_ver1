using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuanLyNangSuat.Model
{
    public class ModelInputDayInfo
    {
        public int STT { get; set; }
        public int LineId { get; set; }
        public string LineName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int QuantityRecieve { get; set; }
        public TimeSpan Time { get; set; }
        public DateTime Date { get; set; }
        public int ClusterId { get; set; }
        public string ClusterName { get; set; }
        public bool IsEndOfLine { get; set; }
        public int STTLine_Product { get; set; }
        public int ErrorId { get; set; }
        public string ErrorName { get; set; }
        public int CommandTypeId { get; set; }
        public string CommandTypeName { get; set; }
        public int ProductOutputTypeId { get; set; }
        public string ProductOutputTypeName { get; set; }
    }
}
