using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.POJO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DuAn03_HaiDang.DAO
{
    public class ErrorDAO
    {
        public DataTable LoadListMail()
        {
            DataTable dt = null;
            try
            {
                string sql = "select e.Id, e.Code, e.Name, e.Description, e.GroupErrorId, ge.Name GroupErrorName  from Error e, GroupError ge where e.IsDeleted =0 and e.GroupErrorId=ge.Id and ge.IsDeleted=0 order by e.Id desc";
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public int AddObj(Error obj)
        {
            int kq = 0;
            try
            {
                string sql = "insert into Error(Code, Name, Description, GroupErrorId) values('" + obj.Code.Trim() + "', N'" + obj.ErrorName + "', N'" + obj.Description + "', " + obj.GroupErrorId+ ")";
                kq = dbclass.TruyVan_XuLy(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return kq;
        }

        public int UpdateObj(Error obj)
        {
            int kq = 0;
            try
            {
                string sql = "update Error set Code = '" + obj.Code.Trim() + "', Name=N'" + obj.ErrorName + "', Description=N'" + obj.Description + "', GroupErrorId="+obj.GroupErrorId+" where Id =" + obj.Id + " and IsDeleted=0";
                kq = dbclass.TruyVan_XuLy(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return kq;
        }

        public bool CheckExistCode(string code, int Id)
        {
            var kq = false;
            try
            {
                string sql = "select * from Error where Id !="+Id+" and Code = '"+code+"' and IsDeleted=0";
                DataTable dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                    kq = true;
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
                string sql = "update Error set IsDeleted = 1 where Id ='" + Id + "'";
                kq = dbclass.TruyVan_XuLy(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return kq;
        }

        public List<Error> GetListError()
        {
            List<Error> listError=null;
            try
            {
                DataTable dt = LoadListMail();
                if (dt != null && dt.Rows.Count > 0)
                {
                    listError = new List<Error>();                    
                    foreach (DataRow row in dt.Rows)
                    {
                        int groupErrorId = 0;
                        int.TryParse(row["GroupErrorId"].ToString(), out groupErrorId);
                        listError.Add(new Error() { 
                            Id = int.Parse(row["Id"].ToString()),
                            Code = row["Code"].ToString(),
                            ErrorName = row["Name"].ToString(),
                            Description =row["Description"].ToString(),
                            GroupErrorId = groupErrorId,
                            GroupErrorName = row["GroupErrorName"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return listError;
        }
    }
}
