using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MobileStore.Data;
using MobileStore.Models;

namespace MobileStore.Controllers
{
    public class OrdersController : Controller
    {
        private readonly MobileStoreContext _context;

        public OrdersController(MobileStoreContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            return View(await _context.Orders.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details()
        {
            int id = int.Parse(User.Claims.FirstOrDefault(x => x.Type.Equals("Id", StringComparison.OrdinalIgnoreCase))?.Value);
            User user = _context.User.Where(u => u.Id == id).ToList().First();
            List<Orders> ord = await _context.Orders.Where(o => o.User == user).ToListAsync();
            return View(ord);
        }

        // GET: Orders/Create
        public async Task<IActionResult> Create(Orders o)
        {
            var product = await _context.Product.ToListAsync();
            List<string> l;
            string cart = HttpContext.Session.GetString("cart");
            if (cart == null)
            {
                l = new List<string>();
            }
            else l = cart.Split(',').ToList();
            List<Product> lp = new List<Product>();
            string pro = "";
            for (int i = 0; i < l.Count; i += 6)
            {
                pro += l.ElementAt(i) + "," + l.ElementAt(i + 3);
                if (i + 5 < l.Count - 1)
                    pro += ",";
            }
            o.DateOfOrder = DateTime.Now;
            string id = User.Claims.FirstOrDefault(x => x.Type.Equals("Id", StringComparison.OrdinalIgnoreCase))?.Value;
            int j = int.Parse(id);
            o.User = _context.User.Where(u => u.Id == j).ToList().First();
            _context.Add(o);
            await _context.SaveChangesAsync();
            return RedirectToAction("Create", "ProductOrders", new { prodc = pro });
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        /*public async Task<IActionResult> Create([Bind("Id,DateOfOrder,TotalPrice")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orders);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orders);
        }*/

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders.FindAsync(id);
            if (orders == null)
            {
                return NotFound();
            }
            return View(orders);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateOfOrder,TotalPrice")] Orders orders)
        {
            if (id != orders.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orders);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdersExists(orders.Id))
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
            return View(orders);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orders = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(orders);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdersExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
