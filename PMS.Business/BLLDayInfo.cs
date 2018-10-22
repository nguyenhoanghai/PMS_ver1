using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMS.Business.Enum;

namespace PMS.Business
{
    public class BLLDayInfo
    {
        /// <summary>
        /// Get tat ca thong tin nhap san luong trong ngay form DayInfo
        /// </summary>
        /// <param name="date"></param>
        /// <param name="LineId"></param>
        /// <param name="commoId"></param>
        /// <param name="stt"></param>
        /// <returns></returns>
        public static List<DayInfoModel> GetInforByDate(DateTime date, int LineId, int commoId, int stt)
        {
            var info = new List<DayInfoModel>();
            string ngay = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
            try
            {
                var db = new PMSEntities();
                info.AddRange(db.TheoDoiNgays.Where(x => x.MaChuyen == LineId && x.MaSanPham == commoId && x.Date == ngay && x.IsEndOfLine).Select(x => new DayInfoModel()
                {
                    STT = x.STT,
                    LineName = x.Chuyen.TenChuyen,
                    CommoName = x.SanPham.TenSanPham,
                    ThanhPham = x.ThanhPham,
                    CommandTypeId = x.CommandTypeId,
                    CumId = x.CumId,
                    ClusterName = x.Cum.TenCum,
                    ProductOutputTypeId = x.ProductOutputTypeId,
                    Date = x.Date,
                    Time = x.Time,
                    ErrorId = x.ErrorId,
                    ErrorName = string.Empty,
                    MaChuyen = x.MaChuyen,
                    MaSanPham = x.MaSanPham,
                    STTChuyenSanPham = x.STTChuyenSanPham,
                    IsEndOfLine = x.IsEndOfLine,
                    IsEnterByKeypad = x.IsEnterByKeypad,
                    KeypadType = x.IsEnterByKeypad ? "Bàn Phím" : "Người dùng"
                }).ToList());

                // lay btp
                info.AddRange(db.BTPs.Where(x => !x.IsDeleted && x.IsEndOfLine && x.Ngay == ngay && x.STTChuyen_SanPham == stt).Select(x => new DayInfoModel()
                {
                    STT = x.Id,
                    LineName = x.Chuyen_SanPham.Chuyen.TenChuyen,
                    MaChuyen = x.Chuyen_SanPham.MaChuyen,
                    MaSanPham = x.Chuyen_SanPham.MaSanPham,
                    CommoName = x.Chuyen_SanPham.SanPham.TenSanPham,
                    ThanhPham = x.BTPNgay,
                    ProductOutputTypeId = x.IsBTP_PB_HC ? (int)eProductOutputType.BTP_HC : (int)eProductOutputType.BTP,
                    ProType = "Bán Thành Phẩm",
                    CommandTypeId = x.CommandTypeId,
                    Time = x.TimeUpdate,
                    CumId = x.CumId,
                    ClusterName = x.Cum.TenCum,
                    Date = x.Ngay,
                    STTChuyenSanPham = x.STTChuyen_SanPham,
                    IsEndOfLine = x.IsEndOfLine,
                    IsEnterByKeypad = x.IsEnterByKeypad,
                    KeypadType = x.IsEnterByKeypad ? "Bàn Phím" : "Người dùng",
                    ErrorId = 0,
                    ErrorName = string.Empty,
                }));

                if (info.Count > 0)
                {
                    var errIds = info.Select(x => x.ErrorId).Distinct();
                    var errors = db.Errors.Where(x => !x.IsDeleted && errIds.Contains(x.Id));

                    foreach (var item in info)
                    {
                        switch (item.ProductOutputTypeId)
                        {
                            case (int)eProductOutputType.KCS:
                                item.ProType = "Kiểm Đạt";
                                break;
                            case (int)eProductOutputType.TC:
                                item.ProType = "Thoát Chuyền";
                                break;
                            case (int)eProductOutputType.BTP:
                                item.ProType = "Bán Thành Phẩm";
                                break;
                            case (int)eProductOutputType.BTP_HC:
                                item.ProType = "BTP Phối bộ Hoàn Chỉnh";
                                break;
                            default:
                                item.ProType = "Lỗi";
                                var eId = item.ErrorId ?? 0;
                                var eObj = errors.FirstOrDefault(x => x.Id == eId);
                                item.ErrorName = eObj != null ? eObj.Name : "Lỗi(không biết)";
                                break;
                        }
                        item.ErrorId = item.ErrorId == null ? 0 : item.ErrorId;
                        item.CommandType = (item.CommandTypeId == (int)eCommandRecive.ProductIncrease || item.CommandTypeId == (int)eCommandRecive.ErrorIncrease || item.CommandTypeId == (int)eCommandRecive.BTPIncrease) ? "Tăng" : "Giảm";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return info.OrderByDescending(x => x.Time).ToList();
        }

        /// <summary>
        /// Nhập sản lượng ngày bằng tay
        /// </summary>
        /// <param name="obj">TheoDoiNgay obj</param>
        /// <param name="AppId">AppId</param>
        /// <returns></returns>
        public static ResponseBase InsertOrUpdate(TheoDoiNgay obj, int AppId, bool isFrmInsertBTP, List<string> TypeOfCheckFinishProduction)
        {
            var result = new ResponseBase();
            try
            {
                var db = new PMSEntities();
                var str = string.Empty;
                string ngay = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                var date = DateTime.Now;
                bool continueWork = false;

                var chuyen_Sp = db.Chuyen_SanPham.FirstOrDefault(x => !x.IsDelete && x.STT == obj.STTChuyenSanPham);
                if (obj.CommandTypeId == (int)eCommandRecive.ProductReduce)
                {
                    continueWork = true;
                }
                else
                {
                    switch (obj.ProductOutputTypeId)
                    {
                        case (int)eProductOutputType.TC:
                            if (chuyen_Sp.LuyKeBTPThoatChuyen < chuyen_Sp.SanLuongKeHoach)
                                continueWork = true;
                            if (obj.CommandTypeId == (int)eCommandRecive.ProductIncrease)
                                obj.ThanhPham = (obj.ThanhPham + chuyen_Sp.LuyKeBTPThoatChuyen <= chuyen_Sp.SanLuongKeHoach ? obj.ThanhPham : chuyen_Sp.SanLuongKeHoach - chuyen_Sp.LuyKeBTPThoatChuyen);
                            break;
                        case (int)eProductOutputType.KCS:
                            continueWork = chuyen_Sp.LuyKeTH >= chuyen_Sp.SanLuongKeHoach ? false : true;

                            if (obj.CommandTypeId == (int)eCommandRecive.ProductIncrease)
                                obj.ThanhPham = (obj.ThanhPham + chuyen_Sp.LuyKeTH <= chuyen_Sp.SanLuongKeHoach ? obj.ThanhPham : chuyen_Sp.SanLuongKeHoach - chuyen_Sp.LuyKeTH);

                            break;
                        case (int)eProductOutputType.BTP:
                            continueWork = chuyen_Sp.LK_BTP >= chuyen_Sp.SanLuongKeHoach ? false : true;

                            if (obj.CommandTypeId == (int)eCommandRecive.ProductIncrease)
                                obj.ThanhPham = (obj.ThanhPham + chuyen_Sp.LK_BTP <= chuyen_Sp.SanLuongKeHoach ? obj.ThanhPham : chuyen_Sp.SanLuongKeHoach - chuyen_Sp.LK_BTP);

                            break;
                        case (int)eProductOutputType.BTP_HC:
                            continueWork = chuyen_Sp.LK_BTP_HC >= chuyen_Sp.SanLuongKeHoach ? false : true;

                            if (obj.CommandTypeId == (int)eCommandRecive.ProductIncrease)
                                obj.ThanhPham = (obj.ThanhPham + chuyen_Sp.LK_BTP_HC <= chuyen_Sp.SanLuongKeHoach ? obj.ThanhPham : chuyen_Sp.SanLuongKeHoach - chuyen_Sp.LK_BTP_HC);

                            break;
                        case (int)eProductOutputType.Error:
                            continueWork = true;
                            break;
                    }
                }

                if (continueWork)
                {
                    var monthInfo = db.P_MonthlyProductionPlans.FirstOrDefault(x => x.STT_C_SP == obj.STTChuyenSanPham && x.Month == DateTime.Now.Month && x.Year == DateTime.Now.Year);

                    var dayMap = db.MapIdSanPhamNgays.FirstOrDefault(x => !x.IsDeleted && x.STTChuyenSanPham == obj.STTChuyenSanPham && x.MaChuyen == obj.MaChuyen && x.Ngay == ngay);
                    if (dayMap != null)
                    {
                        #region
                        TheoDoiNgay theodoingay = null;
                        var keypad = db.KeyPad_Object.Where(x => !x.IsDeleted && !x.KeyPad.IsDeleted && x.ClusterId == obj.CumId).Select(x => new KeypadModel() { EquipmentId = x.KeyPad.EquipmentId }).FirstOrDefault();
                        var NS_Cum = db.NangSuat_Cum.FirstOrDefault(x => !x.IsDeleted && x.Ngay == ngay && x.STTChuyen_SanPham == obj.STTChuyenSanPham && x.IdCum == obj.CumId);
                        var NangSuat = db.NangXuats.FirstOrDefault(x => !x.IsDeleted && x.Ngay == ngay && x.STTCHuyen_SanPham == obj.STTChuyenSanPham);
                        var hieuTime = BLLDayInfo.TimeIsWork(obj.MaChuyen, date);
                        int second = (int)hieuTime.TotalSeconds;
                        var config = db.Configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eShowLCDConfigName.GetBTPInLineByType) && x.IsActive);
                        int getBTPInLineByType = 0;

                        if (config != null)
                        {
                            var appConfig = db.Config_App.FirstOrDefault(x => x.ConfigId == config.Id && x.AppId == AppId);
                            if (appConfig != null)
                                int.TryParse(appConfig.Value, out getBTPInLineByType);
                        }
                        var btps = db.BTPs.Where(x => x.STTChuyen_SanPham == chuyen_Sp.STT && x.IsEndOfLine);
                        int LKBTPTang = 0, LKBTPGiam = 0;
                        if (btps != null && btps.Count() > 0)
                        {
                            var a = btps.Where(x => x.CommandTypeId == (int)eCommandRecive.BTPIncrease && !x.IsBTP_PB_HC);
                            LKBTPTang = (a != null && a.Count() > 0 ? a.Sum(x => x.BTPNgay) : 0);
                            a = null;
                            a = btps.Where(x => x.CommandTypeId == (int)eCommandRecive.BTPReduce && !x.IsBTP_PB_HC);
                            LKBTPGiam = (a != null && a.Count() > 0 ? a.Sum(x => x.BTPNgay) : 0);
                        }

                        if (obj.STT == 0)
                        {
                            theodoingay = new TheoDoiNgay();
                            theodoingay.MaChuyen = obj.MaChuyen;
                            theodoingay.MaSanPham = obj.MaSanPham;
                            theodoingay.CumId = obj.CumId;
                            theodoingay.STTChuyenSanPham = obj.STTChuyenSanPham;
                            theodoingay.ThanhPham = obj.ThanhPham;
                            theodoingay.Time = date.TimeOfDay;
                            theodoingay.Date = ngay;
                            theodoingay.IsEndOfLine = true;
                            theodoingay.ErrorId = null;

                            switch (obj.ProductOutputTypeId)
                            {
                                case (int)eProductOutputType.KCS:
                                    #region KCS - Kiểm đạt
                                    if (obj.CommandTypeId == (int)eCommandRecive.ProductIncrease)
                                    {
                                        NS_Cum.SanLuongKCSTang += obj.ThanhPham;
                                        theodoingay.CommandTypeId = (int)eCommandRecive.ProductIncrease;

                                        chuyen_Sp.LuyKeTH += obj.ThanhPham;
                                        NangSuat.ThucHienNgay += obj.ThanhPham;

                                        int nhipDoThucTe = second / (NangSuat.ThucHienNgay - NangSuat.ThucHienNgayGiam);
                                        NangSuat.NhipDoThucTe = nhipDoThucTe;

                                        if (monthInfo != null)
                                            monthInfo.LK_TH += obj.ThanhPham;
                                    }
                                    else
                                    {
                                        NS_Cum.SanLuongKCSGiam += obj.ThanhPham;
                                        theodoingay.CommandTypeId = (int)eCommandRecive.ProductReduce;

                                        chuyen_Sp.LuyKeTH -= obj.ThanhPham;
                                        NangSuat.ThucHienNgayGiam += obj.ThanhPham;

                                        int thucHienNgay = NangSuat.ThucHienNgay - NangSuat.ThucHienNgayGiam;
                                        if (thucHienNgay > 0)
                                        {
                                            int nhipDoThucTe = second / thucHienNgay;
                                            NangSuat.NhipDoThucTe = nhipDoThucTe;
                                        }
                                        if (monthInfo != null)
                                            monthInfo.LK_TH -= obj.ThanhPham;
                                    }
                                    theodoingay.ProductOutputTypeId = (int)eProductOutputType.KCS;
                                    NangSuat.IsBTP = 1;
                                    switch (getBTPInLineByType)
                                    {
                                        case 1://  luỹ kế btp - luỹ kế thực hiện 
                                            NangSuat.BTPTrenChuyen = ((LKBTPTang - LKBTPGiam) - chuyen_Sp.LuyKeTH);
                                            break;
                                        case 2: //luỹ kế btp-luỹ kế thoát chuyền 
                                            NangSuat.BTPTrenChuyen = ((LKBTPTang - LKBTPGiam) - chuyen_Sp.LuyKeBTPThoatChuyen);
                                            break;
                                    }
                                    #endregion
                                    db.TheoDoiNgays.Add(theodoingay);
                                    str = keypad.EquipmentId + "," + (int)eCommandSend.ChangeProductQuantity + "," + dayMap.STT + "," + (NangSuat.ThucHienNgay - NangSuat.ThucHienNgayGiam) + "," + (int)eProductOutputType.KCS;
                                    break;
                                case (int)eProductOutputType.TC:
                                    #region Thoát Chuyền
                                    if (obj.CommandTypeId == (int)eCommandRecive.ProductIncrease)
                                    {
                                        NS_Cum.SanLuongTCTang += obj.ThanhPham;
                                        theodoingay.CommandTypeId = (int)eCommandRecive.ProductIncrease;

                                        chuyen_Sp.LuyKeBTPThoatChuyen += obj.ThanhPham;
                                        NangSuat.BTPThoatChuyenNgay += obj.ThanhPham;
                                        if (monthInfo != null)
                                            monthInfo.LK_TC += obj.ThanhPham;
                                    }
                                    else
                                    {
                                        NS_Cum.SanLuongTCGiam += obj.ThanhPham;
                                        theodoingay.CommandTypeId = (int)eCommandRecive.ProductReduce;

                                        chuyen_Sp.LuyKeBTPThoatChuyen -= obj.ThanhPham;
                                        NangSuat.BTPThoatChuyenNgayGiam += obj.ThanhPham;
                                        if (monthInfo != null)
                                            monthInfo.LK_TC -= obj.ThanhPham;
                                    }

                                    theodoingay.ProductOutputTypeId = (int)eProductOutputType.TC;
                                    NangSuat.IsBTP = 1;
                                    int btpThoatChuyenNgay = (NangSuat.BTPThoatChuyenNgay - NangSuat.BTPThoatChuyenNgayGiam);
                                    if (btpThoatChuyenNgay > 0)
                                    {
                                        int nhipDoThucTeBTPThoatChuyen = second / btpThoatChuyenNgay;
                                        NangSuat.NhipDoThucTeBTPThoatChuyen = nhipDoThucTeBTPThoatChuyen;
                                    }

                                    switch (getBTPInLineByType)
                                    {
                                        case 1://  luỹ kế btp - luỹ kế thực hiện 
                                            NangSuat.BTPTrenChuyen = ((LKBTPTang - LKBTPGiam) - chuyen_Sp.LuyKeTH);
                                            break;
                                        case 2: //luỹ kế btp-luỹ kế thoát chuyền 
                                            NangSuat.BTPTrenChuyen = ((LKBTPTang - LKBTPGiam) - chuyen_Sp.LuyKeBTPThoatChuyen);
                                            break;
                                    }
                                    #endregion
                                    db.TheoDoiNgays.Add(theodoingay);
                                    str = keypad.EquipmentId + "," + (int)eCommandSend.ChangeProductQuantity + "," + dayMap.STT + "," + (NangSuat.BTPThoatChuyenNgay - NangSuat.BTPThoatChuyenNgayGiam) + "," + (int)eProductOutputType.TC;
                                    break;
                                case (int)eProductOutputType.BTP:
                                case (int)eProductOutputType.BTP_HC:
                                    #region BTP
                                    var btp = new BTP();
                                    btp.Ngay = ngay;
                                    btp.STTChuyen_SanPham = chuyen_Sp.STT;
                                    btp.STT = 1;
                                    btp.CumId = obj.CumId;
                                    btp.BTPNgay = obj.ThanhPham;
                                    btp.TimeUpdate = date.TimeOfDay;
                                    btp.IsEndOfLine = true;
                                    btp.IsBTP_PB_HC = false;
                                    btp.CreatedDate = DateTime.Now;
                                    if (obj.CommandTypeId == (int)eCommandRecive.ProductIncrease)
                                    {
                                        if (obj.ProductOutputTypeId == (int)eProductOutputType.BTP_HC)
                                        {
                                            btp.IsBTP_PB_HC = true;
                                            NS_Cum.BTP_HC_Tang += obj.ThanhPham;
                                            NangSuat.BTP_HC_Tang += obj.ThanhPham;
                                            chuyen_Sp.LK_BTP_HC += obj.ThanhPham;
                                            if (monthInfo != null)
                                                monthInfo.LK_BTP_HC += obj.ThanhPham;
                                        }
                                        else
                                        {
                                            NS_Cum.BTPTang += obj.ThanhPham;
                                            NangSuat.BTPTang += obj.ThanhPham;
                                            LKBTPTang += obj.ThanhPham;
                                            chuyen_Sp.LK_BTP += obj.ThanhPham;
                                            if (monthInfo != null)
                                                monthInfo.LK_BTP += obj.ThanhPham;
                                        }
                                        btp.CommandTypeId = (int)eCommandRecive.BTPIncrease;
                                    }
                                    else
                                    {
                                        if (obj.ProductOutputTypeId == (int)eProductOutputType.BTP_HC)
                                        {
                                            btp.IsBTP_PB_HC = true;
                                            NS_Cum.BTP_HC_Giam += obj.ThanhPham;
                                            NangSuat.BTP_HC_Giam += obj.ThanhPham;
                                            chuyen_Sp.LK_BTP_HC -= obj.ThanhPham;
                                            if (monthInfo != null)
                                                monthInfo.LK_BTP_HC -= obj.ThanhPham;
                                        }
                                        else
                                        {
                                            NS_Cum.BTPGiam += obj.ThanhPham;
                                            NangSuat.BTPGiam += obj.ThanhPham;
                                            LKBTPGiam += obj.ThanhPham;
                                            chuyen_Sp.LK_BTP -= obj.ThanhPham;
                                            if (monthInfo != null)
                                                monthInfo.LK_BTP -= obj.ThanhPham;
                                        }
                                        btp.CommandTypeId = (int)eCommandRecive.BTPReduce;
                                    }
                                    NangSuat.IsBTP = 1;
                                    switch (getBTPInLineByType)
                                    {
                                        case 1://  luỹ kế btp - luỹ kế thực hiện 
                                            NangSuat.BTPTrenChuyen = ((LKBTPTang - LKBTPGiam) - chuyen_Sp.LuyKeTH);
                                            break;
                                        case 2: //luỹ kế btp-luỹ kế thoát chuyền 
                                            NangSuat.BTPTrenChuyen = ((LKBTPTang - LKBTPGiam) - chuyen_Sp.LuyKeBTPThoatChuyen);
                                            break;
                                    }
                                    #endregion
                                    db.BTPs.Add(btp);
                                    str = keypad.EquipmentId + "," + (int)eCommandSend.ChangeBTPQuantities + "," + dayMap.STT + "," + (NangSuat.BTPTang - NangSuat.BTPGiam) + ",,";
                                    break;
                                case (int)eProductOutputType.Error:
                                    #region Error
                                    var NSCumLoi = db.NangSuat_CumLoi.FirstOrDefault(x => !x.IsDeleted && x.STTChuyenSanPham == chuyen_Sp.STT && x.CumId == obj.CumId && x.Ngay == ngay && x.ErrorId == obj.ErrorId);
                                    var isExists = false;
                                    if (NSCumLoi == null)
                                    {
                                        NSCumLoi = new NangSuat_CumLoi();
                                        NSCumLoi.CumId = obj.CumId;
                                        NSCumLoi.ErrorId = obj.ErrorId ?? 0;
                                        NSCumLoi.Ngay = ngay;
                                        NSCumLoi.STTChuyenSanPham = obj.STTChuyenSanPham;
                                        NSCumLoi.SoLuongGiam = 0;
                                        NSCumLoi.SoLuongTang = 0;
                                        isExists = true;
                                    }

                                    if (obj.CommandTypeId == (int)eCommandRecive.ProductIncrease)
                                    {
                                        NSCumLoi.SoLuongTang += obj.ThanhPham;
                                        theodoingay.CommandTypeId = (int)eCommandRecive.ErrorIncrease;
                                        NangSuat.SanLuongLoi += obj.ThanhPham;
                                    }
                                    else
                                    {
                                        NSCumLoi.SoLuongGiam += obj.ThanhPham;
                                        theodoingay.CommandTypeId = (int)eCommandRecive.ErrorReduce;
                                        NangSuat.SanLuongLoiGiam += obj.ThanhPham;
                                    }
                                    theodoingay.ErrorId = obj.ErrorId;
                                    db.TheoDoiNgays.Add(theodoingay);
                                    if (isExists)
                                        db.NangSuat_CumLoi.Add(NSCumLoi);
                                    #endregion
                                    str = keypad.EquipmentId + "," + (int)eCommandSend.ChangeProductError + "," + obj.ErrorId + "," + (NangSuat.SanLuongLoi - NangSuat.SanLuongLoiGiam) + "," + dayMap.STT;
                                    break;
                            }
                            NangSuat.IsChange = 1;
                        }
                        else
                            theodoingay = GetByStt(obj.STT);

                        chuyen_Sp.IsFinish = false; 
                        if (TypeOfCheckFinishProduction != null && TypeOfCheckFinishProduction.Count > 0)
                            foreach (var item in TypeOfCheckFinishProduction)
                            {
                                if (item == "KCS" && chuyen_Sp.LuyKeTH >= chuyen_Sp.SanLuongKeHoach)
                                {
                                    chuyen_Sp.IsFinish = true;
                                    chuyen_Sp.STTThucHien = 900;
                                    break;
                                }
                                else if (item == "TC" && chuyen_Sp.LuyKeBTPThoatChuyen >= chuyen_Sp.SanLuongKeHoach)
                                {
                                    chuyen_Sp.IsFinish = true;
                                    chuyen_Sp.STTThucHien = 900;
                                    break;
                                }
                                else if (item == "BTP" && chuyen_Sp.LK_BTP >= chuyen_Sp.SanLuongKeHoach)
                                {
                                    chuyen_Sp.IsFinish = true;
                                    chuyen_Sp.STTThucHien = 900;
                                    break;
                                }
                            } 
                         
                        db.SaveChanges();
                        result.IsSuccess = true;
                        result.DataSendKeyPad = str;
                        if (chuyen_Sp.IsFinish)
                        {
                            BLLDayInfo.CreateNewDayInfoAfterFinishAssignment(chuyen_Sp.MaChuyen);
                            result.Records = chuyen_Sp.IsFinish;
                        }
                        #endregion
                    }
                    else if (isFrmInsertBTP)
                    {
                        #region
                        switch (obj.ProductOutputTypeId)
                        {
                            case (int)eProductOutputType.BTP:
                            case (int)eProductOutputType.BTP_HC:
                                #region BTP
                                var btp = new BTP();
                                btp.Ngay = ngay;
                                btp.STTChuyen_SanPham = chuyen_Sp.STT;
                                btp.STT = 1;
                                btp.CumId = obj.CumId;
                                btp.BTPNgay = obj.ThanhPham;
                                btp.TimeUpdate = date.TimeOfDay;
                                btp.IsEndOfLine = true;
                                btp.IsBTP_PB_HC = false;
                                btp.CreatedDate = DateTime.Now;
                                if (obj.CommandTypeId == (int)eCommandRecive.ProductIncrease)
                                {
                                    if (obj.ProductOutputTypeId == (int)eProductOutputType.BTP_HC)
                                    {
                                        btp.IsBTP_PB_HC = true;
                                        chuyen_Sp.LK_BTP_HC += obj.ThanhPham;
                                        if (monthInfo != null)
                                            monthInfo.LK_BTP_HC += obj.ThanhPham;
                                    }
                                    else
                                    {
                                        chuyen_Sp.LK_BTP += obj.ThanhPham;
                                        if (monthInfo != null)
                                            monthInfo.LK_BTP += obj.ThanhPham;
                                    }
                                    btp.CommandTypeId = (int)eCommandRecive.BTPIncrease;
                                }
                                else
                                {
                                    if (obj.ProductOutputTypeId == (int)eProductOutputType.BTP_HC)
                                    {
                                        btp.IsBTP_PB_HC = true;
                                        chuyen_Sp.LK_BTP_HC -= obj.ThanhPham;
                                        if (monthInfo != null)
                                            monthInfo.LK_BTP_HC -= obj.ThanhPham;
                                    }
                                    else
                                    {
                                        chuyen_Sp.LK_BTP -= obj.ThanhPham;
                                        if (monthInfo != null)
                                            monthInfo.LK_BTP -= obj.ThanhPham;
                                    }
                                    btp.CommandTypeId = (int)eCommandRecive.BTPReduce;
                                }
                                #endregion
                                db.BTPs.Add(btp);
                                break;
                        }
                        db.SaveChanges();
                        result.IsSuccess = true;
                        result.DataSendKeyPad = null;
                        result.Records = null;
                        #endregion
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Messages.Add(new Message() { Title = "Lỗi", msg = "Chuyền chưa nhập thông tin ngày. Vui lòng nhập thông tin ngày cho chuyền." });
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Messages.Add(new Message() { Title = "Lỗi", msg = "Sản lượng đã đủ không thể thêm được nữa." });
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Messages.Add(new Message() { Title = "Lỗi Exception", msg = "Nhập thông tin ngày cho chuyền bi lỗi." });
            }
            return result;
        }


        /// <summary>
        /// Function set lại LK BTP for all
        /// </summary>
        public static void SetlailkBTPchochuyen()
        {
            try
            {
                var db = new PMSEntities();
                var btps = db.BTPs.Where(x => !x.IsDeleted && !x.IsBTP_PB_HC && x.IsEndOfLine).ToList();
                var sttcsp = btps.Select(x => x.STTChuyen_SanPham).Distinct().ToList();
                var csps = db.Chuyen_SanPham.Where(x => sttcsp.Contains(x.STT)).ToList();
                var mdetails = db.P_MonthlyProductionPlans.Where(x => !x.IsDeleted && sttcsp.Contains(x.STT_C_SP)).ToList();
                foreach (var stt in sttcsp)
                {
                    var csp = csps.FirstOrDefault(x => x.STT == stt);
                    if (csp != null)
                    {
                        int LKBTPTang = 0, LKBTPGiam = 0;
                        var btpOfLine = btps.Where(x => x.STTChuyen_SanPham == stt).ToList();
                        if (btpOfLine.Count() > 0)
                        {
                            LKBTPTang = btpOfLine.Where(x => x.CommandTypeId == (int)eCommandRecive.BTPIncrease).Sum(x => x.BTPNgay);
                            LKBTPGiam = btpOfLine.Where(x => x.CommandTypeId == (int)eCommandRecive.BTPReduce).Sum(x => x.BTPNgay);
                            csp.LK_BTP = LKBTPTang - LKBTPGiam;

                            var mdetai = mdetails.Where(x => x.STT_C_SP == stt).OrderBy(x => x.Month).ThenBy(x => x.Year);
                            if (mdetai != null && mdetai.Count() > 0)
                            {
                                foreach (var item in mdetai)
                                {
                                    LKBTPTang = btpOfLine.Where(x => x.CommandTypeId == (int)eCommandRecive.BTPIncrease && x.CreatedDate.Month == item.Month && x.CreatedDate.Year == item.Year).Sum(x => x.BTPNgay);
                                    LKBTPGiam = btpOfLine.Where(x => x.CommandTypeId == (int)eCommandRecive.BTPReduce && x.CreatedDate.Month == item.Month && x.CreatedDate.Year == item.Year).Sum(x => x.BTPNgay);
                                    item.LK_BTP = LKBTPTang - LKBTPGiam;
                                }
                            }
                        }
                    }
                }
                db.SaveChanges();
            }
            catch (Exception)
            {
            }
        }


        /// <summary>
        /// Xoa thông tin ngày
        /// </summary>
        /// <param name="stt">số thứ tự chuyền sản phẫm</param>
        /// <param name="date">ngày yêu cầu xóa</param>
        /// <returns></returns>
        public static bool Delete(int stt, string date)
        {
            try
            {
                var db = new PMSEntities();
                var tp = db.ThanhPhams.FirstOrDefault(x => !x.IsDeleted && x.STTChuyen_SanPham == stt && x.Ngay == date);
                if (tp != null)
                {
                    tp.IsDeleted = true;

                    var btp = db.BTPs.Where(x => !x.IsDeleted && x.STTChuyen_SanPham == stt && x.Ngay == date).ToList();
                    if (btp.Count > 0)
                    {
                        foreach (var item in btp)
                        {
                            item.IsDeleted = true;
                        }
                    }

                    var nx = db.NangXuats.FirstOrDefault(x => !x.IsDeleted && x.STTCHuyen_SanPham == stt && x.Ngay == date);
                    if (nx != null)
                        nx.IsDeleted = true;
                    var nx_c = db.NangSuat_Cum.Where(x => !x.IsDeleted && x.STTChuyen_SanPham == stt && x.Ngay == date).ToList();
                    if (nx_c.Count > 0)
                    {
                        foreach (var item in nx_c)
                        {
                            item.IsDeleted = true;
                        }
                    }
                    var nx_cL = db.NangSuat_CumLoi.Where(x => !x.IsDeleted && x.STTChuyenSanPham == stt && x.Ngay == date).ToList();
                    if (nx_cL.Count > 0)
                    {
                        foreach (var item in nx_cL)
                        {
                            item.IsDeleted = true;
                        }
                    }
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        public static TheoDoiNgay GetByStt(int stt)
        {
            try
            {
                var db = new PMSEntities();
                return db.TheoDoiNgays.FirstOrDefault(x => x.STT == stt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static TimeSpan TimeIsWork(int MaChuyen, DateTime date)
        {
            TimeSpan timeWork = new TimeSpan();
            try
            {
                var db = new PMSEntities();
                var daynow = date.Day + "/" + date.Month + "/" + date.Year;
                var TGTNDTT = db.ThoiGianTinhNhipDoTTs.FirstOrDefault(x => x.MaChuyen == MaChuyen && x.Ngay == daynow);
                var shiftsOfLine = BLLShift.GetWorkingTimeOfLine(MaChuyen);

                TimeSpan timeStartTT = TGTNDTT != null ? TGTNDTT.ThoiGianBatDau : TimeSpan.Parse("00:00:00");

                timeWork = TimeSpan.Parse("00:00:00");
                TimeSpan timeNow = DateTime.Now.TimeOfDay;
                if (shiftsOfLine != null && shiftsOfLine.Count() > 0)
                {
                    foreach (var shift in shiftsOfLine)
                    {
                        if (timeNow > shift.TimeStart)
                        {
                            if (timeNow < shift.TimeEnd)
                                timeWork += (timeNow - shift.TimeStart);
                            else
                                timeNow += (shift.TimeEnd - shift.TimeStart);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return timeWork;
        }

        private static TimeSpan TimeIsWork(int MaChuyen)
        {
            TimeSpan timeWork = new TimeSpan();
            try
            {
                var db = new PMSEntities();
                DateTime daynow = DateTime.Now.Date;
                timeWork = TimeSpan.Parse("00:00:00");
                TimeSpan timeNow = DateTime.Now.TimeOfDay;
                var chuyenTimeWork = BLLShift.GetShiftsOfLine(MaChuyen);
                var obj = db.ThoiGianTinhNhipDoTTs.FirstOrDefault(x => x.MaChuyen == MaChuyen);
                TimeSpan timeStartTT = obj != null ? TimeSpan.Parse(obj.ThoiGianBatDau.ToString()) : TimeSpan.Parse("00:00:00");
                foreach (var item in chuyenTimeWork.OrderBy(x => x.ShiftOrder))
                {
                    if (item.Start < timeStartTT && timeStartTT < item.End)
                    {
                        item.Start = timeStartTT;
                    }
                    else if (item.End <= timeStartTT)
                    {
                        chuyenTimeWork.Remove(item);
                    }
                }
                if (chuyenTimeWork.Count > 0)
                {
                    for (int j = 0; j < chuyenTimeWork.Count; j++)
                    {
                        if (timeNow > chuyenTimeWork[j].Start)
                        {
                            if (timeNow < chuyenTimeWork[j].End)
                            {
                                timeWork += (timeNow - chuyenTimeWork[j].Start);
                            }
                            else
                            {
                                timeWork += (chuyenTimeWork[j].End - chuyenTimeWork[j].Start);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return timeWork;
        }

        public static bool KeypadInsert(TheoDoiNgay obj)
        {
            try
            {
                var db = new PMSEntities();
                db.TheoDoiNgays.Add(obj);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static List<TheoDoiNgay> GetByCommoId(int sttCSP, int CommoId, string ngay)
        {
            try
            {
                var db = new PMSEntities();
                return db.TheoDoiNgays.Where(x => x.Date == ngay && x.STTChuyenSanPham == sttCSP && x.MaSanPham == CommoId).ToList();
            }
            catch (Exception)
            {
            }
            return new List<TheoDoiNgay>();
        }

        public static ResponseBase KeypadInsert(int clusterId, int quantityIncrease, int sttChuyenSanPham, int maSanPham, int productCode, int lineId, bool isEndOfLine, int errorId, int total, int equipmentId, bool isIncrease, int ProductType, int setTotalByMinOrMax, int getBTPInLineByType)
        {
            var result = new ResponseBase();
            try
            {
                var db = new PMSEntities();
                string dateNow = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                var isCheckAndSendTotal = false;
                if (sttChuyenSanPham == 0)
                {
                    isCheckAndSendTotal = true;
                    var mapId = db.MapIdSanPhamNgays.FirstOrDefault(x => !x.IsDeleted && x.MaChuyen == lineId && x.Ngay == dateNow && x.STT == productCode);
                    if (mapId != null)
                    {
                        sttChuyenSanPham = mapId.STTChuyenSanPham;
                        maSanPham = mapId.MaSanPham;
                    }
                }

                if (sttChuyenSanPham > 0)
                {
                    dynamic nangSuatCum;
                    if (ProductType == (int)eProductOutputType.Error)
                        nangSuatCum = BLLProductivity.Find_NangSuatCumLoi(clusterId, errorId, sttChuyenSanPham, dateNow);
                    else
                        nangSuatCum = BLLProductivity.Find_NangSuatCum(sttChuyenSanPham, clusterId, dateNow);
                    if (nangSuatCum != null)
                    {
                        #region sử lý NS Cụm

                        #region
                        switch (ProductType)
                        {
                            case (int)eProductOutputType.KCS:
                                if (isIncrease)
                                    nangSuatCum.SanLuongKCSTang += quantityIncrease;
                                else
                                    nangSuatCum.SanLuongKCSGiam += quantityIncrease;
                                break;
                            case (int)eProductOutputType.TC:
                                if (isIncrease)
                                    nangSuatCum.SanLuongTCTang += quantityIncrease;
                                else
                                    nangSuatCum.SanLuongTCGiam += quantityIncrease;
                                break;
                            case (int)eProductOutputType.Error:
                                if (isIncrease)
                                    nangSuatCum.SoLuongTang += quantityIncrease;
                                else
                                    nangSuatCum.SoLuongGiam += quantityIncrease;
                                break;
                        }
                        #endregion

                        int min = total, max = total;
                        if (isCheckAndSendTotal)
                        {
                            int tongSanLuong = 0;

                            #region
                            switch (ProductType)
                            {
                                case (int)eProductOutputType.KCS:
                                    tongSanLuong = nangSuatCum.SanLuongKCSTang - nangSuatCum.SanLuongKCSGiam;
                                    break;
                                case (int)eProductOutputType.TC:
                                    tongSanLuong = nangSuatCum.SanLuongTCTang - nangSuatCum.SanLuongTCGiam;
                                    break;
                                case (int)eProductOutputType.Error:
                                    tongSanLuong = nangSuatCum.SoLuongTang - nangSuatCum.SoLuongGiam;
                                    break;
                            }
                            #endregion

                            if (tongSanLuong != total)
                            {
                                if (tongSanLuong > total)
                                {
                                    #region
                                    min = total;
                                    max = tongSanLuong;
                                    switch (setTotalByMinOrMax)
                                    {
                                        case 1: //nho                                      
                                        case 2: //lon
                                            switch (ProductType)
                                            {
                                                case (int)eProductOutputType.KCS:
                                                    result.DataSendKeyPad = (equipmentId + "," + (int)eCommandSend.ChangeProductQuantity + "," + productCode + "," + max + "," + (int)eProductOutputType.KCS); break;
                                                case (int)eProductOutputType.TC:
                                                    result.DataSendKeyPad = (equipmentId + "," + (int)eCommandSend.ChangeProductQuantity + "," + productCode + "," + max + "," + (int)eProductOutputType.TC); break;
                                                case (int)eProductOutputType.Error:
                                                    result.DataSendKeyPad = (equipmentId + "," + (int)eCommandSend.ChangeProductError + "," + errorId + "," + max + "," + productCode); break;
                                            }
                                            break;
                                        case 3: //keypad
                                            switch (ProductType)
                                            {
                                                case (int)eProductOutputType.KCS:
                                                    nangSuatCum.SanLuongKCSGiam += (max - min); break;
                                                case (int)eProductOutputType.TC:
                                                    nangSuatCum.SanLuongTCGiam += (max - min); break;
                                                case (int)eProductOutputType.Error:
                                                    nangSuatCum.SoLuongGiam += (max - min); break;
                                            }
                                            break;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region
                                    min = tongSanLuong;
                                    max = total;
                                    switch (setTotalByMinOrMax)
                                    {
                                        case 1: // nho
                                            switch (ProductType)
                                            {
                                                case (int)eProductOutputType.KCS:
                                                    result.DataSendKeyPad = (equipmentId + "," + (int)eCommandSend.ChangeProductQuantity + "," + productCode + "," + min + "," + (int)eProductOutputType.KCS); break;
                                                case (int)eProductOutputType.TC:
                                                    result.DataSendKeyPad = (equipmentId + "," + (int)eCommandSend.ChangeProductQuantity + "," + productCode + "," + min + "," + (int)eProductOutputType.TC); break;
                                                case (int)eProductOutputType.Error:
                                                    result.DataSendKeyPad = (equipmentId + "," + (int)eCommandSend.ChangeProductError + "," + errorId + "," + min + "," + productCode); break;
                                            }
                                            break;
                                        case 2: // lon
                                        case 3: //keypad
                                            switch (ProductType)
                                            {
                                                case (int)eProductOutputType.KCS:
                                                    nangSuatCum.SanLuongKCSTang += (max - min); break;
                                                case (int)eProductOutputType.TC:
                                                    nangSuatCum.SanLuongTCTang += (max - min); break;
                                                case (int)eProductOutputType.Error:
                                                    nangSuatCum.SoLuongTang += (max - min); break;
                                            }
                                            break;
                                    }
                                    #endregion
                                }
                            }
                        }
                        if (ProductType == (int)eProductOutputType.Error)
                            BLLProductivity.Update_NS_CumLoi(nangSuatCum);
                        else
                            BLLProductivity.Update_NS_Cum(nangSuatCum);
                        #endregion

                        #region Sử lý cụm cuối chuyền
                        if (isEndOfLine)
                        {
                            #region
                            var nangSuat = BLLProductivity.TTNangXuatTrongNgay(dateNow, sttChuyenSanPham);
                            var chuyenSanPham = BLLAssignmentForLine.Instance.GetAssignmentByDay(dateNow, sttChuyenSanPham, lineId);
                            var monthPro = BLLMonthlyProductionPlans.Find(sttChuyenSanPham, DateTime.Now.Month, DateTime.Now.Year);

                            var old = nangSuat.ThucHienNgay - nangSuat.ThucHienNgayGiam;
                            switch (setTotalByMinOrMax)
                            {
                                case (int)eSetTotalByMinOrMax.byMin:
                                    #region
                                    switch (ProductType)
                                    {
                                        case (int)eProductOutputType.KCS:
                                            nangSuat.ThucHienNgay = min;
                                            nangSuat.ThucHienNgayGiam = 0;
                                            break;
                                        case (int)eProductOutputType.TC:
                                            nangSuat.BTPThoatChuyenNgay = min;
                                            nangSuat.BTPThoatChuyenNgayGiam = 0;
                                            break;
                                        case (int)eProductOutputType.Error:
                                            nangSuat.SanLuongLoi = min;
                                            nangSuat.SanLuongLoiGiam = 0;
                                            break;
                                    }
                                    #endregion
                                    break;
                                case (int)eSetTotalByMinOrMax.byMax:
                                    #region
                                    switch (ProductType)
                                    {
                                        case (int)eProductOutputType.KCS:
                                            nangSuat.ThucHienNgay = max;
                                            nangSuat.ThucHienNgayGiam = 0;
                                            break;
                                        case (int)eProductOutputType.TC:
                                            nangSuat.BTPThoatChuyenNgay = max;
                                            nangSuat.BTPThoatChuyenNgayGiam = 0;
                                            break;
                                        case (int)eProductOutputType.Error:
                                            nangSuat.SanLuongLoi = max;
                                            nangSuat.SanLuongLoiGiam = 0;
                                            break;
                                    }
                                    #endregion
                                    break;
                                case (int)eSetTotalByMinOrMax.byKeypad:
                                    #region
                                    switch (ProductType)
                                    {
                                        case (int)eProductOutputType.KCS:
                                            nangSuat.ThucHienNgay = total;
                                            nangSuat.ThucHienNgayGiam = 0;
                                            break;
                                        case (int)eProductOutputType.TC:
                                            nangSuat.BTPThoatChuyenNgay = total;
                                            nangSuat.BTPThoatChuyenNgayGiam = 0;
                                            break;
                                        case (int)eProductOutputType.Error:
                                            nangSuat.SanLuongLoi = total;
                                            nangSuat.SanLuongLoiGiam = 0;
                                            break;
                                    }
                                    #endregion
                                    break;
                            }
                            nangSuat.IsBTP = 1;
                            if (ProductType == (int)eProductOutputType.KCS)
                            {
                                chuyenSanPham.LuyKeTH = ((chuyenSanPham.LuyKeTH - old) + nangSuat.ThucHienNgay);
                                monthPro.LK_TH = (monthPro.LK_TH - old) + nangSuat.ThucHienNgay;
                            }
                            else
                            {
                                chuyenSanPham.LuyKeBTPThoatChuyen = ((chuyenSanPham.LuyKeBTPThoatChuyen - old) + nangSuat.BTPThoatChuyenNgay);
                                monthPro.LK_TC = (monthPro.LK_TC - old) + nangSuat.BTPThoatChuyenNgay;
                            }

                            #region Báo hết hàng
                            var baoHetHangs = db.BaoHetHangs.OrderByDescending(x => x.SoSanPhamConLai).ToList();
                            if (baoHetHangs.Count > 0)
                            {
                                if ((chuyenSanPham.SanLuongKeHoach - chuyenSanPham.LuyKeTH) <= baoHetHangs[0].SoSanPhamConLai)
                                {
                                    for (int n = 0; n < baoHetHangs.Count; n++)
                                    {
                                        if ((chuyenSanPham.SanLuongKeHoach - chuyenSanPham.LuyKeTH) <= baoHetHangs[n].SoSanPhamConLai)
                                        {
                                            result.AlertRepeat = baoHetHangs[n].SoLanBao;
                                            if (n == baoHetHangs.Count - 1)
                                            {
                                                chuyenSanPham.IsFinishNow = true;
                                                chuyenSanPham.IsFinish = true;
                                            }
                                        }
                                        else
                                            break;
                                    }

                                    if (chuyenSanPham.CountAssignment <= 1)
                                        result.AlertHetHang = true;
                                }
                                else
                                {
                                    if (!chuyenSanPham.IsFinishNow)
                                        chuyenSanPham.IsFinishNow = false;
                                    chuyenSanPham.IsFinish = false;
                                }
                            }
                            else
                            {
                                if (chuyenSanPham.LuyKeTH >= chuyenSanPham.SanLuongKeHoach)
                                {
                                    chuyenSanPham.IsFinish = true;
                                    chuyenSanPham.IsFinishNow = true;
                                }
                                else
                                {
                                    chuyenSanPham.IsFinish = false;
                                    chuyenSanPham.IsFinishNow = false;
                                }
                            }
                            #endregion

                            TimeSpan hieutime = new TimeSpan();
                            hieutime = BLLDayInfo.TimeIsWork(lineId);
                            int second = (int)hieutime.TotalSeconds;
                            if (ProductType == (int)eProductOutputType.KCS)
                            {
                                int thucHienNgay = nangSuat.ThucHienNgay - nangSuat.ThucHienNgayGiam;
                                if (thucHienNgay > 0)
                                {
                                    int nhipDoThucTe = second / thucHienNgay;
                                    nangSuat.NhipDoThucTe = nhipDoThucTe;
                                }
                            }
                            else
                            {
                                int btpThoatChuyenNgay = (nangSuat.BTPThoatChuyenNgay - nangSuat.BTPThoatChuyenNgayGiam);
                                if (btpThoatChuyenNgay > 0)
                                {
                                    int nhipDoThucTeBTPThoatChuyen = second / btpThoatChuyenNgay;
                                    nangSuat.NhipDoThucTeBTPThoatChuyen = nhipDoThucTeBTPThoatChuyen;
                                }
                            }

                            switch (getBTPInLineByType)
                            {
                                case 1:
                                    nangSuat.BTPTrenChuyen = (chuyenSanPham.LK_BTP - chuyenSanPham.LK_BTP_G) - chuyenSanPham.LuyKeTH;
                                    break;
                                case 2:
                                    nangSuat.BTPTrenChuyen = (chuyenSanPham.LK_BTP - chuyenSanPham.LK_BTP_G) - chuyenSanPham.LuyKeBTPThoatChuyen;
                                    break;
                            }
                            nangSuat.TimeLastChange = DateTime.Now.TimeOfDay;
                            BLLProductivity.UpdateNangXuat(nangSuat);
                            BLLMonthlyProductionPlans.Update(monthPro);
                            BLLAssignmentForLine.Instance.Update(sttChuyenSanPham, chuyenSanPham.LuyKeTH, null, chuyenSanPham.IsFinish, chuyenSanPham.IsFinishNow);
                            #endregion

                            #region
                            var tdn = new PMS.Data.TheoDoiNgay();
                            tdn.MaChuyen = lineId;
                            tdn.MaSanPham = maSanPham;
                            tdn.CumId = clusterId;
                            tdn.STTChuyenSanPham = sttChuyenSanPham;
                            tdn.Time = DateTime.Now.TimeOfDay;
                            tdn.Date = dateNow;
                            tdn.IsEndOfLine = isEndOfLine;
                            tdn.IsEnterByKeypad = true;

                            var tdns = GetByCommoId(sttChuyenSanPham, maSanPham, dateNow);
                            int Tang = 0, Giam = 0;
                            switch (ProductType)
                            {
                                case (int)eProductOutputType.KCS:
                                    #region
                                    Tang = tdns.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductIncrease && c.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(c => c.ThanhPham);
                                    Giam = tdns.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductReduce && c.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(c => c.ThanhPham);
                                    tdn.ProductOutputTypeId = (int)eProductOutputType.KCS;
                                    Tang = (Tang - Giam) < 0 ? 0 : (Tang - Giam);
                                    if (Tang > nangSuat.ThucHienNgay)
                                    {
                                        tdn.ThanhPham = Tang - nangSuat.ThucHienNgay;
                                        tdn.CommandTypeId = (int)eCommandRecive.ProductReduce;
                                    }
                                    else
                                    {
                                        tdn.ThanhPham = nangSuat.ThucHienNgay - Tang;
                                        tdn.CommandTypeId = (int)eCommandRecive.ProductIncrease;
                                    }
                                    #endregion
                                    break;
                                case (int)eProductOutputType.TC:
                                    #region
                                    Tang = tdns.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductIncrease && c.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(c => c.ThanhPham);
                                    Giam = tdns.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductReduce && c.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(c => c.ThanhPham);
                                    tdn.ProductOutputTypeId = (int)eProductOutputType.TC;
                                    Tang = (Tang - Giam) < 0 ? 0 : (Tang - Giam);
                                    if (Tang > nangSuat.BTPThoatChuyenNgay)
                                    {
                                        tdn.ThanhPham = Tang - nangSuat.BTPThoatChuyenNgay;
                                        tdn.CommandTypeId = (int)eCommandRecive.ProductReduce;
                                    }
                                    else
                                    {
                                        tdn.ThanhPham = nangSuat.BTPThoatChuyenNgay - Tang;
                                        tdn.CommandTypeId = (int)eCommandRecive.ProductIncrease;
                                    }
                                    #endregion
                                    break;
                                case (int)eProductOutputType.Error:
                                    #region
                                    Tang = tdns.Where(c => c.CommandTypeId == (int)eCommandRecive.ErrorIncrease).Sum(c => c.ThanhPham);
                                    Giam = tdns.Where(c => c.CommandTypeId == (int)eCommandRecive.ErrorReduce).Sum(c => c.ThanhPham);
                                    if (Tang > nangSuat.SanLuongLoi)
                                    {
                                        tdn.ThanhPham = Tang - nangSuat.SanLuongLoi;
                                        tdn.CommandTypeId = (int)eCommandRecive.ErrorReduce;
                                    }
                                    else
                                    {
                                        tdn.ThanhPham = nangSuat.SanLuongLoi - Tang;
                                        tdn.CommandTypeId = (int)eCommandRecive.ErrorIncrease;
                                    }
                                    #endregion
                                    break;
                            }
                            KeypadInsert(tdn);
                            #endregion
                        }
                        #endregion

                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.IsPlaySound = true;
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.IsPlaySound = true;
                }
            }
            catch (Exception)
            {
            }
            return result;
        }

        public static void DeleteAllInformation()
        {
            try
            {
                var db = new PMSEntities();
                db.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('TheoDoiNgay',RESEED,0)");
                DateTime time = DateTime.Now.AddDays(-14).Date;
                db.Database.ExecuteSqlCommand("delete from TheoDoiNgay where Date < '" + time + "'");
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<TheoDoiNgay> GetByStt_CSP(List<int> sttCSPs, string ngay)
        {
            try
            {
                var db = new PMSEntities();
                return db.TheoDoiNgays.Where(x => x.Date == ngay && sttCSPs.Contains(x.STTChuyenSanPham)).ToList();
            }
            catch (Exception)
            {
            }
            return new List<TheoDoiNgay>();
        }

        /// <summary>
        /// tạo thông tin ngày của sản phẫm kế tiếp sau khi kết thúc mã hàng 
        /// </summary>
        /// <param name="lineId">mã chuyền</param> 
        /// <returns></returns>
        public static ResponseBase CreateNewDayInfoAfterFinishAssignment(int lineId)
        {
            var rs = new ResponseBase();
            try
            {
                var db = new PMSEntities();
                var csp = db.Chuyen_SanPham.Where(x => !x.IsDelete && !x.Chuyen.IsDeleted && x.MaChuyen == lineId && !x.SanPham.IsDelete && !x.IsFinish).OrderBy(x => x.STTThucHien).FirstOrDefault();
                if (csp != null)
                {
                    string date = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                    int workingTime = (int)BLLShift.GetTotalWorkingHourOfLine(lineId).TotalSeconds;
                    var oldTp = db.ThanhPhams.FirstOrDefault(x => !x.IsDeleted && x.Chuyen_SanPham.MaChuyen == lineId && x.Ngay == date);
                    if (oldTp != null)
                    {
                        #region 1. tạo thong tin thành phẩm
                        ThanhPham tp = db.ThanhPhams.FirstOrDefault(x => !x.IsDeleted && x.Ngay == date && x.STTChuyen_SanPham == csp.STT);
                        if (tp == null)
                        {
                            tp = new ThanhPham();
                            tp.Ngay = date;
                            tp.STTChuyen_SanPham = csp.STT;
                            tp.LeanKH = oldTp.LeanKH;
                            tp.NangXuatLaoDong = (float)Math.Round((workingTime / ((csp.SanPham.ProductionTime *100 )/oldTp.HieuSuat)), 2);
                            tp.LaoDongChuyen = oldTp.LaoDongChuyen;
                            tp.HieuSuat = oldTp.HieuSuat;
                            tp.CreatedDate = DateTime.Now;
                            tp.IsDeleted = false;
                            db.ThanhPhams.Add(tp);
                        }
                        #endregion

                        #region 2. tạo thong tin nang xuat
                        NangXuat nangxuat = db.NangXuats.FirstOrDefault(x => !x.IsDeleted && x.Ngay == date && x.STTCHuyen_SanPham == csp.STT);
                        if (nangxuat == null)
                        {
                            nangxuat = new NangXuat();
                            nangxuat.Ngay = date;
                            nangxuat.STTCHuyen_SanPham = csp.STT;
                            nangxuat.DinhMucNgay = (float)Math.Round((tp.NangXuatLaoDong * tp.LaoDongChuyen), 1);
                            nangxuat.NhipDoSanXuat = (float)Math.Round((((csp.SanPham.ProductionTime*100)/oldTp.HieuSuat) / tp.LaoDongChuyen), 1);
                            nangxuat.TimeLastChange = DateTime.Now.TimeOfDay;
                            nangxuat.BTPTrenChuyen = 0;
                            nangxuat.IsEndDate = false;
                            nangxuat.CreatedDate = DateTime.Now;
                            db.NangXuats.Add(nangxuat);
                        }
                        #endregion

                        var clusters = BLLProductivity.GetClustersOfLine(lineId);
                        if (clusters != null && clusters.Count > 0)
                        {
                            var clusterIds = clusters.Select(x => x.Id).Distinct();

                            #region 3. tao thong tin nang xuat cum
                            var ns_cums = db.NangSuat_Cum.Where(x => !x.IsDeleted && clusterIds.Contains(x.IdCum) && x.STTChuyen_SanPham == csp.STT && x.Ngay == date).ToList();
                            NangSuat_Cum nxc;
                            foreach (var c in clusters)
                            {
                                var check = ns_cums.FirstOrDefault(x => x.IdCum == c.Id);
                                if (check == null)
                                {
                                    nxc = new NangSuat_Cum();
                                    nxc.Ngay = date;
                                    nxc.STTChuyen_SanPham = csp.STT;
                                    nxc.IdCum = c.Id;
                                    nxc.CreatedDate = DateTime.Now;
                                    db.NangSuat_Cum.Add(nxc);
                                }
                            }
                            #endregion

                            #region 4. tao thong tin nang xuat cum loi
                            var errors = BLLError.GetAll();
                            if (errors != null && errors.Count > 0)
                            {
                                var ns_cumlois = db.NangSuat_CumLoi.Where(x => !x.IsDeleted && clusterIds.Contains(x.CumId) && x.STTChuyenSanPham == csp.STT && x.Ngay == date).ToList();
                                NangSuat_CumLoi nxc_l;
                                foreach (var c in clusters)
                                {
                                    foreach (var err in errors)
                                    {
                                        var check = ns_cumlois.FirstOrDefault(x => x.CumId == c.Id && x.ErrorId == err.Id);
                                        if (check == null)
                                        {
                                            nxc_l = new NangSuat_CumLoi();
                                            nxc_l.Ngay = date;
                                            nxc_l.STTChuyenSanPham = csp.STT;
                                            nxc_l.CumId = c.Id;
                                            nxc_l.ErrorId = err.Id;
                                            nxc_l.CreatedDate = DateTime.Now;
                                            db.NangSuat_CumLoi.Add(nxc_l);
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        db.SaveChanges();
                        rs.IsSuccess = true;
                    }
                    else
                    {
                        rs.IsSuccess = false;
                        rs.Messages.Add(new Message() { Title = "Thông báo", msg = "Không tìm thấy thông tin ngày vui lòng nhập thông tin ngày cho chuyền." });
                    }
                }
                else
                    rs.IsSuccess = false;
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Messages.Add(new Message() { Title = "Lỗi Exception", msg = "Không thể tự động tạo thông tin ngày cho chuyền." });
            }
            return rs;
        }

    }
}
