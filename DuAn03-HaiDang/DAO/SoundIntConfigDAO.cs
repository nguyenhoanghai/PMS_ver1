using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.Model;
using DuAn03_HaiDang.POJO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DuAn03_HaiDang.DAO
{
    public class SoundIntConfigDAO
    {
        public DataTable GetListIntConfig()
        {
            DataTable dt = null;
            try
            {
                string sql = "Select Id, Code, Name, Description, Formula, IsProductivity from SOUND_IntConfig where IsActive=1 and IsDeleted=0";
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            return dt;
        }

        public List<SoundIntConfig> GetListReadIntConfig()
        {
            List<SoundIntConfig> listSoundInt = null;
            try
            {
                DataTable dt = GetListIntConfig();
                if (dt != null && dt.Rows.Count > 0)
                {
                    listSoundInt = new List<SoundIntConfig>();
                    foreach (DataRow row in dt.Rows)
                    {
                        listSoundInt.Add(new SoundIntConfig() { 
                            Id = int.Parse(row["Id"].ToString()),
                            Code = row["Code"].ToString(),
                            Formula = row["Formula"].ToString(),
                            Name = row["Name"].ToString(),
                            IsProductivity = bool.Parse(row["IsProductivity"].ToString())
                        });
                    }
                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            return listSoundInt;
        }

        public void LoadDataForGridView(DataGridView dg)
        {
            try
            {
                string sql = "Select Id, Code,  Name, Description, Formula, IsProductivity, IsActive from SOUND_IntConfig where IsDeleted=0";
                DataTable dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if(dt!=null && dt.Rows.Count>0)
                    dbclass.loaddataridviewcolorrow(dg, dt);
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        public List<ModelSelect> GetListSelectItem()
        {
            List<ModelSelect> listSelect = null;
            try
            {
                var dt = GetListIntConfig();
                if (dt != null && dt.Rows.Count > 0)
                {
                    listSelect = new List<ModelSelect>();
                    foreach (DataRow row in dt.Rows)
                    {
                        listSelect.Add(new ModelSelect()
                        {
                            Value = int.Parse(row["Id"].ToString()),
                            Text = row["Name"].ToString(),
                            Code = row["Code"].ToString(),
                        });
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return listSelect;
        }

        public int AddObj(SoundIntConfig obj)
        {
            int kq = 0;
            try
            {
                string sql = "insert into SOUND_IntConfig(Name, Description, Formula, IsActive, Code, IsProductivity) values(N'" + obj.Name + "', N'" + obj.Description + "', N'" + obj.Formula + "', '" + obj.IsActive + "','" + obj.Code + "', '"+obj.IsProductivity+"' )";
                kq = dbclass.TruyVan_XuLy(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return kq;
        }

        public int UpdateObj(SoundIntConfig obj)
        {
            int kq = 0;
            try
            {
                string sql = "update SOUND_IntConfig set Name = N'" + obj.Name + "', Description=N'" + obj.Description + "', Formula= N'" + obj.Formula + "', IsActive='" + obj.IsActive + "',  Code='" + obj.Code + "', IsProductivity='"+obj.IsProductivity+"' where Id =" + obj.Id + " and IsDeleted=0";
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
                string sql = "update SOUND_IntConfig set IsDeleted = 1 where Id ='" + Id + "'";
                kq = dbclass.TruyVan_XuLy(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return kq;
        }

        public string GetFormulaById(int id)
        {
            string formula = string.Empty;
            try
            {
                string sql = "Select Formula from SOUND_IntConfig where Id="+id+" and IsActive=1 and IsDeleted=0";
                DataTable dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                    formula = dt.Rows[0]["Formula"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return formula;
        }
    }
}
