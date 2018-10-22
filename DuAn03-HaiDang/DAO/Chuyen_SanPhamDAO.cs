using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DuAn03_HaiDang.DATAACCESS;
using System.Windows.Forms;
using DuAn03_HaiDang.POJO;
using DuAn03_HaiDang.Model;
using QuanLyNangSuat.Model;
using DuAn03_HaiDang.KeyPad_Chuyen.dao;
using GPRO.Ultilities;
using DuAn03_HaiDang.Enum;
using PMS.Business.Enum;

namespace DuAn03_HaiDang.DAO
{
    public class Chuyen_SanPhamDAO
    {
        private DataTable dt;
        private ChuyenDAO chuyenDAO;
        private SanPhamDAO sanPhamDAO;
        private DataTable dtChuyenSanPham;

        public Chuyen_SanPhamDAO()
        {
            chuyenDAO = new ChuyenDAO();
            sanPhamDAO = new SanPhamDAO();
            dtChuyenSanPham = new DataTable();
            this.dt = new DataTable();
        }

        public DataTable DSPhanCongCuaChuyen(string MaChuyen)
        {
            dt.Clear();
            var dateNow = DateTime.Now;
         //   string sql = "select csp.STT, sp.TenSanPham, sp.MaSanPham, csp.NangXuatSanXuat, csp.SanLuongKeHoach, csp.LuyKeTH, csp.LuyKeBTPThoatChuyen, csp.STTThucHien, csp.IsFinish from Chuyen_SanPham csp, SanPham sp Where sp.MaSanPham = csp.MaSanPham and MaChuyen = '" + MaChuyen + "' and csp.IsDelete = 0 and csp.IsFinish = 0 and sp.IsDelete=0 and csp.Thang=" + dateNow.Month + " and csp.Nam=" + dateNow.Year + " Order By csp.STTThucHien ASC";
            string sql = "select csp.STT, sp.TenSanPham, sp.MaSanPham, csp.NangXuatSanXuat, csp.SanLuongKeHoach, csp.LuyKeTH, csp.LuyKeBTPThoatChuyen, csp.STTThucHien, csp.IsFinish from Chuyen_SanPham csp, SanPham sp Where sp.MaSanPham = csp.MaSanPham and MaChuyen = '" + MaChuyen + "' and csp.IsDelete = 0 and csp.IsFinish = 0 and sp.IsDelete=0 Order By csp.STT  asc";
            try
            {
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                return dt;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: không thể lấy được danh sách các mặt hàng phân công cho chuyền từ CSDL.", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return dt;
            }
        }

        #region HoangHai
        public DataTable DSPhanCongTongHopCacChuyen(string strListId)
        {
            dt.Clear();
            var dateNow = DateTime.Now;
            string sql = "select csp.STT, sp.TenSanPham, sp.MaSanPham, csp.NangXuatSanXuat, csp.SanLuongKeHoach, csp.LuyKeTH, csp.LuyKeBTPThoatChuyen, csp.STTThucHien, csp.IsFinish, csp.MaChuyen, csp.STTThucHien from Chuyen_SanPham csp, SanPham sp Where sp.MaSanPham = csp.MaSanPham and MaChuyen in (" + strListId + ") and csp.IsDelete = 0 and csp.IsFinish = 0 and sp.IsDelete=0 and csp.Thang=" + dateNow.Month + " and csp.Nam=" + dateNow.Year + " Order By csp.STTThucHien ASC";
            try
            {
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                return dt;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: không thể lấy được danh sách các mặt hàng phân công cho chuyền từ CSDL.", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return dt;
            }
        }

        public DataTable LayPCCuaChuyenTuDanhSachTongHop(string machuyen, DataTable DSPCTongHop)
        {  
            var tb = new DataTable();
            tb = DSPCTongHop.Clone();
            try
            {              
                if (DSPCTongHop.Rows.Count > 0)
                {
                    foreach (DataRow row in DSPCTongHop.Rows)
                    {
                        if (row["MaChuyen"].ToString() == machuyen)
                        {
                            tb.ImportRow(row);
                        }
                    }
                }
                tb.DefaultView.Sort = "STTThucHien ASC";               
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: không thể lấy được danh sách các mặt hàng phân công cho chuyền từ CSDL.", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
             } 
            return tb;
        }
        #endregion

