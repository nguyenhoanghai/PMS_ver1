using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business
{
    public class BLLLine
    {
        public static List<LineModel> GetLines(int floorId)
        {
            try
            {
                var db = new PMSEntities();
                var list = new List<LineModel>();
                return db.Chuyens.Where(x => !x.IsDeleted && !x.Floor.IsDeleted && x.FloorId == floorId).Select(x => new LineModel()
               {
                   MaChuyen = x.MaChuyen,
                   TenChuyen = x.TenChuyen,
                   Code = x.Code,
                   DinhNghia = x.DinhNghia,
                   LaoDongDinhBien = x.LaoDongDinhBien,
                   IdDen = x.IdDen,
                   IdDenNangSuat = x.IdDenNangSuat,
                   IdTyLeDoc = x.IdTyLeDoc,
                   IntSTT = x.IntSTT,
                   Floor = x.Floor,
                   Sound = x.Sound,
                   STTReadNS = x.STTReadNS,
               }).ToList();
            }
            catch (Exception ex)
            {
            }
            return new List<LineModel>();
        }

        public static List<LineModel> GetLines(List<int> lineIds)
        {
            try
            {
                var db = new PMSEntities();
                var list = new List<LineModel>();
                list.AddRange(db.Chuyens.Where(x => !x.IsDeleted && lineIds.Contains(x.MaChuyen)).Select(x => new LineModel()
                {
                    MaChuyen = x.MaChuyen,
                    TenChuyen = x.TenChuyen,
                    Code = x.Code,
                    DinhNghia = x.DinhNghia,
                    LaoDongDinhBien = x.LaoDongDinhBien,
                    IdDen = x.IdDen,
                    IdDenNangSuat = x.IdDenNangSuat,
                    IdTyLeDoc = x.IdTyLeDoc,
                    IntSTT = x.IntSTT,
                    Floor = x.Floor,
                    Sound = x.Sound,
                    STTReadNS = x.STTReadNS,
                }).ToList());
                if (list.Count > 0)
                {
                    var idChuyens = list.Select(c => c.MaChuyen).Distinct();
                    var shifts = db.P_LineWorkingShift.Where(x => !x.IsDeleted && !x.Chuyen.IsDeleted && !x.P_WorkingShift.IsDeleted && idChuyens.Contains(x.LineId)).Select(x => new LineWorkingShiftModel()
                    {
                        LineId = x.LineId,
                        Start = x.P_WorkingShift.TimeStart,
                        End = x.P_WorkingShift.TimeEnd,
                        Id = x.Id,
                        ShiftId = x.ShiftId,
                        ShiftOrder = x.ShiftOrder
                    }).ToList();
                    var clusters = db.Cums.Where(x => !x.IsDeleted && x.IsEndOfLine != null && x.IsEndOfLine && idChuyens.Contains(x.IdChuyen));
                    foreach (var item in list)
                    {
                        var obj = clusters.FirstOrDefault(x => x.IdChuyen == item.MaChuyen);
                        item.LastClusterId = obj != null ? obj.Id : 0;
                        item.Shifts.AddRange(shifts.Where(x => x.LineId == item.MaChuyen).OrderBy(x => x.ShiftOrder));
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<LineModel> GetLines_s(List<int> lineIds)
        {
            try
            {
                var db = new PMSEntities();
                var list = new List<LineModel>();
                list.AddRange(db.Chuyens.Where(x => !x.IsDeleted && lineIds.Contains(x.MaChuyen)).Select(x => new LineModel()
                {
                    MaChuyen = x.MaChuyen,
                    TenChuyen = x.TenChuyen,
                    Code = x.Code,
                    DinhNghia = x.DinhNghia,
                    LaoDongDinhBien = x.LaoDongDinhBien,
                    IdDen = x.IdDen,
                    IdDenNangSuat = x.IdDenNangSuat,
                    IdTyLeDoc = x.IdTyLeDoc,
                    IntSTT = x.IntSTT,
                    Floor = x.Floor,
                    Sound = x.Sound,
                    STTReadNS = x.STTReadNS,
                }).ToList());
                if (list.Count > 0)
                {
                    var idChuyens = list.Select(c => c.MaChuyen).Distinct();
                    var clusters = db.Cums.Where(x => !x.IsDeleted && x.IsEndOfLine != null && x.IsEndOfLine && idChuyens.Contains(x.IdChuyen));

                    var readIds = list.Where(x => x.IdTyLeDoc != null).Select(c => c.IdTyLeDoc).Distinct().ToList() ;
                    var readPercentObjs = db.ReadPercents.Where(x => readIds.Contains(x.Id)).ToList();
                    foreach (var item in list)
                    {
                        var obj = clusters.FirstOrDefault(x => x.IdChuyen == item.MaChuyen);
                        item.LastClusterId = obj != null ? obj.Id : 0;

                        if (item.IdTyLeDoc != null)
                        {
                            var obj_ = readPercentObjs.FirstOrDefault(x => x.Id == item.IdTyLeDoc);
                            item.ReadPercentName = obj_ != null ? obj_.Name : string.Empty;
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
        
        public static List<LineModel> GetLines(List<int> lineIds, int floorId, bool isOwner)
        {
            try
            {
                var db = new PMSEntities();
                var list = new List<LineModel>();
                if (!isOwner)
                {
                    list.AddRange(db.Chuyens.Where(x => !x.IsDeleted && lineIds.Contains(x.MaChuyen) && x.FloorId == floorId).Select(x => new LineModel()
                    {
                        MaChuyen = x.MaChuyen,
                        TenChuyen = x.TenChuyen,
                        Code = x.Code,
                        DinhNghia = x.DinhNghia,
                        LaoDongDinhBien = x.LaoDongDinhBien,
                        IdDen = x.IdDen,
                        IdDenNangSuat = x.IdDenNangSuat,
                        IdTyLeDoc = x.IdTyLeDoc,
                        IntSTT = x.IntSTT,
                        Floor = x.Floor,
                        FloorName = x.Floor.Name,
                        Sound = x.Sound,
                        STTReadNS = x.STTReadNS,
                    }).ToList());
                }
                else
                {
                    list.AddRange(db.Chuyens.Where(x => !x.IsDeleted && x.FloorId == floorId).Select(x => new LineModel()
                    {
                        MaChuyen = x.MaChuyen,
                        TenChuyen = x.TenChuyen,
                        Code = x.Code,
                        DinhNghia = x.DinhNghia,
                        LaoDongDinhBien = x.LaoDongDinhBien,
                        IdDen = x.IdDen,
                        IdDenNangSuat = x.IdDenNangSuat,
                        IdTyLeDoc = x.IdTyLeDoc,
                        IntSTT = x.IntSTT,
                        Floor = x.Floor,
                        FloorName = x.Floor.Name,
                        Sound = x.Sound,
                        STTReadNS = x.STTReadNS,
                    }).ToList());
                }
                return list;
            }
            catch (Exception)
            { }
            return null;
        }

        public static List<LineModel> GetLinesForMainForm(List<int> lineIds, int TableId)
        {
            var lines = new List<LineModel>();
            try
            {
                var db = new PMSEntities();
                lines.AddRange(db.Chuyens.Where(x => !x.IsDeleted && lineIds.Contains(x.MaChuyen)).Select(x => new LineModel()
                {
                    MaChuyen = x.MaChuyen,
                    TenChuyen = x.TenChuyen,
                    Code = x.Code,
                    Sound = x.Sound,
                    STTReadNS = x.STTReadNS,
                    IdTyLeDoc = x.IdTyLeDoc,
                    ReadPercentName = x.ReadPercent.Name,
                    LaoDongDinhBien = x.LaoDongDinhBien,
                    IdDen = x.IdDen,
                    IdDenNangSuat = x.IdDenNangSuat,
                    DinhNghia = x.DinhNghia,
                    IntSTT = x.IntSTT,
                    IsEndDate = x.IsEndDate,
                    FloorId = x.FloorId,
                }).ToList());

                if (lines.Count > 0)
                {
                    // get shift of line
                    var shifts = db.P_LineWorkingShift.Where(x => !x.IsDeleted && !x.Chuyen.IsDeleted && !x.P_WorkingShift.IsDeleted && lineIds.Contains(x.LineId)).Select(x => new LineWorkingShiftModel()
                    {
                        LineId = x.LineId,
                        Start = x.P_WorkingShift.TimeStart,
                        End = x.P_WorkingShift.TimeEnd,
                        Id = x.Id,
                        ShiftId = x.ShiftId,
                        ShiftOrder = x.ShiftOrder
                    }).ToList();

                    // lấy tt vi tri
                    var showInformations = db.TTHienThis.Where(x => lineIds.Contains(x.MaChuyen) && x.IdCatalogTable == TableId).Select(x => new Position_Model()
                    {
                        MaHienThi = x.MaHienThi,
                        MaChuyen = x.MaChuyen,
                        IsInt = x.IsInt,
                        ChangeBTP = x.ChangeBTP,
                        ChangeTP = x.ChangeTP,
                        STTDen = x.STTDen,
                        ViTri = x.ViTri,
                        IdCatalogTable = x.IdCatalogTable
                    }).ToList();

                    // lay thoi gian tinh nhip do tt
                    var ngay = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                    var nhipdoTTs = db.ThoiGianTinhNhipDoTTs.Where(x => lineIds.Contains(x.MaChuyen) && x.Ngay == ngay).ToList();

                    if (showInformations.Count > 0)
                    {
                        var infoIds = showInformations.Select(x => x.MaHienThi).Distinct();
                        // lay chi tiet vi tri
                        var details = db.ChiTietHienThis.Where(x => !infoIds.Contains(x.MaHienThi)).Select(x => new Cell_Model()
                        {
                            IDMain = x.IDMain,
                            IDCell = x.IDCell,
                            IDMat = x.IDMat,
                            SoLed = x.SoLed,
                            STT = x.STT,
                            MaHienThi = x.MaHienThi
                        }).ToList();

                        if (details.Count > 0)
                        {
                            // gan chi tiet vi tri vao vi tri
                            foreach (var item in showInformations)
                            {
                                item.Cells.AddRange(details.Where(x => x.MaHienThi == item.MaHienThi));
                            }
                        }

                        // gan tt vi tri vao chuyen
                        foreach (var item in lines)
                        {
                            item.Positions.AddRange(showInformations.Where(x => x.MaChuyen == item.MaChuyen));
                            item.Shifts.AddRange(shifts.Where(x => x.LineId == item.MaChuyen));
                            var obj = nhipdoTTs.FirstOrDefault(x => x.MaChuyen == item.MaChuyen);
                            item.TimeCalculateTT = obj != null ? TimeSpan.Parse(obj.ThoiGianBatDau.ToString()) : TimeSpan.Parse("00:00:00");
                        }
                    }
                }
            }
            catch (Exception) { }
            return lines;
        }

        public static ResponseBase InsertOrUpdate(Chuyen obj)
        {
            var result = new ResponseBase();
            try
            {
                var db = new PMSEntities();

                Chuyen line = null;
                line = db.Chuyens.FirstOrDefault(x => !x.IsDeleted && !x.Floor.IsDeleted && x.MaChuyen != obj.MaChuyen && x.Code.Trim().ToUpper().Equals(obj.Code.Trim().ToUpper()));
                if (line != null)
                {
                    result.IsSuccess = false;
                    result.Messages.Add(new Message() { Title = "Lỗi trùng mã", msg = "Mã chuyền này đã tồn tại vui lòng chọn mã chuyền khác." });
                }
                else
                {
                    var check = false;
                    if (obj.MaChuyen == 0)
                    {
                        db.Chuyens.Add(obj);
                        check = true;
                    }
                    else
                    {
                        line = db.Chuyens.FirstOrDefault(x => !x.IsDeleted && !x.Floor.IsDeleted && x.MaChuyen == obj.MaChuyen);
                        if (line != null)
                        {
                            line.Code = obj.Code;
                            line.TenChuyen = obj.TenChuyen;
                            line.DinhNghia = obj.DinhNghia;
                            line.Sound = obj.Sound;
                            line.LaoDongDinhBien = obj.LaoDongDinhBien;
                            line.FloorId = obj.FloorId;
                            check = true;
                        }
                    }
                    if (check)
                    {
                        db.SaveChanges();
                        result.IsSuccess = true;
                        result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Lưu thành công." });
                    }
                    else
                    {
                        result.IsSuccess = true;
                        result.Messages.Add(new Message() { Title = "Lỗi", msg = "không tìm thấy thông tin chuyền." });
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = true;
                result.Messages.Add(new Message() { Title = "Lỗi", msg = "Lỗi Lưu thông tin không thành công.\n" + ex.Message });
            }
            return result;
        }

        public static ResponseBase Delete(int lineId)
        {
            var result = new ResponseBase();
            try
            {
                var db = new PMSEntities();
                var line = db.Chuyens.FirstOrDefault(x => !x.IsDeleted && !x.Floor.IsDeleted && x.MaChuyen == lineId);
                if (line != null)
                {
                    line.IsDeleted = true;
                    db.SaveChanges();
                    result.IsSuccess = true;
                    result.Messages.Add(new Message() { Title = "Thông Báo", msg = "Xóa thành công." });
                }
                else
                {
                    result.IsSuccess = true;
                    result.Messages.Add(new Message() { Title = "Lỗi", msg = "không tìm thấy thông tin chuyền." });
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = true;
                result.Messages.Add(new Message() { Title = "Lỗi", msg = "Xóa chuyền không thành công.\n" + ex.Message });
            }
            return result;
        }
    }
}
