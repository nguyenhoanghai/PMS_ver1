using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Configuration;
using DuAn03_HaiDang.DATAACCESS;
using System.Diagnostics;
using System.IO;
using System.Data.Sql;

namespace DuAn03_HaiDang
{
    public partial class frmCaiDat : FormBase
    {
        public frmCaiDat()
        {
            InitializeComponent();
            loadInformationCOM();
            txtUsername.Enabled = false;
            txtPassword.Enabled = false;
            var NSType = dbclass.listAppConfig.Where(c => c.Name.Trim().ToUpper().Equals("NSTYPE")).Select(c => c.Value).FirstOrDefault().ToString();
            if (NSType == "0")
            {
                checkDMTTD.Checked = false;
            }
            else
            {
                checkDMTTD.Checked = true;
            }
        }
        private void loadInformationCOM()
        {
            string[] ports = SerialPort.GetPortNames();
            cbbCOM.Items.AddRange(ports);
            cbbCOM2.Items.AddRange(ports);
            string[] BaudRate = { "1200", "2400", "4800", "9600", "19200", "38400", "57600", "115200" };
            cbbBaudRate.Items.AddRange(BaudRate);
            cbbBaudRate2.Items.AddRange(BaudRate);
            string[] Databits = { "6", "7", "8" };
            cbbDataBits.Items.AddRange(Databits);
            cbbDataBits2.Items.AddRange(Databits);
            string[] Parity = { "none", "odd", "even" };
            cbbParity.Items.AddRange(Parity);
            cbbParity2.Items.AddRange(Parity);
            string[] StopBit = { "1", "1.5", "2" };
            cbbStopBit.Items.AddRange(StopBit);
            cbbStopBit2.Items.AddRange(StopBit);
        }
        private void loadTTCOMPort()
        {

            cbbCOM.Text = dbclass.listAppConfig.Where(c => c.Name.Trim().ToUpper().Equals("COM")).Select(c => c.Value).FirstOrDefault().ToString();
            cbbBaudRate.Text = dbclass.listAppConfig.Where(c => c.Name.Trim().ToUpper().Equals("BAUDRATE")).Select(c => c.Value).FirstOrDefault().ToString();
            cbbDataBits.Text = dbclass.listAppConfig.Where(c => c.Name.Trim().ToUpper().Equals("DATABITS")).Select(c => c.Value).FirstOrDefault().ToString();
            string parity = dbclass.listAppConfig.Where(c => c.Name.Trim().ToUpper().Equals("PARITY")).Select(c => c.Value).FirstOrDefault().ToString();
            switch (parity)
            {
                case "None":
                    cbbParity.Text = "None";
                    break;
                case "Odd":
                    cbbParity.Text = "Odd";
                    break;
                case "Even":
                    cbbParity.Text = "Even";
                    break;
            }

            string StopBit = dbclass.listAppConfig.Where(c => c.Name.Trim().ToUpper().Equals("STOPBITS")).Select(c => c.Value).FirstOrDefault().ToString();
            switch (StopBit)
            {
                case "One":
                    cbbStopBit.Text = "1";
                    break;
                case "OnePointFive":
                    cbbStopBit.Text = "1.5";
                    break;
                case "Two":
                    cbbStopBit.Text = "2";
                    break;
            }
            // Load information COM2 
            cbbCOM2.Text = dbclass.listAppConfig.Where(c => c.Name.Trim().ToUpper().Equals("COM2")).Select(c => c.Value).FirstOrDefault().ToString();
            cbbBaudRate2.Text = dbclass.listAppConfig.Where(c => c.Name.Trim().ToUpper().Equals("BAUDRATE2")).Select(c => c.Value).FirstOrDefault().ToString();
            cbbDataBits2.Text = dbclass.listAppConfig.Where(c => c.Name.Trim().ToUpper().Equals("DATABITS2")).Select(c => c.Value).FirstOrDefault().ToString();
            string parity2 = dbclass.listAppConfig.Where(c => c.Name.Trim().ToUpper().Equals("PARITY2")).Select(c => c.Value).FirstOrDefault().ToString();
            switch (parity2)
            {
                case "None":
                    cbbParity2.Text = "None";
                    break;
                case "Odd":
                    cbbParity2.Text = "Odd";
                    break;
                case "Even":
                    cbbParity2.Text = "Even";
                    break;
            }

            string StopBit2 = dbclass.listAppConfig.Where(c => c.Name.Trim().ToUpper().Equals("STOPBITS2")).Select(c => c.Value).FirstOrDefault().ToString();
            switch (StopBit2)
            {
                case "One":
                    cbbStopBit2.Text = "1";
                    break;
                case "OnePointFive":
                    cbbStopBit2.Text = "1.5";
                    break;
                case "Two":
                    cbbStopBit2.Text = "2";
                    break;
            }


        }
        
