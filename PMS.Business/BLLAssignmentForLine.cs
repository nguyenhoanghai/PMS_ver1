using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMS.Business.Models;
using GPRO.Ultilities;
using PMS.Business.Enum;
using System.Globalization;

namespace PMS.Business
{
    public class BLLAssignmentForLine
    {
        static Object key = new object();
        private static volatile BLLAssignmentForLine _Instance;  //volatile =>  tranh dung thread
        public static BLLAssignmentForLine Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLAssignmentForLine();

                return _Instance;
            }
        }

        private BLLAssignmentForLine() { }


        /// <summary>
        /// New Fuction return List of Line Assignment
        /// Dùng cho report ns hàng giờ
        /// </summary>
        /// <param name="Date , lineIds"></param>
        /// <returns>List<ChuyenSanPhamModel></returns>
        public List<ChuyenSanPhamModel> GetProductivitiesOfLines(DateTime date, List<int> lineIds)
        {
            try
            {
                using (var db = new PMSEntities())
                {
                    var ngay = date.Day + "/" + date.Month + "/" + date.Year;
                    var assigns = db.NangXuats.Where(x => !x.IsDeleted && !x.Chuyen_SanPham.SanPham.IsDelete && !x.Chuyen_SanPham.Chuyen.IsDeleted && lineIds.Contains(x.Chuyen_SanPham.MaChuyen) && x.Ngay == ngay).Select(x => new ChuyenSanPhamModel()
                    {
                        STT = x.STTCHuyen_SanPham,
                        STTThucHien = x.Chuyen_SanPham.STTThucHien,
                        MaChuyen = x.Chuyen_SanPham.MaChuyen,
                        LineName = x.Chuyen_SanPham.Chuyen.TenChuyen,
                        BaseLabors = x.Chuyen_SanPham.Chuyen.LaoDongDinhBien,
                        MaSanPham = x.Chuyen_SanPham.MaSanPham,
                        CommoName = x.Chuyen_SanPham.SanPham.TenSanPham,
                        IsFinish = x.Chuyen_SanPham.IsFinish,
                        NormsDay = x.DinhMucNgay,
                        SanLuongKeHoach = x.Chuyen_SanPham.SanLuongKeHoach,
                        LuyKeTH = x.Chuyen_SanPham.LuyKeTH,
                        LuyKeBTPThoatChuyen = x.Chuyen_SanPham.LuyKeBTPThoatChuyen,
                        BTP_Day = x.BTPTang,
                        BTP_Day_G = x.BTPGiam,
                        TH_Day = x.ThucHienNgay,
                        TH_Day_G = x.ThucHienNgayGiam,
                        TC_Day = x.BTPThoatChuyenNgay,
                        TC_Day_G = x.BTPThoatChuyenNgayGiam,
                        Err_Day = x.SanLuongLoi,
                        Err_Day_G = x.SanLuongLoiGiam,
                        Price = x.Chuyen_SanPham.SanPham.DonGia,
                        PriceCM = x.Chuyen_SanPham.SanPham.DonGiaCM,
                        NhipTT = x.NhipDoThucTe,
                        NhipSX = x.NhipDoSanXuat,
                        NhipTC = x.NhipDoThucTeBTPThoatChuyen,
                        BTPInLine = x.BTPTrenChuyen
                    }).OrderBy(x => x.STTThucHien).ToList();

                    if (assigns != null && assigns.Count() > 0)
                    {
                        var assIds = assigns.Select(x => x.STT).Distinct();
                        var tps = db.ThanhPhams.Where(x => !x.IsDeleted && x.Ngay == ngay && assIds.Contains(x.STTChuyen_SanPham)).ToList();
                        var btps = db.BTPs.Where(x => !x.IsDeleted && !x.IsBTP_PB_HC && x.IsEndOfLine && x.Ngay == ngay && assIds.Contains(x.STTChuyen_SanPham)).ToList();
                        var tdns = db.TheoDoiNgays.Where(x => x.Date == ngay && assIds.Contains(x.STTChuyenSanPham)).ToList();
                        var monthDetails = db.P_MonthlyProductionPlans.Where(x => !x.IsDeleted && x.Month == date.Month && x.Year == date.Year && assIds.Contains(x.STT_C_SP)).ToList();

                        foreach (var item in assigns)
                        {
                            var workHours = BLLShift.GetListWorkHoursOfLineByLineId(item.MaChuyen);
                            var tp = tps.FirstOrDefault(x => x.STTChuyen_SanPham == item.STT);
                            var mDetail = monthDetails.FirstOrDefault(x => x.STT_C_SP == item.STT);
                            item.CurrentLabors = tp != null ? tp.LaoDongChuyen : 0;
                            item.ProductionPlansInMonth = mDetail != null ? mDetail.ProductionPlans : 0;
                            item.LK_TH_InMonth = mDetail != null ? mDetail.LK_TH : 0;
                            item.LK_TC_InMonth = mDetail.LK_TC;
                            item.LK_BTP_InMonth = mDetail.LK_BTP;
                            item.LK_BTP = btps.Where(x => x.STTChuyen_SanPham == item.STT && x.CommandTypeId == (int)eCommandRecive.BTPIncrease).Sum(x => x.BTPNgay);
                            item.LK_BTP_G = btps.Where(x => x.STTChuyen_SanPham == item.STT && x.CommandTypeId == (int)eCommandRecive.BTPReduce).Sum(x => x.BTPNgay);
                            item.Percent_TH = item.NormsDay <= 0 || (item.TH_Day - item.TH_Day_G) <= 0 ? 0 : Math.Round(((item.TH_Day - item.TH_Day_G) / item.NormsDay) * 100, 0);
                            item.Percent_Error = item.NormsDay <= 0 || (item.Err_Day - item.Err_Day_G) <= 0 ? 0 : Math.Round(((item.Err_Day - item.Err_Day_G) / item.NormsDay) * 100, 0);
                            item.Percent_Nhip = item.NhipTT <= 0 || item.NhipSX <= 0 ? 0 : Math.Round((item.NhipSX / item.NhipTT) * 100, 0);

                            item.RevenuesInDay = item.PriceCM <= 0 || (item.TH_Day - item.TH_Day_G) <= 0 ? 0 : Math.Round(((item.TH_Day - item.TH_Day_G) * item.PriceCM), 2);
                            item.RevenuesInMonth = mDetail.LK_TH <= 0 || item.PriceCM <= 0 ? 0 : Math.Round(mDetail.LK_TH * item.PriceCM, 2);

                            item.NormsHours = item.NormsDay <= 0 ? 0 : Math.Round(item.NormsDay / workHours.Count, 0);
                            item.Lean = item.BTPInLine <= 0 || item.CurrentLabors <= 0 ? 0 : (int)(Math.Ceiling((double)item.BTPInLine / item.CurrentLabors));
                            item.TGDaLV = 0;

                            if (workHours != null && workHours.Count > 0)
                            {
                                item.workingTimes.AddRange(workHours);
                                foreach (var time in item.workingTimes)
                                {
                                    if (date.TimeOfDay > time.TimeStart)
                                        item.TGDaLV++;
                                    var tang = tdns.Where(x => x.IsEndOfLine && x.MaChuyen == item.MaChuyen && x.STTChuyenSanPham == item.STT && x.Time >= time.TimeStart && x.Time <= time.TimeEnd && x.CommandTypeId == (int)eCommandRecive.ProductIncrease && x.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(x => x.ThanhPham);
                                    var giam = tdns.Where(x => x.IsEndOfLine && x.MaChuyen == item.MaChuyen && x.STTChuyenSanPham == item.STT && x.Time >= time.TimeStart && x.Time <= time.TimeEnd && x.CommandTypeId == (int)eCommandRecive.ProductReduce && x.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(x => x.ThanhPham);
                                    time.KCS = tang - giam;

                                    tang = tdns.Where(x => x.IsEndOfLine && x.MaChuyen == item.MaChuyen && x.STTChuyenSanPham == item.STT && x.Time >= time.TimeStart && x.Time <= time.TimeEnd && x.CommandTypeId == (int)eCommandRecive.ProductIncrease && x.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(x => x.ThanhPham);
                                    giam = tdns.Where(x => x.IsEndOfLine && x.MaChuyen == item.MaChuyen && x.STTChuyenSanPham == item.STT && x.Time >= time.TimeStart && x.Time <= time.TimeEnd && x.CommandTypeId == (int)eCommandRecive.ProductReduce && x.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(x => x.ThanhPham);
                                    time.TC = tang - giam;

                                    tang = tdns.Where(x => x.IsEndOfLine && x.MaChuyen == item.MaChuyen && x.STTChuyenSanPham == item.STT && x.Time >= time.TimeStart && x.Time <= time.TimeEnd && x.CommandTypeId == (int)eCommandRecive.ErrorIncrease).Sum(x => x.ThanhPham);
                                    giam = tdns.Where(x => x.IsEndOfLine && x.MaChuyen == item.MaChuyen && x.STTChuyenSanPham == item.STT && x.Time >= time.TimeStart && x.Time <= time.TimeEnd && x.CommandTypeId == (int)eCommandRecive.ErrorReduce).Sum(x => x.ThanhPham);
                                    time.Error = tang - giam;

                                    tang = btps.Where(x => x.IsEndOfLine && x.STTChuyen_SanPham == item.STT && x.TimeUpdate >= time.TimeStart && x.TimeUpdate <= time.TimeEnd && x.CommandTypeId == (int)eCommandRecive.BTPIncrease).Sum(x => x.BTPNgay);
                                    giam = btps.Where(x => x.IsEndOfLine && x.STTChuyen_SanPham == item.STT && x.TimeUpdate >= time.TimeStart && x.TimeUpdate <= time.TimeEnd && x.CommandTypeId == (int)eCommandRecive.BTPReduce).Sum(x => x.BTPNgay);
                                    time.BTP = tang - giam;
                                }
                            }
                        }
                        return assigns;
                    }
                }
            }
            catch (Exception ex)
            { }
            return null;
        }

        /// <summary>
        /// New Fuction return List of Line Assignment but times get NS follow config
        /// Report ns hang gio 1
        /// </summary>
        /// <param name="date"></param>
        /// <param name="lineIds"></param>
        /// <returns></returns>
        public List<ChuyenSanPhamModel> GetProductivitiesOfLines(DateTime date, List<int> lineIds, int timeGetNS, int getBTPInLineByType, int maCongDoan)
        {
            try
            {
                using (var db = new PMSEntities())
                {
                    var ngay = date.Day + "/" + date.Month + "/" + date.Year;
                    var assigns = db.NangXuats.Where(x => !x.IsDeleted && !x.Chuyen_SanPham.SanPham.IsDelete && !x.Chuyen_SanPham.Chuyen.IsDeleted && lineIds.Contains(x.Chuyen_SanPham.MaChuyen) && x.Ngay == ngay).Select(x => new ChuyenSanPhamModel()
                    {
                        STT = x.STTCHuyen_SanPham,
                        STTThucHien = x.Chuyen_SanPham.STTThucHien,
                        MaChuyen = x.Chuyen_SanPham.MaChuyen,
                        LineName = x.Chuyen_SanPham.Chuyen.TenChuyen,
                        BaseLabors = x.Chuyen_SanPham.Chuyen.LaoDongDinhBien,
                        MaSanPham = x.Chuyen_SanPham.MaSanPham,
                        CommoName = x.Chuyen_SanPham.SanPham.TenSanPham,
                        IsFinish = x.Chuyen_SanPham.IsFinish,
                        NormsDay = x.DinhMucNgay,
                        SanLuongKeHoach = x.Chuyen_SanPham.SanLuongKeHoach,
                        LuyKeTH = x.Chuyen_SanPham.LuyKeTH,
                        LuyKeBTPThoatChuyen = x.Chuyen_SanPham.LuyKeBTPThoatChuyen,
                        BTP_Day = x.BTPTang,
                        BTP_Day_G = x.BTPGiam,
                        TH_Day = x.ThucHienNgay,
                        TH_Day_G = x.ThucHienNgayGiam,
                        TC_Day = x.BTPThoatChuyenNgay,
                        TC_Day_G = x.BTPThoatChuyenNgayGiam,
                        Err_Day = x.SanLuongLoi,
                        Err_Day_G = x.SanLuongLoiGiam,
                        Price = x.Chuyen_SanPham.SanPham.DonGia,
                        PriceCM = x.Chuyen_SanPham.SanPham.DonGiaCM,
                        NhipTT = x.NhipDoThucTe,
                        NhipSX = x.NhipDoSanXuat,
                        NhipTC = x.NhipDoThucTeBTPThoatChuyen,
                        BTPInLine = x.BTPTrenChuyen,
                        ProductionTime = x.Chuyen_SanPham.SanPham.ProductionTime,
                        lkCongDoan = 0
                    }).OrderBy(x => x.STTThucHien).ToList();

                    if (assigns != null && assigns.Count() > 0)
                    {
                        var shiftsOfLines = db.P_LineWorkingShift.Where(x => !x.IsDeleted).Select(x => new LineWorkingShiftModel()
                        {
                            Id = x.Id,
                            ShiftId = x.ShiftId,
                            ShiftOrder = x.ShiftOrder,
                            LineId = x.LineId,
                            Start = x.P_WorkingShift.TimeStart,
                            End = x.P_WorkingShift.TimeEnd
                        }).OrderBy(x => x.ShiftOrder).ToList();
                        var assIds = assigns.Select(x => x.STT).Distinct();
                        var tps = db.ThanhPhams.Where(x => !x.IsDeleted && x.Ngay == ngay && assIds.Contains(x.STTChuyen_SanPham)).ToList();
                        var btps = db.BTPs.Where(x => !x.IsDeleted && !x.IsBTP_PB_HC && x.IsEndOfLine && assIds.Contains(x.STTChuyen_SanPham)).ToList();
                        var tdns = db.TheoDoiNgays.Where(x => x.Date == ngay && assIds.Contains(x.STTChuyenSanPham)).ToList();
                        var monthDetails = db.P_MonthlyProductionPlans.Where(x => !x.IsDeleted && x.Month == date.Month && x.Year == date.Year && assIds.Contains(x.STT_C_SP)).ToList();
                        var slCongdoanNgays = db.P_PhaseDailyLog.Where(x => x.NangXuat.Ngay == ngay && x.PhaseId == maCongDoan).ToList();
                        var lkcongdoans = db.P_Phase_Assign_Log.Where(x =>   x.PhaseId == maCongDoan).ToList();
                        foreach (var item in assigns)
                        {
                            var lkcs = lkcongdoans.FirstOrDefault(x => x.AssignId == item.STT);
                            if (lkcs != null)
                                item.lkCongDoan = lkcs.Quantity;

                            var shifts = shiftsOfLines.Where(x => x.LineId == item.MaChuyen).OrderBy(x => x.ShiftOrder).ToList();
                            var totalWorkingTimeInDay = BLLShift.GetTotalWorkingHourOfLine(shifts);
                            int intWorkTime = (int)(totalWorkingTimeInDay.TotalHours);
                            int intWorkMinuter = (int)totalWorkingTimeInDay.TotalMinutes;
                            double NangSuatPhutKH = 0;
                            int NangSuatGioKH = 0;
                            NangSuatPhutKH = (double)item.NormsDay / intWorkMinuter;
                            NangSuatGioKH = (int)(item.NormsDay / intWorkTime);
                            if (item.NormsDay % intWorkTime != 0)
                                NangSuatGioKH++;

                            var workHours = BLLShift.GetListWorkHoursOfLineByLineId(shifts, timeGetNS);
                            var tp = tps.FirstOrDefault(x => x.STTChuyen_SanPham == item.STT);
                            var mDetail = monthDetails.FirstOrDefault(x => x.STT_C_SP == item.STT);

                            item.HieuSuatNgay = tp != null ? tp.HieuSuat : 0;
                            item.LeanKH = tp != null ? tp.LeanKH : 0;
                            item.CurrentLabors = tp != null ? tp.LaoDongChuyen : 0;
                            item.ProductionPlansInMonth = mDetail != null ? mDetail.ProductionPlans : 0;
                            item.LK_TH_InMonth = mDetail != null ? mDetail.LK_TH : 0;
                            item.LK_TC_InMonth = mDetail.LK_TC;
                            item.LK_BTP_InMonth = mDetail.LK_BTP;
                            item.LK_BTP = btps.Where(x => x.STTChuyen_SanPham == item.STT && x.CommandTypeId == (int)eCommandRecive.BTPIncrease).Sum(x => x.BTPNgay);
                            item.LK_BTP_G = btps.Where(x => x.STTChuyen_SanPham == item.STT && x.CommandTypeId == (int)eCommandRecive.BTPReduce).Sum(x => x.BTPNgay);
                            item.Percent_TH = item.NormsDay <= 0 || (item.TH_Day - item.TH_Day_G) <= 0 ? 0 : Math.Round(((item.TH_Day - item.TH_Day_G) / item.NormsDay) * 100, 0);
                            item.Percent_Error = item.NormsDay <= 0 || (item.Err_Day - item.Err_Day_G) <= 0 ? 0 : Math.Round(((item.Err_Day - item.Err_Day_G) / item.NormsDay) * 100, 0);
                            item.Percent_Nhip = item.NhipTT <= 0 || item.NhipSX <= 0 ? 0 : Math.Round((item.NhipSX / item.NhipTT) * 100, 0);

                            item.RevenuesInDay = item.PriceCM <= 0 || (item.TH_Day - item.TH_Day_G) <= 0 ? 0 : Math.Round(((item.TH_Day - item.TH_Day_G) * item.PriceCM), 2);
                            item.RevenuesInMonth = mDetail.LK_TH <= 0 || item.PriceCM <= 0 ? 0 : Math.Round(mDetail.LK_TH * item.PriceCM, 2);

                            item.Lean = item.BTPInLine <= 0 || item.CurrentLabors <= 0 ? 0 : (int)(Math.Ceiling((double)item.BTPInLine / item.CurrentLabors));
                            item.TGDaLV = 0;

                            if (workHours != null && workHours.Count > 0)
                            {
                                item.workingTimes.AddRange(workHours);
                                for (int i = 0; i < item.workingTimes.Count; i++)
                                {
                                    item.workingTimes[i].NormsHour = Math.Round(NangSuatPhutKH * (int)((item.workingTimes[0].TimeEnd - item.workingTimes[0].TimeStart).TotalMinutes));

                                    if (date.TimeOfDay > item.workingTimes[i].TimeStart)
                                        item.TGDaLV++;
                                    var tang = tdns.Where(x => x.IsEndOfLine && x.MaChuyen == item.MaChuyen && x.STTChuyenSanPham == item.STT && x.Time >= item.workingTimes[i].TimeStart && x.Time <= item.workingTimes[i].TimeEnd && x.CommandTypeId == (int)eCommandRecive.ProductIncrease && x.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(x => x.ThanhPham);
                                    var giam = tdns.Where(x => x.IsEndOfLine && x.MaChuyen == item.MaChuyen && x.STTChuyenSanPham == item.STT && x.Time >= item.workingTimes[i].TimeStart && x.Time <= item.workingTimes[i].TimeEnd && x.CommandTypeId == (int)eCommandRecive.ProductReduce && x.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(x => x.ThanhPham);
                                    item.workingTimes[i].KCS = tang - giam;

                                    tang = tdns.Where(x => x.IsEndOfLine && x.MaChuyen == item.MaChuyen && x.STTChuyenSanPham == item.STT && x.Time >= item.workingTimes[i].TimeStart && x.Time <= item.workingTimes[i].TimeEnd && x.CommandTypeId == (int)eCommandRecive.ProductIncrease && x.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(x => x.ThanhPham);
                                    giam = tdns.Where(x => x.IsEndOfLine && x.MaChuyen == item.MaChuyen && x.STTChuyenSanPham == item.STT && x.Time >= item.workingTimes[i].TimeStart && x.Time <= item.workingTimes[i].TimeEnd && x.CommandTypeId == (int)eCommandRecive.ProductReduce && x.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(x => x.ThanhPham);
                                    item.workingTimes[i].TC = tang - giam;

                                    tang = tdns.Where(x => x.IsEndOfLine && x.MaChuyen == item.MaChuyen && x.STTChuyenSanPham == item.STT && x.Time >= item.workingTimes[i].TimeStart && x.Time <= item.workingTimes[i].TimeEnd && x.CommandTypeId == (int)eCommandRecive.ErrorIncrease).Sum(x => x.ThanhPham);
                                    giam = tdns.Where(x => x.IsEndOfLine && x.MaChuyen == item.MaChuyen && x.STTChuyenSanPham == item.STT && x.Time >= item.workingTimes[i].TimeStart && x.Time <= item.workingTimes[i].TimeEnd && x.CommandTypeId == (int)eCommandRecive.ErrorReduce).Sum(x => x.ThanhPham);
                                    item.workingTimes[i].Error = tang - giam;

                                    tang = btps.Where(x => x.IsEndOfLine && x.Ngay == ngay && x.STTChuyen_SanPham == item.STT && x.TimeUpdate >= item.workingTimes[i].TimeStart && x.TimeUpdate <= item.workingTimes[i].TimeEnd && x.CommandTypeId == (int)eCommandRecive.BTPIncrease).Sum(x => x.BTPNgay);
                                    giam = btps.Where(x => x.IsEndOfLine && x.Ngay == ngay && x.STTChuyen_SanPham == item.STT && x.TimeUpdate >= item.workingTimes[i].TimeStart && x.TimeUpdate <= item.workingTimes[i].TimeEnd && x.CommandTypeId == (int)eCommandRecive.BTPReduce).Sum(x => x.BTPNgay);
                                    item.workingTimes[i].BTP = tang - giam;

                                    if (slCongdoanNgays != null && slCongdoanNgays.Count() > 0)
                                    {
                                        tang = slCongdoanNgays.Where(x => x.NangXuat.Ngay == ngay && x.NangXuat.STTCHuyen_SanPham == item.STT && x.CreatedDate.TimeOfDay >= item.workingTimes[i].TimeStart && x.CreatedDate.TimeOfDay <= item.workingTimes[i].TimeEnd && x.CommandTypeId == (int)eCommandRecive.ProductIncrease).Sum(x => x.Quantity);
                                        giam = slCongdoanNgays.Where(x => x.NangXuat.Ngay == ngay && x.NangXuat.STTCHuyen_SanPham == item.STT && x.CreatedDate.TimeOfDay >= item.workingTimes[i].TimeStart && x.CreatedDate.TimeOfDay <= item.workingTimes[i].TimeEnd && x.CommandTypeId == (int)eCommandRecive.ProductReduce).Sum(x => x.Quantity);
                                        item.workingTimes[i].CongDoan = tang - giam;
                                    } 
                                    //  
                                    if (DateTime.Now.TimeOfDay >= item.workingTimes[i].TimeStart)
                                    {
                                        var btp = item.BTP_Day - item.BTP_Day_G;
                                        btp = ((item.LK_BTP - item.LK_BTP_G) - btp);
                                        btp += item.workingTimes.Where(x => x.TimeEnd <= item.workingTimes[i].TimeEnd).ToList().Sum(x => x.BTP);
                                        //  +item.workingTimes[i].BTP;

                                        double btpInLine = 0;
                                        switch (getBTPInLineByType)
                                        {
                                            case 1:
                                                var th = (item.TH_Day - item.TH_Day_G);
                                                //  th = (item.LuyKeTH - th) + item.workingTimes[i].KCS;
                                                th = (item.LuyKeTH - th);
                                                th += item.workingTimes.Where(x => x.TimeEnd <= item.workingTimes[i].TimeEnd).ToList().Sum(x => x.KCS);
                                                btpInLine = btp - th;
                                                break;
                                            case 2:
                                                var tc = (item.TC_Day - item.TC_Day_G);
                                                // tc = (item.LuyKeBTPThoatChuyen - tc) + item.workingTimes[i].TC;
                                                tc = (item.LuyKeBTPThoatChuyen - tc);
                                                tc += item.workingTimes.Where(x => x.TimeEnd <= item.workingTimes[i].TimeEnd).ToList().Sum(x => x.TC);
                                                btpInLine = btp - tc;
                                                break;
                                        }
                                        item.workingTimes[i].BTPInLine = btpInLine;
                                        item.workingTimes[i].Lean = btpInLine > 0 ? (int)(Math.Round((double)btpInLine / item.CurrentLabors)) : 0;
                                    }
                                    else
                                        item.workingTimes[i].Lean = 0;
                                }
                            }
                        }

                        return assigns;
                    }
                }
            }
            catch (Exception ex)
            { }
            return null;
        }

        public List<AssignmentForLine_Grid_Model> GetDataForGridView(int LineId)
        {
            try
            {
                using (var db = new PMSEntities())
                {
                    var list = db.Chuyen_SanPham.Where(x => !x.IsDelete && !x.Chuyen.IsDeleted && !x.SanPham.IsDelete && x.MaChuyen == LineId && (x.Nam == DateTime.Now.Year || !x.IsFinish)).OrderByDescending(x => x.Nam).ThenByDescending(x => x.Thang).ThenBy(x => x.IsFinish).Select(x => new AssignmentForLine_Grid_Model()
                    {
                        STT = x.STT,
                        STT_TH = x.STTThucHien,
                        CommoName = x.SanPham.TenSanPham,
                        LK_TH = x.LuyKeTH,
                        Month = x.Thang,
                        Year = x.Nam,
                        ProductionPlans = x.SanLuongKeHoach,
                        TimeProductPerCommo = x.SanPham.ProductionTime,
                        IsFinishStr = x.IsFinish ? "kết thúc" : "Đang thực hiện"
                    }).ToList();
                    return list != null && list.Count() > 0 ? list : new List<AssignmentForLine_Grid_Model>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseBase InsertOrUpdate(AssignmentForLineModel obj)
        {
            var result = new ResponseBase();
            try
            {
                using (var db = new PMSEntities())
                {
                    Chuyen_SanPham csp;
                    if (obj.STT == 0)
                    {
                        csp = new Chuyen_SanPham();
                        Parse.CopyObject(obj, ref csp);
                        db.Chuyen_SanPham.Add(csp);
                    }
                    else
                    {
                        csp = db.Chuyen_SanPham.FirstOrDefault(x => !x.IsDelete && !x.IsFinish && x.STT == obj.STT);
                        if (csp != null)
                        {
                            csp.STTThucHien = obj.STTThucHien;
                            csp.Thang = obj.Thang;
                            csp.Nam = obj.Nam;
                            csp.SanLuongKeHoach = obj.SanLuongKeHoach;
                            csp.IsFinishBTPThoatChuyen = obj.IsFinishBTPThoatChuyen;
                            csp.IsFinish = obj.IsFinish;
                            csp.TimeAdd = obj.TimeAdd;
                        }
                    }
                    db.SaveChanges();
                    result.IsSuccess = true;
                    result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Lưu Phân công thành công." });
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Messages.Add(new Message() { Title = "Lỗi Thêm Phân Công", msg = "Thêm Phân công cho Chuyền bị lỗi.\n" + ex.Message });
            }
            return result;
        }

        public Chuyen_SanPham Find(int LineId, int CommoId)
        {
            try
            {
                using (var db = new PMSEntities())
                {
                    return db.Chuyen_SanPham.OrderByDescending(x => x.STT).FirstOrDefault(x => !x.IsDelete && !x.IsFinish && x.MaChuyen == LineId && x.MaSanPham == CommoId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Chuyen_SanPham CheckExists(int assignId, int LineId, int CommoId)
        {
            try
            {
                using (var db = new PMSEntities())
                {
                    if (assignId == 0)
                        return db.Chuyen_SanPham.Where(x => !x.IsDelete && x.MaChuyen == LineId && x.MaSanPham == CommoId).OrderByDescending(x => x.TimeAdd).FirstOrDefault();
                    else
                        return db.Chuyen_SanPham.FirstOrDefault(x => !x.IsDelete && x.STT == assignId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Chuyen_SanPham FindByStt(int stt)
        {
            try
            {
                using (var db = new PMSEntities())
                {
                    return db.Chuyen_SanPham.FirstOrDefault(x => !x.IsDelete && x.STT == stt);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ChuyenSanPhamModel FindByLineId(int lineId)
        {
            try
            {
                using (var db = new PMSEntities())
                {
                    var date = DateTime.Now;
                    return db.Chuyen_SanPham.Where(x => !x.IsDelete && !x.IsFinish && !x.Chuyen.IsDeleted && !x.SanPham.IsDelete && x.MaChuyen == lineId).Select(x => new ChuyenSanPhamModel()
                    {
                        STT = x.STT,
                        CommoName = x.SanPham.TenSanPham,
                        MaSanPham = x.MaSanPham,
                        MaChuyen = x.MaChuyen,
                        LineName = x.Chuyen.TenChuyen,
                        IdDen = x.Chuyen.IdDen,
                        STTThucHien = x.STTThucHien,
                        ProductionTime = x.SanPham.ProductionTime,
                        HieuSuatNgay = x.ThanhPhams.FirstOrDefault(u => u.Ngay == "").HieuSuat,
                        SanLuongKeHoach = x.SanLuongKeHoach,
                        LuyKeTH = x.LuyKeTH,
                        Thang = x.Thang,
                        Nam = x.Nam,
                        IsFinishStr = (!x.IsFinish ? "Đang Thực Hiện" : "Hoàn Thành")
                    }).OrderBy(x => x.STTThucHien).FirstOrDefault();
                }
            }
            catch (Exception) { }
            return null;
        }

        /// <summary>
        /// Delete phan hàng cho chuyen
        /// </summary>
        /// <param name="stt"></param>
        /// <returns></returns>
        public ResponseBase Delete(int stt)
        {
            var result = new ResponseBase();
            try
            {
                using (var db = new PMSEntities())
                {
                    var btps = db.BTPs.Where(x => !x.IsDeleted && x.STTChuyen_SanPham == stt);
                    if (btps != null && btps.Count() > 0)
                    {
                        foreach (var item in btps)
                        {
                            item.IsDeleted = true;
                        }
                    }

                    var pros = db.NangXuats.Where(x => !x.IsDeleted && x.STTCHuyen_SanPham == stt);
                    if (pros != null && pros.Count() > 0)
                    {
                        foreach (var item in pros)
                        {
                            item.IsDeleted = true;
                        }
                    }

                    var csp = db.Chuyen_SanPham.FirstOrDefault(x => !x.IsDelete && !x.IsFinish && x.STT == stt);
                    if (csp != null)
                    {
                        csp.IsDelete = true;
                        db.SaveChanges();
                        result.IsSuccess = true;
                        result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Xóa Thành công." });
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Messages.Add(new Message() { Title = "Lỗi", msg = "không tìm thấy lỗi." });
                    }
                }
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Messages.Add(new Message() { Title = "Lỗi", msg = "Lỗi trong quá trình thực thi." });
            }
            return result;
        }

        /// <summary>
        /// Cập nhật phân hàng cho chuyền
        /// </summary>
        /// <param name="obj">Thông tin phân hàng Chuyen_SanPham Obj</param>
        /// <param name="isUpdateForContinuePro">mã hàng đã finish cập nhật lại sản lượng kế hoạch sản xuất tiếp hay ko ?</param>
        /// <param name="calculateBTPInLineConfig">Config tính BTP trên chuyền</param>
        /// <returns></returns>
        public ResponseBase Update(Chuyen_SanPham obj, bool isUpdateForContinuePro, int calculateBTPInLineConfig)
        {
            var result = new ResponseBase();
            try
            {
                using (var db = new PMSEntities())
                {
                    var csp = db.Chuyen_SanPham.FirstOrDefault(x => !x.IsDelete && x.STT == obj.STT);
                    if (csp != null)
                    {
                        if (!CheckOrderIndex(obj.STT, obj.MaChuyen, obj.STTThucHien))
                        {
                            var mDetail = db.P_MonthlyProductionPlans.FirstOrDefault(x => !x.IsDeleted && x.STT_C_SP == obj.STT && x.Month == DateTime.Now.Month && x.Year == DateTime.Now.Year);
                            if (mDetail != null)
                            {
                                if (csp.SanLuongKeHoach > obj.SanLuongKeHoach)
                                    mDetail.ProductionPlans -= (csp.SanLuongKeHoach - obj.SanLuongKeHoach);
                                else
                                    mDetail.ProductionPlans += (obj.SanLuongKeHoach - csp.SanLuongKeHoach);
                            }

                            if (!isUpdateForContinuePro)
                            {
                                csp.STTThucHien = obj.STTThucHien;
                                csp.Nam = obj.Nam;
                                csp.Thang = obj.Thang;
                            }
                            csp.UpdatedDate = obj.UpdatedDate;
                            csp.SanLuongKeHoach = obj.SanLuongKeHoach;
                            csp.IsFinish = csp.SanLuongKeHoach > csp.LuyKeTH ? false : true;
                            csp.IsFinishBTPThoatChuyen = obj.IsFinishBTPThoatChuyen;

                            // update lai nang suat san suat va dinh muc ngay hien tai
                            var ngay = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                            var tp = db.ThanhPhams.FirstOrDefault(x => !x.IsDeleted && x.STTChuyen_SanPham == csp.STT && x.Ngay == ngay);
                            var nx = db.NangXuats.FirstOrDefault(x => !x.IsDeleted && x.STTCHuyen_SanPham == csp.STT && x.Ngay == ngay);
                            if (tp != null && nx != null)
                            {
                                var tgLVTrongNgay = (int)BLLShift.GetTotalWorkingHourOfLine(csp.MaChuyen).TotalSeconds;
                                tp.NangXuatLaoDong = (float)Math.Round((tgLVTrongNgay / csp.SanPham.ProductionTime), 2);

                                nx.DinhMucNgay = (float)Math.Round((tp.NangXuatLaoDong * tp.LaoDongChuyen), 1);
                                nx.NhipDoSanXuat = (float)Math.Round((csp.SanPham.ProductionTime / tp.LaoDongChuyen), 1);
                                nx.TimeLastChange = DateTime.Now.TimeOfDay;
                            }
                            db.SaveChanges();
                            //BLLProductivity.ResetNormsDayAndBTPInLine(2, obj.MaChuyen, false);
                            result.IsSuccess = true;
                            result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Lưu Phân công thành công." });

                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Messages.Add(new Message() { Title = "Thông Báo Trùng số thứ tự", msg = "Số thứ tự thực hiện Mã Hàng này được chọn. Vui lòng chọn số thứ tự thực hiện khác." });
                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Messages.Add(new Message() { Title = "Lỗi Lưu Phân Công", msg = "không tìm thấy Phân Công.\n" });
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Messages.Add(new Message() { Title = "Lỗi Lưu Phân Công", msg = "Lưu Phân công cho Chuyền bị lỗi.\n" + ex.Message });
            }
            return result;
        }

        private bool CheckOrderIndex(int stt, int LineId, int orderIndex)
        {
            using (var db = new PMSEntities())
            {
                var check = db.Chuyen_SanPham.FirstOrDefault(x => !x.IsDelete && !x.IsFinish && x.STT != stt && x.MaChuyen == LineId && x.STTThucHien == orderIndex);
                if (check != null)
                    return true;
                return false;
            }
        }

        public ResponseBase Update(int stt, int? LKTH, int? LKTC, bool isfinish, bool isfinishnow)
        {
            var result = new ResponseBase();
            try
            {
                using (var db = new PMSEntities())
                {
                    var obj = db.Chuyen_SanPham.FirstOrDefault(x => !x.IsDelete && !x.IsFinish && x.STT == stt); ;
                    if (obj != null)
                    {
                        if (LKTH != null)
                            obj.LuyKeTH = LKTH.Value;
                        if (LKTC != null)
                            obj.LuyKeBTPThoatChuyen = LKTC.Value;
                        obj.IsFinish = isfinish;
                        obj.IsFinishNow = isfinishnow;
                        if (isfinish)
                            obj.FinishedDate = DateTime.Now;
                        db.SaveChanges();
                        result.IsSuccess = true;
                        result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Lưu Phân công thành công." });
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Messages.Add(new Message() { Title = "Lỗi Lưu Phân Công", msg = "không tìm thấy Phân Công.\n" });
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Messages.Add(new Message() { Title = "Lỗi Lưu Phân Công", msg = "Lưu Phân công cho Chuyền bị lỗi.\n" + ex.Message });
            }
            return result;
        }

        public bool UpdateQuantityTotalOfPCC(int sttChuyenSanPham, int quantityTotal, int tpThoatChuyen, bool isFinish, bool isFinishThoatChuyen)
        {
            try
            {
                using (var db = new PMSEntities())
                {
                    var csp = db.Chuyen_SanPham.FirstOrDefault(x => !x.IsDelete && !x.IsFinish && x.STT == sttChuyenSanPham); ;
                    if (csp != null)
                    {
                        csp.LuyKeTH = quantityTotal;
                        csp.LuyKeBTPThoatChuyen = tpThoatChuyen;
                        csp.IsFinish = isFinish;
                        csp.IsFinishBTPThoatChuyen = isFinishThoatChuyen;
                        db.SaveChanges();
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public ResponseBase Insert(Chuyen_SanPham obj)
        {
            var result = new ResponseBase();
            try
            {
                using (var db = new PMSEntities())
                {
                    var exist = db.Chuyen_SanPham.FirstOrDefault(x => !x.IsDelete && !x.IsFinish && x.MaChuyen == obj.MaChuyen && x.STT != obj.STT && x.STTThucHien == obj.STTThucHien);
                    if (exist == null)
                    {
                        var monthDetail = new P_MonthlyProductionPlans();
                        monthDetail.Chuyen_SanPham = obj;
                        monthDetail.Month = DateTime.Now.Month;
                        monthDetail.Year = DateTime.Now.Year;
                        monthDetail.ProductionPlans = obj.SanLuongKeHoach;
                        obj.P_MonthlyProductionPlans = new List<P_MonthlyProductionPlans>();
                        obj.P_MonthlyProductionPlans.Add(monthDetail);
                        db.Chuyen_SanPham.Add(obj);
                        db.SaveChanges();
                        result.IsSuccess = true;
                        result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Lưu thành công" });
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Messages.Add(new Message() { Title = "Lỗi", msg = "Số thứ tự phân công sản xuất đã tồn tại.\nVui lòng chọn số thứ tự phân công khác." });
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Messages.Add(new Message()
                {
                    Title = "Lỗi",
                    msg = "Lỗi: việc phân công Mặt Hàng  thất bại.\n" + ex.Message
                });
            }
            return result;
        }

        public Chuyen_SanPham GetPCOldOfLineBySTT(int lineId, int sttChuyenSanPhamNow)
        {
            try
            {
                using (var db = new PMSEntities())
                {
                    var csp = db.Chuyen_SanPham.Where(x => !x.IsDelete && x.MaChuyen == lineId).ToList();
                    if (csp.Count > 0)
                    {
                        bool isGetSTTOld = false;
                        foreach (var item in csp)
                        {
                            if (item.STT == sttChuyenSanPhamNow)
                                isGetSTTOld = true;

                            if (isGetSTTOld)
                                return item;
                        }
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Chuyen_SanPham GetPCOldOfLineBySTT(int LineId)
        {
            try
            {
                using (var db = new PMSEntities())
                {
                    return db.Chuyen_SanPham.Where(x => !x.IsDelete && !x.SanPham.IsDelete && x.MaChuyen == LineId && !x.IsFinish).OrderBy(x => x.STT).FirstOrDefault();
                }
            }
            catch (Exception)
            { }
            return null;
        }

        public List<ChuyenSanPhamModel> GetListChuyenSanPham(int chuyenId, bool isGetForPhanHang)
        {
            try
            {
                using (var db = new PMSEntities())
                {
                    var dateTimeNow = DateTime.Now;
                    if (isGetForPhanHang)
                        return db.Chuyen_SanPham.Where(x => !x.IsDelete && !x.Chuyen.IsDeleted && !x.SanPham.IsDelete && x.MaChuyen == chuyenId).OrderByDescending(x => x.Nam).ThenByDescending(x => x.Thang).Select(x => new ChuyenSanPhamModel()
                        {
                            STT = x.STT,
                            CommoName = x.SanPham.TenSanPham,
                            MaSanPham = x.MaSanPham,
                            MaChuyen = x.MaChuyen,
                            IdDen = x.Chuyen.IdDen,
                            STTThucHien = x.STTThucHien,
                            ProductionTime = x.SanPham.ProductionTime,
                            SanLuongKeHoach = x.SanLuongKeHoach,
                            LuyKeTH = x.LuyKeTH,
                            Thang = x.Thang,
                            Nam = x.Nam,
                            IsFinishStr = (!x.IsFinish ? "Đang Thực Hiện" : "Hoàn Thành")
                        }).ToList();
                    else
                        return db.Chuyen_SanPham.Where(x => !x.IsDelete && !x.Chuyen.IsDeleted && !x.SanPham.IsDelete && !x.IsFinish && x.MaChuyen == chuyenId).OrderByDescending(x => x.Nam).ThenByDescending(x => x.Thang).Select(x => new ChuyenSanPhamModel()
                        {
                            STT = x.STT,
                            CommoName = x.SanPham.TenSanPham,
                            MaSanPham = x.MaSanPham,
                            MaChuyen = x.MaChuyen,
                            IdDen = x.Chuyen.IdDen,
                            STTThucHien = x.STTThucHien,
                            ProductionTime = x.SanPham.ProductionTime,
                            SanLuongKeHoach = x.SanLuongKeHoach,
                            LuyKeTH = x.LuyKeTH,
                            Thang = x.Thang,
                            Nam = x.Nam,
                            IsFinishStr = (!x.IsFinish ? "Đang Thực Hiện" : "Hoàn Thành")
                        }).ToList();
                }
            }
            catch (Exception) { }
            return null;
        }

        public Chuyen_SanPham LayLuyKeTHandKeHoachTheoSTT(int STT)
        {
            try
            {
                using (var db = new PMSEntities())
                {
                    return db.Chuyen_SanPham.FirstOrDefault(x => !x.IsDelete && x.STT == STT);
                }
            }
            catch (Exception) { }
            return null;
        }

        /// <summary>
        /// return ChuyenSanPhamModel
        /// tra ve tat ca thong tin phan cong trong ngay
        /// </summary>
        /// <param name="ngay"></param>
        /// <param name="sttChuyenSanPham"></param>
        /// <param name="LineId"></param>
        /// <returns></returns>
        public ChuyenSanPhamModel GetAssignmentByDay(string ngay, int sttChuyenSanPham, int LineId)
        {
            try
            {
                using (var db = new PMSEntities())
                {
                    var csp = db.Chuyen_SanPham.Where(x => !x.IsDelete && !x.Chuyen.IsDeleted && !x.SanPham.IsDelete && x.STT == sttChuyenSanPham).Select(x => new ChuyenSanPhamModel()
                    {
                        STT = x.STT,
                        Thang = x.Thang,
                        Nam = x.Nam,
                        MaChuyen = x.MaChuyen,
                        LineName = x.Chuyen.TenChuyen,
                        MaSanPham = x.MaSanPham,
                        CommoName = x.SanPham.TenSanPham,
                        Price = x.SanPham.DonGia,
                        PriceCM = x.SanPham.DonGiaCM,
                        SanLuongKeHoach = x.SanLuongKeHoach,
                        ProductionTime = x.SanPham.ProductionTime,
                        IsFinish = x.IsFinish,
                        IsFinishBTPThoatChuyen = x.IsFinishBTPThoatChuyen,
                        IsFinishNow = x.IsFinishNow,
                        Sound = x.Chuyen.Sound
                    }).FirstOrDefault();
                    if (csp != null)
                    {
                        var ns = db.NangXuats.Where(x => !x.IsDeleted && x.STTCHuyen_SanPham == sttChuyenSanPham).ToList();
                        var btps = db.BTPs.Where(x => !x.IsDeleted && !x.IsBTP_PB_HC && x.STTChuyen_SanPham == sttChuyenSanPham && x.IsEndOfLine).ToList();

                        csp.LuyKeTH = ns.Sum(x => x.ThucHienNgay) - ns.Sum(x => x.ThucHienNgayGiam);
                        csp.LuyKeBTPThoatChuyen = ns.Sum(x => x.BTPThoatChuyenNgay) - ns.Sum(x => x.BTPThoatChuyenNgayGiam);
                        csp.LK_BTP = btps.Where(x => x.CommandTypeId == (int)eCommandRecive.BTPIncrease).Sum(x => x.BTPNgay);
                        csp.LK_BTP_G = btps.Where(x => x.CommandTypeId == (int)eCommandRecive.BTPReduce).Sum(x => x.BTPNgay);
                        csp.CountAssignment = db.Chuyen_SanPham.Where(x => !x.IsDelete && x.MaChuyen == LineId && !x.IsFinish).Count();
                    }
                    return csp;
                }
            }
            catch (Exception) { }
            return null;
        }


        public int GetKCSToDay(DateTime ngay, int sttChuyenSanPham)
        {
            try
            {
                using (var db = new PMSEntities())
                {
                    return db.NangXuats.Where(x => !x.IsDeleted && x.STTCHuyen_SanPham == sttChuyenSanPham && x.CreatedDate < ngay).Sum(x => (x.ThucHienNgay - x.ThucHienNgayGiam));
                }
            }
            catch (Exception) { }
            return 0;
        }

        public List<AssignmentForLineModel> GetAssignmentForLine(int lineId, bool isForDayInfo)
        {
            try
            {
                using (var db = new PMSEntities())
                {
                    if (isForDayInfo)
                    {
                        return db.Chuyen_SanPham.Where(x => !x.IsDelete && !x.SanPham.IsDelete && !x.Chuyen.IsDeleted && x.MaChuyen == lineId && (!x.IsFinish || (x.FinishedDate != null && x.FinishedDate.Value.Day == DateTime.Now.Day && x.FinishedDate.Value.Month == DateTime.Now.Month && x.FinishedDate.Value.Year == DateTime.Now.Year))).Select(x => new AssignmentForLineModel()
                        {
                            STT = x.STT,
                            STTThucHien = x.STTThucHien,
                            Thang = x.Thang,
                            Nam = x.Nam,
                            SanLuongKeHoach = x.SanLuongKeHoach,
                            NangXuatSanXuat = x.SanPham.ProductionTime,
                            LuyKeTH = x.LuyKeTH,
                            LuyKeBTPThoatChuyen = x.LuyKeBTPThoatChuyen,
                            TimeAdd = x.TimeAdd,
                            IsFinish = x.IsFinish,
                            IsFinishBTPThoatChuyen = x.IsFinishBTPThoatChuyen,
                            IsFinishNow = x.IsFinishNow,
                            IsMoveQuantityFromMorthOld = x.IsMoveQuantityFromMorthOld,
                            MaChuyen = x.MaChuyen,
                            LineName = x.Chuyen.TenChuyen,
                            MaSanPham = x.MaSanPham,
                            CommoName = x.SanPham.TenSanPham,
                            CommoPrice = x.SanPham.DonGia,
                            CommoPriceCM = x.SanPham.DonGiaCM
                        }).OrderBy(x => x.STTThucHien).ToList();
                    }
                    else
                    {
                        return db.Chuyen_SanPham.Where(x => !x.IsDelete && !x.SanPham.IsDelete && !x.Chuyen.IsDeleted && x.MaChuyen == lineId && !x.IsFinish).Select(x => new AssignmentForLineModel()
                        {
                            STT = x.STT,
                            STTThucHien = x.STTThucHien,
                            Thang = x.Thang,
                            Nam = x.Nam,
                            SanLuongKeHoach = x.SanLuongKeHoach,
                            NangXuatSanXuat = x.SanPham.ProductionTime,
                            LuyKeTH = x.LuyKeTH,
                            LuyKeBTPThoatChuyen = x.LuyKeBTPThoatChuyen,
                            TimeAdd = x.TimeAdd,
                            IsFinish = x.IsFinish,
                            IsFinishBTPThoatChuyen = x.IsFinishBTPThoatChuyen,
                            IsFinishNow = x.IsFinishNow,
                            IsMoveQuantityFromMorthOld = x.IsMoveQuantityFromMorthOld,
                            MaChuyen = x.MaChuyen,
                            LineName = x.Chuyen.TenChuyen,
                            MaSanPham = x.MaSanPham,
                            CommoName = x.SanPham.TenSanPham,
                            CommoPrice = x.SanPham.DonGia,
                            CommoPriceCM = x.SanPham.DonGiaCM
                        }).OrderBy(x => x.STTThucHien).ToList();
                    }
                }
            }
            catch (Exception)
            { return null; }
        }

        public List<AssignmentForLineModel> GetAssignmentForLine(int lineId)
        {
            try
            {
                using (var db = new PMSEntities())
                {
                    var objs = db.NangXuats.Where(x => !x.IsDeleted && !x.Chuyen_SanPham.SanPham.IsDelete && !x.Chuyen_SanPham.Chuyen.IsDeleted &&
                        x.Chuyen_SanPham.MaChuyen == lineId && (!x.Chuyen_SanPham.IsFinish || (x.Chuyen_SanPham.FinishedDate != null && x.Chuyen_SanPham.FinishedDate.Value.Day == DateTime.Now.Day && x.Chuyen_SanPham.FinishedDate.Value.Month == DateTime.Now.Month && x.Chuyen_SanPham.FinishedDate.Value.Year == DateTime.Now.Year))).Select(x => new AssignmentForLineModel()
                    {
                        NangSuatId = x.Id,
                        STT = x.Chuyen_SanPham.STT,
                        STTThucHien = x.Chuyen_SanPham.STTThucHien,
                        Thang = x.Chuyen_SanPham.Thang,
                        Nam = x.Chuyen_SanPham.Nam,
                        SanLuongKeHoach = x.Chuyen_SanPham.SanLuongKeHoach,
                        NangXuatSanXuat = x.Chuyen_SanPham.SanPham.ProductionTime,
                        LuyKeTH = x.Chuyen_SanPham.LuyKeTH,
                        LuyKeBTPThoatChuyen = x.Chuyen_SanPham.LuyKeBTPThoatChuyen,
                        LK_BTP_HC = x.Chuyen_SanPham.LK_BTP_HC,
                        LK_BTP = x.Chuyen_SanPham.LK_BTP,
                        TimeAdd = x.Chuyen_SanPham.TimeAdd,
                        IsFinish = x.Chuyen_SanPham.IsFinish,
                        IsFinishBTPThoatChuyen = x.Chuyen_SanPham.IsFinishBTPThoatChuyen,
                        IsFinishNow = x.Chuyen_SanPham.IsFinishNow,
                        IsMoveQuantityFromMorthOld = x.Chuyen_SanPham.IsMoveQuantityFromMorthOld,
                        MaChuyen = x.Chuyen_SanPham.MaChuyen,
                        LineName = x.Chuyen_SanPham.Chuyen.TenChuyen,
                        MaSanPham = x.Chuyen_SanPham.MaSanPham,
                        CommoName = x.Chuyen_SanPham.SanPham.TenSanPham,
                        CommoPrice = x.Chuyen_SanPham.SanPham.DonGia,
                        CommoPriceCM = x.Chuyen_SanPham.SanPham.DonGiaCM,
                        
                    }).OrderBy(x => x.STTThucHien).ToList();

                    if (objs != null && objs.Count > 0)
                    {
                        var ids = objs.Select(x => x.STT);
                        var btps = db.BTPs.Where(x => !x.IsDeleted && x.IsEndOfLine && ids.Contains(x.STTChuyen_SanPham)).ToList();
                        int tang = 0, giam = 0;
                        foreach (var item in objs)
                        {
                            tang = btps.Where(x => !x.IsBTP_PB_HC && x.CommandTypeId == (int)eCommandRecive.BTPIncrease && x.STTChuyen_SanPham == item.STT).Sum(x => x.BTPNgay);
                            giam = btps.Where(x => !x.IsBTP_PB_HC && x.CommandTypeId == (int)eCommandRecive.BTPReduce && x.STTChuyen_SanPham == item.STT).Sum(x => x.BTPNgay);
                            item.LK_BTP = tang - giam;

                            //   tang = btps.Where(x => x.IsBTP_PB_HC && x.CommandTypeId == (int)eCommandRecive.BTPIncrease && x.STTChuyen_SanPham == item.STT).Sum(x => x.BTPNgay);
                            //  giam = btps.Where(x => x.IsBTP_PB_HC && x.CommandTypeId == (int)eCommandRecive.BTPReduce && x.STTChuyen_SanPham == item.STT).Sum(x => x.BTPNgay);
                            //  item.LK_BTP_HC = tang - giam;
                        }
                        return objs;
                    }
                    return null;
                }
            }
            catch (Exception)
            { return null; }
        }

        public List<AssignmentForLineModel> GetAssignmentForLines(List<int> lineIds)
        {
            try
            {
                using (var db = new PMSEntities())
                {
                    var objs = db.Chuyen_SanPham.Where(x => !x.IsDelete && !x.SanPham.IsDelete && !x.Chuyen.IsDeleted && lineIds.Contains(x.MaChuyen) && !x.IsFinish).Select(x => new AssignmentForLineModel()
                    {
                        STT = x.STT,
                        STTThucHien = x.STTThucHien,
                        Thang = x.Thang,
                        Nam = x.Nam,
                        SanLuongKeHoach = x.SanLuongKeHoach,
                        NangXuatSanXuat = x.SanPham.ProductionTime,
                        LuyKeTH = x.LuyKeTH,
                        LuyKeBTPThoatChuyen = x.LuyKeBTPThoatChuyen,
                        TimeAdd = x.TimeAdd,
                        IsFinish = x.IsFinish,
                        IsFinishBTPThoatChuyen = x.IsFinishBTPThoatChuyen,
                        IsFinishNow = x.IsFinishNow,
                        IsMoveQuantityFromMorthOld = x.IsMoveQuantityFromMorthOld,
                        MaChuyen = x.MaChuyen,
                        LineName = x.Chuyen.TenChuyen,
                        MaSanPham = x.MaSanPham,
                        CommoName = x.SanPham.TenSanPham,
                        CommoPrice = x.SanPham.DonGia,
                        CommoPriceCM = x.SanPham.DonGiaCM,
                        ReadPercentId = x.Chuyen.IdTyLeDoc ?? 0
                    }).OrderBy(x => x.MaChuyen).ThenBy(x => x.STT).ToList();

                    if (objs.Count > 0)
                    {
                        var readIds = objs.Where(x => x.ReadPercentId > 0).Select(c => c.ReadPercentId).Distinct().ToList();
                        var readPercentObjs = db.ReadPercents.Where(x => readIds.Contains(x.Id)).ToList();
                        foreach (var item in objs)
                        {
                            if (item.ReadPercentId > 0)
                            {
                                var obj_ = readPercentObjs.FirstOrDefault(x => x.Id == item.ReadPercentId);
                                item.ReadPercentName = obj_ != null ? obj_.Name : string.Empty;
                            }
                        }
                    }
                    return objs;
                }
            }
            catch (Exception)
            { return null; }
        }

        public AssignmentModel_BangDienTu GetAssignmentModel_BangDienTu(int assignId, int lineId, string dateNow, bool hienThiDenTheoTPThoatChuyen, int TypeOfShowProductToLCD)
        {
            try
            {
                using (var db = new PMSEntities())
                {
                    ThanhPham tp = null;
                    if (TypeOfShowProductToLCD == 2)
                        tp = db.ThanhPhams.Where(x => !x.IsDeleted && x.Chuyen_SanPham.MaChuyen == lineId && x.Ngay == dateNow && x.ShowLCD).FirstOrDefault();
                    if (tp == null)
                        tp = db.ThanhPhams.Where(x => !x.IsDeleted && x.Ngay == dateNow && x.STTChuyen_SanPham == assignId).FirstOrDefault();

                    var csp = db.Chuyen_SanPham.Where(x => !x.IsDelete && !x.Chuyen.IsDeleted && !x.SanPham.IsDelete && x.STT == tp.STTChuyen_SanPham).Select(x => new AssignmentModel_BangDienTu()
                    {
                        lineId = x.MaChuyen,
                        LineName = x.Chuyen.TenChuyen,
                        CommoName = x.SanPham.TenSanPham,
                        ProductionPlans = x.SanLuongKeHoach,
                        ProductionNorms = 0,
                        KCSInDay = 0,
                        LK_KCS = x.LuyKeTH,
                        KCSInHours = 0,
                        TCInDay = 0,
                        LK_TC = x.LuyKeBTPThoatChuyen,
                        TCInHours = 0,
                        BTPInDay = 0,
                        LK_BTP = x.LK_BTP,
                        ErrorInDay = 0,
                        CommoPrice = x.SanPham.DonGia,
                        CommoPriceCM = x.SanPham.DonGiaCM,

                        lightPercent = 0,

                        KCSPercent = 0,
                        Labours = 0,
                        BTPInLine = 0,
                        BTPPerLabour = 0,
                        ProductionPace = 0,
                        CurrentPace = 0,
                        Current_TC_Pace = 0,
                        SalesDate = 0,
                    }).FirstOrDefault();
                    var ns = db.NangXuats.Where(x => !x.IsDeleted && x.Ngay == dateNow && x.STTCHuyen_SanPham == assignId).FirstOrDefault();
                    //      var tp = db.ThanhPhams.Where(x => !x.IsDeleted && x.Ngay == dateNow && x.STTChuyen_SanPham == assignId).FirstOrDefault();
                    if (csp != null && ns != null && tp != null)
                    {
                        var btps = db.BTPs.Where(x => !x.IsDeleted && !x.IsBTP_PB_HC && x.STTChuyen_SanPham == assignId && x.IsEndOfLine).ToList();

                        if (!hienThiDenTheoTPThoatChuyen)
                            csp.lightPercent = (ns.NhipDoThucTe == 0 || ns.NhipDoSanXuat == 0) ? 0 : Math.Round(((ns.NhipDoSanXuat / ns.NhipDoThucTe) * 100));
                        else
                            csp.lightPercent = (ns.NhipDoThucTeBTPThoatChuyen == 0 || ns.NhipDoSanXuat == 0) ? 0 : Math.Round(((ns.NhipDoSanXuat / ns.NhipDoThucTeBTPThoatChuyen) * 100));
                        csp.IsShowLCD = tp.ShowLCD;
                        csp.ProductionNorms = ns.DinhMucNgay;
                        csp.KCSInDay = ns.ThucHienNgay - ns.ThucHienNgayGiam;

                        csp.TCInDay = ns.BTPThoatChuyenNgay - ns.BTPThoatChuyenNgayGiam;
                        csp.KCSPercent = (csp.KCSInDay == 0 || csp.ProductionNorms == 0) ? 0 : Math.Round(((csp.KCSInDay / csp.ProductionNorms) * 100), 0);

                        csp.Labours = tp.LaoDongChuyen;

                        int tang = 0, giam = 0;
                        tang = btps.Where(x => x.Ngay == dateNow && x.CommandTypeId == (int)eCommandRecive.BTPIncrease).Sum(x => x.BTPNgay);
                        giam = btps.Where(x => x.Ngay == dateNow && x.CommandTypeId == (int)eCommandRecive.BTPReduce).Sum(x => x.BTPNgay);
                        csp.BTPInDay = tang - giam;

                        tang = btps.Where(x => x.CommandTypeId == (int)eCommandRecive.BTPIncrease).Sum(x => x.BTPNgay);
                        giam = btps.Where(x => x.CommandTypeId == (int)eCommandRecive.BTPReduce).Sum(x => x.BTPNgay);
                        csp.LK_BTP = tang - giam;

                        csp.BTPInLine = ns.BTPTrenChuyen;
                        csp.ProductionPace = ns.NhipDoSanXuat;
                        csp.CurrentPace = ns.NhipDoThucTe;
                        csp.BTPPerLabour = (csp.BTPInLine == 0 || csp.Labours == 0) ? 0 : Math.Round((((double)csp.BTPInLine / csp.Labours) * 100), 0);

                        csp.ErrorInDay = ns.SanLuongLoi - ns.SanLuongLoiGiam;
                        csp.Current_TC_Pace = ns.NhipDoThucTeBTPThoatChuyen;
                        csp.SalesDate = (csp.KCSInDay == 0 || csp.CommoPrice == 0) ? 0 : Math.Round((((double)csp.KCSInDay * csp.CommoPrice) * 100), 0);

                        var listWorkHoursOfLine = BLLShift.GetListWorkHoursOfLineByLineId(csp.lineId);
                        if (listWorkHoursOfLine != null && listWorkHoursOfLine.Count > 0)
                        {
                            var time = DateTime.Now.TimeOfDay;
                            var currentTime = listWorkHoursOfLine.FirstOrDefault(x => time >= x.TimeStart && time < x.TimeEnd);
                            if (currentTime != null)
                            {
                                var tdns = db.TheoDoiNgays.Where(c => c.MaChuyen == csp.lineId && c.Date == dateNow && c.Time > currentTime.TimeStart && c.Time <= currentTime.TimeEnd && c.IsEndOfLine).Select(x => new DayInfoModel()
                                    {
                                        CommandTypeId = x.CommandTypeId,
                                        CumId = x.CumId,
                                        MaChuyen = x.MaChuyen,
                                        MaSanPham = x.MaSanPham,
                                        Time = x.Time,
                                        Date = x.Date,
                                        ErrorId = x.ErrorId,
                                        IsEndOfLine = x.IsEndOfLine,
                                        IsEnterByKeypad = x.IsEnterByKeypad,
                                        STT = x.STT,
                                        STTChuyenSanPham = x.STTChuyenSanPham,
                                        ThanhPham = x.ThanhPham,
                                        ProductOutputTypeId = x.ProductOutputTypeId
                                    }).ToList();
                                if (tdns.Count > 0)
                                {
                                    tang = tdns.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductIncrease && c.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(c => c.ThanhPham);
                                    giam = tdns.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductReduce && c.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(c => c.ThanhPham);
                                    csp.KCSInHours = tang - giam;

                                    //b2 
                                    tang = tdns.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductIncrease && c.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(c => c.ThanhPham);
                                    giam = tdns.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductReduce && c.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(c => c.ThanhPham);
                                    csp.TCInHours = tang - giam;
                                    // lỗi
                                    //Tang = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ErrorIncrease).Sum(c => c.ThanhPham);
                                    //Giam = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ErrorReduce).Sum(c => c.ThanhPham);
                                    //Tang -= Giam;
                                    //listWorkHoursOfLine[i].Error = Tang;
                                }
                            }
                        }
                    }
                    return csp;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<AssignmentModel_BangDienTu> GetAssignmentModel_BangDienTu(string dateNow, bool hienThiDenTheoTPThoatChuyen)
        {
            var assignInDay = new List<AssignmentModel_BangDienTu>();
            try
            {
                using (var db = new PMSEntities())
                {
                    var proInday = db.NangXuats.Where(x => !x.IsDeleted && x.Ngay == dateNow && x.IsChange == 1).ToList();
                    if (proInday != null && proInday.Count() > 0)
                    {
                        var assignIds = proInday.Select(x => x.STTCHuyen_SanPham);
                        assignInDay.AddRange(db.Chuyen_SanPham.Where(x => !x.IsDelete && !x.Chuyen.IsDeleted && !x.SanPham.IsDelete && assignIds.Contains(x.STT)).Select(x => new AssignmentModel_BangDienTu()
                                      {
                                          #region
                                          AssignId = x.STT,
                                          lineId = x.MaChuyen,
                                          LineName = x.Chuyen.TenChuyen,
                                          LightPercentId = x.Chuyen.IdDenNangSuat,
                                          CommoName = x.SanPham.TenSanPham,
                                          ProductionPlans = x.SanLuongKeHoach,
                                          ProductionNorms = 0,
                                          TimeProductPerItem = x.SanPham.ProductionTime,
                                          KCSInDay = 0,
                                          LK_KCS = x.LuyKeTH,
                                          KCSInHours = 0,
                                          TCInDay = 0,
                                          LK_TC = x.LuyKeBTPThoatChuyen,
                                          TCInHours = 0,
                                          BTPInDay = 0,
                                          LK_BTP = x.LK_BTP,
                                          ErrorInDay = 0,
                                          CommoPrice = x.SanPham.DonGia,
                                          CommoPriceCM = x.SanPham.DonGiaCM,

                                          lightPercent = 0,

                                          KCSPercent = 0,
                                          Labours = 0,
                                          BTPInLine = 0,
                                          BTPPerLabour = 0,
                                          ProductionPace = 0,
                                          CurrentPace = 0,
                                          Current_TC_Pace = 0,
                                          SalesDate = 0,
                                          IsChange = true
                                          #endregion
                                      }));

                        #region lay thong tin refernces
                        var lineids = assignInDay.Select(x => x.lineId).Distinct().ToList();
                        var tempObjs = db.NangXuats.Where(x => !x.IsDeleted && x.Ngay == dateNow && !assignIds.Contains(x.STTCHuyen_SanPham) && lineids.Contains(x.Chuyen_SanPham.MaChuyen)).ToList();
                        proInday.AddRange(tempObjs);

                        assignIds = tempObjs.Select(x => x.STTCHuyen_SanPham);
                        assignInDay.AddRange(db.Chuyen_SanPham.Where(x => !x.IsDelete && !x.Chuyen.IsDeleted && !x.SanPham.IsDelete && assignIds.Contains(x.STT)).Select(x => new AssignmentModel_BangDienTu()
                        {
                            #region
                            AssignId = x.STT,
                            lineId = x.MaChuyen,
                            LineName = x.Chuyen.TenChuyen,
                            LightPercentId = x.Chuyen.IdDenNangSuat,
                            CommoName = x.SanPham.TenSanPham,
                            ProductionPlans = x.SanLuongKeHoach,
                            ProductionNorms = 0,
                            TimeProductPerItem = x.SanPham.ProductionTime,
                            KCSInDay = 0,
                            LK_KCS = x.LuyKeTH,
                            KCSInHours = 0,
                            TCInDay = 0,
                            LK_TC = x.LuyKeBTPThoatChuyen,
                            TCInHours = 0,
                            BTPInDay = 0,
                            LK_BTP = x.LK_BTP,
                            ErrorInDay = 0,
                            CommoPrice = x.SanPham.DonGia,
                            CommoPriceCM = x.SanPham.DonGiaCM,

                            lightPercent = 0,

                            KCSPercent = 0,
                            Labours = 0,
                            BTPInLine = 0,
                            BTPPerLabour = 0,
                            ProductionPace = 0,
                            CurrentPace = 0,
                            Current_TC_Pace = 0,
                            SalesDate = 0,
                            IsChange = false
                            #endregion
                        }));
                        assignIds = proInday.Select(x => x.STTCHuyen_SanPham);
                        #endregion

                        var tpInDay = db.ThanhPhams.Where(x => !x.IsDeleted && x.Ngay == dateNow && assignIds.Contains(x.STTChuyen_SanPham));
                        if (assignInDay != null && tpInDay != null)
                        {
                            #region Lấy thông tin
                            var btps = db.BTPs.Where(x => !x.IsDeleted && !x.IsBTP_PB_HC && assignIds.Contains(x.STTChuyen_SanPham) && x.IsEndOfLine).ToList();
                            NangXuat ns;
                            ThanhPham tp;
                            foreach (var assignObj in assignInDay)
                            {
                                ns = proInday.FirstOrDefault(x => x.STTCHuyen_SanPham == assignObj.AssignId);
                                tp = tpInDay.FirstOrDefault(x => x.STTChuyen_SanPham == assignObj.AssignId);

                                if (!hienThiDenTheoTPThoatChuyen)
                                    assignObj.lightPercent = (ns.NhipDoThucTe == 0 || ns.NhipDoSanXuat == 0) ? 0 : Math.Round(((ns.NhipDoSanXuat / ns.NhipDoThucTe) * 100));
                                else
                                    assignObj.lightPercent = (ns.NhipDoThucTeBTPThoatChuyen == 0 || ns.NhipDoSanXuat == 0) ? 0 : Math.Round(((ns.NhipDoSanXuat / ns.NhipDoThucTeBTPThoatChuyen) * 100));
                                assignObj.IsShowLCD = tp.ShowLCD;
                                assignObj.ProductionNorms = ns.DinhMucNgay;
                                if (assignObj.ProductionNorms < 0) assignObj.ProductionNorms = 0;

                                assignObj.KCSInDay = (ns.ThucHienNgay - ns.ThucHienNgayGiam);
                                if (assignObj.KCSInDay < 0) assignObj.KCSInDay = 0;

                                assignObj.TCInDay = ns.BTPThoatChuyenNgay - ns.BTPThoatChuyenNgayGiam;
                                if (assignObj.TCInDay < 0) assignObj.TCInDay = 0;

                                assignObj.KCSPercent = (assignObj.KCSInDay == 0 || assignObj.ProductionNorms == 0) ? 0 : Math.Round(((assignObj.KCSInDay / assignObj.ProductionNorms) * 100), 0);
                                if (assignObj.KCSPercent < 0) assignObj.KCSPercent = 0;

                                assignObj.Labours = tp.LaoDongChuyen;

                                int tang = 0, giam = 0;
                                //BTP trong ngày
                                tang = btps.Where(x => x.Ngay == dateNow && x.CommandTypeId == (int)eCommandRecive.BTPIncrease && x.STTChuyen_SanPham == assignObj.AssignId).Sum(x => x.BTPNgay);
                                giam = btps.Where(x => x.Ngay == dateNow && x.CommandTypeId == (int)eCommandRecive.BTPReduce && x.STTChuyen_SanPham == assignObj.AssignId).Sum(x => x.BTPNgay);
                                assignObj.BTPInDay = tang - giam;
                                if (assignObj.BTPInDay < 0) assignObj.BTPInDay = 0;

                                // LK BTP
                                tang = btps.Where(x => x.CommandTypeId == (int)eCommandRecive.BTPIncrease && x.STTChuyen_SanPham == assignObj.AssignId).Sum(x => x.BTPNgay);
                                giam = btps.Where(x => x.CommandTypeId == (int)eCommandRecive.BTPReduce && x.STTChuyen_SanPham == assignObj.AssignId).Sum(x => x.BTPNgay);
                                assignObj.LK_BTP = tang - giam;
                                if (assignObj.LK_BTP < 0) assignObj.LK_BTP = 0;

                                assignObj.BTPInLine = ns.BTPTrenChuyen;
                                if (assignObj.BTPInLine < 0) assignObj.BTPInLine = 0;

                                assignObj.ProductionPace = ns.NhipDoSanXuat;
                                assignObj.CurrentPace = ns.NhipDoThucTe;
                                assignObj.BTPPerLabour = (assignObj.BTPInLine == 0 || assignObj.Labours == 0) ? 0 : Math.Round((((double)assignObj.BTPInLine / assignObj.Labours) * 100), 0);
                                if (assignObj.BTPPerLabour < 0) assignObj.BTPPerLabour = 0;

                                assignObj.ErrorInDay = ns.SanLuongLoi - ns.SanLuongLoiGiam;
                                if (assignObj.ErrorInDay < 0) assignObj.ErrorInDay = 0;

                                assignObj.Current_TC_Pace = ns.NhipDoThucTeBTPThoatChuyen;

                                assignObj.SalesDate = (assignObj.KCSInDay == 0 || assignObj.CommoPrice == 0) ? 0 : Math.Round((((double)assignObj.KCSInDay * assignObj.CommoPrice) * 100), 0);
                                if (assignObj.SalesDate < 0) assignObj.SalesDate = 0;

                                var listWorkHoursOfLine = BLLShift.GetListWorkHoursOfLineByLineId(assignObj.lineId);
                                if (listWorkHoursOfLine != null && listWorkHoursOfLine.Count > 0)
                                {
                                    #region thong tin gio
                                    var time = DateTime.Now.TimeOfDay;
                                    var currentTime = listWorkHoursOfLine.FirstOrDefault(x => time >= x.TimeStart && time < x.TimeEnd);
                                    if (currentTime != null)
                                    {
                                        var tdns = db.TheoDoiNgays.Where(c => c.STTChuyenSanPham == assignObj.AssignId && c.Date == dateNow && c.Time > currentTime.TimeStart && c.Time <= currentTime.TimeEnd && c.IsEndOfLine).Select(x => new DayInfoModel()
                                        {
                                            CommandTypeId = x.CommandTypeId,
                                            ThanhPham = x.ThanhPham,
                                            ProductOutputTypeId = x.ProductOutputTypeId
                                        }).ToList();
                                        if (tdns.Count > 0)
                                        {
                                            //KCS
                                            tang = tdns.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductIncrease && c.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(c => c.ThanhPham);
                                            giam = tdns.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductReduce && c.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(c => c.ThanhPham);
                                            assignObj.KCSInHours = tang - giam;
                                            if (assignObj.KCSInHours < 0) assignObj.KCSInHours = 0;

                                            //TC 
                                            tang = tdns.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductIncrease && c.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(c => c.ThanhPham);
                                            giam = tdns.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductReduce && c.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(c => c.ThanhPham);
                                            assignObj.TCInHours = tang - giam;
                                            if (assignObj.TCInHours < 0) assignObj.TCInHours = 0;
                                        }
                                    }
                                    #endregion
                                }
                            }
                            #endregion

                            #region trả lại trạng thái ko thay đổi cho năng xuất sau khi lấy thông tin xong
                            foreach (var item in proInday)
                            {
                                item.IsChange = 0;
                            }
                            db.SaveChanges();
                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //   throw ex;
            }
            return assignInDay;
        }


        public int[] GetAssignmentIds(string date, int floorId)
        {
            using (var db = new PMSEntities())
            {
                if (floorId == 0)
                    return db.NangXuats.Where(x => !x.IsDeleted && x.Ngay == date && !x.Chuyen_SanPham.IsFinish && x.Chuyen_SanPham.LuyKeBTPThoatChuyen > 0).Select(x => x.STTCHuyen_SanPham).ToArray();
                return db.NangXuats.Where(x => !x.IsDeleted && x.Ngay == date && !x.Chuyen_SanPham.IsFinish && x.Chuyen_SanPham.LuyKeBTPThoatChuyen > 0 && x.Chuyen_SanPham.Chuyen.FloorId == floorId).Select(x => x.STTCHuyen_SanPham).ToArray();
            }
        }


        /// <summary>
        /// lay thong tin ma chuyen nao co ma hang ket thuc trong ngay
        /// </summary>
        /// <param name="lineIds"></param>
        /// <returns></returns>
        public List<String> GetLineIsFinishProduction(int[] lineIds)
        {
            using (var db = new PMSEntities())
            {
                var str = new List<String>();
                var objs = db.Chuyen_SanPham.Where(x => !x.IsDelete && x.IsFinishNow && lineIds.Contains(x.MaChuyen));
                if (objs != null && objs.Count() > 0)
                {
                    foreach (var item in objs)
                    {
                        str.Add(item.MaChuyen.ToString());
                    }
                }
                return str;
            }
        }

        public void ChangeStatusIsFinishNowFromTrueToFalse(int[] lineIds)
        {
            using (var db = new PMSEntities())
            {
                var objs = db.Chuyen_SanPham.Where(x => !x.IsDelete && x.IsFinishNow && lineIds.Contains(x.MaChuyen));
                if (objs != null && objs.Count() > 0)
                {
                    foreach (var item in objs)
                    {
                        item.IsFinishNow = false;
                    }
                    db.SaveChanges();
                }
            }
        }

    }
}
