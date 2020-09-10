using PMS.Business.Enum;
using PMS.Business.Models;
using PMS.Business.Web.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PMS.Business.Web
{
    public class BLLLCD
    {
        static Object key = new object();
        private static volatile BLLLCD _Instance;
        public static BLLLCD Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLLCD();

                return _Instance;
            }
        }

        private BLLLCD() { }

        public List<Kanban_LCD> GetKanBanLCD(List<int> listLineId, int tableType, bool includingBTPHC)
        {
            var listModel = new List<Kanban_LCD>();
            var now = DateTime.Now.ToString("d/M/yyyy");
            try
            {
                using (var db = new PMSEntities())
                {
                    if (listLineId != null && listLineId.Count > 0)
                    {
                        listLineId = listLineId.Distinct().ToList();
                        var listPCC = db.Chuyen_SanPham.Where(c => listLineId.Contains(c.MaChuyen) && !c.IsDelete && !c.IsFinish && !c.SanPham.IsDelete && !c.Chuyen.IsDeleted).ToList();
                        if (listPCC.Count > 0)
                        {
                            var listSTTLineProduct = listPCC.Select(c => c.STT).ToList();
                            var nangsuatngays = db.NangXuats.Where(c => listSTTLineProduct.Contains(c.STTCHuyen_SanPham) && !c.IsDeleted && c.Ngay == now).ToList();
                            var thanhphamngays = db.ThanhPhams.Where(c => listSTTLineProduct.Contains(c.STTChuyen_SanPham) && !c.IsDeleted && c.Ngay == now).ToList();
                            var btpngays = db.BTPs.Where(c => listSTTLineProduct.Contains(c.STTChuyen_SanPham) && !c.IsDeleted && c.IsEndOfLine && (c.CommandTypeId == 8 || c.CommandTypeId == 13)).ToList();
                            foreach (var item in listPCC.OrderBy(x => x.MaChuyen).ThenBy(x => x.STTThucHien))
                            {
                                var model = new Kanban_LCD();
                                //  var line = db.Chuyens.Where(c => c.MaChuyen == item.MaChuyen && !c.IsDeleted).FirstOrDefault();
                                //  var PCOfLine = listPCC.FirstOrDefault(c => c.STT == item.STT);

                                //   var product = db.SanPhams.Where(c => c.MaSanPham == PCOfLine.MaSanPham && !c.IsDelete).FirstOrDefault();
                                var productivity = nangsuatngays.FirstOrDefault(c => c.STTCHuyen_SanPham == item.STT);
                                var dayInfo = thanhphamngays.Where(c => c.STTChuyen_SanPham == item.STT).FirstOrDefault();
                                var listBTP = btpngays.Where(c => c.STTChuyen_SanPham == item.STT).ToList();
                                var listTyLeDen = db.P_ReadPercentOfLine.FirstOrDefault(x => !x.IsDeleted && !x.Chuyen_SanPham.IsDelete && x.AssignmentId == item.STT && x.P_LightPercent.Type == (int)eLightType.KanBan && x.LineId == item.MaChuyen);

                                model.LineName = item.Chuyen.TenChuyen;// line.TenChuyen;
                                                                       //  if (product != null)
                                model.ProductName = item.SanPham.TenSanPham;// product.TenSanPham;
                                int btpTrenChuyen = 0;
                                int dinhMucNgay = 0;
                                if (productivity != null)
                                {
                                    btpTrenChuyen = productivity.BTPTrenChuyen;
                                    dinhMucNgay = (int)productivity.DinhMucNgay;
                                }
                                int laoDongChuyen = 0;
                                if (dayInfo != null)
                                    laoDongChuyen = dayInfo.LaoDongChuyen;
                                int btpGiaoChuyenNgay = 0;
                                int luyKeBTP_HC = 0, luyKeBTP = 0;
                                if (listBTP != null && listBTP.Count > 0)
                                {
                                    int btpGiaoChuyenNgayTang = listBTP.Where(c => !c.IsBTP_PB_HC && c.Ngay == now && c.CommandTypeId == (int)eCommandRecive.BTPIncrease).Sum(c => c.BTPNgay);
                                    int btpGiaoChuyenNgayGiam = listBTP.Where(c => !c.IsBTP_PB_HC && c.Ngay == now && c.CommandTypeId == (int)eCommandRecive.BTPReduce).Sum(c => c.BTPNgay);
                                    btpGiaoChuyenNgay = btpGiaoChuyenNgayTang - btpGiaoChuyenNgayGiam;
                                    int luyKeBTPTang = listBTP.Where(c => c.IsBTP_PB_HC && c.CommandTypeId == (int)eCommandRecive.BTPIncrease).Sum(c => c.BTPNgay);
                                    int luyKeBTPGiam = listBTP.Where(c => c.IsBTP_PB_HC && c.CommandTypeId == (int)eCommandRecive.BTPReduce).Sum(c => c.BTPNgay);
                                    luyKeBTP_HC = luyKeBTPTang - luyKeBTPGiam;

                                    luyKeBTPTang = listBTP.Where(c => !c.IsBTP_PB_HC && c.CommandTypeId == (int)eCommandRecive.BTPIncrease).Sum(c => c.BTPNgay);
                                    luyKeBTPGiam = listBTP.Where(c => !c.IsBTP_PB_HC && c.CommandTypeId == (int)eCommandRecive.BTPReduce).Sum(c => c.BTPNgay);
                                    luyKeBTP = luyKeBTPTang - luyKeBTPGiam;
                                }
                                int btpBinhQuan = laoDongChuyen == 0 ? 0 : (int)(Math.Ceiling((double)btpTrenChuyen / laoDongChuyen));
                                int von = btpTrenChuyen > 0 && laoDongChuyen > 0 ? (int)(Math.Ceiling((double)btpTrenChuyen / laoDongChuyen)) : 0;

                                int tyLeDenThucTe = von;
                                model.Von = von;
                                model.BTPOnDay = btpGiaoChuyenNgay;
                                model.LK_BTP_HC = luyKeBTP_HC;
                                model.ProductionPlans = item.SanLuongKeHoach;// PCOfLine.SanLuongKeHoach;
                                model.BTPBQ = btpBinhQuan + "|" + btpTrenChuyen;
                                model.BTPBinhQuan = btpBinhQuan;
                                model.BTPInLine = btpTrenChuyen;
                                model.LK_BTP = luyKeBTP;

                                string colorDen = "Black";
                                if (listTyLeDen != null)
                                {
                                    var den = db.P_LightPercent_De.FirstOrDefault(c => !c.IsDeleted && tyLeDenThucTe >= c.From && tyLeDenThucTe <= c.To && c.LightPercentId == listTyLeDen.KanbanLightPercentId);
                                    if (den != null)
                                    {
                                        if (den.ColorName.Trim().ToUpper().Equals("ĐỎ"))
                                            colorDen = "Red";
                                        else if (den.ColorName.Trim().ToUpper().Equals("VÀNG"))
                                            colorDen = "Yellow";
                                        if (den.ColorName.Trim().ToUpper().Equals("XANH"))
                                            colorDen = "Blue";
                                    }
                                }
                                model.StatusColor = colorDen;

                                int minBTP_HC = -1;
                                if (includingBTPHC)
                                {
                                    #region  lay thong tin btphc
                                    model.BTPHC_Structs.AddRange(db.P_Phase.Where(x => !x.IsDeleted && x.Type == (int)ePhaseType.BTP_HC && x.IsShow).Select(x => new PhaseModel()
                                    {
                                        Id = x.Id,
                                        Index = x.Index,
                                        Name = x.Name,
                                        Note = "0"
                                    }).OrderBy(x => x.Index));
                                    if (model.BTPHC_Structs.Count > 0)
                                    {
                                        var btpHC_collec = db.P_Phase_Assign_Log.Where(x => x.AssignId == item.STT).ToList();
                                        if (btpHC_collec.Count > 0)
                                        {
                                            foreach (var iObj in model.BTPHC_Structs)
                                            {
                                                var sl = 0;
                                                var foundObj = btpHC_collec.FirstOrDefault(x => x.PhaseId == iObj.Id);
                                                if (foundObj != null)
                                                    sl = foundObj.Quantity;
                                                iObj.Note = sl.ToString();

                                                if (minBTP_HC == -1)
                                                    minBTP_HC = sl;
                                                else if (sl < minBTP_HC)
                                                    minBTP_HC = sl;
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                minBTP_HC = minBTP_HC < 0 ? 0 : minBTP_HC;
                                model.BTP_Ton = minBTP_HC - model.LK_BTP;
                                model.BTP_Ton = model.BTP_Ton < 0 ? 0 : model.BTP_Ton;

                                //den btp con lai
                                int nangSuatGioKH = 0;
                                var listModelWorkHours = BLLShift.GetListWorkHoursOfLineByLineId(item.MaChuyen);
                                if (listModelWorkHours != null && listModelWorkHours.Count > 0)
                                    nangSuatGioKH = (int)(dinhMucNgay / listModelWorkHours.Count);

                                colorDen = "";
                                int maTiLeBTPConLai = 0;
                                var chuyen = db.Chuyens.FirstOrDefault(x => x.MaChuyen == item.MaChuyen);
                                if (chuyen != null && chuyen.IdTiLeBTPConLai.HasValue)
                                    maTiLeBTPConLai = chuyen.IdTiLeBTPConLai.Value;
                                if (maTiLeBTPConLai > 0)
                                {
                                    int btpconlai = model.BTP_Ton;
                                    if (btpconlai < 0)
                                        btpconlai = 0;
                                    var den = db.P_LightPercent_De.FirstOrDefault(c => btpconlai >= (c.From * nangSuatGioKH) && btpconlai <= (c.To * nangSuatGioKH) && c.LightPercentId == maTiLeBTPConLai && c.P_LightPercent.Type == (int)eLightType.BTPConLai);
                                    if (den != null)
                                    {
                                        if (den.ColorName.Trim().ToUpper().Equals("ĐỎ"))
                                            colorDen = "Red";
                                        else if (den.ColorName.Trim().ToUpper().Equals("VÀNG"))
                                            colorDen = "Yellow";
                                        if (den.ColorName.Trim().ToUpper().Equals("XANH"))
                                            colorDen = "Blue";
                                    }
                                }
                                model.LightBTPConLai = colorDen;
                                listModel.Add(model);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listModel;
        }

        public List<HoanTat_LCD> GetHoanTatLCD(List<int> listLineId)
        {
            var listModel = new List<HoanTat_LCD>();
            var now = DateTime.Now.ToString("d/M/yyyy");
            try
            {
                using (var db = new PMSEntities())
                {
                    if (listLineId != null && listLineId.Count > 0)
                    {
                        listLineId = listLineId.Distinct().ToList();
                        listModel.AddRange(db.Chuyen_SanPham.Where(c => !c.IsDelete && !c.IsFinish && !c.SanPham.IsDelete && !c.Chuyen.IsDeleted && listLineId.Contains(c.MaChuyen)).Select(x => new HoanTat_LCD()
                        {
                            LineId = x.MaChuyen,
                            CSPId = x.STT,
                            Index = x.STTThucHien,
                            LineName = x.Chuyen.TenChuyen,
                            ProName = x.SanPham.TenSanPham,
                            SLKH = x.SanLuongKeHoach,
                            LK_TC = x.LuyKeBTPThoatChuyen,
                            LK_KCS = x.LuyKeTH,

                        }).OrderBy(x => x.LineId).ThenBy(x => x.Index).ToList());
                        if (listModel.Count > 0)
                        {
                            var logs = db.P_Phase_Assign_Log.Where(y => !y.P_Phase.IsDeleted && y.P_Phase.Type == (int)ePhaseType.HOANTAT && y.P_Phase.IsShow).ToList();
                            foreach (var item in listModel)
                            {
                                item.Phases.AddRange(db.P_Phase.Where(x => !x.IsDeleted && x.Type == (int)ePhaseType.HOANTAT && x.IsShow).Select(y => new HoanTatPhase()
                                {
                                    Index = y.Index,
                                    LK = 0,
                                    PhaseId = y.Id,
                                    PhaseName = y.Name
                                }).OrderBy(y => y.Index).ToList());
                                for (int i = 0; i < item.Phases.Count; i++)
                                {
                                    var found = logs.FirstOrDefault(x => x.PhaseId == item.Phases[i].PhaseId && x.AssignId == item.CSPId);
                                    if (found != null)
                                        item.Phases[i].LK = found.Quantity;
                                    else
                                        item.Phases[i].LK = 0;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listModel;
        }

        public List<TongHop_LCD> GetTongHopLCD(List<int> lineIds, int tableTypeId, int TimesGetNS, int KhoangCachGetNSOnDay, int phaseHT)
        {
            try
            {
                using (var db = new PMSEntities())
                {
                    var listObjs = new List<TongHop_LCD>();
                    int hienThiNSGio = 1, lightId = 0;
                    var now = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                    var datetime = DateTime.Now;

                    for (int iii = 0; iii < lineIds.Count; iii++)
                    {
                        int lineId = lineIds[iii];
                        var listPCC = db.Chuyen_SanPham.Where(c => !c.IsDelete && !c.IsFinish && !c.SanPham.IsDelete && !c.Chuyen.IsDeleted && c.MaChuyen == lineId).OrderBy(c => c.STTThucHien).ToList();
                        if (listPCC.Count > 0)
                        {
                            #region get Data
                            var listSTTLineProduct = listPCC.Select(c => c.STT).ToList();
                            var listProductivity = db.NangXuats.Where(c => listSTTLineProduct.Contains(c.STTCHuyen_SanPham) && !c.IsDeleted).Select(x => new ProductivitiesModel()
                            {
                                #region   
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
                                TGCheTaoSP = x.TGCheTaoSP,
                                CreatedDate = x.CreatedDate
                                #endregion
                            }).ToList();

                            var thanhphams = db.ThanhPhams.Where(c => listSTTLineProduct.Contains(c.STTChuyen_SanPham) && !c.IsDeleted).ToList();
                            var listDayInfo = db.ThanhPhams.Where(c => listSTTLineProduct.Contains(c.STTChuyen_SanPham) && !c.IsDeleted && c.Ngay == now).ToList();
                            #endregion

                            for (int z = 0; z < listSTTLineProduct.Count; z++)
                            {
                                //var historyObj = BLLHistoryPressedKeypad.Instance.Get(lineId, now);
                                //var pccSX = ((historyObj == null || historyObj.AssignmentId == null) ? listPCC.First() : listPCC.FirstOrDefault(x => x.STT == historyObj.AssignmentId));
                                //if (pccSX == null && listPCC.Count > 0)
                                //    pccSX = listPCC.First();

                                //update son ha 22/10/2018
                                var pccSX = listPCC.FirstOrDefault(x => x.STT == listSTTLineProduct[z]);

                                var productivity = listProductivity.FirstOrDefault(c => c.STTCHuyen_SanPham == pccSX.STT && c.Ngay == now);
                                var dayInfo = listDayInfo.FirstOrDefault(c => c.STTChuyen_SanPham == pccSX.STT);
                                var listBTPOfLine = db.BTPs.Where(c => !c.IsBTP_PB_HC && listSTTLineProduct.Contains(c.STTChuyen_SanPham) && !c.IsDeleted && c.IsEndOfLine && (c.CommandTypeId == (int)eCommandRecive.BTPIncrease || c.CommandTypeId == (int)eCommandRecive.BTPReduce)).ToList();
                                var monthDetails = db.P_MonthlyProductionPlans.Where(x => !x.IsDeleted && x.Month == datetime.Month && x.Year == datetime.Year && listSTTLineProduct.Contains(x.STT_C_SP)).ToList();
                                var nxInMonth = listProductivity.Where(x => x.STTCHuyen_SanPham == pccSX.STT && x.CreatedDate.Month == datetime.Month && x.CreatedDate.Year == datetime.Year).ToList();
                                var congdoanLog = db.P_Phase_Assign_Log.FirstOrDefault(x => x.AssignId == pccSX.STT && x.PhaseId == phaseHT);

                                #region MyRegion
                                if (productivity != null && dayInfo != null)
                                {
                                    double tyLeDen = 0;
                                    var model = new TongHop_LCD();
                                    model.LineName = productivity.LineName;
                                    model.ProductName = productivity.ProductName;
                                    model.LK_KCS = (int)pccSX.LuyKeTH;
                                    model.LK_TC = (int)pccSX.LuyKeBTPThoatChuyen;
                                    model.LK_BTP = pccSX.LK_BTP;
                                    model.LDDB = productivity.LaborsBase;
                                    model.LDTT = dayInfo.LaoDongChuyen;
                                    model.SLKH = pccSX.SanLuongKeHoach;
                                    model.SLCL = (pccSX.SanLuongKeHoach - model.LK_KCS);
                                    model.DMN = (int)Math.Round(productivity.DinhMucNgay);
                                    model.KCS = productivity.ThucHienNgay - productivity.ThucHienNgayGiam;
                                    model.TC = productivity.BTPThoatChuyenNgay - productivity.BTPThoatChuyenNgayGiam;
                                    model.tiLeThucHien = (model.DMN > 0 && model.KCS > 0) ? ((int)((model.KCS * 100) / model.DMN)) + "" : "0";
                                    model.NhipSX = Math.Round((double)productivity.NhipDoSanXuat, 1);
                                    model.NhipTT = Math.Round((double)productivity.NhipDoThucTe, 1);
                                    model.NhipTC = Math.Round((double)productivity.NhipDoThucTeBTPThoatChuyen, 1);
                                    model.Error = productivity.SanLuongLoi - productivity.SanLuongLoiGiam;
                                    #region
                                    int btpTrenChuyenBinhQuan = dayInfo.LaoDongChuyen == 0 || productivity.BTPTrenChuyen == 0 ? 0 : (int)Math.Ceiling((double)productivity.BTPTrenChuyen / dayInfo.LaoDongChuyen);
                                    model.BTPInLine = productivity.BTPTrenChuyen;
                                    model.BTPInLine_BQ = btpTrenChuyenBinhQuan;

                                    model.SLKH = pccSX.SanLuongKeHoach;
                                    model.LK_TC = pccSX.LuyKeBTPThoatChuyen;
                                    lightId = productivity.IdDenNangSuat ?? 0;
                                    model.BTP = 0;
                                    model.LK_BTP = 0;
                                    model.KCSHour = 0;
                                    model.TCHour = 0;
                                    model.ErrHour = 0;
                                    model.DMHour = 0;
                                    model.TiLeLoi_D = 0;
                                    if (listBTPOfLine != null && listBTPOfLine.Count > 0)
                                    {
                                        int btpGiaoChuyenNgayTang = listBTPOfLine.Where(c => !c.IsBTP_PB_HC && c.Ngay == now && c.CommandTypeId == (int)eCommandRecive.BTPIncrease).Sum(c => c.BTPNgay);
                                        int btpGiaoChuyenNgayGiam = listBTPOfLine.Where(c => !c.IsBTP_PB_HC && c.Ngay == now && c.CommandTypeId == (int)eCommandRecive.BTPReduce).Sum(c => c.BTPNgay);
                                        model.BTP = btpGiaoChuyenNgayTang - btpGiaoChuyenNgayGiam;

                                        btpGiaoChuyenNgayTang = listBTPOfLine.Where(c => !c.IsBTP_PB_HC && c.CommandTypeId == (int)eCommandRecive.BTPIncrease).Sum(c => c.BTPNgay);
                                        btpGiaoChuyenNgayGiam = listBTPOfLine.Where(c => !c.IsBTP_PB_HC && c.CommandTypeId == (int)eCommandRecive.BTPReduce).Sum(c => c.BTPNgay);
                                        model.LK_BTP = btpGiaoChuyenNgayTang - btpGiaoChuyenNgayGiam;
                                    }
                                    #endregion
                                    model.LK_HOANTHANH = 0;
                                    if (congdoanLog != null)
                                        model.LK_HOANTHANH = congdoanLog.Quantity;

                                    #region Get Hours Productivity
                                    var totalWorkingTimeInDay = BLLShift.GetTotalWorkingHourOfLine(lineId);
                                    int intWorkTime = (int)(totalWorkingTimeInDay.TotalHours);
                                    int intWorkMinuter = (int)totalWorkingTimeInDay.TotalMinutes;
                                    double NangSuatPhutKH = 0;
                                    int NangSuatGioKH = 0;
                                    var dateNow = DateTime.Now.Date;
                                    int tongTCNgay = 0, tongKCSNgay = 0;
                                    model.DMHour = (int)Math.Ceiling((double)model.DMN / intWorkTime);
                                    double phutToNow = GetSoPhutLamViecTrongNgay_(DateTime.Now.TimeOfDay, BLLShift.GetShiftsOfLine(lineId));
                                    if (intWorkTime > 0)
                                    {
                                        NangSuatPhutKH = (double)model.DMN / intWorkMinuter;
                                        NangSuatGioKH = (int)(model.DMN / intWorkTime);
                                        if (model.DMN % intWorkTime != 0)
                                            NangSuatGioKH++;


                                        #region  hiển thị một ô năng suất hiện tại duy nhất

                                        double nsKHToNow = (phutToNow / intWorkMinuter) * model.DMN;
                                        double tiLePhanTram = 0;
                                        tiLePhanTram = (model.KCS > 0 && nsKHToNow > 0) ? (Math.Round((double)((model.KCS * 100) / nsKHToNow), 2)) : 0;
                                        // model.CurrentNS = model.KCS + "/" + (int)nsKHToNow + "  (" + tiLePhanTram + "%)";
                                        model.SLKHToNow = (int)nsKHToNow;
                                        #endregion


                                        #region
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
                                            var dayInformations = db.TheoDoiNgays.Where(c => c.MaChuyen == lineId && c.STTChuyenSanPham == pccSX.STT && c.Date == now && c.IsEndOfLine).Select(x => new DayInfoModel()
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

                                            var t = dayInformations.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductIncrease && c.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(c => c.ThanhPham);
                                            var g = dayInformations.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductReduce && c.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(c => c.ThanhPham);
                                            tongTCNgay += (t - g);
                                            bool isValid = false;
                                            for (int i = 0; i < listWorkHoursOfLine.Count; i++)
                                            {
                                                if (DateTime.Now.TimeOfDay > listWorkHoursOfLine[i].TimeStart && DateTime.Now.TimeOfDay <= listWorkHoursOfLine[i].TimeEnd)
                                                    isValid = true;

                                                //DM Gio
                                                listWorkHoursOfLine[i].NormsHour = Math.Round(NangSuatPhutKH * (int)((listWorkHoursOfLine[0].TimeEnd - listWorkHoursOfLine[0].TimeStart).TotalMinutes));
                                                if ((hienThiNSGio == (int)eShowNSType.TH_DM_FollowHour || hienThiNSGio == (int)eShowNSType.PercentTH_FollowHour) && i == listWorkHoursOfLine.Count - 1)
                                                    listWorkHoursOfLine[i].NormsHour = Math.Round(NangSuatPhutKH * (int)((listWorkHoursOfLine[i].TimeEnd - listWorkHoursOfLine[i].TimeStart).TotalMinutes));

                                                #region
                                                int Tang = 0, Giam = 0;
                                                var theoDoiNgays = dayInformations.Where(c => c.MaChuyen == lineId && c.Time > listWorkHoursOfLine[i].TimeStart && c.Time <= listWorkHoursOfLine[i].TimeEnd && c.Date == now && c.IsEndOfLine).ToList();
                                                if (theoDoiNgays.Count > 0)
                                                {
                                                    //Kcs
                                                    Tang = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductIncrease && c.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(c => c.ThanhPham);
                                                    Giam = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductReduce && c.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(c => c.ThanhPham);
                                                    Tang -= Giam;

                                                    listWorkHoursOfLine[i].KCS = Tang;
                                                    listWorkHoursOfLine[i].HoursProductivity = (listWorkHoursOfLine[i].KCS < 0 ? 0 : listWorkHoursOfLine[i].KCS) + "/" + listWorkHoursOfLine[i].NormsHour;
                                                    tongKCSNgay += Tang;
                                                    if (isValid)
                                                        model.KCSHour = Tang;

                                                    // TC
                                                    Tang = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductIncrease && c.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(c => c.ThanhPham);
                                                    Giam = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductReduce && c.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(c => c.ThanhPham);
                                                    Tang -= Giam;

                                                    listWorkHoursOfLine[i].TC = Tang;
                                                    listWorkHoursOfLine[i].HoursProductivity_1 = (listWorkHoursOfLine[i].KCS < 0 ? 0 : listWorkHoursOfLine[i].KCS) + "/" + (listWorkHoursOfLine[i].TC < 0 ? 0 : listWorkHoursOfLine[i].TC);
                                                    //   tongTCNgay += Tang;
                                                    if (isValid)
                                                        model.TCHour = Tang;

                                                    // lỗi
                                                    Tang = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ErrorIncrease).Sum(c => c.ThanhPham);
                                                    Giam = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ErrorReduce).Sum(c => c.ThanhPham);
                                                    Tang -= Giam;
                                                    listWorkHoursOfLine[i].Error = Tang;
                                                    //  model.Error += Tang;
                                                    if (isValid)
                                                        model.ErrHour = Tang;

                                                }
                                                else
                                                {
                                                    listWorkHoursOfLine[i].HoursProductivity = "0/" + listWorkHoursOfLine[i].NormsHour;
                                                    listWorkHoursOfLine[i].HoursProductivity_1 = "0/0";
                                                }
                                                #endregion
                                            }
                                        }
                                        #endregion

                                        //  model.ErrorNgay += finishError;
                                        model.listWorkHours = listWorkHoursOfLine;
                                        model.KieuHienThiNangSuatGio = db.Configs.Where(x => x.Name.Trim().ToUpper().Equals("KieuHienThiNangSuatGio")).FirstOrDefault().ValueDefault.Trim();

                                        //  model.KCS = tongKCSNgay;
                                        model.DMN = (int)productivity.DinhMucNgay;
                                        //  model.TC = tongTCNgay;

                                        double minutes = 0, pro = 0, pro_lech = 0, time_lech = 0, LK_err = 0;
                                        minutes = GetSoPhutLamViecTrongNgay_(DateTime.Now.TimeOfDay, BLLShift.GetShiftsOfLine(lineId));
                                        pro = (minutes / intWorkMinuter) * model.DMN;
                                        pro_lech = pro - productivity.BTPThoatChuyenNgay;
                                        if (pro_lech > 0)
                                            time_lech = Math.Round(((pro_lech / model.LDTT) * productivity.TGCheTaoSP) / 3600);
                                        model.Hour_ChenhLech_Day = time_lech > 0 ? (int)time_lech : 0;

                                        var proOfCommo = listProductivity.Where(x => x.STTCHuyen_SanPham == productivity.STTCHuyen_SanPham && x.Ngay != now);
                                        foreach (var item in proOfCommo)
                                        {
                                            pro_lech = item.DinhMucNgay - (item.BTPThoatChuyenNgay - item.BTPThoatChuyenNgayGiam);
                                            if (pro_lech > 0)
                                                time_lech += ((pro_lech / item.LaborsBase) * item.TGCheTaoSP) / 3600;
                                            LK_err += item.SanLuongLoi - item.SanLuongLoiGiam;
                                        }

                                        model.Hour_ChenhLech = time_lech > 0 ? (int)time_lech : 0;

                                        model.KCS_QuaTay = model.KCS + model.Error;
                                        model.LK_KCS_QuaTay = model.LK_KCS + (int)LK_err;
                                        model.TiLeLoi_D = (model.Error != 0 ? (int)Math.Ceiling((model.Error / (double)(model.Error + model.KCS)) * 100) : 0);

                                    }
                                    #endregion

                                    //if (productivity.NhipDoThucTe > 0)
                                    //    tyLeDen = (model.NhipSX * 100) / productivity.NhipDoThucTe;

                                    // sua lai kcs / dmtoNow * 100
                                    //  if (model.SLKHToNow > 0)
                                    //      tyLeDen = Math.Round((model.KCS / model.SLKHToNow) * 100, 1);

                                    //update theo yc cua sonha 17/10/2018
                                    // tyLeDen = kcs gio/muctieugio*100
                                    if (model.SLKHToNow > 0)
                                        tyLeDen = (model.KCSHour / model.DMHour) * 100;

                                    var lightConfig = db.Dens.Where(c => c.IdCatalogTable == tableTypeId && c.STTParent == lightId && c.ValueFrom <= tyLeDen && tyLeDen < c.ValueTo).FirstOrDefault();
                                    model.mauDen = lightConfig != null ? lightConfig.Color.Trim().ToUpper() : "ĐỎ";

                                    if (model.BTPInLine < 0)
                                        model.BTPInLine = 0;
                                    model.Lean = Math.Ceiling((double)(model.BTPInLine > 0 ? (model.BTPInLine / model.LDTT) : 0));
                                    model.NSHienTai = (model.KCS + "/" + model.SLKHToNow + " (" + (model.SLKHToNow > 0 ? Math.Round((model.KCS / model.SLKHToNow), 1) : 0) + "%)");

                                    //  Hiệu suất = (Tổng sản lượng ra chuyền X thời gian chế tạo) : Số lao động X thời gian làm việc thực tế(giay). 
                                    model.HieuSuat = (int)(((model.KCS * productivity.TGCheTaoSP) / Math.Round((model.LDTT * (phutToNow * 60)))) * 100);
                                    //  model.HieuSuat = (model.SLKHToNow > 0 ? (int)Math.Round((model.KCS / model.SLKHToNow), 1) : 0);

                                    lightConfig = db.Dens.FirstOrDefault(c => c.IdCatalogTable == tableTypeId && c.STTParent == lightId && c.ValueFrom <= model.HieuSuat && model.HieuSuat < c.ValueTo);
                                    model.mauDenHieuSuat = lightConfig != null ? lightConfig.Color.Trim().ToUpper() : "ĐỎ";

                                    listObjs.Add(model);
                                }
                                #endregion
                            }
                        }
                    }
                    return listObjs;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private double GetSoPhutLamViecTrongNgay_(TimeSpan timeNow, List<LineWorkingShiftModel> workShift)
        {
            double soPhut = 0;
            try
            {
                if (workShift != null && workShift.Count > 0)
                {
                    foreach (var sh in workShift)
                    {
                        if (timeNow >= sh.Start)
                        {
                            if (timeNow < sh.End)
                            {
                                var h = timeNow.Hours - sh.Start.Hours;
                                soPhut += (h * 60 + timeNow.Minutes) - sh.Start.Minutes;
                            }
                            else if (timeNow >= sh.End)
                                soPhut += ((sh.End - sh.Start).TotalMinutes);
                        }
                    }
                }
            }
            catch (Exception)
            { }
            return soPhut;
        }

     }
}
