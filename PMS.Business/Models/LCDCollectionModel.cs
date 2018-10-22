using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Models
{
   public class MonthlyProductionModel : P_MonthlyProductionPlans
    {
        public int LineId { get; set; }
        public double PriceCM { get; set; }
    }
}
