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
    
    public partial class P_VideoLibrary
    {
        public P_VideoLibrary()
        {
            this.P_PlayVideoSheduleDetail = new HashSet<P_PlayVideoSheduleDetail>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public double Size { get; set; }
        public double Length { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual ICollection<P_PlayVideoSheduleDetail> P_PlayVideoSheduleDetail { get; set; }
    }
}
