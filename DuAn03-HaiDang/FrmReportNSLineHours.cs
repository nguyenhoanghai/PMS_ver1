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
    public partial class FrmReportNSLineHours : Form
    {
        public FrmReportNSLineHours()
        {
            InitializeComponent();
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
         
        private void FrmReportNSLineHours_Load(object sender, EventArgs e)
        {
            try
            {
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
                var line = (LineModel)cbbLine.SelectedItem;
                if (line != null)
                {
                    GetDataForChart(line.MaChuyen, line.TenChuyen, dtpDate.Value);
                }
                else
                    MessageBox.Show("Lỗi: Bạn chưa chọn thông tin chuyền.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetDataForChart(int lineId, string lineName, DateTime date)
        {
            try
            {
                List<ModelSeries> listModelSeries = new List<ModelSeries>();
                ModelSeries modelSeries = new ModelSeries() { SeriesName = "Chuyen", ShowInLegend = false };
                List<Model.Point> listPoint = new List<Model.Point>();
                var ngay = date.Day + "/" + date.Month + "/" + date.Year;
                var times = BLLReport.GetNSCForChart(lineId, ngay);
                if (times != null && times.Count > 0)
                {
                    foreach (var item in times)
                    {
                        listPoint.Add(new Model.Point() { X = item.IntHours, Y = item.KCS });
                    }
                    modelSeries.ListPoint = listPoint;
                    listModelSeries.Add(modelSeries);
                    Helper.DrawChart.DrawBarChart(this.chartControl1, "Năng Suất Giờ Của Chuyền " + lineName + " Ngày " + ngay, "Năng Suất", "Giờ", listModelSeries);
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

        private void cbbLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetDataChart();
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            GetDataChart();
        }
    }
}
