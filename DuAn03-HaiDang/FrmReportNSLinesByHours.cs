using DuAn03_HaiDang.DAO;
using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.Enum;
using DuAn03_HaiDang.KeyPad_Chuyen.dao;
using DuAn03_HaiDang.KeyPad_Chuyen.pojo;
using PMS.Business;
using PMS.Business.Enum;
using PMS.Business.Models;
using QuanLyNangSuat.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QuanLyNangSuat
{
    public partial class FrmReportNSLinesByHours : Form
    {
        private ChuyenDAO chuyenDAO;
        private ClusterDAO clusterDAO;
      //  private ShiftDAO shiftDAO;
        private List<Chuyen> listLine;
     //   private List<ModelWorkHours> listModelWorkHours;
        private List<WorkingTimeModel> listModelWorkHours;
        private bool isNSAllDay;

        public FrmReportNSLinesByHours()
        {
            InitializeComponent();
            this.chuyenDAO = new ChuyenDAO();
            this.clusterDAO = new ClusterDAO();
       //     this.shiftDAO = new ShiftDAO();
            this.isNSAllDay = false;
        }

        private bool GetDataChart()
        {
            try
            {
                var modelWorkHours = (ModelWorkHours)cbbHours.SelectedItem;
                if (modelWorkHours != null)
                {                    
                    if (isNSAllDay)
                    {                        
                        if (dtpToDate.Value.Date < dtpDate.Value.Date)
                        {
                             MessageBox.Show("Lỗi: Ngày đến không được nhỏ hơn ngày chọn.", "Lỗi thao tác",MessageBoxButtons.OK,  MessageBoxIcon.Error);
                             return false;
                        }
                    }
                    GetDataForChart(modelWorkHours.TimeStart, modelWorkHours.TimeEnd, dtpDate.Value, dtpToDate.Value);
                    return true;
                }
                else
                    {
                    MessageBox.Show("Lỗi: Không có thông tin giờ.", "Thông Báo Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetDataForChart(TimeSpan timeStart, TimeSpan timeEnd, DateTime date, DateTime toDate)
        {
            try
            {
                List<ModelSeries> listModelSeries = new List<ModelSeries>();
                ModelSeries modelSeries = new ModelSeries() { SeriesName = "Sản lượng", ShowInLegend = true };
                List<Model.Point> listPoint = new List<Model.Point>();
                ModelSeries modelSeriesDM = new ModelSeries() { SeriesName = "Định mức", ShowInLegend = true };
                List<Model.Point> listPointDM = new List<Model.Point>();               
                if (listLine != null && listLine.Count > 0)
                {
                    foreach (var line in listLine)
                    {
                        string sqlSanLuongGio = string.Empty;
                        if(!isNSAllDay)
                            sqlSanLuongGio = "select (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + line.MaChuyen + " and Time >= '" + timeStart + "' and Time <='" + timeEnd + "' and Date='" + date + "' and CommandTypeId=" + (int)eCommandRecive.ProductIncrease + " and ProductOutputTypeId=" + (int)eProductOutputType.KCS + " and IsEndOfLine=1) AS SanLuongTang, (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + line.MaChuyen + " and Time >= '" + timeStart + "' and Time <='" + timeEnd + "' and Date='" + date + "' and CommandTypeId=" + (int)eCommandRecive.ProductReduce + " and ProductOutputTypeId=" + (int)eProductOutputType.KCS + " and IsEndOfLine=1) AS SanLuongGiam";
                        else
                        {
                            sqlSanLuongGio = "select (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + line.MaChuyen + " and Date >='" + date + "' and Date <='" + toDate + "' and CommandTypeId=" + (int)eCommandRecive.ProductIncrease + " and ProductOutputTypeId=" + (int)eProductOutputType.KCS + " and IsEndOfLine=1) AS SanLuongTang, (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + line.MaChuyen + " and Date >='" + date + "' and Date <='" + toDate + "' and CommandTypeId=" + (int)eCommandRecive.ProductReduce + " and ProductOutputTypeId=" + (int)eProductOutputType.KCS + " and IsEndOfLine=1) AS SanLuongGiam";
                        }
                        int sanLuongGioTang = 0;
                        int sanLuongGioGiam = 0;
                        int sanLuongGio = 0;
                        DataTable dtSanLuongGio = dbclass.TruyVan_TraVe_DataTable(sqlSanLuongGio);
                        if (dtSanLuongGio != null && dtSanLuongGio.Rows.Count > 0)
                        {
                            DataRow rowSanLuongGio = dtSanLuongGio.Rows[0];
                            if (rowSanLuongGio["SanLuongTang"] != null)
                                int.TryParse(rowSanLuongGio["SanLuongTang"].ToString(), out sanLuongGioTang);
                            if (rowSanLuongGio["SanLuongGiam"] != null)
                                int.TryParse(rowSanLuongGio["SanLuongGiam"].ToString(), out sanLuongGioGiam);
                            sanLuongGio = sanLuongGioTang - sanLuongGioGiam;
                        }
                        if (sanLuongGio < 0)
                            sanLuongGio = 0;
                        listPoint.Add(new Model.Point() { X = line.TenChuyen, Y = sanLuongGio });

                        string sqlDinhMuc = string.Empty;
                        double dinhMuc = 0;
                        
                        if (!isNSAllDay)
                        {
                            sqlDinhMuc = "select SUM(nx.DinhMucNgay) as DinhMucNgay from NangXuat nx, Chuyen_SanPham csp where nx.Ngay='" + date + "' and nx.STTChuyen_SanPham=csp.STT and csp.MaChuyen=" + line.MaChuyen;
                        }
                        else
                        {
                            sqlDinhMuc = "select SUM(nx.DinhMucNgay) as DinhMucNgay from NangXuat nx, Chuyen_SanPham csp where nx.Ngay>='" + date + "' and nx.Ngay<='" + toDate + "' and nx.STTChuyen_SanPham=csp.STT and csp.MaChuyen=" + line.MaChuyen;                            
                        }
                        DataTable dtDinhMuc = dbclass.TruyVan_TraVe_DataTable(sqlDinhMuc);
                        if (dtDinhMuc != null && dtDinhMuc.Rows.Count > 0)
                        {
                            DataRow rowDinhMuc = dtDinhMuc.Rows[0];
                            if (rowDinhMuc["DinhMucNgay"] != null)
                                double.TryParse(rowDinhMuc["DinhMucNgay"].ToString(), out dinhMuc);

                        }
                        if(!isNSAllDay)
                        {
                            var workTimeInDate = BLLShift.GetTotalWorkingHourOfLine(int.Parse(line.MaChuyen));// shiftDAO.TimeIsWorkAllDayOfLine(line.MaChuyen);
                            int intHour = workTimeInDate.Hours;
                            if (workTimeInDate.Minutes > 0)
                                intHour++;
                            dinhMuc = dinhMuc / intHour;
                        }
                        listPointDM.Add(new Model.Point() { X = line.TenChuyen, Y = (int)dinhMuc });
                    }                    
                }
                modelSeriesDM.ListPoint = listPointDM;
                modelSeries.ListPoint = listPoint;
                listModelSeries.Add(modelSeries);
                listModelSeries.Add(modelSeriesDM);
                string strDate = string.Empty;
                if (!isNSAllDay)
                    strDate = " Ngày " + date.Day + "/" + date.Month + "/" + date.Year;
                else
                    strDate = " Từ Ngày " + date.Day + "/" + date.Month + "/" + date.Year + " Đến Ngày " + toDate.Day + "/" + toDate.Month + "/" + toDate.Year;
                Helper.DrawChart.DrawBarChart(this.chartControl1, "Năng Suất Các Chuyền " + strDate, "Năng Suất", "Chuyền", listModelSeries);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetDataForCbbHours()
        {
            try
            {
                if (listLine != null && listLine.Count > 0)
                { 
                    foreach(var line in listLine)
                    {
                        var listModel = BLLShift.GetListWorkHoursOfLineByLineId(int.Parse(line.MaChuyen)); // shiftDAO.GetListWorkHoursOfLineByLineId(line.MaChuyen);
                        if (listModel != null && listModel.Count > 0)
                        {
                            if (listModel.Count > listModelWorkHours.Count)
                                listModelWorkHours = listModel;
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmReportNSClusterOfLine_Load(object sender, EventArgs e)
        {
            try
            {
                cbbSearchType.SelectedIndex = 0;
               // listModelWorkHours = new List<ModelWorkHours>();
                listModelWorkHours = new List<WorkingTimeModel>();
                listLine = chuyenDAO.GetListChuyenInfByListId(AccountSuccess.strListChuyenId);
                if (listLine != null && listLine.Count > 0)
                {
                    GetDataForCbbHours();
                    if (listModelWorkHours!=null && listModelWorkHours.Count>0)
                    {
                        cbbHours.DataSource = listModelWorkHours;
                        cbbHours.DisplayMember = "Name";
                        GetDataChart();
                    }                    
                }
                else
                    MessageBox.Show("Lỗi: Không có thông tin Chuyền", "Thông Báo Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Lỗi: "+ex.Message, "Thông Báo Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void butView_Click(object sender, EventArgs e)
        {
            try
            {
                GetDataChart();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Lỗi: " + ex.Message, "Thông Báo Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void butExportExcel_Click(object sender, EventArgs e)
        {            
            try
            {
                Helper.DrawChart.ExportExcel(this.chartControl1);
            }
            catch (Exception ex)
            {

                MessageBox.Show("Lỗi: " + ex.Message, "Thông Báo Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbbSearchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbbSearchType.SelectedIndex == 0)
                {
                    cbbHours.Visible = true;
                    dtpToDate.Visible = false;
                    isNSAllDay = false;
                }
                else
                {
                    cbbHours.Visible = false;
                    dtpToDate.Visible = true;
                    isNSAllDay = true;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("Lỗi: " + ex.Message, "Thông Báo Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       
    }
}
