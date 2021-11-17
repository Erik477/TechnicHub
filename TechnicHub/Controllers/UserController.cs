using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicHub.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Shop()
        {
            return View();
        }
        public IActionResult Chatforum()
        {
            return View();
        }
        public IActionResult Newsletter()
        {
            return View();
        }
        public IActionResult Infopage()
        {
            return View();
        }
    }
}
