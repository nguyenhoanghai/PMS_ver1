using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuAn03_HaiDang.POJO;

namespace QuanLyNangSuat.Model
{
    public class ModelGroupErrorDetail: ModelGroupError
    {
        public List<Error> ListError { get; set; }
        public int TotalRow { get; set; }
    }
}
