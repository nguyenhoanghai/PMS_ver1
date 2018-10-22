using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.POJO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DuAn03_HaiDang.DAO
{
    public class SanLuongCatDAO
    {
        public DataTable DSOBJ(int IdToCat, int IdSanPham, DateTime NgayNapSL)
        {
            DataTable dt = new DataTable();
            string sql = "";
            sql = "select Id, SanLuong, ThoiGianNapSL from SanLuongCat where IsDeleted =0 and IdToCat="+IdToCat+" and IdSanPham="+IdSanPham+" and NgayNapSL='"+NgayNapSL+"'";
            try
            {
                if (sql != "")
                {
                    dt = dbclass.TruyVan_TraVe_DataTable(sql);
                }

                return dt;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: không thể lấy được danh sách đối tượng từ CSDL.", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return dt;
            }
        }

        public int ThemOBJ(SanLuongCat obj)
        {
            int kq = 0;
            try
            {

                string sql = "insert into SanLuongCat (IdToCat, IdSanPham, SanLuong, ThoiGianNapSL, NgayNapSL) values(" + obj.IdToCat + "," + obj.IdSanPham + ","+obj.SanLuong+",'"+obj.ThoiGianNapSL+"', '"+obj.NgayNapSL+"')";
                kq = dbclass.TruyVan_XuLy(sql);

                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể thêm đối tượng mới vào CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public int SuaThongTinOBJ(SanLuongCat obj)
        {
            int kq = 0;
            try
            {

                string sql = "update SanLuongCat set SanLuongKSCTang = " + obj.SanLuong + ", ThoiGianNapSL =" + obj.ThoiGianNapSL + " where IdToCat ='" + obj.Id + "'";
                kq = dbclass.TruyVan_XuLy(sql);

                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể thay đổi thông tin tổ cắt dưới CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public int XoaOBJ(int IdObj)
        {
            int kq = 0;

            try
            {
                string sql = "update SanLuongCat set IsDeleted =1 where Id =" + IdObj ;
                kq = dbclass.TruyVan_XuLy(sql);
                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể xoá tổ cắt dưới CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }



        public void LoadOBJToDataGirdview(DataGridView dg, int IdToCat, int IdSanPham, DateTime NgayNapSL)
        {

            DataTable dt = new DataTable();
            string Strsql = "";
            Strsql = "select Id, SanLuong, ThoiGianNapSL from SanLuongCat where IsDeleted =0 and IdToCat=" + IdToCat + " and IdSanPham=" + IdSanPham + " and NgayNapSL='" + NgayNapSL + "' order by Id desc";
            if (Strsql != "")
            {
                dt = dbclass.TruyVan_TraVe_DataTable(Strsql);
                dbclass.loaddataridviewcolorrow(dg, dt);
            }
            else
            {
                MessageBox.Show("Lỗi: không lấy được danh sách đối tượng!!!");
            }
        }
    }
}
