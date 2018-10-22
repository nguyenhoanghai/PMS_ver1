using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business
{
   public class BLLMailReceive
    { 
        public static ResponseBase CreateOrUpdate(MAIL_RECEIVE obj)
        {
            var result = new ResponseBase();
            var flag = true;
            try
            {
                var db = new PMSEntities();
                if (obj.Id == 0)
                {
                    db.MAIL_RECEIVE.Add(obj);
                }
                else
                {
                    var mail = db.MAIL_RECEIVE.FirstOrDefault(x => !x.IsDeleted && x.Id == obj.Id);
                    if (mail != null)
                    {
                        mail.Address = obj.Address; 
                        mail.IsActive = obj.IsActive; 
                        mail.Note = obj.Note;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Không tìm thấy thông tin Mail bạn đang thao tác." });
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

        public static MAIL_RECEIVE GetById(int Id)
        {
            try
            {
                var db = new PMSEntities();
                return db.MAIL_RECEIVE.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
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
                var MAIL_RECEIVE = db.MAIL_RECEIVE.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (MAIL_RECEIVE != null)
                {
                    MAIL_RECEIVE.IsDeleted = true;
                    db.SaveChanges();
                    result.IsSuccess = true;
                    result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Xóa thành công." });
                }
                else
                {
                    result.IsSuccess = false;
                    result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Không tìm thấy thông tin Mail bạn đang thao tác." });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static List<MAIL_RECEIVE> GetAll()
        {
            try
            {
                var db = new PMSEntities();
                return db.MAIL_RECEIVE.Where(x => !x.IsDeleted).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<MAIL_RECEIVE> GetAll(List<int> Ids)
        {
            try
            {
                var db = new PMSEntities();
                return db.MAIL_RECEIVE.Where(x => !x.IsDeleted && Ids.Contains(x.Id)).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
