//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TMDTProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class BinhLuanTourDuLich
    {
        public int MaBinhLuan { get; set; }
        public Nullable<int> MaTour { get; set; }
    
        public virtual BinhLuan BinhLuan { get; set; }
        public virtual TourDuLich TourDuLich { get; set; }
    }
}