        private void SendData(string value)
        {
            try
            {
                if (FrmMainNew.P.IsOpen)
                {
                    string strCS = "";
                    strCS = clsString.XOR(value);
                    value = value + strCS;
                    value = "02" + clsString.Ascii2HexStringNull(value) + "03";
                    byte[] newMsg = HexStringToByteArray(value);
                    //send the message to the port
                    FrmMainNew.P.Write(newMsg, 0, newMsg.Length);
                }
                else
                {
                    MessageBox.Show("Không có kết nối đến cổng COM Bảng", "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
            catch
            {

            }
        }

        private void SendRequest(string value)
        {
            try
            {
                if (FrmMainNew.P2.IsOpen)
                {
                    string strCS = "";
                    strCS = clsString.XOR(value);
                    value = value + strCS;
                    value = "02" + clsString.Ascii2HexStringNull(value) + "03";
                    byte[] newMsg = HexStringToByteArray(value);
                    //send the message to the port
                    FrmMainNew.P2.Write(newMsg, 0, newMsg.Length);
                }
                else
                {
                    MessageBox.Show("Không có kết nối đến cổng COM Keypad", "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
            catch
            {

            }
        }
        private byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            if (s.Length % 2 != 0)
                return new byte[] { 0x00 };
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }
        private void btnGuiLenh_Click(object sender, EventArgs e)
        {
            if (txtNoiDungGuiTable.Text != "")
            {
                SendData(txtNoiDungGuiKeyPad.Text);
            }
            if (txtNoiDungGuiKeyPad.Text != "")
            {
                SendRequest(txtNoiDungGuiKeyPad.Text);
            }
        }

        private void btnLuuKetNoi_Click(object sender, EventArgs e)
        {
            try
            {
                //frmMain.P.PortName = cbbCOM.Text;
                //frmMain.P.BaudRate = Convert.ToInt32(cbbBaudRate.Text);
                //frmMain.P.DataBits = Convert.ToInt32(cbbDataBits.Text);
                //string parity = cbbParity.Text;
                //switch (parity)
                //{
                //    case "None":
                //        frmMain.P.Parity = Parity.None;
                //        break;
                //    case "Odd":
                //        frmMain.P.Parity = Parity.Odd;
                //        break;
                //    case "Even":
                //        frmMain.P.Parity = Parity.Even;
                //        break;
                //}

                //string StopBit = cbbStopBit.Text;
                //switch (StopBit)
                //{
                //    case "One":
                //        frmMain.P.StopBits = StopBits.One;
                //        break;
                //    case "OnePointFive":
                //        frmMain.P.StopBits = StopBits.OnePointFive;
                //        break;
                //    case "Two":
                //        frmMain.P.StopBits = StopBits.Two;
                //        break;
                //}
                //frmMain.P.RtsEnable = true;               
                //frmMain.P.Open();
                
                                   
                Configuration _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                _config.AppSettings.Settings["COM"].Value = cbbCOM.Text;
                _config.AppSettings.Settings["BaudRate"].Value = cbbBaudRate.Text;
                _config.AppSettings.Settings["DataBits"].Value = cbbDataBits.Text;
                _config.AppSettings.Settings["Parity"].Value = cbbParity.Text;
                _config.AppSettings.Settings["StopBits"].Value = cbbStopBit.Text;
                // Information COM2 port
                _config.AppSettings.Settings["COM2"].Value = cbbCOM2.Text;
                _config.AppSettings.Settings["BaudRate2"].Value = cbbBaudRate2.Text;
                _config.AppSettings.Settings["DataBits2"].Value = cbbDataBits2.Text;
                _config.AppSettings.Settings["Parity2"].Value = cbbParity2.Text;
                _config.AppSettings.Settings["StopBits2"].Value =cbbStopBit2.Text;
                _config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                MessageBox.Show("Thay đổi thông tin cổng COM thành công, Bạn cần khởi động lại chương trình để áp dụng cấu hình mới", "Thay đổi thành công", MessageBoxButtons.OK, MessageBoxIcon.None);
                           
            }
            catch (Exception)
            {

                MessageBox.Show("Không thể thay đổi thông tin cổng COM, Vui lòng thử lại", "Kết quả thay đổi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            
            if (cbbServerName.Text == "")
            {
                MessageBox.Show("Vui lòng nhập Servername", "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbbServerName.Focus();
            }
            else
            {
                if (cboDatabase.Text == "")
                {
                    MessageBox.Show("Vui lòng nhập tên Database", "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cboDatabase.Focus();
                }
                else
                {
                    string servername = servername = cbbServerName.Text;
                    string Database = cboDatabase.Text;
                    if (cbbAuthentication.Text == "Windows Authentication")
                    {
                        Configuration _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        _config.AppSettings.Settings["Server"].Value = servername;
                        _config.AppSettings.Settings["Database"].Value = Database;
                        _config.AppSettings.Settings["Username"].Value = "";
                        _config.AppSettings.Settings["Password"].Value = "";
                        _config.Save(ConfigurationSaveMode.Modified);
                        ConfigurationManager.RefreshSection("appSettings");

                    }
                    else
                    {
                        Configuration _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        _config.AppSettings.Settings["Server"].Value = servername;
                        _config.AppSettings.Settings["Database"].Value = Database;
                        _config.AppSettings.Settings["Username"].Value = txtUsername.Text;
                        _config.AppSettings.Settings["Password"].Value = txtPassword.Text;
                        _config.Save(ConfigurationSaveMode.Modified);
                        ConfigurationManager.RefreshSection("appSettings");

                    }
                }
            }

            FrmMainNew.sqlCon = dbclass.taoketnoi();
            if (FrmMainNew.sqlCon.State == ConnectionState.Open)
            {
                MessageBox.Show("Kết nối thành công", "Kết nối", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Không thể kết nối với CSDL", "Kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmCaiDat_Load(object sender, EventArgs e)
        {
            //Load defaut imformation to control
            loadTTCOMPort();
            string strServer = dbclass.server;
            string strData = dbclass.data;
            string strUser = dbclass.user;
            string strPass = dbclass.pass;
            cbbServerName.Text = strServer;
            cboDatabase.Text = strData;
            if (strUser != "")
            {
                cbbAuthentication.Text = "SQL Sever Authentication";
                txtUsername.Enabled = true;
                txtUsername.Text = strUser;
                txtPassword.Enabled = true;
                txtPassword.Text = strPass;
            }
            
            
        }
        private void butHuyThayDoiHienThiDen_Click(object sender, EventArgs e)
        {
            lblSTTDen.Text = "";
            txtMauDen.Text = "";
            nudTyLeTu.Value = 0;
            nudTyLeDen.Value = 0;
        }

        private void cbbServerName_DropDown(object sender, EventArgs e)
        {
            if (cbbServerName.Items.Count <= 0)
            {
                string myServer = Environment.MachineName;
                DataTable servers = SqlDataSourceEnumerator.Instance.GetDataSources();
                for (int i = 0; i < servers.Rows.Count; i++)
                {
                    if (myServer == servers.Rows[i]["ServerName"].ToString())
                    {
                        cbbServerName.Items.Add(servers.Rows[i]["ServerName"] + "\\" + servers.Rows[i]["InstanceName"]);

                    }
                    else
                    {
                        cbbServerName.Items.Add(servers.Rows[i]["ServerName"]);
                    }
                }
                cbbServerName.SelectedIndex = 0;
            }
            
        }

        private void cbbAuthentication_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbAuthentication.SelectedIndex == 0)
            {
                txtUsername.Enabled = false;
                txtPassword.Enabled = false;
            }
            else
            {
                txtUsername.Enabled = true;
                txtPassword.Enabled = true;
            }
        }

        private void cboDatabase_DropDown(object sender, EventArgs e)
        {
            if (cboDatabase.Items.Count <= 0)
            {
                string strConnect = "";
                if (cbbAuthentication.SelectedIndex == 0)
                {
                    strConnect = @"Data Source=" + cbbServerName.Text + ";Integrated Security=True;";
                }
                else
                {
                    strConnect = @"Data Source=" + cbbServerName.Text + "; User Id=" + txtUsername.Text + ";Password=" + txtPassword.Text + ";";
                }
                try
                {
                    dbclass.taoketnoi(strConnect);
                    string strSQL = "SELECT name FROM master..sysdatabases";
                    DataTable dt = new DataTable();
                    dt = dbclass.TruyVan_TraVe_DataTable(strSQL);
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
                    MessageBox.Show("Không thể kết nối với Server...", "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void butSaveSetting_Click(object sender, EventArgs e)
        {
            try
            {                
                Configuration _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (checkDMTTD.Checked)
                {
                    _config.AppSettings.Settings["NSType"].Value = "1";
                }
                else
                {
                    _config.AppSettings.Settings["NSType"].Value = "0";
                }              
                
                _config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                MessageBox.Show("Lưu cài đặt thành công, Bạn cần khởi động lại chương trình để áp dụng cấu hình mới", "Thay đổi thành công", MessageBoxButtons.OK, MessageBoxIcon.None);

            }
            catch (Exception)
            {

                MessageBox.Show("Không thể lưu thông tin cài đặt, Vui lòng thử lại", "Kết quả thay đổi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       

        
        
    }
}
