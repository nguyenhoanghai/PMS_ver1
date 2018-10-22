using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DuAn03_HaiDang.DAO
{
    public class MailFileDAO
    {
        public DataTable LoadListMailFile()
        {
            DataTable dt = null;
            try
            {
                string sql = "select Id, Name, Path from MAIL_FILE where IsActive=1 and IsDeleted =0 order by Id desc";
                dt = dbclass.TruyVan_TraVe_DataTable(sql);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public List<ModelSelect> GetListSelectItem()
        {
            List<ModelSelect> listSelect = null;
            try
            {
                var dt = LoadListMailFile();
                if (dt != null && dt.Rows.Count > 0)
                {
                    listSelect = new List<ModelSelect>();
                    foreach (DataRow row in dt.Rows)
                    {
                        listSelect.Add(new ModelSelect()
                        {
                            Value = int.Parse(row["Id"].ToString()),
                            Text = row["Name"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return listSelect;
        }
    }
}
