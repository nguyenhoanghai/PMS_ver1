
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QuanLyNangSuat.Model;
using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.POJO;

namespace QuanLyNangSuat.DAO
{
    public class LCDConfigDAO
    {
        public List<ModelTableLayoutPanel> GetTableLayoutPanelConfig(int tableType)
        {
            DataTable dt = new DataTable();
            string sql = "select Id, ColumnInt, RowInt, TableLayoutPanelName, SizePercent, IsShow from ShowLCD_TableLayoutPanel where TableType="+tableType;
            List<ModelTableLayoutPanel> listModel = new List<ModelTableLayoutPanel>();
            try
            {
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var model = new ModelTableLayoutPanel();
                        model.Id = int.Parse(row["Id"].ToString());
                        if (!string.IsNullOrEmpty( row["ColumnInt"].ToString()))
                             model.ColumnInt= int.Parse(row["ColumnInt"].ToString());
                        if (!string.IsNullOrEmpty(row["RowInt"].ToString()))
                            model.RowInt = int.Parse(row["RowInt"].ToString());
                        model.TableLayoutPanelName = row["TableLayoutPanelName"].ToString();                        
                        if (!string.IsNullOrEmpty(row["SizePercent"].ToString()))
                            model.SizePercent = float.Parse(row["SizePercent"].ToString());
                        model.IsShow = bool.Parse(row["IsShow"].ToString());
                        listModel.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi không thể lấy thông tin cấu hình TableLayoutPanel của LCD: " + ex.Message, "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return listModel;
        }

        public List<ModelPanel> GetPanelConfig(int tableType)
        {
            DataTable dt = new DataTable();
            string sql = "select Id, Name, BackColor from ShowLCD_Panel where TableType="+tableType;
            List<ModelPanel> listModel = new List<ModelPanel>();
            try
            {
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var model = new ModelPanel();
                        model.Id = int.Parse(row["Id"].ToString());                        
                        model.Name = row["Name"].ToString();
                        model.BackColor = row["BackColor"].ToString();  
                        listModel.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi không thể lấy thông tin cấu hình Panel của LCD: " + ex.Message, "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return listModel;
        }

        public List<ModelLabelArea> GetLabelAreaConfig(int tableType)
        {
            DataTable dt = new DataTable();
            string sql = "select Id, Font, Size, Bold, Italic, Color, Position, TableLayoutPanelName from ShowLCD_LabelArea where TableType=" + tableType;
            List<ModelLabelArea> listModel = new List<ModelLabelArea>();
            try
            {
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var model = new ModelLabelArea();
                        model.Id = int.Parse(row["Id"].ToString());
                        model.Font = row["Font"].ToString();
                        model.Size = float.Parse(row["Size"].ToString());
                        model.Bold = bool.Parse(row["Bold"].ToString());
                        model.Italic = bool.Parse(row["Italic"].ToString());
                        model.Color = row["Color"].ToString();
                        model.Position = int.Parse(row["Position"].ToString());
                        model.TableLayoutPanelName = row["TableLayoutPanelName"].ToString();
                        listModel.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi không thể lấy thông tin cấu hình Panel của LCD: " + ex.Message, "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return listModel;
        }

        public List<ModelLabelForPanelContent> GetLabelForPanelContentConfig(int tableType)
        {
            DataTable dt = new DataTable();
            string sql = "select Id, SystemValueName, LabelName, IntRowTBLPanelContent, SttNext, IsShow from ShowLCD_LabelForPanelContent where TableType=" + tableType;
            List<ModelLabelForPanelContent> listModel = new List<ModelLabelForPanelContent>();
            try
            {
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var model = new ModelLabelForPanelContent();
                        model.Id = int.Parse(row["Id"].ToString());
                        model.LabelName = row["LabelName"].ToString();
                        model.IntRowTBLPanelContent = int.Parse(row["IntRowTBLPanelContent"].ToString());
                        model.SttNext = int.Parse(row["SttNext"].ToString());
                        model.IsShow = bool.Parse(row["IsShow"].ToString());
                        model.SystemValueName = row["SystemValueName"].ToString();
                        listModel.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi không thể lấy thông tin cấu hình Panel của LCD: " + ex.Message, "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return listModel;
        }

        public bool SaveTableLayoutPanelConfig(List<ModelTableLayoutPanel> listModel)
        {
            bool isSuccess = false;
            try
            {
                if (listModel != null && listModel.Count > 0)
                {
                    List<string> listSQLQuery = new List<string>();
                    foreach (var model in listModel)
                    {                        
                        string sql = "update ShowLCD_TableLayoutPanel set";
                        if (model.ColumnInt != 0)
                            sql += " ColumnInt=" + model.ColumnInt;
                        else
                            sql += " ColumnInt=NULL";
                        if (model.RowInt != 0)
                            sql += ", RowInt =" + model.RowInt;
                        else
                            sql += ", RowInt =NULL";
                        sql+=", TableLayoutPanelName='" + model.TableLayoutPanelName + "', SizePercent=" + model.SizePercent + ", IsShow='" + model.IsShow + "' where Id =" + model.Id + " and TableType=" + model.TableType;
                        listSQLQuery.Add(sql);
                    }
                    int result = dbclass.ExecuteSqlTransaction(listSQLQuery);
                    if (result > 0)
                        isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi không lưu được cấu hình PanelLayoutPanel của LCD: " + ex.Message, "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return isSuccess;
        }

        public bool SavePanelConfig(List<ModelPanel> listModel)
        {
            bool isSuccess = false;
            try
            {
                if (listModel != null && listModel.Count > 0)
                {
                    List<string> listSQLQuery = new List<string>();
                    foreach (var model in listModel)
                    {
                        string sql = "update ShowLCD_Panel set Name='" + model.Name + "', BackColor ='" + model.BackColor + "' where Id =" + model.Id + " and TableType=" + model.TableType;
                        listSQLQuery.Add(sql);
                    }
                    int result = dbclass.ExecuteSqlTransaction(listSQLQuery);
                    if (result > 0)
                        isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi không lưu được thông tin cấu hình Panel của LCD: " + ex.Message, "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return isSuccess;
        }

        public bool SaveLabelAreaConfig(List<ModelLabelArea> listModel)
        {
            bool isSuccess = false;
            try
            {
                if (listModel != null && listModel.Count > 0)
                {
                    List<string> listSQLQuery = new List<string>();
                    foreach (var model in listModel)
                    {
                        string sql = "update ShowLCD_LabelArea set Font='" + model.Font + "', Size =" + model.Size + ", Bold='"+model.Bold+"', Italic='"+model.Italic+"', Color='"+model.Color+"', Position="+model.Position+", TableLayoutPanelName='"+model.TableLayoutPanelName+"' where Id =" + model.Id + " and TableType=" + model.TableType;
                        listSQLQuery.Add(sql);
                    }
                    int result = dbclass.ExecuteSqlTransaction(listSQLQuery);
                    if (result > 0)
                        isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi không lưu được thông tin cấu hình LabelArea của LCD: " + ex.Message, "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return isSuccess;
    
    }

        public bool SaveLabelForPanelContentConfig(List<ModelLabelForPanelContent> listModel)
        {
            bool isSuccess = false;
            try
            {
                if (listModel != null && listModel.Count > 0)
                {
                    List<string> listSQLQuery = new List<string>();
                    foreach (var model in listModel)
                    {
                        string sql = "update ShowLCD_LabelForPanelContent set LabelName=N'" + model.LabelName + "', IntRowTBLPanelContent =" + model.IntRowTBLPanelContent + ", SttNext='" + model.SttNext + "', IsShow='" + model.IsShow + "' where Id =" + model.Id + " and TableType=" + model.TableType;
                        listSQLQuery.Add(sql);
                    }
                    int result = dbclass.ExecuteSqlTransaction(listSQLQuery);
                    if (result > 0)
                        isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi không lưu được thông tin cấu hình LabelForPanelContent của LCD: " + ex.Message, "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return isSuccess;

        }

        public bool SaveLCDConfig(List<Config> listModel)
        {
            bool isSuccess = false;
            try
            {
                if (listModel != null && listModel.Count > 0)
                {
                    List<string> listSQLQuery = new List<string>();
                    foreach (var model in listModel)
                    {
                        string sql = "update ShowLCD_Config set Value=N'" + model.Value + "' where Id =" + model.Id;
                        listSQLQuery.Add(sql);
                    }
                    int result = dbclass.ExecuteSqlTransaction(listSQLQuery);
                    if (result > 0)
                        isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi không lưu được thông tin cấu hình LCD: " + ex.Message, "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return isSuccess;

        }


    }
}
