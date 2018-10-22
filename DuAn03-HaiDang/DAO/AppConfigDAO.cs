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
    public class AppConfigDAO
    {
        public DataTable LoadAppConfig(int appId)
        {
            DataTable dt = null;
            try
            {
                string sql = string.Empty;
                if (appId == 0)
                {
                    sql = "select Id,DisplayName, Name, ValueDefault as Value, Description from Config where IsActive=1";
                    dt = dbclass.TruyVan_TraVe_DataTable(sql);
                }
                else
                {
                    sql = "select Id,DisplayName, Name, ValueDefault as Value, Description from Config where IsActive=1";
                    dt = dbclass.TruyVan_TraVe_DataTable(sql);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        sql = "select ca.ConfigId, ca.Value from Config_App ca where ca.AppId=" + appId;
                        DataTable dtConfigOfApp = dbclass.TruyVan_TraVe_DataTable(sql);
                        if (dtConfigOfApp != null && dtConfigOfApp.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                foreach (DataRow rowConfigApp in dtConfigOfApp.Rows)
                                {
                                    if (dt.Rows[i]["Id"].ToString().Trim().Equals(rowConfigApp["ConfigId"].ToString().Trim()))
                                    {
                                        dt.Rows[i]["Value"] = rowConfigApp["Value"];
                                        break;
                                    }
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
            return dt;
        }

        public List<AppConfig> GetListAppConfig(int appId)
        {
            List<AppConfig> listAppConfig = null;
            try
            {
                DataTable dt = LoadAppConfig(appId);
                if (dt != null && dt.Rows.Count > 0)
                {
                    listAppConfig = new List<AppConfig>();
                    foreach (DataRow row in dt.Rows)
                    {
                        var appConfig = new AppConfig();
                        appConfig.Name = row["Name"].ToString().Trim();
                        appConfig.Value = row["Value"].ToString().Trim();
                        listAppConfig.Add(appConfig);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listAppConfig;
        }
         
        public int AddObj(List<AppConfig> listAppConfig)
        {
            int kq = 0;
            try
            {
                if (listAppConfig != null && listAppConfig.Count > 0)
                {
                    string sqlGetAppId = "select AppId from Config_App order by AppId desc";
                    DataTable dtGetAppId = dbclass.TruyVan_TraVe_DataTable(sqlGetAppId);
                    int appId = 0;
                    if (dtGetAppId != null && dtGetAppId.Rows.Count > 0)
                        int.TryParse(dtGetAppId.Rows[0]["AppId"].ToString(), out appId);
                    appId++;
                    List<string> listStrSQL = new List<string>();
                    foreach (var item in listAppConfig)
                    {
                        listStrSQL.Add("insert into Config_App(ConfigId, AppId, Value) values(" + item.ConfigId + ", " + appId + ", '" + item.Value + "')");
                    }
                    kq = dbclass.ExecuteSqlTransaction(listStrSQL);
                    if (kq == 1)
                        kq = appId;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return kq;
        }

        public int UpdateObj(List<AppConfig> listAppConfig, int appId)
        {
            int kq = 0;
            try
            {
                if (listAppConfig != null && listAppConfig.Count > 0)
                {
                    List<string> listStrSQL = new List<string>();
                    foreach (var item in listAppConfig)
                    {
                        string sqlCheckExist = "select * from Config_App where ConfigId=" + item.ConfigId + " and AppId=" + appId;
                        DataTable dtCheckExist = dbclass.TruyVan_TraVe_DataTable(sqlCheckExist);
                        if (dtCheckExist != null && dtCheckExist.Rows.Count > 0)
                            listStrSQL.Add("update Config_App set Value='" + item.Value + "' where ConfigId=" + item.ConfigId + " and AppId=" + appId);
                        else
                            listStrSQL.Add("insert into Config_App(ConfigId, AppId, Value) values (" + item.ConfigId + ", " + appId + ", N'" + item.Value + "')");
                    }
                    kq = dbclass.ExecuteSqlTransaction(listStrSQL);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return kq;
        }

        public void LoadAppConfigToDataGirdview(DataGridView dg, int appId)
        {
            try
            {
                DataTable dt = null;
                dt = LoadAppConfig(appId);
                dbclass.loaddataridviewcolorrow(dg, dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
