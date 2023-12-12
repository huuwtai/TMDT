using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TMDTProject.Models;
using System.Drawing;

namespace TMDTProject.Controllers
{
    public class CustomerController : Controller
    {
        TMDT_DBEntities1 db = new TMDT_DBEntities1();

        // GET: Customer
        public ActionResult Index()
        {
            var tourDuLiches = db.DichVus.Include(d => d.DoiTac).Include(d => d.DoiTac.LoaiDichVu).Where(d=>d.LoaiDV==5);
            return View(tourDuLiches.ToList());
        }
        public ActionResult Register()
        {
            return View();
        }

        // POST: Customer/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(KhachHang customer)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem tên đăng nhập đã tồn tại chưa
                if (db.TaiKhoans.Any(t => t.TenDN == customer.TaiKhoan.TenDN))
                {
                    ModelState.AddModelError("TaiKhoan.TenDN", "Tên đăng nhập đã tồn tại.");
                    return View(customer);
                }
                // Kiểm tra xem MaKH đã tồn tại chưa
                int maxMaKH = db.KhachHangs.Max(kh => kh.MaKH);
                int newMaKH = maxMaKH + 1;
                int maxMaTK = db.TaiKhoans.Max(kh => kh.MaTK);
                int newMaTK = maxMaTK + 1;
                // Thực hiện thêm mã khách hàng vào cơ sở dữ liệu
                customer.MaKH = newMaKH; 
                customer.TaiKhoan.MaTK = newMaTK;
                customer.MaLoaiKH = 1;
                customer.TaiKhoan.MaPQ = 3; // Gán mã phân quyền cho khách hàng (3 là mã phân quyền của khách hàng)
                
                db.KhachHangs.Add(customer);
                db.SaveChanges();

                // Chuyển hướng đến trang đăng nhập sau khi đăng ký thành công
                return RedirectToAction("Login", "Customer");
            }

