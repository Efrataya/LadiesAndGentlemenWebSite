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

        public IActionResult AddToCart(int id)
        {
            string cart = HttpContext.Session.GetString("cart");
            if (cart == null)
                cart = "";

            cart += id + ",";
            HttpContext.Session.SetString("cart", cart);
            cart = HttpContext.Session.GetString("cart");
            string[] productIds = cart.Split(",", StringSplitOptions.RemoveEmptyEntries);
            //int[] myInts = Array.ConvertAll(productIds, s => int.Parse(s));
            string sum = HttpContext.Session.GetString("sum");
            if (sum == null)
                HttpContext.Session.SetString("sum", "1");
            else
            {
                int x = Int32.Parse(sum);
                x = productIds.Count();
                string updateSum = x.ToString();
                HttpContext.Session.SetString("sum", updateSum);
            }
            return RedirectToAction("Cart");
        }

        // GET: cart
        public async Task<IActionResult> Cart()
        {
            string cart = HttpContext.Session.GetString("cart");
            var products = new List<Product>();
            if (cart != null)
            {
                string[] productIds = cart.Split(",", StringSplitOptions.RemoveEmptyEntries);
                 //int[] myInts = Array.ConvertAll(productIds, s => int.Parse(s));
                products = _context.Product.Where(x => productIds.Contains(x.Id.ToString())).ToList();
                float finalPrice = 0;
                HttpContext.Session.SetString("price", "0");
                foreach (var Product in products)
                    finalPrice += Product.price;
                string price = finalPrice.ToString();
                HttpContext.Session.SetString("price", price);
                Dictionary<string, int> dict = new Dictionary<string, int>();
                foreach (var id in productIds)
                {
                    if (dict.ContainsKey(id))
                        dict[id]++;
                    else
                        dict.Add(id, 1);
                }
                ViewData["quantity"] = dict;
                
            }

            return View(products);

        }
        // GET: cart
        public async Task<IActionResult> RemoveFromCart(int Id)
        {
            string myString = HttpContext.Session.GetString("cart");
            if (myString.ToLower().Contains(','))
            {
                string[] productIds = myString.Split(",", StringSplitOptions.RemoveEmptyEntries);
                //int[] myInts = Array.ConvertAll(productIds, s => int.Parse(s));

                productIds = productIds.Where(val => val != Id.ToString()).ToArray();
                string updated = String.Concat(productIds);
                HttpContext.Session.SetString("cart", updated);
                string sum = HttpContext.Session.GetString("sum");
                int x = Int32.Parse(sum);
                x = productIds.Count();
                string updateSum = x.ToString();
                HttpContext.Session.SetString("sum", updateSum);
                string productId = HttpContext.Session.GetString("cart");
                HttpContext.Session.SetString("cart", productId);
                //string[] idsList = productId.Split(",", StringSplitOptions.RemoveEmptyEntries);
                //int[] myIntsList = idsList.Select(int.Parse).ToArray();
                var c = from p in _context.Product
                        where productIds.Contains(p.Id.ToString())
                        select p;
                float finalPrice = 0;
                foreach (var Product in c)
                    finalPrice += Product.price;
                string price = finalPrice.ToString();
                HttpContext.Session.SetString("price", price);
                return RedirectToAction("Cart");
            }
            else
            {

                HttpContext.Session.SetString("cart", "");
                HttpContext.Session.SetString("price", "0");
                HttpContext.Session.SetString("sum", "0");
                return RedirectToAction("Index", "Home");
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
        public async Task<IActionResult> Create([Bind("Id,Image,Description,price")] Product product, int category)
        {
            if (HttpContext.Session.GetString("FirstName") != "L&G1234")
                return RedirectToAction("Login", "Clients");
            if (ModelState.IsValid)
            {
                Category c= new Category();
                c.Id = category;
                product.Category = c;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }


        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id ,int id2)
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
            await _context.SaveChangesAsync();
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
