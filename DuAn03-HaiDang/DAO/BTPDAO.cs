using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.POJO;
using DuAn03_HaiDang.Model;
using DuAn03_HaiDang.Enum;
using PMS.Business.Enum;
namespace DuAn03_HaiDang.DAO
{
    public class BTPDAO
    {
        private DataTable dt;
        private Chuyen_SanPhamDAO chuyenSanPhamDAO;
        private NangXuatDAO nangSuatDAO;
        private ThanhPhamDAO thanhPhamDAO;
        private DenDAO denDAO;
        public BTPDAO()
        {
            this.dt = new DataTable();
            this.chuyenSanPhamDAO = new Chuyen_SanPhamDAO();
            this.nangSuatDAO = new NangXuatDAO();
            this.thanhPhamDAO = new ThanhPhamDAO();
            this.denDAO = new DenDAO();
        }
        public int ThemOBJ(BTP obj)
        {
            int kq = -1;
            try
            {

                string sql = "insert into BTP(Ngay, STTChuyen_SanPham, STT, BTPNgay, TimeUpdate, CumId, CommandTypeId, IsEndOfLine) values('" + obj.Ngay + "','" + obj.STTChuyen_SanPham + "', '" + obj.STT + "', '" + obj.BTPNgay + "', '" + obj.TimeUpdate + "', " + obj.CumId + ", " + obj.CommandTypeId + ", '" + obj.IsEndOfLine + "')";
                kq = dbclass.TruyVan_XuLy(sql);

                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể lưu thông tin bán thành phẩm của chuyền vào CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public int SuaThongTinOBJ(BTP obj)
        {
            int kq = -1;
            try
            {

                string sql = "update BTP set BTPNgay = '" + obj.BTPNgay + "', TimeUpdate = '" + obj.TimeUpdate + "' where Ngay ='" + obj.Ngay + "' and STTChuyen_SanPham ='" + obj.STTChuyen_SanPham + "' and STT=" + obj.STT + " and CumId=" + obj.CumId + " and CommandTypeId=" + obj.CommandTypeId;
                kq = dbclass.TruyVan_XuLy(sql);

                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể thay đổi thông tin bán thành phẩm của chuyền dưới CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public DataTable DSOBJ(string Ngay, string STTChuyen_SanPham)
        {
            dt.Clear();
            string sql = "select  STTChuyen_SanPham, STT, CumId, BTPNgay, TimeUpdate from BTP where Ngay = '" + Ngay + "' and STTChuyen_SanPham = '" + STTChuyen_SanPham + "'";

            try
            {
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                return dt;
            }
            catch (Exception)
            {

                return dt;
            }
        }

        public void LoadOBJToDataGirdview(DataGridView dg, string Ngay, string STTChuyen_SanPham)
        {
            dt.Clear();
            dt = DSOBJ(Ngay, STTChuyen_SanPham);
            dbclass.loaddataridviewcolorrow(dg, dt);
        }

        public bool TimKiemOBJ(string Ngay, string STTChuyen_SanPham, string STT)
        {
            dt.Clear();
            bool result = false;
            string sql = "select * from BTP where Ngay = '" + Ngay + "' and STTChuyen_SanPham = '" + STTChuyen_SanPham + "' and STT = '" + STT + "'";
            try
            {

                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    result = true;
                }
                return result;
            }
            catch (Exception)
            {

                return result;
            }
        }

        public int XoaOBJ(string STTChuyen_SanPham)
        {
            int kq = 0;

            try
            {
                string sql = "delete from BTP where STTChuyen_SanPham ='" + STTChuyen_SanPham + "'";
                kq = dbclass.TruyVan_XuLy(sql);
                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể xoá thông tin bán thành phẩm dưới CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public List<BTP> GetListBTPBySTT(string sttChuyen_SanPham)
        {
            List<BTP> listBTP = null;
            try
            {
                dt.Clear();
                string sql = "select STT, BTPNgay, Ngay, IsEndOfLine, CommandTypeId, CumId from BTP where IsBTP_PB_HC = 0 and STTChuyen_SanPham = '" + sttChuyen_SanPham + "'";
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    listBTP = new List<BTP>();
                    foreach (DataRow row in dt.Rows)
                    {
                        BTP btp = new BTP();
                        int btpNgay = 0;
                        int.TryParse(row["BTPNgay"].ToString(), out btpNgay);
                        btp.BTPNgay = btpNgay;
                        btp.Ngay = row["Ngay"].ToString();
                        bool isEndOfLine = false;
                        bool.TryParse(row["IsEndOfLine"].ToString(), out isEndOfLine);
                        int commandTypeId = 0;
                        int.TryParse(row["CommandTypeId"].ToString(), out commandTypeId);
                        int clusterId = 0;
                        int.TryParse(row["CumId"].ToString(), out clusterId);
                        btp.CommandTypeId = commandTypeId;
                        btp.CumId = clusterId;
                        btp.IsEndOfLine = isEndOfLine;
                        listBTP.Add(btp);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return listBTP;
        }

        public ModelInfoChuyenOfKANBAN GetDataChuyen(string chuyenId, int tableType, ChuyenSanPham ChuyenSanPham)
        {
            ModelInfoChuyenOfKANBAN data = null;
            try
            {
                if (ChuyenSanPham == null)
                {
                    var listChuyenSanPham = chuyenSanPhamDAO.GetListChuyenSanPham(chuyenId, false);
                    if (listChuyenSanPham != null)
                        ChuyenSanPham = listChuyenSanPham.FirstOrDefault();
                }
                if (ChuyenSanPham != null)
                {
                    int btpTrenChuyen = 0;
                    int dinhMucNgay = 0;
                    var ngay = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;

                    var nangSuat = nangSuatDAO.TTNangXuatTrongNgay(ngay, ChuyenSanPham.STT);
                    if (nangSuat != null)
                    {
                        btpTrenChuyen = nangSuat.BTPTrenChuyen;
                        dinhMucNgay = (int)nangSuat.DinhMucNgay;
                    }
                    btpTrenChuyen = btpTrenChuyen < 0 ? 0 : btpTrenChuyen;
                    int laoDongChuyen = 0;
                    var thanhPham = thanhPhamDAO.GetThanhPhamByNgayAndSTT(ngay, ChuyenSanPham.STT);
                    if (thanhPham != null)
                        laoDongChuyen = thanhPham.LaoDongChuyen;
                    var listBTP = GetListBTPBySTT(ChuyenSanPham.STT);
                    int btpGiaoChuyenNgay = 0;
                    int luyKeBTP = 0;
                    if (listBTP != null && listBTP.Count > 0)
                    {
                        int btpGiaoChuyenNgayTang = listBTP.Where(c => c.Ngay == ngay && c.IsEndOfLine == true && c.CommandTypeId == (int)eCommandRecive.BTPIncrease).Sum(c => c.BTPNgay);
                        int btpGiaoChuyenNgayGiam = listBTP.Where(c => c.Ngay == ngay && c.IsEndOfLine == true && c.CommandTypeId == (int)eCommandRecive.BTPReduce).Sum(c => c.BTPNgay);
                        btpGiaoChuyenNgay = btpGiaoChuyenNgayTang - btpGiaoChuyenNgayGiam;
                        int luyKeBTPTang = listBTP.Where(c => c.IsEndOfLine && c.CommandTypeId == (int)eCommandRecive.BTPIncrease).Sum(c => c.BTPNgay);
                        int luyKeBTPGiam = listBTP.Where(c => c.IsEndOfLine && c.CommandTypeId == (int)eCommandRecive.BTPReduce).Sum(c => c.BTPNgay);
                        luyKeBTP = luyKeBTPTang - luyKeBTPGiam;
                    }
                    int btpBinhQuan = laoDongChuyen < 0 || btpTrenChuyen < 0 ? 0 : (int)(Math.Ceiling((double)btpTrenChuyen / laoDongChuyen));
                    //  int tyLeDenThucTe = dinhMucNgay < 0 ? 0 : (btpTrenChuyen * 100) / dinhMucNgay;
                    //  string colorDen = denDAO.GetColorDen(ChuyenSanPham.IdDen, tableType, tyLeDenThucTe);
                    int von = btpTrenChuyen > 0 && laoDongChuyen > 0 ? (int)(Math.Ceiling((double)btpTrenChuyen / laoDongChuyen)) : 0;
                    string colorDen = denDAO.GetColorDen(ChuyenSanPham.IdDen, tableType, von);
                    data = new ModelInfoChuyenOfKANBAN()
                    {
                        chuyenId = int.Parse(ChuyenSanPham.MaChuyen),
                        maHang = ChuyenSanPham.TenSanPham,
                        btpGiaoChuyenNgay = btpGiaoChuyenNgay.ToString(),
                        luyKeBTP = luyKeBTP.ToString(),
                        btpBinhQuanBTPTrenNgay = ((btpBinhQuan > 0 ? btpBinhQuan.ToString() : "0") + " / " + (btpTrenChuyen > 0 ? btpTrenChuyen.ToString() : "0")),
                        btpTrenChuyen = btpTrenChuyen.ToString(),
                        tinhTrangBTP = colorDen
                    };
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
    }
}
