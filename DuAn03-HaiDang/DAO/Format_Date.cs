using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuAn03_HaiDang.DAO
{
    class Format_Date
    {
        public DateTime getMMDDYYYY(DateTime time)
        {
            DateTime format = DateTime.Parse(time.Month.ToString()+"/"+time.Day.ToString()+"/"+time.Year.ToString()+" "+time.TimeOfDay);
            return format;
        }
    }
}
