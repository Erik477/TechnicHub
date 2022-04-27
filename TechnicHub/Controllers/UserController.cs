
using TechnicHub.Models;
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
        [HttpGet]
        public IActionResult Chatforum()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Chatforum(Chatpost p)
        {
            if (p == null)
            {
                return RedirectToAction("Chatforum");
            }
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
