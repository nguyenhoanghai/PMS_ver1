using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Models
{
    public class LineModel : Chuyen
    {
        public string FloorName { get; set; }
        public string ReadPercentName { get; set; }
        public List<Position_Model> Positions { get; set; }
        public List<WorkingTimeModel> WorkingTimes { get; set; }
        public int KCS { get; set; }
        public int TC { get; set; } 
        public int ProBase { get; set; }  //Định mức
        public List<ErrorModel> Errors { get; set; }
        public int STT { get; set; }
        public int LastClusterId { get; set; }
         public List<LineWorkingShiftModel> Shifts { get; set; }
        public TimeSpan TimeCalculateTT { get; set; }
        public LineModel()
        {
            Shifts = new List<LineWorkingShiftModel>();
            Errors = new List<ErrorModel>();
            WorkingTimes = new List<WorkingTimeModel>();
            Positions = new List<Position_Model>();
        }
    }
}
