using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.Model;
using DuAn03_HaiDang.POJO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DuAn03_HaiDang.DAO
{
    public class TableLayoutPanelConfigDAO
    {
        public List<TableLayoutPanelConfig> GetTableLayoutPanelConfig(int tableType)
        {
            DataTable dt = new DataTable();
            string sql = "select Id, ColumnInt, RowInt, TableLayoutPanelName, SizePercent, IsShow from ShowLCD_TableLayoutPanel where TableType="+tableType;
            List<TableLayoutPanelConfig> result = new List<TableLayoutPanelConfig>();
            try
            {
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var item = new TableLayoutPanelConfig();
                        item.Id = row["Id"].ToString();
                        item.ColumnInt = row["ColumnInt"].ToString();
                        int intRowInt = 0;
                        int.TryParse(row["RowInt"].ToString(), out intRowInt);
                        item.IntRowInt = intRowInt;
                        item.RowInt = row["RowInt"].ToString();                       
                        item.TableLayoutTableName = row["TableLayoutPanelName"].ToString();
                        item.SizePercent = row["SizePercent"].ToString();
                        bool isShow = false;
                        bool.TryParse(row["IsShow"].ToString(), out isShow);
                        item.IsShow = isShow;                        
                        result.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi không thể lấy được cấu hình Panel: " + ex.Message, "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            return result;
        }

        public List<ShowLCDLabelForPanelContent> GetLabelForTBLPanelContent()
        {
            DataTable dt = new DataTable();
            string sql = "select LabelName, SystemValueName, IntRowTBLPanelContent, SttNext, IsShow from ShowLCD_LabelForPanelContent where IsShow=1";
            List<ShowLCDLabelForPanelContent> result = new List<ShowLCDLabelForPanelContent>();
            try
            {
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        result.Add(new ShowLCDLabelForPanelContent()
                        {
                            LabelName = row["LabelName"].ToString(),
                            SystemValueName = row["SystemValueName"].ToString(),
                            IntRow = int.Parse(row["IntRowTBLPanelContent"].ToString()),
                            SttNext = int.Parse(row["SttNext"].ToString()),
                            IsShow = bool.Parse(row["IsShow"].ToString())
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi không thể lấy được cấu hình label: " + ex.Message, "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }
    }
}
