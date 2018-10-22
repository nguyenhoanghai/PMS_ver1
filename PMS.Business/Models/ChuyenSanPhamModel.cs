using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Models
{
    public class ChuyenSanPhamModel : Chuyen_SanPham
    {
        public string LineName { get; set; }
        public string CommoName { get; set; }
        public double? Price { get; set; }
        public double PriceCM { get; set; }
        
        /// <summary>
        /// Định mức ngày
        /// </summary>
        public double NormsDay { get; set; }

        /// <summary>
        /// Định mức giờ
        /// </summary>
        public double NormsHours { get; set; }

        /// <summary>
        /// Định mức mỗi lần lấy NS
        /// </summary>
        public double NormsPerTimes { get; set; }

        public int TH_Day { get; set; }
        public int TH_Day_G { get; set; }
        public int TC_Day { get; set; }
        public int TC_Day_G { get; set; }
        public int Err_Day { get; set; }
        public int Err_Day_G { get; set; }
        public int BTP_Day { get; set; }
        public int BTP_Day_G { get; set; }
        public int BTPInLine { get; set; }

        public double NhipSX { get; set; }
        public double NhipTT { get; set; }
        public double NhipTC { get; set; }

        public int? IdDen { get; set; }
        public string IsFinishStr { get; set; }
        public string Sound { get; set; }
        public int CountAssignment { get; set; }
        public int LK_BTP { get; set; }
        public int LK_BTP_G { get; set; }

        /// <summary>
        /// Thời gian đã Làm việc trong ngày
        /// </summary>
        public int TGDaLV { get; set; }

        /// <summary>
        /// Vốn
        /// </summary>
        public int Lean { get; set; }

        /// <summary>
        /// Lao động định biên
        /// </summary>
        public int BaseLabors { get; set; }

        /// <summary>
        /// Lao động thực tế
        /// </summary>
        public int CurrentLabors { get; set; }

        /// <summary>
        /// sản lượng kế hoạch trong tháng
        /// </summary>
        public int ProductionPlansInMonth { get; set; }

        /// <summary>
        /// Lũy Kế thực hiện trong tháng
        /// </summary>
        public int LK_TH_InMonth { get; set; } 

        /// <summary>
        /// Lũy Kế thoát chuyền trong tháng
        /// </summary>
        public int LK_TC_InMonth { get; set; }

        /// <summary>
        /// Lũy Kế BTP trong tháng
        /// </summary>
        public int LK_BTP_InMonth { get; set; }

        /// <summary>
        /// Tỉ lệ thực hiện
        /// </summary>
        public double Percent_TH { get; set; }

        /// <summary>
        /// Tỉ lệ Lỗi
        /// </summary>
        public double Percent_Error { get; set; }

        /// <summary>
        /// Tỉ lệ Nhịp
        /// </summary>
        public double Percent_Nhip { get; set; }

        /// <summary>
        /// Doanh thu tháng
        /// </summary>
        public double RevenuesInMonth { get; set; }  

        /// <summary>
        /// Doanh thu ngày
        /// </summary>
        public double RevenuesInDay { get; set; }

        /// <summary>
        /// Danh sách thông tin giờ làm việc của chuyền
        /// </summary>
        public List<WorkingTimeModel> workingTimes { get; set; }
        public double LeanKH { get; set; }
        public int BTPLoi { get; set; }
        public int ReadPercentId { get; set; }
        public bool IsEndDate { get; set; }
        // public double NangXuatSanXuat { get; set; }
         public double ProductionTime { get; set; }
         public double HieuSuatNgay { get; set; }

         public int lkCongDoan { get; set; }
        public ChuyenSanPhamModel()
        {
            workingTimes = new List<WorkingTimeModel>();
        }
    }
}
