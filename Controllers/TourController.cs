using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using projecttour.Models;

namespace projecttour.Controllers
{
    public class TourController : Controller
    {
        private readonly DatabaseContext _context;
        public INotyfService _notyfService { get; }
        public TourController(DatabaseContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        [Route("shop.html", Name = "ShopTour")]
        public IActionResult Index(int? page)
        {
            try
            {
                var pageNumber = page == null || page <= 0 ? 1 : page.Value;
                var pageSize = 12;
                var lsTinDangs = _context.Tours
                    .OrderBy(x => x.CategoryTourId);
                PagedList<Tour> models = new PagedList<Tour>(lsTinDangs, pageNumber, pageSize);

                ViewBag.CurrentPage = pageNumber;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [Route("danhmuc/{categoryTourId}", Name = "ListTour")]
        public IActionResult List(int categoryTourId, int page = 1)
        {
            try
            {
                var pageSize = 12;
                var danhmuc = _context.CategoryTours.AsNoTracking().SingleOrDefault(x => x.CategoryTourId == categoryTourId);

                var lsTinDangs = _context.Tours
                    .Where(x => x.CategoryTourName == danhmuc.CategoryTourName)
                    .OrderByDescending(x => x.DepartureDate);
                PagedList<Tour> models = new PagedList<Tour>(lsTinDangs, page, pageSize);

                ViewBag.CurrentPage = page;
                ViewBag.CurrentCat = danhmuc;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [Route("/{CategoryTourName}-{id}.html", Name = "TourDetails")]
        public IActionResult Details(int id)
        {
            try
            {
                var product = _context.Tours.Include(x => x.CategoryTour).FirstOrDefault(x => x.TourId == id);
                if (product == null)
                {
                    return RedirectToAction("Index");
                }
                var lsProduct = _context.Tours
                    .Where(x => x.CategoryTourName == product.CategoryTourName && x.TourId != id && x.Status == 1)
                    .Take(4)
                    .OrderByDescending(x => x.DepartureDate)
                    .ToList();

                ViewBag.SanPham = lsProduct;
                return View(product);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
