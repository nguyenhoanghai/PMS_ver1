using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DuAn03_HaiDang.DATAACCESS;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using DuAn03_HaiDang.POJO;
using DuAn03_HaiDang.DAO;
using System.Configuration;
using DuAn03_HaiDang.KeyPad_Chuyen.dao;
using System.IO;
using System.Threading;
using System.Media;
using System.Collections;
using DuAn03_HaiDang.Model;
using DuAn03_HaiDang.HelperControl;
using GPRO.Ultilities;
using QuanLyNangSuat;
using QuanLyNangSuat.DAO;
using DevExpress.XtraBars;
using PMS.Data;
using PMS.Business;
using PMS.Business.Models;
using PMS.Business.Enum;

namespace DuAn03_HaiDang
{
    public partial class FrmMainNew : Form
    {
        #region khai bao bien
        // COM bảng điện tử
        public static SerialPort P = new SerialPort();
        // COM keypad
        public static SerialPort P2 = new SerialPort();
        public static SqlConnection sqlCon = new SqlConnection();
        private SqlCommand sqlCom = new SqlCommand();
        public static Label lblTenNV = new Label();
        public static LinkLabel butThoat = new LinkLabel();
        public static LinkLabel butChangePass = new LinkLabel();
        // Cờ, dùng để tắt mở quá trình yêu cầu nhận dữ liệu từ phần cứng
        public static bool IsQuet = false;
        ChuyenDAO chuyenDAO = new ChuyenDAO();
        NangSuatCumDAO nangSuatCumDAO = new NangSuatCumDAO();
        SoundTimeConfigDAO soundTimeConfigDAO = new SoundTimeConfigDAO();
        SoundReadConfigDAO soundReadConfigDAO = new SoundReadConfigDAO();
        SoundDAO soundDAO = new SoundDAO();
        CumDAO cumDAO = new CumDAO();
        NangXuatDAO nangSuatDAO = new NangXuatDAO();
        ThanhPhamDAO thanhPhamDAO = new ThanhPhamDAO();
        TurnCOMMngDAO turnCOMMngDAO = new TurnCOMMngDAO();
        Chuyen_SanPhamDAO chuyenSanPhamDAO = new Chuyen_SanPhamDAO();

        List<int> ListKeyPab = new List<int>();
        // datatable lưu thông tin năng xuất của chuyền
        DataTable dtTimKiemTTNangXuat = new DataTable();
        SqlDataAdapter da;
        // datatable để lưu thông tin của Mặt Hàng đầu tiên theo thứ tự sản xuất, mà Mặt Hàng đó chưa hoàn thành
        DataTable dtIdNotFinishFirst = new DataTable();
        DataTable dtDSChuyenCuaKeyPad = new DataTable();
        DataTable dtTTNangXuat = new DataTable();
        DataTable dt = new DataTable();
        List<Nut_Chuyen> listNutChuyen = new List<Nut_Chuyen>();
        // Khai báo biến để lưu STTChuyen_SanPham, để bật cờ gửi và lấy thông tin cần show lên bảng của chuyền khi nhân dư liệu từ keypad và xử lý xong
        string IndexChuyen = null;


        // DataTable lưu thông tin gửi xuống bảng của chuyền. Lưu khi Indexchuyen != null
        DataTable dtLoadToTable = new DataTable();
        DataTable dtLoadToTable1 = new DataTable();
        //List dữ liệu gửi xuống table
        List<string> listData = new List<string>();
        List<string> listDataSending = new List<string>();



        // Bien luu ma chuyen de laod data xuống bảng, khi quyền là tất cả các chuyền
        public static int strMaChuyenTatca = 11;
        // Bien luu danh sach cac chuyen co thay doi
        List<string> listIndexChuyen = new List<string>();
        // list luu anh sach ma chuyen co san pham hoan thanh tai thoi diem hien tai
        List<string> listMaChuyenIsFinish = new List<string>();

        MessageBox_Show ms = new MessageBox_Show();
        Thread threadplay, threadSendData;

        public FrmSendMailAndReadSound frmSendMailAndReadSound;
        List<KeypadModel> listKeyPadConfig = null;
        List<KeypadModel> listKeyPadObjectInfo = null;
        public List<PMS.Data.MapIdSanPhamNgay> listMapIdSanPhamNgay = null;
        public List<string> listDataSendKeyPad = new List<string>();


        List<QuanLyNangSuat.POJO.TurnCOMMng> listTurnOnOffCOM = new List<QuanLyNangSuat.POJO.TurnCOMMng>();




        int timeoutcheckACK = 0,
            NSType = 0,
            isHienThiRaManHinhLCD = 0,
            isAutoTurnOnOffCOM = 0,
            intMaxErrorOfTimer = 5,
            intErrorTimerSendData = 0,
            intErrorTimerLoadData = 0,
          isAutoMoveQuantityMorth = 0,
          autoMoveQuantityMorthType = 1,
          thoiGianLatCacLCD = 10,
          SoundSilent = 0,
          IsUseTableComport = 0,
            // so lan gui du lieu len ma ko su ly dc
          countTimeSendRequestKCSButHandleError = 0,
            countTimeSendRequestTCButHandleError = 0,
            countTimeSendRequestErrorButHandleError = 0,
            timeSendRequestKCSButHandleError = 0,
            timeSendRequestTCButHandleError = 0,
            timeSendRequestErrorButHandleError = 0,
            ClusterId_Insert = 0,
              LineId_Insert = 0,
              setTotalByMinOrMax_default = 0,
              TimerReadNotifyForKanban = 0,
              TimerReadNotifyForInventoryInKCS = 0,
            IsUseReadNotifyForKanban = 0,
            IsUseReadNotifyForInventoryInKCS = 0,
            hienThiDenTheoTPThoatChuyen = 0,
          tinhBTPThoatChuyen = 0,
          isSendMail = 0,
          isReadSound = 0,
          setTotalByMinOrMax = 1,
          timeSendRequestAndData = 100,
autoSetDayInfo = 0,
          intTimeReadSound = 1;


        public int appId = 0,
            TimesGetNSInDay = 1,
            KhoangCachGetNSInDay = 1,
            TimeRefreshFromDayInfoView = 0,
            TimeCloseFromDayInfoViewIfNotUse = 0,
             getBTPInLineByType = 1,
          calculateNormsdayType = 1,
        TypeOfCaculateDayNorms = 1, KeypadQuantityProcessingType = 0,
        TypeOfShowProductToLCD = 1;


        public bool IsStopProcess = true,
            IsIncresea = false,
            TachNhanDuLieu = false;

        bool isSendRequest = false,
            readIndexMaChuyenIsFinish = false,
              isDataSending = false,
            // Cờ gửi dữ liệu xuống bảng
          IsSend = false,
            // Khai báo cờ check ack khi gửi dữ liệu xuống bảng
          CheckACK = false;

        string idTable = string.Empty,
            SaveMediaFileAddress = string.Empty,
            ActionName = string.Empty,
                   filewavSlient = string.Empty;

        public string todayStr = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
        public List<string> TypeOfCheckFinishProduction = new List<string>();
        List<InformationChuyen> listChuyen_O = new List<InformationChuyen>();

        // list tt hien thi
        List<LineModel> listChuyen = new List<LineModel>();
        List<AppConfigModel> Configs = new List<AppConfigModel>();

        // dung luu vet lai moi khi nhan thong tin tu ban phim len
        List<CurrentAssignmentObj> currentAssignments = new List<CurrentAssignmentObj>();

        #endregion

        public FrmMainNew()
        {
            InitializeComponent();
            AccountSuccess.ListFormLCD = new List<Form>();
        }

