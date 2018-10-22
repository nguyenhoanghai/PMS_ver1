using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuanLyNangSuat.GridView_Model
{
  public  class Error_Grid_Model
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int GroupErrorId { get; set; }
        public string GroupName { get; set; }
    }
}
