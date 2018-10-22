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
    public class MailScheduleDAO
    {
        public DataTable LoadListMail(int mailTemplateId)
        {
            DataTable dt = null;
            try
            {
                string sql = "select Id, Time from MAIL_SCHEDULE where IsActive=1 and MailTemplateId =" + mailTemplateId + " and IsDeleted=0 order by Id desc";
                dt = dbclass.TruyVan_TraVe_DataTable(sql);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }       

        public int AddObj(MailSchedule mailSchedule)
        {
            int kq = 0;
            try
            {
                string sql = "insert into MAIL_SCHEDULE(MailTemplateId, Time) values(" + mailSchedule.MailTemplateId + ", '" + mailSchedule.Time + "')";
                kq = dbclass.TruyVan_XuLy(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return kq;
        }

        public int UpdateObj(MailSchedule mailSchedule)
        {
            int kq = 0;
            try
            {
                string sql = "update MAIL_SCHEDULE set Time = N'" + mailSchedule.Time + "', IsActive='" + mailSchedule.IsActive + "' where Id =" + mailSchedule.Id;
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
                string sql = "update MAIL_SCHEDULE set IsDeleted = 1 where Id ='" + Id + "'";
                kq = dbclass.TruyVan_XuLy(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return kq;
        }

        public void LoadDataToDataGirdview(DataGridView dg, int mailTemplateId)
        {
            try
            {
                DataTable dt = new DataTable();
                string Strsql = "";
                Strsql = "select Id, Time, IsActive from MAIL_SCHEDULE Where IsDeleted =0 and MailTemplateId=" + mailTemplateId;
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
