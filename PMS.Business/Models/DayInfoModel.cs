using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Models
{
   public class DayInfoModel : TheoDoiNgay
    {
        public string LineName { get; set; }
        public string ClusterName { get; set; }
        public string CommoName { get; set; }
        public string ProType { get; set; }
        public string CommandType { get; set; }
        public string ErrorName { get; set; }
        public string KeypadType { get; set; }
        public int EquipmentId { get; set; }
    }
}
