using GPRO.Ultilities;
using PMS.Business.Enum;
using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PMS.Business
{
    public class BLLProductivity
    {
        public static List<WorkingTimeModel> GetProductivitiesEachHourOfLine(DateTime time, int LineId, List<WorkingTimeModel> workingTimes)
        {
            try
            {
                var db = new PMSEntities();
                var ngay = time.Day + "/" + time.Month + "/" + time.Year;
                if (workingTimes != null && workingTimes.Count > 0)
                {
                    var productivities = db.TheoDoiNgays.Where(x => x.Date == ngay && x.MaChuyen == LineId);
                    if (productivities != null && productivities.Count() > 0)
                    {
                        foreach (var item in workingTimes)
                        {
                            //KCS
                            var kcs = productivities.Where(x => x.Time >= item.TimeStart && x.Time <= item.TimeEnd && x.CommandTypeId == (int)eCommandRecive.ProductIncrease && x.ProductOutputTypeId == (int)eProductOutputType.KCS && x.IsEndOfLine).Sum(x => x.ThanhPham);
                            var kcs_G = productivities.Where(x => x.Time >= item.TimeStart && x.Time <= item.TimeEnd && x.CommandTypeId == (int)eCommandRecive.ProductReduce && x.ProductOutputTypeId == (int)eProductOutputType.KCS && x.IsEndOfLine).Sum(x => x.ThanhPham);
                            kcs = kcs - kcs_G;
                            //TC
                            var tc = productivities.Where(x => x.Time >= item.TimeStart && x.Time <= item.TimeEnd && x.CommandTypeId == (int)eCommandRecive.ProductIncrease && x.ProductOutputTypeId == (int)eProductOutputType.TC && x.IsEndOfLine).Sum(x => x.ThanhPham);
                            var tc_G = productivities.Where(x => x.Time >= item.TimeStart && x.Time <= item.TimeEnd && x.CommandTypeId == (int)eCommandRecive.ProductReduce && x.ProductOutputTypeId == (int)eProductOutputType.TC && x.IsEndOfLine).Sum(x => x.ThanhPham);
                            tc = tc - tc_G;
                            //Error
                            var err = productivities.Where(x => x.Time >= item.TimeStart && x.Time <= item.TimeEnd && x.CommandTypeId == (int)eCommandRecive.ErrorIncrease && x.IsEndOfLine).Sum(x => x.ThanhPham);
                            var err_G = productivities.Where(x => x.Time >= item.TimeStart && x.Time <= item.TimeEnd && x.CommandTypeId == (int)eCommandRecive.ErrorReduce && x.IsEndOfLine).Sum(x => x.ThanhPham);
                            err = err - err_G;

                            item.KCS = kcs < 0 ? 0 : kcs;
                            item.TC = tc < 0 ? 0 : tc;
                            item.Error = err < 0 ? 0 : err;
                        }
                    }
                    return workingTimes;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<LineModel> GetProductiviesOfLinesInDay(List<int> LineIds, DateTime date, bool IsInHour, int GetType)
        {
            try
            {
                var db = new PMSEntities();

                var ngay = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                var lines = db.Chuyens.Where(x => LineIds.Contains(x.MaChuyen)).Select(x => new LineModel()
                {
                    MaChuyen = x.MaChuyen,
                    TenChuyen = x.TenChuyen
                }).ToList();

                // dinh muc ngay 
                var proBaseOnDays = db.NangXuats.Where(x => x.Ngay == ngay && LineIds.Contains(x.Chuyen_SanPham.MaChuyen)).Select(x => new ProductivitiesModel()
                {
                    STTCHuyen_SanPham = x.STTCHuyen_SanPham,
                    LineId = x.Chuyen_SanPham.MaChuyen,
                    DinhMucNgay = x.DinhMucNgay
                });

                if (lines.Count > 0)
                {
                    var end = date.TimeOfDay;
                    var start = new TimeSpan((end.Hours - 1), end.Minutes, end.Seconds);
                    List<DayInfoModel> products = null;
                    if (IsInHour)
                        products = db.TheoDoiNgays.Where(x => LineIds.Contains(x.MaChuyen) && x.Date == ngay && x.Time >= start && x.Time <= end && x.IsEndOfLine).Select(x => new DayInfoModel()
                        {
                            STT = x.STT,
                            MaChuyen = x.MaChuyen,
                            LineName = x.Chuyen.TenChuyen,
                            CumId = x.CumId,
                            ClusterName = x.Cum.TenCum,
                            STTChuyenSanPham = x.STTChuyenSanPham,
                            MaSanPham = x.MaSanPham,
                            CommoName = x.SanPham.TenSanPham,
                            ThanhPham = x.ThanhPham,
                            Time = x.Time,
                            Date = x.Date,
                            CommandTypeId = x.CommandTypeId,
                            ProductOutputTypeId = x.ProductOutputTypeId,
                            ErrorId = x.ErrorId,
                            IsEndOfLine = x.IsEndOfLine
                        }).ToList();
                    else
                        products = db.TheoDoiNgays.Where(x => LineIds.Contains(x.MaChuyen) && x.Date == ngay && x.IsEndOfLine).Select(x => new DayInfoModel()
                        {
                            STT = x.STT,
                            MaChuyen = x.MaChuyen,
                            LineName = x.Chuyen.TenChuyen,
                            CumId = x.CumId,
                            ClusterName = x.Cum.TenCum,
                            STTChuyenSanPham = x.STTChuyenSanPham,
                            MaSanPham = x.MaSanPham,
                            CommoName = x.SanPham.TenSanPham,
                            ThanhPham = x.ThanhPham,
                            Time = x.Time,
                            Date = x.Date,
                            CommandTypeId = x.CommandTypeId,
                            ProductOutputTypeId = x.ProductOutputTypeId,
                            ErrorId = x.ErrorId,
                            IsEndOfLine = x.IsEndOfLine
                        }).ToList();


                    //lỗi
                    IEnumerable<int> errorCodes = null;
                    IEnumerable<Error> errors = null;
                    if (GetType == (int)eGetType.Error)
                    {
                        errorCodes = products.Where(x => x.ErrorId.HasValue && x.ErrorId != null).Select(x => x.ErrorId.Value).Distinct();
                        errors = db.Errors.Where(x => errorCodes.Contains(x.Code.Value));
                    }

                    foreach (var item in lines)
                    {
                        var obj = proBaseOnDays.Where(x => x.LineId == item.MaChuyen).FirstOrDefault();
                        var baseDay = obj != null ? obj.DinhMucNgay : 0;
                        if (IsInHour)
                        {
                            var hours = BLLShift.GetTotalWorkingHourOfLine(item.MaChuyen).Hours;
                            item.ProBase = (int)(baseDay / hours);
                        }
                        else
                            item.ProBase = (int)baseDay;
                        switch (GetType)
                        {
                            case (int)eGetType.KCS:
                                var kcs = products.Where(x => x.MaChuyen == item.MaChuyen && x.CommandTypeId == (int)eCommandRecive.ProductIncrease && x.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(x => x.ThanhPham);
                                var kcs_G = products.Where(x => x.MaChuyen == item.MaChuyen && x.CommandTypeId == (int)eCommandRecive.ProductReduce && x.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(x => x.ThanhPham);
                                kcs = kcs - kcs_G;
                                item.KCS = kcs < 0 ? 0 : kcs;
                                break;
                            case (int)eGetType.TC:
                                var tc = products.Where(x => x.MaChuyen == item.MaChuyen && x.CommandTypeId == (int)eCommandRecive.ProductIncrease && x.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(x => x.ThanhPham);
                                var tc_G = products.Where(x => x.MaChuyen == item.MaChuyen && x.CommandTypeId == (int)eCommandRecive.ProductReduce && x.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(x => x.ThanhPham);
                                tc = tc - tc_G;
                                item.TC = tc < 0 ? 0 : tc;
                                break;
                            case (int)eGetType.Error:
                                item.Errors = new List<ErrorModel>();
                                var errorOfLine = products.Where(x => x.ErrorId != null && x.MaChuyen == item.MaChuyen);
                                if (errors != null && errors.Count() > 0)
                                {
                                    foreach (var err in errors)
                                    {
                                        var eT = errorOfLine.Where(x => x.CommandTypeId == (int)eCommandRecive.ErrorIncrease && x.ErrorId == err.Code).Sum(x => x.ThanhPham);
                                        var eG = errorOfLine.Where(x => x.CommandTypeId == (int)eCommandRecive.ErrorReduce && x.ErrorId == err.Code).Sum(x => x.ThanhPham);
                                        eT = eT - eG;
                                        item.Errors.Add(new ErrorModel() { Code = err.Code, Name = err.Name, Quantity = eT < 0 ? 0 : eT });
                                    }
                                }
                                break;
                        }
                    }
                }
                return lines;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// lấy thông tin sản suất trong ngày cho form theodoingay
        /// </summary>
        /// <param name="lineIds"></param>
        /// <param name="AppId"></param>
        /// <returns></returns>
        public static List<ProductivitiesInDayModel> GetProductivitiesInDay(List<int> lineIds, int AppId)
        {
            try
            {
                var db = new PMSEntities();
                var list = new List<ProductivitiesInDayModel>();
                var ngay = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                TimeSpan timeNow = DateTime.Now.TimeOfDay;

                var csp = db.NangXuats.Where(x => !x.IsDeleted && !x.Chuyen_SanPham.IsDelete && x.Ngay == ngay && !x.Chuyen_SanPham.SanPham.IsDelete && lineIds.Contains(x.Chuyen_SanPham.MaChuyen)).Select(x => new ChuyenSanPhamModel()
                {
                    STT = x.Chuyen_SanPham.STT,
                    STTThucHien = x.Chuyen_SanPham.STTThucHien,
                    Thang = x.Chuyen_SanPham.Thang,
                    Nam = x.Chuyen_SanPham.Nam,
                    MaChuyen = x.Chuyen_SanPham.MaChuyen,
                    LineName = x.Chuyen_SanPham.Chuyen.TenChuyen,
                    MaSanPham = x.Chuyen_SanPham.MaSanPham,
                    CommoName = x.Chuyen_SanPham.SanPham.TenSanPham,
                    Price = x.Chuyen_SanPham.SanPham.DonGia,
                    PriceCM = x.Chuyen_SanPham.SanPham.DonGiaCM,
                    SanLuongKeHoach = x.Chuyen_SanPham.SanLuongKeHoach,
                    //   NangXuatSanXuat = x.Chuyen_SanPham.NangXuatSanXuat,
                    LuyKeTH = x.Chuyen_SanPham.LuyKeTH,
                    LuyKeBTPThoatChuyen = x.Chuyen_SanPham.LuyKeBTPThoatChuyen,
                    IsFinish = x.Chuyen_SanPham.IsFinish,
                    IsFinishBTPThoatChuyen = x.Chuyen_SanPham.IsFinishBTPThoatChuyen,
                    IsFinishNow = x.Chuyen_SanPham.IsFinishNow,
                    IsMoveQuantityFromMorthOld = x.Chuyen_SanPham.IsMoveQuantityFromMorthOld,
                    TimeAdd = x.Chuyen_SanPham.TimeAdd,
                    NormsDay = x.DinhMucNgay,
                    TH_Day = x.ThucHienNgay,
                    TH_Day_G = x.ThucHienNgayGiam,
                    TC_Day = x.BTPThoatChuyenNgay,
                    TC_Day_G = x.BTPThoatChuyenNgayGiam,
                    Err_Day = x.SanLuongLoi,
                    Err_Day_G = x.SanLuongLoiGiam,
                    BTP_Day = x.BTPTang,
                    BTP_Day_G = x.BTPGiam,
                    BTPInLine = x.BTPTrenChuyen,
                    NhipSX = x.NhipDoSanXuat,
                    NhipTC = x.NhipDoThucTeBTPThoatChuyen,
                    NhipTT = x.NhipDoThucTe,
                }).ToList();

                if (csp != null && csp.Count() > 0)
                {
                    var cf = db.Configs.ToList();
                    var cfApp = db.Config_App.Where(x => x.AppId == AppId).ToList();

                    var config = cf.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eAppConfigName.KieuTinhNhipThucTe));
                    string cfType = "1", cfErrorType = "1";
                    if (config != null)
                    {
                        var cfA = cfApp.FirstOrDefault(x => x.ConfigId == config.Id);
                        cfType = cfA != null ? cfA.Value.Trim() : config.ValueDefault.Trim();
                    }
                    config = cf.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eAppConfigName.KieuTinhTyLeHangLoi));
                    if (config != null)
                    {
                        var cfA = cfApp.FirstOrDefault(x => x.ConfigId == config.Id);
                        cfErrorType = cfA != null ? cfA.Value.Trim() : config.ValueDefault.Trim();
                    }

                    var stts = csp.Select(x => x.STT).Distinct();
                    var thanhPhams = db.ThanhPhams.Where(x => stts.Contains(x.STTChuyen_SanPham) && x.Ngay == ngay).ToList();
                    var btps = db.BTPs.Where(x => !x.IsDeleted && x.IsEndOfLine && !x.IsBTP_PB_HC && stts.Contains(x.STTChuyen_SanPham)).ToList();
                    var errors = db.TheoDoiNgays.Where(x => x.IsEndOfLine && stts.Contains(x.STTChuyenSanPham) && x.Date == ngay).ToList();
                    var monthlyInfos = db.P_MonthlyProductionPlans.Where(x => !x.IsDeleted && x.Month == DateTime.Now.Month && x.Year == DateTime.Now.Year && stts.Contains(x.STT_C_SP)).ToList();
                    var configg = db.Config_App.FirstOrDefault(x => x.AppId == 11 && x.Config.Name == eAppConfigName.TypeOfCalculateRevenues);
                    string typeOfCalculateRevenues = "TH";
                    if (configg != null)
                        typeOfCalculateRevenues = configg.Value;
                    foreach (var id in lineIds)
                    {
                        var lineInfos = csp.Where(x => x.MaChuyen == id).OrderBy(x => x.STTThucHien);
                        if (lineInfos != null && lineInfos.Count() > 0)
                        {
                            foreach (var item in lineInfos)
                            {
                                var tp = thanhPhams.FirstOrDefault(x => x.STTChuyen_SanPham == item.STT);
                                int LK_btptang = 0, LK_btpgiam = 0;
                                //  item.Err_Day = errors.Where(x => x.STTChuyenSanPham == item.STT && x.MaChuyen == item.MaChuyen && x.MaSanPham == item.MaSanPham && x.CommandTypeId.Value == (int)eCommandRecive.ErrorIncrease).Sum(x => x.ThanhPham);
                                //   item.Err_Day_G = errors.Where(x => x.STTChuyenSanPham == item.STT && x.MaChuyen == item.MaChuyen && x.MaSanPham == item.MaSanPham && x.CommandTypeId.Value == (int)eCommandRecive.ErrorReduce).Sum(x => x.ThanhPham);
                                LK_btptang = btps.Where(x => x.CommandTypeId == (int)eCommandRecive.BTPIncrease && x.STTChuyen_SanPham == item.STT).Sum(x => x.BTPNgay);
                                LK_btpgiam = btps.Where(x => x.CommandTypeId == (int)eCommandRecive.BTPReduce && x.STTChuyen_SanPham == item.STT).Sum(x => x.BTPNgay);
                                LK_btptang = LK_btptang - LK_btpgiam;
                                item.TC_Day = item.TC_Day - item.TC_Day_G;
                                item.TH_Day = item.TH_Day - item.TH_Day_G;
                                item.Err_Day = item.Err_Day - item.Err_Day_G;
                                item.BTP_Day = item.BTP_Day - item.BTP_Day_G;

                                var obj = new ProductivitiesInDayModel();
                                obj.LineName = item.LineName;
                                obj.CommoName = item.CommoName;
                                obj.LaborInLine = tp != null ? tp.LaoDongChuyen : 0;
                                obj.ProductionPlans = item.SanLuongKeHoach;
                                obj.LK_TH = item.LuyKeTH;
                                obj.LK_TC = item.LuyKeBTPThoatChuyen;
                                obj.LK_BTP = LK_btptang < 0 ? 0 : LK_btptang;
                                obj.NormsOfDay = (int)Math.Round(item.NormsDay, 0);
                                obj.TH_Day = item.TH_Day < 0 ? 0 : item.TH_Day;
                                obj.TC_Day = item.TC_Day < 0 ? 0 : item.TC_Day;
                                obj.ErrorsInDay = item.Err_Day < 0 ? 0 : item.Err_Day;
                                obj.BTP_Day = item.BTP_Day < 0 ? 0 : item.BTP_Day;
                                obj.BTPInLine = item.BTPInLine < 0 ? 0 : item.BTPInLine;
                                obj.TH_Percent = item.TH_Day <= 0 || item.NormsDay <= 0 ? 0 : (int)((item.TH_Day / item.NormsDay) * 100);
                                switch (cfErrorType)
                                {
                                    case "1": obj.ErrorPercent = item.NormsDay > 0 && item.Err_Day > 0 ? (int)((item.Err_Day / item.NormsDay) * 100) : 0; break;
                                    case "2": obj.ErrorPercent = item.Err_Day > 0 ? (float)Math.Round((((double)item.Err_Day / ((double)item.Err_Day + (double)item.TH_Day)) * 100), 2) : 0; break;
                                    case "3": obj.ErrorPercent = item.Err_Day > 0 && item.TC_Day > 0 ? (float)Math.Round(((double)((double)item.Err_Day / (double)item.TC_Day) * 100), 2) : 0; break;
                                }
                                obj.Funds = item.BTPInLine > 0 ? (int)(Math.Ceiling((double)item.BTPInLine / tp.LaoDongChuyen)) : 0;

                                if (typeOfCalculateRevenues == "TH")
                                    obj.RevenuesInDay = (item.TH_Day > 0 && item.PriceCM > 0) ? Math.Ceiling(((double)item.TH_Day * item.PriceCM)) : 0;
                                else
                                    obj.RevenuesInDay = (item.TC_Day > 0 && item.PriceCM > 0) ? Math.Ceiling(((double)item.TC_Day * item.PriceCM)) : 0;

                                var monthInfo = monthlyInfos.FirstOrDefault(x => x.STT_C_SP == item.STT);
                                obj.RevenuesInMonth = monthInfo == null ? 0 : (monthInfo.LK_TH > 0 && item.PriceCM > 0 ? Math.Ceiling((double)monthInfo.LK_TH * item.PriceCM) : 0);
                                obj.ResearchPaced = (int)item.NhipSX;

                                double tgLVToiHienTai = BLLShift.TGLamViecToiHienTai(id).TotalSeconds;

                                if (cfType == "1")
                                {
                                    item.NhipTT = (obj.TH_Day > 0 ? tgLVToiHienTai / obj.TH_Day : 0);
                                    obj.CurrentPacedProduction = (int)item.NhipTT;
                                    obj.TC_Paced = item.NhipTT > 0 ? (int)((item.NhipSX / item.NhipTT) * 100) : 0;
                                }
                                else
                                {
                                    item.NhipTC = (obj.TC_Day > 0 ? tgLVToiHienTai / obj.TC_Day : 0);
                                    obj.CurrentPacedProduction = (int)item.NhipTC;
                                    obj.TC_Paced = item.NhipTC > 0 ? (int)((item.NhipSX / item.NhipTC) * 100) : 0;
                                }
                                obj.IsFinish = item.IsFinish;
                                list.Add(obj);
                            }
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static NangXuat CheckIsHasProductivity(int sttC_SP)
        {
            try
            {
                var db = new PMSEntities();
                var now = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                return db.NangXuats.FirstOrDefault(x => !x.IsDeleted && x.STTCHuyen_SanPham == sttC_SP && x.ThucHienNgay > 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static NangXuat TTNangXuatTrongNgay(string ngay, int sttChuyenSanPham)
        {
            try
            {
                var db = new PMSEntities();
                if (!string.IsNullOrEmpty(ngay))
                    return db.NangXuats.FirstOrDefault(x => !x.IsDeleted && x.Ngay == ngay && x.STTCHuyen_SanPham == sttChuyenSanPham);
            }
            catch (Exception) { }
            return null;
        }

        public static List<ProductivitiesModel> TTNangXuatTrongNgay(string ngay, int[] lineIds)
        {
            try
            {
                var db = new PMSEntities();
                if (!string.IsNullOrEmpty(ngay))
                    return db.NangXuats.Where(x => !x.IsDeleted && x.Ngay == ngay && lineIds.Contains(x.Chuyen_SanPham.MaChuyen)).Select(x => new ProductivitiesModel()
                    {
                        ThucHienNgay = x.ThucHienNgay - x.ThucHienNgayGiam,
                        BTPThoatChuyenNgay = x.BTPThoatChuyenNgay - x.BTPThoatChuyenNgayGiam,
                        OrderIndex = x.Chuyen_SanPham.STTThucHien,
                        STTCHuyen_SanPham = x.STTCHuyen_SanPham,
                        LineId = x.Chuyen_SanPham.MaChuyen
                    }).ToList();
            }
            catch (Exception) { }
            return null;
        }

        public static List<NangXuat> GetAllNSOfPCC(int sttChuyenSanPham)
        {
            try
            {
                var db = new PMSEntities();
                var objs = db.NangXuats.Where(x => !x.IsDeleted && x.STTCHuyen_SanPham == sttChuyenSanPham).OrderByDescending(x => x.Ngay);
                if (objs != null && objs.Count() > 0)
                {
                    foreach (var item in objs)
                    {
                        if ((item.BTPThoatChuyenNgay - item.BTPThoatChuyenNgayGiam) < 0)
                        {
                            item.BTPThoatChuyenNgay = 0;
                            item.BTPThoatChuyenNgayGiam = 0;
                        }

                        if ((item.ThucHienNgay - item.ThucHienNgayGiam) < 0)
                        {
                            item.ThucHienNgay = 0;
                            item.ThucHienNgayGiam = 0;
                        }

                        if ((item.BTPTang - item.BTPGiam) < 0)
                        {
                            item.BTPTang = 0;
                            item.BTPGiam = 0;
                        }
                    }
                    db.SaveChanges();
                    return objs.ToList();
                }
            }
            catch (Exception ex)
            { }
            return null;
        }

        public static NangXuat CheckExistProductivityWork(int sttCSP, string ngay)
        {
            try
            {
                var db = new PMSEntities();
                return db.NangXuats.FirstOrDefault(x => !x.IsDeleted && x.STTCHuyen_SanPham == sttCSP && x.Ngay == ngay);
            }
            catch (Exception) { }
            return null;
        }


        //public static bool UpdateBTPTrenChuyenFollowConfig(int config, int lineId, bool isUpdateAll)
        //{
        //    try
        //    {
        //        var db = new PMSEntities();
        //        var now = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
        //        var nsObjs = db.NangXuats.Where(x => x.Ngay == now && !x.Chuyen_SanPham.IsDelete && !x.Chuyen_SanPham.IsFinish);
        //        if (nsObjs != null && nsObjs.Count() > 0)
        //        {
        //            var stts = nsObjs.Select(x => x.STTCHuyen_SanPham).Distinct().ToList();
        //            var btps = db.BTPs.Where(x => stts.Contains(x.STTChuyen_SanPham)).ToList();
        //            var csps = db.Chuyen_SanPham.Where(x => stts.Contains(x.STT)).ToList();
        //            var thanhphams = db.ThanhPhams.Where(x => stts.Contains(x.STTChuyen_SanPham) && x.Ngay == now).ToList();
        //            int tang = 0, giam = 0;

        //            foreach (var ns in nsObjs)
        //            {
        //                tang = btps.Where(x => x.CommandTypeId == (int)eCommandRecive.BTPIncrease && x.STTChuyen_SanPham == ns.STTCHuyen_SanPham).Sum(x => x.BTPNgay);
        //                giam = btps.Where(x => x.CommandTypeId == (int)eCommandRecive.BTPReduce && x.STTChuyen_SanPham == ns.STTCHuyen_SanPham).Sum(x => x.BTPNgay);
        //                var csp = csps.FirstOrDefault(x => x.STT == ns.STTCHuyen_SanPham);
        //                switch (config)
        //                {
        //                    case 1:
        //                        ns.BTPTrenChuyen = (tang - giam) - csp.LuyKeTH;
        //                        break;
        //                    case 2:
        //                        ns.BTPTrenChuyen = (tang - giam) - csp.LuyKeBTPThoatChuyen;
        //                        break;
        //                }

        //                // cap nhat dinh muc ngay
        //                var shift = BLLShift.GetTotalWorkingHourOfLine(csp.MaChuyen);
        //                var tp = thanhphams.FirstOrDefault(x => x.STTChuyen_SanPham == ns.STTCHuyen_SanPham);
        //                if (tp != null)
        //                {
        //                    tp.NangXuatLaoDong = shift.TotalSeconds / csp.NangXuatSanXuat;
        //                    ns.DinhMucNgay = tp.NangXuatLaoDong * tp.LaoDongChuyen;
        //                    var sanluongConLai = csp.SanLuongKeHoach - (csp.LuyKeTH - (ns.ThucHienNgay - ns.ThucHienNgayGiam));
        //                    if (ns.DinhMucNgay > sanluongConLai)
        //                        ns.DinhMucNgay = sanluongConLai;
        //                }
        //            }
        //            db.SaveChanges();
        //            return true;
        //        }
        //    }
        //    catch (Exception)
        //    { }
        //    return false;
        //} 

        /// <summary>
        /// Reset lai thong tin Định mức ngày và bTp trên chuyen (Hai - 19/9/2016)
        /// </summary>
        /// <param name="config">config tinh btp trên chuyền</param>
        /// <param name="calculateNormsdayType">cách tinh dinh muc ngay</param>
        /// <param name="TypeOfCalculateNormsday">kiểu so sanh  dinh muc ngay</param>
        /// <param name="lineId">neu update tat ca chuyen thi  = 0 or truyen vao mã chuyền</param>
        /// <param name="isUpdateAll">up tat ca cac chuyền hay chỉ 1 chuyền</param>
        /// <param name="date">ngay</param>
        public static void ResetNormsDayAndBTPInLine(int config, int calculateNormsdayType, int TypeOfCalculateNormsday, int lineId, bool isUpdateAll, string date)
        {
            try
            {
                var db = new PMSEntities();
                List<ProductivitiesModel> NSModels;
                List<ProductivitiesModel> NSModelsOfLine;
                List<NangXuat> NSs = new List<NangXuat>();
                List<BTP> btps;
                List<ThanhPham> tps;
                ThanhPham tp;
                NangXuat nx;
                Chuyen_SanPham cspObj;
                int workingTime = 0, workingTimeFull = 0, tang = 0, giam = 0;
                double dinhmuc = 0;
                #region get data
                if (isUpdateAll)
                {
                    NSModels = db.NangXuats.Where(x => !x.IsDeleted && x.Ngay == date).OrderBy(x => x.Chuyen_SanPham.STTThucHien).Select(x => new ProductivitiesModel()
                    {
                        Id = x.Id,
                        Ngay = x.Ngay,
                        STTCHuyen_SanPham = x.STTCHuyen_SanPham,
                        BTPGiam = x.BTPGiam,
                        BTPLoi = x.BTPLoi,
                        BTPTang = x.BTPTang,
                        BTPThoatChuyenNgay = x.BTPThoatChuyenNgay,
                        BTPThoatChuyenNgayGiam = x.BTPThoatChuyenNgayGiam,
                        BTPTrenChuyen = x.BTPTrenChuyen,
                        DinhMucNgay = x.DinhMucNgay,
                        IsBTP = x.IsBTP,
                        IsChange = x.IsChange,
                        IsChangeBTP = x.IsChangeBTP,
                        IsEndDate = x.IsEndDate,
                        IsStopOnDay = x.IsStopOnDay,
                        NhipDoSanXuat = x.NhipDoSanXuat,
                        NhipDoThucTe = x.NhipDoThucTe,
                        NhipDoThucTeBTPThoatChuyen = x.NhipDoThucTeBTPThoatChuyen,
                        SanLuongLoi = x.SanLuongLoi,
                        SanLuongLoiGiam = x.SanLuongLoiGiam,
                        ThucHienNgay = x.ThucHienNgay,
                        ThucHienNgayGiam = x.ThucHienNgayGiam,
                        TimeLastChange = x.TimeLastChange,
                        TimeStopOnDay = x.TimeStopOnDay,
                        productId = x.Chuyen_SanPham.SanPham.MaSanPham,
                        ProductName = x.Chuyen_SanPham.SanPham.TenSanPham,
                        ProductPrice = x.Chuyen_SanPham.SanPham.DonGia,
                        ProductPriceCM = x.Chuyen_SanPham.SanPham.DonGiaCM,
                        LineId = x.Chuyen_SanPham.MaChuyen,
                        LineName = x.Chuyen_SanPham.Chuyen.TenChuyen,
                        IdDenNangSuat = x.Chuyen_SanPham.Chuyen.IdDenNangSuat,
                        LaborsBase = x.Chuyen_SanPham.Chuyen.LaoDongDinhBien,
                        NangSuatSanXuat = x.Chuyen_SanPham.SanPham.ProductionTime,
                        ProductionPlans = x.Chuyen_SanPham.SanLuongKeHoach,
                        LK_TH = x.Chuyen_SanPham.LuyKeTH,
                        LK_TC = x.Chuyen_SanPham.LuyKeBTPThoatChuyen,
                        OrderIndex = x.Chuyen_SanPham.STTThucHien,
                        TGCheTaoSP = x.TGCheTaoSP
                    }).ToList();
                    if (NSModels.Count > 0)
                        NSs = db.NangXuats.Where(x => !x.IsDeleted && x.Ngay == date).OrderBy(x => x.Chuyen_SanPham.STTThucHien).ToList();
                }
                else
                {
                    NSModels = db.NangXuats.Where(x => !x.IsDeleted && x.Ngay == date && x.Chuyen_SanPham.MaChuyen == lineId).OrderBy(x => x.Chuyen_SanPham.STTThucHien).Select(x => new ProductivitiesModel()
                    {
                        Id = x.Id,
                        Ngay = x.Ngay,
                        STTCHuyen_SanPham = x.STTCHuyen_SanPham,
                        BTPGiam = x.BTPGiam,
                        BTPLoi = x.BTPLoi,
                        BTPTang = x.BTPTang,
                        BTPThoatChuyenNgay = x.BTPThoatChuyenNgay,
                        BTPThoatChuyenNgayGiam = x.BTPThoatChuyenNgayGiam,
                        BTPTrenChuyen = x.BTPTrenChuyen,
                        DinhMucNgay = x.DinhMucNgay,
                        IsBTP = x.IsBTP,
                        IsChange = x.IsChange,
                        IsChangeBTP = x.IsChangeBTP,
                        IsEndDate = x.IsEndDate,
                        IsStopOnDay = x.IsStopOnDay,
                        NhipDoSanXuat = x.NhipDoSanXuat,
                        NhipDoThucTe = x.NhipDoThucTe,
                        NhipDoThucTeBTPThoatChuyen = x.NhipDoThucTeBTPThoatChuyen,
                        SanLuongLoi = x.SanLuongLoi,
                        SanLuongLoiGiam = x.SanLuongLoiGiam,
                        ThucHienNgay = x.ThucHienNgay,
                        ThucHienNgayGiam = x.ThucHienNgayGiam,
                        TimeLastChange = x.TimeLastChange,
                        TimeStopOnDay = x.TimeStopOnDay,
                        productId = x.Chuyen_SanPham.SanPham.MaSanPham,
                        ProductName = x.Chuyen_SanPham.SanPham.TenSanPham,
                        ProductPrice = x.Chuyen_SanPham.SanPham.DonGia,
                        ProductPriceCM = x.Chuyen_SanPham.SanPham.DonGiaCM,
                        LineId = x.Chuyen_SanPham.MaChuyen,
                        LineName = x.Chuyen_SanPham.Chuyen.TenChuyen,
                        IdDenNangSuat = x.Chuyen_SanPham.Chuyen.IdDenNangSuat,
                        LaborsBase = x.Chuyen_SanPham.Chuyen.LaoDongDinhBien,
                        NangSuatSanXuat = x.Chuyen_SanPham.SanPham.ProductionTime,
                        ProductionPlans = x.Chuyen_SanPham.SanLuongKeHoach,
                        LK_TH = x.Chuyen_SanPham.LuyKeTH,
                        LK_TC = x.Chuyen_SanPham.LuyKeBTPThoatChuyen,
                        OrderIndex = x.Chuyen_SanPham.STTThucHien,
                        TGCheTaoSP = x.TGCheTaoSP
                    }).ToList();
                    if (NSModels.Count > 0)
                        NSs = db.NangXuats.Where(x => !x.IsDeleted && x.Ngay == date && x.Chuyen_SanPham.MaChuyen == lineId).OrderBy(x => x.Chuyen_SanPham.STTThucHien).ToList();
                }
                #endregion

                if (NSModels.Count > 0)
                {
                    var stts = NSModels.Select(x => x.STTCHuyen_SanPham).Distinct().ToList();
                    var csp = db.Chuyen_SanPham.Where(x => !x.IsDelete && !x.IsFinish && stts.Contains(x.STT));
                    btps = db.BTPs.Where(x => !x.IsDeleted && stts.Contains(x.STTChuyen_SanPham)).ToList();
                    tps = db.ThanhPhams.Where(x => !x.IsDeleted && x.Ngay == date && stts.Contains(x.STTChuyen_SanPham)).ToList();
                    var tdns = db.TheoDoiNgays.Where(x => x.Date == date).ToList();
                    foreach (var line in NSModels.Select(x => x.LineId).Distinct())
                    {
                        NSModelsOfLine = NSModels.Where(x => x.LineId == line).OrderBy(x => x.OrderIndex).ToList();
                        workingTimeFull = (int)BLLShift.GetTotalWorkingHourOfLine(line).TotalSeconds;
                        workingTime = workingTimeFull;
                        foreach (var item in NSModelsOfLine)
                        {
                            nx = NSs.FirstOrDefault(x => x.Id == item.Id);
                            tp = tps.FirstOrDefault(x => x.STTChuyen_SanPham == item.STTCHuyen_SanPham);

                            // item.NangSuatSanXuat = Math.Round((item.NangSuatSanXuat * 100) / tp.HieuSuat);
                            // tp.NangXuatLaoDong = (float)Math.Round((workingTimeFull / item.NangSuatSanXuat), 1);

                            #region Dinh Muc Ngay
                            if (workingTime > 0)
                            {
                                if (tp != null)
                                {
                                    if (calculateNormsdayType == 0) // => tính riêng lẻ từng mã riêng biệt
                                        nx.DinhMucNgay = Math.Round((workingTimeFull / (double)item.TGCheTaoSP) * tp.LaoDongChuyen);
                                    else
                                    //=> tinh cộng dồn tất cả các mã trong ngày
                                    {
                                        dinhmuc = Math.Round((workingTime / (double)item.TGCheTaoSP) * tp.LaoDongChuyen);

                                        if ((item.ProductionPlans - (TypeOfCalculateNormsday == 1 ? (item.LK_TH - (nx.ThucHienNgay - nx.ThucHienNgayGiam)) : (item.LK_TC - (nx.BTPThoatChuyenNgay - nx.BTPThoatChuyenNgayGiam)))) > dinhmuc)
                                        {
                                            workingTime = 0;
                                            nx.DinhMucNgay = dinhmuc;
                                        }
                                        else
                                        {
                                            nx.DinhMucNgay = (item.ProductionPlans - (TypeOfCalculateNormsday == 1 ? (item.LK_TH - (nx.ThucHienNgay - nx.ThucHienNgayGiam)) : (item.LK_TC - (nx.BTPThoatChuyenNgay - nx.BTPThoatChuyenNgayGiam))));
                                            nx.IsEndDate = true;
                                            if (calculateNormsdayType != 0)
                                            {
                                                var nangSuatLaoDongNow = nx.DinhMucNgay / tp.LaoDongChuyen;
                                                int totalSecondFinishMH1 = (int)(nangSuatLaoDongNow * (double)item.TGCheTaoSP);
                                                workingTime = workingTime - totalSecondFinishMH1;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                nx.DinhMucNgay = 0;
                                nx.IsEndDate = false;
                            }
                            if (nx.DinhMucNgay < 0)
                                nx.DinhMucNgay = 0;
                            nx.NhipDoSanXuat = (float)Math.Round(((double)item.TGCheTaoSP / tp.LaoDongChuyen), 1);
                            nx.TimeLastChange = DateTime.Now.TimeOfDay;
                            #endregion

                            #region BTP trên chuyền
                            tang = btps.Where(x => x.CommandTypeId == (int)eCommandRecive.BTPIncrease && !x.IsBTP_PB_HC && x.STTChuyen_SanPham == nx.STTCHuyen_SanPham).Sum(x => x.BTPNgay);
                            giam = btps.Where(x => x.CommandTypeId == (int)eCommandRecive.BTPReduce && !x.IsBTP_PB_HC && x.STTChuyen_SanPham == nx.STTCHuyen_SanPham).Sum(x => x.BTPNgay);
                            switch (config)
                            {
                                case 1:
                                    nx.BTPTrenChuyen = (tang - giam) - item.LK_TH;
                                    break;
                                case 2:
                                    nx.BTPTrenChuyen = (tang - giam) - item.LK_TC;
                                    break;
                            }

                            if (csp != null)
                            {
                                cspObj = csp.FirstOrDefault(x => x.STT == item.STTCHuyen_SanPham);
                                if (cspObj != null)
                                {
                                    cspObj.LK_BTP = (tang - giam);
                                    tang = btps.Where(x => x.CommandTypeId == (int)eCommandRecive.BTPIncrease && x.IsBTP_PB_HC && x.STTChuyen_SanPham == nx.STTCHuyen_SanPham).Sum(x => x.BTPNgay);
                                    giam = btps.Where(x => x.CommandTypeId == (int)eCommandRecive.BTPReduce && x.IsBTP_PB_HC && x.STTChuyen_SanPham == nx.STTCHuyen_SanPham).Sum(x => x.BTPNgay);
                                    cspObj.LK_BTP_HC = (tang - giam);

                                    if (cspObj.LuyKeTH > cspObj.SanLuongKeHoach)
                                        cspObj.LuyKeTH = cspObj.SanLuongKeHoach;
                                    if (cspObj.LuyKeBTPThoatChuyen > cspObj.SanLuongKeHoach)
                                        cspObj.LuyKeBTPThoatChuyen = cspObj.SanLuongKeHoach;
                                    if (cspObj.LK_BTP > cspObj.SanLuongKeHoach)
                                        cspObj.LK_BTP = cspObj.SanLuongKeHoach;
                                    if (cspObj.LK_BTP_HC > cspObj.SanLuongKeHoach)
                                        cspObj.LK_BTP_HC = cspObj.SanLuongKeHoach;
                                }
                            }
                            #endregion
                        }
                    }
                    db.SaveChanges();
                }

                var csps = db.Chuyen_SanPham.Where(x => x.IsFinish && x.STT != 900);
                if (csps != null && csps.Count() > 0)
                    foreach (var item in csps)
                    {
                        item.STTThucHien = 900;
                    }
                db.SaveChanges();
            }
            catch (Exception)
            { }
        }


        public static bool InsertOrUpdateNangXuat(NangXuat obj)
        {
            try
            {
                var db = new PMSEntities();
                if (obj.Id == 0)
                {
                    obj.CreatedDate = DateTime.Now;
                    db.NangXuats.Add(obj);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    var nx = db.NangXuats.FirstOrDefault(x => !x.IsDeleted && x.Ngay == obj.Ngay && x.STTCHuyen_SanPham == obj.STTCHuyen_SanPham);
                    if (nx != null)
                    {
                        nx.DinhMucNgay = obj.DinhMucNgay;
                        nx.NhipDoSanXuat = obj.NhipDoSanXuat;
                        nx.TimeLastChange = obj.TimeLastChange;
                        nx.IsEndDate = obj.IsEndDate;
                        nx.IsStopOnDay = obj.IsStopOnDay;
                        nx.TimeStopOnDay = obj.TimeStopOnDay;
                        nx.UpdatedDate = DateTime.Now;
                        db.SaveChanges();
                        return true;
                    }
                }
            }
            catch (Exception ex) { }
            return false;
        }

        public static bool UpdateNangXuat(NangXuat obj)
        {
            try
            {
                var db = new PMSEntities();
                var ns = db.NangXuats.FirstOrDefault(x => !x.IsDeleted && x.Ngay == obj.Ngay && x.STTCHuyen_SanPham == obj.STTCHuyen_SanPham);
                if (ns != null)
                {
                    ns.ThucHienNgay = obj.ThucHienNgay;
                    ns.ThucHienNgayGiam = obj.ThucHienNgayGiam;
                    ns.BTPTrenChuyen = obj.BTPTrenChuyen;
                    ns.NhipDoThucTe = obj.NhipDoThucTe;
                    ns.NhipDoThucTeBTPThoatChuyen = obj.NhipDoThucTeBTPThoatChuyen;
                    ns.TimeLastChange = DateTime.Now.TimeOfDay;
                    ns.IsBTP = obj.IsBTP;
                    ns.IsChange = 1;
                    ns.BTPTang = obj.BTPTang;
                    ns.BTPGiam = obj.BTPGiam;
                    ns.BTPThoatChuyenNgay = obj.BTPThoatChuyenNgay;
                    ns.BTPThoatChuyenNgayGiam = obj.BTPThoatChuyenNgayGiam;
                    ns.SanLuongLoi = obj.SanLuongLoi;
                    ns.SanLuongLoiGiam = obj.SanLuongLoiGiam;
                    ns.UpdatedDate = DateTime.Now;
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception) { }
            return false;
        }

        public static ThanhPham GetThanhPhamByNgayAndSTT(string Ngay, int STTChuyen_SanPham)
        {
            try
            {
                var db = new PMSEntities();
                if (!string.IsNullOrEmpty(Ngay))
                    return db.ThanhPhams.FirstOrDefault(x => !x.IsDeleted && x.Ngay == Ngay && x.STTChuyen_SanPham == STTChuyen_SanPham);
            }
            catch (Exception) { }
            return null;
        }

        public static ThanhPham FindThanhPham(int sttCSP, string ngay)
        {
            try
            {
                var db = new PMSEntities();
                return db.ThanhPhams.FirstOrDefault(x => !x.IsDeleted && x.STTChuyen_SanPham == sttCSP && x.Ngay == ngay);
            }
            catch (Exception) { }
            return null;
        }

        public static List<ThanhPhamModel> GetDailyWorkerInformation(int lineId, DateTime date)
        {
            try
            {
                var db = new PMSEntities();
                string dateNow = date.Day + "/" + date.Month + "/" + date.Year;
                return db.ThanhPhams.Where(x => !x.IsDeleted && !x.Chuyen_SanPham.IsDelete && !x.Chuyen_SanPham.IsFinish && !x.Chuyen_SanPham.SanPham.IsDelete && x.Chuyen_SanPham.MaChuyen == lineId && x.Ngay == dateNow).Select(x => new ThanhPhamModel()
                {
                    Id = x.Id,
                    LaoDongChuyen = x.LaoDongChuyen,
                    NangXuatLaoDong = x.NangXuatLaoDong,
                    CommoId = x.Chuyen_SanPham.MaSanPham,
                    CommoName = x.Chuyen_SanPham.SanPham.TenSanPham,
                    STTChuyen_SanPham = x.STTChuyen_SanPham,
                    LeanKH = x.LeanKH,
                    ShowLCD = x.ShowLCD,
                    HieuSuat = x.HieuSuat,
                    LDOff = x.LDOff,
                    LDNew = x.LDNew,
                    LDPregnant = x.LDPregnant,
                    LDVacation = x.LDVacation
                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new List<ThanhPhamModel>();
        }

        public static ThanhPham GetTPByWorkDayOld(int STTChuyen_SanPham)
        {
            try
            {
                var db = new PMSEntities();
                return db.ThanhPhams.Where(x => !x.IsDeleted && x.STTChuyen_SanPham == STTChuyen_SanPham).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool UpdateThanhPham(ThanhPham obj)
        {
            try
            {
                var db = new PMSEntities();
                var tp = db.ThanhPhams.FirstOrDefault(x => !x.IsDeleted && x.STTChuyen_SanPham == obj.STTChuyen_SanPham && x.Ngay == obj.Ngay);
                if (tp != null)
                {
                    tp.NangXuatLaoDong = obj.NangXuatLaoDong;
                    tp.LaoDongChuyen = obj.LaoDongChuyen;
                    tp.LeanKH = obj.LeanKH;
                }
                else
                    db.ThanhPhams.Add(obj);

                db.SaveChanges();
                return true;
            }
            catch (Exception) { }
            return false;
        }

        public static bool InsertThanhPham(ThanhPham obj)
        {
            try
            {
                var db = new PMSEntities();
                obj.CreatedDate = DateTime.Now;
                db.ThanhPhams.Add(obj);
                db.SaveChanges();
                return true;
            }
            catch (Exception) { }
            return false;
        }

        public static bool AddNangSuatCumOfChuyen(int sttChuyenSanPham, int maChuyen)
        {
            try
            {
                var db = new PMSEntities();
                string dateNow = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                var listCluster = BLLProductivity.GetClustersOfLine(maChuyen);
                if (listCluster != null && listCluster.Count > 0)
                {
                    var clusterIds = listCluster.Select(x => x.Id).Distinct();
                    var ns_cums = db.NangSuat_Cum.Where(x => !x.IsDeleted && clusterIds.Contains(x.IdCum) && x.STTChuyen_SanPham == sttChuyenSanPham && x.Ngay == dateNow).ToList();
                    NangSuat_Cum obj;
                    foreach (var c in listCluster)
                    {
                        var check = ns_cums.FirstOrDefault(x => x.IdCum == c.Id);
                        if (check == null)
                        {
                            obj = new NangSuat_Cum();
                            obj.Ngay = dateNow;
                            obj.STTChuyen_SanPham = sttChuyenSanPham;
                            obj.IdCum = c.Id;
                            db.NangSuat_Cum.Add(obj);
                        }
                    }
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool AddNangSuatCumLoiOfChuyen(int sttChuyenSanPham, int maChuyen)
        {
            try
            {
                var db = new PMSEntities();
                string dateNow = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                var clusters = BLLProductivity.GetClustersOfLine(maChuyen);
                var errors = BLLError.GetAll();
                if (clusters != null && clusters.Count > 0 && errors != null && errors.Count > 0)
                {
                    var clusterIds = clusters.Select(x => x.Id).Distinct();
                    var ns_cumlois = db.NangSuat_CumLoi.Where(x => !x.IsDeleted && clusterIds.Contains(x.CumId) && x.STTChuyenSanPham == sttChuyenSanPham && x.Ngay == dateNow).ToList();
                    NangSuat_CumLoi obj;
                    foreach (var c in clusters)
                    {
                        foreach (var err in errors)
                        {
                            var check = ns_cumlois.FirstOrDefault(x => x.CumId == c.Id && x.ErrorId == err.Id);
                            if (check == null)
                            {
                                obj = new NangSuat_CumLoi();
                                obj.Ngay = dateNow;
                                obj.STTChuyenSanPham = sttChuyenSanPham;
                                obj.CumId = c.Id;
                                obj.ErrorId = err.Id;
                                db.NangSuat_CumLoi.Add(obj);
                            }
                        }
                    }
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static NangSuat_Cum Find_NangSuatCum(int sttCSP, int clusterId, string ngay)
        {
            try
            {
                var db = new PMSEntities();
                return db.NangSuat_Cum.FirstOrDefault(x => !x.IsDeleted && x.STTChuyen_SanPham == sttCSP && x.Ngay == ngay && x.IdCum == clusterId);
            }
            catch (Exception) { }
            return null;
        }

        public static NangSuat_Cum Find_NangSuatCum(int Id)
        {
            try
            {
                var db = new PMSEntities();
                return db.NangSuat_Cum.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
            }
            catch (Exception) { }
            return null;
        }

        public static bool Update_NS_Cum(NangSuat_Cum obj)
        {
            try
            {
                var db = new PMSEntities();
                var nsc = db.NangSuat_Cum.FirstOrDefault(x => !x.IsDeleted && x.Id == obj.Id);
                if (nsc != null)
                {
                    nsc.SanLuongKCSTang = obj.SanLuongKCSTang;
                    nsc.SanLuongKCSGiam = obj.SanLuongKCSGiam;

                    nsc.SanLuongTCTang = obj.SanLuongTCTang;
                    nsc.SanLuongTCGiam = obj.SanLuongTCGiam;

                    nsc.BTPTang = obj.BTPTang;
                    nsc.BTPGiam = obj.BTPGiam;
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            { }
            return false;
        }

        public static List<Cum> GetClustersOfLine(int lineId)
        {
            try
            {
                var db = new PMSEntities();
                return db.Cums.Where(x => !x.IsDeleted && !x.Chuyen.IsDeleted && x.IdChuyen == lineId).ToList();
            }
            catch (Exception) { }
            return null;
        }

        public static NangSuat_CumLoi Find_NangSuatCumLoi(int clusterId, int errorId, int sttChuyenSanPham, string ngay)
        {
            try
            {
                var db = new PMSEntities();
                return db.NangSuat_CumLoi.FirstOrDefault(x => !x.IsDeleted && x.STTChuyenSanPham == sttChuyenSanPham && x.Ngay == ngay && x.CumId == clusterId && x.ErrorId == errorId);
            }
            catch (Exception) { }
            return null;
        }

        public static NangSuat_CumLoi Find_NangSuatCumLoi(int Id)
        {
            try
            {
                var db = new PMSEntities();
                return db.NangSuat_CumLoi.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);
            }
            catch (Exception) { }
            return null;
        }

        public static bool Update_NS_CumLoi(NangSuat_CumLoi obj)
        {
            try
            {
                var db = new PMSEntities();
                var nsc = db.NangSuat_CumLoi.FirstOrDefault(x => !x.IsDeleted && x.Id == obj.Id);
                if (nsc != null)
                {
                    nsc.SoLuongTang = obj.SoLuongTang;
                    nsc.SoLuongGiam = obj.SoLuongGiam;
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            { }
            return false;
        }

        public static List<string> GetSanLuongCumCuaChuyen(string Ngay, int STTChuyen_SanPham)
        {
            var db = new PMSEntities();
            var listSanLuong = new List<string>();
            try
            {
                var data = db.NangSuat_Cum.Where(x => !x.IsDeleted && x.Ngay == Ngay && x.STTChuyen_SanPham == STTChuyen_SanPham).OrderBy(x => x.IdCum).Select(x => x.SanLuongKCSTang).ToList();
                if (data != null && data.Count > 0)
                    listSanLuong.AddRange(data.Select(x => x.ToString()));
            }
            catch (Exception ex)
            {
            }
            return listSanLuong;
        }

        /// <summary>
        /// Function update lai thông tin ngay của chuyền Form DAyinfo
        /// </summary>
        /// <param name="sttc_sp"></param>
        /// <param name="newTH"></param>
        /// <param name="newTC"></param>
        /// <param name="newBTP"></param>
        /// <param name="date"></param>
        /// <param name="lastClusterId"></param>
        /// <param name="TypeOfCalculateBTPInLine"></param>
        /// <returns></returns>
        public static ResponseBase UpdateLKOnDay(int sttc_sp, int newTH, int newTC, int newBTP, string date, int lastClusterId, int TypeOfCalculateBTPInLine, int calculateNormsdayType, int TypeOfcalculateNormsday, List<string> TypeOfCheckFinishProduction)
        {
            var result = new ResponseBase();
            try
            {
                var db = new PMSEntities();
                int[] dateArr = date.Split('/').Select(x => Convert.ToInt32(x)).ToArray();
                int month = dateArr[1], year = dateArr[2];
                var csp = db.Chuyen_SanPham.FirstOrDefault(x => !x.IsDelete && x.STT == sttc_sp);
                if (csp != null)
                {
                    var proOnMonth = db.P_MonthlyProductionPlans.FirstOrDefault(x => !x.IsDeleted && x.Month == month && x.Year == year && x.STT_C_SP == sttc_sp);
                    var ns = db.NangXuats.FirstOrDefault(x => !x.IsDeleted && x.Ngay == date && x.STTCHuyen_SanPham == sttc_sp);
                    var ns_cum = db.NangSuat_Cum.FirstOrDefault(x => !x.IsDeleted && x.Ngay == date && x.STTChuyen_SanPham == sttc_sp && x.Cum.IsEndOfLine);

                    if (ns != null)
                    {

                        BTP btpObj;
                        TheoDoiNgay tdnObj;
                        int thuchien = ns.ThucHienNgay - ns.ThucHienNgayGiam,
                            thoatchuyen = ns.BTPThoatChuyenNgay - ns.BTPThoatChuyenNgayGiam,
                            btp = ns.BTPTang - ns.BTPGiam;

                        #region Thuc Hien Ngay KCS
                        if (newTH != thuchien)
                        {
                            tdnObj = new TheoDoiNgay();
                            tdnObj.STTChuyenSanPham = sttc_sp;
                            tdnObj.MaChuyen = csp.MaChuyen;
                            tdnObj.MaSanPham = csp.MaSanPham;
                            tdnObj.Time = DateTime.Now.TimeOfDay;
                            tdnObj.Date = date;
                            tdnObj.ErrorId = null;
                            tdnObj.IsEndOfLine = true;
                            tdnObj.IsEnterByKeypad = false;
                            tdnObj.CumId = lastClusterId;

                            newTH = (((csp.LuyKeTH + newTH) - thuchien) > csp.SanLuongKeHoach ? (csp.SanLuongKeHoach - (csp.LuyKeTH - thuchien)) : newTH);
                            tdnObj.ProductOutputTypeId = (int)eProductOutputType.KCS;
                            ns.ThucHienNgay = newTH;
                            ns.ThucHienNgayGiam = 0;
                            ns_cum.SanLuongKCSTang = newTH;
                            ns_cum.SanLuongKCSGiam = 0;
                            if (newTH > thuchien)
                            {
                                proOnMonth.LK_TH += (newTH - thuchien);
                                csp.LuyKeTH += (newTH - thuchien);
                                tdnObj.ThanhPham = (newTH - thuchien);
                                tdnObj.CommandTypeId = (int)eCommandRecive.ProductIncrease;
                            }
                            else
                            {
                                proOnMonth.LK_TH -= (thuchien - newTH);
                                csp.LuyKeTH -= (thuchien - newTH);
                                tdnObj.ThanhPham = (thuchien - newTH);
                                tdnObj.CommandTypeId = (int)eCommandRecive.ProductReduce;
                            }
                            db.TheoDoiNgays.Add(tdnObj);
                        }
                        #endregion

                        #region Thoat Chuyen Ngay
                        if (newTC != thoatchuyen)
                        {
                            tdnObj = new TheoDoiNgay();
                            tdnObj.STTChuyenSanPham = sttc_sp;
                            tdnObj.MaChuyen = csp.MaChuyen;
                            tdnObj.MaSanPham = csp.MaSanPham;
                            tdnObj.Time = DateTime.Now.TimeOfDay;
                            tdnObj.Date = date;
                            tdnObj.ErrorId = null;
                            tdnObj.IsEndOfLine = true;
                            tdnObj.IsEnterByKeypad = false;
                            tdnObj.CumId = lastClusterId;

                            newTC = (((csp.LuyKeBTPThoatChuyen + newTC) - thoatchuyen) > csp.SanLuongKeHoach ? (csp.SanLuongKeHoach - (csp.LuyKeBTPThoatChuyen - thoatchuyen)) : newTC);
                            tdnObj.ProductOutputTypeId = (int)eProductOutputType.TC;

                            ns.BTPThoatChuyenNgay = newTC;
                            ns.BTPThoatChuyenNgayGiam = 0;
                            ns_cum.SanLuongTCTang = newTC;
                            ns_cum.SanLuongTCGiam = 0;
                            if (newTC > thoatchuyen)
                            {
                                proOnMonth.LK_TC += (newTC - thoatchuyen);
                                csp.LuyKeBTPThoatChuyen += (newTC - thoatchuyen);
                                tdnObj.ThanhPham = (newTC - thoatchuyen);
                                tdnObj.CommandTypeId = (int)eCommandRecive.ProductIncrease;
                            }
                            else
                            {
                                proOnMonth.LK_TC -= (thoatchuyen - newTC);
                                csp.LuyKeBTPThoatChuyen -= (thoatchuyen - newTC);
                                tdnObj.ThanhPham = (thoatchuyen - newTC);
                                tdnObj.CommandTypeId = (int)eCommandRecive.ProductReduce;
                            }
                            db.TheoDoiNgays.Add(tdnObj);
                        }
                        #endregion


                        #region btp
                        if (newBTP != btp)
                        {
                            newBTP = (((csp.LK_BTP + newBTP) - btp) >= csp.SanLuongKeHoach ? (csp.SanLuongKeHoach - (csp.LK_BTP - btp)) : newBTP);
                            btpObj = new BTP();
                            btpObj.Ngay = date;
                            btpObj.STTChuyen_SanPham = sttc_sp;
                            btpObj.STT = 1;
                            btpObj.CumId = lastClusterId;
                            btpObj.TimeUpdate = DateTime.Now.TimeOfDay;
                            btpObj.IsEndOfLine = true;
                            btpObj.IsEnterByKeypad = false;
                            btpObj.CreatedDate = DateTime.Now;

                            ns.BTPTang = newBTP;
                            ns.BTPGiam = 0;
                            ns_cum.BTPTang = newBTP;
                            ns_cum.BTPGiam = 0;
                            if (newBTP > btp)
                            {
                                proOnMonth.LK_BTP += (newBTP - btp);
                                btpObj.BTPNgay = (newBTP - btp);
                                btpObj.CommandTypeId = (int)eCommandRecive.BTPIncrease;
                            }
                            else
                            {
                                proOnMonth.LK_BTP -= (btp - newBTP);
                                btpObj.BTPNgay = (btp - newBTP);
                                btpObj.CommandTypeId = (int)eCommandRecive.BTPReduce;
                            }
                            db.BTPs.Add(btpObj);
                        }
                        #endregion

                        if (TypeOfCheckFinishProduction != null && TypeOfCheckFinishProduction.Count > 0)
                        {
                            int count = 0;
                            foreach (var item in TypeOfCheckFinishProduction)
                            {
                                if (item == "KCS" && csp.LuyKeTH >= csp.SanLuongKeHoach)
                                {
                                    count++;
                                    break;
                                }
                                else if (item == "TC" && csp.LuyKeBTPThoatChuyen >= csp.SanLuongKeHoach)
                                {
                                    count++;
                                    break;
                                }
                                else if (item == "BTP" && csp.LK_BTP >= csp.SanLuongKeHoach)
                                {

                                    count++;
                                    break;
                                }
                            }
                            if (count > 0 && count == TypeOfCheckFinishProduction.Count)
                            {
                                csp.IsFinish = true;
                                csp.STTThucHien = 900;
                            }
                        }

                        db.SaveChanges();
                        //  BLLProductivity.ResetNormsDayAndBTPInLine(TypeOfCalculateBTPInLine, calculateNormsdayType, TypeOfcalculateNormsday, csp.MaChuyen, false, date);
                        result.IsSuccess = true;
                        result.Messages.Add(new Message() { msg = "Thay đổi Thực Hiện Ngày Thành Công.", Title = "Thông Báo" });
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = true;
                result.Messages.Add(new Message() { Title = "Lỗi", msg = "Cập nhật thực hiện ngày bị lỗi./n" + ex.Message });
            }
            return result;
        }

        public static bool UpdateDayInformation(string date, int stt_CSP, int TypeOfCalculateBTPInLine)
        {
            try
            {
                var db = new PMSEntities();
                var csp = db.Chuyen_SanPham.FirstOrDefault(x => !x.IsDelete && x.STT == stt_CSP);
                var monthDetail = db.P_MonthlyProductionPlans.Where(x => !x.IsDeleted && x.STT_C_SP == stt_CSP).ToList();
                if (csp != null)
                {
                    var ns_current = db.NangXuats.FirstOrDefault(x => !x.IsDeleted && x.Ngay == date && x.STTCHuyen_SanPham == stt_CSP);
                    if (ns_current != null)
                    {
                        switch (TypeOfCalculateBTPInLine)
                        {
                            case 1: ns_current.BTPTrenChuyen = monthDetail.Sum(x => x.LK_BTP) - csp.LuyKeTH; break;
                            case 2: ns_current.BTPTrenChuyen = monthDetail.Sum(x => x.LK_BTP) - csp.LuyKeBTPThoatChuyen; break;
                        }
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception)
            { return false; }
            return true;
        }



        /// <summary>
        /// Get thông tin màn hình LCD năng suât 
        /// Định mức ngày riêng lẻ
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="tableTypeId"></param>
        /// <param name="hienThiNSGio"></param>
        /// <param name="TimesGetNS"></param>
        /// <param name="KhoangCachGetNSOnDay"></param>
        /// <returns></returns>
        public static ModelProductivity GetProductivityByLineId_0(int lineId, int tableTypeId, int hienThiNSGio, int TimesGetNS, int KhoangCachGetNSOnDay)
        {
            try
            {
                var db = new PMSEntities();
                var now = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                var datetime = DateTime.Now;
                var model = new ModelProductivity();
                var listPCC = db.Chuyen_SanPham.Where(c => !c.IsDelete && !c.IsFinish && !c.SanPham.IsDelete && !c.Chuyen.IsDeleted && c.MaChuyen == lineId).OrderBy(c => c.STTThucHien).ToList();
                if (listPCC.Count > 0)
                {
                    #region get Data
                    var listSTTLineProduct = listPCC.Select(c => c.STT).ToList();
                    var listProductivity = db.NangXuats.Where(c => listSTTLineProduct.Contains(c.STTCHuyen_SanPham) && !c.IsDeleted).Select(x => new ProductivitiesModel()
                    {
                        Id = x.Id,
                        Ngay = x.Ngay,
                        STTCHuyen_SanPham = x.STTCHuyen_SanPham,
                        BTPGiam = x.BTPGiam,
                        BTPLoi = x.BTPLoi,
                        BTPTang = x.BTPTang,
                        BTPThoatChuyenNgay = x.BTPThoatChuyenNgay,
                        BTPThoatChuyenNgayGiam = x.BTPThoatChuyenNgayGiam,
                        BTPTrenChuyen = x.BTPTrenChuyen,
                        DinhMucNgay = x.DinhMucNgay,
                        IsBTP = x.IsBTP,
                        IsChange = x.IsChange,
                        IsChangeBTP = x.IsChangeBTP,
                        IsEndDate = x.IsEndDate,
                        IsStopOnDay = x.IsStopOnDay,
                        NhipDoSanXuat = x.NhipDoSanXuat,
                        NhipDoThucTe = x.NhipDoThucTe,
                        NhipDoThucTeBTPThoatChuyen = x.NhipDoThucTeBTPThoatChuyen,
                        SanLuongLoi = x.SanLuongLoi,
                        SanLuongLoiGiam = x.SanLuongLoiGiam,
                        ThucHienNgay = x.ThucHienNgay,
                        ThucHienNgayGiam = x.ThucHienNgayGiam,
                        TimeLastChange = x.TimeLastChange,
                        TimeStopOnDay = x.TimeStopOnDay,
                        productId = x.Chuyen_SanPham.SanPham.MaSanPham,
                        ProductName = x.Chuyen_SanPham.SanPham.TenSanPham,
                        ProductPrice = x.Chuyen_SanPham.SanPham.DonGia,
                        ProductPriceCM = x.Chuyen_SanPham.SanPham.DonGiaCM,
                        LineId = x.Chuyen_SanPham.MaChuyen,
                        LineName = x.Chuyen_SanPham.Chuyen.TenChuyen,
                        IdDenNangSuat = x.Chuyen_SanPham.Chuyen.IdDenNangSuat,
                        LaborsBase = x.Chuyen_SanPham.Chuyen.LaoDongDinhBien,
                        CreatedDate = x.CreatedDate
                    }).ToList();

                    var thanhphams = db.ThanhPhams.Where(c => listSTTLineProduct.Contains(c.STTChuyen_SanPham) && !c.IsDeleted).ToList();
                    var listDayInfo = db.ThanhPhams.Where(c => listSTTLineProduct.Contains(c.STTChuyen_SanPham) && !c.IsDeleted && c.Ngay == now).ToList();

                    var pccSX = listPCC.First();

                    var productivity = listProductivity.FirstOrDefault(c => c.STTCHuyen_SanPham == pccSX.STT && c.Ngay == now);

                    var dayInfo = listDayInfo.FirstOrDefault(c => c.STTChuyen_SanPham == pccSX.STT);

                    var listBTPOfLine = db.BTPs.Where(c => !c.IsDeleted && !c.IsBTP_PB_HC && listSTTLineProduct.Contains(c.STTChuyen_SanPham) && c.IsEndOfLine && (c.CommandTypeId == (int)eCommandRecive.BTPIncrease || c.CommandTypeId == (int)eCommandRecive.BTPReduce)).ToList();
                    var monthDetails = db.P_MonthlyProductionPlans.Where(x => !x.IsDeleted && x.Month == datetime.Month && x.Year == datetime.Year && listSTTLineProduct.Contains(x.STT_C_SP)).ToList();
                    var nxInMonth = listProductivity.Where(x => x.STTCHuyen_SanPham == pccSX.STT && x.CreatedDate.Month == datetime.Month && x.CreatedDate.Year == datetime.Year).ToList();
                    #endregion

                    if (productivity != null && dayInfo != null)
                    {
                        var config = db.Config_App.FirstOrDefault(x => x.AppId == 11 && x.Config.Name == eAppConfigName.TypeOfCalculateRevenues);
                        string typeOfCalculateRevenues = "TH";
                        if (config != null)
                            typeOfCalculateRevenues = config.Value;
                        #region Get Content Data
                        model.chuyen = productivity.LineName;
                        model.maHang = productivity.ProductName;

                        int luyKeThucHien = pccSX.LuyKeTH != null ? (int)pccSX.LuyKeTH : 0;
                        model.luyKeThucHien = luyKeThucHien + " / " + (pccSX.SanLuongKeHoach - luyKeThucHien);
                        model.luyKeBTP = pccSX.LuyKeBTPThoatChuyen != null ? pccSX.LuyKeBTPThoatChuyen + "" : "0";
                        model.laoDong = dayInfo.LaoDongChuyen + "/" + productivity.LaborsBase;
                        model.LaborBase = productivity.LaborsBase;

                        #region
                        var donGia = productivity.ProductPrice != null ? (double)productivity.ProductPrice : 0;
                        var donGiaCM = (double)productivity.ProductPriceCM;
                        double doanhThuTHNgay = 0, doanhThuDMNgay = 0, doanhThuKHThang = 0, doanhThuTHThang = 0, ThuNhapBQThang = 0, DoanhThuBQThang = 0, DoanhThuBQNgay = 0;
                        int dinhMucNgay = 0, finishTH = 0, finishTC = 0, finishError = 0, BTPTrenChuyen = 0, tongTHThang = 0, tongSL_KH = 0;
                        dinhMucNgay = (int)Math.Round(productivity.DinhMucNgay);
                        doanhThuDMNgay = Math.Round((donGia * dinhMucNgay), 2);
                        if (typeOfCalculateRevenues == "TH")
                        {
                            doanhThuTHNgay = Math.Round((donGia * (productivity.ThucHienNgay - productivity.ThucHienNgayGiam)), 2);
                            DoanhThuBQNgay = Math.Round((donGiaCM * (productivity.ThucHienNgay - productivity.ThucHienNgayGiam)) / dayInfo.LaoDongChuyen, 2);
                        }
                        else
                        {
                            doanhThuTHNgay = Math.Round((donGia * (productivity.BTPThoatChuyenNgay - productivity.BTPThoatChuyenNgayGiam)), 2);
                            DoanhThuBQNgay = Math.Round((donGiaCM * (productivity.BTPThoatChuyenNgay - productivity.BTPThoatChuyenNgayGiam)) / dayInfo.LaoDongChuyen, 2);
                        }

                        var currentMonthDetail = db.P_MonthlyProductionPlans.FirstOrDefault(x => x.STT_C_SP == productivity.STTCHuyen_SanPham);
                        if (currentMonthDetail != null)
                        {
                            doanhThuKHThang = Math.Round((donGia * currentMonthDetail.ProductionPlans), 2);
                            if (typeOfCalculateRevenues == "TH")
                            {
                                doanhThuTHThang = Math.Round((donGia * currentMonthDetail.LK_TH), 2);
                            }
                            else
                            {
                                doanhThuTHThang = Math.Round((donGia * currentMonthDetail.LK_TC), 2);
                            }
                            tongTHThang = currentMonthDetail.LK_TH;
                            tongSL_KH = currentMonthDetail.ProductionPlans;
                        }


                        if (nxInMonth.Count > 0)
                        {
                            foreach (var item in nxInMonth)
                            {
                                var day_info = thanhphams.FirstOrDefault(x => x.Ngay == item.Ngay);
                                if (day_info != null)
                                {
                                    double th = ((item.ThucHienNgay - item.ThucHienNgayGiam) * item.ProductPrice);
                                    ThuNhapBQThang += th > 0 ? ((th * donGia) / day_info.LaoDongChuyen) : 0;
                                    if (typeOfCalculateRevenues == "TH")
                                    {
                                        th = ((item.ThucHienNgay - item.ThucHienNgayGiam) * item.ProductPriceCM);
                                        DoanhThuBQThang += th > 0 ? ((th * donGiaCM) / day_info.LaoDongChuyen) : 0;
                                    }
                                    else
                                    {
                                        double tc = ((item.BTPThoatChuyenNgay - item.BTPThoatChuyenNgayGiam) * item.ProductPrice);
                                        tc = ((item.ThucHienNgay - item.ThucHienNgayGiam) * item.ProductPriceCM);
                                        DoanhThuBQThang += tc > 0 ? ((tc * donGiaCM) / day_info.LaoDongChuyen) : 0;
                                    }
                                }
                            }
                            ThuNhapBQThang = (ThuNhapBQThang > 0 ? Math.Round((ThuNhapBQThang / nxInMonth.Count), 2) : 0);
                            DoanhThuBQThang = (DoanhThuBQThang > 0 ? Math.Round((DoanhThuBQThang / nxInMonth.Count), 2) : 0);
                        }

                        int workingTimeOfLine = (int)BLLShift.GetTotalWorkingHourOfLine(lineId).TotalSeconds;
                        if (productivity.IsEndDate)
                        {
                            //ngay cuoi thi dinh muc ngay se bi thieu
                            var pc_next = listPCC.FirstOrDefault(x => x.STTThucHien > pccSX.STTThucHien);
                            if (pc_next != null)
                            {
                                // dinhMucNgay = TinhDinhMuc(pccSX, productivity, dayInfo.LaoDongChuyen, workingTimeOfLine, pc_next);
                                var pro = db.SanPhams.FirstOrDefault(x => !x.IsDelete && x.MaSanPham == pc_next.MaSanPham && x.DonGia > 0);
                                if (pro != null)
                                {
                                    //  doanhThuDMNgay += Math.Round((pro.DonGia * (dinhMucNgay - productivity.DinhMucNgay)), 2);
                                    //  var ns_next = listProductivity.FirstOrDefault(x => x.STTCHuyen_SanPham == pc_next.STT);
                                    //   if (ns_next != null)
                                    //  doanhThuTHNgay += Math.Round((donGia * (ns_next.ThucHienNgay - ns_next.ThucHienNgayGiam)), 2);
                                    var monthD = monthDetails.FirstOrDefault(x => x.STT_C_SP == pc_next.STT);
                                    if (monthD != null)
                                    {
                                        var newDT = Math.Round((monthD.ProductionPlans * pro.DonGia), 2);
                                        doanhThuKHThang += newDT;

                                        if (typeOfCalculateRevenues == "TH")
                                            newDT = Math.Round((monthD.LK_TH * pro.DonGia), 2);
                                        else
                                            newDT = Math.Round((monthD.LK_TC * pro.DonGia), 2);
                                        doanhThuTHThang += newDT;

                                        tongTHThang += monthD.LK_TH;
                                        tongSL_KH += monthD.ProductionPlans;
                                    }

                                    var nxInMonth_next = listProductivity.Where(x => x.STTCHuyen_SanPham == pc_next.STT && x.CreatedDate.Month == datetime.Month && x.CreatedDate.Year == datetime.Year).ToList();
                                    if (nxInMonth_next.Count > 0)
                                    {
                                        double TN_BQ = 0, DT_BQ = 0;
                                        foreach (var item in nxInMonth_next)
                                        {
                                            var day_info = thanhphams.FirstOrDefault(x => x.Ngay == item.Ngay);
                                            if (day_info != null)
                                            {
                                                double th = ((item.ThucHienNgay - item.ThucHienNgayGiam) * item.ProductPrice);
                                                TN_BQ += th > 0 ? ((th * pro.DonGia) / day_info.LaoDongChuyen) : 0;
                                                if (typeOfCalculateRevenues == "TH")
                                                    th = ((item.ThucHienNgay - item.ThucHienNgayGiam) * item.ProductPriceCM);
                                                else
                                                    th = ((item.BTPThoatChuyenNgay - item.BTPThoatChuyenNgayGiam) * item.ProductPriceCM);
                                                DT_BQ += th > 0 ? ((th * pro.DonGiaCM) / day_info.LaoDongChuyen) : 0;
                                            }
                                        }
                                        ThuNhapBQThang += (TN_BQ > 0 ? Math.Round((TN_BQ / nxInMonth_next.Count), 2) : 0);
                                        DoanhThuBQThang += (DT_BQ > 0 ? Math.Round((DT_BQ / nxInMonth_next.Count), 2) : 0);
                                    }
                                }
                            }
                        }
                        else
                        {
                            ///ktra xem co ma hang nao cua chuyen nay ket thuc trong ngay hay ko
                            ///neu co lay dinh muc cua ma hang truoc tinh xem thoi gian san xuat la bao nhieu gio 
                            ///con lai bao nhieu gio de san xuat ma 2 dc bao nhieu hang 2 cai cong lai ra dc dinh muc 
                            ///cua chuyen trong ngay 
                            var objFinishOnDay = db.Chuyen_SanPham.FirstOrDefault(x => !x.IsDelete && !x.SanPham.IsDelete && !x.Chuyen.IsDeleted && x.IsFinish && x.MaChuyen == lineId && x.FinishedDate.HasValue && x.FinishedDate.Value.Year == datetime.Year && x.FinishedDate.Value.Month == datetime.Month && x.FinishedDate.Value.Day == datetime.Day);
                            if (objFinishOnDay != null)
                            {
                                var nsOfFinishObj = db.NangXuats.FirstOrDefault(x => !x.IsDeleted && x.Ngay == now && x.STTCHuyen_SanPham == objFinishOnDay.STT);
                                //   dinhMucNgay = TinhDinhMuc(objFinishOnDay, nsOfFinishObj, dayInfo.LaoDongChuyen, workingTimeOfLine, pccSX);
                                // finishTH = nsOfFinishObj.ThucHienNgay - nsOfFinishObj.ThucHienNgayGiam;
                                // finishTC = nsOfFinishObj.BTPThoatChuyenNgay - nsOfFinishObj.BTPThoatChuyenNgayGiam;
                                //   finishError = db.TheoDoiNgays.Where(x => x.Date == now && x.STTChuyenSanPham == nsOfFinishObj.STTCHuyen_SanPham && x.CommandTypeId == (int)eCommandRecive.ErrorIncrease).ToList().Sum(x => x.ThanhPham);
                                //    finishError -= db.TheoDoiNgays.Where(x => x.Date == now && x.STTChuyenSanPham == nsOfFinishObj.STTCHuyen_SanPham && x.CommandTypeId == (int)eCommandRecive.ErrorReduce).ToList().Sum(x => x.ThanhPham);
                                //  BTPTrenChuyen = nsOfFinishObj.BTPTrenChuyen;
                                var pro = db.SanPhams.FirstOrDefault(x => !x.IsDelete && x.MaSanPham == objFinishOnDay.MaSanPham && x.DonGia > 0);
                                if (pro != null)
                                {
                                    //  doanhThuTHNgay += Math.Round((pro.DonGia * finishTH), 2);
                                    //  DoanhThuBQNgay += Math.Round((pro.DonGiaCM * finishTH) / dayInfo.LaoDongChuyen, 2);

                                    var monthD = db.P_MonthlyProductionPlans.FirstOrDefault(x => x.STT_C_SP == objFinishOnDay.STT);
                                    if (monthD != null)
                                    {
                                        var newDT = Math.Round((monthD.ProductionPlans * pro.DonGia), 2);
                                        doanhThuKHThang += newDT;
                                        if (typeOfCalculateRevenues == "TH")
                                            newDT = Math.Round((monthD.LK_TH * pro.DonGia), 2);
                                        else
                                            newDT = Math.Round((monthD.LK_TC * pro.DonGia), 2);
                                        doanhThuTHThang += newDT;

                                        tongTHThang += monthD.LK_TH;
                                        tongSL_KH += monthD.ProductionPlans;

                                        var nxInMonth_finish = db.NangXuats.Where(x => !x.IsDeleted && x.STTCHuyen_SanPham == objFinishOnDay.STT && x.CreatedDate.Month == datetime.Month && x.CreatedDate.Year == datetime.Year).ToList();
                                        var thanhpham_finish = db.ThanhPhams.Where(x => !x.IsDeleted && x.STTChuyen_SanPham == objFinishOnDay.STT).ToList();
                                        if (nxInMonth_finish.Count > 0)
                                        {
                                            double TN_BQ = 0, DT_BQ = 0;
                                            foreach (var item in nxInMonth_finish)
                                            {
                                                var day_info = thanhpham_finish.FirstOrDefault(x => x.Ngay == item.Ngay);
                                                if (day_info != null)
                                                {
                                                    double th = ((item.ThucHienNgay - item.ThucHienNgayGiam) * pro.DonGia);
                                                    TN_BQ += th > 0 ? ((th * pro.DonGia) / day_info.LaoDongChuyen) : 0;
                                                    if (typeOfCalculateRevenues == "TH")
                                                        th = ((item.ThucHienNgay - item.ThucHienNgayGiam) * pro.DonGiaCM);
                                                    else
                                                        th = ((item.BTPThoatChuyenNgay - item.BTPThoatChuyenNgayGiam) * pro.DonGiaCM);

                                                    DT_BQ += th > 0 ? (th / day_info.LaoDongChuyen) : 0;
                                                }
                                            }
                                            ThuNhapBQThang += (TN_BQ > 0 ? Math.Round((TN_BQ / nxInMonth_finish.Count), 2) : 0);
                                            DoanhThuBQThang += (DT_BQ > 0 ? Math.Round((DT_BQ / nxInMonth_finish.Count), 2) : 0);
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                        model.DinhMucNgay = dinhMucNgay;
                        model.doanhThuKHThang = doanhThuTHThang + " / " + doanhThuKHThang;
                        model.thucHienKHThang = tongTHThang + " / " + tongSL_KH;
                        model.doanhThuThang = doanhThuTHNgay + " / " + doanhThuTHThang;
                        #region
                        int thucHienNgay = 0, TCNgay = 0, ErrorDay = 0;
                        var listProductivityOnDay = listProductivity.Where(c => c.Ngay == now).ToList();
                        if (listProductivityOnDay.Count > 0)
                        {
                            foreach (var p in listProductivityOnDay)
                            {
                                var THNgay = p.ThucHienNgay - p.ThucHienNgayGiam;
                                if (THNgay > 0)
                                {
                                    thucHienNgay += THNgay;
                                    TCNgay += (p.BTPThoatChuyenNgay - p.BTPThoatChuyenNgayGiam);
                                    if (p.ProductPrice > 0)
                                        doanhThuTHNgay += Math.Round((p.ProductPrice * THNgay), 2);
                                }
                                BTPTrenChuyen += p.BTPTrenChuyen;
                            }
                        }
                        thucHienNgay += finishTH > 0 ? finishTH : 0;
                        TCNgay += finishTC > 0 ? finishTC : 0;

                        model.ThucHienNgay = thucHienNgay;
                        model.ThoatChuyenNgay = TCNgay;


                        model.thucHienVaDinhMuc = thucHienNgay + " / " + dinhMucNgay;
                        model.tiLeThucHien = (dinhMucNgay > 0 && thucHienNgay > 0) ? ((int)((thucHienNgay * 100) / dinhMucNgay)) + "" : "0";
                        model.doanhThuNgayTrenDinhMuc = doanhThuTHNgay + " / " + doanhThuDMNgay;

                        double nhipDoSanXuat = Math.Round((double)productivity.NhipDoSanXuat, 1);
                        model.nhipChuyen = productivity.NhipDoThucTe + " / " + nhipDoSanXuat;

                        double btpTrenChuyenBinhQuan = dayInfo.LaoDongChuyen == 0 ? 0 : Math.Round(((double)BTPTrenChuyen / dayInfo.LaoDongChuyen), 0);
                        model.btpTrenChuyen = btpTrenChuyenBinhQuan + " / " + BTPTrenChuyen;

                        int lightId = productivity.IdDenNangSuat ?? 0;
                        double tyLeDen = 0;
                        if (productivity.NhipDoThucTe > 0)
                            tyLeDen = (nhipDoSanXuat * 100) / productivity.NhipDoThucTe;

                        var lightConfig = db.Dens.Where(c => c.IdCatalogTable == tableTypeId && c.STTParent == lightId && c.ValueFrom <= tyLeDen && tyLeDen < c.ValueTo).FirstOrDefault();
                        model.mauDen = lightConfig != null ? lightConfig.Color.Trim().ToUpper() : "ĐỎ";
                        model.LuyKeKCS = pccSX.LuyKeBTPThoatChuyen + " / " + pccSX.SanLuongKeHoach;
                        model.LKTH_SLKH = luyKeThucHien + " / " + pccSX.SanLuongKeHoach;
                        #endregion

                        model.thuNhapBQThang = Math.Round((doanhThuTHNgay / dayInfo.LaoDongChuyen)) + " / " + Math.Round((ThuNhapBQThang));
                        model.DoanhThuBQ = Math.Round(DoanhThuBQNgay) + " / " + Math.Round(DoanhThuBQThang);

                        #endregion

                        #region Get Hours Productivity
                        var totalWorkingTimeInDay = BLLShift.GetTotalWorkingHourOfLine(lineId);
                        int intWorkTime = (int)(totalWorkingTimeInDay.TotalHours);
                        int intWorkMinuter = (int)totalWorkingTimeInDay.TotalMinutes;
                        double NangSuatPhutKH = 0;
                        int NangSuatGioKH = 0;
                        var dateNow = DateTime.Now.Date;
                        int tongTCNgay = 0, tongKCSNgay = 0;
                        if (intWorkTime > 0)
                        {
                            NangSuatPhutKH = (double)dinhMucNgay / intWorkMinuter;
                            NangSuatGioKH = (int)(dinhMucNgay / intWorkTime);
                            if (dinhMucNgay % intWorkTime != 0)
                                NangSuatGioKH++;

                            List<WorkingTimeModel> listWorkHoursOfLine = new List<WorkingTimeModel>();
                            switch (hienThiNSGio)
                            {
                                case (int)eShowNSType.PercentTH_FollowHour:
                                case (int)eShowNSType.TH_Err_FollowHour:
                                case (int)eShowNSType.TH_DM_FollowHour:
                                case (int)eShowNSType.TH_TC_FollowHour:
                                    listWorkHoursOfLine = BLLShift.GetListWorkHoursOfLineByLineId(lineId);
                                    break;
                                case (int)eShowNSType.PercentTH_FollowConfig:
                                case (int)eShowNSType.TH_Err_FollowConfig:
                                case (int)eShowNSType.TH_DM_FollowConfig:
                                case (int)eShowNSType.TH_TC_FollowConfig:
                                    listWorkHoursOfLine = BLLShift.GetListWorkHoursOfLineByLineId(lineId, TimesGetNS);
                                    break;
                                case (int)eShowNSType.TH_DM_OnDay:
                                case (int)eShowNSType.TH_TC_OnDay:
                                case (int)eShowNSType.TH_Error_OnDay:
                                    listWorkHoursOfLine.Add(new WorkingTimeModel()
                                    {
                                        TimeStart = DateTime.Now.AddHours(-KhoangCachGetNSOnDay).TimeOfDay,
                                        TimeEnd = DateTime.Now.TimeOfDay,
                                        IntHours = 1,
                                    });

                                    listWorkHoursOfLine.Add(new WorkingTimeModel()
                                    {
                                        TimeStart = DateTime.Now.AddHours(-(KhoangCachGetNSOnDay + KhoangCachGetNSOnDay)).TimeOfDay,
                                        TimeEnd = DateTime.Now.AddHours(-KhoangCachGetNSOnDay).TimeOfDay,
                                        IntHours = 2,
                                    });
                                    break;
                            }

                            if (listWorkHoursOfLine != null && listWorkHoursOfLine.Count > 0)
                            {
                                var dayInformations = db.TheoDoiNgays.Where(c => c.MaChuyen == lineId && c.Date == now && c.IsEndOfLine).Select(x => new DayInfoModel()
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

                                for (int i = 0; i < listWorkHoursOfLine.Count; i++)
                                {
                                    listWorkHoursOfLine[i].NormsHour = Math.Round(NangSuatPhutKH * (int)((listWorkHoursOfLine[0].TimeEnd - listWorkHoursOfLine[0].TimeStart).TotalMinutes));
                                    if ((hienThiNSGio == (int)eShowNSType.TH_DM_FollowHour || hienThiNSGio == (int)eShowNSType.PercentTH_FollowHour) && i == listWorkHoursOfLine.Count - 1)
                                        listWorkHoursOfLine[i].NormsHour = Math.Round(NangSuatPhutKH * (int)((listWorkHoursOfLine[i].TimeEnd - listWorkHoursOfLine[i].TimeStart).TotalMinutes));

                                    #region
                                    int Tang = 0, Giam = 0;
                                    var theoDoiNgays = dayInformations.Where(c => c.MaChuyen == lineId && c.Time > listWorkHoursOfLine[i].TimeStart && c.Time <= listWorkHoursOfLine[i].TimeEnd && c.Date == now && c.IsEndOfLine).ToList();
                                    if (theoDoiNgays.Count > 0)
                                    {
                                        Tang = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductIncrease && c.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(c => c.ThanhPham);
                                        Giam = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductReduce && c.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(c => c.ThanhPham);
                                        Tang -= Giam;
                                        listWorkHoursOfLine[i].KCS = Tang;
                                        listWorkHoursOfLine[i].HoursProductivity = (listWorkHoursOfLine[i].KCS < 0 ? 0 : listWorkHoursOfLine[i].KCS) + "/" + listWorkHoursOfLine[i].NormsHour;
                                        tongKCSNgay += Tang;

                                        //b2 
                                        Tang = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductIncrease && c.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(c => c.ThanhPham);
                                        Giam = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductReduce && c.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(c => c.ThanhPham);
                                        Tang -= Giam;
                                        listWorkHoursOfLine[i].TC = Tang;
                                        listWorkHoursOfLine[i].HoursProductivity_1 = (listWorkHoursOfLine[i].KCS < 0 ? 0 : listWorkHoursOfLine[i].KCS) + "/" + (listWorkHoursOfLine[i].TC < 0 ? 0 : listWorkHoursOfLine[i].TC);
                                        tongTCNgay += Tang;

                                        // lỗi
                                        Tang = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ErrorIncrease).Sum(c => c.ThanhPham);
                                        Giam = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ErrorReduce).Sum(c => c.ThanhPham);
                                        Tang -= Giam;
                                        listWorkHoursOfLine[i].Error = Tang;
                                        model.ErrorNgay += Tang;
                                        if (DateTime.Now.TimeOfDay > listWorkHoursOfLine[i].TimeStart && DateTime.Now.TimeOfDay <= listWorkHoursOfLine[i].TimeEnd)
                                        {
                                            model.TCInHour = listWorkHoursOfLine[i].TC;
                                            model.KCSInHour = listWorkHoursOfLine[i].KCS;
                                        }
                                    }
                                    else
                                    {
                                        listWorkHoursOfLine[i].HoursProductivity = "0/" + listWorkHoursOfLine[i].NormsHour;
                                        listWorkHoursOfLine[i].HoursProductivity_1 = "0/0";
                                    }
                                    #endregion
                                }
                            }

                            model.ErrorNgay += finishError;
                            model.listWorkHours = listWorkHoursOfLine;
                            model.KieuHienThiNangSuatGio = db.Configs.Where(x => x.Name.Trim().ToUpper().Equals("KieuHienThiNangSuatGio")).FirstOrDefault().ValueDefault.Trim();

                            // tong thuc hien cua chuyen trong ngay cua cac ma hang
                            // model.tongThucHienNgay = tongKCSNgay + "/" + tongDinhMucNgay;
                            model.tongThucHienNgay = thucHienNgay + "/" + dinhMucNgay;
                            // tong kcs cua chuyen cho cac ma hang trong ngay
                            model.ThoatChuyen = tongTCNgay + " / " + pccSX.LuyKeBTPThoatChuyen;
                        }
                        #endregion
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get thông tin màn hình LCD năng suât 
        /// Định mức ngày cộng dồn
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="tableTypeId"></param>
        /// <param name="hienThiNSGio"></param>
        /// <param name="TimesGetNS"></param>
        /// <param name="KhoangCachGetNSOnDay"></param>
        /// <returns></returns>
        public static ModelProductivity GetProductivityByLineId_1(int lineId, int tableTypeId, int hienThiNSGio, int TimesGetNS, int KhoangCachGetNSOnDay)
        {
            try
            {
                var db = new PMSEntities();
                var now = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                var datetime = DateTime.Now;
                var model = new ModelProductivity();
                var listPCC = db.Chuyen_SanPham.Where(c => !c.IsDelete && !c.IsFinish && !c.SanPham.IsDelete && !c.Chuyen.IsDeleted && c.MaChuyen == lineId).OrderBy(c => c.STTThucHien).ToList();
                if (listPCC.Count > 0)
                {
                    #region get Data
                    var listSTTLineProduct = listPCC.Select(c => c.STT).ToList();
                    var listProductivity = db.NangXuats.Where(c => listSTTLineProduct.Contains(c.STTCHuyen_SanPham) && !c.IsDeleted).Select(x => new ProductivitiesModel()
                    {
                        Id = x.Id,
                        Ngay = x.Ngay,
                        STTCHuyen_SanPham = x.STTCHuyen_SanPham,
                        BTPGiam = x.BTPGiam,
                        BTPLoi = x.BTPLoi,
                        BTPTang = x.BTPTang,
                        BTPThoatChuyenNgay = x.BTPThoatChuyenNgay,
                        BTPThoatChuyenNgayGiam = x.BTPThoatChuyenNgayGiam,
                        BTPTrenChuyen = x.BTPTrenChuyen,
                        DinhMucNgay = x.DinhMucNgay,
                        IsBTP = x.IsBTP,
                        IsChange = x.IsChange,
                        IsChangeBTP = x.IsChangeBTP,
                        IsEndDate = x.IsEndDate,
                        IsStopOnDay = x.IsStopOnDay,
                        NhipDoSanXuat = x.NhipDoSanXuat,
                        NhipDoThucTe = x.NhipDoThucTe,
                        NhipDoThucTeBTPThoatChuyen = x.NhipDoThucTeBTPThoatChuyen,
                        SanLuongLoi = x.SanLuongLoi,
                        SanLuongLoiGiam = x.SanLuongLoiGiam,
                        ThucHienNgay = x.ThucHienNgay,
                        ThucHienNgayGiam = x.ThucHienNgayGiam,
                        TimeLastChange = x.TimeLastChange,
                        TimeStopOnDay = x.TimeStopOnDay,
                        productId = x.Chuyen_SanPham.SanPham.MaSanPham,
                        ProductName = x.Chuyen_SanPham.SanPham.TenSanPham,
                        ProductPrice = x.Chuyen_SanPham.SanPham.DonGia,
                        ProductPriceCM = x.Chuyen_SanPham.SanPham.DonGiaCM,
                        LineId = x.Chuyen_SanPham.MaChuyen,
                        LineName = x.Chuyen_SanPham.Chuyen.TenChuyen,
                        IdDenNangSuat = x.Chuyen_SanPham.Chuyen.IdDenNangSuat,
                        LaborsBase = x.Chuyen_SanPham.Chuyen.LaoDongDinhBien,
                        CreatedDate = x.CreatedDate
                    }).ToList();

                    var thanhphams = db.ThanhPhams.Where(c => listSTTLineProduct.Contains(c.STTChuyen_SanPham) && !c.IsDeleted).ToList();
                    var listDayInfo = db.ThanhPhams.Where(c => listSTTLineProduct.Contains(c.STTChuyen_SanPham) && !c.IsDeleted && c.Ngay == now).ToList();

                    var pccSX = listPCC.First();

                    var productivity = listProductivity.FirstOrDefault(c => c.STTCHuyen_SanPham == pccSX.STT && c.Ngay == now);

                    var dayInfo = listDayInfo.FirstOrDefault(c => c.STTChuyen_SanPham == pccSX.STT);

                    var listBTPOfLine = db.BTPs.Where(c => !c.IsDeleted && !c.IsBTP_PB_HC && listSTTLineProduct.Contains(c.STTChuyen_SanPham) && c.IsEndOfLine && (c.CommandTypeId == (int)eCommandRecive.BTPIncrease || c.CommandTypeId == (int)eCommandRecive.BTPReduce)).ToList();
                    var monthDetails = db.P_MonthlyProductionPlans.Where(x => !x.IsDeleted && x.Month == datetime.Month && x.Year == datetime.Year && listSTTLineProduct.Contains(x.STT_C_SP)).ToList();
                    var nxInMonth = listProductivity.Where(x => x.STTCHuyen_SanPham == pccSX.STT && x.CreatedDate.Month == datetime.Month && x.CreatedDate.Year == datetime.Year).ToList();
                    #endregion

                    if (productivity != null && dayInfo != null)
                    {
                        var config = db.Config_App.FirstOrDefault(x => x.AppId == 11 && x.Config.Name == eAppConfigName.TypeOfCalculateRevenues);
                        string typeOfCalculateRevenues = "TH";
                        if (config != null)
                            typeOfCalculateRevenues = config.Value;
                        #region Get Content Data
                        model.chuyen = productivity.LineName;
                        model.maHang = productivity.ProductName;

                        int luyKeThucHien = pccSX.LuyKeTH != null ? (int)pccSX.LuyKeTH : 0;
                        model.luyKeThucHien = luyKeThucHien + " / " + (pccSX.SanLuongKeHoach - luyKeThucHien);
                        model.luyKeBTP = pccSX.LuyKeBTPThoatChuyen != null ? pccSX.LuyKeBTPThoatChuyen + "" : "0";
                        model.laoDong = dayInfo.LaoDongChuyen + "/" + productivity.LaborsBase;
                        model.LaborBase = productivity.LaborsBase;

                        #region
                        var donGia = productivity.ProductPrice != null ? (double)productivity.ProductPrice : 0;
                        var donGiaCM = (double)productivity.ProductPriceCM;
                        double doanhThuTHNgay = 0, doanhThuDMNgay = 0, doanhThuKHThang = 0, doanhThuTHThang = 0, ThuNhapBQThang = 0, DoanhThuBQThang = 0, DoanhThuBQNgay = 0;
                        int dinhMucNgay = 0, finishTH = 0, finishTC = 0, finishError = 0, BTPTrenChuyen = 0, tongTHThang = 0, tongSL_KH = 0;
                        dinhMucNgay = (int)Math.Round(productivity.DinhMucNgay);
                        doanhThuDMNgay = Math.Round((donGia * dinhMucNgay), 2);
                        int sl = 0, lkSanLuong = 0;
                        if (typeOfCalculateRevenues == "TH")
                        {
                            sl = productivity.ThucHienNgay - productivity.ThucHienNgayGiam;
                        }
                        else
                        {
                            sl = productivity.BTPThoatChuyenNgay - productivity.BTPThoatChuyenNgay;
                        }

                        doanhThuTHNgay = Math.Round((donGia * sl), 2);
                        DoanhThuBQNgay = Math.Round((donGiaCM * sl) / dayInfo.LaoDongChuyen, 2);

                        var currentMonthDetail = db.P_MonthlyProductionPlans.FirstOrDefault(x => x.STT_C_SP == productivity.STTCHuyen_SanPham);
                        if (currentMonthDetail != null)
                        {
                            doanhThuKHThang = Math.Round((donGia * currentMonthDetail.ProductionPlans), 2);
                            if (typeOfCalculateRevenues == "TH")
                                doanhThuTHThang = Math.Round((donGia * currentMonthDetail.LK_TH), 2);
                            else
                                doanhThuTHThang = Math.Round((donGia * currentMonthDetail.LK_TC), 2);
                            tongTHThang = currentMonthDetail.LK_TH;
                            tongSL_KH = currentMonthDetail.ProductionPlans;
                        }


                        if (nxInMonth.Count > 0)
                        {
                            foreach (var item in nxInMonth)
                            {
                                var day_info = thanhphams.FirstOrDefault(x => x.Ngay == item.Ngay);
                                if (day_info != null)
                                {
                                    double th = ((item.ThucHienNgay - item.ThucHienNgayGiam) * item.ProductPrice);
                                    ThuNhapBQThang += th > 0 ? ((th * donGia) / day_info.LaoDongChuyen) : 0;
                                    if (typeOfCalculateRevenues == "TH")
                                        th = ((item.ThucHienNgay - item.ThucHienNgayGiam) * item.ProductPriceCM);
                                    else
                                        th = ((item.BTPThoatChuyenNgay - item.BTPThoatChuyenNgayGiam) * item.ProductPriceCM);
                                    DoanhThuBQThang += th > 0 ? ((th) / day_info.LaoDongChuyen) : 0;
                                }
                            }
                            ThuNhapBQThang = (ThuNhapBQThang > 0 ? Math.Round((ThuNhapBQThang / nxInMonth.Count), 2) : 0);
                            DoanhThuBQThang = (DoanhThuBQThang > 0 ? Math.Round((DoanhThuBQThang / nxInMonth.Count), 2) : 0);
                        }

                        int workingTimeOfLine = (int)BLLShift.GetTotalWorkingHourOfLine(lineId).TotalSeconds;
                        if (productivity.IsEndDate)
                        {
                            //ngay cuoi thi dinh muc ngay se bi thieu
                            var pc_next = listPCC.FirstOrDefault(x => x.STTThucHien > pccSX.STTThucHien);
                            if (pc_next != null)
                            {
                                dinhMucNgay = TinhDinhMuc(pccSX, productivity, dayInfo, workingTimeOfLine, pc_next);
                                var pro = db.SanPhams.FirstOrDefault(x => !x.IsDelete && x.MaSanPham == pc_next.MaSanPham && x.DonGia > 0);
                                if (pro != null)
                                {
                                    doanhThuDMNgay += Math.Round((pro.DonGia * (dinhMucNgay - productivity.DinhMucNgay)), 2);
                                    var ns_next = listProductivity.FirstOrDefault(x => x.STTCHuyen_SanPham == pc_next.STT);
                                    if (ns_next != null)
                                    {
                                        if (typeOfCalculateRevenues == "TH")
                                            doanhThuTHNgay += Math.Round((donGia * (ns_next.ThucHienNgay - ns_next.ThucHienNgayGiam)), 2);
                                        else
                                            doanhThuTHNgay += Math.Round((donGia * (ns_next.BTPThoatChuyenNgay - ns_next.BTPThoatChuyenNgayGiam)), 2);
                                    }

                                    var monthD = monthDetails.FirstOrDefault(x => x.STT_C_SP == pc_next.STT);
                                    if (monthD != null)
                                    {
                                        var newDT = Math.Round((monthD.ProductionPlans * pro.DonGia), 2);
                                        doanhThuKHThang += newDT;
                                        if (typeOfCalculateRevenues == "TH")
                                            newDT = Math.Round((monthD.LK_TH * pro.DonGia), 2);
                                        else
                                            newDT = Math.Round((monthD.LK_TC * pro.DonGia), 2);
                                        doanhThuTHThang += newDT;

                                        tongTHThang += monthD.LK_TH;
                                        tongSL_KH += monthD.ProductionPlans;
                                    }

                                    var nxInMonth_next = listProductivity.Where(x => x.STTCHuyen_SanPham == pc_next.STT && x.CreatedDate.Month == datetime.Month && x.CreatedDate.Year == datetime.Year).ToList();
                                    if (nxInMonth_next.Count > 0)
                                    {
                                        double TN_BQ = 0, DT_BQ = 0;
                                        foreach (var item in nxInMonth_next)
                                        {
                                            var day_info = thanhphams.FirstOrDefault(x => x.Ngay == item.Ngay);
                                            if (day_info != null)
                                            {
                                                double th = ((item.ThucHienNgay - item.ThucHienNgayGiam) * item.ProductPrice);
                                                TN_BQ += th > 0 ? ((th * pro.DonGia) / day_info.LaoDongChuyen) : 0;
                                                if (typeOfCalculateRevenues == "TH")
                                                    th = ((item.ThucHienNgay - item.ThucHienNgayGiam) * item.ProductPriceCM);
                                                else
                                                    th = ((item.BTPThoatChuyenNgay - item.BTPThoatChuyenNgayGiam) * item.ProductPriceCM);
                                                DT_BQ += th > 0 ? ((th * pro.DonGiaCM) / day_info.LaoDongChuyen) : 0;
                                            }
                                        }
                                        ThuNhapBQThang += (TN_BQ > 0 ? Math.Round((TN_BQ / nxInMonth_next.Count), 2) : 0);
                                        DoanhThuBQThang += (DT_BQ > 0 ? Math.Round((DT_BQ / nxInMonth_next.Count), 2) : 0);
                                    }
                                }
                            }
                        }
                        else
                        {
                            ///ktra xem co ma hang nao cua chuyen nay ket thuc trong ngay hay ko
                            ///neu co lay dinh muc cua ma hang truoc tinh xem thoi gian san xuat la bao nhieu gio 
                            ///con lai bao nhieu gio de san xuat ma 2 dc bao nhieu hang 2 cai cong lai ra dc dinh muc 
                            ///cua chuyen trong ngay 
                            var objFinishOnDay = db.Chuyen_SanPham.FirstOrDefault(x => !x.IsDelete && !x.SanPham.IsDelete && !x.Chuyen.IsDeleted && x.IsFinish && x.MaChuyen == lineId && x.FinishedDate.HasValue && x.FinishedDate.Value.Year == datetime.Year && x.FinishedDate.Value.Month == datetime.Month && x.FinishedDate.Value.Day == datetime.Day);
                            if (objFinishOnDay != null)
                            {
                                var nsOfFinishObj = db.NangXuats.FirstOrDefault(x => !x.IsDeleted && x.Ngay == now && x.STTCHuyen_SanPham == objFinishOnDay.STT);
                                dinhMucNgay = TinhDinhMuc(objFinishOnDay, nsOfFinishObj, dayInfo, workingTimeOfLine, pccSX);
                                finishTH = nsOfFinishObj.ThucHienNgay - nsOfFinishObj.ThucHienNgayGiam;
                                finishTC = nsOfFinishObj.BTPThoatChuyenNgay - nsOfFinishObj.BTPThoatChuyenNgayGiam;
                                finishError = db.TheoDoiNgays.Where(x => x.Date == now && x.STTChuyenSanPham == nsOfFinishObj.STTCHuyen_SanPham && x.CommandTypeId == (int)eCommandRecive.ErrorIncrease).ToList().Sum(x => x.ThanhPham);
                                finishError -= db.TheoDoiNgays.Where(x => x.Date == now && x.STTChuyenSanPham == nsOfFinishObj.STTCHuyen_SanPham && x.CommandTypeId == (int)eCommandRecive.ErrorReduce).ToList().Sum(x => x.ThanhPham);
                                BTPTrenChuyen = nsOfFinishObj.BTPTrenChuyen;
                                var pro = db.SanPhams.FirstOrDefault(x => !x.IsDelete && x.MaSanPham == objFinishOnDay.MaSanPham && x.DonGia > 0);
                                if (pro != null)
                                {
                                    if (typeOfCalculateRevenues == "TH")
                                    {
                                        doanhThuTHNgay += Math.Round((pro.DonGia * finishTH), 2);
                                        DoanhThuBQNgay += Math.Round((pro.DonGiaCM * finishTH) / dayInfo.LaoDongChuyen, 2);
                                    }
                                    else
                                    {
                                        doanhThuTHNgay += Math.Round((pro.DonGia * finishTC), 2);
                                        DoanhThuBQNgay += Math.Round((pro.DonGiaCM * finishTC) / dayInfo.LaoDongChuyen, 2);
                                    }
                                    var monthD = db.P_MonthlyProductionPlans.FirstOrDefault(x => x.STT_C_SP == objFinishOnDay.STT);
                                    if (monthD != null)
                                    {
                                        var newDT = Math.Round((monthD.ProductionPlans * pro.DonGia), 2);
                                        doanhThuKHThang += newDT;

                                        if (typeOfCalculateRevenues == "TH")
                                            newDT = Math.Round((monthD.LK_TH * pro.DonGia), 2);
                                        else
                                            newDT = Math.Round((monthD.LK_TC * pro.DonGia), 2);
                                        doanhThuTHThang += newDT;

                                        tongTHThang += monthD.LK_TH;
                                        tongSL_KH += monthD.ProductionPlans;

                                        var nxInMonth_finish = db.NangXuats.Where(x => !x.IsDeleted && x.STTCHuyen_SanPham == objFinishOnDay.STT && x.CreatedDate.Month == datetime.Month && x.CreatedDate.Year == datetime.Year).ToList();
                                        var thanhpham_finish = db.ThanhPhams.Where(x => !x.IsDeleted && x.STTChuyen_SanPham == objFinishOnDay.STT).ToList();
                                        if (nxInMonth_finish.Count > 0)
                                        {
                                            double TN_BQ = 0, DT_BQ = 0;
                                            foreach (var item in nxInMonth_finish)
                                            {
                                                var day_info = thanhpham_finish.FirstOrDefault(x => x.Ngay == item.Ngay);
                                                if (day_info != null)
                                                {
                                                    double th = ((item.ThucHienNgay - item.ThucHienNgayGiam) * pro.DonGia);
                                                    TN_BQ += th > 0 ? ((th * pro.DonGia) / day_info.LaoDongChuyen) : 0;
                                                    if (typeOfCalculateRevenues == "TH")
                                                        th = ((item.ThucHienNgay - item.ThucHienNgayGiam) * pro.DonGiaCM);
                                                    else
                                                        th = ((item.BTPThoatChuyenNgay - item.BTPThoatChuyenNgayGiam) * pro.DonGiaCM);
                                                    DT_BQ += th > 0 ? ((th) / day_info.LaoDongChuyen) : 0;
                                                }
                                            }
                                            ThuNhapBQThang += (TN_BQ > 0 ? Math.Round((TN_BQ / nxInMonth_finish.Count), 2) : 0);
                                            DoanhThuBQThang += (DT_BQ > 0 ? Math.Round((DT_BQ / nxInMonth_finish.Count), 2) : 0);
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                        model.DinhMucNgay = dinhMucNgay;
                        model.doanhThuKHThang = doanhThuTHThang + " / " + doanhThuKHThang;
                        model.thucHienKHThang = tongTHThang + " / " + tongSL_KH;
                        model.doanhThuThang = doanhThuTHNgay + " / " + doanhThuTHThang;
                        #region
                        int thucHienNgay = 0, TCNgay = 0, ErrorDay = 0;
                        var listProductivityOnDay = listProductivity.Where(c => c.Ngay == now).ToList();
                        if (listProductivityOnDay.Count > 0)
                        {
                            foreach (var p in listProductivityOnDay)
                            {
                                var THNgay = p.ThucHienNgay - p.ThucHienNgayGiam;
                                if (THNgay > 0)
                                {
                                    thucHienNgay += THNgay;
                                    TCNgay += (p.BTPThoatChuyenNgay - p.BTPThoatChuyenNgayGiam);
                                    if (p.ProductPrice > 0)
                                        doanhThuTHNgay += Math.Round((p.ProductPrice * THNgay), 2);
                                }
                                BTPTrenChuyen += p.BTPTrenChuyen;
                            }
                        }
                        thucHienNgay += finishTH > 0 ? finishTH : 0;
                        TCNgay += finishTC > 0 ? finishTC : 0;

                        model.ThucHienNgay = thucHienNgay;
                        model.ThoatChuyenNgay = TCNgay;


                        model.thucHienVaDinhMuc = thucHienNgay + " / " + dinhMucNgay;
                        model.tiLeThucHien = (dinhMucNgay > 0 && thucHienNgay > 0) ? ((int)((thucHienNgay * 100) / dinhMucNgay)) + "" : "0";
                        model.doanhThuNgayTrenDinhMuc = doanhThuTHNgay + " / " + doanhThuDMNgay;

                        double nhipDoSanXuat = Math.Round((double)productivity.NhipDoSanXuat, 1);
                        model.nhipChuyen = productivity.NhipDoThucTe + " / " + nhipDoSanXuat;

                        double btpTrenChuyenBinhQuan = dayInfo.LaoDongChuyen == 0 ? 0 : Math.Round((double)BTPTrenChuyen / dayInfo.LaoDongChuyen);
                        model.btpTrenChuyen = btpTrenChuyenBinhQuan + " / " + BTPTrenChuyen;

                        int lightId = productivity.IdDenNangSuat ?? 0;
                        double tyLeDen = 0;
                        if (productivity.NhipDoThucTe > 0)
                            tyLeDen = (nhipDoSanXuat * 100) / productivity.NhipDoThucTe;

                        var lightConfig = db.Dens.Where(c => c.IdCatalogTable == tableTypeId && c.STTParent == lightId && c.ValueFrom <= tyLeDen && tyLeDen < c.ValueTo).FirstOrDefault();
                        model.mauDen = lightConfig != null ? lightConfig.Color.Trim().ToUpper() : "ĐỎ";
                        model.LuyKeKCS = pccSX.LuyKeBTPThoatChuyen + " / " + pccSX.SanLuongKeHoach;
                        model.LKTH_SLKH = luyKeThucHien + " / " + pccSX.SanLuongKeHoach;
                        #endregion

                        model.thuNhapBQThang = Math.Round((doanhThuTHNgay / dayInfo.LaoDongChuyen)) + " / " + Math.Round((ThuNhapBQThang));
                        model.DoanhThuBQ = Math.Round(DoanhThuBQNgay) + " / " + Math.Round(DoanhThuBQThang);

                        #endregion

                        #region Get Hours Productivity
                        var totalWorkingTimeInDay = BLLShift.GetTotalWorkingHourOfLine(lineId);
                        int intWorkTime = (int)(totalWorkingTimeInDay.TotalHours);
                        int intWorkMinuter = (int)totalWorkingTimeInDay.TotalMinutes;
                        double NangSuatPhutKH = 0;
                        int NangSuatGioKH = 0;
                        var dateNow = DateTime.Now.Date;
                        int tongTCNgay = 0, tongKCSNgay = 0;
                        if (intWorkTime > 0)
                        {
                            NangSuatPhutKH = (double)dinhMucNgay / intWorkMinuter;
                            NangSuatGioKH = (int)(dinhMucNgay / intWorkTime);
                            if (dinhMucNgay % intWorkTime != 0)
                                NangSuatGioKH++;

                            List<WorkingTimeModel> listWorkHoursOfLine = new List<WorkingTimeModel>();
                            switch (hienThiNSGio)
                            {
                                case (int)eShowNSType.PercentTH_FollowHour:
                                case (int)eShowNSType.TH_Err_FollowHour:
                                case (int)eShowNSType.TH_DM_FollowHour:
                                case (int)eShowNSType.TH_TC_FollowHour:
                                    listWorkHoursOfLine = BLLShift.GetListWorkHoursOfLineByLineId(lineId);
                                    break;
                                case (int)eShowNSType.PercentTH_FollowConfig:
                                case (int)eShowNSType.TH_Err_FollowConfig:
                                case (int)eShowNSType.TH_DM_FollowConfig:
                                case (int)eShowNSType.TH_TC_FollowConfig:
                                    listWorkHoursOfLine = BLLShift.GetListWorkHoursOfLineByLineId(lineId, TimesGetNS);
                                    break;
                                case (int)eShowNSType.TH_DM_OnDay:
                                case (int)eShowNSType.TH_TC_OnDay:
                                case (int)eShowNSType.TH_Error_OnDay:
                                    listWorkHoursOfLine.Add(new WorkingTimeModel()
                                    {
                                        TimeStart = DateTime.Now.AddHours(-KhoangCachGetNSOnDay).TimeOfDay,
                                        TimeEnd = DateTime.Now.TimeOfDay,
                                        IntHours = 1,
                                    });

                                    listWorkHoursOfLine.Add(new WorkingTimeModel()
                                    {
                                        TimeStart = DateTime.Now.AddHours(-(KhoangCachGetNSOnDay + KhoangCachGetNSOnDay)).TimeOfDay,
                                        TimeEnd = DateTime.Now.AddHours(-KhoangCachGetNSOnDay).TimeOfDay,
                                        IntHours = 2,
                                    });
                                    break;
                            }

                            if (listWorkHoursOfLine != null && listWorkHoursOfLine.Count > 0)
                            {
                                var dayInformations = db.TheoDoiNgays.Where(c => c.MaChuyen == lineId && c.Date == now && c.IsEndOfLine).Select(x => new DayInfoModel()
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

                                for (int i = 0; i < listWorkHoursOfLine.Count; i++)
                                {
                                    listWorkHoursOfLine[i].NormsHour = Math.Round(NangSuatPhutKH * (int)((listWorkHoursOfLine[0].TimeEnd - listWorkHoursOfLine[0].TimeStart).TotalMinutes));
                                    if ((hienThiNSGio == (int)eShowNSType.TH_DM_FollowHour || hienThiNSGio == (int)eShowNSType.PercentTH_FollowHour) && i == listWorkHoursOfLine.Count - 1)
                                        listWorkHoursOfLine[i].NormsHour = Math.Round(NangSuatPhutKH * (int)((listWorkHoursOfLine[i].TimeEnd - listWorkHoursOfLine[i].TimeStart).TotalMinutes));

                                    #region
                                    int Tang = 0, Giam = 0;
                                    var theoDoiNgays = dayInformations.Where(c => c.MaChuyen == lineId && c.Time > listWorkHoursOfLine[i].TimeStart && c.Time <= listWorkHoursOfLine[i].TimeEnd && c.Date == now && c.IsEndOfLine).ToList();
                                    if (theoDoiNgays.Count > 0)
                                    {
                                        Tang = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductIncrease && c.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(c => c.ThanhPham);
                                        Giam = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductReduce && c.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(c => c.ThanhPham);
                                        Tang -= Giam;
                                        listWorkHoursOfLine[i].KCS = Tang;
                                        listWorkHoursOfLine[i].HoursProductivity = (listWorkHoursOfLine[i].KCS < 0 ? 0 : listWorkHoursOfLine[i].KCS) + "/" + listWorkHoursOfLine[i].NormsHour;
                                        tongKCSNgay += Tang;

                                        //b2 
                                        Tang = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductIncrease && c.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(c => c.ThanhPham);
                                        Giam = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductReduce && c.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(c => c.ThanhPham);
                                        Tang -= Giam;
                                        listWorkHoursOfLine[i].TC = Tang;
                                        listWorkHoursOfLine[i].HoursProductivity_1 = (listWorkHoursOfLine[i].KCS < 0 ? 0 : listWorkHoursOfLine[i].KCS) + "/" + (listWorkHoursOfLine[i].TC < 0 ? 0 : listWorkHoursOfLine[i].TC);
                                        tongTCNgay += Tang;

                                        // lỗi
                                        Tang = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ErrorIncrease).Sum(c => c.ThanhPham);
                                        Giam = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ErrorReduce).Sum(c => c.ThanhPham);
                                        Tang -= Giam;
                                        listWorkHoursOfLine[i].Error = Tang;
                                        model.ErrorNgay += Tang;
                                        if (DateTime.Now.TimeOfDay > listWorkHoursOfLine[i].TimeStart && DateTime.Now.TimeOfDay <= listWorkHoursOfLine[i].TimeEnd)
                                        {
                                            model.TCInHour = listWorkHoursOfLine[i].TC;
                                            model.KCSInHour = listWorkHoursOfLine[i].KCS;
                                        }
                                    }
                                    else
                                    {
                                        listWorkHoursOfLine[i].HoursProductivity = "0/" + listWorkHoursOfLine[i].NormsHour;
                                        listWorkHoursOfLine[i].HoursProductivity_1 = "0/0";
                                    }
                                    #endregion
                                }
                            }

                            model.ErrorNgay += finishError;
                            model.listWorkHours = listWorkHoursOfLine;
                            model.KieuHienThiNangSuatGio = db.Configs.Where(x => x.Name.Trim().ToUpper().Equals("KieuHienThiNangSuatGio")).FirstOrDefault().ValueDefault.Trim();

                            // tong thuc hien cua chuyen trong ngay cua cac ma hang
                            // model.tongThucHienNgay = tongKCSNgay + "/" + tongDinhMucNgay;
                            model.tongThucHienNgay = thucHienNgay + "/" + dinhMucNgay;
                            // tong kcs cua chuyen cho cac ma hang trong ngay
                            model.ThoatChuyen = tongTCNgay + " / " + pccSX.LuyKeBTPThoatChuyen;
                        }
                        #endregion
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private static int TinhDinhMuc(Chuyen_SanPham currentPC, dynamic currentNS, ThanhPham currentDayinfo, int workingTimeOfLine, Chuyen_SanPham pc_next)
        {
            var nangSuatLaoDongNow = currentNS.DinhMucNgay / currentDayinfo.LaoDongChuyen;
            int totalSecondFinishMH1 = (int)(nangSuatLaoDongNow * Math.Round((currentPC.SanPham.ProductionTime * 100) / currentDayinfo.HieuSuat));
            int secondsAfter = workingTimeOfLine - totalSecondFinishMH1;

            double dinhMuc_Next = (secondsAfter / Math.Round((pc_next.SanPham.ProductionTime * 100) / currentDayinfo.HieuSuat)) * currentDayinfo.LaoDongChuyen;
            return (int)Math.Round(currentNS.DinhMucNgay + dinhMuc_Next);
        }

        /// <summary>
        /// Reset lai thông tin ngay cho keypad
        /// </summary>
        /// <param name="Date"></param>
        /// <param name="assignIds"></param>
        /// <param name="TypeOfCalculateBTPInLine"></param>
        /// <returns></returns>
        public static bool ResetDayInforByDate(DateTime Date, List<int> assignIds, int TypeOfCalculateBTPInLine)
        {
            try
            {
                using (var db = new PMSEntities())
                {
                    var csps = db.Chuyen_SanPham.Where(x => !x.IsDelete && assignIds.Contains(x.STT)).ToList();
                    if (csps != null)
                    {
                        var ngay = Date.Day + "/" + Date.Month + "/" + Date.Year;
                        var NSCs = db.NangSuat_Cum.Where(x => !x.IsDeleted && x.Ngay == ngay && assignIds.Contains(x.STTChuyen_SanPham));
                        var NSC_Errs = db.NangSuat_CumLoi.Where(x => !x.IsDeleted && x.Ngay == ngay && assignIds.Contains(x.STTChuyenSanPham));
                        var NSs = db.NangXuats.Where(x => !x.IsDeleted && assignIds.Contains(x.STTCHuyen_SanPham));
                        var tdns = db.TheoDoiNgays.Where(x => x.Date == ngay && assignIds.Contains(x.STTChuyenSanPham));
                        var btps = db.BTPs.Where(x => !x.IsDeleted && !x.IsBTP_PB_HC && assignIds.Contains(x.STTChuyen_SanPham)).ToList();
                        var mDetails = db.P_MonthlyProductionPlans.Where(x => !x.IsDeleted && assignIds.Contains(x.STT_C_SP));

                        foreach (var item in csps)
                        {

                            #region Nand Suat Cum

                            var nsc = NSCs.Where(x => x.STTChuyen_SanPham == item.STT);
                            if (nsc != null && nsc.Count() > 0)
                            {
                                foreach (var obj in nsc)
                                {
                                    obj.SanLuongKCSTang = 0;
                                    obj.SanLuongKCSGiam = 0;
                                    obj.SanLuongTCTang = 0;
                                    obj.SanLuongTCGiam = 0;
                                    obj.BTPTang = 0;
                                    obj.BTPGiam = 0;
                                }
                            }

                            var nscL = NSC_Errs.Where(x => x.STTChuyenSanPham == item.STT);
                            if (nscL != null && nscL.Count() > 0)
                            {
                                foreach (var obj in nscL)
                                {
                                    obj.SoLuongTang = 0;
                                    obj.SoLuongGiam = 0;
                                }
                            }
                            #endregion

                            var NSNotToday = NSs.Where(x => x.Ngay != ngay && x.STTCHuyen_SanPham == item.STT).ToList();
                            var tang = NSNotToday.Sum(x => x.ThucHienNgay);
                            var giam = NSNotToday.Sum(x => x.ThucHienNgayGiam);
                            tang = tang - giam;
                            item.LuyKeTH = tang > 0 ? tang : 0;

                            tang = NSNotToday.Sum(x => x.BTPThoatChuyenNgay);
                            giam = NSNotToday.Sum(x => x.BTPThoatChuyenNgayGiam);
                            tang = tang - giam;
                            item.LuyKeBTPThoatChuyen = tang > 0 ? tang : 0;

                            var oldM = mDetails.Where(x => x.STT_C_SP == item.STT && (x.Year != Date.Year) || (x.Year == Date.Year && x.Month != Date.Month)).ToList();
                            var monthDT = mDetails.FirstOrDefault(x => x.STT_C_SP == item.STT && x.Month == Date.Month && x.Year == Date.Year);
                            //BTP
                            var LKbtp = btps.Where(x => x.Ngay == ngay && assignIds.Contains(x.STTChuyen_SanPham));
                            if (LKbtp != null && LKbtp.Count() > 0)
                            {
                                foreach (var btp in LKbtp)
                                {
                                    btp.BTPNgay = 0;
                                    btp.TimeUpdate = Date.TimeOfDay;
                                    btp.UpdatedDate = Date;
                                }

                                tang = btps.Where(x => x.Ngay != ngay && x.CommandTypeId == (int)eCommandRecive.BTPIncrease).Sum(x => x.BTPNgay);
                                giam = btps.Where(x => x.Ngay != ngay && x.CommandTypeId == (int)eCommandRecive.BTPReduce).Sum(x => x.BTPNgay);
                                tang = tang - giam;
                                if (monthDT != null)
                                    monthDT.LK_BTP = tang - oldM.Sum(x => x.LK_BTP);
                            }

                            if (monthDT != null)
                            {
                                monthDT.LK_TH = item.LuyKeTH - oldM.Sum(x => x.LK_TH);
                                monthDT.LK_TC = item.LuyKeBTPThoatChuyen - oldM.Sum(x => x.LK_TC);
                            }
                            var tp = db.ThanhPhams.FirstOrDefault(x => !x.IsDeleted && x.Ngay == ngay && x.STTChuyen_SanPham == item.STT);
                            var ns = NSs.FirstOrDefault(x => x.Ngay == ngay && x.STTCHuyen_SanPham == item.STT);
                            if (ns != null)
                            {
                                ns.SanLuongLoi = 0;
                                ns.SanLuongLoiGiam = 0;
                                ns.ThucHienNgay = 0;
                                ns.ThucHienNgayGiam = 0;
                                ns.BTPTang = 0;
                                ns.BTPGiam = 0;
                                ns.BTPLoi = 0;
                                ns.BTPThoatChuyenNgay = 0;
                                ns.BTPThoatChuyenNgayGiam = 0;
                                ns.NhipDoThucTe = 0;
                                ns.TimeLastChange = Date.TimeOfDay;
                                ns.NhipDoThucTeBTPThoatChuyen = 0;
                                // ns.NhipDoSanXuat = 0;
                                double tgchetao = Math.Round((item.SanPham.ProductionTime * 100) / tp.HieuSuat);
                                ns.NhipDoSanXuat = (float)Math.Round((tgchetao / tp.LaoDongChuyen), 1);

                                switch (TypeOfCalculateBTPInLine)
                                {
                                    case 1: ns.BTPTrenChuyen = mDetails.Sum(x => x.LK_BTP) - item.LuyKeTH; break;
                                    case 2: ns.BTPTrenChuyen = mDetails.Sum(x => x.LK_BTP) - item.LuyKeBTPThoatChuyen; break;
                                }
                            }

                            if (tdns != null && tdns.Count() > 0)
                            {
                                foreach (var td in tdns)
                                {
                                    td.ThanhPham = 0;
                                    td.Time = Date.TimeOfDay;
                                }
                            }
                        }
                        db.SaveChanges();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            { }
            return false;
        }

        public static ProductivitiesModel GetProductivitiesInDay(DateTime Date, int sttCSP)
        {
            try
            {
                var date = Date.Day + "/" + Date.Month + "/" + Date.Year;
                var db = new PMSEntities();
                var nx = db.NangXuats.Where(x => !x.IsDeleted && x.STTCHuyen_SanPham == sttCSP && x.Ngay == date).Select(x => new ProductivitiesModel()
                {
                    Id = x.Id,
                    STTCHuyen_SanPham = x.STTCHuyen_SanPham,
                    BTPThoatChuyenNgay = x.BTPThoatChuyenNgay,
                    BTPThoatChuyenNgayGiam = x.BTPThoatChuyenNgayGiam,
                    ThucHienNgay = x.ThucHienNgay,
                    ThucHienNgayGiam = x.ThucHienNgayGiam,
                    BTPTang = x.BTPTang,
                    BTPGiam = x.BTPGiam,
                    productId = x.Chuyen_SanPham.MaSanPham
                }).FirstOrDefault();
                if (nx != null)
                {
                    nx.Errors = new List<NangSuat_CumLoi>();
                    nx.Errors.AddRange(db.NangSuat_CumLoi.Where(x => !x.IsDeleted && x.STTChuyenSanPham == sttCSP && x.Ngay == date).ToList());
                }
                return nx;
            }
            catch (Exception)
            { }
            return null;
        }

        public static List<ProductivitiesModel> GetProductivitiesInDay(string Date, List<int> lineIds)
        {
            try
            {
                var db = new PMSEntities();
                return db.NangXuats.Where(x => !x.IsDeleted && x.Ngay == Date && (!x.Chuyen_SanPham.IsFinish || (x.Chuyen_SanPham.FinishedDate != null && x.Chuyen_SanPham.FinishedDate.Value.Day == DateTime.Now.Day && x.Chuyen_SanPham.FinishedDate.Value.Month == DateTime.Now.Month && x.Chuyen_SanPham.FinishedDate.Value.Year == DateTime.Now.Year)) && lineIds.Contains(x.Chuyen_SanPham.MaChuyen)).Select(x => new ProductivitiesModel()
                {
                    Id = x.Id,
                    Ngay = x.Ngay,
                    STTCHuyen_SanPham = x.STTCHuyen_SanPham,
                    BTPGiam = x.BTPGiam,
                    BTPLoi = x.BTPLoi,
                    BTPTang = x.BTPTang,
                    BTPThoatChuyenNgay = x.BTPThoatChuyenNgay,
                    BTPThoatChuyenNgayGiam = x.BTPThoatChuyenNgayGiam,
                    BTPTrenChuyen = x.BTPTrenChuyen,
                    DinhMucNgay = x.DinhMucNgay,
                    IsBTP = x.IsBTP,
                    IsChange = x.IsChange,
                    IsChangeBTP = x.IsChangeBTP,
                    IsEndDate = x.IsEndDate,
                    IsStopOnDay = x.IsStopOnDay,
                    NhipDoSanXuat = x.NhipDoSanXuat,
                    NhipDoThucTe = x.NhipDoThucTe,
                    NhipDoThucTeBTPThoatChuyen = x.NhipDoThucTeBTPThoatChuyen,
                    SanLuongLoi = x.SanLuongLoi,
                    SanLuongLoiGiam = x.SanLuongLoiGiam,
                    ThucHienNgay = x.ThucHienNgay,
                    ThucHienNgayGiam = x.ThucHienNgayGiam,
                    TimeLastChange = x.TimeLastChange,
                    TimeStopOnDay = x.TimeStopOnDay,
                    productId = x.Chuyen_SanPham.SanPham.MaSanPham,
                    ProductName = x.Chuyen_SanPham.SanPham.TenSanPham,
                    ProductPrice = x.Chuyen_SanPham.SanPham.DonGia,
                    ProductPriceCM = x.Chuyen_SanPham.SanPham.DonGiaCM,
                    LineId = x.Chuyen_SanPham.MaChuyen,
                    LineName = x.Chuyen_SanPham.Chuyen.TenChuyen,
                    IdDenNangSuat = x.Chuyen_SanPham.Chuyen.IdDenNangSuat,
                    LaborsBase = x.Chuyen_SanPham.Chuyen.LaoDongDinhBien,
                    CreatedDate = x.CreatedDate,
                    OrderIndex = x.Chuyen_SanPham.STTThucHien,
                    ProductionPlans = x.Chuyen_SanPham.SanLuongKeHoach,
                    LK_TH = x.Chuyen_SanPham.LuyKeTH,
                    LK_TC = x.Chuyen_SanPham.LuyKeBTPThoatChuyen
                }).ToList();
            }
            catch (Exception)
            { }
            return null;
        }

        public static List<ProductivitiesModel> GetProductivitiesInDay(string Date, int lineId)
        {
            try
            {
                var db = new PMSEntities();
                return db.NangXuats.Where(x => !x.IsDeleted && !x.Chuyen_SanPham.IsDelete && !x.Chuyen_SanPham.SanPham.IsDelete && (!x.Chuyen_SanPham.IsFinish || (x.Chuyen_SanPham.FinishedDate != null && x.Chuyen_SanPham.FinishedDate.Value.Day == DateTime.Now.Day && x.Chuyen_SanPham.FinishedDate.Value.Month == DateTime.Now.Month && x.Chuyen_SanPham.FinishedDate.Value.Year == DateTime.Now.Year)) && x.Ngay == Date && x.Chuyen_SanPham.MaChuyen == lineId).Select(x => new ProductivitiesModel()
                {
                    Id = x.Id,
                    Ngay = x.Ngay,
                    STTCHuyen_SanPham = x.STTCHuyen_SanPham,
                    BTPGiam = x.BTPGiam,
                    BTPLoi = x.BTPLoi,
                    BTPTang = x.BTPTang,
                    BTPThoatChuyenNgay = x.BTPThoatChuyenNgay,
                    BTPThoatChuyenNgayGiam = x.BTPThoatChuyenNgayGiam,
                    BTPTrenChuyen = x.BTPTrenChuyen,
                    DinhMucNgay = x.DinhMucNgay,
                    IsBTP = x.IsBTP,
                    IsChange = x.IsChange,
                    IsChangeBTP = x.IsChangeBTP,
                    IsEndDate = x.IsEndDate,
                    IsStopOnDay = x.IsStopOnDay,
                    NhipDoSanXuat = x.NhipDoSanXuat,
                    NhipDoThucTe = x.NhipDoThucTe,
                    NhipDoThucTeBTPThoatChuyen = x.NhipDoThucTeBTPThoatChuyen,
                    SanLuongLoi = x.SanLuongLoi,
                    SanLuongLoiGiam = x.SanLuongLoiGiam,
                    ThucHienNgay = x.ThucHienNgay,
                    ThucHienNgayGiam = x.ThucHienNgayGiam,
                    TimeLastChange = x.TimeLastChange,
                    TimeStopOnDay = x.TimeStopOnDay,
                    productId = x.Chuyen_SanPham.SanPham.MaSanPham,
                    ProductName = x.Chuyen_SanPham.SanPham.TenSanPham,
                    ProductPrice = x.Chuyen_SanPham.SanPham.DonGia,
                    ProductPriceCM = x.Chuyen_SanPham.SanPham.DonGiaCM,
                    LineId = x.Chuyen_SanPham.MaChuyen,
                    LineName = x.Chuyen_SanPham.Chuyen.TenChuyen,
                    IdDenNangSuat = x.Chuyen_SanPham.Chuyen.IdDenNangSuat,
                    LaborsBase = x.Chuyen_SanPham.Chuyen.LaoDongDinhBien,
                    CreatedDate = x.CreatedDate,
                    OrderIndex = x.Chuyen_SanPham.STTThucHien
                }).OrderBy(x => x.OrderIndex).ToList();
            }
            catch (Exception)
            { }
            return null;
        }

        /// <summary>
        /// Lấy thông tin Năng suất cho LCD Tổng Hợp
        /// </summary>
        /// <param name="Date">Ngày</param>
        /// <param name="lineIds">danh sách chuyền</param>
        /// <returns></returns>
        public static LCDCollectionModel GetProductivitiesForLCDCollection(string Date, List<int> lineIds)
        {
            try
            {
                var db = new PMSEntities();
                var productions = db.NangXuats.Where(x => !x.IsDeleted && x.Ngay == Date && (!x.Chuyen_SanPham.IsFinish || (x.Chuyen_SanPham.FinishedDate != null && x.Chuyen_SanPham.FinishedDate.Value.Day == DateTime.Now.Day && x.Chuyen_SanPham.FinishedDate.Value.Month == DateTime.Now.Month && x.Chuyen_SanPham.FinishedDate.Value.Year == DateTime.Now.Year)) && lineIds.Contains(x.Chuyen_SanPham.MaChuyen)).Select(x => new ProductivitiesModel()
                {
                    Id = x.Id,
                    Ngay = x.Ngay,
                    STTCHuyen_SanPham = x.STTCHuyen_SanPham,
                    BTPGiam = x.BTPGiam,
                    BTPLoi = x.BTPLoi,
                    BTPTang = x.BTPTang,
                    BTPThoatChuyenNgay = x.BTPThoatChuyenNgay,
                    BTPThoatChuyenNgayGiam = x.BTPThoatChuyenNgayGiam,
                    BTPTrenChuyen = x.BTPTrenChuyen,
                    DinhMucNgay = x.DinhMucNgay,
                    IsBTP = x.IsBTP,
                    IsChange = x.IsChange,
                    IsChangeBTP = x.IsChangeBTP,
                    IsEndDate = x.IsEndDate,
                    IsStopOnDay = x.IsStopOnDay,
                    NhipDoSanXuat = x.NhipDoSanXuat,
                    NhipDoThucTe = x.NhipDoThucTe,
                    NhipDoThucTeBTPThoatChuyen = x.NhipDoThucTeBTPThoatChuyen,
                    SanLuongLoi = x.SanLuongLoi,
                    SanLuongLoiGiam = x.SanLuongLoiGiam,
                    ThucHienNgay = x.ThucHienNgay,
                    ThucHienNgayGiam = x.ThucHienNgayGiam,
                    TimeLastChange = x.TimeLastChange,
                    TimeStopOnDay = x.TimeStopOnDay,
                    productId = x.Chuyen_SanPham.SanPham.MaSanPham,
                    ProductName = x.Chuyen_SanPham.SanPham.TenSanPham,
                    ProductPrice = x.Chuyen_SanPham.SanPham.DonGia,
                    ProductPriceCM = x.Chuyen_SanPham.SanPham.DonGiaCM,
                    LineId = x.Chuyen_SanPham.MaChuyen,
                    LineName = x.Chuyen_SanPham.Chuyen.TenChuyen,
                    IdDenNangSuat = x.Chuyen_SanPham.Chuyen.IdDenNangSuat,
                    LaborsBase = x.Chuyen_SanPham.Chuyen.LaoDongDinhBien,
                    CreatedDate = x.CreatedDate,
                    OrderIndex = x.Chuyen_SanPham.STTThucHien,
                    ProductionPlans = x.Chuyen_SanPham.SanLuongKeHoach,
                    LK_TH = x.Chuyen_SanPham.LuyKeTH,
                    LK_TC = x.Chuyen_SanPham.LuyKeBTPThoatChuyen
                }).ToList();
                if (productions.Count > 0)
                {
                    var data = new LCDCollectionModel();
                    int count = 0;
                    double tongnhipTT = 0, tongnhipSX = 0;
                    var monthDetails = db.P_MonthlyProductionPlans.Where(x => !x.IsDeleted && x.Month == DateTime.Now.Month && x.Year == DateTime.Now.Year).Select(x => new MonthlyProductionModel()
                    {
                        Id = x.Id,
                        LineId = x.Chuyen_SanPham.MaChuyen,
                        LK_BTP = x.LK_BTP,
                        // LK_BTP_HC = x.LK_BTP_HC,
                        LK_TC = x.LK_TC,
                        LK_TH = x.LK_TH,
                        Month = x.Month,
                        STT_C_SP = x.STT_C_SP,
                        Year = x.Year,
                        ProductionPlans = x.ProductionPlans,
                        PriceCM = x.Chuyen_SanPham.SanPham.DonGiaCM
                    }).ToList();
                    foreach (var item in lineIds)
                    {
                        var nsOfLine = productions.Where(x => x.LineId == item).ToList();
                        if (nsOfLine.Count > 0)
                        {
                            var pro = new ProductivitiesModel();
                            pro.LineId = item;
                            pro.LineName = nsOfLine[0].LineName;
                            pro.ThucHienNgay = nsOfLine.Sum(x => x.ThucHienNgay);
                            pro.ThucHienNgayGiam = nsOfLine.Sum(x => x.ThucHienNgayGiam);
                            pro.ThucHienNgay -= pro.ThucHienNgayGiam;
                            pro.DinhMucNgay = nsOfLine.Sum(x => x.DinhMucNgay);

                            pro.ProductionPlans = monthDetails.Where(x => x.LineId == item).Sum(x => x.ProductionPlans);
                            pro.LK_TH = monthDetails.Where(x => x.LineId == item).Sum(x => x.LK_TH);

                            pro.RevenuePlan = Math.Round(monthDetails.Where(x => x.LineId == item).Sum(x => x.ProductionPlans * x.PriceCM));
                            pro.RevenueTH = Math.Round(monthDetails.Where(x => x.LineId == item).Sum(x => x.LK_TH * x.PriceCM));

                            pro.PercentTH = Math.Round(((double)(pro.LK_TH * 100) / pro.ProductionPlans), 1);
                            data.Lines.Add(pro);
                            tongnhipTT = nsOfLine.Sum(x => x.NhipDoThucTe);
                            tongnhipSX = nsOfLine.Sum(x => x.NhipDoSanXuat);
                            count += nsOfLine.Count;
                        }
                    }
                    var sttCSPs = productions.Select(x => x.STTCHuyen_SanPham).Distinct().ToList();
                    data.DayInfos = db.TheoDoiNgays.Where(x => x.Date == Date && sttCSPs.Contains(x.STTChuyenSanPham)).ToList();
                    data.NhipSX = tongnhipSX > 0 ? Math.Round(tongnhipSX / count) : 0;
                    data.NhipTT = tongnhipTT > 0 ? Math.Round(tongnhipTT / count) : 0;
                    return data;
                }
            }
            catch (Exception ex)
            { }
            return null;
        }


        /// <summary>
        /// Lấy thông tin ngày của ngày cuối cùng sản xuất dùng để tạo thông tin ngày tự động cho ngày mới
        /// </summary>
        /// <returns></returns>
        public static ResponseBase GetYesterday_NX()
        {
            var result = new ResponseBase();
            try
            {
                var db = new PMSEntities();
                var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                var yesterday = db.ThanhPhams.Where(x => x.CreatedDate < date).OrderByDescending(x => x.CreatedDate).FirstOrDefault().CreatedDate;
                var yesterdayStr = yesterday.Day + "/" + yesterday.Month + "/" + yesterday.Year;
                var yesterday_TP = db.ThanhPhams.Where(x => !x.IsDeleted && !x.Chuyen_SanPham.IsDelete && !x.Chuyen_SanPham.IsFinish && x.Ngay == yesterdayStr).ToList();
                if (yesterday_TP.Count > 0)
                {
                    result.IsSuccess = false;
                    result.Data = yesterday_TP;
                    var today = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                    result.Records = db.ThanhPhams.Where(x => !x.IsDeleted && x.Ngay == today).ToList();
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Messages.Add(new Message() { msg = "Lỗi Ngoại lệ.\n" + ex.Message, Title = "Lỗi Tạo Thông tin Ngày" });
            }
            return result;
        }

        /// <summary>
        /// Tạo thông tin ngày cho chuyền
        /// </summary>
        /// <param name="obj_TP">model ThanhPham</param>
        /// <param name="calculateBTPInLineConfig">config tính BTP trên chuyền</param>
        /// <returns></returns>
        public static ResponseBase InsertOrUpdate_TP(ThanhPhamModel obj_TP, int calculateBTPInLineConfig, int calculateNormsdayType, int TypeOfCalculateNormsday)
        {
            var rs = new ResponseBase();
            try
            {
                var db = new PMSEntities();
                ThanhPham tp;
                NangXuat nx;
                if (obj_TP.ShowLCD)
                    ChangeShowLCDStatus(obj_TP.Ngay, obj_TP.LineId, db);

                tp = db.ThanhPhams.FirstOrDefault(x => !x.IsDeleted && x.STTChuyen_SanPham == obj_TP.STTChuyen_SanPham && x.Ngay == obj_TP.Ngay);

                if (tp == null)
                {
                    #region Create
                    tp = new ThanhPham();
                    Parse.CopyObject(obj_TP, ref tp);
                    db.ThanhPhams.Add(tp);

                    //nang xuat
                    obj_TP.NangSuatObj = CreateNangSuat(obj_TP, db, tp);

                    nx = db.NangXuats.FirstOrDefault(x => !x.IsDeleted && x.STTCHuyen_SanPham == tp.STTChuyen_SanPham && x.Ngay == tp.Ngay);
                    if (nx == null)
                    {
                        nx = new NangXuat();
                        Parse.CopyObject(obj_TP.NangSuatObj, ref nx);
                        nx.CreatedDate = DateTime.Now;
                        db.NangXuats.Add(nx);
                    }
                    else
                    {
                        nx.DinhMucNgay = obj_TP.NangSuatObj.DinhMucNgay;
                        nx.NhipDoSanXuat = obj_TP.NangSuatObj.NhipDoSanXuat;
                        nx.TimeLastChange = obj_TP.NangSuatObj.TimeLastChange;
                        nx.IsEndDate = obj_TP.NangSuatObj.IsEndDate;
                        nx.IsStopOnDay = obj_TP.NangSuatObj.IsStopOnDay;
                        nx.TimeStopOnDay = obj_TP.NangSuatObj.TimeStopOnDay;
                        nx.UpdatedDate = DateTime.Now;
                    }
                    #endregion
                }
                else
                {
                    #region update

                    tp.NangXuatLaoDong = obj_TP.NangXuatLaoDong;
                    tp.LaoDongChuyen = obj_TP.LaoDongChuyen;
                    tp.LeanKH = obj_TP.LeanKH;
                    tp.ShowLCD = obj_TP.ShowLCD;
                    tp.HieuSuat = obj_TP.HieuSuat;
                    tp.LDOff = obj_TP.LDOff;
                    tp.LDNew = obj_TP.LDNew;
                    tp.LDPregnant = obj_TP.LDPregnant;
                    tp.LDVacation = obj_TP.LDVacation;

                    //nang xuat
                    obj_TP.NangSuatObj = CreateNangSuat(obj_TP, db, tp);

                    nx = db.NangXuats.FirstOrDefault(x => !x.IsDeleted && x.STTCHuyen_SanPham == tp.STTChuyen_SanPham && x.Ngay == tp.Ngay);
                    if (nx == null)
                    {
                        nx = new NangXuat();
                        Parse.CopyObject(obj_TP.NangSuatObj, ref nx);
                        nx.CreatedDate = DateTime.Now;
                        db.NangXuats.Add(nx);
                    }
                    else
                    {
                        nx.DinhMucNgay = obj_TP.NangSuatObj.DinhMucNgay;
                        nx.NhipDoSanXuat = obj_TP.NangSuatObj.NhipDoSanXuat;
                        nx.TimeLastChange = obj_TP.NangSuatObj.TimeLastChange;
                        nx.IsEndDate = obj_TP.NangSuatObj.IsEndDate;
                        nx.IsStopOnDay = obj_TP.NangSuatObj.IsStopOnDay;
                        nx.TimeStopOnDay = obj_TP.NangSuatObj.TimeStopOnDay;
                        nx.UpdatedDate = DateTime.Now;
                        nx.TGCheTaoSP = obj_TP.NangSuatObj.TGCheTaoSP;
                    }
                    #endregion
                }
                var clusters = BLLProductivity.GetClustersOfLine(obj_TP.LineId);
                #region ns cum
                if (clusters != null && clusters.Count > 0)
                {
                    var clusterIds = clusters.Select(x => x.Id).Distinct();
                    var ns_cums = db.NangSuat_Cum.Where(x => !x.IsDeleted && clusterIds.Contains(x.IdCum) && x.STTChuyen_SanPham == obj_TP.STTChuyen_SanPham && x.Ngay == obj_TP.Ngay).ToList();
                    NangSuat_Cum obj;
                    foreach (var c in clusters)
                    {
                        var check = ns_cums.FirstOrDefault(x => x.IdCum == c.Id);
                        if (check == null)
                        {
                            obj = new NangSuat_Cum();
                            obj.Ngay = obj_TP.Ngay;
                            obj.STTChuyen_SanPham = obj_TP.STTChuyen_SanPham;
                            obj.IdCum = c.Id;
                            db.NangSuat_Cum.Add(obj);
                        }
                    }
                }
                #endregion

                #region ns cum loi
                var errors = BLLError.GetAll();
                if (clusters != null && clusters.Count > 0 && errors != null && errors.Count > 0)
                {
                    var clusterIds = clusters.Select(x => x.Id).Distinct();
                    var ns_cumlois = db.NangSuat_CumLoi.Where(x => !x.IsDeleted && clusterIds.Contains(x.CumId) && x.STTChuyenSanPham == obj_TP.STTChuyen_SanPham && x.Ngay == obj_TP.Ngay).ToList();
                    NangSuat_CumLoi obj;
                    foreach (var c in clusters)
                    {
                        foreach (var err in errors)
                        {
                            var check = ns_cumlois.FirstOrDefault(x => x.CumId == c.Id && x.ErrorId == err.Id);
                            if (check == null)
                            {
                                obj = new NangSuat_CumLoi();
                                obj.Ngay = obj_TP.Ngay;
                                obj.STTChuyenSanPham = obj_TP.STTChuyen_SanPham;
                                obj.CumId = c.Id;
                                obj.ErrorId = err.Id;
                                db.NangSuat_CumLoi.Add(obj);
                            }
                        }
                    }
                }
                #endregion

                db.SaveChanges();

                BLLProductivity.ResetNormsDayAndBTPInLine(calculateBTPInLineConfig, calculateNormsdayType, TypeOfCalculateNormsday, obj_TP.LineId, false, tp.Ngay);
                rs.IsSuccess = true;
                rs.Messages.Add(new Message() { Title = "Thông Báo", msg = "Cập nhật thông tin ngày thành công." });
            }
            catch (Exception ex)
            {
                rs.IsSuccess = true;
                rs.Messages.Add(new Message() { Title = "Lỗi", msg = "Cập nhật thông tin ngày không thành công.\n" + ex.Message });
            }
            return rs;
        }

        private static void ChangeShowLCDStatus(string date, int lineId, PMSEntities db)
        {
            var tps = db.ThanhPhams.Where(x => !x.IsDeleted && x.Ngay == date && x.Chuyen_SanPham.MaChuyen == lineId);
            if (tps != null && tps.Count() > 0)
                foreach (var item in tps)
                {
                    item.ShowLCD = false;
                }
        }

        public static NangXuat CreateNangSuat(ThanhPhamModel obj_TP, PMSEntities db, ThanhPham tp)
        {
            var nxOfLine = db.NangXuats.Where(x => !x.IsDeleted && x.Ngay == tp.Ngay && x.STTCHuyen_SanPham != tp.STTChuyen_SanPham && x.Chuyen_SanPham.MaChuyen == obj_TP.LineId).OrderBy(x => x.Chuyen_SanPham.STTThucHien).Select(x => new ProductivitiesModel()
            {
                Id = x.Id,
                Ngay = x.Ngay,
                STTCHuyen_SanPham = x.STTCHuyen_SanPham,
                BTPGiam = x.BTPGiam,
                BTPLoi = x.BTPLoi,
                BTPTang = x.BTPTang,
                BTPThoatChuyenNgay = x.BTPThoatChuyenNgay,
                BTPThoatChuyenNgayGiam = x.BTPThoatChuyenNgayGiam,
                BTPTrenChuyen = x.BTPTrenChuyen,
                DinhMucNgay = x.DinhMucNgay,
                IsBTP = x.IsBTP,
                IsChange = x.IsChange,
                IsChangeBTP = x.IsChangeBTP,
                IsEndDate = x.IsEndDate,
                IsStopOnDay = x.IsStopOnDay,
                NhipDoSanXuat = x.NhipDoSanXuat,
                NhipDoThucTe = x.NhipDoThucTe,
                NhipDoThucTeBTPThoatChuyen = x.NhipDoThucTeBTPThoatChuyen,
                SanLuongLoi = x.SanLuongLoi,
                SanLuongLoiGiam = x.SanLuongLoiGiam,
                ThucHienNgay = x.ThucHienNgay,
                ThucHienNgayGiam = x.ThucHienNgayGiam,
                TimeLastChange = x.TimeLastChange,
                TimeStopOnDay = x.TimeStopOnDay,
                productId = x.Chuyen_SanPham.SanPham.MaSanPham,
                ProductName = x.Chuyen_SanPham.SanPham.TenSanPham,
                ProductPrice = x.Chuyen_SanPham.SanPham.DonGia,
                ProductPriceCM = x.Chuyen_SanPham.SanPham.DonGiaCM,
                LineId = x.Chuyen_SanPham.MaChuyen,
                LineName = x.Chuyen_SanPham.Chuyen.TenChuyen,
                IdDenNangSuat = x.Chuyen_SanPham.Chuyen.IdDenNangSuat,
                LaborsBase = x.Chuyen_SanPham.Chuyen.LaoDongDinhBien,
                NangSuatSanXuat = x.Chuyen_SanPham.SanPham.ProductionTime
            }).ToList();
            if (nxOfLine.Count > 0)
            {
                int workingTimeOfLine = (int)BLLShift.GetTotalWorkingHourOfLine(obj_TP.LineId).TotalSeconds;
                var tpOfLine = db.ThanhPhams.Where(x => !x.IsDeleted && x.Ngay == tp.Ngay && x.STTChuyen_SanPham != tp.STTChuyen_SanPham && x.Chuyen_SanPham.MaChuyen == obj_TP.LineId).OrderBy(x => x.Chuyen_SanPham.STTThucHien).ToList();
                foreach (var item in nxOfLine)
                {
                    if (workingTimeOfLine > 0)
                    {
                        var tp_curr = tpOfLine.FirstOrDefault(x => x.STTChuyen_SanPham == item.STTCHuyen_SanPham);
                        if (tp_curr != null)
                        {
                            var nangSuatLaoDongNow = item.DinhMucNgay / tp_curr.LaoDongChuyen;
                            int totalSecondFinishMH1 = (int)(nangSuatLaoDongNow * item.NangSuatSanXuat);
                            workingTimeOfLine = workingTimeOfLine - totalSecondFinishMH1;
                        }
                    }
                }
                if (workingTimeOfLine > 0)
                {
                    var csp = db.Chuyen_SanPham.FirstOrDefault(x => !x.IsDelete && x.STT == obj_TP.STTChuyen_SanPham);
                    if (csp != null)
                        obj_TP.NangSuatObj.DinhMucNgay = (workingTimeOfLine / Math.Round((csp.SanPham.ProductionTime * 100) / obj_TP.HieuSuat)) * obj_TP.LaoDongChuyen;
                }
                else
                    obj_TP.NangSuatObj.DinhMucNgay = 0;
            }

            return obj_TP.NangSuatObj;
        }

        public static ChuyenSanPhamModel GetProductivitiesInDayForKanban(int AssignId, string date)
        {
            try
            {
                var db = new PMSEntities();
                return db.NangXuats.Where(x => !x.IsDeleted && !x.Chuyen_SanPham.IsDelete && x.Ngay == date && !x.Chuyen_SanPham.SanPham.IsDelete && x.STTCHuyen_SanPham == AssignId && x.Chuyen_SanPham.LuyKeBTPThoatChuyen > 0).Select(x => new ChuyenSanPhamModel()
                {
                    STT = x.Chuyen_SanPham.STT,
                    //STTThucHien = x.Chuyen_SanPham.STTThucHien,
                    //Thang = x.Chuyen_SanPham.Thang,
                    //Nam = x.Chuyen_SanPham.Nam,
                    MaChuyen = x.Chuyen_SanPham.MaChuyen,
                    LineName = x.Chuyen_SanPham.Chuyen.TenChuyen,
                    IdDen = x.Chuyen_SanPham.Chuyen.IdDen,
                    Sound = x.Chuyen_SanPham.Chuyen.Sound,
                    //MaSanPham = x.Chuyen_SanPham.MaSanPham,
                    //CommoName = x.Chuyen_SanPham.SanPham.TenSanPham,
                    //Price = x.Chuyen_SanPham.SanPham.DonGia,
                    //PriceCM = x.Chuyen_SanPham.SanPham.DonGiaCM,
                    SanLuongKeHoach = x.Chuyen_SanPham.SanLuongKeHoach,
                    BTPInLine = x.BTPTrenChuyen,
                    NormsDay = x.DinhMucNgay,
                    LuyKeTH = x.Chuyen_SanPham.LuyKeTH,
                    //NangXuatSanXuat = x.Chuyen_SanPham.NangXuatSanXuat,
                    LuyKeBTPThoatChuyen = x.Chuyen_SanPham.LuyKeBTPThoatChuyen,
                    //IsFinish = x.Chuyen_SanPham.IsFinish,
                    //IsFinishBTPThoatChuyen = x.Chuyen_SanPham.IsFinishBTPThoatChuyen,
                    //IsFinishNow = x.Chuyen_SanPham.IsFinishNow,
                    //IsMoveQuantityFromMorthOld = x.Chuyen_SanPham.IsMoveQuantityFromMorthOld,
                    //TimeAdd = x.Chuyen_SanPham.TimeAdd,
                    TH_Day = x.ThucHienNgay,
                    TH_Day_G = x.ThucHienNgayGiam,
                    TC_Day = x.BTPThoatChuyenNgay,
                    TC_Day_G = x.BTPThoatChuyenNgayGiam,
                    Err_Day = x.SanLuongLoi,
                    Err_Day_G = x.SanLuongLoiGiam,
                    BTP_Day = x.BTPTang,
                    BTP_Day_G = x.BTPGiam,
                    LK_BTP = x.Chuyen_SanPham.LK_BTP,
                    BTPLoi = x.BTPLoi,
                    Percent_TH = x.DinhMucNgay > 0 ? Math.Round(((x.BTPTrenChuyen * 100) / (x.DinhMucNgay)), 1) : 0,
                    //NhipSX = x.NhipDoSanXuat,
                    //NhipTC = x.NhipDoThucTeBTPThoatChuyen,
                    //NhipTT = x.NhipDoThucTe,
                    IsEndDate = x.IsEndDate,
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

    public class BLLProductivity_
    {
        private static string ngay;
        static object key = new object();
        private static volatile BLLProductivity_ _Instance;  //volatile =>  tranh dung thread
        public static BLLProductivity_ Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (key)
                    {
                        _Instance = new BLLProductivity_();
                        ngay = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                    }
                }
                return _Instance;
            }
        }

        private BLLProductivity_() { }

        /// <summary>
        /// lấy thông tin sản suất trong ngày cho form theodoingay
        /// </summary>
        /// <param name="lineIds"></param>
        /// <param name="AppId"></param>
        /// <returns></returns>
        public List<ProductivitiesInDayModel> GetProductivitiesInDay(List<int> lineIds, int AppId)
        {
            using (var db = new PMSEntities())
            {
                try
                {
                    var list = new List<ProductivitiesInDayModel>();
                    var csp = db.NangXuats.Where(x => !x.IsDeleted && !x.Chuyen_SanPham.IsDelete && x.Ngay == ngay && !x.Chuyen_SanPham.SanPham.IsDelete && lineIds.Contains(x.Chuyen_SanPham.MaChuyen)).Select(x => new ChuyenSanPhamModel()
                    {
                        STT = x.Chuyen_SanPham.STT,
                        STTThucHien = x.Chuyen_SanPham.STTThucHien,
                        Thang = x.Chuyen_SanPham.Thang,
                        Nam = x.Chuyen_SanPham.Nam,
                        MaChuyen = x.Chuyen_SanPham.MaChuyen,
                        LineName = x.Chuyen_SanPham.Chuyen.TenChuyen,
                        MaSanPham = x.Chuyen_SanPham.MaSanPham,
                        CommoName = x.Chuyen_SanPham.SanPham.TenSanPham,
                        Price = x.Chuyen_SanPham.SanPham.DonGia,
                        PriceCM = x.Chuyen_SanPham.SanPham.DonGiaCM,
                        SanLuongKeHoach = x.Chuyen_SanPham.SanLuongKeHoach,
                        //NangXuatSanXuat = x.Chuyen_SanPham.NangXuatSanXuat,
                        LuyKeTH = x.Chuyen_SanPham.LuyKeTH,
                        LuyKeBTPThoatChuyen = x.Chuyen_SanPham.LuyKeBTPThoatChuyen,
                        IsFinish = x.Chuyen_SanPham.IsFinish,
                        IsFinishBTPThoatChuyen = x.Chuyen_SanPham.IsFinishBTPThoatChuyen,
                        IsFinishNow = x.Chuyen_SanPham.IsFinishNow,
                        IsMoveQuantityFromMorthOld = x.Chuyen_SanPham.IsMoveQuantityFromMorthOld,
                        TimeAdd = x.Chuyen_SanPham.TimeAdd,
                        NormsDay = x.DinhMucNgay,
                        TH_Day = x.ThucHienNgay,
                        TH_Day_G = x.ThucHienNgayGiam,
                        TC_Day = x.BTPThoatChuyenNgay,
                        TC_Day_G = x.BTPThoatChuyenNgayGiam,
                        Err_Day = x.SanLuongLoi,
                        Err_Day_G = x.SanLuongLoiGiam,
                        BTP_Day = x.BTPTang,
                        BTP_Day_G = x.BTPGiam,
                        BTPInLine = x.BTPTrenChuyen,
                        NhipSX = x.NhipDoSanXuat,
                        NhipTC = x.NhipDoThucTeBTPThoatChuyen,
                        NhipTT = x.NhipDoThucTe,
                    });

                    if (csp != null && csp.Count() > 0)
                    {
                        #region
                        var cf = db.Configs.ToList();
                        var cfApp = db.Config_App.Where(x => x.AppId == AppId).ToList();

                        var config = cf.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eAppConfigName.KieuTinhNhipThucTe));
                        string cfType = "1", cfErrorType = "1";
                        if (config != null)
                        {
                            var cfA = cfApp.FirstOrDefault(x => x.ConfigId == config.Id);
                            cfType = cfA != null ? cfA.Value.Trim() : config.ValueDefault.Trim();
                        }
                        config = cf.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eAppConfigName.KieuTinhTyLeHangLoi));
                        if (config != null)
                        {
                            var cfA = cfApp.FirstOrDefault(x => x.ConfigId == config.Id);
                            cfErrorType = cfA != null ? cfA.Value.Trim() : config.ValueDefault.Trim();
                        }

                        var stts = csp.Select(x => x.STT).Distinct();
                        var thanhPhams = db.ThanhPhams.Where(x => stts.Contains(x.STTChuyen_SanPham) && x.Ngay == ngay).ToList();
                        var btps = db.BTPs.Where(x => !x.IsDeleted && x.IsEndOfLine && !x.IsBTP_PB_HC && stts.Contains(x.STTChuyen_SanPham)).ToList();
                        var errors = db.TheoDoiNgays.Where(x => x.IsEndOfLine && stts.Contains(x.STTChuyenSanPham) && x.Date == ngay).ToList();
                        var monthlyInfos = db.P_MonthlyProductionPlans.Where(x => !x.IsDeleted && x.Month == DateTime.Now.Month && x.Year == DateTime.Now.Year && stts.Contains(x.STT_C_SP)).ToList();
                        var configg = db.Config_App.FirstOrDefault(x => x.AppId == 11 && x.Config.Name == eAppConfigName.TypeOfCalculateRevenues);
                        string typeOfCalculateRevenues = "TH";
                        if (config != null)
                            typeOfCalculateRevenues = configg.Value;
                        foreach (var id in lineIds)
                        {
                            #region
                            var lineInfos = csp.Where(x => x.MaChuyen == id).OrderBy(x => x.STTThucHien);
                            if (lineInfos != null && lineInfos.Count() > 0)
                            {
                                foreach (var item in lineInfos)
                                {
                                    var tp = thanhPhams.FirstOrDefault(x => x.STTChuyen_SanPham == item.STT);
                                    int LK_btptang = 0, LK_btpgiam = 0;
                                    item.Err_Day = errors.Where(x => x.STTChuyenSanPham == item.STT && x.MaChuyen == item.MaChuyen && x.MaSanPham == item.MaSanPham && x.CommandTypeId.Value == (int)eCommandRecive.ErrorIncrease).Sum(x => x.ThanhPham);
                                    item.Err_Day_G = errors.Where(x => x.STTChuyenSanPham == item.STT && x.MaChuyen == item.MaChuyen && x.MaSanPham == item.MaSanPham && x.CommandTypeId.Value == (int)eCommandRecive.ErrorReduce).Sum(x => x.ThanhPham);
                                    LK_btptang = btps.Where(x => x.CommandTypeId == (int)eCommandRecive.BTPIncrease && x.STTChuyen_SanPham == item.STT).Sum(x => x.BTPNgay);
                                    LK_btpgiam = btps.Where(x => x.CommandTypeId == (int)eCommandRecive.BTPReduce && x.STTChuyen_SanPham == item.STT).Sum(x => x.BTPNgay);
                                    LK_btptang = LK_btptang - LK_btpgiam;
                                    item.TC_Day = item.TC_Day - item.TC_Day_G;
                                    item.TH_Day = item.TH_Day - item.TH_Day_G;
                                    item.Err_Day = item.Err_Day - item.Err_Day_G;
                                    item.BTP_Day = item.BTP_Day - item.BTP_Day_G;

                                    var obj = new ProductivitiesInDayModel();
                                    obj.LineName = item.LineName;
                                    obj.CommoName = item.CommoName;
                                    obj.LaborInLine = tp != null ? tp.LaoDongChuyen : 0;
                                    obj.ProductionPlans = item.SanLuongKeHoach;
                                    obj.LK_TH = item.LuyKeTH;
                                    obj.LK_TC = item.LuyKeBTPThoatChuyen;
                                    obj.LK_BTP = LK_btptang < 0 ? 0 : LK_btptang;
                                    obj.NormsOfDay = (int)Math.Round(item.NormsDay, 0);
                                    obj.TH_Day = item.TH_Day < 0 ? 0 : item.TH_Day;
                                    obj.TC_Day = item.TC_Day < 0 ? 0 : item.TC_Day;
                                    obj.ErrorsInDay = item.Err_Day < 0 ? 0 : item.Err_Day;
                                    obj.BTP_Day = item.BTP_Day < 0 ? 0 : item.BTP_Day;
                                    obj.BTPInLine = item.BTPInLine < 0 ? 0 : item.BTPInLine;
                                    obj.TH_Percent = item.TH_Day <= 0 || item.NormsDay <= 0 ? 0 : (int)((item.TH_Day / item.NormsDay) * 100);
                                    switch (cfErrorType)
                                    {
                                        case "1": obj.ErrorPercent = item.NormsDay > 0 && item.Err_Day > 0 ? (int)((item.Err_Day / item.NormsDay) * 100) : 0; break;
                                        case "2": obj.ErrorPercent = item.Err_Day > 0 ? (float)Math.Round((((double)item.Err_Day / ((double)item.Err_Day + (double)item.TH_Day)) * 100), 2) : 0; break;
                                        case "3": obj.ErrorPercent = item.Err_Day > 0 && item.TC_Day > 0 ? (float)Math.Round(((double)((double)item.Err_Day / (double)item.TC_Day) * 100), 2) : 0; break;
                                    }
                                    obj.Funds = item.BTPInLine > 0 ? (int)(Math.Round((double)item.BTPInLine / tp.LaoDongChuyen)) : 0;
                                    if (typeOfCalculateRevenues == "TH")
                                        obj.RevenuesInDay = (item.TH_Day > 0 && item.PriceCM > 0) ? Math.Ceiling(((double)item.TH_Day * item.PriceCM)) : 0;
                                    else
                                        obj.RevenuesInDay = (item.TC_Day > 0 && item.PriceCM > 0) ? Math.Ceiling(((double)item.TC_Day * item.PriceCM)) : 0;

                                    var monthInfo = monthlyInfos.FirstOrDefault(x => x.STT_C_SP == item.STT);
                                    obj.RevenuesInMonth = monthInfo == null ? 0 : (monthInfo.LK_TH > 0 && item.PriceCM > 0 ? Math.Ceiling((double)monthInfo.LK_TH * item.PriceCM) : 0);
                                    obj.ResearchPaced = (int)item.NhipSX;
                                    if (cfType == "1")
                                    {
                                        obj.CurrentPacedProduction = (int)item.NhipTT;
                                        obj.TC_Paced = item.NhipTT > 0 ? (int)((item.NhipSX / item.NhipTT) * 100) : 0;
                                    }
                                    else
                                    {
                                        obj.CurrentPacedProduction = (int)item.NhipTC;
                                        obj.TC_Paced = item.NhipTC > 0 ? (int)((item.NhipSX / item.NhipTC) * 100) : 0;
                                    }
                                    obj.IsFinish = item.IsFinish;
                                    list.Add(obj);
                                }
                            }
                            #endregion
                        }
                        #endregion
                    }
                    return list;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public NangXuat GetLatestWork()
        {
            using (var db = new PMSEntities())
            {
                try
                {
                    var today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                    return (from x in db.NangXuats where x.CreatedDate < today orderby x.CreatedDate descending select x).FirstOrDefault();
                }
                catch (Exception)
                {
                }
                return null;
            }
        }

        public DateTime NgaySXCuoiCung()
        {
            using (var db = new PMSEntities())
            {
                var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                var yesterday = db.ThanhPhams.Where(x => x.CreatedDate < date).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                if (yesterday != null)
                    return yesterday.CreatedDate;
                return date;
            }
        }
    }
}
