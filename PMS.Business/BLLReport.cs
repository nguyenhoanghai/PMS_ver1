using PMS.Business.Enum;
using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business
{
    public static class BLLReport
    {
        public static List<WorkingTimeModel> GetNSCForChart(int lineId, int clusterId, string date)
        {
            try
            {
                var db = new PMSEntities();
                var times = BLLShift.GetWorkingTimeOfLine(lineId);
                var tdns = db.TheoDoiNgays.Where(x => !x.Chuyen.IsDeleted && !x.Cum.IsDeleted && x.Date == date && x.MaChuyen == lineId && x.CumId == clusterId).ToList();
                if (times != null && times.Count > 0)
                {
                    foreach (var item in times)
                    {
                        var tang = tdns.Where(x => x.Time >= item.TimeStart && x.Time <= item.TimeEnd && x.CommandTypeId == (int)eCommandRecive.ProductIncrease && x.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(x => x.ThanhPham);
                        var giam = tdns.Where(x => x.Time >= item.TimeStart && x.Time <= item.TimeEnd && x.CommandTypeId == (int)eCommandRecive.ProductReduce && x.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(x => x.ThanhPham);
                        tang = tang - giam;
                        item.KCS = tang > 0 ? tang : 0;
                    }
                    return times;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        public static List<ClusterModel> GetNSCForChart(int lineId, TimeSpan timeStart, TimeSpan timeEnd, string date)
        {
            try
            {
                var db = new PMSEntities();
                var clusters = BLLCluster.GetAllByLineId(lineId);
                if (clusters != null && clusters.Count > 0)
                {
                    var ids = clusters.Select(x => x.Id);
                    var tdns = db.TheoDoiNgays.Where(x => !x.Chuyen.IsDeleted && !x.Cum.IsDeleted && x.Date == date && x.MaChuyen == lineId && ids.Contains(x.CumId)).ToList();

                    foreach (var item in clusters)
                    {
                        var tang = tdns.Where(x => x.CumId == item.Id && x.Time >= timeStart && x.Time <= timeEnd && x.CommandTypeId == (int)eCommandRecive.ProductIncrease && x.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(x => x.ThanhPham);
                        var giam = tdns.Where(x => x.CumId == item.Id && x.Time >= timeStart && x.Time <= timeEnd && x.CommandTypeId == (int)eCommandRecive.ProductReduce && x.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(x => x.ThanhPham);
                        tang = tang - giam;
                        item.KCS = tang > 0 ? tang : 0;
                    }
                    return clusters;
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public static List<WorkingTimeModel> GetNSCForChart(int lineId, string date)
        {
            try
            {
                var db = new PMSEntities();
                var times = BLLShift.GetWorkingTimeOfLine(lineId);
                var tdns = db.TheoDoiNgays.Where(x => !x.Chuyen.IsDeleted && !x.Cum.IsDeleted && x.Date == date && x.MaChuyen == lineId).ToList();
                if (times != null && times.Count > 0)
                {
                    foreach (var item in times)
                    {
                        var tang = tdns.Where(x => x.Time >= item.TimeStart && x.Time <= item.TimeEnd && x.CommandTypeId == (int)eCommandRecive.ProductIncrease && x.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(x => x.ThanhPham);
                        var giam = tdns.Where(x => x.Time >= item.TimeStart && x.Time <= item.TimeEnd && x.CommandTypeId == (int)eCommandRecive.ProductReduce && x.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(x => x.ThanhPham);
                        tang = tang - giam;
                        item.KCS = tang > 0 ? tang : 0;
                    }
                    return times;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        public static List<LineModel> GetNSCErrorForChart(List<LineModel> lines, TimeSpan timeStart, TimeSpan timeEnd, string date)
        {
            try
            {
                if (lines.Count > 0)
                {
                    var db = new PMSEntities();
                    var ids = lines.Select(x => x.MaChuyen);
                    var tdns = db.TheoDoiNgays.Where(x => !x.Chuyen.IsDeleted && !x.Cum.IsDeleted && x.Date == date && ids.Contains(x.MaChuyen) && x.IsEndOfLine && x.ErrorId != null).ToList();

                    foreach (var item in lines)
                    {
                        var tang = tdns.Where(x => x.MaChuyen == item.MaChuyen && x.Time >= timeStart && x.Time <= timeEnd && x.CommandTypeId == (int)eCommandRecive.ErrorIncrease).Sum(x => x.ThanhPham);
                        var giam = tdns.Where(x => x.MaChuyen == item.MaChuyen && x.Time >= timeStart && x.Time <= timeEnd && x.CommandTypeId == (int)eCommandRecive.ErrorReduce).Sum(x => x.ThanhPham);
                        tang = tang - giam;
                        item.TC = tang > 0 ? tang : 0;
                    }
                }
                return lines;
            }
            catch (Exception ex)
            { }
            return null;
        }


    }
}
