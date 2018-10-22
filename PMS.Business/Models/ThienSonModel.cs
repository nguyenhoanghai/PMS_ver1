using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Models
{
    public class ThienSonModel
    {
        public List<LineObj> Lines { get; set; }
        public List<RowObj> Rows { get; set; }
        public ThienSonModel() {
            Lines = new List<LineObj>();
            Rows = new List<RowObj>();
        }
    }

    public class LineObj
    {
        public string Name { get; set; }
        public int Labours { get; set; }
        public string CusName { get; set; }
        public string CommoName { get; set; }
        public double PriceCM { get; set; }
    }

    public class RowObj
    {
        public dynamic DaTH { get; set; }
        public dynamic LuyKe { get; set; }
        public dynamic ConLai { get; set; }
        public dynamic h1 { get; set; }
        public dynamic h2 { get; set; }
        public dynamic h3 { get; set; }
        public dynamic h4 { get; set; }
        public dynamic h5 { get; set; }
        public dynamic h6 { get; set; }
        public dynamic h7 { get; set; }
        public dynamic h8 { get; set; }
        public dynamic h9 { get; set; }
        public dynamic h10 { get; set; }
        public dynamic h11 { get; set; }
        public dynamic h12 { get; set; }
        public dynamic h13 { get; set; }
        public dynamic h14 { get; set; }
        public dynamic h15 { get; set; }
        public dynamic tong { get; set; }
    }
}
