using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.POJO;
using DuAn03_HaiDang.Model;
using QuanLyNangSuat.Model;
using QuanLyNangSuat.POJO;
using DuAn03_HaiDang.KeyPad_Chuyen.pojo;
using DuAn03_HaiDang.KeyPad_Chuyen.dao;
using DuAn03_HaiDang.Enum;
using PMS.Business.Enum;
namespace DuAn03_HaiDang.DAO
{
    class NangXuatDAO
    {
        private DataTable dt;
        private Chuyen_SanPhamDAO chuyen_sanphamDAO;
        private ChuyenDAO chuyenDAO;
       // private ShiftDAO shiftDAO;

        public NangXuatDAO()
        {
            this.dt = new DataTable();
            chuyen_sanphamDAO = new Chuyen_SanPhamDAO();
            chuyenDAO = new ChuyenDAO();
          //  shiftDAO = new ShiftDAO();
        }

        public NangXuat TTNangXuatTrongNgay(string ngay, string sttChuyenSanPham)
        {
            NangXuat nangSuat = null;
            try
            {
                dt.Clear();
                if (!string.IsNullOrEmpty(ngay) && !string.IsNullOrEmpty(sttChuyenSanPham))
                {
                    string sql = "select * from NangXuat where Ngay = '" + ngay + "' and STTCHuyen_SanPham=" + sttChuyenSanPham + " and IsDeleted=0";
                    dt = dbclass.TruyVan_TraVe_DataTable(sql);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow rowFirst = dt.Rows[0];
                        nangSuat = new NangXuat();
                        int btpTrenChuyen = 0;
                        int.TryParse(rowFirst["BTPTrenChuyen"].ToString(), out btpTrenChuyen);
                        nangSuat.BTPTrenChuyen = btpTrenChuyen;
                        float dinhMucNgay = 0;
                        float.TryParse(rowFirst["DinhMucNgay"].ToString(), out dinhMucNgay);
                        nangSuat.DinhMucNgay = dinhMucNgay;
                        nangSuat.STTChuyen_SanPham = sttChuyenSanPham;
                        nangSuat.Ngay =  (ngay);
                        int nhipDoSanXuat = 0;
                        int.TryParse(rowFirst["NhipDoSanXuat"].ToString(), out nhipDoSanXuat);
                        nangSuat.NhipDoSanXuat = nhipDoSanXuat;
                        int thuHienNgay = 0;
                        int thuHienNgayGiam = 0;
                        int.TryParse(rowFirst["ThucHienNgay"].ToString(), out thuHienNgay);
                        int.TryParse(rowFirst["ThucHienNgayGiam"].ToString(), out thuHienNgayGiam);
                        bool isEndDate = false;
                        bool.TryParse(rowFirst["IsEndDate"].ToString(), out isEndDate);
                        nangSuat.IsEndDate = isEndDate;
                        nangSuat.ThucHienNgay = thuHienNgay;
                        nangSuat.ThucHienNgayGiam = thuHienNgayGiam;

                        int tpThoatChuyenNgay = 0;
                        int tpThoatChuyenNgayGiam = 0;
                        int.TryParse(rowFirst["BTPThoatChuyenNgay"].ToString(), out tpThoatChuyenNgay);
                        int.TryParse(rowFirst["BTPThoatChuyenNgayGiam"].ToString(), out tpThoatChuyenNgayGiam);
                        nangSuat.BTPThoatChuyenNgay = tpThoatChuyenNgay;
                        nangSuat.BTPThoatChuyenNgayGiam = tpThoatChuyenNgayGiam;

                        int btpNgay = 0;
                        int btpNgayGiam = 0;
                        int.TryParse(rowFirst["BTPTang"].ToString(), out btpNgay);
                        int.TryParse(rowFirst["BTPGiam"].ToString(), out btpNgayGiam);
                        nangSuat.BTPNgay = btpNgay;
                        nangSuat.BTPNgayGiam = btpNgayGiam;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return nangSuat;
        }

        public int ThemOBJ(NangXuat obj)
        {
            int kq = 0;
            try
            {
                string sql = "insert into NangXuat(Ngay, STTChuyen_SanPham, DinhMucNgay, NhipDoSanXuat, BTPTrenChuyen, TimeLastChange, IsEndDate, IsStopOnDay, TimeStopOnDay, CreatedDate) values('" + obj.Ngay + "','" + obj.STTChuyen_SanPham + "','" + obj.DinhMucNgay + "', '" + obj.NhipDoSanXuat + "','" + obj.BTPTrenChuyen + "','" + obj.TimeLastChange + "','" + obj.IsEndDate + "','" + obj.IsStopOnDay + "', '" + obj.TimeStopOnDay + "', '" + DateTime.Now + "')";
                kq = dbclass.TruyVan_XuLy(sql);
                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể lưu thông tin năng xuất của chuyền vào CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public int SuaThongTinOBJ(NangXuat obj)
        {
            int kq = 0;
            try
            {

                string sql = "update NangXuat set DinhMucNgay = '" + obj.DinhMucNgay + "', NhipDoSanXuat ='" + obj.NhipDoSanXuat + "', TimeLastChange ='" + obj.TimeLastChange + "', IsEndDate='" + obj.IsEndDate + "', IsStopOnDay='" + obj.IsStopOnDay + "', TimeStopOnDay='" + obj.TimeStopOnDay + "', UpdatedDate='" + DateTime.Now + "' where Ngay ='" + obj.Ngay + "' and STTChuyen_SanPham ='" + obj.STTChuyen_SanPham + "' and IsDeleted=0";
                kq = dbclass.TruyVan_XuLy(sql);

                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể thay đổi thông tin thành phẩm của chuyền dưới CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public int XoaOBJ(string STTChuyen_SanPham)
        {
            int kq = 0;

            try
            {
                string sql = "Update NangXuat Set IsDeleted=1 where STTChuyen_SanPham ='" + STTChuyen_SanPham + "'";
                kq = dbclass.TruyVan_XuLy(sql);
                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể xoá thông tin năng xuất dưới CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public int SuaThongTinBTP(NangXuat obj)
        {
            int kq = 0;
            try
            {

                string sql = "update NangXuat set BTPTrenChuyen = '" + obj.BTPTrenChuyen + "', IsChangeBTP =1, IsBTP =1 where Ngay ='" + obj.Ngay + "' and STTChuyen_SanPham ='" + obj.STTChuyen_SanPham + "' and IsDeleted=0";
                kq = dbclass.TruyVan_XuLy(sql);

                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể thay đổi thông tin bán thành phẩm của chuyền dưới CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public int SuaThongTinBTPLoi(NangXuat obj)
        {
            int kq = 0;
            try
            {

                string sql = "update NangXuat set BTPLoi = '" + obj.BTPLoi + "' where Ngay ='" + obj.Ngay + "' and STTChuyen_SanPham ='" + obj.STTChuyen_SanPham + "' and IsDeleted=0";
                kq = dbclass.TruyVan_XuLy(sql);

                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể thay đổi thông tin bán thành phẩm lỗi của chuyền dưới CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public List<ChuyenSanPham> CheckIsBTP()
        {
            List<ChuyenSanPham> listResult = null;
            try
            {
                dt.Clear();
                string strSQL = "";
                var ngay = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                if (AccountSuccess.IsAll != 1)
                {
                    strSQL = "select nx.STTCHuyen_SanPham, c_sp.MaChuyen, c.IdDen, sp.TenSanPham from NangXuat nx, Chuyen_SanPham c_sp, Chuyen c, SanPham sp WHERE nx.IsBTP = 1 and nx.Ngay ='" + ngay + "' and nx.IsDeleted=0 and nx.STTCHuyen_SanPham = c_sp.STT and c_sp.MaChuyen = c.MaChuyen and c.FloorId = '" + AccountSuccess.IdFloor + "' and c_sp.MaSanPham=sp.MaSanPham";
                }
                else
                {
                    strSQL = "select nx.STTCHuyen_SanPham, c_sp.MaChuyen, c.IdDen, sp.TenSanPham from NangXuat nx, Chuyen_SanPham c_sp, Chuyen c, SanPham sp WHERE nx.IsBTP = 1 and nx.Ngay ='" + ngay + "' and nx.IsDeleted=0 and nx.STTCHuyen_SanPham = c_sp.STT and c_sp.MaChuyen = c.MaChuyen and c_sp.MaSanPham=sp.MaSanPham";
                }
                dt = dbclass.TruyVan_TraVe_DataTable(strSQL);
                if (dt != null && dt.Rows.Count > 0)
                {
                    EndIsBTP();
                    listResult = new List<ChuyenSanPham>();
                    foreach (DataRow row in dt.Rows)
                    {
                        listResult.Add(new ChuyenSanPham()
                        {
                            STT = row["STTCHuyen_SanPham"].ToString(),
                            MaChuyen = row["MaChuyen"].ToString(),
                            TenSanPham = row["TenSanPham"].ToString(),
                            IdDen = row["IdDen"].ToString()
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listResult;

        }

        private int EndIsBTP()
        {
            int result = 0;
            try
            {
                string strSQL = "update NangXuat set IsBTP =0 where IsBTP =1 and IsDeleted=0";
                result = dbclass.TruyVan_XuLy(strSQL);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public bool CheckThucHienOfPhanCong(string sttChuyenSanPham)
        {
            bool isThucHien = false;
            try
            {
                dt.Clear();
                string strSQL = "select * from NangXuat where STTCHuyen_SanPham='" + sttChuyenSanPham + "' and Ngay < '" + DateTime.Now.Date + "' and ThucHienNgay > 0 and IsDeleted=0";
                dt = dbclass.TruyVan_TraVe_DataTable(strSQL);
                if (dt != null && dt.Rows.Count > 0)
                    isThucHien = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isThucHien;
        }

        public int SuaThucHienNgay(NangXuat obj)
        {
            int kq = 0;
            try
            {
                string sql = "update NangXuat set ThucHienNgay = '" + obj.ThucHienNgay + "', ThucHienNgayGiam='" + obj.ThucHienNgayGiam + "',BTPThoatChuyenNgay='" + obj.BTPThoatChuyenNgay + "',BTPThoatChuyenNgayGiam='" + obj.BTPThoatChuyenNgayGiam + "',BTPTang='" + obj.BTPNgay + "',BTPGiam='" + obj.BTPNgayGiam + "' where Ngay ='" + obj.Ngay + "' and STTChuyen_SanPham ='" + obj.STTChuyen_SanPham + "' and IsDeleted=0";

                kq = dbclass.TruyVan_XuLy(sql);
                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể thay đổi thông tin bán thành phẩm lỗi của chuyền dưới CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public List<NangXuat> GetAllNSOfPCC(string sttChuyenSanPham)
        {
            List<NangXuat> listNS = null;
            try
            {
                dt.Clear();
                if (!string.IsNullOrEmpty(sttChuyenSanPham))
                {
                    string sql = "select * from NangXuat where STTCHuyen_SanPham=" + sttChuyenSanPham + " and IsDeleted=0 Order by Ngay desc";
                    dt = dbclass.TruyVan_TraVe_DataTable(sql);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        listNS = new List<NangXuat>();
                        foreach (DataRow row in dt.Rows)
                        {
                            var nangSuat = new NangXuat();
                            int btpTrenChuyen = 0;
                            int.TryParse(row["BTPTrenChuyen"].ToString(), out btpTrenChuyen);
                            nangSuat.BTPTrenChuyen = btpTrenChuyen;
                            float dinhMucNgay = 0;
                            float.TryParse(row["DinhMucNgay"].ToString(), out dinhMucNgay);
                            nangSuat.DinhMucNgay = dinhMucNgay;
                            nangSuat.STTChuyen_SanPham = sttChuyenSanPham;
                            nangSuat.Ngay =  row["Ngay"].ToString() ;
                            int nhipDoSanXuat = 0;
                            int.TryParse(row["NhipDoSanXuat"].ToString(), out nhipDoSanXuat);
                            nangSuat.NhipDoSanXuat = nhipDoSanXuat;
                            int thuHienNgay = 0;
                            int thuHienNgayGiam = 0;
                            int.TryParse(row["ThucHienNgay"].ToString(), out thuHienNgay);
                            int.TryParse(row["ThucHienNgayGiam"].ToString(), out thuHienNgayGiam);
                            if (thuHienNgay < 0)
                                thuHienNgay = 0;
                            if (thuHienNgayGiam < 0)
                                thuHienNgayGiam = 0;
                            nangSuat.ThucHienNgayGiam = thuHienNgayGiam;
                            nangSuat.ThucHienNgay = thuHienNgay - thuHienNgayGiam;
                            if (nangSuat.ThucHienNgay < 0)
                                nangSuat.ThucHienNgay = 0;

                            //HoangHai
                            int tpThoatChuyenNgay = 0, tpThoatChuyenNgayGiam = 0;
                            int.TryParse(row["BTPThoatChuyenNgay"].ToString(), out tpThoatChuyenNgay);
                            int.TryParse(row["BTPThoatChuyenNgayGiam"].ToString(), out tpThoatChuyenNgayGiam);
                            if (tpThoatChuyenNgay < 0)
                                tpThoatChuyenNgay = 0;
                            if (tpThoatChuyenNgayGiam < 0)
                                tpThoatChuyenNgayGiam = 0;
                            nangSuat.BTPThoatChuyenNgay = (tpThoatChuyenNgay - tpThoatChuyenNgayGiam);
                            nangSuat.BTPThoatChuyenNgayGiam = tpThoatChuyenNgayGiam;
                            nangSuat.BTPThoatChuyenNgay = (nangSuat.BTPThoatChuyenNgay < 0 ? 0 : nangSuat.BTPThoatChuyenNgay);

                            int btp = 0, btpGiam = 0;
                            int.TryParse(row["BTPTang"].ToString(), out btp);
                            int.TryParse(row["BTPGiam"].ToString(), out btpGiam);
                            btp = btp < 0 ? 0 : btp;
                            btpGiam = btpGiam < 0 ? 0 : btpGiam;

                            nangSuat.BTPNgay = ((btp - btpGiam) < 0 ? 0 : (btp - btpGiam));
                            nangSuat.BTPNgayGiam = btpGiam;

                            listNS.Add(nangSuat);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listNS;
        }

        public bool CheckExistProductivityWork(NangXuat nangxuat)
        {
            bool IsExist = false;
            try
            {
                string strSQL = "SELECT * FROM NangXuat nx WHERE nx.STTChuyen_SanPham = '" + nangxuat.STTChuyen_SanPham + "' and nx.Ngay ='" + nangxuat.Ngay + "' and IsDeleted=0";
                var dtTonTaiNangXuat = dbclass.TruyVan_TraVe_DataTable(strSQL);
                if (dtTonTaiNangXuat != null && dtTonTaiNangXuat.Rows.Count > 0)
                    IsExist = true;
                return IsExist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ModelDayInfo> GetListProductivityOfLineOnDay(int lineId, DateTime dateTime)
        {
            try
            {
                List<ModelDayInfo> listProductivity = null;
                string strSQL = "Select csp.NangXuatSanXuat, nx.STTChuyen_SanPham, nx.ThucHienNgay, nx.ThucHienNgayGiam, nx.DinhMucNgay, tp.LaoDongChuyen, tp.NangXuatLaoDong, nx.IsStopOnDay From NangXuat nx, ThanhPham tp Where csp.STT = nx.STTChuyen_SanPham and nx.STTChuyen_SanPham==tp.STTChuyen_SanPham and tp.Ngay='" + dateTime + "' and tp.IsDeleted=0 and nx.IsDeleted=0 and nx.Ngay='" + dateTime + "' and nx.STTChuyen_SanPham in (Select STT From Chuyen_SanPham Where IsDelete=0 and MaChuyen=" + lineId + " and Thang=" + dateTime.Month + " and =" + dateTime.Year + ")";
                var dtProductivity = dbclass.TruyVan_TraVe_DataTable(strSQL);
                if (dtProductivity != null && dtProductivity.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        listProductivity = new List<ModelDayInfo>();
                        int thucHienNgay = 0;
                        int thucHienNgayGiam = 0;
                        int.TryParse(row["ThucHienNgay"].ToString(), out thucHienNgay);
                        int.TryParse(row["ThucHienNgayGiam"].ToString(), out thucHienNgayGiam);
                        thucHienNgay = thucHienNgay - thucHienNgayGiam;
                        if (thucHienNgay < 0)
                            thucHienNgay = 0;
                        int dinhMucNgay = 0;
                        int.TryParse(row["DinhMucNgay"].ToString(), out dinhMucNgay);
                        int laoDongChuyen = 0;
                        int.TryParse(row["LaoDongChuyen"].ToString(), out laoDongChuyen);
                        float nangSuatLaoDong = 0;
                        float.TryParse(row["NangXuatLaoDong"].ToString(), out nangSuatLaoDong);
                        float thoiGianCheTao = 0;
                        float.TryParse(row["NangXuatSanXuat"].ToString(), out thoiGianCheTao);
                        bool isStopOnDay = false;
                        bool.TryParse(row["IsStopOnDay"].ToString(), out isStopOnDay);
                        listProductivity.Add(new ModelDayInfo()
                        {
                            ThucHienNgay = thucHienNgay,
                            DinhMucNgay = dinhMucNgay,
                            STTChuyenSanPham = int.Parse(row["STTChuyen_SanPham"].ToString()),
                            SoLaoDong = laoDongChuyen,
                            NangSuatLaoDong = nangSuatLaoDong,
                            ThoiGianCheTao = thoiGianCheTao,
                            IsStopOnDay = isStopOnDay
                        });
                    }
                }
                return listProductivity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        DataTable dtSanLuongGio = new DataTable();
       
        //public List<NangSuat_Report_H> GetProductivitiesOfLines(DateTime  date)
        //{
        //    var productivities = new List<NangSuat_Report_H>();
        //    var dateNow = date.Day + "/" + date.Month + "/" + date.Year;
        //    try
        //    {   //HoangHai
        //        var lines = chuyenDAO.GetListDSChuyen(AccountSuccess.strListChuyenId).OrderBy(c => c.IntSTT).ToList();
        //        foreach (var chuyen in lines)
        //        {
        //            var listSTTChuyen_SanPham = chuyenDAO.FindListSTTChuyen_SanPhamOfDateNow(chuyen.MaChuyen);
        //            if (listSTTChuyen_SanPham != null && listSTTChuyen_SanPham.Count > 0)
        //            {
        //                foreach (var sttChuyen_SanPham in listSTTChuyen_SanPham)
        //                {
        //                    string strSQL = "SELECT c.TenChuyen, c.LaoDongDinhBien, tp.LaoDongChuyen, sp.TenSanPham, csp.SanLuongKeHoach, csp.IsFinish, csp.LuyKeTH, nx.DinhMucNgay,";
        //                    strSQL += " (SELECT SUM(BTPNgay) FROM BTP b WHERE b.STTChuyen_SanPham = '" + sttChuyen_SanPham + "'AND b.IsEndOfLine=1 and CommandTypeId=" + (int)eCommandRecive.BTPIncrease + ") LuyKeBTPTang,";
        //                    strSQL += " (SELECT SUM(BTPNgay) FROM BTP b WHERE b.STTChuyen_SanPham = '" + sttChuyen_SanPham + "' and b.IsEndOfLine=1 and CommandTypeId=" + (int)eCommandRecive.BTPReduce + ") LuyKeBTPGiam,";
        //                    strSQL += "(nx.BTPTang-nx.BTPGiam) BTPNgay,";
        //                    strSQL += " (nx.ThucHienNgay-nx.ThucHienNgayGiam) ThucHienNgay, (nx.SanLuongLoi-nx.SanLuongLoiGiam) SanLuongLoi, ";
        //                    strSQL += " case when nx.DinhMucNgay=0 then 0 else (ROUND((((nx.ThucHienNgay-nx.ThucHienNgayGiam)/nx.DinhMucNgay)*100),0))end TyLeThucHien, ";
        //                    strSQL += " case when nx.DinhMucNgay=0 then 0 else (ROUND((((nx.SanLuongLoi-nx.SanLuongLoiGiam)/nx.DinhMucNgay)*100),0))end  TyLeLoi,  nx.BTPTrenChuyen,  nx.NhipDoSanXuat, nx.NhipDoThucTe,nx.NhipDoThucTeBTPThoatChuyen,";
        //                    strSQL += " case when nx.NhipDoThucTe = 0 then 0 else ROUND((nx.NhipDoSanXuat/nx.NhipDoThucTe)*100,0)end  TiLeNhip,";
        //                    strSQL += " ROUND((nx.ThucHienNgay-nx.ThucHienNgayGiam)*sp.DonGiaCM,2) DoanhThuNgay, (select ROUND(SUM((nx.ThucHienNgay-nx.ThucHienNgayGiam)*sp.DonGiaCM),2) from NangXuat nx, SanPham sp, Chuyen_SanPham c_sp ";
        //                    strSQL += " where nx.STTCHuyen_SanPham=c_sp.STT and c_sp.MaSanPham=sp.MaSanPham and c_sp.STT='" + sttChuyen_SanPham + "') DoanhThuThang, (nx.BTPThoatChuyenNgay-nx.BTPThoatChuyenNgayGiam) BTPThoatChuyenNgay, csp.LuyKeBTPThoatChuyen";
        //                    strSQL += " FROM Chuyen c, Chuyen_SanPham csp, NangXuat nx, SanPham sp, ThanhPham tp WHERE c.IsDeleted = 0 and csp.IsDelete = 0 and nx.IsDeleted = 0 and sp.IsDelete = 0 and tp.IsDeleted = 0 and csp.STT ='" + sttChuyen_SanPham + "' AND nx.STTCHuyen_SanPham = csp.STT  and nx.Ngay ='" + dateNow + "' AND csp.MaSanPham = sp.MaSanPham AND tp.STTChuyen_SanPham = csp.STT AND tp.Ngay = nx.Ngay AND c.MaChuyen = csp.MaChuyen";
                            
        //                    dt.Clear();
        //                    dt = dbclass.TruyVan_TraVe_DataTable(strSQL);
        //                    if (dt != null && dt.Rows.Count > 0)
        //                    {
        //                        var workHours = shiftDAO.GetListWorkHoursOfLineByLineId(chuyen.MaChuyen);

        //                        NangSuat_Report_H pro;
        //                        int sanLuongKeHoach = 0, luyKeTH = 0, luyKeBTPTang = 0, luyKeBTPGiam = 0, btpNgayTang = 0, btpTrenChuyen = 0, thucHienNgay = 0, TCNgay = 0, LDChuyen = 0;
        //                        float dinhMucNgay = 0, doanhThuNgay = 0, doanhThuThang = 0, nhipDoThucTe = 0, nhipDoSanXuat = 0, Von = 0;

        //                        for (int j = 0; j < dt.Rows.Count; j++)
        //                        {
        //                            #region TT NS
        //                            sanLuongKeHoach = 0;
        //                            luyKeTH = 0;
        //                            luyKeBTPTang = 0;
        //                            luyKeBTPGiam = 0;
        //                            btpNgayTang = 0;
        //                            btpTrenChuyen = 0;
        //                            dinhMucNgay = 0;
        //                            doanhThuNgay = 0;
        //                            doanhThuThang = 0;
        //                            nhipDoThucTe = 0;
        //                            nhipDoSanXuat = 0;
        //                            thucHienNgay = 0;
        //                            TCNgay = 0;
        //                            Von = 0;
        //                            pro = new NangSuat_Report_H();
        //                            pro.LineName = dt.Rows[j]["TenChuyen"].ToString();

        //                            int.TryParse(dt.Rows[j]["LaoDongChuyen"].ToString(), out LDChuyen);
        //                            pro.Labors = LDChuyen + "/" + dt.Rows[j]["LaoDongDinhBien"].ToString();
        //                            pro.CommoditityName = dt.Rows[j]["TenSanPham"].ToString();

        //                            int.TryParse(dt.Rows[j]["SanLuongKeHoach"].ToString(), out sanLuongKeHoach);
        //                            pro.SanLuongKeHoach = sanLuongKeHoach;

        //                            int.TryParse(dt.Rows[j]["LuyKeTH"].ToString(), out luyKeTH);
        //                            pro.LuyKeTH = luyKeTH < 0 ? 0 : luyKeTH;

        //                            float.TryParse(dt.Rows[j]["DoanhThuThang"].ToString(), out doanhThuThang);
        //                            pro.ThuNhapThang = doanhThuThang < 0 ? 0 : doanhThuThang;
        //                            float.TryParse(dt.Rows[j]["DoanhThuNgay"].ToString(), out doanhThuNgay);
        //                            pro.ThuNhapNgay = doanhThuNgay < 0 ? 0 : doanhThuNgay;

        //                            int.TryParse(dt.Rows[j]["LuyKeBTPTang"].ToString(), out luyKeBTPTang);
        //                            int.TryParse(dt.Rows[j]["LuyKeBTPGiam"].ToString(), out luyKeBTPGiam);
        //                            pro.LuyKeBTP = (luyKeBTPTang - luyKeBTPGiam) < 0 ? 0 : (luyKeBTPTang - luyKeBTPGiam);
        //                            int.TryParse(dt.Rows[j]["BTPTrenChuyen"].ToString(), out btpTrenChuyen);
        //                            pro.BTPTrenChuyen = btpTrenChuyen < 0 ? 0 : btpTrenChuyen;
        //                            int.TryParse(dt.Rows[j]["BTPNgay"].ToString(), out btpNgayTang);
        //                            pro.BTPNgay = btpNgayTang < 0 ? 0 : btpNgayTang;

        //                            float.TryParse(dt.Rows[j]["DinhMucNgay"].ToString(), out dinhMucNgay);
        //                            pro.DinhMucNgay = (int)dinhMucNgay;

        //                            pro.DinhMucGio = dinhMucNgay / workHours.Count;

        //                            float.TryParse(dt.Rows[j]["NhipDoThucTe"].ToString(), out nhipDoThucTe);
        //                            pro.NhipTT = nhipDoThucTe < 0 ? 0 : nhipDoThucTe;

        //                            float.TryParse(dt.Rows[j]["NhipDoSanXuat"].ToString(), out nhipDoSanXuat);
        //                            pro.NhipNC = nhipDoSanXuat < 0 ? 0 : nhipDoSanXuat;

        //                            int.TryParse(dt.Rows[j]["ThucHienNgay"].ToString(), out thucHienNgay);
        //                            pro.TongTHNgay = thucHienNgay < 0 ? 0 : thucHienNgay;

        //                            int.TryParse(dt.Rows[j]["BTPThoatChuyenNgay"].ToString(), out TCNgay);
        //                            pro.TongTCNgay = TCNgay < 0 ? 0 : TCNgay;

        //                            Von = (int)(Math.Ceiling((double)btpTrenChuyen / LDChuyen));
        //                            pro.Von = Von < 0 ? 0 : Von;

        //                            pro.LineWorkingTimes = new List<ModelWorkHours>();

        //                            pro.TGDaLV = 0;
        //                            #endregion

        //                            #region Danh Sach Gio LV cua Chuyen
        //                            if (workHours != null && workHours.Count > 0)
        //                            { 
        //                                int KCSTang = 0, KCSGiam = 0, TongKCS = 0, TCTang = 0, TCGiam = 0, tongTC = 0, ErrorTang = 0, ErrorGiam = 0, TongError = 0;

        //                                foreach (var item in workHours)
        //                                {
        //                                    KCSTang = 0;
        //                                    KCSGiam = 0;
        //                                    TongKCS = 0;
        //                                    TCTang = 0;
        //                                    TCGiam = 0;
        //                                    tongTC = 0;
        //                                    ErrorTang = 0;
        //                                    ErrorGiam = 0;
        //                                    TongError = 0;
        //                                    if (date.TimeOfDay > item.TimeStart)
        //                                        pro.TGDaLV++;

        //                                    string sqlStr = "";
        //                                    sqlStr += "select (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + chuyen.MaChuyen + " and Time >= '" + item.TimeStart + "' and Time <='" + item.TimeEnd + "' and Date='" + dateNow + "' and CommandTypeId=" + (int)eCommandRecive.ProductIncrease + " and ProductOutputTypeId=" + (int)eProductOutputType.KCS + " and IsEndOfLine=1) AS SanLuongTang,";
        //                                    sqlStr += "(select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + chuyen.MaChuyen + " and Time >= '" + item.TimeStart + "' and Time <='" + item.TimeEnd + "' and Date='" + dateNow + "' and CommandTypeId=" + (int)eCommandRecive.ProductReduce + " and ProductOutputTypeId=" + (int)eProductOutputType.KCS + " and IsEndOfLine=1) AS SanLuongGiam,";
        //                                    sqlStr += "(select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + chuyen.MaChuyen + " and Time >= '" + item.TimeStart + "' and Time <='" + item.TimeEnd + "' and Date='" + dateNow + "' and CommandTypeId=" + (int)eCommandRecive.ProductIncrease + " and ProductOutputTypeId=" + (int)eProductOutputType.TC + " and IsEndOfLine=1) AS TCTang,";
        //                                    sqlStr += "(select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + chuyen.MaChuyen + " and Time >= '" + item.TimeStart + "' and Time <='" + item.TimeEnd + "' and Date='" + dateNow + "' and CommandTypeId=" + (int)eCommandRecive.ProductReduce + " and ProductOutputTypeId=" + (int)eProductOutputType.TC + " and IsEndOfLine=1) AS TCGiam,";
        //                                    sqlStr += "(select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + chuyen.MaChuyen + " and Time >= '" + item.TimeStart + "' and Time <='" + item.TimeEnd + "' and Date='" + dateNow + "' and CommandTypeId=" + (int)eCommandRecive.ErrorIncrease + " and IsEndOfLine=1) AS ErrorTang,";
        //                                    sqlStr += "(select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + chuyen.MaChuyen + " and Time >= '" + item.TimeStart + "' and Time <='" + item.TimeEnd + "' and Date='" + dateNow + "' and CommandTypeId=" + (int)eCommandRecive.ErrorReduce + " and IsEndOfLine=1) AS ErrorGiam";

        //                                    dtSanLuongGio = dbclass.TruyVan_TraVe_DataTable(sqlStr);
        //                                    if (dtSanLuongGio != null && dtSanLuongGio.Rows.Count > 0)
        //                                    {
        //                                        DataRow rowSanLuongGio = dtSanLuongGio.Rows[0];
        //                                        if (rowSanLuongGio["SanLuongTang"] != null)
        //                                            int.TryParse(rowSanLuongGio["SanLuongTang"].ToString(), out KCSTang);
        //                                        if (rowSanLuongGio["SanLuongGiam"] != null)
        //                                            int.TryParse(rowSanLuongGio["SanLuongGiam"].ToString(), out KCSGiam);
        //                                        TongKCS = KCSTang - KCSGiam;
        //                                        item.KCS = TongKCS < 0 ? 0 : TongKCS;

        //                                        if (rowSanLuongGio["TCTang"] != null)
        //                                            int.TryParse(rowSanLuongGio["TCTang"].ToString(), out TCTang);
        //                                        if (rowSanLuongGio["TCGiam"] != null)
        //                                            int.TryParse(rowSanLuongGio["TCGiam"].ToString(), out TCGiam);
        //                                        tongTC = TCTang - TCGiam;
        //                                        item.TC = tongTC < 0 ? 0 : tongTC;

        //                                        if (rowSanLuongGio["ErrorTang"] != null)
        //                                            int.TryParse(rowSanLuongGio["ErrorTang"].ToString(), out ErrorTang);
        //                                        if (rowSanLuongGio["ErrorGiam"] != null)
        //                                            int.TryParse(rowSanLuongGio["ErrorGiam"].ToString(), out ErrorGiam);
        //                                        TongError = ErrorTang - ErrorGiam;
        //                                        item.Error = TongError < 0 ? 0 : TongError;

        //                                        // VC = BTP TRen Chuyen tai thoi diem do / so lao dong
        //                                    }

        //                                }
        //                                pro.LineWorkingTimes.AddRange(workHours); 
        //                            }
        //                            #endregion

        //                            productivities.Add(pro);

        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return productivities;
        //}
  
    }
}
