using DuAn03_HaiDang.DATAACCESS;
using QuanLyNangSuat.POJO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace QuanLyNangSuat.DAO
{
    public class TurnCOMMngDAO
    {
        public DataTable LoadListTurnCOMMng()
        {
            DataTable dt = null;
            try
            {
                string sql = "select Id, ComTypeId, Status, TimeAction, Description, IsActive from TurnCOMMng where IsDeleted =0 order by Id desc";
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public List<TurnCOMMng> GetListTurnCOMConfig()
        {
            List<TurnCOMMng> listConfig = null;
            try
            {
                var dt = LoadListTurnCOMMng();
                if (dt != null && dt.Rows.Count > 0)
                {
                    listConfig = new List<TurnCOMMng>();
                    foreach (DataRow row in dt.Rows)
                    {
                        int Id = 0;
                        int.TryParse(row["Id"].ToString(), out Id);
                        int ComTypeId = 0;
                        int.TryParse(row["ComTypeId"].ToString(), out ComTypeId);
                        int Status = 0;
                        int.TryParse(row["Status"].ToString(), out Status);
                        TimeSpan TimeAction = new TimeSpan(0, 0, 0);
                        TimeSpan.TryParse(row["TimeAction"].ToString(), out TimeAction);
                        bool IsActive = false;
                        bool.TryParse(row["IsActive"].ToString(), out IsActive);
                        listConfig.Add(new TurnCOMMng()
                        {
                            Id = Id,
                            COMTypeId = ComTypeId,
                            Status = Status,
                            TimeAction = TimeAction,
                            IsActive = IsActive,
                            Description = row["Description"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return listConfig;
        }

        public int AddObj(TurnCOMMng obj)
        {
            int kq = 0;
            try
            {
                string sql = "insert into TurnCOMMng(ComTypeId, Status, TimeAction, Description, IsActive) values(" + obj.COMTypeId + ", " + obj.Status + ", N'" + obj.TimeAction + "', '" + obj.Description + "', '" + obj.IsActive + "' )";
                kq = dbclass.TruyVan_XuLy(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return kq;
        }

        public int UpdateObj(TurnCOMMng obj)
        {
            int kq = 0;
            try
            {
                string sql = "update TurnCOMMng set ComTypeId= " + obj.COMTypeId + ", Status = " + obj.Status + ", TimeAction='"+obj.TimeAction+"', Description=N'" + obj.Description + "', IsActive='" + obj.IsActive + "' where Id =" + obj.Id + " and IsDeleted=0";
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
                string sql = "update TurnCOMMng set IsDeleted = 1 where Id ='" + Id + "'";
                kq = dbclass.TruyVan_XuLy(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return kq;
        }
    }
}
