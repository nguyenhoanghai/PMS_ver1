using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMS.Data;
using PMS.Business.Models;
using GPRO.Ultilities;

namespace PMS.Business
{
    public class BLLSound
    {
        public static ResponseBase CreateOrUpdate(SOUND soundObj)
        {
            var result = new ResponseBase();
            var flag = true;
            try
            {
                var db = new PMSEntities();
                if (soundObj.Id == 0)
                {
                    db.SOUNDs.Add(soundObj);
                }
                else
                {
                    var sound = db.SOUNDs.FirstOrDefault(x => !x.IsDeleted && x.Id == soundObj.Id);
                    if (sound != null)
                    {
                        sound.Code = soundObj.Code;
                        sound.Name = soundObj.Name;
                        sound.IsActive = soundObj.IsActive;
                        sound.Path = soundObj.Path;
                        sound.Description = soundObj.Description;
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

        public static SOUND GetById(int Id)
        {
            try
            {
                var db = new PMSEntities();
                return db.SOUNDs.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ResponseBase Delete(int Id)
        {
            var result = new ResponseBase();
            try
            {
                var db = new PMSEntities();
                var sound = db.SOUNDs.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
                if (sound != null)
                {
                    sound.IsDeleted = true;
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

        public static List<SOUND> Gets()
        {
            try
            {
                var db = new PMSEntities();
                return db.SOUNDs.Where(x => !x.IsDeleted).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<SOUND_TimeConfig> GetListTimeByConfigType(int configType)
        {
            try
            {
                var db = new PMSEntities();
                return db.SOUND_TimeConfig.Where(x => !x.IsDeleted && x.IsActive && x.ConfigType == configType).ToList();
            }
            catch (Exception)
            {
            }
            return null;
        }

        public static List<ModelSoundItem> GetListSoundItemByChuyen(int idChuyen, int configType)
        {
            var returnList = new List<ModelSoundItem>();
            try
            {
                var db = new PMSEntities();
                var cfs = db.SOUND_ReadConfigDetail.Where(x => !x.IsDeleted && !x.SOUND_ReadConfig.IsDeleted && x.IsActive && x.SOUND_ReadConfig.IsActive && x.SOUND_ReadConfig.IdChuyen == idChuyen && x.SOUND_ReadConfig.ConfigType == configType).OrderBy(x => x.OrderIndex).ToList();
                if (cfs != null && cfs.Count > 0)
                {
                    var soundInts = db.SOUND_IntConfig.Where(x => !x.IsDeleted && x.IsActive).ToList();
                    var sounds = db.SOUNDs.ToList();                  
                    foreach (var item in cfs)
                    {  
                        var soundItem = new ModelSoundItem();
                        soundItem.FileType = item.IntType;
                        if (item.IntType == 0)
                        {
                            var intCF = soundInts.FirstOrDefault(x => x.Id == item.IdIntConfig);
                            if (intCF != null)
                            {
                                soundItem.Formula = intCF.Formula;
                                soundItem.IsProductivity = intCF.IsProductivity;
                            }
                        }
                        else
                        {
                            var s = sounds.FirstOrDefault(x => x.Id == item.IdSound);
                            soundItem.SoundPath = s != null ? s.Path : string.Empty;
                        }
                        returnList.Add(soundItem);
                    }
                }
            }
            catch (Exception)
            { }
            return returnList;
        }

        public static List<ModelSelectItem> GetLinesHaveReadSoundConfig()
        { 
            using (var db = new PMSEntities())
            {
                return (from x in db.SOUND_ReadConfig
                        where 
                        x.IsActive &&
                        !x.IsDeleted &&
                        x.SOUND_ReadConfigDetail.Count > 0
                        select new ModelSelectItem
                        {
                            Id = x.IdChuyen,
                            Data = x.Id,
                            Name =x.Name +"( " + x.Chuyen.TenChuyen + " )"
                        }).OrderBy(x=>x.Id).ToList();
            } 
        }

        public static bool CopyReadSoundConfig(int lineId, int copyReadConfigId)
        {
            using (var db = new PMSEntities())
            { 
                var copySource =  (from x in db.SOUND_ReadConfig
                        where x.Id == copyReadConfigId
                        select x).FirstOrDefault();
                if (copySource != null)
                {
                    var newObj = new SOUND_ReadConfig();
                    Parse.CopyObject(copySource, ref newObj);
                    newObj.Name = newObj.Name + "(copy)";
                    newObj.IdChuyen = lineId;
                    newObj.Chuyen = null;
                    newObj.SOUND_ReadConfigDetail = new List<SOUND_ReadConfigDetail>();
                    foreach (var item in copySource.SOUND_ReadConfigDetail)
                    {
                        var detail = new SOUND_ReadConfigDetail();
                        Parse.CopyObject(item, ref detail);
                        detail.IdReadConfig = 0;
                        detail.SOUND_ReadConfig = newObj;
                        newObj.SOUND_ReadConfigDetail.Add(detail);
                    }
                    db.SOUND_ReadConfig.Add(newObj);
                    db.SaveChanges();
                    return true;
                }
                else
                    return false;
            } 
        }
    }
}
