using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuAn03_HaiDang.POJO;
using DuAn03_HaiDang.DATAACCESS;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace DuAn03_HaiDang.DAO
{
    class DenDAO
    {
        private DataTable dt;
        public DenDAO()
        {
            this.dt = new DataTable();
        }
        public int ThemTTDen(Den den)
        {
            int kq = 0;
            try
            {
                string sql="";
                if (den.STTParent != null)
                {
                    sql = "insert into Den(Color, ValueFrom, ValueTo, IdCatalogTable, STTParent, MaMauDen) values(N'" + den.Color + "','" + den.ValueFrom + "','" + den.ValueTo + "','" + den.IdCatalogTable + "'," + den.STTParent + ",'"+den.MaMauDen+"')";
                }
                else
                {
                    sql = "insert into Den(Color, ValueFrom, ValueTo, IdCatalogTable) values(N'" + den.Color + "','" + den.ValueFrom + "','" + den.ValueTo + "','" + den.IdCatalogTable + "')";
                }
                kq = dbclass.TruyVan_XuLy(sql);
                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể thêm thông tin mới vào CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }

        public int SuaThongTinDen(Den den)
        {
            int kq = 0;
            try
            {

                string sql = "update Den set ValueFrom='" + den.ValueFrom + "' , ValueTo='" + den.ValueTo + "' where STT = '" + den.STT + "'";
                kq = dbclass.TruyVan_XuLy(sql);

                return kq;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể thay đổi thông tin mặt hàng dưới CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }        

        public string FindToFinalId()
        {
            dt.Clear();
            string Id = "";
            try
            {

                string sql = "Select top 1 STT from Den order by STT DESC";
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    Id = dt.Rows[0][0].ToString();
                }
                return Id;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể lấy id cuối cùng của bảng Den dưới CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return Id;
            }
        }

        public string TestName(string Name)
        {
            dt.Clear();
            string Id = "";
            try
            {

                string sql = "SELECT STT FROM den WHERE Color =N'"+Name+"'";
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    Id = dt.Rows[0][0].ToString();
                }
                return Id;
            }
            catch (Exception)
            {
                return Id;
            }
        }

        public int XoaTyLeDen(string Id)
        {
            int kq = 0;
            try
            {
                /////////
                SqlConnection conn = dbclass.taoketnoi();
                string sql = "delete from Den where STTParent ='" + Id + "' " + "delete from Den where STT ='" + Id + "'";
                SqlCommand cm = new SqlCommand();
                cm.CommandType = CommandType.Text; //loại lệnh
                cm.CommandText = sql; // gán câu lệnh SQL cho SQLCommand
                cm.Connection = conn;//ấn định kết nối cho đối tượng SQLCommand
                SqlTransaction transaction;//tạo đối tượng transaction
                transaction = conn.BeginTransaction();//bắt đầu quá trình quản lý Transaction
                cm.Transaction = transaction;//gắn Transaction vào đối tượng SQLCommand
                try
                {
                    cm.ExecuteNonQuery();
                    transaction.Commit();
                    return 1;
                }
                catch
                {
                    MessageBox.Show("Không thể xoá tỷ lệ này, vì tỷ lệ đang được phân chia cho chuyền. Vui lòng phân chia tỷ lệ lại cho chuyền trước khi thực hiện thao tác này.","Lỗi thao tác", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    transaction.Rollback();
                    return 0;
                }  
                ///////
                //string sql = "delete from Den where STTParent ='"+Id+"'";
                //dbclass.TruyVan_XuLy(sql);
                //sql = "delete from Den where STT ='" + Id + "'";
                //dbclass.TruyVan_XuLy(sql);
                
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi: Không thể thay đổi thông tin mặt hàng dưới CSDL", "Lỗi truy vấn CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return kq;
            }
        }
       

        public List<Den> GetListDenByParentId(string denId, int tableType)
        {
            List<Den> listDen = null;
            try
            {
                string sql = "Select * from Den Where STTParent='" + denId + "' and IdCatalogTable=" + tableType;
                dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    listDen = new List<Den>();
                    foreach (DataRow row in dt.Rows)
                    {
                        Den den = new Den();
                        den.Color = row["Color"].ToString();
                        den.STT = row["STT"].ToString();
                        int valueFrom = 0;
                        int.TryParse(row["ValueFrom"].ToString(), out valueFrom);
                        den.ValueFrom = valueFrom;
                        int valueTo = 0;
                        int.TryParse(row["ValueTo"].ToString(), out valueTo);
                        den.ValueTo = valueTo;
                        int maMauDen = 0;
                        int.TryParse(row["MaMauDen"].ToString(), out maMauDen);
                        den.MaMauDen = maMauDen;
                        listDen.Add(den);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listDen;
        }

        public string GetColorDen(string idDen, int tableType, int tyLeDenThucTe)
        {
            string colorDen = "Black";
            try
            {

                var listTyLeDen = GetListDenByParentId(idDen, tableType);
                if (listTyLeDen!=null && listTyLeDen.Count > 0)
                {                   
                    var den = listTyLeDen.Where(c => tyLeDenThucTe >= c.ValueFrom &&  tyLeDenThucTe < c.ValueTo).FirstOrDefault();
                    if (den != null)
                    {
                        if (den.Color.Trim().ToUpper().Equals("ĐỎ"))
                            colorDen = "Red";
                        else if (den.Color.Trim().ToUpper().Equals("VÀNG"))
                            colorDen = "Yellow";
                        if (den.Color.Trim().ToUpper().Equals("XANH"))
                            colorDen = "Green";
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            return colorDen;
        }
    }
}
