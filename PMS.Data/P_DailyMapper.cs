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
    
    public partial class P_DailyMapper
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public int ProIndex { get; set; }
        public int AssignId { get; set; }
        public int PhaseIndex { get; set; }
        public int PhaseId { get; set; }
        public int LineId { get; set; }
        public int KeypadId { get; set; }
        public int EquipCode { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual Chuyen Chuyen { get; set; }
        public virtual P_Keypad P_Keypad { get; set; }
        public virtual Chuyen_SanPham Chuyen_SanPham { get; set; }
    }
}
