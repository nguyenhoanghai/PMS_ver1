using DuAn03_HaiDang;
using DuAn03_HaiDang.DAO;
using DuAn03_HaiDang.DATAACCESS;
using DuAn03_HaiDang.KeyPad_Chuyen.dao;
using DuAn03_HaiDang.POJO;
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

namespace QuanLyNangSuat
{
    public partial class FrmImportPCCFromExcel : Form
    {
        private HangDAO hangDAO = new HangDAO();
        private ChuyenDAO chuyenDAO = new ChuyenDAO();
        private SanPhamDAO sanPhamDAO = new SanPhamDAO();
        private Chuyen_SanPhamDAO chuyenSanPhamDAO = new Chuyen_SanPhamDAO();
        private List<Chuyen_SanPham> listChuyenPhamCong = null;
        private List<SanPham> listSanPham = null;
        public FrmImportPCCFromExcel()
        {
            InitializeComponent();
        }

        private void butCreateTemplateExcel_Click(object sender, EventArgs e)
        {
            try
            { 
                List<ModelSheetExcel> listModelSheet = new List<ModelSheetExcel>();                
                DataTable dtLines = chuyenDAO.GetDataTableLineByStrListId(AccountSuccess.strListChuyenId);
                if (dtLines != null)
                    listModelSheet.Add(new ModelSheetExcel() { Title = "Danh Sách Chuyền", Data = dtLines });
                DataTable dtSanPham = hangDAO.DSHangExportExcel(AccountSuccess.IdFloor);
                if(dtSanPham!=null)
                    listModelSheet.Add(new ModelSheetExcel() { Title = "Danh Sách Mặt Hàng", Data = dtSanPham });
                DataTable dtPHC = new DataTable();
                dtPHC = new System.Data.DataTable("PHC");
                dtPHC.Columns.Add("TÊN CHUYỀN", typeof(System.String));
                dtPHC.Columns.Add("Mặt Hàng", typeof(System.String));
                dtPHC.Columns.Add("TG CHẾ TẠO", typeof(System.String));
                dtPHC.Columns.Add("SẢN LƯỢNG KH", typeof(System.String));
                dtPHC.Columns.Add("THÁNG", typeof(System.String));
                dtPHC.Columns.Add("NĂM", typeof(System.String));
                listModelSheet.Add(new ModelSheetExcel() { Title = "Phân Công Mặt Hàng Cho Chuyền", Data = dtPHC });
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = @"C:\";
                saveFileDialog1.Title = "Save excel file"; 
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {                    
                    var result = ReportDB.exportMulDataToExcel(saveFileDialog1.FileName, listModelSheet);
                    if (result)
                        MessageBox.Show("Tạo file excel nhập phân công chuyền thành công.");
                }
                
            }
            catch (Exception ex)
            {                
                MessageBox.Show("Lỗi:"+ex.Message);
            }
        }

