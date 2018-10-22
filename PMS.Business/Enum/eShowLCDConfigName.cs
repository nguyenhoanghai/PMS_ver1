using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Enum
{
   public static class eShowLCDConfigName
    {
        public const string GetBTPInLineByType = "GETBTPINLINEBYTYPE";
        public const string WebPageHeight = "WEBPAGEHEIGHT";
        public const string LCDTongHop_Paging = "SODONGHIENTHILCDTONGHOP";
        public const string LCDKanBan_Paging = "SODONGHIENTHILCDKANBAN";
        public const string Interval_VerticalAutoScroll_Tick = "THOIGIANNHAYCHUYEN";
        public const string Interval_ChangeLCD = "THOIGIANCHUYENLCD";
        public const string TinhBTPThoatChuyen = "TINHBTPTHOATCHUYEN";
        public const string TypeShowProductivitiesPerHour = "HIENTHINSGIO";
        public const string IntervalGetTime = "INTERVALGETTIME";
        public const string IntervalShow = "INTERVALSHOW";
        public const string TimeShowMH1_TimeShowMH2 = "TIMESHOWMH1/TIMESHOWMH2";
        public const string IntervalLoadData = "INTERVALLOADDATA";
        public const string Logo = "LOGO";
        public const string LoaiDoanhThuThang = "LOAIDOANHTHUTHANG";

        /// <summary>
        /// số lần lấy thông tin năng suất trong ngày của chuyền
        /// </summary>
        public const string TimesGetNSInDay = "TIMESGET_NSINDAY";
        /// <summary>
        /// khoảng thời gian cách nhau của lần lấy thông tin năng suất trong ngày của chuyền
        /// </summary>
        public const string KhoangCachLayNangSuat = "KHOANGCACHLAYNANGSUAT";
    }
}
