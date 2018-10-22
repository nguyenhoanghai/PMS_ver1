using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DuAn03_HaiDang.DATAACCESS;
using System.Windows.Forms;
using DuAn03_HaiDang.POJO;

namespace DuAn03_HaiDang.DAO
{
    class FloorDAO
    {      
        public List<Floor> GetListFloor()
        {
            List<Floor> listFloor = null;
            try
            {
                DataTable dt = new DataTable();
                string sql = "select IdFloor, Name, IsDefault from Floor where IsDeleted=0  order by Name ASC"; ;
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    listFloor = new List<Floor>();
                    foreach (DataRow row in dt.Rows)
                    {
                        listFloor.Add(new Floor() { 
                            IdFloor = int.Parse(row["IdFloor"].ToString()),
                            Name = row["Name"].ToString(),
                            IsDefault = Boolean.Parse(row["IsDefault"].ToString())
                        });
                    }
                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            return listFloor;
        }
    }
}
