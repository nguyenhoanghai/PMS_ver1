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
    
    public partial class NangXuat
    {
        public NangXuat()
        {
            this.P_PhaseDailyLog = new HashSet<P_PhaseDailyLog>();
        }
    
        public int Id { get; set; }
        public string Ngay { get; set; }
        public int STTCHuyen_SanPham { get; set; }
        public double DinhMucNgay { get; set; }
        public int ThucHienNgay { get; set; }
        public int ThucHienNgayGiam { get; set; }
        public int BTPTrenChuyen { get; set; }
        public double NhipDoThucTeBTPThoatChuyen { get; set; }
        public double NhipDoThucTe { get; set; }
        public double NhipDoSanXuat { get; set; }
        public int IsChange { get; set; }
        public int IsChangeBTP { get; set; }
        public int IsBTP { get; set; }
        public Nullable<System.TimeSpan> TimeLastChange { get; set; }
        public int BTPLoi { get; set; }
        public bool IsEndDate { get; set; }
        public int BTPThoatChuyenNgay { get; set; }
        public int BTPThoatChuyenNgayGiam { get; set; }
        public int SanLuongLoi { get; set; }
        public int SanLuongLoiGiam { get; set; }
        public int BTPTang { get; set; }
        public int BTPGiam { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public bool IsStopOnDay { get; set; }
        public Nullable<System.TimeSpan> TimeStopOnDay { get; set; }
        public int BTP_HC_Tang { get; set; }
        public int BTP_HC_Giam { get; set; }
        public int TGCheTaoSP { get; set; }
    
        public virtual ICollection<P_PhaseDailyLog> P_PhaseDailyLog { get; set; }
        public virtual Chuyen_SanPham Chuyen_SanPham { get; set; }
    }
}
