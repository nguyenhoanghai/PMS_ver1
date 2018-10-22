using QuanLyNangSuat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QuanLyNangSuat.Helper
{
    public class DrawChart
    {
        public static void DrawBarChart(DevExpress.XtraCharts.ChartControl chartControl1, string titleChart, string columnName, string rowName, List<ModelSeries> listModelSeries)
        {
            try
            {
                chartControl1.DataSource = null;
                chartControl1.Series.Clear();
                chartControl1.Titles.Clear();
                DevExpress.XtraCharts.TextAnnotation textAnnotation1 = new DevExpress.XtraCharts.TextAnnotation();
                DevExpress.XtraCharts.ChartAnchorPoint chartAnchorPoint1 = new DevExpress.XtraCharts.ChartAnchorPoint();
                DevExpress.XtraCharts.FreePosition freePosition1 = new DevExpress.XtraCharts.FreePosition();
                DevExpress.XtraCharts.TextAnnotation textAnnotation2 = new DevExpress.XtraCharts.TextAnnotation();
                DevExpress.XtraCharts.ChartAnchorPoint chartAnchorPoint2 = new DevExpress.XtraCharts.ChartAnchorPoint();
                DevExpress.XtraCharts.FreePosition freePosition2 = new DevExpress.XtraCharts.FreePosition();
                DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();

                DevExpress.XtraCharts.ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();

                ((System.ComponentModel.ISupportInitialize)(chartControl1)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(textAnnotation1)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(textAnnotation2)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();

                chartAnchorPoint1.X = 0;
                chartAnchorPoint1.Y = 20;
                textAnnotation1.AnchorPoint = chartAnchorPoint1;
                textAnnotation1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
                textAnnotation1.ConnectorStyle = DevExpress.XtraCharts.AnnotationConnectorStyle.None;
                textAnnotation1.Name = columnName;
                textAnnotation1.ShapeKind = DevExpress.XtraCharts.ShapeKind.Rectangle;
                freePosition1.InnerIndents.Left = 30;
                freePosition1.InnerIndents.Top = 51;
                textAnnotation1.ShapePosition = freePosition1;
                textAnnotation1.Text = columnName;
                textAnnotation1.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                chartAnchorPoint2.X = 340;
                chartAnchorPoint2.Y = 490;
                textAnnotation2.AnchorPoint = chartAnchorPoint2;
                textAnnotation2.ConnectorStyle = DevExpress.XtraCharts.AnnotationConnectorStyle.None;
                textAnnotation2.Name = rowName;
                textAnnotation2.ShapeKind = DevExpress.XtraCharts.ShapeKind.Rectangle;
                freePosition2.DockCorner = DevExpress.XtraCharts.DockCorner.RightBottom;
                freePosition2.DockTargetName = "Default Pane";
                freePosition2.InnerIndents.Left = 11;
                freePosition2.InnerIndents.Top = 11;
                textAnnotation2.ShapePosition = freePosition2;
                textAnnotation2.Text = rowName;
                textAnnotation2.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                chartControl1.AnnotationRepository.AddRange(new DevExpress.XtraCharts.Annotation[] {
                    textAnnotation1,
                    textAnnotation2});
                xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
                xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
                chartControl1.Diagram = xyDiagram1;
                chartControl1.Dock = System.Windows.Forms.DockStyle.Fill;
                chartControl1.Location = new System.Drawing.Point(3, 53);
                chartControl1.Size = new System.Drawing.Size(889, 371);
                chartControl1.TabIndex = 1;
                chartTitle1.Text = titleChart;
                chartControl1.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1});
                if (listModelSeries != null && listModelSeries.Count > 0)
                {
                    List<DevExpress.XtraCharts.Series> listSeries = new List<DevExpress.XtraCharts.Series>();
                    foreach (var model in listModelSeries)
                    {
                        DevExpress.XtraCharts.Series series = new DevExpress.XtraCharts.Series() { Name = model.SeriesName, ShowInLegend = model.ShowInLegend };
                        foreach (var point in model.ListPoint)
                        {
                            DevExpress.XtraCharts.SeriesPoint seriesPoint = new DevExpress.XtraCharts.SeriesPoint(point.X, new object[] { ((object)(point.Y)) });
                            series.Points.Add(seriesPoint);
                        }
                        listSeries.Add(series);
                    }
                    chartControl1.SeriesSerializable = listSeries.ToArray();
                }
                ((System.ComponentModel.ISupportInitialize)(textAnnotation1)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(textAnnotation2)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(chartControl1)).EndInit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ExportExcel(DevExpress.XtraCharts.ChartControl chartControl1)
        {
            if (chartControl1.IsPrintingAvailable)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = @"C:\";
                saveFileDialog1.Title = "Save excel file";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    chartControl1.ExportToXls(@saveFileDialog1.FileName + ".xls");
                    MessageBox.Show("Xuất biểu đồ ra file excel thành công.", "Xuất excel thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
        }
    }
}
