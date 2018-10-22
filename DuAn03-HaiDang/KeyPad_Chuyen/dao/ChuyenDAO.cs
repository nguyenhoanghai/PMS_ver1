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
    public class ChuyenDAO
    {
        DataTable dt;
        public ChuyenDAO()
        {
            dt = new DataTable();
        }
        public DataTable GetDataTableLineByStrListId(string strListId)
        {
            dt.Clear();            
            try
            {
                string sql = "select c.MaChuyen, c.Code, c.TenChuyen, c.Sound from Chuyen c Where c.IsDeleted=0 and MaChuyen in ("+strListId+") ORDER By MaChuyen DESC";
               dt = dbclass.TruyVan_TraVe_DataTable(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public List<Chuyen> GetListDSChuyen(string strListChuyen)
        {
            List<Chuyen> listChuyen = new List<Chuyen>();
            try
            {
                string sql = "select c.MaChuyen, c.Code, c.TenChuyen, c.Sound, c.FloorId, c.IdDen, c.IdDenNangSuat, c.IdTyLeDoc, c.IsEndDate, c.IntSTT, c.STTReadNS, c.LaoDongDinhBien, f.Name as FloorName from Chuyen c, Floor f Where c.MaChuyen in ("+strListChuyen+") and c.FloorId = f.IdFloor and c.IsDeleted=0  ORDER By MaChuyen DESC";
                DataTable dataTable = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        int intSTT = 1;
                        int.TryParse(row["IntSTT"].ToString(), out intSTT);
                        int sttReadNS = 1;
                        int.TryParse(row["STTReadNS"].ToString(), out sttReadNS);
                        int laoDongDinhBien=0;
                        int.TryParse(row["LaoDongDinhBien"].ToString(), out laoDongDinhBien);
                        if (!string.IsNullOrEmpty(row["TenChuyen"].ToString()))
                            listChuyen.Add(new Chuyen() { MaChuyen = row["MaChuyen"].ToString(), Code = row["Code"].ToString(), TenChuyen = row["TenChuyen"].ToString(), Sound = row["Sound"].ToString(), IntSTT = intSTT, STTReadNS = sttReadNS, Floor = row["FloorId"].ToString(), FloorName = row["FloorName"].ToString(),
                                                          LaoDongDinhBien = laoDongDinhBien,
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listChuyen;
        }

        public List<Chuyen> GetListDSChuyenByFloorId(string floor)
        {
            List<Chuyen> listChuyen = new List<Chuyen>();
            try
            {
                string sql = "select c.MaChuyen, c.Code, c.TenChuyen, c.Sound, c.FloorId, c.IdDen, c.IdDenNangSuat, c.IdTyLeDoc, c.IsEndDate, c.IntSTT, c.STTReadNS, f.Name as FloorName from Chuyen c, Floor f Where c.FloorId = f.IdFloor and c.IsDeleted=0 and c.FloorId = '" + floor + "' ORDER By MaChuyen DESC";
                DataTable dataTable = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        int intSTT = 1;
                        int.TryParse(row["IntSTT"].ToString(), out intSTT);
                        int sttReadNS = 1;
                        int.TryParse(row["STTReadNS"].ToString(), out intSTT);                       
                        if (!string.IsNullOrEmpty(row["TenChuyen"].ToString()))
                            listChuyen.Add(new Chuyen() { MaChuyen = row["MaChuyen"].ToString(), Code = row["Code"].ToString(), TenChuyen = row["TenChuyen"].ToString(), Sound=row["Sound"].ToString(), IntSTT = intSTT, STTReadNS = sttReadNS, Floor = row["FloorId"].ToString() });
                    }
                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            return listChuyen;
        }

        public int AddObj(Chuyen chuyen)
        {
            int kq = 0;
            try
            {
                string sql = "insert into Chuyen(Code, TenChuyen, DinhNghia, Sound, Floor, LaoDongDinhBien) values('" + chuyen.Code + "',N'" + chuyen.TenChuyen + "',N'" + chuyen.DinhNghia + "', N'"+chuyen.Sound+"', "+chuyen.Floor+", "+chuyen.LaoDongDinhBien+")";
                kq = dbclass.TruyVan_XuLy(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return kq;
        }

        public int UpdateObj(Chuyen chuyen)
        {
            int kq = 0;
            try
            {
                string sql = "update Chuyen set Code='"+chuyen.Code+"', TenChuyen =N'" + chuyen.TenChuyen + "', DinhNghia = N'" + chuyen.DinhNghia + "', Sound=N'"+chuyen.Sound+"', Floor="+chuyen.Floor+", LaoDongDinhBien="+chuyen.LaoDongDinhBien+" where MaChuyen = '" + chuyen.MaChuyen + "' and IsDeleted=0";
                kq = dbclass.TruyVan_XuLy(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return kq;
        }

        public int DeleteObj(int MaChuyen)
        {
            int kq = 0;
            try
            {
                string sql = "update Chuyen set IsDeleted=1 where MaChuyen ='" + MaChuyen + "'";
                kq = dbclass.TruyVan_XuLy(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return kq;
        }

        public List<Chuyen> GetListChuyenInfByListId(string listIdstr)
        {
            List<Chuyen> listChuyenInf = new List<Chuyen>();
            try
            {
                if (!string.IsNullOrEmpty(listIdstr))
                {
                    string sql = "select c.MaChuyen, c.Code, c.TenChuyen, c.Sound, c.FloorId, c.IdDen, c.IdDenNangSuat, c.IdTyLeDoc, c.IsEndDate, c.IntSTT, c.STTReadNS, r.Name TenTyLeDoc from Chuyen c left join ReadPercent r on c.IdTyLeDoc=r.Id Where c.MaChuyen in (" + listIdstr + ")";
                    DataTable dt = dbclass.TruyVan_TraVe_DataTable(sql);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            Chuyen Chuyen = new Chuyen();
                            Chuyen.MaChuyen = row["MaChuyen"].ToString();
                            Chuyen.TenChuyen = row["TenChuyen"].ToString();
                            int sttReadNS = 0;
                            int.TryParse(row["STTReadNS"].ToString(), out sttReadNS);
                            Chuyen.STTReadNS = sttReadNS;
                            int idDen = 0;
                            int.TryParse(row["IdDen"].ToString(), out idDen);
                            Chuyen.IdDen = idDen;
                            int idDenNangSuat = 0;
                            int.TryParse(row["IdDenNangSuat"].ToString(), out idDenNangSuat);
                            Chuyen.IdDenNangSuat = idDenNangSuat;
                            Chuyen.Floor = row["FloorId"].ToString();
                            Chuyen.Sound = row["Sound"].ToString();
                            int idTyLeDoc = 0;
                            int.TryParse(row["IdTyLeDoc"].ToString(), out idTyLeDoc);
                            Chuyen.IdTyLeDoc = idTyLeDoc;
                            Chuyen.TenTyLeDoc = row["TenTyLeDoc"].ToString().Trim();
                            listChuyenInf.Add(Chuyen);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listChuyenInf;
        }

        public List<string> FindListSTTChuyen_SanPham(string IdChuyen)
        {
            var dateNow = DateTime.Now;
            List<string> listSTTChuyen_SanPham = new List<string>();
            dt.Clear();
            string strSQL = "SELECT csp.STT, c.TenChuyen FROM Chuyen_SanPham csp, Chuyen c, SanPham sp WHERE csp.MaChuyen = '" + IdChuyen + "' and csp.MaChuyen = c.MaChuyen and csp.IsFinish = 0 and csp.IsDelete = 0 and sp.IsDelete=0 and csp.MaSanPham = sp.MaSanPham and sp.IsDelete=0 and csp.Thang=" + dateNow.Month + " and csp.Nam=" + dateNow.Year + " Order By csp.STTThucHien ASC";
            try
            {
                dt = dbclass.TruyVan_TraVe_DataTable(strSQL);
                if (dt!=null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string sttChuyen_SanPham = dt.Rows[i]["STT"].ToString();
                        if (!string.IsNullOrEmpty(sttChuyen_SanPham))
                            listSTTChuyen_SanPham.Add(sttChuyen_SanPham);
                    }
                }                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listSTTChuyen_SanPham;
        }

        public List<string> FindListSTTChuyen_SanPhamOfDateNow(string IdChuyen)
        {
            var dateNow = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
            List<string> listSTTChuyen_SanPham = new List<string>();
           // string strSQL = "SELECT csp.STT, c.TenChuyen FROM Chuyen_SanPham csp, Chuyen c, SanPham sp WHERE csp.MaChuyen = '" + IdChuyen + "' and csp.MaChuyen = c.MaChuyen and csp.IsDelete = 0 and sp.IsDelete=0 and csp.MaSanPham = sp.MaSanPham and sp.IsDelete=0 and Thang=" + (dateNow.Month) + " and Nam=" + dateNow.Year + " and csp.STT In (select STTChuyen_SanPham from NangXuat where Ngay='" + dateNow.Date + "' and IsDeleted=0 ) Order By csp.STTThucHien ASC";
            string strSQL = "SELECT csp.STT, c.TenChuyen FROM Chuyen_SanPham csp, Chuyen c, SanPham sp WHERE csp.MaChuyen = '" + IdChuyen + "' and csp.MaChuyen = c.MaChuyen and csp.IsDelete = 0 and sp.IsDelete=0 and csp.MaSanPham = sp.MaSanPham and sp.IsDelete=0 and csp.STT In (select STTChuyen_SanPham from NangXuat where Ngay='" + dateNow + "' and IsDeleted=0 ) Order By csp.STTThucHien ASC";
            try
            {
                dt = dbclass.TruyVan_TraVe_DataTable(strSQL);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string sttChuyen_SanPham = dt.Rows[i]["STT"].ToString();
                        if (!string.IsNullOrEmpty(sttChuyen_SanPham))
                            listSTTChuyen_SanPham.Add(sttChuyen_SanPham);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listSTTChuyen_SanPham;
        }
        
        public ChuyenSanPham GetChuyenSanPhamInfByChuyenId(string IdChuyen)
        {
            ChuyenSanPham ChuyenSanPham = null;          
            string strSQL = "SELECT TOP 1 csp.STT, c.TenChuyen, csp.SanLuongKeHoach, sp.TenSanPham FROM Chuyen_SanPham csp, Chuyen c, SanPham sp WHERE csp.MaChuyen = '" + IdChuyen + "' and csp.MaChuyen = c.MaChuyen and csp.IsFinish = 0 and csp.IsDelete = 0 and sp.IsDelete=0 and csp.MaSanPham = sp.MaSanPham and sp.IsDelete=0 and csp.Thang="+DateTime.Now.Month+" and csp.Nam="+DateTime.Now.Year+" Order By csp.STTThucHien ASC";
            try
            {
                dt.Clear();
                dt = dbclass.TruyVan_TraVe_DataTable(strSQL);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ChuyenSanPham = new ChuyenSanPham();
                    ChuyenSanPham.STT = dt.Rows[0]["STT"].ToString();
                    ChuyenSanPham.TenSanPham = dt.Rows[0]["TenSanPham"].ToString();
                    int sanLuongKeHoach=0;
                    int.TryParse(dt.Rows[0]["SanLuongKeHoach"].ToString(), out sanLuongKeHoach);
                    ChuyenSanPham.SanLuongKeHoach = sanLuongKeHoach;
                    ChuyenSanPham.TenChuyen = dt.Rows[0]["TenChuyen"].ToString();
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ChuyenSanPham;
        }

        public bool UpdateThuTuDocAmThanh(int idChuyen, int thuTuDoc)
        {
            bool result = false;
            try
            {
                if (idChuyen > 0 && thuTuDoc > 0)
                {
                    string sql = "update Chuyen set STTReadNS=" + thuTuDoc + " where MaChuyen=" + idChuyen;
                    dbclass.TruyVan_XuLy(sql);
                    result = true;
                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            return result;
        }

        public bool UpdateFileAmThanh(int idChuyen, string soundFile)
        {
            bool result = false;
            try
            {
                if (idChuyen > 0 && !string.IsNullOrEmpty(soundFile))
                {
                    string sql = "update Chuyen set Sound='" + soundFile + "' where MaChuyen=" + idChuyen;
                    dbclass.TruyVan_XuLy(sql);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public bool CheckExistCode(string code, int id)
        {
            bool result = false;
            try
            {
                string sql = string.Empty;
                if(id==0)
                    sql = "Select * from Chuyen where Code='"+code.Trim()+"'";
                else
                    sql = "Select * from Chuyen where Code='" + code.Trim() + "' and MaChuyen!="+id;
                DataTable dt = dbclass.TruyVan_TraVe_DataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                    result = true;
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            return result;
        }

        public int GetSanLuongLoiCuaChuyen(int errorId, int maChuyen, int sttChuyenSanPham)
        {
            try
            {
                int sanLuong = 0;
                string strSQLSelectCumCuoi = "Select Id From Cum Where IdChuyen=" + maChuyen + " and IsDeleted=0 and IsEndOfLine=1";
                DataTable dtCum = dbclass.TruyVan_TraVe_DataTable(strSQLSelectCumCuoi);
                if (dtCum != null && dtCum.Rows.Count > 0)
                {
                    var row = dtCum.Rows[0];
                    int id = 0;
                    int.TryParse(row["Id"].ToString(), out id);
                    string strSQLSanLuong = "Select SoLuongTang, SoLuongGiam From NangSuat_CumLoi Where STTChuyenSanPham=" + sttChuyenSanPham + " and CumId=" + id + " and ErrorId=" + errorId+" and Ngay='"+DateTime.Now.Date+"'";
                    DataTable dtSanLuong = dbclass.TruyVan_TraVe_DataTable(strSQLSanLuong);
                    if (dtSanLuong != null && dtSanLuong.Rows.Count > 0)
                    {                        
                        DataRow rowSanLuong = dtSanLuong.Rows[0];
                        int sanLuongTang = 0;
                        int sanLuongGiam = 0;
                        int.TryParse(rowSanLuong["SoLuongTang"].ToString(), out sanLuongTang);
                        int.TryParse(rowSanLuong["SoLuongGiam"].ToString(), out sanLuongGiam);
                        sanLuong = (sanLuongTang - sanLuongGiam);
                       
                    }
                }
                return sanLuong;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public Chuyen GetLineByName(string lineName, string strListLineId)
        {
            try
            {
                Chuyen chuyen = null;
                var listLine = GetListChuyenInfByListId(strListLineId);
                if (listLine != null && listLine.Count>0)
                {
                    chuyen = listLine.Where(c => c.TenChuyen.Trim().ToUpper() == lineName.Trim().ToUpper()).FirstOrDefault();
                }
                return chuyen;
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        public Chuyen GetLineById(string lineId, string strListLineId)
        {
            try
            {
                Chuyen chuyen = null;
                var listLine = GetListChuyenInfByListId(strListLineId);
                if (listLine != null && listLine.Count > 0)
                {
                    chuyen = listLine.Where(c => c.MaChuyen.Trim()==lineId.Trim()).FirstOrDefault();
                }
                return chuyen;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
