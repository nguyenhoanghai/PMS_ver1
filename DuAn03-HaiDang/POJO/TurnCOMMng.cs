using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuanLyNangSuat.POJO
{
    public class TurnCOMMng
    {
        public int Id { get; set; }
        public int COMTypeId { get; set; }
        public string COMTypeName { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public TimeSpan TimeAction { get; set; }
        public string Description { get; set; }
        public bool IsActive {get; set; }
    }
}
