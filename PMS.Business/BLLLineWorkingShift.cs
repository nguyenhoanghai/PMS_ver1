using GPRO.Ultilities;
using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business
{
    public static class BLLLineWorkingShift
    {
        public static List<LineWorkingShiftModel> GetWorkingShiftOfLine(int lineId)
        {
            try
            {
                var db = new PMSEntities();
                return db.P_LineWorkingShift.Where(x => !x.P_WorkingShift.IsDeleted && !x.Chuyen.IsDeleted && !x.IsDeleted && x.LineId == lineId).Select(x => new LineWorkingShiftModel()
                {
                    Id = x.Id,
                    LineId = x.LineId,
                    LineName = x.Chuyen.TenChuyen,
                    ShiftId = x.ShiftId,
                    ShiftName = x.P_WorkingShift.Name,
                    Start = x.P_WorkingShift.TimeStart,
                    End = x.P_WorkingShift.TimeEnd ,
                    ShiftOrder = x.ShiftOrder
                }).ToList();
            }
            catch (Exception ex)
            { }
            return null;
        }

        public static ResponseBase InsertOrUpdate(LineWorkingShiftModel obj)
        {
            var result = new ResponseBase();
            try
            {
                var db = new PMSEntities();
                P_LineWorkingShift lineShift;

                lineShift = db.P_LineWorkingShift.FirstOrDefault(x => !x.IsDeleted && x.LineId == obj.LineId && x.ShiftId == obj.ShiftId && x.Id != obj.Id);
                if (lineShift != null)
                {
                    result.IsSuccess = false;
                    result.Messages.Add(new Message() { Title = "Trùng ca Làm Việc", msg = "Chuyển :'" + obj.LineName + "' đã có ca làm việc này rồi. Vui lòng chọn ca làm việc khác." });
                }
                else
                {
                    lineShift = db.P_LineWorkingShift.FirstOrDefault(x => !x.IsDeleted && x.LineId == obj.LineId && x.ShiftOrder == obj.ShiftOrder && x.Id != obj.Id);
                    if (lineShift != null)
                    {
                        result.IsSuccess = false;
                        result.Messages.Add(new Message() { Title = "Trùng số thứ tự Ca", msg = "Chuyển :'" + obj.LineName + "' đã có số thứ tự ca làm việc này rồi. Vui lòng chọn số thứ tự ca làm việc khác." });
                    }
                    else
                    {
                        var check = false;
                        if (obj.Id == 0)
                        {
                            lineShift = new P_LineWorkingShift();
                            Parse.CopyObject(obj, ref lineShift);
                            db.P_LineWorkingShift.Add(lineShift);
                            check = true;
                        }
                        else
                        {
                            lineShift = db.P_LineWorkingShift.FirstOrDefault(x => !x.IsDeleted && x.Id == obj.Id);
                            if (lineShift != null)
                            {
                                check = true;
                                lineShift.LineId = obj.LineId;
                                lineShift.ShiftOrder = obj.ShiftOrder;
                                lineShift.ShiftId = obj.ShiftId;
                            }
                            else
                            {
                                result.IsSuccess = false;
                                result.Messages.Add(new Message() { Title = "Lỗi", msg = "Không tìm thấy thông tin." });
                                check = false;
                            }
                        }

                        if (check)
                        {
                            db.SaveChanges();
                            result.IsSuccess = true;
                            result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Lưu thành công." });
                        }
                    }
                }
            }
            catch (Exception)
            { }
            return result;
        }

        public static ResponseBase Delete(int Id)
        {
            var result = new ResponseBase();
            try
            {
                var db = new PMSEntities();
                var lineShift = db.P_LineWorkingShift.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (lineShift != null)
                {
                    lineShift.IsDeleted = true;
                    db.SaveChanges();
                    result.IsSuccess = true;
                    result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Xóa thành công." });
                }
                else
                {
                    result.IsSuccess = false;
                    result.Messages.Add(new Message() { Title = "Lỗi", msg = "Không tìm thấy thông tin." });
                }
            }
            catch (Exception)
            { }
            return result;
        }
    }
}
