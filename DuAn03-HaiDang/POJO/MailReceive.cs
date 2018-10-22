using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuAn03_HaiDang.POJO
{
    public class MailReceive
    {
        
        public int Id { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public bool IsActive { get; set; }
        public int MailTypeId { get; set; }
    }
}
