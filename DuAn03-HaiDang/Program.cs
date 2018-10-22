using DevExpress.LookAndFeel;
using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.Model;
using QuanLyNangSuat;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using SystemGPRO.Serial;

namespace DuAn03_HaiDang
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (AccountSuccess.TenTK == null)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                DevExpress.Skins.SkinManager.EnableFormSkins();
                DevExpress.UserSkins.BonusSkins.Register();
                UserLookAndFeel.Default.SetSkinStyle("DevExpress Style");
            }
            try
            {
                //Check active
                SerialKey serialKey = new SerialKey();
                var now = DateTime.Now;
                ModelStatic.dateCheckActive =new DateTime(now.Year, now.Month, now.Day) ;
                ModelCheckKey modelCheckKey = serialKey.CheckActive("GPRO_Product_0001", Application.StartupPath); 
                if (modelCheckKey != null) 
                {
                    if (!modelCheckKey.checkResult)
                    {
                        if (!string.IsNullOrEmpty(modelCheckKey.message))
                            MessageBox.Show(modelCheckKey.message);
                        else
                            MessageBox.Show("Phần mềm chưa được kích hoạt. Vui lòng liên hệ với nhà cung cấp để kích hoạt sử dụng.");
                        Application.Exit();
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(modelCheckKey.message))
                            MessageBox.Show(modelCheckKey.message);
                        SqlConnection connect = null;
                        try
                        {
                            dbclass.server = dbclass.DecryptString(ConfigurationManager.AppSettings["server"].ToString(), dbclass.password);
                            dbclass.data = dbclass.DecryptString(ConfigurationManager.AppSettings["database"].ToString(), dbclass.password);
                            dbclass.user = dbclass.DecryptString(ConfigurationManager.AppSettings["Username"].ToString(), dbclass.password);
                            dbclass.pass = dbclass.DecryptString(ConfigurationManager.AppSettings["Password"].ToString(), dbclass.password);
                            connect = dbclass.taoketnoi();
                        }
                        catch (Exception)
                        { }
                        if (connect != null)
                        {
                            int appId = 0;
                            int.TryParse(ConfigurationManager.AppSettings["AppId"].ToString(), out appId);
                            if (appId > 0)
                            {
                                Application.Run(new frmDangNhap());

                                if (!string.IsNullOrEmpty(AccountSuccess.TenTK))
                                {
                                    if (AccountSuccess.IsOwner)
                                    {
                                        if (AccountSuccess.IsCompleteAcc)
                                            Application.Run(new frmMainComplete());
                                        else
                                            Application.Run(new FrmMainNew());
                                    }
                                    else
                                        Application.Run(new frmMainShow());



                                    if (AccountSuccess.TenTK.Equals("(none)"))
                                        Main();
                                    else
                                    {
                                        Process[] processe;
                                        processe = Process.GetProcessesByName("QuanLyNangSuat");
                                        foreach (Process dovi in processe)
                                        {
                                            dovi.Kill();
                                        }
                                    }

                                }
                                else
                                    Application.Exit();
                            }
                            else
                                Application.Run(new FrmConfig());
                        }
                        else
                            Application.Run(new FrmConnectDatabase());
                    }
                }
                else
                {
                    MessageBox.Show("Phần mềm chưa được kích hoạt. Vui lòng liên hệ với nhà cung cấp để kích hoạt sử dụng.");
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi program: " + ex.Message);
            }
        }
    }
}
