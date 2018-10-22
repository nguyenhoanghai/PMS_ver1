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
    public class HangDAO
    {
        public DataTable DSHang(string floor, int isall)
        {
            DataTable dt = new DataTable();
            string sql = "";
            if (isall == 1)
                sql = "select MaSanPham, TenSanPham, DonGia, DonGiaCM from SanPham where IsDelete =0 order by MaSanPham desc";
            else
                sql = "select MaSanPham, TenSanPham, DonGia, DonGiaCM from SanPham where IsDelete =0 and Floor='" + floor + "'order by MaSanPham desc";
            try
            {
                if (sql != "")
                    dt = dbclass.TruyVan_TraVe_DataTable(sql);
                return dt;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: không thể lấy được danh sách các mặt hàng từ CSDL.", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return dt;
            }
        }

        public List<SanPham> GetListProduct(string floor, int isall)
        {
            List<SanPham> listProduct = new List<SanPham>();
            try
            {

                string strSQL = string.Empty;
                if (isall == 1)
                    strSQL = "select MaSanPham, TenSanPham, DinhNghia, DonGia, DonGiaCM from SanPham where IsDelete =0 order by MaSanPham desc";
                else
                    strSQL = "select MaSanPham, TenSanPham, DinhNghia, DonGia, DonGiaCM from SanPham where IsDelete =0 and Floor='" + floor + "'order by MaSanPham desc";
                var dtProducts = dbclass.TruyVan_TraVe_DataTable(strSQL);
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow row in dtProducts.Rows)
                    {
                        float donGia = 0;
                        float.TryParse(row["DonGia"].ToString(), out donGia);
                        float donGiaCM = 0;
                        float.TryParse(row["DonGiaCM"].ToString(), out donGiaCM);
                        listProduct.Add(new SanPham() { 
                            MaSanPham=row["MaSanPham"].ToString(),
                            TenSanPham=row["TenSanPham"].ToString(),
                            DonGia = donGia,
                            DonGiaCM = donGiaCM,
                            DinhNghia = row["DinhNghia"].ToString()
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listProduct;
        }

        public int ThemMatHang(Hang hang)
        {
            int kq = 0;
            try
            {
                
                string sql = "insert into SanPham(TenSanPham) values(N'"+hang.TenHang+"')";
                kq = dbclass.TruyVan_XuLy(sql);                
                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể thêm mặt hàng mới vào CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public int SuaThongTinMatHang(Hang hang)
        {
            int kq = 0;
            try
            {
               
                string sql = "update SanPham set TenSanPham = N'" + hang.TenHang + "' where MaSanPham ='" + hang.MaHang + "'";
                kq = dbclass.TruyVan_XuLy(sql);                
                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể thay đổi thông tin mặt hàng dưới CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public int XoaMatHang(string MaHang)
        {
            int kq = 0;
            
            try
            {


                string sql = "update SanPham set IsDelete = 1 where MaSanPham ='" + MaHang + "'";
                kq = dbclass.TruyVan_XuLy(sql);                
                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể xoá mặt hàng dưới CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public DataTable TimKiemHang(string noidung)
        {
            DataTable dt = new DataTable();
            string sql = "select MaSanPham, TenSanPham from SanPham where TenSanPham like N'" + noidung + "' or MaSanPham = '" + noidung + "'";
            try
            {
                
                dt = dbclass.TruyVan_TraVe_DataTable(sql);                
                return dt;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: không thể thực hiện tìm kiếm mặt hàng trên CSDL.", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return dt;
            }
        }

        public void loadHangraDataGirdview(DataGridView dg, string floor)
        {

            DataTable dt = new DataTable();
            string Strsql = "";
            if (floor == "")
                Strsql = "select MaSanPham, TenSanPham, DinhNghia from SanPham Where IsDelete =0";
            else
                Strsql = "select MaSanPham, TenSanPham, DinhNghia from SanPham Where IsDelete =0 and Floor ='" + floor + "'";
            if (!string.IsNullOrEmpty(Strsql))
            {
                dt = dbclass.TruyVan_TraVe_DataTable(Strsql);
                dbclass.loaddataridviewcolorrow(dg, dt);
            }
            else
                MessageBox.Show("Lỗi: không lấy được danh sách Mặt Hàng!!!");
        }

        public DataTable DSHangExportExcel(string floor)
        {
            DataTable dt = new DataTable();
            string sql = "select TenSanPham, DonGia, DonGiaCM from SanPham where IsDelete =0 and Floor='" + floor + "'order by MaSanPham desc";
            try
            {
                if (sql != "")
                    dt = dbclass.TruyVan_TraVe_DataTable(sql);
                return dt;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: không thể lấy được danh sách các mặt hàng từ CSDL.", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return dt;
            }
        }
    }
}
