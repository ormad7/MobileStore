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
    public class BranchesController : Controller
    {
        private readonly MobileStoreContext _context;

        public BranchesController(MobileStoreContext context)
        {
            _context = context;
        }

        // GET: Branches
        public async Task<IActionResult> Index()
        {
            return View(await _context.Branch.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> Search(string Name, string City, string Address)
        {
            var query = from b in _context.Branch
                        where EF.Functions.Like(b.Name, "%" + Name) &&
                              EF.Functions.Like(b.City, "%" + City) &&
                              EF.Functions.Like(b.Address, "%" + Address)
                        select new { id = b.Id, name = b.Name, city = b.City, address = b.Address, telephone = b.Telephone, isSaturday = b.IsSaturday };

            return Json(await query.ToListAsync());
        }

        // GET: Branches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _context.Branch
                .FirstOrDefaultAsync(m => m.Id == id);
            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }

        // GET: Branches/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Branches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Lat,Long,Name,City,Address,Telephone,IsSaturday")] Branch branch)
        {
            if (ModelState.IsValid)
            {
                _context.Add(branch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return BadRequest();
        }

        public async Task<IActionResult> ValidateCreate(double Lat, double Long, string Name)
        {
            string strLoc = null;
            string strName = null;

            if (await _context.Branch.AnyAsync(b => b.Lat == Lat && b.Long == Long))
            {
                strLoc = "There is a branch at this location, please enter different location";
            }

            if (await _context.Branch.AnyAsync(b => b.Name == Name))
            {
                strName = "There is a branch with this name, please enter different name";
            }

            return Json(new { loc = strLoc, name = strName });
        }

        public async Task<IActionResult> ValidateEdit(int id, double Lat, double Long, string Name)
        {
            string strLoc = null;
            string strName = null;

            if (await _context.Branch.AnyAsync(b => b.Id != id && b.Lat == Lat && b.Long == Long))
            {
                strLoc = "There is a branch at this location, please enter different location";
            }

            if (await _context.Branch.AnyAsync(b => b.Id != id && b.Name == Name))
            {
                strName = "There is a branch with this name, please enter different name";
            }

            return Json(new { loc = strLoc, name = strName });
        }

        // GET: Branches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _context.Branch.FindAsync(id);
            if (branch == null)
            {
                return NotFound();
            }
            return View(branch);
        }

        // POST: Branches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Lat,Long,Name,City,Address,Telephone,IsSaturday")] Branch branch)
        {
            if (id != branch.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(branch);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BranchExists(branch.Id))
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
            return View(branch);
        }

        // GET: Branches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _context.Branch
                .FirstOrDefaultAsync(m => m.Id == id);
            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }

        // POST: Branches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var branch = await _context.Branch.FindAsync(id);
            if (branch == null)
            {
                return View("Views/Branches/NotFound.cshtml");
            }
            _context.Branch.Remove(branch);
            branch.Orders.ToList().ForEach(ord => branch.Orders.Remove(ord));
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BranchExists(int id)
        {
            return _context.Branch.Any(e => e.Id == id);
        }
    }
}
