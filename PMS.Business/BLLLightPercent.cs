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
    public class BLLLightPercent
    {
        static Object key = new object();
        private static PMSEntities db;
        private static volatile BLLLightPercent _Instance;
        public static BLLLightPercent Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLLightPercent();

                return _Instance;
            }
        }

        private BLLLightPercent() { }

        /// <summary>
        /// Get All
        /// </summary>
        /// <param name="isGetKanban"></param>
        /// <returns></returns>
        public List<LightPercentModel> GetAll(int type)
        {
            var list = new List<LightPercentModel>();
            list.Add(new LightPercentModel { Id = 0, Name = "Chọn tỷ lệ đọc thông báo" });
            try
            {
                db = new PMSEntities();
                list.AddRange(db.P_LightPercent.Where(x => !x.IsDeleted && x.Type == type).Select(x => new LightPercentModel()
                {
                    Id = x.Id,
                    Name = x.Name
                }).OrderBy(x => x.Name));

                if (list.Count > 1)
                {
                    var ids = list.Select(x => x.Id).Distinct();
                    var subItems = db.P_LightPercent_De.Where(x => !x.IsDeleted && ids.Contains(x.LightPercentId)).Select(x => new LightPercentDetailModel()
                    {
                        Id = x.Id,
                        ColorName = x.ColorName,
                        From = x.From,
                        To = x.To,
                        LightPercentId = x.LightPercentId
                    }).ToList();
                    if (subItems != null && subItems.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            item.Childs.AddRange(subItems.Where(x => x.LightPercentId == item.Id));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        public ResponseBase InsertOrUpdate(LightPercentModel model)
        {
            var result = new ResponseBase();
            try
            {
                using (var _db = new PMSEntities())
                {
                    P_LightPercent lightObj;
                    if (model.Id == 0)
                    {
                        lightObj = new P_LightPercent();
                        Parse.CopyObject(model, ref lightObj);
                        lightObj.IsDeleted = false;
                        if (model.Childs != null && model.Childs.Count > 0)
                        {
                            lightObj.P_LightPercent_De = new List<P_LightPercent_De>();
                            P_LightPercent_De subItem;
                            foreach (var itemObj in model.Childs)
                            {
                                subItem = new P_LightPercent_De();
                                Parse.CopyObject(itemObj, ref subItem);
                                subItem.IsDeleted = false;
                                subItem.P_LightPercent = lightObj;
                                lightObj.P_LightPercent_De.Add(subItem);
                            }
                        }
                        _db.P_LightPercent.Add(lightObj);
                    }
                    else
                    {
                        lightObj = _db.P_LightPercent.FirstOrDefault(x => x.Id == model.Id);
                        if (lightObj != null)
                        {
                            lightObj.Name = model.Name;
                            lightObj.P_LightPercent_De.ToList().ForEach(x => _db.P_LightPercent_De.Remove(x));
                            lightObj.P_LightPercent_De = new List<P_LightPercent_De>();
                            if (model.Childs.Count > 0)
                            {
                                P_LightPercent_De subItem;
                                foreach (var itemObj in model.Childs)
                                {
                                    subItem = new P_LightPercent_De();
                                    Parse.CopyObject(itemObj, ref subItem);
                                    subItem.IsDeleted = false;
                                    subItem.P_LightPercent = lightObj;
                                    lightObj.P_LightPercent_De.Add(subItem);
                                }
                            }
                        }
                    }
                    _db.SaveChanges();
                    result.IsSuccess = true;
                    result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Lưu thành công." });
                }
            }
            catch (Exception ex)
            { }
            return result;
        }

        public ResponseBase Update(int Id, string name, List<LightPercentDetailModel> items)
        {
            var result = new ResponseBase();
            try
            {
                db = new PMSEntities();
                var obj = db.P_LightPercent.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                    obj.Name = name;

                var olds = db.P_LightPercent_De.Where(x => !x.IsDeleted && x.LightPercentId == Id);
                if (olds != null && olds.Count() > 0)
                {
                    foreach (var item in olds)
                    {
                        item.IsDeleted = true;
                    }
                }

                if (items != null && items.Count > 0)
                {
                    P_LightPercent_De subItem;
                    foreach (var itemObj in items)
                    {
                        subItem = new P_LightPercent_De();
                        Parse.CopyObject(itemObj, ref subItem);
                        subItem.IsDeleted = false;
                        subItem.LightPercentId = Id;
                        db.P_LightPercent_De.Add(subItem);
                    }
                }
                db.SaveChanges();
                result.IsSuccess = true;
                result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Lưu thành công." });
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public ResponseBase Delete(int Id)
        {
            var result = new ResponseBase();
            try
            {
                db = new PMSEntities();
                var obj = db.P_LightPercent.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    if (obj.Chuyens.Where(x => !x.IsDeleted).Count() > 0 || obj.P_ReadPercentOfLine.Where(x => !x.IsDeleted).Count() > 0)
                    {
                        result.IsSuccess = true;
                        result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Tỉ lệ đang được sử dụng không thể xóa được." });
                    }
                    else
                    {
                        obj.IsDeleted = true;
                        db.SaveChanges();
                        result.IsSuccess = true;
                        result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Xóa Thành công." });
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Messages.Add(new Message() { Title = "Lỗi", msg = "không tìm thấy thông tin." });
                }
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Messages.Add(new Message() { Title = "Lỗi", msg = "không tìm thấy thông tin." });
            }
            return result;
        }

        public ResponseBase Update(List<P_ReadPercentOfLine> items)
        {
            var result = new ResponseBase();
            try
            {
                db = new PMSEntities();
                if (items != null && items.Count > 0)
                {
                    var ids = items.Select(x => x.Id).ToArray();
                    var objs = db.P_ReadPercentOfLine.Where(x => !x.IsDeleted && ids.Contains(x.Id));
                    foreach (var itemObj in objs)
                    {
                        var obj = items.FirstOrDefault(x => x.Id == itemObj.Id);
                        if (obj != null)
                            itemObj.ReadPercent_KCSInventoryId = obj.ReadPercent_KCSInventoryId;
                        itemObj.ReadPercent_KCSInventoryId = obj.ReadPercent_KCSInventoryId == 0 ? null : obj.ReadPercent_KCSInventoryId;
                    }
                }
                db.SaveChanges();
                result.IsSuccess = true;
                result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Lưu thành công." });
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public P_ReadPercent_KCSInventory_De Get(int AssignId, int Percent)
        {
            try
            {
                var db = new PMSEntities();
                var obj = db.P_ReadPercentOfLine.FirstOrDefault(x => !x.IsDeleted && x.AssignmentId == AssignId);
                if (obj != null)
                    return db.P_ReadPercent_KCSInventory_De.FirstOrDefault(x => !x.IsDeleted && x.KCSInventoryId == obj.ReadPercent_KCSInventoryId && Percent >= x.From && Percent <= x.To);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public List<ModelSelectItem> GetPhanCongBTPConLai(int[] lineIds)
        {
            using (var db = new PMSEntities())
            {
                return db.Chuyens.Where(x => !x.IsDeleted && lineIds.Contains(x.MaChuyen)).Select(x => new ModelSelectItem() { Id = x.MaChuyen, Name = x.TenChuyen, Value = x.IdTiLeBTPConLai ?? 0 }).ToList();
            }
        }

        public bool SavePhanCongTiLeBTPConLai(List<ModelSelectItem> objs)
        {
            using (var db = new PMSEntities())
            {
                var lines = db.Chuyens.ToList();
                for (int i = 0; i < lines.Count; i++)
                {
                    var found = objs.FirstOrDefault(x => x.Id == lines[i].MaChuyen);
                    if (found != null)
                    {
                        lines[i].IdTiLeBTPConLai = found.Value;
                    }
                }
                db.SaveChanges();
                return true;
            }
        }

    }
}
