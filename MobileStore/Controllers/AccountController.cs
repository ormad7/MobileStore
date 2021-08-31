using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MobileStore.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {

            return RedirectToAction(nameof(Login), "Users");

        }
    }
}
