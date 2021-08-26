using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MobileStore.Data;
using MobileStore.Models;

namespace MobileStoreMarket.Controllers
{
    public class UsersController : Controller
    {
        private readonly MobileStoreContext _context;

        public UsersController(MobileStoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }


        private async void SignIn(User user)
        {
            //HttpContext.Session.SetString("Type", user.Type.ToString());

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("FirstName", user.firstName),
                    new Claim("LastName", user.LastName),
                    new Claim(ClaimTypes.Role, user.UserType.ToString()),
                    new Claim("Id",user.Id.ToString()),
                };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        public bool IsLogin()
        {
            return (User?.Identity != null && User.Identity.IsAuthenticated);
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register([Bind("Id,UserName,Password,firstName,LastName,Phone,Email,UserType")] User user)
        {

            // TO DO : Check if username like this is already exists 

            // User with the same email already exists

            user.UserType = UserType.Customer;

            bool ModelError = false;

            if (_context.User.Any(x => x.Email == user.Email))
            {
                ModelState.AddModelError("Email", "User with this email already exists");
                ModelError = true;


            }

            if (_context.User.Any(x => x.UserName == user.UserName))
            {
                ModelState.AddModelError("UserName", "This User name already exists");
                ModelError = true;

            }

            if (ModelError)
                return View(user);


            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                SignIn(user);
                return RedirectToAction(nameof(Index), "Products");
            }
            return RedirectToAction(nameof(Index), "Products");
        }


        [HttpPost]
        public IActionResult Login([Bind("Id,UserName,Password,firstName,LastName,Phone,Email,UserType")] User user)
        {
            var users = _context.User.Where(u => u.UserName == user.UserName && u.Password == user.Password).ToList();

            if (users != null && users.Count() > 0)
            {
                SignIn(users.FirstOrDefault());
                return RedirectToAction(nameof(Index), "Products");
            }

            ModelState.AddModelError("Password", "The user name or password provided is incorrect.");

            return View(user);
        }



        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }


        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            int Id = _context.User.Where(u => u.UserName == HttpContext.User.Identity.Name).FirstOrDefault().Id;

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == Id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }


        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Password,firstName,LastName,Phone,Email")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
