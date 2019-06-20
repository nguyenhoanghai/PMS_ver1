using System; 

namespace PMS.Business.Models
{

    public class HoangGiaReportModel  
    {
        public int STT { get; set; }
        public int AssignId { get; set; }
        public int LineId { get; set; }
        public string LineName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CustomerCode { get; set; }
        public int BaseLabours { get; set; }
        public int CurrentLabours { get; set; }
        public int NewLabours { get; set; }
        public int OffLabours { get; set; }
        public int OnVacationLabours { get; set; }
        public int PregnantLabours { get; set; }
        public int SLKH { get; set; }
        public double PriceCM { get; set; }
        public double Price { get; set; }
        public double PriceCut { get; set; }
        public double KCS { get; set; }
        public double LK_KCS { get; set; }
        public double LK_TC { get; set; }
        public double TC { get; set; }
        public double Ui { get; set; }
        public double LK_Ui { get; set; }
        public double DongThung { get; set; }
        public double LK_DongThung { get; set; }
        public double Cut { get; set; }
        public double LK_Cut { get; set; }
        public double BTP { get; set; }
        public double LK_BTP { get; set; }
        public DateTime? DateInput { get; set; }
        public DateTime? DateOutput { get; set; }  
    }

}