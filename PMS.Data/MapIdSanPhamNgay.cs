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
    
    public partial class MapIdSanPhamNgay
    {
        public int Id { get; set; }
        public string Ngay { get; set; }
        public int MaChuyen { get; set; }
        public int STT { get; set; }
        public int STTChuyenSanPham { get; set; }
        public int MaSanPham { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual Chuyen Chuyen { get; set; }
        public virtual Chuyen_SanPham Chuyen_SanPham { get; set; }
        public virtual SanPham SanPham { get; set; }
    }
}