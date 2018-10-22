using GPRO.Ultilities;
using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business
{
    public class BLLCompletionPhase
    {
        /// <summary>
        /// Lấy tất cả thông tin công đoạn hoàn thành
        /// </summary>
        /// <returns></returns>
        public static List<CompletionPhaseModel> GetAll()
        {
            try
            {
                var db = new PMSEntities();
                return db.P_CompletionPhase.Where(x => !x.IsDeleted).Select(x => new CompletionPhaseModel()
                {
                    Id = x.Id,
                    OrderIndex = x.OrderIndex,
                    Code = x.Code,
                    Name = x.Name,
                    Note = x.Note,
                    IsShow = x.IsShow,
                    IsShowStr = x.IsShow ? "Có" : "Không"
                }).ToList();
            }
            catch (Exception)
            { }
            return new List<CompletionPhaseModel>();
        }

        /// <summary>
        /// Insert or Update công đoạn hoàn thành
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ResponseBase InsertOrUpdate(P_CompletionPhase obj)
        {
            var rs = new ResponseBase();
            try
            {
                var db = new PMSEntities();
                P_CompletionPhase newObj;
                rs.IsSuccess = true;
                if (!string.IsNullOrEmpty(obj.Code))
                {
                    if (CheckExists(obj.Id, obj.Code, false, db) != null)
                    {
                        rs.IsSuccess = false;
                        rs.Messages.Add(new Message() { msg = "Mã công đoạn đã tồn vui lòng chọn mã khác.", Title = "Lỗi trùng dữ liệu" });
                    }
                }
                if (rs.IsSuccess)
                {
                    if (CheckExists(obj.Id, obj.Name, true, db) != null)
                    {
                        rs.IsSuccess = false;
                        rs.Messages.Add(new Message() { msg = "Tên công đoạn đã tồn vui lòng chọn mã khác.", Title = "Lỗi trùng dữ liệu" });
                    }
                    else
                    {
                        if (obj.Id == 0)
                        {
                            newObj = new P_CompletionPhase();
                            Parse.CopyObject(obj, ref newObj);
                            db.P_CompletionPhase.Add(newObj);
                            rs.IsSuccess = true;
                        }
                        else
                        {
                            newObj = db.P_CompletionPhase.FirstOrDefault(x => x.Id == obj.Id);
                            if (newObj != null)
                            {
                                newObj.OrderIndex = obj.OrderIndex;
                                newObj.Code = obj.Code;
                                newObj.Name = obj.Name;
                                newObj.Note = obj.Note;
                                newObj.IsShow = obj.IsShow;
                                rs.IsSuccess = true;
                            }
                            else
                            {
                                rs.IsSuccess = false;
                                rs.Messages.Add(new Message() { Title = "Lỗi", msg = "Không tìm thấy thông tin. cập nhật thất bại" });
                            }
                        }
                        if (rs.IsSuccess)
                        {
                            db.SaveChanges();
                            rs.IsSuccess = true;
                            rs.Messages.Add(new Message() { Title = "Thông báo", msg = "Lưu thành công." });
                        }
                    }
                }
            }
            catch (Exception)
            {
                rs.IsSuccess = false;
                rs.Messages.Add(new Message() { Title = "Lỗi", msg = "Không tìm thấy thông tin. cập nhật thất bại" });
            }
            return rs;
        }

        /// <summary>
        /// Ktra trùng
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="text">Thông tin cần ktra</param>
        /// <param name="isCheckName">true : check name else check code</param>
        /// <param name="db"></param>
        /// <returns></returns>
        private static P_CompletionPhase CheckExists(int Id, string text, bool isCheckName, PMSEntities db)
        {
            try
            {
                if (!isCheckName)
                    return db.P_CompletionPhase.Where(x => !x.IsDeleted && x.Id != Id && x.Code.Trim().ToUpper().Equals(text)).FirstOrDefault();
                else
                    return db.P_CompletionPhase.Where(x => !x.IsDeleted && x.Id != Id && x.Name.Trim().ToUpper().Equals(text)).FirstOrDefault();
            }
            catch (Exception)
            {
            }
            return null;
        }

        /// <summary>
        /// Delete công đoạn hoàn thành
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static ResponseBase Delete(int Id)
        {
            var result = new ResponseBase();
            try
            {
                var db = new PMSEntities();
                var obj = db.P_CompletionPhase.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    obj.DeletedDate = DateTime.Now;
                    db.SaveChanges();
                    result.IsSuccess = true;
                    result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Xóa Thành công." });
                }
                else
                {
                    result.IsSuccess = false;
                    result.Messages.Add(new Message() { Title = "Lỗi", msg = "không tìm thấy thông tin cần xóa." });
                }
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Messages.Add(new Message() { Title = "Lỗi", msg = "không tìm thấy thông tin cần xóa. Exception" });
            }
            return result;
        }

        public static List<SelectListItem> GetSelectListItem()
        {
            var rs = new List<SelectListItem>();
            try
            {
                var db = new PMSEntities();
                var list = db.P_CompletionPhase.Where(x => !x.IsDeleted).Select(x => new SelectListItem()
                {
                    Value = x.Id,
                    Text = x.Name
                }).ToList();
                if (list.Count > 0)
                    rs.AddRange(list);
                else
                    rs.Add(new SelectListItem() { Text = "Không có dữ liệu", Value = 0 });
            }
            catch (Exception ex)
            {  }
            return rs;
        }
    }
}
