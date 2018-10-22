using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Models
{
   public class ErrorModel : Error
    {
        public int Quantity { get; set; }
        public string GroupName { get; set; }
    }
}
