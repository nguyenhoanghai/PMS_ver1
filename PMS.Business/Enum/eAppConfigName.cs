using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Enum
{
    public static class eAppConfigName
    {
        public const string TABLE = "TABLE";
        public const string TIMECHECK = "TIMECHECK";
        public const string KieuTinhNhipThucTe = "KIEUTINHNHIPTHUCTE";
        public const string KieuTinhTyLeHangLoi = "KIEUTINHTYLEHANGLOI";
        public const string SoundFileExtentions = "SOUNDFILEEXTENTIONS";
        public const string MANHINHLCD = "MANHINHLCD";
        public const string HIENTHIDENNS = "HIENTHIDENNS";
        public const string TINHBTPTHOATCHUYEN = "TINHBTPTHOATCHUYEN";
        public const string SENDMAIL = "SENDMAIL";
        public const string READSOUND = "READSOUND";
        public const string TIMEOUTACK = "TIMEOUTACK";
        public const string NSTYPE = "NSTYPE";

        /// <summary>
        /// cài đặt thông số keypad
        /// min => theo số nhỏ
        /// max => theo số lớn
        /// keypad => theo keypad
        /// </summary>
        public const string SETTOTALBYMINORMAX = "SETTOTALBYMINORMAX";

        /// <summary>
        /// Thời gian quét nhận data từ kaypad
        /// </summary>
        public const string TIMESENDREQUESTANDDATA = "TIMESENDREQUESTANDDATA";
        public const string TimeSleepWhenInitKeypad = "TimeSleepWhenInitKeypad";

        /// <summary>
        /// kiểu tính BTP trên chuyền
        /// 1 => LK BTP / LK Thoát Chuyền
        /// 2 => LK BTP / LK KCS
        /// </summary>
        public const string GETBTPINLINEBYTYPE = "GETBTPINLINEBYTYPE";

        /// <summary>
        /// tự động set thong tin ngày hay ko
        /// </summary>
        public const string AUTOSETDAYINFO = "AUTOSETDAYINFO";
        public const string TIMEREADSOUND = "TIMEREADSOUND";
        public const string ISAUTOTURNONOFFCOM = "ISAUTOTURNONOFFCOM";
        public const string ISAUTOMOVEQUANTITYMORTH = "ISAUTOMOVEQUANTITYMORTH";
        public const string AUTOMOVEQUANTITYMORTHTYPE = "AUTOMOVEQUANTITYMORTHTYPE";

        /// <summary>
        /// thời gian chuyển trang LCD dùng cho Application
        /// </summary>
        public const string THOIGIANLATCACLCD = "THOIGIANLATCACLCD";

        public const string TACHNHANDULIEU = "TACHNHANDULIEU";
        public const string SoundSilent = "SOUNDSILENT";
        public const string COM = "COM";
        public const string BAUDRATE = "BAUDRATE";
        public const string DATABITS = "DATABITS";
        public const string PARITY = "PARITY";
        public const string STOPBITS = "STOPBITS";
        public const string COM2 = "COM2";
        public const string BAUDRATE2 = "BAUDRATE2";
        public const string DATABITS2 = "DATABITS2";
        public const string PARITY2 = "PARITY2";
        public const string STOPBITS2 = "STOPBITS2";
        /// <summary>
        /// Thời gian Reset cổng Com
        /// </summary>
        public const string TimeResetComport = "TIMERESETCOMPORT";
        /// <summary>
        /// có sử dụng com bảng hay ko
        /// </summary>
        public const string IsUseTableComport = "ISUSETABLECOMPORT";

        /// <summary>
        /// cách tính định mức ngày 0 => từng mã riêng biệt , 1 => cộng dồn tất cả mã trong ngày
        /// </summary>
        public const string CalculateNormsdayType = "CALCULATENORMSDAYTYPE";

        public const string timeSendRequestKCSButHandleError = "timeSendRequestKCSButHandleError";
        public const string timeSendRequestTCButHandleError = "timeSendRequestTCButHandleError";
        public const string timeSendRequestErrorButHandleError = "timeSendRequestErrorButHandleError";
        public const string SaveMediaFileAddress = "SaveMediaFileAddress";

        /// <summary>
        /// Thời gian refresh dữ liệu form thông tin năng suất trong ngày : đơn vị milisecond
        /// </summary>
        public const string TimeRefreshFromDayInfoView = "TimeRefreshFromDayInfoView";
        /// <summary>
        /// Thời gian đóng form thông tin năng suất trong ngày khi không sử dụng để giảm CPU
        /// </summary>
        public const string TimeCloseFromDayInfoViewIfNotUse = "TimeCloseFromDayInfoViewIfNotUse";
        public const string Slient = "SLIENT";

        public const string IsUseReadNotifyForKanban = "IsUseReadNotifyForKanban";
        public const string TimerReadNotifyForKanban = "TimerReadNotifyForKanban";
        public const string IsUseReadNotifyForInventoryInKCS = "IsUseReadNotifyForInventoryInKCS";
        public const string TimerReadNotifyForInventoryInKCS = "TimerReadNotifyForInventoryInKCS";

        /// <summary>
        /// Kiểu ktra kết thúc mã hàng
        /// 1 => LK KCS
        /// 2 => LK KCS + LK TC
        /// </summary>
        public const string TypeOfCheckFinishProduction = "TypeOfCheckFinishProduction";

        /// <summary>
        /// Kiểu ktra dinh muc ngay = 1 lk KCS ; 2 => LK TC 
        /// </summary>
        public const string TypeOfCaculateDayNorms = "TypeOfCaculateDayNorms";

        /// <summary>
        ///Kieu hien thi thong tin san pham ra LCD  => 1 theo ban phim bam  ; 2 => set theo thong tin ngay dc chon
        /// </summary>
        public const string TypeOfShowProductToLCD = "TypeOfShowProductToLCD";

        /// <summary>
        /// Kieu hien xử lý sản lượng khi nhận lên từ keypad
        /// 0 => đồng bộ sản lượng tất cả các keypad chung với nhau và gửi thông tin đồng bộ lại keypad sau khi xử lý xong.   
        /// 1 => tính riên sản lượng cho từng keypad và không gửi thông tin đồng bộ lại cho các keypad sau khi xử lý xong. 
        /// </summary>
        public const string KeypadQuantityProcessingType = "KeypadQuantityProcessingType";
        public const string SoundPath = "SoundPath"; 
        public const string  IsUseBTP_HC = "IsUseBTP_HC"; 

        /// <summary>
        /// doc canh bao khi san luong vuot ke hoach
        /// </summary>
        public const string  IsWarningIfProductIsOver = "IsWarningIfProductIsOver";
        public const string  SoundBTPOrverPlan = "SoundBTPOrverPlan";
        public const string  SoundKCSOrverTC = "SoundKCSOrverTC";
        public const string SoundTCOrverBTP = "SoundTCOrverBTP";
        /// <summary>
        /// tính doanh thu theo thoát chuyền or thực hiện
        /// </summary>
        public const string TypeOfCalculateRevenues = "TypeOfCalculateRevenues";



    }
}
