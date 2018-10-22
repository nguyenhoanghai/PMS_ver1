using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business
{
    public class BLLMailSend
    {
        public static ResponseBase CreateOrUpdate(MAIL_SEND obj)
        {
            var result = new ResponseBase();
            var flag = true;
            try
            {
                var db = new PMSEntities();
                if (CheckExists(obj.Id, obj.MailTypeId, obj.Address) != null)
                {
                    result.IsSuccess = false;
                    result.Messages.Add(new Message() { Title = "Lỗi", msg = "Loại Mail với địa chỉ mail này đã tồn tại." });
                }
                else
                {
                    if (obj.Id == 0)
                    {
                        db.MAIL_SEND.Add(obj);
                    }
                    else
                    {
                        var mail = db.MAIL_SEND.FirstOrDefault(x => !x.IsDeleted && x.Id == obj.Id);
                        if (mail != null)
                        {
                            mail.Address = obj.Address;
                            mail.MailTypeId = obj.MailTypeId;
                            mail.IsActive = obj.IsActive;
                            if (!string.IsNullOrEmpty(obj.PassWord))
                                mail.PassWord = obj.PassWord;
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static MailSend_Model GetById(int Id)
        {
            try
            {
                var db = new PMSEntities();
                return db.MAIL_SEND.Where(x => !x.IsDeleted && x.Id == Id).Select(x => new MailSend_Model()
                {
                    Id = x.Id,
                    Address = x.Address,
                    IsActive = x.IsActive,
                    MailTypeId = x.MailTypeId,
                    MailTypeName = x.MAIL_TYPE.TypeName,
                    Host = x.MAIL_TYPE.host,
                    Port = x.MAIL_TYPE.port,
                    Note = x.Note,
                    PassWord = x.PassWord
                }).FirstOrDefault();
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
                var MAIL_SEND = db.MAIL_SEND.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (MAIL_SEND != null)
                {
                    MAIL_SEND.IsDeleted = true;
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

        public static List<MailSend_Model> GetAll()
        {
            try
            {
                var db = new PMSEntities();
                return db.MAIL_SEND.Where(x => !x.IsDeleted).Select(x => new MailSend_Model()
                {
                    Id = x.Id,
                    Address = x.Address,
                    MailTypeId = x.MailTypeId,
                    MailTypeName = x.MAIL_TYPE.TypeName,
                    Note = x.Note,
                    PassWord = x.PassWord,
                    IsActive = x.IsActive
                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static MAIL_SEND CheckExists(int Id, int Type, string add)
        {
            try
            {
                var db = new PMSEntities();
                return db.MAIL_SEND.FirstOrDefault(x => !x.IsDeleted && x.Id != Id && x.MailTypeId == Type && x.Address.Trim().ToUpper().Equals(add.Trim().ToUpper()));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
