using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuAn03_HaiDang.KeyPad_Chuyen.pojo;
using DuAn03_HaiDang.DATAACCESS;
using System.Data;
using System.Windows.Forms;
using DuAn03_HaiDang.Model;
namespace DuAn03_HaiDang.KeyPad_Chuyen.dao
{
    public class KeyPadDAO
    {
        public DataTable DSKeyPad(string floor)
        {
            DataTable dt = new DataTable();
            string sql = "select Id, KeyPadName, EquipmentId from KeyPad where FloorId ='"+floor+"'";
            try
            {

                dt = dbclass.TruyVan_TraVe_DataTable(sql);

                return dt;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: không thể lấy được danh sách các keypad từ CSDL.", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return dt;
            }
        }

        public int TimKiemIDKeyPad(int STT)
        {
            DataTable dt = new DataTable();
            int id = -1;
            string sql = "select ID from KeyPad where STT  = " + STT + "";
            try
            {

                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    string ma = dt.Rows[0][0].ToString();
                    try
                    {
                        id = int.Parse(ma);
                        return id;
                    }
                    catch (Exception)
                    {

                        return id;
                    }
                }
                else
                {
                    return id;
                }
                
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: không thể thực hiện tìm kiếm chuyền trên CSDL.", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return id;
            }
        }

        public List<ModelKeyPadConfig> GetListKeyPadLineConfig(int floor)
        {
            List<ModelKeyPadConfig> listModel = new List<ModelKeyPadConfig>();
            try
            {
                DataTable dtKeyPad = new DataTable();
                string sqlKeyPad = "select Id, KeyPadName, EquipmentId, UseTypeId from KeyPad where IsDeleted =0 and FloorId='" + floor + "'";
                dtKeyPad = dbclass.TruyVan_TraVe_DataTable(sqlKeyPad);
                if (dtKeyPad != null && dtKeyPad.Rows.Count > 0)
                {
                    ModelKeyPadConfig model;
                    DataTable dtObjectConfig = new DataTable();
                    foreach (DataRow row in dtKeyPad.Rows)
                    {
                        model = new ModelKeyPadConfig();
                        int equipmentId = 0;
                        int.TryParse(row["EquipmentId"].ToString(), out equipmentId);
                        model.EquipmentId = equipmentId;
                        model.KeyPadName = row["KeyPadName"].ToString();
                        int useTypeId = 0;
                        int.TryParse(row["UseTypeId"].ToString(), out useTypeId);
                        model.UseTypeId = useTypeId;
                        
                        string sqlObjectConfig = "Select kpo.STTNut, kpo.ClusterId, kpo.CommandTypeId, ch.MaChuyen, ch.Sound, c.IsEndOfLine From KeyPad_Object kpo, Cum c, Chuyen ch Where kpo.IsDeleted =0 and kpo.KeyPadId=" + row["Id"].ToString()+" and c.Id=kpo.ClusterId and c.IsDeleted=0 and c.IdChuyen=ch.MaChuyen and ch.IsDeleted=0";
                        dtObjectConfig.Clear();
                        dtObjectConfig = dbclass.TruyVan_TraVe_DataTable(sqlObjectConfig);
                        if (dtObjectConfig != null && dtObjectConfig.Rows.Count > 0)
                        {
                            model.ListObjectConfig = new List<ModelKeyPadObjectConfig>();
                            ModelKeyPadObjectConfig modelObjectConfig;
                            foreach (DataRow rowObjectConfig in dtObjectConfig.Rows)
                            {
                                modelObjectConfig = new ModelKeyPadObjectConfig();
                                int sttNut=0;
                                int.TryParse(rowObjectConfig["STTNut"].ToString(), out sttNut);
                                modelObjectConfig.STTNut = sttNut;
                                int clusterId=0;
                                int.TryParse(rowObjectConfig["ClusterId"].ToString(), out clusterId);                                
                                int commandTypeId=0;
                                int.TryParse(rowObjectConfig["CommandTypeId"].ToString(), out commandTypeId);
                                modelObjectConfig.ClusterId = clusterId;                                
                                modelObjectConfig.CommandTypeId = commandTypeId;
                                int lineId = 0;
                                bool isEndOfLine = false;
                                int.TryParse(rowObjectConfig["MaChuyen"].ToString(), out lineId);
                                bool.TryParse(rowObjectConfig["IsEndOfLine"].ToString(), out isEndOfLine);
                                modelObjectConfig.LineId = lineId;
                                modelObjectConfig.IsEndOfLine = isEndOfLine;
                                modelObjectConfig.LineSound = rowObjectConfig["Sound"].ToString();
                                model.ListObjectConfig.Add(modelObjectConfig);
                            }
                        }
                        listModel.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listModel;
        }
    }
}
