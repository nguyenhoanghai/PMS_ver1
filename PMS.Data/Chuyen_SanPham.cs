//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PMS.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Chuyen_SanPham
    {
        public Chuyen_SanPham()
        {
            this.BTPs = new HashSet<BTP>();
            this.MapIdSanPhamNgays = new HashSet<MapIdSanPhamNgay>();
            this.NangXuats = new HashSet<NangXuat>();
            this.P_Phase_Assign_Log = new HashSet<P_Phase_Assign_Log>();
            this.P_DailyMapper = new HashSet<P_DailyMapper>();
            this.P_DailyPlans = new HashSet<P_DailyPlans>();
            this.P_MonthlyProductionPlans = new HashSet<P_MonthlyProductionPlans>();
            this.P_PhaseDaily = new HashSet<P_PhaseDaily>();
            this.P_ReadPercentOfLine = new HashSet<P_ReadPercentOfLine>();
            this.ThanhPhams = new HashSet<ThanhPham>();
        }
    
        public int STT { get; set; }
        public int STTThucHien { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        public int MaChuyen { get; set; }
        public int MaSanPham { get; set; }
        public int SanLuongKeHoach { get; set; }
        public int LuyKeTH { get; set; }
        public int LuyKeBTPThoatChuyen { get; set; }
        public bool IsFinishBTPThoatChuyen { get; set; }
        public bool IsMoveQuantityFromMorthOld { get; set; }
        public bool IsFinishNow { get; set; }
        public bool IsFinish { get; set; }
        public bool IsDelete { get; set; }
        public System.DateTime TimeAdd { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<System.DateTime> FinishedDate { get; set; }
        public int LK_BTP { get; set; }
        public int LK_BTP_HC { get; set; }
        public bool HideForever { get; set; }
    
        public virtual ICollection<BTP> BTPs { get; set; }
        public virtual Chuyen Chuyen { get; set; }
        public virtual SanPham SanPham { get; set; }
        public virtual ICollection<MapIdSanPhamNgay> MapIdSanPhamNgays { get; set; }
        public virtual ICollection<NangXuat> NangXuats { get; set; }
        public virtual ICollection<P_Phase_Assign_Log> P_Phase_Assign_Log { get; set; }
        public virtual ICollection<P_DailyMapper> P_DailyMapper { get; set; }
        public virtual ICollection<P_DailyPlans> P_DailyPlans { get; set; }
        public virtual ICollection<P_MonthlyProductionPlans> P_MonthlyProductionPlans { get; set; }
        public virtual ICollection<P_PhaseDaily> P_PhaseDaily { get; set; }
        public virtual ICollection<P_ReadPercentOfLine> P_ReadPercentOfLine { get; set; }
        public virtual ICollection<ThanhPham> ThanhPhams { get; set; }
    }
}
