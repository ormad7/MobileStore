using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MobileStore.Data;
using MobileStore.Models;

namespace MobileStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly MobileStoreContext _context;

        public ProductsController(MobileStoreContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            if (User?.Identity != null && User.Identity.IsAuthenticated)
            {
                ViewData["login"] = User.Identity.Name;
            }
            else ViewData["login"] = "guest";
            return View(await _context.Product.ToListAsync());
        }


        public async Task<IActionResult> Search(string name)
        {
            IQueryable<Product> products = _context.Product;

            if (!string.IsNullOrEmpty(name))
                products = products.Where(p => p.Name.Contains(name) || p.Company.Contains(name));

            return View("List", await products.ToListAsync());
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

        // GET: Products/Sales
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Sales()
        {


            IQueryable<User> users = _context.User;
            IQueryable<Orders> orders = _context.Orders;

            List<User> usersList = await users.ToListAsync();

            List<Orders> ordersList = await orders.ToListAsync();

            var query = (from u in usersList
                         join o in ordersList
                         on u.Id equals o.User.Id
                         group new { u, o } by new { u.Id } into g
                         select new GroupJoin
                         {
                             UserName = g.FirstOrDefault().u.UserName,
                             TotalPrice = g.Sum(pt => pt.o.TotalPrice)
                         }).ToList();

            return View(query);
        }

        [Authorize(Roles = "Admin")]
        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult AddToCart(Product p)
        {
            if (p.Company != null)
            {
                p.Price = p.Price * p.Quantity;
                List<string> l;
                string cart = HttpContext.Session.GetString("cart");
                if (cart == null)
                {
                    l = new List<string>();
                }
                else l = cart.Split(',').ToList();
                l.Add(p.Id.ToString() + ',' + p.Company.ToString() + ',' + p.Name.ToString() + ',' + p.Quantity.ToString() + ',' + p.Price.ToString() + ',' + p.img.ToString());
                HttpContext.Session.SetString("cart", String.Join(",", l.ToArray()));
            }
            if (User?.Identity != null && User.Identity.IsAuthenticated)
            {
                ViewData["login"] = User.Identity.Name;
            }
            else ViewData["login"] = "guest";
            return View("Index");
        }

        public async Task<IActionResult> Cart()
        {
            var product = await _context.Product.ToListAsync();
            List<Product> lp = ToList();
            ViewData["products"] = lp;

            return View();
        }

        private List<Product> ToList()
        {
            List<string> l;
            string cart = HttpContext.Session.GetString("cart");
            if (cart == null)
            {
                l = new List<string>();
            }
            else l = cart.Split(',').ToList();
            List<Product> lp = new List<Product>();
            for (int i = 0; i < l.Count; i += 6)
            {
                string id = l.ElementAt(i);
                string com = l.ElementAt(i + 1);
                string name = l.ElementAt(i + 2);
                string q = l.ElementAt(i + 3);
                string pri = l.ElementAt(i + 4);
                string img = l.ElementAt(i + 5);
                Product p = new Product();
                p.Id = int.Parse(id);
                p.Name = name;
                p.Company = com;
                p.Price = double.Parse(pri);
                p.Quantity = int.Parse(q);
                p.img = img;
                lp.Add(p);
            }
            return lp;
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductType,Company,Name,Price,Date,Quantity,Size")] Product product)
        {
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductType,Company,Name,Price,Date,Quantity,Size")] Product product)
        {
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
