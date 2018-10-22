using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Models
{
   public class KeypadModel : KeyPad
    {
       
        public List<KeypadObjectModel> objs { get; set; }
        public int? sttNut { get; set; }
        public int? CommandTypeId { get; set; }
        public int ClusterId { get; set; }
        public int LineId { get; set; }
        public bool IsEndOfLine { get; set; }

       /// <summary>
       /// hình thức keypad sử dụng
       /// 0 = KCS + TC
       /// 1 = KCS
       /// 2 = TC
       /// </summary>
        public int TypeOfKeypad { get; set; }
       public KeypadModel() {
           objs = new List<KeypadObjectModel>();
       }
    }
}
