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
    class LabelConfigDAO
    {
        public List<LabelConfig> GetLabelConfig(int tableType)
        {
            DataTable dt = new DataTable();
            string sql = "select Font, Size, Bold, Color, Italic, Position, TableLayoutPanelName from ShowLCD_LabelArea where TableType="+tableType;
            List<LabelConfig> result = new List<LabelConfig>();
            try
            {
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        result.Add(new LabelConfig() { 
                            Font = row["Font"].ToString(),
                            Size = int.Parse(row["Size"].ToString()),
                            Bold = bool.Parse(row["Bold"].ToString()),
                            Italic = bool.Parse(row["Italic"].ToString()),
                            Color = row["Color"].ToString(),
                            Position = int.Parse(row["Position"].ToString()),
                            TableLayoutPanelName = row["TableLayoutPanelName"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi không thể lấy thông tin cấu hình Lable: "+ex.Message , "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
            return result;
        }

        public List<LabelForTablePanel> GetLabelForTablePanel(int tableType)
        {
            DataTable dt = new DataTable();
            string sql = "select LabelName, SystemValueName, IntRowTBLPanelContent, SttNext, IsShow from ShowLCD_LabelForPanelContent where TableType=" + tableType;
            List<LabelForTablePanel> result = new List<LabelForTablePanel>();
            try
            {
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        result.Add(new LabelForTablePanel()
                        {
                            LabelName = row["LabelName"].ToString(),
                            SystemValueName = row["SystemValueName"].ToString(),
                            IntRowTBLPanelContent = int.Parse(row["IntRowTBLPanelContent"].ToString()),
                            IsShow = bool.Parse(row["IsShow"].ToString()),
                            SttNext = int.Parse(row["SttNext"].ToString()),
                           
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi không thể lấy thông tin cấu hình Lable: " + ex.Message, "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            return result;
        }
    }
}
