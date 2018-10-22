using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Models
{
   public class AssignmentForLine_Grid_Model
    {

        public int STT { get; set; }
        public int STT_TH { get; set; }
        public string CommoName { get; set; }
        public double TimeProductPerCommo { get; set; }
        public int ProductionPlans { get; set; }
        public int LK_TH { get; set; }
        public int Month { get; set; }
        public int Year { get; set; } 
        public string IsFinishStr { get; set; } 
    }
}
