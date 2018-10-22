using DuAn03_HaiDang.POJO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuAn03_HaiDang.Model
{
    public class ModelMailTeamplate: MailTemplateFile
    {
        public ModelMailSend mailSend { get; set; }
        public List<string> listMailReceive { get; set; }
        public List<ModelFile> listFile { get; set; }     

    }
}
