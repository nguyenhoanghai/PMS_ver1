using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Models
{
   public class KeypadObjectModel : KeyPad_Object
    {
        public int LineId { get; set; }
        public bool IsEndOfLine { get; set; }
        public string LineSound { get; set; }
    }
}
