﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TMDTEntities1 : DbContext
    {
        public TMDTEntities1()
            : base("name=TMDTEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ChiTietChoThueXe> ChiTietChoThueXes { get; set; }
        public virtual DbSet<ChiTietPhongKhachSan> ChiTietPhongKhachSans { get; set; }
        public virtual DbSet<ChiTietVeMayBay> ChiTietVeMayBays { get; set; }
        public virtual DbSet<ChiTietVeXeKhach> ChiTietVeXeKhaches { get; set; }
        public virtual DbSet<DoiTac> DoiTacs { get; set; }
        public virtual DbSet<DonDat> DonDats { get; set; }
        public virtual DbSet<HoaDon> HoaDons { get; set; }
        public virtual DbSet<KhachHang> KhachHangs { get; set; }
        public virtual DbSet<KhachSan> KhachSans { get; set; }
        public virtual DbSet<LoaiDichVu> LoaiDichVus { get; set; }
        public virtual DbSet<LoaiDoiTac> LoaiDoiTacs { get; set; }
        public virtual DbSet<LoaiKhachHang> LoaiKhachHangs { get; set; }
        public virtual DbSet<LoaiPhong> LoaiPhongs { get; set; }
        public virtual DbSet<PhanQuyen> PhanQuyens { get; set; }
        public virtual DbSet<Phong> Phongs { get; set; }
        public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }
        public virtual DbSet<ThueXe> ThueXes { get; set; }
        public virtual DbSet<TinhTrang> TinhTrangs { get; set; }
        public virtual DbSet<TourDuLich> TourDuLiches { get; set; }
        public virtual DbSet<VeMayBay> VeMayBays { get; set; }
        public virtual DbSet<VeXeKhach> VeXeKhaches { get; set; }
        public virtual DbSet<voucher> vouchers { get; set; }
    }
}
