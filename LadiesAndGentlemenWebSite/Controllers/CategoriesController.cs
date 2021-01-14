using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LadiesAndGentlemenWebSite.Data;
using LadiesAndGentlemenWebSite.Models;
using Microsoft.AspNetCore.Http;

namespace LadiesAndGentlemenWebSite.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly LadiesAndGentlemenWebSiteContext _context;

        public CategoriesController(LadiesAndGentlemenWebSiteContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("FirstName") != "L&G1234")
                return RedirectToAction("Login", "Clients");
            var ladiesAndGentlemenContext = _context.Category.Include(c => c.SubCategory);
            return View(await ladiesAndGentlemenContext.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id, int? id2)
        {
            if (HttpContext.Session.GetString("FirstName") != "L&G1234")
                return RedirectToAction("Login", "Clients");
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            var p = from product in _context.Product
                    where product.Category.SubCategory.Id == id
                    select product;

            if (id2 > 0)
            {
                p = p.Where(x => x.Category.Id == id2);
            }

            ViewData["products"] = await p.ToListAsync();

            return View(category);
        }


        // GET: Categories/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("FirstName") != "L&G1234")
                return RedirectToAction("Login", "Clients");
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Name");
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,SubCategoryId")] Category category)
        {
            if (HttpContext.Session.GetString("FirstName") != "L&G1234")
                return RedirectToAction("Login", "Clients");
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Name", category.SubCategoryId);
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("FirstName") != "L&G1234")
                return RedirectToAction("Login", "Clients");
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Name", category.SubCategoryId);
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,SubCategoryId")] Category category)
        {
            if (HttpContext.Session.GetString("FirstName") != "L&G1234")
                return RedirectToAction("Login", "Clients");
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Name", category.SubCategoryId);
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("FirstName") != "L&G1234")
                return RedirectToAction("Login", "Clients");
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .Include(c => c.SubCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("FirstName") != "L&G1234")
                return RedirectToAction("Login", "Clients");
            var category = await _context.Category.FindAsync(id);
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }
    }
}
