using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business
{
    public class BLLMailSchedule
    {
        public static ResponseBase CreateOrUpdate(MAIL_SCHEDULE obj)
        {
            var result = new ResponseBase();
            var flag = true;
            try
            {
                var db = new PMSEntities();
                if (BLLMailSchedule.CheckExists(obj.Id, obj.Time, obj.MailTemplateId ?? 0) != null)
                {
                    result.IsSuccess = false;
                    result.Messages.Add(new Message() { Title = "Lỗi", msg = "Báo cáo này trong khoảng thời gian này đã tồn tại." });
                }
                else
                {
                    if (obj.Id == 0)
                    {
                        db.MAIL_SCHEDULE.Add(obj);
                    }
                    else
                    {
                        var mail = db.MAIL_SCHEDULE.FirstOrDefault(x => !x.IsDeleted && x.Id == obj.Id);
                        if (mail != null)
                        {
                            mail.MailTemplateId = obj.MailTemplateId;
                            mail.IsActive = obj.IsActive;
                            mail.Time = obj.Time;
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Không tìm thấy thông tin bạn đang thao tác." });
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

        public static MAIL_SCHEDULE GetById(int Id)
        {
            try
            {
                var db = new PMSEntities();
                return db.MAIL_SCHEDULE.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
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
                var MAIL_SCHEDULE = db.MAIL_SCHEDULE.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (MAIL_SCHEDULE != null)
                {
                    MAIL_SCHEDULE.IsDeleted = true;
                    db.SaveChanges();
                    result.IsSuccess = true;
                    result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Xóa thành công." });
                }
                else
                {
                    result.IsSuccess = false;
                    result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Không tìm thấy thông tin bạn đang thao tác." });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static List<MailScheduleModel> GetByTemplateId(int templateId)
        {
            try
            {
                var db = new PMSEntities();
                return db.MAIL_SCHEDULE.Where(x => !x.IsDeleted && x.MailTemplateId == templateId).Select(x => new MailScheduleModel()
                {
                    Id = x.Id,
                    IsActive = x.IsActive,
                    IsActiveStr = x.IsActive ? "Có" : "Không",
                    MailTemplateId = x.MailTemplateId,
                    Time = x.Time
                }).OrderBy(x=>x.Time).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static MAIL_SCHEDULE CheckExists(int Id, string time, int TemplateId)
        {
            try
            {
                var db = new PMSEntities();
                return db.MAIL_SCHEDULE.FirstOrDefault(x => !x.IsDeleted && x.Id != Id && x.MailTemplateId == TemplateId && x.Time == time);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<MAIL_SCHEDULE> GetAll( )
        {
            try
            {
                var db = new PMSEntities();
                return db.MAIL_SCHEDULE.Where(x => !x.IsDeleted && !x.MAIL_TEMPLATE.IsDeleted && x.IsActive && x.MAIL_TEMPLATE.IsActive).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
