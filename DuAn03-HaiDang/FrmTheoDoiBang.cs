using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.KeyPad_Chuyen.dao;
using System.Configuration;
using DuAn03_HaiDang.KeyPad_Chuyen.pojo;
using DuAn03_HaiDang.Enum;
using DuAn03_HaiDang.DAO; 
using PMS.Business;
using PMS.Data;
using PMS.Business.Enum;

namespace DuAn03_HaiDang
{
    public partial class FrmTheoDoiBang : FormBase
    {
        private DataTable dtLoadData;
        private List<DuAn03_HaiDang.KeyPad_Chuyen.pojo.Chuyen> listChuyen;
        private ChuyenDAO chuyenDAO;
    //    private AppConfigDAO AppConfigDAO;
         
        private int AppId = 0;
        public FrmTheoDoiBang()
        {
            InitializeComponent();
            dtLoadData = new DataTable();
            listChuyen = new List<DuAn03_HaiDang.KeyPad_Chuyen.pojo.Chuyen>();
            chuyenDAO = new ChuyenDAO();
        //    AppConfigDAO = new AppConfigDAO();
             
            int.TryParse(ConfigurationManager.AppSettings["AppId"].ToString(), out AppId);
            
        }
        DataTable dt = new DataTable();
        private void LoadData()
        {
            dtLoadData.Clear();
            try
            {
                if (listChuyen != null && listChuyen.Count > 0)
                {
                   // dbclass.listAppConfig = AppConfigDAO.GetListAppConfig(0);

                    var appConfigs = BLLConfig.Instance.GetAll(0);
                    var config = appConfigs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eAppConfigName.KieuTinhNhipThucTe));
                    var cfType = config != null ? config.Value : "1";
                    config = appConfigs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eAppConfigName.KieuTinhTyLeHangLoi));
                    var cfErrorType = config != null ? config.Value : "1";

