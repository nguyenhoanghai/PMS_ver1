using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Models
{
   public class LightPercentModel: P_LightPercent
    {
        public List<P_LightPercent_De> Childs { get; set; }

        public LightPercentModel()
        {
            Childs = new List<P_LightPercent_De>();
        }
    }
}
