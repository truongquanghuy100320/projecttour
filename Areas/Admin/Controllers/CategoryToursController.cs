using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projecttour.Models;

namespace projecttour.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryToursController : Controller
    {
        private readonly DatabaseContext _context;

        public CategoryToursController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Admin/CategoryTours
        public async Task<IActionResult> Index()
        {
              return _context.CategoryTours != null ? 
                          View(await _context.CategoryTours.ToListAsync()) :
                          Problem("Entity set 'DatabaseContext.CategoryTours'  is null.");
        }

        // GET: Admin/CategoryTours/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CategoryTours == null)
            {
                return NotFound();
            }

            var categoryTour = await _context.CategoryTours
                .FirstOrDefaultAsync(m => m.CategoryTourId == id);
            if (categoryTour == null)
            {
                return NotFound();
            }

            return View(categoryTour);
        }

        // GET: Admin/CategoryTours/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/CategoryTours/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryTourId,CategoryTourName")] CategoryTour categoryTour)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoryTour);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoryTour);
        }

        // GET: Admin/CategoryTours/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CategoryTours == null)
            {
                return NotFound();
            }

            var categoryTour = await _context.CategoryTours.FindAsync(id);
            if (categoryTour == null)
            {
                return NotFound();
            }
            return View(categoryTour);
        }

        // POST: Admin/CategoryTours/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryTourId,CategoryTourName")] CategoryTour categoryTour)
        {
            if (id != categoryTour.CategoryTourId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoryTour);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryTourExists(categoryTour.CategoryTourId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(categoryTour);
        }

        // GET: Admin/CategoryTours/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CategoryTours == null)
            {
                return NotFound();
            }

            var categoryTour = await _context.CategoryTours
                .FirstOrDefaultAsync(m => m.CategoryTourId == id);
            if (categoryTour == null)
            {
                return NotFound();
            }

            return View(categoryTour);
        }

        // POST: Admin/CategoryTours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CategoryTours == null)
            {
                return Problem("Entity set 'DatabaseContext.CategoryTours'  is null.");
            }
            var categoryTour = await _context.CategoryTours.FindAsync(id);
            if (categoryTour != null)
            {
                _context.CategoryTours.Remove(categoryTour);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryTourExists(int id)
        {
          return (_context.CategoryTours?.Any(e => e.CategoryTourId == id)).GetValueOrDefault();
        }
    }
}
