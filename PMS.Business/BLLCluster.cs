using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business
{
    public static class BLLCluster
    {
        public static List<ClusterModel> GetAllByLineId(int LineId)
        {
            try
            {
                var db = new PMSEntities();
                return db.Cums.Where(x => !x.IsDeleted && !x.Chuyen.IsDeleted && x.IdChuyen == LineId).Select(x => new ClusterModel()
                {
                    Id = x.Id,
                    IdChuyen = x.IdChuyen,
                    FloorId = x.FloorId,
                    Code = x.Code,
                    IsEndOfLine = x.IsEndOfLine,
                    MoTa = x.MoTa,
                    TenCum = x.TenCum
                }).ToList();
            }
            catch (Exception)
            {
            }
            return null;
        }

        public static ResponseBase InsertOrUpdate(Cum obj)
        {
            var result = new ResponseBase();
            try
            {
                var db = new PMSEntities();
                var check = false;
                if (obj.Id == 0)
                  db.Cums.Add(obj);
                  else
                {
                    var cum = db.Cums.FirstOrDefault(x => !x.IsDeleted && x.Id == obj.Id);
                    if (cum != null)
                    {
                        cum.TenCum = obj.TenCum;
                        cum.IdChuyen = obj.IdChuyen;
                        cum.FloorId = obj.FloorId;
                        cum.MoTa = obj.MoTa;
                        cum.IsEndOfLine = obj.IsEndOfLine;
                        cum.Code = obj.Code;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Messages.Add(new Message() { msg = "Không tìm thấy thông tin Cụm.", Title = "Lỗi" });
                    }
                }
                if (!check)
                {
                    db.SaveChanges();
                    result.IsSuccess = true;
                    result.Messages.Add(new Message() { msg = "Lưu thành công.", Title = "Thông Báo" });
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Messages.Add(new Message() { msg = "Lỗi Ngoại lệ :"+ex.Message, Title = "Lỗi" });
            }
            return result;
        }
    }
}
