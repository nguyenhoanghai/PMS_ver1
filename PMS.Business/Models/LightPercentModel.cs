using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Models
{
   public class LightPercentModel 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public List<LightPercentDetailModel> Childs { get; set; }

        public LightPercentModel()
        {
            Childs = new List<LightPercentDetailModel>();
        }
    }

   public class LightPercentDetailModel
   {
       public int Id { get; set; }
       public int LightPercentId { get; set; }
       public string ColorName { get; set; }
       public double From { get; set; }
       public double To { get; set; }
   }
}
