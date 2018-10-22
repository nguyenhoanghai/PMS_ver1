using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuanLyNangSuat.Model
{
    public class ModelTableLayoutPanel
    {
        public int Id { get; set; }
        public int ColumnInt { get; set; }
        public int RowInt { get; set; }
        public string TableLayoutPanelName { get; set; }
        public float SizePercent { get; set; }
        public bool IsShow { get; set; }
        public int TableType { get; set; }
    }
}
