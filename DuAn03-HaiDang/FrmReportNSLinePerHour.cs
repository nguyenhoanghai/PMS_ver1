using DuAn03_HaiDang;
using DuAn03_HaiDang.DAO;
using DuAn03_HaiDang.DATAACCESS;
using PMS.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QuanLyNangSuat
{
    public partial class FrmReportNSLinePerHour : Form
    {
        private NangXuatDAO nangxuatDAO;
        public FrmReportNSLinePerHour()
        {
            InitializeComponent();
            nangxuatDAO = new NangXuatDAO();
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            string path = Application.StartupPath + "\\Report\\NSChuyen";
            Process.Start("explorer.exe", path);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string path = Application.StartupPath + "\\Report\\NSChuyen\\";
                var date = dtpDate.Value;
                var dateStr = date.ToString("dd_MM_yyyy");
                var fileName = "BaoCaoNSChuyen" + dateStr + ".xlsx";
               var templatePath = Application.StartupPath + @"\Report\Template\ATri_NSGio_Template.xlsx"; 
                if (!File.Exists(templatePath))
                    MessageBox.Show("Không tìm thấy file mail 'ATri_NSGio_Template.xlsx' trong thu mực template.");
                else
                {

                    var ns = BLLAssignmentForLine.Instance.GetProductivitiesOfLines(date, AccountSuccess.strListChuyenId.Split(',').Select(x => Convert.ToInt32(x)).ToList()); // nangxuatDAO.GetProductivitiesOfLines(date);
                    var result = ReportDB.ExportToExcel_ProductivitiesByHour("ATri_NSGio_Template.xlsx", "Thông tin năng suất tổng hợp Ngày " + dateStr, path, fileName, ns.OrderBy(x => x.MaChuyen).ToList());
                    if (result)
                        Process.Start("explorer.exe", (path + fileName));
                    else
                        MessageBox.Show("Lỗi: Tạo file excel năng suất chuyền hàng giờ không thành công.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void GetData()
        {
            try
            {              
                dgTTNangXuat.Rows.Clear();
                dgTTNangXuat.Refresh();
                var ns = BLLAssignmentForLine.Instance.GetProductivitiesOfLines(dtpDate.Value, AccountSuccess.strListChuyenId.Split(',').Select(x => Convert.ToInt32(x)).ToList()).OrderBy(x => x.MaChuyen).ToList();  //nangxuatDAO.GetProductivitiesOfLines(dtpDate.Value);
                if (ns.Count > 0)
                {
                    DataGridViewRow row; 
                    for (int i = 0; i < ns.Count(); i++)
                    {
                        row = new DataGridViewRow();
                        row.Cells.Add(bindCollValue(ns[i].LineName));
                        row.Cells.Add(bindCollValue(ns[i].CurrentLabors));
                        row.Cells.Add(bindCollValue(ns[i].CommoName));
                        row.Cells.Add(bindCollValue(ns[i].SanLuongKeHoach));
                        row.Cells.Add(bindCollValue(ns[i].LuyKeTH));
                        row.Cells.Add(bindCollValue(ns[i].RevenuesInDay));
                        row.Cells.Add(bindCollValue(ns[i].RevenuesInMonth));
                       // row.Cells.Add(bindCollValue(ns[i].ThuNhapBQ));
                        row.Cells.Add(bindCollValue(0));
                        row.Cells.Add(bindCollValue(ns[i].LK_BTP_InMonth));
                        row.Cells.Add(bindCollValue((ns[i].Lean+"/"+ns[i].BTPInLine)));
                        row.Cells.Add(bindCollValue(ns[i].BTP_Day - ns[i].BTP_Day_G));
                        row.Cells.Add(bindCollValue(Math.Round(ns[i].NormsDay)));
                        row.Cells.Add(bindCollValue(Math.Round(ns[i].NormsHours)));
                        row.Cells.Add(bindCollValue(ns[i].TH_Day - ns[i].TH_Day_G));
                        row.Cells.Add(bindCollValue(ns[i].TC_Day - ns[i].TC_Day_G));

                        var DM = ns[i].NormsDay * ns[i].TGDaLV;
                        var tc = ns[i].workingTimes.Sum(x => x.TC);
                        row.Cells.Add(bindCollValue(tc > 0 ? Math.Round(((tc / DM) * 100)) : 0));

                        var kd = ns[i].workingTimes.Sum(x => x.KCS);
                        row.Cells.Add(bindCollValue(kd > 0 ? Math.Round(((kd / DM) * 100)) : 0));

                        var tongLoi = ns[i].workingTimes.Sum(x => x.Error);
                        row.Cells.Add(bindCollValue(tongLoi > 0 ? Math.Round((double)(tongLoi / (ns[i].workingTimes.Sum(x => x.KCS) + tongLoi)) * 100, 1) : tongLoi));

                        row.Cells.Add(bindCollValue(ns[i].NhipTT + "/" + Math.Round(ns[i].NhipSX, 2)));
                        dgTTNangXuat.Rows.Add(row);
                        if (dgTTNangXuat.Rows.Count % 2 == 0)
                            dgTTNangXuat.Rows[dgTTNangXuat.Rows.Count - 1].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                    }
                }
                else
                    MessageBox.Show("Không tìm thấy thông tin Năng Suất của ngày : "+dtpDate.Value.ToString("dd/MM/yyyy"));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private DataGridViewCell bindCollValue(dynamic value)
        {
            var col = new DataGridViewTextBoxCell();
            col.Value = value;
            return col;
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            GetData();
        }
    }
}
