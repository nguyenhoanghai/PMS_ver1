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
    public class MailSendDAO
    {
        public DataTable LoadListMail()
        {
            DataTable dt = null; 
            try
            {
                string sql = "select Id, Address from MAIL_SEND where IsActive=1 and IsDeleted =0 order by Id desc";
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
                        listSelect.Add(new ModelSelect() { 
                            Value = int.Parse(row["Id"].ToString()),
                            Text = row["Address"].ToString()
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

        public int AddObj(MailSend mail)
        {
            int kq = 0;
            try
            {
                string sql = "insert into MAIL_SEND(MailTypeId, Address, Password, Note) values(" + mail.MailTypeId + ", N'"+mail.Address+"', N'"+mail.Password+"', N'"+mail.Note+"' )";
                kq = dbclass.TruyVan_XuLy(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return kq;
        }

        public int UpdateObj(MailSend mail)
        {
            int kq = 0;
            try
            {
                string sql = "update MAIL_SEND set Address = N'" + mail.Address + "', Note=N'" + mail.Note + "', MailTypeId=" + mail.MailTypeId + ", IsActive='" + mail.IsActive + "' where Id =" + mail.Id + " and IsDeleted=0";
                if(!string.IsNullOrEmpty(mail.Password))
                    sql = "update MAIL_SEND set Address = N'" + mail.Address + "', Note=N'"+mail.Note+"', MailTypeId="+mail.MailTypeId+", Password=N'"+mail.Password+"', IsActive='"+mail.IsActive+"' where Id =" + mail.Id + " and IsDeleted=0";
                kq = dbclass.TruyVan_XuLy(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return kq;
        }

        public int DeleteObj(int Id)
        {
            int kq = 0;
            try
            {
                string sql = "update MAIL_SEND set IsDeleted = 1 where Id ='" + Id + "'";
                kq = dbclass.TruyVan_XuLy(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return kq;
        }

        public void LoadMailToDataGirdview(DataGridView dg)
        {
            try
            {
                DataTable dt = new DataTable();
                string Strsql = "";
                Strsql = "select m.Id,  t.TypeName, m.Address, m.MailTypeId, m.IsActive, m.Note from MAIL_SEND m, MAIL_TYPE t Where m.IsDeleted =0 and m.MailTypeId=t.Id";
                dt = dbclass.TruyVan_TraVe_DataTable(Strsql);
                dbclass.loaddataridviewcolorrow(dg, dt);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            
        }
    }
}
