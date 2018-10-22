using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business
{
   public class BLLMapCommoIdForDay
    { 
       public static bool AddMapIdSanPhamNgay(List<int> LineIds, List<MapIdSanPhamNgay> listMapIdSanPhamNgay)
       {
           try
           {
               var db = new PMSEntities();
               bool result = false;
               List<string> listStrSql = new List<string>();
               var now = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;

               var oldMaps = db.MapIdSanPhamNgays.Where(x => !x.IsDeleted && x.Ngay == now && LineIds.Contains(x.MaChuyen)) ;
               if (oldMaps != null && oldMaps.Count() > 0)
               {
                   foreach (var item in oldMaps)
                   {
                       item.IsDeleted = true;
                   }
               }

               if (listMapIdSanPhamNgay != null && listMapIdSanPhamNgay.Count > 0)
               {
                   foreach (var map in listMapIdSanPhamNgay)
                   {
                     //  string strSQLInsert = "Insert Into MapIdSanPhamNgay(Ngay, MaChuyen, STT, STTChuyenSanPham, MaSanPham) Values('" + map.Ngay + "', " + map.MaChuyen + ", " + map.STT + "," + map.STTChuyenSanPham + "," + map.MaSanPham + ")";
                      // listStrSql.Add(strSQLInsert);
                       db.MapIdSanPhamNgays.Add(map);
                   }
                   db.SaveChanges();
                   result = true;
               }
              

                   
               //    string strSQLMapInDay = "Select Id From MapIdSanPhamNgay Where Ngay='" + now + "' and MaChuyen in (" + strListChuyen + ") and IsDeleted=0";
               //DataTable dtMapInDay = dbclass.TruyVan_TraVe_DataTable(strSQLMapInDay);
               //if (dtMapInDay != null && dtMapInDay.Rows.Count > 0)
               //{
               //    foreach (DataRow row in dtMapInDay.Rows)
               //    {
               //        string strSQLDeleteMapOld = "Update MapIdSanPhamNgay Set IsDeleted=1 Where Id=" + row["Id"].ToString();
               //        listStrSql.Add(strSQLDeleteMapOld);
               //    }
               //}
               //if (listMapIdSanPhamNgay != null && listMapIdSanPhamNgay.Count > 0)
               //{
               //    foreach (var map in listMapIdSanPhamNgay)
               //    {
               //        string strSQLInsert = "Insert Into MapIdSanPhamNgay(Ngay, MaChuyen, STT, STTChuyenSanPham, MaSanPham) Values('" + map.Ngay + "', " + map.MaChuyen + ", " + map.STT + "," + map.STTChuyenSanPham + "," + map.MaSanPham + ")";
               //        listStrSql.Add(strSQLInsert);
               //    }
               //}
               //if (listStrSql.Count > 0)
               //{
               //    int rs = dbclass.ExecuteSqlTransaction(listStrSql);
               //    if (rs > 0)
               //        result = true;
               //}
               return result;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
        
    }
}
