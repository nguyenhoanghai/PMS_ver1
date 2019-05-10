using DuAn03_HaiDang.DAO;
using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.Enum;
using DuAn03_HaiDang.KeyPad_Chuyen.dao;
using DuAn03_HaiDang.Model;
using PMS.Business;
using PMS.Business.Enum;
using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace DuAn03_HaiDang
{
    public partial class FrmSendMailAndReadSound : Form
    {
        private NangXuatDAO nangxuatDAO;
        private CumDAO cumDAO;
        //  private MailTemplateFileDAO mailTemplateFileDAO;
        private ChuyenDAO chuyenDAO;
        //  private List<ModelMailSchedule> listMailSchedule;
        private List<MAIL_SCHEDULE> listMailSchedule;
        private DataTable dtLoadData;
        private DataTable dtDataNSChuyenExportExcel;
        private DataTable dtDataNSCumExportExcel;
        private int maxCountCum;
        private SqlConnection sqlConnect;
        private SqlDataAdapter sqlDataAdapter;
        private List<DuAn03_HaiDang.KeyPad_Chuyen.pojo.Chuyen> listChuyen;
        private List<string> listSTTChuyen_SanPham;
        private List<ModelNangSuatCum> listModelNangSuatCum;
        private List<NSCum> listNSCum;
        private ChuyenSanPham ChuyenSanPham;
        private Thread threadSendMail;
        private List<string> listFileAttactment = null;
        private bool isSendMail;
        private string strType;
        private string strHost;
        private int intPort;
        private string strFrom;
        private string strDisplayName;
        private string strPassword;
        private string strSubject;
        private string strBody;
        private string strTo;
        private string error;
        private int timesGetNSInDay;
        private int getBTPInLineByType;
        private DateTime today = DateTime.Now;

        public FrmSendMailAndReadSound(int _timesGetNSInDay, int _getBTPInLineByType)
        {
            InitializeComponent();
            timesGetNSInDay = _timesGetNSInDay;
            getBTPInLineByType = _getBTPInLineByType;
            this.cumDAO = new CumDAO();
            this.chuyenDAO = new ChuyenDAO();
            this.dtLoadData = new DataTable();
            this.dtDataNSChuyenExportExcel = new DataTable();
            this.dtDataNSCumExportExcel = new DataTable();
            this.listSTTChuyen_SanPham = new List<string>();
            this.listModelNangSuatCum = new List<ModelNangSuatCum>();
            this.listNSCum = new List<NSCum>();
            this.ChuyenSanPham = new ChuyenSanPham();
            this.isSendMail = false;
            this.error = string.Empty;
            this.strType = string.Empty;
            this.strHost = string.Empty;
            this.intPort = 0;
            this.strFrom = string.Empty;
            this.strDisplayName = string.Empty;
            this.strPassword = string.Empty;
            this.strSubject = string.Empty;
            this.strBody = string.Empty;
            this.strTo = string.Empty;

            this.nangxuatDAO = new NangXuatDAO();
        }

        private void CreateToDataTableNSChuyen()
        {
            try
            {
                List<string> listTitle = new List<string>() {"Chuyền", "Lao Động", "Mã Hàng", "Kế Hoạch", "Luỹ Kế Kiểm đạt",
                "Luỹ Kế BTP", "Định Mức Ngày", "BTP Ngày", "Kiểm đạt Ngày", "Hàng Lỗi", "Tỷ Lệ Đạt", "Tỷ Lệ Hàng Lỗi", "BTP Trên Chuyền",
                "Vốn", "Nhịp Nghiên Cứu (s)", "Nhịp Thực Tế (s)", "Tỉ Lệ Nhịp (%)", "Doanh Thu Ngày", "Doanh Thu Tháng", "TP Thoát Chuyền Ngày", "Luỹ Kế TP Thoát Chuyền"};
                if (listTitle.Count > 0)
                {
                    foreach (var title in listTitle)
                        dtDataNSChuyenExportExcel.Columns.Add(title, typeof(System.String));
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void CreateToDataTableNSCum()
        {
            try
            {
                List<string> listTitle = new List<string>() { "Chuyền", "Mã Hàng", "Kế Hoạch" };
                if (maxCountCum > 0)
                {
                    for (int i = 0; i < maxCountCum; i++)
                    {
                        string stri = (i + 1).ToString();
                        listTitle.Add("Trạm " + stri);
                        listTitle.Add("Lũy Kế Trạm " + stri);
                    }
                }
                if (listTitle.Count > 0)
                {
                    foreach (var title in listTitle)
                        dtDataNSCumExportExcel.Columns.Add(title, typeof(System.String));
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void SendMails(int mailTemplateId)
        {
            try
            {
                GhiFileLog(DateTime.Now + "SendMails  ");
                error = string.Empty;
                var mailTemplateInfo = BLLMailTemplate.GetById(mailTemplateId);  //mailTemplateFileDAO.GetIrnfoById(mailTemplateId);
                if (mailTemplateInfo != null)
                {
                    var mSend = BLLMailSend.GetById(mailTemplateInfo.MailSendId);
                    if (mSend != null)
                    {
                        strType = mSend.MailTypeName;
                        strHost = mSend.Host.Trim();
                        intPort = int.Parse(mSend.Port);
                        strFrom = mSend.Address;
                        strDisplayName = mSend.Note;
                        strPassword = mSend.PassWord;
                        strSubject = mailTemplateInfo.Subject;
                        strBody = mailTemplateInfo.Content;
                        strTo = string.Empty;

                        if (!string.IsNullOrEmpty(mailTemplateInfo.MailReceiveIds))
                        {
                            var mRecives = BLLMailReceive.GetAll(mailTemplateInfo.MailReceiveIds.Split('|').Select(x => Convert.ToInt32(x)).ToList());
                            if (mRecives != null && mRecives.Count > 0)
                            {
                                foreach (var email in mRecives)
                                {
                                    strTo += (strTo.Length == 0 ? email.Address : ", " + email.Address);
                                }
                            }

                            if (!string.IsNullOrEmpty(mailTemplateInfo.MailFileIds))
                            {
                                var mFiles = BLLMailFile.GetAll(mailTemplateInfo.MailFileIds.Split('|').Select(x => Convert.ToInt32(x)).ToList());
                                if (mFiles != null && mFiles.Count > 0)
                                {
                                    if (listFileAttactment == null)
                                        listFileAttactment = new List<string>();
                                    DateTime dtTo = DateTime.Now;
                                    foreach (var file in mFiles)
                                    {
                                        string path = Application.StartupPath + @file.Path;
                                        string tieuDe = file.Name;
                                        string fileName = file.Code.Trim() + dtTo.ToString("dd_MM_yyyy_hh_mm") + ".xlsx";
                                        switch (file.SystemName.Trim().ToUpper())
                                        {
                                            case eFile.NSCHUYEN:
                                                listFileAttactment.AddRange(LoadDataNSChuyenExportToExcel(tieuDe, path, fileName));
                                                break;
                                            case eFile.NSCCUM:
                                                listFileAttactment.AddRange(LoadDataNSCum(tieuDe, path, fileName));
                                                break;
                                            case eFile.NS_CHUYEN_THEO_GIO_MrTri:
                                                listFileAttactment.AddRange(LoadNS_Chuyen_Theo_Gio(tieuDe, path, fileName, (int)eReportType.MrTri));
                                                break;
                                            case eFile.NS_CHUYEN_THEO_GIO_MDG:
                                                listFileAttactment.AddRange(LoadNS_Chuyen_Theo_Gio(tieuDe, path, fileName, (int)eReportType.MDG));
                                                break;
                                            case eFile.NS_CHUYEN_THEO_GIO_THIENSON:
                                                var list = LoadNS_Chuyen_Theo_Gio(tieuDe, path, fileName, (int)eReportType.ThienSon);
                                                listFileAttactment.AddRange(list);
                                                break;
                                            case eFile.NS_CHUYEN_THEO_GIO_SONHA:
                                                var list_ = LoadNS_Chuyen_Theo_Gio(tieuDe, path, fileName, (int)eReportType.SonHa);
                                                listFileAttactment.AddRange(list_);
                                                break;
                                            case eFile.Chart_KCSInHour:
                                                listFileAttactment.AddRange(GetChart(tieuDe, path, fileName, false, true, true, (int)eGetType.KCS));
                                                break;
                                            case eFile.Chart_TCInHour:
                                                listFileAttactment.AddRange(GetChart(tieuDe, path, fileName, false, false, true, (int)eGetType.TC));
                                                break;
                                            case eFile.Chart_ERRORInHour:
                                                listFileAttactment.AddRange(GetChart(tieuDe, path, fileName, true, false, true, (int)eGetType.Error));
                                                break;
                                            case eFile.Chart_KCSInDay:
                                                listFileAttactment.AddRange(GetChart(tieuDe, path, fileName, false, true, false, (int)eGetType.KCS));
                                                break;
                                            case eFile.Chart_TCInDay:
                                                listFileAttactment.AddRange(GetChart(tieuDe, path, fileName, false, false, false, (int)eGetType.TC));
                                                break;
                                            case eFile.Chart_ERRORInDay:
                                                listFileAttactment.AddRange(GetChart(tieuDe, path, fileName, true, true, false, (int)eGetType.Error));
                                                break;
                                        }
                                    }
                                }
                            }
                            isSendMail = true;
                        }
                        else
                            error = "Lỗi: Không có địa chỉ nhận mail.";
                    }
                    else
                        error = "Lỗi: Không tìm thấy thông tin mail gửi.";
                }
                else
                    error = "Lỗi: Không tìm thấy cấu hình mail.";
                if (!string.IsNullOrEmpty(error))
                    MessageBox.Show(error);
            }
            catch (Exception ex)
            {
            }
        }

        public void GhiFileLog(string txt)
        {
            try
            {
                string path = Application.StartupPath + "\\FileLog.txt";
                if (!File.Exists(path))
                    File.Create(path);
                FileStream file = new FileStream(path, FileMode.Append);
                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.WriteLine(txt);
                    sw.Flush();
                    sw.Close();
                }
                file.Close();
                file.Dispose();
            }
            catch (Exception ex)
            {
            }
        }

        private List<string> GetChart(string tieuDe, string path, string fileName, bool IsError, bool IsKCS, bool IsInHour, int getType)
        {
            var listFilePath = new List<string>();
            try
            {
                var lines = BLLProductivity.GetProductiviesOfLinesInDay(AccountSuccess.strListChuyenId.Split(',').Select(x => int.Parse(x)).ToList(), DateTime.Now, IsInHour, getType);
                var result = ReportDB.DrawChartAndExport(lines, tieuDe, path, fileName, IsError, IsKCS);

                if (result)
                    listFilePath.Add(path + fileName);
                else
                    MessageBox.Show("Lỗi: Tạo file excel không thành công.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            return listFilePath;
        }

        private List<string> LoadNS_Chuyen_Theo_Gio(string tieuDe, string path, string fileName, int template)
        {
            var listFilePath = new List<string>();
            try
            {
                bool result = false;
                List<ChuyenSanPhamModel> ns;
                string templatePath = string.Empty;
                template = 4;
                switch (template)
                {
                    case (int)eReportType.MrTri:
                        templatePath = Application.StartupPath + @"\Report\Template\ATri_NSGio_Template.xlsx";
                        if (!File.Exists(templatePath))
                            MessageBox.Show("Không tìm thấy file mail 'ATri_NSGio_Template.xlsx' trong thu mực template.");
                        else
                        {
                            ns = BLLAssignmentForLine.Instance.GetProductivitiesOfLines(DateTime.Now, AccountSuccess.strListChuyenId.Split(',').Select(x => Convert.ToInt32(x)).ToList());
                            result = ReportDB.ExportToExcel_ProductivitiesByHour("ATri_NSGio_Template.xlsx", tieuDe, path, fileName, ns.OrderBy(x => x.MaChuyen).ToList());
                        }
                        break;
                    case (int)eReportType.MDG:
                        var maCD = Convert.ToInt32(ConfigurationManager.AppSettings["MaCDChoReportTS"].ToString());
                        ns = BLLAssignmentForLine.Instance.GetProductivitiesOfLines(DateTime.Now, AccountSuccess.strListChuyenId.Split(',').Select(x => Convert.ToInt32(x)).ToList(), timesGetNSInDay, getBTPInLineByType, maCD);
                        result = ReportDB.ExportToExcel_ProductivitiesByHour_2(tieuDe, path, fileName, ns.OrderBy(x => x.MaChuyen).ToList(), timesGetNSInDay);
                        break;
                    case (int)eReportType.ThienSon:
                        templatePath = Application.StartupPath + @"\Report\Template\TS_Template.xlsx";
                        if (!File.Exists(templatePath))
                            MessageBox.Show("Không tìm thấy file mail 'TS_Template.xlsx' trong thư mục template.");
                        else
                            result = CreateThienSonReport(tieuDe, path, fileName, "TS_Template.xlsx");
                        break;
                    case (int)eReportType.SonHa:
                        templatePath = Application.StartupPath + @"\Report\Template\SH_Template.xlsx";
                        if (!File.Exists(templatePath))
                            MessageBox.Show("Không tìm thấy file mail 'SH_Template.xlsx' trong thư mục template.");
                        else
                            result = Create_Son_Ha_Report(tieuDe, path, fileName, "SH_Template.xlsx");
                        break;
                }
                if (result)
                    listFilePath.Add(path + fileName);
                else
                    MessageBox.Show("Lỗi: Tạo file excel năng suất chuyền hàng giờ không thành công.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            return listFilePath;
        }

        private bool CreateThienSonReport(string tieuDe, string path, string fileName, string templateName)
        {
            var maCD = Convert.ToInt32(ConfigurationManager.AppSettings["MaCDChoReportTS"].ToString());
            var ns = BLLAssignmentForLine.Instance.GetProductivitiesOfLines(DateTime.Now, AccountSuccess.strListChuyenId.Split(',').Select(x => Convert.ToInt32(x)).ToList(), timesGetNSInDay, getBTPInLineByType, maCD);
            return ReportDB.ExportToExcel_ThienSon_Edit(tieuDe, path, templateName, fileName, ns.OrderBy(x => x.MaChuyen).ToList(), timesGetNSInDay);
        }

        private bool Create_Son_Ha_Report(string tieuDe, string path, string fileName, string templateName)
        {
            var maCD = Convert.ToInt32(ConfigurationManager.AppSettings["MaCDChoReportTS"].ToString());
            var ns = BLLAssignmentForLine.Instance.GetProductivitiesOfLines(DateTime.Now, AccountSuccess.strListChuyenId.Split(',').Select(x => Convert.ToInt32(x)).ToList(), null, getBTPInLineByType, maCD);
            return ReportDB.ExportToExcel_ThienSon_Edit(tieuDe, path, templateName, fileName, ns.OrderBy(x => x.MaChuyen).ToList(), timesGetNSInDay);
        }

        public List<NSCum> GetNangSuatCumOfChuyen(string sttChuyenSanPham)
        {
            listNSCum.Clear();
            try
            {
                var now = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                dtLoadData.Columns.Clear();
                dtLoadData.Rows.Clear();
                string strSQL = "select ns_c.IdCum, ns_c.SanLuongKCSTang from NangSuat_Cum ns_c where ns_c.STTChuyen_SanPham = '" + sttChuyenSanPham + "' and Ngay ='" + now + "'";
                sqlDataAdapter = new SqlDataAdapter(strSQL, sqlConnect);
                sqlDataAdapter.Fill(dtLoadData);
                if (dtLoadData != null && dtLoadData.Rows.Count > 0)
                {
                    foreach (DataRow row in dtLoadData.Rows)
                    {
                        listNSCum.Add(new NSCum()
                        {
                            sanLuong = "0",
                            cum = row["SanLuongKCSTang"].ToString()
                        });
                    }
                    dtLoadData.Columns.Clear();
                    dtLoadData.Rows.Clear();
                    string sql = "select ns_c.IdCum, SUM(ns_c.SanLuongKCSTang) LuyKe from NangSuat_Cum ns_c where ns_c.STTChuyen_SanPham = '" + sttChuyenSanPham + "' GROUP BY IdCum";
                    sqlDataAdapter = new SqlDataAdapter(sql, sqlConnect);
                    sqlDataAdapter.Fill(dtLoadData);
                    if (dtLoadData != null && dtLoadData.Rows.Count > 0 && listNSCum != null && listNSCum.Count > 0)
                    {
                        for (int i = 0; i < dtLoadData.Rows.Count; i++)
                        {
                            listNSCum[i].sanLuong = dtLoadData.Rows[i]["LuyKe"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return listNSCum;
        }

        public void GetChuyenSanPhamInfByChuyenId(string IdChuyen)
        {
            ChuyenSanPham.STT = string.Empty;
            ChuyenSanPham.TenChuyen = string.Empty;
            ChuyenSanPham.SanLuongKeHoach = 0;
            ChuyenSanPham.TenSanPham = string.Empty;

            string strSQL = "SELECT TOP 1 csp.STT, c.TenChuyen, csp.SanLuongKeHoach, sp.TenSanPham FROM Chuyen_SanPham csp, Chuyen c, SanPham sp WHERE csp.MaChuyen = '" + IdChuyen + "' and csp.MaChuyen = c.MaChuyen and csp.IsFinish = 0 and csp.IsDelete = 0 and sp.IsDelete=0 and csp.MaSanPham = sp.MaSanPham and sp.IsDelete=0 Order By STTThucHien ASC";
            try
            {
                dtLoadData.Columns.Clear();
                dtLoadData.Rows.Clear();
                sqlDataAdapter = new SqlDataAdapter(strSQL, sqlConnect);
                sqlDataAdapter.Fill(dtLoadData);
                if (dtLoadData != null && dtLoadData.Rows.Count > 0)
                {
                    ChuyenSanPham.STT = dtLoadData.Rows[0]["STT"].ToString();
                    ChuyenSanPham.TenSanPham = dtLoadData.Rows[0]["TenSanPham"].ToString();
                    int sanLuongKeHoach = 0;
                    int.TryParse(dtLoadData.Rows[0]["SanLuongKeHoach"].ToString(), out sanLuongKeHoach);
                    ChuyenSanPham.SanLuongKeHoach = sanLuongKeHoach;
                    ChuyenSanPham.TenChuyen = dtLoadData.Rows[0]["TenChuyen"].ToString();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public List<ModelNangSuatCum> GetTTNangSuatCum()
        {
            try
            {
                listModelNangSuatCum.Clear();
                if (listChuyen != null && listChuyen.Count > 0)
                {
                    foreach (var chuyen in listChuyen)
                    {
                        GetChuyenSanPhamInfByChuyenId(chuyen.MaChuyen);
                        if (!string.IsNullOrEmpty(ChuyenSanPham.STT))
                        {
                            ModelNangSuatCum model = new ModelNangSuatCum();
                            model.chuyen = ChuyenSanPham.TenChuyen;
                            model.maHang = ChuyenSanPham.TenSanPham;
                            model.sanLuongKeHoach = ChuyenSanPham.SanLuongKeHoach.ToString();
                            model.listNangSuatCum = GetNangSuatCumOfChuyen(ChuyenSanPham.STT);
                            listModelNangSuatCum.Add(model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return listModelNangSuatCum;
        }

        private List<string> LoadDataNSCum(string tieuDe, string path, string fileName)
        {
            List<string> listFilePath = new List<string>();
            try
            {
                var listModelNangSuatCum = GetTTNangSuatCum();
                if (listModelNangSuatCum != null && listModelNangSuatCum.Count > 0)
                {
                    dtDataNSCumExportExcel.Rows.Clear();
                    foreach (var item in listModelNangSuatCum)
                    {
                        DataRow row = dtDataNSCumExportExcel.NewRow();
                        row.SetField(0, item.chuyen);
                        row.SetField(1, item.maHang);
                        row.SetField(2, item.sanLuongKeHoach);
                        if (item.listNangSuatCum != null && item.listNangSuatCum.Count > 0)
                        {
                            int i = 2;
                            foreach (var nscum in item.listNangSuatCum)
                            {
                                i++;
                                row.SetField(i, nscum.cum);
                                row.SetField(i + 1, nscum.sanLuong);
                                i++;
                            }
                        }
                        else
                        {
                            if (maxCountCum > 0)
                            {
                                for (int i = 0; i < maxCountCum; i++)
                                {
                                    row.SetField(2, "");
                                    row.SetField(2, "");
                                }
                            }
                        }
                        dtDataNSCumExportExcel.Rows.Add(row);
                    }
                    var result = ReportDB.ExportToExcelByDataTable(tieuDe, path, fileName, dtDataNSCumExportExcel);
                    if (result)
                        listFilePath.Add(path + fileName);
                    else
                        MessageBox.Show("Lỗi: Tạo file excel năng suất cụm không thành công.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            return listFilePath;
        }

        public List<string> FindListSTTChuyen_SanPham(string IdChuyen)
        {
            listSTTChuyen_SanPham.Clear();
            dtLoadData.Columns.Clear();
            dtLoadData.Rows.Clear();
            string strSQL = "SELECT csp.STT, c.TenChuyen FROM Chuyen_SanPham csp, Chuyen c, SanPham sp WHERE csp.MaChuyen = '" + IdChuyen + "' and csp.MaChuyen = c.MaChuyen and csp.IsFinish = 0 and csp.IsDelete = 0 and sp.IsDelete=0 and csp.MaSanPham = sp.MaSanPham and sp.IsDelete=0 Order By csp.STTThucHien ASC";
            try
            {
                sqlDataAdapter = new SqlDataAdapter(strSQL, sqlConnect);
                sqlDataAdapter.Fill(dtLoadData);
                if (dtLoadData != null && dtLoadData.Rows.Count > 0)
                {
                    foreach (DataRow row in dtLoadData.Rows)
                    {
                        string sttChuyen_SanPham = row["STT"].ToString();
                        if (!string.IsNullOrEmpty(sttChuyen_SanPham))
                            listSTTChuyen_SanPham.Add(sttChuyen_SanPham);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return listSTTChuyen_SanPham;
        }

        private List<string> LoadDataNSChuyenExportToExcel(string tieuDe, string path, string fileName)
        {
            List<string> listFilePath = new List<string>();
            try
            {
                if (listChuyen != null && listChuyen.Count > 0)
                {
                    int appId = 1;
                    int.TryParse(ConfigurationManager.AppSettings["AppId"].ToString(), out appId);
                    var nx = BLLProductivity.GetProductivitiesInDay(AccountSuccess.strListChuyenId.Split(',').Select(x => Convert.ToInt32(x)).ToList(), appId);
                    dtDataNSChuyenExportExcel.Rows.Clear();
                    if (nx.Count > 0)
                    {
                        DataRow row;
                        foreach (var item in nx)
                        {
                            row = dtDataNSChuyenExportExcel.NewRow();
                            row.SetField(0, item.LineName);
                            row.SetField(1, item.LaborInLine);
                            row.SetField(2, item.CommoName);
                            row.SetField(3, item.ProductionPlans);
                            row.SetField(4, item.LK_TH);
                            row.SetField(5, item.LK_BTP);
                            row.SetField(6, item.NormsOfDay);
                            row.SetField(7, item.BTP_Day);
                            row.SetField(8, item.TH_Day);
                            row.SetField(9, item.ErrorsInDay);
                            row.SetField(10, item.TH_Percent);
                            row.SetField(11, item.ErrorPercent);
                            row.SetField(12, item.BTPInLine);
                            row.SetField(13, item.Funds);
                            row.SetField(14, item.ResearchPaced);
                            row.SetField(15, item.CurrentPacedProduction);
                            row.SetField(16, item.TC_Paced);
                            row.SetField(17, item.RevenuesInDay);
                            row.SetField(18, item.RevenuesInMonth);
                            row.SetField(19, item.TC_Day);
                            row.SetField(20, item.LK_TC);
                            dtDataNSChuyenExportExcel.Rows.Add(row);
                        }
                        var result = ReportDB.ExportToExcelByDataTable(tieuDe, path, fileName, dtDataNSChuyenExportExcel);
                        if (result)
                            listFilePath.Add(path + fileName);
                        else
                            MessageBox.Show("Lỗi: Tạo file excel năng suất chuyền không thành công.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            return listFilePath;
        }

        TimeSpan timeSendMail = TimeSpan.Parse("00:00:00");

        private void CheckTimeSendMail()
        {
            try
            {
                if (listMailSchedule != null && listMailSchedule.Count > 0)
                {
                    TimeSpan dateTimeNow = DateTime.Now.TimeOfDay;
                    TimeSpan timeNow = TimeSpan.Parse(dateTimeNow.Hours.ToString() + ":" + dateTimeNow.Minutes.ToString() + ":00");
                    foreach (var item in listMailSchedule)
                    {
                        if (TimeSpan.Parse(item.Time) == timeNow)
                        {
                            if (timeSendMail != timeNow)
                            {
                                timeSendMail = timeNow;
                                SendMails(item.MailTemplateId ?? 0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void timerSendMailAndReadSound_Tick(object sender, EventArgs e)
        {
            try
            {
                //  Thread ts = new Thread(CheckTimeSendMail);
                // ts.Start();
                CheckTimeSendMail();
            }
            catch (Exception ex)
            {
                timerSendMailAndReadSound.Enabled = false;
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void FrmSendMailAndReadSound_Load(object sender, EventArgs e)
        {
            try
            {
                string strConnectionString = dbclass.GetConnectionString();
                this.sqlConnect = new SqlConnection(strConnectionString);
                sqlConnect.Open();
                listChuyen = chuyenDAO.GetListChuyenInfByListId(AccountSuccess.strListChuyenId);
                GetMailSchedule(false);

                //string path = Application.StartupPath + @"\Report\Template\ThienSon_" + today.ToString("dd_MM_yyyy") + ".xlsx";
                //if (!File.Exists(path))
                //{
                //    // tao template
                //    Thread thrTempalte = new Thread(CreateReportTemplate);
                //    thrTempalte.Start();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
                this.Close();
            }
        }

        public void GetMailSchedule(bool isReconnect)
        {
            try
            {
                listMailSchedule = BLLMailSchedule.GetAll();

                if (listMailSchedule == null)
                    MessageBox.Show("Không có cấu hình gửi mail.");
                else
                {
                    if (!isReconnect)
                    {
                        maxCountCum = cumDAO.GetMaxCountOfChuyen();
                        CreateToDataTableNSChuyen();
                        CreateToDataTableNSCum();
                        timerSendMailAndReadSound.Enabled = true;
                        CheckForIllegalCrossThreadCalls = false;
                        threadSendMail = new Thread(SendMail);
                        threadSendMail.Start();
                    }
                }
            }
            catch (Exception)
            {
            }

        }

        private void SendMail()
        {
            try
            {
                while (true)
                {
                    try
                    {
                        if (isSendMail)
                        {
                            //  GhiFileLog(DateTime.Now + "Method SendMail  :  " + listFileAttactment.Count);
                            var hasFile = false;
                            clsMail mail = new clsMail();
                            mail.Type = strType;
                            mail.Host = strHost;
                            mail.Port = intPort;
                            mail.From = strFrom;
                            mail.DisplayName = strDisplayName;
                            mail.Password = strPassword;
                            mail.To = strTo;
                            mail.Subject = strSubject;
                            mail.Body = strBody;
                            if (listFileAttactment != null && listFileAttactment.Count > 0)
                            {
                                hasFile = true;
                                foreach (var filePath in listFileAttactment)
                                    mail.AddAttachment(filePath);
                            }
                            mail.SendMail();
                            //Clear
                            this.strType = string.Empty;
                            this.strHost = string.Empty;
                            this.intPort = 0;
                            this.strFrom = string.Empty;
                            this.strDisplayName = string.Empty;
                            this.strPassword = string.Empty;
                            this.strSubject = string.Empty;
                            this.strBody = string.Empty;
                            this.strTo = string.Empty;
                            isSendMail = false;

                            if (hasFile)
                            {
                                var path = string.Empty;
                                foreach (var filePath in listFileAttactment)
                                {
                                    if (!path.Equals(filePath.Substring(0, filePath.LastIndexOf('\\'))))
                                    {
                                        path = filePath.Substring(0, filePath.LastIndexOf('\\'));
                                        DeleteAllFileInPath(path);
                                    }
                                }
                            }
                            listFileAttactment = new List<string>();
                        }
                        //  Thread.Sleep(10000);
                    }
                    catch (Exception ex)
                    {
                        threadSendMail.Abort();
                        MessageBox.Show("Lỗi gửi mail: " + ex.Message);
                    }
                }
            }
            catch (Exception)
            {
            }

        }

        private static void DeleteAllFileInPath(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    var dr = new DirectoryInfo(path);
                    foreach (var f in dr.GetFiles())
                    {
                        // xoa het file trong folder truoc khi tao file moi   
                        f.Delete();
                    }
                }
                else
                    Directory.CreateDirectory(path);
            }
            catch (Exception)
            {
            }
        }

    }

}
