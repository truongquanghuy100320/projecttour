using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projecttour.Models;

namespace projecttour.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SearchController : Controller
    {
        private readonly DatabaseContext _context;
        public INotyfService _notyfService { get; }
        public SearchController(DatabaseContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        [HttpPost]
        public IActionResult FindTour(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return PartialView("ListProductsSearchPartial", null);
            }

            var tours = _context.Tours
                .Include(t => t.CategoryTour)
                .Where(t => t.CategoryTourName.Contains(keyword))
                .OrderByDescending(t => t.CategoryTourName)
                .Take(10)
                .ToList();

            if (tours == null)
            {
                return PartialView("ListProductsSearchPartial", null);
            }
            else
            {
                return PartialView("ListProductsSearchPartial", tours);
            }
        }


    }
}
