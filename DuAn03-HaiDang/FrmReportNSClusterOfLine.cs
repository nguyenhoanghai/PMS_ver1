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
    public partial class FrmReportNSClusterOfLine : Form
    {
        public FrmReportNSClusterOfLine()
        {
            InitializeComponent();
        }

        private void GetDataChart()
        {
            try
            {
                var line = (LineModel)cbbLine.SelectedItem;
                if (line != null)
                {
                    var modelWorkHours = (WorkingTimeModel)cbbHours.SelectedItem;
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

        private void GetDataForChart(int lineId, string lineName, TimeSpan timeStart, TimeSpan timeEnd, DateTime date)
        {
            try
            {
                List<ModelSeries> listModelSeries = new List<ModelSeries>();
                ModelSeries modelSeries = new ModelSeries() { SeriesName = "Cụm", ShowInLegend = false };
                List<Model.Point> listPoint = new List<Model.Point>();
                var ngay = date.Day + "/" + date.Month + "/" + date.Year;
                var clusters = BLLReport.GetNSCForChart(lineId, timeStart, timeEnd, ngay);
                if (clusters != null && clusters.Count > 0)
                {
                    foreach (var item in clusters)
                    {
                        listPoint.Add(new Model.Point() { X = item.TenCum, Y = item.KCS });
                    }
                }
                modelSeries.ListPoint = listPoint;
                listModelSeries.Add(modelSeries);
                Helper.DrawChart.DrawBarChart(this.chartControl1, "Năng Suất Các Cụm Của " + lineName + " Ngày " + ngay, "Năng Suất", "Cụm", listModelSeries);
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
                var listLine = BLLLine.GetLines(AccountSuccess.strListChuyenId.Split(',').Select(x => Convert.ToInt32(x)).ToList());// chuyenDAO.GetListChuyenInfByListId(AccountSuccess.strListChuyenId);
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
                var line = (LineModel)cbbLine.SelectedItem;
                if (line != null)
                {
                    var listModelWorkHours = BLLShift.GetWorkingTimeOfLine(line.MaChuyen);// shiftDAO.GetListWorkHoursOfLineByLineId(line.MaChuyen);
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

        private void FrmReportNSClusterOfLine_Load(object sender, EventArgs e)
        {
            try
            {
                GetDataForCbbLine();
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

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            GetDataChart();
        }

        private void cbbHours_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetDataChart();
        }
    }
}
