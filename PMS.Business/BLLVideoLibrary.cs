using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business
{
   public class BLLVideoLibrary
    {
       public static ResponseBase CreateOrUpdate(P_VideoLibrary obj)
       {
           var result = new ResponseBase();
           var flag = true;
           try
           {
               var db = new PMSEntities();
               if (obj.Id == 0)
               {
                   db.P_VideoLibrary.Add(obj);
               }
               else
               {
                   var oldObj = db.P_VideoLibrary.FirstOrDefault(x => !x.IsDeleted && x.Id == obj.Id);
                   if (oldObj != null)
                   {
                       //sound.Code = obj.Code;
                       oldObj.Name = obj.Name; 
                     //  sound.Path = obj.Path; 
                   }
                   else
                   {
                       result.IsSuccess = false;
                       result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Không tìm thấy thông tin tệp bạn đang thao tác." });
                   }
               }
               if (flag)
               {
                   db.SaveChanges();
                   result.IsSuccess = true;
                   result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Lưu thành công." });
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
           return result;
       }

       public static List<P_VideoLibrary> Gets()
       {
           try
           {
               var db = new PMSEntities();
               return db.P_VideoLibrary.Where(x => !x.IsDeleted).ToList();
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static P_VideoLibrary GetById(int Id)
       {
           try
           {
               var db = new PMSEntities();
               return db.P_VideoLibrary.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static ResponseBase Delete(int Id)
       {
           var result = new ResponseBase();
           try
           {
               var db = new PMSEntities();
               var obj = db.P_VideoLibrary.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
               if (obj != null)
               {
                   obj.IsDeleted = true;
                   db.SaveChanges();
                   result.IsSuccess = true;
                   result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Xóa thành công." });
               }
               else
               {
                   result.IsSuccess = false;
                   result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Không tìm thấy thông tin tệp bạn đang thao tác." });
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
           return result;
       }

    }
}
