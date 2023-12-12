using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using TMDTProject.Models;
using TMDTProject.Models.ViewModels;

namespace TMDTProject.Controllers
{
    public class PartnerController : Controller
    {
        private TMDT_DBEntities1 db = new TMDT_DBEntities1();

        // GET: Partner
        public ActionResult Index()
        {
            ManagerServices();
            return View();
        }
        public ActionResult ViewInformation() {
            if (Session["MaDT"] == null)
            {
                TempData["ErrorMessage"] = "Bạn chưa đăng nhập. Vui lòng đăng nhập để xem thông tin đối tác.";
                return RedirectToAction("Login", "Customer");
            }

            int maDT = (int)Session["MaDT"];
            var partner = db.DoiTacs.FirstOrDefault(dt => dt.MaDT == maDT);

            if (partner == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin đối tác.";
                return RedirectToAction("Index", "Partner");
            }

            return View(partner);
        }
        public ActionResult Register()
        {
            ViewBag.MaLoaiDV = new SelectList(db.LoaiDichVus, "MaLoaiDV", "TenLoaiDV");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(DoiTac partner)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem tên đăng nhập đã tồn tại chưa
                if (db.TaiKhoans.Any(t => t.TenDN == partner.TaiKhoan.TenDN))
                {
                    ModelState.AddModelError("TaiKhoan.TenDN", "Tên đăng nhập đã tồn tại.");
                    return View(partner);
                }
                // Kiểm tra xem MaDT đã tồn tại chưa
                int maxMaDT = db.DoiTacs.Max(kh => kh.MaDT);
                int newMaDT = maxMaDT + 1;
                int maxMaTK = db.TaiKhoans.Max(kh => kh.MaTK);
                int newMaTK = maxMaTK + 1;

                partner.MaDT = newMaDT;
                partner.TaiKhoan.MaTK = newMaTK;
                partner.MaLoaiDT = 1;
                partner.TaiKhoan.MaPQ = 2; // Gán mã phân quyền cho đối tác (2 là mã phân quyền của đối tác)

                db.DoiTacs.Add(partner);
                db.SaveChanges();

                // Chuyển hướng đến trang đăng nhập sau khi đăng ký thành công
                return RedirectToAction("Login", "Customer");
            }

            return View(partner);
        }
        public ActionResult Logout() {
            Session.Clear();
            return RedirectToAction("Login","Customer");
        }
        public ActionResult ManagerServices()
        {
            if (Session["MaDT"] == null)
            {
                TempData["ErrorMessage"] = "Bạn chưa đăng nhập. Vui lòng đăng nhập để tiếp tục.";
                return RedirectToAction("Login", "Customer");
            }
            int maDT = (int)Session["MaDT"];
            //Lấy Danh sách Các DV thuộc đối tác hiện tại
            var dichVus = db.DichVus.Include(d => d.DoiTac).Include(d => d.DoiTac.LoaiDichVu).Where(d => d.MaDT == maDT);
            return View(dichVus.ToList());
        }
        public ActionResult ManagerVoucher()
        {
            if (Session["MaDT"] == null)
            {
                return RedirectToAction("Login", "Customer");
            }
            int maDT = (int)Session["MaDT"];

            var vouchers = db.vouchers.ToList();

            return View(vouchers);
        }
        public ActionResult CreateVoucher()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateVoucher(voucher voucher)
        {
            if (ModelState.IsValid)
            {
                int maxVoucher = db.vouchers.Max(vc => vc.MaVoucher);
                int newMaVoucher = maxVoucher + 1;
                voucher.MaVoucher = newMaVoucher;
                db.vouchers.Add(voucher);
                db.SaveChanges();
                return RedirectToAction("ManagerVoucher");
            }
            return View(voucher);
        }

