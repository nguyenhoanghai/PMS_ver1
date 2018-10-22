using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business
{
    public class BLLFloor
    {
        public static List<Floor> GetAll()
        {
            var db = new PMSEntities();
            return db.Floors.ToList();
        }

        public static SelectListAndDefaultModel GetFloorForComBoBox()
        {
            var rs = new SelectListAndDefaultModel();
            try
            {
                var db = new PMSEntities();
                var floors = db.Floors.ToList();
                var defaultValue = 0;
                if (floors.Count > 0)
                { 
                    foreach (var item in floors)
                    {
                        rs.SelectList.Add(item);
                        defaultValue = item.IsDefault ? item.IdFloor : defaultValue;
                    }
                }
                else
                    rs.SelectList.Add(new Floor() { IdFloor = 0, Name = "Không có thông tin Lầu" });
                rs.DefaultValue = defaultValue;
            }
            catch (Exception)
            { }
            return rs;
        }
    }
}
