using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business
{
    public static class BLLKeyPad
    {
        public static List<KeypadModel> GetListKeyPadLineConfig(int floorId)
        {
            try
            {
                var db = new PMSEntities();
                var keypads = db.KeyPads.Where(x => !x.IsDeleted && !x.Floor.IsDeleted && x.FloorId == floorId).Select(x => new KeypadModel()
                {
                    Id = x.Id,
                    EquipmentId = x.EquipmentId,
                    Description = x.Description,
                    FloorId = x.FloorId,
                    KeyPadName = x.KeyPadName,
                    UseTypeId = x.UseTypeId
                }).ToList();

                if (keypads != null && keypads.Count > 0)
                {
                    var ids = keypads.Select(x => x.Id);
                    var objs = db.KeyPad_Object.Where(x => !x.IsDeleted && !x.Cum.IsDeleted && !x.Cum.Chuyen.IsDeleted).Select(x => new KeypadObjectModel()
                    {
                        Id = x.Id,
                        ClusterId = x.ClusterId,
                        CommandTypeId = x.CommandTypeId,
                        Description = x.Description,
                        IsEndOfLine = x.Cum.IsEndOfLine,
                        KeyPadId = x.KeyPadId,
                        LineId = x.Cum.IdChuyen,
                        STTNut = x.STTNut,
                        LineSound = x.Cum.Chuyen.Sound,
                        
                        
                    }).ToList();
                    if (objs != null && objs.Count > 0)
                    {
                        foreach (var item in keypads)
                        {
                            item.objs.AddRange(objs.Where(x => x.KeyPadId == item.Id));
                        }
                    }
                    return keypads;
                }
            }
            catch (Exception)
            { }
            return new List<KeypadModel>();
        }

        public static List<KeypadModel> GetKeyPadInfoByLineId(int maChuyen)
        {
            try
            {
                var db = new PMSEntities();
                var clusters = BLLCluster.GetAllByLineId(maChuyen);
                if (clusters != null && clusters.Count > 0)
                {
                    var ids = clusters.Select(x => x.Id);
                    return db.KeyPad_Object.Where(x => !x.IsDeleted && !x.KeyPad.IsDeleted && !x.Cum.IsDeleted && !x.Cum.Chuyen.IsDeleted && ids.Contains(x.ClusterId)).Select(x => new KeypadModel()
                    {
                        Id = x.KeyPadId,
                        sttNut = x.STTNut,
                        EquipmentId = x.KeyPad.EquipmentId,
                        CommandTypeId = x.CommandTypeId,
                        FloorId = x.KeyPad.FloorId,
                        UseTypeId = x.KeyPad.UseTypeId,
                        LineId = x.Cum.IdChuyen,
                        ClusterId = x.ClusterId,
                        IsEndOfLine = x.Cum.IsEndOfLine,
                        TypeOfKeypad = x.TypeOfKeypad
                    }).ToList();
                }
            }
            catch (Exception ex)
            { }
            return new List<KeypadModel>();
        }

        public static List<int> DSKeyPad(int floorId)
        {
            try
            {
                var db = new PMSEntities();
                return db.KeyPads.Where(x => !x.IsDeleted && !x.Floor.IsDeleted && x.FloorId == floorId).Select(x => x.EquipmentId ?? 0).ToList();
            }
            catch (Exception)
            { }
            return new List<int>();
        }
    }
}
