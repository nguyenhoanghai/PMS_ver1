﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuanLyNangSuat.Model
{
    public class ModelLabelArea
    {
        public int Id { get; set; }
        public string Font { get; set; }
        public float Size { get; set; }
        public bool Bold { get; set; }
        public bool Italic { get; set; }
        public string Color { get; set; }
        public int Position { get; set; }
        public string TableLayoutPanelName { get; set; }
        public int TableType { get; set; }
    }
}
