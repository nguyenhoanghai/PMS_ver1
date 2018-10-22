using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business
{
    public static class BLLMailFile
    {
        public static List<MAIL_FILE> GetAll()
        {
            try
            {
                var db = new PMSEntities();
                return db.MAIL_FILE.Where(x => !x.IsDeleted && x.IsActive).ToList();
            }
            catch (Exception ex)
            { }
            return null;
        }
        public static List<MAIL_FILE> GetAll(List<int> Ids)
        {
            try
            {
                var db = new PMSEntities();
                return db.MAIL_FILE.Where(x => !x.IsDeleted && Ids.Contains(x.Id)).ToList();
            }
            catch (Exception ex)
            { }
            return null;
        }
    }
}
