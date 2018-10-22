using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuAn03_HaiDang.POJO
{
    public class MailSchedule
    {
        public int Id { get; set; }
        public int MailTemplateId { get; set; }
        public TimeSpan Time { get; set; }
        public bool IsActive { get; set; }
    }
}
