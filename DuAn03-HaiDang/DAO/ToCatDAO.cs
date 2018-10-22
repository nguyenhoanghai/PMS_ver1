using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.POJO;

namespace DuAn03_HaiDang.DAO
{
    public class ToCatDAO
    {
        public DataTable DSOBJ(int Idfloor, bool isall)
        {
            DataTable dt = new DataTable();
            string sql = "";            
            if (isall)
            {
                sql = "select IdToCat, TenToCat, DinhNghia, IdFloor from ToCat where IsDeleted =0 order by IdToCat desc";
            }
            else
            {
                sql = "select IdToCat, TenToCat, DinhNghia, IdFloor from ToCat where IsDeleted =0 and IdFloor=" + Idfloor + " order by IdToCat desc";
            }
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
                MessageBox.Show("Lỗi: không thể lấy được danh sách các tổ cắt từ CSDL.", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return dt;
            }
        }
        
        public int ThemOBJ(ToCat obj)
        {
            int kq = 0;
            try
            {

                string sql = "insert into ToCat (TenToCat, DinhNghia, IdFloor) values(N'" + obj.TenToCat + "',N'" + obj.DinhNghia + "', "+obj.IdFloor+")";
                kq = dbclass.TruyVan_XuLy(sql);

                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể thêm tổ cắt mới vào CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public int SuaThongTinOBJ(ToCat obj)
        {
            int kq = 0;
            try
            {

                string sql = "update ToCat set TenToCat = N'" + obj.TenToCat + "', DinhNghia =N'" + obj.DinhNghia + "', IdFloor="+obj.IdFloor+" where IdToCat ='" + obj.IdToCat + "'";
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
                string sql = "update ToCat set IsDeleted =1 where IdToCat =" + IdObj;
                kq = dbclass.TruyVan_XuLy(sql);
                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể xoá tổ cắt dưới CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public void LoadOBJToDataGirdview(DataGridView dg, int Idfloor, bool isall)
        {

            DataTable dt = new DataTable();
            string Strsql = "";
            
            if (isall)
            {
                Strsql = "select IdToCat, TenToCat, DinhNghia, IdFloor from ToCat Where IsDeleted =0 order by IdToCat desc";
            }
            else
            {
                Strsql = "select IdToCat, TenToCat, DinhNghia, IdFloor from ToCat Where IsDeleted =0 and IdFloor=" + Idfloor + "order by IdToCat desc";
            }
            if (Strsql != "")
            {
                dt = dbclass.TruyVan_TraVe_DataTable(Strsql);
                dbclass.loaddataridviewcolorrow(dg, dt);
            }
            else
            {
                MessageBox.Show("Lỗi: không lấy được danh sách tổ cắt!!!");
            }
        }

        
    }
}
