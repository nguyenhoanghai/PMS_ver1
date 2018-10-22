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
    public class SoundDAO
    {
        public DataTable LoadListSound()
        {
            DataTable dt = null;
            try
            {
                string sql = "select Id, Code, Name, Description, Path from SOUND where IsActive=1 and IsDeleted =0 order by Id desc";
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
                var dt = LoadListSound();
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

        public int AddObj(Sound obj)
        {
            int kq = 0;
            try
            {
                string sql = "insert into SOUND(Code, Name, Description, Path, IsActive) values(N'" + obj.Code + "', N'" + obj.Name + "', N'" + obj.Description + "', N'" + obj.Path + "', '" + obj.IsActive + "' )";
                kq = dbclass.TruyVan_XuLy(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return kq;
        }

        public int UpdateObj(Sound obj)
        {
            int kq = 0;
            try
            {
                string sql = "update SOUND set Code= N'" + obj.Code + "', Name = N'" + obj.Name + "', Description=N'" + obj.Description + "', IsActive='" + obj.IsActive + "' where Id =" + obj.Id + " and IsDeleted=0";
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
                string sql = "update SOUND set IsDeleted = 1 where Id ='" + Id + "'";
                kq = dbclass.TruyVan_XuLy(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return kq;
        }

        public void LoadSoundToDataGirdview(DataGridView dg)
        {
            try
            {
                string sql = "select Id, Code, Name, Path, IsActive ,Description from SOUND where IsDeleted =0 order by Code ";
                DataTable dt = new DataTable();
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                dbclass.loaddataridviewcolorrow(dg, dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Sound> GetSounds()
        {
            try
            {
                var sounds = new List<Sound>();
                string sql = "select Id, Code, Name, Description, Path, IsActive from SOUND where IsDeleted =0 order by Id desc";
                var dt = new DataTable();
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var s = new Sound();
                        s.Id = int.Parse(dt.Rows[i]["Id"].ToString());
                        s.Code = dt.Rows[i]["Code"].ToString().Trim();
                        s.Name = dt.Rows[i]["Name"].ToString().Trim();
                        s.Description = dt.Rows[i]["Description"].ToString().Trim();
                        s.Path = dt.Rows[i]["Path"].ToString().Trim();
                        sounds.Add(s);
                    }
                }
                return sounds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetPathById(int id)
        {
            string path = string.Empty;
            try
            {
                string sql = "select Path from SOUND where Id=" + id + " and IsActive=1 and IsDeleted =0";
                DataTable dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                    path = dt.Rows[0]["Path"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return path;
        }

        public List<string> LoadListSoundFileByString(string chuoi)
        {
            List<string> listPath = null;
            try
            {
                if (!string.IsNullOrEmpty(chuoi))
                {
                    DataTable dt = LoadListSound();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        listPath = new List<string>();
                        var tuArr = chuoi.Split(new char[] { ' ' });
                        if (tuArr != null && tuArr.Length > 0)
                        {
                            foreach (string tu in tuArr)
                            {
                                string tuStandard = tu.Trim().ToUpper();
                                foreach (DataRow row in dt.Rows)
                                {
                                    string code = row["Code"].ToString();
                                    if (!string.IsNullOrEmpty(code))
                                    {
                                        string codeStandard = code.Trim().ToUpper();
                                        if (tuStandard.Equals(codeStandard))
                                        {
                                            listPath.Add(row["Path"].ToString().Trim().ToUpper());
                                            break;
                                        }
                                    }
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
            return listPath;
        }
    }
}
