using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Castle.Core.Resource;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PagedList.Core;
using projecttour.Helpper;
using projecttour.Models;

namespace projecttour.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ToursController : Controller
    {
        private readonly DatabaseContext _context;
        public INotyfService _notyfService { get; }
        public ToursController(DatabaseContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/Tours
        public IActionResult Index(int page = 1, int? CategoryTourId = null)
        {
            var pageNumber = page;
            var pageSize = 10;

            IQueryable<Tour> tours = _context.Tours.AsQueryable();

            if (CategoryTourId.HasValue)
            {
                tours = tours.Where(x => x.CategoryTourId == CategoryTourId.Value);
            }

            var pagedTours = tours.Include(x => x.CategoryTour)
                .OrderBy(x => x.CategoryTourId)
                .ToPagedList(pageNumber, pageSize);

            // Truyền danh sách CategoryTour vào ViewBag
            var categoryList = _context.CategoryTours
                .Select(x => new SelectListItem
                {
                    Text = x.CategoryTourName,
                    Value = x.CategoryTourId.ToString()
                })
                .ToList();

            ViewBag.CategoryTourId = CategoryTourId;
            ViewBag.CategoryTourList = new SelectList(categoryList, "Value", "Text", CategoryTourId);

            return View(pagedTours);
        }
        [HttpPost]
        public IActionResult Filter(int CategoryTourId = 0, string CategoryTourName = null)
        {
            var url = $"/Admin/Tours?CategoryTourId={CategoryTourId}&CategoryTourName={CategoryTourName}";
            if (CategoryTourId == 0)
            {
                url = "/Admin/Tours";
            }
            return Json(new { status = "success", redirectUrl = url });
        }
        [HttpGet]
        public IActionResult Search(string searchString)
        {
            try
            {
                var toursQuery = _context.Tours.Include(t => t.CategoryTour);

                if (!string.IsNullOrEmpty(searchString))
                {
                    toursQuery = toursQuery.Where(t => t.Title.Contains(searchString)).Include(t => t.CategoryTour);
                    ViewBag.SearchString = searchString;
                }

                var tours = toursQuery.ToList();

                return View("Index", tours);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }




        // GET: Admin/Tours/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tours == null)
            {
                return NotFound();
            }

            var tour = await _context.Tours
                .Include(t => t.CategoryTour)
                .FirstOrDefaultAsync(m => m.TourId == id);
            if (tour == null)
            {
                return NotFound();
            }

            return View(tour);
        }

        // GET: Admin/Tours/Create
        /*public IActionResult Create()
          {
              //ViewBag.category = new SelectList(_context.CategoryTours, "CategoryTourId", "CategoryTourName");
              //ViewBag.category = new SelectList(_context.CategoryTours, "CategoryTourId", "CategoryTourName");

              var category = _context.CategoryTours.Select(c => new SelectListItem
              {
                  Value = c.CategoryTourId.ToString(),
                  Text = c.CategoryTourName
              });

              ViewBag.Category = new SelectList(category, "Value", "Text", null, "data-CategoryTourName");

              return View();
          }



          // POST: Admin/Tours/Create
          // To protect from overposting attacks, enable the specific properties you want to bind to.
          // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
          [HttpPost]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> Create([Bind("TourId,CategoryTourId,Title,Duration,DepartureDate,Tranfer,Hotel,EmptySeat,PointOfDeparture,TourType,Price,Description,Image,CategoryTourName")] Tour tour, Microsoft.AspNetCore.Http.IFormFile fImage)
          {
              if (ModelState.IsValid)
              {
                  tour.Title = Utilities.ToTitleCase(tour.Title);
                  if (fImage != null)
                  {
                      string extension = Path.GetExtension(fImage.FileName);
                      string image = Utilities.SEOUrl(tour.Title) + extension;
                      tour.Image = await Utilities.UploadFile(fImage, @"tours", image.ToLower());
                  }
                  if (string.IsNullOrEmpty(tour.Image)) tour.Image = "default.jpg";

                  _context.Add(tour);
                  await _context.SaveChangesAsync();
                  _notyfService.Success("Thêm mới thành công");
                  return RedirectToAction(nameof(Index));
              }
              ViewBag.category = new SelectList(_context.CategoryTours, "CategoryTourId", "CategoryTourName");


              return View(tour);
          }*/
        public IActionResult Create(int? CategoryTourId = null)
        {
            var categoryList = _context.CategoryTours
               .Select(x => new SelectListItem
               {
                   Text = x.CategoryTourName,
                   Value = x.CategoryTourId.ToString()
               })
               .ToList();

            ViewBag.CategoryTourId = CategoryTourId;
            ViewBag.CategoryTourList = new SelectList(categoryList, "Value", "Text", CategoryTourId);

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TourId,CategoryTourId,Title,Duration,DepartureDate,Tranfer,Hotel,EmptySeat,PointOfDeparture,TourType,Price,Description,Image,CategoryTourName")] Tour tour, IFormFile fImage)
        {
            if (ModelState.IsValid)
            {
                tour.Title = Utilities.ToTitleCase(tour.Title);
                if (fImage != null)
                {
                    string extension = Path.GetExtension(fImage.FileName);
                    string image = Utilities.SEOUrl(tour.Title) + extension;
                    tour.Image = await Utilities.UploadFile(fImage, @"tours", image.ToLower());
                }
                if (string.IsNullOrEmpty(tour.Image)) tour.Image = "default.jpg";

                _context.Add(tour);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm mới thành công");
                return RedirectToAction(nameof(Index));
            }

            var category = _context.CategoryTours.Select(c => new SelectListItem
            {
                Value = c.CategoryTourId.ToString(),
                Text = c.CategoryTourName
            });

            ViewBag.Category = new SelectList(category, "Value", "Text", null, "data-CategoryTourName");

            return View(tour);
        }


        // GET: Admin/Tours/Edit/5
        public IActionResult Edit(int id)
        {
            var tour = _context.Tours.Find(id);
            if (id == null)
            {
                return NotFound();
            }

            var categoryList = _context.CategoryTours
                .Select(x => new SelectListItem
                {
                    Text = x.CategoryTourName,
                    Value = x.CategoryTourId.ToString(),
                    Selected = x.CategoryTourId == tour.CategoryTourId
                })
                .ToList();

            ViewBag.CategoryTourId = tour.CategoryTourId;
            ViewBag.CategoryTourList = new SelectList(categoryList, "Value", "Text");

            return View(tour);
        }


        // POST: Admin/Tours/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int tourId, [Bind("TourId,CategoryTourId,CategoryTour,Title,Duration,DepartureDate,Tranfer,Hotel,EmptySeat,PointOfDeparture,TourType,Price,Description,Image")]
 Tour tour, Microsoft.AspNetCore.Http.IFormFile fImage)
        {
            var tours = await _context.Tours.FindAsync(tourId);
            if (tourId == null)
            {
                return NotFound();
            }

            try
            {
                // Delete old image if exists.
                if (!string.IsNullOrEmpty(tour.Image) && tour.Image.StartsWith("~/images/tours"))
                {
                    string oldImagePath = Path.Combine("wwwroot", tour.Image.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Upload new image.
                string extension = Path.GetExtension(fImage.FileName);
                string image = tour.TourId.ToString() + extension;
                tour.Image = await Utilities.UploadFile(fImage, @"tours", image.ToLower());

                // Update the tour in the database.
                _context.Tours.Update(tour);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }

        // GET: Admin/Tours/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tours == null)
            {
                return NotFound();
            }

            var tour = await _context.Tours
                .Include(t => t.CategoryTour)
                .FirstOrDefaultAsync(m => m.TourId == id);
            if (tour == null)
            {
                return NotFound();
            }

            return View(tour);
        }

        // POST: Admin/Tours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tours == null)
            {
                return Problem("Entity set 'DatabaseContext.Tours'  is null.");
            }
            var tour = await _context.Tours.FindAsync(id);
            if (tour != null)
            {
                _context.Tours.Remove(tour);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TourExists(int id)
        {
            return _context.Tours.Any(e => e.TourId == id);
        }
    }
}
