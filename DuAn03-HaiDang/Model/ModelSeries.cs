using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuanLyNangSuat.Model
{
    public class ModelSeries
    {
        public string SeriesName { get; set; }
        public bool ShowInLegend { get; set; }
        public List<Point> ListPoint { get; set; }
    }

    public class Point
    {
        public dynamic X { get; set; }
        public double Y { get; set; }
    }
}
