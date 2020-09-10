using PMS.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Web.Models
{
   public class TongHop_LCD_App
    {
        public DateTime Now { get; set; }
        public int KCS  { get; set; }
        public int KH { get; set; }
        public double DoanhThu  { get; set; }
        public double DoanhThuKH  { get; set; }
        public double NhipSX { get; set; }
        public double NhipKH { get; set; }
        public List<TongHop_LCD> Lines { get; set; }
        public TongHop_LCD_App()
        {
            Lines = new List<TongHop_LCD>();
        }
    }
}