            return View(customer);
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string TenDN,string MatKhau)
        {
            if (ModelState.IsValid)
            {
                var user = db.TaiKhoans.FirstOrDefault(u => u.TenDN == TenDN && u.MatKhau == MatKhau);
                if (user != null)
                {
                    if (user.MaPQ == 1)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (user.MaPQ == 2)
                    {
                        int maDT = db.DoiTacs.FirstOrDefault(dt => dt.TaiKhoan.TenDN == TenDN).MaDT;
                        Session["MaDT"] = maDT;
                        return RedirectToAction("Index", "Partner");
                    }
                    else if (user.MaPQ == 3)
                    {
                        int maKH = db.KhachHangs.FirstOrDefault(kh => kh.TaiKhoan.TenDN == TenDN).MaKH; // Lấy mã khách hàng từ cơ sở dữ liệu
                        Session["MaKH"] = maKH; // Lưu mã khách hàng vào Session
                        return RedirectToAction("Index", "Customer");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
        public ActionResult BookServices(int id)
        {
            if (Session["MaKH"] == null)
            {
                return RedirectToAction("Login");
            }

            int maKH = (int)Session["MaKH"]; // Lấy mã khách hàng từ Session

            // Truy vấn cơ sở dữ liệu để lấy thông tin khách hàng từ mã khách hàng
            var khachHang = db.KhachHangs.Find(maKH);

            if (khachHang == null)
            {
                return HttpNotFound(); // Xử lý khi không tìm thấy thông tin khách hàng
            }

            ViewBag.TenKhachHang = khachHang.HoTen; // Sử dụng ViewBag để truyền thông tin khách hàng qua view
            var tour = db.DichVus.Find(id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            Session["MaTour"] = tour.MaDV;
            ViewBag.Tour = tour;
            return View();
        }

        [HttpPost]
        public ActionResult BookServices(DateTime ngayDat, int soKhach)
        {
            if (Session["MaKH"] == null)
            {
                return RedirectToAction("Login");
            }
            int maKH = (int)Session["MaKH"]; // Lấy mã khách hàng từ Session
                                             // Kiểm tra xem tour có sẵn và khách hàng có tồn tại không
            int matour = (int)Session["MaTour"];      

            var tour = db.DichVus.Find(matour);
            var customer = db.KhachHangs.Find(maKH);
            int maxMaDon = db.DonDats.Any() ? db.DonDats.Max(d => d.MaDon) : 0;
            int newMaDon = maxMaDon + 1;
            int maxMaHoaDon = db.HoaDons.Any() ? db.HoaDons.Max(d => d.MaHD) : 0;
            int newMaHoaDon = maxMaHoaDon + 1;
            int maxCT = db.ChiTietHoaDons.Any() ? db.ChiTietHoaDons.Max(d => d.MaCT) : 0;
            int newCT = maxCT + 1;
            if (tour == null || customer == null)
            {
                return HttpNotFound(); // Trả về lỗi 404 nếu tour hoặc khách hàng không tồn tại
            }

            // Tạo đơn đặt tour mới
            DonDat donDat = new DonDat
            {
                MaDon = newMaDon,
                MaKH = maKH,
                NgayDat = ngayDat,
                SoKhach = soKhach
            };
            db.DonDats.Add(donDat);
            db.SaveChanges();

            // Tạo hóa đơn cho đơn đặt tour
            HoaDon hoaDon = new HoaDon
            {
                MaHD = newMaHoaDon,
                MaKH = maKH,
                MaDon = donDat.MaDon,
                GiaDV = tour.Gia,
                // Tính giá phụ thu
                GiaPhuThu = tour.Gia * 0.1m,// 10% của giá tour
                GiaHoaHong = tour.Gia * 0.1m,// 10% của giá tour
                TongTien = tour.Gia + (tour.Gia * 0.1m) +(tour.Gia * 0.1m),
                // Các thông tin khác của hóa đơn
                Ngay = DateTime.Now,
                TinhTrangHD = "Chưa thanh toán" // Giả sử mặc định là chưa thanh toán
            };
            db.HoaDons.Add(hoaDon);
            db.SaveChanges();
            ChiTietHoaDon chitiet = new ChiTietHoaDon
            {
                MaCT = newCT,
                MaDon = hoaDon.MaDon,
                MaDV = matour,
                ChiTiet = ""
            };
            db.ChiTietHoaDons.Add(chitiet);
            db.SaveChanges();
            return RedirectToAction("Index", "Home"); // Chuyển hướng về trang chủ sau khi đặt tour thành công
        }
        public ActionResult ViewInformation()
        {
            if (Session["MaKH"] == null)
            {
                return RedirectToAction("Login");
            }
            int maKH = (int)Session["MaKH"]; // Lấy mã khách hàng từ Session
            var khachHang = db.KhachHangs.Find(maKH); // Tìm thông tin khách hàng từ mã khách hàng
            if (khachHang == null)
            {
                return HttpNotFound(); // Xử lý khi không tìm thấy thông tin khách hàng
            }
            return View(khachHang);
        }
        public ActionResult ViewOrderList()
        {
            if (Session["MaKH"] == null)
            {
                return RedirectToAction("Login");
            }
            int maKH = (int)Session["MaKH"]; // Lấy mã khách hàng từ Session
            var donDatList = db.DonDats.Where(d => d.MaKH == maKH).ToList(); // Lấy danh sách đơn đã đặt của khách hàng

            // Tạo một Dictionary hoặc List chứa thông tin tình trạng hóa đơn
            Dictionary<int, string> tinhTrangDict = new Dictionary<int, string>();
            foreach (var donDat in donDatList)
            {
                var hoaDon = db.HoaDons.FirstOrDefault(h => h.MaDon == donDat.MaDon);
                if (hoaDon != null)
                {
                    tinhTrangDict.Add(donDat.MaDon, hoaDon.TinhTrangHD);
                }
                else
                {
                    tinhTrangDict.Add(donDat.MaDon, "Chưa có hóa đơn");
                }
            }

            ViewBag.TinhTrangHoaDon = tinhTrangDict;

            return View(donDatList);
        }

        public ActionResult CancelBook(int id)
        {
            var donDat = db.DonDats.Find(id); // Tìm đơn đặt tour từ mã đơn
            if (donDat == null)
            {
                return HttpNotFound(); // Xử lý khi không tìm thấy đơn đặt tour
            }
            return View(donDat);
        }
        [HttpPost]
        public ActionResult CancelBookConfirmed(int id)
        {
            var donDat = db.DonDats.Find(id); // Tìm đơn đặt tour từ mã đơn
            if (donDat == null)
            {
                return HttpNotFound(); // Xử lý khi không tìm thấy đơn đặt tour
            }

            // Lấy danh sách hóa đơn liên quan đến đơn đặt tour
            var relatedHoaDons = db.HoaDons.Where(hd => hd.MaDon == id).ToList();

            // Xóa các hóa đơn liên quan
            foreach (var hoaDon in relatedHoaDons)
            {
                db.HoaDons.Remove(hoaDon);
            }

            // Xóa đơn đặt tour
            db.DonDats.Remove(donDat);
            db.SaveChanges();

            return RedirectToAction("ViewHistory"); // Chuyển hướng về trang lịch sử đặt tour sau khi hủy đặt tour thành công
        }

        public ActionResult ViewHistory() {
            if (Session["MaKH"] == null)
            {
                return RedirectToAction("Login");
            }
            int maKH = (int)Session["MaKH"]; // Lấy mã khách hàng từ Session
            var donDats = db.DonDats.Where(d => d.MaKH == maKH).ToList(); // Lấy danh sách đơn đặt tour của khách hàng
            return View(donDats);
        }
        public ActionResult ViewOrder(int id) {
            var hoaDon = db.HoaDons.Find(id); // Tìm hóa đơn từ mã hóa đơn
            if (hoaDon == null)
            {
                return HttpNotFound(); // Xử lý khi không tìm thấy hóa đơn
            }
            // Lấy thông tin khách hàng từ hoaDon
            var khachHang = db.KhachHangs.Find(hoaDon.MaKH);

            // Truyền thông tin khách hàng vào ViewBag để sử dụng trong View
            ViewBag.KhachHang = khachHang;
            return View(hoaDon);
        }
        public ActionResult PayOrder(int id)
        {
            var hoaDon = db.HoaDons.Find(id); // Tìm hóa đơn từ mã hóa đơn
            if (hoaDon == null)
            {
                return HttpNotFound(); // Xử lý khi không tìm thấy hóa đơn
            }
            var khachHang = db.KhachHangs.Find(hoaDon.MaKH);

            // Truyền thông tin khách hàng vào ViewBag để sử dụng trong View
            ViewBag.KhachHang = khachHang;
            return View(hoaDon);
        }
        [HttpPost]
        public ActionResult PayOrderConfirmed(int id)
        {
            var hoaDon = db.HoaDons.Find(id); // Tìm hóa đơn từ mã hóa đơn
            if (hoaDon == null)
            {
                return HttpNotFound(); // Xử lý khi không tìm thấy hóa đơn
            }
            hoaDon.TinhTrangHD = "Đã thanh toán"; // Cập nhật tình trạng hóa đơn thành đã thanh toán
            db.SaveChanges();
            return RedirectToAction("ViewOrder", new { id = hoaDon.MaHD }); // Chuyển hướng về trang thông tin hóa đơn sau khi thanh toán thành công
        }
        public ActionResult FavoriteList()
        {
            if (Session["MaKH"] == null)
            {
                return RedirectToAction("Login");
            }
            int maKH = (int)Session["MaKH"];

            List<YeuThich> favoriteProducts = db.YeuThiches.Where(p => p.MaKH == maKH).ToList();

            // Lấy danh sách các dịch vụ mà khách hàng yêu thích
            List<DichVu> yeuThichDichVuList = favoriteProducts.Select(p => p.DichVu).ToList();

            ViewBag.YeuThichDichVuList = yeuThichDichVuList; // Truyền danh sách dịch vụ yêu thích sang view

            return View(favoriteProducts);
        }

        public ActionResult EditProfile(int id)
        {
            if (Session["MaKH"] == null)
            {
                return RedirectToAction("Login");
            }
            int maKH = (int)Session["MaKH"];

            var khachHang = db.KhachHangs.Find(maKH);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(int id, KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingKhachHang = db.KhachHangs.Find(id);
                    if (existingKhachHang == null)
                    {
                        return HttpNotFound();
                    }

                    existingKhachHang.HoTen = khachHang.HoTen;
                    existingKhachHang.GioiTinh = khachHang.GioiTinh;
                    existingKhachHang.NgaySinh = khachHang.NgaySinh;
                    existingKhachHang.DiaChi = khachHang.DiaChi;
                    existingKhachHang.Email = khachHang.Email;
                    existingKhachHang.SDT = khachHang.SDT;
                    existingKhachHang.Facebook = khachHang.Facebook;

                    db.Entry(existingKhachHang).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Xử lý ngoại lệ và ghi log
                    ModelState.AddModelError("", "Đã xảy ra lỗi khi cập nhật thông tin khách hàng.");
                    // Ghi log lỗi ex.Message
                    return View(khachHang);
                }
            }
            else
            {
                // Trả về view với thông báo lỗi nếu dữ liệu không hợp lệ
                return View(khachHang);
            }
        }
        [HttpPost]
        public ActionResult CheckVoucher(string voucherCode, int orderId)
        {
            var voucher = db.vouchers.FirstOrDefault(v => v.MaCode == voucherCode && v.expiration_date > DateTime.Now);
            if (voucher != null)
            {
                var hoaDon = db.HoaDons.Find(orderId);
                hoaDon.TongTien -= hoaDon.TongTien * voucher.discount; // Áp dụng giảm giá từ voucher
                db.SaveChanges();
                return Json(new { success = true, newTotal = hoaDon.TongTien });
            }
            else
            {
                return Json(new { success = false, message = "Mã voucher không hợp lệ." });
            }
        }
        public ActionResult FilteredServices(string category, decimal? minPrice, decimal? maxPrice)
        {
            IQueryable<DichVu> filteredServices = db.DichVus;

            if (!string.IsNullOrEmpty(category))
            {
                filteredServices = filteredServices.Where(d => d.LoaiDichVu.TenLoaiDV == category || d.TenDV == category);

            }

            if (minPrice != null)
            {
                filteredServices = filteredServices.Where(d => d.Gia >= minPrice);
            }

            if (maxPrice != null)
            {
                filteredServices = filteredServices.Where(d => d.Gia <= maxPrice);
            }

            ViewBag.CurrentCategory = category;
            ViewBag.CurrentMinPrice = minPrice;
            ViewBag.CurrentMaxPrice = maxPrice;

            return View(filteredServices.ToList());
        }
        public ActionResult ViewService(int id)
        {
            // Lấy thông tin sản phẩm đang xem từ cơ sở dữ liệu
            var services = db.DichVus.Find(id);

            // Truy vấn cơ sở dữ liệu để lấy danh sách các sản phẩm gợi ý liên quan
            var suggestedProducts = db.DichVus.Where(p => p.LoaiDichVu.TenLoaiDV == services.LoaiDichVu.TenLoaiDV && p.MaDV != id).Take(5).ToList();

            ViewBag.SuggestedProducts = suggestedProducts; // Truyền danh sách sản phẩm gợi ý sang view

            return View(services);
        }

    }

} 