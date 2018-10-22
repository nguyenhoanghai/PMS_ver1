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
    public class SoundReadConfigDAO
    {
        private SoundDAO soundDAO;
        private SoundIntConfigDAO soundIntConfigDAO;
        public SoundReadConfigDAO()
        {
            soundDAO = new SoundDAO();
            soundIntConfigDAO = new SoundIntConfigDAO();
        }
        public DataTable GetListConfigByIdChuyen(int idChuyen, int configType)
        {
            DataTable dt = null;
            try
            {
                string sql = "Select Id, IsDeleted, Name, Description, IsActive from SOUND_ReadConfig where IdChuyen=" + idChuyen + " and IsDeleted=0 and ConfigType="+configType;
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            return dt;
        }

        public void LoadConfigDataToGridView(DataGridView dg, int idChuyen, int configType)
        {
            try
            {
                DataTable dt = GetListConfigByIdChuyen(idChuyen, configType);
                if (dt != null && dt.Rows.Count > 0)
                    dbclass.loaddataridviewcolorrow(dg, dt); 
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        public int AddObj(SoundReadConfig obj)
        {
            int kq = 0;
            try
            {
                string sql = "insert into SOUND_ReadConfig(Name, Description, IsActive, IdChuyen, ConfigType) values(N'" + obj.Name + "', N'" + obj.Description + "','"+obj.IsActive+"', "+obj.IdChuyen+", "+obj.ConfigType+" )";
                var resultAddReadConfig = dbclass.TruyVan_XuLy(sql);
                if (resultAddReadConfig != 0)
                {
                    if (obj.listItem.Count > 0)
                    {
                        string sqlSel = "select top 1 Id from SOUND_ReadConfig order by Id desc";
                        DataTable dt = dbclass.TruyVan_TraVe_DataTable(sqlSel);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            string Id = dt.Rows[0]["Id"].ToString();
                            foreach (SoundReadItem item in obj.listItem)
                            {
                                string sqlInsert = "insert into SOUND_ReadConfigDetail(IdReadConfig, OrderIndex, IntType, IdSound, IdIntConfig, IsActive) values("+Id+", "+item.OrderIndex+", "+item.IntType+","+item.IsSound+","+item.IdIntConfig+", '"+item.IsActive+"')";
                                dbclass.TruyVan_XuLy(sqlInsert);
                            }
                        }
                        
                    }
                }
                kq = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return kq;
        }

        public int UpdateObj(SoundReadConfig obj)
        {
            int kq = 0;
            try
            {
                string sql = "update SOUND_ReadConfig set Name=N'"+obj.Name+"', Description =N'"+obj.Description+"', IsActive='"+obj.IsActive+"', ConfigType="+obj.ConfigType+" where Id="+obj.Id;
                var resultAddReadConfig = dbclass.TruyVan_XuLy(sql);
                if (resultAddReadConfig != 0)
                {
                    string sqlListItemOld = "select Id from SOUND_ReadConfigDetail where IsDeleted=0 and IdReadConfig=" + obj.Id;
                    DataTable dtListItemOld = dbclass.TruyVan_TraVe_DataTable(sqlListItemOld);
                    if (dtListItemOld != null && dtListItemOld.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtListItemOld.Rows)
                        {
                            string sqlDelete = "update SOUND_ReadConfigDetail set IsDeleted=1 where Id = "+row["Id"].ToString();
                            dbclass.TruyVan_XuLy(sqlDelete);
                        }
                    }
                    if (obj.listItem.Count > 0)
                    {
                        foreach (SoundReadItem item in obj.listItem)
                        {
                            string sqlInsert = "insert into SOUND_ReadConfigDetail(IdReadConfig, OrderIndex, IntType, IdSound, IdIntConfig, IsActive) values(" + obj.Id + ", " + item.OrderIndex + ", " + item.IntType + "," + item.IsSound + "," + item.IdIntConfig + ", '" + item.IsActive + "')";
                            dbclass.TruyVan_XuLy(sqlInsert);
                        }
                    }
                }
                kq = 1;
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
                string sql = "update SOUND_ReadConfig set IsDeleted = 1 where Id ='" + Id + "'";
                kq = dbclass.TruyVan_XuLy(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return kq;
        }

        public SoundReadConfig GetInfoById(int Id)
        {
            SoundReadConfig config = null;
            try
            {
                string sqlSelectConfig = "select * from SOUND_ReadConfig where Id=" + Id + " and IsDeleted=0;";
                DataTable dt = dbclass.TruyVan_TraVe_DataTable(sqlSelectConfig);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    config = new SoundReadConfig();
                    config.Id = int.Parse(row["Id"].ToString());
                    config.Name = row["Name"].ToString();
                    config.Description = row["Description"].ToString();
                    bool isActive = false;
                    bool.TryParse(row["IsActive"].ToString(), out isActive);
                    config.IsActive = isActive;
                    string sqlSelectListItem = "select * from SOUND_ReadConfigDetail where IdReadConfig="+config.Id+" and IsDeleted=0";
                    DataTable dtList = dbclass.TruyVan_TraVe_DataTable(sqlSelectListItem);
                    if (dtList != null && dtList.Rows.Count > 0)
                    {
                        foreach (DataRow rowItem in dtList.Rows)
                        {
                            SoundReadItem item = new SoundReadItem();
                            item.IntType = int.Parse(rowItem["IntType"].ToString());
                            item.OrderIndex = int.Parse(rowItem["OrderIndex"].ToString());
                            item.IsSound = int.Parse(rowItem["IdSound"].ToString());
                            item.IdIntConfig = int.Parse(rowItem["IdIntConfig"].ToString());
                            bool isActiveItem = false;
                            bool.TryParse(rowItem["IsActive"].ToString(), out isActiveItem);
                            item.IsActive = isActiveItem;
                            config.listItem.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            return config;
        }

        public List<ModelSoundItem> GetListSoundItemByChuyen(int idChuyen)
        {
            List<ModelSoundItem> listItem = null;
            try
            {
                string sqlReadConfig = "Select Id from SOUND_ReadConfig where IdChuyen=" + idChuyen + " and IsActive=1 and IsDeleted=0";
                DataTable dtReadConfig = dbclass.TruyVan_TraVe_DataTable(sqlReadConfig);
                if (dtReadConfig != null && dtReadConfig.Rows.Count > 0)
                {
                    foreach (DataRow row in dtReadConfig.Rows)
                    {
                        int idConfig = int.Parse(row["Id"].ToString());
                        string sqlReadConfigDetail = "Select IntType, IdSound, IdIntConfig from SOUND_ReadConfigDetail where IdReadConfig=" + idConfig + " and IsActive=1 and IsDeleted=0 order by OrderIndex ASC";
                        DataTable dtReadConfigDetail = dbclass.TruyVan_TraVe_DataTable(sqlReadConfigDetail);
                        if(dtReadConfigDetail!=null && dtReadConfigDetail.Rows.Count>0)
                        {
                            listItem = new List<ModelSoundItem>();
                            foreach(DataRow rowDetail in dtReadConfigDetail.Rows)
                            {
                                ModelSoundItem item = new ModelSoundItem();
                                int intType = 0;
                                int.TryParse(rowDetail["IntType"].ToString(), out intType);
                                item.FileType = intType;
                                if (intType == 0)
                                {
                                    int idIntConfig = 0;
                                    int.TryParse(rowDetail["IdIntConfig"].ToString(), out idIntConfig);
                                    string formula = soundIntConfigDAO.GetFormulaById(idIntConfig);
                                    if(!string.IsNullOrEmpty(formula))
                                    {
                                        formula = formula.Replace('[',' ');
                                        formula = formula.Replace(']', ' ');
                                        item.Formula = formula;
                                    }
                                }
                                else
                                {
                                    int idSound = 0;
                                    int.TryParse(rowDetail["IdSound"].ToString(), out idSound);
                                    string path = soundDAO.GetPathById(idSound);
                                    if (!string.IsNullOrEmpty(path))
                                        item.SoundPath = path;
                                }
                                listItem.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listItem;
        }
        
    }
}
