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
    
    public partial class ChiTietVeXeKhach
    {
        public int MaChiTiet { get; set; }
        public Nullable<int> MaDon { get; set; }
        public Nullable<int> MaVeXe { get; set; }
        public string ChiTiet { get; set; }
    
        public virtual DonDat DonDat { get; set; }
        public virtual VeXeKhach VeXeKhach { get; set; }
    }
}
