using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business
{
    public class BLLHistoryPressedKeypad
    {
        private static PMSEntities db;
        static Object key = new object();
        private static volatile BLLHistoryPressedKeypad _Instance;  //volatile =>  tranh dung thread
        public static BLLHistoryPressedKeypad Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (key)
                    {
                        _Instance = new BLLHistoryPressedKeypad();
                        db = new PMSEntities();
                    }
                }
                return _Instance;
            }
        }

        private BLLHistoryPressedKeypad() { }

        public P_HistoryPressedKeypad Get(int lineId, string date)
        {
            return db.P_HistoryPressedKeypad.FirstOrDefault(x => !x.IsDeleted && x.LineId == lineId && x.Date == date);
        }

        public P_HistoryPressedKeypad[] Get(List<int> lineIds, string date)
        {
            return db.P_HistoryPressedKeypad.Where(x => !x.IsDeleted && lineIds.Contains(x.LineId) && x.Date == date).ToArray();
        }

        public bool Insert(P_HistoryPressedKeypad objModel)
        {
            try
            {
                db.P_HistoryPressedKeypad.Add(objModel);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            { }
            return false;
        }

        public bool Update(int lineId, int assignId, string date)
        {
            try
            {
                var obj = Get(lineId, date);
                if (obj != null)
                {
                    obj.AssignmentId = assignId;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    obj = new P_HistoryPressedKeypad();
                    obj.Id = 0; 
                    obj.LineId = lineId;
                    obj.AssignmentId = assignId;
                    obj.Date = date;
                    obj.IsDeleted = false;
                    Insert(obj);
                }
            }
            catch (Exception)
            { }
            return false;
        }

    }
}
