﻿using PMS.Business.Enum;
using PMS.Business.Models;
using PMS.Business.Web.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PMS.Business.Web
{
    public class BLLPhaseInDay
    {
        #region constructor
        static object key = new object();
        private static volatile BLLPhaseInDay _Instance;
        public static BLLPhaseInDay Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLPhaseInDay();

                return _Instance;
            }
        }
        private BLLPhaseInDay() { }
        #endregion

        public List<ModelSelectItem> GetPhases(int type)
        {
            using (var db = new PMSEntities())
            {
                return db.P_Phase.Where(x => !x.IsDeleted && x.Type == type).OrderBy(x => x.Index).Select(x => new ModelSelectItem() { Value = x.Id, Name = x.Name }).ToList();
            }
        }

        public bool InsertPhaseQuantities(P_PhaseDaily model, int csp)
        {
            try
            {
                using (var db = new PMSEntities())
                {
                    var phaseLog = db.P_Phase_Assign_Log.FirstOrDefault(x => x.AssignId == csp && x.PhaseId == model.PhaseId);
                    if (phaseLog != null)
                    {
                        switch (model.CommandTypeId)
                        {
                            case (int)eCommandRecive.ProductIncrease: phaseLog.Quantity += model.Quantity; break;
                            case (int)eCommandRecive.ProductReduce: phaseLog.Quantity -= model.Quantity; break;
                        }
                    }
                    else
                    {
                        var newObj = new P_Phase_Assign_Log()
                        {
                            Quantity = 0,
                            PhaseId = model.PhaseId,
                            AssignId = csp,
                             CreatedDate = DateTime.Now
                        };
                        switch (model.CommandTypeId)
                        {
                            case (int)eCommandRecive.ProductIncrease: newObj.Quantity += model.Quantity; break;
                            case (int)eCommandRecive.ProductReduce: newObj.Quantity -= model.Quantity; break;
                        }
                        db.P_Phase_Assign_Log.Add(newObj);
                    }
                  //  model.CreatedDate = DateTime.Now;
                    model.AssignId = csp;
                    db.P_PhaseDaily.Add(model);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        public ModelSelectItem GetLKPhase(int phaseId, int cspId)
        {
            var rs = new ModelSelectItem() { Id = 0, Value = 0 };
            using (var db = new PMSEntities())
            {
                var phaseLog = db.P_Phase_Assign_Log.FirstOrDefault(x => x.AssignId == cspId && x.PhaseId == phaseId);
                if (phaseLog != null)
                {
                    rs.Id = phaseLog.Quantity;
                    rs.Value = phaseLog.Chuyen_SanPham.LuyKeTH;
                    rs.Name = phaseLog.Chuyen_SanPham.Chuyen.TenChuyen;
                }
                else
                {
                    var csp = db.Chuyen_SanPham.FirstOrDefault(x => x.STT == cspId);
                    if (csp != null)
                    {
                        rs.Value = csp.LuyKeTH;
                        rs.Name = csp.Chuyen.TenChuyen;
                    }
                }
            }
            return rs;
        }

        public List<AddPhaseQuantitiesModel> GetPhaseDayInfo(int assignId, int phaseId, DateTime date)
        {
            var rs = new List<AddPhaseQuantitiesModel>();
            using (var db = new PMSEntities())
            {
                rs.AddRange(db.P_PhaseDaily.Where(x =>
                    x.CreatedDate.Day == date.Day &&
                    x.CreatedDate.Month == date.Month &&
                    x.CreatedDate.Year == date.Year &&
                    x.AssignId == assignId &&
                    x.PhaseId == phaseId)
                  .OrderByDescending(x => x.CreatedDate).Select(x => new AddPhaseQuantitiesModel()
                  {
                      LineName = x.Chuyen_SanPham.Chuyen.TenChuyen,
                      Date = x.CreatedDate,
                      Quantity = x.Quantity,
                      CommandTypeId = x.CommandTypeId,
                      strCommandType = (x.CommandTypeId == (int)eCommandRecive.ProductIncrease ? "Tăng" : "Giảm")
                  }));
                for (int i = 0; i < rs.Count; i++)
                    rs[i].strDate = rs[i].Date.ToString("d/M/yyyy HH:mm");
            }
            return rs;
        }

        public List<ModelSelectItem> GetHistoryPhase(int assignId, int phaseId)
        {
            var rs = new List<ModelSelectItem>();
            using (var db = new PMSEntities())
            {
                rs.AddRange((from p in db.P_PhaseDaily
                             where p.AssignId == assignId && p.PhaseId == phaseId
                             select p).ToList()
                  .Select(p => new ModelSelectItem()
                  {
                      Id = p.Id,
                      Name = p.CreatedDate.ToString("dd/MM/yyyy"),
                      Data = p.Quantity,
                      Date = p.CreatedDate,
                      Value = p.CommandTypeId
                  }).OrderByDescending(x => x.Date)
                  .GroupBy(p => p.Name).Select(p => new ModelSelectItem
                  {
                      Name = p.Key,
                      Data = p.Where(x => x.Value == (int)eCommandRecive.ProductIncrease).Sum(x => x.Data),
                      Value = p.Where(x => x.Value == (int)eCommandRecive.ProductReduce).Sum(x => x.Data),
                      Date = p.FirstOrDefault().Date
                  }));
            }
            return rs;
        }

        public List<AddPhaseQuantitiesModel> GetPhaseDayInfo(List<int> assignIds, int phaseType, DateTime date)
        {
            var rs = new List<AddPhaseQuantitiesModel>();
            using (var db = new PMSEntities())
            {
                rs.AddRange(db.P_PhaseDaily.Where(x =>
                    x.CreatedDate.Day == date.Day &&
                    x.CreatedDate.Month == date.Month &&
                    x.CreatedDate.Year == date.Year &&
                    assignIds.Contains(x.AssignId) &&
                    x.P_Phase.Type == phaseType)
                  .OrderByDescending(x => x.CreatedDate).Select(x => new AddPhaseQuantitiesModel()
                  {
                      AssignId = x.AssignId,
                      PhaseId = x.PhaseId,
                      LineName = x.Chuyen_SanPham.Chuyen.TenChuyen,
                      Date = x.CreatedDate,
                      Quantity = x.Quantity,
                      CommandTypeId = x.CommandTypeId,
                      strCommandType = (x.CommandTypeId == (int)eCommandRecive.ProductIncrease ? "Tăng" : "Giảm")
                  }));
                for (int i = 0; i < rs.Count; i++)
                    rs[i].strDate = rs[i].Date.ToString("d/M/yyyy HH:mm");
            }
            return rs;
        }

        public void UpdatePhaseDayQuantity(int assignId, int phaseType, DateTime date, int quantities)
        {
            using (var db = new PMSEntities())
            {
                var details = from x in db.P_PhaseDaily
                              where x.CreatedDate.Day == date.Day &&
                                    x.CreatedDate.Month == date.Month &&
                                    x.CreatedDate.Year == date.Year &&
                                    x.AssignId == assignId &&
                                    x.P_Phase.Type == phaseType
                              select x;
                foreach (var item in details)
                    item.Quantity = 0;

                var first = details.FirstOrDefault(x => x.CommandTypeId == (int)eCommandRecive.ProductIncrease);
                first.Quantity = quantities;
                db.SaveChanges();
            }
        }

        public ModelSelectItem GetPhaseInfo(int assignId, int phaseId, DateTime date)
        {

            var rs = new ModelSelectItem() { Data = 0, Value = 0 };
            using (var db = new PMSEntities())
            {
                try
                {
                    var inDay = (db.P_PhaseDaily.Where(x =>
                                     x.CreatedDate.Day == date.Day &&
                                     x.CreatedDate.Month == date.Month &&
                                     x.CreatedDate.Year == date.Year &&
                                     x.AssignId == assignId &&
                                     x.PhaseId == phaseId)
                                   .OrderByDescending(x => x.CreatedDate).Select(x => new AddPhaseQuantitiesModel()
                                   {
                                       LineName = x.Chuyen_SanPham.Chuyen.TenChuyen,
                                       Date = x.CreatedDate,
                                       Quantity = x.Quantity,
                                       CommandTypeId = x.CommandTypeId,
                                       strCommandType = (x.CommandTypeId == (int)eCommandRecive.ProductIncrease ? "Tăng" : "Giảm")
                                   })).ToList();

                    if (inDay != null && inDay.Count() > 0)
                    {
                        rs.Data = inDay.Where(x => x.CommandTypeId == (int)eCommandRecive.ProductIncrease).Sum(x => x.Quantity);
                        rs.Data -= inDay.Where(x => x.CommandTypeId == (int)eCommandRecive.ProductReduce).Sum(x => x.Quantity);
                    }

                    var phaseLog = db.P_Phase_Assign_Log.FirstOrDefault(x => x.AssignId == assignId && x.PhaseId == phaseId);

                    if (phaseLog != null)
                        rs.Value = phaseLog.Quantity;
                }
                catch (Exception)
                {
                }

            }
            return rs;
        }

        public ModelSelectItem GetPhaseInfoInMonth(int assignId, int phaseId, DateTime date)
        {

            var rs = new ModelSelectItem() { Data = 0, Value = 0 };
            using (var db = new PMSEntities())
            {
                try
                {
                    var inDay = (db.P_PhaseDaily.Where(x => 
                                     x.CreatedDate.Month == date.Month &&
                                     x.CreatedDate.Year == date.Year &&
                                     x.AssignId == assignId &&
                                     x.PhaseId == phaseId)
                                   .OrderByDescending(x => x.CreatedDate).Select(x => new AddPhaseQuantitiesModel()
                                   {
                                       LineName = x.Chuyen_SanPham.Chuyen.TenChuyen,
                                       Date = x.CreatedDate,
                                       Quantity = x.Quantity,
                                       CommandTypeId = x.CommandTypeId,
                                       strCommandType = (x.CommandTypeId == (int)eCommandRecive.ProductIncrease ? "Tăng" : "Giảm")
                                   })).ToList();

                    if (inDay != null && inDay.Count() > 0)
                    {
                        rs.Data = inDay.Where(x => x.CommandTypeId == (int)eCommandRecive.ProductIncrease).Sum(x => x.Quantity);
                        rs.Data -= inDay.Where(x => x.CommandTypeId == (int)eCommandRecive.ProductReduce).Sum(x => x.Quantity);
                    } 
                }
                catch (Exception)
                {
                }

            }
            return rs;
        }

    }
}
