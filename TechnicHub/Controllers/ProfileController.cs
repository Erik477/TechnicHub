using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TechnicHub.Controllers
{

    public class ProfileController : Controller
   {
        public int loggedIn = 0;
        public string name;
        public IActionResult Activity()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult Settings()
        {
            return View();
        }

        public IActionResult LogIn()
        {
            HttpContext.Session.SetString(name, "fabian");

            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

    }
}