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
    
    public partial class P_ReadPercentOfLine
    {
        public int Id { get; set; }
        public Nullable<int> ReadPercent_KCSInventoryId { get; set; }
        public Nullable<int> ReadPercentId { get; set; }
        public Nullable<int> KanbanLightPercentId { get; set; }
        public Nullable<int> ProductLightPercentId { get; set; }
        public int LineId { get; set; }
        public int AssignmentId { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual Chuyen Chuyen { get; set; }
        public virtual Chuyen_SanPham Chuyen_SanPham { get; set; }
        public virtual P_LightPercent P_LightPercent { get; set; }
        public virtual P_LightPercent P_LightPercent1 { get; set; }
        public virtual P_ReadPercent_KCSInventory P_ReadPercent_KCSInventory { get; set; }
        public virtual ReadPercent ReadPercent { get; set; }
    }
}
