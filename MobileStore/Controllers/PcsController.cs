using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MobileStore.Data;
using MobileStore.Data;
using MobileStore.Models;

namespace MobileStoreMarket.Controllers
{
    public class PcsController : Controller
    {
        private readonly MobileStoreContext _context;

        public PcsController(MobileStoreContext context)
        {
            _context = context;
        }

        // GET: Pcs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pc.ToListAsync());
        }

        public async Task<IActionResult> Search(string name, double price)
        {
            IQueryable<Pc> pcs = _context.Pc;

            if (!string.IsNullOrEmpty(name))
                pcs = pcs.Where(p => p.Name.Contains(name) || p.Company.Contains(name));

            if (price > 0)
                pcs = pcs.Where(p => p.Price < price);

            return View("Index", await pcs.ToListAsync());
        }


        // GET: Pcs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pc = await _context.Pc
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pc == null)
            {
                return NotFound();
            }

            return View(pc);
        }

        // GET: Pcs/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pcs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Cpu,Laptop,Ram,GraphicsCard,Storage,Id,ProductType,Company,Name,Price,Date,Quantity,Size,img")] Pc pc)
        {
            pc.ProductType = "PC";
            if (ModelState.IsValid)
            {
                _context.Add(pc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pc);
        }

        // GET: Pcs/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pc = await _context.Pc.FindAsync(id);
            if (pc == null)
            {
                return NotFound();
            }
            return View(pc);
        }

        // POST: Pcs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Cpu,Laptop,Ram,GraphicsCard,Storage,Id,ProductType,Company,Name,Price,Date,Quantity,Size,img")] Pc pc)
        {
            if (id != pc.Id)
            {
                return NotFound();
            }
            pc.ProductType = "PC";
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PcExists(pc.Id))
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
            return View(pc);
        }

        // GET: Pcs/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pc = await _context.Pc
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pc == null)
            {
                return NotFound();
            }

            return View(pc);
        }

        // POST: Pcs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pc = await _context.Pc.FindAsync(id);
            _context.Pc.Remove(pc);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PcExists(int id)
        {
            return _context.Pc.Any(e => e.Id == id);
        }
    }
}
