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
    public class ConfigDAO
    {
        public List<Config> GetConfig()
        {
            DataTable dt = new DataTable();
            string sql = "select Id, Name, Value from ShowLCD_Config";
            List<Config> result = new List<Config>();
            try
            {
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        result.Add(new Config()
                        {
                            Id = int.Parse(row["Id"].ToString()),
                            Name = row["Name"].ToString(),
                            Value = row["Value"].ToString(),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi không thể lấy thông tin cấu hình LCD: " + ex.Message, "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            return result;
        }
    }
}
