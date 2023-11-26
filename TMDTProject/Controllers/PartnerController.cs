using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TMDTProject.Models;

namespace TMDTProject.Controllers
{
    public class PartnerController : Controller
    {
        private TMDTEntities2 db = new TMDTEntities2();

        // GET: Partner
        public ActionResult Index()
        {
            var doiTacs = db.DoiTacs.Include(d => d.LoaiDoiTac).Include(d => d.LoaiDichVu);
            return View(doiTacs.ToList());
        }

        // GET: Partner/Details/5
        public ActionResult Details(int? id)
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

        // GET: Partner/Create
        public ActionResult Create()
        {
            ViewBag.MaLoaiDT = new SelectList(db.LoaiDoiTacs, "MaLoaiDT", "TenLoaiDT");
            ViewBag.MaLoaiDV = new SelectList(db.LoaiDichVus, "MaLoaiDV", "TenLoaiDV");
            return View();
        }

        // POST: Partner/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDT,MaLoaiDV,MaLoaiDT,TenDT,AnhLogo,ThongTinChiTiet,ThongTinLienHe,DiaChi")] DoiTac doiTac)
        {
            if (ModelState.IsValid)
            {
                db.DoiTacs.Add(doiTac);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaLoaiDT = new SelectList(db.LoaiDoiTacs, "MaLoaiDT", "TenLoaiDT", doiTac.MaLoaiDT);
            ViewBag.MaLoaiDV = new SelectList(db.LoaiDichVus, "MaLoaiDV", "TenLoaiDV", doiTac.MaLoaiDV);
            return View(doiTac);
        }

        // GET: Partner/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Partner/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDT,MaLoaiDV,MaLoaiDT,TenDT,AnhLogo,ThongTinChiTiet,ThongTinLienHe,DiaChi")] DoiTac doiTac)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doiTac).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaLoaiDT = new SelectList(db.LoaiDoiTacs, "MaLoaiDT", "TenLoaiDT", doiTac.MaLoaiDT);
            ViewBag.MaLoaiDV = new SelectList(db.LoaiDichVus, "MaLoaiDV", "TenLoaiDV", doiTac.MaLoaiDV);
            return View(doiTac);
        }

        // GET: Partner/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Partner/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DoiTac doiTac = db.DoiTacs.Find(id);
            db.DoiTacs.Remove(doiTac);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Register()
        {
            return View();
        }

        // POST: Customer/Register
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
                return RedirectToAction("Login", "Partner");
            }

            return View(partner);
        }

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
