using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Models
{
  public  class SelectListAndDefaultModel
    {
        public List<dynamic> SelectList { get; set; }
        public dynamic DefaultValue { get; set; }
        public SelectListAndDefaultModel()
        {
            SelectList = new List<dynamic>();
        }
    }
}
