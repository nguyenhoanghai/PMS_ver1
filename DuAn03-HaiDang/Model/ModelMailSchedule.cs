using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuAn03_HaiDang.Model
{
    public class ModelMailSchedule
    {
        public List<TimeSpan> listTime { get; set; }
        public int MailTemplateId { get; set; }
    }
}
