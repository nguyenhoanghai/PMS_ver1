using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Web.Models
{
    public class HoanTat_LCD
    {
        public int LineId { get; set; }
        public int CSPId { get; set; }
        public int Index { get; set; }
        public string LineName { get; set; }
        public string ProName { get; set; }
        public int SLKH { get; set; }
        public int LK_KCS { get; set; }
        public int LK_TC { get; set; }
        public List<HoanTatPhase> Phases { get; set; }
        public HoanTat_LCD()
        {
            Phases = new List<HoanTatPhase>();
        }
    }

    public class HoanTatPhase
    {
        public int PhaseId { get; set; }
        public int Index { get; set; }
        public int LK { get; set; }
        public string PhaseName { get; set; }
    }
}
