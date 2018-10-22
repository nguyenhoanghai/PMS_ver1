using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Models
{
   public class AppConfigModel
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string  Name { get; set; } 
        public string Value { get; set; }
        public string Description { get; set; }
    }
}