        public List<ChuyenSanPham> GetListChuyenSanPham(string chuyenId, bool isGetForPhanHang)
        {
            List<ChuyenSanPham> listChuyenSanPham = null;
            try
            {
                dt.Clear();
                var dateTimeNow = DateTime.Now;
                var sql = string.Empty;
                if (isGetForPhanHang)  // trường hợp nếu lấy cho form phân hàng cho chuyền cần lấy những mã hàng đã finish lên luôn
                  //  sql = "select csp.STT, csp.STTThucHien, sp.TenSanPham, sp.MaSanPham, csp.NangXuatSanXuat, csp.SanLuongKeHoach, csp.LuyKeTH, csp.MaChuyen, c.IdDen, csp.Thang, csp.Nam, csp.IsFinish from Chuyen_SanPham csp, SanPham sp, Chuyen c Where sp.MaSanPham = csp.MaSanPham and c.MaChuyen = '" + chuyenId + "' and csp.IsDelete = 0  and sp.IsDelete=0 and csp.MaChuyen=c.MaChuyen and Thang=" + dateTimeNow.Month + " and Nam=" + dateTimeNow.Year + " Order By csp.STTThucHien ASC";
                    sql = "select csp.STT, csp.STTThucHien, sp.TenSanPham, sp.MaSanPham, csp.NangXuatSanXuat, csp.SanLuongKeHoach, csp.LuyKeTH, csp.MaChuyen, c.IdDen, csp.Thang, csp.Nam, csp.IsFinish from Chuyen_SanPham csp, SanPham sp, Chuyen c Where sp.MaSanPham = csp.MaSanPham and c.MaChuyen = '" + chuyenId + "' and csp.IsDelete = 0  and sp.IsDelete=0 and csp.MaChuyen=c.MaChuyen Order By csp.Thang desc , csp.Nam desc";
                else
                   // sql = "select csp.STT, csp.STTThucHien, sp.TenSanPham, sp.MaSanPham, csp.NangXuatSanXuat, csp.SanLuongKeHoach, csp.LuyKeTH, csp.MaChuyen, c.IdDen, csp.Thang, csp.Nam,csp.IsFinish from Chuyen_SanPham csp, SanPham sp, Chuyen c Where sp.MaSanPham = csp.MaSanPham and c.MaChuyen = '" + chuyenId + "' and csp.IsDelete = 0 and csp.IsFinish = 0 and sp.IsDelete=0 and csp.MaChuyen=c.MaChuyen and Thang=" + dateTimeNow.Month + " and Nam=" + dateTimeNow.Year + " Order By csp.STTThucHien ASC";
                    sql = "select csp.STT, csp.STTThucHien, sp.TenSanPham, sp.MaSanPham, csp.NangXuatSanXuat, csp.SanLuongKeHoach, csp.LuyKeTH, csp.MaChuyen, c.IdDen, csp.Thang, csp.Nam,csp.IsFinish from Chuyen_SanPham csp, SanPham sp, Chuyen c Where sp.MaSanPham = csp.MaSanPham and c.MaChuyen = '" + chuyenId + "' and csp.IsDelete = 0 and csp.IsFinish = 0 and sp.IsDelete=0 and csp.MaChuyen=c.MaChuyen  Order By csp.Thang desc , csp.Nam desc";
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    listChuyenSanPham = new List<ChuyenSanPham>();
                    foreach (DataRow row in dt.Rows)
                    {
                        int sttThucHien = 0, sanLuongKeHoach = 0, luyKeTH = 0, thang = 0, nam = 0, isFinish = 0;
                        float thoiGianCheTaoSP = 0;

                        int.TryParse(row["STTThucHien"].ToString(), out sttThucHien);
                        float.TryParse(row["NangXuatSanXuat"].ToString(), out thoiGianCheTaoSP);
                        int.TryParse(row["SanLuongKeHoach"].ToString(), out sanLuongKeHoach);
                        int.TryParse(row["LuyKeTH"].ToString(), out luyKeTH);
                        int.TryParse(row["Thang"].ToString(), out thang);
                        int.TryParse(row["Nam"].ToString(), out nam);
                        int.TryParse(row["IsFinish"].ToString(), out isFinish);
                        listChuyenSanPham.Add(new ChuyenSanPham()
                        {
                            STT = row["STT"].ToString(),
                            TenSanPham = row["TenSanPham"].ToString(),
                            MaSanPham = row["MaSanPham"].ToString(),
                            MaChuyen = row["MaChuyen"].ToString(),
                            IdDen = row["IdDen"].ToString(),
                            STTThucHien = sttThucHien,
                            NangXuatSanXuat = thoiGianCheTaoSP,
                            SanLuongKeHoach = sanLuongKeHoach,
                            LuyKeTH = luyKeTH,
                            Thang = thang,
                            Nam = nam,
                            IsFinishStr = (isFinish == 0 ? "Đang Thực Hiện" : "Hoàn Thành")
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return listChuyenSanPham;
        }

        public DataTable DSPhanCongCuaChuyen_BTPLoi(string MaChuyen)
        {
            dt.Clear();
            string sql = "select csp.STT, sp.TenSanPham, sp.MaSanPham, csp.NangXuatSanXuat, csp.SanLuongKeHoach, csp.LuyKeTH, nx.BTPLoi, csp.STTThucHien from Chuyen_SanPham csp, SanPham sp, NangXuat nx Where sp.MaSanPham = csp.MaSanPham and MaChuyen = '" + MaChuyen + "' and csp.IsDelete = 0 and csp.IsFinish = 0 and sp.IsDelete=0 and nx.STTCHuyen_SanPham = csp.STT and nx.Ngay='" + DateTime.Now.Date.ToString() + "' and csp.Thang=" + DateTime.Now.Month + " and csp.Nam=" + DateTime.Now.Year + " Order By csp.STTThucHien ASC";
            try
            {

                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                return dt;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: không thể lấy được danh sách các mặt hàng phân công cho chuyền từ CSDL.", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return dt;
            }
        }

        public List<string> TimKiemPhanCong(string MaChuyen, string MaSanPham, int thang, int nam)
        {
            List<string> result = new List<string>();
            try
            {
                dt.Clear();
                string sql = "SELECT TOP 1 STT, IsFinish from Chuyen_SanPham  csp Where csp.MaChuyen = '" + MaChuyen + "' and csp.MaSanPham = '" + MaSanPham + "' and Thang=" + thang + " and Nam=" + nam + " and  csp.IsDelete = 0 ORDER BY csp.STT DESC";
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    result.Add(dt.Rows[0]["STT"].ToString());
                    result.Add(dt.Rows[0]["IsFinish"].ToString());
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public string GetSTTChuyenSanPham(string MaChuyen, string MaSanPham, int Thang, int Nam)
        {
            string sttChuyenSanPham = string.Empty;
            try
            {
                dt.Clear();
                string sql = "select STT from Chuyen_SanPham  csp Where csp.MaChuyen = '" + MaChuyen + "' and csp.MaSanPham = '" + MaSanPham + "' and csp.IsDelete = 0 and csp.IsFinish = 0 and csp.Thang=" + Thang + " and Nam=" + Nam;
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    sttChuyenSanPham = dt.Rows[0]["STT"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sttChuyenSanPham;
        }

        public ModelResult ThemPhanCong(Chuyen_SanPham chuyen_sanpham)
        {
            ModelResult modelResult = new ModelResult();
            try
            {
                var dateNow = DateTime.Now;
                int morthNow = dateNow.Month;
                int yearNow = dateNow.Year;
                if (chuyen_sanpham.Nam >= yearNow)
                {
                    if (chuyen_sanpham.Nam == yearNow)
                    {
                        if (chuyen_sanpham.Thang >= morthNow)
                        {
                            var cspExist = CheckExistPCC(chuyen_sanpham.MaChuyen, chuyen_sanpham.MaSanPham, chuyen_sanpham.Thang, chuyen_sanpham.Nam);
                            if (cspExist == null)
                            {
                                string sql = "insert into Chuyen_SanPham(MaChuyen, MaSanPham, SanLuongKeHoach, NangXuatSanXuat, TimeAdd, Thang, Nam, IsMoveQuantityFromMorthOld, STTThucHien) values('" + chuyen_sanpham.MaChuyen + "','" + chuyen_sanpham.MaSanPham + "','" + chuyen_sanpham.SanLuongKeHoach + "','" + chuyen_sanpham.NangXuatSanXuat + "','" + chuyen_sanpham.TimeAdd + "', " + chuyen_sanpham.Thang + ", " + chuyen_sanpham.Nam + ",'" + chuyen_sanpham.IsMoveQuantityFromMorthOld + "'," + chuyen_sanpham.STTThucHien + ")";
                                var rsAdd = dbclass.TruyVan_XuLy(sql);
                                if (rsAdd > 0)
                                    modelResult.IsSuccsess = true;
                            }
                            else
                            {
                                if (cspExist.LuyKeTH == 0)
                                {
                                    if (MessageBox.Show("Bạn đã phân công mặt hàng " + chuyen_sanpham.TenSanPham + " cho chuyền " + chuyen_sanpham.TenChuyen + " vào thời gian " + chuyen_sanpham.Thang + "/" + chuyen_sanpham.Nam + " rồi. Bạn có muốn cập nhập thông tin này ?", "Cập nhập dữ liệu", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                    {
                                        int reUpdate = SuaThongTinPhanCong(chuyen_sanpham);
                                        if (reUpdate == 0)
                                            MessageBox.Show("Cập nhập thông tin phân công mặt hàng " + chuyen_sanpham.TenSanPham + " cho chuyền " + chuyen_sanpham.TenChuyen + " vào thời gian " + chuyen_sanpham.Thang + "/" + chuyen_sanpham.Nam + "không thành công.");
                                        else
                                            modelResult.IsSuccsess = true;
                                    }
                                }
                                else
                                    MessageBox.Show("Bạn đã phân công mặt hàng " + chuyen_sanpham.TenSanPham + " cho chuyền " + chuyen_sanpham.TenChuyen + " vào thời gian " + chuyen_sanpham.Thang + "/" + chuyen_sanpham.Nam + " rồi. Việc phân công có thông tin sản xuất nên không thể chỉnh sửa");
                            }
                        }
                        else
                        {
                            modelResult.ListError.Add(new ModelError()
                            {
                                ClassName = this.GetType().Name,
                                MethodName = "AddListPCC",
                                ErrorContent = "Lỗi: Tháng nhập phân công hàng cho chuyền không được nhỏ hơn tháng hiện tại."
                            });
                        }
                    }
                    else
                    {
                        var cspExist = CheckExistPCC(chuyen_sanpham.MaChuyen, chuyen_sanpham.MaSanPham, chuyen_sanpham.Thang, chuyen_sanpham.Nam);
                        if (cspExist == null)
                        {
                            string sql = "insert into Chuyen_SanPham(MaChuyen, MaSanPham, SanLuongKeHoach, NangXuatSanXuat, TimeAdd, Thang, Nam, IsMoveQuantityFromMorthOld, STTThucHien) values('" + chuyen_sanpham.MaChuyen + "','" + chuyen_sanpham.MaSanPham + "','" + chuyen_sanpham.SanLuongKeHoach + "','" + chuyen_sanpham.NangXuatSanXuat + "','" + chuyen_sanpham.TimeAdd + "', " + chuyen_sanpham.Thang + ", " + chuyen_sanpham.Nam + ",'" + chuyen_sanpham.IsMoveQuantityFromMorthOld + "'," + chuyen_sanpham.STTThucHien + ")";
                            var rsAdd = dbclass.TruyVan_XuLy(sql);
                            if (rsAdd > 0)
                                modelResult.IsSuccsess = true;
                        }
                        else
                        {
                            if (chuyen_sanpham.LuyKeTH == 0)
                            {
                                if (MessageBox.Show("Bạn đã phân công mặt hàng " + chuyen_sanpham.TenSanPham + " cho chuyền " + chuyen_sanpham.TenChuyen + " vào thời gian " + chuyen_sanpham.Thang + "/" + chuyen_sanpham.Nam + " rồi. Bạn có muốn cập nhập thông tin này ?", "Cập nhập dữ liệu", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    int reUpdate = SuaThongTinPhanCong(chuyen_sanpham);
                                    if (reUpdate == 0)
                                        MessageBox.Show("Cập nhập thông tin phân công mặt hàng " + chuyen_sanpham.TenSanPham + " cho chuyền " + chuyen_sanpham.TenChuyen + " vào thời gian " + chuyen_sanpham.Thang + "/" + chuyen_sanpham.Nam + "không thành công.");
                                    else
                                        modelResult.IsSuccsess = true;
                                }
                            }
                            else
                                MessageBox.Show("Bạn đã phân công mặt hàng " + chuyen_sanpham.TenSanPham + " cho chuyền " + chuyen_sanpham.TenChuyen + " vào thời gian " + chuyen_sanpham.Thang + "/" + chuyen_sanpham.Nam + " rồi. Việc phân công có thông tin sản xuất nên không thể chỉnh sửa");
                        }
                    }
                }
                else
                {
                    modelResult.ListError.Add(new ModelError()
                    {
                        ClassName = this.GetType().Name,
                        MethodName = "AddListPCC",
                        ErrorContent = "Lỗi: Năm nhập phân công hàng cho chuyền không được nhỏ hơn năm hiện tại."
                    });
                }
            }
            catch (Exception ex)
            {
                modelResult.ListError.Add(new ModelError()
                {
                    ClassName = this.GetType().Name,
                    MethodName = "AddListPCC",
                    ErrorContent = "Lỗi: việc phân công Mặt Hàng " + chuyen_sanpham.TenSanPham + " cho chuyền " + chuyen_sanpham.TenChuyen + " thất bại."
                });
            }
            return modelResult;
        }

        public int SuaThongTinPhanCong(Chuyen_SanPham chuyen_sanpham)
        {
            int kq = 0;
            try
            {

                string sql = "update Chuyen_SanPham set STTThucHien=" + chuyen_sanpham.STTThucHien + ", SanLuongKeHoach = '" + chuyen_sanpham.SanLuongKeHoach + "', NangXuatSanXuat = '" + chuyen_sanpham.NangXuatSanXuat + "', IsFinish = '" + chuyen_sanpham.IsFinish + "', IsFinishBTPThoatChuyen='" + chuyen_sanpham.IsFinishBTPThoatChuyen + "', Thang=" + chuyen_sanpham.Thang + ", Nam=" + chuyen_sanpham.Nam + " where STT ='" + chuyen_sanpham.STT + "'";
                kq = dbclass.TruyVan_XuLy(sql);
                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể thay đổi thông tin phân công mặt hàng cho chuyền dưới CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public int SuaThongTinPhanCongTheoSTT(Chuyen_SanPham chuyen_sanpham)
        {
            int kq = 0;
            try
            {

                string sql = "update Chuyen_SanPham set STTThucHien=" + chuyen_sanpham.STTThucHien + ", SanLuongKeHoach = '" + chuyen_sanpham.SanLuongKeHoach + "', NangXuatSanXuat = '" + chuyen_sanpham.NangXuatSanXuat + "', IsFinish = '" + chuyen_sanpham.IsFinish + "' where STT ='" + chuyen_sanpham.STT + "'";
                kq = dbclass.TruyVan_XuLy(sql);

                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể thay đổi thông tin phân công mặt hàng cho chuyền dưới CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public int XoaPhanCong(string MaChuyen, string MaSanPham)
        {
            int kq = 0;

            try
            {


                string sql = "update Chuyen_SanPham set IsDelete = 1 where MaChuyen = '" + MaChuyen + "' and MaSanPham ='" + MaSanPham + "'";
                kq = dbclass.TruyVan_XuLy(sql);

                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể xoá phân công mặt hàng cho chuyền dưới CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public int XoaOBJ(string sttChuyen_SanPham)
        {
            int kq = 0;

            try
            {
                string sql = "update Chuyen_SanPham set IsDelete = 1 where STT ='" + sttChuyen_SanPham + "'";
                kq = dbclass.TruyVan_XuLy(sql);
                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể xoá thông tin chuyền_Mặt Hàng dưới CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public void loadPhanCongraDataGirdview(DataGridView dg, string MaChuyen)
        {

            dt.Clear();
            string Strsql = "select csp.STT, csp.STTThucHien, sp.TenSanPham, csp.NangXuatSanXuat, csp.SanLuongKeHoach, csp.Thang, csp.Nam from Chuyen_SanPham csp, SanPham sp Where sp.MaSanPham = csp.MaSanPham and MaChuyen ='" + MaChuyen + "' and csp.IsDelete = 0 and csp.IsFinish = 0 and sp.IsDelete =0 Order By csp.STTThucHien ASC";
            dt = dbclass.TruyVan_TraVe_DataTable(Strsql);
            dbclass.loaddataridviewcolorrow(dg, dt);

        }

        public int LayLuyKeTHTheoSTT(string sttChuyenSanPham)
        {
            dt.Clear();
            string sql = "select TOP 1 csp.LuyKeTH from Chuyen_SanPham csp Where csp.STT=" + sttChuyenSanPham;
            try
            {

                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    return int.Parse(dt.Rows[0]["LuyKeTH"].ToString());
                }
                return 0;

            }
            catch (Exception)
            {
                return 0;
            }
        }

        public void LayTTChuyenTheoNgay(string MaChuyen, string TuNgay, string DenNgay)
        {

            dt.Clear();
            string Strsql = "select sp.TenSanPham, csp.SanLuongKeHoach, csp.LuyKeTH, (ROUND(((csp.LuyKeTH/csp.SanLuongKeHoach)*100),0))TyLeThucHien, csp.TimeAdd  from Chuyen_SanPham csp, SanPham sp Where sp.MaSanPham = csp.MaSanPham and MaChuyen ='" + MaChuyen + "' and csp.IsDelete = 0 and csp.IsFinish = 0";
            dt = dbclass.TruyVan_TraVe_DataTable(Strsql);


        }

        public int SuaTTLuykeTH(Chuyen_SanPham chuyen_sampham)
        {
            int kq = 0;

            try
            {
                string sql = "Update Chuyen_SanPham set LuyKeTH ='" + chuyen_sampham.LuyKeTH + "', IsFinish='" + chuyen_sampham.IsFinish + "', LuyKeBTPThoatChuyen='" + chuyen_sampham.LuyKeBTPThoatChuyen + "', IsFinishBTPThoatChuyen='" + chuyen_sampham.IsFinishBTPThoatChuyen + "' where STT ='" + chuyen_sampham.STT + "'";
                kq = dbclass.TruyVan_XuLy(sql);
                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể thay đổi thông tin luỹ kế thực hiện của chuyền_Mặt Hàng dưới CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public Chuyen_SanPham LayLuyKeTHandKeHoachTheoSTT(string STT, DateTime date)
        {
            Chuyen_SanPham chuyenSanPham = null;
            try
            {
               // string sql = "select TOP 1 csp.LuyKeTH, csp.SanLuongKeHoach from Chuyen_SanPham csp Where csp.STT ='" + STT + "' and Thang=" + date.Month + " and Nam =" + date.Year;
                string sql = "select TOP 1 csp.LuyKeTH, csp.SanLuongKeHoach from Chuyen_SanPham csp Where csp.STT ='" + STT + "'";
                var dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    chuyenSanPham = new Chuyen_SanPham();
                    int luyKeThucHien = 0;
                    int.TryParse(row["LuyKeTH"].ToString(), out luyKeThucHien);
                    int sanLuongKeHoach = 0;
                    int.TryParse(row["SanLuongKeHoach"].ToString(), out sanLuongKeHoach);
                    chuyenSanPham.LuyKeTH = luyKeThucHien;
                    chuyenSanPham.SanLuongKeHoach = sanLuongKeHoach;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return chuyenSanPham;
        }

        public int UpdateIsFinishBTPChuyen()
        {
            int result = 0;
            try
            {
                string sql = "update Chuyen_SanPham set IsFinishBTPThoatChuyen=1 where IsFinish=1 and IsFinishBTPThoatChuyen=0";
                result = dbclass.TruyVan_XuLy(sql);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public ModelResult AddListPCC(List<Chuyen_SanPham> listChuyenSanPham, string strListLineId, string floor)
        {
            try
            {
                ModelResult modelResult = new ModelResult();
                if (listChuyenSanPham != null && listChuyenSanPham.Count > 0)
                {
                    var dateNow = DateTime.Now;
                    modelResult.IsSuccsess = true;
                    foreach (var csp in listChuyenSanPham)
                    {
                        try
                        {
                            var line = chuyenDAO.GetLineByName(csp.TenChuyen, strListLineId);
                            if (line != null)
                            {
                                csp.MaChuyen = line.MaChuyen;
                                var product = sanPhamDAO.GetProductByName(floor, csp.TenSanPham);
                                if (product != null)
                                {
                                    csp.MaSanPham = product.MaSanPham;
                                    csp.TimeAdd = dateNow;
                                    var rsAdd = ThemPhanCong(csp);
                                    if (!rsAdd.IsSuccsess)
                                    {
                                        modelResult.IsSuccsess = false;
                                        modelResult.ListError.AddRange(rsAdd.ListError);
                                    }
                                }
                                else
                                {
                                    modelResult.IsSuccsess = false;
                                    modelResult.ListError.Add(new ModelError()
                                    {
                                        ClassName = this.GetType().Name,
                                        MethodName = "AddListPCC",
                                        ErrorContent = "Lỗi: Không tìm thấy thông tin Mặt Hàng có tên Mặt Hàng: " + csp.TenSanPham
                                    });
                                }
                            }
                            else
                            {
                                modelResult.IsSuccsess = false;
                                modelResult.ListError.Add(new ModelError()
                                {
                                    ClassName = this.GetType().Name,
                                    MethodName = "AddListPCC",
                                    ErrorContent = "Lỗi: Không tìm thấy thông tin Chuyền có tên chuyền: " + csp.TenChuyen
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            modelResult.IsSuccsess = false;
                            modelResult.ListError.Add(new ModelError()
                            {
                                ClassName = this.GetType().Name,
                                MethodName = "AddListPCC",
                                ErrorContent = "Lỗi: việc phân công Mặt Hàng " + csp.TenSanPham + " cho chuyền " + csp.TenChuyen + " thất bại. Nguyên nhân: " + ex.Message
                            });
                        }
                    }
                }
                else
                {
                    modelResult.IsSuccsess = false;
                    modelResult.ListError.Add(new ModelError()
                    {
                        ClassName = this.GetType().Name,
                        MethodName = "AddListPCC",
                        ErrorContent = "Lỗi: Không có thông tin phân công."
                    });
                }
                return modelResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Chuyen_SanPham CheckExistPCC(string lineId, string productId, int morth, int year)
        {
            Chuyen_SanPham csp = null;
            try
            {
                string strSQL = "Select * From Chuyen_SanPham Where Thang=" + morth + " and Nam=" + year + " and MaChuyen=" + lineId + " and MaSanPham=" + productId + " and IsFinish=0 and IsDelete=0";
                DataTable dt = dbclass.TruyVan_TraVe_DataTable(strSQL);
                if (dt != null && dt.Rows.Count > 0)
                {
                    csp = new Chuyen_SanPham();
                    DataRow row = dt.Rows[0];
                    csp.STT = row["STT"].ToString();
                    csp.MaChuyen = row["MaChuyen"].ToString();
                    csp.MaSanPham = row["MaSanPham"].ToString();
                    int sanLuongKH = 0;
                    int.TryParse(row["SanLuongKeHoach"].ToString(), out sanLuongKH);
                    csp.SanLuongKeHoach = sanLuongKH;
                    int luyKeTH = 0;
                    int.TryParse(row["LuyKeTH"].ToString(), out luyKeTH);
                    csp.LuyKeTH = luyKeTH;
                    float nangXuatSanXuat = 0;
                    float.TryParse(row["NangXuatSanXuat"].ToString(), out nangXuatSanXuat);
                    csp.NangXuatSanXuat = nangXuatSanXuat;
                    csp.Thang = morth;
                    csp.Nam = year;
                }
            }
            catch (Exception ex)
            { throw ex; }
            return csp;
        }

        public bool MoveQuantityToMorthNow(int type)
        {
            bool result = false;
            try
            {
                BTPDAO btpDAO = new BTPDAO();
                ClusterDAO clusterDAO = new ClusterDAO();
                var ngay = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                var dateNow = DateTime.Now;
                int morthNow = dateNow.Month;
                int yearNow = dateNow.Year;
                var dateOld = dateNow.AddMonths(-1);
                int morthOld = dateOld.Month;
                int yearOld = dateOld.Year;
                string strSQL = "Select * From Chuyen_SanPham Where Thang=" + morthOld + " and Nam=" + yearOld + " and MaChuyen in (" + AccountSuccess.strListChuyenId.Trim() + ") and IsFinish=0 and IsDelete=0";
                List<Chuyen_SanPham> listChuyenSanPham = new List<Chuyen_SanPham>();
                DataTable dtTable = dbclass.TruyVan_TraVe_DataTable(strSQL);
                if (dtTable != null && dtTable.Rows.Count > 0)
                {
                    int sttThucHien = 1;
                    foreach (DataRow row in dtTable.Rows)
                    {
                        Chuyen_SanPham csp = new Chuyen_SanPham();
                        csp.STT = row["STT"].ToString();
                        int.TryParse(row["STTThucHien"].ToString(), out sttThucHien);
                        csp.STTThucHien = sttThucHien;
                        csp.MaChuyen = row["MaChuyen"].ToString();
                        csp.MaSanPham = row["MaSanPham"].ToString();
                        int sanLuongKH = 0;
                        int.TryParse(row["SanLuongKeHoach"].ToString(), out sanLuongKH);
                        csp.SanLuongKeHoach = sanLuongKH;
                        int luyKeTH = 0;
                        int.TryParse(row["LuyKeTH"].ToString(), out luyKeTH);
                        csp.LuyKeTH = luyKeTH;
                        float nangXuatSanXuat = 0;
                        float.TryParse(row["NangXuatSanXuat"].ToString(), out nangXuatSanXuat);
                        csp.NangXuatSanXuat = nangXuatSanXuat;
                        csp.Thang = morthOld;
                        csp.Nam = yearOld;
                        listChuyenSanPham.Add(csp);
                    }
                }
                if (listChuyenSanPham.Count > 0)
                {
                    listChuyenSanPham = listChuyenSanPham.OrderBy(c => c.STTThucHien).ToList();
                    int sttThucHien = 0;
                    foreach (var csp in listChuyenSanPham)
                    {
                        if (type == 1)
                        {
                            csp.IsFinish = 1;
                            csp.IsFinishBTPThoatChuyen = true;
                        //    SuaThongTinPhanCong(csp);
                        }
                        else
                        {
                            sttThucHien++;
                            int sumBTPOfLine = 0;
                            var listBTP = btpDAO.GetListBTPBySTT(csp.STT);
                            if (listBTP != null && listBTP.Count > 0)
                            {
                                sumBTPOfLine = listBTP.Where(c => c.IsEndOfLine).Sum(c => c.BTPNgay);
                            }
                            var listClusterOfLine = clusterDAO.GetCumOfChuyen(int.Parse(csp.MaChuyen));
                            Cluster clusterEndLine = null;
                            if (listClusterOfLine != null && listClusterOfLine.Count > 0)
                            {
                                clusterEndLine = listClusterOfLine.First(c => c.IsEndOfLine);
                            }

                            if (csp.LuyKeTH == 0)
                            {
                                var cspNew = new Chuyen_SanPham();
                                Parse.CopyObject(csp, ref cspNew);
                                cspNew.TimeAdd = dateNow;
                                cspNew.Thang = morthNow;
                                cspNew.Nam = yearNow;
                                cspNew.IsMoveQuantityFromMorthOld = true;
                                cspNew.STTThucHien = sttThucHien;
                                // var resultAdd = ThemPhanCong(cspNew);
                                //if (resultAdd.IsSuccsess)
                                //{
                                //    csp.IsFinish = 1;
                                //    csp.IsFinishBTPThoatChuyen = true;
                                //    SuaThongTinPhanCong(csp);

                                //    string sttPCC = GetSTTChuyenSanPham(cspNew.MaChuyen, cspNew.MaSanPham, cspNew.Thang, cspNew.Nam);
                                //    if (sumBTPOfLine > 0)
                                //    {
                                //        BTP btp = new BTP();
                                //        btp.STTChuyen_SanPham = sttPCC;
                                //        btp.IsEndOfLine = true;
                                //        btp.Ngay = dateNow.Date;
                                //        btp.STT = 1;
                                //        btp.TimeUpdate = dateNow.TimeOfDay;
                                //        btp.BTPNgay = sumBTPOfLine;
                                //        btp.CommandTypeId = 8;
                                //        btp.CumId = clusterEndLine.Id;
                                //      btpDAO.ThemOBJ(btp);
                                //    }
                                //}
                            }
                            else
                            {
                                int soLuongTon = csp.SanLuongKeHoach - csp.LuyKeTH;
                                var cspExist = CheckExistPCC(csp.MaChuyen, csp.MaSanPham, morthNow, yearNow);
                                if (cspExist != null)
                                {
                                    cspExist.SanLuongKeHoach += soLuongTon;
                                    var rsMove = SuaThongTinPhanCong(cspExist);
                                    if (rsMove > 0)
                                    {
                                        csp.SanLuongKeHoach = csp.LuyKeTH;
                                        csp.IsFinish = 1;
                                        csp.IsFinishBTPThoatChuyen = true;
                                     // SuaThongTinPhanCong(csp);

                                        int btpOnLine = sumBTPOfLine - csp.LuyKeTH;
                                        if (btpOnLine > 0)
                                        {
                                            BTP btp = new BTP();
                                            btp.STTChuyen_SanPham = cspExist.STT;
                                            btp.IsEndOfLine = true;
                                            btp.Ngay = ngay;
                                            btp.STT = 1;
                                            btp.TimeUpdate = dateNow.TimeOfDay;
                                            btp.BTPNgay = btpOnLine;
                                            btp.CommandTypeId = 8;
                                            btp.CumId = clusterEndLine.Id;
                                          //  btpDAO.ThemOBJ(btp);
                                        }
                                    }
                                }
                                else
                                {
                                    var cspNew = new Chuyen_SanPham();
                                    Parse.CopyObject(csp, ref cspNew);
                                    cspNew.TimeAdd = dateNow;
                                    cspNew.Thang = morthNow;
                                    cspNew.Nam = yearNow;
                                    cspNew.SanLuongKeHoach = soLuongTon;
                                    cspNew.IsMoveQuantityFromMorthOld = true;
                                    cspNew.LuyKeTH = 0;
                                    csp.STTThucHien = sttThucHien;
                                    //var rsMove = ThemPhanCong(cspNew);
                                    //if (rsMove.IsSuccsess)
                                    //{
                                    //    csp.SanLuongKeHoach = csp.LuyKeTH;
                                    //    csp.IsFinish = 1;
                                    //    csp.IsFinishBTPThoatChuyen = true;
                                    //    SuaThongTinPhanCong(csp);

                                    //    string sttPCC = GetSTTChuyenSanPham(cspNew.MaChuyen, cspNew.MaSanPham, cspNew.Thang, cspNew.Nam);
                                    //    int btpOnLine = sumBTPOfLine - csp.LuyKeTH;
                                    //    if (btpOnLine > 0)
                                    //    {
                                    //        BTP btp = new BTP();
                                    //        btp.STTChuyen_SanPham = sttPCC;
                                    //        btp.IsEndOfLine = true;
                                    //        btp.Ngay = dateNow.Date;
                                    //        btp.STT = 1;
                                    //        btp.TimeUpdate = dateNow.TimeOfDay;
                                    //        btp.BTPNgay = btpOnLine;
                                    //        btp.CommandTypeId = 8;
                                    //        btp.CumId = clusterEndLine.Id;
                                    //        btpDAO.ThemOBJ(btp);
                                    //    }
                                    //}
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
            return result;
        }

        public Chuyen_SanPham GetPCOldOfLineBySTT(string lineId, int sttChuyenSanPhamNow)
        {
            try
            {
                Chuyen_SanPham csp = null;
                string strSQL = "Select  * From Chuyen_SanPham Where IsDelete=0 and MaChuyen=" + lineId + " ORDER BY stt ";
                DataTable dtTable = dbclass.TruyVan_TraVe_DataTable(strSQL);
                if (dtTable != null && dtTable.Rows.Count > 0)
                {
                    bool isGetSTTOld = false;
                    foreach (DataRow row in dtTable.Rows)
                    {
                        int sttChuyenSamPham = 0;
                        if (row["STT"] != null)
                            int.TryParse(row["STT"].ToString(), out sttChuyenSamPham);
                         if (sttChuyenSamPham == sttChuyenSanPhamNow)
                            isGetSTTOld = true;

                        if (isGetSTTOld)
                        {
                            csp = new Chuyen_SanPham();
                            csp.STT = sttChuyenSamPham.ToString();
                            break;
                        }
                       
                    }
                }
                return csp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Chuyen_SanPham GetChuyenSanPham(DateTime Ngay, int sttChuyenSanPham, int lineId)
        {
            try
            {
                var date = Ngay.Day + "/" + Ngay.Month + "/" + Ngay.Year;
                Chuyen_SanPham chuyenSanPham = null;
                string strSQLSelect = "Select csp.Thang, csp.Nam, csp.MaChuyen, csp.MaSanPham, csp.SanLuongKeHoach, csp.NangXuatSanXuat, (select SUM(ThucHienNgay-ThucHienNgayGiam) from NangXuat where STTChuyen_SanPham=" + sttChuyenSanPham + " and Ngay <= '" + date + "') as LuyKeTH,";
                strSQLSelect += "(select SUM(BTPThoatChuyenNgay-BTPThoatChuyenNgayGiam) from NangXuat where STTChuyen_SanPham=" + sttChuyenSanPham + " and Ngay <= '" + date + "') as LuyKeBTPThoatChuyen, csp.IsFinish, csp.IsFinishNow, csp.IsFinishBTPThoatChuyen, c.Sound,";
                strSQLSelect += "(SELECT SUM(BTPNgay) FROM BTP b WHERE b.STTChuyen_SanPham = '" + sttChuyenSanPham + "' and b.IsEndOfLine=1 and b.CommandTypeId=" + (int)eCommandRecive.BTPIncrease + ") LuyKeBTPTang, ";
                strSQLSelect += "(SELECT SUM(BTPNgay) FROM BTP b WHERE b.STTChuyen_SanPham = '" + sttChuyenSanPham + "' and b.IsEndOfLine=1 and b.CommandTypeId=" + (int)eCommandRecive.BTPReduce + ") LuyKeBTPGiam, ";
                strSQLSelect += "(SELECT COUNT(csp.STT) FROM Chuyen_SanPham csp WHERE csp.MaChuyen = '" + lineId + "' and csp.IsFinish = 0 and csp.IsDelete = 0) SoPhanCong  From Chuyen_SanPham csp, Chuyen c Where csp.STT=" + sttChuyenSanPham + " and csp.MaChuyen=c.MaChuyen";
                dtChuyenSanPham.Clear();
                dtChuyenSanPham = dbclass.TruyVan_TraVe_DataTable(strSQLSelect);
                if (dtChuyenSanPham != null && dtChuyenSanPham.Rows.Count > 0)
                {
                    chuyenSanPham = new Chuyen_SanPham();
                    DataRow row = dtChuyenSanPham.Rows[0];
                    chuyenSanPham.MaChuyen = row["MaChuyen"].ToString();
                    chuyenSanPham.MaSanPham = row["MaSanPham"].ToString();
                    int sanLuongKeHoach = 0;
                    int.TryParse(row["SanLuongKeHoach"].ToString(), out sanLuongKeHoach);
                    chuyenSanPham.SanLuongKeHoach = sanLuongKeHoach;
                    float nangSuatSanXuat = 0;
                    float.TryParse(row["NangXuatSanXuat"].ToString(), out nangSuatSanXuat);
                    chuyenSanPham.NangXuatSanXuat = nangSuatSanXuat;
                    int luyKeTH = 0;
                    int.TryParse(row["LuyKeTH"].ToString(), out luyKeTH);
                    chuyenSanPham.LuyKeTH = luyKeTH;
                    int isFinish = 0;
                    int.TryParse(row["IsFinish"].ToString(), out isFinish);
                    chuyenSanPham.IsFinish = isFinish;
                    int isFinishNow = 0;
                    int.TryParse(row["IsFinishNow"].ToString(), out isFinishNow);
                    chuyenSanPham.IsFinishNow = isFinishNow;
                    int luyKeBTPThoatChuyen = 0;
                    int.TryParse(row["LuyKeBTPThoatChuyen"].ToString(), out luyKeBTPThoatChuyen);
                    chuyenSanPham.LuyKeBTPThoatChuyen = luyKeBTPThoatChuyen;
                    bool isFinishBTPThoatChuyen = false;
                    bool.TryParse(row["IsFinishBTPThoatChuyen"].ToString(), out isFinishBTPThoatChuyen);
                    chuyenSanPham.IsFinishBTPThoatChuyen = isFinishBTPThoatChuyen;
                    chuyenSanPham.SoundChuyen = row["Sound"].ToString();
                    int luyKeBTP = 0;
                    int luyKeBTPTang = 0;
                    int.TryParse(row["LuyKeBTPTang"].ToString(), out luyKeBTPTang);
                    int luyKeBTPGiam = 0;
                    int.TryParse(row["LuyKeBTPGiam"].ToString(), out luyKeBTPGiam);
                    luyKeBTP = luyKeBTPTang - luyKeBTPGiam;
                    chuyenSanPham.LuyKeBTP = luyKeBTP;
                    int soPhanCong = 0;
                    int.TryParse(row["SoPhanCong"].ToString(), out soPhanCong);
                    chuyenSanPham.SoPhanCong = soPhanCong;
                }
                return chuyenSanPham;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool UpdateQuantityTotalOfPCC(int sttChuyenSanPham, int quantityTotal, int tpThoatChuyen, int isFinish, bool isFinishThoatChuyen)
        {
            try
            {
                bool result = false;
                string sql = "Update Chuyen_SanPham set LuyKeTH=" + quantityTotal + ",LuyKeBTPThoatChuyen=" + tpThoatChuyen + ", IsFinish=" + isFinish + ", IsFinishBTPThoatChuyen='" + isFinishThoatChuyen + "' where STT=" + sttChuyenSanPham;
                var intRS = dbclass.TruyVan_XuLy(sql);
                if (intRS > 0)
                    result = true;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
