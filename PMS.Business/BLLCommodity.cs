using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PMS.Business
{
    public class BLLCommodity
    {
        public static List<SanPhamModel> GetAll(int floorId, int All)
        {
            using (var db = new PMSEntities())
            {
                try
                {
                    if (All == 1)
                        return db.SanPhams.Where(x => !x.IsDelete).OrderBy(x => x.MaSanPham).Select(x => new SanPhamModel()
                        {
                            MaSanPham = x.MaSanPham,
                            TenSanPham = x.TenSanPham,
                            DinhNghia = x.DinhNghia,
                            DonGia = x.DonGia,
                            DonGiaCat = x.DonGiaCat,
                            DonGiaCM = x.DonGiaCM,
                            MaKhachHang = x.MaKhachHang,
                            ProductionTime = x.ProductionTime
                        }).ToList();
                    return db.SanPhams.Where(x => !x.IsDelete && x.Floor.Value == floorId).OrderBy(x => x.MaSanPham).Select(x => new SanPhamModel()
                    {
                        MaSanPham = x.MaSanPham,
                        TenSanPham = x.TenSanPham,
                        DinhNghia = x.DinhNghia,
                        DonGia = x.DonGia,
                        DonGiaCat = x.DonGiaCat,
                        DonGiaCM = x.DonGiaCM,
                        MaKhachHang = x.MaKhachHang,
                        ProductionTime = x.ProductionTime
                    }).ToList();
                }
                catch (Exception ex)
                {
                }
            }
            return new List<SanPhamModel>();
        }

        public static List<ProductModel> Gets(int floorId, int All)
        {
            try
            {
                using (var db = new PMSEntities())
                {
                    var query = db.SanPhams.Where(x => !x.IsDelete);
                    if (All != 1)
                        query = query.Where(x => x.Floor.Value == floorId);

                    return query.OrderBy(x => x.MaSanPham).Select(x => new ProductModel()
                    {
                        MaSanPham = x.MaSanPham,
                        TenSanPham = x.TenSanPham,
                        DinhNghia = x.DinhNghia,
                        DonGia = x.DonGia,
                        DonGiaCM = x.DonGiaCM,
                        Floor = x.Floor ?? 0,
                        ProductionTime = x.ProductionTime
                    }).ToList();
                }

            }
            catch (Exception)
            {
            }
            return null;
        }

        public static ResponseBase Delete(int commoId)
        {
            var result = new ResponseBase();
            try
            {
                var db = new PMSEntities();
                var commo = db.SanPhams.FirstOrDefault(x => x.MaSanPham == commoId);
                if (commo != null)
                {
                    commo.IsDelete = true;
                    var assigns = db.Chuyen_SanPham.Where(x => !x.IsDelete && x.MaSanPham == commoId);
                    if (assigns != null && assigns.Count() > 0)
                    {
                        foreach (var item in assigns)
                        {
                            item.IsDelete = true;
                        }
                    }
                    db.SaveChanges();
                    result.IsSuccess = true;
                    result.Messages.Add(new Message() { msg = "Xóa mã hàng thành công.", Title = "Thông Báo" });
                }
                else
                {
                    result.IsSuccess = true;
                    result.Messages.Add(new Message() { msg = "Không tìm thấy thông tin Mã Hàng . Xóa mã hàng thất bại.", Title = "Lỗi CSDL" });
                }
            }
            catch (Exception)
            {
                result.IsSuccess = true;
                result.Messages.Add(new Message() { msg = "Không tìm thấy thông tin Mã Hàng . Xóa mã hàng thất bại.", Title = "Lỗi Exception" });
            }
            return result;
        }

        /// <summary>
        /// chuyen phan hang sang trang thai xoa cho nhung mat hang da bi xoa
        /// </summary>
        /// <param name="commoId"></param>
        /// <returns></returns>
        public static ResponseBase ChangeAssignmentStatusforAllDelatedCommodities()
        {
            var result = new ResponseBase();
            try
            {
                var db = new PMSEntities();
                var assigns = db.Chuyen_SanPham.Where(x => !x.IsDelete && (x.Chuyen.IsDeleted || x.SanPham.IsDelete));
                if (assigns != null && assigns.Count() > 0)
                {
                    foreach (var item in assigns)
                    {
                        item.IsDelete = true;
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception)
            { }
            return result;
        }

        public static ResponseBase InsertOrUpdate(SanPham objModel)
        {
            var rs = new ResponseBase();
            var db = new PMSEntities();
            try
            {
                if (CheckName(objModel.MaSanPham, objModel.TenSanPham.Trim()) != null)
                {
                    rs.IsSuccess = false;
                    rs.Messages.Add(new Message() { msg = "Tên mã hàng đã tồn tại. Vui lòng chọn lại tên khác", Title = "Lỗi trùng tên" });
                }
                else
                {
                    if (objModel.MaSanPham == 0)
                    {
                        db.SanPhams.Add(objModel);
                        rs.IsSuccess = true;
                    }
                    else
                    {
                        var oldObj = db.SanPhams.FirstOrDefault(x => !x.IsDelete && x.MaSanPham == objModel.MaSanPham);
                        if (oldObj != null)
                        {
                            oldObj.TenSanPham = objModel.TenSanPham;
                            oldObj.DonGia = objModel.DonGia;
                            oldObj.DonGiaCM = objModel.DonGiaCM;
                            oldObj.ProductionTime = objModel.ProductionTime;
                            oldObj.Floor = objModel.Floor;
                            oldObj.DinhNghia = objModel.DinhNghia;
                            oldObj.MaKhachHang = objModel.MaKhachHang;
                            oldObj.DonGiaCat = objModel.DonGiaCat;
                            rs.IsSuccess = true;
                        }
                        else
                        {
                            rs.IsSuccess = false;
                            rs.Messages.Add(new Message() { msg = "Mã hàng đang thao tác không tồn tại hoặc đã bị xóa. Vui lòng chọn lại tên khác", Title = "Lỗi trùng tên" });
                        }
                    }
                    if (rs.IsSuccess)
                    {
                        db.SaveChanges();
                        rs.IsSuccess = true;
                    }
                }
            }
            catch (Exception)
            {
            }
            return rs;
        }

        private static SanPham CheckName(int Id, string name)
        {
            var db = new PMSEntities();
            return db.SanPhams.FirstOrDefault(x => !x.IsDelete && x.MaSanPham != Id && x.TenSanPham.Trim().Equals(name));
        }
    }
}
