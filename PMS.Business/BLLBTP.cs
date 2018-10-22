using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business
{
    public class BLLBTP
    { 

        //public ResponseBase Delete(int Id)
        //{
        //    var result = new ResponseBase();
        //    try
        //    {
        //        var btps = db.BTPs.Where(x=>!x.IsDeleted && x.STTChuyen_SanPham == stt_Csp);
        //        if (btps != null && btps.Count() > 0)
        //        {
        //            foreach (var item in btps)
        //            {
        //                item.IsDeleted = true;
        //            }                   
        //            db.SaveChanges();
        //            result.IsSuccess = true;
        //            result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Xóa Thành công." });
        //        }
        //        else
        //        {
        //            result.IsSuccess = false;
        //            result.Messages.Add(new Message() { Title = "Lỗi", msg = "không tìm thấy lỗi." });
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        result.IsSuccess = false;
        //        result.Messages.Add(new Message() { Title = "Lỗi", msg = "không tìm thấy lỗi." });
        //    }
        //    return result;
        //}

        private static BTP Find(int Id)
        {
            try
            {
                var db = new PMSEntities();
                return db.BTPs.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static BTP GetBTP(string Ngay, int STTChuyen_SanPham)
        {
            try
            {
                var db = new PMSEntities();
                return db.BTPs.FirstOrDefault(x => !x.IsDeleted && x.Ngay == Ngay && x.STTChuyen_SanPham == STTChuyen_SanPham);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void InsertOrUpdate(BTP obj)
        {
            try
            {
                var db = new PMSEntities();
                if (obj.Id == 0)
                {
                    obj.CreatedDate = DateTime.Now;
                    db.BTPs.Add(obj);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            { 
            }
        }

        public static List<BTP> GetByCommoId(int sttCSP, string ngay)
        {
            try
            {
                var db = new PMSEntities();
                return db.BTPs.Where(x =>!x.IsDeleted && !x.IsBTP_PB_HC && x.Ngay == ngay && x.STTChuyen_SanPham == sttCSP ).ToList();
            }
            catch (Exception)
            {
            }
            return new List<BTP>();
        }
    }
}
