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
    public class ProductsController : Controller
    {
        private readonly LadiesAndGentlemenWebSiteContext _context;

        public ProductsController(LadiesAndGentlemenWebSiteContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Details1(int? id, int? id2)
        {
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
            return View(await p.ToListAsync());
            //ViewData["products"] = await p.ToListAsync();

            //return View(category);
        }


        public async Task<IActionResult> Search(String n)
        {

            int z = 0;
            if (int.TryParse(n, out z))
            {

                var p = from product in _context.Product
                        where product.price <= z
                        orderby product.price descending
                        select product;
                return View(await p.ToListAsync());

            }
            else
            {
                var p = from product in _context.Product
                        where product.Description.Contains(n)
                        select product;
                return View(await p.ToListAsync());
            }
        }



        // GET: Products
        public async Task<IActionResult> Index()
        {
            
            return View(await _context.Product.ToListAsync());

        }



        // GET: cart
        public async Task<IActionResult> Cart(int Id)
        {

            if (HttpContext.Session.GetString("cart") == null)
            {
                HttpContext.Session.SetString("sum", "1");
                string myString = Id.ToString();
                HttpContext.Session.SetString("cart", myString);

                var purchased = from p in _context.Product
                                where Id == p.Id
                                select p;
                string price = purchased.First().price.ToString();
                HttpContext.Session.SetString("price", price);
                return View(await purchased.ToListAsync());
            }
            else
            {
                string productId = HttpContext.Session.GetString("cart");
                productId += ",";
                productId += Id;
                HttpContext.Session.SetString("cart", productId);
                string[] ids = productId.Split(',');
                int[] myInts = ids.Select(int.Parse).ToArray();
                string sum = HttpContext.Session.GetString("sum");
                int x = Int32.Parse(sum);
                x= myInts.Count();
                string updateSum = x.ToString();
                HttpContext.Session.SetString("sum", updateSum);
                var c = from p in _context.Product
                        where myInts.Contains(p.Id)
                        select p;
                float finalPrice = 0;
                HttpContext.Session.SetString("price", "0");
                foreach (var Product in c)
                    finalPrice += Product.price;
                string price = finalPrice.ToString();
                HttpContext.Session.SetString("price", price);
                return View(await c.ToListAsync());
            }

        }
        // GET: cart
        public async Task<IActionResult> RemoveFromCart(int Id)
        {
            string myString = HttpContext.Session.GetString("cart");
            string[] ids = myString.Split(',');
            int[] myInts = new int[100];
            myInts= ids.Select(int.Parse).ToArray();

            if (myInts.Count()==1)
            {
                HttpContext.Session.SetString("cart", "");
                HttpContext.Session.SetString("price", "0");
                HttpContext.Session.SetString("sum", "0");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                string MyString = HttpContext.Session.GetString("cart");
                string[] Ids = myString.Split(',');
                int[] MyInts = Ids.Select(int.Parse).ToArray();
                MyInts = MyInts.Where(val => val != Id).ToArray();
                string updated = MyInts.ToString();
                HttpContext.Session.SetString("cart", updated);
                string sum = HttpContext.Session.GetString("sum");
                int x = Int32.Parse(sum);
                x = MyInts.Count();
                string updateSum = x.ToString();
                HttpContext.Session.SetString("sum", updateSum);
                string productId = HttpContext.Session.GetString("cart");
                HttpContext.Session.SetString("cart", productId);
                string[] idsList = productId.Split(',');
                int[] myIntsList = ids.Select(int.Parse).ToArray();
                var c = from p in _context.Product
                        where myIntsList.Contains(p.Id)
                        select p;
                float finalPrice = 0;
                foreach (var Product in c)
                    finalPrice += Product.price;
                string price = finalPrice.ToString();
                HttpContext.Session.SetString("price", price);
                return View(await c.ToListAsync());
            }
           
        }


        public async Task<IActionResult> Search()
        {

            return View(await _context.Product.ToListAsync());
        }

        [HttpPost]

        public async Task<IActionResult> Search(float pricee, String Name)
        {

            var p2 = from product in _context.Product
                     where product.price < pricee || product.Description.Contains(Name)
                     orderby product.price descending
                     select product;

            return View(await p2.ToListAsync());
        }



        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("FirstName") != "L&G1234")
                return RedirectToAction("Login", "Clients");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Image,Description,price")] Product product)
        {
            if (HttpContext.Session.GetString("FirstName") != "L&G1234")
                return RedirectToAction("Login", "Clients");
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }


        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("FirstName") != "L&G1234")
                return RedirectToAction("Login", "Clients");
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Image,Description,price")] Product product)
        {
            if (HttpContext.Session.GetString("FirstName") != "L&G1234")
                return RedirectToAction("Login", "Clients");
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("FirstName") != "L&G1234")
                return RedirectToAction("Login", "Clients");
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
