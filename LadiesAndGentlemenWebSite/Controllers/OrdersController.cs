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
    public class OrdersController : Controller
    {
        private readonly LadiesAndGentlemenWebSiteContext _context;

        public OrdersController(LadiesAndGentlemenWebSiteContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            {
                if (HttpContext.Session.GetString("cart") == null)
                {
                    return View(await _context.Product.ToListAsync());
                }
                else
                {
                    string productId = HttpContext.Session.GetString("cart");
                    string[] ids = productId.Split(',');
                    int[] myInts = ids.Select(int.Parse).ToArray();
                    var purchased = from p in _context.Product
                                    where myInts.Any(s => s == p.Id)
                                    select p;
                    var leftItems = _context.Product.Except(purchased);
                    return View(await leftItems.ToListAsync());
                }

            }
        }
        //public async Task<IActionResult> MyOrder()
        //{
        //    return View();
        //}
        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.Include(x => x.Carts).ThenInclude(x => x.Product)
               
                .FirstOrDefaultAsync(m => m.Id == id);
            order = await _context.Order.Include(x => x.Client).ThenInclude(x=>x.Address).FirstOrDefaultAsync(m => m.Id == id);
            
            if (order == null)
            {
                return NotFound();
            }
            
            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Sum")] Order order)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(order);
            //    await _context.SaveChangesAsync();

            //    string cart = HttpContext.Session.GetString("Cart");
            //    string[] productIds = cart.Split(",", StringSplitOptions.RemoveEmptyEntries);

            //    foreach (var id in productIds)
            //    {
            //        ProductsOrder po = new ProductsOrder();
            //        po.ProductId = int.Parse(id);
            //        po.OrderId = order.Id;
            //        _context.Add(po);
            //        await _context.SaveChangesAsync();
            //    }

            //    HttpContext.Session.SetString("Cart", "");

            //    return RedirectToAction(nameof(Index));
            //}
            //return View(order);
            if (ModelState.IsValid)
            {
                Order myOrder = new Order();
                _context.Add(order);
                await _context.SaveChangesAsync();
                string productId = HttpContext.Session.GetString("cart");
                string[] ids = productId.Split(',');
                int[] myInts = ids.Select(int.Parse).ToArray();
                foreach (var id in myInts)
                {
                    Cart c = new Cart();
                    c.ProductId = id;
                    c.OrderId = order.Id;
                    _context.Add(c);
                    await _context.SaveChangesAsync();
                }
                HttpContext.Session.SetString("Cart", "");
                string clientId=HttpContext.Session.GetString("clientId");
                int cId = Int32.Parse(clientId);
                var myClient = from clients in _context.Client
                               where cId == clients.Id
                               select clients;
                order.Client = myClient.First();
                float total = Int32.Parse(@HttpContext.Session.GetString("price"));
                order.Sum = total;
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = order.Id });
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Sum")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.FindAsync(id);
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.Id == id);
        }
    }
}
