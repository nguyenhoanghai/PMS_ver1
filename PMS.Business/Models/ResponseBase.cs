using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Business.Models
{
    public class ResponseBase
    {
        public ResponseBase()
        {
            Messages = new List<Message>();
            DataSendKeyPads = new List<string>();
        }
        public bool IsSuccess { get; set; }
        public List<Message> Messages { get; set; }

        public dynamic Data { get; set; }
        public dynamic Records { get; set; }
        public List<string> DataSendKeyPads { get; set; }
        public bool IsPlaySound { get; set; }
        public bool AlertHetHang { get; set; }
        public int AlertRepeat { get; set; }
        public string DataSendKeyPad { get; set; }
    }

    public class Message
    {
        public string Title { get; set; }
        public string msg { get; set; }
    }
}
