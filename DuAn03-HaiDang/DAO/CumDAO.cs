using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.KeyPad_Chuyen.dao;
using DuAn03_HaiDang.Model;
using DuAn03_HaiDang.POJO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DuAn03_HaiDang.DAO
{
    public class CumDAO
    {
        private DataTable dt;
        private ChuyenDAO chuyenDAO;
        public CumDAO()
        {
            this.dt = new DataTable();
            this.chuyenDAO = new ChuyenDAO();
        }

        public List<int> GetIdCumOfChuyen(int IdChuyen)
        {
            List<int> listIdCum = new List<int>();
            try
            {
                dt.Clear();
                string sql = "select Id from Cum where IdChuyen ="+IdChuyen;
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if(dt!=null && dt.Rows.Count>0)
                {
                    foreach(DataRow row in dt.Rows)
                    {
                        int idCum=0;
                        int.TryParse(row["Id"].ToString(), out idCum);
                        if(idCum!=0)
                            listIdCum.Add(idCum);
                    }
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listIdCum;
        }

        public int GetMaxCountOfChuyen()
        {
            int maxCount=0;
            try
            {
                dt.Clear();
                string sql = "select max(a.socum) maxCount from (select Count(Id) as SoCum, IdChuyen from Cum group by IdChuyen) a";
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    int.TryParse(dt.Rows[0]["maxCount"].ToString(), out maxCount);
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            return maxCount;
        }

        public List<NSCum> GetNangSuatCumOfChuyen(string sttChuyenSanPham)
        {
            List<NSCum> listNSCum= null;
            try
            {
                dt.Clear();
                string strSQL = "select ns_c.IdCum, ns_c.SanLuongKCSTang, ns_c.SanLuongKCSGiam from NangSuat_Cum ns_c where ns_c.STTChuyen_SanPham = '" + sttChuyenSanPham + "' and Ngay ='" + DateTime.Now.Date.ToString() + "'";
                dt = dbclass.TruyVan_TraVe_DataTable(strSQL);
                if (dt != null && dt.Rows.Count > 0)
                {
                    listNSCum = new List<NSCum>();
                    foreach (DataRow row in dt.Rows)                    
                    {
                        int IdCum = 0;
                        int.TryParse(row["IdCum"].ToString(), out IdCum);
                        int sanLuongKCSTang = 0;
                        int sanLuongKCSGiam = 0;
                        int.TryParse(row["SanLuongKCSTang"].ToString(), out sanLuongKCSTang);
                        int.TryParse(row["SanLuongKCSGiam"].ToString(), out sanLuongKCSGiam);
                        int sanLuong = sanLuongKCSTang - sanLuongKCSGiam;
                        if (sanLuong < 0)
                            sanLuong = 0;
                         listNSCum.Add(new NSCum()
                        {
                            clusterId = IdCum,
                            sanLuong = "0",
                            cum = sanLuong.ToString()
                        });                        
                    }
                    dt.Clear();
                    string sql = "select ns_c.IdCum, SUM(ns_c.SanLuongKCSTang) LuyKeTang, SUM(ns_c.SanLuongKCSGiam) LuyKeGiam from NangSuat_Cum ns_c where ns_c.STTChuyen_SanPham = '" + sttChuyenSanPham + "' GROUP BY IdCum";
                    dt = dbclass.TruyVan_TraVe_DataTable(sql);                    
                    if (dt != null && dt.Rows.Count > 0 && listNSCum!=null && listNSCum.Count>0)
                    {                        
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int IdCum = 0;
                            int.TryParse(dt.Rows[i]["IdCum"].ToString(), out IdCum);
                            for (int j = 0; j < listNSCum.Count; j++)
                            {
                                if (IdCum == listNSCum[j].clusterId)
                                {
                                    int luyKeTang = 0;
                                    int luyKeGiam = 0;
                                    int.TryParse(dt.Rows[i]["LuyKeTang"].ToString(), out luyKeTang);
                                    int.TryParse(dt.Rows[i]["LuyKeGiam"].ToString(), out luyKeGiam);
                                    int luyKe = luyKeTang - luyKeGiam;
                                    if (luyKe < 0)
                                        luyKe = 0;
                                    listNSCum[j].sanLuong = luyKe.ToString();
                                    break;
                                }
                            }
                        }                        
                    }       
                }                         
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            return listNSCum;
        }

        public List<ModelNangSuatCum> GetTTNangSuatCum(string listStrLineId)
        {
            List<ModelNangSuatCum> listModelNangSuatCum = new List<ModelNangSuatCum>();
            try
            {
                var listChuyen = chuyenDAO.GetListChuyenInfByListId(listStrLineId);
                if (listChuyen != null && listChuyen.Count > 0)
                {                    
                    foreach (var chuyen in listChuyen)
                    {
                        var ChuyenSanPham = chuyenDAO.GetChuyenSanPhamInfByChuyenId(chuyen.MaChuyen);
                        if (ChuyenSanPham != null)
                        {
                            ModelNangSuatCum model = new ModelNangSuatCum();
                            model.chuyen = ChuyenSanPham.TenChuyen;
                            model.maHang = ChuyenSanPham.TenSanPham;
                            model.sanLuongKeHoach = ChuyenSanPham.SanLuongKeHoach.ToString();
                            model.listNangSuatCum = GetNangSuatCumOfChuyen(ChuyenSanPham.STT);
                            listModelNangSuatCum.Add(model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            return listModelNangSuatCum;
        }

        public List<Cluster> GetListClusterOfLine (string lineId) 
        {
            List<Cluster> listCluster = null;
            try
            {
                string strSQL = "Select * From Cum Where IsDeleted=0 and IdChuyen="+lineId+" and IsEndOfLine=0";
                DataTable dtCluster = dbclass.TruyVan_TraVe_DataTable(strSQL);
                if(dtCluster!=null && dtCluster.Rows.Count>0)
                {
                    listCluster = new List<Cluster>();
                    foreach(DataRow row in dtCluster.Rows)
                    {
                        Cluster cluster = new Cluster();
                        int id = 0;
                        int.TryParse(row["Id"].ToString(), out id);
                        cluster.Id = id;
                        cluster.Name = row["TenCum"].ToString();
                        cluster.Code = row["Code"].ToString();
                        listCluster.Add(cluster);
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            return listCluster;
        }
    }
}
