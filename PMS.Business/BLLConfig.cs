using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business
{
    public class BLLConfig
    {
        static Object key = new object();
        private static volatile BLLConfig _Instance;
        public static BLLConfig Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLConfig();

                return _Instance;
            }
        }

        private BLLConfig() { }

        public string FindConfigValue(int appId, string configName)
        {
            var value = string.Empty;
            try
            {
                var db = new PMSEntities();
                dynamic cf = db.Config_App.FirstOrDefault(x => x.AppId == appId && x.Config.Name.Trim().ToUpper().Equals(configName));
                if (cf == null)
                {
                    cf = db.Configs.FirstOrDefault(x => x.IsActive && x.Name.Trim().ToUpper().Equals(configName));
                    if (cf != null)
                        value = cf.ValueDefault.Trim();
                }
                else
                    value = cf.Value.Trim();
            }
            catch (Exception)
            {
            }
            return value;
        }

        public List<AppConfigModel> GetAll(int appId)
        {
            var configs = new List<AppConfigModel>();
            try
            {
                var db = new PMSEntities();
                configs.AddRange(db.Configs.Where(x => x.IsActive).Select(x => new AppConfigModel() { Id = x.Id, Name = x.Name, DisplayName = x.DisplayName, Value = x.ValueDefault, Description = x.Description }).ToList());
                var configApp = db.Config_App.Where(x => x.AppId == appId).ToList();

                if (configs.Count > 0 && configApp.Count > 0)
                {
                    foreach (var item in configApp)
                    {
                        var obj = configs.FirstOrDefault(x => x.Id == item.ConfigId);
                        if (obj != null)
                            obj.Value = item.Value;
                    }
                }
            }
            catch (Exception)
            {
            }
            return configs;
        }

        public List<ShowLCD_Config> GetShowLCDConfig()
        {
            try
            {
                var db = new PMSEntities();
                return db.ShowLCD_Config.ToList();
            }
            catch (Exception)
            {
            }
            return null;
        }

        public ShowLCD_Config GetShowLCDConfigByName(string configName)
        {
            try
            {
                var db = new PMSEntities();
                return db.ShowLCD_Config.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(configName));
            }
            catch (Exception)
            {
            }
            return null;
        }

        public bool UpdateComport(int AppId, string comName, int baudRate, int dataBit, int parity, int stopBit, bool IsKeyPad)
        {
            try
            {
                var db = new PMSEntities();
                var configs = db.Configs;
                var configApp = db.Config_App.Where(x => x.AppId == AppId);
                if (IsKeyPad)
                {
                    #region COM KEYPAD
                    // comname
                    var cf = configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals("COM2"));
                    if (cf != null)
                    {
                        var cfApp = configApp.FirstOrDefault(x => x.ConfigId == cf.Id);
                        if (cfApp != null)
                            cfApp.Value = comName;
                        else
                            cf.ValueDefault = comName;
                    }
                    //
                    cf = configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals("BaudRate2"));
                    if (cf != null)
                    {
                        var cfApp = configApp.FirstOrDefault(x => x.ConfigId == cf.Id);
                        if (cfApp != null)
                            cfApp.Value = baudRate.ToString();
                        else
                            cf.ValueDefault = baudRate.ToString();
                    }
                    //
                    cf = configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals("DataBits2"));
                    if (cf != null)
                    {
                        var cfApp = configApp.FirstOrDefault(x => x.ConfigId == cf.Id);
                        if (cfApp != null)
                            cfApp.Value = dataBit.ToString();
                        else
                            cf.ValueDefault = dataBit.ToString();
                    }
                    //
                    cf = configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals("Parity2"));
                    if (cf != null)
                    {
                        var cfApp = configApp.FirstOrDefault(x => x.ConfigId == cf.Id);
                        if (cfApp != null)
                            cfApp.Value = parity.ToString();
                        else
                            cf.ValueDefault = parity.ToString();
                    }
                    //
                    cf = configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals("StopBits2"));
                    if (cf != null)
                    {
                        var cfApp = configApp.FirstOrDefault(x => x.ConfigId == cf.Id);
                        if (cfApp != null)
                            cfApp.Value = stopBit.ToString();
                        else
                            cf.ValueDefault = stopBit.ToString();
                    }
                    #endregion
                }
                else
                {
                    #region COM TABLE
                    // comname
                    var cf = configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals("COM"));
                    if (cf != null)
                    {
                        var cfApp = configApp.FirstOrDefault(x => x.ConfigId == cf.Id);
                        if (cfApp != null)
                            cfApp.Value = comName;
                        else
                            cf.ValueDefault = comName;
                    }
                    //
                    cf = configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals("BaudRate"));
                    if (cf != null)
                    {
                        var cfApp = configApp.FirstOrDefault(x => x.ConfigId == cf.Id);
                        if (cfApp != null)
                            cfApp.Value = baudRate.ToString();
                        else
                            cf.ValueDefault = baudRate.ToString();
                    }
                    //
                    cf = configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals("DataBits"));
                    if (cf != null)
                    {
                        var cfApp = configApp.FirstOrDefault(x => x.ConfigId == cf.Id);
                        if (cfApp != null)
                            cfApp.Value = dataBit.ToString();
                        else
                            cf.ValueDefault = dataBit.ToString();
                    }
                    //
                    cf = configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals("Parity"));
                    if (cf != null)
                    {
                        var cfApp = configApp.FirstOrDefault(x => x.ConfigId == cf.Id);
                        if (cfApp != null)
                            cfApp.Value = parity.ToString();
                        else
                            cf.ValueDefault = parity.ToString();
                    }
                    //
                    cf = configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals("StopBits"));
                    if (cf != null)
                    {
                        var cfApp = configApp.FirstOrDefault(x => x.ConfigId == cf.Id);
                        if (cfApp != null)
                            cfApp.Value = stopBit.ToString();
                        else
                            cf.ValueDefault = stopBit.ToString();
                    }
                    #endregion
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        public ConfigModel GetConfig(int tableType)
        {
            var db = new PMSEntities();
            var cfObj = new ConfigModel();
            cfObj.Configs = db.ShowLCD_Config.ToList();
            cfObj.Panels = db.ShowLCD_Panel.Where(x => x.TableType == tableType).ToList();
            cfObj.LabelNames = db.ShowLCD_LabelForPanelContent.Where(x => x.TableType == tableType && x.IsShow).ToList();
            cfObj.ColumnConfigs = db.ShowLCD_TableLayoutPanel.Where(x => x.TableType == tableType).ToList();
            cfObj.LabelConfigs = db.ShowLCD_LabelArea.Where(x => x.TableType == tableType).ToList();
            return cfObj;
        }
 
        public bool InsertOrUpdate(List<AppConfigModel> models, int AppId)
        {
            try
            {
                var db = new PMSEntities();
                var configs = db.Configs;
                var appConfigs = db.Config_App.Where(x => x.AppId == AppId);
                if (configs != null && configs.Count() > 0)
                {
                    AppConfigModel obj;
                    Config_App cfAppObj;
                    foreach (var item in configs)
                    {
                        obj = models.FirstOrDefault(x => x.Id == item.Id);
                        if (obj != null)
                        {
                            item.DisplayName = obj.DisplayName;
                            item.Description = obj.Description;

                            cfAppObj = appConfigs.FirstOrDefault(x => x.ConfigId == item.Id);
                            if (cfAppObj != null)
                                cfAppObj.Value = obj.Value;
                            else
                            {
                                cfAppObj = new Config_App();
                                cfAppObj.AppId = AppId;
                                cfAppObj.ConfigId = item.Id;
                                cfAppObj.Value = obj.Value;
                                db.Config_App.Add(cfAppObj);
                            }
                        }
                    }
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {               
                throw ex;
            } 
            return false;
        }

    }
}
