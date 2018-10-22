using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuanLyNangSuat.Model
{
    public class ModelLCDError
    {
        public ModelLCDError()
        {
            this.listModelGroupErrorDetail = new List<ModelGroupErrorDetail>();
        }
        public List<ModelGroupErrorDetail> listModelGroupErrorDetail {get; set;}
        public int totalRow { get; set; }
    }
}
