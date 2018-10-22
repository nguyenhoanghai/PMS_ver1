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
    class SanPhamDAO
    {
        public DataTable DSOBJ(string floor)
        {
            DataTable dt = new DataTable();
            string sql = "select MaSanPham, TenSanPham, DonGia, DonGiaCM from SanPham where IsDelete =0 and Floor='" + floor + "'";
             dt = dbclass.TruyVan_TraVe_DataTable(sql);
             return dt;            
        }

        public List<SanPham> GetListProduct(string floor)
        {
            try
            {
                List<SanPham> listProduct = null;
                DataTable dtProduct = DSOBJ(floor);
                if(dtProduct!=null && dtProduct.Rows.Count>0)
                {
                    listProduct = new List<SanPham>();
                    foreach(DataRow row in dtProduct.Rows)
                    {
                        int ProductId = 0;
                        string ProductName = row["TenSanPham"].ToString();
                        float DonGia = 0;
                        float DonGiaCM = 0;
                        int.TryParse(row["MaSanPham"].ToString(), out ProductId);
                        float.TryParse(row["DonGia"].ToString(), out DonGia);
                        float.TryParse(row["DonGiaCM"].ToString(), out DonGiaCM);
                        listProduct.Add(new SanPham() { 
                            TenSanPham = ProductName,
                            MaSanPham = ProductId.ToString(),
                            DonGia = DonGia,
                            DonGiaCM = DonGiaCM
                        });
                    }
                }
                return listProduct;
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        public int ThemOBJ(SanPham obj)
        {
            int kq = 0;
            try
            {

                string sql = "insert into SanPham (TenSanPham, DinhNghia, DonGia, Floor, DonGiaCM) values(N'" + obj.TenSanPham + "',N'" + obj.DinhNghia + "', "+obj.DonGia+" ,'"+obj.Floor+"', "+obj.DonGiaCM+")";
                kq = dbclass.TruyVan_XuLy(sql);

                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể thêm mặt hàng mới vào CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public int SuaThongTinOBJ(SanPham obj)
        {
            int kq = 0;
            try
            {

                string sql = "update SanPham set TenSanPham = N'" + obj.TenSanPham + "', DinhNghia =N'"+obj.DinhNghia+"', DonGia="+obj.DonGia+", DonGiaCM="+obj.DonGiaCM+" where MaSanPham ='" + obj.MaSanPham + "'";
                kq = dbclass.TruyVan_XuLy(sql);

                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể thay đổi thông tin mặt hàng dưới CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public int XoaOBJ(string IdObj)
        {
            int kq = 0;

            try
            {
                string sql = "update SanPham set IsDelete =1 where MaSanPham ='" + IdObj + "'";
                kq = dbclass.TruyVan_XuLy(sql);
                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể xoá mặt hàng dưới CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public DataTable TimKiemOBJ(string content, string idfloor)
        {
            DataTable dt = new DataTable();
            string sql = "select MaSanPham, TenSanPham, DinhNghia, DonGia, DonGiaCM from SanPham where IsDelete =0 and TenSanPham like N'" + content + "' and Floor ='"+idfloor+"'";
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

        public void LoadOBJToDataGirdview(DataGridView dg, string floor)
        {

            DataTable dt = new DataTable();
            string Strsql = "select MaSanPham, TenSanPham, DonGia, DinhNghia, DonGiaCM from SanPham Where IsDelete =0 and Floor ='" + floor + "' order by MaSanPham desc";
            dt = dbclass.TruyVan_TraVe_DataTable(Strsql);
            dbclass.loaddataridviewcolorrow(dg, dt);
            
        }

        public SanPham GetProductByName(string floor, string productName)
        {
            try
            {
                SanPham product = null;
                var listProduct = GetListProduct(floor);
                if(listProduct!=null && listProduct.Count>0)
                {
                    product = listProduct.Where(c => c.TenSanPham.Trim().ToUpper() == productName.Trim().ToUpper()).FirstOrDefault();
                }
                return product;
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }
        
    }
}
