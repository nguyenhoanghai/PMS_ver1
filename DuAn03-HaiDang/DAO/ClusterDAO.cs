using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.Model;
using DuAn03_HaiDang.POJO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DuAn03_HaiDang.DAO
{
    public class ClusterDAO
    {
        public List<Cluster> GetListCluster()
        {
             List<Cluster> listCluster= null;
            try
            {
                string sql = "select c.Id, c.Code, c.TenCum, c.MoTa, c.IdChuyen, c.FloorId, ch.TenChuyen , f.Name as FloorName, c.IsEndOfLine from Cum c, Chuyen ch, Floor f where c.IdChuyen=ch.MaChuyen and c.FloorId=f.IdFloor and c.IsDeleted =0 order by c.Id desc";
                DataTable dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    listCluster = new List<Cluster>();
                    foreach (DataRow row in dt.Rows)
                    {
                        listCluster.Add(new Cluster() { 
                            Id = int.Parse(row["Id"].ToString()),
                            Code = row["Code"].ToString().Trim(),
                            Name = row["TenCum"].ToString(),
                            Description = row["MoTa"].ToString(),
                            LineId = int.Parse(row["IdChuyen"].ToString()),
                            FloorId = int.Parse(row["FloorId"].ToString()),
                            LineName = row["TenChuyen"].ToString(),
                            FloorName = row["FloorName"].ToString(),
                            IsEndOfLine = bool.Parse(row["IsEndOfLine"].ToString())
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listCluster;
        }

        

        public int AddObj(Cluster obj)
        {
            int kq = 0;
            try
            {
                string sql = "insert into Cum(Code, TenCum, MoTa, IdChuyen, FloorId, IsEndOfLine) values('" + obj.Code + "', N'" + obj.Name + "', N'" + obj.Description + "', " + obj.LineId + ", "+obj.FloorId+" ,'"+obj.IsEndOfLine+"')";
                kq = dbclass.TruyVan_XuLy(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return kq;
        }

        public int UpdateObj(Cluster obj)
        {
            int kq = 0;
            try
            {
                string sql = "update Cum set Code = '" + obj.Code + "', TenCum=N'" + obj.Name + "', MoTa=N'" + obj.Description+ "', IdChuyen=" + obj.LineId + ", FloorId="+obj.FloorId+", IsEndOfLine='"+obj.IsEndOfLine+"' where Id =" + obj.Id + " and IsDeleted=0";
                kq = dbclass.TruyVan_XuLy(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return kq;
        }

        public int DeleteObj(int Id)
        {
            int kq = 0;
            try
            {
                string sql = "update Cum set IsDeleted = 1 where Id ='" + Id + "'";
                kq = dbclass.TruyVan_XuLy(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return kq;
        }

        public bool CheckExistCode(string code, int id)
        {
            bool result = false;
            try
            {
                string sql = string.Empty;
                if (id == 0)
                    sql = "Select * from Cum where Code='" + code.Trim() + "'";
                else
                    sql = "Select * from Cum where Code='" + code.Trim() + "' and Id!=" + id;
                DataTable dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                    result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public List<Cluster> GetCumOfChuyen(int IdChuyen)
        {
            List<Cluster> listCluster = new List<Cluster>();
            try
            {                
                string sql = "select Id, Code, TenCum, IsEndOfLine  from Cum where IdChuyen =" + IdChuyen+" and IsDeleted=0";
                DataTable dtCum = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dtCum != null && dtCum.Rows.Count > 0)
                {
                    foreach (DataRow row in dtCum.Rows)
                    {
                        Cluster model = new Cluster();
                        model.Id = int.Parse(row["Id"].ToString());
                        model.Code = row["Code"].ToString();
                        model.Name = row["TenCum"].ToString();
                        bool isEndOfLine = false;
                        bool.TryParse(row["IsEndOfLine"].ToString(), out isEndOfLine);
                        model.IsEndOfLine = isEndOfLine;
                        listCluster.Add(model);
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
