using DuAn03_HaiDang.DAO;
using DuAn03_HaiDang.POJO;
using PMS.Business;
using PMS.Business.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DuAn03_HaiDang
{
    public partial class FrmConfig : FormBase
    { 
        private int appId = 0;

        public FrmConfig()
        {
            InitializeComponent(); 
        }

        private void FrmConfig_Load(object sender, EventArgs e)
        {
            try
            {
                txtAppId.Text = ConfigurationManager.AppSettings["AppId"].ToString();
                int.TryParse(txtAppId.Text, out appId);

                dgvListConfig.DataSource = null;
                dgvListConfig.DataSource = BLLConfig.Instance.GetAll(appId); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
  
        private void butSave_Click(object sender, EventArgs e)
        {
            List<AppConfigModel> listModel = null;
            try
            {
                if (dgvListConfig.RowCount > 0)
                {
                    listModel = new List<AppConfigModel>();
                    foreach (DataGridViewRow row in dgvListConfig.Rows)
                    {
                        var model = new AppConfigModel();
                        model.Id = int.Parse(row.Cells["Id"].Value.ToString());
                        model.DisplayName = row.Cells["DisplayName"].Value.ToString();
                        model.Name = row.Cells["Name"].Value.ToString();
                        model.Description = row.Cells["Description"].Value != null ? row.Cells["Description"].Value.ToString() : "";
                        model.Value = row.Cells["Value"].Value != null ? row.Cells["Value"].Value.ToString() : "";
                        listModel.Add(model);
                    }
                }

                if (listModel != null && listModel.Count > 0)
                {
                    var result = false;
                    if (appId == 0)
                    {
                        result = BLLConfig.Instance.InsertOrUpdate(listModel, appId);
                        Configuration _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        _config.AppSettings.Settings["AppId"].Value = appId.ToString();
                        _config.Save(ConfigurationSaveMode.Modified);
                        ConfigurationManager.RefreshSection("appSettings");
                        result = true;
                    }
                    else
                        result = BLLConfig.Instance.InsertOrUpdate(listModel, appId);
                    
                    if (result)
                    {
                        MessageBox.Show("Lưu thông tin thành công. Hệ thống sẽ khởi động lại");
                        Application.Restart();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
