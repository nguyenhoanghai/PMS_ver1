using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Models
{
    public class MailSend_Model : MAIL_SEND
    {
        public string MailTypeName { get; set; }
        public string Port { get; set; }
        public string Host { get; set; }
    }
}
