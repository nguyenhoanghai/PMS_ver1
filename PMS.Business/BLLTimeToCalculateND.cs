using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business
{
    public static class BLLTimeToCalculateND
    {
        public static TimeSpan LayTimeBatDau(DateTime Ngay, int MaChuyen)
        {
            try
            {
                var db = new PMSEntities();
                var daynow = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                var time = db.ThoiGianTinhNhipDoTTs.FirstOrDefault(x => x.Ngay == daynow && x.MaChuyen == MaChuyen);
                if (time != null)
                    return TimeSpan.Parse(time.ThoiGianBatDau.ToString());
                else
                    return TimeSpan.Parse("00:00:00");
            }
            catch (Exception)
            {
                return TimeSpan.Parse("00:00:00");
            }
        }

        public static ResponseBase InsertOrUpdate(ThoiGianTinhNhipDoTT obj)
        {
            var rs = new ResponseBase();
            try
            {
                var db = new PMSEntities();
                var old = db.ThoiGianTinhNhipDoTTs.FirstOrDefault(x => x.Ngay == obj.Ngay && x.MaChuyen == obj.MaChuyen);
                if (old == null)
                    db.ThoiGianTinhNhipDoTTs.Add(obj);
                else
                    old.ThoiGianBatDau = obj.ThoiGianBatDau;
                db.SaveChanges();
                rs.IsSuccess = true;
            }
            catch (Exception)
            {
                rs.IsSuccess = false;
            }
            return rs;
        }
    }
}