        public ActionResult EditVoucher(int id)
        {
            var voucher = db.vouchers.Find(id);
            if (voucher == null)
            {
                return HttpNotFound();
            }
            return View(voucher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditVoucher(voucher voucher)
        {
            if (ModelState.IsValid)
            {
                if (voucher.expiration_date == null)
                {
                    var existingVoucher = db.vouchers.AsNoTracking().FirstOrDefault(v => v.MaVoucher == voucher.MaVoucher);
                    if (existingVoucher != null)
                    {
                        voucher.expiration_date = existingVoucher.expiration_date;
                    }
                }
                db.Entry(voucher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ManagerVoucher");
            }
            return View(voucher);
        }

            public ActionResult DeleteVoucher(int id)
        {
            var voucher = db.vouchers.Find(id);
            if (voucher == null)
            {
                return HttpNotFound();
            }
            db.vouchers.Remove(voucher);
            db.SaveChanges();
            return RedirectToAction("ManagerVoucher");
        }

        public ActionResult ManagerBookServices()
        {
            if (Session["MaDT"] == null)
            {
                return RedirectToAction("Login", "Customer");
            }
            int maDT = (int)Session["MaDT"];
            var viewModel = (from don in db.DonDats
                             join dd in db.DonDats on don.MaKH equals dd.MaKH
                             join hd in db.HoaDons on dd.MaDon equals hd.MaDon
                             join cthd in db.ChiTietHoaDons on hd.MaHD equals cthd.MaDon
                             join dv in db.DichVus on cthd.MaDV equals dv.MaDV
                             join dt in db.DoiTacs on dv.MaDT equals dt.MaDT
                             where dt.MaDT == maDT
                             select new BookServicesViewModel
                             {
                                 DonDat = don,
                                 HoaDon = hd,
                                 ChiTietHoaDon = cthd,
                                 DichVu = dv,
                                 khachHang = don.KhachHang
                             }).Distinct().ToList();

            return View(viewModel);
        }





        public ActionResult ManagerRevenue()
        {
            if (Session["MaDT"] == null)
            {
                return RedirectToAction("Login", "Customer");
            }
            int maDT = (int)Session["MaDT"];
            var monthlyRevenue = db.HoaDons
                .Where(hd => db.ChiTietHoaDons.Any(ct => ct.MaDon == hd.MaDon) &&
                             db.DichVus.Any(dv => dv.MaDV == db.ChiTietHoaDons.FirstOrDefault(ct => ct.MaDon == hd.MaDon).MaDV && dv.MaDT == maDT))
                .GroupBy(hd => new {
                    Month = hd.Ngay.HasValue ? hd.Ngay.Value.Month : 0,
                    Year = hd.Ngay.HasValue ? hd.Ngay.Value.Year : 0
                })
                .Select(group => new
                {
                    Month = group.Key.Month,
                    Year = group.Key.Year,
                    TotalRevenue = group.Sum(hd => hd.GiaDV)
                })
                .ToList();
            ViewBag.MonthlyRevenue = monthlyRevenue;

            return View();
        }

        public ActionResult CreateServices()
        {
            if (Session["MaDT"] == null)
            {
                return RedirectToAction("Login", "Customer");
            }
            ViewBag.LoaiDV = new SelectList(db.LoaiDichVus, "MaLoaiDV", "TenLoaiDV");
            ViewBag.MaTT = new SelectList(db.TinhTrangs, "MaTT", "TenTT");
            return View();
        }

        [HttpPost]
        public ActionResult CreateServices(DichVu service)
        {
            if (Session["MaDT"] == null)
            {
                return RedirectToAction("Login", "Customer");
            }
            int maDT = (int)Session["MaDT"];

            if (ModelState.IsValid)
            {
                // Lấy MaLoaiDV từ đối tác
                var doiTac = db.DoiTacs.FirstOrDefault(dt => dt.MaDT == maDT);
                if (doiTac != null)
                {
                    int maLoaiDV = (int)doiTac.MaLoaiDV;

                    // Gán giá trị cho trường LoaiDV của dịch vụ
                    service.LoaiDV = maLoaiDV;
                    service.MaDT = maDT;
                    // Tiếp tục với việc lưu dịch vụ vào cơ sở dữ liệu
                    service.MaDV = db.DichVus.Max(dv => dv.MaDV) + 1;
                    db.DichVus.Add(service);
                    db.SaveChanges();

                    return RedirectToAction("ManagerServices"); // Chuyển hướng về trang quản lý dịch vụ
                }
                else
                {
                    // Xử lý khi không tìm thấy đối tác
                }
            }

            return View(service); // Nếu dữ liệu không hợp lệ, hiển thị lại form tạo mới dịch vụ
        }

        public ActionResult DeleteService(int id)
        {
            var service = db.DichVus.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }

            // Xóa các bình luận liên quan đến dịch vụ
            var comments = db.BinhLuans.Where(b => b.MaDV == id);
            foreach (var comment in comments)
            {
                db.BinhLuans.Remove(comment);
            }

            // Xóa các chi tiết hóa đơn liên quan đến dịch vụ
            var details = db.ChiTietHoaDons.Where(c => c.MaDV == id);
            foreach (var detail in details)
            {
                db.ChiTietHoaDons.Remove(detail);
            }

            // Xóa dịch vụ
            db.DichVus.Remove(service);
            db.SaveChanges();

            return RedirectToAction("ManagerServices");
        }
        public ActionResult EditService(int id)
        { 
            ViewBag.MaTT = new SelectList(db.TinhTrangs, "MaTT", "TenTT");
            var service = db.DichVus.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditService(DichVu service)
        {
            if (ModelState.IsValid)
            {
                int maDT = (int)Session["MaDT"];
                var doiTac = db.DoiTacs.FirstOrDefault(dt => dt.MaDT == maDT);
                int maLoaiDV = (int)doiTac.MaLoaiDV;
                // Gán giá trị cho trường LoaiDV của dịch vụ
                service.LoaiDV = maLoaiDV;
                service.MaDT = maDT;
                    db.Entry(service).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ManagerServices");
            }
            return View(service);
        }

        public ActionResult DetailsService(int id)
        {
            var service = db.DichVus.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }
        public ActionResult ManagerComment()
        {
            if (Session["MaDT"] == null)
            {
                return RedirectToAction("Login", "Customer");
            }
            int maDT = (int)Session["MaDT"];
            var binhLuans = from bl in db.BinhLuans
                            join dv in db.DichVus on bl.MaDV equals dv.MaDV
                            join dt in db.DoiTacs on dv.MaDT equals dt.MaDT
                            where dt.MaDT == maDT
                            select new CommentViewModel { MaBinhLuan = bl.MaBinhLuan, MaNguoiDung = bl.MaNguoiDung, NoiDung = bl.NoiDung, ThoiGian = bl.ThoiGian };

            return View(binhLuans.ToList());
        }
        public ActionResult ManagerCustomer()
        {
            if (Session["MaDT"] == null)
            {
                return RedirectToAction("Login", "Customer");
            }
            int maDT = (int)Session["MaDT"];
            var khachHangs = (from kh in db.KhachHangs
                             join dd in db.DonDats on kh.MaKH equals dd.MaKH
                             join hd in db.HoaDons on dd.MaDon equals hd.MaDon
                             join cthd in db.ChiTietHoaDons on hd.MaHD equals cthd.MaDon
                             join dv in db.DichVus on cthd.MaDV equals dv.MaDV
                             join dt in db.DoiTacs on dv.MaDT equals dt.MaDT
                             where dt.MaDT == maDT
                             select kh).Distinct().ToList();
            return View(khachHangs);
        }
        public ActionResult ChangePassword() { return View(); }
        [HttpPost]
        public ActionResult ChangePassword(string currentPassword, string newPassword)
        {
            int maDT = (int)Session["MaDT"];
            var doiTac = db.DoiTacs.FirstOrDefault(dt => dt.MaDT == maDT);
            if (string.IsNullOrEmpty(newPassword)) 
            {
                ViewBag.Error = "Mật khẩu mới không được để trống";
                return View(); 
            }
            else if (doiTac != null && doiTac.TaiKhoan.MatKhau == currentPassword)
            {
                doiTac.TaiKhoan.MatKhau = newPassword;
                db.Entry(doiTac.TaiKhoan).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Message = "Mật khẩu đã được thay đổi thành công.";
            }
            else
            {
                ViewBag.Error = "Mật khẩu hiện tại không đúng.";
            }

            return View();
        }


    }
}
