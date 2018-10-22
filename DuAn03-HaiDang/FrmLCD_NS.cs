using PMS.Business.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PMS.Business;
using PMS.Data;
using DuAn03_HaiDang.Helper;

namespace QuanLyNangSuat
{
    public partial class FrmLCD_NS : Form
    {
        public FrmLCD_NS()
        {
            InitializeComponent();
            var cfObj = BLLConfig.Instance.GetConfig((int)eTableType.NS);
            if (cfObj.Panels.Count > 0)
                ColorPanel(cfObj.Panels);

            if (cfObj.ColumnConfigs.Count > 0)
                SetPanelConfig(cfObj.ColumnConfigs);
        }



        private void ColorPanel(List<ShowLCD_Panel> configs)
        {
            try
            {
                foreach (var item in configs)
                {
                    switch (item.Name)
                    {
                        case "panelHeader":
                            this.pnHead.BackColor = HelperControl.GetColor(item.BackColor);
                            break;
                        case "panelContent":
                            this.pnBody.BackColor = HelperControl.GetColor(item.BackColor);
                            break;
                        case "panelFooter":
                            this.pnFooter.BackColor = HelperControl.GetColor(item.BackColor);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lấy thông tin cấu hình lỗi =>"+ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetPanelConfig(List<ShowLCD_TableLayoutPanel> list)
        {
            try
            {
                foreach (var item in list)
                {
                    switch (item.TableLayoutPanelName)
                    {
                        case "tblpanelHeader":
                            this.pnHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent,  (float)item.SizePercent.Value ));
                            break;
                        case "tblpanelContent":
                            break;
                        case "tblpanelBody":
                            break;
                        case "tblpanelFooter":
                            break;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void FrmLCD_NS_Load(object sender, EventArgs e)
        {

        }
    }
}
