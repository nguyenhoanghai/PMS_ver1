using DuAn03_HaiDang.DAO;
using DuAn03_HaiDang.DATAACCESS;
using GPRO.Ultilities;
using QuanLyNangSuat.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace QuanLyNangSuat.DAO
{
    public class GroupErrorDAO
    {
        ErrorDAO errorDAO = new ErrorDAO();
        public DataTable LoadListGroupError()
        {
            DataTable dt = null;
            try
            {
                string sql = "select Id, Code, Name, Description from GroupError where IsDeleted =0 order by Id desc";
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public int AddObj(ModelGroupError obj)
        {
            int kq = 0;
            try
            {
                string sql = "insert into GroupError(Code, Name, Description) values('" + obj.Code.Trim() + "', N'" + obj.Name + "', N'" + obj.Description + "')";
                kq = dbclass.TruyVan_XuLy(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return kq;
        }

        public int UpdateObj(ModelGroupError obj)
        {
            int kq = 0;
            try
            {
                string sql = "update GroupError set Code = '" + obj.Code.Trim() + "', Name=N'" + obj.Name + "', Description=N'" + obj.Description + "' where Id =" + obj.Id + " and IsDeleted=0";
                kq = dbclass.TruyVan_XuLy(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return kq;
        }

        public int CheckExistCode(string code, int Id)
        {
            int kq = 0;
            try
            {
                string sql = "select Id from GroupError where Id !=" + Id + " and Code = '" + code + "' and IsDeleted=0";
                DataTable dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                    kq = 1;
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
                string sql = "update GroupError set IsDeleted = 1 where Id ='" + Id + "'";
                kq = dbclass.TruyVan_XuLy(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return kq;
        }

        public List<ModelGroupError> GetListError()
        {
            List<ModelGroupError> listGroupError = null;
            try
            {
                DataTable dt = LoadListGroupError();
                if (dt != null && dt.Rows.Count > 0)
                {
                    listGroupError = new List<ModelGroupError>();
                    foreach (DataRow row in dt.Rows)
                    {
                        listGroupError.Add(new ModelGroupError()
                        {
                            Id = int.Parse(row["Id"].ToString()),
                            Code = row["Code"].ToString(),
                            Name = row["Name"].ToString(),
                            Description = row["Description"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return listGroupError;
        }

        public ModelLCDError GetListGroupErrorDetail()
        {
            ModelLCDError modelLCDError = null;
            try
            {
                var listGError = GetListError();
                int totalRow = 0;
                if (listGError != null && listGError.Count > 0)
                {
                    modelLCDError = new ModelLCDError();
                    var listError = errorDAO.GetListError();
                    foreach (ModelGroupError gerror in listGError)
                    {
                        ModelGroupErrorDetail model = new ModelGroupErrorDetail();
                        Parse.CopyObject(gerror, ref model);
                        if (listError != null && listError.Count > 0)
                        {
                            model.ListError = listError.Where(c => c.GroupErrorId == model.Id).ToList();
                            if (model.ListError.Count > 0)
                            {
                                totalRow += model.ListError.Count;
                                totalRow += 1;
                            }
                        }
                        modelLCDError.listModelGroupErrorDetail.Add(model);
                    }
                    modelLCDError.totalRow = totalRow;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return modelLCDError;
        }
    }
}
