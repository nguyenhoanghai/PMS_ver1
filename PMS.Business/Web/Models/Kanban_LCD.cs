using PMS.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Web.Models
{
    public class Kanban_LCD
    {
         public string LineName { get; set; }
        public string ProductName { get; set; }
        public int BTPOnDay { get; set; }
        public int BTPTotal { get; set; }
        public string BTPBQ { get; set; }
        public string StatusColor { get; set; }
        public string LightBTPConLai { get; set; }
        public int LK_BTP_HC { get; set; }
        public int LK_BTP { get; set; }
        public int ProductionPlans { get; set; }
        public int BTPBinhQuan { get; set; }
        public int BTPInLine { get; set; }
        public int BTP_Ton { get; set; }
        public List<PhaseModel> BTPHC_Structs { get; set; }
        public Kanban_LCD()
        {
            BTPHC_Structs = new List<PhaseModel>();
        }
    }
}
