using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Models
{
   public class Position_Model : TTHienThi
    { 
        public List<Cell_Model> Cells { get; set; }
        public Position_Model()
        {
            Cells = new List<Cell_Model>();
        }
    }
}
