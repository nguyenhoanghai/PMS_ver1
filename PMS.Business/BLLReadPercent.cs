using GPRO.Ultilities;
using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business
{
    public class BLLReadPercent
    {
        static Object key = new object();
        private static PMSEntities db;
        private static volatile BLLReadPercent _Instance;  //volatile =>  tranh dung thread
        public static BLLReadPercent Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLReadPercent();

                return _Instance;
            }
        }

        private BLLReadPercent() { }


        public List<ReadPercentModel> GetAll()
        {
            var list = new List<ReadPercentModel>();
            list.Add(new ReadPercentModel { Id = 0, Name = "Chọn tỷ lệ đọc thông báo" });
            try
            {
                db = new PMSEntities();
                list.AddRange(db.ReadPercents.Where(x => !x.IsDeleted && (x.IdParent == null || x.IdParent == 0)).Select(x => new ReadPercentModel()
                {
                    Id = x.Id,
                    CountRepeat = x.CountRepeat,
                    IdParent = x.IdParent,
                    IsDeleted = x.IsDeleted,
                    Name = x.Name,
                    PercentFrom = x.PercentFrom,
                    PercentTo = x.PercentTo,
                    Sound = x.Sound
                }).OrderBy(x => x.Name));
                if (list.Count > 1)
                {
                    var ids = list.Select(x => x.Id).Distinct();
                    var subItems = db.ReadPercents.Where(x => !x.IsDeleted && x.IdParent != null && ids.Contains(x.IdParent.Value)).ToList();
                    if (subItems != null && subItems.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            item.Childs.AddRange(subItems.Where(x => x.IdParent == item.Id));
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

        public ResponseBase Insert(ReadPercent obj, List<ReadPercent> items)
        {
            var result = new ResponseBase();
            try
            {
                db = new PMSEntities();
                ReadPercent newObj;
                newObj = new ReadPercent();
                Parse.CopyObject(obj, ref newObj);
                obj.IsDeleted = false;
                db.ReadPercents.Add(obj);
                db.SaveChanges();
                if (items != null && items.Count > 0)
                {
                    ReadPercent subItem;
                    foreach (var itemObj in items)
                    {
                        subItem = new ReadPercent();
                        Parse.CopyObject(itemObj, ref subItem);
                        subItem.IsDeleted = false;
                        subItem.IdParent = obj.Id;
                        db.ReadPercents.Add(subItem);
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

        public ResponseBase Update(int Id, List<ReadPercent> items)
        {
            var result = new ResponseBase();
            try
            {
                db = new PMSEntities();
                var olds = db.ReadPercents.Where(x => !x.IsDeleted && x.IdParent != null && x.IdParent.Value == Id);
                if (olds != null && olds.Count() > 0)
                {
                    foreach (var item in olds)
                    {
                        item.IsDeleted = true;
                    }
                }

                if (items != null && items.Count > 0)
                {
                    ReadPercent subItem;
                    foreach (var itemObj in items)
                    {
                        subItem = new ReadPercent();
                        Parse.CopyObject(itemObj, ref subItem);
                        subItem.IsDeleted = false;
                        subItem.IdParent = Id;
                        db.ReadPercents.Add(subItem);
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
                var olds = db.ReadPercents.Where(x => !x.IsDeleted && (x.Id == Id || (x.IdParent != null && x.IdParent.Value == Id)));
                if (olds != null && olds.Count() > 0)
                {
                    foreach (var item in olds)
                    {
                        item.IsDeleted = true;
                    }

                    var readPers = db.P_ReadPercentOfLine.Where(x => !x.IsDeleted && x.ReadPercentId == Id);
                    if (readPers != null && readPers.Count() > 0)
                    {
                        foreach (var item in readPers)
                        {
                            item.ReadPercentId = null;
                        }
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

        public List<ReadPercentOfLineModel> GetAll(int[] LineIds)
        {
            var list = new List<ReadPercentOfLineModel>();
            try
            {
                db = new PMSEntities();
                list.AddRange(db.P_ReadPercentOfLine.Where(x => !x.IsDeleted && LineIds.Contains(x.Chuyen_SanPham.MaChuyen)).Select(x => new ReadPercentOfLineModel()
                {
                    Id = x.Id,
                    LineId = x.LineId,
                    LineName = x.Chuyen.TenChuyen,
                    IsDeleted = x.IsDeleted,
                    AssignmentId = x.AssignmentId,
                    CommoId = x.Chuyen_SanPham.MaSanPham,
                    CommoName = x.Chuyen_SanPham.SanPham.TenSanPham,
                    ReadPercentId = x.ReadPercentId,
                    ReadPercentName = x.ReadPercent.Name,
                    ReadPercent_KCSInventoryId = x.ReadPercent_KCSInventoryId,
                    ReadPercent_KCSInventoryName = x.P_ReadPercent_KCSInventory.Name,
                    KanbanLightPercentId = x.KanbanLightPercentId,
                    KanbanLightReadPercentName = x.P_LightPercent.Name,
                    ProductLightPercentId = x.ProductLightPercentId,
                    ProductLightReadPercentName = x.P_LightPercent.Name

                }).OrderBy(x => x.LineId).ThenBy(x => x.AssignmentId));

                var csp = db.Chuyen_SanPham.Where(x => !x.IsDelete && !x.Chuyen.IsDeleted && !x.SanPham.IsDelete && !x.IsFinish && LineIds.Contains(x.MaChuyen)).ToArray();
                if (csp.Length > 0)
                {
                    var useIds = new List<int>();
                    int count = 0;
                    P_ReadPercentOfLine obj;
                    foreach (var item in csp)
                    {
                        var obj_ = list.FirstOrDefault(x => x.AssignmentId == item.STT);
                        if (obj_ == null)
                        {
                            obj = new P_ReadPercentOfLine();
                            obj.AssignmentId = item.STT;
                            obj.LineId = item.MaChuyen;
                            obj.ReadPercentId = null;
                            obj.ReadPercent_KCSInventoryId = null;
                            obj.AssignmentId = item.STT;
                            db.P_ReadPercentOfLine.Add(obj);
                            count++;
                        }
                        else
                            useIds.Add(obj_.Id);
                    }

                    var removeObjs = db.P_ReadPercentOfLine.Where(x => !x.IsDeleted && !useIds.Contains(x.Id));
                    if (removeObjs != null && removeObjs.Count() > 0)
                    {
                        foreach (var item in removeObjs)
                        {
                            item.IsDeleted = true;
                        }
                    }
                    db.SaveChanges();

                    list.Clear();
                    list.AddRange(db.P_ReadPercentOfLine.Where(x => !x.IsDeleted && LineIds.Contains(x.Chuyen_SanPham.MaChuyen)).Select(x => new ReadPercentOfLineModel()
                    {
                        Id = x.Id,
                        LineId = x.LineId,
                        LineName = x.Chuyen.TenChuyen,
                        IsDeleted = x.IsDeleted,
                        AssignmentId = x.AssignmentId,
                        CommoId = x.Chuyen_SanPham.MaSanPham,
                        CommoName = x.Chuyen_SanPham.SanPham.TenSanPham,
                        ReadPercentId = x.ReadPercentId,
                        ReadPercentName = x.ReadPercent.Name,
                        ReadPercent_KCSInventoryId = x.ReadPercent_KCSInventoryId,
                        ReadPercent_KCSInventoryName = x.P_ReadPercent_KCSInventory.Name,
                        KanbanLightPercentId = x.KanbanLightPercentId,
                        KanbanLightReadPercentName = x.P_LightPercent.Name,
                        ProductLightPercentId = x.ProductLightPercentId,
                        ProductLightReadPercentName = x.P_LightPercent.Name
                    }).OrderBy(x => x.LineId).ThenBy(x => x.AssignmentId));
                }
                else
                    list.Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
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
                            itemObj.ReadPercentId = obj.ReadPercentId;
                        itemObj.ReadPercentId = obj.ReadPercentId == 0 ? null : obj.ReadPercentId;
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


        //  string strSQL = "select CountRepeat, Sound from ReadPercent where PercentFrom <= " + phantram + " and " + phantram + "<= PercentTo and IdParent = '" + IdTyLeDoc + "'";
        //public static List<ReadPercentModel> GetAll()
        //{



        //    //try
        //    //{
        //    //    var db = new PMSEntities();
        //    //   var objs =  db.ReadPercents.Where(x => !x.IsDeleted && x.PercentFrom <= ).Select(x => new ReadPercentModel()
        //    //    {
        //    //        Id = x.Id,
        //    //        CountRepeat = x.CountRepeat,
        //    //        IdParent = x.IdParent,
        //    //        IsDeleted = x.IsDeleted,
        //    //        Name = x.Name,
        //    //        PercentFrom = x.PercentFrom,
        //    //        PercentTo = x.PercentTo,
        //    //        Sound = x.Sound
        //    //    }).OrderBy(x => x.Name) ;
        //    //    if (list.Count > 1)
        //    //    {
        //    //        var ids = list.Select(x => x.Id).Distinct();
        //    //        var subItems = db.ReadPercents.Where(x => !x.IsDeleted && x.IdParent != null && ids.Contains(x.IdParent.Value)).ToList();
        //    //        if (subItems != null && subItems.Count > 0)
        //    //        {
        //    //            foreach (var item in list)
        //    //            {
        //    //                item.Childs.AddRange(subItems.Where(x => x.IdParent == item.Id));
        //    //            }
        //    //        }
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    throw ex;
        //    //}
        //    //return list;
        //}

        public ReadPercent Get(int AssignId, int Percent)
        {
            try
            {
                var db = new PMSEntities();
                var obj = db.P_ReadPercentOfLine.FirstOrDefault(x => !x.IsDeleted && x.AssignmentId == AssignId);
                if (obj != null)
                    return db.ReadPercents.FirstOrDefault(x => !x.IsDeleted && x.IdParent == obj.ReadPercentId &&Percent  >= x.PercentFrom && Percent <= x.PercentTo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        /// <summary>
        /// trả về thong tin doc am thanh dua vao ti le theo phan tram
        /// </summary>
        /// <param name="ReadPercentId"></param>
        /// <param name="percent"></param>
        /// <returns></returns>
        //public ReadPercent Get(int ReadPercentId, int percent)
        //{
        //    db = new PMSEntities();
        //    return db.ReadPercents.FirstOrDefault(x => !x.IsDeleted && x.IdParent == ReadPercentId && x.PercentFrom >= percent && x.PercentTo <= percent);
        //}
    }
}
