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
    
    public partial class TourDuLich
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TourDuLich()
        {
            this.BinhLuanTourDuLiches = new HashSet<BinhLuanTourDuLich>();
        }
    
        public int MaTour { get; set; }
        public Nullable<int> MaDT { get; set; }
        public string TenTour { get; set; }
        public string Anh { get; set; }
        public Nullable<decimal> Gia { get; set; }
        public string ChiTiet { get; set; }
        public Nullable<int> MaTT { get; set; }
    
        public virtual DoiTac DoiTac { get; set; }
        public virtual TinhTrang TinhTrang { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BinhLuanTourDuLich> BinhLuanTourDuLiches { get; set; }
    }
}
