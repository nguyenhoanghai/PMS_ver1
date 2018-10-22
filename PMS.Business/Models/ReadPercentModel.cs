using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Models
{
    public class ReadPercentModel : ReadPercent
    {
        public List<ReadPercent> Childs { get; set; }

        public ReadPercentModel()
        {
            Childs = new List<ReadPercent>();
        }
    }
}
