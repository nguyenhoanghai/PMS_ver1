using DuAn03_HaiDang.DAO;
using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.Enum;
using DuAn03_HaiDang.KeyPad_Chuyen.dao;
using DuAn03_HaiDang.KeyPad_Chuyen.pojo;
using DuAn03_HaiDang.POJO;
using PMS.Business.Enum;
using QuanLyNangSuat.Helper;
using QuanLyNangSuat.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PMS.Business;
using PMS.Data;
using PMS.Business.Models;

namespace QuanLyNangSuat
{
    public partial class FrmReportNSCum : Form
    {

        public FrmReportNSCum()
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

        private void FrmReportNSCum_Load(object sender, EventArgs e)
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
                    var cluster = (Cum)cbbCluster.SelectedItem;
                    if (cluster != null)
                        GetDataForChart(line.MaChuyen, cluster.TenCum, cluster.Id, dtpDate.Value);
                }
                else
                    MessageBox.Show("Lỗi: Bạn chưa chọn thông tin chuyền.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetDataForChart(int lineId, string clusterName, int clusterId, DateTime date)
        {
            try
            {
                List<ModelSeries> listModelSeries = new List<ModelSeries>();
                ModelSeries modelSeries = new ModelSeries() { SeriesName = "Cum", ShowInLegend = false };
                List<Model.Point> listPoint = new List<Model.Point>();
                var ngay = date.Day + "/" + date.Month + "/" + date.Year;
                var times = BLLReport.GetNSCForChart(lineId, clusterId, ngay);
                if (times != null && times.Count > 0)
                {
                    foreach (var item in times)
                    {
                        listPoint.Add(new Model.Point() { X = item.IntHours, Y = item.KCS });
                    }

                    modelSeries.ListPoint = listPoint;
                    listModelSeries.Add(modelSeries);
                    string strDate = " Ngày " + ngay;
                    Helper.DrawChart.DrawBarChart(this.chartControl1, "Năng Suất Giờ Của Cụm " + clusterName + strDate, "Năng Suất", "Giờ", listModelSeries);
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
                var listLine = BLLLine.GetLines(AccountSuccess.strListChuyenId.Split(',').Select(x => Convert.ToInt32(x)).ToList()); // chuyenDAO.GetListChuyenInfByListId(AccountSuccess.strListChuyenId);
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

        private void GetDataForCbbCluster()
        {
            try
            {
                cbbCluster.DataSource = null;
                var line = (LineModel)cbbLine.SelectedItem;
                if (line != null)
                {
                    var listCluster = BLLCluster.GetAllByLineId(line.MaChuyen);// clusterDAO.GetCumOfChuyen(int.Parse(line.MaChuyen)); 
                    if (listCluster != null && listCluster.Count > 0)
                    {
                        cbbCluster.DataSource = listCluster;
                        cbbCluster.DisplayMember = "TenCum";
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
                GetDataForCbbCluster();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            GetDataChart();
        }

        private void cbbCluster_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetDataChart();
        }
    }
}
