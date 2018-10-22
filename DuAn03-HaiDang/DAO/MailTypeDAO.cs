using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DuAn03_HaiDang.DAO
{
    public class MailTypeDAO
    {
        public DataTable LoadListMail()
        {
            DataTable dt = null;
            try
            {
                string sql = "select Id, TypeName from MAIL_TYPE where active=1 order by Id desc";
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
                var dt = LoadListMail();
                if (dt != null && dt.Rows.Count > 0)
                {
                    listSelect = new List<ModelSelect>();
                    foreach (DataRow row in dt.Rows)
                    {
                        listSelect.Add(new ModelSelect()
                        {
                            Value = int.Parse(row["Id"].ToString()),
                            Text = row["TypeName"].ToString()
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
