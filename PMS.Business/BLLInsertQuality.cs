using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMS.Data;
using PMS.Business.Enum;
using PMS.Business.Models;

namespace PMS.Business
{
    public class BLLInsertQuality
    {
        public static List<CompletionPhase_DailyModel> GetDetailInDay(string date, int commoId)
        {
            try
            {
                var db = new PMSEntities();
                return db.P_CompletionPhase_Daily.Where(x => !x.IsDeleted && x.Date == date && x.P_AssignCompletion.CommoId == commoId).Select(x => new CompletionPhase_DailyModel()
                {
                    Id = x.Id,
                    AssignId = x.AssignId,
                    CommandTypeId = x.CommandTypeId,
                    CommoName = x.P_AssignCompletion.SanPham.TenSanPham,
                    CompletionPhaseId = x.CompletionPhaseId,
                    PhaseName = x.P_CompletionPhase.Name,
                    Quantity = x.Quantity,
                    Date = x.Date,
                    CreatedDate = x.CreatedDate,
                    //Time = x.CreatedDate.TimeOfDay,
                    TypeName = x.CommandTypeId == (int)eCommandRecive.ProductIncrease ? "Tăng" : "Giảm"
                }).OrderByDescending(x=>x.CreatedDate).ToList();
            }
            catch (Exception ex)
            {
            }
            return new List<CompletionPhase_DailyModel>();
        }

        public static ResponseBase Insert(P_CompletionPhase_Daily obj)
        {
            var rs = new ResponseBase();
            try
            {
                var db = new PMSEntities();
                var assig = db.P_AssignCompletion.FirstOrDefault(x => !x.IsDeleted && x.Id == obj.AssignId);
                if (assig != null && !assig.IsFinish)
                {
                    db.P_CompletionPhase_Daily.Add(obj);
                    db.SaveChanges();
                    rs.IsSuccess = true;
                }
                else if (assig == null)
                {
                    rs.IsSuccess = false;
                    rs.Messages.Add(new Message() { msg = "Phân công Hàng này đã bị xóa hoặc không tồn tại. Vui lòng kiểm tra lại", Title = "Lỗi" });
                }
                else if (assig != null && assig.IsFinish)
                {
                    rs.IsSuccess = false;
                    rs.Messages.Add(new Message() { msg = "Phân công Hàng này đã kết thúc không thể thêm sản lượng được", Title = "Lỗi" });
                }
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Messages.Add(new Message() { msg = "Lỗi ngoại lệ thêm sản lượng thất bại.", Title = "Lỗi Exception" });
            }
            return rs;
        }
    }
}
