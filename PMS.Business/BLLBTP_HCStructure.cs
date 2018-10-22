using GPRO.Ultilities;
using PMS.Business.Enum;
using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business
{
    public class BLLBTP_HCStructure
    {
        static Object key = new object();
        private static volatile BLLBTP_HCStructure _Instance;
        public static BLLBTP_HCStructure Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLBTP_HCStructure();

                return _Instance;
            }
        }

        private BLLBTP_HCStructure() { }

        public List<PhaseModel> Gets(int phaseType)
        {
            try
            {
                using (var db = new PMSEntities())
                {
                    return db.P_Phase.Where(x => !x.IsDeleted && x.Type == phaseType).Select(x => new PhaseModel()
                      {
                          Id = x.Id,
                          Index = x.Index,
                          Type = x.Type,
                          Name = x.Name,
                          Note = x.Note,
                          IsShow = x.IsShow
                      }).ToList();
                }
            }
            catch (Exception)
            { }
            return null;
        }
        private P_Phase CheckExists(int Id, string name, PMSEntities db)
        {
            return db.P_Phase.FirstOrDefault(x => !x.IsDeleted && x.Id != Id && x.Name == name);
        }

        public ResponseBase InsertOrUpdate(P_Phase model)
        {
            var rs = new ResponseBase();
            rs.IsSuccess = true;
            try
            {
                using (var db = new PMSEntities())
                {
                    if (CheckExists(model.Id, model.Name, db) != null)
                    {
                        rs.IsSuccess = false;
                        rs.Messages.Add(new Message() { Title = "Lỗi Trùng tên", msg = "Tên đã tồn tại vui lòng chọn Tên khác." });
                    }
                    else
                    {
                        if (model.Id == 0)
                            db.P_Phase.Add(model);
                        else
                        {
                            var obj = db.P_Phase.FirstOrDefault(x => !x.IsDeleted && x.Id == model.Id);
                            if (obj != null)
                            {
                                obj.Index = model.Index;
                                obj.Name = model.Name;
                                obj.IsShow = model.IsShow;
                                obj.Note = model.Note;
                            }
                            else
                            {
                                rs.IsSuccess = false;
                                rs.Messages.Add(new Message() { Title = "Lỗi", msg = "cập nhật thông tin thất bại" });
                            }
                        }
                    }
                    if (rs.IsSuccess)
                    {
                        db.SaveChanges();
                        rs.IsSuccess = true;
                        rs.Messages.Add(new Message() { Title = "Thông Báo", msg = "Lưu thành công." });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rs;
        }

        public ResponseBase Delete(int Id)
        {
            var rs = new ResponseBase();
            try
            {
                using (var db = new PMSEntities())
                {
                    var obj = db.Errors.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                    if (obj != null)
                    {
                        obj.IsDeleted = true;
                        db.SaveChanges();
                        rs.IsSuccess = true;
                        rs.Messages.Add(new Message() { Title = "Thông Báo", msg = "Xóa Thành công." });
                    }
                    else
                    {
                        rs.IsSuccess = false;
                        rs.Messages.Add(new Message() { Title = "Lỗi", msg = "không tìm thấy lỗi." });
                    }
                }
            }
            catch (Exception)
            {
                rs.IsSuccess = false;
                rs.Messages.Add(new Message() { Title = "Lỗi", msg = "không tìm thấy lỗi." });
            }
            return rs;
        }

        public ResponseBase InsertBTPDay(PhaseDailyModel model)
        {
            var rs = new ResponseBase();
            rs.IsSuccess = true;
            try
            {
                using (var db = new PMSEntities())
                {
                    int oldQuantities = 0;
                    var obj = db.P_PhaseDaily.FirstOrDefault(x => x.NangSuatId == model.NangSuatId && x.PhaseId == model.PhaseId);
                    if (obj == null)
                    {
                        obj = new P_PhaseDaily();
                        Parse.CopyObject(model, ref obj);
                        obj.CreatedDate = DateTime.Now;
                        db.P_PhaseDaily.Add(obj);
                    }
                    else
                    {
                        oldQuantities = obj.Quantity;
                        obj.Quantity = model.Quantity;
                    }
                    db.SaveChanges();
                    var total = db.P_Phase_Assign_Log.FirstOrDefault(x => x.PhaseId == model.PhaseId && x.AssignId == model.assignId);
                    if (total == null)
                    {
                        total = new P_Phase_Assign_Log()
                      {
                          Quantity = model.Quantity,
                          AssignId = model.assignId,
                          PhaseId = model.PhaseId,
                          CreatedDate = obj.CreatedDate
                      };
                        db.P_Phase_Assign_Log.Add(total);
                    }
                    else
                    {
                        total.Quantity -= oldQuantities;
                        total.Quantity += model.Quantity;
                    }

                    db.SaveChanges();
                    UpdateLKBTP_HC(model.assignId, db);
                    rs.IsSuccess = true;
                    rs.Messages.Add(new Message() { Title = "Thông Báo", msg = "Lưu thành công." });
                }
            }
            catch (Exception ex)
            { throw ex; }
            return rs;
        }

        private void UpdateLKBTP_HC(int cspId, PMSEntities db)
        {
            try
            {
                int min = 0;
                var allObjs = db.P_Phase_Assign_Log.Where(x => !x.Chuyen_SanPham.IsDelete && !x.Chuyen_SanPham.IsFinish && x.AssignId == cspId && x.P_Phase.Type == (int)ePhaseType.BTP_HC).ToList();
                if (allObjs.Count > 0)
                {
                    var structs = db.P_Phase.Where(x => !x.IsDeleted && x.Type == (int)ePhaseType.BTP_HC).ToList();
                    for (int i = 0; i < structs.Count; i++)
                    {
                        int total = allObjs.Where(x => x.PhaseId == structs[i].Id).Sum(x => x.Quantity);
                        if (i == 0)
                            min = total;
                        else if (total < min)
                            min = total;
                    }
                    var csp = db.Chuyen_SanPham.FirstOrDefault(x => x.STT == cspId);
                    csp.LK_BTP_HC = min;
                    db.SaveChanges();
                }
            }
            catch (Exception)
            { }
        }

        public ResponseBase CountQuantities(int cspId, int structId, string date)
        {
            var rs = new ResponseBase();
            try
            {
                using (var db = new PMSEntities())
                {
                    rs.Data = 0;
                    rs.Records = 0;
                    var dayLog = db.P_PhaseDaily.FirstOrDefault(x => x.NangXuat.STTCHuyen_SanPham == cspId && x.PhaseId == structId && x.NangXuat.Ngay == date);
                    if (dayLog != null)
                        rs.Records = dayLog.Quantity;

                    var allLog = db.P_Phase_Assign_Log.FirstOrDefault(x => x.AssignId == cspId && x.PhaseId == structId);
                    if (allLog != null)
                        rs.Data = allLog.Quantity - rs.Records;
                }
            }
            catch (Exception ex)
            { }
            return rs;
        }

    }
}
