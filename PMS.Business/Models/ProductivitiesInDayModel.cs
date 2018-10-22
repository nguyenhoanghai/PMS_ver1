using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Models
{
   public class ProductivitiesInDayModel
    {
        public string LineName { get; set; }
        public int LaborInLine { get; set; }
        public string CommoName { get; set; }
        public int ProductionPlans { get; set; }
        public int LK_TH { get; set; }
        public int LK_TC { get; set; }
        public int LK_BTP { get; set; }
        public int NormsOfDay { get; set; }
        public int TH_Day { get; set; }
        public int TC_Day { get; set; }
        public int BTP_Day { get; set; }
        public int ErrorsInDay { get; set; }
        public int BTPInLine { get; set; }
        public float TH_Percent { get; set; }
        public float ErrorPercent { get; set; }
        public int Funds { get; set; } //vốn
        public double RevenuesInMonth { get; set; } //doanh thu thang
        public double RevenuesInDay { get; set; }
        public double ResearchPaced { get; set; }
        public double CurrentPacedProduction { get; set; }
        public double TC_Paced { get; set; }

        public bool IsFinish { get; set; }

       
    }
}
