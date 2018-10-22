using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMS.Data; 

namespace PMS.Business
{
  public static  class BLLMailType
    {
      public static List<MAIL_TYPE> GetAll()
      {
          try
          {
              var db = new PMSEntities();
              return db.MAIL_TYPE.Where(x => x.active).ToList();
          }
          catch (Exception ex)
          { }
          return null;
      }
    }
}
