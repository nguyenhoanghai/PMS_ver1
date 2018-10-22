using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.POJO;
using QuanLyNangSuat.Model;
namespace DuAn03_HaiDang.DAO
{
    class ThanhPhamDAO
    {
        DataTable dt;

        public ThanhPhamDAO() 
        {
            this.dt = new DataTable();
        }

        public DataTable DSOBJ()
        {
            dt.Clear();
            string sql = "select * from ThanhPham Where IsDeleted=0";
            try
            {

                dt = dbclass.TruyVan_TraVe_DataTable(sql);

                return dt;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: không thể lấy được thông tin thành phẩm từ CSDL.", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return dt;
            }
        }

        public int ThemOBJ(ThanhPham obj)
        {
            int kq = 0;
            try
            {

                string sql = "insert into ThanhPham(Ngay, STTChuyen_SanPham, NangXuatLaoDong, LaoDongChuyen) values('" + obj.Ngay + "','" + obj.STTChuyen_SanPham + "','" + obj.NangXuatLaoDong + "','"+obj.LaoDongChuyen+"')";
                kq = dbclass.TruyVan_XuLy(sql);

                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể lưu thông tin thành phẩm của chuyền vào CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public int SuaThongTinOBJ(ThanhPham obj)
        {
            int kq = 0;
            try
            {

                string sql = "update ThanhPham set NangXuatLaoDong = '" + obj.NangXuatLaoDong + "', LaoDongChuyen ='" + obj.LaoDongChuyen + "' where Ngay ='" + obj.Ngay + "' and STTChuyen_SanPham ='" + obj.STTChuyen_SanPham + "'";
                kq = dbclass.TruyVan_XuLy(sql);

                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể thay đổi thông tin thành phẩm của chuyền dưới CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public int XoaOBJ(string sttLineProduct, DateTime date)
        {
            int kq = 0;
            try
            {
                var dateTimeNow = DateTime.Now;
                List<string> listQuerySQL = new List<string>();
                listQuerySQL.Add("update ThanhPham set IsDeleted=1, DeletedDate='"+dateTimeNow+"' where STTChuyen_SanPham ='" + sttLineProduct + "' and Ngay='" + date + "'");
                listQuerySQL.Add("update BTP set IsDeleted=1, DeletedDate='" + dateTimeNow + "' where STTChuyen_SanPham ='" + sttLineProduct + "' and Ngay='" + date + "'");
                listQuerySQL.Add("update NangXuat set IsDeleted=1, DeletedDate='" + dateTimeNow + "' where STTChuyen_SanPham ='" + sttLineProduct + "' and Ngay='" + date + "'");
                listQuerySQL.Add("update NangSuat_Cum set IsDeleted=1, DeletedDate='" + dateTimeNow + "' where STTChuyen_SanPham ='" + sttLineProduct + "' and Ngay='" + date + "'");
                listQuerySQL.Add("update NangSuat_CumLoi set IsDeleted=1, DeletedDate='" + dateTimeNow + "' where STTChuyenSanPham ='" + sttLineProduct + "' and Ngay='" + date + "'");
                kq = dbclass.ExecuteSqlTransaction(listQuerySQL);
                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể xoá thông tin thành phẩm dưới CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public bool TimKiemOBJ(string Ngay, string STTChuyen_SanPham)
        {
            dt.Clear();
            bool result = false;
            string sql = "select * from ThanhPham where Ngay = '" + Ngay + "' and STTChuyen_SanPham = '" + STTChuyen_SanPham + "' and IsDeleted=0";
            try
            {

                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if(dt.Rows.Count > 0)
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

        public void LoadOBJToDataGirdview(DataGridView dg, string MaChuyen, string Ngay)
        {
            dt.Clear();
            string Strsql = "select sp.TenSanPham, tp.NangXuatLaoDong, tp.LaoDongChuyen from ThanhPham tp,SanPham sp, Chuyen_SanPham csp where tp.Ngay ='" + Ngay + "' and tp.STTChuyen_SanPham = csp.STT and csp.MaChuyen = '" + MaChuyen + "' and csp.MaSanPham = sp.MaSanPham and csp.IsDelete = 0 and csp.IsFinish = 0 and sp.IsDelete =0 and tp.IsDeleted=0";
            dt = dbclass.TruyVan_TraVe_DataTable(Strsql);
            dbclass.loaddataridviewcolorrow(dg, dt);
        }

        public List<ModelDailyWorkerInformation> GetDailyWorkerInformation(int lineId, DateTime date)
        {
            List<ModelDailyWorkerInformation> listInformation = new List<ModelDailyWorkerInformation>();
            try
            {
                string dateNow = date.Day + "/" + date.Month + "/" + date.Year;
                string strSql = "select tp.Id, sp.MaSanPham, sp.TenSanPham, tp.NangXuatLaoDong, tp.LaoDongChuyen, tp.STTChuyen_SanPham from ThanhPham tp,SanPham sp, Chuyen_SanPham csp where tp.IsDeleted=0 and tp.Ngay ='" + dateNow + "' and tp.STTChuyen_SanPham = csp.STT and csp.MaChuyen = '" + lineId + "' and csp.MaSanPham = sp.MaSanPham and csp.IsDelete = 0 and csp.IsFinish = 0 and sp.IsDelete =0";
                var dt = dbclass.TruyVan_TraVe_DataTable(strSql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        int productId = 0;
                        int.TryParse(row["MaSanPham"].ToString(), out productId);
                        float productivityWorker = 0;
                        float.TryParse(row["NangXuatLaoDong"].ToString(), out productivityWorker);
                        int countWorker = 0;
                        int.TryParse(row["LaoDongChuyen"].ToString(), out countWorker);
                        int sttLineProduct = 0;
                        int.TryParse(row["STTChuyen_SanPham"].ToString(), out sttLineProduct);
                        int id = 0;
                        int.TryParse(row["Id"].ToString(), out id);
                        listInformation.Add(new ModelDailyWorkerInformation() { 
                            ProductId = productId,
                            ProductName = row["TenSanPham"].ToString(),
                            ProductivityWorker = productivityWorker,
                            CountWorker = countWorker,
                            STTLineProduct = sttLineProduct,
                            Id = id
                        });
                    }
                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            return listInformation;   
        }

        public ThanhPham GetThanhPhamByNgayAndSTT(string Ngay, string STTChuyen_SanPham)
        {
            ThanhPham thanhPham = null;
            try
            {
                dt.Clear();                
                string sql = "select * from ThanhPham where Ngay = '" + Ngay + "' and STTChuyen_SanPham = '" + STTChuyen_SanPham + "' and IsDeleted=0";
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt!=null && dt.Rows.Count > 0)
                {
                    thanhPham = new ThanhPham();
                    DataRow rowFirst = dt.Rows[0];
                    int laoDong = 0;
                    int.TryParse(rowFirst["LaoDongChuyen"].ToString(), out laoDong);
                    float nangSuatLaoDong = 0;
                    float.TryParse(rowFirst["NangXuatLaoDong"].ToString(), out nangSuatLaoDong);
                    thanhPham.LaoDongChuyen = laoDong;
                    thanhPham.NangXuatLaoDong = nangSuatLaoDong;
                }                
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            return thanhPham;
        }

        public ThanhPham GetTPByWorkDayOld(string STTChuyen_SanPham)
        {
            ThanhPham thanhPham = null;
            try
            {
                dt.Clear();
                string sql = "select * from ThanhPham where STTChuyen_SanPham = '" + STTChuyen_SanPham + "' and IsDeleted=0 Order by  Ngay desc";
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    thanhPham = new ThanhPham();
                    DataRow rowFirst = dt.Rows[0];
                    int laoDong = 0;
                    int.TryParse(rowFirst["LaoDongChuyen"].ToString(), out laoDong);
                    thanhPham.LaoDongChuyen = laoDong;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return thanhPham;
        }
        
    }
}
