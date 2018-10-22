using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.POJO;
using PMS.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DuAn03_HaiDang.DAO
{
    public class NangSuatCumDAO
    {
        private ClusterDAO clusterDAO = new ClusterDAO();
      //  private ErrorDAO errorDAO = new ErrorDAO();
        public int ThemOBJ(NangSuat_Cum obj)
        {            
            int kq = -1;
            try
            {
                string sql = "insert into NangSuat_Cum(Ngay, STTChuyen_SanPham, IdCum, SanLuongKCSTang) values('" + obj.Ngay + "','" + obj.STTChuyen_SanPham + "', '" + obj.IdCum + "', 0)";
                kq = dbclass.TruyVan_XuLy(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return kq;
        }

        public List<string> GetSanLuongCumCuaChuyen(string Ngay, string STTChuyen_SanPham)
        {
            List<string> listSanLuong = new List<string>();           
            try
            {
                DataTable dt = new DataTable();
                string sql = "select SanLuongKCSTang from NangSuat_Cum where Ngay = '" + Ngay + "' and STTChuyen_SanPham = '" + STTChuyen_SanPham + "' and IsDeleted=0 order by IdCum ASC";
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        listSanLuong.Add(row["SanLuongKCSTang"].ToString());
                    }
                }               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listSanLuong;
        }

        public bool CheckExistNangSuatCum(DateTime Ngay, int STTChuyen_SanPham, int CumId)
        {
            var result = false;
            try
            {
                string sql = "select *  from NangSuat_Cum where Ngay='" + Ngay + "' and STTChuyen_SanPham=" + STTChuyen_SanPham + " and IsDeleted=0 and IdCum=" + CumId;
                 DataTable dt = dbclass.TruyVan_TraVe_DataTable(sql);
                 if (dt != null && dt.Rows.Count > 0)
                 {
                     result = true;
                 }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            return result;
        }

        public bool AddNangSuatCumOfChuyen(int sttChuyenSanPham, int maChuyen) 
        {
            try                
            {
                bool result = false;
                string dateNow = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                var listCluster = clusterDAO.GetCumOfChuyen(maChuyen);
                if (listCluster != null && listCluster.Count > 0)
                {
                    List<string> listQuery = new List<string>();
                    foreach (var cluster in listCluster)
                    {
                        string strSQLCheckExist = "Select Id From NangSuat_Cum Where STTChuyen_SanPham=" + sttChuyenSanPham + " and IsDeleted=0 and IdCum=" + cluster.Id + " and Ngay ='" + dateNow + "'";
                        DataTable dtCheckExist = dbclass.TruyVan_TraVe_DataTable(strSQLCheckExist);
                        if (dtCheckExist == null || (dtCheckExist!=null && dtCheckExist.Rows.Count==0))
                        {
                            listQuery.Add("Insert Into NangSuat_Cum(Ngay, STTChuyen_SanPham, IdCum) Values('" + dateNow + "'," + sttChuyenSanPham + ", " + cluster.Id + ")");                            
                        }
                    }
                    if (listQuery.Count > 0)
                    {
                        var rs = dbclass.ExecuteSqlTransaction(listQuery);
                        if(rs>0)
                            result = true;
                            
                    }
                    else
                        result = true;
                }                 
                return result;
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        public bool AddNangSuatCumLoiOfChuyen(int sttChuyenSanPham, int maChuyen)
        {
            try
            {
                bool result = false;
                string dateNow = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                var listCluster = clusterDAO.GetCumOfChuyen(maChuyen);
                if (listCluster != null && listCluster.Count > 0)
                {
                    List<string> listQuery = new List<string>();
                    foreach (var cluster in listCluster)
                    {
                        var listError = BLLError.GetAll(); // errorDAO.GetListError();
                        if (listError != null && listError.Count > 0)
                        {
                            foreach (var error in listError)
                            {
                                string strSQLCheckExist = "Select Id From NangSuat_CumLoi Where STTChuyenSanPham=" + sttChuyenSanPham + " and IsDeleted=0 and CumId=" + cluster.Id + " and Ngay ='" + dateNow + "' and ErrorId=" + error.Id;
                                DataTable dtCheckExist = dbclass.TruyVan_TraVe_DataTable(strSQLCheckExist);
                                if (dtCheckExist == null || (dtCheckExist != null && dtCheckExist.Rows.Count == 0))
                                {
                                    listQuery.Add("Insert Into NangSuat_CumLoi(Ngay, STTChuyenSanPham, CumId, ErrorId) Values('" + dateNow + "'," + sttChuyenSanPham + ", " + cluster.Id + ", "+error.Id+")");
                                }
                            }
                        }                        
                    }
                    if (listQuery.Count > 0)
                    { int rs = dbclass.ExecuteSqlTransaction(listQuery);
                        if(rs>0)
                            result = true;
                    }
                    else
                        result = true;
                }                
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
