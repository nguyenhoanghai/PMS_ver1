using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Models
{
   public class DepartmentDailyLaboursModel : P_DepartmentDailyLabour
    {
        public string DepartmentName { get; set; }
        public int BaseLabours { get; set; }
    }
}
