using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuAn03_HaiDang.POJO
{
    public class Error
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string ErrorName { get; set; }
        public string Description { get; set; }
        public int GroupErrorId { get; set; }
        public string GroupErrorName { get; set; }
    }
}

