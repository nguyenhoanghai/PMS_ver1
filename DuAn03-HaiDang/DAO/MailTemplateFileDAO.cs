using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.Model;
using DuAn03_HaiDang.POJO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DuAn03_HaiDang.DAO
{
    public class MailTemplateFileDAO
    {
        public int AddObj(MailTemplateFile obj)
        {
            int kq = 0;
            SqlTransaction transaction=null;
            try
            {                
                if (obj != null)
                {
                    SqlConnection sqlConnect = dbclass.taoketnoi();
                    SqlCommand sqlCommand = sqlConnect.CreateCommand();                    
                    transaction = sqlConnect.BeginTransaction("Transaction");
                    sqlCommand.Connection = sqlConnect;
                    sqlCommand.Transaction = transaction;
                    sqlCommand.CommandText = "insert into MAIL_TEMPLATE (Name, Description, Subject, Content, MailSendId, ListMailReceiveId) values(N'" + obj.Name + "', N'" + obj.Description + "', N'" + obj.Subject + "', N'" + obj.Content + "', " + obj.MailSendId + ", '" + obj.ListMailReceiveId + "' )";
                    var result1= sqlCommand.ExecuteNonQuery();  
                    if (result1 != 0)
                    {
                        if (obj.listFileId != null && obj.listFileId.Count > 0)
                        {
                            DataTable dt = new DataTable();
                            sqlCommand.CommandText = "select TOP 1 * from MAIL_TEMPLATE where Name like N'" + obj.Name + "' and MailSendId=" + obj.MailSendId + " and IsDeleted=0 order by Id desc";
                            dt.Load(sqlCommand.ExecuteReader());                             
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                int id = 0;
                                int.TryParse(dt.Rows[0]["Id"].ToString(), out id );
                                if (id != 0)
                                {
                                    foreach (var fileId in obj.listFileId)
                                    {
                                        sqlCommand.CommandText = "insert into MAIL_T_M (MailTemplateId, MailFileId) values(" + id + ", " + fileId + ")";
                                        sqlCommand.ExecuteNonQuery();                                        
                                    }
                                }
                            }
                        }
                    }
                    transaction.Commit();
                    sqlConnect.Close();
                    kq = 1;                    
                }                
                
            }
            catch (Exception ex)
            {                
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
                throw ex;
            }
            return kq;
        }

        public int UpdateObj(MailTemplateFile obj)
        {
            int kq = 0;
            SqlTransaction transaction = null;
            try
            {
                if (obj != null)
                {
                    SqlConnection sqlConnect = dbclass.taoketnoi();
                    SqlCommand sqlCommand = sqlConnect.CreateCommand();
                    transaction = sqlConnect.BeginTransaction("Transaction");
                    sqlCommand.Connection = sqlConnect;
                    sqlCommand.Transaction = transaction;                   
                    sqlCommand.CommandText = "update MAIL_TEMPLATE set Name=N'" + obj.Name + "', [Description] =N'" + obj.Description + "', [Subject]=N'" + obj.Subject + "', Content=N'" + obj.Content + "', MailSendId=" + obj.MailSendId + ", ListMailReceiveId='" + obj.ListMailReceiveId + "' where Id =" + obj.Id + " and IsDeleted=0";
                    var result1 = sqlCommand.ExecuteNonQuery();
                    if (result1 != 0)
                    {
                        if (obj.listFileId != null && obj.listFileId.Count > 0)
                        {                            
                            sqlCommand.CommandText = "select Id from MAIL_T_M where IsDeleted=0 and MailTemplateId=" + obj.Id;
                            SqlDataReader dr = sqlCommand.ExecuteReader();
                            List<string> listId = new List<string>();
                            while (dr.Read())
                            {
                                listId.Add(dr[0].ToString().Trim());                                
                            }
                            sqlCommand.Dispose();
                            dr.Dispose();
                            if (listId.Count > 0)
                            {
                                foreach (var id in listId)
                                {
                                    sqlCommand.CommandText = "update MAIL_T_M set IsDeleted=1 where Id =" + id;
                                    sqlCommand.ExecuteNonQuery();
                                }
                            }
                            
                            foreach (var fileId in obj.listFileId)
                            {
                                sqlCommand.CommandText = "insert into MAIL_T_M (MailTemplateId, MailFileId) values(" + obj.Id + ", " + fileId + ")";
                                sqlCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    transaction.Commit();
                    sqlConnect.Close();
                    kq = 1;                  
                }  
            }
            catch (Exception ex)
            {
                
                transaction.Rollback();
                throw ex;
            }
            return kq;
        }

        public int DeleteObj(int Id)
        {
            int kq = 0;
            try
            {
                string sql = "update MAIL_TEMPLATE set IsDeleted = 1 where Id ='" + Id + "'";
                kq = dbclass.TruyVan_XuLy(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return kq;
        }

        public void LoadDataToDataGirdview(DataGridView dg)
        {
            try
            {
                DataTable dt = new DataTable();
                string Strsql = "";
                Strsql = "select Id, Name,  Subject, Content, Description, IsActive, MailSendId, ListMailReceiveId from MAIL_TEMPLATE Where IsDeleted =0";
                dt = dbclass.TruyVan_TraVe_DataTable(Strsql);                
                dbclass.loaddataridviewcolorrow(dg, dt);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public ModelMailTeamplate GetIrnfoById(int id)
        {
            ModelMailTeamplate mail = null;
            try
            {
                string sqlSelectMailTemplate = "select mt.Name, mt.Description, mt.Subject, mt.Content, mt.MailSendId, mt.ListMailReceiveId, mt.IsActive, ms.Address AddressSend, ms.PassWord, ms.Note, t.host, t.port, t.TypeName from MAIL_TEMPLATE mt, MAIL_SEND ms, MAIL_TYPE t where mt.Id =" + id + " and ms.Id = mt.MailSendId and ms.MailTypeId=t.Id and mt.IsActive=1 and ms.IsActive=1 and t.active=1 and mt.IsDeleted=0";
                DataTable dt = dbclass.TruyVan_TraVe_DataTable(sqlSelectMailTemplate);
                if (dt != null && dt.Rows.Count>0)
                {
                    mail = new ModelMailTeamplate();
                    DataRow dr = dt.Rows[0];
                    mail.Id = id;
                    mail.Name = dr["Name"].ToString();
                    mail.Description = dr["Description"].ToString();
                    mail.Subject = dr["Subject"].ToString();
                    mail.Content = dr["Content"].ToString();
                    mail.MailSendId = int.Parse(dr["MailSendId"].ToString());
                    mail.ListMailReceiveId = dr["ListMailReceiveId"].ToString();
                    mail.IsActive = bool.Parse(dr["IsActive"].ToString());
                    mail.MailSendName = dr["AddressSend"].ToString();
                    string sqlSelectMailTemplateFile = "select mtf.MailFileId, mf.Path, mf.Name, mf.Code from MAIL_T_M mtf, MAIL_FILE mf where mtf.MailTemplateId =" + id + " and mtf.MailFileId=mf.Id and mf.IsActive=1 and mtf.IsActive=1 and mtf.IsDeleted=0";
                    DataTable dtMailTF = dbclass.TruyVan_TraVe_DataTable(sqlSelectMailTemplateFile);
                    mail.listFile = new List<ModelFile>();
                    if (dtMailTF != null && dtMailTF.Rows.Count > 0)
                    {
                        mail.listFileId = new List<int>();
                        foreach (DataRow drow in dtMailTF.Rows)
                        {
                            mail.listFileId.Add(int.Parse(drow["MailFileId"].ToString()));
                            mail.listFile.Add(new ModelFile()
                            {
                                Path = drow["Path"].ToString(),
                                Name = drow["Name"].ToString(),
                                Code = drow["Code"].ToString().Trim()
                            });
                        }
                    }
                    mail.mailSend = new ModelMailSend();
                    mail.mailSend.Address = dr["AddressSend"].ToString();
                    mail.mailSend.Password = dr["PassWord"].ToString();
                    mail.mailSend.Note = dr["Note"].ToString();
                    mail.mailSend.host = dr["host"].ToString();
                    mail.mailSend.port = dr["port"].ToString();
                    mail.mailSend.mail_type = dr["TypeName"].ToString();
                    if (!string.IsNullOrEmpty(mail.ListMailReceiveId.Trim()))
                    {
                        var arrMailReceiveId = mail.ListMailReceiveId.Trim().Split(new char[] { '|' });
                        if (arrMailReceiveId != null && arrMailReceiveId.Count() > 0)
                        {
                            arrMailReceiveId = arrMailReceiveId.Where(c => !c.Trim().Equals(string.Empty)).ToArray();
                            string sqlSelectMailReceive = "select Address from MAIL_RECEIVE where Id in (";
                            for (int i = 0; i < arrMailReceiveId.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(arrMailReceiveId[i]))
                                {
                                    sqlSelectMailReceive += arrMailReceiveId[i].Trim();
                                    if (i < arrMailReceiveId.Length - 1)
                                        sqlSelectMailReceive += ",";
                                    else
                                        sqlSelectMailReceive += ")";
                                }
                            }
                            DataTable dtMailReceive = dbclass.TruyVan_TraVe_DataTable(sqlSelectMailReceive);
                            if (dtMailReceive != null)
                            {
                                mail.listMailReceive = new List<string>();
                                foreach (DataRow row in dtMailReceive.Rows)
                                {
                                    mail.listMailReceive.Add(row["Address"].ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            return mail;
        }
       
        public List<ModelSelect> GetListSelectItem()
        {
            List<ModelSelect> listSelect = null;
            try
            {
                string strSQLSelect = "select Id, Name from MAIL_TEMPLATE Where IsDeleted =0";
                var dt = dbclass.TruyVan_TraVe_DataTable(strSQLSelect);
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

        public List<ModelMailSchedule> GetMailSchedule()
        {
            List<ModelMailSchedule> listModel = null;
            try
            {
                string sqlSelectMailTemplate = "Select Id from MAIL_TEMPLATE where IsActive=1 and IsDeleted=0";
                DataTable dt = dbclass.TruyVan_TraVe_DataTable(sqlSelectMailTemplate);
                if (dt != null && dt.Rows.Count > 0)
                {
                    listModel = new List<ModelMailSchedule>();
                    foreach (DataRow row in dt.Rows)
                    {
                        string sqlSelectSchedule = "select Time from MAIL_SCHEDULE where IsActive=1 and IsDeleted=0 and MailTemplateId="+row["Id"].ToString();
                        DataTable dtTime = dbclass.TruyVan_TraVe_DataTable(sqlSelectSchedule);
                        if(dtTime!=null && dtTime.Rows.Count>0)
                        {
                            ModelMailSchedule model = new ModelMailSchedule();
                            model.MailTemplateId = int.Parse(row["Id"].ToString());
                            model.listTime = new List<TimeSpan>();
                            foreach (DataRow rowTime in dtTime.Rows)
                            {
                                model.listTime.Add(TimeSpan.Parse(rowTime["Time"].ToString()));
                            }
                            listModel.Add(model);
                        }
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
