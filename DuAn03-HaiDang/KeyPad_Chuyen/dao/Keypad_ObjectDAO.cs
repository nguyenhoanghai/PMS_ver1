using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuAn03_HaiDang.KeyPad_Chuyen.pojo;
using DuAn03_HaiDang.DATAACCESS;
using System.Data;
using System.Windows.Forms;
using DuAn03_HaiDang.Model;
using DuAn03_HaiDang.DAO;
using DuAn03_HaiDang.POJO;
namespace DuAn03_HaiDang.KeyPad_Chuyen.dao
{
    public class Keypad_ObjectDAO
    {
        private ClusterDAO clusterDAO = new ClusterDAO();
        public List<ModelKeyPadObject> GetKeyPadInfoByLineId(int maChuyen)
        {
            try
            {
                List<ModelKeyPadObject> listModel = null;
                var listCluster = clusterDAO.GetCumOfChuyen(maChuyen);
                if (listCluster != null && listCluster.Count > 0)
                {
                    listModel = new List<ModelKeyPadObject>();
                    foreach (Cluster cluster in listCluster)
                    {
                        string strSQLSelect = "Select  kpo.KeyPadId, kpo.STTNut, kpo.CommandTypeId, kp.EquipmentId, kp.FloorId, kp.UseTypeId From KeyPad_Object kpo, KeyPad kp ";
                        strSQLSelect += "Where kpo.ClusterId="+cluster.Id+" and kpo.IsDeleted=0 and kp.IsDeleted=0 and kpo.KeyPadId=kp.Id";
                        DataTable dt = dbclass.TruyVan_TraVe_DataTable(strSQLSelect);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                ModelKeyPadObject model = new ModelKeyPadObject();
                                model.ClusterId = cluster.Id;
                                int equipmentId = 0;
                                int.TryParse(row["EquipmentId"].ToString(), out equipmentId);
                                model.EquipmentId = equipmentId;
                                model.LineId = maChuyen;
                                int keyPadId = 0;
                                int.TryParse(row["KeyPadId"].ToString(), out keyPadId);
                                model.KeyPadId = keyPadId;  
                                model.IsEndOfLine = cluster.IsEndOfLine;
                                int useTypeId = 0;
                                int.TryParse(row["UseTypeId"].ToString(), out useTypeId);
                                model.UseTypeId = useTypeId;
                                listModel.Add(model);
                            }
                        }                        
                    }
                }
                return listModel;
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }
    }
}
