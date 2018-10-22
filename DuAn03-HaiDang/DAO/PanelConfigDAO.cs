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
    public class PanelConfigDAO
    {
        public List<PanelConfig> GetPanelConfig(int tableType)
        {
            DataTable dt = new DataTable();
            string sql = "select Name, BackColor from ShowLCD_Panel where TableType="+tableType;
            List<PanelConfig> result = new List<PanelConfig>();
            try
            {
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        result.Add(new PanelConfig()
                        {
                            Name = row["Name"].ToString(),
                            BackColor = row["BackColor"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi không thể lấy được cấu hình Panel: "+ex.Message , "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            return result;
        }
    }
}
