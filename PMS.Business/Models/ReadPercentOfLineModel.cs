using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMS.Data;

namespace PMS.Business.Models
{
   public class ReadPercentOfLineModel : P_ReadPercentOfLine
    {
        public string LineName { get; set; }
        public string ReadPercentName { get; set; }
        public string ReadPercent_KCSInventoryName { get; set; }
        public int CommoId { get; set; }
        public string CommoName { get; set; }
        public string KanbanLightReadPercentName { get; set; }
        public string ProductLightReadPercentName { get; set; }
    }
}
