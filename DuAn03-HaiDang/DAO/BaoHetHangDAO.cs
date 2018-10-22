using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DuAn03_HaiDang.DATAACCESS;
using System.Windows.Forms;
using DuAn03_HaiDang.POJO;

namespace DuAn03_HaiDang.DAO
{
    class BaoHetHangDAO
    {
        public DataTable DSOBJ()
        {
            DataTable dt = new DataTable();
            string sql = "select STT, SoSanPhamConLai, SoLanBao from BaoHetHang";
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

        public int ThemOBJ(BaoHetHang obj)
        {
            int kq = 0;
            try
            {

                string sql = "insert into BaoHetHang (SoSanPhamConLai, SoLanBao) values(" + obj.SoLuongCon +"," + obj.SoLanBao + ")";
                kq = dbclass.TruyVan_XuLy(sql);

                return kq;
            }
            catch (Exception)
            {
                
                return kq;
            }
        }

        public int SuaThongTinOBJ(BaoHetHang obj)
        {
            int kq = 0;
            try
            {

                string sql = "update BaoHetHang set SoSanPhamConLai = " + obj.SoLuongCon + ", SoLanBao =" + obj.SoLanBao + " where STT ='" + obj.STT + "'";
                kq = dbclass.TruyVan_XuLy(sql);

                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể thay đổi thông tin mặt hàng dưới CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public int XoaOBJ(int STT)
        {
            int kq = 0;

            try
            {
                string sql = "delete from BaoHetHang where STT ='" + STT + "'";
                kq = dbclass.TruyVan_XuLy(sql);
                return kq;
            }
            catch (Exception)
            {
                
                return kq;
            }
        }

       

        public void LoadOBJToDataGirdview(DataGridView dg)
        {

            DataTable dt = new DataTable();
            string Strsql = "select STT, SoSanPhamConLai, SoLanBao from BaoHetHang";
            dt = dbclass.TruyVan_TraVe_DataTable(Strsql);
            dbclass.loaddataridviewcolorrow(dg, dt);
        }
    }
}
