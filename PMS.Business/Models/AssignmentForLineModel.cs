using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Models
{
    public class AssignmentForLineModel : Chuyen_SanPham
    {
        public string LineName { get; set; }
        public string CommoName { get; set; }
        public double CommoPrice { get; set; }
        public double CommoPriceCM { get; set; }
        public int ReadPercentId { get; set; }
        public string ReadPercentName { get; set; }
        public double NangXuatSanXuat { get; set; }
        public int NangSuatId { get; set; }
    }

    public class AssignmentModel_BangDienTu
    {
        public int AssignId { get; set; }
        public double TimeProductPerItem { get; set; }
        public int lineId { get; set; }
        public int? LightPercentId { get; set; }
        public string LineName { get; set; }
        public string CommoName { get; set; }
        public int ProductionPlans { get; set; }
        public double ProductionNorms { get; set; }
        public int KCSInDay { get; set; }
        public int KCSInHours { get; set; }
        public int LK_KCS { get; set; }
        public int TCInDay { get; set; }
        public int TCInHours { get; set; }
        public int LK_TC { get; set; }
        public int BTPInDay { get; set; }
        public int LK_BTP { get; set; }
        public int ErrorInDay { get; set; }

        public double Lean { get; set; }

        public double KCSPercent { get; set; }
        public int Labours { get; set; }
        public int BTPInLine { get; set; }
        public double BTPPerLabour { get; set; }
        public double ProductionPace { get; set; }
        public double CurrentPace { get; set; }
        public double Current_TC_Pace { get; set; }
        public double SalesDate { get; set; }
        public double lightPercent { get; set; }


        public double CommoPrice { get; set; }
        public double CommoPriceCM { get; set; }

        public bool IsChange { get; set; }
        public bool IsShowLCD { get; set; }
    }
}
