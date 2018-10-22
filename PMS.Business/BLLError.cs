using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business
{
    public class BLLError
    {
        public static List<ErrorModel> GetAll()
        {
            try
            {
                var db = new PMSEntities();
                return db.Errors.Where(x => !x.IsDeleted && !x.GroupError.IsDeleted).Select(x => new ErrorModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    Description = x.Description,
                    GroupErrorId = x.GroupErrorId,
                    GroupName = x.GroupError.Name
                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ResponseBase InsertOrUpdate(Error obj)
        {
            var result = new ResponseBase();
            try
            {
                var db = new PMSEntities();
                var isOk = true;
                if (CheckExists(obj.Id, obj.Code) != null)
                {
                    isOk = false;
                    result.IsSuccess = false;
                    result.Messages.Add(new Message() { Title = "Lỗi Trùng Mã", msg = "Mã Lỗi tồn tại vui lòng chọn Mã khác." });
                }
                else
                {
                    if (obj.Id == 0)
                    {
                        db.Errors.Add(obj);
                    }
                    else
                    {
                        var error = db.Errors.FirstOrDefault(x => !x.IsDeleted && x.Id == obj.Id);
                        if (error != null)
                        {
                            error.Code = obj.Code;
                            error.Name = obj.Name;
                            error.Description = obj.Description;
                            error.GroupErrorId = obj.GroupErrorId;
                        }
                        else
                        {
                            isOk = false;
                            result.IsSuccess = false;
                            result.Messages.Add(new Message() { Title = "Lỗi", msg = "cập nhật thông tin thất bại" });
                        }
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
                var error = db.Errors.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
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

        public static Error Find(int value, bool byCode)
        {
            try
            {
                var db = new PMSEntities();
                if (byCode)
                    return db.Errors.FirstOrDefault(x => !x.IsDeleted && x.Code == value);
                return db.Errors.FirstOrDefault(x => !x.IsDeleted && x.Id == value);
            }
            catch (Exception)
            {
            }
            return null;
        }

        public static Error GetById(int Id )
        {
            try
            {
                var db = new PMSEntities(); 
                return db.Errors.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
            }
            catch (Exception)
            {
            }
            return null;
        }


        public static Error CheckExists(int Id, int code)
        {
            var db = new PMSEntities();
            return db.Errors.FirstOrDefault(x => !x.IsDeleted && x.Id != Id && x.Code == code);
        }

        public static List<GroupErrorModel> GetListGroupErrors()
        { 
            try
            {
                var db= new PMSEntities();
                var groups = db.GroupErrors.Where(x => !x.IsDeleted).Select(x=> new GroupErrorModel(){
                Id = x.Id, Name = x.Name, Code = x.Code, Description = x.Description
                }).ToList();
                if (groups.Count > 0)
                {
                    var errors = db.Errors.Where(x => !x.IsDeleted && !x.GroupError.IsDeleted).ToList();
                    foreach (var item in groups)
                    {
                        item.ErrorsObj.AddRange(errors.Where(x => x.GroupErrorId == item.Id)); 
                    }
                    return groups;
                }
            }
            catch (Exception  )
            { 
            }
            return new List<GroupErrorModel>();
        }
    }
}
