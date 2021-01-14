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
    public class SubCategoriesController : Controller
    {
        private readonly LadiesAndGentlemenWebSiteContext _context;

        public SubCategoriesController(LadiesAndGentlemenWebSiteContext context)
        {
            _context = context;
        }

        // GET: SubCategories
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("FirstName") != "L&G1234")
                return RedirectToAction("Login", "Clients");
            return View(await _context.SubCategory.ToListAsync());
        }

        // GET: SubCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetString("FirstName") != "L&G1234")
                return RedirectToAction("Login", "Clients");
            if (id == null)
            {
                return NotFound();
            }

            var subCategory = await _context.SubCategory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subCategory == null)
            {
                return NotFound();
            }

            return View(subCategory);
        }

        // GET: SubCategories/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("FirstName") != "L&G1234")
                return RedirectToAction("Login", "Clients");
            return View();
        }

        // POST: SubCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] SubCategory subCategory)
        {
            if (HttpContext.Session.GetString("FirstName") != "L&G1234")
                return RedirectToAction("Login", "Clients");
            if (ModelState.IsValid)
            {
                _context.Add(subCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subCategory);
        }

        // GET: SubCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("FirstName") != "L&G1234")
                return RedirectToAction("Login", "Clients");
            if (HttpContext.Session.GetString("FirstName") != "L&G1234")
                return RedirectToAction("Login", "Clients");
            if (id == null)
            {
                return NotFound();
            }

            var subCategory = await _context.SubCategory.FindAsync(id);
            if (subCategory == null)
            {
                return NotFound();
            }
            return View(subCategory);
        }

        // POST: SubCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] SubCategory subCategory)
        {
            if (HttpContext.Session.GetString("FirstName") != "L&G1234")
                return RedirectToAction("Login", "Clients");
            if (id != subCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubCategoryExists(subCategory.Id))
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
            return View(subCategory);
        }

        // GET: SubCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("FirstName") != "L&G1234")
                return RedirectToAction("Login", "Clients");
            if (id == null)
            {
                return NotFound();
            }

            var subCategory = await _context.SubCategory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subCategory == null)
            {
                return NotFound();
            }

            return View(subCategory);
        }

        // POST: SubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("FirstName") != "L&G1234")
                return RedirectToAction("Login", "Clients");
            var subCategory = await _context.SubCategory.FindAsync(id);
            _context.SubCategory.Remove(subCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubCategoryExists(int id)
        {
            return _context.SubCategory.Any(e => e.Id == id);
        }
    }
}
