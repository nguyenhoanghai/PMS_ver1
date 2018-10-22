using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DuAn03_HaiDang.DATAACCESS;
using System.Windows.Forms;
using QuanLyNangSuat.Model;
using DuAn03_HaiDang.Enum;
using PMS.Business.Enum;

namespace DuAn03_HaiDang.DAO
{
    public class TheoDoiNgayDAO
    {
        private ErrorDAO errorDAO = new ErrorDAO();
        public void LoadOBJToDataGirdview(DataGridView dg, string MaChuyen, string MaSanPham)
        {

            DataTable dt = new DataTable();
            string Strsql = "select ThanhPham, Time from TheoDoiNgay Where MaChuyen ='" + MaChuyen + "' and MaSanPham = '" + MaSanPham + "' and Date = '" + DateTime.Now.Date + "'";
            dt = dbclass.TruyVan_TraVe_DataTable(Strsql);
            dbclass.loaddataridviewcolorrow(dg, dt);
        }
        public void DeleteAllInformation()
        {
            try
            {
                string strSQLResetIdentity = "DBCC CHECKIDENT ('TheoDoiNgay',RESEED,0)";
                DateTime time = DateTime.Now.AddDays(-14).Date;
                string strSQL = "delete from TheoDoiNgay where Date < '" + time + "'";
                int kq = dbclass.TruyVan_XuLy(strSQL);
                if (kq > 0)
                {
                    dbclass.TruyVan_XuLy(strSQLResetIdentity);
                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }            
        }
        
        public List<ModelInputDayInfo> GetInputDayInfo (string lineId, DateTime date)
        {
            List<ModelInputDayInfo> listInfo = null;
            try
            {
                string strSQL = "Select DISTINCT tt.STT, tt.MaChuyen, ch.TenChuyen, tt.CumId, c.TenCum, tt.IsEndOfLine, tt.MaSanPham, ";
                strSQL += "sp.TenSanPham, tt.ThanhPham, tt.Time, tt.Date, tt.STTChuyenSanPham, tt.CommandTypeId, tt.ProductOutputTypeId, tt.ErrorId";
                strSQL += " From Chuyen ch, Cum c, Error e, TheoDoiNgay tt, SanPham sp Where tt.MaChuyen=ch.MaChuyen and tt.CumId=c.Id ";
                strSQL += "and tt.MaSanPham=sp.MaSanPham and tt.Date='" + date.Date + "' ";
                if (lineId != "0")
                    strSQL +="and ch.MaChuyen=" + lineId;
                DataTable dtInfo = dbclass.TruyVan_TraVe_DataTable(strSQL);
                if(dtInfo!=null && dtInfo.Rows.Count>0)
                {
                    listInfo = new List<ModelInputDayInfo>();
                    var listError = errorDAO.GetListError();
                    foreach(DataRow row in dtInfo.Rows)
                    {
                        ModelInputDayInfo model = new ModelInputDayInfo();
                        int stt = 0;
                        int.TryParse(row["STT"].ToString(), out stt);
                        int maChuyen = 0;
                        int.TryParse(row["MaChuyen"].ToString(), out maChuyen);
                        int cumId = 0;
                        int.TryParse(row["CumId"].ToString(), out cumId);
                        int maSanPham = 0;
                        int.TryParse(row["MaSanPham"].ToString(), out maSanPham);
                        int errorId = 0;
                        int.TryParse(row["ErrorId"].ToString(), out errorId);
                        int commandTypeId = 0;
                        int.TryParse(row["CommandTypeId"].ToString(), out commandTypeId);
                        bool isEndOfLine = false;
                        bool.TryParse(row["IsEndOfLine"].ToString(), out isEndOfLine);
                        int quantity = 0;
                        int.TryParse(row["ThanhPham"].ToString(), out quantity);
                        TimeSpan time = new TimeSpan(0, 0, 0);
                        TimeSpan.TryParse(row["Time"].ToString(), out time);
                        int sttLineProduct = 0;
                        int.TryParse(row["STTChuyenSanPham"].ToString(), out sttLineProduct);
                        int productOutputTypeId = 0;
                        int.TryParse(row["ProductOutputTypeId"].ToString(), out productOutputTypeId);

                        model.STT = stt;
                        model.LineId = maChuyen;
                        model.ClusterId = cumId;
                        model.ProductId = maSanPham;
                        model.ErrorId = errorId;
                        model.CommandTypeId = commandTypeId;
                        switch(commandTypeId)
                        { 
                            case (int)eCommandRecive.ProductIncrease:
                                model.CommandTypeName = "Tăng sản lượng";
                                break;
                            case (int)eCommandRecive.ProductReduce:
                                model.CommandTypeName = "Giảm sản lượng";
                                break;
                            case (int)eCommandRecive.ErrorIncrease:
                                model.CommandTypeName = "Tăng lỗi";
                                break;
                            case (int)eCommandRecive.ErrorReduce:
                                model.CommandTypeName = "Giảm lỗi";
                                break;
                            case (int)eCommandRecive.BTPIncrease:
                                model.CommandTypeName = "Tăng BTP";
                                break;
                            case (int)eCommandRecive.BTPReduce:
                                model.CommandTypeName = "Giảm BTP";
                                break;
                        }
                        model.LineName = row["TenChuyen"].ToString();
                        model.ClusterName = row["TenCum"].ToString();
                        model.IsEndOfLine = isEndOfLine;
                        model.ProductName = row["TenSanPham"].ToString();                        
                        model.QuantityRecieve = quantity;
                        model.Time = time;
                        model.Date = date;
                        model.STTLine_Product = sttLineProduct;
                        model.ProductOutputTypeId = productOutputTypeId;
                        switch(productOutputTypeId)
                        { 
                            case (int)eProductOutputType.KCS:
                                model.ProductOutputTypeName = "Kiểm đạt hàng";
                                break;
                            case (int)eProductOutputType.TC:
                                model.ProductOutputTypeName = "Thoát chuyền hàng";
                                break;
                        }
                        if (errorId>0)
                        {
                            if(listError!=null && listError.Count>0)
                            {
                                var error = listError.Where(c => c.Id == errorId).FirstOrDefault();
                                if (error != null)
                                    model.ErrorName = error.ErrorName;
                            }
                        }
                        listInfo.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {               
                throw ex;
            }
            return listInfo;
        }
    }
}
