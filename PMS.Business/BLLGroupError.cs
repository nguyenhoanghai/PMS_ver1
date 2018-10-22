using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business
{
   public class BLLGroupError
    { 
       public static List<GroupError> GetAll()
       {
           try
           {
               var db = new PMSEntities();
               return db.GroupErrors.Where(x => !x.IsDeleted).ToList();
           }
           catch (Exception ex)
           {               
                throw ex;
           }
       }

       public static ResponseBase InsertOrUpdate(GroupError obj)
       {
           var result = new ResponseBase();
           try
           {
               var db = new PMSEntities();
               var isOk = true;
               if (obj.Id == 0)
               {
                   db.GroupErrors.Add(obj);
               }
               else
               {
                   var error = db.GroupErrors.FirstOrDefault(x => !x.IsDeleted && x.Id == obj.Id);
                   if (error != null)
                   {
                       error.Code = obj.Code;
                       error.Name = obj.Name;
                       error.Description = obj.Description; 
                   }
                   else
                   {
                       isOk = false;
                       result.IsSuccess = false;
                       result.Messages.Add(new Message() { Title = "Lỗi", msg = "cập nhật thông tin thất bại" });
                   }
               }
               if (isOk)
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

       public static ResponseBase Delete(int Id)
       {  
           var result = new ResponseBase();
           try
           {
               var db = new PMSEntities();
               var error = db.GroupErrors.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
               if (error != null)
               {
                   error.IsDeleted = true;
                   db.SaveChanges();
                   result.IsSuccess = true;
                   result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Xóa Thành công." });
               }
               else
               {
                   result.IsSuccess = false;
                   result.Messages.Add(new Message() { Title = "Lỗi", msg = "không tìm thấy lỗi." });
               }
           }
           catch (Exception)
           {
               result.IsSuccess = false;
               result.Messages.Add(new Message() { Title = "Lỗi", msg = "không tìm thấy lỗi." });
           }
           return result;
       }

       public static GroupError Find(int Id)
       {
           try
           {
               var db = new PMSEntities();
               return db.GroupErrors.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
           }
           catch (Exception ex)
           {
               throw ex;
           }
           return null;
       }
    }
}
