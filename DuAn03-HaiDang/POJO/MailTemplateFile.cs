using DuAn03_HaiDang.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuAn03_HaiDang.POJO
{
    public class MailTemplateFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }
        public int MailSendId { get; set; }
        public string ListMailReceiveId { get; set; }
        public List<int> listFileId { get; set; }
        public string MailSendName { get; set; }
    }
}
