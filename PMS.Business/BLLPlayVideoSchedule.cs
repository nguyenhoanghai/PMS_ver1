using GPRO.Ultilities;
using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business
{
    public class BLLPlayVideoSchedule
    {
        public static List<VideoScheduleModel> Gets(int LineId)
        {
            try
            {
                var db = new PMSEntities();
                var objs = db.P_PlayVideoShedule.Where(x => !x.IsDeleted && x.LineId == LineId).Select(x => new VideoScheduleModel()
                {
                    Id = x.Id,
                    TimeStart = x.TimeStart,
                    TimeEnd = x.TimeEnd,
                    LineId = x.LineId,
                    IsActive = x.IsActive
                }).ToList();
                if (objs.Count > 0)
                {
                    var ids = objs.Select(x => x.Id);
                    var detailObjs = db.P_PlayVideoSheduleDetail.Where(x => !x.IsDeleted && !x.P_VideoLibrary.IsDeleted && ids.Contains(x.VideoSheduleId)).ToList();
                    if (detailObjs.Count > 0)
                    {
                        foreach (var item in objs)
                        {
                            item.Detail.AddRange(detailObjs.Where(x => x.VideoSheduleId == item.Id));
                        }
                    }
                }
                return objs;
            }
            catch (Exception)
            {
            }
            return new List<VideoScheduleModel>();
        }

        public static ResponseBase CreateOrUpdate(VideoScheduleModel objModel)
        {
            var result = new ResponseBase();
            var flag = true;
            P_PlayVideoShedule pObj;
            P_PlayVideoSheduleDetail pObjDetail;
            try
            {
                var db = new PMSEntities();
                if (objModel.Id == 0)
                {
                    pObj = new P_PlayVideoShedule();
                    Parse.CopyObject(objModel, ref pObj);
                    if (objModel.Detail.Count > 0)
                    {
                        pObj.P_PlayVideoSheduleDetail = new List<P_PlayVideoSheduleDetail>();
                        foreach (var item in objModel.Detail)
                        {
                            pObjDetail = new P_PlayVideoSheduleDetail();
                            Parse.CopyObject(item, ref pObjDetail);
                            pObjDetail.P_PlayVideoShedule = pObj;
                            pObj.P_PlayVideoSheduleDetail.Add(pObjDetail);
                        }
                    }
                    db.P_PlayVideoShedule.Add(pObj);
                }
                else
                {
                    pObj = db.P_PlayVideoShedule.FirstOrDefault(x => !x.IsDeleted && x.Id == objModel.Id);
                    if (pObj != null)
                    {
                        pObj.TimeStart = objModel.TimeStart;
                        pObj.TimeEnd = objModel.TimeEnd;
                        pObj.IsActive = objModel.IsActive;

                        var oldDetails = db.P_PlayVideoSheduleDetail.Where(x => !x.IsDeleted && !x.P_VideoLibrary.IsDeleted && x.VideoSheduleId == objModel.Id).ToList();
                        if (oldDetails.Count > 0 && objModel.Detail.Count > 0)
                        {
                            foreach (var item in oldDetails)
                            {
                                var obj = objModel.Detail.FirstOrDefault(x => x.VideoId == item.VideoId);
                                if (obj != null)
                                {
                                    item.OrderIndex = obj.OrderIndex;
                                    objModel.Detail.Remove(obj);
                                }
                                else
                                    item.IsDeleted = true;
                            }
                            if (objModel.Detail.Count > 0)
                            {
                                foreach (var item in objModel.Detail)
                                {
                                    pObjDetail = new P_PlayVideoSheduleDetail();
                                    Parse.CopyObject(item, ref pObjDetail);
                                    pObjDetail.VideoSheduleId = pObj.Id;
                                    db.P_PlayVideoSheduleDetail.Add(pObjDetail);
                                }
                            }
                        }
                        else if (oldDetails.Count > 0 && objModel.Detail.Count == 0)
                        {
                            foreach (var item in oldDetails)
                            {
                                item.IsDeleted = true;
                            }
                        }
                        else if (oldDetails.Count == 0 && objModel.Detail.Count > 0)
                        {
                            foreach (var item in objModel.Detail)
                            {
                                pObjDetail = new P_PlayVideoSheduleDetail();
                                Parse.CopyObject(item, ref pObjDetail);
                                pObjDetail.VideoSheduleId = pObj.Id;
                                db.P_PlayVideoSheduleDetail.Add(pObjDetail);
                            }
                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Không tìm thấy thông tin tệp bạn đang thao tác." });
                    }
                }
                if (flag)
                {
                    db.SaveChanges();
                    result.IsSuccess = true;
                    result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Lưu thành công." });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static ResponseBase Delete(int Id)
        {
            var result = new ResponseBase();
            try
            {
                var db = new PMSEntities();
                var obj = db.P_PlayVideoShedule.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (obj != null)
                {
                    obj.IsDeleted = true;

                    var oldDetails = db.P_PlayVideoSheduleDetail.Where(x => !x.IsDeleted && !x.P_VideoLibrary.IsDeleted && x.VideoSheduleId == Id).ToList();
                    if (oldDetails.Count > 0)
                    {
                        foreach (var item in oldDetails)
                        {
                            item.IsDeleted = true;
                        }
                    }
                    db.SaveChanges();
                    result.IsSuccess = true;
                    result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Xóa thành công." });
                }
                else
                {
                    result.IsSuccess = false;
                    result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Không tìm thấy thông tin tệp bạn đang thao tác." });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

 
    }
}
