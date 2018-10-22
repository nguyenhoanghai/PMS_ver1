using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMS.Data;

namespace PMS.Business.Models
{
   public class GroupErrorModel : GroupError
    {
       public GroupErrorModel()
       {
            ErrorsObj = new List<Error>();
        }
        public List<Error> ErrorsObj { get; set; }
    }
}
