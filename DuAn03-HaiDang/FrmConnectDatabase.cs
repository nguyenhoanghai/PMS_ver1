using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.Sql;
using System.Configuration;
using System.Data.SqlClient;
using DuAn03_HaiDang.DATAACCESS;

namespace DuAn03_HaiDang
{
    public partial class FrmConnectDatabase : DevExpress.XtraEditors.XtraForm
    {
        public FrmConnectDatabase()
        {
            InitializeComponent();
        }

        private void FrmConnectDatabase_Load(object sender, EventArgs e)
        {
            try
            {
                cboServerName.Text = dbclass.server;
                cboDatabase.Text = dbclass.data;
                if (string.IsNullOrEmpty(dbclass.user) && string.IsNullOrEmpty(dbclass.pass))
                {
                    cboAuthenticaion.SelectedIndex = 0;
                    txtPassword.Enabled = false;
                    txtUsername.Enabled = false;
                }
                else
                {
                    cboAuthenticaion.SelectedIndex = 1;
                    txtUsername.Text = dbclass.user;
                    txtPassword.Enabled = true;
                    txtUsername.Enabled = true;
                }   
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: "+ex.Message);
            }           
        }

        private void cboServerName_DropDown(object sender, EventArgs e)
        {
            if (cboServerName.Items.Count <= 0)
            {
                string myServer = Environment.MachineName;
                DataTable servers = SqlDataSourceEnumerator.Instance.GetDataSources();
                if (servers != null && servers.Rows != null && servers.Rows.Count>0)
                {
                    for (int i = 0; i < servers.Rows.Count; i++)
                    {
                        if (myServer == servers.Rows[i]["ServerName"].ToString())
                        {
                            cboServerName.Items.Add(servers.Rows[i]["ServerName"] + "\\" + servers.Rows[i]["InstanceName"]);

                        }
                        else
                        {
                            cboServerName.Items.Add(servers.Rows[i]["ServerName"]);
                        }
                    }
                    cboServerName.SelectedIndex = 0;
                }
            }
            
           
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboDatabase_DropDown(object sender, EventArgs e)
        {
            if (cboDatabase.Items.Count <= 0)
            {
                string strConnect = "";
                if (cboAuthenticaion.SelectedIndex == 0)
                {
                    strConnect = @"Data Source=" + cboServerName.Text + ";Integrated Security=True;";
                }
                else
                {
                    strConnect = @"Data Source=" + cboServerName.Text + "; User Id=" + txtUsername.Text + ";Password=" + txtPassword.Text + ";";
                }
                try
                {
                    var con = dbclass.taoketnoi(strConnect);
                    string strSQL = "SELECT name FROM master..sysdatabases";
                    DataTable dt = new DataTable();
                    dt = dbclass.TruyVan_TraVe_DataTable_Con(strSQL, con);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            cboDatabase.Items.Add(dr[0].ToString());
                        }
                        cboDatabase.SelectedIndex = 0;
                    }
                }
                catch (Exception)
                {
                    XtraMessageBox.Show("Không thể kết nối với Server...","Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }  
                
            }
        }

        private void cboAuthenticaion_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cboAuthenticaion.SelectedIndex == 0)
            {
                txtPassword.Enabled = false;
                txtUsername.Enabled = false;
            }
            else
            {
                txtPassword.Enabled = true;
                txtUsername.Enabled = true;
            }
        }

        private void butConnect_Click(object sender, EventArgs e)
        {
            string strConnect = "";
            if (cboAuthenticaion.SelectedIndex == 0)
            {
                strConnect = @"Data Source=" + cboServerName.Text + ";Initial Catalog=" + cboDatabase.Text + ";Integrated Security=True;";
            }
            else
            {
                strConnect = @"Data Source=" + cboServerName.Text + ";Initial Catalog=" + cboDatabase.Text + "; User Id=" + txtUsername.Text + ";Password=" + txtPassword.Text + ";";
            }
            try
            {
                SqlConnection con = dbclass.taoketnoi(strConnect);
                if (con != null)
                {
                    Configuration _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    _config.AppSettings.Settings["Server"].Value = dbclass.EncryptString(cboServerName.Text, dbclass.password);
                    _config.AppSettings.Settings["Database"].Value = dbclass.EncryptString(cboDatabase.Text, dbclass.password); ;
                    _config.AppSettings.Settings["Username"].Value = dbclass.EncryptString(txtUsername.Text, dbclass.password); 
                    _config.AppSettings.Settings["Password"].Value = dbclass.EncryptString(txtPassword.Text, dbclass.password);
                    _config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                    Application.Restart();
                }
                else
                {
                    XtraMessageBox.Show("Không thể kết nối với Server...", "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: "+ex.Message);
            }
            
        }

       
    }
}