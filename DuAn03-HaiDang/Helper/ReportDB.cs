using DevExpress.XtraCharts;
using PMS.Business;
using PMS.Business.Enum;
using PMS.Business.Models;
using PMS.Business.Web;
using QuanLyNangSuat.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace DuAn03_HaiDang
{
    public class ReportDB
    {

        static public bool exportMulDataToExcel(string path, List<ModelSheetExcel> listModelSheet)
        {
            bool result = false;
            if (listModelSheet != null && listModelSheet.Count > 0)
            {
                Excel.Application xlApp;
                Excel.Workbook xlWorkbook;
                Excel.Sheets xlSheets = null;
                Excel.Worksheet xlWorksheet = null;
                object missValue = System.Reflection.Missing.Value;
                xlApp = new Excel.Application();

                xlWorkbook = xlApp.Workbooks.Add(missValue);
                xlWorkbook.CheckCompatibility = false;
                xlWorkbook.DoNotPromptForConvert = true;
                xlApp.Visible = false;
                for (int index = 0; index < listModelSheet.Count; index++)
                {
                    var model = listModelSheet[index];
                    xlSheets = xlWorkbook.Sheets;
                    xlWorksheet = (Excel.Worksheet)xlSheets.Add(xlSheets[1], Type.Missing, Type.Missing, Type.Missing);
                    xlWorksheet.Name = model.Title;
                    System.Data.DataTable dt = model.Data;
                    int countColumn = dt.Columns.Count;
                    int countRow = dt.Rows.Count;
                    xlWorksheet.get_Range("A1", Convert.ToChar(countColumn + 65) + "1").Merge(false);
                    Excel.Range caption = xlWorksheet.get_Range("A1", Convert.ToChar(countColumn + 65) + "1");
                    caption.Select();
                    caption.FormulaR1C1 = model.Title;
                    caption.HorizontalAlignment = Excel.Constants.xlCenter;
                    caption.Font.Bold = true;
                    caption.VerticalAlignment = Excel.Constants.xlCenter;
                    caption.Font.Size = 15;
                    caption.Interior.ColorIndex = 20;
                    caption.RowHeight = 30;
                    Excel.Range header = xlWorksheet.get_Range("A2", Convert.ToChar(countColumn + 65) + "2");
                    header.Select();
                    header.HorizontalAlignment = Excel.Constants.xlCenter;
                    header.Font.Bold = true;
                    header.Font.Size = 10;

                    int i, j;

                    for (i = 0; i < countColumn; i++)
                        xlWorksheet.Cells[2, i + 2] = dt.Columns[i].ColumnName;
                    xlWorksheet.Cells[2, 1] = "STT";
                    for (i = 0; i < countRow; i++)
                        xlWorksheet.Cells[i + 3, 1] = i + 1;
                    for (i = 0; i < countRow; i++)
                    {
                        for (j = 0; j < countColumn; j++)
                        {
                            xlWorksheet.Cells[i + 3, j + 2] = dt.Rows[i][j];
                        }
                    }
                    for (i = 0; i < countRow; i++)
                        ((Excel.Range)xlWorksheet.Cells[1, i + 1]).EntireColumn.AutoFit();
                }

                //  DeleteAllFileInPath(path);
                xlWorkbook.SaveAs(path, Excel.XlFileFormat.xlWorkbookDefault, missValue, missValue, missValue, missValue, Excel.XlSaveAsAccessMode.xlNoChange, missValue, missValue, missValue, missValue, missValue);
                xlWorkbook.Close(true, missValue, missValue);
                xlApp.Quit();
                releaseObject(xlWorkbook);
                releaseObject(xlApp);
                result = true;
            }
            return result;
        }

        static public bool exportDataToExcel(string tieude, string path, string fileName, System.Data.DataTable dt)
        {
            bool result = false;
            //khoi tao cac doi tuong Com Excel de lam viec
            Excel.Application xlApp;
            Excel.Worksheet xlSheet;
            Excel.Workbook xlBook;
            //doi tuong Trống để thêm  vào xlApp sau đó lưu lại sau
            object missValue = System.Reflection.Missing.Value;
            //khoi tao doi tuong Com Excel moi
            xlApp = new Excel.Application();
            xlBook = xlApp.Workbooks.Add(missValue);
            xlBook.DoNotPromptForConvert = true;
            xlBook.CheckCompatibility = false;

            //su dung Sheet dau tien de thao tac
            xlSheet = (Excel.Worksheet)xlBook.Worksheets.get_Item(1);

            //không cho hiện ứng dụng Excel lên để tránh gây đơ máy
            xlApp.Visible = false;
            int socot = dt.Columns.Count;
            int sohang = dt.Rows.Count;
            int i, j;
            //set thuoc tinh cho tieu de
            xlSheet.get_Range("A1", Convert.ToChar(socot + 65) + "1").Merge(false);
            Excel.Range caption = xlSheet.get_Range("A1", Convert.ToChar(socot + 65) + "1");
            caption.Select();
            caption.FormulaR1C1 = tieude;
            //căn lề cho tiêu đề
            caption.HorizontalAlignment = Excel.Constants.xlCenter;
            caption.Font.Bold = true;
            caption.VerticalAlignment = Excel.Constants.xlCenter;
            caption.Font.Size = 15;
            //màu nền cho tiêu đề
            caption.Interior.ColorIndex = 20;
            caption.RowHeight = 30;
            //set thuoc tinh cho cac header
            Excel.Range header = xlSheet.get_Range("A2", Convert.ToChar(socot + 65) + "2");
            header.Select();

            header.HorizontalAlignment = Excel.Constants.xlCenter;
            header.Font.Bold = true;
            header.Font.Size = 10;
            //điền tiêu đề cho các cột trong file excel
            for (i = 0; i < socot; i++)
                xlSheet.Cells[2, i + 2] = dt.Columns[i].ColumnName;
            //dien cot stt
            xlSheet.Cells[2, 1] = "STT";
            for (i = 0; i < sohang; i++)
                xlSheet.Cells[i + 3, 1] = i + 1;
            //dien du lieu vao sheet
            for (i = 0; i < sohang; i++)
                for (j = 0; j < socot; j++)
                {
                    xlSheet.Cells[i + 3, j + 2] = dt.Rows[i][j];
                }
            //autofit độ rộng cho các cột 
            for (i = 0; i < sohang; i++)
                ((Excel.Range)xlSheet.Cells[1, i + 1]).EntireColumn.AutoFit();

            // DeleteAllFileInPath(path);
            //save file 
            xlBook.SaveAs(path + fileName, Excel.XlFileFormat.xlWorkbookDefault, missValue, missValue, missValue, missValue, Excel.XlSaveAsAccessMode.xlNoChange, missValue, missValue, missValue, missValue, missValue);
            xlBook.Close(true, missValue, missValue);
            xlApp.Quit();

            // release cac doi tuong COM
            releaseObject(xlSheet);
            releaseObject(xlBook);
            releaseObject(xlApp);
            result = true;
            return result;
        }

        static public bool ExportToTextFile(System.Data.DataTable dataTable)
        {
            bool resultCreate = false;
            try
            {
                SaveFileDialog f = new SaveFileDialog();
                f.Filter = "Text file (*.txt)|*.txt";
                if (f.ShowDialog() == DialogResult.OK)
                {
                    using (Stream s = File.Open(f.FileName, FileMode.CreateNew))
                    using (StreamWriter sw = new StreamWriter(s))
                    {
                        var result = new StringBuilder();
                        int columnCount = dataTable.Columns.Count;
                        if (columnCount < 5)
                        {
                            foreach (DataColumn column in dataTable.Columns)
                            {
                                result.Append(column.Caption.ToString());
                                result.Append("\t\t\t");
                            }

                            foreach (DataRow row in dataTable.Rows)
                            {
                                for (int i = 0; i < columnCount; i++)
                                {
                                    result.Append(row[i].ToString());
                                    result.Append(i == dataTable.Columns.Count - 1 ? "\n" : "\t\t\t");
                                }
                                result.AppendLine();
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                result.Append(dataTable.Columns[i].Caption.ToString());
                                result.Append("\t\t\t");
                            }
                            result.AppendLine();

                            foreach (DataRow row in dataTable.Rows)
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    result.Append(row[i].ToString());
                                    result.Append("\t\t\t");
                                }
                                result.AppendLine();
                            }

                            result.AppendLine(); result.AppendLine(); result.AppendLine(); result.AppendLine();

                            for (int i = 4; i < columnCount; i++)
                            {
                                result.Append(dataTable.Columns[i].Caption.ToString());
                                result.Append("\t\t\t");
                            }
                            result.AppendLine();

                            foreach (DataRow row in dataTable.Rows)
                            {
                                for (int i = 5; i < columnCount; i++)
                                {
                                    result.Append(row[i].ToString());
                                    result.Append("\t\t\t");
                                }
                                result.AppendLine();
                            }
                        }
                        sw.Write(result.ToString());
                        sw.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resultCreate;
        }

        static public void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                throw new Exception("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        static public System.Data.DataTable ConvertGridViewToDataTable(DevComponents.DotNetBar.Controls.DataGridViewX gridView)
        {
            System.Data.DataTable dt = null;
            try
            {
                if (gridView != null)
                {
                    dt = new System.Data.DataTable(gridView.Name);
                    List<string> listProName = new List<string>();
                    foreach (System.Windows.Forms.DataGridViewColumn col in gridView.Columns)
                    {
                        dt.Columns.Add(col.HeaderText, typeof(System.String));
                        listProName.Add(col.Name);
                    }
                    for (int j = 0; j < gridView.RowCount; j++)
                    {
                        var values = new object[listProName.Count];
                        for (int i = 0; i < listProName.Count; i++)
                        {
                            values[i] = gridView.Rows[j].Cells[listProName[i]].Value.ToString();

                        }
                        dt.Rows.Add(values);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        static public bool ExportToExcelByGirdView(string tieuDe, string path, string fileName, DevComponents.DotNetBar.Controls.DataGridViewX gridView)
        {
            bool result = false;
            try
            {
                System.Data.DataTable dt = ConvertGridViewToDataTable(gridView);
                if (dt != null && dt.Rows.Count > 0)
                {
                    result = exportDataToExcel(tieuDe, path, fileName, dt);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        static public bool ExportToExcelByDataTable(string tieuDe, string path, string fileName, System.Data.DataTable dt)
        {
            bool result = false;
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    result = exportDataToExcel(tieuDe, path, fileName, dt);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


        //HoangHai
        static public bool ExportToExcel_ProductivitiesByHour(string templateName, string tieuDe, string path, string fileName, List<ChuyenSanPhamModel> lines)
        {
            var result = false;
            try
            {
                #region khoi tao cac doi tuong Com Excel de lam viec
                Excel.Application xlApp;
                Excel.Worksheet xlSheet;
                Excel.Workbook xlBook;
                Excel.Range oRng;
                //doi tuong Trống để thêm  vào xlApp sau đó lưu lại sau
                object missValue = System.Reflection.Missing.Value;
                //khoi tao doi tuong Com Excel moi
                xlApp = new Excel.Application();
                //xlBook = xlApp.Workbooks.Add(missValue);
                //xlBook.CheckCompatibility = false;
                //xlBook.DoNotPromptForConvert = true;
                string templatePath = System.Windows.Forms.Application.StartupPath + @"\Report\Template\" + templateName;
                xlBook = xlApp.Workbooks.Open(templatePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                xlBook.CheckCompatibility = false;
                xlBook.DoNotPromptForConvert = true;

                //su dung Sheet dau tien de thao tac
                xlSheet = (Excel.Worksheet)xlBook.Worksheets.get_Item(1);
                //không cho hiện ứng dụng Excel lên để tránh gây đơ máy
                xlApp.Visible = false;
                #endregion
                // var headerArr = ("Chuyền,Lao Động (TT/ĐB),Mã Hàng,Sản Lượng Kế hoạch,Lũy Kế Kiểm đạt, Doanh Thu Tháng,Doanh Thu Ngày,Thu Nhập Bình Quân,Lũy kế BTP,Vốn / BTP Trên Chuyền,BTP Ngày,Định Mức Ngày,Định Mức Giờ,Thông số năng suất").Split(',').ToArray();
                // int socot = headerArr.Length;
                int socot = 16; // headerArr.Length;
                int sohang = 5;
                int i, j;
                string endChar = "", kytu = "";
                int start = 5;
                int thoigianLV = 0;

                //Excel.Range header = xlSheet.get_Range("B2", Convert.ToChar(socot + 65) + "2");
                //header.Interior.ColorIndex = 6;
                //header.Font.ColorIndex = 3;
                //header.Font.Bold = true;
                //header.Font.Size = 12;
                //header.HorizontalAlignment = Excel.Constants.xlCenter;
                //header.VerticalAlignment = Excel.Constants.xlCenter;
                //header.WrapText = true;

                #region header
                //for (int a = 0; a < socot; a++)
                //{
                //    kytu = Convert.ToChar((a + 1) + 65).ToString();
                //    oRng = xlSheet.get_Range(kytu + "2:" + kytu + "4", kytu + "2:" + kytu + "4");
                //    oRng.Select();
                //    oRng.Merge();
                //    oRng.Value = headerArr[a];
                //    oRng.Borders.ColorIndex = 56;
                //    if ((a + 1) == 15)
                //        oRng.Interior.ColorIndex = 45;
                //}
                #endregion

                var congDoans = BLLBTP_HCStructure.Instance.Gets((int)ePhaseType.HOANTAT);
                var slCongDoanTrongNgay = BLLPhaseInDay.Instance.GetPhaseDayInfo(lines.Select(x => x.STT).ToList(), (int)ePhaseType.HOANTAT, DateTime.Now);
                if (lines.Count > 0)
                {
                    for (int y = 0; y <= lines.Count; y++)
                    {
                        #region TTChung
                        for (int a = 0; a < socot; a++)
                        {
                            var ch = Convert.ToChar((a + 1) + 65).ToString();
                            if (y < lines.Count)
                            {
                                #region TT từng chuyền
                                if ((a + 2) < 17)
                                {
                                    oRng = xlSheet.get_Range(ch + start + ":" + ch + (start + 1 + congDoans.Count), ch + start + ":" + ch + (start + 1 + congDoans.Count));
                                    oRng.Borders.ColorIndex = 56;
                                    oRng.Merge();
                                    if (y % 2 == 0)
                                        oRng.Font.ColorIndex = 46;

                                    switch ((a + 2))
                                    {
                                        case 2:
                                            oRng.Value = lines[y].LineName;
                                            break;
                                        case 3:
                                            oRng.Value = lines[y].CurrentLabors;
                                            break;
                                        case 4:
                                            oRng.Value = lines[y].CommoName;
                                            break;
                                        case 5:
                                            oRng.Value = lines[y].SanLuongKeHoach;
                                            break;
                                        case 6:
                                            oRng.Value = (lines[y].BTP_Day - lines[y].BTP_Day_G);
                                            break;
                                        case 7:
                                            oRng.Value = lines[y].LK_BTP_InMonth;
                                            break;
                                        case 8:
                                            oRng.Value = (lines[y].Lean + " | " + lines[y].BTPInLine);
                                            break;
                                        case 9:
                                            oRng.Value = lines[y].LuyKeBTPThoatChuyen;
                                            break;
                                        case 10:
                                            oRng.Value = lines[y].LuyKeTH;
                                            break;
                                        case 11:
                                            oRng.Value = lines[y].LK_Loi;
                                            break;
                                        case 12:
                                            oRng.Value = lines[y].RevenuesInMonth;
                                            break;
                                        case 13:
                                            oRng.Value = lines[y].RevenuesInDay;
                                            break;
                                        case 14:
                                            oRng.Value = 0; // lines[y].ThuNhapBQ;   
                                            break;
                                        case 15:
                                            oRng.Value = Math.Round(lines[y].NormsDay);
                                            break;
                                        case 16:
                                            oRng.Value = Math.Round(lines[y].NormsHours);
                                            break;
                                    }
                                }
                                else
                                {
                                    #region                                     
                                    oRng = xlSheet.get_Range(ch + start);
                                    oRng.Value = "TC";
                                    SetBorder_TextAlign(oRng, true);
                                    oRng.Borders.ColorIndex = 15;
                                    oRng.Interior.ColorIndex = 46;
                                    oRng.Font.ColorIndex = 1;
                                    oRng.Font.Bold = true;

                                    var oRng1 = xlSheet.get_Range(ch + (start + 1));
                                    oRng1.Value = "KĐ/LỖI";
                                    SetBorder_TextAlign(oRng1, true);
                                    oRng1.Borders.ColorIndex = 15;
                                    oRng1.Interior.ColorIndex = 46;
                                    oRng1.Font.ColorIndex = 1;
                                    oRng1.Font.Bold = true;
                                    if (congDoans.Count > 0)
                                    {
                                        for (int iii = 0; iii < congDoans.Count; iii++)
                                        {
                                            oRng1 = xlSheet.get_Range(ch + (start + iii + 2));
                                            oRng1.Value = congDoans[iii].Name.ToUpper();
                                            SetBorder_TextAlign(oRng1, true);
                                            oRng1.Borders.ColorIndex = 15;
                                            oRng1.Interior.ColorIndex = 46;
                                            oRng1.Font.ColorIndex = 1;
                                            oRng1.Font.Bold = true;

                                        }
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            if (y == lines.Count)
                            {
                                #region TT Tổng Xưởng
                                oRng = xlSheet.get_Range(ch + start);
                                switch ((a + 2))
                                {
                                    case 2: oRng.Value = "Xưởng"; break;
                                    case 5: oRng.Value = lines.Sum(x => x.SanLuongKeHoach); break;
                                    case 6: oRng.Value = lines.Sum(x => x.BTP_Day) - lines.Sum(x => x.BTP_Day_G); break;
                                    case 7: oRng.Value = lines.Sum(x => x.LK_BTP - x.LK_BTP_G); break;
                                    case 9: oRng.Value = lines.Sum(x => x.LuyKeBTPThoatChuyen); break;
                                    case 10: oRng.Value = lines.Sum(x => x.LuyKeTH); break;
                                    case 11: oRng.Value = lines.Sum(x => x.LK_Loi); break;
                                    case 12: oRng.Value = lines.Sum(x => x.RevenuesInMonth); break;
                                    case 13: oRng.Value = lines.Sum(x => x.RevenuesInDay); break;
                                    case 14:
                                        oRng.Value = 0;// lines.Sum(x => x.ThuNhapBQ);
                                        break;
                                    case 15: oRng.Value = Math.Round(lines.Sum(x => x.NormsDay)); break;
                                    case 16: oRng.Value = Math.Round(lines.Sum(x => x.NormsHours)); break;
                                }
                                SetBorder_TextAlign(oRng, true);
                                oRng.Interior.ColorIndex = 6;
                                oRng.RowHeight = 30;
                                oRng.Borders.ColorIndex = 56;
                                #endregion
                            }
                        }
                        #endregion

                        #region NS Theo Gio
                        if (y < lines.Count)
                        {
                            if (lines[y].workingTimes != null && lines[y].workingTimes.Count > 0)
                            {
                                thoigianLV = lines[y].workingTimes.Count > thoigianLV ? lines[y].workingTimes.Count : thoigianLV;
                                for (int yy = 0; yy < lines[y].workingTimes.Count; yy++)
                                {
                                    var c = ConvertChar(yy + 65 + 17);

                                    if (y == 0)
                                    {
                                        oRng = xlSheet.get_Range(c + 4);
                                        oRng.Value = string.Format("{0}h:{1}", lines[0].workingTimes[yy].TimeEnd.Hours, lines[0].workingTimes[yy].TimeEnd.Minutes);
                                        SetBorder_TextAlign(oRng, true);
                                        oRng.Interior.ColorIndex = 6;
                                        oRng.Font.ColorIndex = 3;
                                    }

                                    oRng = xlSheet.get_Range(c + start);
                                    oRng.Value = lines[y].workingTimes[yy].TC;
                                    oRng.Borders.ColorIndex = 56;

                                    var oRng1 = xlSheet.get_Range(c + (start + 1));
                                    oRng1.Value = lines[y].workingTimes[yy].KCS + " | " + lines[y].workingTimes[yy].Error;
                                    oRng1.Borders.ColorIndex = 56;

                                    if (y % 2 == 0)
                                    {
                                        oRng.Font.ColorIndex = 46;
                                        oRng1.Font.ColorIndex = 46;
                                    }

                                    if (congDoans.Count > 0)
                                    {
                                        for (int iii = 0; iii < congDoans.Count; iii++)
                                        {
                                            oRng1 = xlSheet.get_Range(c + (start + iii + 2));
                                            int sl = 0;
                                            if (slCongDoanTrongNgay.Count > 0)
                                            {
                                                var slCD = slCongDoanTrongNgay.Where(x =>
                                                x.AssignId == lines[y].STT &&
                                                x.PhaseId == congDoans[iii].Id &&
                                                x.Date.TimeOfDay >= lines[y].workingTimes[yy].TimeStart &&
                                                x.Date.TimeOfDay <= lines[y].workingTimes[yy].TimeEnd
                                                ).ToList();
                                                if (slCD.Count > 0)
                                                {
                                                    sl = slCD.Where(x => x.CommandTypeId == (int)eCommandRecive.ProductIncrease).Sum(x => x.Quantity);
                                                    sl -= slCD.Where(x => x.CommandTypeId == (int)eCommandRecive.ProductReduce).Sum(x => x.Quantity);
                                                }
                                            }
                                            oRng1.Value = sl;
                                            SetBorder_TextAlign(oRng1, false);
                                            oRng1.Borders.ColorIndex = 56;
                                            if (y % 2 == 0)
                                                oRng1.Font.ColorIndex = 46;
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                        start += (2 + congDoans.Count);
                    }

                    #region nap tieu de NS theo gio
                    //var cha = Convert.ToChar(socot + 1 + 65).ToString();
                    //var cha1 = ConvertChar(socot + thoigianLV + 65);
                    //oRng = xlSheet.get_Range(cha + "2:" + cha + "3", cha1 + "2:" + cha1 + "3");
                    //oRng.Select();
                    //oRng.Merge();
                    //oRng.Value = "THÔNG TIN NĂNG SUẤT HÀNG GIỜ";
                    //SetBorder_TextAlign(oRng, true);
                    //oRng.Font.ColorIndex = 3;
                    //oRng.Interior.ColorIndex = 6;

                    //for (int y = 0; y < thoigianLV; y++)
                    //{
                    //    var c = ConvertChar(y + 65 + 17);
                    //    oRng = xlSheet.get_Range(c + "4");
                    //    oRng.Value = (y + 1) + "H";
                    //    SetBorder_TextAlign(oRng, true);
                    //    oRng.Interior.ColorIndex = 6;
                    //    oRng.Font.ColorIndex = 3;
                    //}

                    oRng = null;
                    #endregion

                    int reStart = 5;
                    var col = 17 + thoigianLV;

                    int titleLength = 8;
                    if (lines.Count > 0)
                    {
                        #region Thông so sau
                        for (int y = 0; y <= lines.Count; y++)
                        {
                            for (int z = 0; z < titleLength; z++)
                            {
                                var c = ConvertChar(col + z + 65);
                                // bind value 
                                #region TT
                                if (y != lines.Count)
                                {
                                    oRng = xlSheet.get_Range(c + reStart + ":" + c + (reStart + 1 + congDoans.Count), c + reStart + ":" + c + (reStart + 1 + congDoans.Count));
                                    oRng.Borders.ColorIndex = 56;
                                    oRng.Font.ColorIndex = 1;
                                    oRng.Merge();
                                    if (y % 2 == 0)
                                    {
                                        //oRng.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(0, 176, 80));
                                        oRng.Font.ColorIndex = 46;
                                    }

                                    switch (z)
                                    {
                                        case 0: oRng.Value = (y == lines.Count ? lines.Sum(x => x.TC_Day) - lines.Sum(x => x.TC_Day_G) : lines[y].TC_Day - lines[y].TC_Day_G); break;
                                        case 1: oRng.Value = (y == lines.Count ? lines.Sum(x => x.TH_Day) - lines.Sum(x => x.TH_Day_G) : (lines[y].TH_Day - lines[y].TH_Day_G)); break;
                                        case 2: oRng.Value = (y == lines.Count ? lines.Sum(x => x.Err_Day) - lines.Sum(x => x.Err_Day_G) : (lines[y].Err_Day - lines[y].Err_Day_G)); break;
                                        case 3: //TC / Muc khoan
                                            if (y != lines.Count)
                                            {
                                                var DM = lines[y].NormsHours * lines[y].TGDaLV;
                                                var tc = lines[y].workingTimes.Sum(x => x.TC);
                                                oRng.Value = tc > 0 ? Math.Round(((tc / DM) * 100)) : 0;
                                            }
                                            break;
                                        case 4: // KCS / Muc Khoan
                                            if (y != lines.Count)
                                            {
                                                var DM = lines[y].NormsHours * lines[y].TGDaLV;
                                                var kd = lines[y].workingTimes.Sum(x => x.KCS);
                                                oRng.Value = kd > 0 ? Math.Round(((kd / DM) * 100)) : 0;
                                            }
                                            break;
                                        case 5://loi / kiem qua tay
                                            if (y != lines.Count)
                                            {
                                                var tongLoi = lines[y].workingTimes.Sum(x => x.Error);
                                                oRng.Value = tongLoi > 0 ? Math.Round((double)(tongLoi / (lines[y].workingTimes.Sum(x => x.KCS) + tongLoi)) * 100, 1) : tongLoi;
                                            }
                                            break;
                                        case 6: if (y != lines.Count) { oRng.Value = lines[y].NhipTT + " | " + Math.Round(lines[y].NhipSX, 2); } break;
                                        case 7: oRng.Value = " "; break;
                                    }
                                }
                                #endregion

                                #region Tong
                                if (y == lines.Count)
                                {
                                    endChar = c;
                                    oRng = xlSheet.get_Range(c + reStart);
                                    oRng.Interior.ColorIndex = 6;
                                    oRng.RowHeight = 30;
                                    oRng.Borders.ColorIndex = 56;
                                    switch (z)
                                    {
                                        case 0: oRng.Value = lines.Sum(x => x.TC_Day) - lines.Sum(x => x.TC_Day_G); break;
                                        case 1: oRng.Value = lines.Sum(x => x.TH_Day) - lines.Sum(x => x.TH_Day_G); break;
                                    }
                                }
                                #endregion
                                SetBorder_TextAlign(oRng, true);
                            }
                            reStart += (2 + congDoans.Count);
                        }
                        #endregion
                    }
                }

                oRng = xlSheet.get_Range("B5:B" + start, ConvertChar(socot + 65 + thoigianLV) + "5:" + ConvertChar(socot + 65 + thoigianLV) + start);
                oRng.HorizontalAlignment = Excel.Constants.xlCenter;
                oRng.VerticalAlignment = Excel.Constants.xlCenter;
                oRng.WrapText = true;
                #region

                #region Title
                //set thuoc tinh cho tieu de
                xlSheet.get_Range("B1", endChar + "1").Merge(false);
                Excel.Range caption = xlSheet.get_Range("B1", endChar + "1");
                caption.Select();
                caption.FormulaR1C1 = tieuDe + " - Ngày " + DateTime.Now.ToString("dd/MM/yyyy - hh:mm");// "Báo Cáo Thông Tin Năng Suất Hàng Giờ của Xưởng - Ngày " + DateTime.Now.ToString("dd-MM-yyyy : hh:mm:ss");
                //căn lề cho tiêu đề
                caption.HorizontalAlignment = Excel.Constants.xlCenter;
                caption.VerticalAlignment = Excel.Constants.xlCenter;
                // caption.Font.Bold = true;

                //caption.Font.Size = 15;
                ////màu nền cho tiêu đề
                //caption.Interior.ColorIndex = 6;
                //caption.RowHeight = 30;

                Excel.Range v = xlSheet.get_Range("B" + ((lines.Count * (2 + congDoans.Count)) + 5), endChar + ((lines.Count * (2 + congDoans.Count)) + 5));
                v.Interior.ColorIndex = 6;
                v.Borders.ColorIndex = 56;
                #endregion
                //autofit độ rộng cho các cột 
                for (i = 0; i < sohang; i++)
                    ((Excel.Range)xlSheet.Cells[1, i + 1]).EntireColumn.AutoFit();

                DeleteAllFileInPath(path);
                //save file
                xlBook.SaveAs(path + fileName, Excel.XlFileFormat.xlWorkbookDefault, missValue, missValue, missValue, missValue, Excel.XlSaveAsAccessMode.xlNoChange, missValue, missValue, missValue, missValue, missValue);
                xlBook.Close(true, missValue, missValue);
                xlApp.Quit();

                // release cac doi tuong COM
                releaseObject(xlSheet);
                releaseObject(xlBook);
                releaseObject(xlApp);
                result = true;
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private static void SetBorder_TextAlign(Excel.Range oRng, bool fontWeight)
        {
            oRng.Select();
            oRng.Merge();
            oRng.Interior.ColorIndex = 2;
            oRng.Borders.ColorIndex = 56;
            oRng.Font.Name = "Times New Roman";
            oRng.Font.Bold = fontWeight;
            oRng.Font.Size = 10;
            oRng.HorizontalAlignment = Excel.Constants.xlCenter;
            oRng.VerticalAlignment = Excel.Constants.xlCenter;
            oRng.WrapText = true;
        }

        static public bool DrawChartAndExport(List<LineModel> lines, string Title, string path, string fileName, bool IsError, bool IsKCS)
        {
            try
            {
                // Create an empty chart.
                ChartControl chart = new ChartControl();
                chart.Size = new System.Drawing.Size(1500, 750);
                if (IsError)
                {
                    var series = lines.FirstOrDefault().Errors;
                    DevExpress.XtraCharts.Series s = null;
                    for (int i = 0; i <= series.Count; i++)
                    {
                        if (i == series.Count)
                            s = new DevExpress.XtraCharts.Series("Tổng Lỗi", ViewType.Bar);
                        else
                            s = new DevExpress.XtraCharts.Series(series[i].Name, ViewType.Bar);
                        s.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                        chart.Series.Add(s);
                    }
                    foreach (var item in lines)
                    {
                        for (int i = 0; i < chart.Series.Count; i++)
                        {
                            chart.Series[i].Points.Add(new SeriesPoint(item.TenChuyen, new double[] { ((i == (chart.Series.Count - 1)) ? item.Errors.Sum(x => x.Quantity) : item.Errors[i].Quantity) }));
                        }
                    }
                }
                else
                {
                    var s1 = new DevExpress.XtraCharts.Series(IsKCS ? "Kiểm đạt" : "Thoát chuyền", ViewType.Bar);
                    s1.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                    var s2 = new DevExpress.XtraCharts.Series("Định mức", ViewType.Bar);
                    s2.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                    foreach (var item in lines)
                    {
                        s1.Points.Add(new SeriesPoint(item.TenChuyen, new double[] { IsKCS ? item.KCS : item.TC }));
                        s2.Points.Add(new SeriesPoint(item.TenChuyen, new double[] { item.ProBase }));
                    }
                    chart.Series.Add(s1);
                    chart.Series.Add(s2);
                }
                // Hide the legend (if necessary). 
                chart.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;
                // Add a title to the chart (if necessary).
                DevExpress.XtraCharts.ChartTitle chartTitle = new DevExpress.XtraCharts.ChartTitle();
                chartTitle.Text = Title;
                chart.Titles.Add(chartTitle);
                string fullPath = path + fileName;
                //   DeleteAllFileInPath(path);
                chart.ExportToXlsx(fullPath);
                return true;
            }
            catch (Exception)
            {
                return false;
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

        /// <summary>
        /// Report NS theo giờ mẫu 2 May Đức Giang
        /// </summary>
        /// <param name="tieuDe"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="lines"></param>
        /// <returns></returns>
        static public bool ExportToExcel_ProductivitiesByHour_2(string tieuDe, string path, string fileName, List<ChuyenSanPhamModel> lines, int timesGetNSInDay)
        {
            var result = false;
            try
            {
                #region khoi tao cac doi tuong Com Excel de lam viec
                Excel.Application xlApp;
                Excel.Worksheet xlSheet;
                Excel.Workbook xlBook;
                Excel.Range oRng;
                //doi tuong Trống để thêm  vào xlApp sau đó lưu lại sau
                object missValue = System.Reflection.Missing.Value;
                //khoi tao doi tuong Com Excel moi
                xlApp = new Excel.Application();
                xlBook = xlApp.Workbooks.Add(missValue);
                xlBook.CheckCompatibility = false;
                xlBook.DoNotPromptForConvert = true;
                //su dung Sheet dau tien de thao tac
                xlSheet = (Excel.Worksheet)xlBook.Worksheets.get_Item(1);
                //không cho hiện ứng dụng Excel lên để tránh gây đơ máy
                xlApp.Visible = false;

                #endregion
                #region header
                var headerArr = ("Chuyền,Tổng Lao Động ,Mã Hàng / Đơn Hàng,BTP,Giá CM,đã Ra Chuyền,Còn lại,Thời gian chế tạo (s),Chỉ tiêu").Split(',').ToArray();
                int socot = headerArr.Length;
                int sohang = 5;
                int i, j;
                string endChar = "", kytu = "";
                int start = 5;
                int thoigianLV = timesGetNSInDay;

                Excel.Range header = xlSheet.get_Range("B2", Convert.ToChar(socot + 65) + "2");
                header.Interior.ColorIndex = 6;
                header.Font.ColorIndex = 3;
                header.Font.Bold = true;
                header.Font.Size = 12;
                header.HorizontalAlignment = Excel.Constants.xlCenter;
                header.VerticalAlignment = Excel.Constants.xlCenter;
                header.WrapText = true;

                for (int a = 0; a < socot; a++)
                {
                    kytu = Convert.ToChar((a + 1) + 65).ToString();
                    oRng = xlSheet.get_Range(kytu + "2:" + kytu + "4", kytu + "2:" + kytu + "4");
                    oRng.Select();
                    oRng.Merge();
                    oRng.Value = headerArr[a];
                    oRng.Borders.ColorIndex = 56;
                    if ((a + 1) == 15)
                        oRng.Interior.ColorIndex = 45;
                }

                #region nap tieu de NS theo gio
                var cha = ConvertChar(socot + 1 + 65);
                var cha1 = ConvertChar(socot + (thoigianLV * 3) + 65);
                oRng = xlSheet.get_Range(cha + "2", cha1 + "2");
                oRng.Select();
                oRng.Merge();
                oRng.Value = "THÔNG TIN NĂNG SUẤT NGÀY";
                SetBorder_TextAlign(oRng, true);
                oRng.Font.ColorIndex = 3;
                oRng.Interior.ColorIndex = 6;

                int so = socot + 1;
                for (int y = 0; y < thoigianLV; y++)
                {
                    cha = ConvertChar((y + 65 + so));
                    cha1 = ConvertChar(y + 2 + 65 + so);
                    oRng = xlSheet.get_Range(cha + "3", cha1 + "3");
                    oRng.Select();
                    oRng.Merge();
                    oRng.Value = "Lần " + (y + 1);
                    SetBorder_TextAlign(oRng, true);
                    oRng.Interior.ColorIndex = 6;
                    oRng.Font.ColorIndex = 3;

                    for (int z = 0; z < 3; z++)
                    {
                        cha = ConvertChar((y + 65 + so + z));
                        oRng = xlSheet.get_Range(cha + "4");
                        switch (z)
                        {
                            case 0: oRng.Value = "Kế Hoạch"; break;
                            case 1: oRng.Value = "Kiểm đạt"; break;
                            case 2: oRng.Value = "So sánh"; break;
                        }
                        SetBorder_TextAlign(oRng, true);
                        oRng.Interior.ColorIndex = 6;
                        oRng.Font.ColorIndex = 3;
                    }
                    so += 2;
                }

                so += thoigianLV;
                cha = ConvertChar((65 + so));
                cha1 = ConvertChar(2 + 65 + so);
                oRng = xlSheet.get_Range(cha + "2:" + cha + "3", cha1 + "2:" + cha1 + "3");
                oRng.Select();
                oRng.Merge();
                oRng.Value = "TỔNG HỢP THÁNG";
                SetBorder_TextAlign(oRng, true);
                oRng.Interior.ColorIndex = 6;
                oRng.Font.ColorIndex = 3;
                endChar = cha1;

                for (int z = 0; z < 3; z++)
                {
                    cha = ConvertChar((65 + so + z));
                    oRng = xlSheet.get_Range(cha + "4");
                    switch (z)
                    {
                        case 0: oRng.Value = "Kế Hoạch"; break;
                        case 1: oRng.Value = "Kiểm đạt"; break;
                        case 2: oRng.Value = "So sánh"; break;
                    }
                    SetBorder_TextAlign(oRng, true);
                    oRng.Interior.ColorIndex = 6;
                    oRng.Font.ColorIndex = 3;
                }
                #endregion
                #endregion

                if (lines.Count > 0)
                {
                    int row = 5;
                    double value1 = 0, value2 = 0;
                    Excel.Range oRng1, oRng2, oRng3, oRng4, oRng5;
                    var title = ("Lean,BTP,Nhịp chuyền,Ra chuyền,Doanh thu,Số lượng lỗi").Split(',').ToArray();
                    for (int ii = 0; ii < lines.Count; ii++)
                    {
                        #region bind thông tin chung
                        for (int a = 0; a < headerArr.Length; a++)
                        {
                            #region TT từng chuyền
                            cha = ConvertChar((a + 1) + 65);
                            if ((a + 2) < 10)
                            {
                                oRng = xlSheet.get_Range(cha + row + ":" + (cha + (row + 5)), cha + row + ":" + (cha + (row + 5)));
                                oRng.Select();
                                oRng.Merge();
                                SetBorder_TextAlign(oRng, true);

                                //
                                if (((a + 2) == 2 || (a + 2) == 3) && ii == 0)
                                {
                                    oRng1 = xlSheet.get_Range(cha + (row + (lines.Count * 6)) + ":" + (cha + (row + (lines.Count > 5 ? lines.Count - 1 : 5) + (lines.Count * 6))), cha + (row + (lines.Count * 6)) + ":" + (cha + (row + (lines.Count > 5 ? lines.Count - 1 : 5) + (lines.Count * 6))));
                                    oRng1.Select();
                                    oRng1.Merge();
                                }
                                else
                                    oRng1 = xlSheet.get_Range(cha + (ii + 5 + (lines.Count * 6)));

                                SetBorder_TextAlign(oRng1, true);

                                if (ii % 2 == 0)
                                {
                                    //  oRng.Interior.Color = 4;
                                    oRng.Font.ColorIndex = 1;
                                }

                                switch ((a + 2))
                                {
                                    case 2:
                                        oRng.Value = lines[ii].LineName;
                                        if (ii == 0)
                                            oRng1.Value = "Tổng cả XN";
                                        break;
                                    case 3:
                                        oRng.Value = lines[ii].CurrentLabors;
                                        if (ii == 0)
                                        {
                                            var ds = lines.Distinct().Sum(x => x.CurrentLabors);
                                            oRng1.Value = ds;
                                        }

                                        break;
                                    case 4:
                                        oRng.Value = lines[ii].CommoName;
                                        oRng1.Value = lines[ii].CommoName;
                                        break;
                                    case 5:
                                        oRng.Value = lines[ii].LK_BTP_InMonth;
                                        oRng1.Value = lines[ii].LK_BTP_InMonth;
                                        break;
                                    case 6:
                                        oRng.Value = lines[ii].PriceCM;
                                        oRng1.Value = lines[ii].PriceCM;
                                        break;
                                    case 7:
                                        oRng.Value = lines[ii].LK_TH_InMonth;
                                        oRng1.Value = lines[ii].LK_TH_InMonth;
                                        break;
                                    case 8:
                                        oRng.Value = lines[ii].ProductionPlansInMonth - lines[ii].LK_TH_InMonth;
                                        oRng1.Value = lines[ii].ProductionPlansInMonth - lines[ii].LK_TH_InMonth;
                                        break;
                                    case 9:
                                        oRng.Value = Math.Round((lines[ii].ProductionTime * 100) / lines[ii].HieuSuatNgay);//thoi gian che tao
                                        oRng1.Value = Math.Round((lines[ii].ProductionTime * 100) / lines[ii].HieuSuatNgay);//thoi gian che tao
                                        break;
                                }
                            }
                            else
                            {
                                for (int z = 0; z < title.Length; z++)
                                {
                                    var zz = (row + z);
                                    oRng = xlSheet.get_Range(cha + zz);
                                    oRng.Value = title[z];
                                    oRng.Interior.ColorIndex = 46;
                                    oRng.Font.ColorIndex = 1;
                                    SetBorder_TextAlign(oRng, true);

                                    if (ii == 0)
                                    {
                                        zz = ((lines.Count * 6) + z + 5);
                                        oRng = xlSheet.get_Range(cha + zz);
                                        oRng.Value = title[z];
                                        oRng.Interior.ColorIndex = 46;
                                        oRng.Font.ColorIndex = 1;
                                        SetBorder_TextAlign(oRng, true);
                                    }
                                }
                            }
                            #endregion
                        }
                        #endregion

                        #region bind nang suat trong ngay
                        int col = headerArr.Length + 1;
                        if (lines[ii].workingTimes != null && lines[ii].workingTimes.Count > 0)
                        {
                            for (int y = 0; y < lines[ii].workingTimes.Count; y++)
                            {
                                for (int z = 0; z < title.Length; z++)
                                {
                                    var zz = (row + z);
                                    cha1 = ConvertChar((y + 65 + col));
                                    oRng = xlSheet.get_Range(cha1 + zz);
                                    SetBorder_TextAlign(oRng, true);

                                    cha1 = ConvertChar((y + 65 + col + 1));
                                    oRng1 = xlSheet.get_Range(cha1 + zz);
                                    SetBorder_TextAlign(oRng1, true);

                                    cha1 = ConvertChar((y + 65 + col + 2));
                                    oRng2 = xlSheet.get_Range(cha1 + zz);
                                    SetBorder_TextAlign(oRng2, true);

                                    if (ii % 2 == 0)
                                    {
                                        //  oRng.Interior.Color = 4;
                                        oRng.Font.ColorIndex = 1;
                                        //  oRng1.Interior.Color = 4;
                                        oRng1.Font.ColorIndex = 1;
                                        //   oRng2.Interior.Color = 4;
                                        oRng2.Font.ColorIndex = 1;
                                    }

                                    if (lines[ii].workingTimes[y].TimeEnd <= DateTime.Now.TimeOfDay)
                                    {
                                        #region Thong tin ngay từng giờ
                                        switch (z)
                                        {
                                            case 0: //lean
                                                value1 = lines[ii].LeanKH;  // lean = btp tren chuyen / so lao dong
                                                value2 = lines[ii].workingTimes[y].Lean;
                                                oRng.Value = value1;
                                                value2 = value2 < 0 ? 0 : value2;
                                                oRng1.Value = value2;
                                                oRng2.Value = value2 - value1;
                                                break;
                                            case 1:  //BTP 
                                                oRng.Value = lines[ii].LeanKH * lines[ii].CurrentLabors;  //= lean KH * ld can bang chuyen
                                                oRng1.Value = lines[ii].workingTimes[y].BTPInLine;
                                                oRng2.Value = ((lines[ii].workingTimes[y].BTPInLine) - (lines[ii].LeanKH * lines[ii].CurrentLabors));
                                                break;
                                            case 2: //nhip chuyen
                                                value1 = Math.Round((((lines[ii].ProductionTime * 100) / lines[ii].HieuSuatNgay) / lines[ii].CurrentLabors), 1);  // = tg che tao / ld can bang chuyen
                                                value2 = (lines[ii].workingTimes[y].KCS > 0 ? Math.Round(((double)(lines[ii].workingTimes[y].TimeEnd - lines[ii].workingTimes[y].TimeStart).TotalSeconds / lines[ii].workingTimes[y].KCS), 1) : 0); // = 3600 / ra chuyen
                                                oRng.Value = value1;
                                                oRng1.Value = value2;
                                                oRng2.Value = ((value1 > 0 && value2 > 0) ? Math.Round((value1 / value2) * 100) : 0) + "%";
                                                break;
                                            case 3: // ra chuyen
                                                value1 = lines[ii].workingTimes[y].NormsHour;  // = 3600 / nhip chuyen KH
                                                value2 = lines[ii].workingTimes[y].KCS;
                                                oRng.Value = value1;
                                                oRng1.Value = value2;
                                                oRng2.Value = ((value1 > 0 && value2 > 0) ? Math.Round((value2 / value1) * 100) : 0) + "%";
                                                break;
                                            case 4: // doanh thu
                                                value1 = lines[ii].workingTimes[y].NormsHour * lines[ii].PriceCM;  // rachuyen KH * gia CM
                                                value2 = lines[ii].workingTimes[y].KCS * lines[ii].PriceCM;  // rachuyen TH * gia CM
                                                oRng.Value = value1;
                                                oRng1.Value = value2;
                                                oRng2.Value = ((value1 > 0 && value2 > 0) ? Math.Round((value2 / value1) * 100) : 0) + "%";
                                                break;
                                            case 5: // loi
                                                oRng.Value = "";
                                                oRng1.Value = lines[ii].workingTimes[y].Error;
                                                oRng2.Value = ((lines[ii].workingTimes[y].Error > 0 && lines[ii].workingTimes[y].KCS > 0) ? Math.Round((double)(lines[ii].workingTimes[y].Error / lines[ii].workingTimes[y].KCS) * 100) : 0) + "%"; //= error / kcs * 100
                                                break;
                                        }
                                        #endregion
                                    }

                                    #region  Tong hop thang
                                    if (y == 0)
                                    {
                                        var l = lines[ii].workingTimes.Count * 3;
                                        cha1 = ConvertChar((l + 65 + col));
                                        oRng3 = xlSheet.get_Range(cha1 + zz);
                                        SetBorder_TextAlign(oRng, true);

                                        cha1 = ConvertChar((l + 65 + col + 1));
                                        oRng4 = xlSheet.get_Range(cha1 + zz);
                                        SetBorder_TextAlign(oRng4, true);

                                        cha1 = ConvertChar((l + 65 + col + 2));
                                        oRng5 = xlSheet.get_Range(cha1 + zz);
                                        SetBorder_TextAlign(oRng5, true);

                                        switch (z)
                                        {
                                            case 0: //lean                                                 
                                                break;
                                            case 1:  //BTP 
                                                oRng4.Value = lines[ii].LK_BTP_InMonth;

                                                break;
                                            case 2: //nhip chuyen  
                                                break;
                                            case 3: // ra chuyen 
                                                value1 = lines[ii].ProductionPlansInMonth;
                                                value2 = lines[ii].LK_TH_InMonth;
                                                oRng3.Value = value1;
                                                oRng4.Value = value2;
                                                oRng5.Value = ((value1 > 0 && value2 > 0) ? Math.Round((value2 / value1) * 100) : 0) + "%";
                                                break;
                                            case 4:
                                                value1 = Math.Round(lines[ii].ProductionPlansInMonth * lines[ii].PriceCM);
                                                value2 = Math.Round(lines[ii].LK_TH_InMonth * lines[ii].PriceCM);
                                                oRng3.Value = value1;
                                                oRng4.Value = value2;
                                                oRng5.Value = ((value1 > 0 && value2 > 0) ? Math.Round((value2 / value1) * 100) : 0) + "%";
                                                break;
                                            case 5: // loi
                                                break;
                                        }
                                    }
                                    #endregion

                                    #region Thong tin Tong Xuong
                                    if (ii == 0)
                                    {
                                        zz = ((ii + 5) + lines.Count * 6);
                                        cha1 = ConvertChar((y + 65 + col));
                                        oRng = xlSheet.get_Range(cha1 + (zz + z));
                                        SetBorder_TextAlign(oRng, true);

                                        cha1 = ConvertChar((y + 65 + col + 1));
                                        oRng1 = xlSheet.get_Range(cha1 + (zz + z));
                                        SetBorder_TextAlign(oRng1, true);

                                        cha1 = ConvertChar((y + 65 + col + 2));
                                        oRng2 = xlSheet.get_Range(cha1 + (zz + z));
                                        SetBorder_TextAlign(oRng2, true);

                                        //     
                                        if (ii % 2 == 0)
                                        {
                                            //  oRng.Interior.Color = 4;
                                            oRng.Font.ColorIndex = 1;
                                            //  oRng1.Interior.Color = 4;
                                            oRng1.Font.ColorIndex = 1;
                                            //   oRng2.Interior.Color = 4;
                                            oRng2.Font.ColorIndex = 1;
                                        }

                                        if (lines[ii].workingTimes[y].TimeEnd <= DateTime.Now.TimeOfDay)
                                        {
                                            #region
                                            switch (z)
                                            {
                                                case 0: //lean
                                                    value1 = lines[ii].LeanKH;  // lean = btp tren chuyen / so lao dong
                                                    value2 = lines.Sum(x => x.workingTimes[y].Lean);
                                                    oRng.Value = value1;
                                                    value2 = value2 < 0 ? 0 : value2;
                                                    oRng1.Value = value2;
                                                    oRng2.Value = value2 - value1;

                                                    if (y == 0)
                                                        DrawCell(xlSheet, cha1, ii, col, z, zz, lines[ii].workingTimes.Count * 3, string.Empty, string.Empty, string.Empty);
                                                    break;
                                                case 1:  //BTP
                                                    value1 = lines.Sum(x => x.LeanKH * x.CurrentLabors); // = lean KH * ld can bang chuyen
                                                    value2 = lines.Sum(x => x.workingTimes[y].BTP);
                                                    oRng.Value = value1;
                                                    oRng1.Value = value2;
                                                    oRng2.Value = value2 - value1;

                                                    if (y == 0)
                                                        DrawCell(xlSheet, cha1, ii, col, z, zz, lines[ii].workingTimes.Count * 3, string.Empty, lines.Sum(x => x.LK_BTP_InMonth), string.Empty);
                                                    break;
                                                case 2: //nhip chuyen
                                                    value1 = 0;
                                                    value2 = 0;
                                                    foreach (var item in lines)
                                                    {
                                                        value1 += Math.Round((((item.ProductionTime * 100) / item.HieuSuatNgay) / item.CurrentLabors), 1);
                                                        value2 += (item.workingTimes[y].KCS > 0 ? Math.Round(((double)(item.workingTimes[y].TimeEnd - item.workingTimes[y].TimeStart).TotalSeconds / item.workingTimes[y].KCS), 1) : 0);
                                                    }
                                                    value1 = value1 / lines.Count;
                                                    value2 = value2 / lines.Count;
                                                    oRng.Value = value1;
                                                    oRng1.Value = value2;
                                                    oRng2.Value = ((value1 > 0 && value2 > 0) ? Math.Round((value1 / value2) * 100) : 0) + "%";

                                                    if (y == 0)
                                                        DrawCell(xlSheet, cha1, ii, col, z, zz, lines[ii].workingTimes.Count * 3, string.Empty, string.Empty, string.Empty);
                                                    break;
                                                case 3: // ra chuyen
                                                    value1 = lines.Sum(x => x.workingTimes[y].NormsHour);
                                                    value2 = lines.Sum(x => x.workingTimes[y].KCS);
                                                    oRng.Value = value1;
                                                    oRng1.Value = value2;
                                                    oRng2.Value = ((value1 > 0 && value2 > 0) ? Math.Round((value2 / value1) * 100) : 0) + "%";

                                                    if (y == 0)
                                                    {
                                                        value1 = lines.Sum(x => x.ProductionPlansInMonth);
                                                        value2 = lines.Sum(x => x.LK_TH_InMonth);
                                                        DrawCell(xlSheet, cha1, ii, col, z, zz, lines[ii].workingTimes.Count * 3, value1, value2, ((value1 > 0 && value2 > 0) ? Math.Round((value2 / value1) * 100) : 0) + "%");
                                                    }
                                                    break;
                                                case 4: // doanh thu
                                                    value1 = lines.Sum(x => x.PriceCM * x.workingTimes[y].NormsHour);
                                                    value2 = lines.Sum(x => x.PriceCM * x.workingTimes[y].KCS);  // rachuyen TH * gia CM
                                                    oRng.Value = value1;
                                                    oRng1.Value = value2;
                                                    oRng2.Value = ((value1 > 0 && value2 > 0) ? Math.Round((value2 / value1) * 100) : 0) + "%";

                                                    if (y == 0)
                                                    {
                                                        value1 = lines.Sum(x => x.ProductionPlansInMonth * x.PriceCM);
                                                        value2 = lines.Sum(x => x.LK_TH_InMonth * x.PriceCM);
                                                        DrawCell(xlSheet, cha1, ii, col, z, zz, lines[ii].workingTimes.Count * 3, value1, value2, ((value1 > 0 && value2 > 0) ? Math.Round((value2 / value1) * 100) : 0) + "%");
                                                    }
                                                    break;
                                                case 5: // loi
                                                    oRng.Value = "";
                                                    oRng1.Value = lines.Sum(x => x.workingTimes[y].Error);
                                                    oRng2.Value = ((lines.Sum(x => x.workingTimes[y].Error) > 0 && lines.Sum(x => x.workingTimes[y].KCS) > 0) ? Math.Round((double)(lines.Sum(x => x.workingTimes[y].Error) / lines.Sum(x => x.workingTimes[y].KCS)) * 100) : 0) + "%"; //= error / kcs * 100
                                                    if (y == 0)
                                                        DrawCell(xlSheet, cha1, ii, col, z, zz, lines[ii].workingTimes.Count * 3, string.Empty, string.Empty, string.Empty);
                                                    break;
                                            }
                                            #endregion
                                        }
                                    }
                                    #endregion
                                }
                                col += 2;
                            }
                        }
                        #endregion
                        row += 6;
                    }

                    // tong xuong

                }

                oRng = xlSheet.get_Range("B5:B" + start, Convert.ToChar(socot + 65 + thoigianLV) + "5:" + Convert.ToChar(socot + 65 + thoigianLV) + start);
                oRng.HorizontalAlignment = Excel.Constants.xlCenter;
                oRng.VerticalAlignment = Excel.Constants.xlCenter;
                oRng.WrapText = true;
                #region

                #region Title
                //set thuoc tinh cho tieu de
                xlSheet.get_Range("B1", endChar + "1").Merge(false);
                Excel.Range caption = xlSheet.get_Range("B1", endChar + "1");
                caption.Select();
                caption.FormulaR1C1 = tieuDe + " - Ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " Năm " + DateTime.Now.Year; // "Báo Cáo Thông Tin Năng Suất Hàng Giờ của Xưởng - Ngày " + DateTime.Now.ToString("dd-MM-yyyy : hh:mm:ss");
                //căn lề cho tiêu đề
                caption.HorizontalAlignment = Excel.Constants.xlCenter;
                caption.Font.Bold = true;
                caption.VerticalAlignment = Excel.Constants.xlCenter;
                caption.Font.Size = 15;
                //màu nền cho tiêu đề
                caption.Interior.ColorIndex = 6;
                caption.RowHeight = 30;

                //Excel.Range v = xlSheet.get_Range("B" + ((lines.Count * 2) + 5), endChar + ((lines.Count * 2) + 5));
                //v.Interior.ColorIndex = 6;
                //v.Borders.ColorIndex = 56;

                #endregion

                //autofit độ rộng cho các cột 
                for (i = 0; i < sohang; i++)
                    ((Excel.Range)xlSheet.Cells[1, i + 1]).EntireColumn.AutoFit();

                //  DeleteAllFileInPath(path);
                //save file
                xlBook.SaveAs(path + fileName, Excel.XlFileFormat.xlWorkbookDefault, missValue, missValue, missValue, missValue, Excel.XlSaveAsAccessMode.xlNoChange, missValue, missValue, missValue, missValue, missValue);
                xlBook.Close(true, missValue, missValue);
                xlApp.Quit();

                // release cac doi tuong COM
                releaseObject(xlSheet);
                releaseObject(xlBook);
                releaseObject(xlApp);
                result = true;
                #endregion
            }
            catch (Exception ex)
            { }
            return result;
        }

        private static void DrawCell(Excel.Worksheet xlSheet, string cha1, int ii, int col, int z, int zz, int l, dynamic value1, dynamic value2, dynamic value3)
        {
            Excel.Range oRng3, oRng4, oRng5;
            cha1 = ConvertChar((l + 65 + col));
            oRng3 = xlSheet.get_Range(cha1 + (zz + z));
            SetBorder_TextAlign(oRng3, true);
            oRng3.Value = value1;

            cha1 = ConvertChar((l + 65 + col + 1));
            oRng4 = xlSheet.get_Range(cha1 + (zz + z));
            SetBorder_TextAlign(oRng4, true);
            oRng4.Value = value2;

            cha1 = ConvertChar((l + 65 + col + 2));
            oRng5 = xlSheet.get_Range(cha1 + (zz + z));
            SetBorder_TextAlign(oRng5, true);
            oRng5.Value = value3;
        }

        private static string ConvertChar(int number)
        {
            var cha = number > 90 ? "A" : "";
            number = number > 90 ? (number - 26) : number;
            cha += Convert.ToChar(number).ToString();
            return cha;
        }

        /// <summary>
        /// Report thien sơn
        /// </summary>
        /// <param name="tieuDe"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="lines"></param>
        /// <param name="timesGetNSInDay"></param>
        /// <returns></returns>
        public static bool ExportToExcel_ThienSon(string tieuDe, string path, string fileName, List<ChuyenSanPhamModel> lines, int timesGetNSInDay)
        {
            var result = false;
            try
            {
                #region khoi tao cac doi tuong Com Excel de lam viec
                Excel.Application xlApp;
                Excel.Worksheet xlSheet;
                Excel.Workbook xlBook;
                Excel.Range oRng;
                //doi tuong Trống để thêm  vào xlApp sau đó lưu lại sau
                object missValue = System.Reflection.Missing.Value;
                //khoi tao doi tuong Com Excel moi
                xlApp = new Excel.Application();
                xlBook = xlApp.Workbooks.Add(missValue);
                xlBook.CheckCompatibility = false;
                xlBook.DoNotPromptForConvert = true;
                //su dung Sheet dau tien de thao tac
                xlSheet = (Excel.Worksheet)xlBook.Worksheets.get_Item(1);
                //không cho hiện ứng dụng Excel lên để tránh gây đơ máy
                xlApp.Visible = false;

                #endregion
                #region header
                var headerArr = ("Chuyền,Số CN,Khách hàng,Mã Hàng,đơn Giá CM (USD),Chỉ tiêu").Split(',').ToArray();
                var subTitles = ("Cấp trong ngày,Tồn trên chuyền,Vốn trên chuyền,Kế hoạch,Kiểm đạt,Chênh lệch,Tỷ lệ % TH/KH,Kiểm đạt,Không đạt (Lỗi),LK Chưa kiểm,Tỷ lệ % KĐ/TH,Tỷ lệ % KĐ/KH").Split(',').ToArray();
                int socot = headerArr.Length;
                int sohang = 5;
                int i, j;
                string endChar = "", kytu = "";
                int start = 5;
                int thoigianLV = timesGetNSInDay, row = 5;
                Excel.Range EndRange;

                Excel.Range header = xlSheet.get_Range("B3", Convert.ToChar(socot + 65) + "3");
                header.Interior.ColorIndex = 24;
                header.Font.Bold = true;
                header.Font.Size = 12;
                header.HorizontalAlignment = Excel.Constants.xlCenter;
                header.VerticalAlignment = Excel.Constants.xlCenter;
                header.WrapText = true;

                for (int li = 0; li < lines.Count; li++)
                {
                    #region
                    for (int a = 0; a < socot; a++)
                    {
                        if (a == (socot - 1))
                        {
                            #region
                            kytu = Convert.ToChar((a + 1) + 65).ToString();
                            endChar = Convert.ToChar((a + 2) + 65).ToString();
                            if (li == 0)
                            {
                                // ve tieu de 
                                oRng = xlSheet.get_Range(kytu + "3:" + endChar + "4", kytu + "3:" + endChar + "4");
                                SetBorder_TextAlign(oRng, true);
                                oRng.Value = headerArr[a].ToUpper();
                                oRng.Interior.ColorIndex = 24;
                            }
                            oRng = xlSheet.get_Range((kytu + (row + li)) + ":" + (kytu + (row + 2 + li)), (kytu + (row + li)) + ":" + (kytu + (row + 2 + li)));
                            SetBorder_TextAlign(oRng, true);
                            oRng.Interior.ColorIndex = 24;
                            oRng.Value = ("BÁN THÀNH PHẨM (Pcs)").ToUpper();

                            oRng = xlSheet.get_Range((kytu + (row + li + 3)) + ":" + (kytu + (row + 6 + li)), (kytu + (row + li + 3)) + ":" + (kytu + (row + 6 + li)));
                            SetBorder_TextAlign(oRng, true);
                            oRng.Interior.ColorIndex = 24;
                            oRng.Value = ("SẢN LƯỢNG (Pcs)").ToUpper();

                            oRng = xlSheet.get_Range((kytu + (row + li + 7)) + ":" + (kytu + (row + 11 + li)), (kytu + (row + li + 7)) + ":" + (kytu + (row + 11 + li)));
                            SetBorder_TextAlign(oRng, true);
                            oRng.Interior.ColorIndex = 24;
                            oRng.Value = ("KCS (Pcs)");

                            oRng = xlSheet.get_Range((kytu + (row + li + subTitles.Length)) + ":" + (endChar + (row + subTitles.Length + li)), (kytu + (row + li + subTitles.Length)) + ":" + (endChar + (row + subTitles.Length + li)));
                            SetBorder_TextAlign(oRng, true);
                            //  oRng.Interior.ColorIndex = 24;
                            oRng.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(242, 220, 219));
                            oRng.Font.ColorIndex = 3;
                            oRng.Value = ("Doanh thu (USD)");

                            for (int s = 0; s < subTitles.Length; s++)
                            {
                                oRng = xlSheet.get_Range((endChar + (row + li + s)));
                                SetBorder_TextAlign(oRng, false);
                                oRng.Interior.ColorIndex = 24;
                                oRng.Value = subTitles[s];

                                ///
                                #region
                                for (int c = 1; c < 4; c++)
                                {
                                    kytu = ConvertChar((65 + socot + c + 1));
                                    if (li == 0)
                                    {
                                        oRng = xlSheet.get_Range(kytu + "4");
                                        SetBorder_TextAlign(oRng, true);
                                        oRng.Interior.ColorIndex = 24;
                                        switch (c)
                                        {
                                            case 1: oRng.Value = ("đã Kiểm đạt").ToUpper(); break;
                                            case 2: oRng.Value = ("lũy kế").ToUpper(); break;
                                            case 3: oRng.Value = ("còn lại").ToUpper(); break;
                                        }
                                    }

                                    oRng = xlSheet.get_Range(kytu + (row + li + s));
                                    SetBorder_TextAlign(oRng, true);
                                    oRng.Font.ColorIndex = 3;
                                    switch (s)
                                    {
                                        case 0:
                                            switch (c)
                                            {
                                                case 0: oRng.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(242, 220, 219)); break;
                                                    //case 1:
                                                    //    oRng.Value = ((lines[li].LK_BTP - lines[li].LK_BTP_G) - (lines[li].BTP_Day - lines[li].BTP_Day_G));
                                                    //    //  oRng.Interior.ColorIndex = 7;
                                                    //    oRng.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(242, 220, 219));
                                                    //    break;
                                                    //case 2: oRng.Value = (lines[li].LK_BTP - lines[li].LK_BTP_G); break;
                                                    //case 3: oRng.Value = (lines[li].SanLuongKeHoach - (lines[li].LK_BTP - lines[li].LK_BTP_G)); break;
                                            }
                                            break;
                                        #region
                                        case 3:
                                            if (c == 1)
                                            {
                                                //  oRng.Value = lines[li].SanLuongKeHoach;
                                                // oRng.Interior.ColorIndex = 10;
                                                oRng.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(0, 176, 80));
                                            }
                                            break;
                                        case 4:
                                            switch (c)
                                            {
                                                case 1:
                                                    // oRng.Value = (lines[li].LuyKeBTPThoatChuyen - (lines[li].TC_Day - lines[li].TC_Day_G));
                                                    // oRng.Interior.ColorIndex = 7;
                                                    oRng.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(242, 220, 219));
                                                    break;
                                                    //   case 2: oRng.Value = lines[li].LuyKeBTPThoatChuyen; break;
                                                    //  case 3: oRng.Value = (lines[li].SanLuongKeHoach - lines[li].LuyKeBTPThoatChuyen); break;
                                            }
                                            break;
                                        //case 5:
                                        //    if (c == 1)
                                        //        oRng.Value = (lines[li].SanLuongKeHoach - (lines[li].LuyKeBTPThoatChuyen - (lines[li].TC_Day - lines[li].TC_Day_G)));
                                        //    break;
                                        //case 6:
                                        //    if (c == 1)
                                        //        oRng.Value = ((lines[li].LuyKeBTPThoatChuyen - (lines[li].TC_Day - lines[li].TC_Day_G)) > 0 ? Math.Round((((lines[li].LuyKeBTPThoatChuyen - (lines[li].TC_Day - lines[li].TC_Day_G)) / (double)lines[li].SanLuongKeHoach) * 100)) : 0) + "%";
                                        //    break;
                                        #endregion
                                        #region
                                        case 7:
                                            switch (c)
                                            {
                                                case 1:
                                                    //  oRng.Value = (lines[li].LuyKeTH - (lines[li].TH_Day - lines[li].TH_Day_G));
                                                    //  oRng.Interior.ColorIndex = 7;
                                                    oRng.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(242, 220, 219));
                                                    break;
                                                    //   case 2: oRng.Value = lines[li].LuyKeTH; break;
                                                    //  case 3: oRng.Value = (lines[li].SanLuongKeHoach - lines[li].LuyKeTH); break;
                                            }
                                            break;
                                        case 9:
                                            switch (c)
                                            {
                                                case 1:
                                                    // oRng.Value = (lines[li].LuyKeBTPThoatChuyen - (lines[li].TC_Day - lines[li].TC_Day_G)) - (lines[li].LuyKeTH - (lines[li].TH_Day - lines[li].TH_Day_G));
                                                    //  oRng.Interior.ColorIndex = 7;
                                                    oRng.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(242, 220, 219));
                                                    break;

                                                    // case 3: oRng.Value = (lines[li].SanLuongKeHoach - (lines[li].LuyKeBTPThoatChuyen - (lines[li].TC_Day - lines[li].TC_Day_G))); break;
                                            }
                                            break;
                                            //case 10:
                                            //    if (c == 1)
                                            //        oRng.Value = (((lines[li].LuyKeTH - (lines[li].TH_Day - lines[li].TH_Day_G)) > 0 && (lines[li].LuyKeBTPThoatChuyen - (lines[li].TC_Day - lines[li].TC_Day_G)) > 0) ? Math.Round(((lines[li].LuyKeTH - (lines[li].TH_Day - lines[li].TH_Day_G)) / (double)(lines[li].LuyKeBTPThoatChuyen - (lines[li].TC_Day - lines[li].TC_Day_G))) * 100) : 0) + "%";
                                            //    break;
                                            //case 11:
                                            //    if (c == 1)
                                            //        oRng.Value = ((lines[li].LuyKeTH - (lines[li].TH_Day - lines[li].TH_Day_G)) > 0 ? Math.Round((((lines[li].LuyKeTH - (lines[li].TH_Day - lines[li].TH_Day_G)) / (double)lines[li].SanLuongKeHoach) * 100)) : 0) + "%";
                                            //    break;
                                            #endregion
                                    }

                                    if (s == 0)
                                    {
                                        #region doanh thu
                                        oRng = xlSheet.get_Range(kytu + (row + li + subTitles.Length));
                                        SetBorder_TextAlign(oRng, true);
                                        oRng.Font.ColorIndex = 3;

                                        switch (c)
                                        {
                                            //   case 1: oRng.Value = Math.Round(((lines[li].LuyKeBTPThoatChuyen - (lines[li].TC_Day - lines[li].TC_Day_G)) * lines[li].PriceCM), 1); break;
                                            case 2:
                                                //  oRng.Value = Math.Round((lines[li].LuyKeBTPThoatChuyen * lines[li].PriceCM), 1);
                                                //  oRng.Interior.ColorIndex = 7;
                                                oRng.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(196, 215, 155));
                                                break;
                                            case 3:
                                                //  oRng.Value = Math.Round(((lines[li].SanLuongKeHoach - lines[li].LuyKeBTPThoatChuyen) * lines[li].PriceCM), 1);
                                                //  oRng.Interior.ColorIndex = 7;
                                                oRng.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(196, 215, 155));
                                                break;
                                        }
                                        #endregion
                                    }
                                }
                                #endregion

                                ///
                                #region
                                int so = socot + 5;
                                int vl = 0;
                                if (lines[0] != null && lines[0].workingTimes != null && lines[0].workingTimes.Count > 0)
                                {
                                    string name = string.Empty;
                                    for (int y = 0; y < lines[0].workingTimes.Count; y++)
                                    {
                                        var show = lines[0].workingTimes[y].TimeEnd < DateTime.Now.TimeOfDay ? true : false;
                                        kytu = ConvertChar((y + 65 + so));
                                        var newChar = ConvertChar((thoigianLV + 65 + so));
                                        if (li == 0)
                                        {
                                            name = lines[0].workingTimes[y].TimeStart.Hours + "h" + lines[0].workingTimes[y].TimeStart.Minutes + " - " + lines[0].workingTimes[y].TimeEnd.Hours + "h" + lines[0].workingTimes[y].TimeEnd.Minutes;
                                            oRng = xlSheet.get_Range(kytu + "4");
                                            SetBorder_TextAlign(oRng, true);
                                            oRng.Interior.ColorIndex = 24;
                                            oRng.Value = name;

                                            oRng = xlSheet.get_Range(newChar + "4");
                                            SetBorder_TextAlign(oRng, true);
                                            oRng.Interior.ColorIndex = 24;
                                            oRng.Value = "Tổng Ngày";
                                        }

                                        xlSheet.Cells[1, 1] = "";

                                        oRng = xlSheet.get_Range(kytu + (row + li + s));
                                        SetBorder_TextAlign(oRng, false);

                                        EndRange = xlSheet.get_Range(newChar + (row + li + s));
                                        SetBorder_TextAlign(EndRange, true);

                                        switch (s)
                                        {
                                            case 0: oRng.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(242, 220, 219)); break;
                                            //case 1:
                                            //    if (show)
                                            //     oRng.Value = lines[li].workingTimes[y].BTPInLine; 
                                            //    break;
                                            //case 2: if (show) oRng.Value = lines[li].workingTimes[y].Lean; break;
                                            case 3:
                                                //if (show)
                                                //    oRng.Value = lines[li].workingTimes[y].NormsHour;

                                                //EndRange.Value = lines[li].NormsDay;
                                                // EndRange.Interior.ColorIndex = 10;
                                                EndRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(0, 176, 80));
                                                break;
                                            case 4:
                                                // if (show)
                                                //    oRng.Value = lines[li].workingTimes[y].TC;

                                                oRng.Font.ColorIndex = 3;
                                                //  oRng.Interior.ColorIndex = 7;
                                                oRng.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(242, 220, 219));

                                                EndRange.Value = lines[li].TC_Day - lines[li].TC_Day_G;
                                                break;
                                            case 5:
                                                //if (show)
                                                //{
                                                //    vl = lines[li].workingTimes[y].TC - (int)lines[li].workingTimes[y].NormsHour;
                                                //    oRng.Value = vl;
                                                //    oRng.Font.Color = ColorTranslator.ToOle(Color.FromArgb(192, 58, 0));
                                                //    if (vl < 0)
                                                //        //  oRng.Interior.ColorIndex = 7;
                                                //        oRng.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(255, 255, 0));
                                                //}

                                                //  vl = (lines[li].TC_Day - lines[li].TC_Day_G) - (int)lines[li].NormsDay;
                                                //  EndRange.Value = vl;
                                                EndRange.Font.Color = ColorTranslator.ToOle(Color.FromArgb(192, 58, 0));
                                                //   if (vl < 0)
                                                //  oRng.Interior.ColorIndex = 7;
                                                //     EndRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(255, 255, 0));
                                                break;
                                            //case 6:
                                            //   if (show)
                                            //        oRng.Value = (lines[li].workingTimes[y].TC > 0 ? Math.Round((lines[li].workingTimes[y].TC / lines[li].workingTimes[y].NormsHour) * 100) : 0) + "%";
                                            //     EndRange.Value = ((lines[li].TC_Day - lines[li].TC_Day_G) > 0 ? Math.Round(((lines[li].TC_Day - lines[li].TC_Day_G) / lines[li].NormsDay) * 100) : 0) + "%";
                                            //    break;
                                            case 7:
                                                //  if (show)
                                                //      oRng.Value = lines[li].workingTimes[y].KCS;
                                                //  oRng.Interior.ColorIndex = 7;
                                                oRng.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(242, 220, 219));
                                                oRng.Font.ColorIndex = 3;

                                                //  EndRange.Value = lines[li].TH_Day - lines[li].TH_Day_G;
                                                break;
                                            case 8:
                                                //   if (show)
                                                //      oRng.Value = lines[li].workingTimes[y].Error;

                                                // oRng.Interior.ColorIndex = 7;
                                                oRng.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(242, 220, 219));
                                                oRng.Font.ColorIndex = 3;

                                                // EndRange.Value = lines[li].Err_Day - lines[li].Err_Day_G;
                                                break;
                                                //case 9:
                                                //    if (show)
                                                //    {
                                                //        vl = lines[li].workingTimes.Where(x => x.TimeEnd <= lines[li].workingTimes[y].TimeEnd).Sum(x => x.TC);
                                                //        vl -= lines[li].workingTimes.Where(x => x.TimeEnd <= lines[li].workingTimes[y].TimeEnd).Sum(x => x.KCS);
                                                //        oRng.Value = vl;
                                                //        oRng.Font.ColorIndex = 3;
                                                //    }
                                                //    break;
                                                //case 10:
                                                //    if (show)
                                                //        oRng.Value = ((lines[li].workingTimes[y].TC > 0 && lines[li].workingTimes[y].KCS > 0) ? Math.Round(((lines[li].workingTimes[y].KCS / (double)lines[li].workingTimes[y].TC) * 100)) : 0) + "%";
                                                //    EndRange.Value = (((lines[li].TC_Day - lines[li].TC_Day_G) > 0 && (lines[li].TH_Day - lines[li].TH_Day_G) > 0) ? Math.Round((((lines[li].TH_Day - lines[li].TH_Day_G) / (double)(lines[li].TC_Day - lines[li].TC_Day_G)) * 100)) : 0) + "%";
                                                //    break;
                                                //case 11:
                                                //    if (show)
                                                //        oRng.Value = ((lines[li].workingTimes[y].KCS > 0) ? Math.Round(((lines[li].workingTimes[y].KCS / (double)lines[li].workingTimes[y].NormsHour) * 100)) : 0) + "%";
                                                //    EndRange.Value = ((lines[li].TH_Day - lines[li].TH_Day_G) > 0 ? Math.Round((((lines[li].TH_Day - lines[li].TH_Day_G) / (double)lines[li].NormsDay) * 100)) : 0) + "%";
                                                //    break;
                                        }

                                        //if (show)
                                        //{
                                        //    oRng = xlSheet.get_Range(kytu + (row + li + subTitles.Length));
                                        //    SetBorder_TextAlign(oRng, true);
                                        //    oRng.Font.ColorIndex = 3;
                                        //    oRng.Value = lines[li].workingTimes[y].KCS * lines[li].PriceCM;
                                        //}

                                        EndRange = xlSheet.get_Range(newChar + (row + li + subTitles.Length));
                                        SetBorder_TextAlign(EndRange, true);
                                        EndRange.Font.ColorIndex = 3;
                                        //  EndRange.Value = (lines[li].TH_Day - lines[li].TH_Day_G) * lines[li].PriceCM;
                                    }
                                }
                                #endregion
                            }
                            #endregion
                        }
                        else
                        {
                            #region
                            kytu = Convert.ToChar((a + 1) + 65).ToString();
                            if (li == 0)
                            {
                                // ve tieu de 
                                oRng = xlSheet.get_Range(kytu + "3:" + kytu + "4", kytu + "3:" + kytu + "4");
                                SetBorder_TextAlign(oRng, true);
                                oRng.Value = headerArr[a].ToUpper();
                                oRng.Interior.ColorIndex = 24;
                            }
                            oRng = xlSheet.get_Range((kytu + (row + li)) + ":" + (kytu + (row + subTitles.Length + li)), (kytu + (row + li)) + ":" + (kytu + (row + subTitles.Length + li)));
                            SetBorder_TextAlign(oRng, true);
                            //switch (a)
                            //{
                            //    case 0: oRng.Value = lines[li].LineName.ToUpper(); break;
                            //    case 1: oRng.Value = lines[li].CurrentLabors; break;
                            //    case 2: oRng.Value = string.Empty; break;
                            //    case 3: oRng.Value = lines[li].CommoName.ToUpper(); break;
                            //    case 4: oRng.Value = lines[li].PriceCM; break;
                            //}
                            #endregion
                        }
                    }
                    #endregion

                    #region
                    if (li == 0)
                    {
                        kytu = ConvertChar(socot + 2 + 65);
                        endChar = ConvertChar(socot + 4 + 65);
                        oRng = xlSheet.get_Range(kytu + "3", endChar + "3");
                        SetBorder_TextAlign(oRng, true);
                        oRng.Value = "SẢN LƯỢNG";
                        oRng.Interior.ColorIndex = 24;

                        kytu = ConvertChar(socot + 5 + 65);
                        endChar = ConvertChar(socot + (5 + thoigianLV) + 65);
                        oRng = xlSheet.get_Range(kytu + "3", endChar + "3");
                        oRng.Select();
                        oRng.Merge();
                        oRng.Value = "SẢN LƯỢNG THEO GIỜ";
                        SetBorder_TextAlign(oRng, true);
                        oRng.Interior.ColorIndex = 24;
                    }

                    #endregion
                    row += subTitles.Length;
                }
                #endregion

                oRng = xlSheet.get_Range("B6:B" + start, Convert.ToChar(socot + 65 + thoigianLV) + "6:" + Convert.ToChar(socot + 65 + thoigianLV) + start);
                oRng.HorizontalAlignment = Excel.Constants.xlCenter;
                oRng.VerticalAlignment = Excel.Constants.xlCenter;
                oRng.WrapText = true;
                #region

                #region Title
                //set thuoc tinh cho tieu de
                endChar = ConvertChar(socot + (5 + thoigianLV) + 65);
                xlSheet.get_Range("B1", endChar + "1").Merge(false);
                Excel.Range caption = xlSheet.get_Range("B1", endChar + "1");
                caption.Select();
                caption.FormulaR1C1 = tieuDe.ToUpper();
                caption.Font.Bold = true;
                caption.Font.Size = 16;
                caption.HorizontalAlignment = Excel.Constants.xlCenter;
                caption.VerticalAlignment = Excel.Constants.xlCenter;
                caption.WrapText = true;
                caption.RowHeight = 20;

                xlSheet.get_Range("B2", endChar + "2").Merge(false);
                caption = xlSheet.get_Range("B2", endChar + "2");
                caption.Select();
                caption.FormulaR1C1 = ("Ngày " + DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year).ToUpper();
                caption.Font.Bold = true;
                caption.Font.Size = 12;
                caption.HorizontalAlignment = Excel.Constants.xlCenter;
                caption.VerticalAlignment = Excel.Constants.xlCenter;
                caption.WrapText = true;

                #endregion

                //autofit độ rộng cho các cột 
                for (i = 0; i < sohang; i++)
                    ((Excel.Range)xlSheet.Cells[1, i + 1]).EntireColumn.AutoFit();

                //save file
                xlBook.SaveAs(path + fileName, Excel.XlFileFormat.xlWorkbookDefault, missValue, missValue, missValue, missValue, Excel.XlSaveAsAccessMode.xlNoChange, missValue, missValue, missValue, missValue, missValue);
                xlBook.Close(true, missValue, missValue);
                xlApp.Quit();

                // release cac doi tuong COM
                releaseObject(xlSheet);
                releaseObject(xlBook);
                releaseObject(xlApp);
                result = true;
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tạo file mail bị lỗi.\n" + ex.Message);
            }
            return result;
        }


        public static bool ExportToExcel_ThienSon_Edit(string tieuDe, string path, string templateName, string fileName, List<ChuyenSanPhamModel> lines, int timesGetNSInDay, DateTime date)
        {
            var result = false;
            try
            {
                #region khoi tao cac doi tuong Com Excel de lam viec
                Excel.Application xlApp;
                Excel.Worksheet xlSheet;
                Excel.Workbook xlBook;
                Excel.Range oRng;
                //doi tuong Trống để thêm  vào xlApp sau đó lưu lại sau
                object missValue = System.Reflection.Missing.Value;
                //khoi tao doi tuong Com Excel moi
                xlApp = new Excel.Application();
                string templatePath = System.Windows.Forms.Application.StartupPath + @"\Report\Template\" + templateName;
                xlBook = xlApp.Workbooks.Open(templatePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                xlBook.CheckCompatibility = false;
                xlBook.DoNotPromptForConvert = true;

                //su dung Sheet dau tien de thao tac
                xlSheet = (Excel.Worksheet)xlBook.Worksheets.get_Item(1);
                //không cho hiện ứng dụng Excel lên để tránh gây đơ máy
                xlApp.Visible = false;

                #endregion
                #region header
                //  var headerArr = ("Chuyền,Số CN,Khách hàng,Mã Hàng,đơn Giá CM (USD),Chỉ tiêu").Split(',').ToArray();
                //  var subTitles = ("Cấp trong ngày,Tồn trên chuyền,Vốn trên chuyền,Kế hoạch,Kiểm đạt,Chênh lệch,Tỷ lệ % TH/KH,Kiểm đạt,Không đạt (Lỗi),LK Chưa kiểm,Tỷ lệ % KĐ/TH,Tỷ lệ % KĐ/KH").Split(',').ToArray();
                int so_cot_tieu_de = 6;// headerArr.Length;
                int sohang = 5;
                int i, j;
                string endChar = "", kytu = "";
                int start = 5;
                int thoigianLV = timesGetNSInDay, row = 5,
                    so_dong_thong_tin_1_chuyen = 17;
                bool checkTime = date.Day != DateTime.Now.Day ? false : true;
                Excel.Range EndRange;

                Excel.Range header = xlSheet.get_Range("B3", Convert.ToChar(so_cot_tieu_de + 65) + "3");

                if (lines != null && lines.Count > 0)
                {
                    for (int li = 0; li < lines.Count; li++)
                    {
                        #region
                        for (int a = 0; a < so_cot_tieu_de; a++)
                        {
                            if (a == (so_cot_tieu_de - 1))
                            {
                                #region
                                kytu = Convert.ToChar((a + 1) + 65).ToString();
                                endChar = Convert.ToChar((a + 2) + 65).ToString();

                                for (int s = 0; s < so_dong_thong_tin_1_chuyen; s++)
                                {
                                    ///
                                    #region sl tong hop
                                    for (int c = 1; c < 4; c++)
                                    {
                                        kytu = ConvertChar((65 + so_cot_tieu_de + c + 1));
                                        oRng = xlSheet.get_Range(kytu + (row + s));
                                        switch (s)
                                        {
                                            case 0:
                                                switch (c)
                                                {
                                                    case 1:
                                                        oRng.Value = ((lines[li].LK_BTP - lines[li].LK_BTP_G) - (lines[li].BTP_Day - lines[li].BTP_Day_G));
                                                        break;
                                                    case 2: oRng.Value = (lines[li].LK_BTP - lines[li].LK_BTP_G); break;
                                                    case 3: oRng.Value = (lines[li].SanLuongKeHoach - (lines[li].LK_BTP - lines[li].LK_BTP_G)); break;
                                                }
                                                break;
                                            #region TC Box
                                            case 3:
                                                if (c == 1)
                                                    oRng.Value = lines[li].SanLuongKeHoach;
                                                break;
                                            case 4:
                                                switch (c)
                                                {
                                                    case 1:
                                                        oRng.Value = (lines[li].LuyKeBTPThoatChuyen - (lines[li].TC_Day - lines[li].TC_Day_G));
                                                        break;
                                                    case 2: oRng.Value = lines[li].LuyKeBTPThoatChuyen; break;
                                                    case 3: oRng.Value = (lines[li].SanLuongKeHoach - lines[li].LuyKeBTPThoatChuyen); break;
                                                }
                                                break;
                                            case 5:
                                                if (c == 1)
                                                    oRng.Value = (lines[li].SanLuongKeHoach - (lines[li].LuyKeBTPThoatChuyen - (lines[li].TC_Day - lines[li].TC_Day_G)));
                                                break;
                                            case 6:
                                                if (c == 1)
                                                {
                                                    int lktruocHomNay = lines[li].LuyKeBTPThoatChuyen - (lines[li].TC_Day - lines[li].TC_Day_G);
                                                    double tile = 0;
                                                    if (lktruocHomNay > 0)
                                                        tile = Math.Round((lktruocHomNay / (double)lines[li].SanLuongKeHoach) * 100, 2);

                                                    //  oRng.Value = (() > 0 ? Math.Round((((lines[li].LuyKeBTPThoatChuyen - (lines[li].TC_Day - lines[li].TC_Day_G)) / lines[li].SanLuongKeHoach) * 100)) : 0) + "%";
                                                    oRng.Value = tile + "%";
                                                }
                                                break;
                                            #endregion

                                            #region  KCS box
                                            case 8:  //kiem dat
                                                switch (c)
                                                {
                                                    case 1:
                                                        oRng.Value = (lines[li].LuyKeTH - (lines[li].TH_Day - lines[li].TH_Day_G));
                                                        break;
                                                    case 2: oRng.Value = lines[li].LuyKeTH; break;
                                                    case 3: oRng.Value = (lines[li].SanLuongKeHoach - lines[li].LuyKeTH); break;
                                                }
                                                break;
                                            case 10: //lk chua kiem
                                                switch (c)
                                                {
                                                    case 1:
                                                        oRng.Value = (lines[li].LuyKeBTPThoatChuyen - (lines[li].TC_Day - lines[li].TC_Day_G)) - (lines[li].LuyKeTH - (lines[li].TH_Day - lines[li].TH_Day_G));
                                                        break;

                                                    case 3: oRng.Value = (lines[li].SanLuongKeHoach - (lines[li].LuyKeBTPThoatChuyen - (lines[li].TC_Day - lines[li].TC_Day_G))); break;
                                                }
                                                break;
                                            case 11: // Tỷ lệ % KĐ/TH
                                                if (c == 1)
                                                    oRng.Value = (((lines[li].LuyKeTH - (lines[li].TH_Day - lines[li].TH_Day_G)) > 0 && (lines[li].LuyKeBTPThoatChuyen - (lines[li].TC_Day - lines[li].TC_Day_G)) > 0) ? Math.Round(((lines[li].LuyKeTH - (lines[li].TH_Day - lines[li].TH_Day_G)) / (double)(lines[li].LuyKeBTPThoatChuyen - (lines[li].TC_Day - lines[li].TC_Day_G))) * 100) : 0) + "%";
                                                break;
                                            case 12: //Tỷ lệ % KĐ/KH
                                                if (c == 1)
                                                    oRng.Value = ((lines[li].LuyKeTH - (lines[li].TH_Day - lines[li].TH_Day_G)) > 0 ? Math.Round((((lines[li].LuyKeTH - (lines[li].TH_Day - lines[li].TH_Day_G)) / (double)lines[li].SanLuongKeHoach) * 100)) : 0) + "%";
                                                break;
                                            #endregion

                                            case 16: //giao hoan thanh
                                                switch (c)
                                                {
                                                    case 2: oRng.Value = (lines[li].lkCongDoan == null ? 0 : lines[li].lkCongDoan); break;
                                                    case 3: oRng.Value = lines[li].SanLuongKeHoach - (lines[li].lkCongDoan == null ? 0 : lines[li].lkCongDoan); break;
                                                        // case 3: oRng.Value = (lines[li].SanLuongKeHoach - (lines[li].LuyKeBTPThoatChuyen - (lines[li].TC_Day - lines[li].TC_Day_G))); break;
                                                }
                                                break;
                                        }

                                        if (s == 0)
                                        {
                                            #region doanh thu
                                            //   oRng = xlSheet.get_Range(kytu + (row + li + subTitles.Length));
                                            oRng = xlSheet.get_Range(kytu + (row + so_dong_thong_tin_1_chuyen));
                                            oRng.Font.ColorIndex = 3;

                                            switch (c)
                                            {
                                                case 1:
                                                    oRng.Value = "dT ngay truoc";// Math.Round(((lines[li].LuyKeBTPThoatChuyen - (lines[li].TC_Day - lines[li].TC_Day_G)) * lines[li].PriceCM), 1);
                                                    break;
                                                case 2:
                                                    oRng.Value = "DT tong";// Math.Round((lines[li].LuyKeBTPThoatChuyen * lines[li].PriceCM), 1);
                                                    break;
                                                case 3:
                                                    oRng.Value = "DT con lai";// Math.Round(((lines[li].SanLuongKeHoach - lines[li].LuyKeBTPThoatChuyen) * lines[li].PriceCM), 1);
                                                    break;
                                            }
                                            #endregion
                                        }
                                    }
                                    #endregion

                                    ///
                                    #region ns gio
                                    int so = so_cot_tieu_de + 5;
                                    int vl = 0, TCtoNow = 0, KCSToNow = 0;
                                    if (lines[0] != null && lines[0].workingTimes != null && lines[0].workingTimes.Count > 0)
                                    {
                                        string name = string.Empty;
                                        for (int y = 0; y < lines[0].workingTimes.Count; y++)
                                        {
                                            TCtoNow += lines[li].workingTimes[y].TC;
                                            KCSToNow += lines[li].workingTimes[y].KCS;

                                            var workTimeToNow = (lines[0].workingTimes[y].TimeEnd - lines[0].workingTimes[y].TimeStart).TotalMinutes * (y + 1);
                                            var show = checkTime ? (lines[0].workingTimes[y].TimeEnd < DateTime.Now.TimeOfDay ? true : false) : true;

                                            kytu = ConvertChar((y + 65 + so));
                                            var newChar = ConvertChar((lines[0].workingTimes.Count + 65 + so));

                                            xlSheet.Cells[1, 1] = "";

                                            if (li == 0)
                                            {
                                                oRng = xlSheet.get_Range(kytu + "4");
                                                oRng.Value = string.Format("{0}h:{1} - {2}h:{3}", lines[0].workingTimes[y].TimeStart.Hours, lines[0].workingTimes[y].TimeStart.Minutes, lines[0].workingTimes[y].TimeEnd.Hours, lines[0].workingTimes[y].TimeEnd.Minutes);
                                            }

                                            EndRange = xlSheet.get_Range(newChar + (row + s));
                                            oRng = xlSheet.get_Range(kytu + (row + s));
                                            #region
                                            switch (s)
                                            {
                                                case 0:
                                                    if (show) oRng.Value = lines[li].workingTimes[y].BTP;
                                                    EndRange.Value = lines[li].BTP_Day - lines[li].BTP_Day_G;
                                                    break;
                                                case 1:
                                                    if (show)
                                                        oRng.Value = lines[li].workingTimes[y].BTPInLine;
                                                    break;
                                                case 2: if (show) oRng.Value = lines[li].workingTimes[y].Lean; break;
                                                case 3:
                                                    if (show)
                                                        oRng.Value = lines[li].workingTimes[y].NormsHour;

                                                    EndRange.Value = lines[li].NormsDay;
                                                    break;
                                                case 4:
                                                    if (show)
                                                        oRng.Value = lines[li].workingTimes[y].TC;
                                                    EndRange.Value = lines[li].TC_Day - lines[li].TC_Day_G;
                                                    break;
                                                case 5:
                                                    if (show)
                                                    {
                                                        vl = lines[li].workingTimes[y].TC - (int)lines[li].workingTimes[y].NormsHour;
                                                        oRng.Value = vl;
                                                        oRng.Font.Color = ColorTranslator.ToOle(Color.FromArgb(192, 58, 0));
                                                        if (vl < 0)
                                                            oRng.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(255, 255, 0));
                                                    }

                                                    vl = (lines[li].TC_Day - lines[li].TC_Day_G) - (int)lines[li].NormsDay;
                                                    EndRange.Value = vl;
                                                    EndRange.Font.Color = ColorTranslator.ToOle(Color.FromArgb(192, 58, 0));
                                                    if (vl < 0)
                                                        EndRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(255, 255, 0));
                                                    break;
                                                case 6:
                                                    if (show)
                                                        oRng.Value = (lines[li].workingTimes[y].TC > 0 && lines[li].workingTimes[y].NormsHour > 0 ? Math.Round((lines[li].workingTimes[y].TC / lines[li].workingTimes[y].NormsHour) * 100) : 0) + "%";
                                                    EndRange.Value = ((lines[li].TC_Day - lines[li].TC_Day_G) > 0 && lines[li].NormsDay > 0 ? Math.Round(((lines[li].TC_Day - lines[li].TC_Day_G) / lines[li].NormsDay) * 100) : 0) + "%";
                                                    break;

                                                case 7:
                                                    if (show)
                                                    {
                                                        //Hiệu suất = (Tổng sản lượng ra chuyền X thời gian chế tạo) : Số lao động X thời gian làm việc thực tế. 
                                                        var hieusuat = ((TCtoNow * Math.Round((lines[li].ProductionTime * 100) / lines[li].HieuSuatNgay)) / (lines[li].CurrentLabors * (workTimeToNow * 60)));
                                                        if (double.IsInfinity(hieusuat))
                                                            hieusuat = 0;
                                                        oRng.Value = Math.Round(hieusuat * 100, 1) + "%";
                                                    }
                                                    //  EndRange.Value = ((lines[li].TC_Day - lines[li].TC_Day_G) > 0 && lines[li].NormsDay > 0 ? Math.Round(((lines[li].TC_Day - lines[li].TC_Day_G) / lines[li].NormsDay) * 100) : 0) + "%";
                                                    break;

                                                case 8:
                                                    if (show)
                                                        oRng.Value = lines[li].workingTimes[y].KCS;
                                                    EndRange.Value = lines[li].TH_Day - lines[li].TH_Day_G;
                                                    break;
                                                case 9:
                                                    if (show)
                                                        oRng.Value = lines[li].workingTimes[y].Error;
                                                    EndRange.Value = lines[li].Err_Day - lines[li].Err_Day_G;
                                                    break;
                                                case 10:
                                                    if (show)
                                                    {
                                                        vl = lines[li].workingTimes.Where(x => x.TimeEnd <= lines[li].workingTimes[y].TimeEnd).Sum(x => x.TC);
                                                        vl -= lines[li].workingTimes.Where(x => x.TimeEnd <= lines[li].workingTimes[y].TimeEnd).Sum(x => x.KCS);
                                                        oRng.Value = vl;
                                                    }
                                                    break;
                                                case 11:
                                                    if (show)
                                                        oRng.Value = ((lines[li].workingTimes[y].TC > 0 && lines[li].workingTimes[y].KCS > 0) ? Math.Round(((lines[li].workingTimes[y].KCS / (double)lines[li].workingTimes[y].TC) * 100)) : 0) + "%";
                                                    EndRange.Value = (((lines[li].TC_Day - lines[li].TC_Day_G) > 0 && (lines[li].TH_Day - lines[li].TH_Day_G) > 0) ? Math.Round((((lines[li].TH_Day - lines[li].TH_Day_G) / (double)(lines[li].TC_Day - lines[li].TC_Day_G)) * 100)) : 0) + "%";
                                                    break;
                                                case 12:
                                                    if (show)
                                                        oRng.Value = ((lines[li].workingTimes[y].KCS > 0 && lines[li].workingTimes[y].NormsHour > 0) ? Math.Round(((lines[li].workingTimes[y].KCS / (double)lines[li].workingTimes[y].NormsHour) * 100)) : 0) + "%";
                                                    EndRange.Value = ((lines[li].TH_Day - lines[li].TH_Day_G) > 0 && lines[li].NormsDay > 0 ? Math.Round((((lines[li].TH_Day - lines[li].TH_Day_G) / (double)lines[li].NormsDay) * 100)) : 0) + "%";
                                                    break;
                                                case 13:
                                                    if (show)
                                                    {
                                                        //Hiệu suất = (Tổng sản lượng ra chuyền X thời gian chế tạo) : Số lao động X thời gian làm việc thực tế. 
                                                        var hieusuat = ((KCSToNow * Math.Round((lines[li].ProductionTime * 100) / lines[li].HieuSuatNgay)) / (lines[li].CurrentLabors * (workTimeToNow * 60)));
                                                        if (double.IsInfinity(hieusuat))
                                                            hieusuat = 0;
                                                        oRng.Value = Math.Round(hieusuat * 100, 1) + "%";
                                                    }
                                                    break;
                                                case 14:
                                                    // doanh thu
                                                    //if (show)
                                                    //{
                                                    //   // oRng = xlSheet.get_Range(kytu + (row + 14));
                                                    //    oRng.Font.ColorIndex = 3;
                                                    //    oRng.Value = ("doanh thu gio" + lines[li].workingTimes[y].KCS * lines[li].PriceCM);
                                                    //}
                                                    //EndRange.Value = ("doanh thu ngay " + (lines[li].TH_Day - lines[li].TH_Day_G) * lines[li].PriceCM);
                                                    break;
                                                case 15: //thu nhap BQ
                                                    if (show)
                                                        oRng.Value = Math.Ceiling((double)(lines[li].workingTimes[y].KCS * lines[li].Price) / lines[li].CurrentLabors);
                                                    EndRange.Value = Math.Ceiling((double)((lines[li].TH_Day - lines[li].TH_Day_G) * lines[li].Price) / lines[li].CurrentLabors);
                                                    break;
                                                case 16: //giao Hoan thanh
                                                    if (show)
                                                        oRng.Value = lines[li].workingTimes[y].CongDoan;
                                                    EndRange.Value = lines[li].workingTimes.Sum(x => x.CongDoan);
                                                    break;
                                            }
                                            #endregion
                                        }
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            else
                            {
                                #region ten chuyen, san pham ...
                                kytu = Convert.ToChar((a + 1) + 65).ToString();

                                //   oRng = xlSheet.get_Range((kytu + (row + li)) + ":" + (kytu + (row + subTitles.Length + li)), (kytu + (row + li)) + ":" + (kytu + (row + subTitles.Length + li)));
                                oRng = xlSheet.get_Range((kytu + row) + ":" + (kytu + (row + so_dong_thong_tin_1_chuyen)), (kytu + row) + ":" + (kytu + (row + so_dong_thong_tin_1_chuyen)));
                                switch (a)
                                {
                                    case 0: oRng.Value = lines[li].LineName.ToUpper(); break;
                                    case 1: oRng.Value = lines[li].CurrentLabors; break;
                                    case 2: oRng.Value = string.Empty; break;
                                    case 3: oRng.Value = lines[li].CommoName.ToUpper(); break;
                                    //  case 4: oRng.Value = lines[li].PriceCM; break;
                                    case 4: oRng.Value = 0; break;
                                }
                                #endregion
                            }
                        }
                        #endregion

                        row += (so_dong_thong_tin_1_chuyen); // subTitles.Length;
                    }
                }
                #endregion
                // row += 9;
                #region
                if (row < 324)
                {
                    Microsoft.Office.Interop.Excel.Range range = xlSheet.get_Range("A" + row + ":V" + row, "A324:V324");
                    range.Delete(Microsoft.Office.Interop.Excel.XlDeleteShiftDirection.xlShiftUp);
                }

                #region Title
                Excel.Range caption = xlSheet.get_Range("B1", endChar + "1");
                caption.Select();
                caption.FormulaR1C1 = tieuDe.ToUpper();

                xlSheet.get_Range("B2", endChar + "2").Merge(false);
                caption = xlSheet.get_Range("B2", endChar + "2");
                caption.Select();
                caption.FormulaR1C1 = ("Ngày " + date.ToString("dd/MM/yyyy")).ToUpper();

                #endregion
                //save file
                xlBook.SaveAs(path + fileName, Excel.XlFileFormat.xlWorkbookDefault, missValue, missValue, missValue, missValue, Excel.XlSaveAsAccessMode.xlNoChange, missValue, missValue, missValue, missValue, missValue);
                xlBook.Close(true, missValue, missValue);
                xlApp.Quit();

                // release cac doi tuong COM
                releaseObject(xlSheet);
                releaseObject(xlBook);
                releaseObject(xlApp);
                result = true;
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tạo file mail bị lỗi.\n" + ex.Message);
            }
            return result;
        }

        ///// <summary>
        ///// Deletes empty rows from the end of the given worksheet
        ///// </summary>
        //public static void TrimRows(this Excel.Worksheet worksheet)
        //{
        //    Excel.Range range = worksheet.UsedRange;
        //    while (worksheet.Application.WorksheetFunction.CountA(range.Rows[range.Rows.Count]) == 0)
        //        (range.Rows[range.Rows.Count] as Excel.Range).Delete();
        //}

        /// <summary>
        /// Hoàng Gia
        /// </summary>
        /// <param name="tieuDe"></param>
        /// <param name="path"></param>
        /// <param name="templateName"></param>
        /// <param name="fileName"></param>
        /// <param name="lines_ngay"></param>
        /// <param name="timesGetNSInDay"></param>
        /// <returns></returns>
        public static bool ExportToExcel_HoangGia(string tieuDe, string path, string templateName, string fileName, List<HoangGiaReportModel> lines_ngay, List<HoangGiaReportModel> lines_thang, int timesGetNSInDay, List<DepartmentDailyLaboursModel> departments, DateTime date)
        {
            //  MessageBox.Show("ExportToExcel_HoangGia");
            var result = false;
            try
            {
                #region khoi tao cac doi tuong Com Excel de lam viec
                Excel.Application xlApp;
                Excel.Worksheet xlSheet;
                Excel.Workbook xlBook;
                //doi tuong Trống để thêm  vào xlApp sau đó lưu lại sau
                object missValue = System.Reflection.Missing.Value;
                //khoi tao doi tuong Com Excel moi
                xlApp = new Excel.Application();
                string templatePath = System.Windows.Forms.Application.StartupPath + @"\Report\Template\" + templateName;
                xlBook = xlApp.Workbooks.Open(templatePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                xlBook.CheckCompatibility = false;
                xlBook.DoNotPromptForConvert = true;

                //không cho hiện ứng dụng Excel lên để tránh gây đơ máy
                xlApp.Visible = false;

                #endregion
                int soCotTD = 27,
                     soDongBĐ = 5,
                     lineId = 0,
                     soDongChuyen = 1;
                bool changeLine = false;
                Excel.Range eRange;
                Excel.Range eRangeTotal = null;

                #region thông tin Ngày

                #region sheet 1
                //su dung Sheet dau tien de thao tac
                xlSheet = (Excel.Worksheet)xlBook.Worksheets.get_Item(1);
                xlSheet.Activate();
                eRange = xlSheet.get_Range("B2");
                eRange.Value = date.ToString("dd/MM/yyyy");
                foreach (var item in lines_ngay)
                {
                    eRange = null;
                    if (item.LineId != lineId)
                    {
                        soDongChuyen = lines_ngay.Where(x => x.LineId == item.LineId).Count();
                        soDongChuyen = (soDongChuyen == 0 ? soDongChuyen : soDongChuyen - 1);
                        lineId = item.LineId;
                        changeLine = true;
                    }
                    else
                        changeLine = false;

                    for (int ii = 0; ii < soCotTD; ii++)
                    {
                        string colChar = ConvertChar((ii + 65));
                        if (ii < 7 && changeLine)
                        {
                            if (soDongChuyen > 0)
                            {
                                eRange = xlSheet.get_Range((colChar + soDongBĐ), (colChar + (soDongBĐ + soDongChuyen)));
                                eRange.Select();
                                eRange.Merge();
                            }
                            else
                                eRange = xlSheet.get_Range((colChar + soDongBĐ));
                            switch (ii)
                            {
                                case 0: eRange.Value = item.LineName; break;
                                case 1: eRange.Value = item.BaseLabours; break;
                                case 2: eRange.Value = item.NewLabours; break;
                                case 3: eRange.Value = item.OffLabours; break;
                                case 4: eRange.Value = item.OnVacationLabours; break;
                                case 5: eRange.Value = item.PregnantLabours; break;
                                case 6: eRange.Value = item.CurrentLabours; break;
                            }
                        }
                        else
                        {
                            eRange = xlSheet.get_Range((colChar + soDongBĐ));
                            switch (ii)
                            {
                                case 7: eRange.Value = item.ProductName; break;
                                case 8: eRange.Value = item.CustomerCode; break;
                                case 9: eRange.Value = item.SLKH; break;
                                case 10: eRange.Value = item.PriceCM; break;
                                case 11: eRange.Value = item.Price; break;
                                case 12:
                                    if (item.DateInput.HasValue)
                                        eRange.Value = item.DateInput.Value.ToString("dd/MM/yyyy"); break;
                                case 13: eRange.Value = item.TC; break;
                                case 14: eRange.Value = item.LK_TC; break;
                                case 15: eRange.Value = item.SLKH - item.LK_TC; break;
                                case 16: eRange.Value = item.TC * item.PriceCM; break;
                                case 17: eRange.Value = item.LK_TC * item.PriceCM; break;
                                case 18: eRange.Value = item.LK_TC * item.Price; break;
                                case 19: eRange.Value = ((item.LK_TC * item.Price) > 0 ? (item.LK_TC * item.Price) / item.CurrentLabours : 0); break;
                                case 20: eRange.Value = item.Ui; break;
                                case 21: eRange.Value = item.LK_Ui; break;
                                case 22: eRange.Value = item.SLKH - item.LK_Ui; break;
                                case 23: eRange.Value = item.DongThung; break;
                                case 24: eRange.Value = item.LK_DongThung; break;
                                case 25: eRange.Value = item.SLKH - item.LK_DongThung; break;
                                case 26:
                                    if (item.DateOutput.HasValue)
                                    {
                                        eRange.Value = "";
                                        string dt = item.DateOutput.Value.ToString("dd/MM/yyyy");
                                        eRange.Value = (dt + " ");
                                    }
                                    break;
                            }
                        }
                        SetBorder_TextAlign(eRange, (ii == 18 || ii == 19 ? true : false));
                    }
                    soDongBĐ++;
                }

                eRangeTotal = xlSheet.get_Range("H" + soDongBĐ + ":I" + soDongBĐ);
                SetBorder_TextAlign(eRangeTotal, true);
                eRangeTotal.Value = "CỘNG";
                eRangeTotal.Font.ColorIndex = 3;
                eRangeTotal.Interior.ColorIndex = 6;

                for (int ii = 9; ii < soCotTD; ii++)
                {
                    #region Row Total
                    string colChar = ConvertChar((ii + 65));
                    eRangeTotal = xlSheet.get_Range(colChar + soDongBĐ);
                    SetBorder_TextAlign(eRangeTotal, true);
                    eRangeTotal.Font.ColorIndex = 3;
                    eRangeTotal.Interior.ColorIndex = 6;
                    if (ii != 11 && ii != 12 && ii != 10 && ii != 26)
                    {
                        if (ii == 19) //BQ
                            eRangeTotal.Value = "=Sum(" + (colChar) + "5:" + (colChar + (soDongBĐ - 1)) + ")/" + lines_ngay.Count;
                        else
                            eRangeTotal.Value = "=Sum(" + (colChar) + "5:" + (colChar + (soDongBĐ - 1)) + ")";
                    }
                    #endregion
                }

                int y = 0;
                if (departments.Count > 0)
                {
                    foreach (var department in departments)
                    {
                        for (int ii = 0; ii < soCotTD; ii++)
                        {
                            string colChar = ConvertChar((ii + 65));
                            eRange = xlSheet.get_Range((colChar + soDongBĐ), (colChar + (soDongBĐ)));
                            SetBorder_TextAlign(eRange, false);

                            switch (ii)
                            {
                                case 0: eRange.Value = department.DepartmentName; break;
                                case 1: eRange.Value = department.BaseLabours; break;
                                case 2: eRange.Value = department.LDNew; break;
                                case 3: eRange.Value = department.LDOff; break;
                                case 4: eRange.Value = department.LDVacation; break;
                                case 5: eRange.Value = department.LDPregnant; break;
                                case 6: eRange.Value = department.LDCurrent; break;
                            }

                            #region Row Total
                            if (y == 0)
                            {
                                eRangeTotal = xlSheet.get_Range(colChar + (soDongBĐ + departments.Count));
                                SetBorder_TextAlign(eRangeTotal, true);
                                eRangeTotal.Font.ColorIndex = 3;
                                eRangeTotal.Interior.ColorIndex = 6;
                                if (ii == 0)
                                    eRangeTotal.Value = "CỘNG";
                                else if (ii > 0 && ii < 7)
                                    eRangeTotal.Value = "=Sum(" + (colChar) + "5:" + (colChar + (soDongBĐ + departments.Count - 1)) + ")";

                            }
                            #endregion

                            if (ii == 6)
                                break;
                        }
                        soDongBĐ++;
                        y++;
                    }
                }
                else
                {
                    for (int ii = 0; ii < soCotTD; ii++)
                    {
                        string colChar = ConvertChar((ii + 65));
                        #region Row Total
                        eRangeTotal = xlSheet.get_Range(colChar + (soDongBĐ));
                        SetBorder_TextAlign(eRangeTotal, true);
                        if (ii == 0)
                            eRangeTotal.Value = "CỘNG";
                        else if (ii > 0 && ii < 7)
                            eRangeTotal.Value = "=Sum(" + (colChar) + "5:" + (colChar + (soDongBĐ)) + ")";

                        #endregion

                        if (ii == 6)
                            break;
                    }
                }

                #endregion

                soCotTD = 15;
                soDongBĐ = 7;
                int stt = 1;

                #region sheet2
                //su dung Sheet dau tien de thao tac
                xlSheet = (Excel.Worksheet)xlBook.Worksheets.get_Item(2);
                xlSheet.Activate();
                eRange = xlSheet.get_Range("B3");
                eRange.Value = date.ToString("dd/MM/yyyy");

                foreach (var item in lines_ngay)
                {
                    for (int i = 0; i < soCotTD; i++)
                    {
                        string colChar = ConvertChar((i + 65));
                        eRange = xlSheet.get_Range((colChar + soDongBĐ));
                        SetBorder_TextAlign(eRange, (i == 12 || i == 13 ? true : false));
                        switch (i)
                        {
                            case 0: eRange.Value = stt; break;
                            case 1: eRange.Value = item.ProductName; break;
                            case 2: eRange.Value = item.CustomerCode; break;
                            case 3: eRange.Value = item.SLKH; break;
                            case 4: eRange.Value = item.PriceCut; break;
                            case 5: eRange.Value = item.Cut; break;
                            case 6: eRange.Value = item.LK_Cut; break;
                            case 7: eRange.Value = item.SLKH - item.LK_Cut; break;
                            case 8: eRange.Value = item.BTP; break;
                            case 9: eRange.Value = item.LK_BTP; break;
                            case 10: eRange.Value = item.SLKH - item.LK_BTP; break;
                            case 11: eRange.Value = item.Cut * item.PriceCut; break;
                            case 12: eRange.Value = item.LK_Cut * item.PriceCut; break;
                            case 13:
                                var cutDepart = departments.FirstOrDefault(x => x.DepartmentId == 1);
                                if (cutDepart != null)
                                    eRange.Value = ((item.LK_Cut * item.PriceCut) != 0 && cutDepart.LDCurrent != 0 ? (item.LK_Cut * item.PriceCut) / cutDepart.LDCurrent : 0);
                                break;
                            case 14: eRange.Value = ""; break;
                        }

                        #region Row Total
                        if (stt == 1)
                        {
                            eRangeTotal = xlSheet.get_Range(colChar + (soDongBĐ + lines_ngay.Count));
                            SetBorder_TextAlign(eRangeTotal, true);
                            switch (i)
                            {
                                case 0: eRangeTotal.Value = "CỘNG"; break;
                                case 3:
                                case 5:
                                case 8:
                                case 11:
                                    eRangeTotal.Value = "=Sum(" + (colChar + soDongBĐ.ToString()) + ":" + (colChar + (soDongBĐ + lines_ngay.Count - 1)) + ")";
                                    break;
                            }
                        }
                        #endregion
                    }
                    soDongBĐ++;
                    stt++;
                }

                #endregion

                #endregion

                #region thong tin tháng
                soCotTD = 18;
                soDongBĐ = 5;
                lineId = 0;
                soDongChuyen = 1;

                #region sheet 3
                //su dung Sheet dau tien de thao tac
                xlSheet = (Excel.Worksheet)xlBook.Worksheets.get_Item(3);
                xlSheet.Activate();
                eRange = xlSheet.get_Range("B2");
                eRange.Value = ((DateTime.Now.Day > 1 ? "1 -> " : "") + date.ToString("dd/MM/yyyy"));

                int bd = soDongBĐ;
                foreach (var item in lines_thang)
                {
                    eRange = null;
                    if (item.LineId != lineId)
                    {
                        if (lineId != 0)
                        {
                            #region  sum mỗi chuyen sau khi nhảy qua chuyen khac
                            for (int ii = 0; ii < soCotTD; ii++)
                            {
                                string colChar = ConvertChar((ii + 65));
                                eRangeTotal = xlSheet.get_Range(colChar + (soDongBĐ));

                                SetBorder_TextAlign(eRangeTotal, true);
                                eRangeTotal.Font.ColorIndex = 3;
                                eRangeTotal.Interior.ColorIndex = 6;
                                switch (ii)
                                {
                                    case 2: eRangeTotal.Value = "CỘNG"; break;
                                    case 4:
                                    case 8:
                                    case 9:
                                    case 10:
                                    case 11:
                                    case 12:
                                    case 13:
                                    case 14:
                                    case 15:
                                        eRangeTotal.Value = "=Sum(" + (colChar + bd) + ":" + (colChar + (soDongBĐ - 1)) + ")"; break;
                                }
                            }
                            soDongBĐ++;
                            bd = soDongBĐ;
                            #endregion
                        }

                        soDongChuyen = lines_thang.Where(x => x.LineId == item.LineId).Count();
                        soDongChuyen = (soDongChuyen == 0 ? soDongChuyen : soDongChuyen - 1);
                        lineId = item.LineId;
                        changeLine = true;

                    }
                    else
                        changeLine = false;

                    for (int ii = 0; ii < soCotTD; ii++)
                    {
                        string colChar = ConvertChar((ii + 65));
                        if (ii < 2 && changeLine)
                        {
                            eRange = xlSheet.get_Range((colChar + soDongBĐ), (colChar + (soDongBĐ + soDongChuyen)));
                            eRange.Select();
                            eRange.Merge();
                            switch (ii)
                            {
                                case 0: eRange.Value = item.LineName; break;
                                case 1: eRange.Value = item.BaseLabours; break;
                            }
                        }
                        else
                        {
                            eRange = xlSheet.get_Range((colChar + soDongBĐ));
                            switch (ii)
                            {
                                case 2: eRange.Value = item.ProductName; break;
                                case 3: eRange.Value = item.CustomerCode; break;
                                case 4: eRange.Value = item.SLKH; break;
                                case 5: eRange.Value = item.PriceCM; break;
                                case 6: eRange.Value = item.Price; break;
                                case 7:
                                    if (item.DateInput.HasValue)
                                        eRange.Value = item.DateInput.Value.ToString("dd/MM/yyyy"); break;
                                case 8: eRange.Value = item.LK_TC; break;
                                case 9: eRange.Value = item.SLKH - item.LK_TC; break;
                                case 10: eRange.Value = item.LK_TC * item.PriceCM; break;
                                case 11: eRange.Value = item.LK_TC * item.Price; break;
                                case 12: eRange.Value = ((item.LK_TC * item.Price) > 0 ? (item.LK_TC * item.Price) / item.BaseLabours : 0); break;
                                case 13: eRange.Value = item.LK_Ui; break;
                                case 14: eRange.Value = item.SLKH - item.LK_Ui; break;
                                case 15: eRange.Value = item.DongThung; break;
                                case 16: eRange.Value = item.SLKH - item.DongThung; break;
                                case 17:
                                    if (item.DateOutput.HasValue)
                                    {
                                        string dt = item.DateOutput.Value.ToString("dd/MM/yyyy");
                                        eRange.Value = dt;
                                    }
                                    break;
                            }
                        }
                        SetBorder_TextAlign(eRange, (ii == 11 || ii == 12 ? true : false));
                    }
                    soDongBĐ++;
                }

                y = 0;
                for (int ii = 0; ii < soCotTD; ii++)
                {
                    string colChar = ConvertChar((ii + 65));
                    #region Row Total
                    eRangeTotal = xlSheet.get_Range(colChar + (soDongBĐ));
                    SetBorder_TextAlign(eRangeTotal, true);
                    eRangeTotal.Font.ColorIndex = 3; //set text color
                    eRangeTotal.Interior.ColorIndex = 6; //set background color
                    switch (ii)
                    {
                        case 2: eRangeTotal.Value = "CỘNG"; break;
                        case 4:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                            eRangeTotal.Value = "=Sum(" + (colChar + bd) + ":" + (colChar + (soDongBĐ - 1)) + ")"; break;
                    }
                    #endregion
                }
                #endregion

                soCotTD = 12;
                soDongBĐ = 7;
                stt = 1;

                #region sheet4
                //su dung Sheet dau tien de thao tac
                xlSheet = (Excel.Worksheet)xlBook.Worksheets.get_Item(4);
                xlSheet.Activate();
                eRange = xlSheet.get_Range("B3");
                eRange.Value = ((DateTime.Now.Day > 1 ? "1 -> " : "") + date.ToString("dd/MM/yyyy"));

                foreach (var item in lines_thang)
                {
                    for (int i = 0; i < soCotTD; i++)
                    {
                        string colChar = ConvertChar((i + 65));
                        eRange = xlSheet.get_Range((colChar + soDongBĐ));
                        SetBorder_TextAlign(eRange, (i == 1 || i == 9 || i == 10 ? true : false));
                        switch (i)
                        {
                            case 0: eRange.Value = stt; break;
                            case 1: eRange.Value = item.ProductName; break;
                            case 2: eRange.Value = item.CustomerCode; break;
                            case 3: eRange.Value = item.SLKH; break;
                            case 4: eRange.Value = item.PriceCut; break;
                            case 5: eRange.Value = item.LK_Cut; break;
                            case 6: eRange.Value = item.SLKH - item.LK_Cut; break;
                            case 7: eRange.Value = item.LK_BTP; break;
                            case 8: eRange.Value = item.SLKH - item.LK_BTP; break;
                            case 9: eRange.Value = item.LK_Cut * item.PriceCut; break;
                            case 10:
                                var cutDepart = departments.FirstOrDefault(x => x.DepartmentId == 1);
                                if (cutDepart != null)
                                    eRange.Value = ((item.LK_Cut * item.PriceCut) != 0 && cutDepart.BaseLabours != 0 ? (item.LK_Cut * item.PriceCut) / cutDepart.BaseLabours : 0);
                                break;
                        }

                        #region Row Total
                        if (stt == 1)
                        {
                            eRangeTotal = xlSheet.get_Range(colChar + (soDongBĐ + lines_thang.Count));
                            SetBorder_TextAlign(eRangeTotal, true);
                            switch (i)
                            {
                                case 0: eRangeTotal.Value = "CỘNG"; break;
                                case 3:
                                case 4:
                                case 5:
                                case 6:
                                case 7:
                                case 8:
                                case 9:
                                    eRangeTotal.Value = "=Sum(" + (colChar + soDongBĐ.ToString()) + ":" + (colChar + (soDongBĐ + lines_thang.Count - 1)) + ")";
                                    break;
                            }
                        }
                        #endregion
                    }
                    soDongBĐ++;
                    stt++;
                }

                #endregion
                #endregion

                #region save file
                //save file
                xlBook.SaveAs(path + fileName, Excel.XlFileFormat.xlWorkbookDefault, missValue, missValue, missValue, missValue, Excel.XlSaveAsAccessMode.xlNoChange, missValue, missValue, missValue, missValue, missValue);
                xlBook.Close(true, missValue, missValue);
                xlApp.Quit();

                // release cac doi tuong COM
                releaseObject(xlSheet);
                releaseObject(xlBook);
                releaseObject(xlApp);
                result = true;
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tạo file mail bị lỗi.\n" + ex.Message);
            }
            return result;
        }

    }
}


#region

#endregion
