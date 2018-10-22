using PMS.Business.Enum;
using PMS.Business.Models;
using PMS.Business.Web.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                return db.P_Phase.Where(x => !x.IsDeleted && x.Type == type).Select(x => new ModelSelectItem() { Value = x.Id, Name = x.Name }).ToList();
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
                    model.CreatedDate = DateTime.Now;
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

        public List<AddPhaseQuantitiesModel> GetPhaseDayInfo(int phaseId, string date)
        {
            var rs = new List<AddPhaseQuantitiesModel>();
            using (var db = new PMSEntities())
            {
                rs.AddRange(db.P_PhaseDaily.Where(x => x.NangXuat.Ngay == date && x.PhaseId == phaseId).OrderByDescending(x => x.CreatedDate).Select(x => new AddPhaseQuantitiesModel()
                {
                    LineName = x.NangXuat.Chuyen_SanPham.Chuyen.TenChuyen,
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
    }
}
