using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicHub.Controllers
{
    public class ChatforumController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