                    foreach (var chuyen in listChuyen)
                    {
                        var listSTTChuyen_SanPham = chuyenDAO.FindListSTTChuyen_SanPhamOfDateNow(chuyen.MaChuyen);
                        if (listSTTChuyen_SanPham != null && listSTTChuyen_SanPham.Count > 0)
                        {
                            foreach (var sttChuyen_SanPham in listSTTChuyen_SanPham)
                            {
                                #region Lam_Old
                                //string strSQL = "SELECT c.TenChuyen, tp.LaoDongChuyen, sp.TenSanPham, csp.SanLuongKeHoach, csp.IsFinish, csp.LuyKeTH,nx.DinhMucNgay,";
                                //strSQL += " (SELECT SUM(BTPNgay) FROM BTP b WHERE b.STTChuyen_SanPham = '" + sttChuyen_SanPham + "'AND b.IsEndOfLine=1 and CommandTypeId=" + (int)eCommandRecive.BTPIncrease + ") LuyKeBTPTang,";
                                //strSQL += " (SELECT SUM(BTPNgay) FROM BTP b WHERE b.STTChuyen_SanPham = '" + sttChuyen_SanPham + "' and b.IsEndOfLine=1 and CommandTypeId=" + (int)eCommandRecive.BTPReduce + ") LuyKeBTPGiam,";
                                //strSQL += " (SELECT SUM(BTPNgay) FROM BTP b WHERE b.STTChuyen_SanPham = '" + sttChuyen_SanPham + "'AND b.Ngay ='" + DateTime.Now.Date.ToString() + "' and b.IsEndOfLine=1 and CommandTypeId=" + (int)eCommandRecive.BTPIncrease + ") BTPNgayTang,";
                                //strSQL += " (SELECT SUM(BTPNgay) FROM BTP b WHERE b.STTChuyen_SanPham = '" + sttChuyen_SanPham + "'AND b.Ngay ='" + DateTime.Now.Date.ToString() + "' and b.IsEndOfLine=1 and CommandTypeId=" + (int)eCommandRecive.BTPReduce + ") BTPNgayGiam,";
                                //strSQL += " (nx.ThucHienNgay-nx.ThucHienNgayGiam) ThucHienNgay, (nx.SanLuongLoi-nx.SanLuongLoiGiam) SanLuongLoi, ";
                                //strSQL += " case when nx.DinhMucNgay=0 then 0 else (ROUND((((nx.ThucHienNgay-nx.ThucHienNgayGiam)/nx.DinhMucNgay)*100),0))end TyLeThucHien, ";
                                //strSQL += " case when nx.DinhMucNgay=0 then 0 else (ROUND((((nx.SanLuongLoi-nx.SanLuongLoiGiam)/nx.DinhMucNgay)*100),0))end  TyLeLoi,  nx.BTPTrenChuyen, (ROUND((nx.BTPTrenChuyen/tp.LaoDongChuyen),0))Von, nx.NhipDoSanXuat, nx.NhipDoThucTe,nx.NhipDoThucTeBTPThoatChuyen,";
                                //strSQL += " case when nx.NhipDoThucTe = 0 then 0 else ROUND((nx.NhipDoSanXuat/nx.NhipDoThucTe)*100,0)end  TiLeNhip,";
                                //strSQL += " ROUND((nx.ThucHienNgay-nx.ThucHienNgayGiam)*sp.DonGiaCM,2) DoanhThuNgay, (select ROUND(SUM((nx.ThucHienNgay-nx.ThucHienNgayGiam)*sp.DonGiaCM),2) from NangXuat nx, SanPham sp, Chuyen_SanPham c_sp ";
                                //strSQL += " where nx.STTCHuyen_SanPham=c_sp.STT and c_sp.MaSanPham=sp.MaSanPham and c_sp.STT='" + sttChuyen_SanPham + "') DoanhThuThang, (nx.BTPThoatChuyenNgay-nx.BTPThoatChuyenNgayGiam) BTPThoatChuyenNgay, csp.LuyKeBTPThoatChuyen";
                                //strSQL += " FROM Chuyen c, Chuyen_SanPham csp, NangXuat nx, SanPham sp, ThanhPham tp WHERE c.IsDeleted = 0 and csp.IsDelete = 0 and nx.IsDeleted = 0 and sp.IsDelete = 0 and tp.IsDeleted = 0 and csp.STT ='" + sttChuyen_SanPham + "' AND nx.STTCHuyen_SanPham = csp.STT  and nx.Ngay ='" + DateTime.Now.Date.ToString() + "' AND csp.MaSanPham = sp.MaSanPham AND tp.STTChuyen_SanPham = csp.STT AND tp.Ngay = nx.Ngay AND c.MaChuyen = csp.MaChuyen";
                                #endregion

                                string strSQL = "SELECT c.TenChuyen, tp.LaoDongChuyen, sp.TenSanPham, csp.SanLuongKeHoach, csp.IsFinish, csp.LuyKeTH,nx.DinhMucNgay,";
                                strSQL += " (SELECT SUM(BTPNgay) FROM BTP b WHERE b.STTChuyen_SanPham = '" + sttChuyen_SanPham + "'AND b.IsEndOfLine=1 and CommandTypeId=" + (int)eCommandRecive.BTPIncrease + ") LuyKeBTPTang,";
                                strSQL += " (SELECT SUM(BTPNgay) FROM BTP b WHERE b.STTChuyen_SanPham = '" + sttChuyen_SanPham + "' and b.IsEndOfLine=1 and CommandTypeId=" + (int)eCommandRecive.BTPReduce + ") LuyKeBTPGiam,";
                                strSQL += "(nx.BTPTang-nx.BTPGiam) BTPNgay,";
                                strSQL += " (nx.ThucHienNgay-nx.ThucHienNgayGiam) ThucHienNgay, (nx.SanLuongLoi-nx.SanLuongLoiGiam) SanLuongLoi, ";
                                strSQL += " case when nx.DinhMucNgay=0 then 0 else (ROUND((((nx.ThucHienNgay-nx.ThucHienNgayGiam)/nx.DinhMucNgay)*100),0))end TyLeThucHien, ";
                                strSQL += " case when nx.DinhMucNgay=0 then 0 else (ROUND((((nx.SanLuongLoi-nx.SanLuongLoiGiam)/nx.DinhMucNgay)*100),0))end  TyLeLoi,  nx.BTPTrenChuyen,  nx.NhipDoSanXuat, nx.NhipDoThucTe, nx.NhipDoThucTeBTPThoatChuyen,";
                                strSQL += " case when nx.NhipDoThucTe = 0 then 0 else ROUND((nx.NhipDoSanXuat/nx.NhipDoThucTe)*100,0)end  TiLeNhip,";
                                strSQL += " ROUND((nx.ThucHienNgay-nx.ThucHienNgayGiam)*sp.DonGiaCM,2) DoanhThuNgay, (select ROUND(SUM((nx.ThucHienNgay-nx.ThucHienNgayGiam)*sp.DonGiaCM),2) from NangXuat nx, SanPham sp, Chuyen_SanPham c_sp ";
                                strSQL += " where nx.STTCHuyen_SanPham=c_sp.STT and c_sp.MaSanPham=sp.MaSanPham and c_sp.STT='" + sttChuyen_SanPham + "') DoanhThuThang, (nx.BTPThoatChuyenNgay-nx.BTPThoatChuyenNgayGiam) BTPThoatChuyenNgay, csp.LuyKeBTPThoatChuyen";
                                strSQL += " FROM Chuyen c, Chuyen_SanPham csp, NangXuat nx, SanPham sp, ThanhPham tp WHERE c.IsDeleted = 0 and csp.IsDelete = 0 and nx.IsDeleted = 0 and sp.IsDelete = 0 and tp.IsDeleted = 0 and csp.STT ='" + sttChuyen_SanPham + "' AND nx.STTCHuyen_SanPham = csp.STT  and nx.Ngay ='" + DateTime.Now.Date.ToString() + "' AND csp.MaSanPham = sp.MaSanPham AND tp.STTChuyen_SanPham = csp.STT AND tp.Ngay = nx.Ngay AND c.MaChuyen = csp.MaChuyen";


                                dt.Clear();
                                dt = dbclass.TruyVan_TraVe_DataTable(strSQL);
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        DataGridViewRow row = new DataGridViewRow();
                                        DataGridViewCell cell;

                                        cell = new DataGridViewTextBoxCell();
                                        cell.Value = dt.Rows[j]["TenChuyen"].ToString();
                                        row.Cells.Add(cell);

                                        int laoDongChuyen = 0;
                                        int.TryParse(dt.Rows[j]["LaoDongChuyen"].ToString(), out laoDongChuyen);
                                        cell = new DataGridViewTextBoxCell();
                                        cell.Value = laoDongChuyen;
                                        row.Cells.Add(cell);

                                        cell = new DataGridViewTextBoxCell();
                                        cell.Value = dt.Rows[j]["TenSanPham"].ToString();
                                        row.Cells.Add(cell);

                                        int sanLuongKeHoach = 0;
                                        int.TryParse(dt.Rows[j]["SanLuongKeHoach"].ToString(), out sanLuongKeHoach);
                                        cell = new DataGridViewTextBoxCell();
                                        cell.Value = sanLuongKeHoach;
                                        row.Cells.Add(cell);

                                        int luyKeTH = 0;
                                        int.TryParse(dt.Rows[j]["LuyKeTH"].ToString(), out luyKeTH);
                                        cell = new DataGridViewTextBoxCell();
                                        cell.Value = luyKeTH < 0 ? 0 : luyKeTH;
                                        row.Cells.Add(cell);

                                        int luyKeBTPThoatChuyen = 0;
                                        int.TryParse(dt.Rows[j]["LuyKeBTPThoatChuyen"].ToString(), out luyKeBTPThoatChuyen);
                                        cell = new DataGridViewTextBoxCell();
                                        cell.Value = luyKeBTPThoatChuyen < 0 ? 0 : luyKeBTPThoatChuyen;
                                        row.Cells.Add(cell);

                                        int luyKeBTPTang = 0;
                                        int.TryParse(dt.Rows[j]["LuyKeBTPTang"].ToString(), out luyKeBTPTang);
                                        int luyKeBTPGiam = 0;
                                        int.TryParse(dt.Rows[j]["LuyKeBTPGiam"].ToString(), out luyKeBTPGiam);
                                        cell = new DataGridViewTextBoxCell();
                                        int luyKeBTP = luyKeBTPTang - luyKeBTPGiam;
                                        cell.Value = luyKeBTP < 0 ? 0 : luyKeBTP;
                                        row.Cells.Add(cell);

                                        float dinhMucNgay = 0;
                                        float.TryParse(dt.Rows[j]["DinhMucNgay"].ToString(), out dinhMucNgay);
                                        cell = new DataGridViewTextBoxCell();
                                        cell.Value = (int)dinhMucNgay;
                                        row.Cells.Add(cell);

                                        int thucHienNgay = 0;
                                        int.TryParse(dt.Rows[j]["ThucHienNgay"].ToString(), out thucHienNgay);
                                        cell = new DataGridViewTextBoxCell();
                                        cell.Value = thucHienNgay < 0 ? 0 : thucHienNgay;
                                        row.Cells.Add(cell);

                                        int btpThoatChuyenNgay = 0;
                                        int.TryParse(dt.Rows[j]["BTPThoatChuyenNgay"].ToString(), out btpThoatChuyenNgay);
                                        cell = new DataGridViewTextBoxCell();
                                        cell.Value = btpThoatChuyenNgay < 0 ? 0 : btpThoatChuyenNgay;
                                        row.Cells.Add(cell);

                                        //HoangHai
                                        int btpNgayTang = 0;
                                        int.TryParse(dt.Rows[j]["BTPNgay"].ToString(), out btpNgayTang);
                                        //int btpNgayGiam = 0;
                                        //int.TryParse(dt.Rows[j]["BTPNgayGiam"].ToString(), out btpNgayGiam);
                                        cell = new DataGridViewTextBoxCell();
                                        int btpNgay = btpNgayTang;
                                        cell.Value = btpNgay < 0 ? 0 : btpNgay;
                                        row.Cells.Add(cell);

                                        int sanLuongLoi = 0;
                                        int.TryParse(dt.Rows[j]["SanLuongLoi"].ToString(), out sanLuongLoi);
                                        cell = new DataGridViewTextBoxCell();
                                        cell.Value = sanLuongLoi < 0 ? 0 : sanLuongLoi;
                                        row.Cells.Add(cell);

                                        int btpTrenChuyen = 0;
                                        int.TryParse(dt.Rows[j]["BTPTrenChuyen"].ToString(), out btpTrenChuyen);
                                        cell = new DataGridViewTextBoxCell();
                                        cell.Value = btpTrenChuyen < 0 ? 0 : btpTrenChuyen;
                                        row.Cells.Add(cell);
                                                
                                        float tyLeThucHien = 0;
                                        float.TryParse(dt.Rows[j]["TyLeThucHien"].ToString(), out tyLeThucHien);
                                        cell = new DataGridViewTextBoxCell();
                                        cell.Value = tyLeThucHien < 0 ? 0 : tyLeThucHien;
                                        row.Cells.Add(cell);

                                        float tyLeLoi = 0;
                                        switch (cfErrorType)
                                        {
                                            case "1": float.TryParse(dt.Rows[j]["TyLeLoi"].ToString(), out tyLeLoi); break;
                                            case "2": tyLeLoi = sanLuongLoi > 0 ? (float)Math.Round((((double)sanLuongLoi / ((double)sanLuongLoi + (double)thucHienNgay)) * 100), 2) : 0; break;
                                            case "3": tyLeLoi = sanLuongLoi > 0 && btpThoatChuyenNgay > 0 ? (float)Math.Round(((double)((double)sanLuongLoi / (double)btpThoatChuyenNgay) * 100), 2) : 0; break;
                                        }

                                        cell = new DataGridViewTextBoxCell();
                                        cell.Value = tyLeLoi < 0 ? 0 : tyLeLoi;
                                        row.Cells.Add(cell);

                                        int von = (int)(Math.Ceiling((double)btpTrenChuyen / laoDongChuyen));
                                        cell = new DataGridViewTextBoxCell();
                                        cell.Value = von < 0 ? 0 : von;
                                        row.Cells.Add(cell);

                                        float doanhThuThang = 0;
                                        float.TryParse(dt.Rows[j]["DoanhThuThang"].ToString(), out doanhThuThang);
                                        cell = new DataGridViewTextBoxCell();
                                        cell.Value = doanhThuThang < 0 ? 0 : doanhThuThang;
                                        row.Cells.Add(cell);

                                        float doanhThuNgay = 0;
                                        float.TryParse(dt.Rows[j]["DoanhThuNgay"].ToString(), out doanhThuNgay);
                                        cell = new DataGridViewTextBoxCell();
                                        cell.Value = doanhThuNgay < 0 ? 0 : doanhThuNgay;
                                        row.Cells.Add(cell);
                                        
                                        float nhipDoSanXuat = 0;
                                        float.TryParse(dt.Rows[j]["NhipDoSanXuat"].ToString(), out nhipDoSanXuat);
                                        cell = new DataGridViewTextBoxCell();
                                        cell.Value = nhipDoSanXuat < 0 ? 0 : nhipDoSanXuat;
                                        row.Cells.Add(cell);

                                        float nhipDoThucTe = 0;
                                        float tiLeNhip = 0;
                                        if (cfType == "1")
                                        {
                                            float.TryParse(dt.Rows[j]["NhipDoThucTe"].ToString(), out nhipDoThucTe);
                                            float.TryParse(dt.Rows[j]["TiLeNhip"].ToString(), out tiLeNhip);
                                        }
                                        else
                                        {
                                            float.TryParse(dt.Rows[j]["NhipDoThucTeBTPThoatChuyen"].ToString(), out nhipDoThucTe);
                                            tiLeNhip = (nhipDoThucTe == 0 ? 0 : (float)(Math.Round((nhipDoSanXuat / nhipDoThucTe) * 100, 0)));
                                        }
                                        cell = new DataGridViewTextBoxCell();
                                        cell.Value = nhipDoThucTe < 0 ? 0 : nhipDoThucTe;
                                        row.Cells.Add(cell);

                                        cell = new DataGridViewTextBoxCell();
                                        cell.Value = tiLeNhip < 0 ? 0 : tiLeNhip;
                                        row.Cells.Add(cell);

                                        int isFinish = 0;
                                        int.TryParse(dt.Rows[j]["IsFinish"].ToString(), out isFinish);

                                        //dgTTNangXuat.Rows.Add(row);
                                        //if (dgTTNangXuat.Rows.Count % 2 == 0)
                                        //{
                                        //    dgTTNangXuat.Rows[dgTTNangXuat.Rows.Count - 1].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                                        //}
                                        //if (isFinish == 1)
                                        //    dgTTNangXuat.Rows[dgTTNangXuat.Rows.Count - 1].DefaultCellStyle.BackColor = Color.PaleVioletRed;

                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void butUpdate_Click(object sender, EventArgs e)
        {
            GetNew();
        }

        private void FrmTheoDoiBang_Load(object sender, EventArgs e)
        {
            //listChuyen = chuyenDAO.GetListChuyenInfByListId(AccountSuccess.strListChuyenId);
            //LoadData();
            GetNew();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //dgTTNangXuat.Rows.Clear();
            //dgTTNangXuat.Refresh();
            //LoadData();
            GetNew();
        }

        private void GetNew()
        { 
            var data = BLLProductivity.GetProductivitiesInDay(AccountSuccess.strListChuyenId.Split(',').Select(x=> Convert.ToInt32(x)).ToList(), AppId);
            dgTTNangXuat.DataSource = data;
            for (int i = 0; i < data.Count; i++) 
            {
                if (data[i].IsFinish )
                    dgTTNangXuat.Rows[i].DefaultCellStyle.BackColor = Color.PaleVioletRed;
            }
        }

    }
}
