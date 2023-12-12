using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMDTProject.Models.ViewModels
{
    public class BookServicesViewModel
    {
        public DonDat DonDat { get; set; }
        public HoaDon HoaDon { get; set; }
        public ChiTietHoaDon ChiTietHoaDon { get; set; }
        public DichVu DichVu { get; set; }
        public KhachHang khachHang { get; set; }
    }
}