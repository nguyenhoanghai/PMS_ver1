using DuAn03_HaiDang.DAO;
using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.Enum;
using DuAn03_HaiDang.KeyPad_Chuyen.dao;
using DuAn03_HaiDang.KeyPad_Chuyen.pojo;
using DuAn03_HaiDang.POJO;
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
    public partial class FrmReportCountErrorHours : Form
    {        
        private ChuyenDAO chuyenDAO;
        private ClusterDAO clusterDAO;
      //  private ShiftDAO shiftDAO;
      //  private ErrorDAO errorDAO;
        private List<ErrorModel> listError;

        public FrmReportCountErrorHours()
        {
            InitializeComponent();
            this.chuyenDAO = new ChuyenDAO();
            this.clusterDAO = new ClusterDAO();
       //     this.shiftDAO = new ShiftDAO();
       //     this.errorDAO = new ErrorDAO();
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

        private void butPrint_Click(object sender, EventArgs e)
        {
            try
            {
                Helper.DrawChart.ExportExcel(this.chartControl1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void FrmReportCountErrorHours_Load(object sender, EventArgs e)
        {
            try
            {
                listError = BLLError.GetAll(); // errorDAO.GetListError();
                GetDataForCbbLine();
                GetDataChart();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void GetDataChart()
        {
            try
            {
                var line = (Chuyen)cbbLine.SelectedItem;
                if (line != null)
                {
                    var modelWorkHours = (ModelWorkHours)cbbHours.SelectedItem;
                    if (modelWorkHours != null)
                    {
                        GetDataForChart(line.MaChuyen, line.TenChuyen, modelWorkHours.TimeStart, modelWorkHours.TimeEnd, dtpDate.Value);
                    }
                    else
                        MessageBox.Show("Lỗi: Không có thông tin giờ.", "Thông Báo Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);                   
                    
                }
                else
                    MessageBox.Show("Lỗi: Bạn chưa chọn thông tin chuyền.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetDataForChart(string lineId, string lineName, TimeSpan timeStart, TimeSpan timeEnd, DateTime date)
        {
            try
            {
                List<ModelSeries> listModelSeries = new List<ModelSeries>();
                ModelSeries modelSeries = new ModelSeries() { SeriesName = "Lỗi", ShowInLegend = false };
                List<Model.Point> listPoint = new List<Model.Point>();
                if (listError != null && listError.Count > 0)
                {
                    foreach (var item in listError)
                    {
                        string sqlSanLuongGio = "select (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + lineId + " and ErrorId=" + item.Id + " and Time >= '" + timeStart + "' and Time <='" + timeEnd + "' and Date='" + date + "' and CommandTypeId=" + (int)eCommandRecive.ErrorIncrease + " and IsEndOfLine=1) AS SanLuongTang, (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + lineId + " and ErrorId=" + item.Id + " and Time >= '" + timeStart + "' and Time <='" + timeEnd + "' and Date='" + date + "' and CommandTypeId=" + (int)eCommandRecive.ErrorReduce + " and IsEndOfLine=1) AS SanLuongGiam";
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
                        listPoint.Add(new Model.Point() { X = item.Name, Y = sanLuongGio });
                    }
                    modelSeries.ListPoint = listPoint;
                    listModelSeries.Add(modelSeries);
                    string strDate = " Ngày " + date.Day + "/" + date.Month + "/" + date.Year;
                    Helper.DrawChart.DrawBarChart(this.chartControl1, "Số Lượng Lỗi Của Chuyền " + lineName + strDate, "Số Lỗi", "Loại Lỗi", listModelSeries);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void GetDataForCbbLine()
        {
            try
            {
                var listLine = chuyenDAO.GetListChuyenInfByListId(AccountSuccess.strListChuyenId);
                if (listLine != null && listLine.Count > 0)
                {
                    cbbLine.DataSource = listLine;
                    cbbLine.DisplayMember = "TenChuyen";
                }
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
                var line = (Chuyen)cbbLine.SelectedItem;
                if (line != null)
                {
                    var listModelWorkHours = BLLShift.GetListWorkHoursOfLineByLineId(int.Parse(line.MaChuyen));// shiftDAO.GetListWorkHoursOfLineByLineId(line.MaChuyen);
                    if (listModelWorkHours != null && listModelWorkHours.Count > 0)
                    {
                        cbbHours.DataSource = listModelWorkHours;
                        cbbHours.DisplayMember = "Name";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void cbbLine_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                GetDataForCbbHours();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Lỗi: " + ex.Message, "Thông Báo Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
