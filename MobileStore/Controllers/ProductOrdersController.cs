using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MobileStore.Data;
using MobileStore.Models;

namespace MobileStore.Controllers
{
    public class ProductOrdersController : Controller
    {
        private readonly MobileStoreContext _context;

        public ProductOrdersController(MobileStoreContext context)
        {
            _context = context;
        }

        // GET: ProductOrders
        public async Task<IActionResult> Index(int? id)
        {
            List<ProductOrder> poList = _context.ProductOrder.Where(po => po.OrderId == id).ToList();
            string d = "";
            string od = "";
            Orders o = _context.Orders.Where(o => o.Id == poList.First().OrderId).ToList().First();
            od += o.Address + " " + o.City + "," + o.DateOfOrder.Date.Day.ToString() + "/" + o.DateOfOrder.Date.Month.ToString() + "/" + o.DateOfOrder.Date.Year.ToString() + "," + o.CreditCard + "," +
                o.TotalPrice + "," + o.Zip;
            foreach (ProductOrder po in poList)
            {
                Product p = _context.Product.Where(p => p.Id == po.ProductId).ToList().First();
                d += p.Company + "," + p.Name + "," + p.Price + "," + po.Quantity.ToString() + ",";
            }
            ViewData["details"] = d;
            ViewData["order"] = od;
            return View(poList);
        }

        // GET: ProductOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productOrder = await _context.ProductOrder
                .Include(p => p.Orders)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (productOrder == null)
            {
                return NotFound();
            }

            return View(productOrder);
        }

        // GET: ProductOrders/Create
        public async Task<IActionResult> Create(string prodc)
        {
            Orders or = _context.Orders.ToList().Last();
            string[] pro = prodc.Split(",");
            for (int i = 0; i < pro.Length; i += 2)
            {
                ProductOrder po = new ProductOrder();
                po.ProductId = int.Parse(pro[i]);
                po.Quantity = int.Parse(pro[i + 1]);

                po.OrderId = or.Id;
                _context.Add(po);
                await _context.SaveChangesAsync();

            }

            return RedirectToAction(nameof(Index), new { id = or.Id });
        }

        // POST: ProductOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,OrderId")] ProductOrder productOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Address", productOrder.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name", productOrder.ProductId);
            return View(productOrder);
        }

        // GET: ProductOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productOrder = await _context.ProductOrder.FindAsync(id);
            if (productOrder == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Address", productOrder.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Discriminator", productOrder.ProductId);
            return View(productOrder);
        }

        // POST: ProductOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,OrderId")] ProductOrder productOrder)
        {
            if (id != productOrder.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductOrderExists(productOrder.ProductId))
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
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Address", productOrder.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Discriminator", productOrder.ProductId);
            return View(productOrder);
        }

        // GET: ProductOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productOrder = await _context.ProductOrder
                .Include(p => p.Orders)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (productOrder == null)
            {
                return NotFound();
            }

            return View(productOrder);
        }

        // POST: ProductOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productOrder = await _context.ProductOrder.FindAsync(id);
            _context.ProductOrder.Remove(productOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductOrderExists(int id)
        {
            return _context.ProductOrder.Any(e => e.ProductId == id);
        }
    }
}