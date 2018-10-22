using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Models
{
    public class LineBase_Model : Chuyen
    {
        public string ReadPercentName { get; set; }
        public List<Position_Model> Positions { get; set; }
        public List<Shift_Model> Shifts { get; set; }
        public LineBase_Model()
        {
            Positions = new List<Position_Model>();
            Shifts = new List<Shift_Model>();
        }
    }
}
