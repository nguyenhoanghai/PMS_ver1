using DuAn03_HaiDang.POJO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuAn03_HaiDang.Model
{
    public class ModelMailSend: MailSend
    {
        public string host { get; set; }
        public string port { get; set; }
        public string mail_type { get; set; }
    }
}
