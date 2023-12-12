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
    public class AdminController : Controller
    {
        private TMDT_DBEntities1 db = new TMDT_DBEntities1();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Logout()
        {
            return RedirectToAction("Index");
        }
        #region DoiTac
        public ActionResult QuanLyDoiTac() {
            var doiTacs = db.DoiTacs.Include(d => d.LoaiDoiTac).Include(d => d.LoaiDichVu);
            return View(doiTacs.ToList());
        }
        public ActionResult ThemDoiTac()
        {
            ViewBag.MaLoaiDT = new SelectList(db.LoaiDoiTacs, "MaLoaiDT", "TenLoaiDT");
            ViewBag.MaLoaiDV = new SelectList(db.LoaiDichVus, "MaLoaiDV", "TenLoaiDV");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemDoiTac([Bind(Include = "MaDT,MaLoaiDV,MaLoaiDT,TenDT,AnhLogo,ThongTinChiTiet,ThongTinLienHe,DiaChi")] DoiTac doiTac)
        {
            if (ModelState.IsValid)
            {
                db.DoiTacs.Add(doiTac);
                db.SaveChanges();
                return RedirectToAction("/QuanLyDoiTac");
            }

            ViewBag.MaLoaiDT = new SelectList(db.LoaiDoiTacs, "MaLoaiDT", "TenLoaiDT", doiTac.MaLoaiDT);
            ViewBag.MaLoaiDV = new SelectList(db.LoaiDichVus, "MaLoaiDV", "TenLoaiDV", doiTac.MaLoaiDV);
            return View(doiTac);
        }
        public ActionResult SuaDoiTac(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoiTac doiTac = db.DoiTacs.Find(id);
            if (doiTac == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoaiDT = new SelectList(db.LoaiDoiTacs, "MaLoaiDT", "TenLoaiDT", doiTac.MaLoaiDT);
            ViewBag.MaLoaiDV = new SelectList(db.LoaiDichVus, "MaLoaiDV", "TenLoaiDV", doiTac.MaLoaiDV);
            return View(doiTac);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaDoiTac([Bind(Include = "MaDT,MaLoaiDV,MaLoaiDT,TenDT,AnhLogo,ThongTinChiTiet,ThongTinLienHe,DiaChi")] DoiTac doiTac)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doiTac).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("/QuanLyDoiTac");
            }
            ViewBag.MaLoaiDT = new SelectList(db.LoaiDoiTacs, "MaLoaiDT", "TenLoaiDT", doiTac.MaLoaiDT);
            ViewBag.MaLoaiDV = new SelectList(db.LoaiDichVus, "MaLoaiDV", "TenLoaiDV", doiTac.MaLoaiDV);
            return View(doiTac);
        }
        public ActionResult XoaDoiTac(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoiTac doiTac = db.DoiTacs.Find(id);
            if (doiTac == null)
            {
                return HttpNotFound();
            }
            return View(doiTac);
        }
        [HttpPost, ActionName("XoaDoiTac")]
        [ValidateAntiForgeryToken]
        public ActionResult XoaDoiTacConfirmed(int id)
        {
            DoiTac doiTac = db.DoiTacs.Find(id);
            db.DoiTacs.Remove(doiTac);
            db.SaveChanges();
            return RedirectToAction("QuanLyDoiTac");
        }
        #endregion
        #region Voucher
        public ActionResult QuanLyVoucher()
        {
            var vouchers = db.vouchers;
            return View(vouchers.ToList());
        }
        public ActionResult ThemVoucher()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemVoucher([Bind(Include = "MaVoucher,MaCode,discount,date")] voucher voucher)
        {
            if (ModelState.IsValid)
            {
                db.vouchers.Add(voucher);
                db.SaveChanges();
                return RedirectToAction("/QuanLyVoucher");
            }

            return View(voucher);
        }
        public ActionResult SuaVoucher(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            voucher voucher = db.vouchers.Find(id);
            if (voucher == null)
            {
                return HttpNotFound();
            }
            return View(voucher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaVoucher([Bind(Include = "MaVoucher,MaCode,discount,date")] voucher voucher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(voucher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("/QuanLyVoucher");
            }
            return View(voucher);
        }
        public ActionResult XoaVoucher(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            voucher v = db.vouchers.Find(id);
            if (v == null)
            {
                return HttpNotFound();
            }
            return View(v);
        }
        [HttpPost, ActionName("XoaVoucher")]
        [ValidateAntiForgeryToken]
        public ActionResult XoaVoucherConfirmed(int id)
        {
            voucher v = db.vouchers.Find(id);
            db.vouchers.Remove(v);
            db.SaveChanges();
            return RedirectToAction("QuanLyVoucher");
        }
        #endregion
        #region KhachHang
        public ActionResult QuanLyKhachHang()
        {
            var khachHangs = db.KhachHangs.Include(d => d.LoaiKhachHang);
            return View(khachHangs.ToList());
        }
        public ActionResult ThemKhachHang()
        {
            ViewBag.MaLoaiKH = new SelectList(db.LoaiKhachHangs, "MaLoaiKH", "TenLoaiKH");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemKhachHang([Bind(Include = "MaKH,MaLoaiKH,AnhDaiDien,HoTen,GioiTinh,NgaySinh,DiaChi,Email,SDT,Facebook")] KhachHang KhachHang)
        {
            if (ModelState.IsValid)
            {
                db.KhachHangs.Add(KhachHang);
                db.SaveChanges();
                return RedirectToAction("/QuanLyKhachHang");
            }
            ViewBag.MaLoaiKH = new SelectList(db.LoaiKhachHangs, "MaLoaiKH", "TenLoaiKH");
            return View(KhachHang);
        }
        public ActionResult SuaKhachHang(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang KhachHang = db.KhachHangs.Find(id);
            if (KhachHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoaiKH = new SelectList(db.LoaiKhachHangs, "MaLoaiKH", "TenLoaiKH");
            return View(KhachHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaKhachHang([Bind(Include = "MaKH,MaLoaiKH,AnhDaiDien,HoTen,GioiTinh,NgaySinh,DiaChi,Email,SDT,Facebook")] KhachHang KhachHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(KhachHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("/QuanLyKhachHang");
            }
            ViewBag.MaLoaiKH = new SelectList(db.LoaiKhachHangs, "MaLoaiKH", "TenLoaiKH");
            return View(KhachHang);
        }
        public ActionResult XoaKhachHang(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang v = db.KhachHangs.Find(id);
            if (v == null)
            {
                return HttpNotFound();
            }
            return View(v);
        }
        [HttpPost, ActionName("XoaKhachHang")]
        [ValidateAntiForgeryToken]
        public ActionResult XoaKhachHangConfirmed(int id)
        {
            KhachHang v = db.KhachHangs.Find(id);
            db.KhachHangs.Remove(v);
            db.SaveChanges();
            return RedirectToAction("QuanLyKhachHang");
        }
        #endregion
        #region TinTuc
        public ActionResult QuanLyTinTuc()
        {
            var tinTucs = db.TinTucs.Include(d=> d.KhachHang).Include(d=>d.DoiTac).Include(d => d.DanhMuc).Include(d=>d.TaiKhoan);
            return View(tinTucs.ToList());
        }
        public ActionResult ThemTinTuc()
        {
            ViewBag.MaDoiTac = new SelectList(db.DoiTacs, "MaDT", "TenDT");
            ViewBag.MaKhachHang = new SelectList(db.KhachHangs, "MaKH", "HoTen");
            ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc");
            ViewBag.TacGia = new SelectList(db.TaiKhoans, "MaTK", "TenDN");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemTinTuc([Bind(Include = "MaTinTuc,TieuDe,NoiDung,MaTK.NgayDang,MaKH,MaDT,MaDanhMuc")] TinTuc tin)
        {
            if (ModelState.IsValid)
            {
                db.TinTucs.Add(tin);
                db.SaveChanges();
                return RedirectToAction("/QuanLyTinTuc");
            }
            ViewBag.MaDoiTac = new SelectList(db.DoiTacs, "MaDT", "TenDT");
            ViewBag.MaKhachHang = new SelectList(db.KhachHangs, "MaKH", "HoTen");
            ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc");
            ViewBag.TacGia = new SelectList(db.TaiKhoans, "MaTK", "TenDN");
            return View(tin);
        }
        public ActionResult SuaTinTuc(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TinTuc tin = db.TinTucs.Find(id);
            if (tin == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaDoiTac = new SelectList(db.DoiTacs, "MaDT", "TenDT");
            ViewBag.MaKhachHang = new SelectList(db.KhachHangs, "MaKH", "HoTen");
            ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc");
            ViewBag.TacGia = new SelectList(db.TaiKhoans, "MaTK", "TenDN");
            return View(tin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaTinTuc([Bind(Include = "MaTinTuc,TieuDe,NoiDung,MaTK.NgayDang,MaKH,MaDT,MaDanhMuc")] TinTuc tin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("/QuanLyTinTuc");
            }
            ViewBag.MaDoiTac = new SelectList(db.DoiTacs, "MaDT", "TenDT");
            ViewBag.MaKhachHang = new SelectList(db.KhachHangs, "MaKH", "HoTen");
            ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc");
            ViewBag.TacGia = new SelectList(db.TaiKhoans, "MaTK", "TenDN");
            return View(tin);
        }
        public ActionResult XoaTinTuc(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TinTuc tin = db.TinTucs.Find(id);
            if (tin == null)
            {
                return HttpNotFound();
            }
            return View(tin);
        }
        [HttpPost, ActionName("XoaTinTuc")]
        [ValidateAntiForgeryToken]
        public ActionResult XoaTinTuc(int id)
        {
            TinTuc tinTuc = db.TinTucs.Find(id);
            db.TinTucs.Remove(tinTuc);
            db.SaveChanges();
            return RedirectToAction("QuanLyTinTuc");
        }
        #endregion
        #region QuanLyDichVu
        public ActionResult QuanLyVeMayBay()
        {
            var veMayBays = db.DichVus.Include(d => d.DoiTac).Include(d => d.DoiTac.LoaiDichVu).Where(d => d.LoaiDV == 1);
            return View(veMayBays.ToList());
        }

        public ActionResult ThemVeMayBay()
        {
            ViewBag.MaDT = new SelectList(db.DoiTacs, "MaDT", "TenDT");
            ViewBag.MaTT = new SelectList(db.TinhTrangs, "MaTT", "TenTT");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemVeMayBay([Bind(Include = "MaDV,TenDV,LoaiDV,MaDT,Gia,ChiTiet,MaTT,Anh")] DichVu ve)
        {
            if (ModelState.IsValid)
            {
                ve.LoaiDV = 1;
                db.DichVus.Add(ve);
                db.SaveChanges();
                return RedirectToAction("/QuanLyVeMayBay");
            }
            ViewBag.MaDT = new SelectList(db.DoiTacs, "MaDT", "TenDT");
            ViewBag.MaTT = new SelectList(db.TinhTrangs, "MaTT", "TenTT");
            return View(ve);
        }
        //public ActionResult SuaVeMayBay(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    VeMayBay ve = db.VeMayBays.Find(id);
        //    if (ve == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.MaDT = new SelectList(db.DoiTacs, "MaDT", "TenDT");
        //    ViewBag.MaTT = new SelectList(db.TinhTrangs, "MaTT", "TenTT");
        //    return View(ve);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult SuaVeMayBay([Bind(Include = "MaVeMB,MaDT,TenVe,Anh,Gia,ChiTiet,MaTT")] VeMayBay ve)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(ve).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("/QuanLyVeMayBay");
        //    }
        //    ViewBag.MaDT = new SelectList(db.DoiTacs, "MaDT", "TenDT");
        //    ViewBag.MaTT = new SelectList(db.TinhTrangs, "MaTT", "TenTT");
        //    return View(ve);
        //}
        //public ActionResult XoaVeMayBay(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    VeMayBay ve = db.VeMayBays.Find(id);
        //    if (ve == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(ve);
        //}

        //[HttpPost, ActionName("XoaVeMayBay")]
        //[ValidateAntiForgeryToken]
        //public ActionResult XacNhanXoaVeMayBay(int id)
        //{
        //    VeMayBay ve = db.VeMayBays.Find(id);
        //    db.VeMayBays.Remove(ve);
        //    db.SaveChanges();
        //    return RedirectToAction("QuanLyVeMayBay");
        //}

        public ActionResult QuanLyVeXeKhach()
        {
            var veXeKhaches = db.DichVus.Include(d => d.DoiTac).Include(d => d.DoiTac.LoaiDichVu).Where(d => d.LoaiDV == 4);
            return View(veXeKhaches.ToList());
        }
        public ActionResult QuanLyTourDuLich()
        {
            var tourDuLiches = db.DichVus.Include(d => d.DoiTac).Include(d => d.DoiTac.LoaiDichVu).Where(d=>d.LoaiDV==5);
            return View(tourDuLiches.ToList());
        }
        public ActionResult QuanLyThueXe()
        {
            var thueXes = db.DichVus.Include(d => d.DoiTac).Include(d => d.DoiTac.LoaiDichVu).Where(d => d.LoaiDV == 3);
            return View(thueXes.ToList());
        }
        public ActionResult QuanLyKhachSan()
        {
            var khachSans = db.DichVus.Include(d => d.DoiTac).Include(d => d.DoiTac.LoaiDichVu).Where(d => d.LoaiDV == 2);
            return View(khachSans.ToList());
        }
        #endregion
        #region QuanLyDonHang
        public ActionResult QuanLyDonDat()
        {
            var don = db.DonDats.Include(d => d.KhachHang);
            return View(don.ToList());
        }
        public ActionResult ThemDonDat()
        {
            ViewBag.MaKhachHang = new SelectList(db.KhachHangs, "MaKH", "HoTen");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemDonDat([Bind(Include = "MaDon,MaKH,NgayDat,SoKhach")] DonDat don)
        {
            if (ModelState.IsValid)
            {
                db.DonDats.Add(don);
                db.SaveChanges();
                return RedirectToAction("/QuanLyDonDat");
            }
            ViewBag.MaKhachHang = new SelectList(db.KhachHangs, "MaKH", "HoTen");
            return View(don);
        }
        #endregion
        #region QuanLyBinhLuan
        public ActionResult QuanLyBinhLuan()
        {
            var bl = db.BinhLuans;
            return View(bl.ToList());
        }
        #endregion
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}