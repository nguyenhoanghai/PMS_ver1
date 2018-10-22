using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Models
{
   public class WorkingTimeModel
    {
        public TimeSpan TimeStart { get; set; }
        public TimeSpan TimeEnd { get; set; }
        public int IntHours { get; set; }
        public string Name { get; set; }
        public int TC { get; set; }
        public double NormsHour { get; set; }
        public int KCS { get; set; }
        public float Lean { get; set; }
        public double BTPInLine { get; set; }
        public int Error { get; set; }
        public int BTP { get; set; }
        public int LineId { get; set; }
        public string HoursProductivity { get; set; }
        public string HoursProductivity_1 { get; set; }
        public bool IsShow { get; set; }
        public int CongDoan { get; set; }
    }
}