        DataTable dtBaoHetHang = new DataTable();
        List<DuAn03_HaiDang.POJO.BaoHetHang> listBaoHetHang = new List<DuAn03_HaiDang.POJO.BaoHetHang>();
        public void LayTTBaoHetHang()
        {
            try
            {
                listBaoHetHang.Clear();
                dtBaoHetHang.Clear();
                string strSQL = "SELECT SoSanPhamConLai, SoLanBao  FROM BaoHetHang Order by SoSanPhamConLai DESC";
                if (sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }
                sqlCon.Open();
                da = new SqlDataAdapter(strSQL, sqlCon);
                da.Fill(dtBaoHetHang);
                if (dtBaoHetHang.Rows.Count > 0)
                {
                    for (int i = 0; i < dtBaoHetHang.Rows.Count; i++)
                    {
                        var baohethang = new DuAn03_HaiDang.POJO.BaoHetHang
                        {
                            SoLuongCon = int.Parse(dtBaoHetHang.Rows[i][0].ToString()),
                            SoLanBao = int.Parse(dtBaoHetHang.Rows[i][1].ToString())
                        };
                        listBaoHetHang.Add(baohethang);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        Queue queuePlayFile = new Queue();
        public void PlayinQueue()
        {
            while (true)
            {
                if (queuePlayFile.Count > 0)
                {
                    try
                    {
                        foreach (InformationPlay obj in (IEnumerable)queuePlayFile)
                        {
                            //if (Speaker == "")
                            //{
                            PlayFile(obj.SoundChuyen, obj.Repeat, true);
                            queuePlayFile.Dequeue();
                            //}
                        }
                        queuePlayFile.Clear();
                    }
                    catch (Exception ex)
                    {
                        //   MessageBox.Show("Lỗi:" + ex.Message);
                    }
                }
                Thread.Sleep(intTimeReadSound);
            }
        }

        Queue queuePlayFileWavKanBan = new Queue();
        public void PlayinQueueKanBan()
        {
            while (true)
            {
                if (queuePlayFileWavKanBan.Count > 0)
                {
                    try
                    {
                        foreach (List<string> listfilewav in (IEnumerable)queuePlayFileWavKanBan)
                        {
                            //if (Speaker == "")
                            //{
                            PlayFileKanBan(listfilewav);
                            listfilewav.Clear();
                            queuePlayFileWavKanBan.Dequeue();
                            //}
                        }
                    }
                    catch
                    { }
                }
                Thread.Sleep(intTimeReadSound);
            }
        }

        DataTable dtNangSuatCum = new DataTable();

        private void TTidnotfinishfirst(int IdChuyen, int productOutputTypeId)
        {
            dtIdNotFinishFirst.Clear();
            try
            {
                string strSQL = string.Empty;
                var dateTimeNow = DateTime.Now;
                switch (productOutputTypeId)
                {
                    case (int)eProductOutputType.TC:
                        strSQL = "SELECT TOP 1 csp.STT, csp.MaSanPham, csp.SanLuongKeHoach, csp.LuyKeTH, (SELECT COUNT(csp.STT) FROM Chuyen_SanPham csp WHERE csp.MaChuyen = '" + IdChuyen + "' and csp.IsFinishBTPThoatChuyen = 0 and csp.IsFinish = 0  and csp.IsDelete = 0) SoPhanCong, csp.LuyKeBTPThoatChuyen  FROM Chuyen_SanPham csp, SanPham sp WHERE csp.MaChuyen = '" + IdChuyen + "' and csp.IsFinish=0 and csp.IsFinishBTPThoatChuyen = 0 and csp.IsDelete = 0 AND csp.MaSanPham = sp.MaSanPham AND sp.IsDelete =0 order by csp.Nam desc, csp.Thang desc, csp.STTThucHien ASC";
                        break;
                    case (int)eProductOutputType.KCS:
                        strSQL = "SELECT TOP 1 csp.STT, csp.MaSanPham, csp.SanLuongKeHoach, csp.LuyKeTH, (SELECT COUNT(csp.STT) FROM Chuyen_SanPham csp WHERE csp.MaChuyen = '" + IdChuyen + "' and csp.IsFinish = 0 and csp.IsDelete = 0) SoPhanCong, csp.LuyKeBTPThoatChuyen  FROM Chuyen_SanPham csp, SanPham sp WHERE csp.MaChuyen = '" + IdChuyen + "' and csp.IsFinish = 0 and csp.IsDelete = 0 AND csp.MaSanPham = sp.MaSanPham AND sp.IsDelete =0 order by csp.Nam desc, csp.Thang desc, csp.STTThucHien ASC";
                        break;
                }
                if (!string.IsNullOrEmpty(strSQL))
                {
                    if (sqlCon.State == ConnectionState.Open)
                        sqlCon.Close();
                    sqlCon.Open();
                    da = new SqlDataAdapter(strSQL, sqlCon);
                    da.Fill(dtIdNotFinishFirst);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            IsQuet = false;
            this.Invoke(new EventHandler(NhanDULieu));
        }

        private void port_DataReceived_ACK(object sender, SerialDataReceivedEventArgs e)
        {
            this.Invoke(new EventHandler(NhanACK));
        }

        private void NhanDULieu(object s, EventArgs e)
        {
            try
            {
                string docdulieu = P2.ReadExisting().ToString();
                this.richtextRec.EditValue += docdulieu;
                string Hex = clsString.Ascii2HexStringNull(richtextRec.EditValue.ToString());
                if (Hex.Contains("02"))
                {
                    if ((Hex.Contains("02") && Hex.Contains("03")) || (Hex.Contains("7D") && Hex.Contains("03")))
                    {
                        string strResult = Hex;
                        ParseToCommand(strResult);
                    }
                }
                else
                {
                    //if (!P2.IsOpen)
                    //    P2.Open();

                    //IsStopProcess = false;
                    //readIndexMaChuyenIsFinish = false;
                    //IsSend = false;
                    //isSendRequest = true;
                    //IsQuet = true;

                    //// ResetComPort();
                    //this.richtextRec.EditValue = string.Empty;
                    // GhiFileLog(DateTime.Now + " NhanDULieu Hex not Contains(02)   + " + Hex);
                }
            }
            catch (Exception ex)
            {
                // ResetComPort(); 
                // GhiFileLog(DateTime.Now + " NhanDULieu exception : " + ex.Message);
            }
        }

        private void ResetComPort()
        {
            try
            {
                countTimeCheckIsQuyet = 0;
                this.richtextRec.EditValue = string.Empty;
                P2 = null;
                P2 = new SerialPort();
                InitComPort_P2();
                RunAllProcess(true);
            }
            catch (Exception ex)
            {
                //  GhiFileLog(DateTime.Now + " Reset comport function ex " + ex.Message);
            }
        }

        private void NhanACK(object s, EventArgs e)
        {
            try
            {
                string docdulieu = P.ReadExisting().ToString();
                this.richtextRec2.EditValue += docdulieu;
            }
            catch (Exception ex)
            {
                //  GhiFileLog(DateTime.Now + " NhanACK + exception : " + ex.Message + "\n");
            }
        }

        private void ParseToCommand(string sCommand)
        {
            try
            {
                string pattern = " ";
                Regex myRegex = new Regex(pattern);
                string[] ArrayCharData1 = myRegex.Split(sCommand);
                string kq = "";
                //List<string> listkq = new List<string>();
                if (ArrayCharData1.Count() > 0)
                {
                    bool isadd = false;
                    for (int i = 0; i < ArrayCharData1.Count(); i++)
                    {
                        if (ArrayCharData1[i] == "02")
                            isadd = true;
                        else if (ArrayCharData1[i] == "03")
                        {
                            isadd = false;
                            kq += ArrayCharData1[i];
                        }
                        if (isadd == true)
                            kq += ArrayCharData1[i] + " ";
                    }
                    //IsThanhPham = true;
                    string chuoi = "";
                    // Convert ASC string
                    string strASC = clsString.HexString2Ascii(kq);
                    //Console.WriteLine(strASC);
                    //Xoa 2 ky tu hex 02, 03
                    chuoi = strASC.Substring(1, strASC.Length - 1);
                    chuoi = chuoi.Remove(chuoi.Length - 1);
                    //chuoi = "0" + chuoi;
                    // Get check sum
                    string strCS = chuoi.Substring(chuoi.Length - 2, 2);
                    //Console.WriteLine(strCS);

                    // Get string again
                    string strSA = clsString.XOR(chuoi.Substring(0, chuoi.Length - 2)).Trim();
                    //Console.WriteLine(strSA);

                    if (strCS == strSA) // đúng check sum
                    {
                        chuoi = chuoi.Substring(0, chuoi.Length - 2);
                        if (txtReview.EditValue != null)
                        {
                            int lengthStrRecieve = txtReview.EditValue.ToString().Length + chuoi.Length;
                            if (lengthStrRecieve > 80)
                                txtReview.EditValue = chuoi;
                            else
                                txtReview.EditValue += chuoi;
                        }
                        else
                            txtReview.EditValue = chuoi;
                        UpdateTableLoiSanXuat(chuoi);
                        this.richtextRec.EditValue = "";
                    }
                    //else
                    //{
                    //    IsQuet = true;
                    //    this.richtextRec.EditValue = "";
                    //    countTimeCheckIsQuyet = 0;
                    //    GhiFileLog(DateTime.Now + "\n\nParseToCommand :ko dung check sum\n\n\n");
                    //    CheckComPortStatus();
                    //    ResetComPort();
                    //}
                }
                //else
                //{
                //    IsQuet = true;
                //    this.richtextRec.EditValue = "";
                //    countTimeCheckIsQuyet = 0;
                //    GhiFileLog("ParseToCommand : ArrayCharData1.Count =0;");
                //    ResetComPort();
                //}
            }
            catch
            {
                //IsQuet = true;
                //this.richtextRec.EditValue = "";
                //countTimeCheckIsQuyet = 0;
                //GhiFileLog(DateTime.Now + " : ParseToCommand : Exception");
                //  ResetComPort();
            }
        }

        public void GhiFileLog(string txt)
        {
            try
            {
                string path = Application.StartupPath + "\\FileLog.txt";
                if (!File.Exists(path))
                    File.Create(path);
                FileStream file = new FileStream(path, FileMode.Append);
                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.WriteLine(txt);
                    sw.Flush();
                    sw.Close();
                }
                file.Close();
                file.Dispose();
            }
            catch (Exception ex)
            {
            }
        }

        private TimeSpan TimeIsWork(int MaChuyen)
        {
            TimeSpan timeWork = new TimeSpan();
            try
            {
                DateTime daynow = DateTime.Now.Date;
                timeWork = TimeSpan.Parse("00:00:00");
                TimeSpan timeNow = DateTime.Now.TimeOfDay;
                if (listChuyen.Count > 0)
                {
                    var lineObj = listChuyen.FirstOrDefault(x => x.MaChuyen == MaChuyen);

                    var chuyenTimeWork = lineObj.Shifts; //BLLShift.GetShiftsOfLine(MaChuyen);
                    TimeSpan timeStartTT = lineObj.TimeCalculateTT; // listChuyen.FirstOrDefault(x => x.MaChuyen == MaChuyen).TimeCalculateTT;
                    foreach (var item in chuyenTimeWork.OrderBy(x => x.ShiftOrder))
                    {
                        if (item.Start < timeStartTT && timeStartTT < item.End)
                        {
                            item.Start = timeStartTT;
                        }
                        else if (item.End <= timeStartTT)
                        {
                            chuyenTimeWork.Remove(item);
                        }
                    }
                    if (chuyenTimeWork.Count > 0)
                    {
                        for (int j = 0; j < chuyenTimeWork.Count; j++)
                        {
                            if (timeNow > chuyenTimeWork[j].Start)
                            {
                                if (timeNow < chuyenTimeWork[j].End)
                                {
                                    timeWork += (timeNow - chuyenTimeWork[j].Start);
                                }
                                else
                                {
                                    timeWork += (chuyenTimeWork[j].End - chuyenTimeWork[j].Start);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return timeWork;
        }

        public void PlayFile(string Sound, int Solanlap, bool isBaoNhan)
        {
            if (Sound != "")
            {
                List<string> listRead = new List<string>();
                if (Solanlap > 0)
                {
                    for (int i = 0; i < Solanlap; i++)
                    {
                        listRead.Add(Application.StartupPath + @"\Sound\" + Sound);
                        if (!isBaoNhan)
                        {
                            listRead.Add(Application.StartupPath + @"\Sound\Complete.wav");
                        }
                    }
                }
                //   if (Speaker == "")
                //  {
                if (listRead.Count > 0)
                {
                    for (int j = 0; j < listRead.Count; j++)
                    {
                        try
                        {
                            SoundPlayer sp = new SoundPlayer();
                            int iTime = SoundInfo.GetSoundLength(listRead[j].ToString()) - SoundSilent;
                            sp.SoundLocation = listRead[j].ToString();
                            sp.Play();
                            Thread.Sleep(iTime);
                        }
                        catch
                        {
                        }
                    }
                }
                //  }
            }
        }

        public void PlayFileKanBan(List<string> listfilewavkanban)
        {
            if (listfilewavkanban.Count() > 0)
            {
                List<string> listRead = new List<string>();
                for (int i = 0; i < listfilewavkanban.Count(); i++)
                {
                    listRead.Add(Application.StartupPath + @"\Sound\" + listfilewavkanban[i]);
                }
                // if (Speaker == "")
                // {
                if (listRead.Count > 0)
                {
                    for (int j = 0; j < listRead.Count; j++)
                    {
                        try
                        {
                            SoundPlayer sp = new SoundPlayer();
                            int iTime = SoundInfo.GetSoundLength(listRead[j].ToString().Trim()) - SoundSilent;
                            sp.SoundLocation = listRead[j].ToString();
                            sp.Play();
                            Thread.Sleep(iTime);
                        }
                        catch
                        { }
                    }
                }
                // }
            }
        }

        private string SoundChuyen = "";
        private int Repeat = 0;
        //  private string FloorNumber = "";
        private void UpdateTableLoiSanXuat(string buff)
        {
            try
            {
                //   GhiFileLog(DateTime.Now + " UpdateTableLoiSanXuat : nhân dữ liệu từ keypad, update vào DB \n");
                //   MessageBox.Show(buff, "buff");
                listIndexChuyen.Clear();
                listMaChuyenIsFinish.Clear();
                string pattern = ",";
                Regex myRegex = new Regex(pattern);
                string[] ArrayCharData = myRegex.Split(buff);
                int equipmentId = 0;
                int.TryParse(ArrayCharData[0], out equipmentId);
                if (listKeyPadConfig != null && listKeyPadConfig.Count > 0)
                {
                    //  MessageBox.Show("equip : " + equipmentId,"");
                    var keyPadConfig = listKeyPadConfig.Where(c => c.EquipmentId == equipmentId).FirstOrDefault();
                    if (keyPadConfig != null)
                    {
                        switch (keyPadConfig.UseTypeId)
                        {
                            #region OnKeyPadOneObject
                            case (int)eUseKeyPadType.OneKeyPadOneObject:
                                {
                                    //    MessageBox.Show("OnKeyPadOneObject", "");
                                    if (listKeyPadObjectInfo != null && listKeyPadObjectInfo.Count > 0)
                                    {
                                        var keyPadObjectInfo = listKeyPadObjectInfo.Where(c => c.EquipmentId == equipmentId).FirstOrDefault();
                                        if (keyPadObjectInfo != null)
                                        {
                                            int commandTypeId = 1;
                                            int.TryParse(ArrayCharData[1], out commandTypeId);
                                            int productCode = 0;
                                            int.TryParse(ArrayCharData[4], out productCode);
                                            int total = 0;
                                            int.TryParse(ArrayCharData[5], out total);
                                            int quantityIncrease = 0;
                                            int.TryParse(ArrayCharData[3], out quantityIncrease);
                                            switch (commandTypeId)
                                            {
                                                case (int)eCommandRecive.ProductIncrease:
                                                case (int)eCommandRecive.ProductReduce:
                                                    {
                                                        int productOutputType = 0;
                                                        int.TryParse(ArrayCharData[2], out productOutputType);
                                                        switch (productOutputType)
                                                        {
                                                            case (int)eProductOutputType.KCS:
                                                                {
                                                                    if (KeypadQuantityProcessingType == 0)
                                                                    {
                                                                        if (commandTypeId == (int)eCommandRecive.ProductIncrease)
                                                                        {
                                                                            TangSanLuongKSC(keyPadObjectInfo.ClusterId, quantityIncrease, 0, 0, productCode, keyPadObjectInfo.LineId, keyPadObjectInfo.IsEndOfLine, total, equipmentId);
                                                                            //  KeyPadInsertProcessing(keyPadObjectInfo.ClusterId, quantityIncrease, 0, 0, productCode, keyPadObjectInfo.LineId, keyPadObjectInfo.IsEndOfLine, 0, total, equipmentId, true, (int)eProductOutputType.KCS);
                                                                        }
                                                                        else if (commandTypeId == (int)eCommandRecive.ProductReduce)
                                                                        {
                                                                            GiamSanLuongKSC(keyPadObjectInfo.ClusterId, quantityIncrease, 0, 0, productCode, keyPadObjectInfo.LineId, keyPadObjectInfo.IsEndOfLine, total, equipmentId);
                                                                            //  KeyPadInsertProcessing(keyPadObjectInfo.ClusterId, quantityIncrease, 0, 0, productCode, keyPadObjectInfo.LineId, keyPadObjectInfo.IsEndOfLine, 0, total, equipmentId, false, (int)eProductOutputType.KCS);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        var rs = BLLDayInfo.TinhSanLuongMoi(listMapIdSanPhamNgay, todayStr, (int)eProductOutputType.KCS, total, keyPadObjectInfo.IsEndOfLine, keyPadObjectInfo.ClusterId, 0, TypeOfCheckFinishProduction, getBTPInLineByType, 0, keyPadObjectInfo.LineId, equipmentId, productCode, 0);
                                                                        listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.HandlingSuccess + "," + productCode + ",," + (int)eProductOutputType.KCS);
                                                                        //if (rs.IsSuccess)
                                                                        //    listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.ChangeProductQuantity + "," + productCode + "," + rs.Data + "," + (int)eProductOutputType.KCS);
                                                                    }
                                                                    break;
                                                                }
                                                            case (int)eProductOutputType.TC:
                                                                {
                                                                    if (KeypadQuantityProcessingType == 0)
                                                                    {
                                                                        if (commandTypeId == (int)eCommandRecive.ProductIncrease)
                                                                        {
                                                                            TangSanLuongTC(keyPadObjectInfo.ClusterId, quantityIncrease, 0, 0, productCode, keyPadObjectInfo.LineId, keyPadObjectInfo.IsEndOfLine, total, equipmentId);
                                                                        }
                                                                        else if (commandTypeId == (int)eCommandRecive.ProductReduce)
                                                                        {
                                                                            GiamSanLuongTC(keyPadObjectInfo.ClusterId, quantityIncrease, 0, 0, productCode, keyPadObjectInfo.LineId, keyPadObjectInfo.IsEndOfLine, total, equipmentId);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        var rs = BLLDayInfo.TinhSanLuongMoi(listMapIdSanPhamNgay, todayStr, (int)eProductOutputType.TC, total, keyPadObjectInfo.IsEndOfLine, keyPadObjectInfo.ClusterId, 0, TypeOfCheckFinishProduction, getBTPInLineByType, 0, keyPadObjectInfo.LineId, equipmentId, productCode, 0);
                                                                        listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.HandlingSuccess + "," + productCode + ",," + (int)eProductOutputType.TC);
                                                                        //if (rs.IsSuccess)
                                                                        //    listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.ChangeProductQuantity + "," + productCode + "," + rs.Data + "," + (int)eProductOutputType.TC);
                                                                    }
                                                                    break;
                                                                }
                                                        }
                                                        break;
                                                    }
                                                case (int)eCommandRecive.ErrorIncrease:
                                                case (int)eCommandRecive.ErrorReduce:
                                                    {
                                                        int errorId = 0;
                                                        int.TryParse(ArrayCharData[2], out errorId);
                                                        if (KeypadQuantityProcessingType == 0)
                                                        {
                                                            if (commandTypeId == (int)eCommandRecive.ErrorIncrease)
                                                            {
                                                                TangSanLuongLoi(keyPadObjectInfo.ClusterId, quantityIncrease, 0, 0, productCode, keyPadObjectInfo.LineId, keyPadObjectInfo.IsEndOfLine, errorId, total, equipmentId);
                                                            }
                                                            else if (commandTypeId == (int)eCommandRecive.ErrorReduce)
                                                            {
                                                                GiamSanLuongLoi(keyPadObjectInfo.ClusterId, quantityIncrease, 0, 0, productCode, keyPadObjectInfo.LineId, keyPadObjectInfo.IsEndOfLine, errorId, total, equipmentId);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            var rs = BLLDayInfo.TinhSanLuongMoi(listMapIdSanPhamNgay, todayStr, (int)eProductOutputType.Error, total, keyPadObjectInfo.IsEndOfLine, keyPadObjectInfo.ClusterId, 0, TypeOfCheckFinishProduction, getBTPInLineByType, 0, keyPadObjectInfo.LineId, equipmentId, productCode, errorId);
                                                             listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.HandlingSuccess + "," + errorId + ",," + productCode);
                                                            if (rs.IsSuccess)
                                                                listDataSendKeyPad.AddRange(rs.DataSendKeyPads);
                                                        }
                                                        break;
                                                    }
                                                case (int)eCommandRecive.BTPIncrease:
                                                case (int)eCommandRecive.BTPReduce:
                                                    {
                                                        if (KeypadQuantityProcessingType == 0)
                                                        {
                                                            if (commandTypeId == (int)eCommandRecive.BTPIncrease)
                                                            {
                                                                TangBTP(keyPadObjectInfo.ClusterId, quantityIncrease, 0, 0, productCode, keyPadObjectInfo.LineId, keyPadObjectInfo.IsEndOfLine, 0, total, equipmentId);
                                                            }
                                                            else if (commandTypeId == (int)eCommandRecive.BTPReduce)
                                                            {
                                                                GiamBTP(keyPadObjectInfo.ClusterId, quantityIncrease, 0, 0, productCode, keyPadObjectInfo.LineId, keyPadObjectInfo.IsEndOfLine, 0, total, equipmentId);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            var rs = BLLDayInfo.TinhSanLuongMoi(listMapIdSanPhamNgay, todayStr, (int)eProductOutputType.BTP, total, keyPadObjectInfo.IsEndOfLine, keyPadObjectInfo.ClusterId, 0, TypeOfCheckFinishProduction, getBTPInLineByType, 0, keyPadObjectInfo.LineId, equipmentId, productCode, 0);
                                                            listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.HandlingSuccess + "," + productCode + ",,,");
                                                            //if (rs.IsSuccess)
                                                            // listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.ChangeBTPQuantities + "," + productCode + "," + rs.Data + "," + (int)eProductOutputType.TC);
                                                         //   listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.ChangeBTPQuantities + "," + productCode + "," + 33 + ",1");
                                                        }
                                                        break;
                                                    }
                                            }
                                        }
                                    }
                                    break;
                                }
                            #endregion
                            #region OneKeypadManyObject
                            case (int)eUseKeyPadType.OneKeyPadManyObject:
                                {
                                    //  MessageBox.Show("OneKeypadManyObject", ("arr" + ArrayCharData.Count()));
                                    for (int i = 2; i < ArrayCharData.Count() - 1; i += 2)
                                    {
                                        int quantityReceive = int.Parse(ArrayCharData[i + 1]);
                                        if (quantityReceive > 0)
                                        {

                                            if (AccountSuccess.isWriteLog)
                                                AccountSuccess.strError += "Có dữ liệu update từ dưới gửi lên \n";
                                            int sttNut = 0;
                                            int.TryParse(ArrayCharData[i], out sttNut);
                                            //
                                            //  MessageBox.Show("sl : " + quantityReceive, "nut : " + sttNut + "keyPadConfig.objs :" + keyPadConfig.objs.Count);

                                            // if (keyPadConfig.ListObjectConfig != null && keyPadConfig.ListObjectConfig.Count > 0)
                                            if (keyPadConfig.objs != null && keyPadConfig.objs.Count > 0)
                                            {
                                                // var keyPadObjectConfig = keyPadConfig.ListObjectConfig.Where(c => c.STTNut == sttNut).FirstOrDefault();
                                                var keyPadObjectConfig = keyPadConfig.objs.Where(c => c.STTNut == sttNut).FirstOrDefault();
                                                if (keyPadObjectConfig != null)
                                                {
                                                    //   MessageBox.Show("found button", "nut : " + sttNut);
                                                    switch (keyPadObjectConfig.CommandTypeId)
                                                    {
                                                        case (int)eCommandRecive.ProductKCSIncrease:
                                                            {// Tìm kiếm thông tin Mặt Hàng của chuyền trong ngày
                                                                TTidnotfinishfirst(keyPadObjectConfig.LineId, (int)eProductOutputType.KCS);
                                                                //    MessageBox.Show("ProductKCSIncrease", (dtIdNotFinishFirst.Rows.Count + ""));

                                                                // nếu tồn tại thì thực hiện update
                                                                if (dtIdNotFinishFirst.Rows.Count > 0)
                                                                {
                                                                    if (AccountSuccess.isWriteLog)
                                                                        AccountSuccess.strError += "Tìm thấy phân công cần sản xuất \n";
                                                                    int sttChuyen_SanPham = 0;
                                                                    int.TryParse(dtIdNotFinishFirst.Rows[0]["STT"].ToString(), out sttChuyen_SanPham);
                                                                    int maSanPham = 0;
                                                                    int.TryParse(dtIdNotFinishFirst.Rows[0]["MaSanPham"].ToString(), out maSanPham);
                                                                    var result = TangSanLuongKSC(keyPadObjectConfig.ClusterId, quantityReceive, sttChuyen_SanPham, maSanPham, 1, keyPadObjectConfig.LineId, keyPadObjectConfig.IsEndOfLine, 0, equipmentId);
                                                                    if (result)
                                                                    {
                                                                        if (AccountSuccess.isWriteLog)
                                                                            AccountSuccess.strError += "Update DB thành công \n";
                                                                    }
                                                                    else
                                                                    {
                                                                        if (AccountSuccess.isWriteLog)
                                                                            AccountSuccess.strError += "Update DB thất bại \n";
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    InformationPlay inf = new InformationPlay { SoundChuyen = keyPadObjectConfig.LineSound, Repeat = 1 };
                                                                    queuePlayFile.Enqueue(inf);
                                                                }
                                                                break;
                                                            }
                                                        case (int)eCommandRecive.ProductKSCReduce:
                                                            {
                                                                TTidnotfinishfirst(keyPadObjectConfig.LineId, (int)eProductOutputType.KCS);
                                                                MessageBox.Show("giam kcs", (dtIdNotFinishFirst.Rows.Count + ""));

                                                                if (dtIdNotFinishFirst.Rows.Count > 0)
                                                                {
                                                                    int sttChuyen_SanPham = 0;
                                                                    int.TryParse(dtIdNotFinishFirst.Rows[0]["STT"].ToString(), out sttChuyen_SanPham);
                                                                    int maSanPham = 0;
                                                                    int.TryParse(dtIdNotFinishFirst.Rows[0]["MaSanPham"].ToString(), out maSanPham);
                                                                    GiamSanLuongKSC(keyPadObjectConfig.ClusterId, quantityReceive, sttChuyen_SanPham, maSanPham, 0, keyPadObjectConfig.LineId, keyPadObjectConfig.IsEndOfLine, 0, 0);
                                                                }
                                                                break;
                                                            }
                                                        case (int)eCommandRecive.ProductTCIncrease:
                                                            {
                                                                TTidnotfinishfirst(keyPadObjectConfig.LineId, (int)eProductOutputType.TC);
                                                                //  MessageBox.Show("tang tc", (dtIdNotFinishFirst.Rows.Count + ""));

                                                                if (dtIdNotFinishFirst.Rows.Count > 0)
                                                                {
                                                                    int sttChuyen_SanPham = 0;
                                                                    int.TryParse(dtIdNotFinishFirst.Rows[0]["STT"].ToString(), out sttChuyen_SanPham);
                                                                    int maSanPham = 0;
                                                                    int.TryParse(dtIdNotFinishFirst.Rows[0]["MaSanPham"].ToString(), out maSanPham);
                                                                    TangSanLuongTC(keyPadObjectConfig.ClusterId, quantityReceive, sttChuyen_SanPham, maSanPham, 0, keyPadObjectConfig.LineId, keyPadObjectConfig.IsEndOfLine, 0, equipmentId);
                                                                }
                                                                break;
                                                            }
                                                        case (int)eCommandRecive.ProductTCReduce:
                                                            {
                                                                TTidnotfinishfirst(keyPadObjectConfig.LineId, (int)eProductOutputType.TC);
                                                                MessageBox.Show("giam tc", (dtIdNotFinishFirst.Rows.Count + ""));

                                                                if (dtIdNotFinishFirst.Rows.Count > 0)
                                                                {
                                                                    int sttChuyen_SanPham = 0;
                                                                    int.TryParse(dtIdNotFinishFirst.Rows[0]["STT"].ToString(), out sttChuyen_SanPham);
                                                                    int maSanPham = 0;
                                                                    int.TryParse(dtIdNotFinishFirst.Rows[0]["MaSanPham"].ToString(), out maSanPham);
                                                                    GiamSanLuongTC(keyPadObjectConfig.ClusterId, quantityReceive, sttChuyen_SanPham, maSanPham, 0, keyPadObjectConfig.LineId, keyPadObjectConfig.IsEndOfLine, 0, 0);
                                                                }
                                                                break;
                                                            }
                                                        case (int)eCommandRecive.ErrorIncrease:
                                                            {
                                                                TTidnotfinishfirst(keyPadObjectConfig.LineId, (int)eProductOutputType.TC);
                                                                MessageBox.Show("tang loi", (dtIdNotFinishFirst.Rows.Count + ""));

                                                                if (dtIdNotFinishFirst.Rows.Count > 0)
                                                                {
                                                                    int sttChuyen_SanPham = 0;
                                                                    int.TryParse(dtIdNotFinishFirst.Rows[0]["STT"].ToString(), out sttChuyen_SanPham);
                                                                    int maSanPham = 0;
                                                                    int.TryParse(dtIdNotFinishFirst.Rows[0]["MaSanPham"].ToString(), out maSanPham);
                                                                    TangSanLuongLoi(keyPadObjectConfig.ClusterId, quantityReceive, sttChuyen_SanPham, maSanPham, 0, keyPadObjectConfig.LineId, keyPadObjectConfig.IsEndOfLine, 1, 0, 0);
                                                                }
                                                                break;
                                                            }
                                                        case (int)eCommandRecive.ErrorReduce:
                                                            {
                                                                TTidnotfinishfirst(keyPadObjectConfig.LineId, (int)eProductOutputType.TC);
                                                                MessageBox.Show("giam loi", (dtIdNotFinishFirst.Rows.Count + ""));

                                                                if (dtIdNotFinishFirst.Rows.Count > 0)
                                                                {
                                                                    int sttChuyen_SanPham = 0;
                                                                    int.TryParse(dtIdNotFinishFirst.Rows[0]["STT"].ToString(), out sttChuyen_SanPham);
                                                                    int maSanPham = 0;
                                                                    int.TryParse(dtIdNotFinishFirst.Rows[0]["MaSanPham"].ToString(), out maSanPham);
                                                                    GiamSanLuongLoi(keyPadObjectConfig.ClusterId, quantityReceive, sttChuyen_SanPham, maSanPham, 0, keyPadObjectConfig.LineId, keyPadObjectConfig.IsEndOfLine, 1, 0, 0);
                                                                }
                                                                break;
                                                            }
                                                        case (int)eCommandRecive.BTPIncrease:
                                                            {
                                                                TTidnotfinishfirst(keyPadObjectConfig.LineId, (int)eProductOutputType.TC);
                                                                MessageBox.Show("tang btp", (dtIdNotFinishFirst.Rows.Count + ""));

                                                                if (dtIdNotFinishFirst.Rows.Count > 0)
                                                                {
                                                                    int sttChuyen_SanPham = 0;
                                                                    int.TryParse(dtIdNotFinishFirst.Rows[0]["STT"].ToString(), out sttChuyen_SanPham);
                                                                    int maSanPham = 0;
                                                                    int.TryParse(dtIdNotFinishFirst.Rows[0]["MaSanPham"].ToString(), out maSanPham);
                                                                    TangBTP(keyPadObjectConfig.ClusterId, quantityReceive, sttChuyen_SanPham, maSanPham, 0, keyPadObjectConfig.LineId, keyPadObjectConfig.IsEndOfLine, 1, 0, 0);
                                                                }
                                                                break;
                                                            }
                                                        case (int)eCommandRecive.BTPReduce:
                                                            {
                                                                MessageBox.Show("giam btp", (dtIdNotFinishFirst.Rows.Count + ""));
                                                                TTidnotfinishfirst(keyPadObjectConfig.LineId, (int)eProductOutputType.TC);
                                                                if (dtIdNotFinishFirst.Rows.Count > 0)
                                                                {
                                                                    int sttChuyen_SanPham = 0;
                                                                    int.TryParse(dtIdNotFinishFirst.Rows[0]["STT"].ToString(), out sttChuyen_SanPham);
                                                                    int maSanPham = 0;
                                                                    int.TryParse(dtIdNotFinishFirst.Rows[0]["MaSanPham"].ToString(), out maSanPham);
                                                                    GiamBTP(keyPadObjectConfig.ClusterId, quantityReceive, sttChuyen_SanPham, maSanPham, 0, keyPadObjectConfig.LineId, keyPadObjectConfig.IsEndOfLine, 1, 0, 0);
                                                                }
                                                                break;
                                                            }
                                                    }
                                                }
                                                else
                                                    MessageBox.Show("not found button", "nut : " + sttNut);
                                            }
                                        }
                                    }
                                    break;
                                }
                            #endregion
                        }
                    }
                }
                IsQuet = true;
            }
            catch (Exception)
            {
                //IsQuet = true;
                //this.richtextRec.EditValue = "";
                //countTimeCheckIsQuyet = 0;
                //GhiFileLog(DateTime.Now + " : Update table loi SX : Exception");
                //   ResetComPort();
            }
        }

        private void SendData(string value)
        {
            try
            {
                if (P.IsOpen)
                {
                    if (AccountSuccess.isWriteLog)
                        AccountSuccess.strError += "bắt đầu gửi dữ liệu gửi xuống qua cổng COM \n";
                    lbSendData.EditValue = value;
                    value = Helper.HelperControl.ConvertVN(value);
                    string strCS = "";
                    strCS = clsString.XOR(value);
                    value = value + strCS;
                    value = "02" + clsString.Ascii2HexStringNull(value) + "03";
                    byte[] newMsg = HexStringToByteArray(value);
                    P.Write(newMsg, 0, newMsg.Length);
                    CheckACK = true;
                    if (AccountSuccess.isWriteLog)
                        AccountSuccess.strError += "gửi dữ liệu gửi xuống thành công \n";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SendRequest(string value)
        {
            try
            {
                if (P2.IsOpen)
                {
                    lbSendRequest.Caption = value;
                    value = Helper.HelperControl.ConvertVN(value);
                    string strCS = "";
                    strCS = clsString.XOR(value);
                    value = value + strCS;
                    value = "02" + clsString.Ascii2HexStringNull(value) + "03";
                    byte[] newMsg = HexStringToByteArray(value);
                    P2.Write(newMsg, 0, newMsg.Length);
                }
            }
            catch
            {
                //   ResetComPort();
            }
        }

        private byte[] HexStringToByteArray(string s)
        {
            try
            {
                s = s.Replace(" ", "");
                if (s.Length % 2 != 0)
                    return new byte[] { 0x00 };
                byte[] buffer = new byte[s.Length / 2];
                for (int i = 0; i < s.Length; i += 2)
                    buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
                return buffer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        int current_Index_Send_Data_KP = 0,
             current_Quet_KP_Index = 0,
             timeshowmessage = -1,
             countTime = 0,
             countTimeCheckIsQuyet = 0,
             indexsend = -1;
        //  public string aaaaa = "";
        private void tmSenData_Tick(object sender, EventArgs e)
        {
            try
            {
                if (showmessage == true)
                {
                    timeshowmessage++;
                    if (timeshowmessage > 25)
                    {
                        ms.Close();
                        timeshowmessage = -1;
                        showmessage = false;
                    }
                }

                if (isSendRequest)
                {
                    if (IsQuet)
                    {
                        if (P2.IsOpen)
                        {
                            if (listDataSendKeyPad.Count > 0)
                            {
                                if (current_Index_Send_Data_KP < listDataSendKeyPad.Count())
                                {
                                    string keypab = listDataSendKeyPad[current_Index_Send_Data_KP];
                                    string send = keypab + ",";
                                    SendRequest(send);

                                    if (current_Index_Send_Data_KP == listDataSendKeyPad.Count() - 1)
                                    {
                                        current_Index_Send_Data_KP = 0;
                                        listDataSendKeyPad.Clear();
                                    }
                                    else
                                        current_Index_Send_Data_KP++;
                                    //  GhiFileLog("id keypad : " + aaaaa);
                                }
                            }
                            else
                            {
                                if (ListKeyPab != null && current_Quet_KP_Index < ListKeyPab.Count())
                                {
                                    string send = ListKeyPab[current_Quet_KP_Index] + ",4,,";
                                    lbSendRequest.Caption = send;
                                    SendRequest(send);

                                    if (current_Quet_KP_Index == ListKeyPab.Count() - 1)
                                        current_Quet_KP_Index = 0;
                                    else
                                        current_Quet_KP_Index++;
                                }
                            }
                        }
                    }
                    else
                    {
                        countTimeCheckIsQuyet += 100;
                        if (countTimeCheckIsQuyet > 500)
                        {
                            IsQuet = true;
                            countTimeCheckIsQuyet = 0;
                            richtextRec.EditValue = "";
                        }
                    }
                }

                if (isReadSound == 1)
                {
                    CheckTimeReadSound();
                    CheckTimeReadSoundError();
                }
                if (isAutoTurnOnOffCOM == 1)
                    CheckTurnOnOffCOM();

                if (AccountSuccess.isWriteLog)
                    AccountSuccess.strError += "Vào timer de ktra va gui dữ liệu xuông bảng điện tử \n";
                if (IsSend == true)
                {
                    if (AccountSuccess.isWriteLog)
                        AccountSuccess.strError += "IsSend=true: cho phép gửi xuống bảng \n";
                    if (P.IsOpen)
                    {
                        if (AccountSuccess.isWriteLog)
                            AccountSuccess.strError += "COM bảng hoạt động \n";
                        if (!CheckACK)
                        {
                            if (AccountSuccess.isWriteLog)
                                AccountSuccess.strError += "Bắt đầu gửi data xuông bảng \n";
                            indexsend++;
                            if (indexsend < listData.Count)
                            {
                                if (AccountSuccess.isWriteLog)
                                    AccountSuccess.strError += "Ktra có dữ liệu gửi xuống \n";
                                listDataSending.AddRange(listData);
                                if (!isDataSending)
                                {
                                    threadSendData = new Thread(sendData);
                                    threadSendData.Start();
                                }
                                listData.Clear();
                            }
                            else
                            {
                                IsSend = false;
                                listData.Clear();
                                countTimeCheckIsQuyet = 0;
                            }

                        }
                        else
                        {
                            if (AccountSuccess.isWriteLog)
                                AccountSuccess.strError += "check ACK \n";
                            countTime += 100;
                            if (countTime < timeoutcheckACK)
                            {
                                string ack = clsString.Ascii2HexStringNull(richtextRec2.Edit != null ? richtextRec2.Edit.ToString() : string.Empty);
                                if (ack == "06" || ack == "06 06" || ack == "06 06 06" || ack == "06 06 06 06" || ack == "06 06 06 06 06" || ack == "06 06 06 06 06 06")
                                {
                                    if (AccountSuccess.isWriteLog)
                                        AccountSuccess.strError += "nhân được ACK \n";
                                    richtextRec.EditValue = "";
                                    countTime = 0;
                                    CheckACK = false;
                                }
                                else
                                    richtextRec2.EditValue = "";
                            }
                            else
                            {
                                if (AccountSuccess.isWriteLog)
                                    AccountSuccess.strError += "không nhân được ACK, reset lại biến để ctrinh chạy tiếp \n";
                                countTime = 0;
                                CheckACK = false;
                                richtextRec2.EditValue = "";
                            }
                        }
                    }
                    else
                    {
                        if (AccountSuccess.isWriteLog)
                            AccountSuccess.strError += "COM bảng không hoạt động, reset lại biến \n";
                        IsSend = false;
                        listData.Clear();
                        countTimeCheckIsQuyet = 0;
                        try
                        {
                            if (!P.IsOpen)
                                P.Open();
                        }
                        catch (Exception)
                        {
                            if (AccountSuccess.isWriteLog)
                                AccountSuccess.strError += "Không mở lại kết nối COM bảng được \n";
                        }
                    }
                }
                //IsSend = false;
                //listData.Clear();
                //countTimeCheckIsQuyet = 0;
            }
            catch (Exception ex)
            {
                //  intErrorTimerSendData++;
                //   if (intErrorTimerSendData > intMaxErrorOfTimer)
                //     tmSenData.Enabled = false;
                //  MessageBox.Show("Lỗi tmSendData: " + ex.Message + ". Hệ thống sẽ tắt tiến trình tự động.");
                // GhiFileLog(DateTime.Now + "tmSenData_Tick : exception");
                //  ResetComPort();
            }
        }

        private void sendData()
        {
            while (listDataSending.Count > 0)
            {
                SendData(listDataSending[0]);
                listDataSending.RemoveAt(0);
                Thread.Sleep(300);
            }
            isDataSending = false;
        }


        private void SetMauDen(InformationPosition position, string DataColor)
        {
            string Send = string.Empty;
            try
            {
                InformationCell cellcolor = position.listCell[0];
                if (cellcolor.IDBang != "")
                {
                    if (cellcolor.IDMat != "")
                    {
                        foreach (var obj in strSendListData)
                        {
                            if (obj.IdBang == int.Parse(cellcolor.IDBang) && obj.IdMat == int.Parse(cellcolor.IDMat))
                            {
                                obj.Data += cellcolor.IDCell + ",D" + DataColor.ToString() + ",";
                                objExist = true;
                                break;
                            }
                        }
                        if (!objExist)
                        {
                            strSendListData.Add(new ModelDataSendTable()
                            {
                                IdBang = string.IsNullOrEmpty(cellcolor.IDBang) ? 0 : int.Parse(cellcolor.IDBang),
                                IdMat = string.IsNullOrEmpty(cellcolor.IDMat) ? 0 : int.Parse(cellcolor.IDMat),
                                Data = cellcolor.IDBang + ",4," + cellcolor.IDCell + ",D" + DataColor.ToString() + ","
                            });
                        }
                        objExist = false;
                    }
                    else
                    {
                        Send = cellcolor.IDBang + ",4," + cellcolor.IDCell + ",D" + DataColor.ToString() + ",";
                        listData.Add(Send);
                    }
                }
                else
                {
                    if (cellcolor.IDMat != "")
                    {
                        Send = cellcolor.IDCell + ",4,D" + DataColor.ToString() + ",";
                        listData.Add(Send);
                    }
                    else
                    {
                        Send = cellcolor.IDCell + ",6,,";
                        listData.Add(Send);
                        Send = cellcolor.IDCell + ",4,D" + DataColor.ToString() + ",";
                        listData.Add(Send);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FinishLoadData()
        {
            string strSQL = "update NangXuat set IsChange =0 where IsChange =1 and Ngay ='" + todayStr + "' And STTCHuyen_SanPham in (select STT from Chuyen_SanPham WHERE Chuyen_SanPham.MaChuyen IN (" + AccountSuccess.strListChuyenId + ")) and IsDeleted=0";
            try
            {
                if (sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }
                sqlCon.Open();
                sqlCom = new SqlCommand(strSQL, sqlCon);
                int sodong = sqlCom.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        List<ModelDataSendTable> strSendListData = new List<ModelDataSendTable>();
        bool objExist = false;

        /// <summary>
        /// Ktra row nào thay đổi thông tin năng suất thì lấy thông tin và gửi xuống bảng điện tử
        /// </summary>
        private void LoadDataTPFromDB()
        {
            try
            {
                dtLoadToTable.Clear();
                strSendListData.Clear();
                objExist = false;
                string strSQL = string.Empty;
                if (hienThiDenTheoTPThoatChuyen == 0)
                    strSQL = "select nx.STTChuyen_SanPham, case when nx.NhipDoThucTe = 0 then 0 else ROUND((nx.NhipDoSanXuat/nx.NhipDoThucTe)*100,0)end  TiLeHienDen ,csp.MaChuyen, c.IdDenNangSuat, csp.NangXuatSanXuat, tp.LaoDongChuyen, nx.DinhMucNgay, (nx.ThucHienNgay-nx.ThucHienNgayGiam) ThucHienNgay, CASE WHEN nx.DinhMucNgay <= 0 THEN 0 ELSE (ROUND((((nx.ThucHienNgay-nx.ThucHienNgayGiam)/nx.DinhMucNgay)*100),0)) end TyLeThucHien, csp.LuyKeTH, nx.BTPTrenChuyen, nx.NhipDoThucTe, (nx.BTPTrenChuyen/tp.LaoDongChuyen) BTPTrenLD, (nx.BTPThoatChuyenNgay-nx.BTPThoatChuyenNgayGiam) BTPThoatChuyenNgay, csp.LuyKeBTPThoatChuyen, nx.SanLuongLoi, nx.NhipDoThucTeBTPThoatChuyen, ROUND(((nx.ThucHienNgay-nx.ThucHienNgayGiam)*sp.DonGia),0) DoanhThuNgay from NangXuat nx, Chuyen_SanPham csp, ThanhPham tp, Chuyen c, SanPham sp Where csp.MaSanPham=sp.MaSanPham and nx.IsDeleted=0 and nx.IsChange = 1 and nx.Ngay ='" + todayStr + "' AND nx.STTChuyen_SanPham = csp.STT AND csp.STT = tp.STTChuyen_SanPham AND nx.Ngay = tp.Ngay AND csp.MaChuyen = c.MaChuyen AND c.FloorId ='" + AccountSuccess.IdFloor + "'";
                else
                    strSQL = "select nx.STTChuyen_SanPham, case when nx.NhipDoThucTeBTPThoatChuyen = 0 then 0 else ROUND((nx.NhipDoSanXuat/nx.NhipDoThucTeBTPThoatChuyen)*100,0)end  TiLeHienDen ,csp.MaChuyen, c.IdDenNangSuat, csp.NangXuatSanXuat, tp.LaoDongChuyen, nx.DinhMucNgay, (nx.ThucHienNgay-nx.ThucHienNgayGiam) ThucHienNgay, CASE WHEN nx.DinhMucNgay <= 0 THEN 0 ELSE (ROUND((((nx.ThucHienNgay-nx.ThucHienNgayGiam)/nx.DinhMucNgay)*100),0)) end TyLeThucHien, csp.LuyKeTH, nx.BTPTrenChuyen, nx.NhipDoThucTe, (nx.BTPTrenChuyen/tp.LaoDongChuyen) BTPTrenLD, (nx.BTPThoatChuyenNgay-nx.BTPThoatChuyenNgayGiam) BTPThoatChuyenNgay, csp.LuyKeBTPThoatChuyen, nx.SanLuongLoi, nx.NhipDoThucTeBTPThoatChuyen, ROUND(((nx.ThucHienNgay-nx.ThucHienNgayGiam)*sp.DonGia),0) DoanhThuNgay from NangXuat nx, Chuyen_SanPham csp, ThanhPham tp, Chuyen c, SanPham sp Where csp.MaSanPham=sp.MaSanPham and nx.IsChange = 1 and nx.IsDeleted=0 and nx.Ngay ='" + todayStr + "' AND nx.STTChuyen_SanPham = csp.STT AND csp.STT = tp.STTChuyen_SanPham AND nx.Ngay = tp.Ngay AND csp.MaChuyen = c.MaChuyen AND c.FloorId ='" + AccountSuccess.IdFloor + "'";
                if (sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }
                sqlCon.Open();
                da = new SqlDataAdapter(strSQL, sqlCon);
                da.Fill(dtLoadToTable);
                if (dtLoadToTable != null && dtLoadToTable.Rows.Count > 0)
                {
                    FinishLoadData();
                    for (int i = 0; i < dtLoadToTable.Rows.Count; i++)
                    {
                        string sttChuyen_SanPham = dtLoadToTable.Rows[i]["STTChuyen_SanPham"].ToString();

                        string MaChuyen = dtLoadToTable.Rows[i]["MaChuyen"].ToString();

                        double NangSuatSanXuat = 0;
                        double.TryParse(dtLoadToTable.Rows[i]["NangXuatSanXuat"].ToString(), out NangSuatSanXuat);

                        int LaoDongChuyen = 0;
                        int.TryParse(dtLoadToTable.Rows[i]["LaoDongChuyen"].ToString(), out LaoDongChuyen);


                        var TotalSecond = (int)Helper.HelperControl.TimeIsWorkAllDayOfLine(BLLShift.GetShiftsOfLine(Convert.ToInt32(MaChuyen)), NSType).TotalSeconds;

                        if (NangSuatSanXuat != 0 && LaoDongChuyen != 0)
                        {
                            dtLoadToTable.Rows[i]["NangXuatSanXuat"] = Math.Round(((TotalSecond / NangSuatSanXuat) * LaoDongChuyen), 0).ToString();
                        }
                        else
                        {
                            dtLoadToTable.Rows[i]["NangXuatSanXuat"] = "0";
                        }

                        var listDataSent = new List<string>();
                        for (int j = 1; j < dtLoadToTable.Columns.Count; j++)
                        {
                            listDataSent.Add(dtLoadToTable.Rows[i][j].ToString());
                        }

                        //San luong cua cac cum trong chuyen
                        var listSanLuongCum = nangSuatCumDAO.GetSanLuongCumCuaChuyen(todayStr, sttChuyen_SanPham);
                        if (listSanLuongCum != null && listSanLuongCum.Count > 0)
                            listDataSent.AddRange(listSanLuongCum);

                        if (listChuyen != null && listChuyen.Count > 0)
                        {
                            var infChuyen = new InformationChuyen();
                            var lineId = int.Parse(MaChuyen);
                            foreach (var chuyen in listChuyen_O)
                            {
                                if (chuyen.MaChuyen == lineId.ToString())
                                {
                                    infChuyen = chuyen;
                                    break;
                                }
                            }
                            if (infChuyen != null)
                            {
                                int idDenNS = 0;
                                if (!string.IsNullOrEmpty(dtLoadToTable.Rows[i]["IdDenNangSuat"].ToString()))
                                {
                                    idDenNS = int.Parse(dtLoadToTable.Rows[i]["IdDenNangSuat"].ToString());
                                }
                                int STTDen = ColorDenForKanBan(float.Parse(dtLoadToTable.Rows[i]["TiLeHienDen"].ToString()), idDenNS).STT;
                                List<InformationPosition> listposition = infChuyen.listPosition;
                                int tt = 5;
                                string Send = "";
                                foreach (var position in listposition)
                                {
                                    if (position.ChangeTP == 1)
                                    {
                                        //Set mau led
                                        if (position.STTDen != 0)
                                        {
                                            if (position.STTDen == STTDen)
                                            {
                                                SetMauDen(position, "8888");
                                            }
                                            else
                                            {
                                                SetMauDen(position, "");
                                            }
                                        }
                                        else
                                        {
                                            tt++;
                                            if (tt < listDataSent.Count)
                                            {
                                                int Data = 0;
                                                if (listDataSent[tt] != null && !string.IsNullOrEmpty(listDataSent[tt]))
                                                    int.TryParse(listDataSent[tt], out Data);
                                                if (position.listCell != null && position.listCell.Count > 0)
                                                {
                                                    var cell = position.listCell[0];
                                                    int soled = cell.SoLed;
                                                    string max = "";
                                                    for (int t = 0; t < soled; t++)
                                                    {
                                                        max += "9";
                                                    }
                                                    if (Data <= int.Parse(max))
                                                    {
                                                        if (!string.IsNullOrEmpty(cell.IDBang))
                                                        {
                                                            if (!string.IsNullOrEmpty(cell.IDMat))
                                                            {
                                                                if (strSendListData.Count > 0)
                                                                {
                                                                    foreach (var obj in strSendListData)
                                                                    {
                                                                        if (obj.IdBang == int.Parse(cell.IDBang) && obj.IdMat == int.Parse(cell.IDMat))
                                                                        {
                                                                            obj.Data += cell.IDCell + ",D" + Data.ToString() + ","; ;
                                                                            objExist = true;
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                                if (!objExist)
                                                                {
                                                                    strSendListData.Add(new ModelDataSendTable()
                                                                    {
                                                                        IdBang = string.IsNullOrEmpty(cell.IDBang) ? 0 : int.Parse(cell.IDBang),
                                                                        IdMat = string.IsNullOrEmpty(cell.IDMat) ? 0 : int.Parse(cell.IDMat),
                                                                        Data = cell.IDBang + ",4," + cell.IDCell + ",D" + Data.ToString() + ","
                                                                    });
                                                                }
                                                                objExist = false;
                                                            }
                                                            else
                                                            {
                                                                Send = cell.IDBang + cell.IDCell + ",4,D" + Data.ToString() + ",";
                                                                listData.Add(Send);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (!string.IsNullOrEmpty(cell.IDMat))
                                                            {
                                                                Send = cell.IDMat + "," + cell.IDCell + ",4,D" + Data.ToString() + ",";
                                                                listData.Add(Send);
                                                            }
                                                            else
                                                            {
                                                                Send = cell.IDCell + ",6,,";
                                                                listData.Add(Send);
                                                                Send = cell.IDCell + ",4,D" + Data.ToString() + ",";
                                                                listData.Add(Send);
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        string first = Data.ToString().Substring(0, Data.ToString().Length - soled);
                                                        string last = Data.ToString().Substring(Data.ToString().Length - soled, Data.ToString().Length - (Data.ToString().Length - soled));
                                                        if (!string.IsNullOrEmpty(cell.IDBang))
                                                        {
                                                            if (!string.IsNullOrEmpty(cell.IDMat))
                                                            {
                                                                Send = cell.IDBang + ",4," + cell.IDMat + "," + cell.IDCell + ",D" + first + ",";
                                                                listData.Add(Send);
                                                                if (position.listCell.Count > 1)
                                                                {
                                                                    Send = cell.IDBang + ",4," + cell.IDMat + "," + position.listCell[1].IDCell + ",D" + last + ",";
                                                                    listData.Add(Send);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                Send = cell.IDBang + ",4," + cell.IDCell + ",D" + first + ",";
                                                                listData.Add(Send);
                                                                if (position.listCell.Count > 1)
                                                                {
                                                                    Send = cell.IDBang + ",4," + position.listCell[1].IDCell + ",D" + last + ",";
                                                                    listData.Add(Send);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (!string.IsNullOrEmpty(cell.IDMat))
                                                            {
                                                                Send = cell.IDMat + "," + cell.IDCell + ",4,D" + first + ",";
                                                                listData.Add(Send);
                                                                if (position.listCell.Count > 1)
                                                                {
                                                                    Send = cell.IDMat + ",4," + position.listCell[1].IDCell + ",D" + last + ",";
                                                                    listData.Add(Send);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                Send = cell.IDCell + ",6,,";
                                                                listData.Add(Send);
                                                                Send = cell.IDCell + ",4,D" + first + ",";
                                                                listData.Add(Send);
                                                                if (position.listCell.Count > 1)
                                                                {
                                                                    Send = position.listCell[1].IDCell + ",6,,";
                                                                    listData.Add(Send);
                                                                    Send = position.listCell[1].IDCell + ",4,D" + last + ",";
                                                                    listData.Add(Send);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                foreach (var obj in strSendListData)
                                {
                                    listData.Add(obj.Data);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        /// <summary>
        /// Ktra row nào thay đổi thông tin năng suất thì lấy thông tin và gửi xuống bảng điện tử
        /// 
        /// </summary>
        private void CheckProductionChangeAndResendDataToBangDienTu()
        {
            try
            {
                dtLoadToTable.Clear();
                strSendListData.Clear();
                var assignInDay = BLLAssignmentForLine.Instance.GetAssignmentModel_BangDienTu(todayStr, (hienThiDenTheoTPThoatChuyen == 0 ? false : true));
                if (assignInDay.Count > 0)
                {
                    var listDataSent = new List<string>();
                    AssignmentModel_BangDienTu assignOut = null;
                    foreach (var item in assignInDay.Where(x => x.IsChange))
                    {
                        // assignOut = new AssignmentModel_BangDienTu();
                        if (TypeOfShowProductToLCD == 2)
                            assignOut = assignInDay.FirstOrDefault(x => x.IsShowLCD && x.lineId == item.lineId);

                        // Trong truong hop neu chon show theo config ma ko co ma hang nao set show thi lay cai dang change ma show len
                        if (assignOut == null)
                            assignOut = item;
                        #region
                        var TotalSecond = (int)Helper.HelperControl.TimeIsWorkAllDayOfLine(BLLShift.GetShiftsOfLine(item.lineId), NSType).TotalSeconds;
                        assignOut.TimeProductPerItem = (assignOut.TimeProductPerItem > 0 && assignOut.Labours > 0 && TotalSecond > 0) ? Math.Round(((TotalSecond / assignOut.TimeProductPerItem) * assignOut.Labours), 0) : 0;

                        listDataSent.Clear();
                        //San luong cua cac cum trong chuyen
                        var listSanLuongCum = BLLProductivity.GetSanLuongCumCuaChuyen(todayStr, assignOut.AssignId);
                        if (listSanLuongCum != null && listSanLuongCum.Count > 0)
                            listDataSent.AddRange(listSanLuongCum);
                        #endregion

                        #region xử lý gửi dữ liệu xuống Bảng điện tử
                        if (listChuyen_O != null && listChuyen_O.Count > 0)
                        {
                            var infChuyen = listChuyen_O.FirstOrDefault(x => x.MaChuyen == assignOut.lineId.ToString());
                            if (infChuyen != null && infChuyen.MaChuyen != "3")
                            {
                                int idDenNS = assignOut.LightPercentId ?? 0;
                                int STTDen = ColorDenForKanBan(assignOut.lightPercent, idDenNS).STT;
                                List<InformationPosition> listposition = infChuyen.listPosition;
                                int positionIndex = 1, Data = 0;
                                string Send = "", DataStr = string.Empty;
                                foreach (var position in listposition)
                                {
                                    // if (position.ChangeTP == 1)
                                    //  {
                                    //Set mau led
                                    if (position.STTDen != 0)
                                    {
                                        if (position.STTDen == STTDen)
                                            SetMauDen(position, "8888");
                                        else
                                            SetMauDen(position, "");
                                    }
                                    else
                                    {
                                        #region

                                        Data = 0;
                                        switch (positionIndex)
                                        {
                                            case 6: DataStr = assignOut.CommoName; break;
                                            case 7: Data = assignOut.ProductionPlans; break;
                                            case 8:
                                                if (calculateNormsdayType == 0)
                                                    Data = (int)assignOut.ProductionNorms;
                                                else
                                                    Data = (int)assignInDay.Where(x => x.lineId == assignOut.lineId).Sum(x => x.ProductionNorms);
                                                break;
                                            //  case 9: Data = (int)assignOut.LK_KCS; break;
                                            case 9: Data = (int)Math.Round(assignOut.KCSPercent, 0); break;
                                            case 10: Data = assignInDay.Where(x => x.lineId == assignOut.lineId).Sum(x => x.KCSInDay); break;
                                            case 11: Data = assignOut.Labours; break;
                                            case 12: Data = assignInDay.Where(x => x.lineId == assignOut.lineId).Sum(x => x.BTPInDay); break;
                                            case 13: Data = assignOut.LK_BTP; break;
                                            case 14: Data = assignOut.BTPInLine; break;
                                            case 15: Data = (int)Math.Round(assignOut.ProductionPace, 0); break;
                                            case 16: Data = (int)Math.Round(assignOut.CurrentPace, 0); break;
                                            case 17: Data = (int)Math.Round(assignOut.BTPPerLabour, 0); break;
                                            case 18: Data = assignInDay.Where(x => x.lineId == assignOut.lineId).Sum(x => x.TCInDay); break;
                                            case 19: Data = assignInDay.Where(x => x.lineId == assignOut.lineId).Sum(x => x.ErrorInDay); break;
                                            case 20: Data = (int)Math.Round(assignOut.Current_TC_Pace, 0); break;
                                            case 21: Data = (int)Math.Round(assignOut.SalesDate, 0); break;
                                            case 22: Data = (int)Math.Round(assignOut.Lean, 0); break;
                                            case 23: Data = assignInDay.Where(x => x.lineId == item.lineId).Sum(x => x.KCSInHours); break;
                                            case 24: Data = assignInDay.Where(x => x.lineId == item.lineId).Sum(x => x.TCInHours); break;
                                        }

                                        InformationCell cell;
                                        if (position.IsInt == 1)
                                        {
                                            #region
                                            if (position.listCell != null && position.listCell.Count > 0)
                                            {
                                                cell = position.listCell[0];
                                                int soled = cell.SoLed;
                                                string max = "";
                                                for (int t = 0; t < soled; t++)
                                                    max += "9";
                                                #region
                                                if (Data <= int.Parse(max))
                                                {
                                                    if (!string.IsNullOrEmpty(cell.IDBang))
                                                    {
                                                        if (!string.IsNullOrEmpty(cell.IDMat))
                                                        {
                                                            if (strSendListData.Count > 0)
                                                            {
                                                                foreach (var obj in strSendListData)
                                                                {
                                                                    if (obj.IdBang == int.Parse(cell.IDBang) && obj.IdMat == int.Parse(cell.IDMat))
                                                                    {
                                                                        obj.Data += cell.IDCell + ",D" + Data.ToString() + ",";
                                                                        if (positionIndex == 11 || positionIndex == 16 || positionIndex == 21)
                                                                            obj.Data += "|" + obj.IdBang + ",4,";

                                                                        objExist = true;
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                            if (!objExist)
                                                            {
                                                                strSendListData.Add(new ModelDataSendTable()
                                                                {
                                                                    IdBang = string.IsNullOrEmpty(cell.IDBang) ? 0 : int.Parse(cell.IDBang),
                                                                    IdMat = string.IsNullOrEmpty(cell.IDMat) ? 0 : int.Parse(cell.IDMat),
                                                                    Data = cell.IDBang + ",4," + cell.IDCell + ",D" + Data.ToString() + ","
                                                                });
                                                            }
                                                            objExist = false;
                                                        }
                                                        else
                                                        {
                                                            Send = cell.IDBang + cell.IDCell + ",4,D" + Data.ToString() + ",";
                                                            listData.Add(Send);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (!string.IsNullOrEmpty(cell.IDMat))
                                                        {
                                                            Send = cell.IDMat + "," + cell.IDCell + ",4,D" + Data.ToString() + ",";
                                                            listData.Add(Send);
                                                        }
                                                        else
                                                        {
                                                            Send = cell.IDCell + ",6,,";
                                                            listData.Add(Send);
                                                            Send = cell.IDCell + ",4,D" + Data.ToString() + ",";
                                                            listData.Add(Send);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    #region
                                                    string first = Data.ToString().Substring(0, Data.ToString().Length - soled);
                                                    string last = Data.ToString().Substring(Data.ToString().Length - soled, Data.ToString().Length - (Data.ToString().Length - soled));
                                                    if (!string.IsNullOrEmpty(cell.IDBang))
                                                    {
                                                        if (!string.IsNullOrEmpty(cell.IDMat))
                                                        {
                                                            Send = cell.IDBang + ",4," + cell.IDMat + "," + cell.IDCell + ",D" + first + ",";
                                                            listData.Add(Send);
                                                            if (position.listCell.Count > 1)
                                                            {
                                                                Send = cell.IDBang + ",4," + cell.IDMat + "," + position.listCell[1].IDCell + ",D" + last + ",";
                                                                listData.Add(Send);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Send = cell.IDBang + ",4," + cell.IDCell + ",D" + first + ",";
                                                            listData.Add(Send);
                                                            if (position.listCell.Count > 1)
                                                            {
                                                                Send = cell.IDBang + ",4," + position.listCell[1].IDCell + ",D" + last + ",";
                                                                listData.Add(Send);
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (!string.IsNullOrEmpty(cell.IDMat))
                                                        {
                                                            Send = cell.IDMat + "," + cell.IDCell + ",4,D" + first + ",";
                                                            listData.Add(Send);
                                                            if (position.listCell.Count > 1)
                                                            {
                                                                Send = cell.IDMat + ",4," + position.listCell[1].IDCell + ",D" + last + ",";
                                                                listData.Add(Send);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Send = cell.IDCell + ",6,,";
                                                            listData.Add(Send);
                                                            Send = cell.IDCell + ",4,D" + first + ",";
                                                            listData.Add(Send);
                                                            if (position.listCell.Count > 1)
                                                            {
                                                                Send = position.listCell[1].IDCell + ",6,,";
                                                                listData.Add(Send);
                                                                Send = position.listCell[1].IDCell + ",4,D" + last + ",";
                                                                listData.Add(Send);
                                                            }
                                                        }
                                                    }
                                                    #endregion
                                                }
                                                #endregion

                                            }
                                            #endregion
                                        }
                                        else if (position.IsInt != 1 && positionIndex == 6)
                                        {
                                            #region Set mau led
                                            if (position.STTDen == 0)
                                            {
                                                cell = position.listCell[0];
                                                if (cell.IDBang != "")
                                                {
                                                    if (cell.IDMat != "")
                                                    {
                                                        Send = cell.IDBang + ",4," + cell.IDCell + ",D" + DataStr + ",";
                                                        listData.Add(Send);
                                                    }
                                                    else
                                                    {
                                                        Send = cell.IDBang + "," + cell.IDCell + ",4,D" + DataStr + ",";
                                                        listData.Add(Send);
                                                    }
                                                }
                                                else
                                                {
                                                    if (cell.IDMat != "")
                                                    {
                                                        Send = cell.IDMat + "," + cell.IDCell + ",4,D" + DataStr + ",";
                                                        listData.Add(Send);
                                                    }
                                                    else
                                                    {
                                                        Send = cell.IDCell + ",6,,";
                                                        listData.Add(Send);
                                                        Send = cell.IDCell + ",4,D" + DataStr + ",";
                                                        listData.Add(Send);
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                        #endregion
                                    }
                                    //   }
                                    positionIndex++;
                                }

                                foreach (var obj in strSendListData)
                                {
                                    barEditItem2.EditValue = obj.Data;
                                    var str = obj.Data.Split('|').ToList();
                                    foreach (var c in str)
                                    {
                                        listData.Add(c);
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            { }
        }



        List<int> lstInt = new List<int>();


        private void LoadDataBTPForKanBan(int STTChuyen_SanPham)
        {
            try
            {
                strSendListData.Clear();
                objExist = false;
                var cspObj = BLLProductivity.GetProductivitiesInDayForKanban(STTChuyen_SanPham, todayStr);
                if (cspObj != null)
                {
                    lstInt.Clear();
                    string machuyen = cspObj.MaChuyen.ToString();
                    var infChuyen = listChuyen_O.FirstOrDefault(x => x.MaChuyen == machuyen);
                    if (infChuyen != null)
                    {
                        int tylebtp = (int)cspObj.Percent_TH;
                        int BTPNgay = cspObj.BTP_Day - cspObj.BTP_Day_G;
                        lstInt.Add(BTPNgay);

                        int sanLuongKeHoach = cspObj.SanLuongKeHoach;
                        lstInt.Add(sanLuongKeHoach);

                        int BTPLoi = cspObj.BTPLoi;

                        int LuyKeBTP = cspObj.LK_BTP;
                        LuyKeBTP = LuyKeBTP - BTPLoi;
                        lstInt.Add(LuyKeBTP);

                        int BTPTrenChuyen = cspObj.BTPInLine;
                        lstInt.Add(BTPTrenChuyen);

                        int luyKeTH = cspObj.LuyKeTH;
                        int dinhMucNgay = (int)cspObj.NormsDay;
                        string filewavChuyen = cspObj.Sound;

                        bool isEnddate = cspObj.IsEndDate;
                        var readPercentObj = BLLReadPercent.Instance.Get(cspObj.STT, tylebtp);
                        int IdDenParent = cspObj.IdDen ?? 0,
                            IdTyLeDoc = readPercentObj != null ? readPercentObj.IdParent.Value : 0;
                        ThongTinDen ttden;
                        if (!isEnddate)
                        {
                            if (IdTyLeDoc != 0 && cspObj.LuyKeBTPThoatChuyen > 0)
                                readpercent_short(filewavChuyen, readPercentObj.Sound, filewavSlient, readPercentObj.CountRepeat);

                            ttden = ColorDenForKanBan(tylebtp, IdDenParent);
                        }
                        else
                        {
                            if ((sanLuongKeHoach - luyKeTH) - BTPTrenChuyen > 0)
                            {
                                if (IdTyLeDoc != 0 && cspObj.LuyKeBTPThoatChuyen > 0)
                                    readpercent_short(filewavChuyen, readPercentObj.Sound, filewavSlient, readPercentObj.CountRepeat);
                                ttden = ColorDenForKanBan(tylebtp, IdDenParent);
                            }
                            else
                                ttden = new ThongTinDen() { STT = 6, Color = "Xanh" };
                        }


                        #region doc gui Led
                        if (isHienThiRaManHinhLCD == 0)
                        {
                            var listposition = infChuyen.listPosition;
                            int tt = -1;
                            string Send = "";
                            foreach (var position in listposition)
                            {
                                if (position.ChangeBTP == 1)
                                {
                                    if (position.STTDen != 0)
                                    {
                                        if (position.STTDen == ttden.STT)
                                            SetMauDen(position, "8888");
                                        else
                                            SetMauDen(position, "");
                                    }
                                    ////
                                    else
                                    {
                                        #region
                                        tt++;
                                        if (tt < lstInt.Count && position.listCell.Count > 0)
                                        {
                                            int Data = lstInt[tt];
                                            var cell = position.listCell[0];
                                            int soled = cell.SoLed;
                                            string max = "";
                                            for (int t = 0; t < soled; t++)
                                            {
                                                max += "9";
                                            }
                                            if (Data < int.Parse(max))
                                            {
                                                if (!string.IsNullOrEmpty(cell.IDBang))
                                                {
                                                    if (!string.IsNullOrEmpty(cell.IDMat))
                                                    {
                                                        foreach (var obj in strSendListData)
                                                        {
                                                            if (obj.IdBang == int.Parse(cell.IDBang) && obj.IdMat == int.Parse(cell.IDMat))
                                                            {
                                                                obj.Data += cell.IDMat + "," + cell.IDCell + ",D" + Data.ToString() + ","; ;
                                                                objExist = true;
                                                                break;
                                                            }
                                                        }
                                                        if (!objExist)
                                                        {
                                                            strSendListData.Add(new ModelDataSendTable()
                                                            {
                                                                IdBang = string.IsNullOrEmpty(cell.IDBang) ? 0 : int.Parse(cell.IDBang),
                                                                IdMat = string.IsNullOrEmpty(cell.IDMat) ? 0 : int.Parse(cell.IDMat),
                                                                Data = cell.IDBang + ",4," + cell.IDMat + "," + cell.IDCell + ",D" + Data.ToString() + ","
                                                            });
                                                        }
                                                        objExist = false;
                                                    }
                                                    else
                                                    {
                                                        //Thay doi 17/11
                                                        Send = cell.IDBang + cell.IDCell + ",4,D" + Data.ToString() + ",";
                                                        listData.Add(Send);
                                                    }
                                                }
                                                else
                                                {
                                                    if (!string.IsNullOrEmpty(cell.IDMat))
                                                    {
                                                        Send = cell.IDMat + "," + cell.IDCell + ",4,D" + Data.ToString() + ",";
                                                        listData.Add(Send);
                                                    }
                                                    else
                                                    {
                                                        Send = cell.IDCell + ",6,,";
                                                        listData.Add(Send);
                                                        Send = cell.IDCell + ",4,D" + Data.ToString() + ",";
                                                        listData.Add(Send);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                string first = Data.ToString().Substring(0, Data.ToString().Length - soled);
                                                string last = Data.ToString().Substring(Data.ToString().Length - soled, Data.ToString().Length - (Data.ToString().Length - soled));
                                                if (!string.IsNullOrEmpty(cell.IDBang))
                                                {
                                                    if (!string.IsNullOrEmpty(cell.IDMat))
                                                    {
                                                        Send = cell.IDBang + ",4," + cell.IDMat + "," + cell.IDCell + ",D" + first + ",";
                                                        listData.Add(Send);
                                                        if (position.listCell.Count > 1)
                                                        {
                                                            Send = cell.IDBang + ",4," + cell.IDMat + "," + position.listCell[1].IDCell + ",D" + last + ",";
                                                            listData.Add(Send);
                                                        }
                                                    }
                                                    else
                                                    {

                                                        Send = cell.IDBang + ",4," + cell.IDCell + ",D" + first + ",";
                                                        listData.Add(Send);
                                                        if (position.listCell.Count > 1)
                                                        {
                                                            Send = cell.IDBang + ",4," + position.listCell[1].IDCell + ",D" + last + ",";
                                                            listData.Add(Send);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (!string.IsNullOrEmpty(cell.IDMat))
                                                    {
                                                        Send = cell.IDMat + "," + cell.IDCell + ",4,D" + first + ",";
                                                        listData.Add(Send);
                                                        if (position.listCell.Count > 1)
                                                        {
                                                            Send = cell.IDMat + ",4," + position.listCell[1].IDCell + ",D" + last + ",";
                                                            listData.Add(Send);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Send = cell.IDCell + ",6,,";
                                                        listData.Add(Send);
                                                        Send = cell.IDCell + ",4,D" + first + ",";
                                                        listData.Add(Send);
                                                        if (position.listCell.Count > 1)
                                                        {
                                                            Send = position.listCell[1].IDCell + ",6,,";
                                                            listData.Add(Send);
                                                            Send = position.listCell[1].IDCell + ",4,D" + last + ",";
                                                            listData.Add(Send);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                }
                            }
                            foreach (var obj in strSendListData)
                            {
                                listData.Add(obj.Data);
                            }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        private void LoadDataBTPForKanBanStartApp(string STTChuyen_SanPham, string idChuyen)
        {
            dtLoadToTable1.Clear();
            strSendListData.Clear();
            objExist = false;
            string strSQL = "select csp.MaChuyen, c.IdDen, c.IdTyLeDoc, nx.IsEndDate, (SELECT SUM(b.BTPNgay) FROM BTP b WHERE b.STTChuyen_SanPham = '" + STTChuyen_SanPham + "'AND b.Ngay ='" + todayStr + "') AS BTPNgay, csp.SanLuongKeHoach, (SELECT SUM(BTPNgay) FROM BTP b WHERE b.STTChuyen_SanPham = '" + STTChuyen_SanPham + "' and b.IsBTP_PB_HC = 0) AS LuyKeBTP, (SELECT SUM(nx.BTPLoi) FROM NangXuat nx WHERE nx.STTCHuyen_SanPham = '" + STTChuyen_SanPham + "' and nx.IsDeleted=0) AS BTPLoi, nx.BTPTrenChuyen,  nx.DinhMucNgay, (case when(DinhMucNgay)=0 THEN 0 ELSE ROUND(((BTPTrenChuyen*100)/(DinhMucNgay)),1)end) TyLe, c.Sound, csp.LuyKeTH from NangXuat nx, Chuyen_SanPham csp, Chuyen c Where nx.STTChuyen_SanPham = '" + STTChuyen_SanPham + "' and nx.IsDeleted=0 and nx.Ngay ='" + todayStr + "' AND nx.STTChuyen_SanPham = csp.STT AND c.MaChuyen = csp.MaChuyen";
            try
            {

                if (sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }
                sqlCon.Open();
                da = new SqlDataAdapter(strSQL, sqlCon);
                da.Fill(dtLoadToTable1);
                if (dtLoadToTable1.Rows.Count > 0)
                {
                    for (int i = 0; i < dtLoadToTable1.Rows.Count; i++)
                    {
                        lstInt.Clear();
                        int MaChuyen = int.Parse(dtLoadToTable1.Rows[i]["MaChuyen"].ToString());
                        var a = MaChuyen.ToString();
                        var infChuyen = listChuyen_O.FirstOrDefault(x => x.MaChuyen == a);

                        if (infChuyen != null)
                        {
                            int tylebtp = 0;
                            try
                            {
                                tylebtp = (int)double.Parse(dtLoadToTable1.Rows[i]["TyLe"].ToString());
                                if (tylebtp < 0)
                                {
                                    tylebtp = 0;
                                }
                            }
                            catch (Exception)
                            { }
                            int BTPNgay = 0;
                            if (!string.IsNullOrEmpty(dtLoadToTable1.Rows[i]["BTPNgay"].ToString()))
                            {
                                BTPNgay = int.Parse(dtLoadToTable1.Rows[i]["BTPNgay"].ToString());
                            }
                            lstInt.Add(BTPNgay);
                            int sanLuongKeHoach = int.Parse(dtLoadToTable1.Rows[i]["SanLuongKeHoach"].ToString());
                            lstInt.Add(sanLuongKeHoach);
                            int BTPLoi = 0;
                            if (!string.IsNullOrEmpty(dtLoadToTable1.Rows[i]["BTPLoi"].ToString()))
                            {
                                BTPLoi = int.Parse(dtLoadToTable1.Rows[i]["BTPLoi"].ToString());
                            }
                            int LuyKeBTP = 0;
                            if (!string.IsNullOrEmpty(dtLoadToTable1.Rows[i]["LuyKeBTP"].ToString()))
                            {
                                LuyKeBTP = int.Parse(dtLoadToTable1.Rows[i]["LuyKeBTP"].ToString());
                            }
                            LuyKeBTP = LuyKeBTP - BTPLoi;
                            lstInt.Add(LuyKeBTP);
                            int BTPTrenChuyen = 0;
                            if (!string.IsNullOrEmpty(dtLoadToTable1.Rows[i]["BTPTrenChuyen"].ToString()))
                            {
                                BTPTrenChuyen = int.Parse(dtLoadToTable1.Rows[i]["BTPTrenChuyen"].ToString());
                            }
                            lstInt.Add(BTPTrenChuyen);
                            int luyKeTH = int.Parse(dtLoadToTable1.Rows[i]["LuyKeTH"].ToString());
                            int dinhMucNgay = Convert.ToInt32(double.Parse(dtLoadToTable1.Rows[i]["DinhMucNgay"].ToString()));
                            string filewavChuyen = dtLoadToTable1.Rows[i]["Sound"].ToString();
                            bool isEnddate = false;
                            if (!string.IsNullOrEmpty(dtLoadToTable1.Rows[i]["IsEndDate"].ToString()))
                            {
                                isEnddate = bool.Parse(dtLoadToTable1.Rows[i]["IsEndDate"].ToString());
                            }
                            ThongTinDen ttden;
                            if (isEnddate == false)
                            {
                                int IdDenParent = 0;
                                if (!string.IsNullOrEmpty(dtLoadToTable1.Rows[i]["IdDen"].ToString()))
                                {
                                    IdDenParent = int.Parse(dtLoadToTable1.Rows[i]["IdDen"].ToString());
                                }
                                int IdTyLeDoc = 0;
                                if (!string.IsNullOrEmpty(dtLoadToTable1.Rows[i]["IdTyLeDoc"].ToString()))
                                {
                                    IdTyLeDoc = int.Parse(dtLoadToTable1.Rows[i]["IdTyLeDoc"].ToString());
                                }
                                CheckPercent(IdTyLeDoc, tylebtp, filewavChuyen);
                                ttden = ColorDenForKanBan(tylebtp, IdDenParent);
                            }
                            else
                            {
                                if ((sanLuongKeHoach - luyKeTH) - BTPTrenChuyen > 0)
                                {
                                    int IdDenParent = 0;
                                    if (!string.IsNullOrEmpty(dtLoadToTable1.Rows[i]["IdDen"].ToString()))
                                    {
                                        IdDenParent = int.Parse(dtLoadToTable1.Rows[i]["IdDen"].ToString());
                                    }
                                    int IdTyLeDoc = 0;
                                    if (!string.IsNullOrEmpty(dtLoadToTable1.Rows[i]["IdTyLeDoc"].ToString()))
                                    {
                                        IdTyLeDoc = int.Parse(dtLoadToTable1.Rows[i]["IdTyLeDoc"].ToString());
                                    }
                                    CheckPercent(IdTyLeDoc, tylebtp, filewavChuyen);
                                    ttden = ColorDenForKanBan(tylebtp, IdDenParent);
                                }
                                else
                                {
                                    ttden = new ThongTinDen() { STT = 6, Color = "Xanh" };
                                }
                            }
                            var listposition = infChuyen.listPosition;
                            int tt = -1;
                            string Send = "";
                            foreach (var position in listposition)
                            {
                                if (position.ChangeBTP == 1)
                                {
                                    if (position.STTDen != 0)
                                    {
                                        if (position.STTDen == ttden.STT)
                                        {
                                            SetMauDen(position, "8888");
                                        }
                                        else
                                        {
                                            SetMauDen(position, "");
                                        }
                                    }
                                    ////
                                    else
                                    {
                                        tt++;
                                        if (tt < lstInt.Count)
                                        {
                                            int Data = lstInt[tt];
                                            if (position.listCell != null && position.listCell.Count > 0)
                                            {
                                                var cell = position.listCell[0];
                                                int soled = cell.SoLed;
                                                string max = "";
                                                for (int t = 0; t < soled; t++)
                                                {
                                                    max += "9";
                                                }
                                                if (Data < int.Parse(max))
                                                {
                                                    if (!string.IsNullOrEmpty(cell.IDBang))
                                                    {
                                                        if (!string.IsNullOrEmpty(cell.IDMat))
                                                        {
                                                            foreach (var obj in strSendListData)
                                                            {
                                                                if (obj.IdBang == int.Parse(cell.IDBang) && obj.IdMat == int.Parse(cell.IDMat))
                                                                {
                                                                    obj.Data += cell.IDMat + "," + cell.IDCell + ",D" + Data.ToString() + ","; ;
                                                                    objExist = true;
                                                                    break;
                                                                }
                                                            }
                                                            if (!objExist)
                                                                strSendListData.Add(new ModelDataSendTable()
                                                                {
                                                                    IdBang = string.IsNullOrEmpty(cell.IDBang) ? 0 : int.Parse(cell.IDBang),
                                                                    IdMat = string.IsNullOrEmpty(cell.IDMat) ? 0 : int.Parse(cell.IDMat),
                                                                    Data = cell.IDBang + ",4," + cell.IDMat + "," + cell.IDCell + ",D" + Data.ToString() + ","
                                                                });

                                                            objExist = false;

                                                        }
                                                        else
                                                        {
                                                            //Thay doi 17/11
                                                            Send = cell.IDBang + cell.IDCell + ",4,D" + Data.ToString() + ",";
                                                            listData.Add(Send);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (!string.IsNullOrEmpty(cell.IDMat))
                                                        {
                                                            Send = cell.IDMat + "," + cell.IDCell + ",4,D" + Data.ToString() + ",";
                                                            listData.Add(Send);
                                                        }
                                                        else
                                                        {
                                                            Send = cell.IDCell + ",6,,";
                                                            listData.Add(Send);
                                                            Send = cell.IDCell + ",4,D" + Data.ToString() + ",";
                                                            listData.Add(Send);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    string first = Data.ToString().Substring(0, Data.ToString().Length - soled);
                                                    string last = Data.ToString().Substring(Data.ToString().Length - soled, Data.ToString().Length - (Data.ToString().Length - soled));
                                                    if (!string.IsNullOrEmpty(cell.IDBang))
                                                    {
                                                        if (!string.IsNullOrEmpty(cell.IDMat))
                                                        {
                                                            Send = cell.IDBang + ",4," + cell.IDMat + "," + cell.IDCell + ",D" + first + ",";
                                                            listData.Add(Send);
                                                            if (position.listCell.Count > 1)
                                                            {
                                                                Send = cell.IDBang + ",4," + cell.IDMat + "," + position.listCell[1].IDCell + ",D" + last + ",";
                                                                listData.Add(Send);
                                                            }
                                                        }
                                                        else
                                                        {

                                                            Send = cell.IDBang + ",4," + cell.IDCell + ",D" + first + ",";
                                                            listData.Add(Send);
                                                            if (position.listCell.Count > 1)
                                                            {
                                                                Send = cell.IDBang + ",4," + position.listCell[1].IDCell + ",D" + last + ",";
                                                                listData.Add(Send);
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (!string.IsNullOrEmpty(cell.IDMat))
                                                        {
                                                            Send = cell.IDMat + "," + cell.IDCell + ",4,D" + first + ",";
                                                            listData.Add(Send);
                                                            if (position.listCell.Count > 1)
                                                            {
                                                                Send = cell.IDMat + ",4," + position.listCell[1].IDCell + ",D" + last + ",";
                                                                listData.Add(Send);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Send = cell.IDCell + ",6,,";
                                                            listData.Add(Send);
                                                            Send = cell.IDCell + ",4,D" + first + ",";
                                                            listData.Add(Send);
                                                            if (position.listCell.Count > 1)
                                                            {
                                                                Send = position.listCell[1].IDCell + ",6,,";
                                                                listData.Add(Send);
                                                                Send = position.listCell[1].IDCell + ",4,D" + last + ",";
                                                                listData.Add(Send);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            foreach (var obj in strSendListData)
                            {
                                listData.Add(obj.Data);
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

        private string SendHour()
        {
            try
            {
                DateTime date = DateTime.Now;
                int g = date.Hour, p = date.Minute, s = date.Second;
                string gio = "", phut = "", giay = "";
                gio = g < 10 ? "0" + g : g.ToString();
                phut = p < 10 ? "0" + p : p.ToString();
                giay = s < 10 ? "0" + s : s.ToString();
                return (gio + phut + giay);
            }
            catch (Exception ex)
            { }
            return null;
        }

        private string SendDate()
        {
            try
            {
                string ngay = "", thang = "";
                int n = DateTime.Now.Date.Day;
                int t = DateTime.Now.Date.Month;
                ngay = n < 10 ? "0" + n : n.ToString();
                thang = t < 10 ? "0" + t : t.ToString();
                string nam = DateTime.Now.Date.Year.ToString();
                return (ngay + thang + nam);
            }
            catch (Exception ex)
            { }
            return "";
        }

        DataTable dtFindID = new DataTable();

        private Chuyen_SanPham_Short Findid(string IdChuyen)
        {
            Chuyen_SanPham_Short c_sp_s = new Chuyen_SanPham_Short();
            dtFindID.Clear();
            string strSQL = "SELECT TOP 1 csp.STT, c.TenChuyen, csp.NangXuatSanXuat FROM Chuyen_SanPham csp, Chuyen c, SanPham sp WHERE csp.MaChuyen = '" + IdChuyen + "' and csp.MaChuyen = c.MaChuyen and csp.IsFinish = 0 and csp.IsDelete = 0 and sp.IsDelete=0 and csp.MaSanPham = sp.MaSanPham and sp.IsDelete=0   Order By csp.STTThucHien ASC";
            try
            {
                if (sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }
                sqlCon.Open();
                da = new SqlDataAdapter(strSQL, sqlCon);
                da.Fill(dtFindID);
                if (dtFindID.Rows.Count > 0)
                {
                    c_sp_s.STTChuyen_SanPham = dtFindID.Rows[0]["STT"].ToString();
                    c_sp_s.TenChuyen = dtFindID.Rows[0]["TenChuyen"].ToString();
                    c_sp_s.NangSuatSanXuat = double.Parse(dtFindID.Rows[0]["NangXuatSanXuat"].ToString());
                }
                return c_sp_s;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return c_sp_s;
        }

        DataTable dtColor = new DataTable();

        private ThongTinDen ColorDen(double TyLeThucHien, int IdDen)
        {
            dtColor.Clear();
            string strSQL = "SELECT MaMauDen, Color FROM Den  Where ValueFrom <= " + TyLeThucHien + " AND " + TyLeThucHien + " < ValueTo AND IdCatalogTable = '" + idTable + "' and STTParent=" + IdDen;
            ThongTinDen ttden = new ThongTinDen();
            try
            {

                if (sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }
                sqlCon.Open();
                da = new SqlDataAdapter(strSQL, sqlCon);
                da.Fill(dtColor);
                if (dtColor.Rows.Count > 0)
                {
                    ttden.STT = int.Parse(dtColor.Rows[0][0].ToString());
                    ttden.Color = dtColor.Rows[0][1].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ttden;
        }

        private ThongTinDen ColorDenForKanBan(double TyLeHienThiDen, int IdDenParent)
        {
            ThongTinDen ttden = new ThongTinDen() { STT = 0, Color = "" };
            try
            {
                dtColor.Clear();
                string strSQL = "";
                if (idTable == "1")
                {
                    strSQL = "SELECT MaMauDen, Color FROM Den  Where ValueFrom <= " + TyLeHienThiDen + " AND " + TyLeHienThiDen + " < ValueTo AND STTParent='" + IdDenParent + "' AND IdCatalogTable=1";
                }
                else if (idTable == "2")
                {
                    strSQL = "SELECT MaMauDen, Color FROM Den  Where ValueFrom <= " + TyLeHienThiDen + " AND " + TyLeHienThiDen + " < ValueTo AND STTParent='" + IdDenParent + "' AND IdCatalogTable=2";
                }
                if (sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }
                sqlCon.Open();
                da = new SqlDataAdapter(strSQL, sqlCon);
                da.Fill(dtColor);
                if (dtColor.Rows.Count > 0)
                {
                    ttden.STT = int.Parse(dtColor.Rows[0][0].ToString());
                    ttden.Color = dtColor.Rows[0][1].ToString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ttden;
        }

        DataTable dtLoadDataAll = new DataTable();
        List<string> listDataAll = new List<string>();
        InformationChuyen ChuyenXX = new InformationChuyen();

        List<string> ListIdChuyenFinish = new List<string>();


        List<string> ListSTTChuyen_SPIsChangeBTP = new List<string>();
        DataTable dtSTTChuyen_SPIsChangeBTP = new DataTable();
        private void CheckChuyen_SPIsChangeBTP()
        {
            try
            {
                ListSTTChuyen_SPIsChangeBTP.Clear();
                dtSTTChuyen_SPIsChangeBTP.Clear();
                string strSQL = "select nx.STTCHuyen_SanPham from NangXuat nx, Chuyen_SanPham c_sp, Chuyen c WHERE nx.IsChangeBTP = 1 and nx.IsDeleted=0 and nx.Ngay ='" + todayStr + "' and nx.STTCHuyen_SanPham = c_sp.STT and c_sp.MaChuyen = c.MaChuyen and c.MaChuyen in (" + AccountSuccess.strListChuyenId + ")";
                if (sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }
                sqlCon.Open();
                da = new SqlDataAdapter(strSQL, sqlCon);
                da.Fill(dtSTTChuyen_SPIsChangeBTP);
                if (dtSTTChuyen_SPIsChangeBTP != null && dtSTTChuyen_SPIsChangeBTP.Rows.Count > 0)
                {
                    foreach (DataRow row in dtSTTChuyen_SPIsChangeBTP.Rows)
                    {
                        ListSTTChuyen_SPIsChangeBTP.Add(row["STTCHuyen_SanPham"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void EndCheckChuyen_SPIsChangeBTP()
        {
            try
            {
                string strSQL = "update NangXuat set IsChangeBTP =0 where IsChangeBTP =1 and IsDeleted=0 And STTCHuyen_SanPham in (select STT from Chuyen_SanPham WHERE Chuyen_SanPham.MaChuyen IN (select MaChuyen from Chuyen where  MaChuyen in (" + AccountSuccess.strListChuyenId + ")))";
                if (sqlCon.State == ConnectionState.Open)
                    sqlCon.Close();
                sqlCon.Open();
                sqlCom = new SqlCommand(strSQL, sqlCon);
                int sodong = sqlCom.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        private void EndIsBTP()
        {
            try
            {
                string strSQL = "update NangXuat set IsBTP =0 where IsBTP =1 and IsDeleted=0";
                if (sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }
                sqlCon.Open();
                sqlCom = new SqlCommand(strSQL, sqlCon);
                int sodong = sqlCom.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        string TTTruyenSP = "";
        bool showmessage = false;

        ThoiGianTinhNhipDoTTDAO thoigiantinhndttDAO = new ThoiGianTinhNhipDoTTDAO();
        private void CreateSendString(int MaChuyen)
        {
            try
            {
                strSendListData.Clear();
                objExist = false;
                ChuyenSanPhamModel c_sp_short = BLLAssignmentForLine.Instance.FindByLineId(MaChuyen);
                if (c_sp_short.STT != null)
                {
                    TTTruyenSP = c_sp_short.LineName + "  --->  ";
                    double NangSuatSanXuat = Math.Round((c_sp_short.ProductionTime * 100) / c_sp_short.HieuSuatNgay);
                    var assignModel = BLLAssignmentForLine.Instance.GetAssignmentModel_BangDienTu(c_sp_short.STT, MaChuyen, todayStr, (hienThiDenTheoTPThoatChuyen == 0 ? false : true), TypeOfShowProductToLCD);
                    if (assignModel != null)
                    {
                        ChuyenXX = listChuyen_O.FirstOrDefault(x => x.MaChuyen == MaChuyen.ToString());
                        if (ChuyenXX != null)
                        {
                            var TotalSecond = (int)TimeIsWorkNS(MaChuyen, NSType).TotalSeconds;
                            lbTTTruyenSP.Caption = TTTruyenSP;
                            TTTruyenSP += assignModel.CommoName.ToString();

                            //Xử lý báo đèn
                            double tyLeHienDen = assignModel.lightPercent;
                            var a = listChuyen.FirstOrDefault(x => x.MaChuyen == MaChuyen);
                            int STTDen = ColorDen(tyLeHienDen, a.IdDenNangSuat ?? 0).STT;
                            //// 
                            #region xử lý gui du lieu
                            InformationCell cell;
                            int soled = 0, tt = 0, Data = 0;
                            string max = "", Send = "", DataStr = "";
                            foreach (var position in ChuyenXX.listPosition)
                            {
                                tt++; DataStr = ""; Data = 0;
                                #region Bind Data
                                switch (tt)
                                {
                                    case 1: DataStr = SendHour(); break;
                                    case 2: DataStr = SendDate(); break;
                                    case 3: if (position.STTDen == STTDen) SetMauDen(position, "8888"); else SetMauDen(position, ""); break;
                                    case 4: if (position.STTDen == STTDen) SetMauDen(position, "8888"); else SetMauDen(position, ""); break;
                                    case 5: if (position.STTDen == STTDen) SetMauDen(position, "8888"); else SetMauDen(position, ""); break;
                                    case 6: DataStr = assignModel.CommoName; break;
                                    case 7: DataStr = assignModel.ProductionPlans.ToString(); break;
                                    case 8: Data = (int)assignModel.ProductionNorms; break;
                                    case 9: Data = (int)Math.Round(assignModel.KCSPercent, 0); break;
                                    case 10: Data = assignModel.KCSInDay; break;
                                    case 11: Data = assignModel.LK_KCS; break;
                                    case 12: Data = assignModel.Labours; break;
                                    case 13: Data = assignModel.BTPInDay; break;
                                    case 14: Data = assignModel.LK_BTP; break;
                                    case 15: Data = assignModel.BTPInLine; break;
                                    case 16: Data = (int)Math.Round(assignModel.ProductionPace, 0); break;
                                    case 17: Data = (int)Math.Round(assignModel.CurrentPace, 0); break;
                                    case 18: Data = (int)Math.Round(assignModel.BTPPerLabour, 0); break;
                                    case 19: Data = assignModel.TCInDay; break;
                                    case 20: Data = assignModel.ErrorInDay; break;
                                    case 21: Data = (int)Math.Round(assignModel.Current_TC_Pace, 0); break;
                                    case 22: Data = (int)Math.Round(assignModel.SalesDate, 0); break;
                                    case 23: Data = (int)Math.Round(assignModel.Lean, 0); break;
                                    case 24: Data = assignModel.KCSInHours; break;
                                    case 25: Data = assignModel.TCInHours; break;
                                }
                                #endregion
                                if (position.listCell.Count > 0)
                                {
                                    if (position.IsInt == 1)
                                    {
                                        #region dữ liệu kiểu số
                                        cell = position.listCell[0];
                                        soled = cell.SoLed;
                                        max = "";
                                        for (int t = 0; t < soled; t++)
                                            max += "9";

                                        if (Data < int.Parse(max))
                                        {
                                            #region IDBang
                                            if (cell.IDBang != "")
                                            {
                                                #region IDMat
                                                if (cell.IDMat != "")
                                                {
                                                    foreach (var obj in strSendListData)
                                                    {
                                                        if (obj.IdBang == int.Parse(cell.IDBang) && obj.IdMat == int.Parse(cell.IDMat))
                                                        {
                                                            obj.Data += cell.IDCell + ",D" + Data.ToString() + ",";
                                                            if (tt == 11 || tt == 18)
                                                                obj.Data += "|" + obj.IdBang + ",4,";
                                                            objExist = true;
                                                            break;
                                                        }
                                                    }
                                                    if (!objExist)
                                                        strSendListData.Add(new ModelDataSendTable()
                                                        {
                                                            IdBang = string.IsNullOrEmpty(cell.IDBang) ? 0 : int.Parse(cell.IDBang),
                                                            IdMat = string.IsNullOrEmpty(cell.IDMat) ? 0 : int.Parse(cell.IDMat),
                                                            Data = cell.IDBang + ",4," + cell.IDCell + ",D" + Data.ToString() + ","
                                                        });
                                                    objExist = false;
                                                }
                                                else
                                                {
                                                    Send = cell.IDBang + "," + cell.IDCell + ",6,,";
                                                    listData.Add(Send);
                                                    // Thay Doi 17/11
                                                    Send = cell.IDBang + "," + cell.IDCell + ",4,D" + Data.ToString() + ",";
                                                    listData.Add(Send);
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                if (cell.IDMat != "")
                                                {
                                                    Send = cell.IDMat + "," + cell.IDCell + ",4,D" + Data.ToString() + ",";
                                                    listData.Add(Send);
                                                }
                                                else
                                                {
                                                    Send = cell.IDCell + ",6,,";
                                                    listData.Add(Send);
                                                    Send = cell.IDCell + ",4,D" + Data.ToString() + ",";
                                                    listData.Add(Send);
                                                }
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region
                                            string first = Data.ToString().Substring(0, Data.ToString().Length - soled);
                                            string last = Data.ToString().Substring(Data.ToString().Length - soled, Data.ToString().Length - (Data.ToString().Length - soled));
                                            if (cell.IDBang != "")
                                            {
                                                if (!string.IsNullOrEmpty(cell.IDMat) && cell.IDMat != "")
                                                {
                                                    Send = cell.IDBang + ",4," + cell.IDMat + "," + cell.IDCell + ",D" + first + ",";
                                                    listData.Add(Send);
                                                    //Send = cell.IDBang + ",4," + cell.IDMat + "," + position.listCell[1].IDCell + ",D" + last + ",";
                                                    Send = cell.IDBang + ",4," + cell.IDMat + "," + position.listCell[0].IDCell + ",D" + last + ",";
                                                    listData.Add(Send);
                                                }
                                                else
                                                {
                                                    Send = cell.IDBang + ",4," + cell.IDCell + ",D" + first + ",";
                                                    listData.Add(Send);
                                                    //  Send = cell.IDBang + ",4," + position.listCell[1].IDCell + ",D" + last + ",";
                                                    Send = cell.IDBang + ",4," + position.listCell[0].IDCell + ",D" + last + ",";
                                                    listData.Add(Send);
                                                }
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(cell.IDMat))
                                                {
                                                    Send = cell.IDMat + "," + cell.IDCell + ",4,D" + first + ",";
                                                    listData.Add(Send);
                                                    //Send = cell.IDMat + ",4," + position.listCell[1].IDCell + ",D" + last + ",";
                                                    Send = cell.IDMat + ",4," + position.listCell[0].IDCell + ",D" + last + ",";
                                                    listData.Add(Send);
                                                }
                                                else
                                                {
                                                    Send = cell.IDCell + ",6,,";
                                                    listData.Add(Send);
                                                    Send = cell.IDCell + ",4,D" + first + ",";
                                                    listData.Add(Send);

                                                    Send = position.listCell[1].IDCell + ",6,,";
                                                    listData.Add(Send);
                                                    Send = position.listCell[1].IDCell + ",4,D" + last + ",";
                                                    listData.Add(Send);
                                                }
                                            }
                                            #endregion
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region Set mau led
                                        if (position.STTDen == 0)
                                        {
                                            cell = position.listCell[0];
                                            if (cell.IDBang != "")
                                            {
                                                if (cell.IDMat != "")
                                                {
                                                    Send = cell.IDBang + ",4," + cell.IDCell + ",D" + DataStr + ",";
                                                    listData.Add(Send);
                                                }
                                                else
                                                {
                                                    Send = cell.IDBang + "," + cell.IDCell + ",4,D" + DataStr + ",";
                                                    listData.Add(Send);
                                                }
                                            }
                                            else
                                            {
                                                if (cell.IDMat != "")
                                                {
                                                    Send = cell.IDMat + "," + cell.IDCell + ",4,D" + DataStr + ",";
                                                    listData.Add(Send);
                                                }
                                                else
                                                {
                                                    Send = cell.IDCell + ",6,,";
                                                    listData.Add(Send);
                                                    Send = cell.IDCell + ",4,D" + DataStr + ",";
                                                    listData.Add(Send);
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                }

                            }
                            #endregion
                            foreach (var obj in strSendListData)
                            {
                                var str = obj.Data.Split('|').ToList();
                                barEditItem2.EditValue = obj.Data;
                                foreach (var item in str)
                                {
                                    listData.Add(item);
                                }
                            }

                            // DateTime daynow = DateTime.Now.Date; 
                            TimeSpan timeStartTinhTT = BLLTimeToCalculateND.LayTimeBatDau(DateTime.Now, MaChuyen);// thoigiantinhndttDAO.LayTimeBatDau(DateTime.Now, MaChuyen);
                            if (timeStartTinhTT == TimeSpan.Parse("00:00:00"))
                            {
                                TimeSpan timeStart = BLLShift.FindTimeStart(MaChuyen); //shiftDAO.FindTimeStart(MaChuyen);
                                var obj = new PMS.Data.ThoiGianTinhNhipDoTT();
                                obj.Ngay = todayStr;
                                obj.MaChuyen = MaChuyen;
                                obj.ThoiGianBatDau = timeStart;
                                // thoigiantinhndttDAO.ThemOBJ(obj);
                                BLLTimeToCalculateND.InsertOrUpdate(obj);
                            }
                            IsSend = true;
                        }
                        else
                        {
                            //   showmessage = true;
                            //  ms.Information = c_sp_short.LineName;
                            //  ms.ShowDialog();
                        }
                    }
                }
                else
                {
                    //   showmessage = true;
                    //   ms.Information = c_sp_short.TenChuyen;
                    //   ms.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadDataAllFromDB()
        {
            try
            {
                if (strMaChuyenTatca != null)
                    CreateSendString(strMaChuyenTatca);
                else
                    MessageBox.Show("Vui lòng chọn Chuyền trong mục Nhập Thông Tin Ngày, trước khi gửi dữ liệu xuống bảng", "Lỗi thao tác", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            catch (Exception ex)
            {
                // throw ex;
                MessageBox.Show("Vui lòng chọn Chuyền trong mục Nhập Thông Tin Ngày, trước khi gửi dữ liệu xuống bảng", "Lỗi thao tác", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private DataTable TTViTriCuaChuyen(string MaChuyen, string IdTable)
        {

            DataTable dtSoViTri = new DataTable();
            string strSQL = "select ViTri, ChangeTP, ChangeBTP, IsInt, STTDen, IdCatalogTable from TTHienThi Where MaChuyen ='" + MaChuyen + "' and IdCatalogTable ='" + IdTable + "' order by ViTri ASC";
            try
            {
                if (sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }
                sqlCon.Open();
                da = new SqlDataAdapter(strSQL, sqlCon);
                da.Fill(dtSoViTri);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtSoViTri;
        }

        private DataTable TimThongTinChiTietCuaViTri(string MaChuyen, string ViTri, int IdCatalogTable)
        {
            DataTable dtTTCT = new DataTable();
            string strSQL = "SELECT ctht.STT, ctht.SoLed, ctht.IDMain, ctht.IDMat, ctht.IDCell FROM ChiTietHienThi ctht WHERE ctht.MaHienThi = (SELECT tt.MaHienThi FROM TTHienThi tt WHERE tt.MaChuyen = '" + MaChuyen + "' AND tt.ViTri ='" + ViTri + "' AND IdCatalogTable =" + IdCatalogTable + ")";
            try
            {
                if (sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }
                sqlCon.Open();
                da = new SqlDataAdapter(strSQL, sqlCon);
                da.Fill(dtTTCT);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtTTCT;
        }

        private List<InformationChuyen> TimTTHienThi(string idTable)
        {
            try
            {
                var listC = new List<InformationChuyen>();
                var listChuyen = chuyenDAO.GetListChuyenInfByListId(AccountSuccess.strListChuyenId);
                DataTable dtTTViTri = new DataTable();
                if (listChuyen != null && listChuyen.Count > 0)
                {
                    for (int i = 0; i < listChuyen.Count; i++)
                    {
                        string MaChuyen = listChuyen[i].MaChuyen;
                        dtTTViTri.Clear();
                        dtTTViTri = TTViTriCuaChuyen(MaChuyen, idTable);
                        List<InformationPosition> listposition = new List<InformationPosition>();
                        if (dtTTViTri.Rows.Count > 0)
                        {
                            DataTable dtTTCTViTri = new DataTable();
                            for (int j = 0; j < dtTTViTri.Rows.Count; j++)
                            {
                                string vitri = dtTTViTri.Rows[j][0].ToString();
                                int idcatalogtable = int.Parse(dtTTViTri.Rows[j][5].ToString());
                                int ChangeTP = 0;
                                try
                                {
                                    ChangeTP = int.Parse(dtTTViTri.Rows[j][1].ToString());
                                }
                                catch (Exception) { }

                                int ChangeBTP = 0;
                                try
                                {
                                    ChangeBTP = int.Parse(dtTTViTri.Rows[j][2].ToString());
                                }
                                catch (Exception) { }

                                int IsInt = 0;
                                try
                                {
                                    IsInt = int.Parse(dtTTViTri.Rows[j][3].ToString());
                                }
                                catch (Exception) { }

                                int STTDen = 0;
                                try
                                {
                                    STTDen = int.Parse(dtTTViTri.Rows[j][4].ToString());
                                }
                                catch (Exception) { }

                                dtTTCTViTri.Clear();
                                dtTTCTViTri = TimThongTinChiTietCuaViTri(MaChuyen, vitri, idcatalogtable);
                                var listcell = new List<InformationCell>();
                                if (dtTTCTViTri.Rows.Count > 0)
                                {
                                    for (int k = 0; k < dtTTCTViTri.Rows.Count; k++)
                                    {
                                        var cell = new InformationCell()
                                        {
                                            STT = int.Parse(dtTTCTViTri.Rows[k][0].ToString()),
                                            SoLed = int.Parse(dtTTCTViTri.Rows[k][1].ToString()),
                                            IDBang = dtTTCTViTri.Rows[k][2].ToString(),
                                            IDMat = dtTTCTViTri.Rows[k][3].ToString(),
                                            IDCell = dtTTCTViTri.Rows[k][4].ToString()
                                        };
                                        listcell.Add(cell);
                                    }
                                }
                                InformationPosition position = new InformationPosition()
                                {
                                    ViTri = vitri,
                                    listCell = listcell,
                                    ChangeTP = ChangeTP,
                                    ChangeBTP = ChangeBTP,
                                    IsInt = IsInt,
                                    STTDen = STTDen
                                };
                                listposition.Add(position);
                            }
                        }
                        var chuyen = new InformationChuyen()
                        {
                            MaChuyen = MaChuyen,
                            TenChuyen = listChuyen[i].TenChuyen,
                            listPosition = listposition,
                            // listShift = TTCaCuaChuyen(MaChuyen)
                        };
                        listC.Add(chuyen);
                    }
                }
                return listC;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void tmLoadData_Tick(object sender, EventArgs e)
        {
            try
            {
                #region Lam
                if (AccountSuccess.isWriteLog)
                    AccountSuccess.strError += "Vào timer load data gửi xuống bảng \n";
                if (IsSend == false)
                {
                    if (AccountSuccess.isWriteLog)
                        AccountSuccess.strError += "IsSend=false nên tiếp tục ktra và load data gửi xuống bảng \n";
                    indexsend = -1;
                    if (readIndexMaChuyenIsFinish == false)
                    {
                        if (AccountSuccess.isWriteLog)
                            AccountSuccess.strError += "readIndexMaChuyenIsFinish=false nên tiếp tục ktra và load data gửi xuống bảng \n";
                        readIndexMaChuyenIsFinish = true;
                        if (idTable == "1")
                        {
                            if (AccountSuccess.isWriteLog)
                                AccountSuccess.strError += "table=1 nên tiếp tục ktra và load data gửi xuống bảng \n";
                            if (AccountSuccess.isWriteLog)
                                AccountSuccess.strError += "Ktra và get data gui xuong bang \n";
                            listData.Clear();

                            // ktra nx co thay doi hay doi hay ko tinh lai thong tin va gui suong bang dien tu
                            CheckProductionChangeAndResendDataToBangDienTu();

                            ListIdChuyenFinish.Clear();
                            ListIdChuyenFinish.AddRange(BLLAssignmentForLine.Instance.GetLineIsFinishProduction(AccountSuccess.strListChuyenId.Split(',').Select(x => Convert.ToInt32(x)).ToArray()));

                            if (ListIdChuyenFinish.Count > 0)
                            {
                                //  EndFindFinish();
                                BLLAssignmentForLine.Instance.ChangeStatusIsFinishNowFromTrueToFalse(AccountSuccess.strListChuyenId.Split(',').Select(x => Convert.ToInt32(x)).ToArray());
                                for (int j = 0; j < ListIdChuyenFinish.Count; j++)
                                {
                                    int machuyen = int.Parse(ListIdChuyenFinish[j]);
                                    //   CreateSendString(machuyen);
                                    var obj = new PMS.Data.ThoiGianTinhNhipDoTT();
                                    obj.Ngay = todayStr;
                                    obj.MaChuyen = machuyen;
                                    obj.ThoiGianBatDau = DateTime.Now.TimeOfDay;
                                    BLLTimeToCalculateND.InsertOrUpdate(obj);
                                }
                            }
                        }
                        else
                        {
                            if (isHienThiRaManHinhLCD == 1)
                            {
                                // CheckIsBTP();
                                var assigIds = BLLAssignmentForLine.Instance.GetAssignmentIds(todayStr, AccountSuccess.IsAll == 0 ? 0 : int.Parse(AccountSuccess.IdFloor.Trim()));
                                if (assigIds.Length > 0)
                                {
                                    EndIsBTP();
                                    for (int j = 0; j < assigIds.Length; j++)
                                        LoadDataBTPForKanBan(assigIds[j]);
                                }
                            }
                        }
                        readIndexMaChuyenIsFinish = false;
                        if (listData.Count > 0)
                        {
                            IsSend = true;
                            if (AccountSuccess.isWriteLog)
                                AccountSuccess.strError += "Có data gửi xuống bảng, bật cờ IsSend=true \n";
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                AccountSuccess.strError += "Vào timer load data gửi xuống bảng +" + ex.Message + "\n";
            }
        }

        private void BackupDatabase()
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = @"D:\";
                saveFileDialog1.Title = "Save backup file";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string sqlBackup = "BACKUP DATABASE [" + dbclass.data + "] TO DISK='" + @saveFileDialog1.FileName + ".bak'";
                    int result = dbclass.TruyVan_XuLy(sqlBackup);
                    MessageBox.Show("Sao lưu dữ liệu thành công.");
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi BackupDatabase: " + ex.Message);
            }
        }

        DataTable dtGetIdNotFinish = new DataTable();

        private string Getidnotfinishfirst(string IdChuyen)
        {
            dtGetIdNotFinish.Clear();
            string strSQL = "SELECT TOP 1 csp.STT FROM Chuyen_SanPham csp WHERE csp.MaChuyen = '" + IdChuyen + "' and csp.IsFinish = 0 and csp.IsDelete = 0 and csp.Thang=" + DateTime.Now.Month + " and csp.Nam=" + DateTime.Now.Year + " Order By csp.STTThucHien ASC";
            try
            {
                if (sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }
                sqlCon.Open();
                da = new SqlDataAdapter(strSQL, sqlCon);
                da.Fill(dtGetIdNotFinish);
                if (dtGetIdNotFinish.Rows.Count > 0)
                {
                    return dtGetIdNotFinish.Rows[0][0].ToString();
                }
                else
                    return "";

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadTTBangKanBan()
        {
            var listChuyen = chuyenDAO.GetListChuyenInfByListId(AccountSuccess.strListChuyenId);
            if (listChuyen != null && listChuyen.Count > 0)
            {
                string MaChuyen = "";
                string STTChuyen_SanPham = "";
                string Ngay = SendDate();
                string Gio = SendHour();
                listData.Add("3,6,,");
                string sendngay = "3,4,D" + Ngay + ",";
                listData.Add(sendngay);
                listData.Add("2,6,,");
                string sendgio = "2,4,D" + Gio + ",";
                listData.Add(sendgio);

                for (int i = 0; i < listChuyen.Count; i++)
                {
                    MaChuyen = listChuyen[i].MaChuyen;
                    STTChuyen_SanPham = Getidnotfinishfirst(MaChuyen);
                    LoadDataBTPForKanBanStartApp(STTChuyen_SanPham, MaChuyen);
                }
                if (listData.Count > 0)
                {
                    IsSend = true;
                }
            }
        }

        private void SetIsChangeForNangXuat(string STT, int NhipDoThucTe)
        {
            try
            {
                string strSQL = "update NangXuat set NhipDoThucTe =" + NhipDoThucTe + ",IsChange =1, TimeLastChange ='" + DateTime.Now.TimeOfDay.ToString() + "' where STTCHuyen_SanPham ='" + STT + "' and Ngay ='" + todayStr + "' and IsDeleted=0";
                if (sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }
                sqlCon.Open();
                sqlCom = new SqlCommand(strSQL, sqlCon);
                sqlCom.ExecuteNonQuery();
            }
            catch (Exception)
            { }
        }

        //Review
        DataTable dtTimeoutChange = new DataTable();
        private void timerTimeoutNDTT_Tick(object sender, EventArgs e)
        {
            TimeSpan timecheck = TimeSpan.Parse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.TIMECHECK)).Value.Trim());
            dtTimeoutChange.Clear();
            string strSQL = "select nx.STTCHuyen_SanPham, nx.TimeLastChange, nx.ThucHienNgay, c_sp.MaChuyen from NangXuat nx, Chuyen_SanPham c_sp Where c_sp.STT = nx.STTCHuyen_SanPham and nx.Ngay ='" + todayStr + "' and nx.IsDeleted=0";
            try
            {
                if (sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }
                sqlCon.Open();
                da = new SqlDataAdapter(strSQL, sqlCon);
                da.Fill(dtTimeoutChange);
                if (dtTimeoutChange.Rows.Count > 0)
                {
                    for (int i = 0; i < dtTimeoutChange.Rows.Count; i++)
                    {
                        TimeSpan timewait = DateTime.Now.TimeOfDay - TimeSpan.Parse(dtTimeoutChange.Rows[i]["TimeLastChange"].ToString());
                        if (timewait > timecheck)
                        {
                            TimeSpan hieutime = new TimeSpan();
                            hieutime = TimeIsWork(int.Parse(dtTimeoutChange.Rows[i][3].ToString()));
                            int second = (int)hieutime.TotalSeconds;
                            int thuchienngay = int.Parse(dtTimeoutChange.Rows[i][2].ToString());
                            int nhipDoThucTe = 0;
                            if (thuchienngay > 0)
                            {
                                nhipDoThucTe = second / thuchienngay;
                            }
                            SetIsChangeForNangXuat(dtTimeoutChange.Rows[i][0].ToString(), nhipDoThucTe);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                timerTimeoutNDTT.Enabled = false;
            }
        }

        DataTable dtReadPercent = new DataTable();
        private void CheckPercent(int IdTyLeDoc, int phantram, string filewavChuyen)
        {
            try
            {
                string filewavSlient = Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals("SLIENT")).Value.Trim();
                dtReadPercent.Clear();
                string strSQL = "select CountRepeat, Sound from ReadPercent where PercentFrom <= " + phantram + " and " + phantram + "<= PercentTo and IdParent = '" + IdTyLeDoc + "'";
                if (sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }
                sqlCon.Open();
                da = new SqlDataAdapter(strSQL, sqlCon);
                da.Fill(dtReadPercent);

                if (dtReadPercent.Rows.Count > 0)
                {
                    string Sound = dtReadPercent.Rows[0][1].ToString();
                    readpercent_short(filewavChuyen, Sound, filewavSlient, int.Parse(dtReadPercent.Rows[0][0].ToString()));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        DataTable dtfile = new DataTable();

        List<string> listfilewav = new List<string>();
        private void readpercent_short(string filewavChuyen, string filewavNoiDung, string filewavslient, int repeatTimes)
        {
            try
            {
                listfilewav.Clear();
                listfilewav.Add(filewavChuyen);
                listfilewav.Add(filewavNoiDung);
                listfilewav.Add(filewavslient);
                if (repeatTimes > 1)
                {
                    int countlist = listfilewav.Count();
                    for (int j = 0; j < repeatTimes - 1; j++)
                    {
                        for (int k = 0; k < countlist; k++)
                        {
                            listfilewav.Add(listfilewav[k]);
                        }
                    }
                }

                if (listfilewav.Count > 0)
                {
                    queuePlayFileWavKanBan.Enqueue(listfilewav);
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        private TimeSpan TimeIsWorkNS(int MaChuyen, int nsType)
        {
            TimeSpan timeWork = new TimeSpan(0, 0, 0);
            try
            {
                // List<DuAn03_HaiDang.POJO.Shift> listShift = TTCaCuaChuyen(MaChuyen);
                var listShift = listChuyen.FirstOrDefault(x => x.MaChuyen == MaChuyen).Shifts;// BLLShift.GetShiftsOfLine(MaChuyen);
                if (listShift != null && listShift.Count > 0)
                {
                    if (nsType == 0)
                    {
                        foreach (var item in listShift.OrderBy(x => x.ShiftOrder))
                        {
                            timeWork += item.End - item.Start;
                        }
                    }
                    else
                    {
                        var timeNow = DateTime.Now.TimeOfDay;
                        foreach (var item in listShift)
                        {
                            if (timeNow > item.Start)
                            {
                                if (timeNow < item.End)
                                {
                                    timeWork += timeNow - item.Start;
                                    break;
                                }
                                else
                                {
                                    timeWork += item.End - item.Start;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            { }
            return timeWork;
        }

        private void FrmMainNew_Load(object sender, EventArgs e)
        {
            try
            {
                //get configs
                GetApplicationConfig();

                // Ket noi CSDL
                ConnectDatabase();

                // ket noi Comport
                ComPortConnection();

                // Update lai BTP tren chuyen chuyen theo kieu tinh cua config hien tai 
                BLLProductivity.ResetNormsDayAndBTPInLine(getBTPInLineByType, calculateNormsdayType, TypeOfCaculateDayNorms, 0, true, todayStr);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi frmainload: " + ex.Message);
            }
        }

        private void ComPortConnection()
        {
            //Ket noi COM port
            GetComPortInformation();
            // loadTTCOMPort();            


            if (idTable == "1")
                ListKeyPab.AddRange(BLLKeyPad.DSKeyPad(int.Parse(AccountSuccess.IdFloor)));
        }


        private void GetApplicationConfig()
        {
            int.TryParse(ConfigurationManager.AppSettings["AppId"].ToString(), out appId);
            Configs = BLLConfig.Instance.GetAll(appId);
            lblUsername.Caption = "Xin chào: " + AccountSuccess.TenChuTK;
            idTable = Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.TABLE)).Value.Trim();
            int.TryParse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.MANHINHLCD)).Value.Trim(), out isHienThiRaManHinhLCD);
            int.TryParse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.HIENTHIDENNS)).Value.Trim(), out hienThiDenTheoTPThoatChuyen);
            int.TryParse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.TINHBTPTHOATCHUYEN)).Value.Trim(), out tinhBTPThoatChuyen);
            int.TryParse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.SENDMAIL)).Value.Trim(), out isSendMail);
            int.TryParse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.READSOUND)).Value.Trim(), out isReadSound);
            int.TryParse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.TIMEOUTACK)).Value.Trim(), out timeoutcheckACK);
            int.TryParse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.NSTYPE)).Value.Trim(), out NSType);
            int.TryParse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.SETTOTALBYMINORMAX)).Value.Trim(), out setTotalByMinOrMax_default);
            int.TryParse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.TIMESENDREQUESTANDDATA)).Value.Trim(), out timeSendRequestAndData);
            int.TryParse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.GETBTPINLINEBYTYPE)).Value.Trim(), out getBTPInLineByType);
            int.TryParse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.CalculateNormsdayType)).Value.Trim(), out calculateNormsdayType);
            int.TryParse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.AUTOSETDAYINFO)).Value.Trim(), out autoSetDayInfo);
            int.TryParse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.TIMEREADSOUND)).Value.Trim(), out intTimeReadSound);
            int.TryParse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.ISAUTOTURNONOFFCOM)).Value.Trim(), out isAutoTurnOnOffCOM);
            int.TryParse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.ISAUTOMOVEQUANTITYMORTH)).Value.Trim(), out isAutoMoveQuantityMorth);
            int.TryParse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.AUTOMOVEQUANTITYMORTHTYPE)).Value.Trim(), out autoMoveQuantityMorthType);
            int.TryParse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.THOIGIANLATCACLCD)).Value.Trim(), out thoiGianLatCacLCD);
            int.TryParse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.SoundSilent)).Value.Trim(), out SoundSilent);

            int.TryParse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.IsUseTableComport)).Value.Trim(), out IsUseTableComport);
            TypeOfCheckFinishProduction = Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.TypeOfCheckFinishProduction.Trim().ToUpper())).Value.ToUpper().Trim().Split(',').ToList();

            int.TryParse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.IsUseReadNotifyForKanban.Trim().ToUpper())).Value.Trim(), out IsUseReadNotifyForKanban);
            int.TryParse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.IsUseReadNotifyForInventoryInKCS.Trim().ToUpper())).Value.Trim(), out IsUseReadNotifyForInventoryInKCS);
            int.TryParse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.TimerReadNotifyForKanban.Trim().ToUpper())).Value.Trim(), out TimerReadNotifyForKanban);
            int.TryParse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.TimerReadNotifyForInventoryInKCS.Trim().ToUpper())).Value.Trim(), out TimerReadNotifyForInventoryInKCS);

            int.TryParse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.timeSendRequestKCSButHandleError.ToUpper())).Value.Trim(), out timeSendRequestKCSButHandleError);
            int.TryParse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.timeSendRequestTCButHandleError.ToUpper())).Value.Trim(), out timeSendRequestTCButHandleError);
            int.TryParse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.timeSendRequestErrorButHandleError.ToUpper())).Value.Trim(), out timeSendRequestErrorButHandleError);
            int.TryParse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.TimeRefreshFromDayInfoView.ToUpper())).Value.Trim(), out TimeRefreshFromDayInfoView);
            int.TryParse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.TimeCloseFromDayInfoViewIfNotUse.ToUpper())).Value.Trim(), out TimeCloseFromDayInfoViewIfNotUse);
            int.TryParse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.TypeOfCaculateDayNorms.ToUpper())).Value.Trim(), out TypeOfCaculateDayNorms);
            int.TryParse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.TypeOfShowProductToLCD.ToUpper())).Value.Trim(), out TypeOfShowProductToLCD);
            int.TryParse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.KeypadQuantityProcessingType.ToUpper())).Value.Trim(), out KeypadQuantityProcessingType);

            filewavSlient = Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.Slient)).Value.Trim();
            SaveMediaFileAddress = Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.SaveMediaFileAddress.ToUpper())).Value.Trim();

            var lcdCF = BLLConfig.Instance.GetShowLCDConfigByName(eShowLCDConfigName.TimesGetNSInDay);
            int.TryParse(lcdCF != null ? lcdCF.Value : "1", out TimesGetNSInDay);
            lcdCF = BLLConfig.Instance.GetShowLCDConfigByName(eShowLCDConfigName.KhoangCachLayNangSuat);
            int.TryParse(lcdCF != null ? lcdCF.Value : "1", out KhoangCachGetNSInDay);
            var tachNhanDuLieuConfig = Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.TACHNHANDULIEU)).Value.Trim();
            if (tachNhanDuLieuConfig == "1")
                TachNhanDuLieu = true;
        }

        private static void ConnectDatabase()
        {
            string strConnectionString = dbclass.GetConnectionString();
            try
            {
                sqlCon = new SqlConnection(strConnectionString);
                sqlCon.Open();
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi ConnectDatabase: Không thể kết nối với CSDL, Vui lòng thử cấu hình lại kết nối", "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FrmConnectDatabase form = new FrmConnectDatabase();
                form.Show();
            }
        }

        private void GetComPortInformation()
        {
            int parity = 0, stopbit = 0;
            try
            {
                #region COM TABLE
                P.PortName = Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.COM)).Value.Trim();
                P.BaudRate = Convert.ToInt32(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.BAUDRATE)).Value.Trim());
                P.DataBits = Convert.ToInt32(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.DATABITS)).Value.Trim());
                parity = int.Parse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.PARITY)).Value.Trim());
                switch (parity)
                {
                    case 0: FrmMainNew.P.Parity = Parity.None; break;
                    case 1: FrmMainNew.P.Parity = Parity.Odd; break;
                    case 2: FrmMainNew.P.Parity = Parity.Even; break;
                    case 3: FrmMainNew.P.Parity = Parity.Mark; break;
                    case 4: FrmMainNew.P.Parity = Parity.Space; break;
                }
                stopbit = int.Parse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.STOPBITS)).Value.Trim());
                switch (stopbit)
                {
                    case 0: FrmMainNew.P.StopBits = StopBits.None; break;
                    case 1: FrmMainNew.P.StopBits = StopBits.One; break;
                    case 2: FrmMainNew.P.StopBits = StopBits.Two; break;
                    case 3: FrmMainNew.P.StopBits = StopBits.OnePointFive; break;
                }
                #endregion

                try
                {
                    FrmMainNew.P.Open();
                    FrmMainNew.P.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived_ACK);
                }
                catch (Exception)
                {
                    if (IsUseTableComport == 1)
                        MessageBox.Show("Lỗi GetComPortInformation: Không thể kết nối với cổng COM Bảng, Vui lòng thử cấu hình lại kết nối", "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                if (IsUseTableComport == 1)
                    MessageBox.Show("Lấy thông tin Com Bảng bị lỗi.\n" + ex.Message, "Lỗi Com Bảng");
            }


            #region COM KEYPAD
            InitComPort_P2();
            if (idTable == "1")
                ListKeyPab.AddRange(BLLKeyPad.DSKeyPad(int.Parse(AccountSuccess.IdFloor)));

            #endregion
        }

        private void InitComPort_P2()
        {
            try
            {
                P2.PortName = Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.COM2)).Value.Trim();
                P2.BaudRate = Convert.ToInt32(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.BAUDRATE2)).Value.Trim());
                P2.DataBits = Convert.ToInt32(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.DATABITS2)).Value.Trim());
                int parity = int.Parse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.PARITY2)).Value.Trim());
                switch (parity)
                {
                    case 0: FrmMainNew.P2.Parity = Parity.None; break;
                    case 1: FrmMainNew.P2.Parity = Parity.Odd; break;
                    case 2: FrmMainNew.P2.Parity = Parity.Even; break;
                    case 3: FrmMainNew.P2.Parity = Parity.Mark; break;
                    case 4: FrmMainNew.P2.Parity = Parity.Space; break;
                }

                int stopbit = int.Parse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.STOPBITS2)).Value.Trim());
                switch (stopbit)
                {
                    case 0: FrmMainNew.P2.StopBits = StopBits.None; break;
                    case 1: FrmMainNew.P2.StopBits = StopBits.One; break;
                    case 2: FrmMainNew.P2.StopBits = StopBits.Two; break;
                    case 3: FrmMainNew.P2.StopBits = StopBits.OnePointFive; break;
                }

                if (idTable == "1")
                {
                    try
                    {
                        FrmMainNew.P2.Open();
                        //Chuyen den ham port_DataReceived khi com port nhan du lieu
                        FrmMainNew.P2.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Lỗi InitComPort_P2: không thể kết nối với cổng COM Keypad, Vui lòng thử cấu hình lại kết nối", "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lấy thông tin Com Keypad bị lỗi.\n" + ex.Message, "Lỗi Com Keypad");
            }
        }

        private bool ActiveForm(Type type)
        {
            bool result = false;
            foreach (Form fm in MdiChildren)
            {
                if (fm.GetType() == type)
                {
                    fm.Activate();
                    result = true;
                }
            }
            return result;
        }

        #region event
        private void butResetKeyPad_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                FrmMainNew.P.Close();
                FrmMainNew.P2.Close();
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi frmMain_FormClosed: " + ex.Message);
            }

        }

        private void butChangePassWord_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmChangePassWord form = new frmChangePassWord();
            form.ShowDialog();
        }
        private void butLogOut_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                Application.Exit();
                //AccountSuccess.TenTK = "(none)";
                //AccountSuccess.BTP = 0;
                //AccountSuccess.TenChuTK = null;
                //AccountSuccess.ThanhPham = 0;
                //AccountSuccess.IdFloor = string.Empty;
                //AccountSuccess.IsAll = 0;

                // this.Close();                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi butLogOut_ItemClick: " + ex.Message);
            }
        }

        private void butClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Application.Exit();
        }

        private void butQuanLyMailNhanVaGui_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var result = ActiveForm(typeof(FrmMailSend));
            if (!result)
            {
                FrmMailSend f = new FrmMailSend();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void butCauHinhMailGui_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var result = ActiveForm(typeof(FrmMailTemplate));
            if (!result)
            {
                FrmMailTemplate f = new FrmMailTemplate();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void butQLThoiGianGui_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var result = ActiveForm(typeof(FrmMailSchedule));
            if (!result)
            {
                FrmMailSchedule f = new FrmMailSchedule(this);
                f.MdiParent = this;
                f.Show();
            }
        }

        private void barButPhanHangChoChuyen_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var result = ActiveForm(typeof(frmPhaHangChoChuyen));
            if (!result)
            {
                frmPhaHangChoChuyen f = new frmPhaHangChoChuyen(this);
                f.MdiParent = this;
                f.Show();
            }
        }

        private void barButNhapThongTinNgay_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var result = ActiveForm(typeof(FrmSetDayInformation));
            if (!result)
            {
                var f = new FrmSetDayInformation(this);
                f.MdiParent = this;
                f.Show();
            }
        }

        private void barButXemThongTinNS_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (idTable == "1")  // xuong may 
            {
                var result = ActiveForm(typeof(FrmDayInfo_View));
                if (!result)
                {
                    var f = new FrmDayInfo_View(this);
                    f.MdiParent = this;
                    f.Show();
                }
            }
            else // xuong cắt
            {
                var result = ActiveForm(typeof(FrmTheoDoiBang));
                if (!result)
                {
                    var f = new FrmTheoDoiBang();
                    f.MdiParent = this;
                    f.Show();
                }
            }
        }

        private void barButNhapSanLuongToCat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var result = ActiveForm(typeof(FrmSanLuongCat));
            if (!result)
            {
                FrmSanLuongCat f = new FrmSanLuongCat();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void barButManHinhHienThi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (idTable == "1")
            {
                Form form = new FrmHienThiLCD();
                //Form form = new FrmLCD_NS();
                form.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                form.Show();
                AccountSuccess.ListFormLCD.Add(form);
            }
            else
            {
                FrmHienThiLCDKANBAN frm = new FrmHienThiLCDKANBAN();
                frm.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                frm.Show();
                AccountSuccess.ListFormLCD.Add(frm);
            }
            if (MessageBox.Show("Bạn có muốn ẩn màn mình chính của chương trình?", "Ẩn màn hình chính", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                this.Hide();
            else
                this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
        }

        private void barButXemNangSuatTram_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var result = ActiveForm(typeof(FrmNangSuatCum));
            if (!result)
            {
                FrmNangSuatCum f = new FrmNangSuatCum();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void butQuanLyToCat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var result = ActiveForm(typeof(FrmToCat));
            if (!result)
            {
                FrmToCat f = new FrmToCat();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void butQuanLyAmThanhBangKANBAN_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //var result = ActiveForm(typeof(FrmQuanlybaotyleKanBan));
            //if (!result)
            //{
            //    FrmQuanlybaotyleKanBan f = new FrmQuanlybaotyleKanBan();
            //    f.MdiParent = this;
            //    f.Show();
            //}
        }

        private void butBTPLoiTrongNgay_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var result = ActiveForm(typeof(frmBTPLoi));
            if (!result)
            {
                frmBTPLoi f = new frmBTPLoi();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void butCaiDatThoiGian_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var result = ActiveForm(typeof(FrmCaiDatTimecs));
            if (!result)
            {
                FrmCaiDatTimecs f = new FrmCaiDatTimecs();
                f.MdiParent = this;
                f.Show();
            }
        }

        //private void butThayDoiLuyKeTH_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    var result = ActiveForm(typeof(FrmUpdateProductQuantityOnDay));
        //    if (!result)
        //    {
        //        FrmUpdateProductQuantityOnDay f = new FrmUpdateProductQuantityOnDay();
        //        f.MdiParent = this;
        //        f.Show();
        //    }

        //}

        //private void butXemLichSuNhanDuLieu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    var result = ActiveForm(typeof(FrmInputDayInformation));
        //    if (!result)
        //    {
        //        FrmInputDayInformation f = new FrmInputDayInformation();
        //        f.MdiParent = this;
        //        f.Show();
        //    }

        //}

        private void butBaoHetDonHang_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void butQuanLyTiLeHienThiDen_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void butQuanLyCaLamViec_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //var result = ActiveForm(typeof(frmSetupTimeShiftWork));
            //if (!result)
            //{
            //    frmSetupTimeShiftWork f = new frmSetupTimeShiftWork();
            //    f.MdiParent = this;
            //    f.Show();
            //}

        }

        private void butQuanLySanPhamVaTK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void butBackup_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                BackupDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi butBackup_ItemClick: " + ex.Message);
            }
        }

        private void butQuanLyFileAmThanh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var result = ActiveForm(typeof(FrmSoundFile));
            if (!result)
            {
                var f = new FrmSoundFile(appId, SaveMediaFileAddress);
                f.MdiParent = this;
                f.Show();
            }

        }

        private void butCauHinhDocSo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var result = ActiveForm(typeof(FrmCauHinhDocSo));
            if (!result)
            {
                FrmCauHinhDocSo f = new FrmCauHinhDocSo();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void butCauHinhDocAmThanhChoChuyen_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var result = ActiveForm(typeof(FrmCauHinhDocNS));
            if (!result)
            {
                FrmCauHinhDocNS f = new FrmCauHinhDocNS();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void butKetNoiCSDL_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmConnectDatabase f = new FrmConnectDatabase();
            f.Show();
        }

        private void butGuiDuLieuXuongBang_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                LoadDataAllFromDB();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi butGuiDuLieuXuongBang_ItemClick: " + ex.Message);
            }
        }

        private void butRestart_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                Application.Restart();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi butRestart_ItemClick: " + ex.Message);
            }
        }

        string Speaker = "";
        private void butSpeaker_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (Speaker == "")
                {
                    Speaker = "mute";
                    butSpeaker.Glyph = global::QuanLyNangSuat.Properties.Resources.speaker_mute;
                }
                else
                {
                    Speaker = "";
                    butSpeaker.Glyph = global::QuanLyNangSuat.Properties.Resources.speaker;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi butSpeaker_ItemClick: " + ex.Message);
            }
        }

        private void butSendDataTest_Click(object sender, EventArgs e)
        {
            //InformationPlay inf = new InformationPlay { SoundChuyen = "6.wav", Repeat = 1 };
            //queuePlayFile.Enqueue(inf);
            UpdateTableLoiSanXuat("2,4,1,01,2,00,3,00,4,00,5,00,6,00,7,00,8,00,9,20,10,00,11,00,12,00");
            //UpdateTableLoiSanXuat("300,4,1,01,2,00,3,01,4,00,5,01,6,00,7,00,8,01,9,00,10,00,11,00,12,00");
            //UpdateTableLoiSanXuat("10,4,1,02,1,133,00,4,00,5,00,6,00,7,00,8,00,9,00,10,01,11,00,12,00");
            //UpdateTableLoiSanXuat("10,5,1,03,1,133,00,4,00,5,00,6,00,7,00,8,00,9,00,10,01,11,00,12,00");
            //UpdateTableLoiSanXuat("10,4,2,03,1,133,00,4,00,5,00,6,00,7,00,8,00,9,00,10,01,11,00,12,00");
            //UpdateTableLoiSanXuat("10,5,2,03,1,133,00,4,00,5,00,6,00,7,00,8,00,9,00,10,01,11,00,12,00");
            //UpdateTableLoiSanXuat("10,6,1,03,1,133,00,4,00,5,00,6,00,7,00,8,00,9,00,10,01,11,00,12,00");
            //UpdateTableLoiSanXuat("10,7,1,01,1,133,00,4,00,5,00,6,00,7,00,8,00,9,00,10,01,11,00,12,00");
            //UpdateTableLoiSanXuat("10,8,1,06,1,80,00,4,00,5,00,6,00,7,00,8,00,9,00,10,01,11,00,12,00");
            //UpdateTableLoiSanXuat("10,13,1,03,1,80,00,4,00,5,00,6,00,7,00,8,00,9,00,10,01,11,00,12,00");    
        }
        #endregion

        public void RunAllProcess(bool IsResetComPort)
        {
            try
            {
                if (!IsResetComPort)
                {
                    // Lay thong tin hien thi
                    listChuyen_O = TimTTHienThi(idTable);

                    listChuyen = BLLLine.GetLinesForMainForm(AccountSuccess.strListChuyenId.Split(',').Select(x => Convert.ToInt32(x)).ToList(), int.Parse(idTable));

                    //set thong tin phan cong cho chuyen
                    BLLMonthlyProductionPlans.InsertForNewMonth();

                    //Tu dong set thong tin ngay
                    if (autoSetDayInfo == 1)
                        SetDayInfoForLine_NN(); // SetDayInfoForLine(); 

                    //Cai dat san pham xuong thiet bi
                    SetupProductOnDay_N();

                    //Get List Turn On/off COM config
                    var listTurnCOMMng = turnCOMMngDAO.GetListTurnCOMConfig();
                    if (listTurnCOMMng != null && listTurnCOMMng.Count > 0)
                        listTurnOnOffCOM = listTurnCOMMng.Where(c => c.IsActive).ToList();
                    int floorId = 0;
                    int.TryParse(AccountSuccess.IdFloor, out floorId);
                    listKeyPadConfig = BLLKeyPad.GetListKeyPadLineConfig(floorId); // keypadDAO.GetListKeyPadLineConfig(floorId);

                    BLLCommodity.ChangeAssignmentStatusforAllDelatedCommodities();
                    BLLProductivity.ResetNormsDayAndBTPInLine(getBTPInLineByType, calculateNormsdayType, TypeOfCaculateDayNorms, 0, true, todayStr);


                    if (IsUseReadNotifyForKanban == 1)
                    {
                        Timer_ReadNotifyForKanban.Interval = TimerReadNotifyForKanban;
                        Timer_ReadNotifyForKanban.Enabled = true;
                    }

                    if (IsUseReadNotifyForInventoryInKCS == 1)
                    {
                        Timer_ReadNotifyForInventoryInKCS.Interval = TimerReadNotifyForInventoryInKCS;
                        Timer_ReadNotifyForInventoryInKCS.Enabled = true;
                    }
                }

                tmSenData.Interval = timeSendRequestAndData;
                tmSenData.Enabled = true;
                if (FrmMainNew.P.IsOpen)
                    butGuiDuLieuXuongBang.Enabled = true;

                tmLoadData.Enabled = true;


                CheckForIllegalCrossThreadCalls = false;
                threadplay = new Thread(PlayinQueueKanBan);
                threadplay.Start();

                if (idTable == "1")
                {
                    if (!IsResetComPort)
                    {
                        LayTTBaoHetHang();
                        CheckForIllegalCrossThreadCalls = false;
                        threadplay = new Thread(PlayinQueue);
                        threadplay.Start();
                    }
                    try
                    {
                        if (FrmMainNew.P2.IsOpen)
                        {
                            isSendRequest = true;
                            IsQuet = true;
                            butPlay.Glyph = global::QuanLyNangSuat.Properties.Resources.Play;
                        }
                        else
                            P2.Open();
                    }
                    catch (Exception ex)
                    {
                        //   ResetComPort(); 
                    }
                }
                else
                {
                    if (!IsResetComPort)
                        LoadTTBangKanBan();
                }

                if (!IsResetComPort)
                {
                    if (isReadSound == 1)
                    {
                        soundIntConfigDAO = new SoundIntConfigDAO();
                        //Lay danh sach cau hinh doc so
                        GetListReadIntConfig();
                        //lay danh sach file am thanh
                        LoadListSound();
                    }
                    if (isSendMail == 1)
                    {
                        frmSendMailAndReadSound = new FrmSendMailAndReadSound(TimesGetNSInDay, getBTPInLineByType);
                        frmSendMailAndReadSound.Show();
                        frmSendMailAndReadSound.Hide();
                    }
                    butRun.Enabled = false;
                    butRun.Caption = "Các tiến trình đang chạy";
                    butTatMoTienTrinhTuDong.Caption = "Tắt các tiến trình tự động";
                }
                IsStopProcess = false;
                readIndexMaChuyenIsFinish = false;
                IsSend = false;

                foreach (var item in listKeyPadObjectInfo)
                {
                    listDataSendKeyPad.Add(item.EquipmentId + "," + (int)eCommandSend.HandlingSuccess + ",,");
                }

            }
            catch (Exception ex)
            {
                //  GhiFileLog(DateTime.Now + "loi RunAllProcess ex :" + ex.Message + " \n");
            }
        }


        #region --------------------                ReadSound               ---------------------------------
        private DataTable dtFormula = new DataTable();
        private DataTable dtReadSound = new DataTable();
        private DataTable dtReadConfigDetail = new DataTable();
        private List<DuAn03_HaiDang.Model.ModelSoundItem> listItem;
        private DuAn03_HaiDang.Model.ModelSoundItem item;
        private DataTable dtListSound = null;
        private List<SOUND> listSounds = null;
        private List<SOUND> sounds = null;
        private SoundIntConfigDAO soundIntConfigDAO;
        private List<SoundIntConfig> listSoundInt;

        public SoundIntConfig GetObjectById(int id)
        {
            SoundIntConfig soundIntConfig = null;
            dtFormula.Clear();
            try
            {
                string sql = "Select Id, Code, Name, Description, Formula, IsProductivity from SOUND_IntConfig where Id=" + id + " and IsActive=1 and IsDeleted=0";
                da = new SqlDataAdapter(sql, sqlCon);
                da.Fill(dtFormula);
                if (dtFormula != null && dtFormula.Rows.Count > 0)
                {
                    soundIntConfig = new SoundIntConfig();
                    soundIntConfig.Formula = dtFormula.Rows[0]["Formula"].ToString();
                    bool isProductivity = false;
                    bool.TryParse(dtFormula.Rows[0]["IsProductivity"].ToString(), out isProductivity);
                    soundIntConfig.IsProductivity = isProductivity;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return soundIntConfig;
        }

        //Ham nay se goi luc an nut chay tiem trinh tu dong
        private void LoadListSound()
        {
            try
            {
                string sql = "select Id, Code, Name, Description, Path from SOUND where IsActive=1 and IsDeleted =0 order by Id desc";
                dtListSound = dbclass.TruyVan_TraVe_DataTable(sql);

                listSounds = BLLSound.Gets();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Lay danh sach cau hinh doc so        
        private void GetListReadIntConfig()
        {
            try
            {
                listSoundInt = soundIntConfigDAO.GetListReadIntConfig();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<string> LoadListSoundFileByString(string chuoi)
        {
            List<string> listPath = null;
            try
            {
                if (!string.IsNullOrEmpty(chuoi))
                {
                    //if (dtListSound != null && dtListSound.Rows.Count > 0)
                    //{
                    //    listPath = new List<string>();
                    //    var tuArr = chuoi.Split(new char[] { ' ' });
                    //    if (tuArr != null && tuArr.Length > 0)
                    //    {
                    //        foreach (string tu in tuArr)
                    //        {
                    //            string tuStandard = tu.Trim().ToUpper();
                    //            foreach (DataRow row in dtListSound.Rows)
                    //            {
                    //                string code = row["Code"].ToString();
                    //                if (!string.IsNullOrEmpty(code))
                    //                {
                    //                    string codeStandard = code.Trim().ToUpper();
                    //                    if (tuStandard.Equals(codeStandard))
                    //                    {
                    //                        listPath.Add(row["Path"].ToString().Trim());
                    //                        break;
                    //                    }
                    //                }
                    //            }
                    //        }

                    //    }
                    //}

                    if (listSounds != null && listSounds.Count > 0)
                    {
                        listPath = new List<string>();
                        var tuArr = chuoi.Split(new char[] { ' ' });
                        if (tuArr != null && tuArr.Length > 0)
                        {
                            foreach (string tu in tuArr)
                            {
                                string tuStandard = tu.Trim().ToUpper();
                                var found = listSounds.FirstOrDefault(x => x.Code.Trim().ToUpper().Equals(tuStandard));
                                if (found != null)
                                    listPath.Add(found.Path.Trim());
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listPath;
        }

        public List<DuAn03_HaiDang.Model.ModelSoundItem> GetListSoundItemByChuyen(int idChuyen, int configType)
        {
            listItem = null;
            try
            {
                dtReadSound.Clear();
                string sqlReadConfig = "Select Id from SOUND_ReadConfig where IdChuyen=" + idChuyen + " and IsActive=1 and IsDeleted=0 and ConfigType=" + configType;
                da = new SqlDataAdapter(sqlReadConfig, sqlCon);
                da.Fill(dtReadSound);
                if (dtReadSound != null && dtReadSound.Rows.Count > 0)
                {
                    listItem = new List<DuAn03_HaiDang.Model.ModelSoundItem>();
                    foreach (DataRow row in dtReadSound.Rows)
                    {
                        dtReadConfigDetail.Clear();
                        int idConfig = int.Parse(row["Id"].ToString());
                        string sqlReadConfigDetail = "Select IntType, IdSound, IdIntConfig from SOUND_ReadConfigDetail where IdReadConfig=" + idConfig + " and IsActive=1 and IsDeleted=0 order by OrderIndex ASC";
                        da = new SqlDataAdapter(sqlReadConfigDetail, sqlCon);
                        da.Fill(dtReadConfigDetail);
                        if (dtReadConfigDetail != null && dtReadConfigDetail.Rows.Count > 0)
                        {
                            foreach (DataRow rowDetail in dtReadConfigDetail.Rows)
                            {
                                item = new DuAn03_HaiDang.Model.ModelSoundItem();
                                int intType = 0;
                                int.TryParse(rowDetail["IntType"].ToString(), out intType);
                                item.FileType = intType;
                                if (intType == 0)
                                {
                                    int idIntConfig = 0;
                                    int.TryParse(rowDetail["IdIntConfig"].ToString(), out idIntConfig);
                                    SoundIntConfig soundIntConfig = GetObjectById(idIntConfig);
                                    if (soundIntConfig != null)
                                    {
                                        item.Formula = soundIntConfig.Formula;
                                        item.IsProductivity = soundIntConfig.IsProductivity;
                                    }
                                }
                                else
                                {
                                    int idSound = 0;
                                    int.TryParse(rowDetail["IdSound"].ToString(), out idSound);
                                    string path = soundDAO.GetPathById(idSound);
                                    if (!string.IsNullOrEmpty(path))
                                        item.SoundPath = path;
                                }
                                listItem.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listItem;
        }

        private int soLanDoc = 1;
        private bool isRead = false;
        Expression expression = new Expression();
        TimeSpan timeReadSound = new TimeSpan(0, 0, 0);
        DataTable dtNSHours = new DataTable();
        private void CheckTimeReadSound()
        {
            try
            {
                var listSoundTimeConfig = BLLSound.GetListTimeByConfigType(1);
                if (listSoundTimeConfig != null && listSoundTimeConfig.Count > 0)
                {
                    var dateTimeNow = DateTime.Now;
                    TimeSpan timeNow = TimeSpan.Parse(dateTimeNow.TimeOfDay.Hours.ToString() + ":" + dateTimeNow.TimeOfDay.Minutes.ToString() + ":00");

                    var listSoundFile = new List<string>();
                    foreach (var item in listSoundTimeConfig)
                    {
                        if (item.Time == timeNow && item.Time != timeReadSound)
                        {
                            timeReadSound = timeNow;
                            soLanDoc = item.SoLanDoc;
                            isRead = true;
                            break;
                        }
                    }
                    if (isRead)
                    {
                        if (listChuyen != null && listChuyen.Count > 0)
                        {
                            var listSoundPath = new List<string>();
                            foreach (var chuyen in listChuyen)
                            {
                                //int idChuyen = 0;
                                //int.TryParse(chuyen.MaChuyen, out idChuyen);
                                if (chuyen.MaChuyen > 0)
                                {

                                    var listItem = BLLSound.GetListSoundItemByChuyen(chuyen.MaChuyen, 1); //  GetListSoundItemByChuyen(chuyen.MaChuyen, 1);
                                    if (listItem != null && listItem.Count > 0)
                                    {
                                        Chuyen_SanPham_Short c_sp_short = Findid(chuyen.MaChuyen.ToString());
                                        //  var c_sp_short = BLLAssignmentForLine.GetAssignmentForLine(chuyen.MaChuyen).FirstOrDefault();
                                        dtLoadDataAll.Clear();
                                        if (c_sp_short != null)
                                        {
                                            string strSQL = "select csp.SanLuongKeHoach, ROUND(nx.DinhMucNgay,0) DinhMucNgay, nx.ThucHienNgay, csp. LuyKeTH, tp.LaoDongChuyen, (SELECT SUM(BTPNgay) FROM BTP b WHERE b.STTChuyen_SanPham = '" + c_sp_short.STTChuyen_SanPham + "'AND b.Ngay ='" + todayStr + "' and b.IsBTP_PB_HC = 0) BTPNgay, ((SELECT SUM(BTPNgay) FROM BTP b WHERE b.STTChuyen_SanPham = '" + c_sp_short.STTChuyen_SanPham + "' and b.IsBTP_PB_HC = 0)-(SELECT SUM(nx.BTPLoi) FROM NangXuat nx WHERE nx.STTCHuyen_SanPham = '" + c_sp_short.STTChuyen_SanPham + "' and nx.IsDeleted=0)) LuyKeBTP, nx.BTPTrenChuyen, nx.NhipDoSanXuat, nx.NhipDoThucTe, (nx.BTPTrenChuyen/tp.LaoDongChuyen) BTPTrenLD, nx.BTPThoatChuyenNgay, csp.LuyKeBTPThoatChuyen, (nx.SanLuongLoi-nx.SanLuongLoiGiam) as SanLuongLoi, nx.NhipDoThucTeBTPThoatChuyen FROM SanPham sp, ThanhPham tp, Chuyen_SanPham csp, NangXuat nx WHERE csp.STT ='" + c_sp_short.STTChuyen_SanPham + "' AND nx.STTCHuyen_SanPham = csp.STT  and nx.Ngay ='" + todayStr + "' AND csp.MaSanPham = sp.MaSanPham AND tp.STTChuyen_SanPham = csp.STT AND tp.Ngay = nx.Ngay and nx.IsDeleted=0";
                                            if (sqlCon.State == ConnectionState.Open)
                                            {
                                                sqlCon.Close();
                                            }
                                            sqlCon.Open();
                                            da = new SqlDataAdapter(strSQL, sqlCon);
                                            da.Fill(dtLoadDataAll);
                                            if (dtLoadDataAll != null && dtLoadDataAll.Rows.Count > 0)
                                            {
                                                foreach (var item in listItem)
                                                {
                                                    if (item.FileType != 0)
                                                        listSoundPath.Add(item.SoundPath);
                                                    else
                                                    {
                                                        DataRow row = dtLoadDataAll.Rows[0];
                                                        if (!string.IsNullOrEmpty(item.Formula))
                                                        {
                                                            var arrFormulaHasNSHours = item.Formula.Split(new char[] { '|' });
                                                            if (arrFormulaHasNSHours != null && arrFormulaHasNSHours.Length > 1)
                                                            {
                                                                for (int i = 1; i < arrFormulaHasNSHours.Length; i += 2)
                                                                {
                                                                    int sanLuongGio = 0;
                                                                    int sanLuongGioTang = 0;
                                                                    int sanLuongGioGiam = 0;
                                                                    int intMinuter = 0;
                                                                    int.TryParse(arrFormulaHasNSHours[1], out intMinuter);
                                                                    if (intMinuter > 0)
                                                                    {
                                                                        var timeStart = dateTimeNow.AddMinutes(-intMinuter).TimeOfDay;
                                                                        string strSQLNSHours = "";
                                                                        switch (arrFormulaHasNSHours[0].Substring(1))
                                                                        {
                                                                            case "NangSuatCachGioHienTai":
                                                                                strSQLNSHours = "select (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + chuyen.MaChuyen + " and Time >= '" + timeStart + "' and Time <='" + timeNow + "' and Date='" + todayStr + "' and CommandTypeId=" + (int)eCommandRecive.ProductIncrease + " and ProductOutputTypeId=" + (int)eProductOutputType.KCS + " and IsEndOfLine=1)  AS SanLuongTang, (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + chuyen.MaChuyen + " and Time >= '" + timeStart + "' and Time <='" + timeNow + "' and Date='" + todayStr + "' and CommandTypeId=" + (int)eCommandRecive.ProductReduce + " and ProductOutputTypeId=" + (int)eProductOutputType.KCS + " and IsEndOfLine=1)  AS SanLuongGiam";
                                                                                break;
                                                                            case "ThoatChuyenCachGioHienTai":
                                                                                strSQLNSHours = "select (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + chuyen.MaChuyen + " and Time >= '" + timeStart + "' and Time <='" + timeNow + "' and Date='" + todayStr + "' and CommandTypeId=" + (int)eCommandRecive.ProductIncrease + " and ProductOutputTypeId=" + (int)eProductOutputType.TC + " and IsEndOfLine=1)  AS SanLuongTang, (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + chuyen.MaChuyen + " and Time >= '" + timeStart + "' and Time <='" + timeNow + "' and Date='" + todayStr + "' and CommandTypeId=" + (int)eCommandRecive.ProductReduce + " and ProductOutputTypeId=" + (int)eProductOutputType.TC + " and IsEndOfLine=1)  AS SanLuongGiam";
                                                                                break;
                                                                        }

                                                                        if (sqlCon.State == ConnectionState.Open)
                                                                            sqlCon.Close();

                                                                        sqlCon.Open();
                                                                        da = new SqlDataAdapter(strSQLNSHours, sqlCon);
                                                                        dtNSHours.Clear();
                                                                        da.Fill(dtNSHours);
                                                                        if (dtNSHours != null && dtNSHours.Rows.Count > 0)
                                                                        {
                                                                            DataRow rowSanLuongGio = dtNSHours.Rows[0];
                                                                            if (rowSanLuongGio["SanLuongTang"] != null)
                                                                                int.TryParse(rowSanLuongGio["SanLuongTang"].ToString(), out sanLuongGioTang);
                                                                            if (rowSanLuongGio["SanLuongGiam"] != null)
                                                                                int.TryParse(rowSanLuongGio["SanLuongGiam"].ToString(), out sanLuongGioGiam);
                                                                            sanLuongGio = sanLuongGioTang - sanLuongGioGiam;
                                                                        }
                                                                        if (sanLuongGio < 0)
                                                                            sanLuongGio = 0;

                                                                        switch (arrFormulaHasNSHours[0].Substring(1))
                                                                        {
                                                                            case "NangSuatCachGioHienTai":
                                                                                item.Formula = item.Formula.Replace("[NangSuatCachGioHienTai|" + intMinuter + "|Phut]", sanLuongGio.ToString());
                                                                                break;
                                                                            case "ThoatChuyenCachGioHienTai":
                                                                                item.Formula = item.Formula.Replace("[ThoatChuyenCachGioHienTai|" + intMinuter + "|Phut]", sanLuongGio.ToString());
                                                                                break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            item.Formula = item.Formula.Replace("[SanLuongKeHoach]", row["SanLuongKeHoach"].ToString());
                                                            item.Formula = item.Formula.Replace("[DinhMucNgay]", row["DinhMucNgay"].ToString());
                                                            item.Formula = item.Formula.Replace("[ThucHienNgay]", row["ThucHienNgay"].ToString());
                                                            item.Formula = item.Formula.Replace("[LuyKeThucHien]", row["LuyKeTH"].ToString());
                                                            item.Formula = item.Formula.Replace("[LaoDongChuyen]", row["LaoDongChuyen"].ToString());
                                                            item.Formula = item.Formula.Replace("[BTPNgay]", row["BTPNgay"].ToString());
                                                            item.Formula = item.Formula.Replace("[LuyKeBTP]", row["LuyKeBTP"].ToString());
                                                            item.Formula = item.Formula.Replace("[BTPTrenChuyen]", row["BTPTrenChuyen"].ToString());
                                                            item.Formula = item.Formula.Replace("[NhipDoSanXuat]", row["NhipDoSanXuat"].ToString());
                                                            item.Formula = item.Formula.Replace("[NhipDoThucTe]", row["NhipDoThucTe"].ToString());
                                                            item.Formula = item.Formula.Replace("[BTPTrenLD]", row["BTPTrenLD"].ToString());
                                                            item.Formula = item.Formula.Replace("[BTPThoatChuyenNgay]", row["BTPThoatChuyenNgay"].ToString());
                                                            item.Formula = item.Formula.Replace("[LuyKeBTPThoatChuyen]", row["LuyKeBTPThoatChuyen"].ToString());
                                                            item.Formula = item.Formula.Replace("[SanLuongLoi]", row["SanLuongLoi"].ToString());
                                                            item.Formula = item.Formula.Replace("[NhipDoThucTeBTPThoatChuyen]", row["NhipDoThucTeBTPThoatChuyen"].ToString());

                                                            double doubleSoGioLamViecTrongNgay = TimeIsWorkNS(chuyen.MaChuyen, 0).TotalHours;
                                                            int intSoGioLamViecTrongNgay = (int)doubleSoGioLamViecTrongNgay;
                                                            if (intSoGioLamViecTrongNgay < doubleSoGioLamViecTrongNgay)
                                                                intSoGioLamViecTrongNgay++;
                                                            double doubleSoGioLamViecHienTai = TimeIsWorkNS(chuyen.MaChuyen, 1).TotalHours;
                                                            int intSoGioLamViecHienTai = (int)doubleSoGioLamViecHienTai;
                                                            if (intSoGioLamViecHienTai < doubleSoGioLamViecHienTai)
                                                                intSoGioLamViecHienTai++;

                                                            item.Formula = item.Formula.Replace("[SoGioLamViecTrongNgay]", intSoGioLamViecTrongNgay.ToString());
                                                            item.Formula = item.Formula.Replace("[SoGioLamViecHienTai]", intSoGioLamViecHienTai.ToString());
                                                            item.Formula = ExprHelper.FormatExpression(item.Formula).Replace(" ", "");
                                                            var result = (decimal)expression.EvaluatePrefix(expression.Infix2Prefix(item.Formula));
                                                            //  MessageBox.Show(result.ToString());
                                                            string chu = string.Empty;
                                                            if (item.IsProductivity)
                                                            {
                                                                if (result > 0)
                                                                    chu += "Vượt ";
                                                            }
                                                            if (result < 0)
                                                            {
                                                                chu += "Âm ";
                                                                result = -result;
                                                            }
                                                            chu += ConvertSoToChu.replace_special_word(ConvertSoToChu.join_unit(result.ToString()));
                                                            // MessageBox.Show(chu);
                                                            var listPath = LoadListSoundFileByString(chu);
                                                            if (listPath != null && listPath.Count > 0)
                                                                listSoundPath.AddRange(listPath);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (listSoundPath.Count > 0)
                            {
                                for (int i = 0; i < soLanDoc; i++)
                                {
                                    foreach (var path in listSoundPath)
                                    {
                                        InformationPlay inf = new InformationPlay { SoundChuyen = path, Repeat = 1 };
                                        queuePlayFile.Enqueue(inf);
                                    }
                                }
                            }
                        }
                        isRead = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int soLanDocError = 1;
        private bool isReadError = false;
        Expression expressionError = new Expression();
        TimeSpan timeReadSoundError = new TimeSpan(0, 0, 0);
        DataTable dtNSHoursError = new DataTable();
        private void CheckTimeReadSoundError()
        {
            try
            {
                var listSoundTimeConfigError = BLLSound.GetListTimeByConfigType(2);
                if (listSoundTimeConfigError != null && listSoundTimeConfigError.Count > 0)
                {
                    var dateTimeNow = DateTime.Now;
                    TimeSpan timeNow = TimeSpan.Parse(dateTimeNow.TimeOfDay.Hours.ToString() + ":" + dateTimeNow.TimeOfDay.Minutes.ToString() + ":00");

                    List<string> listSoundFile = new List<string>();
                    foreach (var item in listSoundTimeConfigError)
                    {
                        if (item.Time == timeNow && item.Time != timeReadSoundError)
                        {
                            timeReadSoundError = timeNow;
                            soLanDocError = item.SoLanDoc;
                            isReadError = true;
                            break;
                        }
                    }
                    if (isReadError)
                    {
                        if (listChuyen != null && listChuyen.Count > 0)
                        {
                            List<string> listSoundPath = new List<string>();
                            foreach (var chuyen in listChuyen)
                            {
                                int idChuyen = chuyen.MaChuyen;
                                //int.TryParse(chuyen.MaChuyen, out idChuyen);
                                if (idChuyen > 0)
                                {
                                    var listItem = GetListSoundItemByChuyen(idChuyen, 2);
                                    if (listItem != null && listItem.Count > 0)
                                    {
                                        Chuyen_SanPham_Short c_sp_short = Findid(idChuyen.ToString());
                                        dtLoadDataAll.Clear();
                                        if (c_sp_short != null)
                                        {
                                            string strSQL = "select csp.SanLuongKeHoach, ROUND(nx.DinhMucNgay,0) DinhMucNgay, nx.ThucHienNgay, csp. LuyKeTH, tp.LaoDongChuyen, (SELECT SUM(BTPNgay) FROM BTP b WHERE b.STTChuyen_SanPham = '" + c_sp_short.STTChuyen_SanPham + "'AND b.Ngay ='" + todayStr + "' and b.IsBTP_PB_HC = 0) BTPNgay, ((SELECT SUM(BTPNgay) FROM BTP b WHERE b.STTChuyen_SanPham = '" + c_sp_short.STTChuyen_SanPham + "' and b.IsBTP_PB_HC = 0)-(SELECT SUM(nx.BTPLoi) FROM NangXuat nx WHERE nx.STTCHuyen_SanPham = '" + c_sp_short.STTChuyen_SanPham + "' and nx.IsDeleted=0)) LuyKeBTP, nx.BTPTrenChuyen, nx.NhipDoSanXuat, nx.NhipDoThucTe, (nx.BTPTrenChuyen/tp.LaoDongChuyen) BTPTrenLD, nx.BTPThoatChuyenNgay, csp.LuyKeBTPThoatChuyen, (nx.SanLuongLoi-nx.SanLuongLoiGiam) as SanLuongLoi, nx.NhipDoThucTeBTPThoatChuyen FROM SanPham sp, ThanhPham tp, Chuyen_SanPham csp, NangXuat nx WHERE csp.STT ='" + c_sp_short.STTChuyen_SanPham + "' AND nx.STTCHuyen_SanPham = csp.STT and nx.IsDeleted=0  and nx.Ngay ='" + todayStr + "' AND csp.MaSanPham = sp.MaSanPham AND tp.STTChuyen_SanPham = csp.STT AND tp.Ngay = nx.Ngay";
                                            if (sqlCon.State == ConnectionState.Open)
                                            {
                                                sqlCon.Close();
                                            }
                                            sqlCon.Open();
                                            da = new SqlDataAdapter(strSQL, sqlCon);
                                            da.Fill(dtLoadDataAll);
                                            if (dtLoadDataAll != null && dtLoadDataAll.Rows.Count > 0)
                                            {
                                                foreach (var item in listItem)
                                                {
                                                    if (item.FileType != 0)
                                                    {
                                                        listSoundPath.Add(item.SoundPath);
                                                    }
                                                    else
                                                    {
                                                        DataRow row = dtLoadDataAll.Rows[0];
                                                        if (!string.IsNullOrEmpty(item.Formula))
                                                        {
                                                            var arrFormulaHasNSHours = item.Formula.Split(new char[] { '|' });
                                                            if (arrFormulaHasNSHours != null && arrFormulaHasNSHours.Length > 1)
                                                            {
                                                                for (int i = 1; i < arrFormulaHasNSHours.Length; i += 2)
                                                                {
                                                                    int sanLuongGio = 0;
                                                                    int sanLuongGioTang = 0;
                                                                    int sanLuongGioGiam = 0;
                                                                    int intMinuter = 0;
                                                                    var arrIntInfo = arrFormulaHasNSHours[1].Split(new char[] { '_' });
                                                                    if (arrIntInfo.Length == 1)
                                                                    {
                                                                        int.TryParse(arrFormulaHasNSHours[1], out intMinuter);
                                                                        if (intMinuter > 0)
                                                                        {
                                                                            var timeStart = dateTimeNow.AddMinutes(-intMinuter).TimeOfDay;
                                                                            string strSQLNSHours = "select (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + idChuyen + " and Time >= '" + timeStart + "' and Time <='" + timeNow + "' and Date='" + dateTimeNow + "' and CommandTypeId=" + (int)eCommandRecive.ErrorIncrease + " and IsEndOfLine=1)  AS SanLuongTang, (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + idChuyen + " and Time >= '" + timeStart + "' and Time <='" + timeNow + "' and Date='" + dateTimeNow + "' and CommandTypeId=" + (int)eCommandRecive.ErrorReduce + "  and IsEndOfLine=1)  AS SanLuongGiam";
                                                                            if (sqlCon.State == ConnectionState.Open)
                                                                            {
                                                                                sqlCon.Close();
                                                                            }
                                                                            sqlCon.Open();
                                                                            da = new SqlDataAdapter(strSQLNSHours, sqlCon);
                                                                            dtNSHoursError.Clear();
                                                                            da.Fill(dtNSHoursError);
                                                                            if (dtNSHoursError != null && dtNSHoursError.Rows.Count > 0)
                                                                            {
                                                                                DataRow rowSanLuongGio = dtNSHoursError.Rows[0];
                                                                                if (rowSanLuongGio["SanLuongTang"] != null)
                                                                                    int.TryParse(rowSanLuongGio["SanLuongTang"].ToString(), out sanLuongGioTang);
                                                                                if (rowSanLuongGio["SanLuongGiam"] != null)
                                                                                    int.TryParse(rowSanLuongGio["SanLuongGiam"].ToString(), out sanLuongGioGiam);
                                                                                sanLuongGio = sanLuongGioTang - sanLuongGioGiam;
                                                                            }
                                                                            if (sanLuongGio < 0)
                                                                                sanLuongGio = 0;
                                                                            item.Formula = item.Formula.Replace("[SoLuongLoiCachGioHienTai|" + intMinuter + "|Phut]", sanLuongGio.ToString());
                                                                        }
                                                                    }
                                                                    else if (arrIntInfo.Length == 2)
                                                                    {
                                                                        int errorId = 1;
                                                                        int.TryParse(arrIntInfo[1], out errorId);
                                                                        if (errorId > 0)
                                                                        {
                                                                            string strSQLNSHours = "select (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + idChuyen + " and ErrorId=" + errorId + " and Date='" + dateTimeNow + "' and CommandTypeId=" + (int)eCommandRecive.ErrorIncrease + " and IsEndOfLine=1)  AS SanLuongTang, (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + idChuyen + " and ErrorId=" + errorId + " and Date='" + dateTimeNow + "' and CommandTypeId=" + (int)eCommandRecive.ErrorReduce + "  and IsEndOfLine=1)  AS SanLuongGiam";
                                                                            if (sqlCon.State == ConnectionState.Open)
                                                                            {
                                                                                sqlCon.Close();
                                                                            }
                                                                            sqlCon.Open();
                                                                            da = new SqlDataAdapter(strSQLNSHours, sqlCon);
                                                                            dtNSHoursError.Clear();
                                                                            da.Fill(dtNSHoursError);
                                                                            if (dtNSHoursError != null && dtNSHoursError.Rows.Count > 0)
                                                                            {
                                                                                DataRow rowSanLuongGio = dtNSHoursError.Rows[0];
                                                                                if (rowSanLuongGio["SanLuongTang"] != null)
                                                                                    int.TryParse(rowSanLuongGio["SanLuongTang"].ToString(), out sanLuongGioTang);
                                                                                if (rowSanLuongGio["SanLuongGiam"] != null)
                                                                                    int.TryParse(rowSanLuongGio["SanLuongGiam"].ToString(), out sanLuongGioGiam);
                                                                                sanLuongGio = sanLuongGioTang - sanLuongGioGiam;
                                                                            }
                                                                            if (sanLuongGio < 0)
                                                                                sanLuongGio = 0;
                                                                            item.Formula = item.Formula.Replace("[SoLuongLoi|" + arrIntInfo[0] + "_" + errorId + "|]", sanLuongGio.ToString());
                                                                        }
                                                                    }
                                                                    else if (arrIntInfo.Length == 3)
                                                                    {
                                                                        int.TryParse(arrIntInfo[2], out intMinuter);
                                                                        int errorId = 1;
                                                                        int.TryParse(arrIntInfo[1], out errorId);
                                                                        if (intMinuter > 0 && errorId > 0)
                                                                        {
                                                                            var timeStart = dateTimeNow.AddMinutes(-intMinuter).TimeOfDay;
                                                                            string strSQLNSHours = "select (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + idChuyen + " and ErrorId=" + errorId + " and Time >= '" + timeStart + "' and Time <='" + timeNow + "' and Date='" + dateTimeNow + "' and CommandTypeId=" + (int)eCommandRecive.ErrorIncrease + " and IsEndOfLine=1)  AS SanLuongTang, (select Sum(ThanhPham) from TheoDoiNgay where MaChuyen =" + idChuyen + " and ErrorId=" + errorId + " and Time >= '" + timeStart + "' and Time <='" + timeNow + "' and Date='" + dateTimeNow + "' and CommandTypeId=" + (int)eCommandRecive.ErrorReduce + "  and IsEndOfLine=1)  AS SanLuongGiam";
                                                                            if (sqlCon.State == ConnectionState.Open)
                                                                            {
                                                                                sqlCon.Close();
                                                                            }
                                                                            sqlCon.Open();
                                                                            da = new SqlDataAdapter(strSQLNSHours, sqlCon);
                                                                            dtNSHoursError.Clear();
                                                                            da.Fill(dtNSHoursError);
                                                                            if (dtNSHoursError != null && dtNSHoursError.Rows.Count > 0)
                                                                            {
                                                                                DataRow rowSanLuongGio = dtNSHoursError.Rows[0];
                                                                                if (rowSanLuongGio["SanLuongTang"] != null)
                                                                                    int.TryParse(rowSanLuongGio["SanLuongTang"].ToString(), out sanLuongGioTang);
                                                                                if (rowSanLuongGio["SanLuongGiam"] != null)
                                                                                    int.TryParse(rowSanLuongGio["SanLuongGiam"].ToString(), out sanLuongGioGiam);
                                                                                sanLuongGio = sanLuongGioTang - sanLuongGioGiam;
                                                                            }
                                                                            if (sanLuongGio < 0)
                                                                                sanLuongGio = 0;
                                                                            item.Formula = item.Formula.Replace("[SoLuongLoiCachThoiGianHienTai|" + arrIntInfo[0] + "_" + errorId + "_" + intMinuter + "|Phut]", sanLuongGio.ToString());
                                                                        }
                                                                    }

                                                                }
                                                            }
                                                            item.Formula = item.Formula.Replace("[SanLuongKeHoach]", row["SanLuongKeHoach"].ToString());
                                                            item.Formula = item.Formula.Replace("[DinhMucNgay]", row["DinhMucNgay"].ToString());
                                                            item.Formula = item.Formula.Replace("[ThucHienNgay]", row["ThucHienNgay"].ToString());
                                                            item.Formula = item.Formula.Replace("[LuyKeThucHien]", row["LuyKeTH"].ToString());
                                                            item.Formula = item.Formula.Replace("[LaoDongChuyen]", row["LaoDongChuyen"].ToString());
                                                            item.Formula = item.Formula.Replace("[BTPNgay]", row["BTPNgay"].ToString());
                                                            item.Formula = item.Formula.Replace("[LuyKeBTP]", row["LuyKeBTP"].ToString());
                                                            item.Formula = item.Formula.Replace("[BTPTrenChuyen]", row["BTPTrenChuyen"].ToString());
                                                            item.Formula = item.Formula.Replace("[NhipDoSanXuat]", row["NhipDoSanXuat"].ToString());
                                                            item.Formula = item.Formula.Replace("[NhipDoThucTe]", row["NhipDoThucTe"].ToString());
                                                            item.Formula = item.Formula.Replace("[BTPTrenLD]", row["BTPTrenLD"].ToString());
                                                            item.Formula = item.Formula.Replace("[BTPThoatChuyenNgay]", row["BTPThoatChuyenNgay"].ToString());
                                                            item.Formula = item.Formula.Replace("[LuyKeBTPThoatChuyen]", row["LuyKeBTPThoatChuyen"].ToString());
                                                            item.Formula = item.Formula.Replace("[SanLuongLoi]", row["SanLuongLoi"].ToString());
                                                            item.Formula = item.Formula.Replace("[NhipDoThucTeBTPThoatChuyen]", row["NhipDoThucTeBTPThoatChuyen"].ToString());
                                                            item.Formula = item.Formula.Replace("[SoGioLamViecTrongNgay]", ((int)TimeIsWorkNS(idChuyen, 0).TotalSeconds).ToString());
                                                            item.Formula = item.Formula.Replace("[SoGioLamViecHienTai]", ((int)TimeIsWorkNS(idChuyen, 1).TotalSeconds).ToString());
                                                            item.Formula = ExprHelper.FormatExpression(item.Formula).Replace(" ", "");
                                                            var result = (decimal)expressionError.EvaluatePrefix(expressionError.Infix2Prefix(item.Formula));
                                                            string chu = string.Empty;
                                                            if (item.IsProductivity)
                                                            {
                                                                if (result > 0)
                                                                    chu += "Vượt ";
                                                            }
                                                            if (result < 0)
                                                            {
                                                                chu += "Âm ";
                                                                result = -result;
                                                            }
                                                            chu += ConvertSoToChu.replace_special_word(ConvertSoToChu.join_unit(result.ToString()));
                                                            var listPath = LoadListSoundFileByString(chu);
                                                            if (listPath != null && listPath.Count > 0)
                                                                listSoundPath.AddRange(listPath);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (listSoundPath.Count > 0)
                            {
                                for (int i = 0; i < soLanDoc; i++)
                                {
                                    foreach (var path in listSoundPath)
                                    {
                                        InformationPlay inf = new InformationPlay { SoundChuyen = path, Repeat = 1 };
                                        queuePlayFile.Enqueue(inf);
                                    }
                                }
                            }
                        }
                        isReadError = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        private void SetupProductOnDay_N()
        {
            KeypadInit();
        }

        private void KeypadInit()
        {
            try
            {
                if (listChuyen != null && listChuyen.Count > 0)
                {
                    #region
                    // listKeyPadObjectInfo = new List<DuAn03_HaiDang.Model.ModelKeyPadObject>();
                    listKeyPadObjectInfo = new List<KeypadModel>();
                    listMapIdSanPhamNgay = new List<PMS.Data.MapIdSanPhamNgay>();
                    string strListChuyen = string.Empty;
                    List<string> listStrProductConfig = new List<string>();
                    var lineIds = AccountSuccess.strListChuyenId.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                    var nxOnDay = BLLProductivity.GetProductivitiesInDay(todayStr, lineIds);
                    var errors = BLLError.GetAll();

                    foreach (var chuyen in listChuyen)
                    {
                        var keypads = BLLKeyPad.GetKeyPadInfoByLineId(chuyen.MaChuyen);  // keyPadObjectDAO.GetKeyPadInfoByLineId(chuyen.MaChuyen);
                        listKeyPadObjectInfo.AddRange(keypads);

                        strListChuyen += chuyen.MaChuyen + ",";
                        int stt = 0;
                        var phancong = nxOnDay.Where(x => x.LineId == chuyen.MaChuyen).OrderBy(x => x.OrderIndex).ToList();
                        if (phancong != null && phancong.Count > 0)
                        {
                            var listStrSend = new List<string>();
                            var listStrSetSanLuong = new List<string>();
                            foreach (var item in phancong)
                            {
                                var mapIdSanPhamNgay = new PMS.Data.MapIdSanPhamNgay();
                                mapIdSanPhamNgay.MaChuyen = chuyen.MaChuyen;
                                mapIdSanPhamNgay.STTChuyenSanPham = item.STTCHuyen_SanPham;
                                mapIdSanPhamNgay.MaSanPham = item.productId;
                                stt += 1;
                                mapIdSanPhamNgay.STT = stt;
                                mapIdSanPhamNgay.Ngay = todayStr;
                                listMapIdSanPhamNgay.Add(mapIdSanPhamNgay);
                                bool result = CheckExistMapIdSanPhamNgay(chuyen.MaChuyen, item.STTCHuyen_SanPham);
                                if (!result)
                                {
                                    string strSend = mapIdSanPhamNgay.STT.ToString() + ",";
                                    if (!string.IsNullOrEmpty(item.ProductName))
                                        strSend += item.ProductName.Length > 10 ? item.ProductName.Substring(0, 10) : item.ProductName;
                                    listStrSend.Add(strSend);
                                    listStrSetSanLuong.Add(mapIdSanPhamNgay.STT.ToString());
                                }
                            }
                            if (keypads != null && keypads.Count > 0)
                            {
                                if (listStrSend.Count > 0 || listStrSetSanLuong.Count > 0)
                                {
                                    foreach (var kp_Obj in keypads)
                                    {
                                        if (kp_Obj.UseTypeId == (int)eUseKeyPadType.OneKeyPadOneObject)
                                        {
                                            listStrProductConfig.Add(kp_Obj.EquipmentId.ToString() + "," + (int)eCommandSend.ClearData + ",,");
                                            foreach (var strSend in listStrSend)
                                            {
                                                listStrProductConfig.Add(kp_Obj.EquipmentId.ToString() + "," + (int)eCommandSend.ProductConfig + "," + strSend);
                                            }
                                            foreach (var strSend in listStrSetSanLuong)
                                            {
                                                listStrProductConfig.Add(kp_Obj.EquipmentId.ToString() + "," + (int)eCommandSend.ChangeBTPQuantities + "," + strSend + ",0,,");
                                                if (errors != null && errors.Count > 0)
                                                    foreach (var e in errors)
                                                    {
                                                        listStrProductConfig.Add(kp_Obj.EquipmentId.ToString() + "," + (int)eCommandSend.ChangeProductError + "," + strSend + ",0," + e.Code);
                                                    }

                                                listStrProductConfig.Add(kp_Obj.EquipmentId.ToString() + "," + (int)eCommandSend.ChangeProductQuantity + "," + strSend + ",0," + (int)eProductOutputType.KCS);
                                                listStrProductConfig.Add(kp_Obj.EquipmentId.ToString() + "," + (int)eCommandSend.ChangeProductQuantity + "," + strSend + ",0," + (int)eProductOutputType.TC);
                                                switch (kp_Obj.TypeOfKeypad)
                                                {
                                                    case (int)eTypeOfKeypad.All:
                                                        listStrProductConfig.Add(kp_Obj.EquipmentId.ToString() + "," + (int)eCommandSend.ChangeProductQuantity + "," + strSend + ",0," + (int)eProductOutputType.KCS);
                                                        listStrProductConfig.Add(kp_Obj.EquipmentId.ToString() + "," + (int)eCommandSend.ChangeProductQuantity + "," + strSend + ",0," + (int)eProductOutputType.TC);
                                                        break;
                                                    case (int)eTypeOfKeypad.KCS:
                                                        listStrProductConfig.Add(kp_Obj.EquipmentId.ToString() + "," + (int)eCommandSend.ChangeProductQuantity + "," + strSend + ",0," + (int)eProductOutputType.KCS);
                                                        break;
                                                    case (int)eTypeOfKeypad.TC:
                                                        listStrProductConfig.Add(kp_Obj.EquipmentId.ToString() + "," + (int)eCommandSend.ChangeProductQuantity + "," + strSend + ",0," + (int)eProductOutputType.TC);
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(strListChuyen) && listMapIdSanPhamNgay.Count > 0)
                    {
                        var resultAdd = BLLMapCommoIdForDay.AddMapIdSanPhamNgay(strListChuyen.Substring(0, strListChuyen.Length - 1).Split(',').Select(x => Convert.ToInt32(x)).ToList(), listMapIdSanPhamNgay);
                        if (resultAdd)
                        {
                            if (listStrProductConfig.Count > 0)
                                listDataSendKeyPad.AddRange(listStrProductConfig);
                        }
                    }
                    #endregion
                }
                else
                    MessageBox.Show("Lỗi KeypadInit: Không thể khởi tạo thông tin KeyPad. Vì không có danh sách chuyền. Có thể bạn chưa chạy tiến trình tự động.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi KeypadInit: Không thể khởi tạo thông tin KeyPad.\nLỗi Ngoại lệ : " + ex.Message);
            }
        }

        DataTable dtMapIdSanPham = new DataTable();
        private bool CheckExistMapIdSanPhamNgay(int maChuyen, int sttChuyenSanPham)
        {
            try
            {
                bool result = false;
                string strSQL = "Select Id From MapIdSanPhamNgay Where Ngay='" + todayStr + "' and MaChuyen=" + maChuyen + " and STTChuyenSanPham=" + sttChuyenSanPham + " and IsDeleted=0";
                if (sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }
                sqlCon.Open();
                da = new SqlDataAdapter(strSQL, sqlCon);
                dtMapIdSanPham.Clear();
                da.Fill(dtMapIdSanPham);
                if (dtMapIdSanPham != null && dtMapIdSanPham.Rows.Count > 0)
                {
                    result = true;
                }
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #region ----------           SU LY SAN LUONG NHAN TU KEYPAD        ------------------------
        public void KeyPadInsertProcessing(int clusterId, int quantityIncrease, int sttChuyenSanPham, int maSanPham, int productCode, int lineId, bool isEndOfLine, int errorId, int total, int equipmentId, bool isIncrease, int ProductType)
        {
            var a = BLLDayInfo.KeypadInsert(clusterId, quantityIncrease, sttChuyenSanPham, maSanPham, productCode, lineId, isEndOfLine, errorId, total, equipmentId, isIncrease, ProductType, setTotalByMinOrMax, getBTPInLineByType);
        }

        public bool TangSanLuongKSC(int clusterId, int quantityIncrease, int sttChuyenSanPham, int maSanPham, int productCode, int lineId, bool isEndOfLine, int total, int equipmentId)
        {
            try
            {
                bool result = false, isFinish = false, isCheckAndSendTotal = false;
                var lineInfo = chuyenDAO.GetLineById(lineId.ToString(), AccountSuccess.strListChuyenId);
                if (sttChuyenSanPham == 0)
                {
                    isCheckAndSendTotal = true;
                    if (listMapIdSanPhamNgay != null && listMapIdSanPhamNgay.Count > 0)
                    {
                        var mapIdSanPhamNgay = listMapIdSanPhamNgay.Where(c => c.MaChuyen == lineId && c.STT == productCode && c.Ngay == todayStr).FirstOrDefault();
                        if (mapIdSanPhamNgay != null)
                        {
                            sttChuyenSanPham = mapIdSanPhamNgay.STTChuyenSanPham;
                            maSanPham = mapIdSanPhamNgay.MaSanPham;
                        }
                    }
                }
                if (sttChuyenSanPham > 0)
                {
                    //  MessageBox.Show("toi day" + todayStr + " - " + sttChuyenSanPham + " - " + lineId, "");
                    var chuyenSanPham = BLLAssignmentForLine.Instance.GetAssignmentByDay(todayStr, sttChuyenSanPham, lineId);
                    var nangSuatCum = BLLProductivity.Find_NangSuatCum(sttChuyenSanPham, clusterId, todayStr);
                    if (nangSuatCum != null && chuyenSanPham != null && chuyenSanPham.LuyKeTH < chuyenSanPham.SanLuongKeHoach)
                    {
                        setTotalByMinOrMax = setTotalByMinOrMax_default;
                        if (lineId == LineId_Insert && clusterId != ClusterId_Insert && ActionName == eActionName.KCS)
                            if (IsIncresea)
                                setTotalByMinOrMax = 2;
                            else
                                setTotalByMinOrMax = 1;

                        bool vuotDM = false;
                        if ((chuyenSanPham.LuyKeTH + quantityIncrease) > chuyenSanPham.SanLuongKeHoach)
                        {
                            quantityIncrease = chuyenSanPham.SanLuongKeHoach - chuyenSanPham.LuyKeTH;
                            vuotDM = true;
                        }

                        #region
                        int min = total, max = total;
                        nangSuatCum.SanLuongKCSTang += quantityIncrease;
                        if (vuotDM)
                            total = nangSuatCum.SanLuongKCSTang - nangSuatCum.SanLuongKCSGiam;

                        if (isCheckAndSendTotal)
                        {
                            int tongSanLuong = nangSuatCum.SanLuongKCSTang - nangSuatCum.SanLuongKCSGiam;
                            if (tongSanLuong != total)
                            {
                                if (tongSanLuong > total)
                                {
                                    min = total;
                                    max = tongSanLuong;
                                    switch (setTotalByMinOrMax)
                                    {
                                        case 1:  //nho
                                        case 3:  // keypad
                                            nangSuatCum.SanLuongKCSGiam = (max - min);
                                            break;
                                        case 2:  // lon
                                            listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.ChangeProductQuantity + "," + productCode + "," + max + "," + (int)eProductOutputType.KCS);
                                            break;
                                    }
                                }
                                else
                                {
                                    min = tongSanLuong;
                                    max = total;
                                    switch (setTotalByMinOrMax)
                                    {
                                        case 1:
                                            listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.ChangeProductQuantity + "," + productCode + "," + min + "," + (int)eProductOutputType.KCS);
                                            break;
                                        case 2:
                                        case 3:
                                            nangSuatCum.SanLuongKCSTang += (max - min);
                                            break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            min = max = (nangSuatCum.SanLuongKCSTang - nangSuatCum.SanLuongKCSGiam);
                        }
                        #endregion

                        BLLProductivity.Update_NS_Cum(nangSuatCum);
                        // MessageBox.Show("isEndOfLine : " + isEndOfLine.ToString(), "");
                        if (isEndOfLine)
                        {
                            #region
                            var nangSuat = BLLProductivity.TTNangXuatTrongNgay(todayStr, sttChuyenSanPham);
                            var monthPro = BLLMonthlyProductionPlans.Find(sttChuyenSanPham, DateTime.Now.Month, DateTime.Now.Year);

                            if (nangSuat != null && chuyenSanPham != null)
                            {
                                var tong = nangSuat.ThucHienNgay - nangSuat.ThucHienNgayGiam;
                                switch (setTotalByMinOrMax)
                                {
                                    case (int)eSetTotalByMinOrMax.byMin:
                                        nangSuat.ThucHienNgay = min;
                                        nangSuat.ThucHienNgayGiam = 0;
                                        break;
                                    case (int)eSetTotalByMinOrMax.byMax:
                                        nangSuat.ThucHienNgay = max;
                                        nangSuat.ThucHienNgayGiam = 0;
                                        break;
                                    case (int)eSetTotalByMinOrMax.byKeypad:
                                        nangSuat.ThucHienNgay = total;
                                        nangSuat.ThucHienNgayGiam = 0;
                                        break;
                                }
                                monthPro.LK_TH = (monthPro.LK_TH - tong) + nangSuat.ThucHienNgay;
                                chuyenSanPham.LuyKeTH = (chuyenSanPham.LuyKeTH - tong) + nangSuat.ThucHienNgay;

                                nangSuat.IsBTP = 1;
                                SoundChuyen = listChuyen.FirstOrDefault(x => x.MaChuyen == lineId).Sound;// chuyenSanPham.SoundChuyen;

                                #region check finish production
                                isFinish = CheckFinishProduction(TypeOfCheckFinishProduction, chuyenSanPham);
                                if (isFinish)
                                {
                                    chuyenSanPham.IsFinish = true;
                                    chuyenSanPham.IsFinishNow = true;
                                    chuyenSanPham.STTThucHien = 900;
                                    if (listBaoHetHang.Count > 0)
                                    {
                                        var objAlertFinish = listBaoHetHang.FirstOrDefault(x => x.SoLuongCon <= (chuyenSanPham.SanLuongKeHoach - chuyenSanPham.LuyKeBTPThoatChuyen));
                                        if (objAlertFinish != null)
                                        {
                                            Repeat = objAlertFinish.SoLanBao;
                                            if (chuyenSanPham.CountAssignment <= 1)
                                            {
                                                InformationPlay inf = new InformationPlay { SoundChuyen = SoundChuyen, Repeat = Repeat };
                                                queuePlayFile.Enqueue(inf);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    chuyenSanPham.IsFinish = false;
                                    chuyenSanPham.IsFinishNow = false;
                                }
                                #endregion

                                TimeSpan hieutime = new TimeSpan();
                                hieutime = TimeIsWork(lineId);
                                int second = (int)hieutime.TotalSeconds;
                                int nhipDoThucTe = ((nangSuat.ThucHienNgay - nangSuat.ThucHienNgayGiam) == 0 ? 0 : second / (nangSuat.ThucHienNgay - nangSuat.ThucHienNgayGiam));
                                nangSuat.NhipDoThucTe = nhipDoThucTe;
                                switch (getBTPInLineByType)
                                {
                                    case 1:
                                        nangSuat.BTPTrenChuyen = (chuyenSanPham.LK_BTP - chuyenSanPham.LK_BTP_G) - chuyenSanPham.LuyKeTH;
                                        break;
                                    case 2:
                                        nangSuat.BTPTrenChuyen = (chuyenSanPham.LK_BTP - chuyenSanPham.LK_BTP_G) - chuyenSanPham.LuyKeBTPThoatChuyen;
                                        break;
                                }

                                //     MessageBox.Show("toi day ns:"+nangSuat.ThucHienNgay , ""+nangSuat.STTCHuyen_SanPham);
                                nangSuat.TimeLastChange = DateTime.Now.TimeOfDay;
                                BLLProductivity.UpdateNangXuat(nangSuat);
                                BLLMonthlyProductionPlans.Update(monthPro);
                                BLLAssignmentForLine.Instance.Update(sttChuyenSanPham, chuyenSanPham.LuyKeTH, null, chuyenSanPham.IsFinish, chuyenSanPham.IsFinishNow);
                                isFinish = chuyenSanPham.IsFinish;
                            }
                            #endregion

                            #region
                            var tdns = BLLDayInfo.GetByCommoId(sttChuyenSanPham, maSanPham, todayStr);
                            int kcsTang = 0, kcsGiam = 0;
                            kcsTang = tdns.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductIncrease && c.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(c => c.ThanhPham);
                            kcsGiam = tdns.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductReduce && c.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(c => c.ThanhPham);
                            kcsTang = (kcsTang - kcsGiam) < 0 ? 0 : (kcsTang - kcsGiam);

                            var tdn = new PMS.Data.TheoDoiNgay();
                            tdn.MaChuyen = lineId;
                            tdn.MaSanPham = maSanPham;
                            tdn.CumId = clusterId;
                            tdn.STTChuyenSanPham = sttChuyenSanPham;
                            tdn.Time = DateTime.Now.TimeOfDay;
                            tdn.Date = todayStr;
                            tdn.ProductOutputTypeId = (int)eProductOutputType.KCS;
                            tdn.IsEndOfLine = isEndOfLine;
                            tdn.IsEnterByKeypad = true;
                            tdn.EquipmentId = equipmentId;
                            if (kcsTang > nangSuat.ThucHienNgay)
                            {
                                tdn.ThanhPham = kcsTang - nangSuat.ThucHienNgay;
                                tdn.CommandTypeId = (int)eCommandRecive.ProductReduce;
                            }
                            else
                            {
                                tdn.ThanhPham = nangSuat.ThucHienNgay - kcsTang;
                                tdn.CommandTypeId = (int)eCommandRecive.ProductIncrease;
                            }
                            BLLDayInfo.KeypadInsert(tdn);
                            #endregion
                        }
                        result = true;

                        IndexChuyen = sttChuyenSanPham.ToString();
                        listIndexChuyen.Add(IndexChuyen);

                        #region kết thúc đơn hàng update lại thông tin keypad
                        if (isFinish)
                        {
                            var rs = BLLDayInfo.CreateNewDayInfoAfterFinishAssignment(lineId);
                            if (!rs.IsSuccess && rs.Messages[0] != null)
                                MessageBox.Show(rs.Messages[0].msg, rs.Messages[0].Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            else
                                BLLProductivity.ResetNormsDayAndBTPInLine(getBTPInLineByType, calculateNormsdayType, TypeOfCaculateDayNorms, lineId, false, todayStr);

                            DuAn03_HaiDang.Helper.HelperControl.ResetKeypad(lineId, false, this);
                        }
                        #endregion

                        countTimeSendRequestKCSButHandleError = 0;

                        ////
                        LineId_Insert = lineId;

                        if (currentAssignments.Count > 0)
                        {
                            var obj = currentAssignments.FirstOrDefault(x => x.AcctionType == (int)eProductOutputType.KCS && sttChuyenSanPham == x.AssignId && x.errorId == 0);
                            if (obj != null)
                                currentAssignments.Add(new CurrentAssignmentObj() { AssignId = sttChuyenSanPham, AcctionType = (int)eProductOutputType.KCS, errorId = 0 });
                        }
                        else
                            currentAssignments.Add(new CurrentAssignmentObj() { AssignId = sttChuyenSanPham, AcctionType = (int)eProductOutputType.KCS, errorId = 0 });

                        ClusterId_Insert = clusterId;
                        ActionName = eActionName.KCS;
                        IsIncresea = true;

                    }
                    else
                    {
                        if (lineInfo != null)
                        {
                            InformationPlay inf = new InformationPlay { SoundChuyen = lineInfo.Sound, Repeat = 1 };
                            queuePlayFile.Enqueue(inf);
                        }
                    }

                    listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.HandlingSuccess + "," + productCode + ",," + (int)eProductOutputType.KCS);
                    Helper.HelperControl.ResetKeypad(lineId, sttChuyenSanPham, 0, (int)eProductOutputType.KCS, todayStr, this);
                    BLLHistoryPressedKeypad.Instance.Update(lineId, sttChuyenSanPham, todayStr);
                    BLLProductivity.ResetNormsDayAndBTPInLine(getBTPInLineByType, calculateNormsdayType, TypeOfCaculateDayNorms, lineId, false, todayStr);
                }
                else
                {
                    #region
                    if (lineInfo != null)
                    {
                        InformationPlay inf = new InformationPlay { SoundChuyen = lineInfo.Sound, Repeat = 1 };
                        queuePlayFile.Enqueue(inf);
                    }

                    if (countTimeSendRequestKCSButHandleError == timeSendRequestKCSButHandleError)
                    {
                        listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.HandlingSuccess + "," + productCode + ",," + (int)eProductOutputType.KCS);
                        countTimeSendRequestKCSButHandleError = 0;
                    }
                    else
                        countTimeSendRequestKCSButHandleError++;
                    #endregion
                }


                return result;
            }
            catch (Exception ex)
            {
                // GhiFileLog(DateTime.Now + "loi TangSanLuongKSC ex :" + ex.Message + " \n");
            }
            return false;
        }

        private bool CheckFinishProduction(List<string> TypeOfCheckFinishProduction, ChuyenSanPhamModel chuyenSanPham)
        {
            int count = 0;
            if (TypeOfCheckFinishProduction != null && TypeOfCheckFinishProduction.Count > 0)
                foreach (var item in TypeOfCheckFinishProduction)
                    if (item == "KCS" && chuyenSanPham.LuyKeTH >= chuyenSanPham.SanLuongKeHoach)
                        count++;
                    else if (item == "TC" && chuyenSanPham.LuyKeBTPThoatChuyen >= chuyenSanPham.SanLuongKeHoach)
                        count++;
                    else if (item == "BTP" && chuyenSanPham.LK_BTP >= chuyenSanPham.SanLuongKeHoach)
                        count++;

            if (TypeOfCheckFinishProduction.Count == count)
                return true;
            return false;
        }

        public bool GiamSanLuongKSC(int clusterId, int quantityIncrease, int sttChuyenSanPham, int maSanPham, int productCode, int lineId, bool isEndOfLine, int total, int equipmentId)
        {
            try
            {
                bool result = false, IsFinish = false, isCheckAndSendTotal = false;
                var lineInfo = chuyenDAO.GetLineById(lineId.ToString(), AccountSuccess.strListChuyenId);
                if (sttChuyenSanPham == 0)
                {
                    isCheckAndSendTotal = true;
                    if (listMapIdSanPhamNgay != null && listMapIdSanPhamNgay.Count > 0)
                    {
                        var mapIdSanPhamNgay = listMapIdSanPhamNgay.Where(c => c.MaChuyen == lineId && c.STT == productCode && c.Ngay == todayStr).FirstOrDefault();
                        if (mapIdSanPhamNgay != null)
                        {
                            sttChuyenSanPham = mapIdSanPhamNgay.STTChuyenSanPham;
                            maSanPham = mapIdSanPhamNgay.MaSanPham;
                        }
                    }
                }
                if (sttChuyenSanPham > 0)
                {
                    var nangSuatCum = BLLProductivity.Find_NangSuatCum(sttChuyenSanPham, clusterId, todayStr);
                    if (nangSuatCum != null)
                    {
                        setTotalByMinOrMax = setTotalByMinOrMax_default;
                        if (lineId == LineId_Insert && clusterId != ClusterId_Insert && ActionName == eActionName.KCS)
                            if (IsIncresea)
                                setTotalByMinOrMax = 2;
                            else
                                setTotalByMinOrMax = 1;

                        nangSuatCum.SanLuongKCSGiam += quantityIncrease;
                        int min = total, max = total;
                        if (isCheckAndSendTotal)
                        {
                            #region
                            int tongSanLuong = nangSuatCum.SanLuongKCSTang - nangSuatCum.SanLuongKCSGiam;
                            if (tongSanLuong != total)
                            {
                                if (tongSanLuong > total)
                                {
                                    min = total;
                                    max = tongSanLuong;
                                    switch (setTotalByMinOrMax)
                                    {
                                        case 1:
                                        case 3:
                                            nangSuatCum.SanLuongKCSGiam = (max - min);
                                            break;
                                        case 2:
                                            listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.ChangeProductQuantity + "," + productCode + "," + max + "," + (int)eProductOutputType.KCS);
                                            break;
                                    }
                                }
                                else
                                {
                                    min = tongSanLuong;
                                    max = total;
                                    switch (setTotalByMinOrMax)
                                    {
                                        case 1:
                                            listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.ChangeProductQuantity + "," + productCode + "," + min + "," + (int)eProductOutputType.KCS);
                                            break;
                                        case 2:
                                        case 3:
                                            nangSuatCum.SanLuongKCSGiam = nangSuatCum.SanLuongKCSGiam + (max - min);
                                            break;
                                    }
                                }
                            }
                            #endregion
                        }
                        BLLProductivity.Update_NS_Cum(nangSuatCum);
                        if (isEndOfLine)
                        {
                            #region
                            var nangSuat = BLLProductivity.TTNangXuatTrongNgay(todayStr, sttChuyenSanPham);// GetNangSuatChuyenNgay(sttChuyenSanPham, ngay);
                            var chuyenSanPham = BLLAssignmentForLine.Instance.GetAssignmentByDay(todayStr, sttChuyenSanPham, lineId);// chuyenSanPhamDAO.GetChuyenSanPham(DateTime.Now, sttChuyenSanPham, lineId);
                            var monthPro = BLLMonthlyProductionPlans.Find(sttChuyenSanPham, DateTime.Now.Month, DateTime.Now.Year);

                            var old = nangSuat.ThucHienNgay - nangSuat.ThucHienNgayGiam;
                            switch (setTotalByMinOrMax)
                            {
                                case (int)eSetTotalByMinOrMax.byMin:
                                    nangSuat.ThucHienNgay = min;
                                    nangSuat.ThucHienNgayGiam = 0;
                                    break;
                                case (int)eSetTotalByMinOrMax.byMax:
                                    nangSuat.ThucHienNgay = max;
                                    nangSuat.ThucHienNgayGiam = 0;
                                    break;
                                case (int)eSetTotalByMinOrMax.byKeypad:
                                    nangSuat.ThucHienNgay = total;
                                    nangSuat.ThucHienNgayGiam = 0;
                                    break;
                            }
                            chuyenSanPham.LuyKeTH = ((chuyenSanPham.LuyKeTH - old) + nangSuat.ThucHienNgay);// (flag ? (chuyenSanPham.LuyKeTH + quantityIncrease) : (chuyenSanPham.LuyKeTH - quantityIncrease));
                            monthPro.LK_TH = (monthPro.LK_TH - old) + nangSuat.ThucHienNgay;

                            nangSuat.IsBTP = 1;
                            var line = listChuyen.FirstOrDefault(x => x.MaChuyen == lineId);

                            #region
                            // update 16/112017
                            // thay doi so sanh tu lk_KCS thành LK_TC
                            //if (listBaoHetHang.Count > 0)
                            //{
                            //    if ((chuyenSanPham.SanLuongKeHoach - chuyenSanPham.LuyKeBTPThoatChuyen) <= listBaoHetHang[0].SoLuongCon)
                            //    {
                            //        SoundChuyen = line.Sound;
                            //        for (int n = 0; n < listBaoHetHang.Count; n++)
                            //        {
                            //            if ((chuyenSanPham.SanLuongKeHoach - chuyenSanPham.LuyKeBTPThoatChuyen) <= listBaoHetHang[n].SoLuongCon)
                            //            {
                            //                Repeat = listBaoHetHang[n].SoLanBao;
                            //                if (chuyenSanPham.LuyKeBTPThoatChuyen >= chuyenSanPham.SanLuongKeHoach && chuyenSanPham.LuyKeTH >= chuyenSanPham.SanLuongKeHoach)
                            //                {
                            //                    chuyenSanPham.IsFinishNow = true;
                            //                    chuyenSanPham.IsFinish = true;
                            //                    chuyenSanPham.STTThucHien = 900;
                            //                }
                            //            }
                            //            else
                            //                break;

                            //        }
                            //        if (chuyenSanPham.CountAssignment <= 1)
                            //        {
                            //            InformationPlay inf = new InformationPlay { SoundChuyen = SoundChuyen, Repeat = Repeat };
                            //            queuePlayFile.Enqueue(inf);
                            //        }
                            //    }
                            //    else
                            //    {
                            //        if (!chuyenSanPham.IsFinishNow)
                            //            chuyenSanPham.IsFinishNow = false;
                            //        chuyenSanPham.IsFinish = false;
                            //    }
                            //}
                            //else
                            //{
                            //    if (chuyenSanPham.LuyKeBTPThoatChuyen >= chuyenSanPham.SanLuongKeHoach && chuyenSanPham.LuyKeTH >= chuyenSanPham.SanLuongKeHoach)
                            //    {
                            //        chuyenSanPham.IsFinish = true;
                            //        chuyenSanPham.IsFinishNow = true;
                            //        SoundChuyen = line.Sound;
                            //    }
                            //    else
                            //    {
                            //        chuyenSanPham.IsFinish = false;
                            //        chuyenSanPham.IsFinishNow = false;
                            //    }
                            //}
                            #endregion

                            TimeSpan hieutime = new TimeSpan();
                            hieutime = TimeIsWork(lineId);
                            int second = (int)hieutime.TotalSeconds;
                            int thucHienNgay = nangSuat.ThucHienNgay - nangSuat.ThucHienNgayGiam;
                            if (thucHienNgay > 0)
                            {
                                int nhipDoThucTe = second / thucHienNgay;
                                nangSuat.NhipDoThucTe = nhipDoThucTe;
                            }
                            switch (getBTPInLineByType)
                            {
                                case 1:
                                    nangSuat.BTPTrenChuyen = (chuyenSanPham.LK_BTP - chuyenSanPham.LK_BTP_G) - chuyenSanPham.LuyKeTH;
                                    break;
                                case 2:
                                    nangSuat.BTPTrenChuyen = (chuyenSanPham.LK_BTP - chuyenSanPham.LK_BTP_G) - chuyenSanPham.LuyKeBTPThoatChuyen;
                                    break;
                            }
                            nangSuat.TimeLastChange = DateTime.Now.TimeOfDay;
                            BLLProductivity.UpdateNangXuat(nangSuat);
                            BLLMonthlyProductionPlans.Update(monthPro);
                            BLLAssignmentForLine.Instance.Update(sttChuyenSanPham, chuyenSanPham.LuyKeTH, null, chuyenSanPham.IsFinish, chuyenSanPham.IsFinishNow);
                            IsFinish = chuyenSanPham.IsFinish;
                            #endregion

                            #region
                            var tdns = BLLDayInfo.GetByCommoId(sttChuyenSanPham, maSanPham, todayStr);
                            int kcsTang = 0, kcsGiam = 0;
                            kcsTang = tdns.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductIncrease && c.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(c => c.ThanhPham);
                            kcsGiam = tdns.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductReduce && c.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(c => c.ThanhPham);
                            kcsTang = (kcsTang - kcsGiam) < 0 ? 0 : (kcsTang - kcsGiam);

                            var tdn = new PMS.Data.TheoDoiNgay();
                            tdn.MaChuyen = lineId;
                            tdn.MaSanPham = maSanPham;
                            tdn.CumId = clusterId;
                            tdn.STTChuyenSanPham = sttChuyenSanPham;
                            tdn.Time = DateTime.Now.TimeOfDay;
                            tdn.Date = todayStr;
                            tdn.ProductOutputTypeId = (int)eProductOutputType.KCS;
                            tdn.IsEndOfLine = isEndOfLine;
                            tdn.IsEnterByKeypad = true;
                            tdn.EquipmentId = equipmentId;
                            if (kcsTang > nangSuat.ThucHienNgay)
                            {
                                tdn.ThanhPham = kcsTang - nangSuat.ThucHienNgay;
                                tdn.CommandTypeId = (int)eCommandRecive.ProductReduce;
                            }
                            else
                            {
                                tdn.ThanhPham = nangSuat.ThucHienNgay - kcsTang;
                                tdn.CommandTypeId = (int)eCommandRecive.ProductIncrease;
                            }
                            BLLDayInfo.KeypadInsert(tdn);
                            #endregion
                        }

                        result = true;
                        listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.HandlingSuccess + "," + productCode + ",," + (int)eProductOutputType.KCS);
                        IndexChuyen = sttChuyenSanPham.ToString();
                        listIndexChuyen.Add(IndexChuyen);

                        #region kết thúc đơn hàng update lại thông tin keypad
                        if (IsFinish)
                            ResetDayInformation(lineId);

                        countTimeSendRequestKCSButHandleError = 0;
                        #endregion

                        ////
                        LineId_Insert = lineId;
                        ClusterId_Insert = clusterId;
                        ActionName = eActionName.KCS;
                        IsIncresea = false;

                        if (currentAssignments.Count > 0)
                        {
                            var obj = currentAssignments.FirstOrDefault(x => x.AcctionType == (int)eProductOutputType.KCS && sttChuyenSanPham == x.AssignId && x.errorId == 0);
                            if (obj != null)
                                currentAssignments.Add(new CurrentAssignmentObj() { AssignId = sttChuyenSanPham, AcctionType = (int)eProductOutputType.KCS, errorId = 0 });
                        }
                        else
                            currentAssignments.Add(new CurrentAssignmentObj() { AssignId = sttChuyenSanPham, AcctionType = (int)eProductOutputType.KCS, errorId = 0 });

                    }
                    else
                    {
                        if (lineInfo != null)
                        {
                            InformationPlay inf = new InformationPlay { SoundChuyen = lineInfo.Sound, Repeat = 1 };
                            queuePlayFile.Enqueue(inf);
                        }
                    }
                    Helper.HelperControl.ResetKeypad(lineId, sttChuyenSanPham, 0, (int)eProductOutputType.KCS, todayStr, this);
                    BLLHistoryPressedKeypad.Instance.Update(lineId, sttChuyenSanPham, todayStr);
                    BLLProductivity.ResetNormsDayAndBTPInLine(getBTPInLineByType, calculateNormsdayType, TypeOfCaculateDayNorms, lineId, false, todayStr);
                }
                else
                {
                    if (lineInfo != null)
                    {
                        InformationPlay inf = new InformationPlay { SoundChuyen = lineInfo.Sound, Repeat = 1 };
                        queuePlayFile.Enqueue(inf);
                    }
                    if (countTimeSendRequestKCSButHandleError == timeSendRequestKCSButHandleError)
                    {
                        listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.HandlingSuccess + "," + productCode + ",," + (int)eProductOutputType.KCS);
                        countTimeSendRequestKCSButHandleError = 0;
                    }
                    else
                        countTimeSendRequestKCSButHandleError++;
                }


                return result;
            }
            catch (Exception ex)
            {
                //  GhiFileLog(DateTime.Now + "loi GiamSanLuongKSC ex :" + ex.Message + " \n");
            }
            return false;
        }

        private void ResetDayInformation(int lineId)
        {
            var rs = BLLDayInfo.CreateNewDayInfoAfterFinishAssignment(lineId);
            if (!rs.IsSuccess && rs.Messages[0] != null)
                MessageBox.Show(rs.Messages[0].msg, rs.Messages[0].Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                BLLProductivity.ResetNormsDayAndBTPInLine(getBTPInLineByType, calculateNormsdayType, TypeOfCaculateDayNorms, lineId, false, todayStr);
            }
            DuAn03_HaiDang.Helper.HelperControl.ResetKeypad(lineId, false, this);
        }

        public bool TangSanLuongTC(int clusterId, int quantityIncrease, int sttChuyenSanPham, int maSanPham, int productCode, int lineId, bool isEndOfLine, int total, int equipmentId)
        {
            try
            {
                bool result = false, isCheckAndSendTotal = false;
                if (sttChuyenSanPham == 0)
                {
                    isCheckAndSendTotal = true;
                    if (listMapIdSanPhamNgay != null && listMapIdSanPhamNgay.Count > 0)
                    {
                        var mapIdSanPhamNgay = listMapIdSanPhamNgay.Where(c => c.MaChuyen == lineId && c.STT == productCode && c.Ngay == todayStr).FirstOrDefault();
                        if (mapIdSanPhamNgay != null)
                        {
                            sttChuyenSanPham = mapIdSanPhamNgay.STTChuyenSanPham;
                            maSanPham = mapIdSanPhamNgay.MaSanPham;
                        }
                    }
                }
                if (sttChuyenSanPham > 0)
                {
                    bool isfinish = false;
                    var chuyenSanPham = BLLAssignmentForLine.Instance.GetAssignmentByDay(todayStr, sttChuyenSanPham, lineId);


                    var nangSuatCum = BLLProductivity.Find_NangSuatCum(sttChuyenSanPham, clusterId, todayStr);
                    int min = total, max = total;
                    if (nangSuatCum != null && chuyenSanPham != null && chuyenSanPham.LuyKeBTPThoatChuyen < chuyenSanPham.SanLuongKeHoach)
                    {
                        #region
                        setTotalByMinOrMax = setTotalByMinOrMax_default;
                        if (lineId == LineId_Insert && clusterId != ClusterId_Insert && ActionName == eActionName.TC)
                            if (IsIncresea)
                                setTotalByMinOrMax = 2;
                            else
                                setTotalByMinOrMax = 1;
                        bool vuotDM = false;
                        if ((chuyenSanPham.LuyKeBTPThoatChuyen + quantityIncrease) > chuyenSanPham.SanLuongKeHoach)
                        {
                            quantityIncrease = chuyenSanPham.SanLuongKeHoach - chuyenSanPham.LuyKeBTPThoatChuyen;
                            vuotDM = true;
                        }

                        nangSuatCum.SanLuongTCTang += quantityIncrease;
                        total = vuotDM ? (nangSuatCum.SanLuongTCTang - nangSuatCum.SanLuongTCGiam) : total;
                        if (isCheckAndSendTotal)
                        {
                            int tongSanLuong = nangSuatCum.SanLuongTCTang - nangSuatCum.SanLuongTCGiam;
                            if (tongSanLuong != total)
                            {
                                if (tongSanLuong > total)
                                {
                                    min = total;
                                    max = tongSanLuong;
                                    switch (setTotalByMinOrMax)
                                    {
                                        case 1:
                                        case 3:
                                            nangSuatCum.SanLuongTCGiam = (max - min);
                                            break;
                                        case 2:
                                            listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.ChangeProductQuantity + "," + productCode + "," + max + "," + (int)eProductOutputType.TC);
                                            break;
                                    }
                                }
                                else
                                {
                                    min = tongSanLuong;
                                    max = total;
                                    switch (setTotalByMinOrMax)
                                    {
                                        case 1:
                                            listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.ChangeProductQuantity + "," + productCode + "," + min + "," + (int)eProductOutputType.TC);
                                            break;
                                        case 2:
                                        case 3:
                                            nangSuatCum.SanLuongTCTang = nangSuatCum.SanLuongTCTang + (max - min);
                                            break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            min = max = (nangSuatCum.SanLuongTCTang - nangSuatCum.SanLuongTCGiam);
                        }
                        BLLProductivity.Update_NS_Cum(nangSuatCum);
                        #endregion

                        if (isEndOfLine)
                        {
                            #region
                            var nangSuat = BLLProductivity.TTNangXuatTrongNgay(todayStr, sttChuyenSanPham); //GetNangSuatChuyenNgay(sttChuyenSanPham, ngay);
                            var monthPro = BLLMonthlyProductionPlans.Find(sttChuyenSanPham, DateTime.Now.Month, DateTime.Now.Year);

                            var old = nangSuat.BTPThoatChuyenNgay - nangSuat.BTPThoatChuyenNgayGiam;
                            switch (setTotalByMinOrMax)
                            {
                                case 1:
                                    nangSuat.BTPThoatChuyenNgay = min;
                                    nangSuat.BTPThoatChuyenNgayGiam = 0;
                                    break;
                                case 2:
                                    nangSuat.BTPThoatChuyenNgay = max;
                                    nangSuat.BTPThoatChuyenNgayGiam = 0;
                                    break;
                                case 3:
                                    nangSuat.BTPThoatChuyenNgay = total;
                                    nangSuat.BTPThoatChuyenNgayGiam = 0;
                                    break;
                            }
                            #endregion

                            chuyenSanPham.LuyKeBTPThoatChuyen = ((chuyenSanPham.LuyKeBTPThoatChuyen - old) + nangSuat.BTPThoatChuyenNgay);
                            monthPro.LK_TC = (monthPro.LK_TC - old) + nangSuat.BTPThoatChuyenNgay;

                            nangSuat.IsBTP = 1;
                            var line = listChuyen.FirstOrDefault(x => x.MaChuyen == lineId);

                            #region check finish production
                            isfinish = CheckFinishProduction(TypeOfCheckFinishProduction, chuyenSanPham);

                            if (listBaoHetHang.Count > 0)
                            {
                                if ((chuyenSanPham.SanLuongKeHoach - chuyenSanPham.LuyKeBTPThoatChuyen) <= listBaoHetHang[0].SoLuongCon)
                                {
                                    SoundChuyen = line.Sound;
                                    for (int n = 0; n < listBaoHetHang.Count; n++)
                                    {
                                        if ((chuyenSanPham.SanLuongKeHoach - chuyenSanPham.LuyKeBTPThoatChuyen) <= listBaoHetHang[n].SoLuongCon)
                                        {
                                            Repeat = listBaoHetHang[n].SoLanBao;
                                            if (n == listBaoHetHang.Count - 1)
                                                chuyenSanPham.IsFinishBTPThoatChuyen = true;
                                            if (isfinish)
                                            {
                                                chuyenSanPham.IsFinish = true;
                                                chuyenSanPham.STTThucHien = 900;
                                            }
                                        }
                                        else
                                            break;

                                    }
                                    if (chuyenSanPham.CountAssignment <= 1)
                                    {
                                        InformationPlay inf = new InformationPlay { SoundChuyen = SoundChuyen, Repeat = Repeat };
                                        queuePlayFile.Enqueue(inf);
                                    }
                                }
                                else
                                    chuyenSanPham.IsFinishBTPThoatChuyen = false;
                            }
                            else
                            {
                                if (isfinish)
                                {
                                    chuyenSanPham.IsFinish = true;
                                    chuyenSanPham.IsFinishBTPThoatChuyen = true;
                                    chuyenSanPham.STTThucHien = 900;
                                    SoundChuyen = line.Sound;
                                }
                                else
                                    chuyenSanPham.IsFinishBTPThoatChuyen = false;
                            }
                            #endregion

                            TimeSpan hieutime = new TimeSpan();
                            hieutime = TimeIsWork(lineId);
                            int second = (int)hieutime.TotalSeconds;
                            int btpThoatChuyenNgay = (nangSuat.BTPThoatChuyenNgay - nangSuat.BTPThoatChuyenNgayGiam);
                            if (btpThoatChuyenNgay > 0)
                            {
                                int nhipDoThucTeBTPThoatChuyen = second / btpThoatChuyenNgay;
                                nangSuat.NhipDoThucTeBTPThoatChuyen = nhipDoThucTeBTPThoatChuyen;
                            }
                            switch (getBTPInLineByType)
                            {
                                case 1:
                                    nangSuat.BTPTrenChuyen = (chuyenSanPham.LK_BTP - chuyenSanPham.LK_BTP_G) - chuyenSanPham.LuyKeTH;
                                    break;
                                case 2:
                                    nangSuat.BTPTrenChuyen = (chuyenSanPham.LK_BTP - chuyenSanPham.LK_BTP_G) - chuyenSanPham.LuyKeBTPThoatChuyen;
                                    break;
                            }
                            nangSuat.TimeLastChange = DateTime.Now.TimeOfDay;

                            BLLProductivity.UpdateNangXuat(nangSuat);
                            BLLMonthlyProductionPlans.Update(monthPro);
                            BLLAssignmentForLine.Instance.Update(sttChuyenSanPham, null, chuyenSanPham.LuyKeBTPThoatChuyen, chuyenSanPham.IsFinish, chuyenSanPham.IsFinishNow);
                            isfinish = chuyenSanPham.IsFinish;
                            //           ------------------------
                            #region
                            var tdns = BLLDayInfo.GetByCommoId(sttChuyenSanPham, maSanPham, todayStr);
                            int tcTang = 0, tcGiam = 0;
                            tcTang = tdns.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductIncrease && c.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(c => c.ThanhPham);
                            tcGiam = tdns.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductReduce && c.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(c => c.ThanhPham);
                            tcTang = (tcTang - tcGiam) < 0 ? 0 : (tcTang - tcGiam);

                            var tdn = new PMS.Data.TheoDoiNgay();
                            tdn.MaChuyen = lineId;
                            tdn.MaSanPham = maSanPham;
                            tdn.CumId = clusterId;
                            tdn.STTChuyenSanPham = sttChuyenSanPham;
                            tdn.Time = DateTime.Now.TimeOfDay;
                            tdn.Date = todayStr;
                            tdn.ProductOutputTypeId = (int)eProductOutputType.TC;
                            tdn.IsEndOfLine = isEndOfLine;
                            tdn.IsEnterByKeypad = true;
                            tdn.EquipmentId = equipmentId;
                            if (tcTang > nangSuat.BTPThoatChuyenNgay)
                            {
                                tdn.ThanhPham = tcTang - nangSuat.BTPThoatChuyenNgay;
                                tdn.CommandTypeId = (int)eCommandRecive.ProductReduce;
                            }
                            else
                            {
                                tdn.ThanhPham = nangSuat.BTPThoatChuyenNgay - tcTang;
                                tdn.CommandTypeId = (int)eCommandRecive.ProductIncrease;
                            }
                            BLLDayInfo.KeypadInsert(tdn);
                            #endregion
                        }
                        result = true;
                        IndexChuyen = sttChuyenSanPham.ToString();
                        listIndexChuyen.Add(IndexChuyen);
                        countTimeSendRequestTCButHandleError = 0;

                        if (isfinish)
                            ResetDayInformation(lineId);

                        ///
                        LineId_Insert = lineId;
                        ClusterId_Insert = clusterId;
                        ActionName = eActionName.TC;
                        IsIncresea = true;

                        if (currentAssignments.Count > 0)
                        {
                            var obj = currentAssignments.FirstOrDefault(x => x.AcctionType == (int)eProductOutputType.TC && sttChuyenSanPham == x.AssignId && x.errorId == 0);
                            if (obj != null)
                                currentAssignments.Add(new CurrentAssignmentObj() { AssignId = sttChuyenSanPham, AcctionType = (int)eProductOutputType.TC, errorId = 0 });
                        }
                        else
                            currentAssignments.Add(new CurrentAssignmentObj() { AssignId = sttChuyenSanPham, AcctionType = (int)eProductOutputType.TC, errorId = 0 });
                    }
                    listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.HandlingSuccess + "," + productCode + ",," + (int)eProductOutputType.TC);
                    Helper.HelperControl.ResetKeypad(lineId, sttChuyenSanPham, 0, (int)eProductOutputType.TC, todayStr, this);
                    BLLHistoryPressedKeypad.Instance.Update(lineId, sttChuyenSanPham, todayStr);
                    BLLProductivity.ResetNormsDayAndBTPInLine(getBTPInLineByType, calculateNormsdayType, TypeOfCaculateDayNorms, lineId, false, todayStr);

                }
                else
                {
                    if (countTimeSendRequestTCButHandleError == timeSendRequestTCButHandleError)
                    {
                        listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.HandlingSuccess + "," + productCode + ",," + (int)eProductOutputType.TC);
                        countTimeSendRequestTCButHandleError = 0;
                    }
                    else
                        countTimeSendRequestTCButHandleError++;
                }
                return result;
            }
            catch (Exception ex)
            {
                //  GhiFileLog(DateTime.Now + "loi TangSanLuongTC ex :" + ex.Message + " \n");
            }
            return false;
        }

        public bool GiamSanLuongTC(int clusterId, int quantityIncrease, int sttChuyenSanPham, int maSanPham, int productCode, int lineId, bool isEndOfLine, int total, int equipmentId)
        {
            try
            {
                bool result = false, isCheckAndSendTotal = false;
                if (sttChuyenSanPham == 0)
                {
                    isCheckAndSendTotal = true;
                    if (listMapIdSanPhamNgay != null && listMapIdSanPhamNgay.Count > 0)
                    {
                        var mapIdSanPhamNgay = listMapIdSanPhamNgay.Where(c => c.MaChuyen == lineId && c.STT == productCode && c.Ngay == todayStr).FirstOrDefault();
                        if (mapIdSanPhamNgay != null)
                        {
                            sttChuyenSanPham = mapIdSanPhamNgay.STTChuyenSanPham;
                            maSanPham = mapIdSanPhamNgay.MaSanPham;
                        }
                    }
                }
                if (sttChuyenSanPham > 0)
                {
                    bool isfinish = false;
                    var nangSuatCum = BLLProductivity.Find_NangSuatCum(sttChuyenSanPham, clusterId, todayStr); // GetNangSuatCumNgayById(clusterId, sttChuyenSanPham, ngay);
                    int min = total, max = total;
                    if (nangSuatCum != null)
                    {
                        setTotalByMinOrMax = setTotalByMinOrMax_default;
                        if (lineId == LineId_Insert && clusterId != ClusterId_Insert && ActionName == eActionName.TC)
                            if (IsIncresea)
                                setTotalByMinOrMax = 2;
                            else
                                setTotalByMinOrMax = 1;

                        nangSuatCum.SanLuongTCGiam += quantityIncrease;
                        if (isCheckAndSendTotal)
                        {
                            int tongSanLuong = nangSuatCum.SanLuongTCTang - nangSuatCum.SanLuongTCGiam;
                            if (tongSanLuong != total)
                            {
                                if (tongSanLuong > total)
                                {
                                    min = total;
                                    max = tongSanLuong;
                                    switch (setTotalByMinOrMax)
                                    {
                                        case 1:
                                        case 3:
                                            nangSuatCum.SanLuongTCGiam += (max - min);
                                            break;
                                        case 2:
                                            listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.ChangeProductQuantity + "," + productCode + "," + max + "," + (int)eProductOutputType.TC);
                                            break;
                                    }
                                }
                                else
                                {
                                    min = tongSanLuong;
                                    max = total;
                                    switch (setTotalByMinOrMax)
                                    {
                                        case 1:
                                            listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.ChangeProductQuantity + "," + productCode + "," + min + "," + (int)eProductOutputType.TC);
                                            break;
                                        case 2:
                                        case 3:
                                            nangSuatCum.SanLuongTCGiam = nangSuatCum.SanLuongTCGiam - (max - min);
                                            break;
                                    }
                                }
                            }
                        }
                        BLLProductivity.Update_NS_Cum(nangSuatCum);

                        if (isEndOfLine)
                        {
                            #region
                            var nangSuat = BLLProductivity.TTNangXuatTrongNgay(todayStr, sttChuyenSanPham); // GetNangSuatChuyenNgay(sttChuyenSanPham, ngay);
                            var chuyenSanPham = BLLAssignmentForLine.Instance.GetAssignmentByDay(todayStr, sttChuyenSanPham, lineId);// chuyenSanPhamDAO.GetChuyenSanPham(DateTime.Now, sttChuyenSanPham, lineId);
                            var monthPro = BLLMonthlyProductionPlans.Find(sttChuyenSanPham, DateTime.Now.Month, DateTime.Now.Year);

                            var old = nangSuat.BTPThoatChuyenNgay - nangSuat.BTPThoatChuyenNgayGiam;
                            switch (setTotalByMinOrMax)
                            {
                                case 1:
                                    nangSuat.BTPThoatChuyenNgay = min;
                                    nangSuat.BTPThoatChuyenNgayGiam = 0;
                                    break;
                                case 2:
                                    nangSuat.BTPThoatChuyenNgay = max;
                                    nangSuat.BTPThoatChuyenNgayGiam = 0;
                                    break;
                                case 3:
                                    nangSuat.BTPThoatChuyenNgay = total;
                                    nangSuat.BTPThoatChuyenNgayGiam = 0;
                                    break;
                            }
                            chuyenSanPham.LuyKeBTPThoatChuyen = (chuyenSanPham.LuyKeBTPThoatChuyen - old) + nangSuat.BTPThoatChuyenNgay;
                            monthPro.LK_TC = (monthPro.LK_TC - old) + nangSuat.BTPThoatChuyenNgay;

                            nangSuat.IsBTP = 1;
                            var line = listChuyen.FirstOrDefault(x => x.MaChuyen == lineId);
                            //if (listBaoHetHang.Count > 0)
                            //{
                            //    if ((chuyenSanPham.SanLuongKeHoach - chuyenSanPham.LuyKeBTPThoatChuyen) <= listBaoHetHang[0].SoLuongCon)
                            //    {
                            //        SoundChuyen = line.Sound;
                            //        for (int n = 0; n < listBaoHetHang.Count; n++)
                            //        {
                            //            if ((chuyenSanPham.SanLuongKeHoach - chuyenSanPham.LuyKeBTPThoatChuyen) <= listBaoHetHang[n].SoLuongCon)
                            //            {
                            //                Repeat = listBaoHetHang[n].SoLanBao;
                            //                if (n == listBaoHetHang.Count - 1)
                            //                    chuyenSanPham.IsFinishBTPThoatChuyen = true;
                            //                if (chuyenSanPham.LuyKeBTPThoatChuyen >= chuyenSanPham.SanLuongKeHoach && chuyenSanPham.LuyKeTH >= chuyenSanPham.SanLuongKeHoach)
                            //                {
                            //                    chuyenSanPham.IsFinish = true;
                            //                    chuyenSanPham.STTThucHien = 900;
                            //                }
                            //            }
                            //            else
                            //                break;
                            //        }
                            //        if (chuyenSanPham.CountAssignment <= 1)
                            //        {
                            //            InformationPlay inf = new InformationPlay { SoundChuyen = SoundChuyen, Repeat = Repeat };
                            //            queuePlayFile.Enqueue(inf);
                            //        }
                            //    }
                            //    else
                            //        chuyenSanPham.IsFinishBTPThoatChuyen = false;
                            //}
                            //else
                            //{
                            //    if (chuyenSanPham.LuyKeBTPThoatChuyen >= chuyenSanPham.SanLuongKeHoach && chuyenSanPham.LuyKeTH >= chuyenSanPham.SanLuongKeHoach)
                            //    {
                            //        chuyenSanPham.IsFinish = true;
                            //        chuyenSanPham.IsFinishBTPThoatChuyen = true;
                            //        chuyenSanPham.STTThucHien = 900;
                            //        SoundChuyen = line.Sound;
                            //    }
                            //    else
                            //        chuyenSanPham.IsFinishBTPThoatChuyen = false;
                            //}
                            TimeSpan hieutime = new TimeSpan();
                            hieutime = TimeIsWork(lineId);
                            int second = (int)hieutime.TotalSeconds;
                            int btpThoatChuyenNgay = (nangSuat.BTPThoatChuyenNgay - nangSuat.BTPThoatChuyenNgayGiam);
                            if (btpThoatChuyenNgay > 0)
                            {
                                int nhipDoThucTeBTPThoatChuyen = second / btpThoatChuyenNgay;
                                nangSuat.NhipDoThucTeBTPThoatChuyen = nhipDoThucTeBTPThoatChuyen;
                            }
                            switch (getBTPInLineByType)
                            {
                                case 1:
                                    nangSuat.BTPTrenChuyen = (chuyenSanPham.LK_BTP - chuyenSanPham.LK_BTP_G) - chuyenSanPham.LuyKeTH;
                                    break;
                                case 2:
                                    nangSuat.BTPTrenChuyen = (chuyenSanPham.LK_BTP - chuyenSanPham.LK_BTP_G) - chuyenSanPham.LuyKeBTPThoatChuyen;
                                    break;
                            }
                            nangSuat.TimeLastChange = DateTime.Now.TimeOfDay;
                            BLLProductivity.UpdateNangXuat(nangSuat);
                            BLLMonthlyProductionPlans.Update(monthPro);
                            BLLAssignmentForLine.Instance.Update(sttChuyenSanPham, null, chuyenSanPham.LuyKeBTPThoatChuyen, chuyenSanPham.IsFinish, chuyenSanPham.IsFinishNow);
                            isfinish = chuyenSanPham.IsFinish;
                            #endregion

                            #region
                            var tdns = BLLDayInfo.GetByCommoId(sttChuyenSanPham, maSanPham, todayStr);
                            int tcTang = 0, tcGiam = 0;
                            tcTang = tdns.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductIncrease && c.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(c => c.ThanhPham);
                            tcGiam = tdns.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductReduce && c.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(c => c.ThanhPham);
                            tcTang = (tcTang - tcGiam) < 0 ? 0 : (tcTang - tcGiam);

                            var tdn = new PMS.Data.TheoDoiNgay();
                            tdn.MaChuyen = lineId;
                            tdn.MaSanPham = maSanPham;
                            tdn.CumId = clusterId;
                            tdn.STTChuyenSanPham = sttChuyenSanPham;
                            tdn.Time = DateTime.Now.TimeOfDay;
                            tdn.Date = todayStr;
                            tdn.ProductOutputTypeId = (int)eProductOutputType.TC;
                            tdn.IsEndOfLine = isEndOfLine;
                            tdn.IsEnterByKeypad = true;
                            tdn.EquipmentId = equipmentId;
                            if (tcTang > nangSuat.BTPThoatChuyenNgay)
                            {
                                tdn.ThanhPham = tcTang - nangSuat.BTPThoatChuyenNgay;
                                tdn.CommandTypeId = (int)eCommandRecive.ProductReduce;
                            }
                            else
                            {
                                tdn.ThanhPham = nangSuat.BTPThoatChuyenNgay - tcTang;
                                tdn.CommandTypeId = (int)eCommandRecive.ProductIncrease;
                            }
                            BLLDayInfo.KeypadInsert(tdn);
                            #endregion
                        }
                        result = true;
                        IndexChuyen = sttChuyenSanPham.ToString();
                        listIndexChuyen.Add(IndexChuyen);
                        countTimeSendRequestTCButHandleError = 0;

                        if (isfinish)
                            ResetDayInformation(lineId);
                        ///
                        LineId_Insert = lineId;
                        ClusterId_Insert = clusterId;
                        ActionName = eActionName.TC;
                        IsIncresea = false;

                        if (currentAssignments.Count > 0)
                        {
                            var obj = currentAssignments.FirstOrDefault(x => x.AcctionType == (int)eProductOutputType.TC && sttChuyenSanPham == x.AssignId && x.errorId == 0);
                            if (obj != null)
                                currentAssignments.Add(new CurrentAssignmentObj() { AssignId = sttChuyenSanPham, AcctionType = (int)eProductOutputType.TC, errorId = 0 });
                        }
                        else
                            currentAssignments.Add(new CurrentAssignmentObj() { AssignId = sttChuyenSanPham, AcctionType = (int)eProductOutputType.TC, errorId = 0 });

                    }
                    listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.HandlingSuccess + "," + productCode + ",," + (int)eProductOutputType.TC);
                    Helper.HelperControl.ResetKeypad(lineId, sttChuyenSanPham, 0, (int)eProductOutputType.TC, todayStr, this);
                    BLLHistoryPressedKeypad.Instance.Update(lineId, sttChuyenSanPham, todayStr);
                    BLLProductivity.ResetNormsDayAndBTPInLine(getBTPInLineByType, calculateNormsdayType, TypeOfCaculateDayNorms, lineId, false, todayStr);

                }
                else
                {
                    if (countTimeSendRequestTCButHandleError == timeSendRequestTCButHandleError)
                    {
                        listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.HandlingSuccess + "," + productCode + ",," + (int)eProductOutputType.TC);
                        countTimeSendRequestTCButHandleError = 0;
                    }
                    else
                        countTimeSendRequestTCButHandleError++;
                }


                return result;
            }
            catch (Exception ex)
            {
                //  GhiFileLog(DateTime.Now + "loi GiamSanLuongTC ex :" + ex.Message + " \n");
            }
            return false;
        }

        public bool TangSanLuongLoi(int clusterId, int quantityIncrease, int sttChuyenSanPham, int maSanPham, int productCode, int lineId, bool isEndOfLine, int errorId, int total, int equipmentId)
        {
            try
            {
                bool result = false, isCheckAndSendTotal = false;
                errorId = BLLError.Find(errorId, true).Id;
                if (sttChuyenSanPham == 0)
                {
                    isCheckAndSendTotal = true;
                    if (listMapIdSanPhamNgay != null && listMapIdSanPhamNgay.Count > 0)
                    {
                        var mapIdSanPhamNgay = listMapIdSanPhamNgay.Where(c => c.MaChuyen == lineId && c.STT == productCode && c.Ngay == todayStr).FirstOrDefault();
                        if (mapIdSanPhamNgay != null)
                        {
                            sttChuyenSanPham = mapIdSanPhamNgay.STTChuyenSanPham;
                            maSanPham = mapIdSanPhamNgay.MaSanPham;
                        }
                    }
                }
                if (sttChuyenSanPham > 0)
                {
                    var nangSuatCumLoi = BLLProductivity.Find_NangSuatCumLoi(clusterId, errorId, sttChuyenSanPham, todayStr); // GetNangSuatCumLoiNgayById(clusterId, errorId, sttChuyenSanPham, ngay);
                    if (nangSuatCumLoi != null)
                    {
                        //if (LineId_Insert != 0 && lineId != LineId_Insert)
                        //    Helper.HelperControl.ResetKeypad(lineId, sttChuyenSanPham, errorId, (int)eProductOutputType.Error, todayStr, this);

                        setTotalByMinOrMax = setTotalByMinOrMax_default;
                        if (lineId == LineId_Insert && clusterId != ClusterId_Insert && ActionName == eActionName.Error)
                            if (IsIncresea)
                                setTotalByMinOrMax = 2;
                            else
                                setTotalByMinOrMax = 1;

                        nangSuatCumLoi.SoLuongTang += quantityIncrease;
                        int min = 0, max = 0;
                        if (isCheckAndSendTotal)
                        {
                            #region
                            int tongSanLuong = nangSuatCumLoi.SoLuongTang - nangSuatCumLoi.SoLuongGiam;
                            if (tongSanLuong != total)
                            {
                                if (tongSanLuong > total)
                                {
                                    min = total;
                                    max = tongSanLuong;
                                    switch (setTotalByMinOrMax)
                                    {
                                        case 1:
                                        case 3:
                                            nangSuatCumLoi.SoLuongTang = nangSuatCumLoi.SoLuongTang - (max - min);
                                            break;
                                        case 2:
                                            listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.ChangeProductError + "," + errorId + "," + max + "," + productCode);
                                            break;
                                    }
                                }
                                else
                                {
                                    min = tongSanLuong;
                                    max = total;
                                    switch (setTotalByMinOrMax)
                                    {
                                        case 1:
                                            listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.ChangeProductError + "," + errorId + "," + min + "," + productCode);
                                            break;
                                        case 2:
                                        case 3:
                                            nangSuatCumLoi.SoLuongTang = nangSuatCumLoi.SoLuongTang + (max - min);
                                            break;
                                    }
                                }
                            }
                            #endregion
                        }
                        BLLProductivity.Update_NS_CumLoi(nangSuatCumLoi);

                        if (isEndOfLine)
                        {
                            #region
                            var nangSuat = BLLProductivity.TTNangXuatTrongNgay(todayStr, sttChuyenSanPham); // GetNangSuatChuyenNgay(sttChuyenSanPham, ngay);
                            if (nangSuat != null)
                            {
                                var old = 0;
                                switch (setTotalByMinOrMax)
                                {
                                    case 1: old = min; break;
                                    case 2: old = max; break;
                                    case 3: old = total; break;
                                }
                                nangSuat.SanLuongLoi += quantityIncrease;
                                BLLProductivity.UpdateNangXuat(nangSuat);

                                #region
                                var tdns = BLLDayInfo.GetByCommoId(sttChuyenSanPham, maSanPham, todayStr);
                                int errTang = 0, errGiam = 0;
                                errTang = tdns.Where(c => c.CommandTypeId == (int)eCommandRecive.ErrorIncrease && c.ErrorId == errorId).Sum(c => c.ThanhPham);
                                errGiam = tdns.Where(c => c.CommandTypeId == (int)eCommandRecive.ErrorReduce && c.ErrorId == errorId).Sum(c => c.ThanhPham);
                                errTang = (errTang - errGiam) < 0 ? 0 : (errTang - errGiam);

                                var tdn = new PMS.Data.TheoDoiNgay();
                                tdn.MaChuyen = lineId;
                                tdn.MaSanPham = maSanPham;
                                tdn.CumId = clusterId;
                                tdn.STTChuyenSanPham = sttChuyenSanPham;
                                tdn.Time = DateTime.Now.TimeOfDay;
                                tdn.Date = todayStr;
                                tdn.IsEndOfLine = isEndOfLine;
                                tdn.IsEnterByKeypad = true;
                                tdn.ErrorId = errorId;
                                tdn.EquipmentId = equipmentId;
                                if (errTang > old)
                                {
                                    tdn.ThanhPham = errTang - old;
                                    tdn.CommandTypeId = (int)eCommandRecive.ErrorReduce;
                                }
                                else
                                {
                                    tdn.ThanhPham = old - errTang;
                                    tdn.CommandTypeId = (int)eCommandRecive.ErrorIncrease;
                                }
                                BLLDayInfo.KeypadInsert(tdn);
                                #endregion
                            }
                            #endregion
                        }
                        result = true;
                        countTimeSendRequestErrorButHandleError = 0;

                        ///
                        LineId_Insert = lineId;
                        ClusterId_Insert = clusterId;
                        ActionName = eActionName.Error;
                        IsIncresea = true;

                        if (currentAssignments.Count > 0)
                        {
                            var obj = currentAssignments.FirstOrDefault(x => x.AcctionType == (int)eProductOutputType.Error && sttChuyenSanPham == x.AssignId && x.errorId == errorId);
                            if (obj != null)
                                currentAssignments.Add(new CurrentAssignmentObj() { AssignId = sttChuyenSanPham, AcctionType = (int)eProductOutputType.Error, errorId = errorId });
                        }
                        else
                            currentAssignments.Add(new CurrentAssignmentObj() { AssignId = sttChuyenSanPham, AcctionType = (int)eProductOutputType.Error, errorId = errorId });


                        listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.HandlingSuccess + "," + errorId + ",," + productCode);
                    }
                }
                else
                {
                    if (countTimeSendRequestErrorButHandleError == timeSendRequestErrorButHandleError)
                    {
                        listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.HandlingSuccess + "," + errorId + ",," + productCode);
                        countTimeSendRequestErrorButHandleError = 0;
                    }
                    else
                        countTimeSendRequestErrorButHandleError++;
                }
                return result;
            }
            catch (Exception ex)
            {
                //  GhiFileLog(DateTime.Now + "loi TangSanLuongloi ex :" + ex.Message + " \n");
            }
            return false;
        }

        public bool GiamSanLuongLoi(int clusterId, int quantityIncrease, int sttChuyenSanPham, int maSanPham, int productCode, int lineId, bool isEndOfLine, int errorId, int total, int equipmentId)
        {
            try
            {
                bool result = false, isCheckAndSendTotal = false;
                errorId = BLLError.Find(errorId, true).Id;
                if (sttChuyenSanPham == 0)
                {
                    isCheckAndSendTotal = true;
                    if (listMapIdSanPhamNgay != null && listMapIdSanPhamNgay.Count > 0)
                    {
                        var mapIdSanPhamNgay = listMapIdSanPhamNgay.Where(c => c.MaChuyen == lineId && c.STT == productCode && c.Ngay == todayStr).FirstOrDefault();
                        if (mapIdSanPhamNgay != null)
                        {
                            sttChuyenSanPham = mapIdSanPhamNgay.STTChuyenSanPham;
                            maSanPham = mapIdSanPhamNgay.MaSanPham;
                        }
                    }
                }
                if (sttChuyenSanPham > 0)
                {
                    var nangSuatCumLoi = BLLProductivity.Find_NangSuatCumLoi(clusterId, errorId, sttChuyenSanPham, todayStr); // GetNangSuatCumLoiNgayById(clusterId, errorId, sttChuyenSanPham, ngay);
                    if (nangSuatCumLoi != null)
                    {
                        //if (LineId_Insert != 0 && lineId != LineId_Insert)
                        //    Helper.HelperControl.ResetKeypad(lineId, sttChuyenSanPham, errorId, (int)eProductOutputType.Error, todayStr, this);

                        setTotalByMinOrMax = setTotalByMinOrMax_default;
                        if (lineId == LineId_Insert && clusterId != ClusterId_Insert && ActionName == eActionName.Error)
                            if (IsIncresea)
                                setTotalByMinOrMax = 2;
                            else
                                setTotalByMinOrMax = 1;

                        nangSuatCumLoi.SoLuongGiam += quantityIncrease;
                        int min = 0, max = 0;
                        if (isCheckAndSendTotal)
                        {
                            int tongSanLuong = nangSuatCumLoi.SoLuongTang - nangSuatCumLoi.SoLuongGiam;
                            if (tongSanLuong != total)
                            {
                                if (tongSanLuong > total)
                                {
                                    min = total;
                                    max = tongSanLuong;
                                    switch (setTotalByMinOrMax)
                                    {
                                        case 1:
                                        case 3:
                                            nangSuatCumLoi.SoLuongGiam = nangSuatCumLoi.SoLuongGiam + (max - min);
                                            break;
                                        case 2:
                                            listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.ChangeProductError + "," + errorId + "," + max + "," + productCode);
                                            break;
                                    }
                                }
                                else
                                {
                                    min = tongSanLuong;
                                    max = total;
                                    switch (setTotalByMinOrMax)
                                    {
                                        case 1:
                                            listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.ChangeProductError + "," + errorId + "," + min + "," + productCode);
                                            break;
                                        case 2:
                                        case 3:
                                            nangSuatCumLoi.SoLuongGiam = nangSuatCumLoi.SoLuongGiam - (max - min);
                                            break;
                                    }
                                }
                            }
                        }
                        BLLProductivity.Update_NS_CumLoi(nangSuatCumLoi);

                        if (isEndOfLine)
                        {
                            var nangSuat = BLLProductivity.TTNangXuatTrongNgay(todayStr, sttChuyenSanPham);
                            if (nangSuat != null)
                            {
                                var old = nangSuat.SanLuongLoi - nangSuat.SanLuongLoiGiam;
                                switch (setTotalByMinOrMax)
                                {
                                    case 1:
                                        nangSuat.SanLuongLoi = min;
                                        nangSuat.SanLuongLoiGiam = 0;
                                        break;
                                    case 2:
                                        nangSuat.SanLuongLoi = max;
                                        nangSuat.SanLuongLoiGiam = 0;
                                        break;
                                    case 3:
                                        nangSuat.SanLuongLoi = total;
                                        nangSuat.SanLuongLoiGiam = 0;
                                        break;
                                }
                                BLLProductivity.UpdateNangXuat(nangSuat);

                                #region
                                var tdns = BLLDayInfo.GetByCommoId(sttChuyenSanPham, maSanPham, todayStr);
                                int errTang = 0, errGiam = 0;
                                errTang = tdns.Where(c => c.CommandTypeId == (int)eCommandRecive.ErrorIncrease && c.ErrorId == errorId).Sum(c => c.ThanhPham);
                                errGiam = tdns.Where(c => c.CommandTypeId == (int)eCommandRecive.ErrorReduce && c.ErrorId == errorId).Sum(c => c.ThanhPham);
                                errTang = (errTang - errGiam) < 0 ? 0 : (errTang - errGiam);
                                var loi = nangSuat.SanLuongLoi - nangSuat.SanLuongLoiGiam;

                                var tdn = new PMS.Data.TheoDoiNgay();
                                tdn.MaChuyen = lineId;
                                tdn.MaSanPham = maSanPham;
                                tdn.CumId = clusterId;
                                tdn.STTChuyenSanPham = sttChuyenSanPham;
                                tdn.Time = DateTime.Now.TimeOfDay;
                                tdn.Date = todayStr;
                                tdn.IsEndOfLine = isEndOfLine;
                                tdn.IsEnterByKeypad = true;
                                tdn.ErrorId = errorId;
                                tdn.EquipmentId = equipmentId;
                                if (errTang > loi)
                                {
                                    tdn.ThanhPham = errTang - loi;
                                    tdn.CommandTypeId = (int)eCommandRecive.ErrorReduce;
                                }
                                else
                                {
                                    tdn.ThanhPham = loi - errTang;
                                    tdn.CommandTypeId = (int)eCommandRecive.ErrorIncrease;
                                }
                                BLLDayInfo.KeypadInsert(tdn);
                                #endregion
                            }
                        }
                        result = true;
                        countTimeSendRequestErrorButHandleError = 0;

                        ///
                        LineId_Insert = lineId;
                        ClusterId_Insert = clusterId;
                        ActionName = eActionName.Error;
                        IsIncresea = false;
                        listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.HandlingSuccess + "," + errorId + ",," + productCode);

                        if (currentAssignments.Count > 0)
                        {
                            var obj = currentAssignments.FirstOrDefault(x => x.AcctionType == (int)eProductOutputType.Error && sttChuyenSanPham == x.AssignId && x.errorId == errorId);
                            if (obj != null)
                                currentAssignments.Add(new CurrentAssignmentObj() { AssignId = sttChuyenSanPham, AcctionType = (int)eProductOutputType.Error, errorId = errorId });
                        }
                        else
                            currentAssignments.Add(new CurrentAssignmentObj() { AssignId = sttChuyenSanPham, AcctionType = (int)eProductOutputType.Error, errorId = errorId });

                        //   Helper.HelperControl.ResetKeypad(lineId, sttChuyenSanPham, errorId, (int)eProductOutputType.Error, todayStr, this);
                    }
                }
                else
                {
                    if (countTimeSendRequestErrorButHandleError == timeSendRequestErrorButHandleError)
                    {
                        listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.HandlingSuccess + "," + errorId + ",," + productCode);
                        countTimeSendRequestErrorButHandleError = 0;
                    }
                    else
                        countTimeSendRequestErrorButHandleError++;
                }
                return result;
            }
            catch (Exception ex)
            {
                // GhiFileLog(DateTime.Now + "loi GiamSanLuongLoi ex :" + ex.Message + " \n");
            }
            return false;
        }

        public bool TangBTP(int clusterId, int quantityIncrease, int sttChuyenSanPham, int maSanPham, int productCode, int lineId, bool isEndOfLine, int errorId, int total, int equipmentId)
        {
            try
            {
                bool result = false, isCheckAndSendTotal = false;
                if (sttChuyenSanPham == 0)
                {
                    isCheckAndSendTotal = true;
                    if (listMapIdSanPhamNgay != null && listMapIdSanPhamNgay.Count > 0)
                    {
                        var mapIdSanPhamNgay = listMapIdSanPhamNgay.Where(c => c.MaChuyen == lineId && c.STT == productCode && c.Ngay == todayStr).FirstOrDefault();
                        if (mapIdSanPhamNgay != null)
                        {
                            sttChuyenSanPham = mapIdSanPhamNgay.STTChuyenSanPham;
                            maSanPham = mapIdSanPhamNgay.MaSanPham;
                        }
                    }
                }
                if (sttChuyenSanPham > 0)
                {
                    var nangSuatCum = BLLProductivity.Find_NangSuatCum(sttChuyenSanPham, clusterId, todayStr);// GetNangSuatCumNgayById(clusterId, sttChuyenSanPham, ngay);
                    var chuyenSanPham = BLLAssignmentForLine.Instance.GetAssignmentByDay(todayStr, sttChuyenSanPham, lineId); // chuyenSanPhamDAO.GetChuyenSanPham(DateTime.Now, sttChuyenSanPham, lineId);
                    if (nangSuatCum != null && chuyenSanPham != null && (chuyenSanPham.LK_BTP - chuyenSanPham.LK_BTP_G) < chuyenSanPham.SanLuongKeHoach)
                    {

                        setTotalByMinOrMax = setTotalByMinOrMax_default;
                        if (lineId == LineId_Insert && clusterId != ClusterId_Insert && ActionName == eActionName.BTP)
                            if (IsIncresea)
                                setTotalByMinOrMax = 2;
                            else
                                setTotalByMinOrMax = 1;

                        quantityIncrease = ((chuyenSanPham.LK_BTP + quantityIncrease) > chuyenSanPham.SanLuongKeHoach ? chuyenSanPham.SanLuongKeHoach - chuyenSanPham.LK_BTP : quantityIncrease);
                        nangSuatCum.BTPTang += quantityIncrease;
                        if (isCheckAndSendTotal)
                        {
                            int tongSanLuong = nangSuatCum.BTPTang - nangSuatCum.BTPGiam;
                            int min = 0, max = 0;
                            if (tongSanLuong != total)
                            {

                                if (tongSanLuong > total)
                                {
                                    min = total;
                                    max = tongSanLuong;
                                    switch (setTotalByMinOrMax)
                                    {
                                        case 1:
                                        case 3:
                                            nangSuatCum.BTPGiam = nangSuatCum.BTPGiam + (max - min);
                                            break;
                                        case 2:
                                            listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.ChangeBTPQuantities + "," + productCode + "," + max + ",,");
                                            break;
                                    }
                                }
                                else
                                {
                                    min = tongSanLuong;
                                    max = total;
                                    switch (setTotalByMinOrMax)
                                    {
                                        case 1:
                                            listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.ChangeBTPQuantities + "," + productCode + "," + min + ",,");
                                            break;
                                        case 2:
                                        case 3:
                                            nangSuatCum.BTPGiam = nangSuatCum.BTPGiam - (max - min);
                                            break;
                                    }
                                }
                            }
                            BLLProductivity.Update_NS_Cum(nangSuatCum);

                            if (isEndOfLine)
                            {
                                var nangSuat = BLLProductivity.TTNangXuatTrongNgay(todayStr, sttChuyenSanPham); // GetNangSuatChuyenNgay(sttChuyenSanPham, ngay);
                                var monthPro = BLLMonthlyProductionPlans.Find(sttChuyenSanPham, DateTime.Now.Month, DateTime.Now.Year);
                                var old = nangSuat.BTPTang - nangSuat.BTPGiam;
                                switch (setTotalByMinOrMax)
                                {
                                    case 1:
                                        nangSuat.BTPTang = min;
                                        nangSuat.BTPGiam = 0;
                                        break;
                                    case 2:
                                        nangSuat.BTPTang = max;
                                        nangSuat.BTPGiam = 0;
                                        break;
                                    case 3:
                                        nangSuat.BTPTang = total;
                                        nangSuat.BTPGiam = 0;
                                        break;
                                }
                                nangSuat.IsBTP = 1;
                                chuyenSanPham.LK_BTP = (chuyenSanPham.LK_BTP - chuyenSanPham.LK_BTP_G - old) + nangSuat.BTPTang;
                                monthPro.LK_BTP = (monthPro.LK_BTP - old) + nangSuat.BTPTang;
                                switch (getBTPInLineByType)
                                {
                                    case 1:
                                        //  nangSuat.BTPTrenChuyen = ((chuyenSanPham.LK_BTP - chuyenSanPham.LK_BTP_G) + quantityIncrease) - chuyenSanPham.LuyKeTH;
                                        nangSuat.BTPTrenChuyen = chuyenSanPham.LK_BTP - chuyenSanPham.LuyKeTH;
                                        break;
                                    case 2:
                                        //   nangSuat.BTPTrenChuyen = ((chuyenSanPham.LK_BTP - chuyenSanPham.LK_BTP_G) + quantityIncrease) - chuyenSanPham.LuyKeBTPThoatChuyen;
                                        nangSuat.BTPTrenChuyen = chuyenSanPham.LK_BTP - chuyenSanPham.LuyKeBTPThoatChuyen;
                                        break;
                                }

                                var btps = BLLBTP.GetByCommoId(sttChuyenSanPham, todayStr);
                                int Tang = 0, Giam = 0;
                                Tang = btps.Where(c => c.CommandTypeId == (int)eCommandRecive.BTPIncrease).Sum(c => c.BTPNgay);
                                Giam = btps.Where(c => c.CommandTypeId == (int)eCommandRecive.BTPReduce).Sum(c => c.BTPNgay);
                                Tang = (Tang - Giam) < 0 ? 0 : (Tang - Giam);
                                var btp = new PMS.Data.BTP();
                                btp.Ngay = todayStr;
                                btp.STTChuyen_SanPham = sttChuyenSanPham;
                                btp.STT = 1;
                                btp.CumId = clusterId;
                                btp.TimeUpdate = DateTime.Now.TimeOfDay;
                                btp.IsEndOfLine = isEndOfLine;
                                btp.IsEnterByKeypad = true;
                                btp.EquipmentId = equipmentId;
                                if (Tang > nangSuat.BTPTang)
                                {
                                    btp.BTPNgay = Tang - nangSuat.BTPTang;
                                    btp.CommandTypeId = (int)eCommandRecive.BTPReduce;
                                }
                                else
                                {
                                    btp.BTPNgay = nangSuat.BTPTang - Tang;
                                    btp.CommandTypeId = (int)eCommandRecive.BTPIncrease;
                                }
                                BLLBTP.InsertOrUpdate(btp);
                                BLLProductivity.UpdateNangXuat(nangSuat);
                                BLLMonthlyProductionPlans.Update(monthPro);
                            }
                            result = true;
                            IndexChuyen = sttChuyenSanPham.ToString();
                            listIndexChuyen.Add(IndexChuyen);

                            ///
                            LineId_Insert = lineId;
                            ClusterId_Insert = clusterId;
                            ActionName = eActionName.BTP;
                            IsIncresea = true;
                            listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.HandlingSuccess + "," + productCode + "," + max + ",,");
                            Helper.HelperControl.ResetKeypad(lineId, sttChuyenSanPham, 0, (int)eProductOutputType.BTP, todayStr, this);
                            BLLHistoryPressedKeypad.Instance.Update(lineId, sttChuyenSanPham, todayStr);
                            if (currentAssignments.Count > 0)
                            {
                                var obj = currentAssignments.FirstOrDefault(x => x.AcctionType == (int)eProductOutputType.BTP && sttChuyenSanPham == x.AssignId && x.errorId == 0);
                                if (obj != null)
                                    currentAssignments.Add(new CurrentAssignmentObj() { AssignId = sttChuyenSanPham, AcctionType = (int)eProductOutputType.BTP, errorId = 0 });
                            }
                            else
                                currentAssignments.Add(new CurrentAssignmentObj() { AssignId = sttChuyenSanPham, AcctionType = (int)eProductOutputType.BTP, errorId = 0 });

                        }
                        BLLProductivity.ResetNormsDayAndBTPInLine(getBTPInLineByType, calculateNormsdayType, TypeOfCaculateDayNorms, lineId, false, todayStr);

                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                //  GhiFileLog(DateTime.Now + "loi TangSanLuongBTP ex :" + ex.Message + " \n");
            }
            return false;
        }

        public bool GiamBTP(int clusterId, int quantityIncrease, int sttChuyenSanPham, int maSanPham, int productCode, int lineId, bool isEndOfLine, int errorId, int total, int equipmentId)
        {
            try
            {
                bool result = false, isCheckAndSendTotal = false;
                if (sttChuyenSanPham == 0)
                {
                    isCheckAndSendTotal = true;
                    if (listMapIdSanPhamNgay != null && listMapIdSanPhamNgay.Count > 0)
                    {
                        var mapIdSanPhamNgay = listMapIdSanPhamNgay.Where(c => c.MaChuyen == lineId && c.STT == productCode && c.Ngay == todayStr).FirstOrDefault();
                        if (mapIdSanPhamNgay != null)
                        {
                            sttChuyenSanPham = mapIdSanPhamNgay.STTChuyenSanPham;
                            maSanPham = mapIdSanPhamNgay.MaSanPham;
                        }
                    }
                }
                if (sttChuyenSanPham > 0)
                {
                    var nangSuatCum = BLLProductivity.Find_NangSuatCum(sttChuyenSanPham, clusterId, todayStr); // GetNangSuatCumNgayById(clusterId, sttChuyenSanPham, ngay);
                    if (nangSuatCum != null)
                    {

                        setTotalByMinOrMax = setTotalByMinOrMax_default;
                        if (lineId == LineId_Insert && clusterId != ClusterId_Insert && ActionName == eActionName.BTP)
                            if (IsIncresea)
                                setTotalByMinOrMax = 2;
                            else
                                setTotalByMinOrMax = 1;

                        nangSuatCum.BTPGiam += quantityIncrease;
                        if (isCheckAndSendTotal)
                        {
                            #region  HoangHai
                            int tongSanLuong = nangSuatCum.BTPTang - nangSuatCum.BTPGiam;
                            int min = 0, max = 0;
                            if (tongSanLuong != total)
                            {

                                if (tongSanLuong > total)
                                {
                                    min = total;
                                    max = tongSanLuong;
                                    switch (setTotalByMinOrMax)
                                    {
                                        case 1:
                                        case 3:
                                            nangSuatCum.BTPGiam = nangSuatCum.BTPGiam + (max - min);
                                            break;
                                        case 2:
                                            listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.ChangeBTPQuantities + "," + productCode + "," + max + ",,");
                                            break;
                                    }
                                }
                                else
                                {
                                    min = tongSanLuong;
                                    max = total;
                                    switch (setTotalByMinOrMax)
                                    {
                                        case 1:
                                            listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.ChangeBTPQuantities + "," + productCode + "," + min + ",,");
                                            break;
                                        case 2:
                                        case 3:
                                            nangSuatCum.BTPGiam = nangSuatCum.BTPGiam - (max - min);
                                            break;
                                    }
                                }
                            }
                            #endregion

                            BLLProductivity.Update_NS_Cum(nangSuatCum);

                            if (isEndOfLine)
                            {
                                var nangSuat = BLLProductivity.TTNangXuatTrongNgay(todayStr, sttChuyenSanPham); // GetNangSuatChuyenNgay(sttChuyenSanPham, ngay);
                                var chuyenSanPham = BLLAssignmentForLine.Instance.GetAssignmentByDay(todayStr, sttChuyenSanPham, lineId);   //chuyenSanPhamDAO.GetChuyenSanPham(DateTime.Now, sttChuyenSanPham, lineId);
                                var monthPro = BLLMonthlyProductionPlans.Find(sttChuyenSanPham, DateTime.Now.Month, DateTime.Now.Year);
                                var old = nangSuat.BTPTang - nangSuat.BTPGiam;

                                switch (setTotalByMinOrMax)
                                {
                                    case 1:
                                        nangSuat.BTPTang = min;
                                        nangSuat.BTPGiam = 0;
                                        break;
                                    case 2:
                                        nangSuat.BTPTang = max;
                                        nangSuat.BTPGiam = 0;
                                        break;
                                    case 3:
                                        nangSuat.BTPTang = total;
                                        nangSuat.BTPGiam = 0;
                                        break;
                                }
                                nangSuat.IsBTP = 1;
                                monthPro.LK_BTP = (monthPro.LK_BTP - old) + nangSuat.BTPTang;

                                switch (getBTPInLineByType)
                                {
                                    case 1:
                                        nangSuat.BTPTrenChuyen = ((chuyenSanPham.LK_BTP - chuyenSanPham.LK_BTP_G) - quantityIncrease) - chuyenSanPham.LuyKeTH;
                                        break;
                                    case 2:
                                        nangSuat.BTPTrenChuyen = ((chuyenSanPham.LK_BTP - chuyenSanPham.LK_BTP_G) - quantityIncrease) - chuyenSanPham.LuyKeBTPThoatChuyen;
                                        break;
                                }

                                var btps = BLLBTP.GetByCommoId(sttChuyenSanPham, todayStr);
                                int Tang = 0, Giam = 0;
                                Tang = btps.Where(c => c.CommandTypeId == (int)eCommandRecive.BTPIncrease).Sum(c => c.BTPNgay);
                                Giam = btps.Where(c => c.CommandTypeId == (int)eCommandRecive.BTPReduce).Sum(c => c.BTPNgay);
                                Tang = (Tang - Giam) < 0 ? 0 : (Tang - Giam);

                                var btp = new PMS.Data.BTP();
                                btp.Ngay = todayStr;
                                btp.STTChuyen_SanPham = sttChuyenSanPham;
                                btp.STT = 1;
                                btp.CumId = clusterId;
                                btp.TimeUpdate = DateTime.Now.TimeOfDay;
                                btp.IsEndOfLine = isEndOfLine;
                                btp.IsEnterByKeypad = true;
                                btp.EquipmentId = equipmentId;
                                if (Tang > nangSuat.BTPTang)
                                {
                                    btp.BTPNgay = Tang - nangSuat.BTPTang;
                                    btp.CommandTypeId = (int)eCommandRecive.BTPReduce;
                                }
                                else
                                {
                                    btp.BTPNgay = nangSuat.BTPTang - Tang;
                                    btp.CommandTypeId = (int)eCommandRecive.BTPIncrease;
                                }
                                BLLBTP.InsertOrUpdate(btp);
                                BLLProductivity.UpdateNangXuat(nangSuat);
                                BLLMonthlyProductionPlans.Update(monthPro);
                            }
                            result = true;
                            IndexChuyen = sttChuyenSanPham.ToString();
                            listIndexChuyen.Add(IndexChuyen);

                            ///
                            LineId_Insert = lineId;
                            ClusterId_Insert = clusterId;
                            ActionName = eActionName.BTP;
                            IsIncresea = false;
                            listDataSendKeyPad.Add(equipmentId + "," + (int)eCommandSend.HandlingSuccess + "," + productCode + "," + max + ",,");
                            Helper.HelperControl.ResetKeypad(lineId, sttChuyenSanPham, 0, (int)eProductOutputType.BTP, todayStr, this);
                            BLLHistoryPressedKeypad.Instance.Update(lineId, sttChuyenSanPham, todayStr);

                            if (currentAssignments.Count > 0)
                            {
                                var obj = currentAssignments.FirstOrDefault(x => x.AcctionType == (int)eProductOutputType.BTP && sttChuyenSanPham == x.AssignId && x.errorId == 0);
                                if (obj != null)
                                    currentAssignments.Add(new CurrentAssignmentObj() { AssignId = sttChuyenSanPham, AcctionType = (int)eProductOutputType.BTP, errorId = 0 });
                            }
                            else
                                currentAssignments.Add(new CurrentAssignmentObj() { AssignId = sttChuyenSanPham, AcctionType = (int)eProductOutputType.BTP, errorId = 0 });

                        }
                        BLLProductivity.ResetNormsDayAndBTPInLine(getBTPInLineByType, calculateNormsdayType, TypeOfCaculateDayNorms, lineId, false, todayStr);

                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                // GhiFileLog(DateTime.Now + "loi GiamSanLuongBTP ex :" + ex.Message + " \n");
            }
            return false;
        }
        #endregion

        #region -----------            EVENT           -----------------
        private void butAppConfig_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmConfig f = new FrmConfig();
            f.Show();
        }

        private void butTatMoTienTrinhTuDong_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!IsStopProcess)
            {
                tmLoadData.Enabled = false;
                tmSenData.Enabled = false;
                timerTimeoutNDTT.Enabled = false;
                threadplay.Abort();
                butRun.Enabled = true;
                butRun.Caption = "Chạy các tiến trình";
                butTatMoTienTrinhTuDong.Caption = "Mở các tiến trình tự động";
                IsStopProcess = true;
                if (frmSendMailAndReadSound != null)
                    frmSendMailAndReadSound.Close();
            }
            else
                RunAllProcess(false);
        }

        private void butDeleteTheoDoiSanLuongTuPCung_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // Xoa bang theo doi 
                BLLDayInfo.DeleteAllInformation();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi butDeleteTheoDoiSanLuongTuPCung_ItemClick: " + ex.Message);
            }
        }

        private void butShowApp_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void butHideApp_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void butExitApp_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void butRun_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                RunAllProcess(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi butRun_ItemClick: " + ex.Message);
            }
        }

        private void butShowLCDTongHop_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmHienThiLCDTongHop form = new FrmHienThiLCDTongHop(sqlCon);
            form.Show();
        }

        public int ExecuteSqlTransaction(List<string> listStrSQL)
        {
            int result = 0;
            sqlCom = sqlCon.CreateCommand();
            SqlTransaction transaction;
            transaction = sqlCon.BeginTransaction("Transaction");
            sqlCom.Connection = sqlCon;
            sqlCom.Transaction = transaction;
            try
            {
                if (listStrSQL != null && listStrSQL.Count > 0)
                {
                    foreach (string strSQL in listStrSQL)
                    {
                        sqlCom.CommandText = strSQL;
                        sqlCom.ExecuteNonQuery();
                    }
                }
                transaction.Commit();
                result = 1;
            }
            catch (Exception ex)
            {
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
            }
            return result;
        }

        private void butXemSoLuongLoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var result = ActiveForm(typeof(FormSanLuongLoiChuyen));
            if (!result)
            {
                FormSanLuongLoiChuyen f = new FormSanLuongLoiChuyen();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void butQuanLyChuyen_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (AccountSuccess.IsOwner)
            {
                var result = ActiveForm(typeof(FrmLine));
                if (!result)
                {
                    FrmLine f = new FrmLine();
                    f.MdiParent = this;
                    f.Show();
                }
            }
            else
                MessageBox.Show("Tài khoản :'" + AccountSuccess.TenChuTK + "' không có quyền thực hiện thao tác này.");
        }

        private void butQuanLyCum_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var result = ActiveForm(typeof(FrmCluster));
            if (!result)
            {
                FrmCluster f = new FrmCluster();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void butKhoiTaoKeypad_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            KeypadInit();
        }

        private void butTurnCOMManagement_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void butCOMStatus_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmCOMStatus frm = new FrmCOMStatus();
            frm.Show();
        }

        private void butShowLCDKanBan_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                FrmHienThiLCDKANBAN frm = new FrmHienThiLCDKANBAN();
                frm.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                frm.Show();
                if (MessageBox.Show("Bạn có muốn ẩn màn mình chính của chương trình?", "Ẩn màn hình chính", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    this.Hide();
                else
                    this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi butShowLCDKanBan_ItemClick:" + ex.Message);
            }
        }

        private void butViewProcessLog_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void butQuanLyLoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void butLCDNSCum_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmLCDNangSuatCum f = new FrmLCDNangSuatCum(sqlCon);
            //f.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            f.Show();
            AccountSuccess.ListFormLCD.Add(f);
        }

        private void butLCDNSLoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmLCDError f = new FrmLCDError(sqlCon);
            //f.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            f.Show();
            AccountSuccess.ListFormLCD.Add(f);
        }

        private void butLatForm_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (butLatForm.GroupIndex == 0)
            {
                butLatForm.GroupIndex = 1;
                timerLatForm.Enabled = true;
                butLatForm.Caption = "Tắt Tính Năng Lật Trang";
            }
            else
            {
                butLatForm.GroupIndex = 0;
                timerLatForm.Enabled = false;
                butLatForm.Caption = "Bật Tính Năng Lật Trang";
            }

        }

        private void butChartNSClusterInHours_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var result = ActiveForm(typeof(FrmReportNSCum));
                if (!result)
                {
                    FrmReportNSCum f = new FrmReportNSCum();
                    f.MdiParent = this;
                    f.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi butChartNSClusterInHours_ItemClick: " + ex.Message);
            }
        }

        private void butChartNSClustersOfLineByHours_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var result = ActiveForm(typeof(FrmReportNSClusterOfLine));
                if (!result)
                {
                    FrmReportNSClusterOfLine f = new FrmReportNSClusterOfLine();
                    f.MdiParent = this;
                    f.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi butChartNSClustersOfLineByHours_ItemClick: " + ex.Message);
            }
        }

        private void butReadSoundError_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var result = ActiveForm(typeof(FrmCauHinhDocNSLoi));
                if (!result)
                {
                    FrmCauHinhDocNSLoi f = new FrmCauHinhDocNSLoi();
                    f.MdiParent = this;
                    f.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi butReadSoundError_ItemClick: " + ex.Message);
            }
        }

        private void butReportNSLineHours_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var result = ActiveForm(typeof(FrmReportNSLineHours));
                if (!result)
                {
                    FrmReportNSLineHours f = new FrmReportNSLineHours();
                    f.MdiParent = this;
                    f.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi butReportNSLineHours_ItemClick: " + ex.Message);
            }
        }

        private void butReportNSLinesByHours_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var result = ActiveForm(typeof(FrmReportNSLinesByHours));
                if (!result)
                {
                    FrmReportNSLinesByHours f = new FrmReportNSLinesByHours();
                    f.MdiParent = this;
                    f.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi butReportNSLinesByHours_ItemClick: " + ex.Message);
            }
        }

        private void butReportNSLinePerHours_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                var result = ActiveForm(typeof(FrmReportNSLinePerHour));
                if (!result)
                {
                    var f = new FrmReportNSLinePerHour();
                    f.MdiParent = this;
                    f.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi butReportNSLinePerHours_ItemClick: " + ex.Message);
            }
        }

        private void butReportCountErrorByHours_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var result = ActiveForm(typeof(FrmReportCountErrorHours));
                if (!result)
                {
                    FrmReportCountErrorHours f = new FrmReportCountErrorHours();
                    f.MdiParent = this;
                    f.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi butReportCountErrorByHours_ItemClick: " + ex.Message);
            }
        }

        private void butReportCountErrorOfLineByHours_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var result = ActiveForm(typeof(FrmReportErrorOfLineByHours));
                if (!result)
                {
                    FrmReportErrorOfLineByHours f = new FrmReportErrorOfLineByHours();
                    f.MdiParent = this;
                    f.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi butReportCountErrorOfLineByHours_ItemClick: " + ex.Message);
            }
        }

        private void butLCDDesignConfig_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var result = ActiveForm(typeof(FrmSetupLCDConfig));
                if (!result)
                {
                    FrmSetupLCDConfig f = new FrmSetupLCDConfig();
                    f.MdiParent = this;
                    f.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi butLCDDesignConfig_ItemClick: " + ex.Message);
            }
        }

        private void butThongTinNgay_ItemClick(object sender, ItemClickEventArgs e)
        {
            var result = ActiveForm(typeof(FrmDayInfo));
            if (!result)
            {
                var f = new FrmDayInfo(this);
                f.MdiParent = this;
                f.Show();
            }
        }
        #endregion


        TimeSpan timeActionOld = new TimeSpan(0, 0, 0);
        private void CheckTurnOnOffCOM()
        {
            try
            {
                if (listTurnOnOffCOM.Count > 0)
                {
                    TimeSpan timeNow = DateTime.Now.TimeOfDay;
                    var timeActionNow = new TimeSpan(timeNow.Hours, timeNow.Minutes, 0);
                    if (timeActionNow != timeActionOld)
                    {
                        foreach (var config in listTurnOnOffCOM)
                        {
                            if (config.TimeAction == timeActionNow)
                            {
                                if (config.Status == 0)
                                {
                                    if (config.COMTypeId == 0)
                                    {
                                        if (P.IsOpen)
                                            P.Close();
                                    }
                                    else
                                    {
                                        if (P2.IsOpen)
                                            P2.Close();
                                    }
                                }
                                else
                                {
                                    if (config.COMTypeId == 0)
                                    {
                                        if (!P.IsOpen)
                                            P.Open();
                                    }
                                    else
                                    {
                                        if (!P2.IsOpen)
                                            P2.Open();
                                    }
                                }
                                timeActionOld = timeActionNow;
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

        int intShowLCD = 0;
        int indexLCDShow = 0;
        private void timerLatForm_Tick(object sender, EventArgs e)
        {
            try
            {
                if (AccountSuccess.ListFormLCD.Count > 1)
                {
                    if (intShowLCD > thoiGianLatCacLCD)
                    {
                        Form form = AccountSuccess.ListFormLCD[indexLCDShow];
                        form.Activate();
                        intShowLCD = 0;
                        indexLCDShow += 1;
                        if (indexLCDShow > AccountSuccess.ListFormLCD.Count - 1)
                            indexLCDShow = 0;
                    }
                    else
                        intShowLCD++;
                }
            }
            catch (Exception ex)
            {
                timerLatForm.Enabled = false;
                MessageBox.Show("Lỗi timerLatForm_Tick: " + ex.Message);
            }
        }

        #region New Methods

        /// <summary>
        /// cài đặt thông tin ngày cho từng chuyền
        /// chỉ set cho những chuyền nào có thông tin ngày trước đó
        /// </summary>
        /// <returns></returns>
        private bool SetDayInfoForLine_NN()
        {
            try
            {
                bool result = false;
                if (listChuyen != null && listChuyen.Count > 0)
                {
                    DateTime dateTimeNow = DateTime.Now;
                    int NSType = 0, TotalSecond = 0;
                    int.TryParse(Configs.FirstOrDefault(c => c.Name.Trim().ToUpper().Equals(eAppConfigName.NSTYPE)).Value.Trim(), out NSType);
                    TimeSpan tgLVTrongNgay;

                    var dailyInfor = BLLProductivity.GetYesterday_NX();
                    if (dailyInfor.Data != null && dailyInfor.Data.Count > 0)
                    {
                        var yes_TP = (List<PMS.Data.ThanhPham>)dailyInfor.Data;
                        var today_TP = dailyInfor.Records != null ? (List<PMS.Data.ThanhPham>)dailyInfor.Records : new List<PMS.Data.ThanhPham>();

                        bool isEndDate = false;
                        double nangSuatLaoDong = 0;
                        int soLaoDong = 0, soLuongConLai = 0;
                        var historyObjs = BLLHistoryPressedKeypad.Instance.Get(listChuyen.Select(x => x.MaChuyen).ToList(), todayStr);
                        foreach (var chuyen in listChuyen)
                        {
                            #region history pressed keypad
                            var historyObj = historyObjs.FirstOrDefault(x => x.LineId == chuyen.MaChuyen);
                            if (historyObj == null)
                            {
                                historyObj = new P_HistoryPressedKeypad();
                                historyObj.LineId = chuyen.MaChuyen;
                                historyObj.Date = todayStr;
                                historyObj.IsDeleted = false;
                                historyObj.AssignmentId = null;
                                historyObj.Id = 0;
                                BLLHistoryPressedKeypad.Instance.Insert(historyObj);
                            }
                            #endregion


                            var dsPhanCong = BLLAssignmentForLine.Instance.GetAssignmentForLine(chuyen.MaChuyen, false);
                            if (dsPhanCong != null && dsPhanCong.Count > 0)
                            {
                                tgLVTrongNgay = Helper.HelperControl.TimeIsWorkAllDayOfLine(BLLShift.GetShiftsOfLine(chuyen.MaChuyen), NSType);
                                TotalSecond = (int)tgLVTrongNgay.TotalSeconds;

                                foreach (var item in dsPhanCong)
                                {
                                    isEndDate = false;
                                    soLaoDong = 0;
                                    soLuongConLai = item.SanLuongKeHoach - (TypeOfCheckFinishProduction.Contains("KCS") ? item.LuyKeTH : item.LuyKeBTPThoatChuyen);
                                    if (soLuongConLai < 0)
                                        soLuongConLai = 0;
                                    var TPNow = today_TP.FirstOrDefault(x => x.STTChuyen_SanPham == item.STT);
                                    if (TPNow == null)
                                    {
                                        #region Thành Phẩm
                                        var thanhPhamNgayCu = yes_TP.FirstOrDefault(x => x.STTChuyen_SanPham == item.STT);
                                        if (thanhPhamNgayCu != null)
                                        {
                                            nangSuatLaoDong = (float)Math.Round((TotalSecond / Math.Round((item.NangXuatSanXuat * 100) / thanhPhamNgayCu.HieuSuat)), 2);
                                            soLaoDong = thanhPhamNgayCu.LaoDongChuyen;
                                            var thanhPham = new PMS.Data.ThanhPham();
                                            thanhPham.LaoDongChuyen = soLaoDong;
                                            thanhPham.NangXuatLaoDong = nangSuatLaoDong;
                                            thanhPham.STTChuyen_SanPham = item.STT;
                                            thanhPham.Ngay = todayStr;
                                            thanhPham.LeanKH = thanhPhamNgayCu.LeanKH;
                                            thanhPham.ShowLCD = thanhPhamNgayCu.ShowLCD;
                                            thanhPham.HieuSuat = thanhPhamNgayCu.HieuSuat;
                                            BLLProductivity.InsertThanhPham(thanhPham);

                                            #region Create NX
                                            var nangxuat = new PMS.Data.NangXuat();
                                            nangxuat.Ngay = todayStr;
                                            nangxuat.STTCHuyen_SanPham = item.STT;
                                            nangxuat.DinhMucNgay = (float)Math.Round((nangSuatLaoDong * soLaoDong), 1);
                                            nangxuat.NhipDoSanXuat = (float)Math.Round((item.NangXuatSanXuat / soLaoDong), 1);
                                            nangxuat.TimeLastChange = DateTime.Now.TimeOfDay;
                                            nangxuat.BTPTrenChuyen = 0;
                                            nangxuat.TGCheTaoSP = (int)((item.NangXuatSanXuat * 100) / thanhPham.HieuSuat);
                                            if (soLuongConLai <= nangxuat.DinhMucNgay)
                                            {
                                                isEndDate = true;
                                                nangxuat.DinhMucNgay = soLuongConLai;
                                                var nangSuatLaoDongNow = nangxuat.DinhMucNgay / soLaoDong;
                                                int totalSecondFinishMH1 = (int)(nangSuatLaoDongNow * item.NangXuatSanXuat);
                                                TotalSecond = TotalSecond - totalSecondFinishMH1;
                                            }
                                            nangxuat.IsEndDate = isEndDate;
                                            BLLProductivity.InsertOrUpdateNangXuat(nangxuat);
                                            BLLProductivity.AddNangSuatCumOfChuyen(item.STT, chuyen.MaChuyen);
                                            BLLProductivity.AddNangSuatCumLoiOfChuyen(item.STT, chuyen.MaChuyen);
                                            // if (!isEndDate)
                                            //      break;
                                            #endregion

                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        soLaoDong = TPNow.LaoDongChuyen;
                                        nangSuatLaoDong = Math.Round((item.NangXuatSanXuat * 100) / TPNow.HieuSuat);
                                        var nangSuat = BLLProductivity.TTNangXuatTrongNgay(todayStr, item.STT);
                                        if (nangSuat != null)
                                        {
                                            nangSuat.DinhMucNgay = (float)Math.Round((nangSuatLaoDong * soLaoDong), 1);
                                            nangSuat.NhipDoSanXuat = (float)Math.Round((item.NangXuatSanXuat / soLaoDong), 1);
                                            nangSuat.TimeLastChange = DateTime.Now.TimeOfDay;
                                            nangSuat.BTPTrenChuyen = 0;
                                            if (soLuongConLai <= nangSuat.DinhMucNgay)
                                            {
                                                isEndDate = true;
                                                nangSuat.DinhMucNgay = soLuongConLai + (nangSuat.ThucHienNgay - nangSuat.ThucHienNgayGiam);
                                            }
                                            nangSuat.IsEndDate = isEndDate;
                                            BLLProductivity.InsertOrUpdateNangXuat(nangSuat);
                                            BLLProductivity.AddNangSuatCumOfChuyen(item.STT, chuyen.MaChuyen);
                                            BLLProductivity.AddNangSuatCumLoiOfChuyen(item.STT, chuyen.MaChuyen);
                                            //if (!isEndDate)
                                            //    break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                    MessageBox.Show("Lỗi: Không thể khởi tạo thông tin ngày cho chuyền. Vì danh sách chuyền không tồn tại.");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        private void btnVideoShedule_ItemClick(object sender, ItemClickEventArgs e)
        {
            var result = ActiveForm(typeof(FrmVideoShedule));
            if (!result)
            {
                var f = new FrmVideoShedule();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            var result = ActiveForm(typeof(FrmInsertBTP));
            if (!result)
            {
                var f = new FrmInsertBTP(this);
                f.MdiParent = this;
                f.Show();
            }
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        List<int> chuyenExists_KB = new List<int>();
        private void Timer_ReadNotifyForKanban_Tick(object sender, EventArgs e)
        {
            chuyenExists_KB.Clear();
            listfilewav.Clear();
            var assigIds = BLLAssignmentForLine.Instance.GetAssignmentIds(todayStr, AccountSuccess.IsAll == 0 ? 0 : int.Parse(AccountSuccess.IdFloor.Trim()));
            if (assigIds.Length > 0)
            {
                for (int j = 0; j < assigIds.Length; j++)
                    CheckNotify(assigIds[j]);
            }

            if (listfilewav.Count > 0)
                queuePlayFileWavKanBan.Enqueue(listfilewav);
        }

        private void CheckNotify(int assignId)
        {
            try
            {
                var cspObj = BLLProductivity.GetProductivitiesInDayForKanban(assignId, todayStr);
                if (cspObj != null && cspObj.LK_BTP < cspObj.SanLuongKeHoach)
                {
                    var obj = chuyenExists_KB.FirstOrDefault(x => x == cspObj.MaChuyen);
                    if (obj == 0)
                    {
                        string machuyen = cspObj.MaChuyen.ToString();
                        var infChuyen = listChuyen_O.FirstOrDefault(x => x.MaChuyen == machuyen);
                        if (infChuyen != null)
                        {
                            int dinhMucNgay = (int)cspObj.NormsDay;
                            string filewavChuyen = cspObj.Sound;

                            bool isEnddate = cspObj.IsEndDate;
                            var tpObj = BLLProductivity.GetThanhPhamByNgayAndSTT(todayStr, assignId);

                            int von = cspObj.BTPInLine > 0 && tpObj != null && tpObj.LaoDongChuyen > 0 ? (int)(Math.Ceiling((double)cspObj.BTPInLine / tpObj.LaoDongChuyen)) : 0;
                            von = von < 0 ? 0 : von;
                            var readPercentObj = BLLReadPercent.Instance.Get(cspObj.STT, von);
                            int IdDenParent = cspObj.IdDen ?? 0,
                                IdTyLeDoc = readPercentObj != null ? readPercentObj.IdParent.Value : 0;
                            if (!isEnddate)
                            {
                                if (IdTyLeDoc != 0 && cspObj.LuyKeBTPThoatChuyen > 0)
                                {   // readpercent_short(filewavChuyen, readPercentObj.Sound, filewavSlient, readPercentObj.CountRepeat);
                                    SumFile(filewavChuyen, readPercentObj.Sound, filewavSlient, readPercentObj.CountRepeat);
                                    chuyenExists_KB.Add(cspObj.MaChuyen);
                                }
                            }
                            else
                            {
                                if ((cspObj.SanLuongKeHoach - cspObj.LuyKeTH) - cspObj.BTPInLine > 0)
                                {
                                    if (IdTyLeDoc != 0 && cspObj.LuyKeBTPThoatChuyen > 0)
                                    {  //  readpercent_short(filewavChuyen, readPercentObj.Sound, filewavSlient, readPercentObj.CountRepeat);
                                        SumFile(filewavChuyen, readPercentObj.Sound, filewavSlient, readPercentObj.CountRepeat);
                                        chuyenExists_KB.Add(cspObj.MaChuyen);
                                    }
                                }
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

        private void SumFile(string filewavChuyen, string filewavNoiDung, string filewavslient, int repeatTimes)
        {
            try
            {
                //  listfilewav.Clear();
                listfilewav.Add(filewavChuyen);
                listfilewav.Add(filewavNoiDung);
                listfilewav.Add(filewavslient);
                if (repeatTimes > 1)
                {
                    int countlist = listfilewav.Count();
                    for (int j = 0; j < repeatTimes - 1; j++)
                    {
                        for (int k = 0; k < countlist; k++)
                        {
                            listfilewav.Add(listfilewav[k]);
                        }
                    }
                }

                //if (listfilewav.Count > 0)
                // queuePlayFileWavKanBan.Enqueue(listfilewav);
            }
            catch (Exception)
            {

                throw;
            }
        }

        List<int> chuyenExists = new List<int>();
        private void Timer_ReadNotifyForInventoryInKCS_Tick(object sender, EventArgs e)
        {
            chuyenExists.Clear();
            var assigIds = BLLAssignmentForLine.Instance.GetAssignmentIds(todayStr, AccountSuccess.IsAll == 0 ? 0 : int.Parse(AccountSuccess.IdFloor.Trim()));
            if (assigIds.Length > 0)
                for (int j = 0; j < assigIds.Length; j++)
                {
                    CheckKCSInventoryNotify(assigIds[j]);
                }

            if (listfilewav.Count > 0)
                queuePlayFileWavKanBan.Enqueue(listfilewav);
        }

        private void CheckKCSInventoryNotify(int assignId)
        {
            try
            {
                var cspObj = BLLProductivity.GetProductivitiesInDayForKanban(assignId, todayStr);
                if (cspObj != null)
                {
                    var obj = chuyenExists.FirstOrDefault(x => x == cspObj.MaChuyen);
                    if (obj == 0)
                    {
                        string machuyen = cspObj.MaChuyen.ToString();
                        var infChuyen = listChuyen_O.FirstOrDefault(x => x.MaChuyen == machuyen);
                        if (infChuyen != null)
                        {
                            string filewavChuyen = cspObj.Sound;
                            int value = (cspObj.TC_Day - cspObj.TC_Day_G) - (cspObj.TH_Day - cspObj.TH_Day_G) - (cspObj.Err_Day - cspObj.Err_Day_G);
                            var readPercentObj = BLLReadPercent_KCSInventory.Instance.Get(cspObj.STT, value);
                            if (readPercentObj != null)
                            {
                                SumFile(filewavChuyen, readPercentObj.SoundPath.Trim(), filewavSlient, readPercentObj.RepeatTimes);
                                chuyenExists.Add(cspObj.MaChuyen);
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

        private void btnSetLightKanban_ItemClick(object sender, ItemClickEventArgs e)
        {
            var result = ActiveForm(typeof(FrmKanbanLightPercent));
            if (!result)
            {
                var f = new FrmKanbanLightPercent();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnSetLightProduction_ItemClick(object sender, ItemClickEventArgs e)
        {
            var result = ActiveForm(typeof(FrmTyLeDen_PhanChuyen));
            if (!result)
            {
                FrmTyLeDen_PhanChuyen f = new FrmTyLeDen_PhanChuyen();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnCallBTP_ItemClick(object sender, ItemClickEventArgs e)
        {
            var result = ActiveForm(typeof(FrmQuanlybaotyleKanBan));
            if (!result)
            {
                FrmQuanlybaotyleKanBan f = new FrmQuanlybaotyleKanBan();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnAlertKCS_ItemClick(object sender, ItemClickEventArgs e)
        {
            var result = ActiveForm(typeof(FrmQL_ReadPercent_KCSInventory));
            if (!result)
            {
                var f = new FrmQL_ReadPercent_KCSInventory();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnQLError_ItemClick(object sender, ItemClickEventArgs e)
        {
            var result = ActiveForm(typeof(FrmErrorMana));
            if (!result)
            {
                var f = new FrmErrorMana();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnQLShift_ItemClick(object sender, ItemClickEventArgs e)
        {
            var result = ActiveForm(typeof(FrmShiftManagement));
            if (!result)
            {
                var f = new FrmShiftManagement();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnQLProduct_ItemClick(object sender, ItemClickEventArgs e)
        {
            var result = ActiveForm(typeof(Form1));
            if (!result)
            {
                Form1 f = new Form1();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnAlertFinishProduct_ItemClick(object sender, ItemClickEventArgs e)
        {
            var result = ActiveForm(typeof(FrmBaoHetHang));
            if (!result)
            {
                FrmBaoHetHang f = new FrmBaoHetHang();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnResetKeypadFollowLine_ItemClick(object sender, ItemClickEventArgs e)
        {
            var result = ActiveForm(typeof(FrmResetKeypad));
            if (!result)
            {
                var f = new FrmResetKeypad(this);
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnViewLog_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmProcessLog frm = new FrmProcessLog();
            frm.Show();
        }

        private void btnQLCloseOrOpenComport_ItemClick(object sender, ItemClickEventArgs e)
        {
            var result = ActiveForm(typeof(FrmTurnComportMng));
            if (!result)
            {
                FrmTurnComportMng f = new FrmTurnComportMng();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnQLProduct_n_ItemClick(object sender, ItemClickEventArgs e)
        {
            var result = ActiveForm(typeof(FrmProduct_N));
            if (!result)
            {
                var f = new FrmProduct_N();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            UpdateTableLoiSanXuat("");
        }



        private void btnBTP_HC_ItemClick(object sender, ItemClickEventArgs e)
        {
            var result = ActiveForm(typeof(FrmBTP_HCStruct));
            if (!result)
            {
                var f = new FrmBTP_HCStruct((int)ePhaseType.BTP_HC);
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnSetLightBTPConLai_ItemClick(object sender, ItemClickEventArgs e)
        {
            var result = ActiveForm(typeof(frmTiLeDenCat));
            if (!result)
            {
                var f = new frmTiLeDenCat();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnPhaseHTManage_ItemClick(object sender, ItemClickEventArgs e)
        {
            var result = ActiveForm(typeof(FrmBTP_HCStruct));
            if (!result)
            {
                var f = new FrmBTP_HCStruct((int)ePhaseType.HOANTAT);
                f.MdiParent = this;
                f.Show();
            }
        }

    }

    public class CurrentAssignmentObj
    {
        public int AssignId { get; set; }
        public int AcctionType { get; set; }
        public int errorId { get; set; }
    }
}
