using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business
{
    public class CompletionPhase_DailyModel : P_CompletionPhase_Daily
    {
        public string CommoName { get; set; }
        public string TypeName { get; set; }
        public string PhaseName { get; set; }
        public string Time { get; set; }
    }
}
