﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Models
{
   public class PhaseDailyModel
    {
        public int Id { get; set; }
        public int assignId { get; set; }
        public int NangSuatId { get; set; }
        public int PhaseId { get; set; }
        public int Quantity { get; set; }
        public int CommandTypeId { get; set; }
    }
}
