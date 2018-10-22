using DuAn03_HaiDang;
using PMS.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using PMS.Business.Enum;
using PMS.Business.Models;


namespace QuanLyNangSuat
{
    public partial class FrmCOMStatus : Form
    {
        int AppId = 0;
        public FrmCOMStatus()
        {
            InitializeComponent();
        }
        private void FrmCOMStatus_Load(object sender, EventArgs e)
        {
            Init();
        }
        private void Init()
        {
            int.TryParse(ConfigurationManager.AppSettings["AppId"].ToString(), out AppId);
            var configs = BLLConfig.Instance.GetAll(AppId);
            foreach (string s in SerialPort.GetPortNames())
            {
                cb_com.Items.Add(new SelectListItem() { Text = s, Value = s });
                cb_com_tb.Items.Add(new SelectListItem() { Text = s, Value = s });
            }

            var parities = ("None,Odd,Even,Mark,Space").Split(',').ToArray();
            for (int i = 0; i < 5; i++)
            {
                cbparity.Items.Add(new SelectListItem() { Text = parities[i], Value = i });
                cb_parity_tb.Items.Add(new SelectListItem() { Text = parities[i], Value = i });
            }

            var stopbits = ("None,One,Two,OnePointFive").Split(',').ToArray();
            for (int i = 0; i < 4; i++)
            {
                cb_stopbit_tb.Items.Add(new SelectListItem() { Text = stopbits[i], Value = i });
                cb_stopbit.Items.Add(new SelectListItem() { Text = stopbits[i], Value = i });
            }
            cb_com.DisplayMember = "Text";
            cb_com_tb.DisplayMember = "Text";
            cbparity.DisplayMember = "Text";
            cb_parity_tb.DisplayMember = "Text";
            cb_stopbit_tb.DisplayMember = "Text";
            cb_stopbit.DisplayMember = "Text";

            // COM BANG
            var cf = configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eAppConfigName.COM));
            lb_current_tb.Text = cf != null ? cf.Value : string.Empty;
            cf = configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eAppConfigName.BAUDRATE));
            txt_baudrate_tb.Value = cf != null ? int.Parse(cf.Value) : 0;
            cf = configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eAppConfigName.DATABITS));
            txt_databit_tb.Value = cf != null ? int.Parse(cf.Value) : 0;
            cf = configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eAppConfigName.STOPBITS));
            cb_stopbit_tb.SelectedIndex = cf != null ? int.Parse(cf.Value) : 0;
            cf = configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eAppConfigName.PARITY));
            try
            {
                cb_parity_tb.SelectedIndex = cf != null ? int.Parse(cf.Value) : 0;
            }
            catch (Exception)
            {
                cb_parity_tb.SelectedIndex = 0;
            }

            if (FrmMainNew.P.IsOpen)
                tb_status_tb.Text = "COM Bảng đã kết nối";
            else
                tb_status_tb.Text = "COM Bảng không kết nối.";

            //COM KEYPAD
            cf = configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eAppConfigName.COM2));
            lb_com_current.Text = cf != null ? cf.Value : string.Empty;
            cf = configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eAppConfigName.BAUDRATE2));
            txtbaudrate.Value = cf != null ? int.Parse(cf.Value) : 0;
            cf = configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eAppConfigName.DATABITS2));
            txtdatabit.Value = cf != null ? int.Parse(cf.Value) : 0;
            cf = configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eAppConfigName.STOPBITS2));
            cb_stopbit.SelectedIndex = cf != null ? int.Parse(cf.Value) : 0;
            cf = configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eAppConfigName.PARITY2));
            try
            {
                cbparity.SelectedIndex = cf != null ? int.Parse(cf.Value) : 0;
            }
            catch (Exception)
            {
                cbparity.SelectedIndex = 0;
            }
            if (FrmMainNew.P2.IsOpen)
                lbstatus.Text = "COM KeyPad đã kết nối";
            else
                lbstatus.Text = "COM KeyPad không kết nối.";
        }

        #region COM TABLE
        private void butCheckComTableStatus_Click(object sender, EventArgs e)
        {
            try
            {
                if (FrmMainNew.P.IsOpen)
                    tb_status_tb.Text = "COM Bảng đã kết nối";
                else
                    tb_status_tb.Text = "COM Bảng không kết nối.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi:" + ex.Message);
            }
        }

        private void butTurnOffComTable_Click(object sender, EventArgs e)
        {
            try
            {
                FrmMainNew.P.Close();
                tb_status_tb.Text = "COM Bảng không kết nối.";
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi:" + FrmMainNew.P.PortName + " không tồn tại hoặc đã được kết nối. Vui lòng kiểm tra lại.");
            }
        }

        private void butTurnOnComTable_Click(object sender, EventArgs e)
        {
            try
            {
                if (!FrmMainNew.P.IsOpen)
                {
                    FrmMainNew.P.Open();
                    tb_status_tb.Text = "COM Bảng đã kết nối";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi:" + FrmMainNew.P.PortName + " không tồn tại hoặc đã được kết nối. Vui lòng kiểm tra lại.");
            }
        }

        #endregion

        #region COM KEYPAD
        private void butCheckComKPStatus_Click(object sender, EventArgs e)
        {
            try
            {
                if (FrmMainNew.P2.IsOpen)
                    lbstatus.Text = "COM KeyPad đã kết nối";
                else
                    lbstatus.Text = "COM KeyPad không kết nối.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi:" + ex.Message);
            }
        }

        private void butTurnOffCOMKP_Click(object sender, EventArgs e)
        {
            try
            {
                FrmMainNew.P2.Close();
                lbstatus.Text = "COM KeyPad không kết nối.";
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi:" + FrmMainNew.P.PortName + " không tồn tại hoặc đã được kết nối. Vui lòng kiểm tra lại.");
            }
        }

        private void butTurnOnCOMKP_Click(object sender, EventArgs e)
        {
            try
            {
                if (!FrmMainNew.P2.IsOpen)
                {
                    FrmMainNew.P2.Open();
                    lbstatus.Text = "COM KeyPad đã kết nối";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi:" + FrmMainNew.P2.PortName + " không tồn tại hoặc đã được kết nối. Vui lòng kiểm tra lại.");
            }
        }

        #endregion

        private void btn_save_tb_Click(object sender, EventArgs e)
        {
            try
            {
                FrmMainNew.P.Close();
                string comName = ((SelectListItem)cb_com_tb.SelectedItem).Text;
                int baudRate = (int)txt_baudrate_tb.Value, dataBit = (int)txt_databit_tb.Value, parity = (int)((SelectListItem)cb_parity_tb.SelectedItem).Value, stopBit = (int)((SelectListItem)cb_stopbit_tb.SelectedItem).Value;
                var rs = BLLConfig.Instance.UpdateComport(AppId, comName, baudRate, dataBit, parity, stopBit, false);
                if (rs)
                {
                    FrmMainNew.P.PortName = comName;
                    FrmMainNew.P.BaudRate = baudRate;
                    FrmMainNew.P.DataBits = dataBit;
                    switch (parity)
                    {
                        case 0: FrmMainNew.P.Parity = Parity.None; break;
                        case 1: FrmMainNew.P.Parity = Parity.Odd; break;
                        case 2: FrmMainNew.P.Parity = Parity.Even; break;
                        case 3: FrmMainNew.P.Parity = Parity.Mark; break;
                        case 4: FrmMainNew.P.Parity = Parity.Space; break;
                    }
                    switch (stopBit)
                    {
                        case 0: FrmMainNew.P.StopBits = StopBits.None; break;
                        case 1: FrmMainNew.P.StopBits = StopBits.One; break;
                        case 2: FrmMainNew.P.StopBits = StopBits.Two; break;
                        case 3: FrmMainNew.P.StopBits = StopBits.OnePointFive; break;
                    }
                    FrmMainNew.P.Open();
                    lb_current_tb.Text = FrmMainNew.P.PortName;
                    tb_status_tb.Text = "COM Bảng đã kết nối";
                }
                else
                    MessageBox.Show("Lỗi: Lưu thông tin không thành công. Vui lòng kiểm tra lại.");
            }
            catch (Exception)
            {
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                FrmMainNew.P2.Close();
                string comName = ((SelectListItem)cb_com.SelectedItem).Text;
                int baudRate = (int)txtbaudrate.Value, dataBit = (int)txtdatabit.Value, parity = (int)((SelectListItem)cbparity.SelectedItem).Value, stopBit = (int)((SelectListItem)cb_stopbit.SelectedItem).Value;
                var rs = BLLConfig.Instance.UpdateComport(AppId, comName, baudRate, dataBit, parity, stopBit, true);
                if (rs)
                {
                    FrmMainNew.P2.PortName = comName;
                    FrmMainNew.P2.BaudRate = baudRate;
                    FrmMainNew.P2.DataBits = dataBit;
                    switch (parity)
                    {
                        case 0: FrmMainNew.P2.Parity = Parity.None; break;
                        case 1: FrmMainNew.P2.Parity = Parity.Odd; break;
                        case 2: FrmMainNew.P2.Parity = Parity.Even; break;
                        case 3: FrmMainNew.P2.Parity = Parity.Mark; break;
                        case 4: FrmMainNew.P2.Parity = Parity.Space; break;
                    }
                    switch (stopBit)
                    {
                        case 0: FrmMainNew.P2.StopBits = StopBits.None; break;
                        case 1: FrmMainNew.P2.StopBits = StopBits.One; break;
                        case 2: FrmMainNew.P2.StopBits = StopBits.Two; break;
                        case 3: FrmMainNew.P2.StopBits = StopBits.OnePointFive; break;
                    }
                    FrmMainNew.P2.Open();
                    lb_com_current.Text = FrmMainNew.P2.PortName;
                    lbstatus.Text = "COM KeyPad đã kết nối";
                }
                else
                    MessageBox.Show("Lỗi: Lưu thông tin không thành công. Vui lòng kiểm tra lại.");
            }
            catch (Exception)
            { }
        }
    }
}
