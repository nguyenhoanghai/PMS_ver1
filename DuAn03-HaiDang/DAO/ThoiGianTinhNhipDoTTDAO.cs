using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuAn03_HaiDang.DATAACCESS;
using System.Windows.Forms;
using DuAn03_HaiDang.POJO;
using System.Data;

namespace DuAn03_HaiDang.DAO
{
    class ThoiGianTinhNhipDoTTDAO
    {
        
        public int ThemOBJ(ThoiGianTinhNhipDoTT obj)
        {
            int kq = 0;
            try
            {

                string sql = "insert into ThoiGianTinhNhipDoTT (Ngay, MaChuyen, ThoiGianBatDau) values(N'" + obj.Ngay + "'," + obj.MaChuyen + ",'"+obj.ThoiGianBatDau+"')";
                kq = dbclass.TruyVan_XuLy(sql);

                return kq;
            }
            catch (Exception)
            {
                return kq;
            }
        }

        public int SuaThongTinOBJ(ThoiGianTinhNhipDoTT obj)
        {
            int kq = 0;
            try
            {
                string sql = "update ThoiGianTinhNhipDoTT set ThoiGianBatDau =N'" + obj.ThoiGianBatDau + "' where Ngay ='" + obj.Ngay + "' and MaChuyen="+obj.MaChuyen+"";
                kq = dbclass.TruyVan_XuLy(sql);

                return kq;
            }
            catch (Exception)
            {
                return kq;
            }
        }

        public TimeSpan LayTimeBatDau(DateTime Ngay, int MaChuyen)
        {
            var daynow = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
            DataTable dt = new DataTable();
            string sql = "select ThoiGianBatDau from ThoiGianTinhNhipDoTT where Ngay ='"+daynow+"' and MaChuyen =" + MaChuyen + "";
            try
            {

                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    return TimeSpan.Parse(dt.Rows[0][0].ToString());
                }
                else
                {
                    return TimeSpan.Parse("00:00:00");
                }
                
            }
            catch (Exception)
            {
                return TimeSpan.Parse("00:00:00");
            }
        }

    }
}
