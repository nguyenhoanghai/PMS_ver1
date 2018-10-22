using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business
{
    public class BLLAssignCompletion
    {
        /// <summary>
        /// Get All Assignment Completion phase including finish obj
        /// </summary>
        /// <param name="WithoutFinish"> có lấy finish hay ko ?</param>
        /// <returns></returns>
        public static List<AssignCompletionModel> GetAll(bool WithoutFinish)
        {
            try
            {
                var db = new PMSEntities();
                if (WithoutFinish)
                {
                    return db.P_AssignCompletion.Where(x => !x.IsDeleted && !x.SanPham.IsDelete).OrderBy(x => x.IsFinish).Select(x => new AssignCompletionModel()
                {
                    Id = x.Id,
                    OrderIndex = x.OrderIndex,
                    CommoId = x.CommoId,
                    CommoName = x.SanPham.TenSanPham,
                    ProductionsPlan = x.ProductionsPlan,
                    CreatedDate = x.CreatedDate,
                    IsFinish = x.IsFinish,
                    IsFinishStr = x.IsFinish ? "kết thúc" : "Đang thực hiện"
                }).ToList();
                }
                else
                {
                    return db.P_AssignCompletion.Where(x => !x.IsDeleted && !x.SanPham.IsDelete && !x.IsFinish).OrderBy(x => x.IsFinish).Select(x => new AssignCompletionModel()
                    {
                        Id = x.Id,
                        OrderIndex = x.OrderIndex,
                        CommoId = x.CommoId,
                        CommoName = x.SanPham.TenSanPham,
                        ProductionsPlan = x.ProductionsPlan,
                        CreatedDate = x.CreatedDate,
                        IsFinish = x.IsFinish,
                        IsFinishStr = x.IsFinish ? "kết thúc" : "Đang thực hiện"
                    }).ToList();
                }                
            }
            catch (Exception ex)
            { }
            return new List<AssignCompletionModel>();
        } 

        /// <summary>
        /// Delete Phân hàng sản xuất
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static ResponseBase Delete(int Id)
        {
            var result = new ResponseBase();
            try
            {
                var db = new PMSEntities();
                //var btps = db.BTPs.Where(x => !x.IsDeleted && x.STTChuyen_SanPham == stt);
                //if (btps != null && btps.Count() > 0)
                //{
                //    foreach (var item in btps)
                //    {
                //        item.IsDeleted = true;
                //    }
                //}

                var csp = db.P_AssignCompletion.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (csp != null)
                {
                    csp.IsDeleted = true;
                    csp.DeletedDate = DateTime.Now;
                    db.SaveChanges();
                    result.IsSuccess = true;
                    result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Xóa Thành công." });
                }
                else
                {
                    result.IsSuccess = false;
                    result.Messages.Add(new Message() { Title = "Lỗi", msg = "không tìm thấy thông tin Phân công." });
                }
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Messages.Add(new Message() { Title = "Lỗi", msg = "không tìm thấy thông tin Phân công.Exception" });
            }
            return result;
        }

        /// <summary>
        /// Lấy thông tin mặt hàng đã dc phân công sản xuất trước đó và đã kết thúc
        /// </summary>
        /// <param name="CommoId">mã mặt hàng</param>
        /// <param name="IsGetFinish">lấy mã hàng kết thúc hay còn đang sản xuất</param>
        /// <returns></returns>
        public static P_AssignCompletion GetAssignByCommoId(int CommoId, bool IsGetFinish)
        {
            try
            {
                var db = new PMSEntities();
                if (IsGetFinish)
                    return db.P_AssignCompletion.FirstOrDefault(x => !x.IsDeleted && x.IsFinish && x.CommoId == CommoId);
                else
                    return db.P_AssignCompletion.FirstOrDefault(x => !x.IsDeleted && !x.IsFinish && x.CommoId == CommoId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update Phân hàng hoàn tất
        /// </summary>
        /// <param name="id"></param> 
        /// <param name="isfinish">kết thúc ?</param>
        /// <param name="productionPlans">Sản lượng kế hoạch</param>
        /// <returns></returns>
        public static ResponseBase Update(P_AssignCompletion csp)
        {
            var result = new ResponseBase();
            try
            {
                var db = new PMSEntities();
                var obj = db.P_AssignCompletion.FirstOrDefault(x => !x.IsDeleted && x.Id == csp.Id); ;
                if (obj != null)
                {
                    obj.ProductionsPlan = csp.ProductionsPlan;
                    obj.OrderIndex = csp.OrderIndex;
                    obj.IsFinish = csp.IsFinish;
                    obj.CommoId = csp.CommoId;
                    obj.UpdatedDate = DateTime.Now;
                    if (obj.IsFinish)
                        obj.FinishedDate = DateTime.Now;
                    db.SaveChanges();
                    result.IsSuccess = true;
                    result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Lưu Phân công thành công." });
                }
                else
                {
                    result.IsSuccess = false;
                    result.Messages.Add(new Message() { Title = "Lỗi Lưu Phân Công", msg = "không tìm thấy Phân Công.\n" });
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Messages.Add(new Message() { Title = "Lỗi Lưu Phân Công", msg = "Lưu Phân công cho Chuyền bị lỗi.\n" + ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Insert phân công hoàn tất
        /// </summary>
        /// <param name="csp"></param>
        /// <returns></returns>
        public static ResponseBase Insert(P_AssignCompletion csp)
        {
            var result = new ResponseBase();
            try
            {
                var db = new PMSEntities();
                db.P_AssignCompletion.Add(csp);
                db.SaveChanges();
                result.IsSuccess = true;
                result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Lưu Phân công thành công." });
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Messages.Add(new Message() { Title = "Lỗi Lưu Phân Công", msg = "Lưu Phân công cho Chuyền bị lỗi.\n" + ex.Message });
            }
            return result;
        }
    }
}
