using GPRO.Ultilities;
using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business
{
    public class BLLReadPercent_KCSInventory
    {
        static Object key = new object();
        private static PMSEntities db;
        private static volatile BLLReadPercent_KCSInventory _Instance;  //volatile =>  tranh dung thread
        public static BLLReadPercent_KCSInventory Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLReadPercent_KCSInventory();

                return _Instance;
            }
        }

        private BLLReadPercent_KCSInventory() { }

        public List<ReadPercentKCSInventoryModel> GetAll()
        {
            var list = new List<ReadPercentKCSInventoryModel>();
            list.Add(new ReadPercentKCSInventoryModel { Id = 0, Name = "Chọn tỷ lệ đọc thông báo" });
            try
            {
                db = new PMSEntities();
                list.AddRange(db.P_ReadPercent_KCSInventory.Where(x => !x.IsDeleted).Select(x => new ReadPercentKCSInventoryModel()
                {
                    Id = x.Id,
                    IsDeleted = x.IsDeleted,
                    Name = x.Name
                }).OrderBy(x => x.Name));

                if (list.Count > 1)
                {
                    var ids = list.Select(x => x.Id).Distinct();
                    var subItems = db.P_ReadPercent_KCSInventory_De.Where(x => !x.IsDeleted && ids.Contains(x.KCSInventoryId)).ToList();
                    if (subItems != null && subItems.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            item.Childs.AddRange(subItems.Where(x => x.KCSInventoryId == item.Id));
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

        public ResponseBase Insert(ReadPercentKCSInventoryModel obj)
        {
            var result = new ResponseBase();
            try
            {
                db = new PMSEntities();
                P_ReadPercent_KCSInventory newObj;
                newObj = new P_ReadPercent_KCSInventory();
                Parse.CopyObject(obj, ref newObj);
                newObj.IsDeleted = false;

                if (obj.Childs != null && obj.Childs.Count > 0)
                {
                    newObj.P_ReadPercent_KCSInventory_De = new List<P_ReadPercent_KCSInventory_De>();
                    P_ReadPercent_KCSInventory_De subItem;
                    foreach (var itemObj in obj.Childs)
                    {
                        subItem = new P_ReadPercent_KCSInventory_De();
                        Parse.CopyObject(itemObj, ref subItem);
                        subItem.IsDeleted = false;
                        subItem.P_ReadPercent_KCSInventory = newObj;
                        newObj.P_ReadPercent_KCSInventory_De.Add(subItem);
                    }
                }
                db.P_ReadPercent_KCSInventory.Add(newObj);
                db.SaveChanges();
                result.IsSuccess = true;
                result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Lưu thành công." });
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public ResponseBase Update(int Id,string name, List<P_ReadPercent_KCSInventory_De> items)
        {
            var result = new ResponseBase();
            try
            {
                db = new PMSEntities();
                var obj = db.P_ReadPercent_KCSInventory.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                    obj.Name = name;

                var olds = db.P_ReadPercent_KCSInventory_De.Where(x => !x.IsDeleted && x.KCSInventoryId == Id);
                if (olds != null && olds.Count() > 0)
                {
                    foreach (var item in olds)
                    {
                        item.IsDeleted = true;
                    }
                }

                if (items != null && items.Count > 0)
                {
                    P_ReadPercent_KCSInventory_De subItem;
                    foreach (var itemObj in items)
                    {
                        subItem = new P_ReadPercent_KCSInventory_De();
                        Parse.CopyObject(itemObj, ref subItem);
                        subItem.IsDeleted = false;
                        subItem.KCSInventoryId = Id;
                        db.P_ReadPercent_KCSInventory_De.Add(subItem);
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
                var olds = db.ReadPercents.Where(x => !x.IsDeleted && x.Id == Id || (x.IdParent != null && x.IdParent.Value == Id));
                if (olds != null && olds.Count() > 0)
                {
                    foreach (var item in olds)
                    {
                        item.IsDeleted = true;
                    }
                    db.SaveChanges();
                    result.IsSuccess = true;
                    result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Xóa Thành công." });
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

        /// <summary>
        /// update
        /// </summary>
        /// <param name="items"></param>
        /// <param name="type">1=> KCSInventory ; 2=> Light Kanban ; 3 => production light</param>
        /// <returns></returns>
        public ResponseBase Update(List<P_ReadPercentOfLine> items, int type)
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
                        switch (type)
                        {
                            case 1: itemObj.ReadPercent_KCSInventoryId = ((obj == null || obj.ReadPercent_KCSInventoryId == 0) ? null : obj.ReadPercent_KCSInventoryId);      break;
                            case 2: itemObj.KanbanLightPercentId = ((obj == null || obj.KanbanLightPercentId == 0) ? null : obj.KanbanLightPercentId); break;
                            case 3: itemObj.ProductLightPercentId = ((obj == null || obj.ProductLightPercentId == 0) ? null : obj.ProductLightPercentId); break;
                        }
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

    }
}
