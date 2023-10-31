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
    
    public partial class KhachHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KhachHang()
        {
            this.DonDats = new HashSet<DonDat>();
            this.HoaDons = new HashSet<HoaDon>();
            this.TinTucs = new HashSet<TinTuc>();
        }
    
        public int MaKH { get; set; }
        public Nullable<int> MaLoaiKH { get; set; }
        public string AnhDaiDien { get; set; }
        public string HoTen { get; set; }
        public string GioiTinh { get; set; }
        public Nullable<System.DateTime> NgaySinh { get; set; }
        public string DiaChi { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }
        public string Facebook { get; set; }
        public Nullable<int> MaTK { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonDat> DonDats { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HoaDon> HoaDons { get; set; }
        public virtual LoaiKhachHang LoaiKhachHang { get; set; }
        public virtual TaiKhoan TaiKhoan { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TinTuc> TinTucs { get; set; }
    }
}