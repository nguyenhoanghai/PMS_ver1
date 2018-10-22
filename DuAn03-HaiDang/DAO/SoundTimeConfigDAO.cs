using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.POJO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DuAn03_HaiDang.DAO
{
    public class SoundTimeConfigDAO
    {
        public DataTable LoadListTimeByConfigType(int configType)
        {
            DataTable dt = null;
            try
            {
                string sql = "select Time, SoLanDoc from SOUND_TimeConfig where IsActive=1 and IsDeleted =0 and ConfigType="+configType;
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public List<SoundTimeConfig> GetListTimeByConfigType(int configType)
        {
            List<SoundTimeConfig> list = null;
            try
            {
                DataTable dt = LoadListTimeByConfigType(configType);
                if (dt != null && dt.Rows.Count > 0)
                {
                    list = new List<SoundTimeConfig>();
                    foreach (DataRow row in dt.Rows)
                    {
                        SoundTimeConfig item = new SoundTimeConfig();
                        item.Time = TimeSpan.Parse(row["Time"].ToString());
                        item.SoLanDoc = int.Parse(row["SoLanDoc"].ToString());
                        list.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            return list;
        }

        public int AddObj(SoundTimeConfig obj)
        {
            int kq = 0;
            try
            {
                string sql = "insert into SOUND_TimeConfig(Time, SoLanDoc, IsActive, ConfigType) values('" + obj.Time + "', " + obj.SoLanDoc + ", '"+obj.IsActive+"', "+obj.ConfigType+" )";
                kq = dbclass.TruyVan_XuLy(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return kq;
        }

        public int UpdateObj(SoundTimeConfig obj)
        {
            int kq = 0;
            try
            {
                string sql = "update SOUND_TimeConfig set Time = '" + obj.Time + "', SoLanDoc=" + obj.SoLanDoc + ", IsActive='" + obj.IsActive + "', ConfigType="+obj.ConfigType+" where Id =" + obj.Id + " and IsDeleted=0";
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
                string sql = "update SOUND_TimeConfig set IsDeleted = 1 where Id ='" + Id + "'";
                kq = dbclass.TruyVan_XuLy(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return kq;
        }

        public void LoadMailToDataGirdview(DataGridView dg, int configType)
        {
            try
            {
                DataTable dt = new DataTable();
                string Strsql = "";
                Strsql = "select m.Id, m.Time, m.SoLanDoc, m.IsActive from SOUND_TimeConfig m Where m.IsDeleted =0 and m.ConfigType="+configType;
                dt = dbclass.TruyVan_TraVe_DataTable(Strsql);
                dbclass.loaddataridviewcolorrow(dg, dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
