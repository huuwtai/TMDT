using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMDTProject.Models;

namespace TMDTProject.Controllers
{
    public class HomeController : Controller
    {
        TMDT_DBEntities1 db = new TMDT_DBEntities1();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult FilteredServices(string category, decimal? minPrice, decimal? maxPrice)
        {
            IQueryable<DichVu> filteredServices = db.DichVus;

            if (!string.IsNullOrEmpty(category))
            {
                filteredServices = filteredServices.Where(d => d.LoaiDichVu.TenLoaiDV == category || d.TenDV==category);

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