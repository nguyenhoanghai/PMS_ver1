using PMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Models
{
   public class ProductivitiesModel : NangXuat
    {
        public int LineId { get; set; }
        public string LineName { get; set; }
        public int productId { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public double ProductPriceCM { get; set; } 
        public int? IdDenNangSuat { get; set; }
        public int LaborsBase { get; set; }

        public List<NangSuat_CumLoi> Errors { get; set; }
        public int OrderIndex { get; set; }
        public int LK_TH { get; set; }
        public int LK_BTP { get; set; }
        public int LK_TC { get; set; }
        public double NangSuatSanXuat { get; set; } 
        public int ProductionPlans { get; set; }

       //doanh thu
        public double RevenuePlan { get; set; }
        public double RevenueTH { get; set; }
       //% thực hiện
        public double PercentTH { get; set; }
        public List<TheoDoiNgay> theodoingays { get; set; }
        public ProductivitiesModel()
        {
            theodoingays = new List<TheoDoiNgay>();
        }
    }
}
