using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.POJO;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DuAn03_HaiDang.DAO
{
    class ReadPercentDAO
    {
        public DataTable DSOBJ()
        {
            DataTable dt = new DataTable();
            string sql = "select Id, PercentFrom, PercentTo, CountRepeay from ReadPercent where IsDeleted=0";
            try
            {
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                return dt;
            }
            catch (Exception)
            {
                return dt;
            }
        }

        public int ThemOBJ(ReadPercent readPercent,  List<ReadPercent> listReadPercentItem)
        {
            int result = 0;
            SqlConnection con = dbclass.taoketnoi();
            SqlCommand cmd = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("Transaction");
            try
            {                
                cmd.Connection = con;
                cmd.Transaction = transaction;
                string sqlInsert = "insert into ReadPercent (Name, PercentFrom, PercentTo, CountRepeat, Sound, IdParent) values(N'" + readPercent.Name + "', " + readPercent.PercentFrom + "," + readPercent.PercentTo + "," + readPercent.CountRepeat + ",'" + readPercent.Sound + "', '0')";
                cmd.CommandText = sqlInsert;
                int insertResult = cmd.ExecuteNonQuery();
                if (insertResult == 1)
                {
                    int parentId = FindFinalId();
                    if (parentId != 0 && listReadPercentItem != null && listReadPercentItem.Count > 0)
                    {
                        foreach (var item in listReadPercentItem)
                        {
                            cmd.CommandText = "insert into ReadPercent (Name, PercentFrom, PercentTo, CountRepeat, Sound, IdParent) values(N'" + item.Name + "', " + item.PercentFrom + "," + item.PercentTo + "," + item.CountRepeat + ",'" + item.Sound + "', '" + parentId + "')";
                            cmd.ExecuteNonQuery();
                        }
                    }
                }               
                transaction.Commit();
                result = 1;
            }
            catch (Exception ex)
            {
                try
                {
                    transaction.Rollback();
                    throw ex;
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
            }
            return result;
        }

        public int SuaThongTinOBJ(int Id, List<ReadPercent> listObj)
        {
            int result = 0;
            SqlConnection con = dbclass.taoketnoi();
            SqlCommand cmd = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("Transaction");
            try
            {
                cmd.Connection = con;
                cmd.Transaction = transaction;
                string sqlSelectOldInfo = "select Id from ReadPercent where IdParent=" + Id;
                DataTable dt = dbclass.TruyVan_TraVe_DataTable(sqlSelectOldInfo);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string strUpdate = "update ReadPercent set IsDeleted=1 where Id=" + row["Id"].ToString();
                        cmd.CommandText = strUpdate;
                        cmd.ExecuteNonQuery();
                    }
                }
                if (listObj != null && listObj.Count > 0)
                {
                    foreach (var item in listObj)
                    {
                        string sql = "insert into ReadPercent (Name, PercentFrom, PercentTo, CountRepeat, Sound, IdParent) values(N'" + item.Name + "', " + item.PercentFrom + "," + item.PercentTo + "," + item.CountRepeat + ",'" + item.Sound + "', '" + item.IdParent + "')";
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }
                }                
                transaction.Commit();
                result = 1;
            }
            catch (Exception ex)
            {
                try
                {
                    transaction.Rollback();
                    throw ex;
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
            }
            return result;
        }

        public int XoaOBJ(int Id)
        {
            int kq = 0;
            try
            {
                string sql = "update ReadPercent set IsDeleted=1 where Id ='"+Id+"'";
                kq = dbclass.TruyVan_XuLy(sql);                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return kq;
        }



        public void LoadOBJToDataGirdview(int Id, DataGridView dg)
        {

            DataTable dt = new DataTable();
            string Strsql = "select Id, PercentFrom, PercentTo, CountRepeat, Sound from ReadPercent where IdParent ='"+Id+"' and IsDeleted=0";
            dt = dbclass.TruyVan_TraVe_DataTable(Strsql);
            dbclass.loaddataridviewcolorrow(dg, dt);
        }

        public int FindFinalId()
        {
            try
            {
                DataTable dt = new DataTable();
                int Id = 0;
                string Strsql = "select top 1 Id from ReadPercent Order by Id DESC";
                dt = dbclass.TruyVan_TraVe_DataTable(Strsql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Id = int.Parse(dt.Rows[0]["Id"].ToString());
                }
                return Id;
            }
            catch (Exception ex)
            {                
                throw;
            }            
        }
    }
}
