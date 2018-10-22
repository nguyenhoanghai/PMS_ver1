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
    public partial class FrmReportErrorOfLineByHours : Form
    { 
        private List<ErrorModel> errors;
        private List<LineModel> lines;
        private List<WorkingTimeModel> times;
        public FrmReportErrorOfLineByHours()
        {
            InitializeComponent(); 
            this.errors = new List<ErrorModel>();
            this.lines = new List<LineModel>();
            this.times = new List<WorkingTimeModel>();
        }

        private void GetDataChart()
        {
            try
            {
                var modelWorkHours = (WorkingTimeModel)cbbHours.SelectedItem;
                if (modelWorkHours != null)
                {
                    GetDataForChart(modelWorkHours.TimeStart, modelWorkHours.TimeEnd, dtpDate.Value);
                }
                else
                    MessageBox.Show("Lỗi: Không có thông tin giờ.", "Thông Báo Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetDataForChart(TimeSpan timeStart, TimeSpan timeEnd, DateTime date)
        {
            try
            {
                List<ModelSeries> listModelSeries = new List<ModelSeries>();
                ModelSeries modelSeries = new ModelSeries() { SeriesName = "Chuyen", ShowInLegend = false };
                List<Model.Point> listPoint = new List<Model.Point>();
                var ngay = date.Day + "/" + date.Month + "/" + date.Year;
                var linesM = BLLReport.GetNSCErrorForChart(lines, timeStart, timeEnd, ngay);
                if (linesM != null && linesM.Count > 0)
                {
                    foreach (var item in linesM)
                    {
                        listPoint.Add(new Model.Point() { X = item.TenChuyen, Y = item.TC });
                    }
                }
                modelSeries.ListPoint = listPoint;
                listModelSeries.Add(modelSeries);
                Helper.DrawChart.DrawBarChart(this.chartControl1, "Số Lỗi Các Chuyền " + " Ngày " + ngay, "Số Lỗi", "Chuyền", listModelSeries);
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
                if (lines.Count > 0)
                {
                    foreach (var item in lines)
                    {
                        var wks = BLLShift.GetWorkingTimeOfLine(item.MaChuyen); // shiftDAO.GetListWorkHoursOfLineByLineId(line.MaChuyen);
                        if (wks != null && wks.Count > 0)
                        {
                            if (wks.Count > times.Count)
                            {
                                times.Clear();
                                times.AddRange(wks);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmReportErrorOfLineByHours_Load(object sender, EventArgs e)
        {
            try
            {
                errors.AddRange(BLLError.GetAll());
                lines.AddRange(BLLLine.GetLines(AccountSuccess.strListChuyenId.Split(',').Select(x => Convert.ToInt32(x)).ToList()));
                if (lines != null && lines.Count > 0)
                {
                    GetDataForCbbHours();
                    if (times != null && times.Count > 0)
                    {
                        cbbHours.DataSource = times;
                        cbbHours.DisplayMember = "Name";
                        GetDataChart();
                    }
                }
                else
                    MessageBox.Show("Lỗi: Không có thông tin Chuyền", "Thông Báo Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void cbbHours_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetDataChart();
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            GetDataChart();
        }
    }
}