        private void butSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(listChuyenPhamCong!=null && listChuyenPhamCong.Count>0)
                {
                    var modelResult = chuyenSanPhamDAO.AddListPCC(listChuyenPhamCong, AccountSuccess.strListChuyenId, AccountSuccess.IdFloor);
                    if (modelResult.IsSuccsess)
                        MessageBox.Show("Phân công hàng cho chuyền thành công !");
                    else
                    { 
                        if(modelResult.ListError.Count>0)
                        {
                            string error = string.Empty;
                            foreach(var err in modelResult.ListError)
                            {
                                error += err.ErrorContent + "\n";
                            }
                            MessageBox.Show(error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Lỗi:" + ex.Message);
            }
        }

        string pathChooseFile = string.Empty;
        private void butChooseFile_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "file excel|*.xls;*.xlsx|all file|*.*";
                dlg.InitialDirectory = @"C:\";                
                dlg.Multiselect = false;
                dlg.RestoreDirectory = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    listChuyenPhamCong = new List<Chuyen_SanPham>();
                    listSanPham = new List<SanPham>();
                    string[] tmp = dlg.FileNames;
                    int sttThucHien = 0;
                    foreach (string i in tmp)
                    {
                        txtFile.Text = dlg.FileName;
                        DateTime dateNow = DateTime.Now;
                        int intMorth = dateNow.Month;
                        int intYear = dateNow.Year;
                        Microsoft.Office.Interop.Excel.Application xlApp;
                        Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
                        Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
                        object misValue = System.Reflection.Missing.Value;

                        xlApp = new Microsoft.Office.Interop.Excel.Application();
                        xlWorkBook = xlApp.Workbooks.Open(dlg.FileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                        
                        if(xlWorkBook.Worksheets!=null)
                        {
                            int countSheet = xlWorkBook.Worksheets.Count;
                            for (int indexSheet = 1; indexSheet <= countSheet; indexSheet++)
                            {
                                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(indexSheet);
                                int indexRow = 10;
                                int intRowClear = 0;
                                dynamic dThangNam = xlWorkSheet.get_Range("M4", "M4").Value2;
                                int intThang = 0;
                                int intNam = 0;
                                if (dThangNam != null)
                                {
                                    string strThangNam = dThangNam.ToString();
                                    strThangNam = strThangNam.Replace("THÁNG", "|");
                                    strThangNam = strThangNam.Replace("NĂM", "|");
                                    var arrThangNam = strThangNam.Split(new char[] { '|' });
                                    if (arrThangNam != null && arrThangNam.Length > 2)
                                    {
                                        int.TryParse(arrThangNam[1], out intThang);
                                        int.TryParse(arrThangNam[2], out intNam);
                                    }
                                }
                                if(intThang==intMorth && intNam ==intYear)
                                {
                                    string tenChuyen = string.Empty;
                                    while (true)
                                    {
                                        dynamic dTenChuyen = xlWorkSheet.get_Range("M" + indexRow, "M" + indexRow).Value2;
                                        dynamic dSanPham = xlWorkSheet.get_Range("N" + indexRow, "N" + indexRow).Value2;
                                        dynamic dTGCheTaoSP = xlWorkSheet.get_Range("O" + indexRow, "O" + indexRow).Value2;
                                        dynamic dSanLuongKH = xlWorkSheet.get_Range("T" + indexRow, "T" + indexRow).Value2;
                                        dynamic dDonGiaCM = xlWorkSheet.get_Range("S" + indexRow, "S" + indexRow).Value2;
                                        if (dSanPham != null && dDonGiaCM != null)
                                        {
                                            float fDonGiaCM = 0;
                                            float.TryParse(dDonGiaCM.ToString(), out fDonGiaCM);
                                            listSanPham.Add(new SanPham()
                                            {
                                                TenSanPham = dSanPham.ToString(),
                                                DonGia = fDonGiaCM,
                                                DonGiaCM = fDonGiaCM,
                                                Floor = AccountSuccess.IdFloor
                                            });
                                        }
                                        if (dSanPham != null && dTGCheTaoSP != null && dSanLuongKH != null)
                                        {
                                            if (dTenChuyen != null)
                                                tenChuyen = dTenChuyen.ToString();
                                            int intSanLuongKH = 0;
                                            int.TryParse(dSanLuongKH.ToString(), out intSanLuongKH);
                                            float fTgCheTaoSP = 0;
                                            float.TryParse(dTGCheTaoSP.ToString(), out fTgCheTaoSP);
                                            sttThucHien++;
                                            listChuyenPhamCong.Add(new Chuyen_SanPham()
                                            {
                                                STTThucHien = sttThucHien,
                                                TenChuyen = tenChuyen.ToString(),
                                                TenSanPham = dSanPham.ToString(),
                                                Thang = intThang,
                                                Nam = intNam,
                                                SanLuongKeHoach = intSanLuongKH,
                                                NangXuatSanXuat = fTgCheTaoSP
                                            });
                                        }
                                        else
                                        {
                                            intRowClear++;
                                            //MessageBox.Show("Lỗi: Thông tin phân công hàng cho chuyền, nhập liệu không đúng. Vui lòng kiểm tra lại, các thông tin không được để trống và các dòng dữ liệu cần liền nhau.");
                                        }
                                        if (intRowClear > 20)
                                            break;
                                        indexRow++;
                                    }
                                }                                
                            }
                            xlWorkBook.Close(true, misValue, misValue);
                            xlApp.Quit();                            
                            ReportDB.releaseObject(xlWorkBook);
                            ReportDB.releaseObject(xlApp); 
                        }                                              
                        break;
                    }
                    gridControl1.DataSource = listChuyenPhamCong;
                    if(listSanPham!=null && listSanPham.Count>0)
                    { 
                        foreach(var p in listSanPham)
                        {
                            var productExist = sanPhamDAO.GetProductByName(AccountSuccess.IdFloor, p.TenSanPham);
                            if(productExist==null)
                            {
                                if (MessageBox.Show("Mặt Hàng: " + p.TenSanPham.Trim() + " chưa có trong hệ thống. Bạn có muốn thêm Mặt Hàng này không?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    sanPhamDAO.ThemOBJ(p);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Lỗi:" + ex.Message);
            }
        }
    }
}
