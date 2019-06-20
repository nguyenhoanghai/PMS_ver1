using GPRO.Ultilities;
using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PMS.Business
{
    public class BLLDepartmentDailyLabour
    {
        static Object key = new object();
        private static volatile BLLDepartmentDailyLabour _Instance;
        public static BLLDepartmentDailyLabour Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLDepartmentDailyLabour();

                return _Instance;
            }
        }

        private BLLDepartmentDailyLabour() { }

        public List<DepartmentDailyLaboursModel> Gets(string date)
        {
            using (var db = new PMSEntities())
            {
                try
                {
                    return db.P_DepartmentDailyLabour.Where(x => x.Date == date).OrderByDescending(x => x.CreatedAt).Select(x => new DepartmentDailyLaboursModel()
                    {
                        Id = x.Id,
                        DepartmentId = x.DepartmentId,
                        LDCurrent = x.LDCurrent,
                        LDNew = x.LDNew,
                        LDOff = x.LDOff,
                        LDPregnant = x.LDPregnant,
                        LDVacation = x.LDVacation
                    }).ToList();
                }
                catch (Exception)
                {
                }
                return new List<DepartmentDailyLaboursModel>();
            }

        }

        public ResponseBase Delete(int Id)
        {
            var result = new ResponseBase();
            using (var db = new PMSEntities())
            {
                try
                {
                    var commo = db.P_DepartmentDailyLabour.FirstOrDefault(x => x.Id == Id);
                    if (commo != null)
                    {
                        db.P_DepartmentDailyLabour.Remove(commo);
                        db.SaveChanges();
                        result.IsSuccess = true;
                        result.Messages.Add(new Message() { msg = "Xóa Bộ phận thành công.", Title = "Thông Báo" });
                    }
                    else
                    {
                        result.IsSuccess = true;
                        result.Messages.Add(new Message() { msg = "Không tìm thấy thông tin Bộ phận . Xóa Bộ phận thất bại.", Title = "Lỗi CSDL" });
                    }
                }
                catch (Exception)
                {
                    result.IsSuccess = true;
                    result.Messages.Add(new Message() { msg = "Không tìm thấy thông tin Bộ phận . Xóa Bộ phận thất bại.", Title = "Lỗi Exception" });
                }
            }
            return result;
        }

        public ResponseBase InsertOrUpdate(P_DepartmentDailyLabour objModel)
        {
            var rs = new ResponseBase();
            using (var db = new PMSEntities())
            {
                try
                {
                    if (CheckName(objModel, db) != null)
                    {
                        rs.IsSuccess = false;
                        rs.Messages.Add(new Message() { msg = "Bộ phận đã tồn tại. Vui lòng chọn lại Bộ phận khác", Title = "Lỗi trùng tên" });
                    }
                    else
                    {
                        if (objModel.Id == 0)
                        {
                            db.P_DepartmentDailyLabour.Add(objModel);
                            rs.IsSuccess = true;
                        }
                        else
                        {
                            var oldObj = db.P_DepartmentDailyLabour.FirstOrDefault(x => x.Id == objModel.Id);
                            if (oldObj != null)
                            {
                                oldObj.DepartmentId = objModel.DepartmentId;
                                oldObj.LDVacation = objModel.LDVacation;
                                oldObj.LDPregnant = objModel.LDPregnant;
                                oldObj.LDOff = objModel.LDOff;
                                oldObj.LDNew = objModel.LDNew;
                                oldObj.LDCurrent = objModel.LDCurrent;
                                rs.IsSuccess = true;
                            }
                            else
                            {
                                rs.IsSuccess = false;
                                rs.Messages.Add(new Message() { msg = "Bộ phận đang thao tác không tồn tại hoặc đã bị xóa. ", Title = "Lỗi trùng tên" });
                            }
                        }
                        if (rs.IsSuccess)
                        {
                            db.SaveChanges();
                            rs.IsSuccess = true;
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            return rs;
        }

        private P_DepartmentDailyLabour CheckName(P_DepartmentDailyLabour obj, PMSEntities db)
        {
            return db.P_DepartmentDailyLabour.FirstOrDefault(x => x.Id != obj.Id && x.Date == obj.Date && x.DepartmentId == obj.DepartmentId);
        }

        public List<DepartmentDailyLaboursModel> GetsForReport(string date)
        {
            using (var db = new PMSEntities())
            {
                try
                {
                    var objs = db.P_Department.Where(x => !x.IsDeleted).OrderBy(x => x.Id).Select(x => new DepartmentDailyLaboursModel()
                    {
                        DepartmentId = x.Id,
                        DepartmentName = x.Name,
                        BaseLabours = x.BaseLabours
                    }).ToList();

                    if (objs.Count > 0)
                    {
                        var todayObjs = db.P_DepartmentDailyLabour.Where(x => x.Date == date).OrderByDescending(x => x.CreatedAt).Select(x => new DepartmentDailyLaboursModel()
                        {
                            Id = x.Id,
                            DepartmentId = x.DepartmentId,
                            LDCurrent = x.LDCurrent,
                            LDNew = x.LDNew,
                            LDOff = x.LDOff,
                            LDPregnant = x.LDPregnant,
                            LDVacation = x.LDVacation
                        }).ToList();
                        if (todayObjs.Count > 0)
                        {
                            foreach (var item in objs)
                            {
                                var detail = todayObjs.FirstOrDefault(x => x.DepartmentId == item.DepartmentId);
                                if (detail != null)
                                {
                                    item.LDCurrent = detail.LDCurrent;
                                    item.LDNew = detail.LDNew;
                                    item.LDOff = detail.LDOff;
                                    item.LDPregnant = detail.LDPregnant;
                                    item.LDVacation = detail.LDVacation;
                                }
                            }
                        }
                    }
                    return objs;
                }
                catch (Exception)
                {
                }
                return new List<DepartmentDailyLaboursModel>();
            }

        }

        public void AutoSetFromYesterday()
        {
            using (var db = new PMSEntities())
            {
                try
                {
                    var latetestWork = BLLProductivity_.Instance.GetLatestWork();
                    if (latetestWork != null)
                    {
                        string date = latetestWork.CreatedDate.ToString("dd/MM/yyyy");
                        var yesterdayInfos = db.P_DepartmentDailyLabour.Where(x => x.Date == date).OrderByDescending(x => x.CreatedAt).Select(x => new DepartmentDailyLaboursModel()
                        {
                            Id = x.Id,
                            DepartmentId = x.DepartmentId,
                            LDCurrent = x.LDCurrent,
                            LDNew = x.LDNew,
                            LDOff = x.LDOff,
                            LDPregnant = x.LDPregnant,
                            LDVacation = x.LDVacation
                        }).ToList();

                        if (yesterdayInfos.Count > 0)
                        {
                            P_DepartmentDailyLabour departmentDailyLabour;
                            DateTime now = DateTime.Now;
                            string todayStr = now.ToString("dd/MM/yyyy");
                            var departs = db.P_DepartmentDailyLabour.Where(x => x.Date == todayStr).Select(x => x.DepartmentId).ToArray();
                            yesterdayInfos = yesterdayInfos.Where(x => !departs.Contains(x.DepartmentId)).ToList();

                            foreach (var item in yesterdayInfos)
                            {
                                departmentDailyLabour = new P_DepartmentDailyLabour();
                                Parse.CopyObject(item, ref departmentDailyLabour);
                                departmentDailyLabour.Date = todayStr;
                                departmentDailyLabour.CreatedAt = now;
                                db.P_DepartmentDailyLabour.Add(departmentDailyLabour);
                            }
                            db.SaveChanges();
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }
    }

}

