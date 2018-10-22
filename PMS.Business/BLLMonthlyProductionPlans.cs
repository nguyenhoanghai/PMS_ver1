using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business
{
    public class BLLMonthlyProductionPlans
    {  
        public static ResponseBase InsertForNewMonth()
       {
           var result = new ResponseBase();
           try
           {
              var db = new PMSEntities();
               var c_sp = db.Chuyen_SanPham.Where(x => !x.IsDelete && !x.IsFinish && !x.IsFinishNow && x.Thang > 0 && x.Nam > 0).ToList();
               if (c_sp != null && c_sp.Count()> 0)
               {
                   // thay đổi lai sl kế hoach trong tháng trước nếu mã hàng vẫn chua finish sau đó chuyển sang tháng mới
                   var stt = c_sp.Select(x => x.STT).Distinct();
                   var preMonth = DateTime.Now.Month - 1;
                   var thisMonth = DateTime.Now.Month;
                   var old_MonthDetail = db.P_MonthlyProductionPlans.Where(x => !x.IsDeleted && stt.Contains(x.STT_C_SP) && x.Month == preMonth && x.Year == DateTime.Now.Year).ToList();
                   if (old_MonthDetail != null && old_MonthDetail.Count() > 0)
                   {
                       foreach (var item in old_MonthDetail)
                       {
                           item.ProductionPlans = item.LK_TH;
                       }
                   }

                   P_MonthlyProductionPlans obj;
                   var new_MonthDetail = db.P_MonthlyProductionPlans.Where(x => !x.IsDeleted && stt.Contains(x.STT_C_SP) && x.Month == thisMonth && x.Year == DateTime.Now.Year).ToList();
                  
                   // tao thong tin cho thang moi
                   if (new_MonthDetail != null && new_MonthDetail.Count() > 0)
                   {
                       // neu co thong tin thang nay roi thi can phai loc trùng
                       foreach (var item in c_sp)
                       {
                           if (new_MonthDetail != null && new_MonthDetail.Count() > 0)
                           {
                               var exists = new_MonthDetail.FirstOrDefault(x => x.STT_C_SP == item.STT);
                               if (exists == null)
                               {
                                   obj = new P_MonthlyProductionPlans();
                                   obj.STT_C_SP = item.STT;
                                   obj.ProductionPlans = item.SanLuongKeHoach - item.LuyKeTH;
                                   obj.Month =thisMonth;
                                   obj.Year = DateTime.Now.Year;
                                   db.P_MonthlyProductionPlans.Add(obj);
                               }
                           } 
                       }
                   }
                   else
                   {
                       // ko co thi them moi
                       foreach (var item in c_sp)
                       { 
                           obj = new P_MonthlyProductionPlans();
                           obj.STT_C_SP = item.STT;
                           obj.ProductionPlans = item.SanLuongKeHoach - item.LuyKeTH;
                           obj.Month = thisMonth;
                           obj.Year = DateTime.Now.Year;
                           db.P_MonthlyProductionPlans.Add(obj);
                       }
                   } 
                   db.SaveChanges();
                   result.IsSuccess = true; 
               }
               else
               {
                   result.IsSuccess = false;
                   result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Không tìm dữ liệu phân hàng cho chuyền. Vui lòng Phân hàng mới cho chuyền" });
               }
           }
           catch (Exception ex)
           {
               result.IsSuccess = false;
               result.Messages.Add(new Message() { Title = "Lỗi Tạo Thông tin Tháng", msg = "Tạo thông tin cho tháng mới từ tháng cũ bị lỗi.\n"+ex.Message });
           }
           return result;
       }

        public static ResponseBase Update(P_MonthlyProductionPlans proObj)
        {
            var result = new ResponseBase();
            try
            {
                var db = new PMSEntities();
                var obj = db.P_MonthlyProductionPlans.FirstOrDefault(x => !x.IsDeleted && x.Id == proObj.Id);
                if (obj != null)
                {
                    obj.LK_TH = proObj.LK_TH;
                    obj.LK_TC = proObj.LK_TC;
                    obj.LK_BTP = proObj.LK_BTP;
                    db.SaveChanges();
                    result.IsSuccess = true;
                }
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Messages.Add(new Message(){ Title="Lỗi", msg="Lỗi không cập nhật được thông tin."});
            }
            return result;
        }

        public static P_MonthlyProductionPlans Find(int sttCSP, int month, int year)
        { 
            try
            {
                var db = new PMSEntities();
                return db.P_MonthlyProductionPlans.Where(x => !x.IsDeleted && x.STT_C_SP == sttCSP ).OrderByDescending(x=>x.Year).ThenByDescending(x=>x.Month).FirstOrDefault();
             }
            catch (Exception)
            { 
            }
            return null;
        }

        public static P_MonthlyProductionPlans Find(int Id)
        {
            try
            {
                var db = new PMSEntities();
                return db.P_MonthlyProductionPlans.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
            }
            catch (Exception)
            {
            }
            return null;
        }
    }
}
