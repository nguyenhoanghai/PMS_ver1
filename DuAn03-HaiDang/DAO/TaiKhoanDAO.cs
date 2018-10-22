using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuAn03_HaiDang.POJO;
using DuAn03_HaiDang.DATAACCESS;
using System.Data;
using System.Windows.Forms;
namespace DuAn03_HaiDang.DAO
{
    class TaiKhoanDAO
    {
        public DataTable DSOBJ(string floor)
        {
            DataTable dt = new DataTable();
            string sql = "select * from TaiKhoan Where Floor ='"+floor+"'";
            try
            {

                dt = dbclass.TruyVan_TraVe_DataTable(sql);

                return dt;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: không thể lấy được danh sách các tài khoản từ CSDL.", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return dt;
            }
        }

        public int ThemOBJ(TaiKhoan obj)
        {
            int kq = 0;
            try
            {

                string sql = "insert into TaiKhoan values(N'" + obj.TenTaiKhoan + "',N'" + obj.MatKhau + "',N'"+obj.TenChuTK+"',"+obj.ThanhPhan+","+obj.BTP+","+obj.ThaoTac+",'"+obj.Floor+"')";
                kq = dbclass.TruyVan_XuLy(sql);

                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể thêm tài khoản mới vào CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public int SuaThongTinOBJ(TaiKhoan obj)
        {
            int kq = 0;
            try
            {

                string sql = "update TaiKhoan set MatKhau = N'" + obj.MatKhau + "', ThanhPham =" + obj.ThanhPhan + ", BTP =" + obj.BTP+ ", ThaoTac="+obj.ThaoTac+" where TaiKhoan ='" + obj.TenTaiKhoan + "'";
                kq = dbclass.TruyVan_XuLy(sql);

                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể thay đổi thông tin tài khoản dưới CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }
        public int ThayDoiMatKhau(string TenTaiKhoan, string MatKhau)
        {
            int kq = 0;
            try
            {

                string sql = "update TaiKhoan set MatKhau = N'" + MatKhau + "' where TaiKhoan ='" + TenTaiKhoan + "'";
                kq = dbclass.TruyVan_XuLy(sql);

                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể thay đổi mật khẩu của tài khoản", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }
        public int XoaOBJ(string IdObj)
        {
            int kq = 0;

            try
            {


                string sql = "delete from TaiKhoan where TaiKhoan ='" + IdObj + "'";
                kq = dbclass.TruyVan_XuLy(sql);
                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể xoá tài khoản dưới CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public DataTable TimKiemOBJ(string content)
        {
            DataTable dt = new DataTable();
            string sql = "select * from TaiKhoan where TaiKhoan like N'" + content + "' or TenChuTK like N'" + content + "'";
            try
            {

                dt = dbclass.TruyVan_TraVe_DataTable(sql);

                return dt;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: không thể thực hiện tìm kiếm thông tin tài khoản trên CSDL.", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return dt;
            }
        }

        public void LoadOBJToDataGirdview(DataGridView dg, string MaChuyen, string Floor)
        {

            DataTable dt = new DataTable();
            string Strsql = "select tk.TaiKhoan, tk.MatKhau, tk.TenChuTK, tk.ThanhPham, tk.BTP   from TaiKhoan tk, Floor f Where tk.MaChuyen='"+MaChuyen+"' and tk.Floor = f.IdFloor and f.IdFloor ='"+Floor+"' ";
            dt = dbclass.TruyVan_TraVe_DataTable(Strsql);
            if (dt.Rows.Count > 0)
            {
                dbclass.loaddataridviewcolorrow(dg, dt);
            }
        }
        public TaiKhoan FindAccount(string strUser, string strPass)
        {
            TaiKhoan tkAccount = new TaiKhoan();
            try
            {
                DataTable dtAccount = new DataTable();
                
                string strSql = "select tk.TaiKhoan, tk.TenChuTK, tk.ThanhPham, tk.BTP, tk.ThaoTac, f.IdFloor, tk.ListChuyenId from TaiKhoan tk, Floor f where TaiKhoan ='" + strUser + "' and MatKhau='" + strPass + "' and tk.Floor= f.IdFloor";
                dtAccount = dbclass.TruyVan_TraVe_DataTable(strSql);
                if (dtAccount.Rows.Count > 0)
                {
                    DataRow row = dtAccount.Rows[0];
                    tkAccount.TenTaiKhoan = row["TaiKhoan"].ToString();
                    tkAccount.TenChuTK = row["TenChuTK"].ToString();
                    tkAccount.ThanhPhan = int.Parse(row["ThanhPham"].ToString());
                    tkAccount.BTP = int.Parse(row["BTP"].ToString());
                    tkAccount.ThaoTac = int.Parse(row["ThaoTac"].ToString());
                    tkAccount.Floor = row["IdFloor"].ToString();                   
                    tkAccount.ListChuyenId = row["ListChuyenId"].ToString();
                }
                return tkAccount;   
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: trong quá trình kiểm tra đăng nhập. "+ex.Message, "Lỗi truy vấn", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return tkAccount;   
            }
                    
        }
    }
}
