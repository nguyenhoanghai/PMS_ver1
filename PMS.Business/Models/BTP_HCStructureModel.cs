using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Models
{
   public class PhaseModel
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public bool IsShow { get; set; }
    }
}
