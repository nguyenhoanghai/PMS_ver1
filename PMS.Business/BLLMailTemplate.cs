using GPRO.Ultilities;
using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business
{
    public class BLLMailTemplate
    {
        public static ResponseBase CreateOrUpdate(MailTemplateModel obj)
        {
            var result = new ResponseBase();
            var flag = true;
            MAIL_TEMPLATE mailTemplate = null;
            MAIL_T_M attachFile = null;
            try
            {
                var db = new PMSEntities();
                if (BLLMailTemplate.CheckExists(obj.Id, obj.Name) != null)
                {
                    result.IsSuccess = false;
                    result.Messages.Add(new Message() { Title = "Lỗi", msg = "Tên mail này đã tồn tại." });
                }
                else
                {
                    if (obj.Id == 0)
                    {
                        mailTemplate = new MAIL_TEMPLATE();
                        Parse.CopyObject(obj, ref mailTemplate);
                        //mailTemplate.Name = obj.Name;
                        //mailTemplate.Subject = obj.Subject;
                        //mailTemplate.Content = obj.Content;
                        //mailTemplate.MailSendId = obj.MailSendId;
                        //mailTemplate.MailReceiveIds = obj.MailReceiveIds;
                        //mailTemplate.MailFileIds = obj.MailFileIds;
                        //mailTemplate.Description = obj.Description;
                        //mailTemplate.IsActive = obj.IsActive;
                        //if (obj.AttachFiles != null && obj.AttachFiles.Count > 0)
                        //{
                        //    mailTemplate.MAIL_T_M = new List<MAIL_T_M>();
                        //    foreach (var item in obj.AttachFiles)
                        //    {
                        //        attachFile = new MAIL_T_M();
                        //        attachFile.MailFileId = item;
                        //        attachFile.MAIL_TEMPLATE = mailTemplate;
                        //        mailTemplate.MAIL_T_M.Add(attachFile);
                        //    }
                        //}
                        db.MAIL_TEMPLATE.Add(mailTemplate);
                    }
                    else
                    {
                        mailTemplate = db.MAIL_TEMPLATE.FirstOrDefault(x => !x.IsDeleted && x.Id == obj.Id);
                        if (mailTemplate != null)
                        {
                            mailTemplate.Name = obj.Name;
                            mailTemplate.Subject = obj.Subject;
                            mailTemplate.Content = obj.Content;
                            mailTemplate.MailSendId = obj.MailSendId;
                            mailTemplate.MailReceiveIds = obj.MailReceiveIds;
                            mailTemplate.MailFileIds = obj.MailFileIds;
                            mailTemplate.Description = obj.Description;
                            mailTemplate.IsActive = obj.IsActive;

                            //if (obj.AttachFileChange)
                            //{
                            //    var oldAtt = db.MAIL_T_M.Where(x => !x.IsDeleted && x.MailTemplateId == mailTemplate.Id);
                            //    var ids = new List<int>();
                            //    if (oldAtt != null && oldAtt.Count() > 0)
                            //    {
                            //        if (obj.AttachFiles != null && obj.AttachFiles.Count > 0)
                            //        {
                            //            foreach (var item in obj.AttachFiles)
                            //            {
                            //                var exists = oldAtt.FirstOrDefault(x => x.MailFileId == item);
                            //                if (exists != null)
                            //                {
                            //                    ids.Add(exists.Id);
                            //                }
                            //                else
                            //                {
                            //                    attachFile = new MAIL_T_M();
                            //                    attachFile.MailFileId = item;
                            //                    attachFile.IsActive = true;
                            //                    attachFile.MailTemplateId = mailTemplate.Id;
                            //                    db.MAIL_T_M.Add(attachFile);
                            //                }
                            //            }
                            //        }

                            //        var deleteobj = oldAtt.Where(x => !ids.Contains(x.Id));
                            //        foreach (var item in deleteobj)
                            //        {
                            //            item.IsDeleted = true;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        foreach (var item in obj.AttachFiles)
                            //        {
                            //            attachFile = new MAIL_T_M();
                            //            attachFile.MailFileId = item;
                            //            attachFile.MailTemplateId = mailTemplate.Id;
                            //            attachFile.IsActive = true;
                            //            db.MAIL_T_M.Add(attachFile);
                            //        }
                            //    }
                            //}
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

        public static MAIL_TEMPLATE GetById(int Id)
        {
            try
            {
                var db = new PMSEntities();
                return db.MAIL_TEMPLATE.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
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
                var MAIL_TEMPLATE = db.MAIL_TEMPLATE.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (MAIL_TEMPLATE != null)
                {
                    MAIL_TEMPLATE.IsDeleted = true;
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

        public static List<MailTemplateModel> GetAll()
        {
            try
            {
                var db = new PMSEntities();
                return db.MAIL_TEMPLATE.Where(x => !x.IsDeleted).Select(x => new MailTemplateModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Subject = x.Subject,
                    Content = x.Content,
                    IsActive = x.IsActive,
                    IsActiveStr = x.IsActive ? "Có": "Không",
                    MailSendId = x.MailSendId, 
                    MailReceiveIds = x.MailReceiveIds,
                    MailFileIds = x.MailFileIds,
                    Description = x.Description, 
                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static MAIL_TEMPLATE CheckExists(int Id, string name)
        {
            try
            {
                var db = new PMSEntities();
                return db.MAIL_TEMPLATE.FirstOrDefault(x => !x.IsDeleted && x.Id != Id && x.Name.Trim().ToUpper().Equals(name.Trim().ToUpper()));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
