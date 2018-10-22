using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuanLyNangSuat.Model
{
    public class ModelWorkHours
    {
        public TimeSpan TimeStart { get; set; }
        public TimeSpan TimeEnd { get; set; }
        public int IntHours { get; set; }
        public string Name { get; set; }
        public float TC { get; set; }
        public float KCS { get; set; }
        public float Lean { get; set; }
        public float Error { get; set; }
    }
}
