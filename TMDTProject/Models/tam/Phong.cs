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
    
    public partial class Phong
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Phong()
        {
            this.ChiTietPhongKhachSans = new HashSet<ChiTietPhongKhachSan>();
        }
    
        public int MaPhong { get; set; }
        public Nullable<int> MaLoaiPhong { get; set; }
        public string TenPhong { get; set; }
        public Nullable<int> MaKS { get; set; }
        public Nullable<decimal> Gia { get; set; }
        public string DVT { get; set; }
        public string Anh { get; set; }
        public string ChiTiet { get; set; }
        public Nullable<int> MaTT { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietPhongKhachSan> ChiTietPhongKhachSans { get; set; }
        public virtual KhachSan KhachSan { get; set; }
        public virtual LoaiPhong LoaiPhong { get; set; }
        public virtual TinhTrang TinhTrang { get; set; }
    }
}
